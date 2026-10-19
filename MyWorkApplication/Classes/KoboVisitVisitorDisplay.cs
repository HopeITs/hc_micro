using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    public static class KoboVisitVisitorDisplay
    {
        public static bool Bind(int visitId, params ComboBox[] controls)
        {
            return BindTable(visitId, "visit", controls);
        }

        public static bool BindMonitoring(int visitId, TextBox notes, params ComboBox[] controls)
        {
            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            using (MySqlCommand command = new MySqlCommand(@"SELECT Raw_JSON FROM kobo_submission
WHERE Local_Table_Name = 'mevisit' AND Local_Record_ID = @id
ORDER BY Kobo_Submission_Time DESC, ID DESC LIMIT 1", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", visitId);
                object raw;
                try { raw = command.ExecuteScalar(); }
                catch (MySqlException ex) { if (ex.Number == 1146) return false; throw; }
                if (raw == null || raw == DBNull.Value) return false;
                notes.Text = KoboMonitoringImporter.ParseSubmission(raw.ToString()).ToString();
                notes.ReadOnly = true;
            }
            BindTable(visitId, "mevisit", controls);
            return true;
        }

        private static bool BindTable(int visitId, string table, ComboBox[] controls)
        {
            List<string> names = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(Program.ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(@"SELECT v.`Display_Name`
FROM `kobo_visit_visitor` v
WHERE v.`Submission_ID` = (
  SELECT s.`ID` FROM `kobo_submission` s
  WHERE s.`Local_Table_Name` = @Table AND s.`Local_Record_ID` = @Visit_ID
    AND (s.`Kobo_Is_Deleted` = 0 OR @Table = 'mevisit')
    AND EXISTS (SELECT 1 FROM `kobo_visit_visitor` names WHERE names.`Submission_ID` = s.`ID`)
  ORDER BY s.`Kobo_Submission_Time` DESC, s.`ID` DESC LIMIT 1
)
ORDER BY v.`Visitor_Order`", connection))
                {
                    command.Parameters.AddWithValue("@Visit_ID", visitId);
                    command.Parameters.AddWithValue("@Table", table);
                    try
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                names.Add(reader["Display_Name"].ToString());
                        }
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1146)
                            return false;
                        throw;
                    }
                }
            }
            if (names.Count == 0)
                return false;

            BindNames(names, controls);
            return true;
        }

        private static void BindNames(List<string> names, ComboBox[] controls)
        {
            string[] slots = GetDisplaySlots(names, controls.Length);
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].DataSource = null;
                controls[i].Items.Clear();
                controls[i].Text = "";
                if (!string.IsNullOrEmpty(slots[i]))
                {
                    controls[i].Items.Add(slots[i]);
                    controls[i].SelectedIndex = 0;
                }
                controls[i].Enabled = false;
            }
        }

        public static string[] GetDisplaySlots(List<string> names, int count)
        {
            string[] slots = new string[count];
            if (count == 0)
                return slots;
            for (int i = 0; i < names.Count; i++)
            {
                int slot = Math.Min(i, count - 1);
                slots[slot] = string.IsNullOrEmpty(slots[slot]) ? names[i] : slots[slot] + "، " + names[i];
            }
            return slots;
        }
    }
}
