using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace MyWorkApplication.Classes
{
    public class KoboSyncResult
    {
        public int FormsCount { get; set; }
        public int ExistingCount { get; set; }
        public int SubmissionsCount { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int UnchangedCount { get; set; }
        public int DeletedCount { get; set; }
        public int AfterSyncCount { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorMessages { get; set; }

        public KoboSyncResult()
        {
            FormsCount = 0;
            ExistingCount = 0;
            SubmissionsCount = 0;
            InsertedCount = 0;
            UpdatedCount = 0;
            UnchangedCount = 0;
            DeletedCount = 0;
            AfterSyncCount = 0;
            ErrorCount = 0;
            ErrorMessages = "";
        }
    }

    public class KoboSync
    {
        private KoboSubmission koboSubmission;

        public KoboSync()
        {
            koboSubmission = new KoboSubmission();
        }

        public KoboSyncResult Sync_Raw_Submissions(string baseUrl, string token)
        {
            DataTable mappings = koboSubmission.Get_Active_Form_Mappings();
            return Sync_Raw_Submissions(baseUrl, token, mappings);
        }

        public KoboSyncResult Sync_Raw_Submissions(string baseUrl, string token, DataTable mappings)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("Kobo server URL is required.");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Kobo token is required.");

            MySqlConnection lockConnection = null;
            try
            {
                if (!TryAcquireSyncLock(out lockConnection))
                    throw new KoboSyncAlreadyRunningException();

                return Sync_Raw_Submissions_Without_Lock(baseUrl, token, mappings);
            }
            finally
            {
                ReleaseSyncLock(lockConnection);
            }
        }

        private KoboSyncResult Sync_Raw_Submissions_Without_Lock(string baseUrl, string token, DataTable mappings)
        {
            KoboSyncResult result = new KoboSyncResult();
            if (mappings == null)
                return result;

            result.ExistingCount = koboSubmission.Count_Active_Submissions_For_Current_Database();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            foreach (DataRow row in mappings.Rows)
            {
                string formUid = row["Kobo_Form_UID"].ToString();
                string formName = row["Kobo_Form_Name"].ToString();
                List<string> activeRootUuids = new List<string>();

                try
                {
                    Sync_Form(baseUrl, token, formUid, formName, activeRootUuids, result);
                    result.FormsCount++;
                    result.DeletedCount += koboSubmission.Mark_Missing_Submissions_As_Deleted(formUid, activeRootUuids);
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    AddError(result, formName + ": " + ex.Message);
                }
            }

            result.AfterSyncCount = koboSubmission.Count_Active_Submissions_For_Current_Database();
            Import_Opening_Visits(result);
            Import_Monitoring_Visits(result);
            return result;
        }

        private void Import_Monitoring_Visits(KoboSyncResult result)
        {
            try
            {
                KoboLocalVisitImportResult imported = new KoboMonitoringImporter().Import();
                result.ErrorCount += imported.ErrorCount;
                if (!string.IsNullOrWhiteSpace(imported.ErrorMessages)) AddError(result, imported.ErrorMessages);
            }
            catch (Exception ex)
            {
                result.ErrorCount++;
                AddError(result, "M&E local import: " + ex.Message);
            }
        }

        private void Import_Opening_Visits(KoboSyncResult result)
        {
            try
            {
                KoboLocalVisitImporter importer = new KoboLocalVisitImporter();
                KoboLocalVisitImportResult importResult = importer.Import_Opening_Visits();
                result.ErrorCount += importResult.ErrorCount;

                if (!string.IsNullOrWhiteSpace(importResult.ErrorMessages))
                    AddError(result, importResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                result.ErrorCount++;
                AddError(result, "Opening visits local import: " + ex.Message);
            }
        }

        private bool TryAcquireSyncLock(out MySqlConnection lockConnection)
        {
            lockConnection = new MySqlConnection(Program.ConnectionString);
            lockConnection.Open();

            using (MySqlCommand sc = new MySqlCommand("SELECT GET_LOCK(@LockName, 0)", lockConnection))
            {
                sc.Parameters.AddWithValue("@LockName", GetSyncLockName());
                object value = sc.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    lockConnection.Close();
                    lockConnection.Dispose();
                    lockConnection = null;
                    throw new Exception("لم يتمكن البرنامج من التحقق من حالة المزامنة الحالية.");
                }

                if (Convert.ToInt32(value) == 1)
                    return true;
            }

            lockConnection.Close();
            lockConnection.Dispose();
            lockConnection = null;
            return false;
        }

        private void ReleaseSyncLock(MySqlConnection lockConnection)
        {
            if (lockConnection == null)
                return;

            try
            {
                if (lockConnection.State == ConnectionState.Open)
                {
                    using (MySqlCommand sc = new MySqlCommand("SELECT RELEASE_LOCK(@LockName)", lockConnection))
                    {
                        sc.Parameters.AddWithValue("@LockName", GetSyncLockName());
                        sc.ExecuteScalar();
                    }
                }
            }
            finally
            {
                lockConnection.Close();
                lockConnection.Dispose();
            }
        }

        private string GetSyncLockName()
        {
            string database = Program.selected_database;
            if (string.IsNullOrWhiteSpace(database))
                database = "unknown";

            return "micro_kobo_visit_sync_" + database.Trim();
        }

        private void Sync_Form(string baseUrl, string token, string formUid, string formName,
            List<string> activeRootUuids, KoboSyncResult result)
        {
            string url = BuildDataUrl(baseUrl, formUid);

            while (!string.IsNullOrWhiteSpace(url))
            {
                string response = DownloadString(url, token);
                JObject page = JObject.Parse(response);
                JArray submissions = page["results"] as JArray;

                if (submissions != null)
                {
                    foreach (JToken submission in submissions)
                    {
                        string rawJson = submission.ToString(Formatting.None);
                        koboSubmission.Save_Raw_Submission(formUid, formName, rawJson);
                        AddStatus(result, koboSubmission.LastSaveStatus);
                        result.SubmissionsCount++;

                        string rootUuid = GetRootUuid(submission);
                        if (!string.IsNullOrWhiteSpace(rootUuid))
                            activeRootUuids.Add(rootUuid);
                    }
                }

                JToken nextToken = page["next"];
                if (nextToken == null || nextToken.Type == JTokenType.Null)
                    url = "";
                else
                    url = nextToken.ToString();
            }
        }

        private string DownloadString(string url, string token)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.Authorization, "Token " + token.Trim());
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                return client.DownloadString(url);
            }
        }

        private string BuildDataUrl(string baseUrl, string formUid)
        {
            baseUrl = baseUrl.Trim().TrimEnd('/');
            return baseUrl + "/api/v2/assets/" + formUid + "/data/?limit=1000";
        }

        private void AddStatus(KoboSyncResult result, string status)
        {
            if (status == "inserted")
                result.InsertedCount++;
            else if (status == "updated")
                result.UpdatedCount++;
            else if (status == "unchanged")
                result.UnchangedCount++;
        }

        private string GetRootUuid(JToken submission)
        {
            string rootUuid = GetValue(submission, "meta/rootUuid");
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = GetValue(submission, "rootUuid");
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = GetValue(submission, "_uuid");
            if (string.IsNullOrWhiteSpace(rootUuid))
                rootUuid = GetValue(submission, "meta/instanceID");

            return NormalizeUuid(rootUuid);
        }

        private string GetValue(JToken submission, string name)
        {
            JToken token = submission[name];
            if (token == null || token.Type == JTokenType.Null)
                return "";
            return token.ToString();
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

        private void AddError(KoboSyncResult result, string message)
        {
            if (!string.IsNullOrWhiteSpace(result.ErrorMessages))
                result.ErrorMessages += Environment.NewLine;
            result.ErrorMessages += message;
        }
    }

    public class KoboSyncAlreadyRunningException : Exception
    {
        public KoboSyncAlreadyRunningException()
            : base("توجد مزامنة زيارات قيد التنفيذ حالياً، يرجى الانتظار حتى تنتهي ثم المحاولة مرة أخرى.")
        {
        }
    }
}
