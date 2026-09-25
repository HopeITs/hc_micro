using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.Sockets;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    internal sealed class CityReportResult
    {
        public readonly Dictionary<string, DataSet> Cities = new Dictionary<string, DataSet>();
        public readonly List<string> FailedCities = new List<string>();
        public readonly Dictionary<string, string> FailureReasons = new Dictionary<string, string>();
        public string[] RequestedCities;

        public string Coverage
        {
            get
            {
                return (FailedCities.Count == 0 ? "Complete" : "Incomplete — " + string.Join("; ", FailedCities.Select(city => city + ": " + (FailureReasons.ContainsKey(city) ? FailureReasons[city] : "unavailable"))))
                    + " — Cities: " + string.Join(", ", Cities.Keys);
            }
        }

        public DataSet Combine(bool includeCity, bool statistics, CancellationToken token = default(CancellationToken))
        {
            if (Cities.Count == 0)
                throw new InvalidOperationException(Coverage + ". No city returned data.");
            DataSet combined = new DataSet();
            foreach (KeyValuePair<string, DataSet> city in Cities)
            {
                for (int i = 0; i < city.Value.Tables.Count; i++)
                {
                    DataTable source = city.Value.Tables[i];
                    if (combined.Tables.Count <= i)
                    {
                        DataTable target = source.Clone();
                        target.PrimaryKey = new DataColumn[0];
                        target.Constraints.Clear();
                        if (includeCity && i == 0 && !statistics)
                            target.Columns.Add("City", typeof(string));
                        combined.Tables.Add(target);
                    }
                    foreach (DataRow row in source.Rows)
                    {
                        token.ThrowIfCancellationRequested();
                        DataRow copy = combined.Tables[i].NewRow();
                        for (int c = 0; c < source.Columns.Count; c++) copy[c] = row[c];
                        if (includeCity && i == 0 && !statistics) copy["City"] = city.Key;
                        combined.Tables[i].Rows.Add(copy);
                    }
                }
            }
            DataSet output = new DataSet();
            for (int i = 0; i < combined.Tables.Count; i++)
            {
                if (statistics || i > 0)
                {
                    DataTable original = combined.Tables[i];
                    DataTable summed = Sum(original, statistics ? (original.Columns.Contains("ReportGroupId") ? "ReportGroupId" : original.Columns[0].ColumnName) : null);
                    // Report queries have one detail table followed by scalar summary tables.
                    summed.TableName = original.TableName;
                    output.Tables.Add(summed);
                }
                else output.Tables.Add(combined.Tables[i].Copy());
            }
            return output;
        }

        internal static DataTable Sum(DataTable source, string groupColumn)
        {
            DataTable result = source.Clone();
            // MySQL COUNT uses Int64; SUM may use Decimal. Preserve precision in totals.
            foreach (DataColumn column in result.Columns)
                if (column.ColumnName != groupColumn && (groupColumn == null || column.Ordinal != 0))
                    column.DataType = typeof(decimal);
            Dictionary<object, DataRow> groups = new Dictionary<object, DataRow>();
            foreach (DataRow row in source.Rows)
            {
                object key = groupColumn == null ? "total" : row[groupColumn];
                DataRow total;
                if (!groups.TryGetValue(key, out total))
                {
                    total = result.NewRow();
                    for (int c = 0; c < source.Columns.Count; c++)
                        total[c] = source.Columns[c].ColumnName == groupColumn || (groupColumn != null && c == 0)
                            ? row[c] : (object)0m;
                    result.Rows.Add(total);
                    groups.Add(key, total);
                }
                for (int c = 0; c < source.Columns.Count; c++)
                    if (source.Columns[c].ColumnName != groupColumn && (groupColumn == null || c != 0))
                        total[c] = (decimal)total[c] + (row.IsNull(c) ? 0m : Convert.ToDecimal(row[c], CultureInfo.InvariantCulture));
            }
            return result;
        }
    }

    internal sealed class CityReportingService
    {
        private sealed class CityResponse
        {
            public string City;
            public DataSet Data;
            public string Failure;
        }
        private readonly Func<string, string, string, IDictionary<string, object>, CancellationToken, DataSet> readCity;

        public CityReportingService() : this(ReadCity) { }

        internal Task<CityReportResult> ExportApplicationsAsync(string connectionString, IEnumerable<string> permitted,
            DataTable rows, CancellationToken token, string username = null)
        {
            var cities = rows.Rows.Cast<DataRow>().Select(row => Convert.ToString(row["City"])).Distinct().ToArray();
            if (cities.Length == 0) throw new InvalidOperationException("There are no visible applications to export.");
            var ids = cities.ToDictionary(city => city, city => rows.Rows.Cast<DataRow>()
                .Where(row => Convert.ToString(row["City"]) == city).Select(row => Convert.ToInt64(row["ID"]))
                .Distinct().OrderBy(id => id).ToArray());
            var exporter = new CityReportingService((connection, city, query, values, cancellation) =>
            {
                DataTable details = null;
                // The procedure accepts TEXT. Bound batches avoid its length/packet limits.
                for (int offset = 0; offset < ids[city].Length; offset += 500)
                {
                    cancellation.ThrowIfCancellationRequested();
                    var batch = ids[city].Skip(offset).Take(500).Select(id => id.ToString(CultureInfo.InvariantCulture));
                    var parameters = new Dictionary<string, object> { { "@MyCondition", " WHERE Current_State = 1 AND PMP_ID IN (" + string.Join(",", batch) + ")" } };
                    if (username != null) parameters.Add("@reportUser", username);
                    DataSet response = readCity(connection, city, "select_fullapplication_details", parameters, cancellation);
                    if (response.Tables.Count == 0) throw new InvalidOperationException("The application export procedure returned no result table for " + city + ".");
                    DataTable source = response.Tables[0];
                    if (details == null)
                    {
                        details = source.Clone();
                        details.PrimaryKey = new DataColumn[0];
                        details.Constraints.Clear();
                    }
                    details.Merge(source, false, MissingSchemaAction.Error);
                }
                var result = new DataSet();
                result.Tables.Add(details);
                return result;
            });
            return exporter.QueryAsync(connectionString, permitted, cities, "", new Dictionary<string, object>(), token);
        }

        internal CityReportingService(Func<string, string, string, IDictionary<string, object>, CancellationToken, DataSet> reader)
        {
            readCity = reader;
        }

        internal static readonly IDictionary<string, string> Databases = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "Aleppo", "hcsyria_micro" }, { "Homs", "hcsyria_micro_h" },
            { "Damascus", "hcsyria_micro_d" }, { "Latakia", "hcsyria_micro_l" },
            { "AlWadi", "hcsyria_micro_w" }, { "Lebanon", "hcsyria_micro_lb" }
        };

        internal static string[] PermittedCities(IEnumerable<string> permitted)
        {
            return (permitted ?? Enumerable.Empty<string>()).Where(Databases.ContainsKey).Distinct().ToArray();
        }

        public async Task<CityReportResult> QueryAsync(string connectionString, IEnumerable<string> permitted,
            IEnumerable<string> selected, string sql, IDictionary<string, object> parameters, CancellationToken cancellationToken,
            IDictionary<string, IDictionary<string, object>> cityParameters = null)
        {
            string[] cities = selected.Distinct().ToArray();
            string[] allowed = PermittedCities(permitted);
            if (cities.Length == 0 || cities.Any(city => !allowed.Contains(city)))
                throw new InvalidOperationException("No permitted reporting cities were selected.");
            var snapshots = cities.ToDictionary(city => city, city => new Dictionary<string, object>(
                cityParameters == null ? parameters : cityParameters[city]));
            var result = new CityReportResult { RequestedCities = cities };
            // All city schemas share a host. Limit simultaneous heavy reports on that host.
            using (var gate = new SemaphoreSlim(2))
            {
                var tasks = cities.Select(async city =>
                {
                    await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
                    try
                    {
                        // MySqlDataAdapter has no asynchronous Fill. Only I/O runs on a worker;
                        // no form controls or global connections are accessed here.
                        DataSet data = await Task.Run(() => readCity(connectionString, city, sql, snapshots[city], cancellationToken), cancellationToken).ConfigureAwait(false);
                        return new CityResponse { City = city, Data = data };
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception error) when (IsCityFailure(error))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return new CityResponse { City = city, Failure = DescribeFailure(error) };
                    }
                    finally { gate.Release(); }
                }).ToArray();
                var responses = await Task.WhenAll(tasks).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var response in responses)
                    if (response.Data == null)
                    {
                        result.FailedCities.Add(response.City);
                        result.FailureReasons.Add(response.City, response.Failure);
                    }
                    else result.Cities.Add(response.City, response.Data);
            }
            return result;
        }

        internal static bool IsCityFailure(Exception error)
        {
            return error is DbException || error is SocketException || error is IOException || error is TimeoutException || error is UnauthorizedAccessException;
        }

        internal static string DescribeFailure(Exception error)
        {
            if (error is UnauthorizedAccessException) return "your role in this city does not permit export";
            bool networkFailure = false;
            for (Exception cause = error; cause != null; cause = cause.InnerException)
            {
                var socket = cause as SocketException;
                if (cause is TimeoutException || (socket != null && socket.SocketErrorCode == SocketError.TimedOut))
                    return "connection/query timed out; refresh to retry";
                if (socket != null || cause is IOException)
                    networkFailure = true;
            }
            if (networkFailure) return "connection lost or server unreachable; refresh to retry";
            var database = error as MySqlException;
            if (database != null && database.Number == -2) return "query timed out; narrow the filters and refresh";
            if (database != null && (database.Number == 1042 || database.Number == 2002 || database.Number == 2003 || database.Number == 2006 || database.Number == 2013))
                return "connection lost or server unreachable; refresh to retry";
            return database == null ? "database read failed" : "database query failed (" + database.Number + ")";
        }

        private static DataSet ReadCity(string connectionString, string city, string sql,
            IDictionary<string, object> parameters, CancellationToken token)
        {
            var builder = new MySqlConnectionStringBuilder(connectionString)
            {
                Database = Databases[city], ConnectionTimeout = 15, DefaultCommandTimeout = 60,
                Pooling = true, MaximumPoolSize = 10, MinimumPoolSize = 0
            };
            using (var connection = new MySqlConnection(builder.ConnectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                if (sql == "select_fullapplication_details") command.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                    if (parameter.Key != "@reportUser") command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                token.ThrowIfCancellationRequested();
                connection.Open();
                if (parameters.ContainsKey("@reportUser"))
                {
                    using (var permission = new MySqlCommand("SELECT UserRoleID FROM `user` WHERE UserName=@user", connection))
                    {
                        permission.Parameters.AddWithValue("@user", parameters["@reportUser"]);
                        object value = permission.ExecuteScalar();
                        if (value == null || new[] { 4, 6, 7, 9 }.Contains(Convert.ToInt32(value)))
                            throw new UnauthorizedAccessException();
                    }
                }
                token.ThrowIfCancellationRequested();
                using (token.Register(() =>
                {
                    // MySqlCommand.Cancel can perform network I/O. Do not run it on the
                    // UI thread that requested cancellation.
                    Task.Run(() => { try { command.Cancel(); } catch (Exception error) when (IsCityFailure(error) || error is InvalidOperationException) { } });
                }))
                using (var adapter = new MySqlDataAdapter(command))
                {
                    DataSet data = new DataSet();
                    try { adapter.Fill(data); }
                    catch (Exception error) when (token.IsCancellationRequested && IsCityFailure(error))
                    { throw new OperationCanceledException(token); }
                    token.ThrowIfCancellationRequested();
                    return data;
                }
            }
        }
    }
}
