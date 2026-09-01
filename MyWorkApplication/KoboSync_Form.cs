using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class KoboSync_Form : Form
    {
        private const int PBM_SETBARCOLOR = 0x0409;
        private const int PBM_SETBKCOLOR = 0x2001;
        private const string KoboServerUrl = "https://kf.kobotoolbox.org";
        private const string KoboToken = "a17ba83c2c73e6a7141e42a5b9553b1bc8c7c13a";
        private bool closeAfterSync = false;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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
                progressBar1.Value = 0;
                LoadExistingVisitsCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingVisitsCount()
        {
            KoboSubmission koboSubmission = new KoboSubmission();
            Existing_value_label.Text = koboSubmission.Count_Active_Submissions_For_Current_Database().ToString();
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
                    if (closeAfterSync)
                    {
                        Close();
                        return;
                    }

                    MessageBox.Show(e.Error.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                KoboSyncResult result = (KoboSyncResult)e.Result;
                Existing_value_label.Text = result.ExistingCount.ToString();
                Submissions_value_label.Text = result.SubmissionsCount.ToString();
                Inserted_value_label.Text = result.InsertedCount.ToString();
                Updated_value_label.Text = result.UpdatedCount.ToString();
                Deleted_value_label.Text = result.DeletedCount.ToString();
                AfterSync_value_label.Text = result.AfterSyncCount.ToString();
                Errors_value_label.Text = result.ErrorCount.ToString();

                if (result.ErrorCount > 0)
                {
                    Status_value_label.Text = "تمت المزامنة مع وجود أخطاء";
                    if (!closeAfterSync)
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

                if (closeAfterSync)
                    Close();
            }
            catch (Exception ex)
            {
                if (closeAfterSync)
                    Close();
                else
                    MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSyncState(bool isSyncing)
        {
            if (isSyncing)
                ActiveControl = null;

            ApplySyncButtonStyle(false);
            Sync_button.Enabled = !isSyncing;
            Status_value_label.Text = isSyncing ? "جاري المزامنة..." : Status_value_label.Text;
            progressBar1.Style = ProgressBarStyle.Continuous;
            ApplyProgressBarColor();

            if (isSyncing)
            {
                progressBar1.Value = 10;
                Sync_progress_timer.Start();
            }
            else
            {
                Sync_progress_timer.Stop();
                progressBar1.Value = 0;
            }
        }

        private void KoboSync_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Sync_backgroundWorker.IsBusy)
            {
                e.Cancel = true;

                DialogResult result = MessageBox.Show(
                    "المزامنة لا تزال قيد التنفيذ.\r\nإذا أغلقت النافذة الآن ستستمر المزامنة في الخلفية حتى تنتهي، ولن تظهر النتيجة في هذه النافذة.\r\n\r\nهل تريد إغلاق النافذة؟",
                    "تنبيه",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                if (result == DialogResult.Yes)
                {
                    closeAfterSync = true;
                    Hide();
                }
            }
        }

        private void Sync_progress_timer_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= 95)
                progressBar1.Value = 10;
            else
                progressBar1.Value += 5;
        }

        private void ApplyTheme()
        {
            if (Settings.Default.theme == "Dark")
            {
                BackColor = NewTheme.light_grey3;
                ApplyThemeToControls(this, NewTheme.light_grey3, NewTheme.light_white, NewTheme.dark_grey2);
                title_label.ForeColor = NewTheme.light_white;
                ApplySectionHeaderStyle(NewTheme.light_grey2, NewTheme.dark_grey2);
                ApplyValueLabelStyle(NewTheme.dark_grey2, NewTheme.light_white);
                ApplySyncButtonStyle(false);
                ApplyProgressBarColor();
            }
            else
            {
                BackColor = NewTheme.light_grey1;
                ApplyThemeToControls(this, NewTheme.light_grey1, NewTheme.light_grey4, NewTheme.light_white);
                title_label.ForeColor = NewTheme.light_red;
                ApplySectionHeaderStyle(NewTheme.light_red, NewTheme.light_white);
                ApplyValueLabelStyle(NewTheme.light_white, NewTheme.light_grey4);
                ApplySyncButtonStyle(false);
                ApplyProgressBarColor();
            }
        }

        private void ApplySectionHeaderStyle(Color backColor, Color foreColor)
        {
            ServerSection_label.BackColor = backColor;
            ServerSection_label.ForeColor = foreColor;
            KoboSection_label.BackColor = backColor;
            KoboSection_label.ForeColor = foreColor;
            AfterSyncSection_label.BackColor = backColor;
            AfterSyncSection_label.ForeColor = foreColor;
        }

        private void ApplyValueLabelStyle(Color backColor, Color foreColor)
        {
            Status_value_label.BackColor = backColor;
            Status_value_label.ForeColor = foreColor;
            Existing_value_label.BackColor = backColor;
            Existing_value_label.ForeColor = foreColor;
            Submissions_value_label.BackColor = backColor;
            Submissions_value_label.ForeColor = foreColor;
            Inserted_value_label.BackColor = backColor;
            Inserted_value_label.ForeColor = foreColor;
            Updated_value_label.BackColor = backColor;
            Updated_value_label.ForeColor = foreColor;
            Deleted_value_label.BackColor = backColor;
            Deleted_value_label.ForeColor = foreColor;
            AfterSync_value_label.BackColor = backColor;
            AfterSync_value_label.ForeColor = foreColor;
            Errors_value_label.BackColor = backColor;
            Errors_value_label.ForeColor = foreColor;
        }

        private void Sync_button_MouseEnter(object sender, EventArgs e)
        {
            ApplySyncButtonStyle(true);
        }

        private void Sync_button_MouseLeave(object sender, EventArgs e)
        {
            ApplySyncButtonStyle(false);
        }

        private void ApplySyncButtonStyle(bool mouseOver)
        {
            Image image = null;
            Color foreColor = new Color();

            if (Settings.Default.theme == "Light")
            {
                image = mouseOver ? Resources.BtnBackground_L : Resources.BtnBackground_D;
                foreColor = mouseOver ? NewTheme.dark_grey2 : NewTheme.light_grey11;
            }
            else
            {
                image = mouseOver ? Resources.BtnBackground_D : Resources.BtnBackground_L;
                foreColor = mouseOver ? NewTheme.light_grey11 : NewTheme.dark_grey2;
            }

            Sync_button.BackColor = Color.Transparent;
            Sync_button.BackgroundImage = image;
            Sync_button.ForeColor = foreColor;
            Sync_button.NotifyDefault(false);
        }

        private void ApplyProgressBarColor()
        {
            if (!progressBar1.IsHandleCreated)
                progressBar1.CreateControl();

            progressBorder_panel.BackColor = NewTheme.light_white;
            SetWindowTheme(progressBar1.Handle, "", "");
            progressBar1.ForeColor = NewTheme.light_red;
            progressBar1.BackColor = NewTheme.light_white;
            SendMessage(progressBar1.Handle, PBM_SETBARCOLOR, IntPtr.Zero,
                (IntPtr)ColorTranslator.ToWin32(NewTheme.light_red));
            SendMessage(progressBar1.Handle, PBM_SETBKCOLOR, IntPtr.Zero,
                (IntPtr)ColorTranslator.ToWin32(NewTheme.light_white));
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

        private void mainLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
