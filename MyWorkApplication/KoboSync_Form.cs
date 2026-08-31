using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class KoboSync_Form : Form
    {
        private const string KoboServerUrl = "https://kf.kobotoolbox.org";
        private const string KoboToken = "a17ba83c2c73e6a7141e42a5b9553b1bc8c7c13a";
        public KoboSync_Form(MainForm mainForm)
        {
            InitializeComponent();
        }

        private void KoboSync_Form_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyTheme();
                Status_value_label.Text = "جاهز";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Check_Theme()
        {
            ApplyTheme();
        }

        private void Sync_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Sync_backgroundWorker.IsBusy)
                    return;

                if (string.IsNullOrWhiteSpace(KoboServerUrl))
                {
                    MessageBox.Show("رابط كوبو غير مضبوط في البرنامج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(KoboToken))
                {
                    MessageBox.Show("رمز الدخول إلى كوبو غير مضبوط في البرنامج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetSyncState(true);
                Sync_backgroundWorker.RunWorkerAsync(new string[] { KoboServerUrl, KoboToken });
            }
            catch (Exception ex)
            {
                SetSyncState(false);
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sync_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = (string[])e.Argument;
            KoboSync koboSync = new KoboSync();
            e.Result = koboSync.Sync_Raw_Submissions(args[0], args[1]);
        }

        private void Sync_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetSyncState(false);

                if (e.Error != null)
                {
                    Status_value_label.Text = "فشلت المزامنة";
                    MessageBox.Show(e.Error.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                KoboSyncResult result = (KoboSyncResult)e.Result;
                Forms_value_label.Text = result.FormsCount.ToString();
                Submissions_value_label.Text = result.SubmissionsCount.ToString();
                Inserted_value_label.Text = result.InsertedCount.ToString();
                Updated_value_label.Text = result.UpdatedCount.ToString();
                Unchanged_value_label.Text = result.UnchangedCount.ToString();
                Deleted_value_label.Text = result.DeletedCount.ToString();
                Errors_value_label.Text = result.ErrorCount.ToString();

                if (result.ErrorCount > 0)
                {
                    Status_value_label.Text = "تمت المزامنة مع وجود أخطاء";
                    MessageBox.Show("تمت المزامنة مع وجود أخطاء، يرجى مراجعة مسؤول النظام.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Status_value_label.Text = "تمت المزامنة";
                }

                try
                {
                    Log l = new Log();
                    l.Insert_Log("Sync Kobo raw submissions", "kobo_submission", Program.UserName, DateTime.Now);
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSyncState(bool isSyncing)
        {
            Sync_button.Enabled = !isSyncing;
            Status_value_label.Text = isSyncing ? "جاري المزامنة..." : Status_value_label.Text;
            progressBar1.Style = isSyncing ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        }

        private void ApplyTheme()
        {
            if (Settings.Default.theme == "Dark")
            {
                BackColor = NewTheme.light_grey3;
                ApplyThemeToControls(this, NewTheme.light_grey3, NewTheme.light_white, NewTheme.dark_grey2);
                title_label.ForeColor = NewTheme.light_white;
                Sync_button.BackColor = NewTheme.light_grey2;
                Sync_button.ForeColor = NewTheme.dark_grey2;
            }
            else
            {
                BackColor = NewTheme.light_grey1;
                ApplyThemeToControls(this, NewTheme.light_grey1, NewTheme.light_grey4, NewTheme.light_white);
                title_label.ForeColor = NewTheme.light_red;
                Sync_button.BackColor = NewTheme.light_red;
                Sync_button.ForeColor = NewTheme.light_white;
            }
        }

        private void ApplyThemeToControls(Control parent, Color backColor, Color foreColor, Color inputBackColor)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.BackColor = inputBackColor;
                    control.ForeColor = foreColor;
                }
                else if (control is Button)
                {
                    control.BackColor = NewTheme.light_red;
                    control.ForeColor = NewTheme.light_white;
                }
                else
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                }

                if (control.HasChildren)
                    ApplyThemeToControls(control, backColor, foreColor, inputBackColor);
            }
        }
    }
}
