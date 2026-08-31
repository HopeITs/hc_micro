using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public int SubmissionsCount { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int UnchangedCount { get; set; }
        public int DeletedCount { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorMessages { get; set; }

        public KoboSyncResult()
        {
            FormsCount = 0;
            SubmissionsCount = 0;
            InsertedCount = 0;
            UpdatedCount = 0;
            UnchangedCount = 0;
            DeletedCount = 0;
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

            KoboSyncResult result = new KoboSyncResult();
            if (mappings == null)
                return result;

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

            return result;
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
}
