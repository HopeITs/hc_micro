using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace MyWorkApplication.Classes
{
    public static class KoboMonitoringIdentity
    {
        public static DataRow ResolveNamedParticipant(DataTable relationships, string koboName)
        {
            HashSet<int> eligible = new HashSet<int>();
            foreach (DataRow row in relationships.Rows)
            {
                string role = row["PersonType"].ToString().Trim();
                if ((role == "مستفيد" || role == "شريك") && row["Person_ID"] != DBNull.Value && Convert.ToInt32(row["Person_ID"]) > 0)
                    eligible.Add(Convert.ToInt32(row["Person_ID"]));
            }
            // Name comparison disambiguates people; it must not invalidate a sole project participant.
            if (eligible.Count == 1) return ResolveParticipant(relationships);
            string key = NormalizePersonName(koboName);
            if (key.Length == 0) return ResolveParticipant(relationships);
            Dictionary<int, DataRow> matches = new Dictionary<int, DataRow>();
            Dictionary<int, string> matchRoles = new Dictionary<int, string>();
            foreach (DataRow row in relationships.Rows)
            {
                string role = row["PersonType"].ToString().Trim();
                if ((role != "مستفيد" && role != "شريك") || row["Person_ID"] == DBNull.Value) continue;
                int id = Convert.ToInt32(row["Person_ID"]);
                if (id <= 0) continue;
                string first = NamePart(row,"P_FirstName"), last = NamePart(row,"P_LastName"), father = NamePart(row,"P_FatherName");
                if (first.Length == 0 || last.Length == 0) continue;
                bool match = NameIsPresent(key, first + " " + last);
                if (father.Length > 0)
                    match |= NameIsPresent(key, first + " " + father + " " + last) ||
                        NameIsPresent(key, first + " " + last + " ابن/ة " + father) ||
                        NameIsPresent(key, first + " " + last + " ابن " + father) ||
                        NameIsPresent(key, first + " " + last + " بنت " + father);
                if (match)
                {
                    matches[id] = row;
                    if (!matchRoles.ContainsKey(id) || role == "مستفيد") matchRoles[id] = role;
                }
            }
            if (matches.Count == 1)
                foreach (DataRow row in matches.Values) return row;
            if (matches.Count > 1)
            {
                DataRow soleBeneficiary = null;
                foreach (KeyValuePair<int, DataRow> match in matches)
                {
                    string role;
                    if (matchRoles.TryGetValue(match.Key, out role) && role == "مستفيد")
                    {
                        if (soleBeneficiary != null)
                            throw new InvalidOperationException("اسم المستفيد في كوبو يطابق أكثر من مستفيد مرتبط بالمشروع. يلزم اختيار الشخص يدوياً.");
                        soleBeneficiary = match.Value;
                    }
                }
                if (soleBeneficiary != null) return soleBeneficiary;
            }
            if (matches.Count != 1)
                throw new InvalidOperationException(matches.Count == 0
                    ? "تعذر مطابقة اسم المستفيد في كوبو «" + koboName + "» مع شخص مرتبط بهذا المشروع. تحقق من الاسم ورقم المشروع؛ لم يتم اختيار شخص بالتخمين."
                    : "اسم المستفيد في كوبو يطابق أكثر من شخص داخل المشروع. يلزم التحقق من الهوية؛ لم يتم اختيار شخص تلقائياً.");
            throw new InvalidOperationException("No unique named participant.");
        }

        private static bool NameIsPresent(string normalizedSource, string name)
        {
            string candidate = NormalizePersonName(name);
            if (candidate.Length == 0) return false;
            int start = 0;
            while (start <= normalizedSource.Length - candidate.Length)
            {
                int index = normalizedSource.IndexOf(candidate, start, StringComparison.Ordinal);
                if (index < 0) return false;
                int end = index + candidate.Length;
                bool leftBoundary = index == 0 || char.IsWhiteSpace(normalizedSource[index - 1]) ||
                    (index >= 2 && normalizedSource[index - 1] == 'و' && char.IsWhiteSpace(normalizedSource[index - 2]));
                bool rightBoundary = end == normalizedSource.Length || char.IsWhiteSpace(normalizedSource[end]);
                if (leftBoundary && rightBoundary) return true;
                start = index + 1;
            }
            return false;
        }

        private static string NamePart(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? NormalizePersonName(row[column].ToString()) : "";
        }

        public static string NormalizePersonName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            StringBuilder text = new StringBuilder();
            foreach (char ch in value.Normalize(NormalizationForm.FormC))
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(ch);
                // Ignore presentation marks, not spelling differences or word boundaries.
                if (ch == '\u0640' || category == UnicodeCategory.Format) continue;
                if (ch >= '\u064b' && ch <= '\u0652' || ch == '\u0670') continue;
                if (char.IsWhiteSpace(ch))
                {
                    if (text.Length > 0 && text[text.Length-1] != ' ') text.Append(' ');
                }
                else text.Append(char.ToLowerInvariant(ch));
            }
            return text.ToString().Trim();
        }

        public static DataRow ResolveParticipant(DataTable relationships)
        {
            Dictionary<int, DataRow> beneficiaries = new Dictionary<int, DataRow>();
            Dictionary<int, DataRow> partners = new Dictionary<int, DataRow>();
            foreach (DataRow row in relationships.Rows)
            {
                string role = row["PersonType"].ToString().Trim();
                if (row["Person_ID"] == DBNull.Value) continue;
                int id = Convert.ToInt32(row["Person_ID"]);
                if (id <= 0) continue;
                if (role == "مستفيد") beneficiaries[id] = row;
                else if (role == "شريك") partners[id] = row;
            }
            Dictionary<int, DataRow> candidates = beneficiaries.Count > 0 ? beneficiaries : partners;
            if (candidates.Count != 1)
                throw new InvalidOperationException("Project must identify exactly one beneficiary, or exactly one partner when no beneficiary exists. Found " + candidates.Count + ". No person was guessed.");
            foreach (DataRow row in candidates.Values) return row;
            throw new InvalidOperationException("No verified project participant.");
        }

        public static bool RequiresProjectIsolation(DataTable projects, int sourceProject)
        {
            HashSet<int> ids = new HashSet<int>();
            foreach (DataRow row in projects.Rows)
                if (row["MicroProject_ID"] != DBNull.Value) ids.Add(Convert.ToInt32(row["MicroProject_ID"]));
            if (!ids.Contains(sourceProject))
                throw new InvalidOperationException("Person is not linked to the explicit Kobo project.");
            return ids.Count > 1;
        }
    }
}
