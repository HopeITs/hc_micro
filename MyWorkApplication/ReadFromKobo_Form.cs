using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;

namespace MyWorkApplication
{
    public partial class ReadFromKobo_Form: Form
    {
        public ReadFromKobo_Form()
        {
            InitializeComponent();
        }

        KoboFormConfig kobo;
        VisitsNew v;
        private void ReadFromKobo_Form_Load(object sender, EventArgs e)
        {
            kobo = new KoboFormConfig();
            v = new VisitsNew();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try {
                //await SyncFormAsync(formId, token);

                //should take just the forms of my city??
                foreach (var form in KoboFormConfig.KoboForms)
                {
                    await SyncFormAsync(form);
                }

                MessageBox.Show("Sync Completed Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Sync Failed!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
        }

        //public async Task SyncFormAsync(KoboFormConfig cfg)
        //{
        //    string url = $"https://kf.kobotoolbox.org/api/v2/assets/{cfg.FormId}/data/";

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Token", cfg.token);

        //    string json = await client.GetStringAsync(url);
        //    JObject parsed = JObject.Parse(json);
        //    var results = (JArray)parsed["results"];

        //    // Load the mapper for this form
        //    var mapper = KoboFormConfig.FormMappers[cfg.FormName];

        //    foreach (JObject row in results)
        //    {
        //        // map JSON → VisitModel
        //        VisitsNew model = mapper(row);

        //        // save to DB
        //        v.InsertVisit(model);
        //    }
        //}

        public static void LogImportError(string city, string formId, long koboId,
                                  VisitsNew.ValidationResult validation, string rawJson)
        {
            Program.buildConnection();

            string sql = @"
INSERT INTO visitimporterrors(City, FormId, KoboId, ErrorMessages, RawJson)
VALUES (@city, @form, @kid, @err, @raw)";

            using (var cmd = new MySqlCommand(sql, Program.MyConn))
            {
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@form", formId);
                cmd.Parameters.AddWithValue("@kid", koboId);
                cmd.Parameters.AddWithValue("@err", string.Join(" | ", validation.Errors));
                cmd.Parameters.AddWithValue("@raw", rawJson);

                cmd.ExecuteNonQuery();
            }

            Program.MyConn.Close();
        }

        public static long GetLastKoboId(string formId)
        {
            Program.buildConnection();

            string sql = "SELECT LastKoboId FROM syncstatus WHERE FormId=@f";

            using (var cmd = new MySqlCommand(sql, Program.MyConn))
            {
                cmd.Parameters.AddWithValue("@f", formId);
                object result = cmd.ExecuteScalar();
                Program.MyConn.Close();

                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToInt64(result);
            }
        }

        public static void UpdateLastKoboId(string formId, long id)
        {
            Program.buildConnection();

            string sql = @"
INSERT INTO syncstatus (FormId, LastKoboId)
VALUES (@f, @id)
ON DUPLICATE KEY UPDATE LastKoboId=@id";

            using (var cmd = new MySqlCommand(sql, Program.MyConn))
            {
                cmd.Parameters.AddWithValue("@f", formId);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            Program.MyConn.Close();
        }


        public static async Task SyncFormAsync(KoboFormConfig cfg)
        {
            // 1. Fetch JSON from Kobo
            string url = "https://kf.kobotoolbox.org/api/v2/assets/" + cfg.FormId + "/data/";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Token", cfg.token);

                string json = await client.GetStringAsync(url);
                var root = JObject.Parse(json);
                var results = (JArray)root["results"];

                // 2. Get last processed ID for this form
                long lastId = GetLastKoboId(cfg.FormId);

                // 3. Get mapper for this form name
                var mapper = KoboFormConfig.FormMappers[cfg.FormName];

                var visitsHelper = new VisitsNew();
                long maxSeenId = lastId;

                foreach (JObject row in results)
                {
                    long koboId = VisitsNew.ToLong(row["_id"]);
                    if (koboId <= lastId)
                        continue; // already processed in earlier run

                    // duplicates protection
                    if (visitsHelper.Exists(koboId))
                        continue;

                    // 4. Validate
                    var validation = visitsHelper.ValidateSubmission(row);
                    if (!validation.IsValid)
                    {
                        LogImportError(cfg.City, cfg.FormId, koboId, validation, row.ToString());
                        continue;
                    }

                    // 5. Map to VisitsNew model
                    VisitsNew v = mapper(row);

                    // Fill common meta fields
                    v.Start = VisitsNew.ToDate(row["start"]);
                    v.End = VisitsNew.ToDate(row["end"]);
                    v.Today = VisitsNew.ToDate(row["today"]);
                    v.Username = (string)row["username"];
                    v.KoboId = koboId;
                    v.Uuid = (string)row["_uuid"];
                    v.SubmissionTime = VisitsNew.ToDate(row["_submission_time"]);
                    v.Status = (string)row["_status"];
                    v.SubmittedBy = (string)row["_submitted_by"];
                    v.Version = (string)row["__version__"];
                    v.ValidationStatus = row["_validation_status"]?.ToString();
                    v.Notes = row["_notes"]?.ToString();

                    // tags might be array or empty
                    var tagsToken = row["_tags"] as JArray;
                    v.Tags = tagsToken != null ? string.Join(",", tagsToken.Select(t => t.ToString())) : null;

                    // some forms don't have _index
                    v.Index = row["_index"] != null ? (int?)Convert.ToInt32(row["_index"]) : null;

                    // 6. Insert into DB
                    v.InsertVisit(v);

                    if (koboId > maxSeenId)
                        maxSeenId = koboId;
                }

                // 7. Update SyncStatus with highest KoboId seen
                if (maxSeenId > lastId)
                    UpdateLastKoboId(cfg.FormId, maxSeenId);
            }
        }
    }
}
