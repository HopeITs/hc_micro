using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class KoboSubmissions_Form : Form
    {
        private bool loading;

        public KoboSubmissions_Form()
        {
            InitializeComponent();
        }

        private void KoboSubmissions_Form_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            status_comboBox.SelectedIndex = 0;
            RefreshSubmissions();
        }

        public void Check_Theme()
        {
            ApplyTheme();
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            RefreshSubmissions();
        }

        private void RefreshSubmissions()
        {
            try
            {
                loading = true;
                int selectedId = SelectedSubmissionId();
                DataTable submissions = KoboSubmissionViewer.GetSubmissions(search_textBox.Text, StatusCode());
                submissions.Columns.Add("Sync_Status_Display", typeof(string));
                submissions.Columns.Add("Local_Link_Display", typeof(string));
                foreach (DataRow row in submissions.Rows)
                {
                    row["Sync_Status_Display"] = StatusLabel(row["Sync_Status"].ToString(), Convert.ToInt32(row["Kobo_Is_Deleted"]));
                    row["Local_Link_Display"] = LocalLinkLabel(Convert.ToString(row["Local_Table_Name"]), Convert.ToString(row["Local_Record_ID"]));
                }
                submissions_dataGridView.DataSource = submissions;
                ConfigureSubmissionColumns();
                submissionsSection_label.Text = "الإرسالات (" + submissions.Rows.Count.ToString("#,##0") + ")";
                if (submissions_dataGridView.Rows.Count > 0)
                {
                    int selectedRow = FindSubmissionRow(selectedId);
                    int firstVisibleColumn = 0;
                    while (firstVisibleColumn < submissions_dataGridView.Columns.Count && !submissions_dataGridView.Columns[firstVisibleColumn].Visible)
                        firstVisibleColumn++;
                    if (firstVisibleColumn < submissions_dataGridView.Columns.Count)
                        submissions_dataGridView.CurrentCell = submissions_dataGridView.Rows[Math.Max(0, selectedRow)].Cells[firstVisibleColumn];
                }
                else
                {
                    answers_dataGridView.DataSource = null;
                    AnswersCount_label.Text = "0";
                    Details_label.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loading = false;
            }
            LoadSelectedAnswers();
        }

        private void ConfigureSubmissionColumns()
        {
            SetHeader("Kobo_Submission_ID", "رقم الإرسال", 105);
            SetHeader("Kobo_Form_Name", "النموذج", 220);
            SetHeader("Kobo_Submission_Time", "تاريخ الإرسال", 145);
            SetHeader("Kobo_Submitted_By", "مرسل النموذج", 130);
            SetHeader("Sync_Status_Display", "حالة المزامنة", 115);
            SetHeader("Local_Link_Display", "السجل المرتبط", 150);
            SetHeader("Error_Message", "رسالة الخطأ", 240);
            HideColumn("ID"); HideColumn("Kobo_Form_ID"); HideColumn("Kobo_Is_Deleted");
            HideColumn("Sync_Status"); HideColumn("Local_Table_Name"); HideColumn("Local_Record_ID");
            submissions_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (submissions_dataGridView.Columns.Contains("Error_Message"))
                submissions_dataGridView.Columns["Error_Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void SetHeader(string name, string text, int width)
        {
            if (!submissions_dataGridView.Columns.Contains(name)) return;
            DataGridViewColumn column = submissions_dataGridView.Columns[name];
            column.HeaderText = text;
            column.MinimumWidth = Math.Min(width, 70);
            column.Width = width;
            column.Visible = true;
        }

        private void HideColumn(string name)
        {
            if (submissions_dataGridView.Columns.Contains(name)) submissions_dataGridView.Columns[name].Visible = false;
        }

        private int SelectedSubmissionId()
        {
            if (submissions_dataGridView.CurrentRow == null || submissions_dataGridView.CurrentRow.Cells["ID"].Value == null)
                return 0;
            int id;
            return int.TryParse(submissions_dataGridView.CurrentRow.Cells["ID"].Value.ToString(), out id) ? id : 0;
        }

        private int FindSubmissionRow(int id)
        {
            if (id <= 0) return 0;
            foreach (DataGridViewRow row in submissions_dataGridView.Rows)
                if (Convert.ToInt32(row.Cells["ID"].Value) == id) return row.Index;
            return 0;
        }

        private string StatusCode()
        {
            if (status_comboBox.SelectedIndex <= 0) return "all";
            return status_comboBox.SelectedIndex == 1 ? "errors" :
                status_comboBox.SelectedIndex == 2 ? "imported" :
                status_comboBox.SelectedIndex == 3 ? "unlinked" : "deleted";
        }

        private string StatusLabel(string status, int deleted)
        {
            if (deleted == 1 || status == "deleted" || status == "local_deleted") return "محذوف من كوبو";
            if (status == "local_error") return "يحتاج مراجعة";
            if (status == "local_inserted") return "تم استيراده";
            if (status == "local_updated") return "تم تحديثه";
            if (status == "review_deferred") return "مؤجل للمراجعة";
            if (status == "review_replaced") return "تم الاستبدال بعد المراجعة";
            if (status == "raw_imported" || status == "raw_updated") return "تمت مزامنته من كوبو";
            return status;
        }

        private string LocalLinkLabel(string table, string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return "غير مرتبط";
            if (table == "mevisit") return "زيارة متابعة رقم " + recordId;
            if (table == "visit") return "زيارة افتتاحية رقم " + recordId;
            if (table == "kobo_monitoring_record") return "سجل كوبو فقط";
            return table + " رقم " + recordId;
        }

        private void Submissions_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (loading) return;
            LoadSelectedAnswers();
        }

        private void LoadSelectedAnswers()
        {
            int id = SelectedSubmissionId();
            if (id <= 0) return;
            try
            {
                DataRow submission = KoboSubmissionViewer.GetSubmission(id);
                if (submission == null) return;
                DataTable answers = KoboSubmissionViewer.ParseAnswers(submission["Kobo_Form_ID"].ToString(), submission["Raw_JSON"].ToString());
                answers_dataGridView.DataSource = answers;
                ConfigureAnswerColumns();
                AnswersCount_label.Text = answers.Rows.Count.ToString("#,##0");
                if (submissions_dataGridView.CurrentRow != null)
                {
                    string form = Convert.ToString(submissions_dataGridView.CurrentRow.Cells["Kobo_Form_Name"].Value);
                    string submissionNumber = Convert.ToString(submissions_dataGridView.CurrentRow.Cells["Kobo_Submission_ID"].Value);
                    Details_label.Text = form + " | " + submissionNumber;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureAnswerColumns()
        {
            if (answers_dataGridView.Columns.Contains("Type")) answers_dataGridView.Columns["Type"].HeaderText = "النوع";
            if (answers_dataGridView.Columns.Contains("Question")) answers_dataGridView.Columns["Question"].HeaderText = "السؤال";
            if (answers_dataGridView.Columns.Contains("Response")) answers_dataGridView.Columns["Response"].HeaderText = "الإجابة";
            if (answers_dataGridView.Columns.Contains("Type")) answers_dataGridView.Columns["Type"].Width = 95;
            if (answers_dataGridView.Columns.Contains("Question")) answers_dataGridView.Columns["Question"].Width = 430;
            if (answers_dataGridView.Columns.Contains("Response")) answers_dataGridView.Columns["Response"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            answers_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void ApplyTheme()
        {
            Color background = Settings.Default.theme == "Dark" ? NewTheme.light_grey3 : NewTheme.light_grey1;
            Color foreground = Settings.Default.theme == "Dark" ? NewTheme.light_white : NewTheme.light_grey4;
            Color inputBackground = Settings.Default.theme == "Dark" ? NewTheme.dark_grey2 : NewTheme.light_white;
            BackColor = background;
            ApplyThemeToControls(this, background, foreground, inputBackground);
            title_label.ForeColor = Settings.Default.theme == "Dark" ? NewTheme.light_white : NewTheme.light_red;
            submissionsSection_label.BackColor = Settings.Default.theme == "Dark" ? NewTheme.light_grey2 : NewTheme.light_red;
            answersSection_label.BackColor = submissionsSection_label.BackColor;
            submissionsSection_label.ForeColor = Settings.Default.theme == "Dark" ? NewTheme.dark_grey2 : NewTheme.light_white;
            answersSection_label.ForeColor = submissionsSection_label.ForeColor;
            StyleGrid(submissions_dataGridView, background, foreground);
            StyleGrid(answers_dataGridView, background, foreground);
        }

        private void ApplyThemeToControls(Control parent, Color background, Color foreground, Color inputBackground)
        {
            foreach (Control control in parent.Controls)
            {
                control.ForeColor = foreground;
                if (control is TextBoxBase || control is ComboBox)
                    control.BackColor = inputBackground;
                else if (!(control is DataGridView))
                    control.BackColor = background;
                ApplyThemeToControls(control, background, foreground, inputBackground);
            }
        }

        private void StyleGrid(DataGridView grid, Color background, Color foreground)
        {
            grid.BackgroundColor = background;
            grid.GridColor = Settings.Default.theme == "Dark" ? NewTheme.light_grey3 : NewTheme.light_grey2;
            grid.DefaultCellStyle.BackColor = Settings.Default.theme == "Dark" ? NewTheme.dark_grey2 : Color.White;
            grid.DefaultCellStyle.ForeColor = foreground;
            grid.DefaultCellStyle.SelectionBackColor = Settings.Default.theme == "Dark" ? NewTheme.light_grey3 : NewTheme.light_grey2;
            grid.DefaultCellStyle.SelectionForeColor = foreground;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Settings.Default.theme == "Dark" ? NewTheme.light_grey3 : Color.FromArgb(232, 232, 232);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Settings.Default.theme == "Dark" ? NewTheme.light_grey2 : NewTheme.light_red;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Settings.Default.theme == "Dark" ? NewTheme.dark_grey2 : Color.White;
            grid.EnableHeadersVisualStyles = false;
        }
    }
}
