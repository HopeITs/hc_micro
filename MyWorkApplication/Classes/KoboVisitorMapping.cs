using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class KoboVisitVisitor
    {
        public string KoboName { get; set; }
        public string DisplayName { get; set; }
        public int? UserId { get; set; }
    }

    public class KoboVisitorMapping
    {
        private DataTable fields;
        private DataTable mappings;
        private DataTable users;

        public void Load()
        {
            Load(false);
        }

        public void Load(bool monitoring)
        {
            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            {
                connection.Open();
                try
                {
                    fields = ReadTable(connection, "SELECT f.* FROM `" +
                        (monitoring ? "kobo_monitoring_field_mapping" : "kobo_opening_field_mapping") + @"` f
JOIN `kobo_form_mapping` m ON BINARY m.`Kobo_Form_UID` = BINARY f.`Kobo_Form_UID`
WHERE m.`LocalDatabase` = DATABASE() AND m.`Is_Active` = 1");
                    mappings = ReadTable(connection, @"SELECT m.`Kobo_Form_UID`, m.`Kobo_Name`, m.`Display_Name`,
m.`Is_Ignored`, u.`UserID` AS `User_ID`
FROM `kobo_user_mapping` m
LEFT JOIN `user` u ON u.`UserID` = m.`User_ID`
WHERE m.`Is_Active` = 1");
                    users = ReadTable(connection, "SELECT `UserID`, `UserName` FROM `user` WHERE `IsVisitor` = 1");
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1146)
                        throw new Exception(monitoring ? "Apply KoboMonitoringImport_Upgrade.sql to the selected database before M&E import."
                            : "يرجى تطبيق ملف KoboVisitorMapping_Upgrade.sql على قاعدة البيانات قبل المزامنة.", ex);
                    throw;
                }
            }
        }

        public DataRow GetFields(string formUid)
        {
            foreach (DataRow row in fields.Rows)
            {
                if (row["Kobo_Form_UID"].ToString() == formUid)
                    return row;
            }
            throw new Exception("لم يتم ضبط حقول نموذج زيارة الافتتاحية: " + formUid);
        }

        public List<KoboVisitVisitor> GetVisitors(JObject submission, string formUid)
        {
            DataRow form = GetFields(formUid);
            List<KoboVisitVisitor> visitors = new List<KoboVisitVisitor>();
            string username = GetValue(submission, "username");
            if (string.IsNullOrWhiteSpace(username))
                username = GetValue(submission, "_submitted_by");
            AddVisitor(visitors, "", username);

            string choice = GetValue(submission, form["Visitor_Choice_Field"].ToString());
            if (!IsNoCompanionChoice(formUid, choice))
                foreach (string name in choice.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    AddVisitor(visitors, formUid, name);

            // This field is independent: it can name a visitor from another team even when the
            // selected companion choice says that no same-team companion joined the visit.
            string text = GetValue(submission, form["Visitor_Text_Field"].ToString());
            foreach (string name in text.Split(new char[] { ',', '،', ';', '\r', '\n', '/', '+', '&', '-', '_', '.' },
                StringSplitOptions.RemoveEmptyEntries))
                AddCompanions(visitors, name);
            return visitors;
        }

        private bool IsNoCompanionChoice(string formUid, string value)
        {
            if (NormalizeName(value) == NormalizeName("لا يوجد مرافق")) return true;
            DataRow mapping = FindMapping(formUid, NormalizeName(value));
            return mapping != null && (Convert.ToInt32(mapping["Is_Ignored"]) == 1 ||
                NormalizeName(mapping["Display_Name"].ToString()) == NormalizeName("لا يوجد مرافق"));
        }

        private void AddCompanions(List<KoboVisitVisitor> visitors, string value)
        {
            string text = string.Join(" ", value.Trim().Split(new char[] { ' ', '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries));
            int start = 0;
            int position = 0;
            // Only configured full aliases establish boundaries, never fuzzy name similarity.
            while (position < text.Length)
            {
                string match = "";
                bool boundary = position == 0 || char.IsWhiteSpace(text[position - 1]);
                int nameStart = position;
                if (boundary && text[position] == 'و')
                {
                    nameStart++;
                    while (nameStart < text.Length && char.IsWhiteSpace(text[nameStart])) nameStart++;
                }
                if (boundary)
                {
                    foreach (DataRow row in mappings.Rows)
                    {
                        if (row["Kobo_Form_UID"].ToString() != "" || Convert.ToInt32(row["Is_Ignored"]) == 1)
                            continue;
                        string alias = NormalizeName(row["Kobo_Name"].ToString());
                        int end = nameStart + alias.Length;
                        if (alias.Length > match.Length && end <= text.Length &&
                            string.Compare(text, nameStart, alias, 0, alias.Length, StringComparison.OrdinalIgnoreCase) == 0 &&
                            (end == text.Length || char.IsWhiteSpace(text[end])))
                            match = alias;
                    }
                }
                if (match.Length == 0) { position++; continue; }
                AddVisitor(visitors, "", text.Substring(start, position - start));
                AddVisitor(visitors, "", text.Substring(nameStart, match.Length));
                position = nameStart + match.Length;
                start = position;
            }
            AddVisitor(visitors, "", text.Substring(start));
        }

        private void AddVisitor(List<KoboVisitVisitor> visitors, string formUid, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            string key = NormalizeName(value);
            DataRow mapping = FindMapping(formUid, key);
            if (mapping != null && Convert.ToInt32(mapping["Is_Ignored"]) == 1)
                return;

            KoboVisitVisitor visitor = new KoboVisitVisitor();
            visitor.KoboName = value.Trim();
            visitor.DisplayName = mapping == null || string.IsNullOrWhiteSpace(mapping["Display_Name"].ToString())
                ? value.Trim() : mapping["Display_Name"].ToString().Trim();
            if (mapping != null)
            {
                if (mapping["User_ID"] != DBNull.Value)
                    visitor.UserId = Convert.ToInt32(mapping["User_ID"]);
            }
            else
            {
                foreach (DataRow user in users.Rows)
                {
                    if (NormalizeName(user["UserName"].ToString()) == key)
                    {
                        visitor.UserId = Convert.ToInt32(user["UserID"]);
                        break;
                    }
                }
            }

            foreach (KoboVisitVisitor existing in visitors)
            {
                if ((visitor.UserId.HasValue && visitor.UserId == existing.UserId) ||
                    NormalizeName(visitor.DisplayName) == NormalizeName(existing.DisplayName))
                    return;
            }
            visitors.Add(visitor);
        }

        private DataRow FindMapping(string formUid, string key)
        {
            DataRow global = null;
            foreach (DataRow row in mappings.Rows)
            {
                if (NormalizeName(row["Kobo_Name"].ToString()) != key)
                    continue;
                if (row["Kobo_Form_UID"].ToString() == formUid)
                    return row;
                if (row["Kobo_Form_UID"].ToString() == "")
                    global = row;
            }
            return global;
        }

        private string NormalizeName(string value)
        {
            return string.Join(" ", value.Trim().Split(new char[] { ' ', '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
        }

        public static string GetValue(JObject submission, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";
            JToken token = submission[path];
            if (token != null && token.Type != JTokenType.Null)
                return token.ToString().Trim();
            // Older form versions may flatten fields or rename their containing group.
            string name = path.Substring(path.LastIndexOf('/') + 1);
            token = submission[name];
            if (token != null && token.Type != JTokenType.Null)
                return token.ToString().Trim();
            string result = "";
            int count = 0;
            foreach (JProperty property in submission.Properties())
            {
                if (property.Name.EndsWith("/" + name, StringComparison.Ordinal) && property.Value.Type != JTokenType.Null)
                {
                    result = property.Value.ToString().Trim();
                    count++;
                }
            }
            if (count > 1)
                throw new Exception("يوجد أكثر من حقل كوبو بالاسم: " + name);
            return result;
        }

        private DataTable ReadTable(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
