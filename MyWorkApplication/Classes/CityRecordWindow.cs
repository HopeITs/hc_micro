using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;

namespace MyWorkApplication.Classes
{
    // Legacy editors use process-wide Program state. A separate process keeps their
    // database, role, image paths and child dialogs isolated for the editor's lifetime.
    internal static class CityRecordWindow
    {
        internal static string ConnectionFor(string connection, string city)
        {
            if (!CityReportingService.Databases.ContainsKey(city)) throw new InvalidOperationException("Unknown city.");
            return new MySqlConnectionStringBuilder(connection)
            {
                Database = CityReportingService.Databases[city], ConnectionTimeout = 15,
                DefaultCommandTimeout = 60, Pooling = true
            }.ConnectionString;
        }

        internal static void Open(string city, int reportType, DataRow row)
        {
            if (!CityReportingService.PermittedCities(Program.User_In_Database).Contains(city))
                throw new InvalidOperationException("You do not have access to " + city + ".");
            var data = new DataSet("CityRecordSession");
            var session = data.Tables.Add("Session");
            foreach (string column in new[] { "City", "Connection", "User", "Password", "Created", "ReportType", "Online" }) session.Columns.Add(column);
            session.Rows.Add(city, Program.ConnectionString, Program.UserName, Program.Password,
                DateTime.UtcNow.ToString("O"), reportType.ToString(), Properties.Settings.Default.Online.ToString());
            var record = row.Table.Clone();
            record.TableName = "Record";
            record.ImportRow(row);
            data.Tables.Add(record);
            string ticket = Guid.NewGuid().ToString("N");
            string path = TicketPath(ticket);
            using (var stream = new MemoryStream())
            {
                data.WriteXml(stream, XmlWriteMode.WriteSchema);
                // No passwords or database credentials appear in command-line arguments.
                File.WriteAllBytes(path, ProtectedData.Protect(stream.ToArray(), null, DataProtectionScope.CurrentUser));
            }
            try
            {
                Process.Start(new ProcessStartInfo(Application.ExecutablePath, "--city-record " + ticket) { UseShellExecute = false });
            }
            catch { File.Delete(path); throw; }
        }

        private static string TicketPath(string ticket)
        {
            Guid id;
            if (!Guid.TryParseExact(ticket, "N", out id)) throw new InvalidOperationException("Invalid city-window request.");
            return Path.Combine(Path.GetTempPath(), "MicroProjects-city-" + id.ToString("N") + ".bin");
        }

        internal static void Run(string ticket)
        {
            try
            {
                string path = TicketPath(ticket);
                byte[] encrypted;
                try { encrypted = File.ReadAllBytes(path); }
                finally { if (File.Exists(path)) File.Delete(path); }
                var data = new DataSet();
                using (var stream = new MemoryStream(ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser)))
                using (var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }))
                    data.ReadXml(reader, XmlReadMode.ReadSchema);
                DataRow session = data.Tables["Session"].Rows[0];
                DateTime created = DateTime.Parse((string)session["Created"], null, System.Globalization.DateTimeStyles.RoundtripKind);
                if (created > DateTime.UtcNow.AddSeconds(10) || DateTime.UtcNow - created > TimeSpan.FromMinutes(2))
                    throw new InvalidOperationException("This city-window request expired. Open the record again.");
                string city = (string)session["City"];
                string username = (string)session["User"];
                string credential = (string)session["Password"];
                // Authenticate the originating session, then obtain the target city's
                // current user ID and role. Never carry an administrator role across cities.
                using (var source = new MySqlConnection(new MySqlConnectionStringBuilder((string)session["Connection"]) { ConnectionTimeout = 15 }.ConnectionString))
                using (var command = new MySqlCommand("SELECT Password FROM `user` WHERE UserName=@user AND UserRoleID<>6", source))
                {
                    command.Parameters.AddWithValue("@user", username);
                    source.Open();
                    string stored = command.ExecuteScalar() as string;
                    if (stored == null || (credential != stored && !PasswordHelper.VerifyPassword(credential, stored)))
                        throw new InvalidOperationException("Your session is no longer valid. Sign in again.");
                }
                string connection = ConnectionFor((string)session["Connection"], city);
                using (var target = new MySqlConnection(connection))
                using (var command = new MySqlCommand("SELECT UserID,UserRoleID FROM `user` WHERE UserName=@user AND UserRoleID<>6", target))
                {
                    command.Parameters.AddWithValue("@user", username);
                    target.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) throw new InvalidOperationException("You no longer have access to " + city + ".");
                        Program.userID = reader.GetInt32(0);
                        Program.role = reader.GetInt32(1);
                    }
                }
                Program.UserName = username;
                Program.Password = credential;
                Program.selected_database = city;
                // This editor window is pinned to one city, including any child tabs.
                Program.User_In_Database = new System.Collections.Generic.List<string> { city };
                Properties.Settings.Default.Online = bool.Parse((string)session["Online"]);
                Program.set_ConnectionString();
                Program.ConnectionString = connection;
                Program.close_app = false;
                using (var main = new MainForm())
                {
                    main.Shown += (sender, args) =>
                    {
                        main.LockCityContext();
                        using (var search = new Search_Form(main))
                            search.OpenReportRecord(data.Tables["Record"].Rows[0], int.Parse((string)session["ReportType"]));
                    };
                    Application.Run(main);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the city record. " + error.Message, "City record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
