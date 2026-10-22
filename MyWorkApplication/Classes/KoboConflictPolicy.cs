using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyWorkApplication.Classes
{
    public static class KoboConflictPolicy
    {
        public static string Hash(string raw)
        {
            using (SHA256 hash = SHA256.Create())
                return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(raw ?? ""))).Replace("-", "").ToLowerInvariant();
        }

        public static void ValidateDecision(string decision, string reason)
        {
            if (decision != "replace" && decision != "keep_both" && decision != "ignore" && decision != "defer")
                throw new InvalidOperationException("Unknown conflict decision.");
            if (decision != "defer" && string.IsNullOrWhiteSpace(reason))
                throw new InvalidOperationException("A reason is required for this decision.");
        }

        public static DataTable Compare(string existing, string incoming)
        {
            SortedDictionary<string, string> left = new SortedDictionary<string, string>();
            SortedDictionary<string, string> right = new SortedDictionary<string, string>();
            Flatten(KoboMonitoringImporter.ParseSubmission(existing), "", left);
            Flatten(KoboMonitoringImporter.ParseSubmission(incoming), "", right);
            SortedSet<string> keys = new SortedSet<string>(left.Keys);
            keys.UnionWith(right.Keys);
            DataTable table = new DataTable();
            table.Columns.Add("Field"); table.Columns.Add("Existing"); table.Columns.Add("Incoming");
            table.Columns.Add("Changed", typeof(bool));
            table.Columns.Add("Question");
            foreach (string key in keys)
            {
                string a, b;
                bool hasA = left.TryGetValue(key, out a), hasB = right.TryGetValue(key, out b);
                table.Rows.Add(key, hasA ? a : "[missing]", hasB ? b : "[missing]", hasA != hasB || a != b,key);
            }
            return table;
        }

        private static void Flatten(JToken token, string path, SortedDictionary<string, string> values)
        {
            JObject obj = token as JObject;
            if (obj != null && obj.HasValues)
            {
                foreach (JProperty property in obj.Properties())
                    Flatten(property.Value, path + "/" + property.Name.Replace("~", "~0").Replace("/", "~1"), values);
            }
            else values[path] = token.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
