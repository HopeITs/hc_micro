using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace MyWorkApplication.Classes
{
    public class KoboMonitoringImporter
    {
        private const string Source = "Kobo Sync";
        private readonly KoboSubmission submissions = new KoboSubmission();
        private readonly KoboVisitorMapping visitors = new KoboVisitorMapping();
        private readonly KoboMonitoringStageMapping stages = new KoboMonitoringStageMapping();

        public KoboLocalVisitImportResult Import()
        {
            KoboLocalVisitImportResult result = new KoboLocalVisitImportResult();
            DataTable rows = submissions.Get_Submissions_For_Local_Import(null, true);
            bool loaded = false;
            foreach (DataRow row in rows.Rows)
            {
                string type = row["Visit_Type"].ToString().ToLowerInvariant();
                if (type != "monitoring" && type != "final") continue;
                try
                {
                    if (!loaded)
                    {
                        visitors.Load(true);
                        stages.Load();
                        loaded = true;
                    }
                    ImportRow(row, result);
                }
                catch (Exception ex)
                {
                    submissions.Mark_Local_Import_Error(Convert.ToInt32(row["ID"]), ex.Message);
                    result.ErrorCount++;
                    result.ErrorMessages += row["Kobo_Form_Name"] + " [" + row["Kobo_Submission_ID"] + "]: " + ex.Message + Environment.NewLine;
                    if (!loaded) break;
                }
            }
            // Keep historical visits on source deletion until their removal is explicitly reviewed.
            foreach (DataRow row in submissions.Get_Deleted_Submissions_For_Local_Import().Rows)
            {
                if (row["Local_Table_Name"].ToString() != "mevisit" &&
                    row["Local_Table_Name"].ToString() != "kobo_monitoring_record") continue;
                string message = "Kobo source was deleted; linked M&E visit retained for review. No local visit was deleted.";
                submissions.Mark_Local_Import_Error(Convert.ToInt32(row["ID"]), message);
                result.ErrorCount++;
                result.ErrorMessages += row["Kobo_Form_Name"] + ": " + message + Environment.NewLine;
            }
            return result;
        }

        private void ImportRow(DataRow row, KoboLocalVisitImportResult result)
        {
            int submissionId = Convert.ToInt32(row["ID"]);
            string uid = row["Kobo_Form_ID"].ToString();
            JObject data = ParseSubmission(row["Raw_JSON"].ToString());
            DataRow fields = visitors.GetFields(uid);
            int project;
            if (!int.TryParse(KoboVisitorMapping.GetValue(data, fields["Project_Number_Field"].ToString()), out project) || project <= 0)
                throw new InvalidOperationException("Missing or invalid project number.");
            string category = row["Visit_Category"].ToString().ToLowerInvariant();
            string number = category == "agriculture" || category == "livestock"
                ? "" : stages.GetStage(data, uid).ToString(CultureInfo.InvariantCulture);
            string kind = category == "vehicle" ? "v" : "o";
            DateTime date = ResolveDate(data);
            List<KoboVisitVisitor> people = visitors.GetVisitors(data, uid);

            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DataTable relationships = Query(connection, transaction, @"SELECT DISTINCT Person_ID, PersonType
FROM person_microproject WHERE MicroProject_ID = @project", "@project", project);
                        DataRow participant = KoboMonitoringIdentity.ResolveParticipant(relationships);
                        int person = Convert.ToInt32(participant["Person_ID"]);
                        string role = participant["PersonType"].ToString().Trim();
                        DataTable projects = Query(connection, transaction, @"SELECT DISTINCT MicroProject_ID
FROM person_microproject WHERE Person_ID = @person", "@person", person);
                        bool isolated = KoboMonitoringIdentity.RequiresProjectIsolation(projects, project);
                        DataTable link = Query(connection, transaction, @"SELECT Local_Table_Name, Local_Record_ID
FROM kobo_submission WHERE ID = @id FOR UPDATE", "@id", submissionId);
                        object linkedId = link.Rows[0]["Local_Record_ID"];
                        bool updated = linkedId != DBNull.Value;
                        DataTable identity = Query(connection, transaction,
                            "SELECT * FROM kobo_monitoring_record WHERE Submission_ID = @id FOR UPDATE", "@id", submissionId);
                        if (identity.Rows.Count > 0 &&
                            (Convert.ToInt32(identity.Rows[0]["MicroProject_ID"]) != project || Convert.ToInt32(identity.Rows[0]["Person_ID"]) != person))
                            throw new InvalidOperationException("Kobo project/person changed after import. Existing identity retained; review required.");
                        if (identity.Rows.Count > 0 && identity.Rows[0]["Legacy_Visit_ID"] != DBNull.Value &&
                            (!updated || link.Rows[0]["Local_Table_Name"].ToString() != "mevisit" ||
                                Convert.ToInt32(identity.Rows[0]["Legacy_Visit_ID"]) != Convert.ToInt32(linkedId)))
                            throw new InvalidOperationException("Stored Kobo project registry disagrees with the local visit link. Review required.");
                        if (updated && link.Rows[0]["Local_Table_Name"].ToString() == "kobo_monitoring_record")
                        {
                            if (Convert.ToInt32(linkedId) != submissionId || identity.Rows.Count != 1)
                                throw new InvalidOperationException("Invalid Kobo-only project link. Review required.");
                            isolated = true;
                        }
                        if (isolated)
                        {
                            if (updated && link.Rows[0]["Local_Table_Name"].ToString() != "kobo_monitoring_record")
                                throw new InvalidOperationException("An existing legacy visit now has multiple possible projects. No automatic relinking allowed.");
                            SaveIdentity(connection, transaction, submissionId, project, person, role, number, kind, date, null);
                            new KoboLocalVisitImporter().SaveVisitorNames(connection, transaction, submissionId, people);
                            Write(connection, transaction, @"UPDATE kobo_submission SET Local_Table_Name = 'kobo_monitoring_record',
Local_Record_ID = @id, Sync_Status = @status, Error_Message = NULL WHERE ID = @id",
                                "@id", submissionId, "@status", updated ? "local_updated" : "local_inserted");
                            transaction.Commit();
                            if (updated) result.UpdatedCount++; else result.ImportedCount++;
                            return;
                        }
                        int visitId;
                        if (updated)
                        {
                            if (link.Rows[0]["Local_Table_Name"].ToString() != "mevisit")
                                throw new InvalidOperationException("Submission is already linked to another local table. Review the old link first.");
                            visitId = Convert.ToInt32(linkedId);
                            DataTable existing = Query(connection, transaction, "SELECT * FROM mevisit WHERE ID = @id FOR UPDATE", "@id", visitId);
                            if (existing.Rows.Count != 1 || existing.Rows[0]["Created_By"].ToString() != Source ||
                                (!string.IsNullOrEmpty(existing.Rows[0]["Edited_By"].ToString()) && existing.Rows[0]["Edited_By"].ToString() != Source))
                                throw new InvalidOperationException("Linked visit is missing or manually edited; no automatic overwrite allowed.");
                            if (Convert.ToInt32(existing.Rows[0]["Person_ID"]) != person || existing.Rows[0]["Kind"].ToString() != kind)
                                throw new InvalidOperationException("Project beneficiary or visit category changed. Review required.");
                            DataTable answers = Query(connection, transaction, "SELECT Visit_ID FROM mevisit_answer WHERE Visit_ID = @id LIMIT 1", "@id", visitId);
                            if (answers.Rows.Count > 0)
                                throw new InvalidOperationException("Visit has legacy questionnaire answers; review required before updating it.");
                            DataTable conflicts = Query(connection, transaction, @"SELECT ID FROM mevisit
WHERE Person_ID = @person AND Kind = @kind AND Number = @number AND Date = @date AND ID <> @id LIMIT 1",
                                "@person", person, "@kind", kind, "@number", number, "@date", date, "@id", visitId);
                            if (conflicts.Rows.Count > 0)
                                throw new InvalidOperationException("Corrected visit date/stage conflicts with another visit. Review required.");
                            Write(connection, transaction, @"UPDATE mevisit SET Number = @number, Date = @date, Edited_By = @source
WHERE ID = @id AND NOT (Number <=> @number AND Date <=> @date)",
                                "@number", number, "@date", date, "@source", Source, "@id", visitId);
                        }
                        else
                        {
                            DataTable duplicates = Query(connection, transaction, @"SELECT ID FROM mevisit
WHERE Person_ID = @person AND Kind = @kind AND Number = @number AND Date = @date",
                                "@person", person, "@kind", kind, "@number", number, "@date", date);
                            if (duplicates.Rows.Count > 0)
                                throw new InvalidOperationException(DescribeDuplicates(connection, transaction, duplicates));
                            Write(connection, transaction, @"INSERT INTO mevisit
(Person_ID, Number, Kind, Type, Date, Profit, Ans1, Ans2, Indicators, Notes, Result, Created_By, Edited_By)
VALUES (@person, @number, @kind, '', @date, 0, '', '', '', '', 0, @source, '')",
                                "@person", person, "@number", number, "@kind", kind, "@date", date, "@source", Source);
                            visitId = Convert.ToInt32(Query(connection, transaction, "SELECT LAST_INSERT_ID() ID").Rows[0]["ID"]);
                        }
                        DataTable otherLinks = Query(connection, transaction, @"SELECT ID FROM kobo_submission
WHERE Local_Table_Name = 'mevisit' AND Local_Record_ID = @visit AND ID <> @submission AND Kobo_Is_Deleted = 0",
                            "@visit", visitId, "@submission", submissionId);
                        if (otherLinks.Rows.Count > 0)
                            throw new InvalidOperationException("Multiple active Kobo sources share this visit; review required.");
                        SaveIdentity(connection, transaction, submissionId, project, person, role, number, kind, date, visitId);
                        new KoboLocalVisitImporter().SaveVisitorNames(connection, transaction, submissionId, people);
                        SavePeople(connection, transaction, visitId, people);
                        Write(connection, transaction, @"UPDATE kobo_submission SET Local_Table_Name = 'mevisit',
Local_Record_ID = @visit, Sync_Status = @status, Error_Message = NULL
WHERE ID = @id", "@visit", visitId, "@status", updated ? "local_updated" : "local_inserted", "@id", submissionId);
                        transaction.Commit();
                        if (updated) result.UpdatedCount++; else result.ImportedCount++;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        private static void SaveIdentity(MySqlConnection connection, MySqlTransaction transaction,
            int submission, int project, int person, string role, string number, string kind, DateTime date, int? visit)
        {
            if (visit.HasValue)
            {
                DataTable owners = Query(connection, transaction, @"SELECT Submission_ID FROM kobo_monitoring_record
WHERE Legacy_Visit_ID = @visit AND Submission_ID <> @id FOR UPDATE", "@visit", visit.Value, "@id", submission);
                if (owners.Rows.Count > 0)
                    throw new InvalidOperationException("Legacy visit already belongs to another Kobo project record, including deleted sources. No registry entry was overwritten.");
            }
            Write(connection, transaction, @"INSERT INTO kobo_monitoring_record
(Submission_ID, MicroProject_ID, Person_ID, Person_Role, Visit_Number, Visit_Kind, Visit_Date, Legacy_Visit_ID)
VALUES (@id, @project, @person, @role, @number, @kind, @date, @visit)
ON DUPLICATE KEY UPDATE Person_Role = VALUES(Person_Role), Visit_Number = VALUES(Visit_Number),
Visit_Kind = VALUES(Visit_Kind), Visit_Date = VALUES(Visit_Date)",
                "@id", submission, "@project", project, "@person", person, "@role", role,
                "@number", number, "@kind", kind, "@date", date, "@visit", visit);
        }

        private static string DescribeDuplicates(MySqlConnection connection, MySqlTransaction transaction, DataTable matches)
        {
            List<string> details = new List<string>();
            foreach (DataRow match in matches.Rows)
            {
                int id = Convert.ToInt32(match["ID"]);
                DataTable owners = Query(connection, transaction, @"SELECT Kobo_Submission_ID, Kobo_Form_Name, Kobo_Is_Deleted
FROM kobo_submission WHERE Local_Table_Name = 'mevisit' AND Local_Record_ID = @id", "@id", id);
                string detail = "Visit " + id;
                if (owners.Rows.Count == 0) detail += " has no verifiable Kobo source link";
                foreach (DataRow owner in owners.Rows)
                    detail += "; owner submission " + owner["Kobo_Submission_ID"] + " (" + owner["Kobo_Form_Name"] +
                        (Convert.ToInt32(owner["Kobo_Is_Deleted"]) == 1 ? ", deleted source" : "") + ")";
                details.Add(detail);
            }
            return "Possible duplicate: " + string.Join(" | ", details) +
                ". Created_By alone does not prove source identity. No visit was overwritten or linked automatically.";
        }

        public static JObject ParseSubmission(string raw)
        {
            using (JsonTextReader reader = new JsonTextReader(new StringReader(raw)))
            {
                reader.DateParseHandling = DateParseHandling.None;
                return JObject.Load(reader);
            }
        }

        public static DateTime ResolveDate(JObject data)
        {
            foreach (string field in new string[] { "today", "_submission_time", "end", "start" })
            {
                string value = KoboVisitorMapping.GetValue(data, field);
                if (string.IsNullOrWhiteSpace(value)) continue;
                DateTimeOffset parsed;
                if (!DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    throw new InvalidOperationException("Invalid Kobo visit date in " + field + ".");
                return parsed.Date;
            }
            throw new InvalidOperationException("Missing Kobo visit date; today's date was not substituted.");
        }

        private void SavePeople(MySqlConnection connection, MySqlTransaction transaction, int id, List<KoboVisitVisitor> people)
        {
            List<int> ids = new List<int>();
            foreach (KoboVisitVisitor person in people)
                if (person.UserId.HasValue && !ids.Contains(person.UserId.Value)) ids.Add(person.UserId.Value);
            DataTable existing = Query(connection, transaction, "SELECT User_ID, Mark FROM mevisit_user WHERE Visit_ID = @id", "@id", id);
            foreach (DataRow row in existing.Rows)
                if (!string.IsNullOrEmpty(row["Mark"].ToString()))
                    throw new InvalidOperationException("Visit has manual visitor evaluations; review required before replacing participants.");
            bool same = existing.Rows.Count == ids.Count;
            foreach (DataRow row in existing.Rows) same &= ids.Contains(Convert.ToInt32(row["User_ID"]));
            if (same) return;
            Write(connection, transaction, "DELETE FROM mevisit_user WHERE Visit_ID = @id", "@id", id);
            foreach (int userId in ids)
                Write(connection, transaction, "INSERT INTO mevisit_user (Visit_ID, User_ID, Mark) VALUES (@id, @user, '')", "@id", id, "@user", userId);
        }

        private static DataTable Query(MySqlConnection connection, MySqlTransaction transaction, string sql, params object[] args)
        {
            using (MySqlCommand command = Command(connection, transaction, sql, args))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
            {
                DataTable table = new DataTable(); adapter.Fill(table); return table;
            }
        }

        private static void Write(MySqlConnection connection, MySqlTransaction transaction, string sql, params object[] args)
        {
            using (MySqlCommand command = Command(connection, transaction, sql, args)) command.ExecuteNonQuery();
        }

        private static MySqlCommand Command(MySqlConnection connection, MySqlTransaction transaction, string sql, object[] args)
        {
            MySqlCommand command = new MySqlCommand(sql, connection, transaction);
            for (int i = 0; i < args.Length; i += 2) command.Parameters.AddWithValue((string)args[i], args[i + 1] ?? DBNull.Value);
            return command;
        }
    }
}
