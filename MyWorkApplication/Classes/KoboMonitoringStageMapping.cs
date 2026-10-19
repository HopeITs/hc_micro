using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class KoboMonitoringStageMapping
    {
        private DataTable forms;

        public void Load()
        {
            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            using (MySqlCommand command = new MySqlCommand(@"SELECT s.*
FROM kobo_monitoring_stage_mapping s
JOIN kobo_form_mapping f ON BINARY f.Kobo_Form_UID = BINARY s.Kobo_Form_UID
WHERE f.LocalDatabase = DATABASE() AND f.Is_Active = 1", connection))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
            {
                forms = new DataTable();
                adapter.Fill(forms);
            }
        }

        public int GetStage(JObject submission, string formUid)
        {
            if (forms == null)
                throw new InvalidOperationException("Load monitoring stage mappings first.");
            foreach (DataRow row in forms.Rows)
            {
                if (row["Kobo_Form_UID"].ToString() != formUid) continue;
                int? fixedStage = row["Fixed_Stage"] == DBNull.Value
                    ? (int?)null : Convert.ToInt32(row["Fixed_Stage"]);
                return ResolveStage(submission, row["Stage_Field"].ToString(), fixedStage);
            }
            throw new InvalidOperationException("No verified monitoring stage mapping for Kobo form: " + formUid);
        }

        public static int ResolveStage(JObject submission, string field, int? fixedStage)
        {
            if (submission == null) throw new ArgumentNullException("submission");
            if (fixedStage.HasValue)
            {
                if (fixedStage.Value < 1 || fixedStage.Value > 4 || !string.IsNullOrWhiteSpace(field))
                    throw new InvalidOperationException("A fixed stage must be M1-M4 with no stage field.");
                return fixedStage.Value;
            }
            if (string.IsNullOrWhiteSpace(field))
                throw new InvalidOperationException("This monitoring form needs an explicit visit stage before import.");
            string value = KoboVisitorMapping.GetValue(submission, field).Trim().ToUpperInvariant();
            for (int stage = 1; stage <= 4; stage++)
            {
                if (value == "M" + stage || value == "M&E_" + stage)
                    return stage;
            }
            throw new InvalidOperationException("Unknown or missing Kobo monitoring stage: " + value);
        }
    }
}
