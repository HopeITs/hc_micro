//using Chilkat;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;


namespace MyWorkApplication
{
    public partial class Application_New_Form : Form
    {
        private FtpConnector c;
        string FullPicName = "";
        private Document d;
        private Log l;
        private User user;
        private Images img;
        private Application_Queries App_Q;

        private readonly MainForm mainForm;
        private Thread myTh;

        public int MicroProject_ID = -1, Person_ID = -1, Family_ID = -1, Existed_Family_ID = -1;

        bool user_mode;
        DataSet ds;

        bool Deleted = false;
        bool NationalNum_Checked, FamilyNum_Checked;
        int ContentUpdated_original = -1;
        ToolTip manualToolTip = new ToolTip();
        bool Delete_Permitions = false;
        string P_Num = "", F_Num = "";
        bool Zero_TO_Project = false;

        bool UserMode = false; string ApplicationState; //pre_project , zero , project

        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, bool wParam, int lParam);


        public Application_New_Form(MainForm mainForm, int MicroProject_ID, int Person_ID, string ApplicationState)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            this.Person_ID = Person_ID;
            this.ApplicationState = ApplicationState;
        }

        private void Application_New_Form_Load(object sender, EventArgs e)
        {
            try
            {
                d = new Document();
                l = new Log();
                user = new User();
                c = new FtpConnector();
                img = new Images();
                App_Q = new Application_Queries();

                Check_Theme();

                Bind_Gridview_Dictionaries();
                Initialize_NumericTextBox_KeyPress();
                Stop_ComboBox_Scrolling();
                Stop_AutoGenerateColumns();
                Set_DeleteColumn_Width();

                Clear_Boxes();
                Bind_Data(false);
                Set_Controls();

                Stop_Highlighting_Combobox();

                PersonType_comboBox.SelectedIndex = 0; //default مستفيد

                if (MicroProject_ID > 0)
                {
                    Fill_Application_Data();
                }

                //unable to cast db.null to system.string !!
                mainForm.MP_ID_label.Visible = mainForm.ProjectNumber_label.Visible =
                    mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.TabName_label.Text = "Application";
                mainForm.MP_ID_label.Text = MPID_textBox.Text;



                Main_panel.AutoScrollPosition = new System.Drawing.Point(0, 0);
                //////////////////////////////
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show("لا يمكن فتح هذه الاستمارة بسبب خطأ ما", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void Application_Form_Shown(object sender, EventArgs e)
        {
            Stop_Highlighting_Combobox();
        }

        private void Application_New_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing && !Deleted)
                {
                    manualToolTip.Hide(Save_MP_ID_button);

                    bool anyDataGrid_ISDirty = IsDirty_DataGrid.Values.Any(v => v);
                    bool anyData_ISDirty = IsDirty_Data.Values.Any(v => v);

                    if (anyDataGrid_ISDirty || anyData_ISDirty)
                    {
                        if (MessageBox.Show("يوجد بعض التعديلات غير المحفوظة، هل تود حفظها قبل إغلاق الصفحة؟؟", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            this.ActiveControl = Save_button;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        #region One time entry functions 
        Dictionary<DataGridView, string> ID_Columns = new Dictionary<DataGridView, string>();
        Dictionary<DataGridView, string> IsDirty_Columns = new Dictionary<DataGridView, string>();

        private void Bind_Gridview_Dictionaries()

        {
            ID_Columns = new Dictionary<DataGridView, string>
                {
                    {Work_dataGridView,  "WP_ID"},
                    {Exp_dataGridView,  "ExpP_ID"},
                    {Education_dataGridView,  "PE_ID"},
                    {Course_dataGridView,  "CPerosn_ID"},
                    {PowerSource_dataGridView,  "Power_MP_ID"},
                    {Materials_dataGridView,  "MMP_ID"},
                    {FamilyMember_dataGridView,  "PF_ID"},
                    {Loss_dataGridView,  "LPerson_ID"},
                    {Illness_dataGridView,  "Ill_Person_ID"},
                    {Assets_dataGridView,  "APerson_ID"},
                    {AidSource_dataGridView,  "AidP_ID"},
                    {HelpSource_dataGridView,  "HelpP_ID"},
                    {HomePowerSource_dataGridView,  "HPowerP_ID"}
                };

            IsDirty_Columns = new Dictionary<DataGridView, string>
                {
                    {Work_dataGridView,  "W_IsDirty"},
                    {Exp_dataGridView,  "Exp_IsDirty"},
                    {Education_dataGridView,  "Edu_IsDirty"},
                    {Course_dataGridView,  "C_IsDirty"},
                    {PowerSource_dataGridView,  "Power_IsDirty"},
                    {Materials_dataGridView,  "M_IsDirty"},
                    {FamilyMember_dataGridView,  "P_IsDirty"},
                    {Loss_dataGridView,  "L_IsDirty"},
                    {Illness_dataGridView,  "Ill_IsDirty"},
                    {Assets_dataGridView,  "Asset_IsDirty"},
                    {AidSource_dataGridView,  "Aid_IsDirty"},
                    {HelpSource_dataGridView,  "Help_IsDirty"},
                    {HomePowerSource_dataGridView,  "HPower_IsDirty"}
                };
        }

        private void Initialize_NumericTextBox_KeyPress()
        {
            MPID_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            MP_RequestedAmount_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
            P_DOB_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            P_WorkYears_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            AmperNumber_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
            P_DebtsAmount_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
        }

        private void Stop_ComboBox_Scrolling()
        {
            //////////////////// handle auto scrolling for all comboBoxes //////////////////////
            SubCategory_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            Career_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            Type_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            SubType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_Sex_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            PersonType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_MaritalState_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_Parish_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_Priest_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_Priest2_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            Street_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            P_HomeType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            MP_PlaceType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            PaperApplicationUser_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            ///////////////////////////////////////////////////////////////////////////////////////
        }

        private void Stop_Highlighting_Combobox()
        {
            SubCategory_comboBox.SelectionLength = 0; SubCategory_comboBox.SelectionStart = SubCategory_comboBox.Text.Length;
            Career_comboBox.SelectionLength = 0; Career_comboBox.SelectionStart = Career_comboBox.Text.Length;
            Type_comboBox.SelectionLength = 0; Type_comboBox.SelectionStart = Type_comboBox.Text.Length;
            SubType_comboBox.SelectionLength = 0; SubType_comboBox.SelectionStart = SubType_comboBox.Text.Length;
            P_Sex_comboBox.SelectionLength = 0; P_Sex_comboBox.SelectionStart = P_Sex_comboBox.Text.Length;
            PersonType_comboBox.SelectionLength = 0; PersonType_comboBox.SelectionStart = PersonType_comboBox.Text.Length;
            P_MaritalState_comboBox.SelectionLength = 0; P_MaritalState_comboBox.SelectionStart = P_MaritalState_comboBox.Text.Length;
            P_Parish_comboBox.SelectionLength = 0; P_Parish_comboBox.SelectionStart = P_Parish_comboBox.Text.Length;
            P_Priest_comboBox.SelectionLength = 0; P_Priest_comboBox.SelectionStart = P_Priest_comboBox.Text.Length;
            P_Priest2_comboBox.SelectionLength = 0; P_Priest2_comboBox.SelectionStart = P_Priest2_comboBox.Text.Length;
            Street_comboBox.SelectionLength = 0; Street_comboBox.SelectionStart = Street_comboBox.Text.Length;
            P_HomeType_comboBox.SelectionLength = 0; P_HomeType_comboBox.SelectionStart = P_HomeType_comboBox.Text.Length;
            MP_PlaceType_comboBox.SelectionLength = 0; MP_PlaceType_comboBox.SelectionStart = MP_PlaceType_comboBox.Text.Length;
            PaperApplicationUser_comboBox.SelectionLength = 0; PaperApplicationUser_comboBox.SelectionStart = PaperApplicationUser_comboBox.Text.Length;
        }

        private void Stop_AutoGenerateColumns()
        {
            Work_dataGridView.AutoGenerateColumns =
                Exp_dataGridView.AutoGenerateColumns =
                Education_dataGridView.AutoGenerateColumns =
                Course_dataGridView.AutoGenerateColumns =
                PowerSource_dataGridView.AutoGenerateColumns =
                Materials_dataGridView.AutoGenerateColumns =
                FamilyMember_dataGridView.AutoGenerateColumns =
                Loss_dataGridView.AutoGenerateColumns =
                Illness_dataGridView.AutoGenerateColumns =
                Assets_dataGridView.AutoGenerateColumns =
                AidSource_dataGridView.AutoGenerateColumns =
                HelpSource_dataGridView.AutoGenerateColumns =
                HomePowerSource_dataGridView.AutoGenerateColumns = false;
        }

        private void Set_DeleteColumn_Width()
        {

            Work_dataGridView.Columns["W_DeleteRow"].Width =
               Exp_dataGridView.Columns["Exp_DeleteRow"].Width =
               Education_dataGridView.Columns["Edu_DeleteRow"].Width =
               Course_dataGridView.Columns["C_DeleteRow"].Width =
               PowerSource_dataGridView.Columns["Power_DeleteRow"].Width =
               Materials_dataGridView.Columns["M_DeleteRow"].Width =
               FamilyMember_dataGridView.Columns["P_DeleteRow"].Width =
               Loss_dataGridView.Columns["L_DeleteRow"].Width =
               Illness_dataGridView.Columns["Ill_DeleteRow"].Width =
               Assets_dataGridView.Columns["Asset_DeleteRow"].Width =
               AidSource_dataGridView.Columns["Aid_DeleteRow"].Width =
               HelpSource_dataGridView.Columns["Help_DeleteRow"].Width =
               HomePowerSource_dataGridView.Columns["HPower_DeleteRow"].Width = 50;
        }

        public void Check_Theme()
        {
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.ApplicationNew_ToNight(this);
            else
                newTheme.ApplicationNew_ToLight(this);
        }
        #endregion

        #region Clear Data
        private void Clear_Boxes()
        {
            UserMode = false;

            #region Project

            P_FullName_textBox.Clear();
            MPID_textBox.Clear();
            P_MobileReadable_textBox.Clear();
            SubCategory_comboBox.SelectedIndex = -1;
            Career_comboBox.SelectedIndex = -1;
            ApplyDate_bcDateTimePicker.Value = DateTime.Now;
            Type_comboBox.SelectedIndex = -1;
            MP_Name_textBox.Clear();
            ContentUpdated_checkBox.Checked = false;
            SubType_comboBox.SelectedIndex = -1;
            Loan_radioButton.Checked = false;
            Grant_radioButton.Checked = false;
            MP_RequestedAmount_textBox.Clear();

            #endregion

            #region ProjectStory

            P_Loss_textBox.Clear();
            P_Goal_textBox.Clear();
            P_GoalHow_textBox.Clear();
            P_OtherNotes_textBox.Clear();

            if (P_Goals_panel.Controls != null && P_Goals_panel.Controls.Count > 0)
            {
                foreach (Control c in P_Goals_panel.Controls)
                {
                    if (c is CheckBox chk)
                    {
                        chk.Checked = false;
                    }
                }
            }


            #endregion

            #region Benificiry

            P_FirstName_textBox.Clear();
            P_LastName_textBox.Clear();
            P_Sex_comboBox.SelectedIndex = -1;
            PersonType_comboBox.SelectedIndex = -1;
            P_FullName_En_textBox.Clear();
            P_FatherName_textBox.Clear();
            P_MotherName_textBox.Clear();
            P_Registration_textBox.Clear();
            P_NationalNum_textBox.Clear();
            P_DOB_textBox.Clear();
            P_MaritalState_comboBox.SelectedIndex = -1;
            P_Parish_comboBox.SelectedIndex = -1;
            P_IntermidiaryName_textBox.Clear();
            P_Priest_comboBox.SelectedIndex = -1;
            P_Priest2_comboBox.SelectedIndex = -1;
            P_Mobile_textBox.Clear();
            P_HomeTel_textBox.Clear();
            P_Mobile2_textBox.Clear();
            Street_comboBox.SelectedIndex = -1;
            P_Address_textBox.Clear();
            P_HomeType_comboBox.SelectedIndex = -1;
            P_HomeStateNote_textBox.Clear();
            MilitarySingle_radioButton.Checked = false;
            MilitaryFinished_radioButton.Checked = false;
            MilitaryNow_radioButton.Checked = false;
            MilitaryExempt_radioButton.Checked = false;
            MilitaryDelayed_radioButton.Checked = false;
            Military_textBox.Clear();
            Military_Date_checkBox.Checked = false;
            Military_Date_dateTimePicker.Value = DateTime.Now;

            #endregion

            #region Work

            Work_dataGridView.Rows.Clear();
            Exp_dataGridView.Rows.Clear();
            P_WorkYears_textBox.Clear();

            #endregion

            #region Edu

            P_HasEducationYes_radioButton.Checked = false;
            P_HasEducationNo_radioButton.Checked = false;
            Education_dataGridView.Rows.Clear();
            P_HasCoursesYes_radioButton.Checked = false;
            P_HasCoursesNo_radioButton.Checked = false;
            Course_dataGridView.Rows.Clear();

            #endregion

            #region Projrct_Info

            MP_PlaceType_comboBox.SelectedIndex = -1;
            MP_PlaceAddress_textBox.Clear();
            MP_PlaceNote_textBox.Clear();
            PowerSource_dataGridView.Rows.Clear();
            AmperNumber_textBox.Clear();
            IsAmper_radioButton.Checked = false;
            IsWatt_radioButton.Checked = false;
            Agreement1_Yes_radioButton.Checked = false;
            Agreement1_No_radioButton.Checked = false;
            Agreement2_Yes_radioButton.Checked = false;
            Agreement2_No_radioButton.Checked = false;
            Agreement3_No_radioButton.Checked = false;
            Agreement3_Yes_radioButton.Checked = false;
            Agreement4_Yes_radioButton.Checked = false;
            Agreement4_No_radioButton.Checked = false;
            Materials_dataGridView.Rows.Clear();

            #endregion

            #region Family

            FamilyNum_textBox1.Clear();
            P_LiveWithFamily_radioButton.Checked = false;
            P_NotLiveWithFamily_radioButton.Checked = false;
            LiveWithFamily_Note_textBox.Clear();
            FamilyMember_dataGridView.Rows.Clear();
            P_HasLossesYes_radioButton.Checked = false;
            P_HasLossesNo_radioButton.Checked = false;
            Loss_dataGridView.Rows.Clear();
            P_HasIllnessYes_radioButton.Checked = false;
            P_HasIllnessNo_radioButton.Checked = false;
            Illness_dataGridView.Rows.Clear();
            P_HasAssetsYes_radioButton.Checked = false;
            P_HasAssetsNo_radioButton.Checked = false;
            Assets_dataGridView.Rows.Clear();
            P_HasAidSourceYes_radioButton.Checked = false;
            P_HasAidSourceNo_radioButton.Checked = false;
            AidSource_dataGridView.Rows.Clear();
            P_AidSourceNote_textBox.Clear();
            P_HasDebtsYes_radioButton.Checked = false;
            P_HasDebtsNo_radioButton.Checked = false;
            P_DebtsAmount_textBox.Clear();
            P_DebtsNote_textBox.Clear();
            #endregion

            #region AditionalInfo
            P_HasHelpSourceYes_radioButton.Checked = false;
            P_HasHelpSourceNo_radioButton.Checked = false;
            HelpSource_dataGridView.Rows.Clear();
            HomePowerSource_dataGridView.Rows.Clear();
            MP_SimpleProfit2_textBox.Clear();
            PaperApplicationUser_comboBox.SelectedIndex = -1;

            #endregion

            UserMode = true;

        }

        private void Set_Controls()
        {
            Initialize_IsDirty();

            UserMode = false;

            manualToolTip.Hide(Save_MP_ID_button);

            if (MicroProject_ID > 0)
            {
                Save_MP_ID_button.BackgroundImage = Properties.Resources.Cheked;
                toolTip1.SetToolTip(Save_MP_ID_button, "تم حجز رقم المشروع");
            }
            else
            {
                Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                manualToolTip.Show("لم يتم حجز الرقم بعد", Save_MP_ID_button, Save_MP_ID_button.Width / 2, -Convert.ToInt32(Save_MP_ID_button.Height / 1.5), 3000);
                toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");
            }




            t2_tableLayoutPanel.Height = 901; t3_tableLayoutPanel.Height = 823; t5_tableLayoutPanel.Height = 385;
            t6_tableLayoutPanel.Height = 323; t7_tableLayoutPanel.Height = 74; t8_tableLayoutPanel.Height = 606;
            t20_tableLayoutPanel.Height = 1188; t21_tableLayoutPanel.Height = 187; t4_tableLayoutPanel.Height = 366;
            t9_tableLayoutPanel.Height = 962; t10_tableLayoutPanel.Height = 396; t11_tableLayoutPanel.Height = 595;
            t12_tableLayoutPanel.Height = 406; final_tableLayoutPanel.Height = 265;

            t2_tableLayoutPanel.Visible = t3_tableLayoutPanel.Visible = t5_tableLayoutPanel.Visible =
            t6_tableLayoutPanel.Visible = t7_tableLayoutPanel.Visible = t8_tableLayoutPanel.Visible =
            t20_tableLayoutPanel.Visible = t21_tableLayoutPanel.Visible = t4_tableLayoutPanel.Visible =
            t9_tableLayoutPanel.Visible = t10_tableLayoutPanel.Visible = t11_tableLayoutPanel.Visible =
            t12_tableLayoutPanel.Visible = final_tableLayoutPanel.Visible = true;

            tableLayoutPanel11.Visible = true;

            FamilyMember_dataGridView.Columns["P_NationalNumber"].Visible = true;
            FamilyMember_dataGridView.Columns["P_EducationLevel"].Visible = true;
            FamilyMember_dataGridView.Columns["P_EducationType"].Visible = true;
            FamilyMember_dataGridView.Columns["P_E_Name"].Visible = true;

            //AidSource_dataGridView.Columns["Aid_PName"].Visible = true;
            //AidSource_dataGridView.Columns["Aid_Relation"].Visible = true;

            FundType_label.Visible = FundType_flowLayoutPanel.Visible =
            MP_RequestedAmount_label.Visible = MP_RequestedAmount_textBox.Visible = true;

            P_FullName_En_label.Visible = P_FullName_En_textBox.Visible =
            P_Registration_label.Visible = P_Registration_textBox.Visible =
            P_Priest_tableLayoutPanel.Visible =
            P_Mobile2_label.Visible = P_Mobile2_textBox.Visible = true;

            Military_flowLayoutPanel.Visible = true;
            flowLayoutPanel6.Visible = true;
            if (ApplicationState == "pre_project")
            {
                SubHeader1_label.Text = "حجز المشروع";

                tableLayoutPanel11.Visible = false;

                FundType_label.Visible = FundType_flowLayoutPanel.Visible =
                MP_RequestedAmount_label.Visible = MP_RequestedAmount_textBox.Visible = false;

                t2_tableLayoutPanel.Height = 0; t3_tableLayoutPanel.Height = 0; t5_tableLayoutPanel.Height = 0;
                t6_tableLayoutPanel.Height = 0; t7_tableLayoutPanel.Height = 0; t8_tableLayoutPanel.Height = 0;
                t20_tableLayoutPanel.Height = 0; t21_tableLayoutPanel.Height = 0; t4_tableLayoutPanel.Height = 0;
                t9_tableLayoutPanel.Height = 0; t10_tableLayoutPanel.Height = 0; t11_tableLayoutPanel.Height = 0;
                t12_tableLayoutPanel.Height = 0; final_tableLayoutPanel.Height = 0;

                t2_tableLayoutPanel.Visible = t3_tableLayoutPanel.Visible = t5_tableLayoutPanel.Visible =
                t6_tableLayoutPanel.Visible = t7_tableLayoutPanel.Visible = t8_tableLayoutPanel.Visible =
                t20_tableLayoutPanel.Visible = t21_tableLayoutPanel.Visible = t4_tableLayoutPanel.Visible =
                t9_tableLayoutPanel.Visible = t10_tableLayoutPanel.Visible = t11_tableLayoutPanel.Visible =
                t12_tableLayoutPanel.Visible = final_tableLayoutPanel.Visible = true;

            }
            else if (ApplicationState == "zero")
            {
                SubHeader1_label.Text = "الاستمارة الأولية للمشروع";

                if (Person_ID <= 0)
                {
                    tableLayoutPanel11.Visible = false;
                }

                FundType_label.Visible = FundType_flowLayoutPanel.Visible =
                MP_RequestedAmount_label.Visible = MP_RequestedAmount_textBox.Visible = false;

                P_FullName_En_label.Visible = P_FullName_En_textBox.Visible =
                P_Registration_label.Visible = P_Registration_textBox.Visible =
                P_Priest_tableLayoutPanel.Visible =
                P_Mobile2_label.Visible = P_Mobile2_textBox.Visible = false;

                t2_tableLayoutPanel.Height = 0; t6_tableLayoutPanel.Height = 0; t8_tableLayoutPanel.Height = 0;
                t20_tableLayoutPanel.Height = 0;
                t9_tableLayoutPanel.Height = 0; t11_tableLayoutPanel.Height = 0;

                t2_tableLayoutPanel.Visible = t6_tableLayoutPanel.Visible = t8_tableLayoutPanel.Visible =
                t20_tableLayoutPanel.Visible =
                t9_tableLayoutPanel.Visible = t11_tableLayoutPanel.Visible = true;

                Military_flowLayoutPanel.Visible = false;

                flowLayoutPanel6.Visible = false;

                FamilyMember_dataGridView.Columns["P_NationalNumber"].Visible = false;
                FamilyMember_dataGridView.Columns["P_EducationLevel"].Visible = false;
                FamilyMember_dataGridView.Columns["P_EducationType"].Visible = false;
                FamilyMember_dataGridView.Columns["P_E_Name"].Visible = false;

                //AidSource_dataGridView.Columns["Aid_PName"].Visible = false;
                //AidSource_dataGridView.Columns["Aid_Relation"].Visible = false;

                //t10_tableLayoutPanel
                P_Debts_tableLayoutPanel.Visible = false;
            }
            else if (ApplicationState == "project")
            {
                SubHeader1_label.Text = "استمارة المشروع";
            }

            NationalNum_Checked = false; FamilyNum_Checked = false;

            #region Project

            MP_RequestedAmount_textBox_original = MP_Name_textBox_original = P_MobileReadable_textBox_original = MPID_textBox_original = P_FullName_textBox_original = "";


            Type_comboBox.SelectedIndex = 0; // default MICRO
            Loan_radioButton.Checked = true;


            ContentUpdated_checkBox.Visible = true;
            if (ApplicationState != "project")
            {
                ContentUpdated_checkBox.Visible = false;
            }

            #endregion


            #region ProjectStory
            P_Loss_textBox_original = P_Goal_textBox_original = P_GoalHow_textBox_original = P_OtherNotes_textBox_original = "";


            #endregion


            #region Benificiry
            P_FirstName_textBox_original = P_LastName_textBox_original = P_FullName_En_textBox_original = P_FatherName_textBox_original = P_MotherName_textBox_original =
            P_Registration_textBox_original = P_NationalNum_textBox_original = P_DOB_textBox_original = P_IntermidiaryName_textBox_original = P_Mobile_textBox_original =
            P_HomeTel_textBox_original = P_Mobile2_textBox_original = P_Address_textBox_original = P_HomeStateNote_textBox_original = Military_textBox_original = "";


            Military_label2.Visible = false;
            Military_textBox.Visible = false;
            Military_Date_checkBox.Visible = false;
            Military_Date_dateTimePicker.Visible = false;

            #endregion


            #region Work
            P_WorkYears_textBox_original = "";
            #endregion


            #region Edu
            Education_dataGridView.ReadOnly = true;
            Course_dataGridView.ReadOnly = true;
            #endregion


            #region Projrct_Info
            AmperNumber_textBox_original = MP_PlaceNote_textBox_original = MP_PlaceAddress_textBox_original = "";

            IsAmper_radioButton.Checked = true;
            #endregion


            #region Family
            Existed_Family_ID = -1;
            FamilyNum_textBox1_original = "";

            P_NumAtHome_textBox.Text = "0";
            P_NumOfMembers_textBox.Text = "0";

            FamilyMember_dataGridView.ReadOnly = true;
            Loss_dataGridView.ReadOnly = true;
            Illness_dataGridView.ReadOnly = true;
            Assets_dataGridView.ReadOnly = true;
            AidSource_dataGridView.ReadOnly = true;


            LiveWithFamily_Note_textBox_original = P_AidSourceNote_textBox_original = P_DebtsAmount_textBox_original = P_DebtsNote_textBox_original = MP_SimpleProfit2_textBox_original = "";

            LiveWithFamily_Note_Label.Visible = LiveWithFamily_Note_textBox.Visible = false;

            P_DebtsNote_textBox.Visible = P_DebtsNote_label.Visible = P_DebtsAmount_textBox.Visible = P_DebtsAmount_label.Visible = false;

            #endregion


            #region AditionalInfo
            HelpSource_dataGridView.ReadOnly = true;
            #endregion

            AddExtention_button.Visible = Delete_MP_button.Visible = FullApp_button.Visible = Save_button.Visible = AddPartner_button.Visible = false;
            AddImage_button.Visible = Download_button.Visible = DeleteImage_button.Visible = false;
            attachments_toolStripMenuItem.Visible = payments_toolStripMenuItem.Visible = Visits_toolStripMenuItem.Visible = showChecklistToolStripMenuItem.Visible = addNotesToolStripMenuItem.Visible = false;


            if (Program.role == 1 || Program.role == 8) //admin, //admin_l0
            {
                Delete_Permitions = true;

                if (ApplicationState == "zero")
                {
                    Save_button.Visible = Delete_MP_button.Visible = true;

                    if (MicroProject_ID > 0 && Person_ID > 0)
                    {
                        FullApp_button.Visible = AddPartner_button.Visible = true;
                    }
                }
                else if (ApplicationState == "project")
                {
                    AddExtention_button.Visible = Delete_MP_button.Visible = FullApp_button.Visible = Save_button.Visible = AddPartner_button.Visible = true;
                }

                AddImage_button.Visible = Download_button.Visible = DeleteImage_button.Visible = true;
                attachments_toolStripMenuItem.Visible = payments_toolStripMenuItem.Visible = Visits_toolStripMenuItem.Visible = showChecklistToolStripMenuItem.Visible = addNotesToolStripMenuItem.Visible = true;
            }
            else if (Program.role == 2) //Data
            {
                Delete_Permitions = true;

                if (ApplicationState == "zero")
                {
                    Save_button.Visible = Delete_MP_button.Visible = true;

                    if (MicroProject_ID > 0 && Person_ID > 0)
                    {
                        FullApp_button.Visible = AddPartner_button.Visible = true;
                    }
                }
                else if (ApplicationState == "project")
                {
                    AddExtention_button.Visible = Delete_MP_button.Visible = FullApp_button.Visible = Save_button.Visible = AddPartner_button.Visible = true;
                }

                AddImage_button.Visible = Download_button.Visible = DeleteImage_button.Visible = true;
                attachments_toolStripMenuItem.Visible = Visits_toolStripMenuItem.Visible = showChecklistToolStripMenuItem.Visible = true;

            }
            else if (Program.role == 3 || Program.role == 7) //Financial, //Lawful
            {
                attachments_toolStripMenuItem.Visible = payments_toolStripMenuItem.Visible = true;
            }
            else if (Program.role == 4 || Program.role == 9) //Media, //Guest
            {

            }
            else
            {

            }
            UserMode = true;
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
                Brush backBrush = (Properties.Settings.Default.theme == "Light") ? Brushes.White : Brushes.DarkGray;

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

        private void Bind_Data(bool KeepSelection)
        {
            user_mode = false;
            string query = "";


            //ComboBoxes
            query += "SELECT ID, Name, Category_ID from `subcategory` ORDER BY Name ASC;"; //SubCategory_comboBox
            query += "SELECT ID, Name, SubCategory_ID from `career` ORDER BY Name ASC;"; //Career_comboBox
            query += "select ID, Name from microprojecttype order by ID ASC;"; //Type_comboBox
            query += "select ID, Name, Type_ID from microprojectsubtype order by ID ASC;"; //SubType_comboBox
            query += "select ID, Name from `maritalstatus` order by ID ASC;"; //P_MaritalState_comboBox
            query += "select ID, Name_ar as 'Name' from `parish` order by ID ASC;"; //P_Parish_comboBox
            query += "select Priest_ID as 'ID',Priest_Name as 'Name' from `priest` order by Priest_Name ASC;"; //P_Priest_comboBox, P_Priest2_comboBox
            query += "SELECT ID, Name from `street` order by Name ASC;"; //Street_comboBox
            query += "select ID, Name from `assettype` order by ID ASC;"; //P_HomeType_comboBox, MP_PlaceType_comboBox, Asset_Type
            query += "select UserID as 'ID', UserName as 'Name' from `user` "
                  + " where IsVisitor = 1 order by UserName ASC;"; //PaperApplicationUser_comboBox


            //Datagrid_ComboBoxes
            query += "select W_ID as 'ID',W_Name as 'Name' from `work` ORDER BY W_Name ASC;"; //W_Name, Exp_Name, P_WorkName
            query += "select ID, Name from `worktype` ORDER BY ID ASC;";  //W_WorkType, Exp_WorkType, FP_Exp_WorkType
            query += "select education.ID , education.Name, educationtype.ID as 'Educationtype_ID', educationtype.Name as 'Educationtype' "
                + " from `education` left join educationtype on educationtype.id = education.Type_ID ORDER BY Name ASC;"; //E_Name
            query += "SELECT ID, Name from `subcategory` ORDER BY Name ASC;"; //C_Cource
            query += "select ID ,Name from `powersource` ORDER BY ID ASC;"; //Power_Name, HPower_Name
            query += "select ID, Name from `relation` ORDER BY ID ASC;";  //P_Relation
            query += "select ID, Name, Type from `personstate` ORDER BY ID;";  //P_State
            query += "SELECT ID, Name from `loss` ORDER BY ID ASC;"; //L_Type
            query += "SELECT ID, Name from `illness` ORDER BY ID ASC;"; //Ill_Name
            query += "SELECT ID, Name from `medicalneeds` ORDER BY ID ASC;"; //Ill_MedicalNeeds
            query += "SELECT ID, Name from `asset` ORDER BY ID ASC;"; //Asset_Name
            query += "select ID, Name from `aid` order by ID ASC;"; //Help_Name
            query += "select ID, Name from `aidperiod` order by ID ASC;"; //Help_Period, Ill_AidPeriod
            query += "select ID ,Name from `aidsource` ORDER BY ID ASC;"; //Aid_Source, Help_Source, Ill_Donor
            query += "select person.P_ID as 'ID', CONCAT(person.P_FirstName,' ',person.P_LastName) as 'Name',"
                 + " relation.name as 'relation' from person"
                + " left join person_family on person_family.Person_ID = person.P_ID"
                + " left join relation on relation.id = person_family.Relation_ID"
                + " where person_family.P_Provider_ID = '" + Person_ID + "';";//Ill_PName, Asset_PName, Aid_PName
            query += "select ID ,Name from `educationlevel` ORDER BY ID ASC;"; //E_Level, P_EducationLevel


            //CheckBoxes
            query += "SELECT ID, Name from `goal` order by ID ASC;"; //GoalsCheckBoxes


            //Micro Project ID
            query += "select MAX(MP_ID) as 'ID' from `microproject`"; //MPID_textBox


            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();


            //ComboBoxes
            DataTable dt0 = ds.Tables[0].Copy(); //SubCategory_comboBox
            Bind_ComboBox(SubCategory_comboBox, dt0, KeepSelection);


            // Fill on SubCategory_comboBox selection changed
            //DataTable dt1 = ds.Tables[1].Copy(); //Career_comboBox
            //Bind_ComboBox(Career_comboBox, dt1, KeepSelection);

            DataTable dt2 = ds.Tables[2].Copy(); //Type_comboBox
            Bind_ComboBox(Type_comboBox, dt2, KeepSelection);

            // Fill on Type_comboBox selection changed
            //DataTable dt3 = ds.Tables[3].Copy(); //SubType_comboBox
            //Bind_ComboBox(SubType_comboBox, dt3, KeepSelection);

            DataTable dt4 = ds.Tables[4].Copy(); //P_MaritalState_comboBox
            Bind_ComboBox(P_MaritalState_comboBox, dt4, KeepSelection);

            DataTable dt5 = ds.Tables[5].Copy(); //P_Parish_comboBox
            Bind_ComboBox(P_Parish_comboBox, dt5, KeepSelection);

            DataTable dt6 = ds.Tables[6].Copy(); //P_Priest_comboBox
            Bind_ComboBox(P_Priest_comboBox, dt6, KeepSelection);

            DataTable dt61 = ds.Tables[6].Copy(); //P_Priest2_comboBox
            Bind_ComboBox(P_Priest2_comboBox, dt61, KeepSelection);

            DataTable dt7 = ds.Tables[7].Copy(); //Street_comboBox
            Bind_ComboBox(Street_comboBox, dt7, KeepSelection);

            DataTable dt8 = ds.Tables[8].Copy(); //P_HomeType_comboBox
            Bind_ComboBox(P_HomeType_comboBox, dt8, KeepSelection);

            DataTable dt81 = ds.Tables[8].Copy(); //MP_PlaceType_comboBox
            Bind_ComboBox(MP_PlaceType_comboBox, dt81, KeepSelection);

            DataTable dt812 = ds.Tables[8].Copy(); //MP_PlaceType_comboBox
            Bind_Gridview_ComboBox(this.Asset_Type, dt812);

            DataTable dt9 = ds.Tables[9].Copy(); //PaperApplicationUser_comboBox
            Bind_ComboBox(PaperApplicationUser_comboBox, dt9, KeepSelection);


            //Datagrid_ComboBoxes
            DataTable dt10 = ds.Tables[10].Copy(); //W_Name
            Bind_Gridview_ComboBox(this.W_Name, dt10);

            DataTable dt101 = ds.Tables[10].Copy(); //Exp_Name
            Bind_Gridview_ComboBox(this.Exp_Name, dt101);

            //DataTable dt122 = ds.Tables[12].Copy(); //P_WorkName
            //Bind_Gridview_ComboBox(this.P_WorkName, dt122);

            DataTable dt11 = ds.Tables[11].Copy(); //W_WorkType
            Bind_Gridview_ComboBox(this.W_WorkType, dt11);

            DataTable dt111 = ds.Tables[11].Copy(); //Exp_WorkType
            Bind_Gridview_ComboBox(this.Exp_WorkType, dt111);

            DataTable dt112 = ds.Tables[11].Copy(); //FP_Exp_WorkType
            Bind_Gridview_ComboBox(this.FP_Exp_WorkType, dt112);

            // Fill on E_Type selection changed
            //DataTable dt12 = ds.Tables[12].Copy(); //E_Name
            //Bind_Gridview_ComboBox(this.E_Name, dt12);

            DataTable dt13 = ds.Tables[13].Copy(); //C_Cource
            Bind_Gridview_ComboBox(this.C_Cource, dt13);

            DataTable dt14 = ds.Tables[14].Copy(); //Power_Name
            Bind_Gridview_ComboBox(this.Power_Name, dt14);

            DataTable dt141 = ds.Tables[14].Copy(); //HPower_Name
            Bind_Gridview_ComboBox(this.HPower_Name, dt141);

            DataTable dt15 = ds.Tables[15].Copy(); //P_Relation
            DataTable selected_dt = new DataTable();
            DataRow[] rows = dt15.Select("Name <> 'مستفيد'");
            if (rows.Length != 0)
            {
                selected_dt = rows.CopyToDataTable();
                if (selected_dt != null && selected_dt.Rows.Count > 0)
                    Bind_Gridview_ComboBox(this.P_Relation, selected_dt);
            }

            DataTable dt16 = ds.Tables[16].Copy(); //P_State
            Bind_Gridview_ComboBox(this.P_State, dt16);

            DataTable dt17 = ds.Tables[17].Copy(); //L_Type
            Bind_Gridview_ComboBox(this.L_Type, dt17);

            DataTable dt18 = ds.Tables[18].Copy(); //Ill_Name
            Bind_Gridview_ComboBox(this.Ill_Name, dt18);

            DataTable dt19 = ds.Tables[19].Copy(); //Ill_MedicalNeeds
            Bind_Gridview_ComboBox(this.Ill_MedicalNeeds, dt19);

            DataTable dt20 = ds.Tables[20].Copy(); //Asset_Name
            Bind_Gridview_ComboBox(this.Asset_Name, dt20);

            DataTable dt21 = ds.Tables[21].Copy(); //Help_Name
            Bind_Gridview_ComboBox(this.Help_Name, dt21);

            DataTable dt22 = ds.Tables[22].Copy(); //Help_Period
            Bind_Gridview_ComboBox(this.Help_Period, dt22);

            // Fill on Ill_Aid selection changed
            //DataTable dt221 = ds.Tables[22].Copy(); //Ill_AidPeriod
            //Bind_Gridview_ComboBox(this.Ill_AidPeriod, dt221);

            DataTable dt23 = ds.Tables[23].Copy(); //Aid_Source
            Bind_Gridview_ComboBox(this.Aid_Source, dt23);

            DataTable dt231 = ds.Tables[23].Copy(); //Help_Source
            Bind_Gridview_ComboBox(this.Help_Source, dt231);

            // Fill on Ill_Aid selection changed
            //DataTable dt232 = ds.Tables[23].Copy(); //Ill_Donor
            //Bind_Gridview_ComboBox(this.Ill_Donor, dt232);


            DataTable dt24 = ds.Tables[24].Copy(); //Ill_PName
            Bind_Gridview_ComboBox(this.Ill_PName, dt24);

            DataTable dt241 = ds.Tables[24].Copy(); //Asset_PName
            Bind_Gridview_ComboBox(this.Asset_PName, dt241);

            DataTable dt242 = ds.Tables[24].Copy(); //Aid_PName
            Bind_Gridview_ComboBox(this.Aid_PName, dt242);

            DataTable dt25 = ds.Tables[25].Copy(); //E_Level
            DataTable selected_dt1 = new DataTable();
            DataRow[] rows1 = dt25.Select("Name <> 'غير متعلّم'");
            if (rows1.Length != 0)
            {
                selected_dt1 = rows1.CopyToDataTable();
                if (selected_dt1 != null && selected_dt1.Rows.Count > 0)
                    Bind_Gridview_ComboBox(this.E_Level, selected_dt1);
            }

            DataTable dt251 = ds.Tables[25].Copy(); //P_EducationLevel
            Bind_Gridview_ComboBox(this.P_EducationLevel, dt251);

            //CheckBoxes
            DataTable dt26 = ds.Tables[26].Copy();//GoalsCheckBoxes
            Bind_Chickbox_List(P_Goals_panel, dt26, true);

            DataTable dt261 = ds.Tables[26].Copy();//Original_GoalsCheckBoxes
            Bind_Chickbox_List(Original_P_Goals_panel, dt261, false);

            //Micro Project ID
            DataTable dt27 = ds.Tables[27].Copy();
            if (MicroProject_ID <= 0)
                Bind_New_MicroProject_ID(dt27);


            user_mode = true;

        }

        private void Bind_Persons_Data(bool KeepSelection)
        {
            string query = "";

            user_mode = false;

            query += "select person.P_ID as 'ID', CONCAT(person.P_FirstName,' ',person.P_LastName) as 'Name',"
                 + " relation.name as 'relation' from person"
                + " left join person_family on person_family.Person_ID = person.P_ID"
                + " left join relation on relation.id = person_family.Relation_ID"
                + " where person_family.P_Provider_ID = '" + Person_ID + "';";//Ill_PName, Asset_PName, Aid_PName

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();

            DataTable dt24 = dt.Copy(); //Ill_PName
            Bind_Gridview_ComboBox(this.Ill_PName, dt24);

            DataTable dt241 = dt.Copy(); //Asset_PName
            Bind_Gridview_ComboBox(this.Asset_PName, dt241);

            DataTable dt242 = dt.Copy(); //Aid_PName
            Bind_Gridview_ComboBox(this.Aid_PName, dt242);

            user_mode = true;
        }

        private void Bind_ComboBox(ComboBox Combox_Name, DataTable dt, bool KeepSelection)
        {
            int SelectedValue = -1;

            if (Combox_Name.SelectedValue != null)
                SelectedValue = int.Parse(Combox_Name.SelectedValue.ToString());

            Combox_Name.DataSource = dt;
            Combox_Name.DisplayMember = "name";
            Combox_Name.ValueMember = "id";

            if (KeepSelection && SelectedValue != -1)
                Combox_Name.SelectedValue = SelectedValue;
            else
                Combox_Name.SelectedIndex = -1;

        }

        private void Bind_Chickbox_List(FlowLayoutPanel FlowLayoutPanel_Name, DataTable dt, bool With_Click_Functions)
        {
            //if (FlowLayoutPanel_Name.Controls != null && FlowLayoutPanel_Name.Controls.Count > 0)
            FlowLayoutPanel_Name.Controls.Clear(); // Clear any old checkboxes

            int checkBoxWidth = 225;
            int checkBoxHeight = 30;
            int margin = 16;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string goalName = dt.Rows[i]["Name"].ToString();
                int goalId = Convert.ToInt32(dt.Rows[i]["ID"]);

                CheckBox chk = new CheckBox
                {
                    Text = goalName,
                    Tag = goalId,              // Store ID in Tag
                    AutoSize = false,
                    Width = checkBoxWidth,
                    Height = checkBoxHeight,
                    RightToLeft = RightToLeft.Yes,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(2, 0, 2, 0),
                    Margin = new Padding(15, 5, 15, 5)
                };

                FlowLayoutPanel_Name.Controls.Add(chk);

                if (With_Click_Functions)
                {
                    chk.CheckedChanged -= P_Goals_checkBox_CheckedChanged;
                    chk.CheckedChanged += P_Goals_checkBox_CheckedChanged;
                }

            }
        }

        private void Bind_New_MicroProject_ID(DataTable dt)
        {
            int MP_ID = 0;
            if (dt.Rows.Count != 0)
                int.TryParse(dt.Rows[0].Field<int>("ID").ToString(), out MP_ID);

            MPID_textBox.Text = (++MP_ID).ToString();
        }

        private void SubCategory_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (SubCategory_comboBox.SelectedIndex != -1 || !string.IsNullOrWhiteSpace(SubCategory_comboBox.Text))
                    {
                        DataTable Career_dt = ds.Tables[1].Copy();

                        string Selected_SubCategory_ID = SubCategory_comboBox.SelectedValue.ToString();

                        DataTable selected_dt = new DataTable();


                        DataRow[] rows = Career_dt.Select("SubCategory_ID = '" + Selected_SubCategory_ID + "'");
                        if (rows.Length != 0)
                        {
                            selected_dt = rows.CopyToDataTable();
                            if (selected_dt != null && selected_dt.Rows.Count > 0)
                                Bind_ComboBox(Career_comboBox, selected_dt, false);
                        }
                    }
                    else
                    {
                        Career_comboBox.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SubCategory_comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (SubCategory_comboBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(SubCategory_comboBox.Text))
                    {
                        Career_comboBox.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Type_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (Type_comboBox.SelectedIndex != -1 || !string.IsNullOrWhiteSpace(Type_comboBox.Text))
                    {
                        DataTable SubType_dt = ds.Tables[3].Copy();

                        string Selected_Type_ID = Type_comboBox.SelectedValue.ToString();

                        DataTable selected_dt = new DataTable();


                        DataRow[] rows = SubType_dt.Select("Type_ID = '" + Selected_Type_ID + "'");
                        if (rows.Length != 0)
                        {
                            selected_dt = rows.CopyToDataTable();
                            if (selected_dt != null && selected_dt.Rows.Count > 0)
                                Bind_ComboBox(SubType_comboBox, selected_dt, false);
                        }
                    }
                    else
                    {
                        SubType_comboBox.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Type_comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (Type_comboBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(Type_comboBox.Text))
                    {
                        SubType_comboBox.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region dataGridView common functions
        private void Bind_Gridview_ComboBox(DataGridViewComboBoxColumn Combox_Name, DataTable dt)
        {
            Combox_Name.DataSource = null;
            Combox_Name.DataSource = dt;
            Combox_Name.DisplayMember = "name";
            Combox_Name.ValueMember = "id";
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                string columnName = dataGridView.Columns[e.ColumnIndex].Name;

                if (columnName == "W_DeleteRow" || columnName == "Exp_DeleteRow" || columnName == "Edu_DeleteRow"
                    || columnName == "C_DeleteRow" || columnName == "Power_DeleteRow" || columnName == "M_DeleteRow"
                    || columnName == "P_DeleteRow" || columnName == "L_DeleteRow" || columnName == "Ill_DeleteRow"
                    || columnName == "Asset_DeleteRow" || columnName == "Aid_DeleteRow" || columnName == "HPower_DeleteRow"
                    || columnName == "Help_DeleteRow")
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new Point((e.CellBounds.Width - size.Width) / 2,
                        (e.CellBounds.Height - size.Height) / 2);
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

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var dataGridView = sender as DataGridView;

            if (e.Control is ComboBox cb)
            {
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;

                cb.MouseWheel -= comboBox_MouseWheel;
                cb.MouseWheel += comboBox_MouseWheel;
            }

            if (e.Control is TextBox textBox && dataGridView.CurrentCell != null)
            {
                string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;

                textBox.KeyPress -= InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
                textBox.KeyPress -= InputValidationHelpercs.Integers_NumericTextBox_KeyPress;

                if (columnName == "W_Salary" || columnName == "Power_Cost" || columnName == "M_Price" ||
                    columnName == "M_Amount" || columnName == "P_Salary" || columnName == "Aid_Amount" ||
                    columnName == "HPower_Cost")
                {
                    textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
                }

                if (columnName == "W_BeginDate" || columnName == "Exp_BeginDate" || columnName == "Exp_EndDate" ||
                    columnName == "C_Date" || columnName == "P_DOB" || columnName == "L_Year" ||
                    columnName == "Asset_Year" || columnName == "Aid_DateYear")
                {
                    textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
                    textBox.MaxLength = 4;
                }
            }
        }

        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                string columnName = dataGridView.Columns[e.ColumnIndex].Name;

                if (columnName == "W_BeginDate" || columnName == "Exp_BeginDate" || columnName == "Exp_EndDate" ||
                    columnName == "C_Date" || columnName == "P_DOB" || columnName == "L_Year" ||
                    columnName == "Asset_Year" || columnName == "Aid_DateYear")
                {
                    var cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string value = cell.EditedFormattedValue?.ToString() ?? "";

                    if (int.TryParse(value, out int year))
                    {
                        if (year < 1900 || year > DateTime.Now.Year)
                        {
                            // Clear the TextBox if it's still in edit mode
                            if (dataGridView.EditingControl is TextBox tb)
                                tb.Text = string.Empty;

                            MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (columnName == "P_NationalNumber")
                {
                    var cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string value = cell.EditedFormattedValue?.ToString() ?? "";

                    if (!string.IsNullOrWhiteSpace(value) && value.Length != 11)
                        MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات الرقم الوطني لا تساوي 11"
                            , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                dataGridView.EndEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex == dataGridView.NewRowIndex) return;

                string columnName = dataGridView.Columns[e.ColumnIndex].Name;

                //Check if click is on specific column 
                if (columnName.Contains("DeleteRow"))
                {
                    if (Delete_Permitions)
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Warning", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (dataGridView.Rows[e.RowIndex].Cells[ID_Columns[dataGridView]].Value != null)
                            {
                                dataGridView.Rows[e.RowIndex].Cells[IsDirty_Columns[dataGridView]].Value = "1";
                                dataGridView.Rows[e.RowIndex].Visible = false;
                                IsDirty_DataGrid[dataGridView] = true;
                            }
                            else
                            {
                                dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                dataGridView.Rows[e.RowIndex].Cells[IsDirty_Columns[dataGridView]].Value = "1";
                IsDirty_DataGrid[dataGridView] = true;


                string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                if (columnName == "E_Type")
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value != null && !string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString()))
                    {
                        string Edu_Type = dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString();

                        dataGridView.Rows[e.RowIndex].Cells["E_Name"] = Load_Education_ComboBox(Edu_Type);

                    }
                }
                else if (columnName == "P_EducationType")
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["P_EducationType"].Value != null && !string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].Cells["P_EducationType"].Value.ToString()))
                    {
                        string Edu_Type = dataGridView.Rows[e.RowIndex].Cells["P_EducationType"].Value.ToString();

                        dataGridView.Rows[e.RowIndex].Cells["P_E_Name"] = Load_Education_ComboBox(Edu_Type);

                    }
                }
                else if (columnName == "Ill_Aid")
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["Ill_Aid"].Value == null || (bool)dataGridView.Rows[e.RowIndex].Cells["Ill_Aid"].Value == false)
                    {
                        dataGridView.Rows[e.RowIndex].Cells["Ill_AidPeriod"].Value = null;
                        dataGridView.Rows[e.RowIndex].Cells["Ill_Donor"].Value = null;

                        dataGridView.Rows[e.RowIndex].Cells["Ill_Donor"] = Load_Ill_Donor_ComboBox(true);
                        dataGridView.Rows[e.RowIndex].Cells["Ill_AidPeriod"] = Load_Ill_AidPeriod_ComboBox(true);
                    }
                    else if ((bool)dataGridView.Rows[e.RowIndex].Cells["Ill_Aid"].Value == true)
                    {
                        if (dataGridView.Rows[e.RowIndex].Cells["Ill_Donor"].Value == null || dataGridView.Rows[e.RowIndex].Cells["Ill_Donor"].Value.ToString() == "")
                            dataGridView.Rows[e.RowIndex].Cells["Ill_Donor"] = Load_Ill_Donor_ComboBox(false);

                        if (dataGridView.Rows[e.RowIndex].Cells["Ill_AidPeriod"].Value == null || dataGridView.Rows[e.RowIndex].Cells["Ill_AidPeriod"].Value.ToString() == "")
                            dataGridView.Rows[e.RowIndex].Cells["Ill_AidPeriod"] = Load_Ill_AidPeriod_ComboBox(false);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                if (dataGridView.Name == "AidSource_dataGridView")
                {
                    string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    if (columnName == "Aid_PName")
                        AidSource_dataGridView.Rows[e.RowIndex].Cells["Aid_PName"] = Load_Persons_ComboBox();

                }

                else if (dataGridView.Name == "FP_Exp_dataGridView")
                {
                    string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    if (columnName == "FP_Exp_PName")
                        FP_Exp_dataGridView.Rows[e.RowIndex].Cells["FP_Exp_PName"] = Load_Persons_ComboBox();
                }
                else if (dataGridView.Name == "Illness_dataGridView")
                {
                    string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    if (columnName == "Ill_PName")
                        Illness_dataGridView.Rows[e.RowIndex].Cells["Ill_PName"] = Load_Persons_ComboBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;

                    var dataGridView = sender as DataGridView;
                    if (dataGridView.Name == "AidSource_dataGridView")
                    {
                        if (dataGridView.Columns[e.ColumnIndex].Name == "Aid_PName")
                        {
                            var selectedCell = dataGridView.Rows[e.RowIndex].Cells["Aid_PName"];
                            if (selectedCell.Value == null) return;

                            int selectedID = Convert.ToInt32(selectedCell.Value);

                            var match = Members_dt?.Rows
                                .Cast<DataRow>()
                                .FirstOrDefault(r => Convert.ToInt32(r["ID"]) == selectedID);

                            if (match != null)
                            {
                                dataGridView.Rows[e.RowIndex].Cells["Aid_Relation"].Value = match["Relation"].ToString();
                            }
                        }
                    }
                    else if (dataGridView.Name == "FP_Exp_dataGridView")
                    {
                        if (dataGridView.Columns[e.ColumnIndex].Name == "FP_Exp_PName")
                        {
                            var selectedCell = dataGridView.Rows[e.RowIndex].Cells["FP_Exp_PName"];
                            if (selectedCell.Value == null) return;

                            int selectedID = Convert.ToInt32(selectedCell.Value);

                            var match = Members_dt?.Rows
                                .Cast<DataRow>()
                                .FirstOrDefault(r => Convert.ToInt32(r["ID"]) == selectedID);

                            if (match != null)
                            {
                                dataGridView.Rows[e.RowIndex].Cells["FP_Exp_Relation"].Value = match["FP_Exp_Relation"].ToString();
                            }
                        }
                    }
                    else if (dataGridView.Name == "Illness_dataGridView")
                    {
                        if (dataGridView.Columns[e.ColumnIndex].Name == "Ill_PName")
                        {
                            var selectedCell = dataGridView.Rows[e.RowIndex].Cells["Ill_PName"];
                            if (selectedCell.Value == null) return;

                            int selectedID = Convert.ToInt32(selectedCell.Value);

                            var match = Members_dt?.Rows
                                .Cast<DataRow>()
                                .FirstOrDefault(r => Convert.ToInt32(r["ID"]) == selectedID);

                            if (match != null)
                            {
                                dataGridView.Rows[e.RowIndex].Cells["Ill_Relation"].Value = match["Ill_Relation"].ToString();
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


        private DataGridViewComboBoxCell Load_Education_ComboBox(string Edu_type)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            DataTable Education_dt = ds.Tables[12].Copy();

            DataTable selected_dt = new DataTable();

            DataRow[] rows = Education_dt.Select("Educationtype = '" + Edu_type + "'");
            if (rows.Length != 0)
            {
                selected_dt = rows.CopyToDataTable();
            }

            cell.DataSource = selected_dt;
            cell.DisplayMember = "name";
            cell.ValueMember = "id";
            cell.FlatStyle = FlatStyle.Flat;

            return cell;
        }

        private DataGridViewComboBoxCell Load_Ill_Donor_ComboBox(bool Empty)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            DataTable Ill_Donor_dt = null;

            if (!Empty)
                Ill_Donor_dt = ds.Tables[23].Copy();


            cell.DataSource = Ill_Donor_dt;
            cell.DisplayMember = "name";
            cell.ValueMember = "id";
            cell.FlatStyle = FlatStyle.Flat;

            return cell;
        }

        private DataGridViewComboBoxCell Load_Ill_AidPeriod_ComboBox(bool Empty)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            DataTable Ill_AidPeriod_dt = null;

            if (!Empty)
                Ill_AidPeriod_dt = ds.Tables[22].Copy();

            cell.DataSource = Ill_AidPeriod_dt;
            cell.DisplayMember = "name";
            cell.ValueMember = "id";
            cell.FlatStyle = FlatStyle.Flat;

            return cell;
        }

        private DataTable Members_dt;
        private DataGridViewComboBoxCell Load_Persons_ComboBox()
        {
            Members_dt = new DataTable();
            Members_dt.Columns.Add("ID", typeof(int));
            Members_dt.Columns.Add("Name", typeof(string));
            Members_dt.Columns.Add("Relation", typeof(string));


            DataTable Relation_dt = ds.Tables[15].Copy();



            for (int i = 0; i <= FamilyMember_dataGridView.Rows.Count - 2; i++)
            {
                var row = FamilyMember_dataGridView.Rows[i];

                int PersonID;
                if (!string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["FPerson_ID"].Value ?? "")))
                    PersonID = Convert.ToInt32(row.Cells["FPerson_ID"].Value.ToString());
                else
                    PersonID = -i;

                string firstName = Convert.ToString(row.Cells["P_FirstName"].Value ?? "");
                string lastName = Convert.ToString(row.Cells["P_LastName"].Value ?? "");


                string relation = Relation_dt.AsEnumerable()
                            .Where(dtrow => dtrow.Field<int>("ID") == Convert.ToInt32(row.Cells["P_Relation"].Value))
                            .Select(dtrow => dtrow.Field<string>("Name"))
                            .FirstOrDefault();

                //string relation = Convert.ToString(row.Cells["P_Relation"].Value ?? "");

                string fullName = $"{firstName} {lastName}";

                Members_dt.Rows.Add(PersonID, fullName, relation);
            }


            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            cell.DataSource = Members_dt;
            cell.DisplayMember = "name";
            cell.ValueMember = "id";
            cell.FlatStyle = FlatStyle.Flat;

            return cell;
        }

        private void FamilyMember_dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            FamilyMember_dataGridView["P_State", e.RowIndex].Value = 1;
            FamilyMember_dataGridView.Refresh();

        }

        #endregion

        #region IsDirty
        Dictionary<DataGridView, bool> IsDirty_DataGrid = new Dictionary<DataGridView, bool>();

        Dictionary<string, bool> IsDirty_Data = new Dictionary<string, bool>();

        FlowLayoutPanel Original_P_Goals_panel = new FlowLayoutPanel();

        bool MP_ID_IsDirty, NationalNum_ISDirty, FamilyNum_ISDirty;

        private void Initialize_IsDirty()
        {
            IsDirty_DataGrid = new Dictionary<DataGridView, bool>
                {
                    {Work_dataGridView,  false},
                    {Exp_dataGridView,  false},
                    {Education_dataGridView,  false},
                    {Course_dataGridView,  false},
                    {PowerSource_dataGridView,  false},
                    {Materials_dataGridView,  false},
                    {FamilyMember_dataGridView,  false},
                    {Loss_dataGridView, false},
                    {Illness_dataGridView,  false},
                    {Assets_dataGridView,  false},
                    {AidSource_dataGridView,  false},
                    {HelpSource_dataGridView,  false},
                    {HomePowerSource_dataGridView, false}
                };

            IsDirty_Data = new Dictionary<string, bool>
                {
                    {"Project_ISDirty",  false},
                    {"ProjectStory_ISDirty",  false},
                    {"Benificiry_ISDirty",  false},
                    {"Work_ISDirty",  false},
                    {"Edu_ISDirty",  false},
                    {"Projrct_Info_ISDirty",  false},
                    {"Family_ISDirty",  false},
                    {"AditionalInfo_ISDirty", false},
                };

            foreach (Control c in Original_P_Goals_panel.Controls)
            {
                if (c is CheckBox chk)
                {
                    chk.Checked = false;
                }
            }

            MP_ID_IsDirty = false; NationalNum_ISDirty = false; FamilyNum_ISDirty = false;
        }


        #region Project_ISDirty
        string MP_RequestedAmount_textBox_original, MP_Name_textBox_original, P_MobileReadable_textBox_original, MPID_textBox_original, P_FullName_textBox_original;

        private void MP_Name_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MP_Name_textBox_original = MP_Name_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_Name_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MP_Name_textBox_original != MP_Name_textBox.Text)
                        IsDirty_Data["Project_ISDirty"] = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }

        }

        private void MPID_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MPID_textBox_original = MPID_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MPID_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MPID_textBox_original != MPID_textBox.Text)
                    {
                        bool ID_TobeChanged = false;

                        if (MPID_textBox.Text == "")
                        {
                            MessageBox.Show("لا يمكن ترك خلية رقم المشروع فارغة!");
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد تغيير رقم المشروع من: " +
                                    MPID_textBox_original + " ؟ " + Environment.NewLine +
                                    "ليصبح الرقم: " + MPID_textBox.Text, "Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (MicroProject_ID == Convert.ToInt32(MPID_textBox.Text))
                                {

                                }
                                else if (Check_MicroProject_ID(MPID_textBox.Text))
                                {
                                    ID_TobeChanged = true;
                                }
                            }
                        }


                        if (ID_TobeChanged)
                        {
                            IsDirty_Data["Project_ISDirty"] = true;
                            MP_ID_IsDirty = true;
                            mainForm.MP_ID_label.Text = MPID_textBox.Text;

                            Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                            manualToolTip.Show("لم يتم حجز الرقم بعد", Save_MP_ID_button, Save_MP_ID_button.Width / 2, -Convert.ToInt32(Save_MP_ID_button.Height / 1.5), 3000);
                            toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

                        }
                        else
                        {
                            MP_ID_IsDirty = false;

                            if (MicroProject_ID > 0)
                            {
                                MPID_textBox.Text = MicroProject_ID.ToString();

                                MPID_textBox.Focus();
                                Save_MP_ID_button.BackgroundImage = Properties.Resources.Cheked;
                                manualToolTip.Hide(Save_MP_ID_button);
                                toolTip1.SetToolTip(Save_MP_ID_button, "تم حجز رقم المشروع");

                            }
                            else
                            {
                                DataTable dt27 = ds.Tables[27].Copy();
                                Bind_New_MicroProject_ID(dt27);

                                MPID_textBox.Focus();
                                Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                                manualToolTip.Show("لم يتم حجز الرقم بعد", Save_MP_ID_button, Save_MP_ID_button.Width / 2, -Convert.ToInt32(Save_MP_ID_button.Height / 1.5), 3000);
                                toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_RequestedAmount_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MP_RequestedAmount_textBox_original = MP_RequestedAmount_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_RequestedAmount_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MP_RequestedAmount_textBox_original != MP_RequestedAmount_textBox.Text)
                        IsDirty_Data["Project_ISDirty"] = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void ProjectArea_Combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["Project_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void ApplyDate_bcDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["Project_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void UpdateContent_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (MicroProject_ID == -1 && ContentUpdated_checkBox.Checked)
                    {
                        ContentUpdated_checkBox.Checked = false;
                        throw new InvalidOperationException("الرجاء حفظ المشروع أولاً ثم يمكنك التعديل على هذه الخلية.");
                    }
                    else
                    {
                        IsDirty_Data["Project_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grant_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (UserMode)
                IsDirty_Data["Project_ISDirty"] = true;
        }
        #endregion

        #region ProjectStory_ISDirty
        string P_Loss_textBox_original, P_Goal_textBox_original, P_GoalHow_textBox_original, P_OtherNotes_textBox_original;

        private void P_Loss_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Loss_textBox_original = P_Loss_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Loss_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_Loss_textBox_original != P_Loss_textBox.Text)
                        IsDirty_Data["ProjectStory_ISDirty"] = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Goal_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Goal_textBox_original = P_Goal_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Goal_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_Goal_textBox_original != P_Goal_textBox.Text)
                        IsDirty_Data["ProjectStory_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_GoalHow_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_GoalHow_textBox_original = P_GoalHow_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_GoalHow_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_GoalHow_textBox_original != P_GoalHow_textBox.Text)
                        IsDirty_Data["ProjectStory_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_OtherNotes_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_OtherNotes_textBox_original = P_OtherNotes_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_OtherNotes_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_OtherNotes_textBox_original != P_OtherNotes_textBox.Text)
                        IsDirty_Data["ProjectStory_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Goals_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    IsDirty_Data["ProjectStory_ISDirty"] = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Benificiry_ISDirty
        string P_FirstName_textBox_original, P_LastName_textBox_original, P_FullName_En_textBox_original, P_FatherName_textBox_original, P_MotherName_textBox_original,
            P_Registration_textBox_original, P_NationalNum_textBox_original, P_DOB_textBox_original, P_IntermidiaryName_textBox_original, P_Mobile_textBox_original,
            P_HomeTel_textBox_original, P_Mobile2_textBox_original, P_Address_textBox_original, P_HomeStateNote_textBox_original, Military_textBox_original;

        private void P_FirstName_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_FirstName_textBox_original = P_FirstName_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_FirstName_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_FirstName_textBox_original != P_FirstName_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_LastName_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_LastName_textBox_original = P_LastName_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_LastName_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_LastName_textBox_original != P_LastName_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_FullName_En_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_FullName_En_textBox_original = P_FullName_En_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_FullName_En_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_FullName_En_textBox_original != P_FullName_En_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_FatherName_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_FatherName_textBox_original = P_FatherName_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_FatherName_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_FatherName_textBox_original != P_FatherName_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_MotherName_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_MotherName_textBox_original = P_MotherName_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_MotherName_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_MotherName_textBox_original != P_MotherName_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Registration_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Registration_textBox_original = P_Registration_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Registration_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_Registration_textBox_original != P_Registration_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_NationalNum_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_NationalNum_textBox_original = P_NationalNum_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_NationalNum_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    UserMode = false;
                    if (P_NationalNum_textBox_original != P_NationalNum_textBox.Text)
                    {
                        NationalNum_Checked = false;
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                        NationalNum_ISDirty = true;

                        if (P_NationalNum_textBox.Text.Length != 11)
                            MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات الرقم الوطني لا تساوي 11"
                                , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        DataTable dt = Check_National_Number(P_NationalNum_textBox.Text);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int Old_P_ID = int.Parse(dt.Rows[0]["P_ID"].ToString());
                                int Old_MP_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                                var Old_Name = dt.Rows[0]["Beneficiary Name"].ToString();
                                string Old_ApplicationState = dt.Rows[0]["ApplicationState"].ToString();

                                var _MessageBox = new Application_MessageBox("National_ID", Old_P_ID, Old_MP_ID, Old_Name);
                                var r = _MessageBox.ShowDialog();
                                if (r == DialogResult.Yes)
                                {
                                    Form Appplication_New_Form = new Application_New_Form(mainForm, Old_MP_ID, Old_P_ID, Old_ApplicationState);
                                    mainForm.showNewTab(Appplication_New_Form, "Application", 0);
                                    P_NationalNum_textBox.Focus();
                                }
                                else if (r == DialogResult.No)
                                {
                                    NationalNum_Checked = true;
                                }
                                else
                                {
                                    P_NationalNum_textBox.Text = P_Num;
                                    P_NationalNum_textBox.Focus();
                                    NationalNum_ISDirty = false;
                                    NationalNum_Checked = true;
                                }
                            }
                            else
                            {
                                NationalNum_Checked = true;
                            }
                        }
                        else
                        {
                            P_NationalNum_textBox.Text = P_Num;
                            P_NationalNum_textBox.Focus();
                            NationalNum_ISDirty = false;
                            NationalNum_Checked = true;
                        }
                    }
                    UserMode = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_DOB_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_DOB_textBox_original = P_DOB_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void PersonDOB_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(P_DOB_textBox.Text))
                {
                    var date = Convert.ToInt32(P_DOB_textBox.Text);
                    if (date < 1900 || date > DateTime.Now.Year)
                    {
                        MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        P_DOB_textBox.Text = P_DOB_textBox_original;
                    }
                    else if (DateTime.Now.Year - date < 18) // منع ادخال استمارة قاصرين
                    {
                        MessageBox.Show("انتبه! المستفيد تحت السن القانوني", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        P_DOB_textBox.Text = P_DOB_textBox_original;
                    }
                }

                if (UserMode)
                {
                    if (P_DOB_textBox_original != P_DOB_textBox.Text)
                    {
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }


        }

        private void P_IntermidiaryName_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_IntermidiaryName_textBox_original = P_IntermidiaryName_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_IntermidiaryName_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_IntermidiaryName_textBox_original != P_IntermidiaryName_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Mobile_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Mobile_textBox_original = P_Mobile_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Mobile_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(P_Mobile_textBox.Text))
                {
                    if (P_Mobile_textBox.Text.Length != 10)
                        MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات رقم الجوال لا تساوي 10"
                            , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (UserMode)
                {
                    if (P_Mobile_textBox_original != P_Mobile_textBox.Text)
                    {
                        IsDirty_Data["Benificiry_ISDirty"] = true;

                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }


        }

        private void P_HomeTel_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_HomeTel_textBox_original = P_HomeTel_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HomeTel_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_HomeTel_textBox_original != P_HomeTel_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Mobile2_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Mobile2_textBox_original = P_Mobile2_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Mobile2_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(P_Mobile2_textBox.Text))
                {
                    if (P_Mobile2_textBox.Text.Length != 10)
                        MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات رقم الجوال لا تساوي 10"
                            , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (UserMode)
                {
                    if (P_Mobile2_textBox_original != P_Mobile2_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Address_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_Address_textBox_original = P_Address_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_Address_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_Address_textBox_original != P_Address_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HomeStateNote_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_HomeStateNote_textBox_original = P_HomeStateNote_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HomeStateNote_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_HomeStateNote_textBox_original != P_HomeStateNote_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void Military_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                Military_textBox_original = Military_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void Military_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (Military_textBox_original != Military_textBox.Text)
                        IsDirty_Data["Benificiry_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_Area_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    IsDirty_Data["Benificiry_ISDirty"] = true;

                    ComboBox cb = sender as ComboBox;
                    if (cb.Name == "PersonType_comboBox")
                    {
                        if (Person_ID > 0)
                        {
                            string message;
                            if (PersonType_comboBox.SelectedItem.ToString() == "المستفيد")
                            {
                                message = "انتبه! سوف يتم تغيير صفة الشخص من شريك إلى مستفيد";
                                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else if (PersonType_comboBox.SelectedItem.ToString() == "الشريك")
                            {
                                message = "انتبه! سوف يتم تغيير صفة الشخص من مستفيد إلى شريك";
                                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void Military_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Military_label2.Visible = false;
                Military_textBox.Visible = false;
                Military_Date_checkBox.Visible = false;
                Military_Date_dateTimePicker.Visible = false;

                if (MilitaryFinished_radioButton.Checked)
                {
                    Military_label2.Visible = true;
                    Military_textBox.Visible = true;
                    Military_Date_checkBox.Visible = true;

                    if (Military_Date_checkBox.Checked)
                        Military_Date_dateTimePicker.Visible = true;
                    else
                        Military_Date_dateTimePicker.Visible = false;
                }
                else if (MilitaryNow_radioButton.Checked)
                {
                    Military_label2.Visible = true;
                    Military_textBox.Visible = true;
                    Military_Date_checkBox.Visible = false;
                    Military_Date_dateTimePicker.Visible = false;
                }

                if (UserMode)
                    IsDirty_Data["Benificiry_ISDirty"] = true;

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void Military_Date_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (Military_Date_checkBox.Checked)
                    Military_Date_dateTimePicker.Visible = true;
                else Military_Date_dateTimePicker.Visible = false;

                if (UserMode)
                    IsDirty_Data["Benificiry_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }


        }

        private void Military_Date_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["Benificiry_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #region Work_ISDirty
        string P_WorkYears_textBox_original;

        private void P_WorkYears_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_WorkYears_textBox_original = P_WorkYears_textBox.Text;

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_WorkYears_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_WorkYears_textBox_original != P_WorkYears_textBox.Text)
                        IsDirty_Data["Work_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #region Edu_ISDirty

        private void P_HasEducationYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasEducationYes_radioButton.Checked)
                {
                    Education_dataGridView.ReadOnly = false;
                    if (UserMode)
                    {
                        IsDirty_Data["Edu_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasEducationNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasEducationNo_radioButton.Checked)
                {
                    Education_dataGridView.ReadOnly = true;
                    if (UserMode)
                    {
                        IsDirty_Data["Edu_ISDirty"] = true;

                        if (Education_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                for (int i = Education_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (Education_dataGridView.Rows[i].Cells[ID_Columns[Education_dataGridView]].Value != null)
                                    {
                                        Education_dataGridView.Rows[i].Cells[IsDirty_Columns[Education_dataGridView]].Value = "1";
                                        Education_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[Education_dataGridView] = true;

                                    }
                                    else
                                    {
                                        Education_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasEducationNo_radioButton.Checked = false;
                                P_HasEducationYes_radioButton.Checked = true;
                                Education_dataGridView.ReadOnly = false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HasCoursesYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasCoursesYes_radioButton.Checked)
                {
                    Course_dataGridView.ReadOnly = false;

                    if (UserMode)
                    {
                        IsDirty_Data["Edu_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasCoursesNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasCoursesNo_radioButton.Checked)
                {
                    Course_dataGridView.ReadOnly = true;

                    if (UserMode)
                    {
                        IsDirty_Data["Edu_ISDirty"] = true;

                        if (Course_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {


                                for (int i = Course_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (Course_dataGridView.Rows[i].Cells[ID_Columns[Course_dataGridView]].Value != null)
                                    {
                                        Course_dataGridView.Rows[i].Cells[IsDirty_Columns[Course_dataGridView]].Value = "1";
                                        Course_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[Course_dataGridView] = true;

                                    }
                                    else
                                    {
                                        Course_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasCoursesNo_radioButton.Checked = false;
                                P_HasCoursesYes_radioButton.Checked = true;
                                Course_dataGridView.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #region Projrct_Info_ISDirty

        string AmperNumber_textBox_original, MP_PlaceNote_textBox_original, MP_PlaceAddress_textBox_original;
        private void AmperNumber_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                AmperNumber_textBox_original = AmperNumber_textBox.Text;

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void AmperNumber_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (AmperNumber_textBox_original != AmperNumber_textBox.Text)
                        IsDirty_Data["Projrct_Info_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_PlaceNote_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MP_PlaceNote_textBox_original = MP_PlaceNote_textBox.Text;

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_PlaceNote_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MP_PlaceNote_textBox_original != MP_PlaceNote_textBox.Text)
                        IsDirty_Data["Projrct_Info_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_PlaceAddress_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MP_PlaceAddress_textBox_original = MP_PlaceAddress_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_PlaceAddress_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MP_PlaceAddress_textBox_original != MP_PlaceAddress_textBox.Text)
                        IsDirty_Data["Projrct_Info_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_PlaceType_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["Projrct_Info_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void Projrct_Info_Area_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["Projrct_Info_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #region Family_ISDirty
        string FamilyNum_textBox1_original, LiveWithFamily_Note_textBox_original, P_AidSourceNote_textBox_original, P_DebtsAmount_textBox_original, P_DebtsNote_textBox_original;
        private void FamilyNum_textBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                FamilyNum_textBox1_original = FamilyNum_textBox1.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void FamilyNum_textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    UserMode = false;
                    if (FamilyNum_textBox1_original != FamilyNum_textBox1.Text)
                    {
                        FamilyNum_Checked = false;
                        IsDirty_Data["Family_ISDirty"] = true;
                        FamilyNum_ISDirty = true;

                        DataTable dt = Check_Family_Num(FamilyNum_textBox1.Text);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int Old_P_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                                int Old_MP_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                                var Old_Name = dt.Rows[0]["الاسم الثلاثي"].ToString();
                                string Old_ApplicationState = dt.Rows[0]["ApplicationState"].ToString();

                                var _MessageBox = new Application_MessageBox("Family_Num", Old_P_ID, Old_MP_ID, Old_Name);
                                var r = _MessageBox.ShowDialog();
                                if (r == DialogResult.Yes)
                                {
                                    Form Appplication_New_Form = new Application_New_Form(mainForm, Old_MP_ID, Old_P_ID, Old_ApplicationState);
                                    mainForm.showNewTab(Appplication_New_Form, "Application", 0);
                                    FamilyNum_textBox1.Focus();
                                }
                                else if (r == DialogResult.No)
                                {
                                    Existed_Family_ID = int.Parse(dt.Rows[0]["Family_ID"].ToString());
                                    FamilyNum_Checked = true;
                                }
                                else
                                {
                                    FamilyNum_textBox1.Text = F_Num;
                                    FamilyNum_textBox1.Focus();
                                    FamilyNum_ISDirty = false;
                                    FamilyNum_Checked = true;
                                }
                            }
                            else
                            {
                                FamilyNum_Checked = true;
                            }
                        }
                        else
                        {
                            FamilyNum_textBox1.Text = F_Num;
                            FamilyNum_textBox1.Focus();
                            FamilyNum_ISDirty = false;
                            FamilyNum_Checked = true;
                        }
                    }
                    UserMode = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }

        }

        private void FamilyNum_textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FamilyNum_textBox1.Text))
                {
                    FamilyMember_dataGridView.ReadOnly = true;
                }
                else
                {
                    FamilyMember_dataGridView.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_LiveWithFamily_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LiveWithFamily_Note_Label.Visible = LiveWithFamily_Note_textBox.Visible = false;

                if (P_LiveWithFamily_radioButton.Checked)
                    LiveWithFamily_Note_Label.Visible = LiveWithFamily_Note_textBox.Visible = true;

                if (UserMode)
                    IsDirty_Data["Family_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HasLossesYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasLossesYes_radioButton.Checked)
                {
                    Loss_dataGridView.ReadOnly = false;
                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasLossesNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasLossesNo_radioButton.Checked)
                {
                    Loss_dataGridView.ReadOnly = true;

                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;

                        if (Loss_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {



                                for (int i = Loss_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (Loss_dataGridView.Rows[i].Cells[ID_Columns[Loss_dataGridView]].Value != null)
                                    {
                                        Loss_dataGridView.Rows[i].Cells[IsDirty_Columns[Loss_dataGridView]].Value = "1";
                                        Loss_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[Loss_dataGridView] = true;

                                    }
                                    else
                                    {
                                        Loss_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasLossesNo_radioButton.Checked = false;
                                P_HasLossesYes_radioButton.Checked = true;
                                Loss_dataGridView.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HasIllnessYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasIllnessYes_radioButton.Checked)
                {
                    Illness_dataGridView.ReadOnly = false;
                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasIllnessNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasIllnessNo_radioButton.Checked)
                {
                    Illness_dataGridView.ReadOnly = true;
                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;

                        if (Illness_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {


                                for (int i = Illness_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (Illness_dataGridView.Rows[i].Cells[ID_Columns[Illness_dataGridView]].Value != null)
                                    {
                                        Illness_dataGridView.Rows[i].Cells[IsDirty_Columns[Illness_dataGridView]].Value = "1";
                                        Illness_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[Illness_dataGridView] = true;

                                    }
                                    else
                                    {
                                        Illness_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasIllnessNo_radioButton.Checked = false;
                                P_HasIllnessYes_radioButton.Checked = true;
                                Illness_dataGridView.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HasAssetsYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasAssetsYes_radioButton.Checked)
                {
                    Assets_dataGridView.ReadOnly = false;

                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasAssetsNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasAssetsNo_radioButton.Checked)
                {
                    Assets_dataGridView.ReadOnly = true;

                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;
                        if (Assets_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {


                                for (int i = Assets_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (Assets_dataGridView.Rows[i].Cells[ID_Columns[Assets_dataGridView]].Value != null)
                                    {
                                        Assets_dataGridView.Rows[i].Cells[IsDirty_Columns[Assets_dataGridView]].Value = "1";
                                        Assets_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[Assets_dataGridView] = true;

                                    }
                                    else
                                    {
                                        Assets_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasAssetsNo_radioButton.Checked = false;
                                P_HasAssetsYes_radioButton.Checked = true;
                                Assets_dataGridView.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }

        }

        private void P_HasAidSourceYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasAidSourceYes_radioButton.Checked)
                {
                    AidSource_dataGridView.ReadOnly = false;

                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasAidSourceNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasAidSourceNo_radioButton.Checked)
                {
                    AidSource_dataGridView.ReadOnly = true;

                    if (UserMode)
                    {
                        IsDirty_Data["Family_ISDirty"] = true;

                        if (AidSource_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                for (int i = AidSource_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (AidSource_dataGridView.Rows[i].Cells[ID_Columns[AidSource_dataGridView]].Value != null)
                                    {
                                        AidSource_dataGridView.Rows[i].Cells[IsDirty_Columns[AidSource_dataGridView]].Value = "1";
                                        AidSource_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[AidSource_dataGridView] = true;
                                    }
                                    else
                                    {
                                        AidSource_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasAidSourceNo_radioButton.Checked = false;
                                P_HasAidSourceYes_radioButton.Checked = true;
                                AidSource_dataGridView.ReadOnly = false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void LiveWithFamily_Note_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                LiveWithFamily_Note_textBox_original = LiveWithFamily_Note_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void LiveWithFamily_Note_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (LiveWithFamily_Note_textBox_original != LiveWithFamily_Note_textBox.Text)
                        IsDirty_Data["Family_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_OtherDonors_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_AidSourceNote_textBox_original = P_AidSourceNote_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_OtherDonors_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_AidSourceNote_textBox_original != P_AidSourceNote_textBox.Text)
                        IsDirty_Data["Family_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void HasDebts_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                P_DebtsNote_textBox.Visible = P_DebtsNote_label.Visible = P_DebtsAmount_textBox.Visible = P_DebtsAmount_label.Visible = false;


                if (P_HasDebtsYes_radioButton.Checked)
                    P_DebtsNote_textBox.Visible = P_DebtsNote_label.Visible = P_DebtsAmount_textBox.Visible = P_DebtsAmount_label.Visible = true;

                if (UserMode)
                    IsDirty_Data["Family_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_DebtsAmount_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_DebtsAmount_textBox_original = P_DebtsAmount_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_DebtsAmount_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_DebtsAmount_textBox_original != P_DebtsAmount_textBox.Text)
                        IsDirty_Data["Family_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_DebtsNote_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                P_DebtsNote_textBox_original = P_DebtsNote_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_DebtsNote_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (P_DebtsNote_textBox_original != P_DebtsNote_textBox.Text)
                        IsDirty_Data["Family_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #region AditionalInfo_ISDirty
        string MP_SimpleProfit2_textBox_original;

        private void MP_SimpleProfit2_textBox_Enter(object sender, EventArgs e)
        {
            try
            {
                MP_SimpleProfit2_textBox_original = MP_SimpleProfit2_textBox.Text;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void MP_SimpleProfit2_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                {
                    if (MP_SimpleProfit2_textBox_original != MP_SimpleProfit2_textBox.Text)
                        IsDirty_Data["AditionalInfo_ISDirty"] = true;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void P_HasHelpSourceYes_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasHelpSourceYes_radioButton.Checked)
                {
                    HelpSource_dataGridView.ReadOnly = false;

                    if (UserMode)
                    {
                        IsDirty_Data["AditionalInfo_ISDirty"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void P_HasHelpSourceNo_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (P_HasHelpSourceNo_radioButton.Checked)
                {
                    HelpSource_dataGridView.ReadOnly = true;

                    if (UserMode)
                    {
                        IsDirty_Data["AditionalInfo_ISDirty"] = true;
                        if (HelpSource_dataGridView.Rows.Count > 1)
                        {
                            DialogResult dialogResult = MessageBox.Show("سيتم حذف جميع المعلومات المدخة سابقاً .. تأكيد الحذف؟", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                for (int i = HelpSource_dataGridView.Rows.Count - 2; i >= 0; i--)
                                {
                                    if (HelpSource_dataGridView.Rows[i].Cells[ID_Columns[HelpSource_dataGridView]].Value != null)
                                    {
                                        HelpSource_dataGridView.Rows[i].Cells[IsDirty_Columns[HelpSource_dataGridView]].Value = "1";
                                        HelpSource_dataGridView.Rows[i].Visible = false;
                                        IsDirty_DataGrid[HelpSource_dataGridView] = true;

                                    }
                                    else
                                    {
                                        HelpSource_dataGridView.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                P_HasHelpSourceNo_radioButton.Checked = false;
                                P_HasHelpSourceYes_radioButton.Checked = true;
                                HelpSource_dataGridView.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void PaperApplicationUser_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (UserMode)
                    IsDirty_Data["AditionalInfo_ISDirty"] = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        #endregion IsDirty

        #region button_Click
        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                this.ActiveControl = Save_button;
                Save_MP_ID_button.Enabled = false;
                Save_button.Enabled = false;

                Save_Application();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
            finally
            {
                Program.MyConn.Close();
                Save_MP_ID_button.Enabled = true;
                Save_button.Enabled = true;
            }
        }

        private void Save_MP_ID_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID > 0 && MicroProject_ID == Convert.ToInt32(MPID_textBox.Text))
                {
                    MessageBox.Show("تم حجز المشروع مسبقاً");
                    return;
                }
                else
                {
                    this.ActiveControl = Save_MP_ID_button;
                    Save_MP_ID_button.Enabled = false;
                    Save_button.Enabled = false;

                    Save_Application();
                    P_FirstName_textBox.Focus();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
            finally
            {
                Program.MyConn.Close();
                Save_MP_ID_button.Enabled = true;
                Save_button.Enabled = true;
            }
        }

        private void AddPartner_button_Click(object sender, EventArgs e)
        {
            try
            {
                Form Appplication_New_Form = new Application_New_Form(mainForm, MicroProject_ID, -1, "zero");
                mainForm.showNewTab(Appplication_New_Form, "Application", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FullApp_button_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("انتظر التحديث القادم");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Data Save / Fill
        private void Save_Application()
        {
            bool anyDataGrid_ISDirty = IsDirty_DataGrid.Values.Any(v => v);
            bool anyData_ISDirty = IsDirty_Data.Values.Any(v => v);

            if (!anyDataGrid_ISDirty && !anyData_ISDirty && ApplicationState != "pre_project")
            {
                MessageBox.Show("لا يوجد أي تغييرات لحفظها ", "Error");
                return;
            }

            string Query = "START TRANSACTION;";
            string dataGrid_Query = "";

            var sc = new MySqlCommand();

            if (MicroProject_ID > 0)
            {
                if (!MP_ID_IsDirty)
                    sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                else
                    sc.Parameters.AddWithValue("@Old_MicroProject_ID", MicroProject_ID);
            }
            else
            {
                Query += "Set @MicroProject_ID = null;";
            }

            if (Person_ID > 0)
            {
                sc.Parameters.AddWithValue("@Person_ID", Person_ID);
            }
            else
            {
                Query += "Set @Person_ID = null;";
            }

            if (Family_ID > 0)
            {
                if (!FamilyNum_ISDirty)
                {
                    sc.Parameters.AddWithValue("@Family_ID", Family_ID);
                }
                else
                {
                    if (Existed_Family_ID > 0)
                    {
                        sc.Parameters.AddWithValue("@Family_ID", Existed_Family_ID);
                    }
                    else
                    {
                        Query += "Set @Family_ID = null;";
                    }
                }
            }
            else
            {
                if (Existed_Family_ID > 0)
                {
                    sc.Parameters.AddWithValue("@Family_ID", Existed_Family_ID);
                }
                else
                {
                    Query += "Set @Family_ID = null;";
                }
            }



            sc.Parameters.AddWithValue("@ProgramUser_ID", Program.userID);


            sc.Parameters.AddWithValue("@Log_User", Program.UserName);
            sc.Parameters.AddWithValue("@Log_Date", DateTime.Now);


            #region Project
            string MP_ID = "", MP_Name = "", RequestedAmount = "";
            int SubCategory = -1, Career = -1, Type = -1, SubType = -1, LoanType = -1, ContentUpdated = -1;
            DateTime ApplyDate = new DateTime();

            if (string.IsNullOrWhiteSpace(MPID_textBox.Text))
            {
                MessageBox.Show("لا يمكن ترك رقم المشروع فارغاً", "Error");
                return;
            }

            if (MicroProject_ID <= 0)
            {
                if (!Check_MicroProject_ID(MPID_textBox.Text))
                {
                    return;
                }
            }
            else
            {
                if (MP_ID_IsDirty)
                    if (!Check_MicroProject_ID(MPID_textBox.Text))
                    {
                        return;
                    }
            }

            if (string.IsNullOrWhiteSpace(SubCategory_comboBox.Text) || SubCategory_comboBox.SelectedIndex < 0 || string.IsNullOrWhiteSpace(Career_comboBox.Text) || Career_comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("الرجاء التأكد من تحديد المهنة في قسم معومات المشروع", "Error");
                return;
            }

            if (ApplyDate_bcDateTimePicker.Value == null)
            {
                MessageBox.Show("الرجاء التأكد من تحديد تاريخ الطلب في قسم معومات المشروع", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(Type_comboBox.Text) || Type_comboBox.SelectedIndex < 0 || string.IsNullOrWhiteSpace(SubType_comboBox.Text) || SubType_comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("الرجاء التأكد من تحديد نوع وطبيعة المشورع في قسم معومات المشروع", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(MP_Name_textBox.Text))
            {
                MessageBox.Show("لا يمكن ترك وصف المشروع فارغاً في قسم معلومات المشروع", "Error");
                return;
            }

            if (ApplicationState == "project")
            {
                if (!Loan_radioButton.Checked && !Grant_radioButton.Checked)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد نوع التمويل في قسم معومات المشروع", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(MP_RequestedAmount_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك المبلغ فارغاً في قسم معلومات المشروع", "Error");
                    return;
                }
            }

            if (IsDirty_Data["Project_ISDirty"])
            {
                MP_ID = MPID_textBox.Text;

                SubCategory = int.Parse(SubCategory_comboBox.SelectedValue.ToString());
                Career = int.Parse(Career_comboBox.SelectedValue.ToString());

                ApplyDate = ApplyDate_bcDateTimePicker.Value;  //Convert to UK format

                Type = int.Parse(Type_comboBox.SelectedValue.ToString());
                SubType = int.Parse(SubType_comboBox.SelectedValue.ToString());

                MP_Name = MP_Name_textBox.Text;


                if (Loan_radioButton.Checked)
                    LoanType = 1;
                else if (Grant_radioButton.Checked)
                    LoanType = 2;

                RequestedAmount = MP_RequestedAmount_textBox.Text;

                if (ContentUpdated_original == 0 && ContentUpdated_checkBox.Checked)
                    ContentUpdated = 1;
                else if (ContentUpdated_original == 1 && !ContentUpdated_checkBox.Checked)
                    ContentUpdated = 0;


                if (MicroProject_ID <= 0)
                {
                    Query += " insert into microproject (MP_ID, MP_DateOfRequest, MP_Name, MP_RequestedAmount, MP_FundType_ID, MP_Type_ID, MP_SubType_ID, Category_ID, SubCategory_ID, Career_ID, ApplicationState, MP_ProgramUser_ID)" +
                        " Values (@MP_ID, @ApplyDate, @MP_Name, @RequestedAmount, @LoanType, @Type, @SubType, (select Category_ID from subcategory where subcategory.id = @SubCategory) ,@SubCategory, @Career, @ApplicationState, @ProgramUser_ID);";

                    Query += " SET @MicroProject_ID = @MP_ID;";

                    Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                        " Values (Concat('Insert zero application for the project:',@MicroProject_ID), 'Micro Project', @Log_User, @Log_Date, '0');";

                    Query += " insert into microproject_state (MicroProject_ID, State_ID, Date, Current_State)" +
                        "Values (@MicroProject_ID, (select state.id from state where state.Name_en = 'On Hold'), NOW(),1);";

                }
                else
                {
                    if (MP_ID_IsDirty)
                    {
                        Query += " update microproject set MP_ID = @MP_ID where MP_ID = @Old_MicroProject_ID;";
                        Query += " SET @MicroProject_ID = @MP_ID;";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Change micro project number - from:',@Old_MicroProject_ID, ' to:', @MicroProject_ID), 'Micro Project', @Log_User, @Log_Date, '0');";

                    }

                    Query += "update microproject set MP_DateOfRequest = @ApplyDate, MP_Name = @MP_Name" +
                        ",MP_RequestedAmount = @RequestedAmount, MP_FundType_ID = @LoanType, MP_Type_ID = @Type," +
                        " MP_SubType_ID = @SubType, Category_ID = (select Category_ID from subcategory where subcategory.id = @SubCategory)," +
                        " SubCategory_ID = @SubCategory, Career_ID = @Career, ApplicationState =@ApplicationState where MP_ID = @MicroProject_ID;";
                }


                sc.Parameters.AddWithValue("@MP_ID", string.IsNullOrEmpty(MP_ID) ? (object)DBNull.Value : MP_ID);
                sc.Parameters.AddWithValue("@SubCategory", SubCategory == -1 ? (object)DBNull.Value : SubCategory);
                sc.Parameters.AddWithValue("@Career", Career == -1 ? (object)DBNull.Value : Career);
                sc.Parameters.AddWithValue("@ApplyDate", ApplyDate == null ? (object)DBNull.Value : ApplyDate);
                sc.Parameters.AddWithValue("@Type", Type == -1 ? (object)DBNull.Value : Type);
                sc.Parameters.AddWithValue("@SubType", SubType == -1 ? (object)DBNull.Value : SubType);
                sc.Parameters.AddWithValue("@MP_Name", MP_Name);
                sc.Parameters.AddWithValue("@LoanType", LoanType == -1 ? (object)DBNull.Value : LoanType);
                sc.Parameters.AddWithValue("@RequestedAmount", RequestedAmount);
            }
            #endregion

            if (ApplicationState == "project" || ApplicationState == "zero")
            {
                if (ApplicationState == "zero")
                {
                    if (!Zero_TO_Project)
                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Update zero appliction for the project:',@MicroProject_ID), '', @Log_User, @Log_Date, '0');";
                    else
                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Insert full appliction for the project:',@MicroProject_ID), '', @Log_User, @Log_Date, '0');";
                }
                else if (ApplicationState == "project")
                {
                    Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                        " Values (Concat('Update full appliction for the project:',@MicroProject_ID), '', @Log_User, @Log_Date, '0');";
                }

                #region ProjectStory
                string P_Loss = "", P_Goal = "", P_GoalHow = "", P_OtherNotes = "";
                string Goal_Query = "";

                if (ApplicationState == "project")
                {
                    if (string.IsNullOrWhiteSpace(P_Loss_textBox.Text))
                    {
                        MessageBox.Show("لا يمكن ترك قصة المستفيد فارغة في قسم معلومات المشروع", "Error");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(P_Goal_textBox.Text))
                    {
                        MessageBox.Show("لا يمكن ترك هدف المستفيد من القرض فارغاً في قسم معلومات المشروع", "Error");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(P_GoalHow_textBox.Text))
                    {
                        MessageBox.Show("لا يمكن ترك كيف سيحقق المشروع أهدافك فارغاً في قسم معلومات المشروع", "Error");
                        return;
                    }
                }

                if (IsDirty_Data["ProjectStory_ISDirty"])
                {
                    if (ApplicationState == "project")
                    {
                        P_Loss = P_Loss_textBox.Text;

                        P_Goal = P_Goal_textBox.Text;

                        P_GoalHow = P_GoalHow_textBox.Text;

                        P_OtherNotes = P_OtherNotes_textBox.Text;

                        var checkBoxMap = P_Goals_panel.Controls
                                .OfType<CheckBox>()
                                .Where(cb => cb.Tag != null)
                                .ToDictionary(cb => Convert.ToInt32(cb.Tag));

                        foreach (Control original_ctrl in Original_P_Goals_panel.Controls)
                        {
                            if (original_ctrl is CheckBox OriginalCheckbox && OriginalCheckbox.Tag != null)
                            {
                                int original_ID = Convert.ToInt32(OriginalCheckbox.Tag);

                                if (checkBoxMap.TryGetValue(original_ID, out CheckBox Checkbox))
                                {
                                    if (OriginalCheckbox.Checked != Checkbox.Checked)
                                    {
                                        if (OriginalCheckbox.Checked)
                                        {
                                            Goal_Query += "DELETE FROM person_goals WHERE Person_ID = @Person_ID AND Goal_ID = '" + original_ID.ToString() + "';";
                                        }
                                        else
                                        {
                                            Goal_Query += "INSERT INTO person_goals (Person_ID, Goal_ID) VALUES (@Person_ID, '" + original_ID.ToString() + "');";
                                        }
                                    }
                                }
                            }
                        }
                    }



                }
                #endregion

                #region Benificiry
                string P_FirstName = "", P_LastName = "", P_Sex = "", PersonType = "", P_FullName_En = "", P_FatherName = "",
                    P_MotherName = "", P_Registration = "", P_NationalNum = "", P_IntermidiaryName = "",
                     P_Mobile = "", P_HomeTel = "", P_Mobile2 = "", P_Address = "", P_HomeStateNote = "", Military = "",
                     Military_Note = "";
                int P_MaritalState = -1, P_Parish = -1, P_Priest = -1, P_Priest2 = -1, Street = -1, P_HomeType = -1;
                DateTime P_DOB = new DateTime(); DateTime Military_Date = new DateTime();

                if (string.IsNullOrWhiteSpace(P_FirstName_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك الاسم فارغاً في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_LastName_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك الكنية فارغة في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_Sex_comboBox.Text) || P_Sex_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد الجنس في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PersonType_comboBox.Text) || PersonType_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد الصفة في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_FatherName_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك اسم الأب فارغاً في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_MotherName_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك اسم الأم فارغاً في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_NationalNum_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك الرقم الوطني فارغاً في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }
                else if (!NationalNum_Checked)
                {
                    bool CheckID = false;
                    if (Person_ID <= 0)
                        CheckID = true;
                    else
                    {
                        if (NationalNum_ISDirty)
                        {
                            CheckID = true;
                        }
                    }

                    if (CheckID)
                    {
                        UserMode = false;

                        DataTable dt = Check_National_Number(P_NationalNum_textBox.Text);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            int Old_P_ID = int.Parse(dt.Rows[0]["P_ID"].ToString());
                            int Old_MP_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                            var Old_Name = dt.Rows[0]["Beneficiary Name"].ToString();
                            string Old_ApplicationState = dt.Rows[0]["ApplicationState"].ToString();

                            var _MessageBox = new Application_MessageBox("National_ID", Old_P_ID, Old_MP_ID, Old_Name);
                            var r = _MessageBox.ShowDialog();
                            if (r == DialogResult.Yes)
                            {
                                Form Appplication_New_Form = new Application_New_Form(mainForm, Old_MP_ID, Old_P_ID, Old_ApplicationState);
                                mainForm.showNewTab(Appplication_New_Form, "Application", 0);
                                P_NationalNum_textBox.Focus();
                                return;
                            }
                            else if (r == DialogResult.No)
                            {

                            }
                            else
                            {
                                P_NationalNum_textBox.Text = P_Num;
                                P_NationalNum_textBox.Focus();
                                return;
                            }
                        }
                        else
                        {

                        }
                    }

                }

                if (string.IsNullOrWhiteSpace(P_DOB_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك خانة المواليد فارغة في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_MaritalState_comboBox.Text) || P_MaritalState_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد الحالة الاجتماعية في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_Parish_comboBox.Text) || P_Parish_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد الطائفة في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_IntermidiaryName_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك الهيئة الوسيطة فارغة في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (ApplicationState == "project")
                {
                    if ((string.IsNullOrWhiteSpace(P_Priest_comboBox.Text) || P_Priest_comboBox.SelectedIndex < 0) && (string.IsNullOrWhiteSpace(P_Priest2_comboBox.Text) || P_Priest2_comboBox.SelectedIndex < 0))
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد كاهن واحد على الأقل في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(P_Mobile_textBox.Text) && string.IsNullOrWhiteSpace(P_HomeTel_textBox.Text) && string.IsNullOrWhiteSpace(P_Mobile2_textBox.Text))
                {
                    MessageBox.Show("الرجاء التأكد من تحديد رقم تواصل واحد على الأقل في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Street_comboBox.Text) || Street_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد المنطقة(القرية) في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_Address_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك العنوان التفصيلي فارغاً في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(P_HomeType_comboBox.Text) || P_HomeType_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد ملكية المنزل في قسم المعلومات الشخصية لمقدم الطلب", "Error");
                    return;
                }

                if (IsDirty_Data["Benificiry_ISDirty"])
                {
                    P_FirstName = P_FirstName_textBox.Text;

                    P_LastName = P_LastName_textBox.Text;

                    P_Sex = P_Sex_comboBox.SelectedItem.ToString();

                    PersonType = PersonType_comboBox.SelectedItem.ToString();

                    P_FullName_En = P_FullName_En_textBox.Text;

                    P_FatherName = P_FatherName_textBox.Text;

                    P_MotherName = P_MotherName_textBox.Text;

                    P_Registration = P_Registration_textBox.Text;

                    P_NationalNum = P_NationalNum_textBox.Text;

                    P_DOB = Convert.ToDateTime(DateTime.ParseExact(P_DOB_textBox.Text, "yyyy", CultureInfo.InvariantCulture));

                    P_MaritalState = int.Parse(P_MaritalState_comboBox.SelectedValue.ToString());

                    P_Parish = int.Parse(P_Parish_comboBox.SelectedValue.ToString());

                    P_IntermidiaryName = P_IntermidiaryName_textBox.Text;

                    if (ApplicationState == "project")
                    {
                        if (!string.IsNullOrWhiteSpace(P_Priest_comboBox.Text) && P_Priest_comboBox.SelectedIndex >= 0)
                            P_Priest = int.Parse(P_Priest_comboBox.SelectedValue.ToString());
                        if (!string.IsNullOrWhiteSpace(P_Priest2_comboBox.Text) && P_Priest2_comboBox.SelectedIndex >= 0)
                            P_Priest2 = int.Parse(P_Priest2_comboBox.SelectedValue.ToString());
                    }

                    P_Mobile = P_Mobile_textBox.Text;
                    P_HomeTel = P_HomeTel_textBox.Text;
                    P_Mobile2 = P_Mobile2_textBox.Text;

                    Street = int.Parse(Street_comboBox.SelectedValue.ToString());

                    P_Address = P_Address_textBox.Text;

                    P_HomeType = int.Parse(P_HomeType_comboBox.SelectedValue.ToString());

                    P_HomeStateNote = P_HomeStateNote_textBox.Text;

                    if (MilitarySingle_radioButton.Checked)
                        Military = "وحيد";
                    else if (MilitaryFinished_radioButton.Checked)
                        Military = "مسرح";
                    else if (MilitaryNow_radioButton.Checked)
                        Military = "عسكري";
                    else if (MilitaryExempt_radioButton.Checked)
                        Military = "معفى/بدل";
                    else if (MilitaryDelayed_radioButton.Checked)
                        Military = "مؤجل";
                    else { Military = ""; }


                    Military_Note = Military_textBox.Text;

                    if (Military_Date_checkBox.Checked)
                    { 
                        Military_Date = Military_Date_dateTimePicker.Value;
                    }

                    if (Person_ID <= 0)
                    {
                        Query += "insert into person (P_FirstName, P_LastName, P_Sex, P_FullName_en, P_FatherName, P_MotherName," +
                            " P_RegistrationPlace, P_NationalNumber, P_DOB, P_MaritalStatus_ID, P_Parish_ID, P_IntermidiarySide," +
                            " P_Priest_ID, P_Priest2_ID, P_Mobile, P_HomeTel, P_Mobile2, Street_ID, P_HomeAddress, P_HomeType_ID," +
                            " P_HomeState_Note, P_MilitaryService, P_MilitaryService_Note, P_MilitaryService_Date, IsProjectOwner, P_NumAtHome, P_NumOfMembers, P_State_ID)" +
                            " Values (@P_FirstName, @P_LastName, @P_Sex, @P_FullName_En, @P_FatherName, @P_MotherName," +
                            " @P_Registration, @P_NationalNum, @P_DOB, @P_MaritalState, @P_Parish, @P_IntermidiaryName, " +
                            " @P_Priest, @P_Priest2, @P_Mobile, @P_HomeTel, @P_Mobile2, @Street, @P_Address, @P_HomeType," +
                            " @P_HomeStateNote, @Military, @Military_Note, @Military_Date, 'Yes', 1, 1, (select id from personstate where Name = 'مقيم/ة'));";

                        Query += " SET @Person_ID = Last_Insert_ID();";

                        Query += "insert into person_microproject (Person_ID, MicroProject_ID, PersonType)" +
                            "Values (@Person_ID, @MicroProject_ID, @PersonType);";

                        Query += "update microproject set Partnership = (SELECT COUNT(person_microproject.Person_ID) FROM person_microproject where person_microproject.MicroProject_ID = MP_ID) where MP_ID = @MicroProject_ID;";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Insert and link the Beneficiary:',@P_FirstName, ' ', @P_LastName, ' ', 'with the project:', @MicroProject_ID), 'Person', @Log_User, @Log_Date, '0');";
                    }
                    else
                    {
                        Query += "Update person set P_FirstName =@P_FirstName, P_LastName =@P_LastName, P_Sex = @P_Sex, P_FullName_en = @P_FullName_En, P_FatherName = @P_FatherName, P_MotherName = @P_MotherName, " +
                            " P_RegistrationPlace = @P_Registration, P_NationalNumber = @P_NationalNum, P_DOB = @P_DOB, P_MaritalStatus_ID = @P_MaritalState, P_Parish_ID = @P_Parish, P_IntermidiarySide = @P_IntermidiaryName," +
                            " P_Priest_ID = @P_Priest, P_Priest2_ID = @P_Priest2, P_Mobile = @P_Mobile, P_HomeTel = @P_HomeTel, P_Mobile2 = @P_Mobile2, Street_ID = @Street, P_HomeAddress = @P_Address, P_HomeType_ID = @P_HomeType," +
                            " P_HomeState_Note = @P_HomeStateNote, P_MilitaryService = @Military, P_MilitaryService_Note = @Military_Note, P_MilitaryService_Date = @Military_Date" +
                            " Where person.P_ID = @Person_ID;";

                        Query += "Update person_microproject set Person_ID = @Person_ID," +
                            " MicroProject_ID = @MicroProject_ID," +
                            " PersonType = @PersonType" +
                            " where MicroProject_ID = @MicroProject_ID and Person_ID = @Person_ID;";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Update Beneficiary:',@P_FirstName, ' ', @P_LastName, ' ', 'in the project:', @MicroProject_ID), 'Person', @Log_User, @Log_Date, '0');";
                    }

                    sc.Parameters.AddWithValue("@P_FirstName", P_FirstName);
                    sc.Parameters.AddWithValue("@P_LastName", P_LastName);
                    sc.Parameters.AddWithValue("@P_Sex", P_Sex);
                    sc.Parameters.AddWithValue("@PersonType", PersonType);
                    sc.Parameters.AddWithValue("@P_FullName_En", P_FullName_En);
                    sc.Parameters.AddWithValue("@P_FatherName", P_FatherName);
                    sc.Parameters.AddWithValue("@P_MotherName", P_MotherName);
                    sc.Parameters.AddWithValue("@P_Registration", P_Registration);
                    sc.Parameters.AddWithValue("@P_NationalNum", P_NationalNum);
                    sc.Parameters.AddWithValue("@P_DOB", P_DOB == null ? (object)DBNull.Value : P_DOB);
                    sc.Parameters.AddWithValue("@P_MaritalState", P_MaritalState == -1 ? (object)DBNull.Value : P_MaritalState);
                    sc.Parameters.AddWithValue("@P_Parish", P_Parish == -1 ? (object)DBNull.Value : P_Parish);
                    sc.Parameters.AddWithValue("@P_IntermidiaryName", P_IntermidiaryName);
                    sc.Parameters.AddWithValue("@P_Priest", P_Priest == -1 ? (object)DBNull.Value : P_Priest);
                    sc.Parameters.AddWithValue("@P_Priest2", P_Priest2 == -1 ? (object)DBNull.Value : P_Priest2);
                    sc.Parameters.AddWithValue("@P_Mobile", P_Mobile);
                    sc.Parameters.AddWithValue("@P_HomeTel", P_HomeTel);
                    sc.Parameters.AddWithValue("@P_Mobile2", P_Mobile2);
                    sc.Parameters.AddWithValue("@Street", Street == -1 ? (object)DBNull.Value : Street);
                    sc.Parameters.AddWithValue("@P_Address", P_Address);
                    sc.Parameters.AddWithValue("@P_HomeType", P_HomeType == -1 ? (object)DBNull.Value : P_HomeType);
                    sc.Parameters.AddWithValue("@P_HomeStateNote", P_HomeStateNote);
                    sc.Parameters.AddWithValue("@Military", Military);
                    if (Military_textBox.Visible)
                    {
                        sc.Parameters.AddWithValue("@Military_Note", Military_Note);
                    }
                    else
                    {
                        sc.Parameters.AddWithValue("@Military_Note", "");

                    }
                    if (Military_Date_checkBox.Visible && Military_Date_checkBox.Checked)
                    {
                        sc.Parameters.AddWithValue("@Military_Date", Military_Date);
                    }
                    else
                    {
                        sc.Parameters.AddWithValue("@Military_Date", DBNull.Value);
                    }
                }
                #endregion

                #region Work
                string P_WorkYears = "";

                for (int i = 0; i <= Work_dataGridView.Rows.Count - 2; i++)
                {
                    if (Work_dataGridView.Rows[i].Visible)
                    {
                        if (Work_dataGridView.Rows[i].Cells["W_Name"].Value == null || Work_dataGridView.Rows[i].Cells["W_Name"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال المهنة في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }
                        else if (Work_dataGridView.Rows[i].Cells["W_WorkType"].Value == null || Work_dataGridView.Rows[i].Cells["W_WorkType"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال طبيعة العمل في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }
                        else if (Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value == null || Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال تاريخ البداية في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }
                        else if (Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value == null || Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال ملكية العقار في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }
                        else if (Work_dataGridView.Rows[i].Cells["W_Place"].Value == null || Work_dataGridView.Rows[i].Cells["W_Place"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال العنوان في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }
                        else if (Work_dataGridView.Rows[i].Cells["W_Salary"].Value == null || Work_dataGridView.Rows[i].Cells["W_Salary"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الربح/الراتب في جميع الأسطر في قسم الخبرات العملية الحالية", "Error");
                            return;
                        }

                    }
                }
                if (ApplicationState == "project")
                {
                    for (int i = 0; i < Exp_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Exp_dataGridView.Rows[i].Visible)
                        {
                            if (Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال المهنة في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            else if (Exp_dataGridView.Rows[i].Cells["Exp_WorkType"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_WorkType"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال طبيعة العمل في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            else if (Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال تاريخ البداية في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            else if (Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال البيانات تاريخ النهاية في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            if (Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال املكية العقار في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            else if (Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال العنوان في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                            else if (Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value == null || Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال سبب التوقف في جميع الأسطر في قسم الخبرات العملية السابقة", "Error");
                                return;
                            }
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(P_WorkYears_textBox.Text))
                {
                    MessageBox.Show("لا يمكن ترك عدد سنوات الخبرة فارغة في قسم الخبرات العملية", "Error");
                    return;
                }


                if (IsDirty_DataGrid[Work_dataGridView])
                {
                    for (int i = 0; i <= Work_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Work_dataGridView.Rows[i].Cells[IsDirty_Columns[Work_dataGridView]].Value != null && (string)Work_dataGridView.Rows[i].Cells[IsDirty_Columns[Work_dataGridView]].Value == "1")
                        {
                            string W_Name = Work_dataGridView.Rows[i].Cells["W_Name"].Value?.ToString() ?? "";
                            string W_WorkType = Work_dataGridView.Rows[i].Cells["W_WorkType"].Value?.ToString() ?? "";
                            string W_Description = Work_dataGridView.Rows[i].Cells["W_Description"].Value?.ToString() ?? "";
                            string W_BeginDate = Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value?.ToString() ?? "";
                            string W_PlaceState = Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value?.ToString() ?? "";
                            string W_Place = Work_dataGridView.Rows[i].Cells["W_Place"].Value?.ToString() ?? "";
                            string W_Contact = Work_dataGridView.Rows[i].Cells["W_Contact"].Value?.ToString() ?? "";
                            string W_Salary = Work_dataGridView.Rows[i].Cells["W_Salary"].Value?.ToString() ?? "";

                            sc.Parameters.AddWithValue("@W_Name" + i, string.IsNullOrEmpty(W_Name) ? (object)DBNull.Value : W_Name);
                            sc.Parameters.AddWithValue("@W_WorkType" + i, string.IsNullOrEmpty(W_WorkType) ? (object)DBNull.Value : W_WorkType);
                            sc.Parameters.AddWithValue("@W_Description" + i, W_Description);
                            sc.Parameters.AddWithValue("@W_BeginDate" + i, W_BeginDate);
                            sc.Parameters.AddWithValue("@W_PlaceState" + i, W_PlaceState);
                            sc.Parameters.AddWithValue("@W_Place" + i, W_Place);
                            sc.Parameters.AddWithValue("@W_Contact" + i, W_Contact);
                            sc.Parameters.AddWithValue("@W_Salary" + i, W_Salary);

                            if (Work_dataGridView.Rows[i].Cells[ID_Columns[Work_dataGridView]].Value != null)
                            {
                                string WP_ID = Work_dataGridView.Rows[i].Cells[ID_Columns[Work_dataGridView]].Value.ToString();
                                sc.Parameters.AddWithValue("@WP_ID" + i, WP_ID);

                                if (Work_dataGridView.Rows[i].Visible == false)
                                    Query += "DELETE FROM person_workexp WHERE PW_ID = @WP_ID" + i + ";";
                                else
                                {
                                    Query += "UPDATE person_workexp set Work_ID = @W_Name" + i + "," +
                                  " W_WorkType_ID = @W_WorkType" + i + "," +
                                  " W_Description = @W_Description" + i + "," +
                                  " W_BeginDate = @W_BeginDate" + i + "," +
                                  " W_PlaceState = @W_PlaceState" + i + "," +
                                  " W_Place = @W_Place" + i + "," +
                                  " W_Contact = @W_Contact" + i + "," +
                                  " W_Salary = @W_Salary" + i + "" +
                                  " where PW_ID = @WP_ID" + i + ";";
                                }
                            }
                            else
                            {
                                Query += "INSERT INTO person_workexp (Person_ID , Work_ID, W_WorkType_ID, W_Description, W_BeginDate, W_PlaceState, W_Place, W_Contact, W_Salary, W_Status, MicroProject_ID)" +
                                    " VALUES (@Person_ID, @W_Name" + i + ", @W_WorkType" + i + ", @W_Description" + i + ", @W_BeginDate" + i + "," +
                                    " @W_PlaceState" + i + ", @W_Place" + i + "," +
                                    " @W_Contact" + i + ", @W_Salary" + i + ", 'Yes', @MicroProject_ID);";
                            }
                        }
                    }
                }

                if (ApplicationState == "project")
                {
                    if (IsDirty_DataGrid[Exp_dataGridView])
                    {
                        for (int i = 0; i < Exp_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Exp_dataGridView.Rows[i].Cells[IsDirty_Columns[Exp_dataGridView]].Value != null && (string)Exp_dataGridView.Rows[i].Cells[IsDirty_Columns[Exp_dataGridView]].Value == "1")
                            {
                                string Exp_Name = Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value?.ToString() ?? "";
                                string Exp_WorkType = Exp_dataGridView.Rows[i].Cells["Exp_WorkType"].Value?.ToString() ?? "";
                                string Exp_Description = Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value?.ToString() ?? "";
                                string Exp_BeginDate = Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value?.ToString() ?? "";
                                string Exp_EndDate = Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value?.ToString() ?? "";
                                string Exp_PlaceState = Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value?.ToString() ?? "";
                                string Exp_Place = Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value?.ToString() ?? "";
                                string Exp_Contact = Exp_dataGridView.Rows[i].Cells["Exp_Contact"].Value?.ToString() ?? "";
                                string Exp_CauseOfLose = Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@Exp_Name" + i, string.IsNullOrEmpty(Exp_Name) ? (object)DBNull.Value : Exp_Name);
                                sc.Parameters.AddWithValue("@Exp_WorkType" + i, string.IsNullOrEmpty(Exp_WorkType) ? (object)DBNull.Value : Exp_WorkType);
                                sc.Parameters.AddWithValue("@Exp_Description" + i, Exp_Description);
                                sc.Parameters.AddWithValue("@Exp_BeginDate" + i, Exp_BeginDate);
                                sc.Parameters.AddWithValue("@Exp_EndDate" + i, Exp_EndDate);
                                sc.Parameters.AddWithValue("@Exp_PlaceState" + i, Exp_PlaceState);
                                sc.Parameters.AddWithValue("@Exp_Place" + i, Exp_Place);
                                sc.Parameters.AddWithValue("@Exp_Contact" + i, Exp_Contact);
                                sc.Parameters.AddWithValue("@Exp_CauseOfLose" + i, Exp_CauseOfLose);

                                if (Exp_dataGridView.Rows[i].Cells[ID_Columns[Exp_dataGridView]].Value != null)
                                {
                                    string ExpP_ID = Exp_dataGridView.Rows[i].Cells[ID_Columns[Exp_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@ExpP_ID" + i, ExpP_ID);

                                    if (Exp_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_workexp WHERE PW_ID = @ExpP_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_workexp set Work_ID = @Exp_Name" + i + "," +
                                                                       " W_WorkType_ID = @Exp_WorkType" + i + "," +
                                                                       " W_Description = @Exp_Description" + i + "," +
                                                                       " W_BeginDate = @Exp_BeginDate" + i + "," +
                                                                       " W_EndDate = @Exp_EndDate" + i + "," +
                                                                       " W_PlaceState = @Exp_PlaceState" + i + "," +
                                                                       " W_Place = @Exp_Place" + i + "," +
                                                                       " W_Contact = @Exp_Contact" + i + "," +
                                                                       " W_CauseOfLoseDescription = @Exp_CauseOfLose" + i + "" +
                                                                       " where PW_ID = @ExpP_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_workexp (Person_ID , Work_ID, W_WorkType_ID, W_Description, W_BeginDate, W_EndDate, W_PlaceState, W_Place, W_Contact, W_CauseOfLoseDescription, W_Status, MicroProject_ID)" +
                                        " VALUES (@Person_ID, @Exp_Name" + i + ", @Exp_WorkType" + i + ",@Exp_Description" + i + ", @Exp_BeginDate" + i + ", @Exp_EndDate" + i + "," +
                                        " @Exp_PlaceState" + i + ", @Exp_Place" + i + "," +
                                        " @Exp_Contact" + i + ", @Exp_CauseOfLose" + i + ", 'NO', @MicroProject_ID);";
                                }
                            }
                        }
                    }
                }

                if (IsDirty_Data["Work_ISDirty"])
                {
                    P_WorkYears = P_WorkYears_textBox.Text;

                    Query += " Update person_microproject set P_WorkYears = @P_WorkYears where Person_ID = @Person_ID and MicroProject_ID = @MicroProject_ID;";

                    sc.Parameters.AddWithValue("@P_WorkYears", P_WorkYears);

                }
                #endregion

                #region Edu
                int HasEducation = -1, HasCourses = -1;

                if (ApplicationState == "project")
                {
                    if (!P_HasEducationYes_radioButton.Checked && !P_HasEducationNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد متلعم/غير متعلم في قسم التحصيل العلمي", "Error");
                        return;
                    }
                    if (P_HasEducationYes_radioButton.Checked)
                    {
                        int visibleRowCount = Education_dataGridView.Rows
                              .Cast<DataGridViewRow>()
                              .Count(row => row.Visible && !row.IsNewRow);

                        if (visibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول التحصيل العلمي فارغاً في حال مستفيد متعلم", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= Education_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Education_dataGridView.Rows[i].Visible)
                        {
                            if (Education_dataGridView.Rows[i].Cells["E_Level"].Value == null || Education_dataGridView.Rows[i].Cells["E_Level"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال التحصيل العلمي في جميع الأسطر في قسم التحصيل العلمي", "Error");
                                return;
                            }
                            else if (Education_dataGridView.Rows[i].Cells["E_Type"].Value == null || Education_dataGridView.Rows[i].Cells["E_Type"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الدرجة العلمية في جميع الأسطر في قسم التحصيل العلمي", "Error");
                                return;
                            }
                            else if (Education_dataGridView.Rows[i].Cells["E_Name"].Value == null || Education_dataGridView.Rows[i].Cells["E_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الاختصاص في جميع الأسطر في قسم التحصيل العلمي", "Error");
                                return;
                            }
                        }
                    }


                    if (!P_HasCoursesYes_radioButton.Checked && !P_HasCoursesNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم الدورات المتبعة", "Error");
                        return;
                    }
                    if (P_HasCoursesYes_radioButton.Checked)
                    {
                        int visibleRowCount = Course_dataGridView.Rows
                                .Cast<DataGridViewRow>()
                                .Count(row => row.Visible && !row.IsNewRow);
                        if (visibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول الدورات المتبعة فارغاً في حال يوجد دورات", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= Course_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Course_dataGridView.Rows[i].Visible)
                        {
                            if (Course_dataGridView.Rows[i].Cells["C_Cource"].Value == null || Course_dataGridView.Rows[i].Cells["C_Cource"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال نوع الدورة في جميع الأسطر في قسم الدورات المتبعة", "Error");
                                return;
                            }
                            else if (Course_dataGridView.Rows[i].Cells["C_Name"].Value == null || Course_dataGridView.Rows[i].Cells["C_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال اسم الدورة في جميع الأسطر في قسم الدورات المتبعة", "Error");
                                return;
                            }
                            else if (Course_dataGridView.Rows[i].Cells["C_Date"].Value == null || Course_dataGridView.Rows[i].Cells["C_Date"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال تاريخ الدورة في جميع الأسطر في قسم الدورات المتبعة", "Error");
                                return;
                            }
                        }
                    }

                    if (IsDirty_Data["Edu_ISDirty"])
                    {
                        if (P_HasEducationYes_radioButton.Checked)
                        {
                            HasEducation = 1;
                        }
                        else if (P_HasEducationNo_radioButton.Checked)
                        {
                            HasEducation = 0;
                        }
                        else
                        {
                            HasEducation = -1;
                        }

                        if (P_HasCoursesYes_radioButton.Checked)
                        {
                            HasCourses = 1;
                        }
                        else if (P_HasCoursesNo_radioButton.Checked)
                        {
                            HasCourses = 0;
                        }
                        else
                        {
                            HasCourses = -1;
                        }
                    }
                    if (IsDirty_DataGrid[Education_dataGridView])
                    {
                        for (int i = 0; i <= Education_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Education_dataGridView.Rows[i].Cells[IsDirty_Columns[Education_dataGridView]].Value != null && (string)Education_dataGridView.Rows[i].Cells[IsDirty_Columns[Education_dataGridView]].Value == "1")
                            {
                                string E_Level = Education_dataGridView.Rows[i].Cells["E_Level"].Value?.ToString() ?? "";
                                string E_Name = Education_dataGridView.Rows[i].Cells["E_Name"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@E_Level" + i, string.IsNullOrEmpty(E_Level) ? (object)DBNull.Value : E_Level);
                                sc.Parameters.AddWithValue("@E_Name" + i, string.IsNullOrEmpty(E_Name) ? (object)DBNull.Value : E_Name);


                                if (Education_dataGridView.Rows[i].Cells[ID_Columns[Education_dataGridView]].Value != null)
                                {
                                    string PE_ID = Education_dataGridView.Rows[i].Cells[ID_Columns[Education_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@PE_ID" + i, PE_ID);

                                    if (Education_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_education WHERE PEdu_ID = @PE_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_education set Education_ID = @E_Name" + i + "," +
                                            " Level_ID = @E_Level" + i + "" +
                                            " where PEdu_ID = @PE_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_education (Person_ID , Education_ID, Level_ID)" +
                                        " VALUES (@Person_ID, @E_Name" + i + ", @E_Level" + i + ");";
                                }
                            }
                        }
                    }
                    if (IsDirty_DataGrid[Course_dataGridView])
                    {
                        for (int i = 0; i <= Course_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Course_dataGridView.Rows[i].Cells[IsDirty_Columns[Course_dataGridView]].Value != null && (string)Course_dataGridView.Rows[i].Cells[IsDirty_Columns[Course_dataGridView]].Value == "1")
                            {
                                string C_Cource = Course_dataGridView.Rows[i].Cells["C_Cource"].Value?.ToString() ?? "";
                                string C_Name = Course_dataGridView.Rows[i].Cells["C_Name"].Value?.ToString() ?? "";
                                string C_Place = Course_dataGridView.Rows[i].Cells["C_Place"].Value?.ToString() ?? "";
                                string C_Donor = Course_dataGridView.Rows[i].Cells["C_Donor"].Value?.ToString() ?? "";
                                string C_Date = Course_dataGridView.Rows[i].Cells["C_Date"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@C_Cource" + i, string.IsNullOrEmpty(C_Cource) ? (object)DBNull.Value : C_Cource);
                                sc.Parameters.AddWithValue("@C_Name" + i, C_Name);
                                sc.Parameters.AddWithValue("@C_Place" + i, C_Place);
                                sc.Parameters.AddWithValue("@C_Donor" + i, C_Donor);
                                sc.Parameters.AddWithValue("@C_Date" + i, C_Date);

                                if (Course_dataGridView.Rows[i].Cells[ID_Columns[Course_dataGridView]].Value != null)
                                {
                                    string CPerosn_ID = Course_dataGridView.Rows[i].Cells[ID_Columns[Course_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@CPerosn_ID" + i, CPerosn_ID);

                                    if (Course_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_course WHERE ID = @CPerosn_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_course set Course_ID = @C_Cource" + i + "," +
                                                      " Name = @C_Name" + i + "," +
                                                      " Place = @C_Place" + i + "," +
                                                      " Donor = @C_Donor" + i + "," +
                                                      " Date = @C_Date" + i + "" +
                                                      " where ID = @CPerosn_ID" + i + ";";
                                    }

                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_course (Person_ID, Course_ID , Name, Place, Donor, Date)" +
                                        " VALUES (@Person_ID, @C_Cource" + i + ", @C_Name" + i + ", @C_Place" + i + ", @C_Donor" + i + ", @C_Date" + i + ");";
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Projrct_Info
                string MP_PlaceAddress = "", MP_PlaceNote = "", AmperNumber = "";
                int MP_PlaceType = -1, IsAmper = -1, Agreement1 = -1, Agreement2 = -1, Agreement3 = -1, Agreement4 = -1;

                if (ApplicationState == "project")
                {
                    if (string.IsNullOrWhiteSpace(MP_PlaceType_comboBox.Text) || MP_PlaceType_comboBox.SelectedIndex < 0)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد ملكية عقار المشروع في قسم المعلومات الخاصة بالمشروع", "Error");
                        return;
                    }

                    for (int i = 0; i <= PowerSource_dataGridView.Rows.Count - 2; i++)
                    {
                        if (PowerSource_dataGridView.Rows[i].Visible)
                        {
                            if (PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value == null || PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال المصدر في جميع الأسطر في قسم مصادر الطاقة في المشروع", "Error");
                                return;
                            }
                        }
                    }

                    if ((!Agreement1_Yes_radioButton.Checked && !Agreement1_No_radioButton.Checked) ||
                        (!Agreement2_Yes_radioButton.Checked && !Agreement2_No_radioButton.Checked) ||
                        (!Agreement3_Yes_radioButton.Checked && !Agreement3_No_radioButton.Checked) ||
                        (!Agreement4_Yes_radioButton.Checked && !Agreement4_No_radioButton.Checked))
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد نعم/لا في جميع الخيارات في قسم الموافقات", "Error");
                        return;
                    }


                    int visibleRowCount = Materials_dataGridView.Rows
                            .Cast<DataGridViewRow>()
                            .Count(row => row.Visible && !row.IsNewRow);
                    if (visibleRowCount == 0)
                    {
                        MessageBox.Show("لا يمكن ترك جدول مقترح الميزانية فارغاً", "Error");
                        return;
                    }
                    for (int i = 0; i <= Materials_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Materials_dataGridView.Rows[i].Visible)
                        {
                            if (Materials_dataGridView.Rows[i].Cells["M_Name"].Value == null || Materials_dataGridView.Rows[i].Cells["M_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال اسم المادة في جميع الأسطر في قسم مقترح الميزانية", "Error");
                                return;
                            }
                            else if (Materials_dataGridView.Rows[i].Cells["M_Price"].Value == null || Materials_dataGridView.Rows[i].Cells["M_Price"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال ثمن القطعة في جميع الأسطر في قسم مقترح الميزانية", "Error");
                                return;
                            }
                            else if (Materials_dataGridView.Rows[i].Cells["M_Amount"].Value == null || Materials_dataGridView.Rows[i].Cells["M_Amount"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الكمية في جميع الأسطر في قسم مقترح الميزانية", "Error");
                                return;
                            }
                        }
                    }

                    if (IsDirty_Data["Projrct_Info_ISDirty"])
                    {
                        MP_PlaceType = int.Parse(MP_PlaceType_comboBox.SelectedValue.ToString());
                        MP_PlaceAddress = MP_PlaceAddress_textBox.Text;
                        MP_PlaceNote = MP_PlaceNote_textBox.Text;
                        AmperNumber = AmperNumber_textBox.Text;

                        if (IsAmper_radioButton.Checked)
                        {
                            IsAmper = 1;
                        }
                        else if (IsWatt_radioButton.Checked)
                        {
                            IsAmper = 0;
                        }
                        else
                        {
                            IsAmper = -1;
                        }

                        if (Agreement1_Yes_radioButton.Checked)
                        {
                            Agreement1 = 1;
                        }
                        else if (Agreement1_No_radioButton.Checked)
                        {
                            Agreement1 = 0;
                        }
                        else
                        {
                            Agreement1 = -1;
                        }

                        if (Agreement2_Yes_radioButton.Checked)
                        {
                            Agreement2 = 1;
                        }
                        else if (Agreement2_No_radioButton.Checked)
                        {
                            Agreement2 = 0;
                        }
                        else
                        {
                            Agreement2 = -1;
                        }

                        if (Agreement3_Yes_radioButton.Checked)
                        {
                            Agreement3 = 1;
                        }
                        else if (Agreement3_No_radioButton.Checked)
                        {
                            Agreement3 = 0;
                        }
                        else
                        {
                            Agreement3 = -1;
                        }

                        if (Agreement4_Yes_radioButton.Checked)
                        {
                            Agreement4 = 1;
                        }
                        else if (Agreement4_No_radioButton.Checked)
                        {
                            Agreement4 = 0;
                        }
                        else
                        {
                            Agreement4 = -1;
                        }
                    }

                    if (IsDirty_DataGrid[PowerSource_dataGridView])
                    {
                        for (int i = 0; i <= PowerSource_dataGridView.Rows.Count - 2; i++)
                        {
                            if (PowerSource_dataGridView.Rows[i].Cells[IsDirty_Columns[PowerSource_dataGridView]].Value != null && (string)PowerSource_dataGridView.Rows[i].Cells[IsDirty_Columns[PowerSource_dataGridView]].Value == "1")
                            {
                                string Power_Name = PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value?.ToString() ?? "";
                                string Power_Note = PowerSource_dataGridView.Rows[i].Cells["Power_Note"].Value?.ToString() ?? "";
                                string Power_Cost = PowerSource_dataGridView.Rows[i].Cells["Power_Cost"].Value?.ToString() ?? "";
                                string Power_Responsability = PowerSource_dataGridView.Rows[i].Cells["Power_Responsability"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@Power_Name" + i, string.IsNullOrEmpty(Power_Name) ? (object)DBNull.Value : Power_Name);
                                sc.Parameters.AddWithValue("@Power_Note" + i, Power_Note);
                                sc.Parameters.AddWithValue("@Power_Cost" + i, Power_Cost);
                                sc.Parameters.AddWithValue("@Power_Responsability" + i, Power_Responsability);

                                if (PowerSource_dataGridView.Rows[i].Cells[ID_Columns[PowerSource_dataGridView]].Value != null)
                                {
                                    string Power_MP_ID = PowerSource_dataGridView.Rows[i].Cells[ID_Columns[PowerSource_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@Power_MP_ID" + i, Power_MP_ID);

                                    if (PowerSource_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM microproject_powersource WHERE ID = @Power_MP_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE microproject_powersource set PowerSource_ID = @Power_Name" + i + "," +
                                            " Note = @Power_Note" + i + "," +
                                            " Cost = @Power_Cost" + i + "," +
                                            " Responsability = @Power_Responsability" + i + "" +
                                            " where ID = @Power_MP_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO microproject_powersource (MicroProject_ID, PowerSource_ID , Note, Cost, Responsability)" +
                                        " VALUES (@MicroProject_ID, @Power_Name" + i + ", @Power_Note" + i + ", @Power_Cost" + i + ", @Power_Responsability" + i + ");";
                                }
                            }
                        }
                    }

                    if (IsDirty_DataGrid[Materials_dataGridView])
                    {
                        for (int i = 0; i <= Materials_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Materials_dataGridView.Rows[i].Cells[IsDirty_Columns[Materials_dataGridView]].Value != null && (string)Materials_dataGridView.Rows[i].Cells[IsDirty_Columns[Materials_dataGridView]].Value == "1")
                            {
                                string M_Name = Materials_dataGridView.Rows[i].Cells["M_Name"].Value?.ToString() ?? "";
                                string M_Comments = Materials_dataGridView.Rows[i].Cells["M_Comments"].Value?.ToString() ?? "";
                                string M_Price = Materials_dataGridView.Rows[i].Cells["M_Price"].Value?.ToString() ?? "";
                                string M_Amount = Materials_dataGridView.Rows[i].Cells["M_Amount"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@M_Name" + i, M_Name);
                                sc.Parameters.AddWithValue("@M_Comments" + i, M_Comments);
                                sc.Parameters.AddWithValue("@M_Price" + i, M_Price);
                                sc.Parameters.AddWithValue("@M_Amount" + i, M_Amount);

                                if (Materials_dataGridView.Rows[i].Cells[ID_Columns[Materials_dataGridView]].Value != null)
                                {
                                    string MMP_ID = Materials_dataGridView.Rows[i].Cells[ID_Columns[Materials_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@MMP_ID" + i, MMP_ID);

                                    if (Materials_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM material WHERE ID = @MMP_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE material set Name = @M_Name" + i + "," +
                                            " Amount = @M_Amount" + i + "," +
                                            " Price = @M_Price" + i + "," +
                                            " Comments = @M_Comments" + i + "" +
                                            " where ID = @MMP_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO material (MicroProject_ID, Name , Amount, Price, Comments)" +
                                        " VALUES (@MicroProject_ID, @M_Name" + i + ", @M_Amount" + i + ", @M_Price" + i + ", @M_Comments" + i + ");";
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Family
                string FamilyNum = "", LiveWithFamily_Note = "", P_AidSourceNote = "", P_DebtsAmount = "", P_DebtsNote = "";
                int LiveWithFamily = -1, HasLosses = -1, HasIllness = -1, HasAsset = -1, HasAidSource = -1, HasDebts = -1;

                if (string.IsNullOrWhiteSpace(FamilyNum_textBox1.Text))
                {
                    MessageBox.Show("لا يمكن ترك رقم دفتر العائلة فارغاً في قسم معلومات عائلة المستفيد", "Error");
                    return;
                }
                else if (!FamilyNum_Checked)
                {
                    bool CheckFamilyNum = false;
                    if (Family_ID <= 0)
                        CheckFamilyNum = true;
                    else
                    {
                        if (FamilyNum_ISDirty)
                        {
                            CheckFamilyNum = true;
                        }
                    }

                    if (CheckFamilyNum)
                    {
                        UserMode = false;
                        DataTable dt = Check_Family_Num(FamilyNum_textBox1.Text);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int Old_P_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                                int Old_MP_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                                var Old_Name = dt.Rows[0]["الاسم الثلاثي"].ToString();
                                string Old_ApplicationState = dt.Rows[0]["ApplicationState"].ToString();

                                var _MessageBox = new Application_MessageBox("Family_Num", Old_P_ID, Old_MP_ID, Old_Name);
                                var r = _MessageBox.ShowDialog();
                                if (r == DialogResult.Yes)
                                {
                                    Form Appplication_New_Form = new Application_New_Form(mainForm, Old_MP_ID, Old_P_ID, Old_ApplicationState);
                                    mainForm.showNewTab(Appplication_New_Form, "Application", 0);
                                    FamilyNum_textBox1.Focus();
                                    return;
                                }
                                else if (r == DialogResult.No)
                                {
                                    Existed_Family_ID = int.Parse(dt.Rows[0]["Family_ID"].ToString());
                                }
                                else
                                {
                                    FamilyNum_textBox1.Text = F_Num;
                                    FamilyNum_textBox1.Focus();
                                    return;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            FamilyNum_textBox1.Text = F_Num;
                            FamilyNum_textBox1.Focus();
                            return;
                        }

                        UserMode = true;
                    }
                }

                if (!P_LiveWithFamily_radioButton.Checked && !P_NotLiveWithFamily_radioButton.Checked)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد هل تقيم مع عائلة أخرى في قسم معلومات عائلة المستفيد", "Error");
                    return;
                }

                for (int i = 0; i <= FamilyMember_dataGridView.Rows.Count - 2; i++)
                {
                    if (FamilyMember_dataGridView.Rows[i].Visible)
                    {
                        if (FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الاسم في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        else if (FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الكنية في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        else if (FamilyMember_dataGridView.Rows[i].Cells["P_FatherName"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_FatherName"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال اسم الأب في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        else if (FamilyMember_dataGridView.Rows[i].Cells["P_Relation"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_Relation"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الصفة في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        else if (FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال المواليد في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        else if (FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال حالة الإقامة في جميع الأسطر في قسم أفراد العائلة", "Error");
                            return;
                        }
                        if (ApplicationState == "project")
                        {
                            if (FamilyMember_dataGridView.Rows[i].Cells["P_NationalNumber"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_NationalNumber"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الرقم الوطني في جميع الأسطر في قسم أفراد العائلة", "Error");
                                return;
                            }
                            else if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال التحصيل العلمي في جميع الأسطر في قسم أفراد العائلة", "Error");
                                return;
                            }
                            else if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationType"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_EducationType"].Value.ToString() == "")
                            {
                                if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value != null && FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString() != "")
                                {
                                    if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value != null && FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString() != "")
                                    {
                                        int EducationLevel_ID = Int32.Parse(FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString());

                                        string EducationLevel_Name = ds.Tables[25].AsEnumerable()
                                                         .Where(row => row.Field<int>("ID").Equals(EducationLevel_ID) == true)
                                                         .Select(row => row.Field<string>("Name"))
                                                         .FirstOrDefault();

                                        if (EducationLevel_Name != "غير متعلم")
                                        {
                                            MessageBox.Show("الرجاء التأكد من إدخال الدرجة العلمية في جميع الأسطر التي تحوي تحصيل علمي في قسم أفراد العائلة", "Error");
                                            return;
                                        }
                                    }
                                }
                            }
                            else if (FamilyMember_dataGridView.Rows[i].Cells["P_E_Name"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_E_Name"].Value.ToString() == "")
                            {
                                if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value != null && FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString() != "")
                                {
                                    if (FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value != null && FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString() != "")
                                    {
                                        int EducationLevel_ID = Int32.Parse(FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value.ToString());

                                        string EducationLevel_Name = ds.Tables[25].AsEnumerable()
                                                         .Where(row => row.Field<int>("ID").Equals(EducationLevel_ID) == true)
                                                         .Select(row => row.Field<string>("Name"))
                                                         .FirstOrDefault();

                                        if (EducationLevel_Name != "غير متعلم")
                                        {
                                            MessageBox.Show("الرجاء التأكد من إدخال الاختصاص في جميع الأسطر التي تحوي تحصيل علمي في قسم أفراد العائلة", "Error");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ApplicationState == "project")
                {
                    if (!P_HasLossesYes_radioButton.Checked && !P_HasLossesNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم الخسارات", "Error");
                        return;
                    }
                    if (P_HasLossesYes_radioButton.Checked)
                    {
                        int visibleRowCount = Loss_dataGridView.Rows
                                .Cast<DataGridViewRow>()
                                .Count(row => row.Visible && !row.IsNewRow);
                        if (visibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول الخسارت فارغاً في حال يوجد خسارات", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= Loss_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Loss_dataGridView.Rows[i].Visible)
                        {
                            if (Loss_dataGridView.Rows[i].Cells["L_Type"].Value == null || Loss_dataGridView.Rows[i].Cells["L_Type"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال نوع الخسارة في جميع الأسطر في قسم الخسارات", "Error");
                                return;
                            }
                            else if (Loss_dataGridView.Rows[i].Cells["L_Year"].Value == null || Loss_dataGridView.Rows[i].Cells["L_Year"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال سنة الواقعة في جميع الأسطر في قسم الخسارات", "Error");
                                return;
                            }
                        }

                    }

                    if (!P_HasIllnessYes_radioButton.Checked && !P_HasIllnessNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم الحالات المرضية", "Error");
                        return;
                    }
                    if (P_HasIllnessYes_radioButton.Checked)
                    {
                        int visibleRowCount = Illness_dataGridView.Rows
                           .Cast<DataGridViewRow>()
                           .Count(row => row.Visible && !row.IsNewRow);
                        if (visibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول الحالات المرضية فارغاً في حال يوجد حالات مرضية", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= Illness_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Illness_dataGridView.Rows[i].Visible)
                        {
                            if (Illness_dataGridView.Rows[i].Cells["Ill_PName"].Value == null || Illness_dataGridView.Rows[i].Cells["Ill_PName"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الاسم في جميع الأسطر في قسم الحالات المرضية", "Error");
                                return;
                            }
                            if (Illness_dataGridView.Rows[i].Cells["Ill_Name"].Value == null || Illness_dataGridView.Rows[i].Cells["Ill_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الحالة الصحية في جميع الأسطر في قسم الحالات المرضية", "Error");
                                return;
                            }
                            else if ((Illness_dataGridView.Rows[i].Cells["Ill_Aid"].Value != null && (bool)Illness_dataGridView.Rows[i].Cells["Ill_Aid"].Value == true) && (Illness_dataGridView.Rows[i].Cells["Ill_Donor"].Value == null || Illness_dataGridView.Rows[i].Cells["Ill_Donor"].Value.ToString() == ""))
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الجهة المساعدة في جميع الأسطر التي تستقبل مساعدات طبية في قسم الحالات المرضية", "Error");
                                return;
                            }
                        }
                    }

                    if (!P_HasAssetsYes_radioButton.Checked && !P_HasAssetsNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم أملاك المستفيد", "Error");
                        return;
                    }
                    if (P_HasAssetsYes_radioButton.Checked)
                    {
                        int visibleRowCount = Assets_dataGridView.Rows
                                .Cast<DataGridViewRow>()
                                .Count(row => row.Visible && !row.IsNewRow);
                        if (visibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول الممتلكات فارغاً في حال يوجد ممتلكات", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= Assets_dataGridView.Rows.Count - 2; i++)
                    {
                        if (Assets_dataGridView.Rows[i].Visible)
                        {
                            if (Assets_dataGridView.Rows[i].Cells["Asset_PName"].Value == null || Assets_dataGridView.Rows[i].Cells["Asset_PName"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الاسم في جميع الأسطر في قسم أملاك المستفيد", "Error");
                                return;
                            }
                            if (Assets_dataGridView.Rows[i].Cells["Asset_Name"].Value == null || Assets_dataGridView.Rows[i].Cells["Asset_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الملك في جميع الأسطر في قسم أملاك المستفيد", "Error");
                                return;
                            }
                            else if (Assets_dataGridView.Rows[i].Cells["Asset_type"].Value == null || Assets_dataGridView.Rows[i].Cells["Asset_type"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال نوع الملك في جميع الأسطر في قسم أملاك المستفيد", "Error");
                                return;
                            }
                            else if (Assets_dataGridView.Rows[i].Cells["Asset_Year"].Value == null || Assets_dataGridView.Rows[i].Cells["Asset_Year"].Value.ToString() == "")
                            {
                                if (Assets_dataGridView.Rows[i].Cells["Asset_type"].Value != null && Assets_dataGridView.Rows[i].Cells["Asset_type"].Value.ToString() != "")
                                {
                                    int Asset_type_ID = Int32.Parse(Assets_dataGridView.Rows[i].Cells["Asset_type"].Value.ToString());

                                    string Asset_type_Name = ds.Tables[8].AsEnumerable()
                                                     .Where(row => row.Field<int>("ID").Equals(Asset_type_ID) == true)
                                                     .Select(row => row.Field<string>("Name"))
                                                     .FirstOrDefault();

                                    if (Asset_type_Name == "ملك")
                                    {
                                        MessageBox.Show("الرجاء التأكد من إدخال سنة الشراء في جميع الأسطر التي تحوي ملك في قسم أملاك المستفيد", "Error");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!P_HasAidSourceYes_radioButton.Checked && !P_HasAidSourceNo_radioButton.Checked)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم الاستفادة من جهات أخرى", "Error");
                    return;
                }
                if (P_HasAidSourceYes_radioButton.Checked)
                {
                    int visibleRowCount = AidSource_dataGridView.Rows
                            .Cast<DataGridViewRow>()
                            .Count(row => row.Visible && !row.IsNewRow);
                    if (visibleRowCount == 0)
                    {
                        MessageBox.Show("لا يمكن ترك جدول الاستفادة من جهات أخرى فارغاً في حال يوجد استفادة", "Error");
                        return;
                    }
                }
                for (int i = 0; i <= AidSource_dataGridView.Rows.Count - 2; i++)
                {
                    if (AidSource_dataGridView.Rows[i].Visible)
                    {

                        if (AidSource_dataGridView.Rows[i].Cells["Aid_PName"].Value == null || AidSource_dataGridView.Rows[i].Cells["Aid_PName"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الاسم في جميع الأسطر في قسم الاستفادة من جهات أخرى", "Error");
                            return;
                        }

                        if (AidSource_dataGridView.Rows[i].Cells["Aid_Source"].Value == null || AidSource_dataGridView.Rows[i].Cells["Aid_Source"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال الجهة في جميع الأسطر في قسم الاستفادة من جهات أخرى", "Error");
                            return;
                        }
                        else if (AidSource_dataGridView.Rows[i].Cells["Aid_DateYear"].Value == null || AidSource_dataGridView.Rows[i].Cells["Aid_DateYear"].Value.ToString() == "")
                        {
                            MessageBox.Show("الرجاء التأكد من إدخال تاريخ الاستفادة في جميع الأسطر في قسم الاستفادة من جهات أخرى", "Error");
                            return;
                        }
                    }

                }

                if (ApplicationState == "project")
                {
                    if (!P_HasDebtsYes_radioButton.Checked && !P_HasDebtsNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد نعم/لا في قسم هل لديك ديون", "Error");
                        return;
                    }
                }

                if (IsDirty_Data["Family_ISDirty"])
                {
                    FamilyNum = FamilyNum_textBox1.Text;

                    if (P_LiveWithFamily_radioButton.Checked)
                    {
                        LiveWithFamily = 1;
                    }
                    else if (P_NotLiveWithFamily_radioButton.Checked)
                    {
                        LiveWithFamily = 0;
                    }
                    else
                    {
                        LiveWithFamily = -1;
                    }

                    LiveWithFamily_Note = LiveWithFamily_Note_textBox.Text;

                    if (ApplicationState == "project")
                    {
                        if (P_HasLossesYes_radioButton.Checked)
                        {
                            HasLosses = 1;
                        }
                        else if (P_HasLossesNo_radioButton.Checked)
                        {
                            HasLosses = 0;
                        }
                        else
                        {
                            HasLosses = -1;
                        }

                        if (P_HasIllnessYes_radioButton.Checked)
                        {
                            HasIllness = 1;
                        }
                        else if (P_HasIllnessNo_radioButton.Checked)
                        {
                            HasIllness = 0;
                        }
                        else
                        {
                            HasIllness = -1;
                        }

                        if (P_HasAssetsYes_radioButton.Checked)
                        {
                            HasAsset = 1;
                        }
                        else if (P_HasAssetsNo_radioButton.Checked)
                        {
                            HasAsset = 0;
                        }
                        else
                        {
                            HasAsset = -1;
                        }
                    }

                    if (P_HasAidSourceYes_radioButton.Checked)
                    {
                        HasAidSource = 1;
                    }
                    else if (P_HasAidSourceNo_radioButton.Checked)
                    {
                        HasAidSource = 0;
                    }
                    else
                    {
                        HasAidSource = -1;
                    }

                    P_AidSourceNote = P_AidSourceNote_textBox.Text;

                    if (P_HasDebtsYes_radioButton.Checked)
                    {
                        HasDebts = 1;
                    }
                    else if (P_HasDebtsNo_radioButton.Checked)
                    {
                        HasDebts = 0;
                    }
                    else
                    {
                        HasDebts = -1;
                    }

                    P_DebtsAmount = P_DebtsAmount_textBox.Text;
                    P_DebtsNote = P_DebtsNote_textBox.Text;

                    if (Family_ID <= 0)
                    {
                        if (Existed_Family_ID <= 0)
                        {
                            Query += "Insert into family (F_Number) Values (@FamilyNum);";

                            Query += "Set @Family_ID = LAST_INSERT_ID();";

                            Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                                " Values (Concat('Insert the family:',@FamilyNum), 'Family', @Log_User, @Log_Date, '0');";
                        }

                        Query += "Insert into person_family (Family_ID, Person_ID, Relation_ID, P_Provider_ID)" +
                            " Values (@Family_ID, @Person_ID, (select id from relation where name = 'مستفيد'), @Person_ID);";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('insert the person:',@Person_ID, ' to the family:', @Family_ID), 'family', @Log_User, @Log_Date, '0');";

                    }
                    else
                    {
                        if (FamilyNum_ISDirty)
                        {
                            if (Existed_Family_ID <= 0)
                            {
                                Query += "Insert into family (F_Number) Values (@FamilyNum);";

                                Query += "Set @Family_ID = LAST_INSERT_ID();";

                                Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                                    " Values (Concat('Insert the family:',@FamilyNum), 'Family', @Log_User, @Log_Date, '0');";
                            }

                            Query += "Update person_family set Family_ID = @Family_ID where P_Provider_ID = @Person_ID;";

                            Query += "DELETE family FROM family LEFT JOIN person_family ON family.F_ID = person_family.Family_ID WHERE person_family.Family_ID is null;";
                        }
                    }


                    Query += "Update person_microproject set P_HasAidSource = @HasAidSource, P_AidSourceNote = @MP_OtherDonors," +
                        " P_IsLivingWithFamily = @LiveWithFamily, P_LiveWithFamily_Note = @LiveWithFamily_Note, P_AidSourceNote = @AidSourceNote" +
                        " Where Person_ID = @Person_ID and MicroProject_ID = @MicroProject_ID;";


                    sc.Parameters.AddWithValue("@FamilyNum", string.IsNullOrEmpty(FamilyNum) ? (object)DBNull.Value : FamilyNum);
                    sc.Parameters.AddWithValue("@LiveWithFamily", LiveWithFamily);
                    if (P_LiveWithFamily_radioButton.Checked)
                    {
                        sc.Parameters.AddWithValue("@LiveWithFamily_Note", LiveWithFamily_Note);
                    }
                    else
                    {
                        sc.Parameters.AddWithValue("@LiveWithFamily_Note", DBNull.Value);
                    }

                    sc.Parameters.AddWithValue("@HasAidSource", HasAidSource);
                    sc.Parameters.AddWithValue("@AidSourceNote", P_AidSourceNote);
                }

                if (IsDirty_DataGrid[FamilyMember_dataGridView])
                {
                    for (int i = 0; i <= FamilyMember_dataGridView.Rows.Count - 2; i++)
                    {
                        if (FamilyMember_dataGridView.Rows[i].Cells[IsDirty_Columns[FamilyMember_dataGridView]].Value != null && (string)FamilyMember_dataGridView.Rows[i].Cells[IsDirty_Columns[FamilyMember_dataGridView]].Value == "1")
                        {
                            string FP_FirstName = FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value?.ToString() ?? "";
                            string FP_LastName = FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value?.ToString() ?? "";
                            string FP_FatherName = FamilyMember_dataGridView.Rows[i].Cells["P_FatherName"].Value?.ToString() ?? "";
                            string FP_Relation = FamilyMember_dataGridView.Rows[i].Cells["P_Relation"].Value?.ToString() ?? "";
                            string FP_NationalNumber = FamilyMember_dataGridView.Rows[i].Cells["P_NationalNumber"].Value?.ToString() ?? "";
                            DateTime FP_DOB = Convert.ToDateTime(DateTime.ParseExact(FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value?.ToString() ?? "", "yyyy", CultureInfo.InvariantCulture));
                            string FP_State = FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value?.ToString() ?? "";
                            string FP_HasWork = "";
                            if (FamilyMember_dataGridView.Rows[i].Cells["P_HasWork"].Value == null || FamilyMember_dataGridView.Rows[i].Cells["P_HasWork"].Value.ToString() == "False")
                            {
                                FP_HasWork = "0";
                            }
                            else if (FamilyMember_dataGridView.Rows[i].Cells["P_HasWork"].Value.ToString() == "True")
                            {
                                FP_HasWork = "1";
                            }
                            string FP_EducationLevel = FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value?.ToString() ?? "";
                            string FP_EducationType = FamilyMember_dataGridView.Rows[i].Cells["P_EducationType"].Value?.ToString() ?? "";
                            string FP_E_Name = FamilyMember_dataGridView.Rows[i].Cells["P_E_Name"].Value?.ToString() ?? "";

                            sc.Parameters.AddWithValue("@FP_FirstName" + i, FP_FirstName);
                            sc.Parameters.AddWithValue("@FP_LastName" + i, FP_LastName);
                            sc.Parameters.AddWithValue("@FP_FatherName" + i, FP_FatherName);
                            sc.Parameters.AddWithValue("@FP_Relation" + i, string.IsNullOrEmpty(FP_Relation) ? (object)DBNull.Value : FP_Relation);
                            sc.Parameters.AddWithValue("@FP_NationalNumber" + i, FP_NationalNumber);
                            sc.Parameters.AddWithValue("@FP_DOB" + i, FP_DOB == null ? (object)DBNull.Value : FP_DOB);
                            sc.Parameters.AddWithValue("@FP_State" + i, string.IsNullOrEmpty(FP_State) ? (object)DBNull.Value : FP_State);
                            sc.Parameters.AddWithValue("@FP_HasWork" + i, FP_HasWork);
                            sc.Parameters.AddWithValue("@FP_EducationLevel" + i, string.IsNullOrEmpty(FP_EducationLevel) ? (object)DBNull.Value : FP_EducationLevel);
                            sc.Parameters.AddWithValue("@FP_E_Name" + i, string.IsNullOrEmpty(FP_E_Name) ? (object)DBNull.Value : FP_E_Name);


                            if (FamilyMember_dataGridView.Rows[i].Cells[ID_Columns[FamilyMember_dataGridView]].Value != null)
                            {
                                string PF_ID = FamilyMember_dataGridView.Rows[i].Cells[ID_Columns[FamilyMember_dataGridView]].Value.ToString();
                                sc.Parameters.AddWithValue("@PF_ID" + i, PF_ID);

                                string FPerson_ID = FamilyMember_dataGridView.Rows[i].Cells["FPerson_ID"].Value.ToString();
                                sc.Parameters.AddWithValue("@FPerson_ID" + i, FPerson_ID);

                                if (FamilyMember_dataGridView.Rows[i].Visible == false)
                                {
                                    Query += "DELETE FROM person WHERE P_ID = @FPerson_ID" + i + ";";

                                    Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                                        " Values (Concat('Delete the person:',@FPerson_ID" + i + ", ' from the family:', @Family_ID), 'Person Family', @Log_User, @Log_Date, '0');";
                                }
                                else
                                {
                                    Query += "Update person set P_FirstName = @FP_FirstName" + i + ", P_LastName = @FP_LastName" + i + "," +
                                        "P_FatherName = @FP_FatherName" + i + ", P_NationalNumber = @FP_NationalNumber" + i + ", P_DOB = @FP_DOB" + i + ",P_State_ID = @FP_State" + i + "" +
                                            " Where P_ID = @FPerson_ID" + i + ";";

                                    Query += "Update person_family set Relation_ID = @FP_Relation" + i + ", EducationLevel_ID = @FP_EducationLevel" + i + ", Education_ID = @FP_E_Name" + i + ", HasWork =  @FP_HasWork" + i + "" +
                                        " Where PF_ID = @PF_ID" + i + ";";
                                }
                            }
                            else
                            {
                                Query += "INSERT INTO person (P_FirstName, P_LastName, P_FatherName, P_NationalNumber, P_DOB, P_State_ID, IsProjectOwner)" +
                                    " VALUES (@FP_FirstName" + i + ", @FP_LastName" + i + ", @FP_FatherName" + i + ", @FP_NationalNumber" + i + ", @FP_DOB" + i + ", @FP_State" + i + ", 'No');";

                                Query += "Set @FPerson_ID" + i + " =  LAST_INSERT_ID();";

                                Query += "INSERT INTO person_family (Family_ID, Person_ID, Relation_ID, P_Provider_ID, EducationLevel_ID, Education_ID, HasWork)" +
                                    " VALUES (@Family_ID, @FPerson_ID" + i + ", @FP_Relation" + i + ", @Person_ID,@FP_EducationLevel" + i + ", @FP_E_Name" + i + ", @FP_HasWork" + i + ");";

                                Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                                    " Values (Concat('insert the person:',@FPerson_ID" + i + ", ' from the family:', @Family_ID), 'Person Family', @Log_User, @Log_Date, '0');";
                            }
                        }
                    }

                    Query += " Update person set" +
                        " P_NumOfMembers = (SELECT COUNT(person_family.PF_ID) FROM person_family where person_family.P_Provider_ID = @Person_ID)," +
                        " P_NumAtHome = (SELECT COUNT(person_family.PF_ID)" +
                        " FROM person_family LEFT JOIN person on person.P_ID = person_family.Person_ID" +
                        " LEFT JOIN personstate on personstate.ID = person.P_State_ID" +
                        " where personstate.Type = 'Active' and person_family.P_Provider_ID =  @Person_ID)" +
                        " where person.P_ID = @Person_ID;";
                }

                if (ApplicationState == "project")
                {
                    if (IsDirty_DataGrid[Loss_dataGridView])
                    {
                        for (int i = 0; i <= Loss_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Loss_dataGridView.Rows[i].Cells[IsDirty_Columns[Loss_dataGridView]].Value != null && (string)Loss_dataGridView.Rows[i].Cells[IsDirty_Columns[Loss_dataGridView]].Value == "1")
                            {
                                string L_Type = Loss_dataGridView.Rows[i].Cells["L_Type"].Value?.ToString() ?? "";
                                string L_Year = Loss_dataGridView.Rows[i].Cells["L_Year"].Value?.ToString() ?? "";
                                string L_Notes = Loss_dataGridView.Rows[i].Cells["L_Notes"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@L_Type" + i, string.IsNullOrEmpty(L_Type) ? (object)DBNull.Value : L_Type);
                                sc.Parameters.AddWithValue("@L_Year" + i, L_Year);
                                sc.Parameters.AddWithValue("@L_Notes" + i, L_Notes);

                                if (Loss_dataGridView.Rows[i].Cells[ID_Columns[Loss_dataGridView]].Value != null)
                                {
                                    string LPerson_ID = Loss_dataGridView.Rows[i].Cells[ID_Columns[Loss_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@LPerson_ID" + i, LPerson_ID);

                                    if (Loss_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_loss WHERE ID = @LPerson_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_loss set Loss_ID = @L_Type" + i + "," +
                                            " Year  = @L_Year" + i + "," +
                                            " Note = @L_Notes" + i + "" +
                                            " where ID = @LPerson_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_loss (Person_ID, Loss_ID, Year , Note)" +
                                        " VALUES (@Person_ID, @L_Type" + i + ", @L_Year" + i + ", @L_Notes" + i + ");";
                                }
                            }
                        }
                    }

                    if (IsDirty_DataGrid[Illness_dataGridView])
                    {
                        for (int i = 0; i <= Illness_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Illness_dataGridView.Rows[i].Cells[IsDirty_Columns[Illness_dataGridView]].Value != null && (string)Illness_dataGridView.Rows[i].Cells[IsDirty_Columns[Illness_dataGridView]].Value == "1")
                            {
                                string Ill_PName = Illness_dataGridView.Rows[i].Cells["Ill_PName"].Value?.ToString() ?? "";
                                string Ill_Name = Illness_dataGridView.Rows[i].Cells["Ill_Name"].Value?.ToString() ?? "";
                                string Ill_Note = Illness_dataGridView.Rows[i].Cells["Ill_Note"].Value?.ToString() ?? "";
                                string Ill_Aid = Illness_dataGridView.Rows[i].Cells["Ill_Aid"].Value?.ToString() ?? "";
                                string Ill_Donor = Illness_dataGridView.Rows[i].Cells["Ill_Donor"].Value?.ToString() ?? "";
                                string Ill_AidPeriod = Illness_dataGridView.Rows[i].Cells["Ill_AidPeriod"].Value?.ToString() ?? "";
                                string Ill_MedicalNeeds = Illness_dataGridView.Rows[i].Cells["Ill_MedicalNeeds"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@Ill_PName" + i, string.IsNullOrEmpty(Ill_PName) ? (object)DBNull.Value : Ill_PName);
                                sc.Parameters.AddWithValue("@Ill_Name" + i, string.IsNullOrEmpty(Ill_Name) ? (object)DBNull.Value : Ill_Name);
                                sc.Parameters.AddWithValue("@Ill_Note" + i, Ill_Note);
                                sc.Parameters.AddWithValue("@Ill_Aid" + i, Ill_Aid);
                                sc.Parameters.AddWithValue("@Ill_Donor" + i, string.IsNullOrEmpty(Ill_Donor) ? (object)DBNull.Value : Ill_Donor);
                                sc.Parameters.AddWithValue("@Ill_AidPeriod" + i, string.IsNullOrEmpty(Ill_AidPeriod) ? (object)DBNull.Value : Ill_AidPeriod);
                                sc.Parameters.AddWithValue("@Ill_MedicalNeeds" + i, string.IsNullOrEmpty(Ill_MedicalNeeds) ? (object)DBNull.Value : Ill_MedicalNeeds);



                                if (Illness_dataGridView.Rows[i].Cells[ID_Columns[Illness_dataGridView]].Value != null)
                                {
                                    string Ill_Person_ID = Illness_dataGridView.Rows[i].Cells[ID_Columns[Illness_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@Ill_Person_ID" + i, Ill_Person_ID);


                                    if (Illness_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_illness WHERE ID = @Ill_Person_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_illness set Person_ID = @Ill_PName" + i + "," +
                                            " Illness_ID  = @Ill_Name" + i + "," +
                                            " Has_MedicalAid = @Ill_Aid" + i + "," +
                                            " Donor = @Ill_Donor" + i + "," +
                                            " Note = @Ill_Note" + i + "," +
                                            " AidPeriod_ID = @Ill_AidPeriod" + i + "," +
                                            " MedicalNeeds_ID  = @Ill_MedicalNeeds" + i + "" +
                                            " where ID = @Ill_Person_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_illness (Person_ID, Illness_ID, Has_MedicalAid , Donor, Note, AidPeriod_ID, MedicalNeeds_ID)" +
                                        " VALUES (@Ill_PName" + i + ", @Ill_Name" + i + ", @Ill_Aid" + i + ", @Ill_Donor" + i + ", @Ill_Note" + i + ", @Ill_AidPeriod" + i + ", @Ill_MedicalNeeds" + i + ");";
                                }
                            }
                        }
                    }

                    if (IsDirty_DataGrid[Assets_dataGridView])
                    {
                        for (int i = 0; i <= Assets_dataGridView.Rows.Count - 2; i++)
                        {
                            if (Assets_dataGridView.Rows[i].Cells[IsDirty_Columns[Assets_dataGridView]].Value != null && (string)Assets_dataGridView.Rows[i].Cells[IsDirty_Columns[Assets_dataGridView]].Value == "1")
                            {
                                string Asset_PName = Assets_dataGridView.Rows[i].Cells["Asset_PName"].Value?.ToString() ?? "";
                                string Asset_Name = Assets_dataGridView.Rows[i].Cells["Asset_Name"].Value?.ToString() ?? "";
                                string Asset_type = Assets_dataGridView.Rows[i].Cells["Asset_type"].Value?.ToString() ?? "";
                                string Asset_Year = Assets_dataGridView.Rows[i].Cells["Asset_Year"].Value?.ToString() ?? "";
                                string Asset_Desc = Assets_dataGridView.Rows[i].Cells["Asset_Desc"].Value?.ToString() ?? "";
                                string Asset_Note = Assets_dataGridView.Rows[i].Cells["Asset_Note"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@Asset_PName" + i, string.IsNullOrEmpty(Asset_PName) ? (object)DBNull.Value : Asset_PName);
                                sc.Parameters.AddWithValue("@Asset_Name" + i, string.IsNullOrEmpty(Asset_Name) ? (object)DBNull.Value : Asset_Name);
                                sc.Parameters.AddWithValue("@Asset_type" + i, string.IsNullOrEmpty(Asset_type) ? (object)DBNull.Value : Asset_type);
                                sc.Parameters.AddWithValue("@Asset_Year" + i, Asset_Year);
                                sc.Parameters.AddWithValue("@Asset_Desc" + i, Asset_Desc);
                                sc.Parameters.AddWithValue("@Asset_Note" + i, Asset_Note);

                                if (Assets_dataGridView.Rows[i].Cells[ID_Columns[Assets_dataGridView]].Value != null)
                                {
                                    string APerson_ID = Assets_dataGridView.Rows[i].Cells[ID_Columns[Assets_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@APerson_ID" + i, APerson_ID);

                                    if (Assets_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_asset WHERE ID = @APerson_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_asset set Person_ID  = @Asset_PName" + i + "," +
                                            " Asset_ID  = @Asset_Name" + i + "," +
                                            " type_ID  = @Asset_type" + i + "," +
                                            " Year  = @Asset_Year" + i + "," +
                                            " Description = @Asset_Desc" + i + "," +
                                            " Note = @Asset_Note" + i + "" +
                                            " where ID = @APerson_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_asset (Person_ID, Asset_ID, type_ID, Year , Description, Note)" +
                                        " VALUES (@Asset_PName" + i + ", @Asset_Name" + i + ", @Asset_type" + i + ", @Asset_Year" + i + ", @Asset_Desc" + i + ", @Asset_Note" + i + ");";
                                }
                            }
                        }
                    }
                }

                if (IsDirty_DataGrid[AidSource_dataGridView])
                {
                    for (int i = 0; i <= AidSource_dataGridView.Rows.Count - 2; i++)
                    {
                        if (AidSource_dataGridView.Rows[i].Cells[IsDirty_Columns[AidSource_dataGridView]].Value != null && (string)AidSource_dataGridView.Rows[i].Cells[IsDirty_Columns[AidSource_dataGridView]].Value == "1")
                        {
                            string Aid_PName = AidSource_dataGridView.Rows[i].Cells["Aid_PName"].Value?.ToString() ?? "";
                            string Aid_Source = AidSource_dataGridView.Rows[i].Cells["Aid_Source"].Value?.ToString() ?? "";
                            string Aid_DateYear = AidSource_dataGridView.Rows[i].Cells["Aid_DateYear"].Value?.ToString() ?? "";
                            string Aid_Amount = AidSource_dataGridView.Rows[i].Cells["Aid_Amount"].Value?.ToString() ?? "";
                            string Aid_ProjectType = AidSource_dataGridView.Rows[i].Cells["Aid_ProjectType"].Value?.ToString() ?? "";
                            string Aid_Notes = AidSource_dataGridView.Rows[i].Cells["Aid_Notes"].Value?.ToString() ?? "";


                            sc.Parameters.AddWithValue("@Aid_PName" + i, string.IsNullOrEmpty(Aid_PName) ? (object)DBNull.Value : Aid_PName);
                            sc.Parameters.AddWithValue("@Aid_Source" + i, string.IsNullOrEmpty(Aid_Source) ? (object)DBNull.Value : Aid_Source);
                            sc.Parameters.AddWithValue("@Aid_DateYear" + i, Aid_DateYear);
                            sc.Parameters.AddWithValue("@Aid_Amount" + i, Aid_Amount);
                            sc.Parameters.AddWithValue("@Aid_ProjectType" + i, Aid_ProjectType);
                            sc.Parameters.AddWithValue("@Aid_Notes" + i, Aid_Notes);

                            if (AidSource_dataGridView.Rows[i].Cells[ID_Columns[AidSource_dataGridView]].Value != null)
                            {
                                string AidP_ID = AidSource_dataGridView.Rows[i].Cells[ID_Columns[AidSource_dataGridView]].Value.ToString();
                                sc.Parameters.AddWithValue("@AidP_ID" + i, AidP_ID);

                                if (AidSource_dataGridView.Rows[i].Visible == false)
                                    Query += "DELETE FROM person_aidsource WHERE ID = @AidP_ID" + i + ";";
                                else
                                {
                                    if (Convert.ToInt32(Aid_PName) > 0)
                                    {
                                        Query += "UPDATE person_aidsource set Person_ID  = @Aid_PName" + i + "," +
                                            " MicroProject_ID= @MicroProject_ID, " +
                                           " AidSource_ID  = @Aid_Source" + i + "," +
                                           " DateYear  = @Aid_DateYear" + i + "," +
                                           " Amount = @Aid_Amount" + i + "," +
                                           " ProjectType = @Aid_ProjectType" + i + "," +
                                           " Notes = @Aid_Notes" + i + "" +
                                           " where ID = @AidP_ID" + i + ";";
                                    }
                                    else
                                    {
                                        string PName = Members_dt.AsEnumerable()
                                                   .Where(dtrow => dtrow.Field<int>("ID") == Convert.ToInt32(Aid_PName))
                                                   .Select(dtrow => dtrow.Field<string>("Name"))
                                                   .FirstOrDefault();

                                        sc.Parameters.AddWithValue("@PName" + i, PName);


                                        Query += "UPDATE person_aidsource set Person_ID  = (select P_ID from person where CONCAT(P_FirstName,' ',P_LastName) = @PName" + i + ")," +
                                            " MicroProject_ID= @MicroProject_ID," +
                                            " AidSource_ID  = @Aid_Source" + i + "," +
                                            " DateYear  = @Aid_DateYear" + i + "," +
                                            " Amount = @Aid_Amount" + i + "," +
                                            " ProjectType = @Aid_ProjectType" + i + "," +
                                            " Notes = @Aid_Notes" + i + "" +
                                            " where ID = @AidP_ID" + i + ";";

                                    }

                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(Aid_PName) > 0)
                                {
                                    Query += "INSERT INTO person_aidsource (Person_ID, MicroProject_ID, AidSource_ID, DateYear , Amount,ProjectType, Notes)" +
                                        " VALUES (@Aid_PName" + i + ", @MicroProject_ID, @Aid_Source" + i + ", @Aid_DateYear" + i + ", @Aid_Amount" + i + ", @Aid_ProjectType" + i + ", @Aid_Notes" + i + ");";
                                }
                                else
                                {
                                    string PName = Members_dt.AsEnumerable()
                                      .Where(dtrow => dtrow.Field<int>("ID") == Convert.ToInt32(Aid_PName))
                                      .Select(dtrow => dtrow.Field<string>("Name"))
                                      .FirstOrDefault();

                                    sc.Parameters.AddWithValue("@PName" + i, PName);


                                    Query += "INSERT INTO person_aidsource (Person_ID, MicroProject_ID, AidSource_ID, DateYear , Amount,ProjectType, Notes)" +
                                        " VALUES ((select P_ID from person where CONCAT(P_FirstName,' ',P_LastName) = @PName" + i + "), @MicroProject_ID, @Aid_Source" + i + ", @Aid_DateYear" + i + ", @Aid_Amount" + i + ", @Aid_ProjectType" + i + ", @Aid_Notes" + i + ");";

                                }
                                //if (ApplicationState == "project")
                                //{
                                //    Query += "INSERT INTO person_aidsource (Person_ID, AidSource_ID, DateYear , Amount,ProjectType, Notes)" +
                                //        " VALUES (@Aid_PName" + i + ", @Aid_Source" + i + ", @Aid_DateYear" + i + ", @Aid_Amount" + i + ", @Aid_ProjectType" + i + ", @Aid_Notes" + i + ");";
                                //}
                                //else
                                //{
                                //    Query += "INSERT INTO person_aidsource (Person_ID, AidSource_ID, DateYear , Amount,ProjectType, Notes)" +
                                //        " VALUES (@Person_ID, @Aid_Source" + i + ", @Aid_DateYear" + i + ", @Aid_Amount" + i + ", @Aid_ProjectType" + i + ", @Aid_Notes" + i + ");";
                                //}

                            }
                        }
                    }
                }
                #endregion

                #region AditionalInfo
                string MP_SimpleProfit2 = "";
                int HasHelpSource = -1, PaperApplicationUser = -1;

                if (ApplicationState == "project")
                {
                    if (!P_HasHelpSourceYes_radioButton.Checked && !P_HasHelpSourceNo_radioButton.Checked)
                    {
                        MessageBox.Show("الرجاء التأكد من تحديد يوجد/لا يوجد في قسم مصادر المساعدة", "Error");
                        return;
                    }
                    if (P_HasHelpSourceYes_radioButton.Checked)
                    {
                        int HelpSourcevisibleRowCount = HelpSource_dataGridView.Rows
                                    .Cast<DataGridViewRow>()
                                    .Count(row => row.Visible && !row.IsNewRow);
                        if (HelpSourcevisibleRowCount == 0)
                        {
                            MessageBox.Show("لا يمكن ترك جدول مصادر المساعدة فارغاً في حال يوجد مصادر مساعدة", "Error");
                            return;
                        }
                    }
                    for (int i = 0; i <= HelpSource_dataGridView.Rows.Count - 2; i++)
                    {
                        if (HelpSource_dataGridView.Rows[i].Visible)
                        {
                            if (HelpSource_dataGridView.Rows[i].Cells["Help_Source"].Value == null || HelpSource_dataGridView.Rows[i].Cells["Help_Source"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الجهة في جميع الأسطر في قسم مصادر المساعدة", "Error");
                                return;
                            }
                            else if (HelpSource_dataGridView.Rows[i].Cells["Help_Name"].Value == null || HelpSource_dataGridView.Rows[i].Cells["Help_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال نوع المساعدة في جميع الأسطر في قسم مصادر المساعدة", "Error");
                                return;
                            }
                            else if (HelpSource_dataGridView.Rows[i].Cells["Help_Period"].Value == null || HelpSource_dataGridView.Rows[i].Cells["Help_Period"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال مدة المساعدة في جميع الأسطر في قسم مصادر المساعدة", "Error");
                                return;
                            }
                        }
                    }

                    int visibleRowCount = HomePowerSource_dataGridView.Rows
                            .Cast<DataGridViewRow>()
                            .Count(row => row.Visible && !row.IsNewRow);
                    if (visibleRowCount == 0)
                    {
                        MessageBox.Show("لا يمكن ترك جدول مصادر الطاقة في المنزل فارغاً", "Error");
                        return;
                    }
                    for (int i = 0; i <= HomePowerSource_dataGridView.Rows.Count - 2; i++)
                    {
                        if (HomePowerSource_dataGridView.Rows[i].Visible)
                        {
                            if (HomePowerSource_dataGridView.Rows[i].Cells["HPower_Name"].Value == null || HomePowerSource_dataGridView.Rows[i].Cells["HPower_Name"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال مصدر الطاقة في جميع الأسطر في قسم مصادر الطاقة في المنزل", "Error");
                                return;
                            }
                            if (HomePowerSource_dataGridView.Rows[i].Cells["HPower_Responsability"].Value == null || HomePowerSource_dataGridView.Rows[i].Cells["HPower_Responsability"].Value.ToString() == "")
                            {
                                MessageBox.Show("الرجاء التأكد من إدخال الجهة المنفذة في جميع الأسطر في قسم مصادر الطاقة في المنزل", "Error");
                                return;
                            }
                        }
                    }

                }

                if (string.IsNullOrWhiteSpace(PaperApplicationUser_comboBox.Text) || PaperApplicationUser_comboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("الرجاء التأكد من تحديد مسجل الطلب ورقياً", "Error");
                    return;
                }

                if (IsDirty_Data["AditionalInfo_ISDirty"])
                {
                    if (ApplicationState == "project")
                    {
                        if (P_HasHelpSourceYes_radioButton.Checked)
                        {
                            HasHelpSource = 1;
                        }
                        else if (P_HasHelpSourceNo_radioButton.Checked)
                        {
                            HasHelpSource = 0;
                        }
                        else
                        {
                            HasHelpSource = -1;
                        }
                    }


                    MP_SimpleProfit2 = MP_SimpleProfit2_textBox.Text;

                    PaperApplicationUser = int.Parse(PaperApplicationUser_comboBox.SelectedValue.ToString());

                    Query += " Update microproject set MP_Profit_Description = @MP_SimpleProfit2, MP_PaperUser_ID = @PaperApplicationUser Where MP_ID = @MicroProject_ID;";

                    sc.Parameters.AddWithValue("@MP_SimpleProfit2", MP_SimpleProfit2);
                    sc.Parameters.AddWithValue("@PaperApplicationUser", PaperApplicationUser == -1 ? (object)DBNull.Value : PaperApplicationUser);
                }

                if (ApplicationState == "project")
                {
                    if (IsDirty_DataGrid[HelpSource_dataGridView])
                    {
                        for (int i = 0; i <= HelpSource_dataGridView.Rows.Count - 2; i++)
                        {
                            if (HelpSource_dataGridView.Rows[i].Cells[IsDirty_Columns[HelpSource_dataGridView]].Value != null && (string)HelpSource_dataGridView.Rows[i].Cells[IsDirty_Columns[HelpSource_dataGridView]].Value == "1")
                            {
                                string Help_Source = HelpSource_dataGridView.Rows[i].Cells["Help_Source"].Value?.ToString() ?? "";
                                string Help_Name = HelpSource_dataGridView.Rows[i].Cells["Help_Name"].Value?.ToString() ?? "";
                                string Help_Period = HelpSource_dataGridView.Rows[i].Cells["Help_Period"].Value?.ToString() ?? "";
                                string Help_Note = HelpSource_dataGridView.Rows[i].Cells["Help_Note"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@Help_Source" + i, string.IsNullOrEmpty(Help_Source) ? (object)DBNull.Value : Help_Source);
                                sc.Parameters.AddWithValue("@Help_Name" + i, string.IsNullOrEmpty(Help_Name) ? (object)DBNull.Value : Help_Name);
                                sc.Parameters.AddWithValue("@Help_Period" + i, string.IsNullOrEmpty(Help_Period) ? (object)DBNull.Value : Help_Period);
                                sc.Parameters.AddWithValue("@Help_Note" + i, Help_Note);

                                if (HelpSource_dataGridView.Rows[i].Cells[ID_Columns[HelpSource_dataGridView]].Value != null)
                                {
                                    string HelpP_ID = HelpSource_dataGridView.Rows[i].Cells[ID_Columns[HelpSource_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@HelpP_ID" + i, HelpP_ID);

                                    if (HelpSource_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_help WHERE ID = @HelpP_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_help set AidSource_ID = @Help_Source" + i + "," +
                                            " Aid_ID  = @Help_Name" + i + "," +
                                            " Aidperiod_ID  = @Help_Period" + i + "," +
                                            " Notes  = @Help_Note" + i + "" +
                                            " where ID = @HelpP_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_help (Person_ID, AidSource_ID, Aid_ID , Aidperiod_ID, Notes)" +
                                        " VALUES (@Person_ID, @Help_Source" + i + ", @Help_Name" + i + ", @Help_Period" + i + ", @Help_Note" + i + ");";
                                }
                            }
                        }
                    }

                    if (IsDirty_DataGrid[HomePowerSource_dataGridView])
                    {
                        for (int i = 0; i <= HomePowerSource_dataGridView.Rows.Count - 2; i++)
                        {
                            if (HomePowerSource_dataGridView.Rows[i].Cells[IsDirty_Columns[HomePowerSource_dataGridView]].Value != null && (string)HomePowerSource_dataGridView.Rows[i].Cells[IsDirty_Columns[HomePowerSource_dataGridView]].Value == "1")
                            {
                                string HPower_Name = HomePowerSource_dataGridView.Rows[i].Cells["HPower_Name"].Value?.ToString() ?? "";
                                string HPower_Note = HomePowerSource_dataGridView.Rows[i].Cells["HPower_Note"].Value?.ToString() ?? "";
                                string HPower_Cost = HomePowerSource_dataGridView.Rows[i].Cells["HPower_Cost"].Value?.ToString() ?? "";
                                string HPower_Responsability = HomePowerSource_dataGridView.Rows[i].Cells["HPower_Responsability"].Value?.ToString() ?? "";

                                sc.Parameters.AddWithValue("@HPower_Name" + i, string.IsNullOrEmpty(HPower_Name) ? (object)DBNull.Value : HPower_Name);
                                sc.Parameters.AddWithValue("@HPower_Note" + i, HPower_Note);
                                sc.Parameters.AddWithValue("@HPower_Cost" + i, HPower_Cost);
                                sc.Parameters.AddWithValue("@HPower_Responsability" + i, HPower_Responsability);

                                if (HomePowerSource_dataGridView.Rows[i].Cells[ID_Columns[HomePowerSource_dataGridView]].Value != null)
                                {
                                    string HPowerP_ID = HomePowerSource_dataGridView.Rows[i].Cells[ID_Columns[HomePowerSource_dataGridView]].Value.ToString();
                                    sc.Parameters.AddWithValue("@HPowerP_ID" + i, HPowerP_ID);

                                    if (HomePowerSource_dataGridView.Rows[i].Visible == false)
                                        dataGrid_Query += "DELETE FROM person_powersource WHERE ID = @HPowerP_ID" + i + ";";
                                    else
                                    {
                                        dataGrid_Query += "UPDATE person_powersource set PowerSource_ID = @HPower_Name" + i + "," +
                                            " Note  = @HPower_Note" + i + "," +
                                            " Cost  = @HPower_Cost" + i + "," +
                                            " Responsability  = @HPower_Responsability" + i + "" +
                                            " where ID = @HPowerP_ID" + i + ";";
                                    }
                                }
                                else
                                {
                                    dataGrid_Query += "INSERT INTO person_powersource (Person_ID, PowerSource_ID, Note , Cost, Responsability)" +
                                        " VALUES (@Person_ID, @HPower_Name" + i + ", @HPower_Note" + i + ", @HPower_Cost" + i + ", @HPower_Responsability" + i + ");";
                                }
                            }
                        }
                    }
                }

                #endregion


                //Query += " update microproject_state set Current_State = 0 where MicroProject_ID = @MicroProject_ID;";

                //Query += " insert into microproject_state (MicroProject_ID, State_ID, Date, Current_State)" +
                //    " Values (@MicroProject_ID, (select state.id from state where state.Name_en = 'Waiting'), NOW(),1);";

                Query += " Update microproject set MP_EditedByUser_ID = @ProgramUser_ID where MP_ID = @MicroProject_ID;";
            }


            Query += "SELECT @MicroProject_ID AS `MicroProject_ID`, @Person_ID AS `Person_ID`, @Family_ID AS `Family_ID`;";
            Query += " COMMIT;";

            if (ApplicationState == "pre_project")
            {
                ApplicationState = "zero";
                sc.Parameters.AddWithValue("@ApplicationState", ApplicationState);
            }
            else if (ApplicationState == "zero")
            {
                ApplicationState = "zero"; //Change to project
                sc.Parameters.AddWithValue("@ApplicationState", ApplicationState);
            }
            else if (ApplicationState == "project")
            {
                ApplicationState = "project";
                sc.Parameters.AddWithValue("@ApplicationState", ApplicationState);
            }

            Program.buildConnection();
            sc.CommandTimeout = 120;
            sc.CommandText = "SET SESSION wait_timeout = @t;" + Query;
            sc.Parameters.AddWithValue("@t", 600);
            sc.Connection = Program.MyConn;

            using (MySqlDataReader reader = sc.ExecuteReader())
            {
                while (reader.HasRows == false && reader.NextResult()) { }

                if (reader.Read())
                {
                    MicroProject_ID = reader.IsDBNull(reader.GetOrdinal("MicroProject_ID")) ? -1 : reader.GetInt32("MicroProject_ID");
                    Person_ID = reader.IsDBNull(reader.GetOrdinal("Person_ID")) ? -1 : reader.GetInt32("Person_ID");
                    Family_ID = reader.IsDBNull(reader.GetOrdinal("Family_ID")) ? -1 : reader.GetInt32("Family_ID");
                }
            }
            Program.MyConn.Close();


            MessageBox.Show("تم الحفظ بنجاح");

            // Suspend redraw
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);

            Clear_Boxes();
            Set_Controls();
            Bind_Persons_Data(false);
            Fill_Application_Data();

            UserMode = false;
            // Resume redraw and refresh everything
            SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            this.ResumeLayout(true);
            this.PerformLayout();
            this.Invalidate(true);
            this.Update();

            // Optional: Scroll into view or to top
            this.ScrollControlIntoView(P_FullName_textBox); // or another key control
            P_FullName_textBox.Focus();                     // bring it into view
            this.AutoScrollPosition = new Point(0, 0); // scroll to top (if applicable)
            UserMode = true;

        }

        private void Fill_Application_Data()
        {
            UserMode = false;

            DataSet Application_ds = App_Q.Select_New_Application(MicroProject_ID, Person_ID);

            if (Application_ds.Tables[0] != null && Application_ds.Tables[0].Rows.Count > 0)
            {
                MicroProject_ID = Application_ds.Tables[0].Rows[0].Field<int>("MicroProject_ID");

                MPID_textBox.Text = Application_ds.Tables[0].Rows[0].Field<int?>("MicroProject_ID")?.ToString() ?? "";
                SubCategory_comboBox.SelectedValue = Application_ds.Tables[0].Rows[0].Field<int?>("SubCategory_ID") ?? -1;
                Career_comboBox.SelectedValue = Application_ds.Tables[0].Rows[0].Field<int?>("Career_ID").ToString();
                if (Application_ds.Tables[0].Rows[0]["Date Of Request"] != DBNull.Value)
                {
                    ApplyDate_bcDateTimePicker.Value = Convert.ToDateTime(Application_ds.Tables[0].Rows[0].Field<DateTime>("Date Of Request").ToString());
                }
                MP_Name_textBox.Text = Application_ds.Tables[0].Rows[0].Field<string>("Project Name");
                Type_comboBox.SelectedValue = Application_ds.Tables[0].Rows[0].Field<int?>("Type_ID") ?? -1;
                SubType_comboBox.SelectedValue = Application_ds.Tables[0].Rows[0].Field<int?>("SubType_ID") ?? -1;
                MP_RequestedAmount_textBox.Text = Application_ds.Tables[0].Rows[0].Field<double?>("Requested Amount")?.ToString() ?? "";
                if (Application_ds.Tables[0].Rows[0]["FundType_ID"] != DBNull.Value)
                {
                    if (Application_ds.Tables[0].Rows[0].Field<int?>("FundType_ID") == 1)
                    {
                        Loan_radioButton.Checked = true;
                    }
                    else if (Application_ds.Tables[0].Rows[0].Field<int?>("FundType_ID") == 0)
                    {
                        Grant_radioButton.Checked = true;
                    }
                }

                MP_SimpleProfit2_textBox.Text = Application_ds.Tables[0].Rows[0].Field<string>("Profit_Description");
                PaperApplicationUser_comboBox.SelectedValue = Application_ds.Tables[0].Rows[0].Field<int?>("PaperUser_ID") ?? -1;

                CreatedBy_User_label.Text = Application_ds.Tables[0].Rows[0].Field<string>("ProgramUser");
                EditedBy_User_label.Text = Application_ds.Tables[0].Rows[0].Field<string>("EditedByUser");
            }

            if (ApplicationState == "zero" || ApplicationState == "project")
            {
                if (Application_ds.Tables[1] != null && Application_ds.Tables[1].Rows.Count > 0)
                {
                    Person_ID = Application_ds.Tables[1].Rows[0].Field<int>("P_ID");
                    P_Num = Application_ds.Tables[1].Rows[0].Field<string>("P_NationalNumber");


                    P_FullName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("FullName");
                    P_MobileReadable_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_Mobile");


                    P_FirstName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_FirstName");
                    P_LastName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_LastName");
                    P_Sex_comboBox.SelectedItem = Application_ds.Tables[1].Rows[0].Field<string>("P_Sex");
                    PersonType_comboBox.SelectedItem = Application_ds.Tables[1].Rows[0].Field<string>("PersonType");
                    P_FullName_En_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_FullName_en");
                    P_FatherName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_FatherName");
                    P_MotherName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_MotherName");
                    P_Registration_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_RegistrationPlace");
                    P_NationalNum_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_NationalNumber");
                    if (Application_ds.Tables[1].Rows[0]["P_DOB"] != DBNull.Value)
                    {
                        P_DOB_textBox.Text = Application_ds.Tables[1].Rows[0].Field<DateTime>("P_DOB").ToString("yyyy");
                    }
                    P_MaritalState_comboBox.SelectedValue = Application_ds.Tables[1].Rows[0].Field<int?>("P_MaritalStatus_ID") ?? -1;
                    P_Parish_comboBox.SelectedValue = Application_ds.Tables[1].Rows[0].Field<int?>("P_Parish_ID") ?? -1;
                    P_IntermidiaryName_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_IntermidiarySide");
                    P_Mobile_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_Mobile");
                    P_HomeTel_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_HomeTel");
                    Street_comboBox.SelectedValue = Application_ds.Tables[1].Rows[0].Field<int?>("Street_ID") ?? -1;
                    P_Address_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_HomeAddress");
                    P_HomeType_comboBox.SelectedValue = Application_ds.Tables[1].Rows[0].Field<int?>("P_HomeType_ID") ?? -1;
                    P_HomeStateNote_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_HomeState_Note");
                    if (Application_ds.Tables[1].Rows[0]["P_MilitaryService"] != DBNull.Value)
                    {
                        if (Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService") == "وحيد")
                        {
                            MilitarySingle_radioButton.Checked = true;
                        }
                        if (Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService") == "مسرح")
                        {
                            MilitaryFinished_radioButton.Checked = true;
                        }
                        if (Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService") == "عسكري")
                        {
                            MilitaryNow_radioButton.Checked = true;
                        }
                        if (Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService") == "معفى/بدل")
                        {
                            MilitaryExempt_radioButton.Checked = true;
                        }
                        if (Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService") == "مؤجل")
                        {
                            MilitaryDelayed_radioButton.Checked = true;
                        }
                    }

                    Military_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_MilitaryService_Note");

                    if (Application_ds.Tables[1].Rows[0]["P_MilitaryService_Date"] != DBNull.Value)
                    {
                        Military_Date_checkBox.Checked = true;
                        Military_Date_dateTimePicker.Value = Convert.ToDateTime(Application_ds.Tables[1].Rows[0].Field<DateTime>("P_MilitaryService_Date").ToString());
                    }

                    P_WorkYears_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_WorkYears");

                    if (Application_ds.Tables[1].Rows[0]["P_IsLivingWithFamily"] != DBNull.Value)
                    {
                        if (Application_ds.Tables[1].Rows[0].Field<int?>("P_IsLivingWithFamily") == 1)
                        {
                            P_LiveWithFamily_radioButton.Checked = true;
                        }
                        else if (Application_ds.Tables[1].Rows[0].Field<int?>("P_IsLivingWithFamily") == 0)
                        {
                            P_NotLiveWithFamily_radioButton.Checked = true;
                        }
                    }


                    LiveWithFamily_Note_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_LiveWithFamily_Note");

                    if (Application_ds.Tables[1].Rows[0]["P_HasAidSource"] != DBNull.Value)
                    {
                        if (Application_ds.Tables[1].Rows[0].Field<int?>("P_HasAidSource") == 1)
                        {
                            P_HasAidSourceYes_radioButton.Checked = true;
                        }
                        else if (Application_ds.Tables[1].Rows[0].Field<int?>("P_HasAidSource") == 0)
                        {
                            P_HasAidSourceNo_radioButton.Checked = true;
                        }
                    }

                    P_AidSourceNote_textBox.Text = Application_ds.Tables[1].Rows[0].Field<string>("P_AidSourceNote");
                }

                if (Application_ds.Tables[2] != null && Application_ds.Tables[2].Rows.Count > 0)
                {
                    string PicName = "";
                    int Image_ID = -1;
                    if (Application_ds.Tables[2].Rows[0]["image_NewPath"] == DBNull.Value || Application_ds.Tables[2].Rows[0]["image_NewPath"] == null)
                        PicName = "";
                    else
                    {
                        PicName = Application_ds.Tables[2].Rows[0].Field<string>("image_NewPath");
                        Image_ID = Application_ds.Tables[2].Rows[0].Field<int>("Image_ID");
                    }

                    if (PicName != null && PicName.Length > 0 && PicName != "")
                    {
                        string OfflinePicPath = Program.OfflineRoot + @"/" + PicName;
                        string OnlinePicPath = Program.OnlineRoot + @"/" + PicName;

                        bool imageExist_offline = false;
                        bool imageExist_online = false;

                        string Type;
                        if (d.Offline_Exist(PicName))
                        {
                            d.GetNameFromPath(OfflinePicPath, out _, out Type);
                            imageExist_offline = true;
                        }
                        else if (d.Online_Exist(PicName))
                        {
                            d.GetNameFromPath(OnlinePicPath, out _, out Type);
                            imageExist_online = true;
                        }

                        if (imageExist_offline || imageExist_online)
                        {
                            string tempDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\micro_images";
                            System.IO.Directory.CreateDirectory(tempDirectory);

                            //Clean Temp Directory
                            System.IO.DirectoryInfo di = new DirectoryInfo(tempDirectory);
                            foreach (FileInfo allfiles in di.GetFiles())
                            {
                                try
                                {
                                    allfiles.Delete();
                                }
                                catch { }
                            }
                            FileInfo file = new FileInfo(tempDirectory + "\\" + PicName);
                            if (imageExist_online)
                                d.CopyToLocal(OnlinePicPath, tempDirectory + "\\" + PicName, imageExist_online);
                            else
                                d.CopyToLocal(OfflinePicPath, tempDirectory + "\\" + PicName, imageExist_online);

                            PersonPicture_pictureBox.ImageLocation = tempDirectory + "\\" + PicName;
                        }
                        else
                        {
                            if (imageExist_online)
                                d.DeleteOnline(OnlinePicPath);
                            if (imageExist_offline)
                                d.DeleteOffline(OfflinePicPath);

                            img.DeleteImage(Image_ID, MicroProject_ID, PicName);

                            PersonPicture_pictureBox.Image = Resources.Unknown_User;
                            ImageLocation_textBox.Text = "";
                            FullPicName =  "";
                            MessageBox.Show("الملف مفقود - تم حذف الاسم من القائمة");
                        }
                    }
                    else
                    {
                        PersonPicture_pictureBox.Image = Resources.Unknown_User;
                    }
                }

                if (Application_ds.Tables[3] != null && Application_ds.Tables[3].Rows.Count > 0)
                {
                    DataTable All_Works_dt = Application_ds.Tables[3].Copy();

                    DataTable work_dt = new DataTable();

                    DataRow[] work_rows = All_Works_dt.Select("W_Status = 'Yes'");
                    if (work_rows.Length != 0)
                    {
                        work_dt = work_rows.CopyToDataTable();

                        for (int i = 0; i < work_dt.Rows.Count; i++)
                        {
                            Work_dataGridView.Rows.Add();
                            Work_dataGridView.Rows[i].Cells["WP_ID"].Value = work_dt.Rows[i].Field<int?>("PW_ID");
                            Work_dataGridView.Rows[i].Cells["W_Name"].Value = work_dt.Rows[i].Field<int?>("Work_ID");
                            Work_dataGridView.Rows[i].Cells["W_WorkType"].Value = work_dt.Rows[i].Field<int?>("W_WorkType_ID");
                            Work_dataGridView.Rows[i].Cells["W_Description"].Value = work_dt.Rows[i].Field<string>("W_Description");
                            Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value = work_dt.Rows[i].Field<string>("W_BeginDate");
                            Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value = work_dt.Rows[i].Field<string>("W_PlaceState");
                            Work_dataGridView.Rows[i].Cells["W_Place"].Value = work_dt.Rows[i].Field<string>("W_Place");
                            Work_dataGridView.Rows[i].Cells["W_Contact"].Value = work_dt.Rows[i].Field<string>("W_Contact");
                            Work_dataGridView.Rows[i].Cells["W_Salary"].Value = work_dt.Rows[i].Field<double>("W_Salary");
                        }
                    }

                    if (ApplicationState == "project")
                    {
                        DataTable Exp_dt = new DataTable();

                        DataRow[] Exp_rows = All_Works_dt.Select("W_Status = 'No'");
                        if (Exp_rows.Length != 0)
                        {
                            Exp_dt = Exp_rows.CopyToDataTable();

                            for (int i = 0; i < Exp_dt.Rows.Count; i++)
                            {
                                Exp_dataGridView.Rows.Add();
                                Exp_dataGridView.Rows[i].Cells["ExpP_ID"].Value = Exp_dt.Rows[i].Field<int?>("PW_ID");
                                Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value = Exp_dt.Rows[i].Field<int?>("Work_ID");
                                Exp_dataGridView.Rows[i].Cells["Exp_WorkType"].Value = Exp_dt.Rows[i].Field<int?>("W_WorkType_ID");
                                Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value = Exp_dt.Rows[i].Field<string>("W_Description");
                                Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value = Exp_dt.Rows[i].Field<string>("W_BeginDate");
                                Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value = Exp_dt.Rows[i].Field<string>("W_EndDate");
                                Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value = Exp_dt.Rows[i].Field<string>("W_PlaceState");
                                Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value = Exp_dt.Rows[i].Field<string>("W_Place");
                                Exp_dataGridView.Rows[i].Cells["Exp_Contact"].Value = Exp_dt.Rows[i].Field<string>("W_Contact");
                                Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value = Exp_dt.Rows[i].Field<string>("W_CauseOfLoseDescription");
                            }
                        }
                    }


                }

                if (Application_ds.Tables[4] != null && Application_ds.Tables[4].Rows.Count > 0)
                {
                    Family_ID = Application_ds.Tables[4].Rows[0].Field<int>("F_ID");
                    F_Num = Application_ds.Tables[4].Rows[0].Field<string>("F_Number");


                    FamilyNum_textBox1.Text = Application_ds.Tables[4].Rows[0].Field<string>("F_Number");

                    P_NumAtHome_textBox.Text = Application_ds.Tables[4].Rows[0].Field<int>("P_NumAtHome").ToString();
                    P_NumOfMembers_textBox.Text = Application_ds.Tables[4].Rows[0].Field<int>("P_NumOfMembers").ToString();

                    DataTable dt = Application_ds.Tables[4].Copy();
                    DataTable selected_dt = new DataTable();
                    DataRow[] rows = dt.Select("Person_ID <> " + Person_ID + "");
                    if (rows.Length != 0)
                    {
                        selected_dt = rows.CopyToDataTable();
                        if (selected_dt != null && selected_dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < selected_dt.Rows.Count; i++)
                            {
                                FamilyMember_dataGridView.Rows.Add();
                                FamilyMember_dataGridView.Rows[i].Cells["PF_ID"].Value = selected_dt.Rows[i].Field<int?>("PF_ID");
                                FamilyMember_dataGridView.Rows[i].Cells["FPerson_ID"].Value = selected_dt.Rows[i].Field<int?>("Person_ID");
                                FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value = selected_dt.Rows[i].Field<string>("P_FirstName");
                                FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value = selected_dt.Rows[i].Field<string>("P_LastName");
                                FamilyMember_dataGridView.Rows[i].Cells["P_FatherName"].Value = selected_dt.Rows[i].Field<string>("P_FatherName");
                                FamilyMember_dataGridView.Rows[i].Cells["P_Relation"].Value = selected_dt.Rows[i].Field<int?>("Relation_ID");
                                if (selected_dt.Rows[i]["P_DOB"] != DBNull.Value)
                                {
                                    FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value = selected_dt.Rows[i].Field<DateTime>("P_DOB").ToString("yyyy");
                                }
                                FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value = selected_dt.Rows[i].Field<int?>("P_State_ID");
                                if (selected_dt.Rows[i].Field<int?>("HasWork") == 1)
                                {
                                    FamilyMember_dataGridView.Rows[i].Cells["P_HasWork"].Value = true;
                                }
                                else
                                {
                                    FamilyMember_dataGridView.Rows[i].Cells["P_HasWork"].Value = false;
                                }

                                if (ApplicationState == "project")
                                {
                                    FamilyMember_dataGridView.Rows[i].Cells["P_NationalNumber"].Value = selected_dt.Rows[i].Field<string>("P_NationalNumber");
                                    FamilyMember_dataGridView.Rows[i].Cells["P_EducationLevel"].Value = selected_dt.Rows[i].Field<int?>("EducationLevel_ID");
                                    FamilyMember_dataGridView.Rows[i].Cells["P_EducationType"].Value = selected_dt.Rows[i].Field<int?>("Education_Type_ID");
                                    FamilyMember_dataGridView.Rows[i].Cells["P_E_Name"].Value = selected_dt.Rows[i].Field<int?>("Education_ID");
                                }
                            }
                        }
                    }
                }

                if (Application_ds.Tables[5] != null && Application_ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < Application_ds.Tables[5].Rows.Count; i++)
                    {
                        AidSource_dataGridView.Rows.Add();
                        AidSource_dataGridView.Rows[i].Cells["AidP_ID"].Value = Application_ds.Tables[5].Rows[i].Field<int?>("ID");
                        AidSource_dataGridView.Rows[i].Cells["Aid_PName"].Value = Application_ds.Tables[5].Rows[i].Field<int?>("Person_ID");
                        AidSource_dataGridView.Rows[i].Cells["Aid_Relation"].Value = Application_ds.Tables[5].Rows[i].Field<string>("Relation_Name");
                        AidSource_dataGridView.Rows[i].Cells["Aid_Source"].Value = Application_ds.Tables[5].Rows[i].Field<int?>("AidSource_ID");
                        AidSource_dataGridView.Rows[i].Cells["Aid_DateYear"].Value = Application_ds.Tables[5].Rows[i].Field<string>("DateYear");
                        AidSource_dataGridView.Rows[i].Cells["Aid_Amount"].Value = Application_ds.Tables[5].Rows[i].Field<string>("Amount");
                        AidSource_dataGridView.Rows[i].Cells["Aid_ProjectType"].Value = Application_ds.Tables[5].Rows[i].Field<string>("ProjectType");
                        AidSource_dataGridView.Rows[i].Cells["Aid_Notes"].Value = Application_ds.Tables[5].Rows[i].Field<string>("Notes");
                    }
                }
            }

            UserMode = true;

        }
        #endregion

        #region Check functions
        private DataTable Check_National_Number(string PersonNationalNum)
        {
            try
            {
                var PersonNationalNum_without_zero = "";
                var PersonNationalNum_with_zero = "";

                if (PersonNationalNum.ElementAt(0).ToString() == "0")
                {
                    PersonNationalNum_without_zero = PersonNationalNum.Remove(0, 1);
                    PersonNationalNum_with_zero = PersonNationalNum;
                }
                else
                {
                    PersonNationalNum_without_zero = PersonNationalNum;
                    PersonNationalNum_with_zero = "0" + PersonNationalNum;
                }

                var query = "select CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                            + ",MP.MP_ID 'MicroProject_ID', MP.ApplicationState"
                            + ",P.P_ID 'P_ID'"
                            + " from `microproject` MP inner join person_microproject PMP on PMP.MicroProject_ID = MP.MP_ID "
                            + " inner join person P on P.P_ID = PMP.Person_ID "
                            + " where (P_NationalNumber like @without_zero "
                            + " or P_NationalNumber like @with_zero) and P.P_ID <> @Person_ID ";

                Program.buildConnection();
                var sc = new MySqlCommand(query, Program.MyConn);
                sc.Parameters.AddWithValue("@without_zero", PersonNationalNum_without_zero);
                sc.Parameters.AddWithValue("@with_zero", PersonNationalNum_with_zero);
                sc.Parameters.AddWithValue("@Person_ID", Person_ID);

                sc.ExecuteNonQuery();
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);

                Program.MyConn.Close();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("لا يمكن التحقق من الرقم الوطني، الرجاء المحاولة مرة أخرى" + Environment.NewLine + ex.Message);
                return null;
            }
        }

        private bool Check_MicroProject_ID(string MP_ID)
        {
            try
            {
                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand("select count(MP_ID) from `microproject` where MP_ID = @MP_ID and MP_ID != @MicroProject_ID;", Program.MyConn);
                sc.Parameters.AddWithValue("@MP_ID", MP_ID);
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                var check_MP_ID = Convert.ToInt32(sc.ExecuteScalar());
                Program.MyConn.Close();
                if (check_MP_ID == 0)
                {
                    return true; //امورنا بالسليم
                }
                else
                {
                    MessageBox.Show("الرجاء استخدام رقم مشروع آخر لأن هذا الرقم مستخدم من قبل مشروع آخر");
                    return false; //في حدا تاني بهالرقم
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("لا يمكن التحقق من رقم المشروع، الرجاء المحاولة مرة أخرى" + Environment.NewLine + ex.Message);
                return false; //مشكلة بالتحقق
            }
        }

        private DataTable Check_Family_Num(string FamilyNum)
        {
            try
            {
                var query = @"select CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'الاسم الثلاثي'
                                ,PMP.Person_ID as 'ID' , PMP.MicroProject_ID as 'MicroProject_ID' , f.F_ID as 'Family_ID',
                                (select microproject.ApplicationState from microproject where microproject.MP_ID = PMP.MicroProject_ID) as 'ApplicationState'
                                From `person` p right outer join `person_family` pf on p.P_ID = pf.Person_ID 
                                left outer join `family` f on f.F_ID = pf.Family_ID 
                                left outer join `person_microproject` PMP on PMP.Person_ID = p.P_ID 
                                left join relation on relation.id = pf.Relation_ID
                                where f.F_Number like @FamilyNum and relation.Name like 'مستفيد' and pf.P_Provider_ID <> @Person_ID";

                Program.buildConnection();
                var sc = new MySqlCommand(query, Program.MyConn);
                sc.Parameters.AddWithValue("@FamilyNum", FamilyNum_textBox1.Text);
                sc.Parameters.AddWithValue("@Person_ID", Person_ID);
                sc.ExecuteNonQuery();
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);
                Program.MyConn.Close();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("لا يمكن التحقق من رقم دفتر العائلة، الرجاء المحاولة مرة أخرى" + Environment.NewLine + ex.Message);
                return null;
            }
        }

        #endregion


        #region image
        private void AddImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Person_ID == -1)
                    throw new InvalidOperationException(
                        "لا يمكن إضافة صورة شخصية الآن! الرجاء حفظ بيانات المشروع أولاً ثم إضافة الصورة الشخصية");

                FullPicName = "";

                myTh = new Thread(CallDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();

                int success = 0;
                if (FullPicName != "")
                {
                    ImageLocation_textBox.Text = FullPicName;

                    if (Save_Attachments(FullPicName))
                        success++;

                    if (success > 0)
                    {
                        PersonPicture_pictureBox.ImageLocation = FullPicName;
                        MessageBox.Show("تم حفظ الصورة بنجاح");
                    }
                    else
                    {
                        MessageBox.Show("تعذّر حفظ الصورة، الرجاء التأكد من اختيار صورة ثم المحاولة مرة ثانية");
                    }

                }
                else MessageBox.Show("تعذّر حفظ الصورة، الرجاء التأكد من اختيار صورة ثم المحاولة مرة ثانية");
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذّر حفظ الصورة على السرفر المحلّي أو الأونلاين، الرجاء المحاولة مرة ثانية");
            }
        }

        private bool Save_Attachments(string FullAttachName)
        {

            //insert image//
            if (ImageLocation_textBox.Text == "") throw new InvalidOperationException("Please choose the image you want to add");

            bool sucess = false;
            string t;
            d.GetNameFromPath(FullAttachName, out t, out _);

            string new_name = MicroProject_ID + "_profile" + "_" + t;

            while (d.Offline_Exist(new_name) || d.Online_Exist(new_name))
            {
                t = "(1)" + t;
                new_name = MicroProject_ID + "_profile" + "_" + t;
            }

            ////////////////// Upload Image to server ////////////////////
            if (d.SaveDocument(FullAttachName, new_name))
            {
                ////////////////// Update/save Image path into person //////////////////// 
                img.InsertProfile(MicroProject_ID
                    , new_name
                    , Person_ID
                    , P_FirstName_textBox.Text + " " + P_LastName_textBox.Text); //profile

                sucess = true;
            }
            return sucess;
        }

        private void DeleteImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف الصورة الشخصية للمستفيد ؟", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Added by Maher
                    //string PicName = s.Person_Image(Person_ID);   
                    DataTable img_dt = img.Select_Image(-1, Person_ID, "Profile");
                    string PicName = img_dt.Rows[0]["image_NewPath"].ToString();
                    int Image_ID = -1;
                    if (img_dt.Rows[0]["Image_ID"] != DBNull.Value)
                        Image_ID = int.Parse(img_dt.Rows[0]["Image_ID"].ToString());

                    if (PicName != null || PicName.Length > 0)
                    {
                        string OfflinePicPath = Program.OfflineRoot + @"/" + PicName;
                        //string RemotePicPath = Program.RemoteRoot + @"\" + PicName;
                        string OnlinePicPath = Program.OnlineRoot + @"/" + PicName;

                        PersonPicture_pictureBox.Image = Resources.Unknown_User;

                        if (d.Offline_Exist(OfflinePicPath)) d.DeleteOffline(OfflinePicPath);

                        if (d.Online_Exist(OnlinePicPath)) d.DeleteOnline(OnlinePicPath);

                        //Judy
                        //Clear Path From Database
                        //Update_PersonPicture(Person_ID, "");  
                        img.DeleteImage(Image_ID, MicroProject_ID, PicName);
                        //l.Insert_Log("Delete Image of:" + MicroProject_ID + ":" + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + " from the server"
                        //    , "Person", Program.UserName, DateTime.Now);

                        PersonPicture_pictureBox.Image = Resources.Unknown_User;
                        ImageLocation_textBox.Text = "";
                        FullPicName =  "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذّر حذف الصورة من السرفر المحلي أو الأونلاين! الرجاء المحاولة مرة ثانية", "Error");
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void CallDialog()
        {
            var open = new OpenFileDialog();
            open.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            var res = open.ShowDialog();
            if (res == DialogResult.OK)
            {
                FullPicName = open.FileName;
            }
            else
                return;
        }

        bool success = false;
        private void SaveDialog()
        {
            success = false;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            saveFileDialog1.Title = "Save Image File";
            saveFileDialog1.FileName = MicroProject_ID.ToString() + "_profile";

            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.PersonPicture_pictureBox.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.PersonPicture_pictureBox.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.PersonPicture_pictureBox.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                fs.Close();
                success = true;
            }
            else
                return;
        }
        private void download_toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Download_button_Click(sender, e);
        }
        private void Download_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID == -1)
                    throw new InvalidOperationException("Please Choose a project first.");

                if (PersonPicture_pictureBox.Image == null)
                    throw new InvalidOperationException("Please choose the image you want to download.");

                myTh = new Thread(SaveDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();

                if (success)
                {
                    l.Insert_Log("download profile image " + ImageLocation_textBox.Text + " from the server: ", "Application",
                      Program.UserName, DateTime.Now);
                    MessageBox.Show("تم حفظ الصورة بنجاح");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #region context menu strip
        private void visits_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var _Form = new ChooseVisitKind_Form(MicroProject_ID, mainForm);
            _Form.ShowDialog();
        }

        private void payments_toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var _Form = new Loans_Form(MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "Loans", 0);
        }

        private void attachments_toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var _Form = new Attachments_Form(MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "Attachments", 0);
        }

        private void showChecklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _Form = new CheckList_Form(MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "CheckList", 0);
        }

        private void addNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Find the admin in this database + who entered the application//
                string my_condition = " and ( UserName like '" + CreatedBy_User_label.Text + "'";
                if (CreatedBy_User_label.Text != EditedBy_User_label.Text)
                    my_condition += " or UserName like '" + EditedBy_User_label.Text + "'";
                my_condition += " or role.Role_Name like 'Admin' ) ";

                DataTable u_dt = user.Select_Users("", "", my_condition);
                List<string> emails = new List<string>();

                if (u_dt.Rows.Count != 0)
                {
                    for (int i = 0; i < u_dt.Rows.Count; i++)
                    {
                        string email = u_dt.Rows[i].Field<string>("Email");
                        if (email != "")
                            emails.Add(email);
                    }
                }

                if (emails.Count == 0)
                {
                    string msg = "لا يمكنك كتابة ملاحظات على المشروع لأن الأشخاص المعنيين بإرسال الملاحظات لهم لا يملكون ايميل محفوظ على البرنامج !"
                        + Environment.NewLine
                        + "لذلك يُرجى إضافتها من واجهة التحكم بالمستخدمين واعادة المحاولة لاحقاً";
                    throw new InvalidOperationException(msg);
                }

                string project = MicroProject_ID.ToString() + ":" + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text;
                var _Form = new NewIdea_Form("Application", project, emails);
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion

        #region events
        private void OverallSyrian_label_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (OverallSyrian_label.Text != "")
                    OverallSyrian_label.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(OverallSyrian_label.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region mouse hover
        private void AddImage_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_L;
            else image = Resources.Plus_Sq_D;

            AddImage_button.BackgroundImage = image;
        }
        private void AddImage_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_D;
            else image = Resources.Plus_Sq_L;

            AddImage_button.BackgroundImage = image;
        }

        private void DeleteImage_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Trash_L;
            else image = Resources.Trash_D;

            DeleteImage_button.BackgroundImage = image;
        }
        private void DeleteImage_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Trash_D;
            else image = Resources.Trash_L;

            DeleteImage_button.BackgroundImage = image;
        }

        private void Download_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Download_button.BackgroundImage = Resources.Download_L;
            else Download_button.BackgroundImage = Resources.Download_D;
        }
        private void Download_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Download_button.BackgroundImage = Resources.Download_D;
            else Download_button.BackgroundImage = Resources.Download_L;
        }

        private void Save_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            else
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }

            Save_button.BackgroundImage = image;
            Save_button.ForeColor = co;
        }
        private void Save_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            else
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            //    image = Resources.Save2_D;
            //else image = Resources.Save2_L;

            Save_button.BackgroundImage = image;
            Save_button.ForeColor = co;
        }

        private void Delete_MP_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Delete2_L;
            else image = Resources.Delete2_D;

            Delete_MP_button.BackgroundImage = image;
        }
        private void Delete_MP_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Delete2_D;
            else image = Resources.Delete2_L;

            Delete_MP_button.BackgroundImage = image;
        }

        private void AddPartner_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            else
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            //    image = Resources.AddPartner_L;
            //else image = Resources.AddPartner_D;

            AddPartner_button.BackgroundImage = image;
            AddPartner_button.ForeColor = co;
        }
        private void AddPartner_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            else
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            //    image = Resources.AddPartner_D;
            //else image = Resources.AddPartner_L;

            AddPartner_button.BackgroundImage = image;
            AddPartner_button.ForeColor = co;
        }

        private void Extention_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            else
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            //if (Settings.Default.theme == "Light") 
            //    image = Resources.AddExtention_L;
            //else image = Resources.AddExtention_D;

            AddExtention_button.BackgroundImage = image;
            AddExtention_button.ForeColor = co;
        }
        private void Extention_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            else
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            //    image = Resources.AddExtention_D;
            //else image = Resources.AddExtention_L;

            AddExtention_button.BackgroundImage = image;
            AddExtention_button.ForeColor = co;
        }

        private void FullApp_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            else
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }

            FullApp_button.BackgroundImage = image;
            FullApp_button.ForeColor = co;
        }
        private void FullApp_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            Color co = new Color();
            if (Settings.Default.theme == "Light")
            {
                image = Resources.BtnBackground_D;
                co = NewTheme.light_grey11;
            }
            else
            {
                image = Resources.BtnBackground_L;
                co = NewTheme.dark_grey2;
            }
            FullApp_button.BackgroundImage = image;
            FullApp_button.ForeColor = co;
        }
        #endregion

        #region delete context menu strip
        private void Delete_MP_button_Click(object sender, EventArgs e)
        {
            delete_contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
        }
        private void delete_person_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف بيانات المستفيد مع المحافظة على شركاء (إن وجد) والمحافظة على رقم المشروع؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (Person_ID <= 0)
                    {
                        MessageBox.Show("لا يمكن حذف المستفيد! المستفيد غير موجود في قاعدة البيانات بعد.");
                    }
                    else
                    {
                        var sc = new MySqlCommand();

                        string Query = "START TRANSACTION;";
                        Query += "DELETE person " +
                            " FROM person" +
                            " LEFT JOIN person_family on person.P_ID = person_family.P_Provider_ID" +
                            " WHERE person.P_ID = @Person_ID;";

                        Query += "DELETE family FROM family LEFT JOIN person_family ON family.F_ID = person_family.Family_ID WHERE person_family.Family_ID is null;";
                        Query += "update microproject set Partnership = (SELECT COUNT(person_microproject.Person_ID) FROM person_microproject where person_microproject.MicroProject_ID = MP_ID) where MP_ID = @MicroProject_ID;";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Delete the person:',@Person_ID, ' from the project:', @MicroProject_ID), 'Person', @Log_User, @Log_Date, '0');";

                        Query += " COMMIT;";


                        Program.buildConnection();
                        sc.Parameters.AddWithValue("@Person_ID", Person_ID);
                        sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                        sc.Parameters.AddWithValue("@Log_User", Program.UserName);
                        sc.Parameters.AddWithValue("@Log_Date", DateTime.Now);

                        sc.CommandTimeout = 120;
                        sc.CommandText = "SET SESSION wait_timeout = @t;" + Query;
                        sc.Parameters.AddWithValue("@t", 600);
                        sc.Connection = Program.MyConn;
                        sc.ExecuteNonQuery();

                        MessageBox.Show("تم حذف المستفيد بنجاح");

                        Deleted = true;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void delete_all_project_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف الاستمارة بشكل كامل مع كل بيانات المشروع والمستفيدين والشركاء؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (MicroProject_ID <= 0)
                    {
                        MessageBox.Show("لا يمكن حذف المشروع! المشروع غير موجود في قاعدة البيانات بعد.");
                    }
                    else
                    {
                        var sc = new MySqlCommand();

                        string Query = "START TRANSACTION;";

                        Query += "DELETE person FROM" +
                            " person LEFT JOIN person_family on person.P_ID = person_family.Person_ID" +
                            " WHERE person_family.P_Provider_ID in" +
                            " (SELECT person_microproject.Person_ID FROM person_microproject WHERE person_microproject.MicroProject_ID = @MicroProject_ID);";

                        Query += "DELETE from microproject where microproject.MP_ID = @MicroProject_ID;";

                        Query += "DELETE family FROM family LEFT JOIN person_family ON family.F_ID = person_family.Family_ID WHERE person_family.Family_ID is null;";

                        Query += "INSERT INTO log (Log_Type, Log_OnTable, Log_User, Log_Date, Log_Seen)" +
                            " Values (Concat('Delete the project:',@MicroProject_ID), 'Micro Project', @Log_User, @Log_Date, '0');";


                        Query += " COMMIT;";


                        Program.buildConnection();
                        sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                        sc.Parameters.AddWithValue("@Log_User", Program.UserName);
                        sc.Parameters.AddWithValue("@Log_Date", DateTime.Now);

                        sc.CommandTimeout = 120;
                        sc.CommandText = "SET SESSION wait_timeout = @t;" + Query;
                        sc.Parameters.AddWithValue("@t", 600);
                        sc.Connection = Program.MyConn;
                        sc.ExecuteNonQuery();

                        MessageBox.Show("تم حذف المشروع كاملاً بنجاح");

                        Deleted = true;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}
