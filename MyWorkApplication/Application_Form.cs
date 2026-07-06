using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Application_Form : Form
    {
        private int Beneficiary_Count;
        private FtpConnector c;
        private int image_state;
        //private string ftpPath = "";
        //private string fullFtpPath = ""; 
        //private string imageFilePath, imageName,
        //private byte[] PersonPicArr; 

        private string RequestedAmount, SimpleProfit;
        //Added By Maher 
        string FullPicName = "";
        string FilePicName = "";
        private Document d;
        private Log l;
        private User user;
        private Street st;
        private SubCategory sub;
        private Career career;
        private Select s;
        private Images img;

        private readonly MainForm mainForm;
        private MicroProject mp;
        private MySqlComponents MySS;
        private TasksOfProjects TasksOfProjects;
        private Thread myTh;
        private bool OldBeneficiary_NewProject;

        private int Person_ID, Family_ID, Priest_ID, Husband_Wife_ID;
        public int MicroProject_ID; // to be visible in main form //

        private double sumOfItemsPrice_SYR;
        private int type;
        private Application_Queries app;
        private bool Update_Mode, NoFamily, CanAddFamilyMembers;
        bool user_mode;
        DataSet ds, grid_ds;

        public Application_Form(MainForm mainForm)
        {
            InitializeComponent();

            clear_Person_boxes();
            Person_ID = -1;
            MicroProject_ID = -1;

            Loan_radioButton.Checked = true;
            MyDataGridViewInitializationMethod();

            this.mainForm = mainForm;
            Update_Mode = false; AddPartner_button.Visible = false;
        }

        public Application_Form(int Person_ID, int MicroProject_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.Person_ID = Person_ID;
            this.MicroProject_ID = MicroProject_ID;

            MyDataGridViewInitializationMethod();
            Update_Mode = true; AddPartner_button.Visible = true;

            this.mainForm = mainForm;
        }

        private void Application_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Check_Theme();

                d = new Document();
                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                app = new Application_Queries();
                user = new User();
                c = new FtpConnector();
                mp = new MicroProject();
                st = new Street();
                sub = new SubCategory();
                career = new Career();
                img = new Images();
                Get_Last_MicroProjectID();

                Education_dataGridView.AutoGenerateColumns = false;
                Work_dataGridView.AutoGenerateColumns = Exp_dataGridView.AutoGenerateColumns
                    = FamilyMember_dataGridView.AutoGenerateColumns = false;
                Materials_dataGridView.AutoGenerateColumns = false;

                Bind_All_ComboBoxes(-1);
                Bind_Gridview_ComboBox(-1);

                //////////////////// handle auto scrolling for all comboBoxes //////////////////////
                Type_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                SubType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                SubCategory_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                Career_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);

                P_Parish_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                P_Priest_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                Street_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);

                PersonType_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                P_Sex_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                P_MaritalState_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                P_State_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);

                FPersonSex_comboBox1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                FPersonMaritalState_comboBox1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                FPersonState_comboBox1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);

                PaperApplicationUser_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                Type_comboBox.SelectedIndex = 0; // default MICRO

                users_bind(PaperApplicationUser_comboBox);

                Save_MP_ID_button.Visible = true;
                Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");
                NoFamily = true;

                //hide delete-partner-extention buttons on Create
                AddExtention_button.Visible = AddPartner_button.Visible = Delete_MP_button.Visible = false;
                /////////////////////////////////////////////////

                user_mode = false;
                PersonType_comboBox.SelectedIndex = 0;
                user_mode = true;

                //Reset the nubmer of family members//
                P_NumAtHome_textBox.Text = "0";
                P_State_comboBox.SelectedValue = 1; //مقيم

                if (MicroProject_ID != -1)
                {
                    user_mode = false;
                    if (Person_ID != -1)
                    {
                        Fill_Person();
                        Fill_PersonDetails();
                        Fill_FamilyMembers(0);
                    }
                    Fill_Project();
                    Fill_ProjectDetails();

                    Save_MP_ID_button.Visible = false;
                    Save_MP_ID_button.BackgroundImage = Properties.Resources.Cheked;
                    toolTip1.SetToolTip(Save_MP_ID_button, "محجوز");

                    /// check if this project got partners or not ///
                    var query = "select count(*) from `person_microproject` where MicroProject_ID = " + MicroProject_ID;
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    var partners_count = Convert.ToInt32(MySS.sc.ExecuteScalar());
                    Program.MyConn.Close();
                    if (partners_count > 1) Partners_lable.Text = "شراكة";
                    else Partners_lable.Text = "فردي";

                    //show delete-partner buttons on Update
                    AddPartner_button.Visible = Delete_MP_button.Visible = true;
                    /////////////////////////////////////////////////

                    user_mode = true;
                }

                //unable to cast db.null to system.string !!
                mainForm.MP_ID_label.Visible = mainForm.ProjectNumber_label.Visible =
                    mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.TabName_label.Text = "Application";
                mainForm.MP_ID_label.Text = MPID_textBox.Text;

                Education_dataGridView.Columns["DeleteRow"].Width = Work_dataGridView.Columns["W_DeleteRow"].Width =
                    Exp_dataGridView.Columns["Exp_DeleteRow"].Width =
                        FamilyMember_dataGridView.Columns["P_DeleteRow"].Width = 50;

                Main_panel.AutoScrollPosition = new Point(0, 0);
                //////////////////////////////
                Stop_Highlighting_Combobox();

                CheckUserPermission();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show("لا يمكن فتح هذه الاستمارة بسبب خطأ ما", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                clear_Person_boxes();
                clear_Project_boxes();
                this.Close();
            }
        }

        public void Check_Theme()
        {
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.Application_ToNight(this);
            else
                newTheme.Application_ToLight(this);
        }
        private bool CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible = AddPriest_button.Visible
                            = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                            = true;
                        return true;
                    }
                case 2: //Data
                    {
                        payments_toolStripMenuItem.Visible
                        = addNotesToolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible = AddPriest_button.Visible
                            = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                            = true;
                        return true;
                    }
                case 3: //Financial 
                case 7: //Lawful
                    {
                        Visits_toolStripMenuItem.Visible
                        = showChecklistToolStripMenuItem.Visible
                        = addNotesToolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible = AddPriest_button.Visible
                            = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                            = false;
                        return false;
                    }
                case 4: //Guest
                case 9: //Media
                    {
                        attachments_toolStripMenuItem.Visible
                        = payments_toolStripMenuItem.Visible
                        = Visits_toolStripMenuItem.Visible
                        = showChecklistToolStripMenuItem.Visible
                        = addNotesToolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible = AddPriest_button.Visible
                            = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                           = false;
                        return false;
                    }
            }
            return false;
        }

        #region combobox painting
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background.
            e.DrawBackground();
            SolidBrush brush = new SolidBrush(Color.FromArgb(189, 189, 189));
            if (e.Index >= 0)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
                object Ob = ((ComboBox)sender).Items[e.Index];
                // Draw the text.
                if (Properties.Settings.Default.theme == "Light")
                    e.Graphics.DrawString(((ComboBox)sender).GetItemText(((ComboBox)sender).Items[e.Index]), e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
                else e.Graphics.DrawString(((ComboBox)sender).GetItemText(((ComboBox)sender).Items[e.Index]), e.Font, Brushes.LightGray, e.Bounds.X, e.Bounds.Y);
            }
        }
        private void Stop_Highlighting_Combobox()
        {
            Type_comboBox.SelectionLength = 0;
            Type_comboBox.SelectionStart = Type_comboBox.Text.Length;

            SubType_comboBox.SelectionLength = 0;
            SubType_comboBox.SelectionStart = SubType_comboBox.Text.Length;

            SubCategory_comboBox.SelectionLength = 0;
            SubCategory_comboBox.SelectionStart = SubCategory_comboBox.Text.Length;

            Career_comboBox.SelectionLength = 0;
            Career_comboBox.SelectionStart = Career_comboBox.Text.Length;

            P_Sex_comboBox.SelectionLength = 0; P_Sex_comboBox.SelectionStart = P_Sex_comboBox.Text.Length;
            P_MaritalState_comboBox.SelectionLength = 0; P_MaritalState_comboBox.SelectionStart = P_MaritalState_comboBox.Text.Length;
            P_State_comboBox.SelectionLength = 0; P_State_comboBox.SelectionStart = P_State_comboBox.Text.Length;

            FPersonSex_comboBox1.SelectionLength = 0; FPersonSex_comboBox1.SelectionStart = FPersonSex_comboBox1.Text.Length;
            FPersonMaritalState_comboBox1.SelectionLength = 0; FPersonMaritalState_comboBox1.SelectionStart = FPersonMaritalState_comboBox1.Text.Length;
            FPersonState_comboBox1.SelectionLength = 0; FPersonState_comboBox1.SelectionStart = FPersonState_comboBox1.Text.Length;

            PersonType_comboBox.SelectionLength = 0; PersonType_comboBox.SelectionStart = PersonType_comboBox.Text.Length;

            P_Priest_comboBox.SelectionLength = 0; P_Priest_comboBox.SelectionStart = P_Priest_comboBox.Text.Length;
            Street_comboBox.SelectionLength = 0; Street_comboBox.SelectionStart = Street_comboBox.Text.Length;
            P_Parish_comboBox.SelectionLength = 0; P_Parish_comboBox.SelectionStart = P_Parish_comboBox.Text.Length;

            PaperApplicationUser_comboBox.SelectionLength = 0; PaperApplicationUser_comboBox.SelectionStart = PaperApplicationUser_comboBox.Text.Length;

        }
        #endregion

        private void s1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var sum = 0;
                if (s1_radioButton.Checked) sum += 1;
                if (s2_radioButton.Checked) sum += 1;
                if (s3_radioButton.Checked) sum += 1;
                if (s4_radioButton.Checked) sum += 1;
                if (s5_radioButton.Checked) sum += 1;
                if (s6_radioButton.Checked) sum += 1;
                if (s7_radioButton.Checked) sum += 1;
                if (s8_radioButton.Checked) sum += 1;
                if (s9_radioButton.Checked) sum += 1;
                if (s0_radioButton.Checked) sum += 1;
                Sum_label.Text = "المجموع : " + sum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearPlace_button_Click(object sender, EventArgs e)
        {
            Street_comboBox.SelectedIndex = -1;
        }

        #region datagridview with comboxBoxes methods
        private void MyDataGridViewInitializationMethod()
        {
            FamilyMember_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(FamilyMember_dataGridView_EditingControlShowing);
            Education_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Education_dataGridView_EditingControlShowing);
            Work_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Work_dataGridView_EditingControlShowing);
            Exp_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Exp_dataGridView_EditingControlShowing);
            PowerSource_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(PowerSource_dataGridView_EditingControlShowing);
            aidsource_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(AidSource_dataGridView_EditingControlShowing);
        }

        private void FamilyMember_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(FamilyMember_dataGridView_Control_KeyPress);
        }
        private void Education_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Education_dataGridView_Control_KeyPress);
        }
        private void Work_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Work_dataGridView_Control_KeyPress);
        }
        private void Exp_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Exp_dataGridView_Control_KeyPress);
        }
        private void PowerSource_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(PowerSource_dataGridView_Control_KeyPress);
        }
        private void AidSource_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(AidSource_dataGridView_Control_KeyPress);
        }

        private void FamilyMember_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar)
                && (FamilyMember_dataGridView.CurrentCellAddress.X == 7))
            {
                e.Handled = true;
            }
        }
        private void Education_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Education_dataGridView.CurrentCellAddress.X == 3)
            {
                e.Handled = true;
            }
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Education_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        private void Work_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Work_dataGridView.CurrentCellAddress.X == 8)
            {
                e.Handled = true;
            }
        }
        private void Exp_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Exp_dataGridView.CurrentCellAddress.X == 8)
            {
                e.Handled = true;
            }
        }
        private void PowerSource_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && PowerSource_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        private void AidSource_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && PowerSource_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region sql queries 
        private int Insert_Person()
        {
            string home_state, military;
            var other_properties = 0;
            var other_incomes = 0;

            if (MilitaryFinished_radioButton.Checked) military = "مسرح";
            else if (MilitaryNow_radioButton.Checked) military = "عسكري";
            else if (MilitaryDelayed_radioButton.Checked) military = "مؤجل";
            else if (MilitarySingle_radioButton.Checked) military = "وحيد";
            else if (MilitaryExempt_radioButton.Checked) military = "معفى/بدل";
            else military = "";

            if (P_HomeRent_radioButton.Checked)
                home_state = P_HomeRent_radioButton.Text; // -0-أجار  
            else if (P_HomeOwnership_radioButton.Checked)
                home_state = P_HomeOwnership_radioButton.Text; // -1-ملك 
            else home_state = "";

            if (P_Car_checkBox.Checked && P_Rented_checkBox.Checked) other_properties = 1; // 1 = سيارةوملك مؤجر
            else if (P_Car_checkBox.Checked && !P_Rented_checkBox.Checked) other_properties = 2; // 2 = سيارة
            else if (!P_Car_checkBox.Checked && P_Rented_checkBox.Checked) other_properties = 3; // 3 = ملك مؤجر
            else other_properties = 0; // 0 -none of them checked

            if (P_SourceRelatives_checkBox.Checked && P_SourceRation_checkBox.Checked)
                other_incomes = 1; // 1 = معونة و أقارب
            else if (P_SourceRelatives_checkBox.Checked && !P_SourceRation_checkBox.Checked)
                other_incomes = 2; // 2 = أقارب
            else if (!P_SourceRelatives_checkBox.Checked && P_SourceRation_checkBox.Checked)
                other_incomes = 3; // 3 = معونة
            else other_incomes = 0; // 0 -none of them checked

            var n = (P_MaritalState_comboBox.SelectedValue == null ? SqlInt32.Null : Convert.ToInt32(P_MaritalState_comboBox.SelectedValue.ToString()));

            string military_date = "";
            if (Military_Date_checkBox.Visible && Military_Date_checkBox.Checked)
                military_date = Military_Date_dateTimePicker.Value.ToString("yyyy/MM/dd");
            else military_date = "null";

            string p_query = "START TRANSACTION;";
            p_query += " Insert Into `person`(`P_FirstName`, `P_LastName`, `P_FatherName`, `P_MotherName`, `P_FullName_en`, `P_DOB`" +
                         ", `P_MilitaryService` ,`P_MilitaryService_Date`, `P_MilitaryService_Note`, `P_Mobile`, `P_HomeTel`, `Street_ID`, `P_HomeAddress`, `P_NationalNumber`, `P_RegistrationPlace`" +
                         ", `P_Sex`, `P_MaritalStatus_ID`, `P_IsLivingWithFamily`,`P_LiveWithFamily_Note`, `P_NumAtHome`" +
                         //", `P_ProfilePath` " + 
                         ", `IsProjectOwner`, `IsProvider`" +
                         ", `P_SourceOfIncome`, `P_MedicalCond`, `P_MedicalHelp`, `P_MedicalHelp_Note`, `P_Loss`, `P_IntermidiarySide`" +
                         ", `P_HomeState`,`P_HomeState_Note`, `P_OtherProperties`, `P_OtherProperties_Note`,`P_OtherIncomeSources`, `P_OtherIncomeSources_Note`" +
                         ", `P_Courses_Note`, `P_MaristesCourse`, `P_OtherCourses`,`P_PowerSource_Note`, `P_Parish_ID`, `P_Priest_ID`, `P_State_ID`) values ("

                         + "TRIM(N'" + P_FirstName_textBox.Text + "'),"
                         + "TRIM(N'" + P_LastName_textBox.Text + "'),"
                         + "TRIM(N'" + P_FatherName_textBox.Text + "'),"
                         + "TRIM(N'" + P_MotherName_textBox.Text + "'),"
                         + "TRIM(N'" + P_FullName_En_textBox.Text + "'),"

                         + (P_DOB_textBox.Text == "" ? "null" : "N'" + P_DOB_textBox.Text + "/01/01'") + ",N'"
                         + military + "',"
                         + (military_date == "null" ? "null" : "'" + military_date + "'") + ",N'"
                         + Military_textBox.Text + "',N'"

                         + P_Mobile_textBox.Text + "',N'"
                         + P_HomeTel_textBox.Text + "',"
                         + (Street_comboBox.SelectedValue == null
                             ? SqlInt32.Null
                             : Convert.ToInt32(Street_comboBox.SelectedValue.ToString())) + ",N'"
                         + P_Address_textBox.Text + "',N'"
                         + P_NationalNum_textBox.Text + "',N'"
                         + P_Registration_textBox.Text + "',N'"
                         + P_Sex_comboBox.Text + "',"
                         + (P_MaritalState_comboBox.SelectedValue == null ? SqlInt32.Null : P_MaritalState_comboBox.SelectedValue) + ",'"
                         + (P_LiveWithFamily_radioButton.Checked ? "Yes" : "No") + "','"
                         + LiveWithFamilyDesc_textBox.Text + "',"
                         + P_NumAtHome_textBox.Text + ",'"

                         //+ "" + "',N'" //P_ProfilePath
                         + "YES',N'" // يملك مشروع IsProjectOwner 
                         + (IsProvider_radioButton.Checked ? "Yes" : "No") + "','"
                         + P_SourceOfIncome_textBox.Text + "',N'"
                         + P_MedicalCond_textBox.Text + "',"
                         + (MedicalHelp_radioButton.Checked ? 1 : 0) + ",N'"
                         + P_MedicalHelp_textBox.Text + "',N'"
                         + P_Loss_textBox.Text + "',N'"
                         + P_IntermidiaryName_textBox.Text + "',N'"
                         + home_state + "',N'"
                         + HomeDesc_textBox.Text + "',"
                         + other_properties + ",N'"
                         + OtherProperties_textBox.Text + "',"
                         + other_incomes + ",N'"
                         + P_OtherIncomeSources_textBox.Text + "',N'"
                         + OtherCourses_textBox.Text + "',N'"
                         + "" + "',N'" //maristes course
                         + "" + "',N'" //other courses 
                         + "" + "'," //power sources note

                         + (P_Parish_comboBox.SelectedValue != null ? P_Parish_comboBox.SelectedValue : SqlInt32.Null) + ","
                         + (P_Priest_comboBox.SelectedValue != null ? P_Priest_comboBox.SelectedValue : SqlInt32.Null) + ","
                         + (P_State_comboBox.SelectedValue != null ? P_State_comboBox.SelectedValue : SqlInt32.Null) +
                         " );";

            p_query += " SET @person_id = Last_Insert_ID();";
            p_query += " SELECT @person_id;";
            p_query += " COMMIT;";

            //the connection string must contain Allow User Variables=True , to execute @id in mysql

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand();
            sc.CommandText = p_query;
            sc.Connection = Program.MyConn;
            int id = Convert.ToInt32(sc.ExecuteScalar());
            Program.MyConn.Close();

            return id;
        }

        private void Insert_MicroProject()
        {
            MySS.query = "START TRANSACTION;";
            MySS.query += " INSERT Into `microproject`(`MP_ID`,`MP_ProgramUser_ID`) VALUES (" +
                Convert.ToInt32(MPID_textBox.Text) + "," + Program.userID + ");";
            MySS.query += " INSERT INTO `microproject_state`(`MicroProject_ID`, `State_ID`, `StateReason_ID`, `Date`) VALUES (" +
                Convert.ToInt32(MPID_textBox.Text) + ",3,1,N'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "');";
            MySS.query += "COMMIT;";

            Program.buildConnection();
            {
                MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                MySS.sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        private void Insert_MicroProject_Material(string name, double amount, double price, double contribution,
            string comments, int MP_ID)
        {
            MySS.query =
                "Insert INTO `material`(`Name`, `Amount`, `Price`,`LocalContribution`,`Comments`, `MicroProject_ID`) VALUES ("
                + "N'" + name + "'," + amount + "," + price + "," + contribution + ",N'" + comments + "'," + MP_ID +
                ")";
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Insert_Person_MicroProject(int personID, int microProjectID, string type)
        {
            MySS.query = "Insert into `person_microproject`(`Person_ID`, `MicroProject_ID`, `PersonType`) values ("
                         + personID + ","
                         + microProjectID + ",N'"
                         + type + "' " +
                         ")";
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }
        private void Update_Person_MicroProject(int personID, int microProjectID, string type)
        {


            MySS.query = " update `person_microproject` set PersonType = N'" + type + "' " +
                " where Person_ID = " + personID + " and MicroProject_ID = " + microProjectID;
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private int Insert_Family(string F_Number, int Person_ID, string IsInNow, string relation
            , int providerID, string WorkName)
        {
            string query = "START TRANSACTION;" +
                "Insert Into `family`(`F_Number`) values (N'" + FamilyNum_textBox1.Text + "');";
            query += " SET @family_id = Last_Insert_ID();";
            query += " Insert Into `person_family`(`Family_ID`, `Person_ID`, `IsInNow`, `Relation`, `P_Provider_ID`, `Work_Name`) values("
              + "@family_id," + Person_ID + ",'" + IsInNow + "',N'" + relation + "'," + providerID + ",N'" + WorkName + "'" + ");";
            query += " SELECT @family_id;";
            query += " COMMIT;";

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            int id = -1;
            int.TryParse(sc.ExecuteScalar().ToString(), out id);
            Program.MyConn.Close();

            return id;
        }

        private int Insert_FMember(string f_name, string l_name, string father_name, string mother_name
            , string date, string national_num, string sex, int state_id, int maritalstate_id
            , int F_ID, string relation, int providerID, string WorkName)
        {
            string p_query = "START TRANSACTION;";
            p_query += " Insert Into `person`(`P_FirstName`, `P_LastName`, `P_FatherName`, `P_MotherName`, `P_DOB`" +
                         ", `P_MilitaryService`,`P_MilitaryService_Note`, `P_Mobile`, `P_HomeTel`, `P_HomeAddress`, `P_NationalNumber`, `P_RegistrationPlace`" +
                         ", `P_Sex`, `P_MaritalStatus_ID`, `P_IsLivingWithFamily`,`P_LiveWithFamily_Note`, `P_NumAtHome`" +
                         //", `P_Picture`,P_PicturePath" +
                         //", `P_ProfilePath` " +
                         ", `IsProjectOwner`, `IsProvider`" +
                         ", `P_SourceOfIncome`, `P_MedicalCond`, `P_MedicalHelp`, `P_MedicalHelp_Note`, `P_Loss`, `P_IntermidiarySide`" +
                         ", `P_HomeState`, `P_HomeState_Note`, `P_OtherProperties`, `P_OtherProperties_Note`, `P_OtherIncomeSources`, `P_OtherIncomeSources_Note`" +
                         ", `P_Courses_Note`, `P_MaristesCourse`, `P_OtherCourses`, `P_Parish_ID`, `P_Priest_ID`, `P_State_ID`) values("
                         + "TRIM(N'" + f_name + "'),"
                         + "TRIM(N'" + (l_name != "" ? l_name : "") + "'),"
                         + "TRIM(N'" + (father_name != "" ? father_name : "") + "'),"
                         + "TRIM(N'" + (mother_name != "" ? mother_name : "") + "'),"
                         + (date == "" ? "null" : "'" + date + "/01/01'") + ",N'"
                         + "" + "',N'" // Military service
                         + "" + "',N'" // Military service Note
                         + "" + "','" // Mobile
                         + "" + "',N'" // HomeTel
                         + "" + "','" // HomeAdd
                         + national_num + "',N'" //NationalNumber 
                         + "" + "',N'" // RegistrationPlace
                         + sex + "',"
                         + (maritalstate_id == -1 ? SqlInt32.Null : maritalstate_id) + ",'"
                         + "" + "',N'" // LiveWithAnotherFamily
                         + "" + "','" // LiveWithAnotherFamily Note
                         + "00" + "',N'" // NumAtHome
                                         ////+ null + "',N'" // Picture
                                         ////+ "" + "',N'"
                                         ////+ "" + "',N'"  //P_ProfilePath
                         + "NO" + "',N'" // IsProjectOwner
                         + "No" + "',N'" //is provider
                         + "No" + "',N'" //SourceOfIncome
                         + "" + "','" //MedicalCondition
                         + null + "',N'" //P_MedicalHelp
                         + "" + "',N'" //P_MedicalHelp_Note
                         + null + "',N'" //Loss
                         + null + "',N'" //IntermidiarySide
                         + "" + "',N'" //home_state
                         + "" + "','" //home_state Note
                         + null + "','" //P_OtherProperties
                         + "" + "'," //P_OtherProperties_Note
                         + "0" + ",'" //P_OtherIncomeSources
                         + "" + "','" //P_OtherIncomeSources_Note
                         + "" + "','" //P_Courses_Note
                         + "" + "',N'" //MaristesCourse
                         + "" + "'," //OtherCourses
                         + SqlInt32.Null + "," //Parish_ID
                         + SqlInt32.Null + "," //Priest_ID
                         + (state_id == -1 ? SqlInt32.Null : state_id) //State_ID
                         + " );";

            p_query += " SET @person_id = Last_Insert_ID();";
            p_query += " Insert Into `person_family`(`Family_ID`, `Person_ID`, `IsInNow`, `Relation`, `P_Provider_ID`, `Work_Name`) values("
              + F_ID + ",@person_id, 'Yes',N'" + relation + "',"
              + providerID + ",N'" + WorkName + "'" + ");";

            p_query += " SELECT @person_id;";
            p_query += " COMMIT;";

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand();
            sc.CommandText = p_query;
            sc.Connection = Program.MyConn;
            int id = Convert.ToInt32(sc.ExecuteScalar());
            Program.MyConn.Close();
            return id;
        }

        private void Update_Person(int PID)
        {
            string home_state, military, LiveWithFamily, IsProvider;
            var other_properties = 0;
            var other_incomes = 0;
            var MedicalHelp = 0;

            if (P_HomeRent_radioButton.Checked)
                home_state = P_HomeRent_radioButton.Text; // -0-أجار  
            else if (P_HomeOwnership_radioButton.Checked)
                home_state = P_HomeOwnership_radioButton.Text; // -1-ملك 
            else home_state = "";

            if (MilitaryFinished_radioButton.Checked) military = "مسرح";
            else if (MilitaryNow_radioButton.Checked) military = "عسكري";
            else if (MilitaryDelayed_radioButton.Checked) military = "مؤجل";
            else if (MilitarySingle_radioButton.Checked) military = "وحيد";
            else if (MilitaryExempt_radioButton.Checked) military = "معفى/بدل";
            else military = "";

            string military_date = "";
            if (Military_Date_checkBox.Visible && Military_Date_checkBox.Checked)
                military_date = Military_Date_dateTimePicker.Value.ToString("yyyy/MM/dd");
            else military_date = "null";

            if (P_LiveWithFamily_radioButton.Checked) LiveWithFamily = "Yes"; // 
            else if (P_NotLiveWithFamily_radioButton.Checked) LiveWithFamily = "No"; // 
            else LiveWithFamily = "";

            if (IsProvider_radioButton.Checked) IsProvider = "Yes"; //  
            else if (IsNotProvider_radioButton.Checked) IsProvider = "No"; //
            else IsProvider = "";

            if (MedicalHelp_radioButton.Checked) MedicalHelp = 1; //  
            else if (NoMedicalHelp_radioButton.Checked) MedicalHelp = 0; //  
            else MedicalHelp = -1; //non of them

            if (P_Car_checkBox.Checked && P_Rented_checkBox.Checked) other_properties = 1; // 1 = سيارةوملك مؤجر
            else if (P_Car_checkBox.Checked && !P_Rented_checkBox.Checked) other_properties = 2; // 2 = سيارة
            else if (!P_Car_checkBox.Checked && P_Rented_checkBox.Checked) other_properties = 3; // 3 = ملك مؤجر
            else other_properties = 0; // 0 -none of them checked

            if (P_SourceRelatives_checkBox.Checked && P_SourceRation_checkBox.Checked)
                other_incomes = 1; // 1 = معونة و أقارب
            else if (P_SourceRelatives_checkBox.Checked && !P_SourceRation_checkBox.Checked)
                other_incomes = 2; // 2 = أقارب
            else if (!P_SourceRelatives_checkBox.Checked && P_SourceRation_checkBox.Checked)
                other_incomes = 3; // 3 = معونة
            else other_incomes = 0; // 0 -none of them checked

            MySS.query = "Update `person` set "
                             + "P_FirstName = TRIM(N'" + P_FirstName_textBox.Text + "')"
                             + ",P_LastName = TRIM(N'" + P_LastName_textBox.Text + "')"
                             + ",P_FatherName = TRIM(N'" + P_FatherName_textBox.Text + "')"
                             + ",P_MotherName = TRIM(N'" + P_MotherName_textBox.Text + "')"
                             + ",P_FullName_en = TRIM(N'" + P_FullName_En_textBox.Text + "')"
                             + ",P_DOB = " + (P_DOB_textBox.Text == "" ? "null" : "N'" + P_DOB_textBox.Text + "/01/01" + "'")
                             + ",P_MilitaryService = N'" + military + "'"
                             + ",P_MilitaryService_Date = " + (military_date == "null" ? "null" : "'" + military_date + "'")
                             + ",P_MilitaryService_Note = N'" + Military_textBox.Text + "'"
                             + ",P_Mobile = N'" + P_Mobile_textBox.Text + "'"
                             + ",P_HomeTel = N'" + P_HomeTel_textBox.Text + "'"
                             + ",Street_ID = " + (Street_comboBox.SelectedValue != null
                                 ? int.Parse(Street_comboBox.SelectedValue.ToString())
                                 : SqlInt32.Null) + " "
                             + ",P_HomeAddress = N'" + P_Address_textBox.Text + "'"
                             + ",P_NationalNumber = N'" + P_NationalNum_textBox.Text + "'"
                             + ",P_RegistrationPlace = N'" + P_Registration_textBox.Text + "'"
                             + ",P_Sex = N'" + P_Sex_comboBox.Text + "'"

                             + ",P_MaritalStatus_ID = " + (P_MaritalState_comboBox.SelectedValue != null
                                 ? P_MaritalState_comboBox.SelectedValue
                                 : SqlInt32.Null) + " "

                             + ",P_IsLivingWithFamily = N'" + LiveWithFamily + "'"
                             + ",P_LiveWithFamily_Note  = N'" + LiveWithFamilyDesc_textBox.Text + "'"
                             + ",P_NumAtHome = " +
                             (P_NumAtHome_textBox.Text == "" ? 0 : Convert.ToInt32(P_NumAtHome_textBox.Text)) + ""

                             + ",IsProvider = N'" + IsProvider + "' "
                             + ",P_SourceOfIncome = N'" + P_SourceOfIncome_textBox.Text + "'"
                             + ",P_MedicalCond = N'" + P_MedicalCond_textBox.Text + "'"
                             + ",P_MedicalHelp = " + (MedicalHelp == -1 ? SqlInt32.Null : MedicalHelp) + ""
                             + ",P_MedicalHelp_Note = N'" + P_MedicalHelp_textBox.Text + "'"
                             + ",P_Loss = N'" + P_Loss_textBox.Text + "'"
                             + ",P_IntermidiarySide = N'" + P_IntermidiaryName_textBox.Text + "'"
                             + ",P_HomeState = N'" + home_state + "'"
                             + ",P_HomeState_Note = N'" + HomeDesc_textBox.Text + "'"
                             + ",P_OtherProperties = N'" + other_properties + "'"
                             + ",P_OtherProperties_Note = N'" + OtherProperties_textBox.Text + "'"
                             + ",P_OtherIncomeSources = " + other_incomes + " "
                             + ",P_OtherIncomeSources_Note = N'" + P_OtherIncomeSources_textBox.Text + "' "
                             + ",P_Courses_Note = N'" + OtherCourses_textBox.Text + "'"
                             + ",P_PowerSource_Note = N'" + PowerNote_textBox.Text + "'"

                             + ",P_Parish_ID = " + (P_Parish_comboBox.SelectedValue != null
                                 ? P_Parish_comboBox.SelectedValue
                                 : SqlInt32.Null) + " "

                             + ",P_Priest_ID = " + (P_Priest_comboBox.SelectedValue != null
                                 ? P_Priest_comboBox.SelectedValue
                                 : SqlInt32.Null) + " "

                             + ",P_State_ID = " + (P_State_comboBox.SelectedValue != null
                             ? P_State_comboBox.SelectedValue
                             : SqlInt32.Null) + " "
                         + " where P_ID =" + PID;

            Program.buildConnection();
            {
                MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                MySS.sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        private void Update_MP(int MP_ID)
        {
            RequestedAmount = MP_RequestedAmount_textBox.Text.Replace(",", "");
            SimpleProfit = MP_SimpleProfit_textBox.Text.Replace(",", "");

            string HasPreviousDonors, IsNeedLicense, HasCompetitors
                , SuppliesInsurance, IncomeKind, HasLedger;

            if (HasPreviousDonors_radioButton.Checked) HasPreviousDonors = "Yes"; // 
            else if (HasNotPreviousDonors_radioButton.Checked) HasPreviousDonors = "No"; // 
            else HasPreviousDonors = "";

            if (MP_NeedLicense_radioButton.Checked) IsNeedLicense = "Yes"; // 
            else if (MP_DontNeedLicense_radioButton.Checked) IsNeedLicense = "No"; // 
            else IsNeedLicense = "";

            if (HasCompetitors_radioButton.Checked) HasCompetitors = "Yes"; // 
            else if (HasNotCompetitors_radioButton.Checked) HasCompetitors = "No"; // 
            else HasCompetitors = "";

            if (EasySupplies_radioButton.Checked) SuppliesInsurance = "Easy"; // 
            else if (HardSupplies_radioButton.Checked) SuppliesInsurance = "Hard"; // 
            else SuppliesInsurance = "";

            if (SingleIncome_radioButton.Checked) IncomeKind = "Single"; // 
            else if (AdditionalIncome_radioButton.Checked) IncomeKind = "Additional"; // 
            else IncomeKind = "";

            if (HasLedger_radioButton.Checked) HasLedger = "Yes"; // 
            else if (HasNotLedger_radioButton.Checked) HasLedger = "No"; // 
            else HasLedger = "";

            //get Category_ID
            int cat_ID = -1;
            int sub_ID = -1;
            if (SubCategory_comboBox.SelectedValue != null)
            {
                sub_ID = int.Parse(SubCategory_comboBox.SelectedValue.ToString());
                DataRow[] rows = ds.Tables[0].Select("ID=" + sub_ID);
                cat_ID = rows[0].Field<int>(2);
            }

            MySS.query = "Update `microproject` set " +
                " MP_Name = N'" + MP_Name_textBox.Text + "'" +
                ",MP_Name_en = N'" + MP_Name_En_textBox.Text + "'" +
                ",MP_RequestedAmount = " + Convert.ToDouble(RequestedAmount) + " " +
                ",MP_PeriodOfExecution = N'" + MP_RequestedTime_textBox.Text + "'" +
                ",MP_DateOfRequest = N'" + ApplyDate_bcDateTimePicker.Value.ToString("yyyy/MM/dd") + "'" +
                ",MP_Description = N'" + MP_Description_textBox.Text + "'" +

                ",MP_FundType_ID = " + (Loan_radioButton.Checked ? 1 : 2) + // 1 = loan , 2 = grant
                ",MP_Type_ID = " + (Type_comboBox.SelectedValue != null
                    ? int.Parse(Type_comboBox.SelectedValue.ToString())
                    : 1) + " " + //Default is Micro

                ",MP_SubType_ID = " + (SubType_comboBox.SelectedValue != null
                    ? int.Parse(SubType_comboBox.SelectedValue.ToString())
                    : SqlInt32.Null) + " " +

                ",Category_ID = " + (cat_ID != -1 ? cat_ID : SqlInt32.Null) + " " +

                ",SubCategory_ID = " + (sub_ID != -1 ? sub_ID : SqlInt32.Null) + " " +

                ",Career_ID = " + (Career_comboBox.SelectedValue != null
                    ? int.Parse(Career_comboBox.SelectedValue.ToString())
                    : SqlInt32.Null) + " " +

                ",MP_HasPreviousDonors = N'" + HasPreviousDonors + "'" +
                ",MP_OtherDonors = N'" + MP_OtherDonors_textBox.Text + "'" +
                ",MP_IsNeedLicense = N'" + IsNeedLicense + "'" +
                ",MP_LicenseSide = N'" + MP_LicenseSide_textBox.Text + "'" +
                ",MP_HasCompetitors = N'" + HasCompetitors + "'" +
                ",MP_Competitors = N'" + MP_Competitors_textBox.Text + "'" +
                ",MP_SuppliesInsurance = N'" + SuppliesInsurance + "'" +
                ",MP_Suppliers = N'" + MP_Suppliers_textBox.Text + "'" +
                ",MP_Risk = N'" + MP_Risk_textBox.Text + "'" +
                ",MP_Protection = N'" + MP_Protection_textBox.Text + "'" +
                ",MP_OtherNotes = N'" + MP_OtherNotes_textBox.Text + "'" +
                ",MP_Marketing = N'" + MP_Marketing_textBox.Text + "'" +
                ",MP_SimpleProfit = " + (SimpleProfit == "" ? 0 : Convert.ToDouble(SimpleProfit)) + "" +
                ",MP_Profit_Description = N'" + MP_SimpleProfit2_textBox.Text + "'" +
                ",MP_IncomeKind = N'" + IncomeKind + "'" +
                ",MP_IncomeKind_Note = N'" + IncomeDesc_textBox.Text + "'" +
                ",MP_HasLedger = N'" + HasLedger + "'" +
                ",MP_AmperNumber = " + AmperNumber_textBox.Text + " " +

                ",Partnership = " + (Partners_lable.Text == "شراكة" ? 2 : 1) + // شراكة 2 - فردي 1

                ",MP_PaperUser_ID = " + (PaperApplicationUser_comboBox.SelectedValue != null
                    ? int.Parse(PaperApplicationUser_comboBox.SelectedValue.ToString())
                    : SqlInt32.Null) + " " +

                ",MP_EditedByUser_ID = " + Program.userID +
                " where MP_ID = " + MP_ID;

            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Update_Family(int FID)
        {
            MySS.query = "Update family set "
                         + "F_Number = N'" + FamilyNum_textBox1.Text + "' "
                         + "where F_ID =" + FID;
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Update_FMember(int PID, string f_name, string l_name, string father_name, string mother_name
            , string date, string national_num, string sex, int state_id, int maritalstate_id
            , int F_ID, string relation, string work)
        {
            string p_query = "Start Transaction;";
            p_query += " Update `person` set "
                        + " P_FirstName = TRIM(N'" + f_name + "')"
                        + ",P_LastName = TRIM(N'" + (l_name != "" ? l_name : "") + "')"
                        + ",P_FatherName = TRIM(N'" + (father_name != "" ? father_name : "") + "')"
                        + ",P_MotherName = TRIM(N'" + (mother_name != "" ? mother_name : "") + "')"
                        + ",P_DOB =" + (date == "" ? "null" : "N'" + date + "/01/01" + "'")
                        + ",P_NationalNumber = N'" + national_num + "'"
                        + ",P_Sex = N'" + sex + "'"
                        + ",P_State_ID = " + (state_id == -1 ? SqlInt32.Null : state_id) + " "
                        + ",P_MaritalStatus_ID = " + (maritalstate_id == -1 ? SqlInt32.Null : maritalstate_id) + " "
                        + " where P_ID =" + PID + ";";

            p_query += "UPDATE `person_family` SET "
                    + " `Relation`= N'" + relation + "' "
                    + ",`Work_Name`= N'" + work + "' "
                    + " WHERE `Person_ID` = " + PID + " and `Family_ID` = " + F_ID + " ;";
            p_query += " COMMIT;";

            Program.buildConnection();
            var sc = new MySqlCommand(p_query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Get_Last_MicroProjectID()
        {
            MySS.query = "select MAX(MP_ID) from `microproject`";
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var n = 0;
            int.TryParse(MySS.sc.ExecuteScalar().ToString(), out n);
            MPID_textBox.Text = (++n).ToString();
            Program.MyConn.Close();
        }

        private void Delete_Person(int PID)
        {
            string q2 = "delete From `person` where P_ID =" + PID;
            Program.buildConnection();
            using (var sc = new MySqlCommand(q2, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        private void Delete_MP(int MP_ID)
        {
            string q2 = "delete from `microproject` where MP_ID = " + MP_ID + " ";
            Program.buildConnection();
            using (var sc = new MySqlCommand(q2, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        private void Edit_MP(int old_MP_ID, int new_MP_ID)
        {

            MySS.query = "update `microproject` set MP_ID = " + new_MP_ID + " where MP_ID = " + old_MP_ID + " ";
            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Bind_All_ComboBoxes(int updated_id)
        {
            user_mode = false;
            string query = "";

            query += "SELECT ID, Name, Category_ID from `subcategory` ORDER BY Name ASC;"; //0
            query += "SELECT ID, Name, SubCategory_ID from `career` ORDER BY Name ASC;"; //1 
            query += "SELECT ID, Name from `street` order by Name ASC;"; //2
            query += "select Priest_ID as 'ID',Priest_Name as 'Name' from `priest` order by Priest_Name ASC;"; //3
            query += "select ID,Name from microprojecttype order by ID ASC;"; //4
            query += "select ID,Name,Type_ID from microprojectsubtype order by ID ASC;"; //5
            query += "select ID,Name,Type from `personstate` order by ID ASC;"; //6
            query += "select ID,Name from `maritalstatus` order by ID ASC;"; //7
            query += "select ID,Name_ar as 'Name' from `parish` order by ID ASC;"; //8

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            if (updated_id == -1 || updated_id == 0) // -1 bind all 
            {
                SubCategory_comboBox.DataSource = null;
                SubCategory_comboBox.DisplayMember = "Name";
                SubCategory_comboBox.ValueMember = "ID";
                SubCategory_comboBox.DataSource = ds.Tables[0];
                SubCategory_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 1) // -1 bind all 
            {
                Career_comboBox.DataSource = null;
                Career_comboBox.DisplayMember = "Name";
                Career_comboBox.ValueMember = "ID";
                Career_comboBox.DataSource = ds.Tables[1];
                Career_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 2) // -1 bind all 
            {
                Street_comboBox.DataSource = null;
                Street_comboBox.DisplayMember = "Name";
                Street_comboBox.ValueMember = "ID";
                Street_comboBox.DataSource = ds.Tables[2];
                Street_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 3) // -1 bind all 
            {
                P_Priest_comboBox.DataSource = null;
                P_Priest_comboBox.DisplayMember = "Name";
                P_Priest_comboBox.ValueMember = "ID";
                P_Priest_comboBox.DataSource = ds.Tables[3];
                P_Priest_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 4) // -1 bind all 
            {
                Type_comboBox.DataSource = null;
                Type_comboBox.DisplayMember = "Name";
                Type_comboBox.ValueMember = "ID";
                Type_comboBox.DataSource = ds.Tables[4];
                Type_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 5) // -1 bind all 
            {
                SubType_comboBox.DataSource = null;
                SubType_comboBox.DisplayMember = "Name";
                SubType_comboBox.ValueMember = "ID";
                SubType_comboBox.DataSource = ds.Tables[5];
                SubType_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 6) // -1 bind all 
            {
                //Multiple Combo Boxes With The Same Data Source (C#)
                FPersonState_comboBox1.BindingContext = new BindingContext();   //create a new context
                P_State_comboBox.DataSource = FPersonState_comboBox1.DataSource = null;
                P_State_comboBox.DisplayMember = FPersonState_comboBox1.DisplayMember = "Name";
                P_State_comboBox.ValueMember = FPersonState_comboBox1.ValueMember = "ID";
                P_State_comboBox.DataSource = FPersonState_comboBox1.DataSource = ds.Tables[6];
                P_State_comboBox.SelectedIndex = FPersonState_comboBox1.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 7) // -1 bind all 
            {
                //Multiple Combo Boxes With The Same Data Source (C#)
                FPersonMaritalState_comboBox1.BindingContext = new BindingContext();   //create a new context
                P_MaritalState_comboBox.DataSource = FPersonMaritalState_comboBox1.DataSource = null;
                P_MaritalState_comboBox.DisplayMember = FPersonMaritalState_comboBox1.DisplayMember = "Name";
                P_MaritalState_comboBox.ValueMember = FPersonMaritalState_comboBox1.ValueMember = "ID";
                P_MaritalState_comboBox.DataSource = FPersonMaritalState_comboBox1.DataSource = ds.Tables[7];
                P_MaritalState_comboBox.SelectedIndex = FPersonMaritalState_comboBox1.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 8) // -1 bind all 
            {
                //Multiple Combo Boxes With The Same Data Source (C#)
                P_Parish_comboBox.BindingContext = new BindingContext();   //create a new context
                P_Parish_comboBox.DataSource = null;
                P_Parish_comboBox.DisplayMember = "Name";
                P_Parish_comboBox.ValueMember = "ID";
                P_Parish_comboBox.DataSource = ds.Tables[8];
                P_Parish_comboBox.SelectedIndex = -1;
            }
            user_mode = true;
        }

        private void Bind_Gridview_ComboBox(int updated_id)
        {
            user_mode = false;
            string query = "";

            query += "select ID, Name, Type from `personstate` ORDER BY ID;";  //0
            query += "select E_ID ,E_Name, E_Type from `education` ORDER BY E_Name ASC;"; //1
            query += "select W_ID ,W_Name from `work` ORDER BY W_Name ASC;"; //2
            query += "select ID ,Name from `powersource` ORDER BY ID ASC;"; //3
            query += "select ID ,Name from `aidsource` ORDER BY ID ASC;"; //4

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            grid_ds = new DataSet();
            da.Fill(grid_ds);
            Program.MyConn.Close();

            //combobox column name in datagridviews// 
            if (updated_id == -1 || updated_id == 0) // -1 bind all 
            {
                P_State.DataSource = grid_ds.Tables[0];
                P_State.DisplayMember = "Name";
                P_State.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 1) // -1 bind all 
            {
                E_Name.DataSource = grid_ds.Tables[1];
                E_Name.DisplayMember = "E_Name";
                E_Name.ValueMember = "E_ID";
            }
            if (updated_id == -1 || updated_id == 2) // -1 bind all 
            {
                W_Name.DataSource = grid_ds.Tables[2];
                W_Name.DisplayMember = "W_Name";
                W_Name.ValueMember = "W_ID";

                Exp_Name.DataSource = grid_ds.Tables[2];
                Exp_Name.DisplayMember = "W_Name";
                Exp_Name.ValueMember = "W_ID";
            }
            //combobox column name in datagridviews// 
            if (updated_id == -1 || updated_id == 3) // -1 bind all 
            {
                Power_Name.DataSource = grid_ds.Tables[3];
                Power_Name.DisplayMember = "Name";
                Power_Name.ValueMember = "ID";
            }
            //combobox column name in datagridviews// 
            if (updated_id == -1 || updated_id == 4) // -1 bind all 
            {
                AidSource_Name.DataSource = grid_ds.Tables[4];
                AidSource_Name.DisplayMember = "Name";
                AidSource_Name.ValueMember = "ID";
            }
            user_mode = true;
        }

        private void Career_bind(string SubCategory_ID)
        {
            user_mode = false;
            Career_comboBox.DataSource = null;
            Career_comboBox.DisplayMember = "Name";
            Career_comboBox.ValueMember = "ID";

            if (SubCategory_ID != "")
            {
                DataRow[] rows = null;
                rows = ds.Tables[1].Select("SubCategory_ID=" + SubCategory_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[1].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Career_comboBox.DataSource = selected_rows_dt;
            }
            else
            {
                Career_comboBox.DataSource = ds.Tables[1];
                Career_comboBox.SelectedIndex = -1;
            }
            user_mode = true;
        }
        private void SubType_bind(string Type_ID)
        {
            user_mode = false;
            SubType_comboBox.DataSource = null;
            SubType_comboBox.DisplayMember = "Name";
            SubType_comboBox.ValueMember = "ID";

            if (Type_ID != "")
            {
                DataRow[] rows = null;
                rows = ds.Tables[5].Select("Type_ID=" + Type_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[5].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    SubType_comboBox.DataSource = selected_rows_dt;
                else
                {

                }
            }
            else
            {
                SubType_comboBox.DataSource = ds.Tables[5];
                SubType_comboBox.SelectedIndex = -1;
            }
            user_mode = true;
        }
        private void users_bind(ComboBox comboBox)
        {
            try
            {
                var dt = s.select_visitors();
                comboBox.DataSource = dt;
                comboBox.DisplayMember = "Visitors";
                comboBox.ValueMember = "U_ID";
                comboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region functions

        private void Insert_Update_FamilyMembers()
        {
            string relationInFamily, WorkName, F_FName, F_LName, F_DOB;
            relationInFamily = WorkName = F_FName = F_LName = F_DOB = "";
            int PersonState_id = -1, Person_MaritalStatus = -1;

            for (var i = 0; i < FamilyMember_dataGridView.RowCount - 1; i++)
            {
                F_FName = FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value.ToString();
                if (FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value == null
                    || FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value == DBNull.Value
                    || FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value.ToString() == "0000-00-00")
                    F_DOB = "";
                else F_DOB = FamilyMember_dataGridView.Rows[i].Cells["P_DOB"].Value.ToString();

                if (FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value == null ||
                    FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value.ToString() == null)
                    F_LName = "";
                else F_LName = FamilyMember_dataGridView.Rows[i].Cells["P_LastName"].Value.ToString();

                if (FamilyMember_dataGridView.Rows[i].Cells["Work_Name"].Value == null ||
                    FamilyMember_dataGridView.Rows[i].Cells["Work_Name"].Value.ToString() == null)
                    WorkName = "";
                else WorkName = FamilyMember_dataGridView.Rows[i].Cells["Work_Name"].Value.ToString();

                if (FamilyMember_dataGridView.Rows[i].Cells["Relation"].Value == null ||
                    FamilyMember_dataGridView.Rows[i].Cells["Relation"].Value.ToString() == null)
                    relationInFamily = "";
                else relationInFamily = FamilyMember_dataGridView.Rows[i].Cells["Relation"].Value.ToString();

                //////////
                if (FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == null ||
                    FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString() == null)
                    PersonState_id = -1;
                else int.TryParse(FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString(), out PersonState_id);
                ////////// 

                if (FamilyMember_dataGridView.Rows[i].Cells["P_InDataBase"].Value == null)
                {
                    int F_Member_ID =
                        Insert_FMember(F_FName, F_LName, "", "", F_DOB, "", "", PersonState_id, Person_MaritalStatus
                        , Family_ID, relationInFamily, Person_ID, WorkName);

                    l.Insert_Log("Insert the Family Member: " + F_FName + " " + F_LName, "Family Member",
                        Program.UserName, DateTime.Now);

                    //set P_ID columns
                    FamilyMember_dataGridView.Rows[i].Cells["FP_ID"].Value = F_Member_ID;
                    FamilyMember_dataGridView.Rows[i].Cells["F_ID"].Value = Family_ID;

                    // set that it is in database
                    FamilyMember_dataGridView.Rows[i].Cells["P_InDataBase"].Value = "1";
                }
                else
                {
                    var F_Member_ID = Convert.ToInt32(FamilyMember_dataGridView.Rows[i].Cells["FP_ID"].Value);
                    var F_ID = Convert.ToInt32(FamilyMember_dataGridView.Rows[i].Cells["F_ID"].Value);

                    Update_FMember(F_Member_ID, F_FName, F_LName, "", ""
                        , F_DOB, "", "", PersonState_id, Person_MaritalStatus
                        , Family_ID, relationInFamily, WorkName);

                    l.Insert_Log("Update the Family Member: " + F_FName + " " + F_LName, "Family Member",
                        Program.UserName, DateTime.Now);
                }
            }
        }
        private void Insert_Update_PersonDetails(int Person_ID)
        {
            // home power_sources //
            // DELETE old power_sources and re Insert the new one //
            var Course_ID = -1;
            string p_details_query = "START TRANSACTION;";
            p_details_query += "Delete from `person_powersource` where Person_ID = " + Person_ID + ";";
            string P_log_query = "";
            //////////////////////////////////////////////////////////
            foreach (Control c in HomePower_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    if (CBox.Checked)
                    {
                        p_details_query += "Insert Into `person_powersource`(`Person_ID`, `PowerSource_ID`) values("
                            + Person_ID + ","
                            + "(select ID from `powersource` where Name like '" + CBox.Text + "' limit 1)"
                            + ")" + ";"; ;

                        P_log_query += "INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (N'"
                            + "Insert PowerSource: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + "',N'"
                            + "person_powersource" + "',N'"
                            + Program.UserName + "',N'"
                            + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
                            + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "',"
                            + "0" + ")" + ";"; //not seen 
                    }
                }

            // Course //
            // DELETE old courses and re Insert the new one //
            p_details_query += "Delete from `person_course` where Person_ID = " + Person_ID + ";";
            //////////////////////////////////////////////////////////
            foreach (Control c in Course_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    if (CBox.Checked)
                    {
                        p_details_query += "Insert Into `person_course`(`Person_ID`, `Course_ID`) values("
                                    + Person_ID + ","
                                    + "(select ID from `course` where Name like '" + CBox.Text + "' limit 1)"
                                    + ")" + ";";

                        P_log_query += "INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (N'"
                           + "Insert the Course: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + "',N'"
                           + "Person_Course" + "',N'"
                           + Program.UserName + "',N'"
                           + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
                           + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "',"
                           + "0" + ")" + ";"; //not seen 

                        //MySS.query = "select ID from `course` where Name like '" + CBox.Text + "' limit 1";
                        //Program.buildConnection();
                        //MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        //Course_ID = Convert.ToInt32(MySS.sc.ExecuteScalar()); 
                        //MySS.query = "Insert Into `person_course`(`Person_ID`, `Course_ID`) values("
                        //             + Person_ID + ","
                        //             + Course_ID + ")";
                        //MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        //MySS.sc.ExecuteNonQuery();
                        //Program.MyConn.Close();
                        //l.Insert_Log(
                        //    "Insert the Course: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        //    P_LastName_textBox.Text, "Person_Course", Program.UserName, DateTime.Now);
                    }
                }

            // Financial % Sentimental Loss //
            // DELETE old Loss and re Insert the new one //
            p_details_query += "Delete from `person_loss` where Person_ID = " + Person_ID + ";";
            //////////////////////////////////////////////////////////
            foreach (Control c in Loss_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    if (CBox.Checked)
                    {
                        p_details_query += "Insert Into `person_loss`(`Person_ID`, `Loss_ID`) values("
                                     + Person_ID + ","
                                     + "(select ID from `loss` where Name like '" + CBox.Text + "' limit 1)"
                                     + ")" + ";";

                        P_log_query += "INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (N'"
                           + "Insert the Loss: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + "',N'"
                           + "Person_Loss" + "',N'"
                           + Program.UserName + "',N'"
                           + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
                           + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "',"
                           + "0" + ")" + ";"; //not seen 

                        //MySS.query = "select ID from `loss` where Name like '" + CBox.Text + "' limit 1";
                        //Program.buildConnection();
                        //MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        //Loss_ID = Convert.ToInt32(MySS.sc.ExecuteScalar());
                        //MySS.query = "Insert Into `person_loss`(`Person_ID`, `Loss_ID`) values("
                        //             + Person_ID + ","
                        //             + Loss_ID + ")";
                        //MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        //MySS.sc.ExecuteNonQuery();
                        //Program.MyConn.Close();
                        //l.Insert_Log(
                        //    "Insert the Loss: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        //    P_LastName_textBox.Text, "Person_Course", Program.UserName, DateTime.Now);
                    }
                }

            p_details_query += P_log_query;
            p_details_query += " COMMIT;";
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(p_details_query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
            // /////////////////////////////////////////////////////////////////////////////////////

            // Education //
            var edu_Name = "";
            for (var i = 0; i < Education_dataGridView.RowCount - 1; i++)
            {
                var E_ID = -1;
                if (Education_dataGridView.Rows[i].Cells["E_Name"].Value == null ||
                    Education_dataGridView.Rows[i].Cells["E_Name"].Value.ToString() == null)
                    continue;
                else
                {
                    if (Education_dataGridView.Rows[i].Cells["E_Name"].Value == null ||
                        Education_dataGridView.Rows[i].Cells["E_Name"].Value.ToString() == null)
                        edu_Name = "";
                    else edu_Name = Education_dataGridView.Rows[i].Cells["E_Name"].Value.ToString();

                    DataRow[] rows = grid_ds.Tables[1].Select("E_Name='" + edu_Name + "'");
                    E_ID = rows[0].Field<int>(0);
                }
                ////set E_ID and P_ID columns
                Education_dataGridView.Rows[i].Cells["P_ID"].Value = Person_ID;
                Education_dataGridView.Rows[i].Cells["E_ID"].Value = E_ID;

                if (Education_dataGridView.Rows[i].Cells["Edu_InDataBase"].Value == null)
                {
                    //Insert into person_education_Table 
                    var query = "select MAX(ID) from `person_education` ";
                    Program.buildConnection();
                    sc = new MySqlCommand(query, Program.MyConn);
                    var ID = 0;

                    ID = sc.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(sc.ExecuteScalar().ToString());

                    ID = ID + 1;
                    Program.MyConn.Close();

                    MySS.query = "Insert Into `person_education`(`Person_ID`, `Education_ID`,`ID`) values("
                                 + Person_ID + ","
                                 + E_ID + ","
                                 + ID + ")";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();
                    Education_dataGridView.Rows[i].Cells["Edu_InDataBase"].Value = "1";
                    Education_dataGridView.Rows[i].Cells["Edu_RowID"].Value = ID;
                    l.Insert_Log(
                        "Insert the Education: " + edu_Name + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Education", Program.UserName, DateTime.Now);
                }
                else /// update
                {
                    var ID = Convert.ToInt32(Education_dataGridView.Rows[i].Cells["Edu_RowID"].Value);
                    app.Update_Person_Education(ID, E_ID);
                    l.Insert_Log(
                        "Update the Education: " + edu_Name + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Education", Program.UserName, DateTime.Now);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////

            // work //
            string Work_status_string, Work_Name_string, Work_beginYear_string, Work_endYear_string,
                WorkCauseOfLoseDesc_string, WorkDescription_string, WorkPlace_string, WorkPlaceState_string;

            Double WorkSalary = 0;
            Work_Name_string = Work_beginYear_string = Work_endYear_string = "0";
            WorkCauseOfLoseDesc_string = WorkDescription_string = WorkPlace_string = WorkPlaceState_string = " ";
            Work_status_string = " ";
            for (var i = 0; i < Work_dataGridView.RowCount - 1; i++)
            {
                // get the data of 1 row

                int W_ID = -1;
                if (Work_dataGridView.Rows[i].Cells["W_Name"].Value == null ||
                    Work_dataGridView.Rows[i].Cells["W_Name"].Value.ToString() == null)
                    continue;
                else
                {
                    int.TryParse(Work_dataGridView.Rows[i].Cells["W_Name"].Value.ToString(), out W_ID);
                    DataRow[] rows = grid_ds.Tables[2].Select("W_ID=" + W_ID);
                    Work_Name_string = rows[0].Field<string>(1);
                }
                if (Work_dataGridView.Rows[i].Cells["W_Description"].Value == null ||
                    Work_dataGridView.Rows[i].Cells["W_Description"].Value.ToString() == null)
                    WorkDescription_string = " ";
                else WorkDescription_string = Work_dataGridView.Rows[i].Cells["W_Description"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value == null ||
                    Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value.ToString() == null)
                    Work_beginYear_string = "0000";
                else Work_beginYear_string = Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_EndDate"].Value == null ||
                    string.IsNullOrWhiteSpace(Work_dataGridView.Rows[i].Cells["W_EndDate"].Value.ToString()))
                    Work_endYear_string = "0000";
                else Work_endYear_string = Work_dataGridView.Rows[i].Cells["W_EndDate"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_Place"].Value == null ||
                    string.IsNullOrWhiteSpace(Work_dataGridView.Rows[i].Cells["W_Place"].Value.ToString()))
                    WorkPlace_string = "";
                else WorkPlace_string = Work_dataGridView.Rows[i].Cells["W_Place"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value == null ||
                    string.IsNullOrWhiteSpace(Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value.ToString()))
                    WorkPlaceState_string = "";
                else WorkPlaceState_string = Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_CauseOfLose"].Value == null ||
                    string.IsNullOrWhiteSpace(Work_dataGridView.Rows[i].Cells["W_CauseOfLose"].Value.ToString()))
                    WorkCauseOfLoseDesc_string = " ";
                else WorkCauseOfLoseDesc_string = Work_dataGridView.Rows[i].Cells["W_CauseOfLose"].Value.ToString();

                if (Work_dataGridView.Rows[i].Cells["W_Salary"].Value == null ||
                    string.IsNullOrWhiteSpace(Work_dataGridView.Rows[i].Cells["W_Salary"].Value.ToString()))
                    WorkSalary = 0;
                else Double.TryParse(Work_dataGridView.Rows[i].Cells["W_Salary"].Value.ToString().Replace(",", ""), out WorkSalary);

                //set W_ID and P_ID columns

                Work_dataGridView.Rows[i].Cells["WP_ID"].Value = Person_ID;

                Work_dataGridView.Rows[i].Cells["W_ID"].Value = W_ID;
                Program.MyConn.Close();

                Work_status_string = "Yes"; //حالي

                if (Work_dataGridView.Rows[i].Cells["W_InDataBase"].Value == null)
                {
                    //Insert into person_workexp_Table

                    var query = "select MAX(ID) from `person_workexp` ";
                    Program.buildConnection();
                    sc = new MySqlCommand(query, Program.MyConn);
                    var ID = 0;

                    ID = sc.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(sc.ExecuteScalar().ToString());
                    ID = ID + 1;
                    Program.MyConn.Close();

                    MySS.query = "Insert Into `person_workexp`(`ID`,`Person_ID`, `Work_ID`" +
                                 ", `W_Description`, `W_BeginDate`, `W_EndDate`, `W_Place`,`W_PlaceState`, `W_Status`" +
                                 ", `W_CauseOfLose`, `W_CauseOfLoseDescription`, `W_Salary`,`MicroProject_ID`) values("
                                 + ID + "," + Person_ID + "," + W_ID
                                 + ",N'" + WorkDescription_string + "',N'" + Work_beginYear_string + "',N'" +
                                 Work_endYear_string + "',N'" + WorkPlace_string
                                 + "',N'" + WorkPlaceState_string + "',N'" + Work_status_string + "',N' ',N'" +
                                 WorkCauseOfLoseDesc_string + "'," + WorkSalary + "," + MicroProject_ID
                                 + ")";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();

                    Work_dataGridView.Rows[i].Cells["W_RowID"].Value = ID;
                    Work_dataGridView.Rows[i].Cells["W_InDataBase"].Value = "1";
                    l.Insert_Log(
                        "Insert the Work: " + Work_Name_string + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Work", Program.UserName, DateTime.Now);
                }
                else /// update
                {
                    var ID = Convert.ToInt32(Work_dataGridView.Rows[i].Cells["W_RowID"].Value);
                    app.Update_Person_WorkExp(ID, W_ID, WorkDescription_string, Work_beginYear_string,
                        Work_endYear_string, WorkPlace_string, WorkPlaceState_string, WorkCauseOfLoseDesc_string,
                        WorkSalary);
                    l.Insert_Log(
                        "Update the Work: " + Work_Name_string + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Work", Program.UserName, DateTime.Now);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////

            Work_beginYear_string = Work_endYear_string = "0";
            WorkCauseOfLoseDesc_string = WorkDescription_string = WorkPlace_string = " ";
            Work_status_string = " ";
            // experience //
            for (var i = 0; i < Exp_dataGridView.RowCount - 1; i++)
            {
                int Exp_ID = -1;

                // get the data of 1 row
                if (Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value == null ||
                    Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value.ToString() == null)
                    continue;
                else
                {
                    int.TryParse(Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value.ToString(), out Exp_ID);
                    DataRow[] rows = grid_ds.Tables[2].Select("W_ID=" + Exp_ID);
                    Work_Name_string = rows[0].Field<string>(1);
                }
                if (Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value == null)
                    WorkDescription_string = " ";
                else WorkDescription_string = Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value == null)
                    Work_beginYear_string = "0000";
                else Work_beginYear_string = Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value == null)
                    Work_endYear_string = "0000";
                else Work_endYear_string = Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value == null)
                    WorkPlace_string = " ";
                else WorkPlace_string = Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value == null ||
                    string.IsNullOrWhiteSpace(Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value.ToString()))
                    WorkPlaceState_string = "";
                else WorkPlaceState_string = Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value == null)
                    WorkCauseOfLoseDesc_string = " ";
                else WorkCauseOfLoseDesc_string = Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value.ToString();

                if (Exp_dataGridView.Rows[i].Cells["Exp_Salary"].Value == null ||
                    string.IsNullOrWhiteSpace(Exp_dataGridView.Rows[i].Cells["Exp_Salary"].Value.ToString()))
                    WorkSalary = 0;
                else Double.TryParse(Exp_dataGridView.Rows[i].Cells["Exp_Salary"].Value.ToString().Replace(",", ""), out WorkSalary);

                //set W_ID and P_ID columns 
                Exp_dataGridView.Rows[i].Cells["ExpP_ID"].Value = Person_ID;
                Exp_dataGridView.Rows[i].Cells["Exp_ID"].Value = Exp_ID;

                Work_status_string = "No"; //سابق
                if (Exp_dataGridView.Rows[i].Cells["Exp_InDataBase"].Value == null)
                {
                    //Insert into person_workexp_Table 
                    var query = "select MAX(ID) from `person_workexp` ";
                    Program.buildConnection();
                    sc = new MySqlCommand(query, Program.MyConn);

                    var ID = 0;
                    ID = sc.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(sc.ExecuteScalar().ToString());

                    ID = ID + 1;
                    Program.MyConn.Close();
                    MySS.query = "Insert Into `person_workexp`(`ID`,`Person_ID`, `Work_ID`," +
                                 " `W_Description`, `W_BeginDate`, `W_EndDate`, `W_Place`, `W_PlaceState`, `W_Status`," +
                                 " `W_CauseOfLose`, `W_CauseOfLoseDescription`, `W_Salary`,`MicroProject_ID`) values("
                                 + ID + "," + Person_ID + "," + Exp_ID
                                 + ",N'" + WorkDescription_string + "',N'" + Work_beginYear_string + "',N'" +
                                 Work_endYear_string + "',N'" + WorkPlace_string + "',N'" + WorkPlaceState_string
                                 + "',N'" + Work_status_string + "',N' ',N'" + WorkCauseOfLoseDesc_string + "'," +
                                 WorkSalary + "," + MicroProject_ID
                                 + ")";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();

                    Exp_dataGridView.Rows[i].Cells["Exp_InDataBase"].Value = "1";
                    Exp_dataGridView.Rows[i].Cells["Exp_RowID"].Value = ID;
                    l.Insert_Log(
                        "Insert the Work: " + Work_Name_string + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Work", Program.UserName, DateTime.Now);
                }
                else /// update
                {
                    var ID = Convert.ToInt32(Exp_dataGridView.Rows[i].Cells["Exp_RowID"].Value);
                    app.Update_Person_WorkExp(ID, Exp_ID, WorkDescription_string, Work_beginYear_string,
                        Work_endYear_string, WorkPlace_string, WorkPlaceState_string, WorkCauseOfLoseDesc_string,
                        WorkSalary);
                    l.Insert_Log(
                        "Update the Work: " + Work_Name_string + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_Work", Program.UserName, DateTime.Now);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////


            // AidSource //
            string aidsource_Name, aidsource_Notes_string, aidsource_Year_string;
            aidsource_Name = aidsource_Notes_string = "";
            aidsource_Year_string = "0000";

            for (var i = 0; i < aidsource_dataGridView.RowCount - 1; i++)
            {
                var AidSource_ID = -1;
                if (aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value == null ||
                    aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value.ToString() == null)
                    continue;
                else
                {
                    if (aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value == null ||
                        aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value.ToString() == null)
                        AidSource_ID = -1;
                    else
                    {
                        AidSource_ID = (int)aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value;
                        DataRow[] rows = grid_ds.Tables[4].Select("ID=" + AidSource_ID + "");
                        aidsource_Name = rows[0].Field<string>(1);
                    }
                }

                //year
                if (aidsource_dataGridView.Rows[i].Cells["AidSource_DateYear"].Value == null)
                    aidsource_Year_string = "0000";
                else aidsource_Year_string = aidsource_dataGridView.Rows[i].Cells["AidSource_DateYear"].Value.ToString();

                //note
                if (aidsource_dataGridView.Rows[i].Cells["AidSource_Notes"].Value == null)
                    aidsource_Notes_string = " ";
                else aidsource_Notes_string = aidsource_dataGridView.Rows[i].Cells["AidSource_Notes"].Value.ToString();

                ////set Aid_ID and P_ID columns
                aidsource_dataGridView.Rows[i].Cells["AidSourceP_ID"].Value = Person_ID;
                aidsource_dataGridView.Rows[i].Cells["AidSource_ID"].Value = E_ID;

                if (aidsource_dataGridView.Rows[i].Cells["AidSource_InDataBase"].Value == null)
                {
                    //Insert into person_education_Table 
                    var query = "select MAX(ID) from `person_aidsource`;";
                    Program.buildConnection();
                    sc = new MySqlCommand(query, Program.MyConn);
                    var ID = 0;

                    ID = sc.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(sc.ExecuteScalar().ToString());

                    ID = ID + 1;
                    Program.MyConn.Close();
                    MySS.query = "Insert Into `person_aidsource`(`ID`, `MicroProject_ID`,`Person_ID`, `AidSource_ID`," +
                                 " `DateYear`, `Notes`) values("
                                 + ID + "," + MicroProject_ID + "," + Person_ID + "," + AidSource_ID
                                 + ",N'" + aidsource_Year_string + "',N'" + aidsource_Notes_string + "')";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();
                    aidsource_dataGridView.Rows[i].Cells["AidSource_InDataBase"].Value = "1";
                    aidsource_dataGridView.Rows[i].Cells["AidSource_RowID"].Value = ID;
                    l.Insert_Log(
                        "Insert the AidSource: " + aidsource_Name + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_AidSource", Program.UserName, DateTime.Now);
                }
                else /// update
                {
                    var ID = Convert.ToInt32(aidsource_dataGridView.Rows[i].Cells["AidSource_RowID"].Value);
                    app.Update_Person_AidSource(ID, AidSource_ID, aidsource_Year_string, aidsource_Notes_string);
                    l.Insert_Log(
                        "Update the AidSource: " + edu_Name + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                        P_LastName_textBox.Text, "Person_AidSource", Program.UserName, DateTime.Now);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////

        }
        private void Insert_Update_ProjectDetails(int MicroProject_ID)
        {
            // project power_source //
            string Power_Name_string, Power_Note_string, Power_Responsability_string;

            Double Power_Cost = 0.0;
            Power_Name_string = "0";
            Power_Note_string = Power_Responsability_string = " ";

            for (var i = 0; i < PowerSource_dataGridView.RowCount - 1; i++)
            {
                // get the data of 1 row 
                int Power_ID = -1;
                if (PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value == null ||
                    PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value.ToString() == null)
                    continue;
                else
                {
                    //when insert: i have the value = id , when update i have the value = name
                    int.TryParse(PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value.ToString(), out Power_ID);
                    if (Power_ID == -1 || Power_ID == 0)
                    {
                        Power_Name_string = PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value.ToString();
                        DataRow[] rows = grid_ds.Tables[3].Select("Name='" + Power_Name_string + "'");
                        Power_ID = rows[0].Field<int>(0);
                    }
                    else
                    {
                        DataRow[] rows = grid_ds.Tables[3].Select("ID=" + Power_ID);
                        Power_Name_string = rows[0].Field<string>(1);
                    }

                }

                if (PowerSource_dataGridView.Rows[i].Cells["Power_Note"].Value == null ||
                    PowerSource_dataGridView.Rows[i].Cells["Power_Note"].Value.ToString() == null)
                    Power_Note_string = " ";
                else Power_Note_string = PowerSource_dataGridView.Rows[i].Cells["Power_Note"].Value.ToString();

                if (PowerSource_dataGridView.Rows[i].Cells["Power_Cost"].Value == null ||
                    string.IsNullOrWhiteSpace(PowerSource_dataGridView.Rows[i].Cells["Power_Cost"].Value.ToString()))
                    Power_Cost = 0;
                else Double.TryParse(PowerSource_dataGridView.Rows[i].Cells["Power_Cost"].Value.ToString().Replace(",", ""), out Power_Cost);

                if (PowerSource_dataGridView.Rows[i].Cells["Power_Responsability"].Value == null ||
                    PowerSource_dataGridView.Rows[i].Cells["Power_Responsability"].Value.ToString() == null)
                    Power_Responsability_string = " ";
                else Power_Responsability_string = PowerSource_dataGridView.Rows[i].Cells["Power_Responsability"].Value.ToString();

                //set W_ID and P_ID columns 
                PowerSource_dataGridView.Rows[i].Cells["Power_MP_ID"].Value = MicroProject_ID;
                PowerSource_dataGridView.Rows[i].Cells["Power_ID"].Value = Power_ID;

                if (PowerSource_dataGridView.Rows[i].Cells["Power_InDataBase"].Value == null)
                {
                    //Insert into microproject_powersource Table 
                    var query = "select MAX(ID) from `microproject_powersource` ";
                    Program.buildConnection();
                    MySqlCommand sc1 = new MySqlCommand(query, Program.MyConn);
                    var ID = 0;

                    ID = sc1.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(sc1.ExecuteScalar().ToString());
                    ID = ID + 1;
                    Program.MyConn.Close();

                    MySS.query = "INSERT INTO `microproject_powersource`(`ID`, `MicroProject_ID`, `PowerSource_ID`, `Note`, `Cost`, `Responsability`) values("
                                 + ID + ","
                                 + MicroProject_ID + ","
                                 + Power_ID + ",N'"
                                 + Power_Note_string + "',"
                                 + Power_Cost + ",N'"
                                 + Power_Responsability_string + "'"
                                 + ")";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();

                    PowerSource_dataGridView.Rows[i].Cells["Power_RowID"].Value = ID;
                    PowerSource_dataGridView.Rows[i].Cells["Power_InDataBase"].Value = "1";
                    l.Insert_Log(
                        "Insert the PowerSource: " + Power_Name_string + " to Project: " + MicroProject_ID,
                        "microproject_powersource", Program.UserName, DateTime.Now);
                }
                else /// update
                {
                    var ID = Convert.ToInt32(PowerSource_dataGridView.Rows[i].Cells["Power_RowID"].Value);

                    string query1 = " UPDATE `microproject_powersource` SET "
                                + "PowerSource_ID = " + Power_ID + ","
                                + "Note  =N'" + Power_Note_string + "',"
                                + "Cost = " + Power_Cost + ","
                                + "Responsability = N'" + Power_Responsability_string + "' "
                                + ";";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    Program.MyConn.Close();

                    l.Insert_Log(
                        "Update the PowerSource: " + Power_Name_string + " to Project: " + MicroProject_ID
                        , "microproject_powersource", Program.UserName, DateTime.Now);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////

            //Budget Materials//
            string Name, Comments, Amount, Price, LocalContribution;
            for (var i = 0; i < Materials_dataGridView.RowCount - 1; i++)
            {
                Name = Materials_dataGridView.Rows[i].Cells["M_Name"].Value.ToString().Replace(",", "");
                Amount = Materials_dataGridView.Rows[i].Cells["M_Amount"].Value.ToString().Replace(",", "");
                Price = Materials_dataGridView.Rows[i].Cells["M_Price"].Value.ToString().Replace(",", "");

                if (Materials_dataGridView.Rows[i].Cells["M_LocalContribution"].Value == null)
                    LocalContribution = "0";
                else
                    LocalContribution = Materials_dataGridView.Rows[i].Cells["M_LocalContribution"].Value.ToString();

                if (Materials_dataGridView.Rows[i].Cells["M_Comments"].Value == null)
                    Comments = "";
                else Comments = Materials_dataGridView.Rows[i].Cells["M_Comments"].Value.ToString();

                Materials_dataGridView.Rows[i].Cells["MMP_ID"].Value = MicroProject_ID;

                if (Materials_dataGridView.Rows[i].Cells["M_InDataBase"].Value == null)
                {
                    //Insert into materials table
                    Insert_MicroProject_Material(Name, Convert.ToDouble(Amount), Convert.ToDouble(Price),
                        Convert.ToDouble(LocalContribution), Comments, MicroProject_ID);

                    MySS.query = "select MAX(ID) from `material` ";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                    var M_ID = Convert.ToInt32(MySS.sc.ExecuteScalar());
                    Program.MyConn.Close();

                    Materials_dataGridView.Rows[i].Cells["M_ID"].Value = M_ID;
                    Materials_dataGridView.Rows[i].Cells["M_InDataBase"].Value = "1";

                    l.Insert_Log("Insert the material: " + Name + " to the project: " + MicroProject_ID, "Material",
                        Program.UserName, DateTime.Now);
                }
                else /// Update on existing rows
                {
                    var M_ID = Convert.ToInt32(Materials_dataGridView.Rows[i].Cells["M_ID"].Value);
                    app.Update_Material(M_ID, Name, Convert.ToDouble(Amount), Convert.ToDouble(Price),
                        Convert.ToDouble(LocalContribution), Comments);

                    l.Insert_Log("Update the material: " + Name + " to the project: " + MicroProject_ID, "Material",
                        Program.UserName, DateTime.Now);
                }
            }

            // Risks //
            // DELETE old Risks and re Insert the new one //
            var Risk_ID = -1;
            var q = "Delete from `microproject_risk` where MicroProject_ID = " + MicroProject_ID;
            Program.buildConnection();
            var sc = new MySqlCommand(q, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
            //////////////////////////////////////////////////////////
            foreach (Control c in Risks_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    if (CBox.Checked)
                    {
                        MySS.query = "select ID from `risk` where Name like '" + CBox.Text + "' limit 1";
                        Program.buildConnection();
                        MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        Risk_ID = Convert.ToInt32(MySS.sc.ExecuteScalar());

                        MySS.query = "Insert Into `microproject_risk`(`MicroProject_ID`, `Risk_ID`) values("
                                     + MicroProject_ID + ","
                                     + Risk_ID + ")";
                        MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                        MySS.sc.ExecuteNonQuery();
                        Program.MyConn.Close();
                        l.Insert_Log(
                            "Insert the Risk: " + CBox.Text + " to Beneficiary : " + P_FirstName_textBox.Text + " " +
                            P_LastName_textBox.Text, "microproject_risk", Program.UserName, DateTime.Now);
                    }
                }

            // Score //
            // DELETE old Score and re Insert the new one //
            var Score_ID = -1;
            var Value = -1;
            q = "Delete from `microproject_score` where MicroProject_ID = " + MicroProject_ID;
            Program.buildConnection();
            sc = new MySqlCommand(q, Program.MyConn);
            sc.ExecuteNonQuery();
            //////////////////////////////////////////////////////////

            if (s1_radioButton.Checked && !n1_radioButton.Checked)
            {
                Score_ID = 1;
                Value = 1;
            }
            else if (!s1_radioButton.Checked && n1_radioButton.Checked)
            {
                Score_ID = 1;
                Value = 0;
            }
            else if (!s1_radioButton.Checked && !n1_radioButton.Checked)
            {
                Score_ID = 1;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s1_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s2_radioButton.Checked && !n2_radioButton.Checked)
            {
                Score_ID = 2;
                Value = 1;
            }
            else if (!s2_radioButton.Checked && n2_radioButton.Checked)
            {
                Score_ID = 2;
                Value = 0;
            }
            else if (!s2_radioButton.Checked && !n2_radioButton.Checked)
            {
                Score_ID = 2;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s2_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s3_radioButton.Checked && !n3_radioButton.Checked)
            {
                Score_ID = 3;
                Value = 1;
            }
            else if (!s3_radioButton.Checked && n3_radioButton.Checked)
            {
                Score_ID = 3;
                Value = 0;
            }
            else if (!s3_radioButton.Checked && !n3_radioButton.Checked)
            {
                Score_ID = 3;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s3_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s4_radioButton.Checked && !n4_radioButton.Checked)
            {
                Score_ID = 4;
                Value = 1;
            }
            else if (!s4_radioButton.Checked && n4_radioButton.Checked)
            {
                Score_ID = 4;
                Value = 0;
            }
            else if (!s4_radioButton.Checked && !n4_radioButton.Checked)
            {
                Score_ID = 4;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s4_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s5_radioButton.Checked && !n5_radioButton.Checked)
            {
                Score_ID = 5;
                Value = 1;
            }
            else if (!s5_radioButton.Checked && n5_radioButton.Checked)
            {
                Score_ID = 5;
                Value = 0;
            }
            else if (!s5_radioButton.Checked && !n5_radioButton.Checked)
            {
                Score_ID = 5;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s5_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s6_radioButton.Checked && !n6_radioButton.Checked)
            {
                Score_ID = 6;
                Value = 1;
            }
            else if (!s6_radioButton.Checked && n6_radioButton.Checked)
            {
                Score_ID = 6;
                Value = 0;
            }
            else if (!s6_radioButton.Checked && !n6_radioButton.Checked)
            {
                Score_ID = 6;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s6_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s7_radioButton.Checked && !n7_radioButton.Checked)
            {
                Score_ID = 7;
                Value = 1;
            }
            else if (!s7_radioButton.Checked && n7_radioButton.Checked)
            {
                Score_ID = 7;
                Value = 0;
            }
            else if (!s7_radioButton.Checked && !n7_radioButton.Checked)
            {
                Score_ID = 7;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s7_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s8_radioButton.Checked && !n8_radioButton.Checked)
            {
                Score_ID = 8;
                Value = 1;
            }
            else if (!s8_radioButton.Checked && n8_radioButton.Checked)
            {
                Score_ID = 8;
                Value = 0;
            }
            else if (!s8_radioButton.Checked && !n8_radioButton.Checked)
            {
                Score_ID = 8;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s8_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s9_radioButton.Checked && !n9_radioButton.Checked)
            {
                Score_ID = 9;
                Value = 1;
            }
            else if (!s9_radioButton.Checked && n9_radioButton.Checked)
            {
                Score_ID = 9;
                Value = 0;
            }
            else if (!s9_radioButton.Checked && !n9_radioButton.Checked)
            {
                Score_ID = 9;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s9_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            if (s0_radioButton.Checked && !n0_radioButton.Checked)
            {
                Score_ID = 10;
                Value = 1;
            }
            else if (!s0_radioButton.Checked && n0_radioButton.Checked)
            {
                Score_ID = 10;
                Value = 0;
            }
            else if (!s0_radioButton.Checked && !n0_radioButton.Checked)
            {
                Score_ID = 10;
                Value = -1;
            }

            MySS.query = "INSERT INTO `microproject_score`(`MicroProject_ID`, `Score_ID`, `value`, `notes`) values(" +
                         MicroProject_ID + "," + Score_ID + "," + Value + ",N'" + s0_textBox.Text + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }
        private void Insert_Update_AllFamilyDetails()
        {
            if (FamilyNum_textBox1.Text == "")
            {
                MessageBox.Show("انتبه! لم تقم بتعبئة رقم دفتر العائلة للمستفيد ! ولن يقوم البرنامج بحفظ بيانات أفراد العائلة.");
            }
            else
            {
                //// create new family /////
                if (NoFamily)
                {
                    // create family and link to beneficiary directly
                    Family_ID = Insert_Family(FamilyNum_textBox1.Text, Person_ID, "Yes", "مستفيد", Person_ID, "لا يوجد");

                    l.Insert_Log("Insert the Family: " + FamilyNum_textBox1.Text, "Family",
                        Program.UserName, DateTime.Now);

                    l.Insert_Log(
                        "Insert the Beneficiary " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text +
                        " to Family: " + FamilyNum_textBox1.Text, "Person_Family", Program.UserName,
                        DateTime.Now);

                    NoFamily = false;
                }
                //// Update family /////
                else
                {
                    Update_Family(Family_ID);
                    l.Insert_Log("Update the Family: " + FamilyNum_textBox1.Text, "Family",
                        Program.UserName, DateTime.Now);
                }

                // ***  Wife - Hunband *** // 
                Insert_Update_WifeHusband();

                //// *** Insert other family members *** //  only that aren't in database
                /////////////////////////////////////////
                Insert_Update_FamilyMembers();
            }
        }
        private void Insert_Update_WifeHusband()
        {
            /////// wife / husband /////////
            if (FPersonFName_textBox1.Text != ""
                || FPersonLName_textBox1.Text != ""
                || FPersonFatherName_textBox1.Text != "") //there is a wife
            {
                //check the national number of wife/husband
                if (FPersonNationalNum_textBox1.Text != "" && FPersonNationalNum_textBox1.Text != "null")
                    Check_National_Number(FPersonNationalNum_textBox1.Text);

                int marital_status_id = (FPersonMaritalState_comboBox1.SelectedValue != null ?
                    int.Parse(FPersonMaritalState_comboBox1.SelectedValue.ToString()) : -1);

                int status_id = (FPersonState_comboBox1.SelectedValue != null ?
                    int.Parse(FPersonState_comboBox1.SelectedValue.ToString()) : -1);

                if (Husband_Wife_ID == -1)
                {
                    // *** Insert the wife/husband *** //
                    Husband_Wife_ID =
                        Insert_FMember(FPersonFName_textBox1.Text, FPersonLName_textBox1.Text
                        , FPersonFatherName_textBox1.Text, FPersonMotherName_textBox1.Text
                        , FPersonDOB_textBox1.Text, FPersonNationalNum_textBox1.Text, FPersonSex_comboBox1.Text
                        , status_id, marital_status_id, Family_ID, "زوج/ة", Person_ID, "");

                    l.Insert_Log("Insert the wife/husband: " + FPersonFName_textBox1.Text + " " +
                        FPersonLName_textBox1.Text + " to Family: " + FamilyNum_textBox1.Text, "Person_Family",
                        Program.UserName, DateTime.Now);
                }
                else /// udpade wife data
                {
                    /////////////////////////////
                    Update_FMember(Husband_Wife_ID, FPersonFName_textBox1.Text, FPersonLName_textBox1.Text
                        , FPersonFatherName_textBox1.Text, FPersonMotherName_textBox1.Text
                        , FPersonDOB_textBox1.Text, FPersonNationalNum_textBox1.Text, FPersonSex_comboBox1.Text
                        , status_id, marital_status_id, Family_ID, "زوج/ة", "");

                    l.Insert_Log("Update the wife/husband: " + FPersonFName_textBox1.Text + " " +
                        FPersonLName_textBox1.Text + " to Family: " + FamilyNum_textBox1.Text, "Person_Family",
                        Program.UserName, DateTime.Now);
                }
            }
        }
        private bool Check_MicroProject_ID(string MP_ID)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("select count(MP_ID) from `microproject` where MP_ID = " + MP_ID, Program.MyConn);
            var check_MP_ID = Convert.ToInt32(sc.ExecuteScalar());
            Program.MyConn.Close();
            if (check_MP_ID == 0)
            {
                return true; //امورنا بالسليم
            }
            else
            {
                return false; //في حدا تاني بهالرقم
            }
        }
        private void Check_National_Number(string PersonNationalNum)
        {
            var PersonNationalNum_without_zero = "";
            if (PersonNationalNum.ElementAt(0).ToString() == "0")
                PersonNationalNum_without_zero = PersonNationalNum.Remove(0, 1);
            else
                PersonNationalNum_without_zero = PersonNationalNum;

            Program.buildConnection();
            string q = "select count(P_NationalNumber) from `person` " +
                " where P_NationalNumber like '" + PersonNationalNum_without_zero + "'" +
                " or P_NationalNumber like '" + PersonNationalNum + "' ";
            MySS.sc = new MySqlCommand(q, Program.MyConn);
            var check_NationalID = Convert.ToInt32(MySS.sc.ExecuteScalar());
            Program.MyConn.Close();
            if (check_NationalID != 0)
            {
                // Get ID 
                var query = "select CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                            + ",MP.MP_ID 'MicroProject_ID'"
                            + ",P.P_ID 'P_ID'"
                            + " from `microproject` MP inner join person_microproject PMP on PMP.MicroProject_ID = MP.MP_ID "
                            + " inner join person P on P.P_ID = PMP.Person_ID "
                            + " where P_NationalNumber like '" + PersonNationalNum_without_zero + "'"
                            + " or P_NationalNumber like '" + PersonNationalNum + "' ";
                Program.buildConnection();
                var sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);

                Program.MyConn.Close();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        int P_ID = int.Parse(dt.Rows[0]["P_ID"].ToString());
                        int MP_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                        var Name = dt.Rows[0]["Beneficiary Name"].ToString();

                        var _MessageBox = new Application_MessageBox("National_ID",P_ID, MP_ID, Name);
                        var r = _MessageBox.ShowDialog();
                        if (r == DialogResult.Yes)
                        {
                            //////open Old Application in new tab 
                            Form application_Form = new Application_Form(P_ID, MP_ID, mainForm);
                            mainForm.showNewTab(application_Form, "Application", 1);
                        }
                        else if (r == DialogResult.No)
                        {
                            OldBeneficiary_NewProject = true;
                            Person_ID = int.Parse(dt.Rows[0]["P_ID"].ToString());
                            MicroProject_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());

                            Fill_Person();
                            Fill_PersonDetails();
                            Fill_FamilyMembers(0);

                            clear_Project_boxes();
                        }
                        else
                        {
                            //تراجع
                            //Nothing to do//
                            //P_NationalNum_textBox.Focus(); 
                            //Person_ID = MicroProject_ID = -1;
                            //OldBeneficiary_NewProject = false;
                            //P_NationalNum_textBox.Clear();
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        "حدث مشكلة أثناء حفظ البيانات ! الرجاء التأكد من البيانات والمحاولة مرة ثانية.");
                }
            }
        }

        #endregion

        #region fill to Update ---- clear

        private void Fill_Person()
        {
            MySS.dt = s.Person_bind(Person_ID);
            if (MySS.dt != null)
                if (MySS.dt.Rows.Count > 0)
                {
                    Beneficiary_Count++;

                    Person_ID = int.Parse(MySS.dt.Rows[0]["ID"].ToString());
                    P_FirstName_textBox.Text = MySS.dt.Rows[0]["First Name"].ToString();
                    P_LastName_textBox.Text = (string)MySS.dt.Rows[0]["Last Name"];
                    P_FatherName_textBox.Text = (string)MySS.dt.Rows[0]["Father Name"];
                    P_MotherName_textBox.Text = (string)MySS.dt.Rows[0]["Mother Name"];

                    P_FullName_En_textBox.Text = (string)MySS.dt.Rows[0]["Full Name En"];

                    if (MySS.dt.Rows[0]["PersonType"] == DBNull.Value || MySS.dt.Rows[0]["PersonType"] == null)
                        PersonType_comboBox.SelectedIndex = 0;
                    else PersonType_comboBox.SelectedItem = ((string)MySS.dt.Rows[0]["PersonType"] == "مستفيد" ? "المستفيد:" : "الشريك:");

                    if (MySS.dt.Rows[0]["State_ID"] == DBNull.Value || MySS.dt.Rows[0]["State_ID"] == null)
                        P_State_comboBox.SelectedIndex = -1;
                    else P_State_comboBox.SelectedValue = int.Parse(MySS.dt.Rows[0]["State_ID"].ToString());

                    if (MySS.dt.Rows[0]["Birth Date"] == DBNull.Value
                        || MySS.dt.Rows[0]["Birth Date"] == null
                        || MySS.dt.Rows[0]["Birth Date"].ToString() == "0001-01-01"
                        || MySS.dt.Rows[0]["Birth Date"].ToString() == "0000-00-00")
                        P_DOB_textBox.Text = "";
                    else
                    {
                        var date = (DateTime)MySS.dt.Rows[0]["Birth Date"];
                        P_DOB_textBox.Text = date.Year.ToString();
                    }

                    var MilitaryServices = (string)MySS.dt.Rows[0]["Military Services"];
                    if (MilitaryServices.Equals("مسرح")) MilitaryFinished_radioButton.Checked = true;
                    else if (MilitaryServices.Equals("عسكري")) MilitaryNow_radioButton.Checked = true;
                    else if (MilitaryServices.Equals("مؤجل")) MilitaryDelayed_radioButton.Checked = true;
                    else if (MilitaryServices.Equals("وحيد")) MilitarySingle_radioButton.Checked = true;
                    else if (MilitaryServices.Equals("معفى/بدل")) MilitaryExempt_radioButton.Checked = true;
                    else
                    {
                        MilitaryFinished_radioButton.Checked = MilitaryNow_radioButton.Checked
                            = MilitaryDelayed_radioButton.Checked = MilitarySingle_radioButton.Checked
                            = MilitaryExempt_radioButton.Checked = false;
                    }
                    if (MySS.dt.Rows[0]["Military Services Date"] == DBNull.Value
                        || MySS.dt.Rows[0]["Military Services Date"] == null
                        || MySS.dt.Rows[0]["Military Services Date"].ToString() == "0001-01-01"
                        || MySS.dt.Rows[0]["Military Services Date"].ToString() == "0000-00-00")
                    {
                        Military_Date_checkBox.Checked = Military_Date_dateTimePicker.Enabled = false;
                    }
                    else
                    {
                        Military_Date_checkBox.Checked = Military_Date_dateTimePicker.Enabled = true;
                        Military_Date_dateTimePicker.Value = (DateTime)MySS.dt.Rows[0]["Military Services Date"];
                    }

                    Military_textBox.Text = (string)MySS.dt.Rows[0]["Military Services Note"];
                    P_Mobile_textBox.Text = (string)MySS.dt.Rows[0]["Mobile"];
                    P_HomeTel_textBox.Text = (string)MySS.dt.Rows[0]["Land Line"];

                    if (MySS.dt.Rows[0]["Street_ID"] == DBNull.Value || MySS.dt.Rows[0]["Street_ID"] == null)
                        Street_comboBox.SelectedIndex = -1;
                    else Street_comboBox.SelectedValue = int.Parse(MySS.dt.Rows[0]["Street_ID"].ToString());

                    P_Address_textBox.Text = (string)MySS.dt.Rows[0]["Home Address"];
                    if (MySS.dt.Rows[0]["National Number"] != DBNull.Value)
                        P_NationalNum_textBox.Text = (string)MySS.dt.Rows[0]["National Number"];
                    else
                        P_NationalNum_textBox.Text = "";

                    P_Registration_textBox.Text = (string)MySS.dt.Rows[0]["Nationality"];
                    P_Sex_comboBox.Text = (string)MySS.dt.Rows[0]["Gender"];

                    if (MySS.dt.Rows[0]["Marital Status"] == DBNull.Value || MySS.dt.Rows[0]["Marital Status"] == null)
                        P_MaritalState_comboBox.SelectedIndex = -1;
                    else P_MaritalState_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["Marital Status"]);

                    var LiveWithAnotherFamily = (string)MySS.dt.Rows[0]["Is Living With Family"];
                    if (LiveWithAnotherFamily == "Yes") P_LiveWithFamily_radioButton.Checked = true;
                    else if (LiveWithAnotherFamily == "No") P_NotLiveWithFamily_radioButton.Checked = true;
                    else P_LiveWithFamily_radioButton.Checked = P_NotLiveWithFamily_radioButton.Checked = false;

                    LiveWithFamilyDesc_textBox.Text = (string)MySS.dt.Rows[0]["Living With Family Note"];

                    P_NumAtHome_textBox.Text = MySS.dt.Rows[0]["Family members at home"].ToString();

                    var Provider = (string)MySS.dt.Rows[0]["Provider"];
                    if (Provider == "Yes") IsProvider_radioButton.Checked = true;
                    else if (Provider == "No") IsNotProvider_radioButton.Checked = true;
                    else IsProvider_radioButton.Checked = IsNotProvider_radioButton.Checked = false;

                    P_SourceOfIncome_textBox.Text = (string)MySS.dt.Rows[0]["Source Of Income"];
                    P_MedicalCond_textBox.Text = (string)MySS.dt.Rows[0]["Medical Conditions"];

                    if (MySS.dt.Rows[0]["Medical Help"] != DBNull.Value)
                    {
                        var MedicalHelp = (int)MySS.dt.Rows[0]["Medical Help"];
                        if (MedicalHelp == 1) MedicalHelp_radioButton.Checked = true;
                        else if (MedicalHelp == 0) NoMedicalHelp_radioButton.Checked = true;
                        else MedicalHelp_radioButton.Checked = NoMedicalHelp_radioButton.Checked = false;
                    }

                    P_MedicalHelp_textBox.Text = (string)MySS.dt.Rows[0]["Medical Help Note"];
                    P_Loss_textBox.Text = (string)MySS.dt.Rows[0]["Loss"];
                    P_IntermidiaryName_textBox.Text = (string)MySS.dt.Rows[0]["Intermidiary Side"];
                    /////////////////////////////////////////////////////////////////////////////////

                    string HomeState, OtherIncomes;
                    if (MySS.dt.Rows[0]["Home State"] == DBNull.Value || MySS.dt.Rows[0]["Home State"].ToString() == "")
                    {
                        P_HomeRent_radioButton.Checked = P_HomeOwnership_radioButton.Checked = false;
                    }
                    else
                    {
                        HomeState = (string)MySS.dt.Rows[0]["Home State"];
                        if (HomeState == P_HomeRent_radioButton.Text) P_HomeRent_radioButton.Checked = true;
                        else if (HomeState == P_HomeOwnership_radioButton.Text) P_HomeOwnership_radioButton.Checked = true;
                        else P_HomeRent_radioButton.Checked = P_HomeOwnership_radioButton.Checked = false;
                    }

                    HomeDesc_textBox.Text = (string)MySS.dt.Rows[0]["Home State Note"];
                    //////////////////////////////////////////////////

                    if (MySS.dt.Rows[0]["Other Properties"] == DBNull.Value)
                        P_Car_checkBox.Checked = P_Rented_checkBox.Checked = false;
                    else
                    {
                        var OtherProperties = (int)MySS.dt.Rows[0]["Other Properties"];

                        if (OtherProperties == 1) // 1 = سيارةوملك مؤجر
                        {
                            P_Car_checkBox.Checked = P_Rented_checkBox.Checked = true;
                        }
                        else if (OtherProperties == 2) // 2 = سيارة
                        {
                            P_Car_checkBox.Checked = true;
                            P_Rented_checkBox.Checked = false;
                        }
                        else if (OtherProperties == 3) // 3 = ملك مؤجر
                        {
                            P_Car_checkBox.Checked = false;
                            P_Rented_checkBox.Checked = true;
                        }
                        else  // 0 -none of them checked
                        {
                            P_Car_checkBox.Checked = P_Rented_checkBox.Checked = false;
                        }
                    }

                    OtherProperties_textBox.Text = (string)MySS.dt.Rows[0]["Other Properties Note"];

                    if (MySS.dt.Rows[0]["Other Income Sources"] == DBNull.Value)
                    {
                        P_SourceRation_checkBox.Checked = P_SourceRelatives_checkBox.Checked = false;
                    }
                    else
                    {
                        var otherIncome = (int)MySS.dt.Rows[0]["Other Income Sources"];

                        if (otherIncome == 1)
                        {
                            P_SourceRation_checkBox.Checked =
                                P_SourceRelatives_checkBox.Checked = true; // 1 = أقارب و معونة
                        }
                        else if (otherIncome == 2)
                        {
                            P_SourceRelatives_checkBox.Checked = true;
                            P_SourceRation_checkBox.Checked = false;
                        } // 2 = أقارب
                        else if (otherIncome == 3)
                        {
                            P_SourceRelatives_checkBox.Checked = false;
                            P_SourceRation_checkBox.Checked = true;
                        } // 3 = معونة
                        else
                        {
                            P_SourceRation_checkBox.Checked = P_SourceRelatives_checkBox.Checked = false;
                        } // 0 -none of them checked
                    }

                    P_OtherIncomeSources_textBox.Text = MySS.dt.Rows[0]["Other Income Sources Note"].ToString();

                    OtherCourses_textBox.Text = MySS.dt.Rows[0]["Other Courses"].ToString();

                    PowerNote_textBox.Text = MySS.dt.Rows[0]["PowerSource_Note"].ToString();


                    if (MySS.dt.Rows[0]["Parish_ID"] == DBNull.Value || MySS.dt.Rows[0]["Parish_ID"] == null)
                        P_Parish_comboBox.SelectedIndex = -1;
                    else P_Parish_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["Parish_ID"]);

                    if (MySS.dt.Rows[0]["Priest_ID"] == DBNull.Value || MySS.dt.Rows[0]["Priest_ID"] == null)
                        P_Priest_comboBox.SelectedIndex = -1;
                    else P_Priest_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["Priest_ID"]);


                    //profile picture
                    string PicName = "";
                    int Image_ID = -1;
                    if (MySS.dt.Rows[0]["Picture Path"] == DBNull.Value || MySS.dt.Rows[0]["Picture Path"] == null)
                        PicName = "";
                    else
                    {
                        PicName = (string)MySS.dt.Rows[0]["Picture Path"];
                        Image_ID = int.Parse(MySS.dt.Rows[0]["Image_ID"].ToString());
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

                            // if there is missing data between online and offline folders
                            // ///////////////////////////////////////////////////////////////////
                            // لم تعمل هذه التوابع بشكل صحيح
                            //if(Settings.Default.Online && !imageExist_online) 
                            //{
                            //    FtpConnector.UploadFileToLocal(tempDirectory + "\\" + PicName
                            //        , OnlinePicPath
                            //        , Program.OnlineserversCredentials);
                            //} 
                            //if(!Settings.Default.Online && !imageExist_offline)
                            //{
                            //    FtpConnector.DowloadToLocal(tempDirectory + "\\" + PicName
                            //        , OfflinePicPath
                            //        , Program.OfflineserversCredentials);
                            //}
                            // ///////////////////////////////////////////////////////////////////

                            //System.Diagnostics.Process.Start(tempDirectory + "\\" + PicName); 
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
                            FullPicName = FilePicName = "";
                            MessageBox.Show("الملف مفقود - تم حذف الاسم من القائمة");
                        }
                    }
                    else
                    {
                        PersonPicture_pictureBox.Image = Resources.Unknown_User;
                    }
                }

        }

        private DataGridViewComboBoxCell Load_Education_ComboBox(string Edu_type, string Name)
        {

            var str = " select `E_ID`,`E_Name` from `education`";

            var condition = "";
            if (Edu_type != "")
                condition += " where `E_Type` like '%" + Edu_type + "%'";
            else if (Name != "")
                condition += " where `E_Name` like '%" + Name + "%'";
            else condition = "";
            str += condition;

            var orderby = " order by `E_Name`";
            str += orderby;


            //check connection//
            Program.buildConnection();
            MySS.sc = new MySqlCommand(str, Program.MyConn);
            MySS.da = new MySqlDataAdapter();
            MySS.ds = new DataSet();
            var dt = new DataTable();
            MySS.da.SelectCommand = MySS.sc;
            MySS.da.Fill(MySS.ds);
            dt = MySS.ds.Tables[0];

            var cell = new DataGridViewComboBoxCell();
            cell.FlatStyle = FlatStyle.Flat;
            cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            var row = new ArrayList();
            //add items to array list from datatable
            foreach (DataRow dr in dt.Rows) row.Add(dr[1].ToString());
            cell.Items.AddRange(row.ToArray());
            return cell;
        }
        private DataGridViewComboBoxCell Load_AidSource_ComboBox(string Name)
        {

            var str = " select `ID`,`Name` from `aidsource`";
            var condition = "";
            if (Name != "")
                condition += " where `Name` like '%" + Name + "%'";
            str += condition;
            var orderby = " order by `Name`";
            str += orderby;

            //check connection//
            Program.buildConnection();
            MySS.sc = new MySqlCommand(str, Program.MyConn);
            MySS.da = new MySqlDataAdapter();
            MySS.ds = new DataSet();
            var dt = new DataTable();
            MySS.da.SelectCommand = MySS.sc;
            MySS.da.Fill(MySS.ds);
            dt = MySS.ds.Tables[0];

            var cell = new DataGridViewComboBoxCell();
            cell.FlatStyle = FlatStyle.Flat;
            cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            var row = new ArrayList();
            //add items to array list from datatable
            foreach (DataRow dr in dt.Rows) row.Add(dr[1].ToString());
            cell.Items.AddRange(row.ToArray());
            return cell;
        }
        private DataGridViewComboBoxCell Load_PowerSource_ComboBox(string Name)
        {

            var str = " select `ID`,`Name` from `powersource`";
            var condition = "";
            if (Name != "")
                condition += " where `Name` like '%" + Name + "%'";
            str += condition;

            var orderby = " order by `ID`";
            str += orderby;

            //check connection//
            Program.buildConnection();
            MySS.sc = new MySqlCommand(str, Program.MyConn);
            MySS.da = new MySqlDataAdapter();
            MySS.ds = new DataSet();
            var dt = new DataTable();
            MySS.da.SelectCommand = MySS.sc;
            MySS.da.Fill(MySS.ds);
            dt = MySS.ds.Tables[0];

            var cell = new DataGridViewComboBoxCell();
            cell.FlatStyle = FlatStyle.Flat;
            cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            var row = new ArrayList();
            //add items to array list from datatable
            foreach (DataRow dr in dt.Rows) row.Add(dr[1].ToString());
            cell.Items.AddRange(row.ToArray());
            return cell;
        }

        private void Fill_PersonDetails()
        {
            // Education //
            var e_dt = new DataTable();
            e_dt = s.Person_Education_bind(Person_ID);
            var edu_Name = "";
            string edu_Type = "";
            if (e_dt != null || e_dt.Rows.Count > 0)
                for (var i = 0; i < e_dt.Rows.Count; i++)
                {
                    Education_dataGridView.Rows.Add();

                    var ID = e_dt.Rows[i].Field<int>("ID");
                    Education_dataGridView.Rows[i].Cells["Edu_RowID"].Value = ID;

                    // get the data of 1 row
                    edu_Type = e_dt.Rows[i].Field<string>("E_Type");
                    Education_dataGridView.Rows[i].Cells["E_Type"].Value = edu_Type;

                    edu_Name = e_dt.Rows[i].Field<string>("Name");
                    //just in education because we have to select specific datasouce and comboBox according to the type
                    Education_dataGridView.Rows[i].Cells["E_Name"] = Load_Education_ComboBox(edu_Type, "");
                    Education_dataGridView.Rows[i].Cells["E_Name"].Value = edu_Name;

                    //set E_ID and P_ID columns
                    Education_dataGridView.Rows[i].Cells["P_ID"].Value = Person_ID;
                    Education_dataGridView.Rows[i].Cells["E_ID"].Value = e_dt.Rows[i].Field<int>("Education_ID");
                    Education_dataGridView.Rows[i].Cells["Edu_InDataBase"].Value = "1";
                }

            if (!OldBeneficiary_NewProject)
            {
                // Work //
                var w_dt = new DataTable();
                w_dt = s.Person_Work_bind(Person_ID, MicroProject_ID);
                var Work_Name_string = "";
                if (w_dt != null)
                    for (var i = 0; i < w_dt.Rows.Count; i++)
                    {
                        Work_dataGridView.Rows.Add();
                        // get the data of 1 row
                        Work_dataGridView.Rows[i].Cells["W_RowID"].Value = w_dt.Rows[i].Field<int>("ID").ToString();

                        Work_Name_string = w_dt.Rows[i].Field<string>("Name");

                        Work_dataGridView.Rows[i].Cells["W_Name"].Value = w_dt.Rows[i].Field<int>("Work_ID");

                        if (w_dt.Rows[i].Field<string>("Description") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Description") == null)
                            Work_dataGridView.Rows[i].Cells["W_Description"].Value = "";
                        else
                            Work_dataGridView.Rows[i].Cells["W_Description"].Value =
                                w_dt.Rows[i].Field<string>("Description");

                        if (w_dt.Rows[i].Field<string>("Begin Date") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Begin Date") == null)
                            Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value = "0000";
                        else
                            Work_dataGridView.Rows[i].Cells["W_BeginDate"].Value =
                                w_dt.Rows[i].Field<string>("Begin Date");

                        if (w_dt.Rows[i].Field<string>("End Date") == string.Empty ||
                            w_dt.Rows[i].Field<string>("End Date") == null)
                            Work_dataGridView.Rows[i].Cells["W_EndDate"].Value = "0000";
                        else
                            Work_dataGridView.Rows[i].Cells["W_EndDate"].Value =
                                w_dt.Rows[i].Field<string>("End Date");

                        if (w_dt.Rows[i].Field<string>("Place") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Place") == null)
                            Work_dataGridView.Rows[i].Cells["W_Place"].Value = "";
                        else Work_dataGridView.Rows[i].Cells["W_Place"].Value = w_dt.Rows[i].Field<string>("Place");

                        if (w_dt.Rows[i].Field<string>("Place State") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Place State") == null)
                            Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value = "";
                        else
                            Work_dataGridView.Rows[i].Cells["W_PlaceState"].Value =
                                w_dt.Rows[i].Field<string>("Place State");

                        if (w_dt.Rows[i].Field<string>("Description Reason Of Lose") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Description Reason Of Lose") == null)
                            Work_dataGridView.Rows[i].Cells["W_CauseOfLose"].Value = "";
                        else
                            Work_dataGridView.Rows[i].Cells["W_CauseOfLose"].Value =
                                w_dt.Rows[i].Field<string>("Description Reason Of Lose");

                        if (w_dt.Rows[i].Field<Double>("Salary") == 0)
                            Work_dataGridView.Rows[i].Cells["W_Salary"].Value = 0;
                        else
                            Work_dataGridView.Rows[i].Cells["W_Salary"].Value =
                                w_dt.Rows[i].Field<Double>("Salary").ToString("#,##0");

                        Work_dataGridView.Rows[i].Cells["W_ID"].Value = w_dt.Rows[i].Field<int>("Work_ID");
                        Work_dataGridView.Rows[i].Cells["W_InDataBase"].Value = "1";
                    }

                // Experience //
                w_dt = new DataTable();
                w_dt = s.Person_Experience_bind(Person_ID, MicroProject_ID);
                var Exp_Name_string = "";
                if (w_dt != null)
                    for (var i = 0; i < w_dt.Rows.Count; i++)
                    {
                        Exp_dataGridView.Rows.Add();
                        // get the data of 1 row
                        Exp_dataGridView.Rows[i].Cells["Exp_RowID"].Value =
                            w_dt.Rows[i].Field<int>("ID").ToString();

                        Exp_Name_string = w_dt.Rows[i].Field<string>("Name");

                        Exp_dataGridView.Rows[i].Cells["Exp_Name"].Value = w_dt.Rows[i].Field<int>("Work_ID");

                        if (w_dt.Rows[i].Field<string>("Description") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Description") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value = "";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_Description"].Value =
                                w_dt.Rows[i].Field<string>("Description");

                        if (w_dt.Rows[i].Field<string>("Begin Date") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Begin Date") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value = "0000";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_BeginDate"].Value =
                                w_dt.Rows[i].Field<string>("Begin Date");

                        if (w_dt.Rows[i].Field<string>("End Date") == string.Empty ||
                            w_dt.Rows[i].Field<string>("End Date") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value = "0000";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_EndDate"].Value =
                                w_dt.Rows[i].Field<string>("End Date");

                        if (w_dt.Rows[i].Field<string>("Place") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Place") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value = "";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_Place"].Value = w_dt.Rows[i].Field<string>("Place");

                        if (w_dt.Rows[i].Field<string>("Place State") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Place State") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value = "";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_PlaceState"].Value =
                                w_dt.Rows[i].Field<string>("Place State");

                        if (w_dt.Rows[i].Field<string>("Description Reason Of Lose") == string.Empty ||
                            w_dt.Rows[i].Field<string>("Description Reason Of Lose") == null)
                            Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value = "";
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_CauseOfLose"].Value =
                                w_dt.Rows[i].Field<string>("Description Reason Of Lose");

                        if (w_dt.Rows[i].Field<Double>("Salary") == 0)
                            Exp_dataGridView.Rows[i].Cells["Exp_Salary"].Value = 0;
                        else
                            Exp_dataGridView.Rows[i].Cells["Exp_Salary"].Value =
                                w_dt.Rows[i].Field<Double>("Salary").ToString("#,##0");

                        //set W_ID and P_ID columns
                        Exp_dataGridView.Rows[i].Cells["ExpP_ID"].Value = Person_ID;

                        Exp_dataGridView.Rows[i].Cells["Exp_ID"].Value = w_dt.Rows[i].Field<int>("Work_ID");
                        Exp_dataGridView.Rows[i].Cells["Exp_InDataBase"].Value = "1";
                    }
            }
            else
            {
                Work_dataGridView.Rows.Clear();
                Exp_dataGridView.Rows.Clear();
            }


            // AidSource //
            var aid_dt = new DataTable();
            aid_dt = s.Person_AidSource_bind(Person_ID, MicroProject_ID);
            string aid_Name, aid_DateYear, aid_Notes;

            if (aid_dt != null || aid_dt.Rows.Count > 0)
                for (var i = 0; i < aid_dt.Rows.Count; i++)
                {
                    aid_Name = aid_DateYear = aid_Notes = "";
                    aidsource_dataGridView.Rows.Add();

                    var ID = aid_dt.Rows[i].Field<int>("ID");
                    aidsource_dataGridView.Rows[i].Cells["AidSource_RowID"].Value = ID;

                    aid_Name = aid_dt.Rows[i].Field<string>("Name");
                    aidsource_dataGridView.Rows[i].Cells["AidSource_Name"].Value = aid_dt.Rows[i].Field<int>("AidSource_ID");

                    aid_DateYear = aid_dt.Rows[i].Field<string>("DateYear");
                    aidsource_dataGridView.Rows[i].Cells["AidSource_DateYear"].Value = aid_DateYear;

                    aid_Notes = aid_dt.Rows[i].Field<string>("Notes");
                    aidsource_dataGridView.Rows[i].Cells["AidSource_Notes"].Value = aid_Notes;

                    aidsource_dataGridView.Rows[i].Cells["AidSourceP_ID"].Value = Person_ID;
                    aidsource_dataGridView.Rows[i].Cells["AidSource_InDataBase"].Value = "1";
                }


            // PowerSource //
            var dt2 = new DataTable();
            dt2 = s.Person_PowerSource_bind(Person_ID);
            if (dt2 != null || dt2.Rows.Count > 0)
                for (var i = 0; i < dt2.Rows.Count; i++)
                    foreach (Control c in HomePower_tableLayoutPanel.Controls)
                        if (c.GetType() == typeof(CheckBox))
                        {
                            var CBox = c as CheckBox;
                            if (CBox.Text == dt2.Rows[i].Field<string>("Name")) CBox.Checked = true;
                        }


            // Courses //
            var dt1 = new DataTable();
            dt1 = s.Person_Courses_bind(Person_ID);
            if (dt1 != null || dt1.Rows.Count > 0)
                for (var i = 0; i < dt1.Rows.Count; i++)
                    foreach (Control c in Course_tableLayoutPanel.Controls)
                        if (c.GetType() == typeof(CheckBox))
                        {
                            var CBox = c as CheckBox;
                            if (CBox.Text == dt1.Rows[i].Field<string>("Name")) CBox.Checked = true;
                        }

            // Loss //
            var dt = new DataTable();
            dt = s.Person_Loss_bind(Person_ID);
            if (dt != null || dt.Rows.Count > 0)
                for (var i = 0; i < dt.Rows.Count; i++)
                    foreach (Control c in Loss_tableLayoutPanel.Controls)
                        if (c.GetType() == typeof(CheckBox))
                        {
                            var CBox = c as CheckBox;
                            if (CBox.Text == dt.Rows[i].Field<string>("Name")) CBox.Checked = true;
                        }

        }

        private void Fill_FamilyMembers(int x)
        {
            //Reset the nubmer of family members//
            //int NumberOfMembers = 0;

            // FAMILY MEMBERS //
            MySS.dt = new DataTable();
            MySS.dt = s.FamilyMembers_bind(-1, Person_ID);
            var Relation = "";
            FamilyNum_textBox1.Text = "";
            Husband_Wife_ID = -1;
            Family_ID = -1;
            NoFamily = true;

            var j = 0;
            if (MySS.dt != null && MySS.dt.Rows.Count > 0)
            {
                //get family book number and family id from first row (0) //
                if (MySS.dt.Rows[0].Field<string>("Family Number") == null)
                {
                    MessageBox.Show("انتبه ! هذا المستفيد لا يملك رقم دفتر العائلة");
                }
                else
                {
                    FamilyNum_textBox1.Text = MySS.dt.Rows[0].Field<string>("Family Number");
                    NoFamily = false;
                    Family_ID = MySS.dt.Rows[0].Field<int>("Family_ID");
                }

                //fill family members //
                for (var i = 0; i < MySS.dt.Rows.Count; i++)
                {
                    Relation = MySS.dt.Rows[i].Field<string>("Relation");
                    if (Relation.Contains("مستفيد"))
                    {
                        if (x == 0) continue;
                        goto Finish;
                    }

                    if (Relation.Equals("زوج/ة")) //add to husband/wife textboxs   
                    {
                        Husband_Wife_ID = MySS.dt.Rows[i].Field<int>("ID");

                        FPersonFName_textBox1.Text = MySS.dt.Rows[i].Field<string>("FirstName");

                        if (MySS.dt.Rows[i]["Birth Date"] == DBNull.Value
                            || MySS.dt.Rows[i]["Birth Date"] == null
                            || MySS.dt.Rows[i]["Birth Date"].ToString() == "0001-01-01"
                            || MySS.dt.Rows[i]["Birth Date"].ToString() == "0000-00-00")
                            FPersonDOB_textBox1.Text = "";
                        else
                        {
                            var date = MySS.dt.Rows[i].Field<DateTime>("Birth Date");
                            FPersonDOB_textBox1.Text = date.Year.ToString();
                        }

                        if (MySS.dt.Rows[i].Field<string>("LastName") == null)
                            FPersonLName_textBox1.Text = "";
                        else FPersonLName_textBox1.Text = MySS.dt.Rows[i].Field<string>("LastName");

                        if (MySS.dt.Rows[i].Field<string>("FatherName") == null)
                            FPersonFatherName_textBox1.Text = "";
                        else FPersonFatherName_textBox1.Text = MySS.dt.Rows[i].Field<string>("FatherName");

                        if (MySS.dt.Rows[i].Field<string>("MotherName") == null)
                            FPersonMotherName_textBox1.Text = "";
                        else FPersonMotherName_textBox1.Text = MySS.dt.Rows[i].Field<string>("MotherName");

                        if (MySS.dt.Rows[i].Field<string>("National Number") == null)
                            FPersonNationalNum_textBox1.Text = "";
                        else FPersonNationalNum_textBox1.Text = MySS.dt.Rows[i].Field<string>("National Number");

                        FPersonSex_comboBox1.Text = MySS.dt.Rows[i].Field<string>("Gender");

                        if (MySS.dt.Rows[i]["Marital Status"] == DBNull.Value || MySS.dt.Rows[i]["Marital Status"] == null)
                            FPersonMaritalState_comboBox1.SelectedIndex = -1;
                        else FPersonMaritalState_comboBox1.SelectedValue = Convert.ToInt32(MySS.dt.Rows[i]["Marital Status"].ToString());

                        if (MySS.dt.Rows[0]["State_ID"] == DBNull.Value || MySS.dt.Rows[i]["State_ID"] == null)
                            FPersonState_comboBox1.SelectedIndex = -1;
                        else FPersonState_comboBox1.SelectedValue = Convert.ToInt32(MySS.dt.Rows[i]["State_ID"].ToString());
                        //NumberOfMembers++;

                        continue;
                    }

                Finish:
                    {
                        //add to datagridview//
                        FamilyMember_dataGridView.Rows.Add();

                        FamilyMember_dataGridView.Rows[j].Cells["FP_ID"].Value =
                            MySS.dt.Rows[i].Field<int>("ID").ToString();
                        FamilyMember_dataGridView.Rows[j].Cells["F_ID"].Value = Family_ID;

                        FamilyMember_dataGridView.Rows[j].Cells["P_FirstName"].Value =
                            MySS.dt.Rows[i].Field<string>("FirstName");

                        if (MySS.dt.Rows[i].Field<string>("LastName") == null ||
                            MySS.dt.Rows[i].Field<string>("LastName") == null)
                            FamilyMember_dataGridView.Rows[j].Cells["P_LastName"].Value = "";
                        else
                            FamilyMember_dataGridView.Rows[j].Cells["P_LastName"].Value =
                                MySS.dt.Rows[i].Field<string>("LastName");

                        if (MySS.dt.Rows[i]["Birth Date"] == DBNull.Value
                            || MySS.dt.Rows[i]["Birth Date"] == null
                            || MySS.dt.Rows[i]["Birth Date"].ToString() == "0001-01-01"
                            || MySS.dt.Rows[i]["Birth Date"].ToString() == "0000-00-00")
                            FamilyMember_dataGridView.Rows[j].Cells["P_DOB"].Value = "";
                        else
                        {
                            var date = MySS.dt.Rows[i].Field<DateTime>("Birth Date");
                            FamilyMember_dataGridView.Rows[j].Cells["P_DOB"].Value = date.Year.ToString();
                        }

                        FamilyMember_dataGridView.Rows[j].Cells["Relation"].Value = Relation;

                        if (MySS.dt.Rows[i].Field<string>("Work") == null)
                            FamilyMember_dataGridView.Rows[j].Cells["Work_Name"].Value = "";
                        else
                            FamilyMember_dataGridView.Rows[j].Cells["Work_Name"].Value =
                                MySS.dt.Rows[i].Field<string>("Work");

                        if (MySS.dt.Rows[0]["State_ID"] == DBNull.Value || MySS.dt.Rows[i]["State_ID"] == null)
                            FamilyMember_dataGridView.Rows[j].Cells["P_State"].Value = -1;
                        else FamilyMember_dataGridView.Rows[j].Cells["P_State"].Value = Convert.ToInt32(MySS.dt.Rows[i]["State_ID"].ToString());

                        //FamilyMember_dataGridView.Rows[j].Cells["P_State"].Value = MySS.dt.Rows[i].Field<int>("State_ID");

                        FamilyMember_dataGridView.Rows[j].Cells["P_InDataBase"].Value = "1";
                        j++;
                    }
                }
            }
            Calculate_NumAtHome();
        }

        private void Fill_Project()
        {
            MySS.dt = new DataTable();
            MySS.dt = s.MicroProject_bind(Person_ID, MicroProject_ID);
            if (MySS.dt != null)
                if (MySS.dt.Rows.Count > 0)
                {
                    MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                    MPID_textBox.Text = MicroProject_ID.ToString();

                    MP_Name_textBox.Text = (string)MySS.dt.Rows[0]["Project Name"];
                    MP_Name_En_textBox.Text = (string)MySS.dt.Rows[0]["Project Name En"];

                    var price = Convert.ToDouble(MySS.dt.Rows[0]["Requested Amount"].ToString());
                    MP_RequestedAmount_textBox.Text = price.ToString();
                    MP_RequestedTime_textBox.Text = (string)MySS.dt.Rows[0]["Requested Time"];

                    if (MySS.dt.Rows[0]["Application Date"] == DBNull.Value || MySS.dt.Rows[0]["Application Date"].ToString().Contains("0001"))
                        ApplyDate_bcDateTimePicker.Value = DateTime.Now;
                    else
                    {
                        var date = (DateTime)MySS.dt.Rows[0]["Application Date"];
                        ApplyDate_bcDateTimePicker.Value = date;
                    }

                    MP_Description_textBox.Text = (string)MySS.dt.Rows[0]["Description"];
                    var temp = "";

                    //fund type
                    if (MySS.dt.Rows[0]["FundType_ID"] == DBNull.Value || MySS.dt.Rows[0]["FundType_ID"] == null)
                        Loan_radioButton.Checked = Grant_radioButton.Checked = false;
                    else
                    {
                        if ((int)MySS.dt.Rows[0]["FundType_ID"] == 1) Loan_radioButton.Checked = true;
                        else Grant_radioButton.Checked = true;
                    }

                    //project type
                    if (MySS.dt.Rows[0]["Type_ID"] == DBNull.Value || MySS.dt.Rows[0]["Type_ID"] == null)
                        Type_comboBox.SelectedIndex = -1;
                    else
                    {
                        //to trigger the event type_selectedValueChanged//
                        user_mode = true;
                        Type_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["Type_ID"]);
                        if (MySS.dt.Rows[0]["Type"].ToString().Contains("ترميم")
                            || MySS.dt.Rows[0]["Type"].ToString().Contains("زراع"))
                            AddExtention_button.Visible = true;
                        else AddExtention_button.Visible = false;
                        user_mode = false;
                    }
                    //project sub type
                    if (MySS.dt.Rows[0]["SubType_ID"] == DBNull.Value || MySS.dt.Rows[0]["SubType_ID"] == null)
                        SubType_comboBox.SelectedIndex = -1;
                    else SubType_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["SubType_ID"]);

                    //sub category
                    if (MySS.dt.Rows[0]["SubCategory_ID"] == DBNull.Value || MySS.dt.Rows[0]["SubCategory_ID"] == null)
                        SubCategory_comboBox.SelectedIndex = -1;
                    else SubCategory_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["SubCategory_ID"]);

                    //career
                    if (MySS.dt.Rows[0]["Career_ID"] == DBNull.Value || MySS.dt.Rows[0]["Career_ID"] == null)
                        Career_comboBox.SelectedIndex = -1;
                    else Career_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["Career_ID"]);

                    temp = (string)MySS.dt.Rows[0]["Has Previous Donors"];
                    if (temp == "Yes") HasPreviousDonors_radioButton.Checked = true;
                    else if (temp == "No") HasNotPreviousDonors_radioButton.Checked = true;
                    else HasPreviousDonors_radioButton.Checked = HasNotPreviousDonors_radioButton.Checked = false;

                    MP_OtherDonors_textBox.Text = (string)MySS.dt.Rows[0]["Other Donors"];

                    temp = (string)MySS.dt.Rows[0]["needs a license"];
                    if (temp == "Yes") MP_NeedLicense_radioButton.Checked = true;
                    else MP_DontNeedLicense_radioButton.Checked = true;
                    MP_LicenseSide_textBox.Text = (string)MySS.dt.Rows[0]["License Side"];

                    temp = (string)MySS.dt.Rows[0]["Has Competitors"];
                    if (temp == "Yes") HasCompetitors_radioButton.Checked = true;
                    else if (temp == "No") HasNotCompetitors_radioButton.Checked = true;
                    else HasCompetitors_radioButton.Checked = HasNotCompetitors_radioButton.Checked = false;

                    MP_Competitors_textBox.Text = MySS.dt.Rows[0]["Competitors"].ToString();

                    temp = (string)MySS.dt.Rows[0]["Supplies Insurance"];
                    if (temp == "Easy") EasySupplies_radioButton.Checked = true;
                    else if (temp == "Hard") HardSupplies_radioButton.Checked = true;
                    else EasySupplies_radioButton.Checked = HardSupplies_radioButton.Checked = false;

                    MP_Suppliers_textBox.Text = MySS.dt.Rows[0]["Suppliers"].ToString();
                    MP_Risk_textBox.Text = MySS.dt.Rows[0]["Risk"].ToString();
                    MP_Protection_textBox.Text = MySS.dt.Rows[0]["Protection"].ToString();

                    MP_OtherNotes_textBox.Text = (string)MySS.dt.Rows[0]["Other Notes"];
                    MP_Marketing_textBox.Text = (string)MySS.dt.Rows[0]["Marketing"];
                    var num = (double)MySS.dt.Rows[0]["Simple Profit"];
                    MP_SimpleProfit_textBox.Text = num.ToString();
                    MP_SimpleProfit2_textBox.Text = (string)MySS.dt.Rows[0]["Profit Comment"];

                    temp = (string)MySS.dt.Rows[0]["Income Kind"];
                    if (temp == "Single") SingleIncome_radioButton.Checked = true;
                    else if (temp == "Additional") AdditionalIncome_radioButton.Checked = true;
                    else SingleIncome_radioButton.Checked = AdditionalIncome_radioButton.Checked = false;

                    IncomeDesc_textBox.Text = (string)MySS.dt.Rows[0]["Income Kind Note"];

                    temp = (string)MySS.dt.Rows[0]["Has Ledger"];
                    if (temp == "Yes") HasLedger_radioButton.Checked = true;
                    else if (temp == "No") HasNotLedger_radioButton.Checked = true;
                    else HasLedger_radioButton.Checked = HasNotLedger_radioButton.Checked = false;

                    AmperNumber_textBox.Text = ((int)MySS.dt.Rows[0]["Amper Number"]).ToString();

                    var visited = (int)MySS.dt.Rows[0]["Visited"];
                    if (visited == 0) Visit_checkBox.Checked = false;
                    else Visit_checkBox.Checked = true;

                    var IsContentUpdated = (int)MySS.dt.Rows[0]["IsContentUpdated"];
                    if (IsContentUpdated == 0) ContentUpdated_checkBox.Checked = false;
                    else ContentUpdated_checkBox.Checked = true;

                    var message = (int)MySS.dt.Rows[0]["Message"];
                    if (message == 0)
                    {
                        Reject_Message_checkBox.Checked = false;
                        Reject_Message_checkBox.Text = "";
                    }
                    else
                    {
                        Reject_Message_checkBox.Checked = true;
                        if (MySS.dt.Rows[0]["Message Date"] != DBNull.Value)
                        {
                            var date = (DateTime)MySS.dt.Rows[0]["Message Date"];
                            Reject_Message_checkBox.Text = date.ToString("dd/MM/yyyy hh:mm:ss");
                        }
                    }

                    //عرض رأي المفتاحيين
                    int KeyPersonNumber = Convert.ToInt32(MySS.dt.Rows[0]["KeyPerson"].ToString());
                    if (KeyPersonNumber == 0)
                        KeyPerson_checkBox.Checked = false;
                    else KeyPerson_checkBox.Checked = true;

                    //user// 
                    if (MySS.dt.Rows[0]["PaperUser_ID"] == DBNull.Value || MySS.dt.Rows[0]["PaperUser_ID"] == null)
                        PaperApplicationUser_comboBox.SelectedIndex = -1;
                    else PaperApplicationUser_comboBox.SelectedValue = Convert.ToInt32(MySS.dt.Rows[0]["PaperUser_ID"]);

                    if (MySS.dt.Rows[0]["ProgramUser"] != DBNull.Value)
                        CreatedBy_User_label.Text = (string)MySS.dt.Rows[0]["ProgramUser"];
                    else CreatedBy_User_label.Text = "";

                    if (MySS.dt.Rows[0]["EditedByUser"] != DBNull.Value)
                        EditedBy_User_label.Text = (string)MySS.dt.Rows[0]["EditedByUser"];
                    else EditedBy_User_label.Text = "";

                }
        }

        private void Fill_ProjectDetails()
        {
            // PowerSource //
            var ps_dt = new DataTable();
            ps_dt = s.MicroProject_PowerSource_bind(MicroProject_ID);

            if (ps_dt != null || ps_dt.Rows.Count > 0)
                for (var i = 0; i < ps_dt.Rows.Count; i++)
                {
                    PowerSource_dataGridView.Rows.Add();

                    var ID = ps_dt.Rows[i].Field<int>("ID");
                    PowerSource_dataGridView.Rows[i].Cells["Power_RowID"].Value = ID;

                    string Power_Name = ps_dt.Rows[i].Field<string>("Name");
                    //just in education because we have to select specific datasouce and comboBox according to the type
                    PowerSource_dataGridView.Rows[i].Cells["Power_Name"] = Load_PowerSource_ComboBox("");
                    PowerSource_dataGridView.Rows[i].Cells["Power_Name"].Value = Power_Name;

                    // get the data of 1 row
                    string Power_Note = ps_dt.Rows[i].Field<string>("Note");
                    PowerSource_dataGridView.Rows[i].Cells["Power_Note"].Value = Power_Note;

                    string Power_Responsability = ps_dt.Rows[i].Field<string>("Responsability");
                    PowerSource_dataGridView.Rows[i].Cells["Power_Responsability"].Value = Power_Responsability;

                    PowerSource_dataGridView.Rows[i].Cells["Power_Cost"].Value = ps_dt.Rows[i].Field<double>("Cost");

                    //set Power_ID and MP_ID columns
                    PowerSource_dataGridView.Rows[i].Cells["Power_MP_ID"].Value = MicroProject_ID;
                    PowerSource_dataGridView.Rows[i].Cells["Power_ID"].Value = ps_dt.Rows[i].Field<int>("Power_ID");
                    PowerSource_dataGridView.Rows[i].Cells["Power_InDataBase"].Value = "1";
                }

            ////// fill material table  /////////
            MySS.dt = s.Materials_bind(MicroProject_ID);
            if (MySS.dt != null)
                if (MySS.dt.Rows.Count > 0)
                    for (var i = 0; i < MySS.dt.Rows.Count; i++)
                    {
                        Materials_dataGridView.Rows.Add();

                        Materials_dataGridView.Rows[i].Cells["MMP_ID"].Value =
                            MySS.dt.Rows[i].Field<int>("MicroProject_ID").ToString();
                        Materials_dataGridView.Rows[i].Cells["M_ID"].Value =
                            MySS.dt.Rows[i].Field<int>("ID").ToString();
                        Materials_dataGridView.Rows[i].Cells["M_Name"].Value =
                            MySS.dt.Rows[i].Field<string>("Name");

                        if (MySS.dt.Rows[i].Field<double>("Amount") == null ||
                            MySS.dt.Rows[i].Field<double>("Amount").ToString() == null)
                            Materials_dataGridView.Rows[i].Cells["M_Amount"].Value = 0;
                        else
                            Materials_dataGridView.Rows[i].Cells["M_Amount"].Value =
                                MySS.dt.Rows[i].Field<double>("Amount").ToString("#,##0");

                        if (MySS.dt.Rows[i].Field<double>("Price") == null ||
                            MySS.dt.Rows[i].Field<double>("Price").ToString() == null)
                            Materials_dataGridView.Rows[i].Cells["M_Price"].Value = 0;
                        else
                            Materials_dataGridView.Rows[i].Cells["M_Price"].Value =
                                MySS.dt.Rows[i].Field<double>("Price").ToString("#,##0");

                        if (MySS.dt.Rows[i].Field<double>("LocalContribution") == null ||
                            MySS.dt.Rows[i].Field<double>("LocalContribution").ToString() == null)
                            Materials_dataGridView.Rows[i].Cells["M_LocalContribution"].Value = 0;
                        else
                            Materials_dataGridView.Rows[i].Cells["M_LocalContribution"].Value =
                                MySS.dt.Rows[i].Field<double>("LocalContribution").ToString("#,##0");

                        if (MySS.dt.Rows[i].Field<string>("Comments") == null ||
                            MySS.dt.Rows[i].Field<string>("Comments") == null)
                            Materials_dataGridView.Rows[i].Cells["M_Comments"].Value = "";
                        else
                            Materials_dataGridView.Rows[i].Cells["M_Comments"].Value =
                                MySS.dt.Rows[i].Field<string>("Comments");

                        Materials_dataGridView.Rows[i].Cells["M_InDataBase"].Value = "1";
                    }

            //calculate sum of items //
            ///////////////////////////
            calculate_SumOfBudgetItems();

            ////// fill risks /////////
            MySS.dt = s.Risks_bind(MicroProject_ID);
            if (MySS.dt != null)
                if (MySS.dt.Rows.Count > 0)
                    for (var i = 0; i < MySS.dt.Rows.Count; i++)
                    {
                        var risk = MySS.dt.Rows[i].Field<string>("Name");
                        foreach (Control c in Risks_tableLayoutPanel.Controls)
                            if (c.GetType() == typeof(CheckBox))
                            {
                                var CBox = c as CheckBox;
                                if (CBox.Text == risk) CBox.Checked = true;
                            }
                    }

            ////// fill score /////////
            MySS.dt = s.Score_bind(MicroProject_ID);
            if (MySS.dt != null)
                if (MySS.dt.Rows.Count > 0)
                    for (var i = 0; i < MySS.dt.Rows.Count; i++)
                    {
                        var Score_ID = MySS.dt.Rows[i].Field<int>("Score_ID");
                        var Value = MySS.dt.Rows[i].Field<int>("value");

                        switch (Score_ID)
                        {
                            case 1:
                                {
                                    if (Value == 1) s1_radioButton.Checked = true;
                                    else if (Value == 0) n1_radioButton.Checked = true;
                                    else s1_radioButton.Checked = n1_radioButton.Checked = false;
                                    s1_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 2:
                                {
                                    if (Value == 1) s2_radioButton.Checked = true;
                                    else if (Value == 0) n2_radioButton.Checked = true;
                                    else s2_radioButton.Checked = n2_radioButton.Checked = false;
                                    s2_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 3:
                                {
                                    if (Value == 1) s3_radioButton.Checked = true;
                                    else if (Value == 0) n3_radioButton.Checked = true;
                                    else s3_radioButton.Checked = n3_radioButton.Checked = false;
                                    s3_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 4:
                                {
                                    if (Value == 1) s4_radioButton.Checked = true;
                                    else if (Value == 0) n4_radioButton.Checked = true;
                                    else s4_radioButton.Checked = n4_radioButton.Checked = false;
                                    s4_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 5:
                                {
                                    if (Value == 1) s5_radioButton.Checked = true;
                                    else if (Value == 0) n5_radioButton.Checked = true;
                                    else s5_radioButton.Checked = n5_radioButton.Checked = false;
                                    s5_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 6:
                                {
                                    if (Value == 1) s6_radioButton.Checked = true;
                                    else if (Value == 0) n6_radioButton.Checked = true;
                                    else s6_radioButton.Checked = n6_radioButton.Checked = false;
                                    s6_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 7:
                                {
                                    if (Value == 1) s7_radioButton.Checked = true;
                                    else if (Value == 0) n7_radioButton.Checked = true;
                                    else s7_radioButton.Checked = n7_radioButton.Checked = false;
                                    s7_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 8:
                                {
                                    if (Value == 1) s8_radioButton.Checked = true;
                                    else if (Value == 0) n8_radioButton.Checked = true;
                                    else s8_radioButton.Checked = n8_radioButton.Checked = false;
                                    s8_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 9:
                                {
                                    if (Value == 1) s9_radioButton.Checked = true;
                                    else if (Value == 0) n9_radioButton.Checked = true;
                                    else s9_radioButton.Checked = n9_radioButton.Checked = false;
                                    s9_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;

                            case 10:
                                {
                                    if (Value == 1) s0_radioButton.Checked = true;
                                    else if (Value == 0) n0_radioButton.Checked = true;
                                    else s0_radioButton.Checked = n0_radioButton.Checked = false;
                                    s0_textBox.Text = MySS.dt.Rows[i].Field<string>("notes");
                                }
                                break;
                        }
                    }

        }

        private void clear_Person_boxes()
        {
            P_FullName_En_textBox.Text = "";
            user_mode = false;
            PersonType_comboBox.SelectedIndex = 0;
            user_mode = true;

            P_FirstName_textBox.Text = P_LastName_textBox.Text = P_FatherName_textBox.Text = P_MotherName_textBox.Text
                = P_Sex_comboBox.Text = P_MaritalState_comboBox.Text = P_State_comboBox.Text
                = P_NationalNum_textBox.Text = P_DOB_textBox.Text = "";

            MilitaryDelayed_radioButton.Checked = MilitaryFinished_radioButton.Checked
                = MilitaryNow_radioButton.Checked = MilitarySingle_radioButton.Checked
                = MilitaryExempt_radioButton.Checked = Military_Date_checkBox.Checked = false;

            Military_textBox.Text = P_IntermidiaryName_textBox.Text
                = P_Parish_comboBox.Text
                = P_Priest_comboBox.Text = Street_comboBox.Text = "";//need to clear selection too

            P_Priest_comboBox.SelectedIndex = Street_comboBox.SelectedIndex = -1;

            P_Mobile_textBox.Text = P_HomeTel_textBox.Text = P_Registration_textBox.Text = P_Address_textBox.Text
                = LiveWithFamilyDesc_textBox.Text = HomeDesc_textBox.Text
                = OtherProperties_textBox.Text = PowerNote_textBox.Text = "";

            //multiple checkbox select 
            foreach (Control c in HomePower_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    CBox.Checked = false;
                }

            P_LiveWithFamily_radioButton.Checked = P_NotLiveWithFamily_radioButton.Checked
                = P_HomeOwnership_radioButton.Checked = P_HomeRent_radioButton.Checked
                = P_Car_checkBox.Checked = P_Rented_checkBox.Checked = false;

            P_NumAtHome_textBox.Text = FamilyNum_textBox1.Text = "";
            FamilyMember_dataGridView.Rows.Clear();//clear datagrid
            Work_dataGridView.Rows.Clear();
            Exp_dataGridView.Rows.Clear();
            Education_dataGridView.Rows.Clear();

            //multiple checkbox select 
            foreach (Control c in Course_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    CBox.Checked = false;
                }

            OtherCourses_textBox.Text
                = P_SourceOfIncome_textBox.Text = P_OtherIncomeSources_textBox.Text = P_MedicalHelp_textBox.Text
                = P_MedicalCond_textBox.Text = P_Loss_textBox.Text = "";

            IsProvider_radioButton.Checked = IsNotProvider_radioButton.Checked
                 = P_SourceRation_checkBox.Checked = P_SourceRelatives_checkBox.Checked
                 = MedicalHelp_radioButton.Checked = NoMedicalHelp_radioButton.Checked = false;

            //multiple checkbox select 
            foreach (Control c in Loss_tableLayoutPanel.Controls)
                if (c.GetType() == typeof(CheckBox))
                {
                    var CBox = c as CheckBox;
                    CBox.Checked = false;
                }


            // Husband/Wife
            FPersonFName_textBox1.Text = FPersonLName_textBox1.Text = FPersonFatherName_textBox1.Text = FPersonMotherName_textBox1.Text
                = FPersonSex_comboBox1.Text = FPersonMaritalState_comboBox1.Text = FPersonState_comboBox1.Text
                = FPersonNationalNum_textBox1.Text = FPersonDOB_textBox1.Text = "";


            Family_ID = -1;
            Husband_Wife_ID = -1;
            NoFamily = true;

            P_ID_textBox.Text = "";
            Person_ID = -1;
            OldBeneficiary_NewProject = false;

            // image //
            PersonPicture_pictureBox.Image = Resources.Unknown_User;
            PersonPicture_pictureBox.ImageLocation = "";
            ImageLocation_textBox.Text = "";
            FilePicName = FullPicName = "";

            Main_panel.AutoScrollPosition = new Point(0, 0);
        }

        private void clear_Project_boxes()
        {
            MicroProject_ID = -1;
            Save_MP_ID_button.Visible = true;
            Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
            toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

            Beneficiary_Count = 0;
            type = 0;

            Partners_lable.Text = "فردي";

            Visit_checkBox.Checked = KeyPerson_checkBox.Checked
                = Reject_Message_checkBox.Checked = ContentUpdated_checkBox.Checked = false;

            SubType_comboBox.SelectedIndex
                = SubCategory_comboBox.SelectedIndex = Career_comboBox.SelectedIndex = -1;

            MP_Name_En_textBox.Text = MP_Name_textBox.Text
                = MP_RequestedAmount_textBox.Text = MP_RequestedTime_textBox.Text
                = MP_Description_textBox.Text = MP_OtherDonors_textBox.Text = "";

            HasNotPreviousDonors_radioButton.Checked = HasPreviousDonors_radioButton.Checked = false;

            PowerSource_dataGridView.Rows.Clear();
            AmperNumber_textBox.Text = "0";

            //تحليل المشروع
            s1_radioButton.Checked = n1_radioButton.Checked =
            s2_radioButton.Checked = n2_radioButton.Checked =
            s3_radioButton.Checked = n3_radioButton.Checked =
            s4_radioButton.Checked = n4_radioButton.Checked =
            s5_radioButton.Checked = n5_radioButton.Checked =
            s6_radioButton.Checked = n6_radioButton.Checked =
            s7_radioButton.Checked = n7_radioButton.Checked =
            s8_radioButton.Checked = n8_radioButton.Checked =
            s9_radioButton.Checked = n9_radioButton.Checked =
            s0_radioButton.Checked = n0_radioButton.Checked = false;

            s1_textBox.Text = s2_textBox.Text = s3_textBox.Text =
            s4_textBox.Text = s5_textBox.Text = s6_textBox.Text =
            s7_textBox.Text = s8_textBox.Text = s9_textBox.Text =
            s0_textBox.Text = "";

            MP_DontNeedLicense_radioButton.Checked = MP_NeedLicense_radioButton.Checked
                = HasNotCompetitors_radioButton.Checked = HasCompetitors_radioButton.Checked
                = EasySupplies_radioButton.Checked = HardSupplies_radioButton.Checked
                = dangers1_checkBox.Checked = dangers2_checkBox.Checked = dangers3_checkBox.Checked = false;

            MP_LicenseSide_textBox.Text = MP_Competitors_textBox.Text
                = MP_Suppliers_textBox.Text = MP_Risk_textBox.Text = MP_Protection_textBox.Text
                = MP_OtherNotes_textBox.Text = MP_Marketing_textBox.Text
                = MP_SimpleProfit2_textBox.Text = IncomeDesc_textBox.Text = "";

            MP_SimpleProfit_textBox.Text = "0";

            SingleIncome_radioButton.Checked = AdditionalIncome_radioButton.Checked
                = HasLedger_radioButton.Checked = HasNotLedger_radioButton.Checked = false;

            Materials_dataGridView.Rows.Clear();
            OverallSyrian_label.Text = Sum_label.Text = "";

            PaperApplicationUser_comboBox.Text = EditedBy_User_label.Text = CreatedBy_User_label.Text = "";
            PaperApplicationUser_comboBox.SelectedIndex = -1;

            Get_Last_MicroProjectID();
            ApplyDate_bcDateTimePicker.Value = DateTime.Now;

        }

        #endregion

        #region education Work Exp material family

        private void Education_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == Education_dataGridView.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == Education_dataGridView.Columns["DeleteRow"].Index)
            {
                if (!CheckUserPermission()) //if the user don't have the permission to delete
                {
                    MessageBox.Show("Sorry! You don't have the permission to do this action.");
                    return;
                }
                else
                {
                    if (Education_dataGridView["Edu_InDataBase", e.RowIndex].Value == null)
                    {
                        Education_dataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف التحصيل العلمي للمستفيد؟", "Delete",
                            MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var P_ID = Convert.ToInt32(Education_dataGridView.Rows[e.RowIndex].Cells["P_ID"].Value
                                .ToString());
                            var E_ID = Convert.ToInt32(Education_dataGridView.Rows[e.RowIndex].Cells["E_ID"].Value
                                .ToString());
                            var ID = Convert.ToInt32(Education_dataGridView.Rows[e.RowIndex].Cells["Edu_RowID"].Value
                                .ToString());


                            var query = "delete from `person_education` where ID = " + ID;
                            Program.buildConnection();
                            var sc = new MySqlCommand(query, Program.MyConn);
                            sc.ExecuteNonQuery();
                            Program.MyConn.Close();

                            l.Insert_Log("Delete education: " + Education_dataGridView.Rows[e.RowIndex].Cells["E_Name"].Value +
                                " from project: " + MicroProject_ID,
                                "Education", Program.UserName, DateTime.Now);

                            Education_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
        }

        private void Education_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == Education_dataGridView.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == Education_dataGridView.Columns["DeleteRow"].Index)
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

        private void Education_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3) // select the type to fill the education
                {
                }
                if (e.ColumnIndex == 4) //check if type iserted before the name
                {
                    var Edu_Type = "";
                    if (Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value == null ||
                        Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString() == "")
                        throw new InvalidOperationException("الرجاء قم باختيار درجة التحصيل العلمي أولا ثم حاول إدخال مجال الدراسة");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Education_dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4) //type updated and enter to name
                {
                    var Edu_Type = "";
                    if (Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value == null ||
                        Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString() == "")
                        throw new Exception("الرجاء قم باختيار درجة التحصيل العلمي أولا ثم حاول إدخال مجال الدراسة");
                    Edu_Type = Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString();

                    Education_dataGridView.Rows[e.RowIndex].Cells[4] = Load_Education_ComboBox(Edu_Type, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /////////////////////////////////////////////////////////////////////////// 

        private void Work_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == Work_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == Work_dataGridView.Columns["W_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (Work_dataGridView["W_InDataBase", e.RowIndex].Value == null)
                        {
                            Work_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف مجال العمل للمستفيد؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var P_ID = Convert.ToInt32(Work_dataGridView.Rows[e.RowIndex].Cells["WP_ID"].Value
                                    .ToString());
                                var W_ID = Convert.ToInt32(
                                    Work_dataGridView.Rows[e.RowIndex].Cells["W_ID"].Value.ToString());
                                var ID = Convert.ToInt32(Work_dataGridView.Rows[e.RowIndex].Cells["W_RowID"].Value
                                    .ToString());

                                var query = "delete from `person_workexp` where W_Status like 'Yes' and ID = " + ID +
                                            "";
                                Program.buildConnection();
                                var sc = new MySqlCommand(query, Program.MyConn);
                                sc.ExecuteNonQuery();
                                Program.MyConn.Close();

                                l.Insert_Log(
                                    "delete work: " + Work_dataGridView.Rows[e.RowIndex].Cells["W_Name"].Value +
                                    " from project: " + MicroProject_ID,
                                    "Work", Program.UserName, DateTime.Now);


                                Work_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void Work_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == Work_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == Work_dataGridView.Columns["W_DeleteRow"].Index)
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

        private void Work_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            Work_dataGridView.EndEdit();
            if (e.ColumnIndex == Work_dataGridView.Columns["W_Salary"].Index)
            {
                if (Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                    Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value &&
                    Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    string value = Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    Double salary = 0;
                    if (!Double.TryParse(value, out salary))
                    {
                        MessageBox.Show("الرجاء إدخال المبلغ كرقم صحيح!", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    else
                        Work_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = salary.ToString("#,##0");
                }
            }

        }
        //////////////////////////////////////////////////////////////////////////


        private void Exp_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == Exp_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click delete button
                if (e.ColumnIndex == Exp_dataGridView.Columns["Exp_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (Exp_dataGridView["Exp_InDataBase", e.RowIndex].Value == null)
                        {
                            Exp_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف مجال العمل للمستفيد؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var P_ID = Convert.ToInt32(Exp_dataGridView.Rows[e.RowIndex].Cells["ExpP_ID"].Value
                                    .ToString());
                                var W_ID = Convert.ToInt32(Exp_dataGridView.Rows[e.RowIndex].Cells["Exp_ID"].Value
                                    .ToString());
                                var ID = Convert.ToInt32(Exp_dataGridView.Rows[e.RowIndex].Cells["Exp_RowID"].Value
                                    .ToString());

                                var query = "delete from `person_workexp` " +
                                            " where W_Status like 'No' and ID = " + ID + "";
                                Program.buildConnection();
                                var sc = new MySqlCommand(query, Program.MyConn);
                                sc.ExecuteNonQuery();
                                Program.MyConn.Close();
                                l.Insert_Log(
                                    "delete experiance: " + Exp_dataGridView.Rows[e.RowIndex].Cells["Exp_Name"].Value +
                                    " from project: " + MicroProject_ID,
                                    "Experiance", Program.UserName, DateTime.Now);


                                Exp_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void Exp_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == Exp_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == Exp_dataGridView.Columns["Exp_DeleteRow"].Index)
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

        private void Exp_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Exp_dataGridView.EndEdit();
                if (e.ColumnIndex == Exp_dataGridView.Columns["Exp_Salary"].Index)
                {
                    if (Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                        Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value &&
                        Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                    {
                        string value = Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        Double salary = 0;
                        if (!Double.TryParse(value, out salary))
                        {
                            MessageBox.Show("الرجاء إدخال المبلغ كرقم صحيح!", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        }
                        else
                            Exp_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = salary.ToString("#,##0");
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        ///////////////////////////////////////////////////////////////////////////

        private void PowerSource_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == PowerSource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == PowerSource_dataGridView.Columns["Power_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (PowerSource_dataGridView["Power_InDataBase", e.RowIndex].Value == null)
                        {
                            PowerSource_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف مصدر الطاقة للمشروع؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var MP_ID = Convert.ToInt32(PowerSource_dataGridView.Rows[e.RowIndex].Cells["Power_MP_ID"].Value.ToString());
                                var Power_ID = Convert.ToInt32(PowerSource_dataGridView.Rows[e.RowIndex].Cells["Power_ID"].Value.ToString());
                                var Row_ID = Convert.ToInt32(PowerSource_dataGridView.Rows[e.RowIndex].Cells["Power_RowID"].Value.ToString());

                                var query = "DELETE FROM `microproject_powersource` WHERE ID = " + Row_ID + "";
                                Program.buildConnection();
                                var sc = new MySqlCommand(query, Program.MyConn);
                                sc.ExecuteNonQuery();
                                Program.MyConn.Close();

                                l.Insert_Log("DELETE powersource: " + PowerSource_dataGridView.Rows[e.RowIndex].Cells["Power_Name"].Value +
                                    " from project: " + MicroProject_ID,
                                    "powersource", Program.UserName, DateTime.Now);


                                PowerSource_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void PowerSource_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == PowerSource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == PowerSource_dataGridView.Columns["Power_DeleteRow"].Index)
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

        private void PowerSource_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            PowerSource_dataGridView.EndEdit();
            if (e.ColumnIndex == PowerSource_dataGridView.Columns["Power_Cost"].Index)
            {
                if (PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                    PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value &&
                    PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    string value = PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    double cost = 0;
                    if (!double.TryParse(value, out cost))
                    {
                        MessageBox.Show("الرجاء إدخال الكلفة كرقم صحيح!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    else
                        PowerSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cost.ToString("#,##0");
                }
            }

        }

        ///////////////////////////////////////////////////////////////////////////

        private void AidSource_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == aidsource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == aidsource_dataGridView.Columns["AidSource_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (aidsource_dataGridView["AidSource_InDataBase", e.RowIndex].Value == null)
                        {
                            aidsource_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف مصدر الطاقة للمشروع؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var Row_ID = Convert.ToInt32(aidsource_dataGridView.Rows[e.RowIndex].Cells["AidSource_RowID"].Value.ToString());

                                var query = "DELETE FROM `person_aidsource` WHERE ID = " + Row_ID + "";
                                Program.buildConnection();
                                var sc = new MySqlCommand(query, Program.MyConn);
                                sc.ExecuteNonQuery();
                                Program.MyConn.Close();

                                l.Insert_Log("DELETE aidsource: " + aidsource_dataGridView.Rows[e.RowIndex].Cells["AidSource_Name"].Value
                                    + " from project: " + MicroProject_ID, "aidsource", Program.UserName, DateTime.Now);

                                aidsource_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void AidSource_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == aidsource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == aidsource_dataGridView.Columns["AidSource_DeleteRow"].Index)
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

        private void AidSource_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            aidsource_dataGridView.EndEdit();
            if (e.ColumnIndex == aidsource_dataGridView.Columns["AidSource_DateYear"].Index)
            {
                if (aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                    aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value &&
                    aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    string value = aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    int year = 0;
                    if (!int.TryParse(value, out year))
                    {
                        MessageBox.Show("الرجاء إدخال السنة على الشكل التالي 2024!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    else
                        aidsource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = year.ToString("");
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        private void Materials_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == Materials_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == Materials_dataGridView.Columns["M_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (Materials_dataGridView["M_InDataBase", e.RowIndex].Value == null)
                        {
                            Materials_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف المادة من الميزانية؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var M_ID = Convert.ToInt32(Materials_dataGridView.Rows[e.RowIndex].Cells["M_ID"].Value
                                    .ToString());


                                MySS.query = "DELETE FROM `material` WHERE `ID` = " + M_ID;
                                Program.buildConnection();
                                MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                                MySS.sc.ExecuteNonQuery();
                                Program.MyConn.Close();
                                l.Insert_Log(
                                    "delete material: " + Materials_dataGridView.Rows[e.RowIndex].Cells["M_Name"].Value +
                                    " from project: " + MicroProject_ID,
                                    "Material", Program.UserName, DateTime.Now);

                                Materials_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void Materials_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Materials_dataGridView.CurrentCell.Value.ToString()))
                {
                    if (e.ColumnIndex == Materials_dataGridView.Columns["M_Price"].Index
                         || e.ColumnIndex == Materials_dataGridView.Columns["M_LocalContribution"].Index
                         || e.ColumnIndex == Materials_dataGridView.Columns["M_Amount"].Index)
                    {
                        string value = Materials_dataGridView.CurrentCell.Value.ToString();
                        double deci_val = 0;
                        double.TryParse(value, out deci_val);
                        Materials_dataGridView.CurrentCell.Value = deci_val.ToString("#,##0");

                        calculate_SumOfBudgetItems();
                    }
                }
                else Materials_dataGridView.CurrentCell.Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void calculate_SumOfBudgetItems()
        {
            try
            {
                sumOfItemsPrice_SYR = 0;

                for (var RowIndex = 0; RowIndex < Materials_dataGridView.RowCount - 1; RowIndex++)
                {
                    double itemOverAllsyr = 0;
                    if (Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value == null)
                        Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value = "0";

                    if (Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value == null)
                        Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value = "0";

                    if (Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value == null)
                        Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value = "0";

                    var price = Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value.ToString().Replace(",", "");
                    var local = Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value.ToString().Replace(",", "");
                    var amount = Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value.ToString().Replace(",", "");

                    Double AllPrice = Double.Parse(price) * Int64.Parse(amount);
                    itemOverAllsyr = AllPrice - Int64.Parse(local);

                    //save the overAll for each item
                    sumOfItemsPrice_SYR += itemOverAllsyr;
                    OverallSyrian_label.Text = sumOfItemsPrice_SYR.ToString("#,##0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Materials_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == Materials_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == Materials_dataGridView.Columns["M_DeleteRow"].Index)
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

        ///////////////////////////////////////////////////////////////////////////

        private void FamilyMember_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                FamilyMember_dataGridView.EndEdit();
                if (e.ColumnIndex == FamilyMember_dataGridView.Columns["P_DOB"].Index)
                {
                    if (FamilyMember_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                        && FamilyMember_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value
                        && FamilyMember_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                    {
                        string value = FamilyMember_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        int date = 0000;
                        int.TryParse(value, out date);
                        if (date < 1900)
                        {
                            MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            FamilyMember_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        }
                    }
                }
                Calculate_NumAtHome();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void Calculate_NumAtHome()
        {
            try
            {
                // beneficiary
                int NumberOfMembers = 0;
                if (!string.IsNullOrWhiteSpace(P_FirstName_textBox.Text)) NumberOfMembers++;

                //  husband/wife
                if (!string.IsNullOrWhiteSpace(FPersonFName_textBox1.Text)
                    && FPersonState_comboBox1.SelectedIndex != -1)
                {
                    string value = FPersonState_comboBox1.SelectedValue.ToString();
                    DataRow[] rows = null; int id;
                    if (int.TryParse(value, out id)) rows = ds.Tables[6].Select("ID=" + id);
                    else rows = ds.Tables[6].Select("Name='" + value + "'");
                    if (rows.Length != 0)
                    {
                        if (rows[0].Field<string>(2).ToLower() == "active")
                            NumberOfMembers++;
                    }
                }

                if (FamilyMember_dataGridView.Rows.Count > 0)
                    for (int i = 0; i < FamilyMember_dataGridView.Rows.Count; i++)
                    {
                        FamilyMember_dataGridView["F_Number", i].Value = FamilyNum_textBox1.Text;

                        if (FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == null ||
                            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == DBNull.Value ||
                            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString() == "")
                            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value = 1; //مقيم

                        if (FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value == null ||
                            FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value == DBNull.Value ||
                            FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value.ToString() == "")
                            continue;

                        string value = FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString();

                        DataRow[] rows = null; int id;
                        if (int.TryParse(value, out id)) rows = ds.Tables[6].Select("ID=" + id);
                        else rows = ds.Tables[6].Select("Name='" + value + "'");

                        if (rows.Length != 0)
                        {
                            if (rows[0].Field<string>(2).ToLower() == "active")
                                NumberOfMembers++;
                        }
                    }
                P_NumAtHome_textBox.Text = NumberOfMembers.ToString();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void FamilyMember_dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            FamilyMember_dataGridView["F_Number", e.RowIndex].Value = FamilyNum_textBox1.Text;
            FamilyMember_dataGridView["P_State", e.RowIndex].Value = 1;
            FamilyMember_dataGridView.Refresh();
        }

        private void FamilyMember_dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Calculate_NumAtHome();
            FamilyMember_dataGridView.Refresh();
        }

        private void FamilyMembers_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == FamilyMember_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == FamilyMember_dataGridView.Columns["P_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (FamilyMember_dataGridView.Rows[e.RowIndex].Cells["P_InDataBase"].Value == null)
                        {
                            FamilyMember_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف فرد من العائلة؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                //get person ID
                                var Member_ID = Convert.ToInt32(FamilyMember_dataGridView.Rows[e.RowIndex].Cells["FP_ID"]
                                    .Value.ToString());
                                var F_ID = Convert.ToInt32(FamilyMember_dataGridView.Rows[e.RowIndex].Cells["F_ID"].Value
                                    .ToString());


                                MySS.query = "DELETE FROM `person` WHERE `P_ID` = " + Member_ID;
                                Program.buildConnection();
                                MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                                MySS.sc.ExecuteNonQuery();
                                Program.MyConn.Close();

                                l.Insert_Log(
                                    "delete family member : " +
                                    FamilyMember_dataGridView.Rows[e.RowIndex].Cells["P_FirstName"].Value + " " +
                                    FamilyMember_dataGridView.Rows[e.RowIndex].Cells["P_LastName"].Value,
                                    "Beneficiary_Family", Program.UserName, DateTime.Now);


                                FamilyMember_dataGridView.Rows.RemoveAt(e.RowIndex);
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

        private void FamilyMembers_DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == FamilyMember_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == FamilyMember_dataGridView.Columns["P_DeleteRow"].Index)
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

        /////////////////////////////////////////////////////////////////////////// 
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /// ### ///  /// ### ///  /// ### ///  /// ### ///  /// ### ///
        #endregion

        #region checkbox changes 
        private void Visit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (MicroProject_ID == -1 && Visit_checkBox.Checked)
                    {
                        Visit_checkBox.Checked = false;
                        throw new InvalidOperationException("الرجاء حفظ المشروع أولاً ثم يمكنك التعديل على هذه الخلية.");
                    }

                    //Update project just in visit cell// 
                    var state = "";

                    if (Visit_checkBox.Checked == false) //not visited
                    {
                        mp.Update_Project(MicroProject_ID, "MP_Visited", "0", "");
                        toolTip1.SetToolTip(Visit_checkBox, "غير مزار حتى الآن");
                        state = "Not Visited";
                    }
                    else // visited
                    {
                        mp.Update_Project(MicroProject_ID, "MP_Visited", "1", "");
                        toolTip1.SetToolTip(Visit_checkBox, "تمت زيارته");
                        state = "Visited";
                    }

                    l.Insert_Log("Update the project: " + MicroProject_ID + " SET visited to:" + state + "",
                        "micro project", Program.UserName, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reject_Message_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (MicroProject_ID == -1 && Reject_Message_checkBox.Checked)
                    {
                        Reject_Message_checkBox.Checked = false;
                        throw new InvalidOperationException("الرجاء حفظ المشروع أولاً ثم يمكنك التعديل على هذه الخلية.");
                    }
                    var query = "";
                    var state = "";

                    if (Reject_Message_checkBox.Checked == false)
                    {
                        mp.Update_Project(MicroProject_ID, "MP_Message", "0", "MP_MessageDate = null");
                        toolTip1.SetToolTip(Reject_Message_checkBox, "لم يتم إرسال رسالة رفض حتى الآن");
                        state = " Delete Sent Message";
                        Reject_Message_checkBox.Text = "";
                    }
                    else
                    {
                        mp.Update_Project(MicroProject_ID, "MP_Message", "1", "MP_MessageDate = '" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "'");
                        toolTip1.SetToolTip(Reject_Message_checkBox, "تم إرسال رسالة الرفض");
                        state = " Message Sent";
                        Reject_Message_checkBox.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

                    }

                    l.Insert_Log("Update the project: " + MicroProject_ID + state, "micro project",
                        Program.UserName, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    var query = "";
                    var state = "";

                    User user1 = new User();
                    UserNotification noti1 = new UserNotification();

                    if (ContentUpdated_checkBox.Checked == false)
                    {
                        mp.Update_Project(MicroProject_ID, "IsContentUpdated", "0", "");
                        toolTip1.SetToolTip(ContentUpdated_checkBox, "المشروع غير معدل");
                        state = " Project content Is Not updated";

                        noti1.Delete_Notification(
                            MicroProject_ID
                           , DateTime.Today.ToString("yyyy/MM/dd")
                           , "تم تبديل المشروع إلى: " + MP_Name_textBox.Text
                           , P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + " ابن/ة " + P_FatherName_textBox.Text, -1);
                    }
                    else
                    {
                        mp.Update_Project(MicroProject_ID, "IsContentUpdated", "1", "");
                        toolTip1.SetToolTip(ContentUpdated_checkBox, "تم تبديل المشروع");
                        state = " Project content updated";

                        //Send Notification to all users if the status changned - except rejected & waiting  
                        var user_db = user1.Select_Users("", "", " and role.Role_Name Not like 'Out of Service' and role.Role_Name Not like 'Guest'");
                        if (user_db != null)
                            for (var i = 0; i < user_db.Rows.Count; i++)
                                noti1.Insert_UserNotification(
                                    DateTime.Today.ToString("yyyy/MM/dd")
                                    , "تم تبديل المشروع إلى: " + MP_Name_textBox.Text
                                    , P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + " ابن/ة " + P_FatherName_textBox.Text
                                    , MicroProject_ID
                                    , user_db.Rows[i].Field<int>(0)
                                    , Program.userID
                                    , -998 + i);
                    }

                    l.Insert_Log("Update the project: " + MicroProject_ID + ":" + state, "micro project",
                        Program.UserName, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        //done 
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
                    PersonPicture_pictureBox.ImageLocation = FullPicName;
                    ImageLocation_textBox.Text = FullPicName;
                    if (Save_Attachments(FullPicName)) success++;

                    MessageBox.Show("تم حفظ الصورة بنجاح");
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
                //Update_PersonPicture(Person_ID, new_name);
                img.InsertProfile(MicroProject_ID
                    , new_name
                    , Person_ID
                    , P_FirstName_textBox.Text + " " + P_LastName_textBox.Text); //profile

                //l.Insert_Log("Upload Image of:" + MicroProject_ID + ":" + P_FirstName_textBox.Text + " " +
                //    P_LastName_textBox.Text + " to the server", "Person", Program.UserName, DateTime.Now); 
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
                        FullPicName = FilePicName = "";
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

                image_state = 1;
            }
            else
                return;
        }

        #endregion
        //done
        #region combo texbox selection change
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
        private void FamilyNum_textBox1_TextChanged(object sender, EventArgs e)
        {
            if (FamilyNum_textBox1.Text != "")
                CanAddFamilyMembers = true;
            else
                CanAddFamilyMembers = false;
        }
        private void FamilyNum_textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!CanAddFamilyMembers)
                    throw new InvalidOperationException("لا يمكن ترك رقم دفتر العائلة فارغ !");

                //clear unused dummy families//
                string q = "DELETE from family WHERE family.F_ID not in (SELECT person_family.Family_ID from person_family);";
                //check if family is already inserted// 
                q += "select count(F_Number) from `family` where F_Number like '" + FamilyNum_textBox1.Text + "';";

                Program.buildConnection();
                MySS.sc = new MySqlCommand(q, Program.MyConn);
                var FamilyBookID = Convert.ToInt32(MySS.sc.ExecuteScalar());
                Program.MyConn.Close();
                if (FamilyBookID != 0)
                {
                    // get beneficiary names of this family

                    var query =
                        @"select CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'الاسم الثلاثي'
,PMP.Person_ID as 'ID' , PMP.MicroProject_ID as 'MicroProject_ID' 
From `person` p right outer join `person_family` pf on p.P_ID = pf.Person_ID 
left outer join `family` f on f.F_ID = pf.Family_ID 
left outer join `person_microproject` PMP on PMP.Person_ID = p.P_ID 
where f.F_Number like '" + FamilyNum_textBox1.Text + "' and pf.Relation like 'مستفيد'";
                    Program.buildConnection();
                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    var name = MySS.sc.ExecuteScalar().ToString();
                    Program.MyConn.Close();

                    //Beneficiary_In_Existing_Family = true;

                    var r = MessageBox.Show(
                        "رقم دفتر العائلة موجود مسبقاً ! " + Environment.NewLine +
                        " المستفيد التابع لدفتر العائلة هذا هو: " + name, "Dublicate", MessageBoxButtons.OK);
                    if (r == DialogResult.OK)
                    {
                        //int P_ID = Int32.Parse(dt.Rows[0]["P_ID"].ToString());
                        //int MP_ID = Int32.Parse(dt.Rows[0]["MicroProject_ID"].ToString());

                        ////////open new tab 
                        //Form application_Form = new Application_Form(Person_ID, MicroProject_ID, mainForm);
                        //mainForm.showNewTab(application_Form, "Application");

                        //Fill_FamilyMembers(1);
                        //FamilyMember_dataGridView.Refresh();
                    }
                }
                else
                {
                    Calculate_NumAtHome();
                    FamilyMember_dataGridView.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void P_Sex_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (P_Sex_comboBox.Text == "أنثى")
            {
                //Military_label.Visible = Military_label2.Visible = Military_textBox.Visible =

                Military_tableLayoutPanel.Visible = false;
                FPersonSex_comboBox1.SelectedItem = "ذكر";
            }
            else
            {
                //Military_label.Visible = Military_label2.Visible = Military_textBox.Visible =

                Military_tableLayoutPanel.Visible = true;
                FPersonSex_comboBox1.SelectedItem = "أنثى";
            }
        }
        private void MilitaryFinished_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (MilitaryFinished_radioButton.Checked)
                Military_Date_dateTimePicker.Visible = Military_Date_checkBox.Visible = true;
            else
                Military_Date_dateTimePicker.Visible = Military_Date_checkBox.Visible = false;
        }

        private void FPersonSex_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FPersonSex_comboBox1.Text == "أنثى")
            {
                P_Sex_comboBox.SelectedItem = "ذكر";
            }
            else
            {
                P_Sex_comboBox.SelectedItem = "أنثى";
            }
        }
        private void P_MaritalState_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (P_MaritalState_comboBox.Text == "متزوج/ة" || P_MaritalState_comboBox.Text == "منفصل/ة")
                {
                    FPersonState_comboBox1.SelectedValue = P_State_comboBox.SelectedValue = 1; //مقيم 
                    FPersonMaritalState_comboBox1.SelectedValue = P_MaritalState_comboBox.SelectedValue;
                }
                else if (P_MaritalState_comboBox.Text == "أرمل/ة")
                {
                    DataRow[] rows = null; int id;
                    rows = ds.Tables[6].Select("Name='" + "متوفي/ة" + "'");
                    if (rows.Length != 0)
                        FPersonState_comboBox1.SelectedValue = rows[0].Field<int>(0);
                }
                else { }
            }
            Calculate_NumAtHome();
        }
        private void P_State_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FPersonState_comboBox1.Text == "متوفي/ة")
            {
                DataRow[] rows = null; int id;
                rows = ds.Tables[7].Select("Name='" + "أرمل/ة" + "'");
                if (rows.Length != 0)
                {
                    P_MaritalState_comboBox.SelectedValue = rows[0].Field<int>(0);
                }
            }
            Calculate_NumAtHome();
        }
        private void Grant_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            P_Priest_comboBox.Enabled = P_IntermidiaryName_textBox.Enabled =
                s4_textBox.Enabled = MP_Marketing_textBox.Enabled =
                MP_NeedLicense_radioButton.Enabled = MP_DontNeedLicense_radioButton.Enabled =
                MP_LicenseSide_textBox.Enabled = true;

            P_IntermidiaryName_label.Enabled = MP_Marketing_lable.Enabled =
                P_Priest_label.Enabled = AddPriest_button.Enabled = MP_NeedLicense_label.Enabled = true;

            if (Loan_radioButton.Checked)
            {
                type = 0;
                TrainingOthers_lable.Text = "هل توافق على الالتزام بدفتر حسابات يمكننا الاطلاع عليه في حال قبول المشروع؟";
            }
            else
            {
                type = 1;

                P_Priest_comboBox.Enabled = P_IntermidiaryName_textBox.Enabled =
                    s4_textBox.Enabled = MP_Marketing_textBox.Enabled =
                        MP_NeedLicense_radioButton.Enabled = MP_DontNeedLicense_radioButton.Enabled =
                            MP_LicenseSide_textBox.Enabled = false;

                P_IntermidiaryName_label.Enabled = P_Priest_label.Enabled =
                    AddPriest_button.Enabled = MP_Marketing_lable.Enabled = MP_NeedLicense_label.Enabled = false;
                TrainingOthers_lable.Text = "هل توافق على تدريب أشخاص في هذه المهنة؟";
            }
        }

        private void Category_comboBox_Enter(object sender, EventArgs e)
        {
            //Category_bind();
        }

        private void MPID_textBox_TextChanged(object sender, EventArgs e)
        {
            mainForm.MP_ID_label.Text = MPID_textBox.Text;
        }

        private void MP_RequestedAmount_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (MP_RequestedAmount_textBox.Text != "")
                {
                    MP_RequestedAmount_textBox.Text =
                        Regex.Replace(
                            string.Format("{0:n" + 4 + "}", Convert.ToDecimal(MP_RequestedAmount_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    MP_RequestedAmount_textBox.SelectionStart = MP_RequestedAmount_textBox.Text.Length;
                    MP_RequestedAmount_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("لا يمكن إدخال أحرف ضمن هذه الخلية، تأكد من إدخال أرقام فقط.");
            }
        }

        private void OverallSyrian_label_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (OverallSyrian_label.Text != "")
                    OverallSyrian_label.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(OverallSyrian_label.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                //OverallSyrian_label.SelectionStart = OverallSyrian_label.Text.Length;
                //OverallSyrian_label.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MP_SimpleProfit_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (MP_SimpleProfit_textBox.Text != "")
                {
                    MP_SimpleProfit_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDouble(MP_SimpleProfit_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    MP_SimpleProfit_textBox.SelectionStart = MP_SimpleProfit_textBox.Text.Length;
                    MP_SimpleProfit_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PersonDOB_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != '.' && !(char.IsNumber(e.KeyChar) || e.KeyChar == 8))
                e.Handled = true;
        }

        private void PersonDOB_textBox_Leave(object sender, EventArgs e)
        {
            if (FPersonDOB_textBox1.Text != "")
            {
                var date = Convert.ToInt32(FPersonDOB_textBox1.Text);
                if (date < 1900)
                {
                    MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    FPersonDOB_textBox1.Text = "";
                }
            }

            if (P_DOB_textBox.Text != "")
            {
                var date = Convert.ToInt32(P_DOB_textBox.Text);
                if (date < 1900 || date > DateTime.Now.Year)
                {
                    MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    P_DOB_textBox.Text = "";
                }
                if (DateTime.Now.Year - date < 18) // منع ادخال استمارة قاصرين
                {
                    MessageBox.Show("انتبه! المستفيد تحت السن القانوني", "Date error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        private void PersonNationalNum_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(P_NationalNum_textBox.Text))
                {
                    if (P_NationalNum_textBox.Text.Length != 11)
                        MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات الرقم الوطني لا تساوي 11"
                            , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Check_National_Number(P_NationalNum_textBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// done
        private void SubCategory_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (SubCategory_comboBox.SelectedIndex != -1 || SubCategory_comboBox.Text != "")
                    {
                        Career_bind(SubCategory_comboBox.SelectedValue.ToString());
                        Career_comboBox.Visible = true;
                    }
                    else
                    {
                        Career_comboBox.DataSource = null;
                        Career_comboBox.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// done
        private void Type_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    SubType_bind(Type_comboBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Military_Date_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Military_Date_checkBox.Checked)
                Military_Date_dateTimePicker.Enabled = true;
            else Military_Date_dateTimePicker.Enabled = false;
        }

        private void AmperNumber_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region new buttons

        private void NewEducation_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var AddNewEducation = new AddNewEducation())
                {

                    var r = AddNewEducation.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        //refresh only this datatable -_-
                        Bind_Gridview_ComboBox(1); //education
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewWork_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var AddNewExperience = new AddNewExperience())
                {

                    var r = AddNewExperience.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        //refresh only this datatable -_-
                        Bind_Gridview_ComboBox(2); //exp - work
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddPriest_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var AddNewPriest = new AddNewPriest())
                {
                    //AddNewPriest.ShowDialog();
                    var r = AddNewPriest.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        //refresh only this datatable -_-
                        Bind_All_ComboBoxes(3); //priest
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        //done
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

        private void AddPriest_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_L_24;
            else image = Resources.Plus_Sq_D_24;

            AddPriest_button.BackgroundImage = image;
        }

        private void AddPriest_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_D_24;
            else image = Resources.Plus_Sq_L_24;

            AddPriest_button.BackgroundImage = image;
        }

        private void NewEducation_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_L_24;
            else image = Resources.Plus_Sq_D_24;

            AddEducation_button.BackgroundImage = image;
        }

        private void NewEducation_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_D_24;
            else image = Resources.Plus_Sq_L_24;

            AddEducation_button.BackgroundImage = image;
        }

        private void Save_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Save2_L;
            else image = Resources.Save2_D;

            Save_button.BackgroundImage = image;
        }

        private void Save_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Save2_D;
            else image = Resources.Save2_L;

            Save_button.BackgroundImage = image;
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
            if (Settings.Default.theme == "Light")
                image = Resources.AddPartner_L;
            else image = Resources.AddPartner_D;

            AddPartner_button.BackgroundImage = image;
        }

        private void AddPartner_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.AddPartner_D;
            else image = Resources.AddPartner_L;

            AddPartner_button.BackgroundImage = image;
        }

        private void Extention_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.AddExtention_L;
            else image = Resources.AddExtention_D;

            AddExtention_button.BackgroundImage = image;
        }

        private void Extention_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.AddExtention_D;
            else image = Resources.AddExtention_L;

            AddExtention_button.BackgroundImage = image;
        }

        private void NewWork_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_L_24;
            else image = Resources.Plus_Sq_D_24;

            AddWork_button.BackgroundImage = image;
        }

        private void NewWork_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Plus_Sq_D_24;
            else image = Resources.Plus_Sq_L_24;

            AddWork_button.BackgroundImage = image;
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
        #endregion

        /// done
        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        /// done
        private void MPID_textBox_Leave(object sender, EventArgs e)
        {
            //عند قيام المستخدم بتغيير رقم المشروع//
            try
            {
                if (Save_MP_ID_button.BackgroundImage == Properties.Resources.Cheked || MicroProject_ID != -1)
                {
                    if (MPID_textBox.Text == "")
                        MessageBox.Show("لا يمكن ترك خلية رقم المشروع فارغة!");
                    else
                    {
                        var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد تغيير رقم المشروع من: " +
                            MicroProject_ID.ToString() + " ؟ " + Environment.NewLine +
                            "ليصبح الرقم: " + MPID_textBox.Text, "Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //check if this number exist//
                            if (Check_MicroProject_ID(MPID_textBox.Text))
                            {
                                int New_MP_ID = Convert.ToInt32(MPID_textBox.Text);
                                Edit_MP(MicroProject_ID, New_MP_ID);
                                l.Insert_Log("Update the project_ID from:" + MicroProject_ID + " to:" + MPID_textBox.Text, "Micro Project", Program.UserName, DateTime.Now);

                                MicroProject_ID = New_MP_ID;
                                MessageBox.Show("تم تعديل رقم المشروع بنجاح");
                            }
                            else
                            {
                                MessageBox.Show("الرجاء استخدام رقم مشروع آخر لأن هذا الرقم مستخدم من قبل مشروع آخر");
                                MPID_textBox.Text = MicroProject_ID.ToString();
                                MPID_textBox.Focus();
                            }
                        }
                        else
                        {
                            MPID_textBox.Text = MicroProject_ID.ToString();
                            //MPID_textBox.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// done
        private void P_Mobile_textBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(P_Mobile_textBox.Text))
            {
                if (P_Mobile_textBox.Text.Length != 10)
                    MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات رقم الجوال لا تساوي 10"
                        , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// done
        private void PersonType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                string message;
                if (PersonType_comboBox.SelectedItem == "المستفيد:")
                    message = "انتبه! سوف يتم تغيير صفة الشخص من شريك إلى مستفيد";
                else message = "انتبه! سوف يتم تغيير صفة الشخص من مستفيد إلى شريك";

                MessageBox.Show(message
                    , "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Done
        #region downloan image
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

        //done
        private void P_FirstName_textBox_Leave(object sender, EventArgs e)
        {
            Calculate_NumAtHome();
        }

        private void Application_Form_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Application_Form_Shown(object sender, EventArgs e)
        {
            Stop_Highlighting_Combobox();
        }

        #region delete context menu strip
        private void Delete_MP_button_Click(object sender, EventArgs e)
        {
            delete_contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
        }
        private void delete_person_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف بيانات المستفيد مع المحافظة على شركاء (إن وجد) والمحافظة على رقم المشروع؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (Person_ID != -1)
                    {
                        Delete_Person(Person_ID);
                        l.Insert_Log("Delete " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ",
                            Program.UserName, DateTime.Now);

                        clear_Person_boxes();
                        Person_ID = -1;
                        Save_MP_ID_button.Visible = true;

                        //hide delete-partner-extention buttons on Create
                        AddExtention_button.Visible = AddPartner_button.Visible = Delete_MP_button.Visible = false;

                        //update mode is on because we didn't delete the project
                        Update_Mode = true;

                        MessageBox.Show("تم حذف المستفيد مع كل بياناته الشخصية والعائلية بنجاح");
                    }
                }
                else
                {
                    MessageBox.Show(
                            "لا يمكن حذف المستفيد! الرجاء اختيار استمارة من قائمة البحث أولاً ثم المحاولة مرة ثانية.");
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
                CheckUserPermission();
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف الاستمارة بشكل كامل مع كل بيانات المشروع والمستفيدين والشركاء؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (MicroProject_ID != -1)
                    {
                        Delete_MP(MicroProject_ID);
                        l.Insert_Log("Delete the project " + MicroProject_ID, "Micro Project", Program.UserName, DateTime.Now);

                        if (Person_ID != -1)
                        {
                            Delete_Person(Person_ID);
                            l.Insert_Log("Delete " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ",
                                Program.UserName, DateTime.Now);
                        }

                        clear_Person_boxes();
                        clear_Project_boxes();
                        Person_ID = -1;
                        MicroProject_ID = -1;

                        Save_MP_ID_button.Visible = true;
                        Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                        toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

                        //hide delete-partner-extention buttons on Create
                        AddExtention_button.Visible = AddPartner_button.Visible = Delete_MP_button.Visible = false;

                        Update_Mode = false;
                        mainForm.TabName_label.Text = "Application";
                        mainForm.MP_ID_label.Text = MPID_textBox.Text;

                        MessageBox.Show("تم حذف الاستمارة بنجاح");
                    }
                    else
                    {
                        MessageBox.Show(
                                "لا يمكن حذف المشروع! الرجاء اختيار مشروع من قائمة البحث أولاً ثم المحاولة مرة ثانية.");
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
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var _Form = new ChooseVisitKind_Form(MicroProject_ID, mainForm);
            _Form.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var _Form = new Loans_Form(MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "Loans", 0);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
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

        private void Save_MP_ID_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save_MP_ID_button.BackgroundImage == Properties.Resources.Cheked)
                {
                    var dialogResult = MessageBox.Show("هل تريد حذف المشروع ذي الرقم: " + MPID_textBox.Text, "Delete", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Delete_MP(MicroProject_ID);
                        l.Insert_Log("Delete the project " + MicroProject_ID, "Micro Project", Program.UserName, DateTime.Now);

                        clear_Project_boxes();
                        Person_ID = -1;
                        MicroProject_ID = -1;
                        EditedBy_User_label.Text = "";

                        Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                        toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

                        Update_Mode = false;
                        mainForm.TabName_label.Text = "Application";
                        mainForm.MP_ID_label.Text = MPID_textBox.Text;
                    }
                }
                else
                {
                    if (MPID_textBox.Text == "")
                        throw new InvalidOperationException("لا يمكن حجز الرقم. الرجاء التأكد من الرقم وإعادة المحاولة.");
                    else
                    {
                        if (Check_MicroProject_ID(MPID_textBox.Text))
                        {
                            Insert_MicroProject();
                            MicroProject_ID = int.Parse(MPID_textBox.Text);
                            l.Insert_Log("Insert the project: " + MicroProject_ID, "Micro Project",
                                Program.UserName, DateTime.Now);
                            CreatedBy_User_label.Text = Program.UserName;

                            Save_MP_ID_button.BackgroundImage = Properties.Resources.Cheked;
                            toolTip1.SetToolTip(Save_MP_ID_button, "محجوز");
                        }
                        else
                        {
                            MessageBox.Show("الرجاء استخدام رقم مشروع آخر لأن هذا الرقم مستخدم من قبل مشروع آخر");
                            MPID_textBox.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void AddPartner_button_Click(object sender, EventArgs e)
        {
            try
            {
                //  create new beneficiary //
                var dialogResult = MessageBox.Show("هل تريد إضافة شريك آخر لهذا المشروع؟", "Partners", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Update_Mode = false;
                    clear_Person_boxes();
                    user_mode = false;
                    PersonType_comboBox.SelectedIndex = 1;
                    user_mode = true;

                    Main_panel.AutoScrollPosition = new Point(0, 0);
                    first_tableLayoutPanel.Focus();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                //التأكد من أنه تم حفظ المشروع
                if (Save_MP_ID_button.BackgroundImage == Properties.Resources.Unchecked || MicroProject_ID == -1)
                    throw new InvalidOperationException("لا يمكن الحفظ قبل حجز رقم المشروع! الرجاء حفظ رقم المشروع ثم إعادة المحاولة..");

                // PARTIAL-SAVE: RESTORE VALIDATION — remove Update_Mode guard to require all fields on every save
                if (!Update_Mode)
                {
                    if (P_DOB_textBox.Text != "" && (DateTime.Now.Year - Convert.ToInt32(P_DOB_textBox.Text) < 18)) // منع ادخال استمارة قاصرين
                        throw new InvalidOperationException("لا يمكن الحفظ لأن المستفيد تحت السن القانوني");

                    if (P_FirstName_textBox.Text == "" || P_LastName_textBox.Text == "" || P_FatherName_textBox.Text == ""
                        || P_NationalNum_textBox.Text == "" || P_NumAtHome_textBox.Text == "" || P_Parish_comboBox.Text == "")
                        throw new InvalidOperationException("لا يمكن الحفظ بوجود خلايا فارغة، يُرجى التأكد من كافة البيانات والمحاولة مرة أخرى.");

                    if (P_Sex_comboBox.Text == "ذكر"
                        && !MilitarySingle_radioButton.Checked && !MilitaryNow_radioButton.Checked
                        && !MilitaryFinished_radioButton.Checked && !MilitaryDelayed_radioButton.Checked
                        && !MilitaryExempt_radioButton.Checked)
                        throw new InvalidOperationException("لا يمكن الحفظ قبل حفط حالة خدمة العلم للمستفيد، الرجاء التأكد من المعلومة والمحاولة مرة أخرى.");

                    if (FamilyNum_textBox1.Text == "")
                    {
                        string msg = "لن يتم حفظ معلومات أفراد العائلة أو الزوج/ة لأنه لا يوجد رقم دفتر عائلة";
                        msg += Environment.NewLine;
                        msg += "الرجاء كتابة أي معلومة أو رقم لامكانية ادخال كل أفراد العائلة بشكل صحيح";
                        throw new InvalidOperationException(msg);
                    }
                }

                ////////////////  Insert Person ///////////////////////
                // أي اننا في مود التعديل ولكن رقم المستفيد -1
                if (!Update_Mode || (Person_ID == -1 && Update_Mode))
                {
                    Person_ID = Insert_Person();
                    l.Insert_Log("Insert " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ", Program.UserName, DateTime.Now);

                    P_ID_textBox.Text = Person_ID.ToString();
                    Beneficiary_Count++;

                    //////////// Link Person to Project ////////////////////////
                    if (Person_ID != -1 || P_ID_textBox.Text != "")
                    {
                        if (Beneficiary_Count == 1 && PersonType_comboBox.SelectedItem.ToString().Contains("مستفيد"))
                            Insert_Person_MicroProject(Convert.ToInt32(Person_ID), MicroProject_ID, "مستفيد");
                        else
                        {
                            Insert_Person_MicroProject(Convert.ToInt32(Person_ID), MicroProject_ID, "شريك");
                            Partners_lable.Text = "شراكة";
                        }
                        l.Insert_Log("Insert and link the Beneficiary: " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text + " with the project:" + MicroProject_ID,
                            " Beneficiary_Project ", Program.UserName, DateTime.Now);
                    }
                    /////////////////////////////////////////////////////////////

                    //insert the tasks of this project//
                    TasksOfProjects = new TasksOfProjects();
                    TasksOfProjects.Insert_AllTasks_MicroProject(MicroProject_ID, DateTime.Now, "a");
                    l.Insert_Log("Insert all (a) tasks of project: " + MicroProject_ID + " ", " Task_Project ",
                        Program.UserName, DateTime.Now);
                    ////////////////////////////////////////

                    // Turn to Update mood before end all the save operation// 
                    Update_Mode = true;
                    AddPartner_button.Visible = true;

                }
                else
                {
                    Update_Person(Person_ID);
                    if (Beneficiary_Count == 1 && PersonType_comboBox.SelectedItem.ToString().Contains("مستفيد"))
                        Update_Person_MicroProject(Person_ID, MicroProject_ID, "مستفيد");
                    else Update_Person_MicroProject(Person_ID, MicroProject_ID, "شريك");


                    l.Insert_Log("Update " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ", Program.UserName, DateTime.Now);
                }

                Update_MP(MicroProject_ID);
                l.Insert_Log("Update the project: " + MicroProject_ID, "Micro Project", Program.UserName, DateTime.Now);
                EditedBy_User_label.Text = Program.UserName;

                /// *** Insert course - education - work - experience - loss - aidsource *** /// only that aren't in database
                Insert_Update_PersonDetails(Person_ID);

                Insert_Update_AllFamilyDetails();

                Insert_Update_ProjectDetails(MicroProject_ID);

                //  create new beneficiary //
                var dialogResult = MessageBox.Show("تم حفظ كافة بيانات المشروع. هل تريد إضافة مشروع جديد ؟", "New Application", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    clear_Person_boxes();
                    clear_Project_boxes();

                    Update_Mode = false;
                    AddPartner_button.Visible = false;

                    mainForm.TabName_label.Text = "Application";
                    mainForm.MP_ID_label.Text = MPID_textBox.Text;

                    Main_panel.AutoScrollPosition = new Point(0, 0);
                }
                else
                {
                    Update_Mode = true;
                    AddPartner_button.Visible = true;
                    if (Type_comboBox.Text.ToString().Contains("ترميم")
                                || Type_comboBox.Text.ToString().Contains("زراع"))
                        AddExtention_button.Visible = true;
                    else AddExtention_button.Visible = false;
                    Save_MP_ID_button.Visible = false;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void AddExtention_button_Click(object sender, EventArgs e)
        {
            try
            {
                Form form;
                if (Type_comboBox.Text.ToString().Contains("ترميم"))
                {
                    form = new ApplicationRehab_Form(mainForm, MicroProject_ID);
                    form.ShowDialog();
                }
                else if (Type_comboBox.Text.ToString().Contains("زراع"))
                {
                    form = new Agriculture_Form(mainForm, MicroProject_ID
                        , P_FirstName_textBox.Text + P_LastName_textBox.Text + " ابن/ة " + P_FatherName_textBox.Text
                        , CreatedBy_User_label.Text, EditedBy_User_label.Text);
                    form.ShowDialog();
                }
                else
                { return; }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}