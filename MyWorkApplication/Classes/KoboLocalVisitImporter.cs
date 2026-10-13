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

        public KoboLocalVisitImporter()
        {
            koboSubmission = new KoboSubmission();
        }

        public KoboLocalVisitImportResult Import_Opening_Visits()
        {
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
                vehicleNumber = GetValue(submission, "__008");

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
                        if (updated && !visitorsOnly)
                            UpdateVisit(connection, transaction, visitId, visitDate, vehicleNumber);
                        else if (!updated)
                            visitId = InsertVisit(connection, transaction, microProjectId, kind, visitDate, vehicleNumber);

                        List<string> unmatchedNames;
                        List<int> visitorUserIds = GetVisitorUserIds(connection, transaction, submission, row, out unmatchedNames);
                        if (unmatchedNames.Count == 0)
                            RefreshVisitUsers(connection, transaction, visitId, visitorUserIds);
                        if (!visitorsOnly)
                        {
                            MarkOpeningTask(connection, transaction, microProjectId, true);
                            MarkVisitNotificationsSeen(connection, transaction, microProjectId, kind);
                            MarkSubmissionSuccess(connection, transaction, koboSubmissionId, visitId, updated);
                        }
                        string visitorError = "";
                        if (unmatchedNames.Count > 0)
                        {
                            visitorError = "تعذر ربط أسماء زوار كوبو بمستخدمي البرنامج: " + string.Join("، ", unmatchedNames);
                            MarkVisitorImportError(connection, transaction, koboSubmissionId, visitorError);
                        }
                        transaction.Commit();

                        if (unmatchedNames.Count > 0)
                        {
                            result.ErrorCount++;
                            AddError(result, row["Kobo_Form_Name"] + " / " + microProjectId + ": " + visitorError);
                        }
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
WHERE `ID` = @ID";

            ExecuteNonQuery(connection, transaction, query,
                Parameter("@ID", visitId),
                Parameter("@Date", visitDate),
                Parameter("@VehicleNumber", vehicleNumber),
                Parameter("@Edited_By", KoboCreatedBy));
        }

        private List<int> GetVisitorUserIds(MySqlConnection connection, MySqlTransaction transaction,
            JObject submission, DataRow row, out List<string> unmatchedNames)
        {
            List<string> visitorNames = GetVisitorNameCandidates(submission, row);
            unmatchedNames = new List<string>();
            List<int> userIds = new List<int>();
            if (visitorNames.Count == 0)
                return userIds;

            DataTable users = GetVisitorUsers(connection, transaction);
            for (int i = 0; i < visitorNames.Count; i++)
            {
                int userId = FindVisitorUserId(users, visitorNames[i]);
                if (userId > 0 && !userIds.Contains(userId))
                    userIds.Add(userId);
                if (userId <= 0)
                    unmatchedNames.Add(visitorNames[i]);
            }

            return userIds;
        }

        private List<string> GetVisitorNameCandidates(JObject submission, DataRow row)
        {
            List<string> visitorNames = new List<string>();

            AddVisitorNameCandidate(visitorNames, GetFirstValue(submission, new string[] { "username", "_submitted_by" }));

            if (GetOpeningVisitKind(row) == "V-op" || IsAgricultureOpeningVisit(row))
            {
                AddVisitorChoiceCandidates(visitorNames, GetValue(submission, "__006"));
                AddVisitorTextCandidates(visitorNames, GetValue(submission, "__007"));
            }
            else
            {
                AddVisitorChoiceCandidates(visitorNames, GetValue(submission, "select_one_vt8tw79"));
                AddVisitorTextCandidates(visitorNames, GetValue(submission, IsLivestockOpeningVisit(row) ? "__005" : "text_xt6wd52"));
            }

            return visitorNames;
        }

        private void AddVisitorChoiceCandidates(List<string> visitorNames, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            string[] names = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < names.Length; i++)
                AddVisitorNameCandidate(visitorNames, names[i]);
        }

        private void AddVisitorTextCandidates(List<string> visitorNames, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            string[] names = value.Split(new char[] { ',', '،', ';', '\r', '\n', '/', '+', '&' },
                StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < names.Length; i++)
                AddVisitorNameCandidate(visitorNames, names[i]);
        }

        private void AddVisitorNameCandidate(List<string> visitorNames, string value)
        {
            string normalizedValue = NormalizeName(value);
            if (string.IsNullOrWhiteSpace(normalizedValue) || normalizedValue == "no compa" ||
                normalizedValue == "لا يوجد مرافق")
                return;

            for (int i = 0; i < visitorNames.Count; i++)
            {
                if (NormalizeName(visitorNames[i]) == normalizedValue)
                    return;
            }

            visitorNames.Add(value.Trim());
        }

        private DataTable GetVisitorUsers(MySqlConnection connection, MySqlTransaction transaction)
        {
            string query = @"SELECT `UserID`, `UserName`
FROM `user`
WHERE `IsVisitor` = 1";

            using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private int FindVisitorUserId(DataTable users, string visitorName)
        {
            string normalizedVisitorName = NormalizeName(visitorName);
            if (string.IsNullOrWhiteSpace(normalizedVisitorName))
                return -1;

            for (int i = 0; i < users.Rows.Count; i++)
            {
                string userName = users.Rows[i]["UserName"].ToString();
                if (NormalizeName(userName) == normalizedVisitorName)
                    return Convert.ToInt32(users.Rows[i]["UserID"]);
            }

            // Choice codes and labels from the supplied Opening XLSForms.
            string choiceLabel = "";
            switch (normalizedVisitorName)
            {
                case "simon": choiceLabel = "سيمون حجل"; break;
                case "kabi": choiceLabel = "كابي دايخ"; break;
                case "cloud": choiceLabel = "كلوديت كيروز"; break;
                case "fadi": choiceLabel = "فادي بغدصار"; break;
                case "dabbagh": choiceLabel = "فادي دباغ"; break;
            }
            if (choiceLabel.Length > 0)
            {
                for (int i = 0; i < users.Rows.Count; i++)
                {
                    if (NormalizeName(users.Rows[i]["UserName"].ToString()) == NormalizeName(choiceLabel))
                        return Convert.ToInt32(users.Rows[i]["UserID"]);
                }
            }

            return -1;
        }

        private string NormalizeName(string value)
        {
            if (value == null)
                return "";

            string name = value.Trim().Replace("_", " ");
            string[] parts = name.Split(new char[] { ' ', '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts).ToLowerInvariant();
        }

        private void RefreshVisitUsers(MySqlConnection connection, MySqlTransaction transaction,
            int visitId, List<int> userIds)
        {
            if (userIds.Count == 0)
                return;

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

        private void MarkVisitorImportError(MySqlConnection connection, MySqlTransaction transaction,
            int koboSubmissionId, string message)
        {
            ExecuteNonQuery(connection, transaction, @"UPDATE `kobo_submission` SET
`Sync_Status` = 'local_error', `Error_Message` = @Error_Message, `Updated_At` = @Updated_At
WHERE `ID` = @ID",
                Parameter("@ID", koboSubmissionId),
                Parameter("@Error_Message", message),
                Parameter("@Updated_At", DateTime.Now));
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
            int result;
            string fieldName = "text_fw08u96";
            if (GetOpeningVisitKind(row) == "V-op")
                fieldName = "__003";
            else if (IsAgricultureOpeningVisit(row) || IsLivestockOpeningVisit(row))
                fieldName = "__001";
            string value = GetValue(submission, fieldName);
            if (int.TryParse(value, out result))
                return result;
            return -1;
        }

        private bool IsAgricultureOpeningVisit(DataRow row)
        {
            string formName = row["Kobo_Form_Name"].ToString().ToLowerInvariant();
            string category = row["Visit_Category"].ToString().ToLowerInvariant();
            return formName.Contains("agre") || formName.Contains("agri") || category.Contains("agr");
        }

        private bool IsLivestockOpeningVisit(DataRow row)
        {
            return row["Kobo_Form_Name"].ToString().ToLowerInvariant().Contains("livestock") ||
                row["Visit_Category"].ToString().ToLowerInvariant().Contains("livestock");
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
