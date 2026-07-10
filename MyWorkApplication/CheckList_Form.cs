using DocumentFormat.OpenXml.ExtendedProperties; 
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;   
using System.Data; 
using System.Drawing;
using System.Drawing.Printing;  
using System.Threading;
using System.Windows.Forms;  
using DataTable = System.Data.DataTable;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Word; // add COM reference
using WordApp = Microsoft.Office.Interop.Word.Application;
using MyWorkApplication.Visit_Forms;

namespace MyWorkApplication
{
    public partial class CheckList_Form : Form
    {
        public CheckList_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = -1;
            this.Person_ID = -1;
            MyDataGridViewInitializationMethod();
        }
        public CheckList_Form(int MicroProject_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            MyDataGridViewInitializationMethod();
        }

        MainForm mainForm;
        CheckList cl;
        MicroProject mp;
        Log l;
        Select s;
        DataSet ds; DataSet ds_eval;
        int MicroProject_ID;
        int Person_ID;
        bool Update_Mode, user_mode;
        System.Data.DataTable checklist_member_dt;
        System.Data.DataTable beneficiaries_dt;

        private void CheckList_Form_Load(object sender, EventArgs e)
        {
            try
            {
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Check List";

                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.ApplicationNew_ToNight(this);
                else
                    newTheme.ApplicationNew_ToLight(this);

                cl = new CheckList();
                l = new Log();
                s = new Classes.Select();
                mp = new MicroProject();
                ds = new DataSet();
                ds_eval = new DataSet();
                user_mode = false;

                //Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                keyperson_dataGridView.AutoGenerateColumns
                    = team_dataGridView.AutoGenerateColumns
                    = commity_dataGridView.AutoGenerateColumns = false;

                Bind_Gridview_ComboBox();

                Stop_ComboBox_Scrolling();
                Stop_Highlighting_Combobox();

                Update_Mode = false;

                if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox_Leave(sender, e);
                }

                user_mode = true;

                CheckUserPermission();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        Save_button.Visible = Delete_button.Visible = Print_button.Visible 
                            = true;
                        return true;
                    }
                case 2: //Data
                    {
                        payments_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible 
                            = true;
                        return true;
                    }
                case 3: //Financial 
                case 7: //Lawful
                    {
                        Visits_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible 
                            = false;
                        return false;
                    }
                case 4: //Guest
                case 9: //Media
                    {
                        payments_toolStripMenuItem.Visible =
                            Visits_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible 
                           = false;
                        return false;
                    }
            }
            return false;
        }

        #region datagridview with comboxBoxe methods
        private void MyDataGridViewInitializationMethod()
        {
            keyperson_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(keyperson_dataGridView_EditingControlShowing);
            team_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(team_dataGridView_EditingControlShowing);
            commity_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(commity_dataGridView_EditingControlShowing);
        }

        private void team_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(team_dataGridView_Control_KeyPress);
        }
        private void commity_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(commity_dataGridView_Control_KeyPress);
        }
        private void keyperson_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(keyperson_dataGridView_Control_KeyPress);
        }
       
        private void team_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) 
                && !char.IsControl(e.KeyChar) 
                && team_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void commity_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) 
                && !char.IsControl(e.KeyChar) 
                && commity_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void keyperson_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) 
                && !char.IsControl(e.KeyChar) 
                && keyperson_dataGridView.CurrentCellAddress.X == 3)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region combobox functions
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            // Draw the background.
            e.DrawBackground();
            SolidBrush brush = new SolidBrush(Color.FromArgb(189, 189, 189));
            if (e.Index >= 0)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                ComboBox comboBox = (ComboBox)sender;
                string text = comboBox.GetItemText(comboBox.Items[e.Index]);

                // Choose text color based on theme
                Brush textBrush = (Properties.Settings.Default.theme == "Light") ? Brushes.Black : Brushes.LightGray;

                // Set up RTL text alignment
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Far, // Right-aligned
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.DirectionRightToLeft
                };

                // Draw the RTL text
                e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds, sf);
            }
            e.DrawFocusRectangle();
        }

        private void comboBox_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!string.IsNullOrWhiteSpace(cb.Text))
            {
                int resultIndex = -1;
                resultIndex = cb.FindStringExact(cb.Text);
                if (resultIndex == -1)
                {
                    MessageBox.Show("انتبه! لا يوجد مثل هذا الخيار، احرص أن تختار قيمة موجودة", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cb.Text = "";
                    cb.Focus();
                }
            }
        }

        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void Stop_ComboBox_Scrolling()
        {
            //////////////////// handle auto scrolling for all comboBoxes //////////////////////
            Beneficiary_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            HVisit_Visitor1_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            HVisit_Visitor2_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            ProTest_User_ID_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            PVisit_Visitor1_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            PVisit_Visitor2_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            RejectMsgSender_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            ///////////////////////////////////////////////////////////////////////////////////////
        }

        private void Stop_Highlighting_Combobox()
        {
            Beneficiary_comboBox.SelectionLength = 0; Beneficiary_comboBox.SelectionStart = Beneficiary_comboBox.Text.Length;
            HVisit_Visitor1_comboBox.SelectionLength = 0; HVisit_Visitor1_comboBox.SelectionStart = HVisit_Visitor1_comboBox.Text.Length;
            HVisit_Visitor2_comboBox.SelectionLength = 0; HVisit_Visitor2_comboBox.SelectionStart = HVisit_Visitor2_comboBox.Text.Length;
            ProTest_User_ID_comboBox.SelectionLength = 0; ProTest_User_ID_comboBox.SelectionStart = ProTest_User_ID_comboBox.Text.Length;
            PVisit_Visitor1_comboBox.SelectionLength = 0; PVisit_Visitor1_comboBox.SelectionStart = PVisit_Visitor1_comboBox.Text.Length;
            PVisit_Visitor2_comboBox.SelectionLength = 0; PVisit_Visitor2_comboBox.SelectionStart = PVisit_Visitor2_comboBox.Text.Length;
            RejectMsgSender_comboBox.SelectionLength = 0; RejectMsgSender_comboBox.SelectionStart = RejectMsgSender_comboBox.Text.Length;
        }
        #endregion

        private void Bind_Gridview_ComboBox()
        {
            user_mode = false;

            checklist_member_dt = cl.Select_Members("", ""); 
            DataTable dt_k = checklist_member_dt.Select("Type='k'").CopyToDataTable();
            DataTable dt_t = checklist_member_dt.Select("Type='t'").CopyToDataTable();
            DataTable dt_c = checklist_member_dt.Select("Type='c'").CopyToDataTable();

            User u = new User();
            DataTable dtusers = u.Select_Users("", "", "and IsVisitor=1");

            K_Name.DataSource = dt_k;
            K_Name.DisplayMember = "Name";
            K_Name.ValueMember = "ID";

            T_Name.DataSource = dt_t;
            T_Name.DisplayMember = "Name";
            T_Name.ValueMember = "ID";

            C_Name.DataSource = dt_c;
            C_Name.DisplayMember = "Name";
            C_Name.ValueMember = "ID";
            
            //simple comboBox
            System.Data.DataTable dt_v = checklist_member_dt.Select("Type='t'").CopyToDataTable();

            DataTable dth1   = dt_v.Copy();
            DataTable dth2   = dt_v.Copy();
            DataTable dtp1   = dt_v.Copy();
            DataTable dtp2   = dt_v.Copy();
            DataTable dt1111 = dt_v.Copy();
            HVisit_Visitor1_comboBox.DataSource = dth1;
            HVisit_Visitor1_comboBox.DisplayMember = "Name";
            HVisit_Visitor1_comboBox.ValueMember = "ID";
            HVisit_Visitor1_comboBox.SelectedIndex = -1;

            HVisit_Visitor2_comboBox.DataSource = dth2;
            HVisit_Visitor2_comboBox.DisplayMember = "Name";
            HVisit_Visitor2_comboBox.ValueMember = "ID";
            HVisit_Visitor2_comboBox.SelectedIndex = -1;

            PVisit_Visitor1_comboBox.DataSource = dtp1;
            PVisit_Visitor1_comboBox.DisplayMember = "Name";
            PVisit_Visitor1_comboBox.ValueMember = "ID";
            PVisit_Visitor1_comboBox.SelectedIndex = -1;

            PVisit_Visitor2_comboBox.DataSource = dtp2;
            PVisit_Visitor2_comboBox.DisplayMember = "Name";
            PVisit_Visitor2_comboBox.ValueMember = "ID";
            PVisit_Visitor2_comboBox.SelectedIndex = -1;

            ProTest_User_ID_comboBox.DataSource = dt1111;
            ProTest_User_ID_comboBox.DisplayMember = "Name";
            ProTest_User_ID_comboBox.ValueMember = "ID";
            ProTest_User_ID_comboBox.SelectedIndex = -1;

            RejectMsgSender_comboBox.DataSource = dtusers;
            RejectMsgSender_comboBox.DisplayMember = "Username";
            RejectMsgSender_comboBox.ValueMember = "ID";
            RejectMsgSender_comboBox.SelectedIndex = -1;
            user_mode = true;
        }

        private void MicroProject_ID_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                    if (MicroProject_ID_textBox.Text != "")
                    {
                        MicroProject_ID = Convert.ToInt32(MicroProject_ID_textBox.Text);
                        fill_beneficiaries_comboBox(MicroProject_ID_textBox.Text);
                        //Fill_checklist(MicroProject_ID_textBox.Text, "");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void fill_beneficiaries_comboBox(string MP_ID)
        { 
            beneficiaries_dt = s.select_Beneficiaries_of_project(MP_ID);

            Beneficiary_comboBox.DisplayMember = "Name";
            Beneficiary_comboBox.ValueMember = "ID";
            Beneficiary_comboBox.DataSource = beneficiaries_dt;
            Beneficiary_comboBox.SelectedIndex = 0;
             
            if (beneficiaries_dt.Rows.Count > 1) 
                partners_button.Visible = true;
            else partners_button.Visible = false;
        }
        private void Beneficiary_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Beneficiary_comboBox.SelectedIndex != -1)
                {
                    Clear_Boxes(false);
                    Person_ID = -1;
                    int.TryParse(Beneficiary_comboBox.SelectedValue.ToString(), out Person_ID);
                    Fill_checklist(Person_ID); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //private void Person_Name_textBox_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (user_mode)
        //            if (Person_Name_textBox.Text != "")
        //            {
        //                Fill_checklist("", Person_Name_textBox.Text);
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        public void Fill_checklist(int P_ID)
        {
            user_mode = false; 
            ds = cl.Get_NewChecklist(P_ID.ToString(),"" ,true);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Career_textBox.Text = ds.Tables[0].Rows[0]["المهنة"].ToString();
                    SubType_textBox.Text = ds.Tables[0].Rows[0]["SubType"].ToString();
                    ExpYears_label.Text = ds.Tables[0].Rows[0]["عدد سنين الخبرة"].ToString();

                    ////HasHomeVisit
                    var hasHomeVisit = ds.Tables[0].Rows[0]["هل تمت زيارة منزل"].ToString();
                    if (hasHomeVisit == "تمت")
                    {
                        HasHVisit_checkBox.Checked = true;
                        HVisitDate_tableLayoutPanel.Enabled = true;

                        if (ds.Tables[0].Rows[0]["تاريخ زيارة المنزل"] != DBNull.Value)
                            HVisitDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ زيارة المنزل"].ToString());

                        //visitors
                        if (ds.Tables[0].Rows[0]["HVisit_Visitor1_ID"] == null || ds.Tables[0].Rows[0]["HVisit_Visitor1_ID"] == DBNull.Value) 
                            HVisit_Visitor1_comboBox.SelectedIndex = -1; 
                        else HVisit_Visitor1_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["HVisit_Visitor1_ID"].ToString()); 
                        if (ds.Tables[0].Rows[0]["HVisit_Visitor2_ID"] == null || ds.Tables[0].Rows[0]["HVisit_Visitor2_ID"] == DBNull.Value) 
                            HVisit_Visitor2_comboBox.SelectedIndex = -1;
                        else HVisit_Visitor2_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["HVisit_Visitor2_ID"].ToString());

                        HVisitTime_textBox.Text = ds.Tables[0].Rows[0]["ساعة زيارة المنزل"].ToString();
                        HVisitNote_textBox.Text = ds.Tables[0].Rows[0]["ملاحظات زيارة المنزل"].ToString();
                    }
                    else
                    {
                        HasHVisit_checkBox.Checked = false;
                        HVisitDate_tableLayoutPanel.Enabled = false;
                    }

                    ////HasProjectVisit
                    var hasProjectVisit = ds.Tables[0].Rows[0]["هل تمت زيارة المحل"].ToString();
                    if (hasProjectVisit == "تمت")
                    {
                        HasPVisit_checkBox.Checked = true;
                        PVisitDate_tableLayoutPanel.Enabled = true;

                        if (ds.Tables[0].Rows[0]["تاريخ زيارة المحل"] != DBNull.Value)
                            PVisitDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ زيارة المحل"].ToString());

                        //visitors
                        if (ds.Tables[0].Rows[0]["PVisit_Visitor1_ID"] == null || ds.Tables[0].Rows[0]["PVisit_Visitor1_ID"] == DBNull.Value) 
                            PVisit_Visitor1_comboBox.SelectedIndex = -1; 
                        else PVisit_Visitor1_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["PVisit_Visitor1_ID"].ToString()); 
                        if (ds.Tables[0].Rows[0]["PVisit_Visitor2_ID"] == null || ds.Tables[0].Rows[0]["PVisit_Visitor2_ID"] == DBNull.Value) 
                            PVisit_Visitor2_comboBox.SelectedIndex = -1; 
                        else PVisit_Visitor2_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["PVisit_Visitor2_ID"].ToString());

                        PVisitTime_textBox.Text = ds.Tables[0].Rows[0]["ساعة زيارة المحل"].ToString();
                        PVisitNote_textBox.Text = ds.Tables[0].Rows[0]["ملاحظات زيارة المحل"].ToString();
                    }
                    else
                    {
                        HasPVisit_checkBox.Checked = false;
                        PVisitDate_tableLayoutPanel.Enabled = false;
                    }

                    ////VisitsReport
                    VisitsReport_textBox.Text = ds.Tables[0].Rows[0]["تقرير الزيارات"].ToString();

                    ////NeedProTest
                    var needProTest = ds.Tables[0].Rows[0]["هل يحتاج لفحص مهني"].ToString();
                    NeedProTestYes_radioButton.Checked = needProTest == "بحاجة";
                    NeedProTestNo_radioButton.Checked = needProTest == "ليس بحاجة";
                    NeedProTestNote_textBox.Text = ds.Tables[0].Rows[0]["سبب الفحص"].ToString();

                    ////HasProTest
                    var hasProTest = ds.Tables[0].Rows[0]["هل تم الفحص المهني"].ToString();
                    if (hasProTest == "نعم")
                    {
                        HasProTestYes_radioButton.Checked = true;
                        HasProfTest_tableLayoutPanel.Enabled = true;

                        if (ds.Tables[0].Rows[0]["تاريخ الفحص المهني"] != DBNull.Value)
                            ProTest_Date_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ الفحص المهني"].ToString());
                        
                        //
                        if (ds.Tables[0].Rows[0]["ProTest_User_ID"] == null || ds.Tables[0].Rows[0]["ProTest_User_ID"] == DBNull.Value) 
                            ProTest_User_ID_comboBox.SelectedIndex = -1; 
                        else ProTest_User_ID_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["ProTest_User_ID"].ToString());

                        ProTest_Note_textBox.Text = ds.Tables[0].Rows[0]["ملاحظات الفحص المهني"].ToString();
                        
                        //
                        var ProTest_Result = (string)ds.Tables[0].Rows[0]["نتيجة الفحص المهني"]; 
                        if (ProTest_Result == "1") 
                            ProTest_ResultYes_radioButton.Checked = true; 
                        else if (ProTest_Result == "0") ProTest_ResultNo_radioButton.Checked = true; 
                        else ProTest_ResultNo_radioButton.Checked = ProTest_ResultYes_radioButton.Checked = false; 
                    }
                    else
                    {
                        HasProTestNo_radioButton.Checked = true;
                        HasProfTest_tableLayoutPanel.Enabled = false;
                    }

                    ////HasProTrain
                    var hasProTrain = ds.Tables[0].Rows[0]["هل لديه تدريب مهني"].ToString();
                    if (hasProTrain == "نعم")
                    {
                        HasProTrainYes_radioButton.Checked = true;
                        HasProTrain_tableLayoutPanel.Enabled = true;

                        if (ds.Tables[0].Rows[0]["تاريخ بدء التدريب /مهني"] != DBNull.Value)
                            ProTrain_BDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ بدء التدريب /مهني"].ToString());

                        if (ds.Tables[0].Rows[0]["تاريخ انتهاء التدريب /مهني"] != DBNull.Value)
                            ProTrain_EDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ انتهاء التدريب /مهني"].ToString());

                        ProTrain_Trainer_textBox.Text = ds.Tables[0].Rows[0]["المدرّب /مهني"].ToString();
                        ProTrain_Place_textBox.Text = ds.Tables[0].Rows[0]["مكان التدريب /مهني"].ToString();
                        ProTrain_Note_textBox.Text = ds.Tables[0].Rows[0]["ملاحظات التدريب /مهني"].ToString();
                        ProTrain_Result_textBox.Text = ds.Tables[0].Rows[0]["نتيجة التدريب /مهني"].ToString();
                    }
                    else
                    {
                        HasProTrainNo_radioButton.Checked = true;
                        HasProTrain_tableLayoutPanel.Enabled = false;
                    }

                    ////HasMngTrain
                    var hasMngTrain = ds.Tables[0].Rows[0]["هل لديه تدريب إداري"].ToString();
                    if (hasMngTrain == "نعم")
                    {
                        HasMngTrainYes_radioButton.Checked = true;
                        HasMngTrain_tableLayoutPanel.Enabled = true;

                        if (ds.Tables[0].Rows[0]["تاريخ التدريب /إداري"] != DBNull.Value)
                            MngTrain_Date_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ التدريب /إداري"].ToString());

                        MngTrain_Trainer_textBox.Text = ds.Tables[0].Rows[0]["المدرّب /إداري"].ToString();
                        MngTrain_Place_textBox.Text = ds.Tables[0].Rows[0]["مكان التدريب /إداري"].ToString();
                        MngTrain_Result_textBox.Text = ds.Tables[0].Rows[0]["نتيجة التدريب /إداري"].ToString();
                    }
                    else
                    {
                        HasMngTrainNo_radioButton.Checked = true;
                        HasMngTrain_tableLayoutPanel.Enabled = false;
                    }

                    ////team
                    if (ds.Tables[0].Rows[0]["تاريخ اجتماع الفريق"] == DBNull.Value)
                        TeamDate_checkBox.Checked = true;
                    else
                    {
                        TeamDate_checkBox.Checked = false;
                        TeamDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ اجتماع الفريق"].ToString());
                    }

                    ////commity
                    if (ds.Tables[0].Rows[0]["تاريخ اجتماع اللجنة"] == DBNull.Value)
                        CommityDate_checkBox.Checked = true;
                    else
                    {
                        CommityDate_checkBox.Checked = false;
                        CommityDate_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ اجتماع اللجنة"].ToString());
                    }

                    Notes_textBox.Text = ds.Tables[0].Rows[0]["ملاحظات عامة"].ToString();
                    State_Reason_textBox.Text = ds.Tables[0].Rows[0]["State_Reason"].ToString();

                    ////RejectMsg
                    var rejectMsg = ds.Tables[0].Rows[0]["رسالة الرفض"].ToString();
                    if (rejectMsg == "تم الارسال")
                    {
                        RejectMsg_checkBox.Checked = true;
                        RejectMsg_tableLayoutPanel.Enabled = true;
                        RejectMsg_checkBox.Text = "تم الإرسال";

                        if (ds.Tables[0].Rows[0]["تاريخ رسالة الرفض"] != DBNull.Value)
                            RejectMsg_Date_dateTimePicker.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["تاريخ رسالة الرفض"].ToString());

                        //sender
                        if (ds.Tables[0].Rows[0]["RejectMsgSender_ID"] == null || ds.Tables[0].Rows[0]["RejectMsgSender_ID"] == DBNull.Value) 
                            RejectMsgSender_comboBox.SelectedIndex = -1; 
                        else RejectMsgSender_comboBox.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["RejectMsgSender_ID"].ToString());
                    }
                    else
                    {
                        RejectMsg_checkBox.Checked = false;
                        RejectMsg_tableLayoutPanel.Enabled = false;
                        RejectMsg_checkBox.Text = "لم يتم الارسال";
                    }

                    //totalMarks needEvaluation
                    GreyLable0_label.Text = "";
                    GreyLable0_label.Text = ds.Tables[0].Rows[0]["TotalMarks"].ToString();
                     
                    Update_Mode = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    keyperson_dataGridView.Rows.Clear();
                    team_dataGridView.Rows.Clear();
                    commity_dataGridView.Rows.Clear();

                    int keyperson_index, team_index, visit_index, commity_index;
                    keyperson_index = team_index = visit_index = commity_index = 0;

                    if (ds.Tables[1].Rows[0]["ID"] != DBNull.Value)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var opinion_text = "";
                            int opinion = ds.Tables[1].Rows[i].Field<int>("Opinion");
                            if (opinion == 1) opinion_text = "مع";
                            else if (opinion == 0) opinion_text = "ضد";
                            else if (opinion == -1) opinion_text = "N/A";

                            if (ds.Tables[1].Rows[i]["Location"].ToString() == "k") //keyperson
                            {
                                keyperson_dataGridView.Rows.Add();
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Checklist_ID"].Value = ds.Tables[1].Rows[i].Field<int>("ID");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_ID"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");

                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Name"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");
 
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Notes"].Value = ds.Tables[1].Rows[i].Field<string>("Notes");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_NameNotes"].Value = ds.Tables[1].Rows[i].Field<string>("NameNotes");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_RoleNotes"].Value = ds.Tables[1].Rows[i].Field<string>("RoleNotes");

                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Date"].Value = ds.Tables[1].Rows[i].Field<DateTime>("created_at").ToString("yyyy-MM-dd hh:mm:ss");

                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_InDataBase"].Value = "1";

                                keyperson_index++;
                            }
                            else if (ds.Tables[1].Rows[i]["Location"].ToString() == "t") //team
                            {
                                team_dataGridView.Rows.Add();
                                team_dataGridView.Rows[team_index].Cells["T_Checklist_ID"].Value = ds.Tables[1].Rows[i].Field<int>("ID");
                                team_dataGridView.Rows[team_index].Cells["T_ID"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");

                                team_dataGridView.Rows[team_index].Cells["T_Name"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");

                                team_dataGridView.Rows[team_index].Cells["T_Position"].Value = ds.Tables[1].Rows[i].Field<string>("Position").ToString();
                                team_dataGridView.Rows[team_index].Cells["T_Notes"].Value = ds.Tables[1].Rows[i].Field<string>("Notes");

                                team_dataGridView.Rows[team_index].Cells["T_Date"].Value = ds.Tables[1].Rows[i].Field<DateTime>("created_at").ToString("yyyy-MM-dd hh:mm:ss");
                                team_dataGridView.Rows[team_index].Cells["T_Opinion"].Value = opinion_text;
                                if (opinion_text == "مع")
                                {
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.BackColor = Color.FromArgb(192, 226, 202);
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.ForeColor = Color.Black;
                                }
                                else if (opinion_text == "ضد")
                                {
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.BackColor = Color.FromArgb(251, 201, 196);
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.ForeColor = Color.Black;
                                }

                                team_dataGridView.Rows[team_index].Cells["T_InDataBase"].Value = "1";

                                team_index++;
                            } 
                            else if (ds.Tables[1].Rows[i]["Location"].ToString() == "c") //commity
                            {
                                commity_dataGridView.Rows.Add();
                                commity_dataGridView.Rows[commity_index].Cells["C_Checklist_ID"].Value = ds.Tables[1].Rows[i].Field<int>("ID");
                                commity_dataGridView.Rows[commity_index].Cells["C_ID"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");

                                commity_dataGridView.Rows[commity_index].Cells["C_Name"].Value = ds.Tables[1].Rows[i].Field<int>("Member_ID");

                                commity_dataGridView.Rows[commity_index].Cells["C_Position"].Value = ds.Tables[1].Rows[i].Field<string>("Position").ToString();
                                commity_dataGridView.Rows[commity_index].Cells["C_Notes"].Value = ds.Tables[1].Rows[i].Field<string>("Notes");

                                commity_dataGridView.Rows[commity_index].Cells["C_Date"].Value = ds.Tables[1].Rows[i].Field<DateTime>("created_at").ToString("yyyy-MM-dd hh:mm:ss");
                                commity_dataGridView.Rows[commity_index].Cells["C_Opinion"].Value = opinion_text;
                                if (opinion_text == "مع")
                                {
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.BackColor = Color.FromArgb(192, 226, 202);
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.ForeColor = Color.Black;
                                }
                                else if (opinion_text == "ضد")
                                {
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.BackColor = Color.FromArgb(251, 201, 196);
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.ForeColor = Color.Black;
                                }

                                commity_dataGridView.Rows[commity_index].Cells["C_InDataBase"].Value = "1";
                                commity_index++;
                            }
                        }
                    }

                    keyperson_dataGridView.ClearSelection();
                    team_dataGridView.ClearSelection();
                    commity_dataGridView.ClearSelection();

                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    NeedEvaluation_dataGridView.DataSource = null;
                    NeedEvaluation_dataGridView.Rows.Clear();
                    NeedEvaluation_dataGridView.Columns.Clear();
                    NeedEvaluation_dataGridView.DataSource = ds.Tables[2];

                    var dgC1 = NeedEvaluation_dataGridView.Columns["P_ID"];
                    dgC1.Visible = false; 

                    NeedEvaluation_dataGridView.Columns["المؤشر"].DefaultCellStyle.Alignment =
                    NeedEvaluation_dataGridView.Columns["العلامة"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    NeedEvaluation_dataGridView.ClearSelection();
                }
                  
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                mainForm.MP_ID_label.Text = MicroProject_ID.ToString();                
            } 
            user_mode = true;
        }

        private void Clear_Boxes(bool with_ids)
        {
            if (with_ids)
            {
                Person_ID = MicroProject_ID = -1;
                Beneficiary_comboBox.SelectedIndex = -1;
                MicroProject_ID_textBox.Text = "";
            }

            Update_Mode = false;

            Career_textBox.Text =
            SubType_textBox.Text =
            State_Reason_textBox.Text = "";

            //HVisit
            HasHVisit_checkBox.Checked = false;
            HVisitDate_tableLayoutPanel.Enabled = false;
            //time
            HVisitDate_dateTimePicker.Value = DateTime.Today;
            HVisitTime_textBox.Text = "";
            //visitors
            HVisit_Visitor1_comboBox.SelectedIndex = -1;
            HVisit_Visitor2_comboBox.SelectedIndex = -1;
            //note
            HVisitNote_textBox.Text = "";

            //PVisit
            HasPVisit_checkBox.Checked = false;
            PVisitDate_tableLayoutPanel.Enabled = false;
            //time
            PVisitDate_dateTimePicker.Value = DateTime.Today;
            PVisitTime_textBox.Text = "";
            //visitors
            PVisit_Visitor1_comboBox.SelectedIndex = -1;
            PVisit_Visitor2_comboBox.SelectedIndex = -1;
            //note
            PVisitNote_textBox.Text = "";

            ////visitsReport
            VisitsReport_textBox.Text = "";

            //need pro test
            NeedProTestYes_radioButton.Checked = NeedProTestNo_radioButton.Checked = false;
            NeedProTestNote_textBox.Text = "";

            //pro test
            HasProTestYes_radioButton.Checked = HasProTestNo_radioButton.Checked = false;
            ProTest_Date_dateTimePicker.Value = DateTime.Today;
            ProTest_Note_textBox.Text = "";
            ProTest_User_ID_comboBox.SelectedIndex = -1;
            ProTest_ResultYes_radioButton.Checked = ProTest_ResultNo_radioButton.Checked = false;

            //pro train
            HasProTrainYes_radioButton.Checked = HasProTrainNo_radioButton.Checked = false;
            ProTrain_BDate_dateTimePicker.Value = ProTrain_EDate_dateTimePicker.Value = DateTime.Today;
            ProTrain_Trainer_textBox.Text = ProTrain_Place_textBox.Text 
                = ProTrain_Note_textBox.Text = ProTrain_Result_textBox.Text = "";

            //mng train
            HasMngTrainYes_radioButton.Checked = HasMngTrainNo_radioButton.Checked = false;
            MngTrain_Date_dateTimePicker.Value = DateTime.Today;
            MngTrain_Trainer_textBox.Text = MngTrain_Place_textBox.Text
                = MngTrain_Result_textBox.Text = "";

            ////team
            TeamDate_checkBox.Checked = true;
            TeamDate_dateTimePicker.Value = DateTime.Today;
            ////commity 
            CommityDate_checkBox.Checked = true;
            CommityDate_dateTimePicker.Value = DateTime.Today;

            //Notes
            Notes_textBox.Text = "";

            ////hasRejectMsg 
            RejectMsg_checkBox.Checked = false;
            RejectMsg_tableLayoutPanel.Enabled = false;
            RejectMsg_checkBox.Text = "لم يتم الارسال";
            
            //sender
            RejectMsgSender_comboBox.SelectedIndex = -1;
            
            //tables
            keyperson_dataGridView.Rows.Clear();
            team_dataGridView.Rows.Clear(); 
            commity_dataGridView.Rows.Clear();
        }

        #region key persons table
        private void keyperson_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == keyperson_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;


                //Check if click is on specific column 
                if (e.ColumnIndex == keyperson_dataGridView.Columns["K_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (keyperson_dataGridView["K_InDataBase", e.RowIndex].Value == null)
                            {
                                keyperson_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(keyperson_dataGridView.Rows[e.RowIndex].Cells["K_Checklist_ID"].Value.ToString());
                                var Name = keyperson_dataGridView.Rows[e.RowIndex].Cells["K_Name"].Value.ToString();

                                cl.Delete_Checklist_Members(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID
                                    , "CheckList", Program.UserName, DateTime.Now);
                                keyperson_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void keyperson_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //keyperson_dataGridView.EndEdit();
        }
        private void keyperson_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                keyperson_dataGridView.EndEdit();
                // No Need to fill position Just in KeyPerson

                //if (e.ColumnIndex == 2) //Name
                //{
                //    if (keyperson_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                //    {
                //        string id = keyperson_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                //        var row = checklist_member_dt.Select("ID = " + id);
                //        string position = row[0].Field<string>(3);
                //        keyperson_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                //    }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void keyperson_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == keyperson_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == keyperson_dataGridView.Columns["K_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
                    location.Offset(e.CellBounds.Location);
                    e.Graphics.DrawImage(image, location);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void keyperson_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region team table
        private void team_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == team_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == team_dataGridView.Columns["T_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (team_dataGridView["T_InDataBase", e.RowIndex].Value == null)
                            {
                                team_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(team_dataGridView.Rows[e.RowIndex].Cells["T_Checklist_ID"].Value.ToString());
                                var Name = team_dataGridView.Rows[e.RowIndex].Cells["T_Name"].Value.ToString();

                                cl.Delete_Checklist_Members(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID,
                                        "CheckList", Program.UserName, DateTime.Now);
                                team_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void team_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2) //Name
                {
                    if (team_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        string id = team_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var row = checklist_member_dt.Select("ID = " + id);
                        string position = row[0].Field<string>(3);
                        team_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void team_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == team_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == team_dataGridView.Columns["T_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
                    location.Offset(e.CellBounds.Location);
                    e.Graphics.DrawImage(image, location);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region commity table
        private void commity_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == commity_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == commity_dataGridView.Columns["C_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (commity_dataGridView["C_InDataBase", e.RowIndex].Value == null)
                            {
                                commity_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(commity_dataGridView.Rows[e.RowIndex].Cells["C_Checklist_ID"].Value.ToString());
                                var Name = commity_dataGridView.Rows[e.RowIndex].Cells["C_Name"].Value.ToString();

                                cl.Delete_Checklist_Members(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID,
                                        "CheckList", Program.UserName, DateTime.Now);
                                commity_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void commity_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2) //Name
                {
                    if (commity_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        string id = commity_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var row = checklist_member_dt.Select("ID = " + id);
                        string position = row[0].Field<string>(3);
                        commity_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void commity_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == commity_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == commity_dataGridView.Columns["C_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
                    location.Offset(e.CellBounds.Location);
                    e.Graphics.DrawImage(image, location);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void TeamDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TeamDate_checkBox.Checked) TeamDate_dateTimePicker.Visible = false;
            else TeamDate_dateTimePicker.Visible = true;
        } 
        private void CommityDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CommityDate_checkBox.Checked) CommityDate_dateTimePicker.Visible = false;
            else CommityDate_dateTimePicker.Visible = true;
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف جميع بيانات هذه الصفحة؟", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (MicroProject_ID == -1 || Person_ID == -1)
                        throw new InvalidOperationException(
                            "لا يمكن حذف المشروع! الرجاء اختيار مشروع من قائمة البحث أولاً ثم المحاولة مرة ثانية.");

                    //mp.Update_Micro_VisitDetails(MicroProject_ID, "", "", "", "", "", "", "");
                    cl.Delete_NewChecklist(Person_ID);
                    cl.Delete_All_Checklist_Members(Person_ID);

                    Clear_Boxes(true);

                    Update_Mode = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (!check_data())
                    return;
                
                #region visit
                string Team_Date, Commity_Date
                    , HVisit_Date, HVisit_Time, HVisit_Notes, isHVisited
                    , PVisit_Date, PVisit_Time, PVisit_Notes, isPVisited
                    , VisitsReport, Notes;
                Team_Date = Commity_Date 
                    = HVisit_Date = HVisit_Time = HVisit_Notes = isHVisited 
                    = PVisit_Date = PVisit_Time = PVisit_Notes = isPVisited 
                    = VisitsReport = Notes = "";
                int H_visitor_id1, H_visitor_id2, P_visitor_id1, P_visitor_id2;
                H_visitor_id1 = H_visitor_id2 = P_visitor_id1 = P_visitor_id2 = -1;

                //HVisit Details//
                if (HasHVisit_checkBox.Checked)
                {
                    isHVisited = "1";
                    if (HVisitDate_dateTimePicker.Visible)
                        HVisit_Date = HVisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    HVisit_Time     = HVisitTime_textBox.Text;
                    if (HVisit_Visitor1_comboBox.SelectedIndex != -1)
                        H_visitor_id1 = int.Parse(HVisit_Visitor1_comboBox.SelectedValue.ToString());
                    if (HVisit_Visitor2_comboBox.SelectedIndex != -1)
                        H_visitor_id2 = int.Parse(HVisit_Visitor2_comboBox.SelectedValue.ToString());
                    HVisit_Notes = HVisitNote_textBox.Text; 
                }
                else isHVisited = "0";

                //PVisit Details//
                if (HasPVisit_checkBox.Checked)
                {
                    isPVisited = "1";
                    if (PVisitDate_dateTimePicker.Visible)
                       PVisit_Date = PVisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    PVisit_Time = PVisitTime_textBox.Text;
                    if (PVisit_Visitor1_comboBox.SelectedIndex != -1)
                        P_visitor_id1 = int.Parse(PVisit_Visitor1_comboBox.SelectedValue.ToString());
                    if (PVisit_Visitor2_comboBox.SelectedIndex != -1)
                        P_visitor_id2 = int.Parse(PVisit_Visitor2_comboBox.SelectedValue.ToString());
                    PVisit_Notes = PVisitNote_textBox.Text;
                }
                else isPVisited = "0";

                //visitReports
                VisitsReport = VisitsReport_textBox.Text;
                #endregion

                #region experience  
                string NeedProTest, NeedProTestNote
                     , HasProTest  , ProTest_Date, ProTest_Note, ProTest_Result
                     , HasProTrain , ProTrain_Trainer, ProTrain_Place
                                   , ProTrain_BDate, ProTrain_EDate, ProTrain_Note, ProTrain_Result
                     , HasMngTrain , MngTrain_Trainer, MngTrain_Place, MngTrain_Date, MngTrain_Result;

                NeedProTest = NeedProTestNote
                     = HasProTest  = ProTest_Date = ProTest_Note = ProTest_Result
                     = HasProTrain = ProTrain_Trainer= ProTrain_Place
                                   = ProTrain_BDate= ProTrain_EDate= ProTrain_Note= ProTrain_Result
                     = HasMngTrain = MngTrain_Trainer = MngTrain_Place = MngTrain_Date = MngTrain_Result = "";

                int ProTest_User_ID;
                ProTest_User_ID = -1;

                //NeedProTest//
                if (NeedProTestYes_radioButton.Checked) NeedProTest = "1";  
                else if (NeedProTestNo_radioButton.Checked) NeedProTest = "0";  
                else NeedProTest = "-1";
                NeedProTestNote = NeedProTestNote_textBox.Text;

                //ProfTest Details//
                if (HasProTestYes_radioButton.Checked)
                {
                    HasProTest = "1";
                    ProTest_Date = ProTest_Date_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    ProTest_Note = ProTest_Note_textBox.Text;
                    if (ProTest_User_ID_comboBox.SelectedIndex != -1)
                        ProTest_User_ID = int.Parse(ProTest_User_ID_comboBox.SelectedValue.ToString());
                    
                    if (ProTest_ResultYes_radioButton.Checked)
                        ProTest_Result = "1";   //بحاجة
                    else if (ProTest_ResultNo_radioButton.Checked) 
                        ProTest_Result = "0";   //جيد - ليس بحاجة
                    else ProTest_Result = "-1"; // N/A
                }
                else if (HasProTestNo_radioButton.Checked) HasProTest = "0";
                else HasProTest = "-1";

                //ProTrain Details//
                if (HasProTrainYes_radioButton.Checked)
                {
                    HasProTrain = "1";
                    ProTrain_Trainer = ProTrain_Trainer_textBox.Text;
                    ProTrain_Place   = ProTrain_Place_textBox.Text; 
                    ProTrain_BDate   = ProTrain_BDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    ProTrain_EDate   = ProTrain_EDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    ProTrain_Note    = ProTrain_Note_textBox.Text; 
                }
                else if (HasProTrainNo_radioButton.Checked) HasProTrain = "0";
                else HasProTrain = "-1";

                //MngTrain Details//
                if (HasMngTrainYes_radioButton.Checked)
                {
                    HasMngTrain = "1";
                    MngTrain_Trainer = MngTrain_Trainer_textBox.Text;
                    MngTrain_Place   = MngTrain_Place_textBox.Text;
                    MngTrain_Date    = MngTrain_Date_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    MngTrain_Result  = MngTrain_Result_textBox.Text;
                }
                else if (HasMngTrainNo_radioButton.Checked) HasMngTrain = "0";
                else HasMngTrain = "-1";
                #endregion

                //team & commity dates
                if (TeamDate_dateTimePicker.Visible)
                {
                    Team_Date = TeamDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                }
                if (CommityDate_dateTimePicker.Visible)
                {
                    Commity_Date = CommityDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                }
                 
                //checklist Notes
                Notes = Notes_textBox.Text;

                #region reject msg  
                string HasRejectMsg, RejectMsg_Date;
                HasRejectMsg = RejectMsg_Date = "";

                int RejectMsgSender_id;
                RejectMsgSender_id = -1;

                //reject msg//
                if (RejectMsg_checkBox.Checked)
                {
                    HasRejectMsg = "1";
                    if(RejectMsg_Date_dateTimePicker.Visible)
                        RejectMsg_Date = RejectMsg_Date_dateTimePicker.Value.ToString("yyyy/MM/dd");
                     
                    if (RejectMsgSender_comboBox.SelectedIndex != -1)
                        RejectMsgSender_id = int.Parse(RejectMsgSender_comboBox.SelectedValue.ToString());
                }
                else HasRejectMsg = "0";
                #endregion

                if(Update_Mode)
                {
                    //update checklist
                    cl.Update_NewChecklist(Person_ID
                    , HVisit_Date, HVisit_Time, HVisit_Notes, isHVisited
                    , PVisit_Date, PVisit_Time, PVisit_Notes, isPVisited
                    , VisitsReport, Team_Date, Commity_Date, Notes
            , H_visitor_id1, H_visitor_id2, P_visitor_id1, P_visitor_id2
            , NeedProTest, NeedProTestNote
                     , HasProTest, ProTest_Date, ProTest_Note, ProTest_Result
                     , HasProTrain, ProTrain_Trainer, ProTrain_Place
                     , ProTrain_BDate, ProTrain_EDate, ProTrain_Note, ProTrain_Result
                     , HasMngTrain, MngTrain_Trainer, MngTrain_Place, MngTrain_Date, MngTrain_Result
            , ProTest_User_ID
            , HasRejectMsg, RejectMsg_Date
            , RejectMsgSender_id);

                    l.Insert_Log("Update checklist of:" + MicroProject_ID + " : " + Beneficiary_comboBox.Text, "CheckList", Program.UserName, DateTime.Now);

                }
                else
                {
                    //insert checklist
                    cl.Insert_NewChecklist(  Person_ID
                    ,   HVisit_Date,   HVisit_Time,   HVisit_Notes,   isHVisited
                    ,   PVisit_Date,   PVisit_Time,   PVisit_Notes,   isPVisited
                    ,   VisitsReport,   Team_Date,   Commity_Date,   Notes
            ,   H_visitor_id1,   H_visitor_id2,   P_visitor_id1,   P_visitor_id2
            ,   NeedProTest,   NeedProTestNote
                     ,   HasProTest,   ProTest_Date,   ProTest_Note,   ProTest_Result
                     ,   HasProTrain,   ProTrain_Trainer,   ProTrain_Place
                     ,   ProTrain_BDate,   ProTrain_EDate,   ProTrain_Note,   ProTrain_Result
                     ,   HasMngTrain,   MngTrain_Trainer,   MngTrain_Place,   MngTrain_Date,   MngTrain_Result
            ,   ProTest_User_ID
            ,   HasRejectMsg,   RejectMsg_Date
            ,   RejectMsgSender_id);

                    l.Insert_Log("Insert checklist of:" + MicroProject_ID + " : " + Beneficiary_comboBox.Text, "CheckList", Program.UserName, DateTime.Now);

                    Update_Mode = true;
                }

                Insert_Update_ChecklistDetails();

                MessageBox.Show("تم حفظ كافة المعلومات بنجاح", "Save", MessageBoxButtons.OK, MessageBoxIcon.None);
                Beneficiary_comboBox_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private bool check_data()
        {
            if (string.IsNullOrWhiteSpace(Beneficiary_comboBox.Text)
                || string.IsNullOrWhiteSpace(MicroProject_ID_textBox.Text))
            {
                MessageBox.Show("يُرجى التأكد من اختيار المشروع واسم المستفيد.", "Error");
                return false;
            }

            if(HasHVisit_checkBox.Checked && HVisit_Visitor1_comboBox.Text == "")
            { 
                MessageBox.Show("يُرجى اختيار زائر واحد على الأقل لزيارة المنزل.", "Error");
                return false;
            }
            if (HasPVisit_checkBox.Checked && PVisit_Visitor1_comboBox.Text == "")
            {
                MessageBox.Show("يُرجى اختيار زائر واحد على الأقل لزيارة المحل.", "Error");
                return false;
            }

            if (HasProTestYes_radioButton.Checked)
            {
                if (HasProfTest_tableLayoutPanel.Enabled &&
                        (ProTest_Note_textBox.Text == ""
                        || ProTest_User_ID_comboBox.SelectedIndex == -1))
                {
                    MessageBox.Show("يُرجى استكمال معلومات الفحص المهني.", "Error");
                    return false;
                }
            }

            if (HasProTrainYes_radioButton.Checked)
            {
                if (HasProTrain_tableLayoutPanel.Enabled &&
                        (ProTrain_Trainer_textBox.Text == ""
                        || ProTrain_Place_textBox.Text == ""
                        || ProTrain_Note_textBox.Text == ""))
                {
                    MessageBox.Show("يُرجى استكمال معلومات التدريب المهني.", "Error");
                    return false;
                }
            }

            if (HasMngTrainYes_radioButton.Checked)
            {
                if (HasMngTrain_tableLayoutPanel.Enabled &&
                    (MngTrain_Trainer_textBox.Text == ""
                    || MngTrain_Place_textBox.Text == ""))
                {
                    MessageBox.Show("يُرجى استكمال معلومات التدريب الإداري.", "Error");
                    return false;
                }
            }


            if (RejectMsg_checkBox.Checked &&
                (RejectMsgSender_comboBox.SelectedIndex == -1) )
            {
                MessageBox.Show("يُرجى اختيار اسم مُرسِل رسالة الرفض.", "Error");
                return false;
            }

            return true;
        }

        private void Insert_Update_ChecklistDetails()
        {
            string notes, opinion_text;
            int opinion, K_ID;
            string name_notes, role_notes;

            // KeyPerson //
            notes = opinion_text = "";
            name_notes = role_notes = ""; //Just for KeyPerson
            opinion = -1;
            for (var i = 0; i < keyperson_dataGridView.RowCount - 1; i++)
            {
                if (keyperson_dataGridView.Rows[i].Cells["K_Name"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_Name"].Value.ToString() == null)
                    K_ID = -1;
                else K_ID = Convert.ToInt32(keyperson_dataGridView.Rows[i].Cells["K_Name"].Value.ToString());

                if (keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value.ToString();

                //Just for KeyPerson
                if (keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value.ToString() == null)
                    name_notes = "";
                else name_notes = keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value.ToString();

                if (keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value.ToString() == null)
                    role_notes = "";
                else role_notes = keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value.ToString();
                 
                ////set ID columns// 
                keyperson_dataGridView.Rows[i].Cells["K_ID"].Value = K_ID;
                if (K_ID != -1)
                {
                    if (keyperson_dataGridView.Rows[i].Cells["K_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert_Checklist_Members(Person_ID, K_ID, opinion, notes, name_notes, role_notes, "k");

                        keyperson_dataGridView.Rows[i].Cells["K_InDataBase"].Value = "1";
                        keyperson_dataGridView.Rows[i].Cells["K_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (k)-" + name_notes + ":" + role_notes + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(keyperson_dataGridView.Rows[i].Cells["K_Checklist_ID"].Value);
                        cl.Update_Checklist_Members(RowID, K_ID, opinion, notes, name_notes, role_notes);
                        l.Insert_Log("Update (k)-" + name_notes + ":" + role_notes + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }

            // Team //
            notes = opinion_text = "";
            opinion = -1;
            int T_ID;
            for (var i = 0; i < team_dataGridView.RowCount - 1; i++)
            {
                if (team_dataGridView.Rows[i].Cells["T_Name"].Value == null || team_dataGridView.Rows[i].Cells["T_Name"].Value.ToString() == null)
                    T_ID = -1;
                else T_ID = Convert.ToInt32(team_dataGridView.Rows[i].Cells["T_Name"].Value.ToString());

                if (team_dataGridView.Rows[i].Cells["T_Notes"].Value == null || team_dataGridView.Rows[i].Cells["T_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = team_dataGridView.Rows[i].Cells["T_Notes"].Value.ToString();

                if (team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString() == "مع")
                { opinion = 1; }
                else if (team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString() == "ضد")
                    opinion = 0;
                else opinion = -1;
                opinion_text = team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString();

                //set ID columns// 
                team_dataGridView.Rows[i].Cells["T_ID"].Value = T_ID;
                if (T_ID != -1)
                {
                    if (team_dataGridView.Rows[i].Cells["T_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert_Checklist_Members(Person_ID, T_ID, opinion, notes, "", "", "t");

                        team_dataGridView.Rows[i].Cells["T_InDataBase"].Value = "1";
                        team_dataGridView.Rows[i].Cells["T_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (t)-" + T_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text, "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(team_dataGridView.Rows[i].Cells["T_Checklist_ID"].Value);
                        cl.Update_Checklist_Members(RowID, T_ID, opinion, notes, "", "");

                        l.Insert_Log("Update (t)-" + T_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text, "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }
             
            // Commity //
            notes = opinion_text = "";
            opinion = -1;
            int C_ID;

            for (var i = 0; i < commity_dataGridView.RowCount - 1; i++)
            {
                if (commity_dataGridView.Rows[i].Cells["C_Name"].Value == null || commity_dataGridView.Rows[i].Cells["C_Name"].Value.ToString() == null)
                    C_ID = -1;
                else C_ID = Convert.ToInt32(commity_dataGridView.Rows[i].Cells["C_Name"].Value.ToString());

                if (commity_dataGridView.Rows[i].Cells["C_Notes"].Value == null || commity_dataGridView.Rows[i].Cells["C_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = commity_dataGridView.Rows[i].Cells["C_Notes"].Value.ToString();

                if (commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString() == "مع")
                { opinion = 1; }
                else if (commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString() == "ضد")
                    opinion = 0;
                else opinion = -1;
                opinion_text = commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString();

                //set ID columns//
                commity_dataGridView.Rows[i].Cells["C_ID"].Value = C_ID;
                if (C_ID != -1)
                {
                    if (commity_dataGridView.Rows[i].Cells["C_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert_Checklist_Members(Person_ID, C_ID, opinion, notes, "", "", "c");

                        commity_dataGridView.Rows[i].Cells["C_InDataBase"].Value = "1";
                        commity_dataGridView.Rows[i].Cells["C_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (c)-" + C_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(commity_dataGridView.Rows[i].Cells["C_Checklist_ID"].Value);
                        cl.Update_Checklist_Members(RowID, C_ID, opinion, notes, "", "");

                        l.Insert_Log("Update (c)-" + C_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Beneficiary_comboBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }
        }

        #region new checklist members
        private void Add_K_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AddNewChecklistMember("Key Person", "k"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_T_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AddNewChecklistMember("Team Member", "t"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_V_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AddNewChecklistMember("Visitors", "t"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_C_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AddNewChecklistMember("Commity Member", "c"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
         
        #region context menu strip
        private void visit_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new ChooseVisitKind_Form(MicroProject_ID, mainForm);
                _Form.ShowDialog();
            }
        }
        private void loan_toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new Loans_Form(MicroProject_ID, mainForm);
                mainForm.showNewTab(_Form, "Loans", 0);
            }
        }
        private void attachment_toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new Attachments_Form(MicroProject_ID, mainForm);
                mainForm.showNewTab(_Form, "Attachments", 0);
            }
        }
        //private void showChecklistToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (MicroProject_ID_textBox.Text != "")
        //    {
        //        var _Form = new Application_New_Form(mainForm,MicroProject_ID,Person_ID,"project");
        //        mainForm.showNewTab(_Form, "Application", 0);
        //    }
        //}
        #endregion

        #region printing 
        DataTable dt4444; 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Person_ID != -1 && MicroProject_ID != -1)
                {
                    //save before print
                    Save_button_Click(sender, e);

                    dt4444 = cl.Print_Checklist_Procedure(Person_ID.ToString());

                    Thread myTh = new Thread(SaveCallDialog_2);
                    myTh.SetApartmentState(ApartmentState.STA);
                    myTh.Start();
                    myTh.Join();
                }
                else
                {
                    MessageBox.Show("الرجاء اختيار المشروع أولاً ثم إعادة المحاولة.");
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
         
        private void SaveCallDialog_2()
        {
            try
            {
                //to close locked word files//
                Process[] workers = Process.GetProcessesByName("Winword.exe");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }

                FileInfo file;
                string sourcedirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\DocPrint";
                string myfilename = @"cl";
                string word_path = sourcedirectory + @"\" + myfilename + ".docx";
                string filename = sourcedirectory + @"\" + myfilename + ".xlsx";
                XLWorkbook wb = new XLWorkbook { RightToLeft = true };

                string appdata_path
                    = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\DocPrint";

                file = new FileInfo(appdata_path);


                // To copy a folder's contents to a new location:
                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(appdata_path);

                // To copy a file to another location and
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(
                      sourcedirectory + @"\" + myfilename + ".docx"
                    , appdata_path + @"\" + myfilename + ".docx"
                    , true);


                filename = appdata_path + @"\" + myfilename + ".xlsx";

                //DELETE old xls files
                FileInfo deletefile = new FileInfo(myfilename + ".xls");
                if (deletefile.Exists.Equals(true))
                {
                    deletefile.Delete();
                }

                wb.Worksheets.Add(dt4444, "Sheet1");
                
                wb.SaveAs(filename);
                wb.Dispose();

                // convert .xlsx to .xls
                string newfilename = filename.Remove(filename.Length - 1, 1);
                System.IO.File.Delete(newfilename);
                System.IO.File.Move(filename, newfilename);

                Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document document = ap.Documents.Open($@"{appdata_path}/{myfilename}.docx");
                document.MailMerge.MainDocumentType = WdMailMergeMainDocType.wdFormLetters;
                document.MailMerge.OpenDataSource($@"{appdata_path}/{myfilename}.xls"
                    , WdMailMergeMainDocType.wdFormLetters
                    , false
                    , false
                    , true
                    , false
                    , ""
                    , ""
                    , false
                    , ""
                    , ""
                    , $@"Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source={appdata_path}/{myfilename}.xlsx;"
                    , ""
                    , "");
                document.MailMerge.Destination = WdMailMergeDestination.wdSendToNewDocument;
                document.MailMerge.SuppressBlankLines = true;
                document.MailMerge.DataSource.LastRecord = (int)WdMailMergeDefaultRecord.wdDefaultFirstRecord;
                document.MailMerge.DataSource.LastRecord = (int)WdMailMergeDefaultRecord.wdDefaultLastRecord;
                document.MailMerge.Execute();
                document.Close(WdSaveOptions.wdDoNotSaveChanges);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                //to close locked word files//
                Process[] workers = Process.GetProcessesByName("Winword.exe");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
        }


        //FROM DISC PROGRAM//

        //        private void Print_button_Click(object sender, EventArgs e)
        //        {
        //            try
        //            {
        //                try
        //                {
        //                    //string condition = " where student.id = " + Student_ID + "";
        //                    dt4444 = new System.Data.DataTable();

        //                    string Images_appdata_path
        //= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DISC App\StudentPic\stup.Png";

        //                    dt4444 = s.Student_Details_Procedure(Student_ID, Images_appdata_path.Replace(@"\", @"\\\\\\\\"));

        //                    Thread myTh = new Thread(SaveCallDialog_2);
        //                    myTh.SetApartmentState(ApartmentState.STA);
        //                    myTh.Start();
        //                    myTh.Join();
        //                }
        //                catch (Exception ex)
        //                { MessageBox.Show(ex.Message); }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);

        //                //to close locked word files//
        //                Process[] workers = Process.GetProcessesByName("Winword.exe");
        //                foreach (Process worker in workers)
        //                {
        //                    worker.Kill();
        //                    worker.WaitForExit();
        //                    worker.Dispose();
        //                }
        //            }
        //        }
        //public static Image resizeImage(Image imgToResize, Size size)
        //{
        //    return (Image)(new Bitmap(imgToResize, size));
        //}

        //private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
        //    e.Graphics.DrawString("رقم الطالب" + ":" + StudentID_textBox.Text, new System.Drawing.Font("Arial", 12, FontStyle.Regular), Brushes.Blue, new PointF(750, 100), format);
        //    e.Graphics.DrawString("اسم الطالب" + ":" + FName_textBox.Text + " " + LName_textBox.Text, new System.Drawing.Font("Arial", 12, FontStyle.Regular), Brushes.Black, new PointF(750, 120), format);

        //}
        #endregion

        private void HasHVisit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (HasHVisit_checkBox.Checked)
                    HVisitDate_tableLayoutPanel.Enabled = true;
                else HVisitDate_tableLayoutPanel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HasPVisit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (HasPVisit_checkBox.Checked)
                    PVisitDate_tableLayoutPanel.Enabled = true;
                else PVisitDate_tableLayoutPanel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HasProTestYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (user_mode)
                //{
                if (HasProTestYes_radioButton.Checked)
                    HasProfTest_tableLayoutPanel.Enabled = true;
                else HasProfTest_tableLayoutPanel.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void HasProTrainYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (user_mode)
                //{
                if (HasProTrainYes_radioButton.Checked)
                    HasProTrain_tableLayoutPanel.Enabled = true;
                else HasProTrain_tableLayoutPanel.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
         
        private void HasMngTrainYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (user_mode)
                //{
                if (HasMngTrainYes_radioButton.Checked)
                    HasMngTrain_tableLayoutPanel.Enabled = true;
                else HasMngTrain_tableLayoutPanel.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void RejectMsg_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (user_mode)
                //{
                if (RejectMsg_checkBox.Checked)
                    RejectMsg_tableLayoutPanel.Enabled = true;
                else RejectMsg_tableLayoutPanel.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

    }
}
