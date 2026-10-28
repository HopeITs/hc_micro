using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyWorkApplication.Classes
{
    public sealed class KoboResultsFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Version { get; set; }
        public string Stage { get; set; }
        public string Kind { get; set; }
        public string ProjectType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Career { get; set; }
        public bool LastVisitOnly { get; set; }
    }

    public sealed class KoboResultsReport
    {
        public DataTable Table { get; set; }
        public int Visits { get; set; }
        public int Excluded { get; set; }
        public int InvalidJson { get; set; }
        public int InvalidDate { get; set; }
        public int UnmappedFields { get; set; }
    }

    public static class KoboVisitResults
    {
        private sealed class Field
        {
            public string Path, Label, Type, SourcePath, Section, Context;
            public bool Repeated;
            public List<KeyValuePair<string, string>> Choices = new List<KeyValuePair<string, string>>();
        }

        private sealed class Response
        {
            public DataRow Source;
            public Dictionary<string, List<string>> Values;
            public HashSet<string> Applicable;
        }

        private static readonly Lazy<JObject> catalog = new Lazy<JObject>(LoadCatalog);

        private static JObject LoadCatalog()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyWorkApplication.KoboQuestionCatalog.json"))
            {
                if (stream == null) throw new InvalidOperationException("Kobo question catalog is missing.");
                using (StreamReader reader = new StreamReader(stream)) return JObject.Parse(reader.ReadToEnd());
            }
        }

        public static DataTable GetForms()
        {
            DataTable forms = Read(@"SELECT MIN(Kobo_Form_ID) AS UID, MAX(Kobo_Form_Name) AS Name
FROM kobo_submission WHERE Kobo_Form_ID IS NOT NULL AND Kobo_Form_ID <> ''
GROUP BY BINARY Kobo_Form_ID ORDER BY Name", null);
            DataRow all = forms.NewRow(); all["UID"] = ""; all["Name"] = "جميع نماذج كوبو";
            forms.Rows.InsertAt(all, 0);
            return forms;
        }

        public static DataTable GetSources(string uid)
        {
            // The registry supplies the project/person pair; never join monitoring visits back by person alone.
            return Read(@"SELECT s.ID, s.Kobo_Submission_ID, s.Kobo_Form_ID, s.Kobo_Form_Name, s.Kobo_Form_Version, s.Raw_JSON,
s.Sync_Status, s.Kobo_Is_Deleted,
COALESCE(r.MicroProject_ID, o.MicroProject_ID) AS Project_ID, r.Person_ID,
COALESCE(r.Visit_Date, o.Date) AS Visit_Date,
CASE WHEN r.Submission_ID IS NOT NULL THEN CONCAT('M', r.Visit_Number)
     WHEN o.ID IS NOT NULL THEN 'Opening' ELSE '' END AS Stage,
COALESCE(r.Visit_Kind, o.Kind) AS Visit_Kind,
CASE WHEN r.Submission_ID IS NOT NULL THEN CONCAT_WS(' ', p.P_FirstName, p.P_LastName)
     ELSE (SELECT GROUP_CONCAT(DISTINCT CONCAT_WS(' ', b.P_FirstName, b.P_LastName) ORDER BY b.P_ID SEPARATOR ' / ')
           FROM person_microproject pm JOIN person b ON b.P_ID = pm.Person_ID
           WHERE pm.MicroProject_ID = o.MicroProject_ID AND TRIM(pm.PersonType) IN ('مستفيد','شريك')) END AS Beneficiary,
mp.MP_Type_ID, mp.Category_ID, mp.SubCategory_ID, mp.Career_ID,
sc.Name AS SubCategory_Name, c.Name AS Career_Name
FROM kobo_submission s
LEFT JOIN kobo_monitoring_record r ON r.Submission_ID = s.ID
 AND ((s.Local_Table_Name = 'mevisit' AND s.Local_Record_ID = r.Legacy_Visit_ID)
 OR (s.Local_Table_Name = 'kobo_monitoring_record' AND s.Local_Record_ID = r.Submission_ID))
LEFT JOIN visit o ON s.Local_Table_Name = 'visit' AND s.Local_Record_ID = o.ID
LEFT JOIN microproject mp ON mp.MP_ID = COALESCE(r.MicroProject_ID, o.MicroProject_ID)
LEFT JOIN person p ON p.P_ID = r.Person_ID
LEFT JOIN subcategory sc ON sc.ID = mp.SubCategory_ID
LEFT JOIN career c ON c.ID = mp.Career_ID
WHERE (@uid IS NULL OR BINARY s.Kobo_Form_ID = BINARY @uid) ORDER BY s.ID", string.IsNullOrEmpty(uid) ? null : uid, true);
        }

        private static DataTable Read(string sql, string uid, bool requireMonitoringRegistry = false)
        {
            Program.buildConnection();
            try
            {
                if (requireMonitoringRegistry)
                {
                    using (MySqlCommand schema = new MySqlCommand(@"SELECT COUNT(*) FROM information_schema.tables
WHERE table_schema = DATABASE() AND table_name = 'kobo_monitoring_record'", Program.MyConn))
                        ValidateMonitoringSchema(Convert.ToInt32(schema.ExecuteScalar()) > 0, Program.MyConn.Database);
                }
                using (MySqlCommand command = new MySqlCommand(sql, Program.MyConn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    if (requireMonitoringRegistry) command.Parameters.AddWithValue("@uid", (object)uid ?? DBNull.Value);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
            finally { Program.MyConn.Close(); }
        }

        private static void ValidateMonitoringSchema(bool exists, string database)
        {
            if (exists) return;
            throw new InvalidOperationException("تعذر عرض نتائج زيارات كوبو لأن تحديث استيراد زيارات المتابعة غير مكتمل في قاعدة البيانات: " + database +
                Environment.NewLine + "الجدول المطلوب غير موجود: kobo_monitoring_record" +
                Environment.NewLine + "بعد أخذ نسخة احتياطية، يرجى تطبيق التحديثات التالية على قاعدة البيانات نفسها بالترتيب:" +
                Environment.NewLine + "1. KoboVisitorMapping_Upgrade.sql" +
                Environment.NewLine + "2. KoboMonitoringStage_Upgrade.sql" +
                Environment.NewLine + "3. KoboMonitoringImport_Upgrade.sql" +
                Environment.NewLine + "ثم أعد المزامنة وحدّث التقرير. لم يتم تغيير أي بيانات أو حساب نتائج غير مكتملة.");
        }

        public static DataTable GetVersions(DataTable sources)
        {
            DataTable versions = new DataTable();
            versions.Columns.Add("Key"); versions.Columns.Add("Name");
            versions.Rows.Add("all", "جميع الإصدارات");
            SortedSet<string> seen = new SortedSet<string>(StringComparer.Ordinal);
            foreach (DataRow row in sources.Rows) seen.Add(Text(row, "Kobo_Form_Version"));
            foreach (string version in seen) versions.Rows.Add("v:" + version, version == "" ? "إصدار غير مسجل" : version);
            return versions;
        }

        public static KoboResultsReport Build(DataTable sources, string uid, KoboResultsFilter filter, bool counts)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            List<Field> fields = Fields(uid);
            Dictionary<string, Field> known = new Dictionary<string, Field>(StringComparer.Ordinal);
            foreach (Field field in fields) known[field.Path] = field;
            KoboResultsReport report = new KoboResultsReport();
            List<Response> responses = ReadResponses(sources, filter, report);
            foreach (Response response in responses)
                foreach (string path in response.Values.Keys)
                    if (!known.ContainsKey(path) && !Metadata(path))
                    {
                        Field field = new Field { Path = path, Label = path, Type = "unknown" };
                        fields.Add(field); known.Add(path, field);
                    }
            report.Visits = responses.Count;
            report.Table = counts ? Counts(fields, responses) : Beneficiaries(fields, responses);
            return report;
        }

        private static List<Response> ReadResponses(DataTable sources, KoboResultsFilter filter, KoboResultsReport report)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            List<Response> responses = new List<Response>();
            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (DataRow row in sources.Rows)
            {
                if (filter.Version != null && filter.Version != Text(row, "Kobo_Form_Version")) continue;
                if (!seen.Add(Text(row, "ID"))) continue;
                if (!Confirmed(row)) { report.Excluded++; continue; }
                try
                {
                    JObject submission = KoboMonitoringImporter.ParseSubmission(Text(row, "Raw_JSON"));
                    DataRow source = row;
                    if (Text(row, "Stage") == "Opening")
                    {
                        // Several sources can share a legacy opening row. Its current date is not each source's visit date.
                        source = row.Table.NewRow(); source.ItemArray = (object[])row.ItemArray.Clone();
                        try { source["Visit_Date"] = KoboMonitoringImporter.ResolveDate(submission).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); }
                        catch (InvalidOperationException) { report.InvalidDate++; continue; }
                        string kind = Text(row, "Visit_Kind");
                        if (kind == "V-op") source["Visit_Kind"] = "v";
                        if (kind == "O-op") source["Visit_Kind"] = "o";
                    }
                    if (!Matches(source, filter)) continue;
                    Dictionary<string, List<string>> values = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                    Flatten(submission, "", values);
                    responses.Add(new Response { Source = source, Values = values });
                }
                catch (Newtonsoft.Json.JsonException) { report.InvalidJson++; }
            }
            if (filter.LastVisitOnly) responses = Latest(responses);
            return responses;
        }

        // Forms store different paths/codes for shared questions. Combine meanings, never raw codes or generated field names.
        public static KoboResultsReport BuildCombined(DataTable sources, KoboResultsFilter filter, bool counts)
        {
            KoboResultsReport report = new KoboResultsReport();
            List<Response> responses = ReadResponses(sources, filter, report);
            Dictionary<string, List<Field>> definitions = new Dictionary<string, List<Field>>(StringComparer.Ordinal);
            Dictionary<string, Field> questions = new Dictionary<string, Field>(StringComparer.Ordinal);
            Dictionary<string, Dictionary<string, string>> choiceKeys = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            List<Field> fields = new List<Field>();
            HashSet<string> unmapped = new HashSet<string>(StringComparer.Ordinal);
            foreach (Response response in responses)
            {
                string uid = Text(response.Source, "Kobo_Form_ID");
                List<Field> local;
                if (!definitions.TryGetValue(uid, out local)) { local = Fields(uid); definitions.Add(uid, local); }
                List<Field> available = new List<Field>(local);
                HashSet<string> paths = new HashSet<string>(StringComparer.Ordinal);
                Dictionary<string, int> labelOccurrences = new Dictionary<string, int>(StringComparer.Ordinal);
                Dictionary<string, int> scopeOccurrences = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (Field field in local)
                {
                    paths.Add(field.Path);
                    string label = NormalizeQuestion(field.Label);
                    int occurrences; labelOccurrences.TryGetValue(label, out occurrences); labelOccurrences[label] = occurrences + 1;
                    string scopeKey = label + "\u001f" + NormalizeQuestion(field.Repeated ? field.Section : field.Context);
                    scopeOccurrences.TryGetValue(scopeKey, out occurrences); scopeOccurrences[scopeKey] = occurrences + 1;
                }
                foreach (string path in response.Values.Keys)
                    if (!paths.Contains(path) && !Metadata(path) && IsAssessmentQuestion(path, path))
                        unmapped.Add(uid + ":" + path);
                available.RemoveAll(delegate(Field field) { return !IsAssessmentQuestion(field.Path, field.Label); });

                Dictionary<string, List<string>> merged = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                response.Applicable = new HashSet<string>(StringComparer.Ordinal);
                foreach (Field localField in available)
                {
                    string label = NormalizeQuestion(localField.Label);
                    string family = ChoiceField(localField) ? (localField.Type.StartsWith("select_multiple ", StringComparison.Ordinal) ? "select_multiple combined" : "select_one combined") : "value";
                    int occurrences; labelOccurrences.TryGetValue(label, out occurrences);
                    bool contextual = localField.Repeated || occurrences > 1 || FollowUp(label);
                    string context = localField.Repeated ? localField.Section : localField.Context;
                    string scope = contextual ? NormalizeQuestion(context) : "";
                    int scopeCount; scopeOccurrences.TryGetValue(label + "\u001f" + scope, out scopeCount);
                    bool unresolved = contextual && (scope == "" || scopeCount > 1);
                    string key = family + "\u001f" + label + "\u001f" + scope + (unresolved ? "\u001f" + uid + "\u001f" + localField.Path : "");
                    Field field;
                    if (!questions.TryGetValue(key, out field))
                    {
                        field = new Field { Path = "question_" + fields.Count, Label = localField.Label, Type = family, SourcePath = "" };
                        if (contextual && !string.IsNullOrWhiteSpace(context) && NormalizeQuestion(context) != label)
                            field.Label += " - " + context;
                        questions.Add(key, field); fields.Add(field);
                        choiceKeys.Add(field.Path, new Dictionary<string, string>(StringComparer.Ordinal));
                    }
                    string sourcePath = uid + ": " + localField.Path;
                    if (!("\n" + field.SourcePath + "\n").Contains("\n" + sourcePath + "\n"))
                        field.SourcePath += (field.SourcePath == "" ? "" : "\n") + sourcePath;
                    response.Applicable.Add(field.Path);
                    List<string> values = Values(response, localField);
                    List<string> converted = new List<string>();
                    if (ChoiceField(localField))
                    {
                        Dictionary<string, string> options = choiceKeys[field.Path];
                        Dictionary<string, string> codes = new Dictionary<string, string>(StringComparer.Ordinal);
                        foreach (KeyValuePair<string, string> choice in localField.Choices)
                        {
                            string choiceLabel = NormalizeLabel(choice.Value);
                            codes[choice.Key] = choiceLabel == ""
                                ? CombinedChoice(field, options, "raw:" + uid + ":" + choice.Key, choice.Key + " (" + FormName(response.Source, uid) + ")")
                                : CombinedChoice(field, options, "label:" + choiceLabel, choice.Value);
                        }
                        foreach (string code in Codes(localField, values))
                        {
                            string choice;
                            if (!codes.TryGetValue(code, out choice))
                                choice = CombinedChoice(field, options, "raw:" + uid + ":" + code, code + " (" + FormName(response.Source, uid) + ")");
                            converted.Add(choice);
                        }
                        if (family == "select_multiple combined" && converted.Count > 0)
                            converted = new List<string> { string.Join(" ", converted) };
                    }
                    else converted.AddRange(values);
                    merged[field.Path] = converted;
                }
                response.Values = merged;
            }
            report.Visits = responses.Count;
            report.UnmappedFields = unmapped.Count;
            if (counts) fields.RemoveAll(delegate(Field field) { return !ChoiceField(field); });
            else fields.RemoveAll(delegate(Field field)
            {
                foreach (Response response in responses) if (Values(response, field).Count > 0) return false;
                return true;
            });
            HashSet<string> captions = new HashSet<string>(StringComparer.Ordinal);
            foreach (Field field in fields)
            {
                string label = field.Label;
                int suffix = 2;
                if (!captions.Add(NormalizeQuestion(field.Label)))
                {
                    label += " - " + (field.Type.StartsWith("select_multiple ", StringComparison.Ordinal) ? "اختيار متعدد" : ChoiceField(field) ? "اختيار واحد" : "إجابة مباشرة");
                    field.Label = label;
                    while (!captions.Add(NormalizeQuestion(field.Label))) field.Label = label + " (" + suffix++ + ")";
                }
            }
            report.Table = counts ? Counts(fields, responses) : Beneficiaries(fields, responses);
            return report;
        }

        private static string CombinedChoice(Field field, Dictionary<string, string> keys, string meaning, string label)
        {
            string code;
            if (!keys.TryGetValue(meaning, out code))
            {
                code = "c" + keys.Count; keys.Add(meaning, code);
                field.Choices.Add(new KeyValuePair<string, string>(code, label));
            }
            return code;
        }

        private static bool IsAssessmentQuestion(string path, string label)
        {
            // These are the audited basic-information and vehicle-identity groups in the supplied XLSForms.
            if (path.StartsWith("group_mo5jr41/", StringComparison.Ordinal) ||
                path.StartsWith("group_vv6uu79/", StringComparison.Ordinal) ||
                path.StartsWith("group_vz9zl11/", StringComparison.Ordinal)) return false;
            if (path == "__010" || path == "vist_" || path == "group_na4gu14/vist_no") return false;
            string question = NormalizeLabel(label);
            return question != "رقم الزيارة" && question != "تاريخ الزيارة";
        }

        private static string FormName(DataRow row, string uid)
        {
            string name = Text(row, "Kobo_Form_Name");
            return string.IsNullOrEmpty(name) ? uid : name;
        }

        private static string NormalizeLabel(string value)
        {
            StringBuilder text = new StringBuilder();
            foreach (char character in value.Normalize(NormalizationForm.FormC))
            {
                if (char.GetUnicodeCategory(character) == UnicodeCategory.Format) continue;
                if (char.IsWhiteSpace(character))
                {
                    if (text.Length > 0 && text[text.Length - 1] != ' ') text.Append(' ');
                }
                else text.Append(character);
            }
            return text.ToString().Trim();
        }

        private static string NormalizeQuestion(string value)
        {
            StringBuilder text = new StringBuilder();
            foreach (char character in NormalizeLabel(value ?? ""))
                if (character != '\u0640' && !(character >= '\u064b' && character <= '\u065f') && character != '\u0670') text.Append(character);
            return text.ToString().TrimEnd(' ', '?', '؟', '.', ':');
        }

        private static bool FollowUp(string label)
        {
            return label == "لماذا" || label == "كيف" || label == "ماذا" || label == "السبب" || label == "أخرى" || label == "اذكر السبب";
        }

        private static bool Confirmed(DataRow row)
        {
            string status = Text(row, "Sync_Status");
            return row.Table.Columns.Contains("Kobo_Is_Deleted") && row["Kobo_Is_Deleted"] != DBNull.Value &&
                Convert.ToInt32(row["Kobo_Is_Deleted"], CultureInfo.InvariantCulture) == 0 &&
                (status == "local_inserted" || status == "local_updated") &&
                Text(row, "Project_ID") != "" && Text(row, "Visit_Date") != "";
        }

        private static bool Matches(DataRow row, KoboResultsFilter filter)
        {
            DateTime date;
            if (!DateTime.TryParse(Text(row, "Visit_Date"), CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) return false;
            if (date.Date < filter.From.Date || date.Date > filter.To.Date) return false;
            string stage = Text(row, "Stage");
            if (!string.IsNullOrEmpty(filter.Stage))
            {
                if (filter.Stage == "Monitorings" && !stage.StartsWith("M", StringComparison.Ordinal)) return false;
                if (filter.Stage == "Openings" && stage != "Opening") return false;
                if (filter.Stage == "Closings" && stage != "Closing") return false;
                if (filter.Stage.StartsWith("M", StringComparison.Ordinal) && filter.Stage != "Monitorings" && stage != filter.Stage) return false;
            }
            return EqualFilter(row, "Visit_Kind", filter.Kind) && EqualFilter(row, "MP_Type_ID", filter.ProjectType) &&
                EqualFilter(row, "Category_ID", filter.Category) && EqualFilter(row, "SubCategory_ID", filter.SubCategory) &&
                EqualFilter(row, "Career_ID", filter.Career);
        }

        private static bool EqualFilter(DataRow row, string name, string value)
        {
            return string.IsNullOrEmpty(value) || Text(row, name) == value;
        }

        private static List<Response> Latest(List<Response> responses)
        {
            Dictionary<string, Response> latest = new Dictionary<string, Response>(StringComparer.Ordinal);
            foreach (Response response in responses)
            {
                string key = Text(response.Source, "Project_ID") + ":" + Text(response.Source, "Person_ID");
                Response previous;
                if (!latest.TryGetValue(key, out previous) ||
                    Convert.ToDateTime(response.Source["Visit_Date"]) > Convert.ToDateTime(previous.Source["Visit_Date"]) ||
                    (Convert.ToDateTime(response.Source["Visit_Date"]) == Convert.ToDateTime(previous.Source["Visit_Date"]) &&
                     Convert.ToInt64(response.Source["ID"]) > Convert.ToInt64(previous.Source["ID"]))) latest[key] = response;
            }
            List<Response> selected = new List<Response>();
            foreach (Response response in responses)
                if (latest[Text(response.Source, "Project_ID") + ":" + Text(response.Source, "Person_ID")] == response) selected.Add(response);
            return selected;
        }

        private static List<Field> Fields(string uid)
        {
            List<Field> fields = new List<Field>();
            JObject form = catalog.Value[uid] as JObject;
            if (form == null) return fields;
            foreach (JObject item in (JArray)form["fields"])
            {
                Field field = new Field { Path = (string)item["path"], Label = (string)item["label"], Type = (string)item["type"] };
                field.Repeated = (bool?)item["repeat"] ?? false;
                field.Section = (string)item["section"] ?? "";
                field.Context = (string)item["context"] ?? "";
                foreach (JObject choice in (JArray)item["choices"])
                    field.Choices.Add(new KeyValuePair<string, string>((string)choice["code"], (string)choice["label"]));
                fields.Add(field);
            }
            return fields;
        }

        private static void Flatten(JToken token, string path, Dictionary<string, List<string>> values)
        {
            JObject group = token as JObject;
            if (group != null)
            {
                foreach (JProperty property in group.Properties())
                {
                    string child = path == "" || property.Name.StartsWith(path + "/", StringComparison.Ordinal) ? property.Name : path + "/" + property.Name;
                    Flatten(property.Value, child, values);
                }
                return;
            }
            JArray array = token as JArray;
            if (array != null) { foreach (JToken child in array) Flatten(child, path, values); return; }
            List<string> list;
            if (!values.TryGetValue(path, out list)) { list = new List<string>(); values.Add(path, list); }
            if (token.Type != JTokenType.Null && !string.IsNullOrWhiteSpace(token.ToString())) list.Add(token.ToString());
        }

        private static bool Metadata(string path)
        {
            return path.StartsWith("_", StringComparison.Ordinal) || path.StartsWith("meta/", StringComparison.Ordinal) ||
                path.StartsWith("formhub/", StringComparison.Ordinal) || path == "start" || path == "end" ||
                path == "today" || path == "username" || path == "deviceid";
        }

        private static List<string> Values(Response response, Field field)
        {
            List<string> values;
            return response.Values.TryGetValue(field.Path, out values) ? values : new List<string>();
        }

        private static bool ChoiceField(Field field) { return field.Type.StartsWith("select_one ", StringComparison.Ordinal) || field.Type.StartsWith("select_multiple ", StringComparison.Ordinal); }

        private static HashSet<string> Codes(Field field, List<string> values)
        {
            HashSet<string> codes = new HashSet<string>(StringComparer.Ordinal);
            foreach (string value in values)
                if (field.Type.StartsWith("select_multiple ", StringComparison.Ordinal))
                    foreach (string code in value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)) codes.Add(code);
                else codes.Add(value);
            return codes;
        }

        private static string Display(Field field, List<string> values)
        {
            List<string> display = new List<string>();
            foreach (string value in values)
            {
                if (!ChoiceField(field)) { display.Add(value); continue; }
                List<string> labels = new List<string>();
                foreach (string code in Codes(field, new List<string> { value }))
                {
                    string label = code;
                    foreach (KeyValuePair<string, string> choice in field.Choices) if (choice.Key == code) { label = choice.Value; break; }
                    labels.Add(label);
                }
                display.Add(string.Join("، ", labels));
            }
            return string.Join(" | ", display);
        }

        private static DataTable Beneficiaries(List<Field> fields, List<Response> responses)
        {
            DataTable table = new DataTable();
            AddColumn(table, "Project_ID", "رقم المشروع", typeof(int));
            AddColumn(table, "Beneficiary", "المستفيد / الشركاء", typeof(string));
            AddColumn(table, "SubCategory_Name", "الفئة الفرعية", typeof(string));
            AddColumn(table, "Career_Name", "المهنة", typeof(string));
            AddColumn(table, "Stage", "رقم الزيارة", typeof(string));
            AddColumn(table, "Visit_Date", "تاريخ الزيارة", typeof(DateTime));
            for (int i = 0; i < fields.Count; i++)
            {
                DataColumn column = AddColumn(table, "q_" + i, fields[i].Label, typeof(string));
                column.ExtendedProperties["Path"] = fields[i].SourcePath ?? fields[i].Path;
            }
            foreach (Response response in responses)
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < 6; i++)
                    row[i] = response.Source.Table.Columns.Contains(table.Columns[i].ColumnName) ? response.Source[table.Columns[i].ColumnName] : DBNull.Value;
                for (int i = 0; i < fields.Count; i++) row["q_" + i] = Display(fields[i], Values(response, fields[i]));
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable Counts(List<Field> fields, List<Response> responses)
        {
            List<List<KeyValuePair<string, string>>> options = new List<List<KeyValuePair<string, string>>>();
            int maxChoices = 0;
            foreach (Field field in fields)
            {
                List<KeyValuePair<string, string>> choices = new List<KeyValuePair<string, string>>(field.Choices);
                HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (KeyValuePair<string, string> choice in choices) seen.Add(choice.Key);
                if (ChoiceField(field))
                    foreach (Response response in responses)
                        foreach (string code in Codes(field, Values(response, field)))
                            if (seen.Add(code)) choices.Add(new KeyValuePair<string, string>(code, code));
                options.Add(choices); maxChoices = Math.Max(maxChoices, choices.Count);
            }
            DataTable table = new DataTable();
            AddColumn(table, "Question", "السؤال", typeof(string));
            for (int i = 1; i <= maxChoices; i++)
                AddColumn(table, "Answer" + i, "إجابة " + i, typeof(string));
            for (int i = 1; i <= maxChoices; i++)
                AddColumn(table, "Count" + i, "إجابات " + i, typeof(int));
            AddColumn(table, "Answered", "الإجابات الكلي", typeof(int));
            AddColumn(table, "Unanswered", "بدون إجابة", typeof(int));
            AddColumn(table, "Visits", "إجمالي الزيارات", typeof(int));
            AddColumn(table, "Type", "نوع السؤال", typeof(string));
            foreach (Field field in fields)
            {
                int index = fields.IndexOf(field);
                DataRow row = table.NewRow();
                row["Question"] = field.Label;
                row["Type"] = field.Type.StartsWith("select_multiple ", StringComparison.Ordinal) ? "اختيار متعدد" : ChoiceField(field) ? "اختيار واحد" : "إجابة مباشرة";
                int answered = 0, applicable = 0;
                Dictionary<string, int> frequencies = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (Response response in responses)
                {
                    if (response.Applicable != null && !response.Applicable.Contains(field.Path)) continue;
                    applicable++;
                    List<string> values = Values(response, field);
                    if (values.Count > 0) answered++;
                    if (ChoiceField(field)) foreach (string code in Codes(field, values))
                    {
                        int count; frequencies.TryGetValue(code, out count); frequencies[code] = count + 1;
                    }
                }
                for (int i = 0; i < options[index].Count; i++)
                {
                    KeyValuePair<string, string> choice = options[index][i];
                    row["Answer" + (i + 1)] = choice.Value;
                    int count; frequencies.TryGetValue(choice.Key, out count); row["Count" + (i + 1)] = count;
                }
                row["Answered"] = answered; row["Unanswered"] = applicable - answered; row["Visits"] = applicable;
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataColumn AddColumn(DataTable table, string name, string caption, Type type)
        {
            DataColumn column = table.Columns.Add(name, type); column.Caption = caption; return column;
        }

        private static string Text(DataRow row, string name)
        {
            return row.Table.Columns.Contains(name) ? Convert.ToString(row[name], CultureInfo.InvariantCulture) : "";
        }
    }
}
