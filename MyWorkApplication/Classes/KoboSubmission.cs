using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MyWorkApplication.Classes
{
    public class KoboSubmission
    {
        private string query;

        public string LastSaveStatus { get; private set; }

        public KoboSubmission()
        {
            query = "";
            LastSaveStatus = "";
        }

        public DataTable Get_Active_Form_Mappings()
        {
            return Get_Active_Form_Mappings(GetCurrentLocalDatabaseName());
        }

        public DataTable Get_Active_Form_Mappings(string localDatabase)
        {
            Program.buildConnection();
            try
            {
                query = @"SELECT `ID`, `LocalDatabase`, `Governorate`, `Kobo_Form_UID`, `Kobo_Form_Name`,
`Visit_Type`, `Visit_Category`, `Target_Table`
FROM `kobo_form_mapping`
WHERE `LocalDatabase` = @LocalDatabase
AND `Is_Active` = 1
ORDER BY `Visit_Type`, `Visit_Category`, `Kobo_Form_Name`";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@LocalDatabase", localDatabase);
                    MySqlDataAdapter da = new MySqlDataAdapter(sc);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public int Save_Raw_Submission(string formName, string rawJson)
        {
            return Save_Raw_Submission("", formName, rawJson);
        }

        public int Save_Raw_Submission(string formUid, string formName, string rawJson)
        {
            JObject submission = JObject.Parse(rawJson);
            string cleanJson = submission.ToString(Formatting.None);

            string rootUuid = NormalizeUuid(GetValue(submission, "meta/rootUuid"));
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = NormalizeUuid(GetValue(submission, "rootUuid"));
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = NormalizeUuid(GetValue(submission, "_uuid"));
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = NormalizeUuid(GetValue(submission, "meta/instanceID"));

            if (string.IsNullOrWhiteSpace(rootUuid))
                throw new Exception("Kobo submission has no root UUID.");

            string currentUuid = NormalizeUuid(GetValue(submission, "_uuid"));
            string submissionId = GetValue(submission, "_id");
            string formId = GetValue(submission, "_xform_id_string");
            if (!string.IsNullOrWhiteSpace(formUid))
                formId = formUid;

            string formVersion = GetValue(submission, "__version__");
            if (string.IsNullOrWhiteSpace(formName))
                formName = formId;
            if (string.IsNullOrWhiteSpace(formName))
                formName = "Kobo Form";

            DateTime? submissionTime = GetDateValue(submission, "_submission_time");
            string submittedBy = GetValue(submission, "_submitted_by");
            string status = GetValue(submission, "_status");
            string validationStatus = GetJsonValue(submission, "_validation_status");
            string jsonHash = GetSha256(cleanJson);

            Program.buildConnection();
            try
            {
                int existingId = GetExistingSubmissionId(rootUuid, jsonHash);
                if (existingId > 0)
                {
                    LastSaveStatus = "unchanged";
                    return existingId;
                }

                int id = GetExistingSubmissionId(rootUuid);
                if (id > 0)
                {
                    UpdateSubmission(id, rootUuid, currentUuid, submissionId, formId, formVersion, formName,
                        submissionTime, submittedBy, status, validationStatus, cleanJson, jsonHash);
                    LastSaveStatus = "updated";
                    return id;
                }

                id = InsertSubmission(rootUuid, currentUuid, submissionId, formId, formVersion, formName,
                    submissionTime, submittedBy, status, validationStatus, cleanJson, jsonHash);
                LastSaveStatus = "inserted";
                return id;
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public int Mark_Missing_Submissions_As_Deleted(string formUid, IList<string> activeRootUuids)
        {
            if (string.IsNullOrWhiteSpace(formUid))
                return 0;

            Program.buildConnection();
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(@"UPDATE `kobo_submission` SET
`Kobo_Is_Deleted` = 1,
`Kobo_Deleted_At` = @Kobo_Deleted_At,
`Sync_Status` = @Sync_Status,
`Updated_At` = @Updated_At
WHERE `Kobo_Form_ID` = @Kobo_Form_ID
AND `Kobo_Is_Deleted` = 0");

                if (activeRootUuids != null && activeRootUuids.Count > 0)
                {
                    builder.Append(" AND `Kobo_Root_UUID` NOT IN (");
                    for (int i = 0; i < activeRootUuids.Count; i++)
                    {
                        if (i > 0)
                            builder.Append(", ");
                        builder.Append("@RootUuid" + i);
                    }
                    builder.Append(")");
                }

                using (MySqlCommand sc = new MySqlCommand(builder.ToString(), Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@Kobo_Form_ID", formUid);
                    sc.Parameters.AddWithValue("@Kobo_Deleted_At", DateTime.Now);
                    sc.Parameters.AddWithValue("@Sync_Status", "deleted");
                    sc.Parameters.AddWithValue("@Updated_At", DateTime.Now);

                    if (activeRootUuids != null)
                    {
                        for (int i = 0; i < activeRootUuids.Count; i++)
                            sc.Parameters.AddWithValue("@RootUuid" + i, NormalizeUuid(activeRootUuids[i]));
                    }

                    return sc.ExecuteNonQuery();
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public int Count_Active_Submissions_For_Current_Database()
        {
            Program.buildConnection();
            try
            {
                query = @"SELECT COUNT(DISTINCT kobo_submission.`ID`)
FROM `kobo_submission`
JOIN `kobo_form_mapping`
ON kobo_form_mapping.`Kobo_Form_UID` = kobo_submission.`Kobo_Form_ID`
WHERE kobo_form_mapping.`LocalDatabase` = @LocalDatabase
AND kobo_form_mapping.`Is_Active` = 1
AND kobo_submission.`Kobo_Is_Deleted` = 0";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@LocalDatabase", GetCurrentLocalDatabaseName());
                    object value = sc.ExecuteScalar();
                    if (value == null || value == DBNull.Value)
                        return 0;
                    return Convert.ToInt32(value);
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Submissions_For_Local_Import()
        {
            return Get_Submissions_For_Local_Import("");
        }

        public DataTable Get_Submissions_For_Local_Import(string targetTable)
        {
            Program.buildConnection();
            try
            {
                query = @"SELECT kobo_submission.`ID`,
kobo_submission.`Kobo_Root_UUID`,
kobo_submission.`Kobo_Current_UUID`,
kobo_submission.`Kobo_Submission_ID`,
kobo_submission.`Kobo_Form_ID`,
kobo_submission.`Kobo_Form_Name`,
kobo_submission.`Kobo_Submission_Time`,
kobo_submission.`Sync_Status`,
kobo_submission.`Local_Table_Name`,
kobo_submission.`Local_Record_ID`,
kobo_submission.`Raw_JSON`,
kobo_form_mapping.`Visit_Type`,
kobo_form_mapping.`Visit_Category`,
kobo_form_mapping.`Target_Table`
FROM `kobo_submission`
JOIN `kobo_form_mapping`
ON kobo_form_mapping.`Kobo_Form_UID` = kobo_submission.`Kobo_Form_ID`
WHERE kobo_form_mapping.`LocalDatabase` = @LocalDatabase
AND kobo_form_mapping.`Is_Active` = 1
AND kobo_submission.`Kobo_Is_Deleted` = 0
AND (kobo_submission.`Sync_Status` IN ('raw_imported', 'raw_updated', 'local_error')
     OR kobo_submission.`Local_Record_ID` IS NULL)";

                if (!string.IsNullOrWhiteSpace(targetTable))
                    query += " AND kobo_form_mapping.`Target_Table` = @TargetTable";

                query += @" ORDER BY kobo_submission.`Kobo_Submission_Time`,
kobo_submission.`ID`";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@LocalDatabase", GetCurrentLocalDatabaseName());
                    if (!string.IsNullOrWhiteSpace(targetTable))
                        sc.Parameters.AddWithValue("@TargetTable", targetTable);

                    MySqlDataAdapter da = new MySqlDataAdapter(sc);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Deleted_Submissions_For_Local_Import()
        {
            Program.buildConnection();
            try
            {
                query = @"SELECT `ID`,
`Kobo_Root_UUID`,
`Kobo_Form_ID`,
`Kobo_Form_Name`,
`Sync_Status`,
`Local_Table_Name`,
`Local_Record_ID`
FROM `kobo_submission`
WHERE `Kobo_Is_Deleted` = 1
AND `Local_Table_Name` IS NOT NULL
AND `Local_Record_ID` IS NOT NULL
AND `Sync_Status` = 'deleted'
ORDER BY `Kobo_Deleted_At`, `ID`";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sc);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public void Mark_Local_Import_Success(int koboSubmissionId, string localTableName, int localRecordId, bool updated)
        {
            Program.buildConnection();
            try
            {
                query = @"UPDATE `kobo_submission` SET
`Sync_Status` = @Sync_Status,
`Local_Table_Name` = @Local_Table_Name,
`Local_Record_ID` = @Local_Record_ID,
`Error_Message` = NULL,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@ID", koboSubmissionId);
                    sc.Parameters.AddWithValue("@Sync_Status", updated ? "local_updated" : "local_inserted");
                    sc.Parameters.AddWithValue("@Local_Table_Name", DbString(localTableName));
                    sc.Parameters.AddWithValue("@Local_Record_ID", localRecordId);
                    sc.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    sc.ExecuteNonQuery();
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public void Mark_Local_Delete_Success(int koboSubmissionId)
        {
            Program.buildConnection();
            try
            {
                query = @"UPDATE `kobo_submission` SET
`Sync_Status` = 'local_deleted',
`Error_Message` = NULL,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@ID", koboSubmissionId);
                    sc.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    sc.ExecuteNonQuery();
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public void Mark_Local_Import_Error(int koboSubmissionId, string errorMessage)
        {
            Program.buildConnection();
            try
            {
                query = @"UPDATE `kobo_submission` SET
`Sync_Status` = 'local_error',
`Error_Message` = @Error_Message,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

                using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
                {
                    sc.Parameters.AddWithValue("@ID", koboSubmissionId);
                    sc.Parameters.AddWithValue("@Error_Message", DbString(errorMessage));
                    sc.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                    sc.ExecuteNonQuery();
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        private int InsertSubmission(string rootUuid, string currentUuid, string submissionId, string formId,
            string formVersion, string formName, DateTime? submissionTime, string submittedBy, string status,
            string validationStatus, string cleanJson, string jsonHash)
        {
            query = @"INSERT INTO `kobo_submission`
(`Kobo_Root_UUID`, `Kobo_Current_UUID`, `Kobo_Submission_ID`, `Kobo_Form_ID`, `Kobo_Form_Version`,
 `Kobo_Form_Name`, `Kobo_Submission_Time`, `Kobo_Submitted_By`, `Kobo_Status`, `Kobo_Validation_Status`,
 `Sync_Status`, `Raw_JSON`, `Raw_JSON_Hash`, `Created_At`)
VALUES
(@Kobo_Root_UUID, @Kobo_Current_UUID, @Kobo_Submission_ID, @Kobo_Form_ID, @Kobo_Form_Version,
 @Kobo_Form_Name, @Kobo_Submission_Time, @Kobo_Submitted_By, @Kobo_Status, @Kobo_Validation_Status,
 @Sync_Status, @Raw_JSON, @Raw_JSON_Hash, @Created_At)";

            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                AddSubmissionParameters(sc, rootUuid, currentUuid, submissionId, formId, formVersion, formName,
                    submissionTime, submittedBy, status, validationStatus, cleanJson, jsonHash, "raw_imported");
                sc.Parameters.AddWithValue("@Created_At", DateTime.Now);
                sc.ExecuteNonQuery();
            }

            using (MySqlCommand sc = new MySqlCommand("SELECT LAST_INSERT_ID();", Program.MyConn))
            {
                return Convert.ToInt32(sc.ExecuteScalar());
            }
        }

        private void UpdateSubmission(int id, string rootUuid, string currentUuid, string submissionId, string formId,
            string formVersion, string formName, DateTime? submissionTime, string submittedBy, string status,
            string validationStatus, string cleanJson, string jsonHash)
        {
            query = @"UPDATE `kobo_submission` SET
`Kobo_Root_UUID` = @Kobo_Root_UUID,
`Kobo_Current_UUID` = @Kobo_Current_UUID,
`Kobo_Submission_ID` = @Kobo_Submission_ID,
`Kobo_Form_ID` = @Kobo_Form_ID,
`Kobo_Form_Version` = @Kobo_Form_Version,
`Kobo_Form_Name` = @Kobo_Form_Name,
`Kobo_Submission_Time` = @Kobo_Submission_Time,
`Kobo_Submitted_By` = @Kobo_Submitted_By,
`Kobo_Status` = @Kobo_Status,
`Kobo_Validation_Status` = @Kobo_Validation_Status,
`Kobo_Is_Deleted` = 0,
`Kobo_Deleted_At` = NULL,
`Sync_Status` = @Sync_Status,
`Raw_JSON` = @Raw_JSON,
`Raw_JSON_Hash` = @Raw_JSON_Hash,
`Error_Message` = NULL,
`Updated_At` = @Updated_At
WHERE `ID` = @ID";

            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", id);
                AddSubmissionParameters(sc, rootUuid, currentUuid, submissionId, formId, formVersion, formName,
                    submissionTime, submittedBy, status, validationStatus, cleanJson, jsonHash, "raw_updated");
                sc.Parameters.AddWithValue("@Updated_At", DateTime.Now);
                sc.ExecuteNonQuery();
            }
        }

        private void AddSubmissionParameters(MySqlCommand sc, string rootUuid, string currentUuid, string submissionId,
            string formId, string formVersion, string formName, DateTime? submissionTime, string submittedBy,
            string status, string validationStatus, string cleanJson, string jsonHash, string syncStatus)
        {
            sc.Parameters.AddWithValue("@Kobo_Root_UUID", rootUuid);
            sc.Parameters.AddWithValue("@Kobo_Current_UUID", DbString(currentUuid));
            sc.Parameters.AddWithValue("@Kobo_Submission_ID", DbString(submissionId));
            sc.Parameters.AddWithValue("@Kobo_Form_ID", DbString(formId));
            sc.Parameters.AddWithValue("@Kobo_Form_Version", DbString(formVersion));
            sc.Parameters.AddWithValue("@Kobo_Form_Name", DbString(formName));
            sc.Parameters.AddWithValue("@Kobo_Submission_Time", DbDate(submissionTime));
            sc.Parameters.AddWithValue("@Kobo_Submitted_By", DbString(submittedBy));
            sc.Parameters.AddWithValue("@Kobo_Status", DbString(status));
            sc.Parameters.AddWithValue("@Kobo_Validation_Status", DbString(validationStatus));
            sc.Parameters.AddWithValue("@Sync_Status", syncStatus);
            sc.Parameters.AddWithValue("@Raw_JSON", cleanJson);
            sc.Parameters.AddWithValue("@Raw_JSON_Hash", jsonHash);
        }

        private int GetExistingSubmissionId(string rootUuid)
        {
            query = "SELECT `ID` FROM `kobo_submission` WHERE `Kobo_Root_UUID` = @Kobo_Root_UUID LIMIT 1";
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Kobo_Root_UUID", rootUuid);
                object value = sc.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return -1;
                return Convert.ToInt32(value);
            }
        }

        private int GetExistingSubmissionId(string rootUuid, string jsonHash)
        {
            query = @"SELECT `ID` FROM `kobo_submission`
WHERE `Kobo_Root_UUID` = @Kobo_Root_UUID
AND `Raw_JSON_Hash` = @Raw_JSON_Hash
LIMIT 1";

            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Kobo_Root_UUID", rootUuid);
                sc.Parameters.AddWithValue("@Raw_JSON_Hash", jsonHash);
                object value = sc.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                    return -1;
                return Convert.ToInt32(value);
            }
        }

        private string GetValue(JObject submission, string name)
        {
            JToken token = submission[name];
            if (token == null || token.Type == JTokenType.Null)
                return "";
            return token.ToString();
        }

        private string GetJsonValue(JObject submission, string name)
        {
            JToken token = submission[name];
            if (token == null || token.Type == JTokenType.Null)
                return "";
            return token.ToString(Formatting.None);
        }

        private DateTime? GetDateValue(JObject submission, string name)
        {
            DateTime result;
            string value = GetValue(submission, name);
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result))
                return result;
            return null;
        }

        private string NormalizeUuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            value = value.Trim();
            if (value.StartsWith("uuid:", StringComparison.OrdinalIgnoreCase))
                value = value.Substring(5);

            return value;
        }

        private string GetSha256(string value)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }

        private object DbString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;
            return value.Trim();
        }

        private object DbDate(DateTime? value)
        {
            if (value.HasValue)
                return value.Value;
            return DBNull.Value;
        }

        private string GetCurrentLocalDatabaseName()
        {
            if (Program.selected_database == "Aleppo")
                return "hcsyria_micro";
            if (Program.selected_database == "Homs")
                return "hcsyria_micro_h";
            if (Program.selected_database == "Damascus")
                return "hcsyria_micro_d";
            if (Program.selected_database == "Latakia")
                return "hcsyria_micro_l";
            if (Program.selected_database == "AlWadi")
                return "hcsyria_micro_w";
            if (Program.selected_database == "Lebanon")
                return "hcsyria_micro_lb";

            return "";
        }
    }
}
