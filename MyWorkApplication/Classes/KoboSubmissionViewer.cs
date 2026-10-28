using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace MyWorkApplication.Classes
{
    public static class KoboSubmissionViewer
    {
        public static DataTable GetSubmissions(string search, string status)
        {
            Program.buildConnection();
            try
            {
                StringBuilder query = new StringBuilder(@"SELECT s.ID, s.Kobo_Submission_ID, s.Kobo_Form_Name,
s.Kobo_Form_ID, s.Kobo_Submission_Time, s.Kobo_Submitted_By, s.Sync_Status, s.Error_Message,
s.Kobo_Is_Deleted, s.Local_Table_Name, s.Local_Record_ID
FROM kobo_submission s WHERE 1 = 1");

                if (!string.IsNullOrWhiteSpace(search))
                    query.Append(@" AND (LOCATE(@search, COALESCE(s.Kobo_Submission_ID, '')) > 0
OR LOCATE(@search, COALESCE(s.Kobo_Form_Name, '')) > 0
OR LOCATE(@search, COALESCE(s.Kobo_Submitted_By, '')) > 0
OR LOCATE(@search, COALESCE(s.Error_Message, '')) > 0)");

                if (status == "errors")
                    query.Append(" AND s.Sync_Status = 'local_error'");
                else if (status == "imported")
                    query.Append(" AND s.Local_Record_ID IS NOT NULL AND s.Sync_Status <> 'local_error' AND s.Kobo_Is_Deleted = 0");
                else if (status == "unlinked")
                    query.Append(" AND s.Local_Record_ID IS NULL AND s.Kobo_Is_Deleted = 0");
                else if (status == "deleted")
                    query.Append(" AND s.Kobo_Is_Deleted = 1");

                query.Append(" ORDER BY s.Kobo_Submission_Time DESC, s.ID DESC");
                using (MySqlCommand command = new MySqlCommand(query.ToString(), Program.MyConn))
                {
                    command.Parameters.AddWithValue("@search", search ?? "");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public static DataRow GetSubmission(int id)
        {
            Program.buildConnection();
            try
            {
                using (MySqlCommand command = new MySqlCommand(@"SELECT Kobo_Form_ID, Raw_JSON
FROM kobo_submission WHERE ID = @id", Program.MyConn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table.Rows.Count == 0 ? null : table.Rows[0];
                    }
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        public static DataTable ParseAnswers(string formUid, string rawJson)
        {
            DataTable answers = new DataTable();
            answers.Columns.Add("Response", typeof(string));
            answers.Columns.Add("Question", typeof(string));
            answers.Columns.Add("Type", typeof(string));

            JObject submission = KoboMonitoringImporter.ParseSubmission(rawJson);
            foreach (JProperty property in submission.Properties())
                AddAnswer(answers, formUid, property.Name, property.Name, property.Value);
            return answers;
        }

        private static void AddAnswer(DataTable answers, string formUid, string path, string labelPath, JToken value)
        {
            if (path.StartsWith("_", StringComparison.Ordinal) ||
                path.StartsWith("__", StringComparison.Ordinal) ||
                path == "meta" || path.StartsWith("meta/", StringComparison.Ordinal) || path == "formhub/uuid")
                return;

            JObject group = value as JObject;
            if (group != null)
            {
                foreach (JProperty child in group.Properties())
                    AddAnswer(answers, formUid, path + "/" + child.Name, labelPath + "/" + child.Name, child.Value);
                return;
            }

            JArray repeat = value as JArray;
            if (repeat != null)
            {
                for (int i = 0; i < repeat.Count; i++)
                    AddAnswer(answers, formUid, path + " [" + (i + 1).ToString(CultureInfo.InvariantCulture) + "]", labelPath, repeat[i]);
                return;
            }

            string type = value.Type == JTokenType.Null ? "null" : value.Type.ToString();
            string response = value.Type == JTokenType.Null ? "" : value.Type == JTokenType.String
                ? value.ToString() : value.ToString(Newtonsoft.Json.Formatting.None);
            string label = KoboReviewLabels.Label(formUid, labelPath);
            string translated = KoboReviewLabels.Value(formUid, labelPath,
                value.Type == JTokenType.String ? Newtonsoft.Json.JsonConvert.ToString(response) : response);
            string suffix = RepeatSuffix(path);
            answers.Rows.Add(translated, label == labelPath ? path : label + suffix, type);
        }

        private static string RepeatSuffix(string path)
        {
            StringBuilder suffix = new StringBuilder();
            int position = 0;
            while (position < path.Length)
            {
                int start = path.IndexOf(" [", position, StringComparison.Ordinal);
                if (start < 0) break;
                int end = path.IndexOf(']', start + 2);
                if (end < 0) break;
                bool digits = end > start + 2;
                for (int i = start + 2; digits && i < end; i++) digits = char.IsDigit(path[i]);
                if (digits) suffix.Append(path.Substring(start, end - start + 1));
                position = end + 1;
            }
            return suffix.ToString();
        }
    }
}
