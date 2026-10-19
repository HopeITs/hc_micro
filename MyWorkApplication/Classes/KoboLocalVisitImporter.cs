using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace MyWorkApplication.Classes
{
    public class KoboLocalVisitImportResult
    {
        public int ImportedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int DeletedCount { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorMessages { get; set; }

        public KoboLocalVisitImportResult()
        {
            ImportedCount = 0;
            UpdatedCount = 0;
            DeletedCount = 0;
            ErrorCount = 0;
            ErrorMessages = "";
        }
    }

    public class KoboLocalVisitImporter
    {
        private const string KoboCreatedBy = "Kobo Sync";
        private KoboSubmission koboSubmission;
        private KoboVisitorMapping visitorMapping;

        public KoboLocalVisitImporter()
        {
            koboSubmission = new KoboSubmission();
        }

        public KoboLocalVisitImportResult Import_Opening_Visits()
        {
            visitorMapping = new KoboVisitorMapping();
            visitorMapping.Load();
            KoboLocalVisitImportResult result = new KoboLocalVisitImportResult();
            Import_Active_Opening_Visits(result);
            Import_Deleted_Opening_Visits(result);
            return result;
        }

        private void Import_Active_Opening_Visits(KoboLocalVisitImportResult result)
        {
            DataTable submissions = koboSubmission.Get_Submissions_For_Local_Import("visit", true);
            foreach (DataRow row in submissions.Rows)
            {
                if (!IsOpeningVisit(row))
                    continue;

                try
                {
                    Import_Opening_Visit(row, result);
                }
                catch (Exception ex)
                {
                    int koboSubmissionId = Convert.ToInt32(row["ID"]);
                    koboSubmission.Mark_Local_Import_Error(koboSubmissionId, ex.Message);
                    result.ErrorCount++;
                    AddError(result, row["Kobo_Form_Name"] + ": " + ex.Message);
                }
            }
        }

        private void Import_Deleted_Opening_Visits(KoboLocalVisitImportResult result)
        {
            DataTable submissions = koboSubmission.Get_Deleted_Submissions_For_Local_Import();
            foreach (DataRow row in submissions.Rows)
            {
                if (row["Local_Table_Name"] == DBNull.Value ||
                    row["Local_Table_Name"].ToString() != "visit")
                    continue;
                if (!IsDeletedOpeningVisit(row))
                    continue;

                try
                {
                    Delete_Local_Visit(row, result);
                }
                catch (Exception ex)
                {
                    int koboSubmissionId = Convert.ToInt32(row["ID"]);
                    koboSubmission.Mark_Local_Import_Error(koboSubmissionId, ex.Message);
                    result.ErrorCount++;
                    AddError(result, row["Kobo_Form_Name"] + ": " + ex.Message);
                }
            }
        }

        private void Import_Opening_Visit(DataRow row, KoboLocalVisitImportResult result)
        {
            int koboSubmissionId = Convert.ToInt32(row["ID"]);
            JObject submission = JObject.Parse(row["Raw_JSON"].ToString());
            int microProjectId = GetProjectNumber(submission, row);
            if (microProjectId <= 0)
                throw new Exception("لا يوجد رقم مشروع صالح ضمن بيانات كوبو.");

            string kind = GetOpeningVisitKind(row);
            string visitDate = GetVisitDate(submission);
            string vehicleNumber = "";
            if (kind == "V-op")
                vehicleNumber = KoboVisitorMapping.GetValue(submission,
                    visitorMapping.GetFields(row["Kobo_Form_ID"].ToString())["Vehicle_Number_Field"].ToString());

            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int visitId = GetLocalRecordId(row);
                        if (visitId <= 0 || !VisitExists(connection, transaction, visitId))
                            visitId = FindExistingOpeningVisit(connection, transaction, microProjectId, kind);

                        bool updated = visitId > 0;
                        string syncStatus = row["Sync_Status"].ToString();
                        bool visitorsOnly = updated && (syncStatus == "local_inserted" || syncStatus == "local_updated");
                        if (updated)
                            UpdateVisit(connection, transaction, visitId, visitDate, vehicleNumber);
                        else if (!updated)
                            visitId = InsertVisit(connection, transaction, microProjectId, kind, visitDate, vehicleNumber);

                        List<KoboVisitVisitor> visitors = visitorMapping.GetVisitors(submission, row["Kobo_Form_ID"].ToString());
                        List<int> visitorUserIds = new List<int>();
                        foreach (KoboVisitVisitor visitor in visitors)
                        {
                            if (visitor.UserId.HasValue && !visitorUserIds.Contains(visitor.UserId.Value))
                                visitorUserIds.Add(visitor.UserId.Value);
                        }
                        SaveVisitorNames(connection, transaction, koboSubmissionId, visitors);
                        if (visitors.Count > 0)
                            RefreshVisitUsers(connection, transaction, visitId, visitorUserIds);
                        if (!visitorsOnly)
                        {
                            MarkOpeningTask(connection, transaction, microProjectId, true);
                            MarkVisitNotificationsSeen(connection, transaction, microProjectId, kind);
                        }
                        if (!visitorsOnly)
                            MarkSubmissionSuccess(connection, transaction, koboSubmissionId, visitId, updated);
                        transaction.Commit();

                        if (updated && !visitorsOnly)
                            result.UpdatedCount++;
                        else if (!updated)
                            result.ImportedCount++;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void Delete_Local_Visit(DataRow row, KoboLocalVisitImportResult result)
        {
            int koboSubmissionId = Convert.ToInt32(row["ID"]);
            int localRecordId = GetLocalRecordId(row);
            if (localRecordId <= 0)
                return;

            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int microProjectId = GetVisitMicroProjectId(connection, transaction, localRecordId);
                        if (CanDeleteLocalVisit(connection, transaction, localRecordId))
                        {
                            ExecuteNonQuery(connection, transaction,
                                "DELETE FROM `visit_user` WHERE `Visit_ID` = @Visit_ID",
                                Parameter("@Visit_ID", localRecordId));
                            ExecuteNonQuery(connection, transaction,
                                "DELETE FROM `office_monitoring` WHERE `Visit_ID` = @Visit_ID",
                                Parameter("@Visit_ID", localRecordId));
                            ExecuteNonQuery(connection, transaction,
                                "DELETE FROM `visit` WHERE `ID` = @ID",
                                Parameter("@ID", localRecordId));
                            MarkOpeningTask(connection, transaction, microProjectId, false);
                        }

                        MarkSubmissionDeleted(connection, transaction, koboSubmissionId);
                        transaction.Commit();
                        result.DeletedCount++;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private int InsertVisit(MySqlConnection connection, MySqlTransaction transaction,
            int microProjectId, string kind, string visitDate, string vehicleNumber)
        {
            string query = @"INSERT INTO `visit`
(`MicroProject_ID`, `Kind`, `Type`, `Date`, `Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`, `Ans7`,
 `Indicators`, `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value`,
 `Ans7_Value`, `VehicleNumber`, `Created_By`, `Edited_By`)
VALUES
(@MicroProject_ID, @Kind, '', @Date, '', '', '', '', '', '', '', '',
 -1, -1, -1, -1, -1, -1, -1, @VehicleNumber, @Created_By, '')";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@MicroProject_ID", microProjectId),
                Parameter("@Kind", kind),
                Parameter("@Date", visitDate),
                Parameter("@VehicleNumber", vehicleNumber),
                Parameter("@Created_By", KoboCreatedBy));

            using (MySqlCommand command = new MySqlCommand("SELECT LAST_INSERT_ID();", connection, transaction))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void UpdateVisit(MySqlConnection connection, MySqlTransaction transaction,
            int visitId, string visitDate, string vehicleNumber)
        {
            string query = @"UPDATE `visit` SET
`Date` = @Date,
`VehicleNumber` = @VehicleNumber,
`Edited_By` = @Edited_By
WHERE `ID` = @ID AND NOT (`Date` <=> @Date AND `VehicleNumber` <=> @VehicleNumber)";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@ID", visitId),
                Parameter("@Date", visitDate),
                Parameter("@VehicleNumber", vehicleNumber),
                Parameter("@Edited_By", KoboCreatedBy));
        }

        internal void SaveVisitorNames(MySqlConnection connection, MySqlTransaction transaction,
            int submissionId, List<KoboVisitVisitor> visitors)
        {
            DataTable existing = new DataTable();
            using (MySqlCommand command = new MySqlCommand(@"SELECT `Kobo_Name`, `Display_Name`, `User_ID`
FROM `kobo_visit_visitor` WHERE `Submission_ID` = @ID ORDER BY `Visitor_Order`", connection, transaction))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
            {
                command.Parameters.Add(Parameter("@ID", submissionId));
                adapter.Fill(existing);
            }
            bool unchanged = existing.Rows.Count == visitors.Count;
            for (int i = 0; unchanged && i < visitors.Count; i++)
            {
                DataRow row = existing.Rows[i];
                unchanged = row["Kobo_Name"].ToString() == visitors[i].KoboName &&
                    row["Display_Name"].ToString() == visitors[i].DisplayName &&
                    (row["User_ID"] == DBNull.Value ? !visitors[i].UserId.HasValue :
                        visitors[i].UserId == Convert.ToInt32(row["User_ID"]));
            }
            if (unchanged)
                return;

            ExecuteNonQuery(connection, transaction, "DELETE FROM `kobo_visit_visitor` WHERE `Submission_ID` = @ID",
                Parameter("@ID", submissionId));
            for (int i = 0; i < visitors.Count; i++)
            {
                ExecuteNonQuery(connection, transaction, @"INSERT INTO `kobo_visit_visitor`
(`Submission_ID`, `Visitor_Order`, `Kobo_Name`, `Display_Name`, `User_ID`)
VALUES (@ID, @Visitor_Order, @Kobo_Name, @Display_Name, @User_ID)",
                    Parameter("@ID", submissionId), Parameter("@Visitor_Order", i),
                    Parameter("@Kobo_Name", visitors[i].KoboName), Parameter("@Display_Name", visitors[i].DisplayName),
                    Parameter("@User_ID", visitors[i].UserId));
            }
        }

        private void RefreshVisitUsers(MySqlConnection connection, MySqlTransaction transaction,
            int visitId, List<int> userIds)
        {
            using (MySqlCommand command = new MySqlCommand(
                "SELECT `User_ID` FROM `visit_user` WHERE `Visit_ID` = @Visit_ID", connection, transaction))
            {
                command.Parameters.Add(Parameter("@Visit_ID", visitId));
                List<int> existingIds = new List<int>();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        existingIds.Add(Convert.ToInt32(reader["User_ID"]));
                }
                if (existingIds.Count == userIds.Count && existingIds.TrueForAll(userIds.Contains))
                    return;
            }

            ExecuteNonQuery(connection, transaction,
                "DELETE FROM `visit_user` WHERE `Visit_ID` = @Visit_ID",
                Parameter("@Visit_ID", visitId));

            for (int i = 0; i < userIds.Count; i++)
            {
                ExecuteNonQuery(connection, transaction,
                    "INSERT INTO `visit_user`(`Visit_ID`, `User_ID`) VALUES(@Visit_ID, @User_ID)",
                    Parameter("@Visit_ID", visitId),
                    Parameter("@User_ID", userIds[i]));
            }
        }

        private void MarkOpeningTask(MySqlConnection connection, MySqlTransaction transaction,
            int microProjectId, bool done)
        {
            string query = @"UPDATE `task_microproject` SET
`State` = @State,
`Date` = @Date
WHERE `MicroProject_ID` = @MicroProject_ID
AND `Task_ID` = 25";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@MicroProject_ID", microProjectId),
                Parameter("@State", done ? 1 : 0),
                Parameter("@Date", DateTime.Now.ToString("yyyy/MM/dd")));
        }

        private void MarkVisitNotificationsSeen(MySqlConnection connection, MySqlTransaction transaction,
            int microProjectId, string kind)
        {
            string body = kind == "V-op" ? "Visit Opening-V" : "Visit Opening-O";
            string query = @"UPDATE `user_notification` SET `Seen` = 1
WHERE `MicroProject_ID` = @MicroProject_ID
AND `Body` LIKE @Body";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@MicroProject_ID", microProjectId),
                Parameter("@Body", body));
        }

        private void MarkSubmissionSuccess(MySqlConnection connection, MySqlTransaction transaction,
            int koboSubmissionId, int visitId, bool updated)
        {
            string query = @"UPDATE `kobo_submission` SET
`Sync_Status` = @Sync_Status,
`Local_Table_Name` = 'visit',
`Local_Record_ID` = @Local_Record_ID,
`Error_Message` = NULL,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@ID", koboSubmissionId),
                Parameter("@Sync_Status", updated ? "local_updated" : "local_inserted"),
                Parameter("@Local_Record_ID", visitId),
                Parameter("@Updated_At", DateTime.Now));
        }

        private void MarkSubmissionDeleted(MySqlConnection connection, MySqlTransaction transaction,
            int koboSubmissionId)
        {
            string query = @"UPDATE `kobo_submission` SET
`Sync_Status` = 'local_deleted',
`Error_Message` = NULL,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@ID", koboSubmissionId),
                Parameter("@Updated_At", DateTime.Now));
        }

        private int GetProjectNumber(JObject submission, DataRow row)
        {
            DataRow fields = visitorMapping.GetFields(row["Kobo_Form_ID"].ToString());
            int result;
            if (int.TryParse(KoboVisitorMapping.GetValue(submission, fields["Project_Number_Field"].ToString()), out result))
                return result;
            return -1;
        }

        private string GetVisitDate(JObject submission)
        {
            DateTime date;
            string value = GetFirstValue(submission, new string[] { "today", "_submission_time", "end", "start" });
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out date))
                return date.ToString("yyyy/MM/dd");

            return DateTime.Now.ToString("yyyy/MM/dd");
        }

        private string GetOpeningVisitKind(DataRow row)
        {
            string formName = row["Kobo_Form_Name"].ToString().ToLowerInvariant();
            string category = row["Visit_Category"].ToString().ToLowerInvariant();

            if (formName.Contains("vehicle") || category.Contains("vehicle") ||
                formName.Contains("taxi") || category.Contains("taxi"))
                return "V-op";

            return "O-op";
        }

        private bool IsOpeningVisit(DataRow row)
        {
            string visitType = row["Visit_Type"].ToString().ToLowerInvariant();
            string formName = row["Kobo_Form_Name"].ToString().ToLowerInvariant();

            if (visitType.Contains("opening") || formName.Contains("opening") || formName.Contains("opining"))
                return true;

            return false;
        }

        private bool IsDeletedOpeningVisit(DataRow row)
        {
            string formName = row["Kobo_Form_Name"].ToString().ToLowerInvariant();

            if (formName.Contains("opening") || formName.Contains("opining"))
                return true;

            return false;
        }

        private int GetLocalRecordId(DataRow row)
        {
            if (row["Local_Record_ID"] == DBNull.Value || row["Local_Record_ID"] == null)
                return -1;

            int result;
            if (int.TryParse(row["Local_Record_ID"].ToString(), out result))
                return result;

            return -1;
        }

        private int FindExistingOpeningVisit(MySqlConnection connection, MySqlTransaction transaction,
            int microProjectId, string kind)
        {
            string query = @"SELECT `ID` FROM `visit`
WHERE `MicroProject_ID` = @MicroProject_ID
AND `Kind` = @Kind
LIMIT 1";

            return ExecuteScalarInt(connection, transaction, query,
                Parameter("@MicroProject_ID", microProjectId),
                Parameter("@Kind", kind));
        }

        private bool VisitExists(MySqlConnection connection, MySqlTransaction transaction, int visitId)
        {
            string query = "SELECT COUNT(*) FROM `visit` WHERE `ID` = @ID";
            return ExecuteScalarInt(connection, transaction, query, Parameter("@ID", visitId)) > 0;
        }

        private int GetVisitMicroProjectId(MySqlConnection connection, MySqlTransaction transaction, int visitId)
        {
            string query = "SELECT `MicroProject_ID` FROM `visit` WHERE `ID` = @ID";
            return ExecuteScalarInt(connection, transaction, query, Parameter("@ID", visitId));
        }

        private bool CanDeleteLocalVisit(MySqlConnection connection, MySqlTransaction transaction, int visitId)
        {
            string query = "SELECT `Created_By` FROM `visit` WHERE `ID` = @ID";
            using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.Add(Parameter("@ID", visitId));
                object value = command.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return false;

                return value.ToString() == KoboCreatedBy;
            }
        }

        private string GetFirstValue(JObject submission, string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                string value = GetValue(submission, names[i]);
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return "";
        }

        private string GetValue(JObject submission, string name)
        {
            JToken token = submission[name];
            if (token != null && token.Type != JTokenType.Null)
                return token.ToString().Trim();

            foreach (JProperty property in submission.Properties())
            {
                if (property.Name.EndsWith("/" + name, StringComparison.OrdinalIgnoreCase) &&
                    property.Value != null &&
                    property.Value.Type != JTokenType.Null)
                    return property.Value.ToString().Trim();
            }

            return "";
        }

        private int ExecuteScalarInt(MySqlConnection connection, MySqlTransaction transaction,
            string query, params MySqlParameter[] parameters)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddRange(parameters);
                object value = command.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return -1;

                int result;
                if (int.TryParse(value.ToString(), out result))
                    return result;

                return -1;
            }
        }

        private void ExecuteNonQuery(MySqlConnection connection, MySqlTransaction transaction,
            string query, params MySqlParameter[] parameters)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        private MySqlParameter Parameter(string name, object value)
        {
            if (value == null)
                value = DBNull.Value;

            return new MySqlParameter(name, value);
        }

        private void AddError(KoboLocalVisitImportResult result, string message)
        {
            if (!string.IsNullOrWhiteSpace(result.ErrorMessages))
                result.ErrorMessages += Environment.NewLine;

            result.ErrorMessages += message;
        }
    }
}
