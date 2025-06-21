//using Chilkat;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;

namespace MyWorkApplication
{
    public partial class Application_New_Form : Form
    {
        private int Beneficiary_Count;
        private FtpConnector c;
        private int image_state;
        private string RequestedAmount, SimpleProfit;
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
        DataSet ds;

        public Application_New_Form(MainForm mainForm, bool zero)
        {
            InitializeComponent();

            //clear_Person_boxes();
            Person_ID = -1;
            MicroProject_ID = -1;
            SubHeader1_label.Text = "الاستمارة المشروع";

            if (zero)
            {
                SubHeader1_label.Text = "الاستمارة الأولية للمشروع";

                //First_tableLayoutPanel
                FundType_label.Visible = FundType_flowLayoutPanel.Visible =
                MP_RequestedAmount_label.Visible = MP_RequestedAmount_textBox.Visible = false;

                //t2_tableLayoutPanel
                t2_tableLayoutPanel.Height = 0;
                t2_tableLayoutPanel.Visible = false;

                //t3_tableLayoutPanel
                P_FullName_En_label.Visible = P_FullName_En_textBox.Visible =
                P_Registration_label.Visible = P_Registration_textBox.Visible =
                P_Priest_tableLayoutPanel.Visible =
                P_Mobile2_label.Visible = P_Mobile2_textBox.Visible =
                P_HomeStateNote_label.Visible = P_HomeStateNote_textBox.Visible = false;

                //t5_tableLayoutPanel

                //t6_tableLayoutPanel
                t6_tableLayoutPanel.Height = 0;
                t6_tableLayoutPanel.Visible = false;

                //t7_tableLayoutPanel

                //t8_tableLayoutPanel
                t8_tableLayoutPanel.Height = 0;
                t8_tableLayoutPanel.Visible = false;

                //t20_tableLayoutPanel
                t20_tableLayoutPanel.Height = 0;
                t20_tableLayoutPanel.Visible = false;

                //t21_tableLayoutPanel

                ////////////////////////////////////
                //t4_tableLayoutPanel
                t4_tableLayoutPanel.Height = 0;
                t4_tableLayoutPanel.Visible = false;
                //////////////////////////////////////

                //t9_tableLayoutPanel
                t9_tableLayoutPanel.Height = 0;
                t9_tableLayoutPanel.Visible = false;

                //t10_tableLayoutPanel
                P_Debts_tableLayoutPanel.Visible = false;

                //t11_tableLayoutPanel
                t11_tableLayoutPanel.Height = 0;
                t11_tableLayoutPanel.Visible = false;
            }

            Loan_radioButton.Checked = true;

            this.mainForm = mainForm;
            Update_Mode = false;
            AddPartner_button.Visible = false;
        }

        private void Application_New_Form_Load(object sender, EventArgs e)
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

                FamilyMember_dataGridView.AutoGenerateColumns =
                Work_dataGridView.AutoGenerateColumns = Exp_dataGridView.AutoGenerateColumns =
                Education_dataGridView.AutoGenerateColumns = Course_dataGridView.AutoGenerateColumns =
                Loss_dataGridView.AutoGenerateColumns = Illness_dataGridView.AutoGenerateColumns =
                Assets_dataGridView.AutoGenerateColumns =
                HomePowerSource_dataGridView.AutoGenerateColumns = PowerSource_dataGridView.AutoGenerateColumns =
                Materials_dataGridView.AutoGenerateColumns = false;


                Bind_Gridview_Dictionaries();
                Initialize_IsDirty();
                Initialize_NumericTextBox_KeyPress();

                Stop_ComboBox_Scrolling();
                Bind_Data(false);
                Stop_Highlighting_Combobox();

                Type_comboBox.SelectedIndex = 0; // default MICRO

                ////Save_MP_ID_button.Visible = true;
                ////Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                ////toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");
                NoFamily = true;

                ////user_mode = false;
                ////PersonType_comboBox.SelectedIndex = 0;
                ////user_mode = true;

                //Reset the nubmer of family members//
                P_NumAtHome_textBox.Text = "0";
                ////P_State_comboBox.SelectedValue = 1; //مقيم

                //TODO
                //////if (MicroProject_ID != -1)
                //////{
                //////    user_mode = false;
                //////    if (Person_ID != -1)
                //////    {
                //////        Fill_Person();
                //////        Fill_PersonDetails();
                //////        Fill_FamilyMembers(0);
                //////    }
                //////    Fill_Project();
                //////    Fill_ProjectDetails();

                //////    Save_MP_ID_button.Visible = false;
                //////    Save_MP_ID_button.BackgroundImage = Properties.Resources.Cheked;
                //////    toolTip1.SetToolTip(Save_MP_ID_button, "محجوز");

                //////    /// check if this project got partners or not ///
                //////    var query = "select count(*) from `person_microproject` where MicroProject_ID = " + MicroProject_ID;
                //////    Program.buildConnection();
                //////    MySS.sc = new MySqlCommand(query, Program.MyConn);
                //////    var partners_count = Convert.ToInt32(MySS.sc.ExecuteScalar());
                //////    Program.MyConn.Close();
                //////    if (partners_count > 1) Partners_lable.Text = "شراكة";
                //////    else Partners_lable.Text = "فردي";

                //////    //show delete-partner buttons on Update
                //////    AddPartner_button.Visible = Delete_MP_button.Visible = true;
                //////    /////////////////////////////////////////////////

                //////    user_mode = true;
                //////}

                //unable to cast db.null to system.string !!
                mainForm.MP_ID_label.Visible = mainForm.ProjectNumber_label.Visible =
                    mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.TabName_label.Text = "Application";
                mainForm.MP_ID_label.Text = MPID_textBox.Text;

                Education_dataGridView.Columns["Edu_DeleteRow"].Width =
                Work_dataGridView.Columns["W_DeleteRow"].Width =
                Exp_dataGridView.Columns["Exp_DeleteRow"].Width =
                FamilyMember_dataGridView.Columns["P_DeleteRow"].Width =
                Course_dataGridView.Columns["C_DeleteRow"].Width =
                Loss_dataGridView.Columns["L_DeleteRow"].Width =
                Illness_dataGridView.Columns["Ill_DeleteRow"].Width =
                Assets_dataGridView.Columns["Asset_DeleteRow"].Width =
                AidSource_dataGridView.Columns["Aid_DeleteRow"].Width =
                HomePowerSource_dataGridView.Columns["HPower_DeleteRow"].Width =
                PowerSource_dataGridView.Columns["Power_DeleteRow"].Width = 50;

                Main_panel.AutoScrollPosition = new System.Drawing.Point(0, 0);
                //////////////////////////////
                Set_Controls(); //from F2F


                CheckUserPermission();
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



        #region combobox functions
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
            query += "select ID, Name from `assettype` order by ID ASC;"; //P_HomeType_comboBox, MP_PlaceType_comboBox
            query += "select UserID as 'ID', UserName as 'Name' from `user` "
                  + " where IsVisitor = 1 order by UserName ASC;"; //PaperApplicationUser_comboBox


            //Datagrid_ComboBoxes
            query += "select W_ID as 'ID',W_Name as 'Name' from `work` ORDER BY W_Name ASC;"; //W_Name, Exp_Name, P_WorkName
            query += "select ID, Name from `worktype` ORDER BY ID ASC;";  //W_WorkType, Exp_WorkType, P_WorkType
            query += "select education.ID , education.Name, educationtype.ID as 'Educationtype_ID', educationtype.Name as 'Educationtype' "
                + " from `education` left join educationtype on educationtype.id = education.Type_ID ORDER BY Name ASC;"; //E_Name
            query += "SELECT ID, Name from `course` ORDER BY Name ASC;"; //C_SubCategory
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

            DataTable dt112 = ds.Tables[11].Copy(); //P_WorkType
            Bind_Gridview_ComboBox(this.P_WorkType, dt112);

            // Fill on E_Type selection changed
            //DataTable dt12 = ds.Tables[12].Copy(); //E_Name
            //Bind_Gridview_ComboBox(this.E_Name, dt12);

            DataTable dt13 = ds.Tables[13].Copy(); //C_SubCategory
            Bind_Gridview_ComboBox(this.C_SubCategory, dt13);

            DataTable dt14 = ds.Tables[14].Copy(); //Power_Name
            Bind_Gridview_ComboBox(this.Power_Name, dt14);

            DataTable dt141 = ds.Tables[14].Copy(); //HPower_Name
            Bind_Gridview_ComboBox(this.HPower_Name, dt141);

            DataTable dt15 = ds.Tables[15].Copy(); //P_Relation
            Bind_Gridview_ComboBox(this.P_Relation, dt15);

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

            DataTable dt221 = ds.Tables[22].Copy(); //Ill_AidPeriod
            Bind_Gridview_ComboBox(this.Ill_AidPeriod, dt221);

            DataTable dt23 = ds.Tables[23].Copy(); //Aid_Source
            Bind_Gridview_ComboBox(this.Aid_Source, dt23);

            DataTable dt231 = ds.Tables[23].Copy(); //Help_Source
            Bind_Gridview_ComboBox(this.Help_Source, dt231);

            DataTable dt232 = ds.Tables[23].Copy(); //Ill_Donor
            Bind_Gridview_ComboBox(this.Ill_Donor, dt232);

            //CheckBoxes
            DataTable dt24 = ds.Tables[24].Copy();//GoalsCheckBoxes
            Bind_Chickbox_List(P_Goals_panel2, dt24);


            //Micro Project ID
            DataTable dt25 = ds.Tables[25].Copy();//GoalsCheckBoxes
            Bind_MicroProject_ID(dt25);


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

        private void Bind_Chickbox_List(FlowLayoutPanel FlowLayoutPanel_Name, DataTable dt)
        {

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
            }
        }

        private void Bind_MicroProject_ID(DataTable dt)
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
                    {Materials_dataGridView,  "M_ID"},
                    {FamilyMember_dataGridView,  "FP_ID"},
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
                    {Loss_dataGridView,  "P_IsDirty"},
                    {Illness_dataGridView,  "Ill_IsDirty"},
                    {Assets_dataGridView,  "Asset_IsDirty"},
                    {AidSource_dataGridView,  "Aid_IsDirty"},
                    {HelpSource_dataGridView,  "Help_IsDirty"},
                    {HomePowerSource_dataGridView,  "HPower_IsDirty"}
                };
        }

        private void Bind_Gridview_ComboBox(DataGridViewComboBoxColumn Combox_Name, DataTable dt)
        {
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
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;

                string columnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                if (columnName == "E_Name")
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value == null || string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString()))
                        throw new Exception("الرجاء قم باختيار الدرجة العلمية أولاً");
                }
                else if (columnName == "P_E_Name")
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["P_EducationType"].Value == null || string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].Cells["P_EducationType"].Value.ToString()))
                        throw new Exception("الرجاء قم باختيار الدرجة العلمية أولاً");
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

        #endregion


        private void Initialize_NumericTextBox_KeyPress()
        {
            MPID_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            MP_RequestedAmount_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
            P_DOB_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            P_WorkYears_textBox.KeyPress += InputValidationHelpercs.Integers_NumericTextBox_KeyPress;
            AmperNumber_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
            P_DebtsAmount_textBox.KeyPress += InputValidationHelpercs.Decimal_NumericTextBox_KeyPress;
        }

        private void PersonDOB_textBox_Leave(object sender, EventArgs e)
        {
            if (P_DOB_textBox.Text != "")
            {
                var date = Convert.ToInt32(P_DOB_textBox.Text);
                if (date < 1900 || date > DateTime.Now.Year)
                {
                    MessageBox.Show("الرجاء إدخال تاريخ صالح!", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    P_DOB_textBox.Text = "";
                }
                else if (DateTime.Now.Year - date < 18) // منع ادخال استمارة قاصرين
                {
                    MessageBox.Show("انتبه! المستفيد تحت السن القانوني", "Date error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    P_DOB_textBox.Text = "";
                }
            }
        }



        #region IsDirty
        Dictionary<DataGridView, bool> IsDirty_DataGrid = new Dictionary<DataGridView, bool>();

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
        }
        #endregion












        public void Check_Theme()
        {
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.ApplicationNew_ToNight(this);
            else
                newTheme.ApplicationNew_ToLight(this);
        }

        private bool CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible
                            //= AddPriest_button.Visible = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                            = true;
                        return true;
                    }
                case 2: //Data
                    {
                        payments_toolStripMenuItem.Visible = addNotesToolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_MP_button.Visible
                            = AddPartner_button.Visible
                            //= AddPriest_button.Visible = AddWork_button.Visible = AddEducation_button.Visible
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
                            = AddPartner_button.Visible
                            //= AddPriest_button.Visible = AddWork_button.Visible = AddEducation_button.Visible
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
                            = AddPartner_button.Visible
                            //= AddPriest_button.Visible = AddWork_button.Visible = AddEducation_button.Visible
                            = AddImage_button.Visible = DeleteImage_button.Visible = Download_button.Visible
                           = false;
                        return false;
                    }
            }
            return false;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            //Save_btn.IsEnabled = false;
            //try
            //{
            //    string inserted_value = "";
            //    double amount = 0;
            //    string ActionDate = "";
            //    string Notes = "";
            //    string E_Notes = "";
            //    string ObserveTill_Type = "";
            //    int DestinationTill_ID = -1;
            //    int ExchangeAcc_ID = -1;
            //    int Donor_ID = -1;
            //    int MicroProject_ID = -1;
            //    int budget_line_ID = -1;
            //    int CashAccount_ID = -1;
            //    int ObserveCashAccount_ID = -1;
            //    int ExchangeCash_ID = -1;
            //    int IsReceipt = -1;
            //    int OriginalRate_ID = -1;
            //    string ReceiptNum = "";
            //    int Employee_Salary_ID = -1;
            //    bool WithExAcc = false;
            //    bool WithExchangeRate = false;

            //    double rate = 0;

            //    #region Check and get data

            //    if (Type_comboBox.Text == "")
            //    {
            //        MessageBox.Show("Please select the operation type.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //        return;
            //    }
            //    else
            //    {
            //        if (Type_comboBox.SelectedItem.ToString().Contains("Income"))
            //        {
            //            inserted_value = "I";
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Adv. Payment"))
            //        {
            //            inserted_value = "O";
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Returned"))
            //        {
            //            inserted_value = "R";
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Expenses"))
            //        {
            //            inserted_value = "E";
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Exchange"))
            //        {
            //            inserted_value = "Ex";
            //            if (Account_Type == "Project")
            //            {
            //                if (!Check_AvgRate())
            //                {
            //                    MessageBox.Show("Please ensure that there is at least one AvgRate that matches the conversion currency and extends for a period that includes the transaction date.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //                    return;
            //                }
            //            }
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Move"))
            //        {
            //            inserted_value = "M";
            //        }
            //    }

            //    if (Amount_txtBox.Text == "")
            //    {
            //        MessageBox.Show("Please make sure to enter the amount.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //        return;

            //    }
            //    else
            //    {

            //        Double.TryParse(Amount_txtBox.Text, out amount);
            //    }

            //    if (ActionDate_datepicker.SelectedDate == null)
            //    {
            //        MessageBox.Show("Please make sure to select the operation date.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //        return;

            //    }
            //    else
            //    {
            //        ActionDate = ((DateTime)ActionDate_datepicker.SelectedDate).ToString("yyyy/MM/dd");
            //    }

            //    if (DestinationTill_comboBox.Visibility == Visibility.Visible)
            //    {

            //        if (Type_comboBox.SelectedItem.ToString().Contains("Exchange"))
            //        {
            //            if (DestinationTill_comboBox.SelectedIndex == -1)
            //            {
            //                MessageBox.Show("Please make sure to select the Corresponding till.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //                return;


            //            }
            //            else
            //            {
            //                DestinationTill_ID = Convert.ToInt32(DestinationTill_comboBox.SelectedValue);

            //                DataRow[] row = t_dt.Select("ID=" + Convert.ToInt32(DestinationTill_ID));

            //                ObserveTill_Type = row[0]["Type"].ToString();

            //                if (Account_Type == "Project" && ExchangeAcc_comboBox.SelectedIndex != -1)
            //                    WithExAcc = true;

            //                if (Account_Type == "Exchange")
            //                    WithExchangeRate = true;
            //            }
            //        }
            //        else if (Type_comboBox.SelectedItem.ToString().Contains("Move"))
            //        {
            //            if (DestinationTill_comboBox.SelectedIndex == -1)
            //            {
            //                MessageBox.Show("Please make sure to select the Corresponding account.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //                return;

            //            }
            //            else
            //            {
            //                if (DestinationTill_comboBox.SelectedIndex != -1)
            //                    DestinationTill_ID = Convert.ToInt32(DestinationTill_comboBox.SelectedValue);

            //                ObserveTill_Type = TillType;
            //            }
            //        }

            //    }

            //    if (ExchangeAcc_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (ExchangeAcc_comboBox.SelectedIndex == -1)
            //        {

            //        }
            //        else
            //        {
            //            ExchangeAcc_ID = Convert.ToInt32(ExchangeAcc_comboBox.SelectedValue);

            //            if (ExchangeCash_comboBox.Visibility == Visibility.Visible)
            //            {
            //                if (ExchangeCash_comboBox.SelectedIndex == -1)
            //                {
            //                    MessageBox.Show("Please make sure to select the Exchange cash till.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //                    return;

            //                }
            //                else
            //                {
            //                    ExchangeCash_ID = Convert.ToInt32(ExchangeCash_comboBox.SelectedValue);
            //                }
            //            }
            //        }
            //    }

            //    if (MircoProject_ID_autoSuggestionUserControl.Visibility == Visibility.Visible)
            //    {
            //        if (MircoProject_ID_autoSuggestionUserControl.autoTextBox.Text != "" && BeneficiaryName_txtBox.autoTextBox.Text != "")
            //            int.TryParse(MircoProject_ID_autoSuggestionUserControl.autoTextBox.Text, out MicroProject_ID);
            //        else MicroProject_ID = -1;

            //        if (MicroProject_ID == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the micro project.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;

            //        }
            //    }

            //    if (Donor_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (Donor_comboBox.SelectedIndex == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the donor", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //            Donor_ID = Convert.ToInt32(Donor_comboBox.SelectedValue);
            //    }

            //    if (CashAccount_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (CashAccount_comboBox.SelectedIndex == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the cash till.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //            CashAccount_ID = Convert.ToInt32(CashAccount_comboBox.SelectedValue);
            //    }

            //    if (ObserveCashAccount_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (ObserveCashAccount_comboBox.SelectedIndex == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the corresponding cash till.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //            ObserveCashAccount_ID = Convert.ToInt32(ObserveCashAccount_comboBox.SelectedValue);
            //    }

            //    if (Budget_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (Line_comboBox.SelectedIndex == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the budget, category and budget line.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //            budget_line_ID = Convert.ToInt32(Line_comboBox.SelectedValue);
            //    }



            //    if (IsReceipt_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (IsReceipt_comboBox.SelectedIndex == -1 && Account_Type != "Personal")
            //        {
            //            MessageBox.Show("Please make sure to select the documet type and to specify its date", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //        {
            //            if (IsReceipt_comboBox.SelectedIndex != -1)
            //            {
            //                if (IsReceipt_comboBox.SelectedItem.ToString().Contains("invoice"))
            //                {
            //                    IsReceipt = 1;
            //                }
            //                else if (IsReceipt_comboBox.SelectedItem.ToString().Contains("receipt"))
            //                {
            //                    IsReceipt = 0;
            //                }
            //                else if (IsReceipt_comboBox.SelectedItem.ToString().Contains("payslip"))
            //                {
            //                    IsReceipt = 2;
            //                }
            //            }
            //        }
            //    }

            //    if (ReceiptNum_txtBox.Visibility == Visibility.Visible)
            //    {
            //        if (ReceiptNum_txtBox.Text == "" && Account_Type != "Personal")
            //        {
            //            MessageBox.Show("Please make sure to enter the document number", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;

            //        }
            //        else
            //            ReceiptNum = ReceiptNum_txtBox.Text;
            //    }

            //    if (Original_AvgRate_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (Original_AvgRate_comboBox.SelectedIndex == -1)
            //        {
            //            if (Account_Type == "Project" && TillType.ToLower() == "syr")
            //            {
            //                MessageBox.Show("Please make sure to select the origin Avg rate", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //                return;

            //            }
            //        }
            //        else
            //            OriginalRate_ID = Convert.ToInt32(Original_AvgRate_comboBox.SelectedValue);
            //    }

            //    if (Rate_txtBox.Visibility == Visibility.Visible)
            //    {
            //        if (Rate_txtBox.Text == "")
            //        {
            //            MessageBox.Show("Please make sure to enter the rate", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //            Double.TryParse(Rate_txtBox.Text, out rate);
            //    }

            //    if (Employee_comboBox.Visibility == Visibility.Visible)
            //    {
            //        if (Employee_comboBox.SelectedIndex == -1)
            //        {
            //            MessageBox.Show("Please make sure to select the Volunteer.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            return;
            //        }
            //        else
            //        {
            //            Employee_Salary_ID = Convert.ToInt32(Employee_comboBox.SelectedValue);
            //        }
            //    }

            //    Notes = Notes_txtBox.Text;
            //    E_Notes = E_Notes_txtBox.Text;
            //    #endregion

            //    if (Login.BuildConnection())
            //    {
            //        if (Action_ID != -1)//Edit
            //        {
            //            if (Approved_Action)
            //            {
            //                if (inserted_value == "Ex" || inserted_value == "M")
            //                {
            //                    a.Update(Action_ID, inserted_value, Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, ExchangeAcc_ID, true, WithExAcc, WithExchangeRate, amount, rate
            //                        , ActionDate, Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //                        , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, Account_ID, "AddAction_Window");
            //                }
            //                else
            //                {
            //                    a.Update(Action_ID, inserted_value, Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, ExchangeAcc_ID, false, false, false, amount, rate
            //                    , ActionDate, Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //                        , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, -1, "AddAction_Window"); ;
            //                }
            //            }
            //            else
            //            {
            //                if (inserted_value == "Ex" || inserted_value == "M")
            //                {
            //                    a.Update_Not_Approved(Action_ID, inserted_value, Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, ExchangeAcc_ID, true, WithExAcc, WithExchangeRate, amount, rate
            //                        , ActionDate, Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //                        , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, Account_ID, "AddAction_Window");
            //                }
            //                else
            //                {
            //                    a.Update_Not_Approved(Action_ID, inserted_value, Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, ExchangeAcc_ID, false, false, false, amount, rate
            //                    , ActionDate, Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //                        , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, -1, "AddAction_Window"); ;
            //                }
            //            }
            //            int success = 0;
            //            if (FullAttachNames != null && FullAttachNames.Length > 0)
            //            {
            //                foreach (var FullAttachName in FullAttachNames)
            //                {
            //                    if (Save_Attachments(FullAttachName, Action_ID)) success++;
            //                }
            //            }

            //            if (FullAttachNames != null && FullAttachNames.Length > 0)
            //                MessageBox.Show("Saved successfully." + Environment.NewLine + "Attachments: Succeeded" + FullAttachNames.Length.ToString() + "/" + success.ToString(), "Confirmation", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            else
            //                MessageBox.Show("Saved successfully.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

            //            this.Close();
            //        }
            //        else//Add new
            //        {
            //            int A_id = -1;
            //            if (inserted_value == "Ex" || inserted_value == "M")
            //            {
            //                A_id = a.Insert(Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, ExchangeAcc_ID, true, WithExAcc, WithExchangeRate, inserted_value, amount, rate, ActionDate
            //                , Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //               , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, Account_ID, "AddAction_Window");
            //            }
            //            else
            //            {
            //                A_id = a.Insert(Till_ID, TillType, DestinationTill_ID, ObserveTill_Type, Donor_ID, -1, false, false, false, inserted_value, amount, rate, ActionDate
            //                , Notes, E_Notes, IsReceipt, ReceiptNum, Employee_Salary_ID
            //                , OriginalRate_ID, MicroProject_ID, budget_line_ID, -1, CashAccount_ID, ObserveCashAccount_ID, ExchangeCash_ID, -1, "AddAction_Window");
            //            }
            //            int success = 0;
            //            if (FullAttachNames != null && FullAttachNames.Length > 0)
            //            {
            //                foreach (var FullAttachName in FullAttachNames)
            //                {
            //                    if (Save_Attachments(FullAttachName, A_id)) success++;
            //                }
            //            }

            //            if (FullAttachNames != null && FullAttachNames.Length > 0)
            //                MessageBox.Show("Saved successfully." + Environment.NewLine + "Attachments: Succeeded" + FullAttachNames.Length.ToString() + "/" + success.ToString(), "Confirmation", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            else
            //                MessageBox.Show("Saved successfully.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            //            UserMode = false;
            //            Clear_Boxes();
            //            UserMode = true;
            //        }

            //        if (TW != null)
            //            TW.Load_Data();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (Login.MyConn != null && Login.MyConn.State == ConnectionState.Open)
            //    {
            //        Login.MyConn.Close();
            //    }
            //    if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL"))
            //    { MessageBox.Show("please check your network and try again", "Error"); }
            //    else if (ex.Message.Contains("host"))
            //    { MessageBox.Show("please check the server or your network connection and try again", "Error"); }
            //    else if (ex.Message.Contains("Duplicate entry"))
            //    { MessageBox.Show("Error … There are duplicate values.", "Error", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign); }
            //    else { MessageBox.Show(ex.Message, "Error"); }
            //}
            //finally
            //{
            //    Save_btn.IsEnabled = true;
            //}
        }



        #region sql queries 
        //TODO
        // edit queries type
        private void Edit_MP(int old_MP_ID, int new_MP_ID)
        {
            string query = "update `microproject` set MP_ID = " + new_MP_ID + " where MP_ID = " + old_MP_ID + " ";
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }
        #endregion

        #region functions
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

                        var _MessageBox = new Application_MessageBox(P_ID, MP_ID, Name);
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

                            //TODO
                            //Fill_Person();
                            //Fill_PersonDetails();
                            //Fill_FamilyMembers(0); 
                            //clear_Project_boxes();
                        }
                        else
                        {
                            //تراجع
                            //Nothing to do// 
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
        private void Calculate_NumAtHome()
        {
            try
            {
                //int NumOfActiveMembers = 0;
                //int NumOfMembers = FamilyMember_dataGridView.Rows.Count; //all

                //if (!string.IsNullOrWhiteSpace(P_FirstName_textBox.Text))
                //{
                //    NumOfActiveMembers++; NumOfMembers++;
                //}
                //////  husband/wife
                ////if (!string.IsNullOrWhiteSpace(FPersonFName_textBox1.Text)
                ////    && FPersonState_comboBox1.SelectedIndex != -1)
                ////{
                ////    string value = FPersonState_comboBox1.SelectedValue.ToString();
                ////    DataRow[] rows = null; int id;
                ////    if (int.TryParse(value, out id)) rows = ds.Tables[6].Select("ID=" + id);
                ////    else rows = ds.Tables[6].Select("Name='" + value + "'");
                ////    if (rows.Length != 0)
                ////    {
                ////        if (rows[0].Field<string>(2).ToLower() == "active")
                ////            NumberOfMembers++;
                ////    }
                ////}

                //if (FamilyMember_dataGridView.Rows.Count > 0)
                //    for (int i = 0; i < FamilyMember_dataGridView.Rows.Count; i++)
                //    {
                //        FamilyMember_dataGridView["F_Number", i].Value = FamilyNum_textBox1.Text;

                //        if (FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == null ||
                //            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value == DBNull.Value ||
                //            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString() == "")
                //            FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value = 1; //مقيم

                //        if (FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value == null ||
                //            FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value == DBNull.Value ||
                //            FamilyMember_dataGridView.Rows[i].Cells["P_FirstName"].Value.ToString() == "")
                //            continue;

                //        string value = FamilyMember_dataGridView.Rows[i].Cells["P_State"].Value.ToString();

                //        DataRow[] rows = null; int id;
                //        if (int.TryParse(value, out id)) rows = ds.Tables[6].Select("ID=" + id);
                //        else rows = ds.Tables[6].Select("Name='" + value + "'");

                //        if (rows.Length != 0)
                //        {
                //            if (rows[0].Field<string>(2).ToLower() == "active")
                //                NumOfActiveMembers++;
                //        }
                //    }
                //P_NumAtHome_textBox.Text = NumOfActiveMembers.ToString();
                //P_NumOfMembers_textBox.Text = NumOfMembers.ToString();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void calculate_SumOfBudgetItems()
        {
            try
            {
                sumOfItemsPrice_SYR = 0;

                for (var RowIndex = 0; RowIndex < Materials_dataGridView.RowCount - 1; RowIndex++)
                {
                    double itemOverAllsyr = 0;
                    // if (Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value == null)
                    //     Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value = "0";

                    if (Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value == null)
                        Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value = "0";

                    if (Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value == null)
                        Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value = "0";

                    var price = Materials_dataGridView.Rows[RowIndex].Cells["M_Price"].Value.ToString().Replace(",", "");
                    //   var local = Materials_dataGridView.Rows[RowIndex].Cells["M_LocalContribution"].Value.ToString().Replace(",", "");
                    var amount = Materials_dataGridView.Rows[RowIndex].Cells["M_Amount"].Value.ToString().Replace(",", "");

                    Double AllPrice = Double.Parse(price) * Int64.Parse(amount);
                    itemOverAllsyr = AllPrice;// - Int64.Parse(local);

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
        #endregion

        /// ////////////////////////////////////////////////////////////////////////

        #region events
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
        private void P_Mobile_textBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(P_MobileReadable_textBox.Text))
            {
                if (P_MobileReadable_textBox.Text.Length != 10)
                    MessageBox.Show("انتبه" + Environment.NewLine + Environment.NewLine + "عدد خانات رقم الجوال لا تساوي 10"
                        , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void PersonType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                string message;
                if (PersonType_comboBox.SelectedItem == "المستفيد")
                    message = "انتبه! سوف يتم تغيير صفة الشخص من شريك إلى مستفيد";
                else message = "انتبه! سوف يتم تغيير صفة الشخص من مستفيد إلى شريك";

                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void P_FirstName_textBox_Leave(object sender, EventArgs e)
        {
            Calculate_NumAtHome();
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
        private void MilitaryFinished_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (MilitaryFinished_radioButton.Checked)
                Military_Date_dateTimePicker.Visible = Military_Date_checkBox.Visible = true;
            else
                Military_Date_dateTimePicker.Visible = Military_Date_checkBox.Visible = false;
        }
        private void Grant_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Loan_radioButton.Checked) type = 0;
            else type = 1;
        }
        private void MPID_textBox_TextChanged(object sender, EventArgs e)
        {
            mainForm.MP_ID_label.Text = MPID_textBox.Text;
        }

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
        private void Military_Date_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Military_Date_checkBox.Checked)
                Military_Date_dateTimePicker.Enabled = true;
            else Military_Date_dateTimePicker.Enabled = false;
        }

        #endregion

        /// ////////////////////////////////////////////////////////////////////////

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



        //from f2f
        #region //IsDirty
        bool Person_Information_IsDirty = false,
             MicroProject_Information_IsDirty = false,
             FamilyMember_DataGridView_IsDirty = false,
             Work_DataGridView_IsDirty = false,
             Exp_DataGridView_IsDirty = false,
             Education_DataGridView_IsDirty = false,
             Course_DataGridView_IsDirty = false,
             Loss_DataGridView_IsDirty = false,
             Illness_DataGridView_IsDirty = false,
             Asset_DataGridView_IsDirty = false,
             AidSource_DataGridView_IsDirty = false,
             HomePowerSource_DataGridView_IsDirty = false,
             PowerSource_DataGridView_IsDirty = false,
             Material_DataGridView_IsDirty = false;
        #endregion
        //from f2f
        #region //Data
        string
            MP_ID, P_Mobile, MP_ApplyDate, MP_Name, MP_RequestedAmount,
            P_Loss,
            P_Goal,         // MP_Description
            P_GoalHow,      // MP_Marketing
            P_OtherNotes,   // MP_OtherNotes
            P_FName, P_FaName, P_LName, P_MoName,
            P_EngName, P_Registration, P_NationalNum, DOB, P_IntermidiaryName,
            P_LandLine, P_Mobile2, P_Address, P_HomeStateNote, P_Military, P_MilitaryDate,
            FamilyNumber,
            P_WorkYears,    // s3
            P_AidSourceNote,  //MP_OtherSources
            P_DeptAmount, P_DeptNote, P_SourceAssosiation, P_SourceRelative,
            MP_Address, MP_AddressNote, MP_Notes;



        int SubCategory_ID, Career_ID, Type_ID, SubType_ID, FundType_ID,
            Sex_ID, PersonType_ID, P_MaritalState_ID,
            Parish_ID, Priest1_ID, Priest2_ID, Street_ID, HomeType_ID, Military_ID, MilitaryFinished,
            NumAtHome, NumAllMembers,
            Has_Education, Has_Course, Has_Loss, Has_Illness, Has_Asset, Has_AidSource, Has_Dept,
            Has_Income, MP_PlaceType, MP_AmperNumber, MP_AmperType,
            MP_Agreement1, MP_Agreement2, MP_Agreement3,
            EntredBy, EditedBy;



        string P_Goals_Query, FamilyMembers_Query,
            Work_Query, Exp_Query, Education_Query, Course_Query,
            Loss_Query, Illness_Query, Asset_Query, AidSource_Query,
            HomePowerSource_Query, PowerSource_Query, Material_Query;

        #endregion
        //from f2f
        #region //Original
        string
            MPID_textBox_OriginalText, P_Mobile_textBox_OriginalText, MP_RequestedAmount_textBox_OriginalText,
            P_Loss_TextBox_OriginalText, P_Goal_textBox_OriginalText, P_GoalHow_textBox_OriginalText, P_OtherNotes_textBox_OriginalText,
            P_FirstName_textBox_OriginalText, P_FatherName_textBox_OriginalText, P_LastName_textBox_OriginalText,
            P_MotherName_textBox_OriginalText, P_Registration_textBox_OriginalText, P_NationalNumber_textBox_OriginalText,
            P_EngName_textBox_OriginalText, P_DOB_textBox_OriginalText, P_Intermidate_textBox_OriginalText,
            P_Landline_textBox_OriginalText, P_Mobile2_textBox_OriginalText, P_HomeAddress_textBox_OriginalText,
            P_HomeStateNote_textBox_OriginalText, P_Military_textBox_OriginalText,
            NumberAtHome_textBox_OriginalText, NumberAllMembers_textBox_OriginalText, FamilyNumber_textBox_OriginalText,
            P_WorkYears_textBox_OriginalText, AidSourceNote_textBox_OriginalText,
            P_DeptAmount_textBox_OriginalText, P_DeptNote_textBox_OriginalText,
            P_SourceAssociation_textBox_OriginalText, P_SourceRelative_textBox_OriginalText,
            MP_Address_textBox_OriginalText, MP_AddressNote_textBox_OriginalText,
            MP_AmperNumber_textBox_OriginalText, MP_Notes_textBox_OriginalText;

        #endregion
        //from f2f
        private void Set_Controls()
        {
            MPID_textBox_OriginalText = P_Mobile_textBox_OriginalText = MP_RequestedAmount_textBox_OriginalText =
            P_Loss_TextBox_OriginalText = P_Goal_textBox_OriginalText = P_GoalHow_textBox_OriginalText = P_OtherNotes_textBox_OriginalText =
            P_FirstName_textBox_OriginalText = P_FatherName_textBox_OriginalText = P_LastName_textBox_OriginalText =
            P_MotherName_textBox_OriginalText = P_Registration_textBox_OriginalText = P_NationalNumber_textBox_OriginalText =
            P_EngName_textBox_OriginalText = P_DOB_textBox_OriginalText = P_Intermidate_textBox_OriginalText =
            P_Landline_textBox_OriginalText = P_Mobile2_textBox_OriginalText = P_HomeAddress_textBox_OriginalText =
            P_HomeStateNote_textBox_OriginalText = P_Military_textBox_OriginalText =
            NumberAtHome_textBox_OriginalText = NumberAllMembers_textBox_OriginalText = FamilyNumber_textBox_OriginalText =
            P_WorkYears_textBox_OriginalText = AidSourceNote_textBox_OriginalText =
            P_DeptAmount_textBox_OriginalText = P_DeptNote_textBox_OriginalText =
            P_SourceAssociation_textBox_OriginalText = P_SourceRelative_textBox_OriginalText =
            MP_Address_textBox_OriginalText = MP_AddressNote_textBox_OriginalText =
            MP_AmperNumber_textBox_OriginalText = MP_Notes_textBox_OriginalText = "";

            Person_Information_IsDirty =
            MicroProject_Information_IsDirty =
            FamilyMember_DataGridView_IsDirty =
            Work_DataGridView_IsDirty =
            Exp_DataGridView_IsDirty =
            Education_DataGridView_IsDirty =
            Course_DataGridView_IsDirty =
            Loss_DataGridView_IsDirty =
            Illness_DataGridView_IsDirty =
            Asset_DataGridView_IsDirty =
            AidSource_DataGridView_IsDirty =
            HomePowerSource_DataGridView_IsDirty =
            PowerSource_DataGridView_IsDirty =
            Material_DataGridView_IsDirty = false;

            MP_ID = P_Mobile = MP_ApplyDate = MP_Name = MP_RequestedAmount =
            P_Loss =
            P_Goal =         // MP_Description
            P_GoalHow =      // MP_Marketing
            P_OtherNotes =   // MP_OtherNotes
            P_FName = P_FaName = P_LName = P_MoName =
            P_EngName = P_Registration = P_NationalNum = DOB = P_IntermidiaryName =
            P_LandLine = P_Mobile2 = P_Address = P_HomeStateNote = P_Military = P_MilitaryDate =
            FamilyNumber =
            P_WorkYears =    // s3
            P_AidSourceNote =  //MP_OtherSources
            P_DeptAmount = P_DeptNote = P_SourceAssosiation = P_SourceRelative =
            MP_Address = MP_AddressNote = MP_Notes = "";

            SubCategory_ID = Career_ID = Type_ID = SubType_ID = FundType_ID =
            Sex_ID = PersonType_ID = P_MaritalState_ID =
            Parish_ID = Priest1_ID = Priest2_ID = Street_ID = HomeType_ID = Military_ID = MilitaryFinished =
            NumAtHome = NumAllMembers =
            Has_Education = Has_Course = Has_Loss = Has_Illness = Has_Asset = Has_AidSource = Has_Dept =
            Has_Income = MP_PlaceType = MP_AmperNumber = MP_AmperType =
            MP_Agreement1 = MP_Agreement2 = MP_Agreement3 =
            EntredBy = EditedBy = -1;

            //NumOfUpdateTry = 0;

            P_Goals_Query = FamilyMembers_Query =
            Work_Query = Exp_Query = Education_Query = Course_Query =
            Loss_Query = Illness_Query = Asset_Query = AidSource_Query =
            HomePowerSource_Query = PowerSource_Query = Material_Query = "";

            Delete_MP_button.Visible
                //= Save_button.Visible 
                = AddPartner_button.Visible
                = AddExtention_button.Visible = false;

            //AddImage_button.Visible = Download_button.Visible = DeleteImage_button.Visible = false;

            foreach (Control c in P_Goals_panel2.Controls)
            {
                if (c.GetType() == typeof(CheckBox))
                {
                    CheckBox cbox = c as CheckBox;
                    cbox.Checked = false;
                }
            }

            if (!Update_Mode)
            {
                Delete_MP_button.Visible
                    //= Save_button.Visible  
                    = AddPartner_button.Visible
                    = AddExtention_button.Visible = false;
            }
            else if (Update_Mode)
            {
                Delete_MP_button.Visible
                    //= Save_button.Visible 
                    = AddPartner_button.Visible
                    = AddExtention_button.Visible = true;

                if (P_HasAidSourceYes_radioButton.Checked)
                    AidSource_dataGridView.Enabled = true;

                if (P_HasAssetsYes_radioButton.Checked)
                    Assets_dataGridView.Enabled = true;

                if (P_HasCoursesYes_radioButton.Checked)
                    Course_dataGridView.Enabled = true;

                if (P_HasDebtsYes_radioButton.Checked)
                    P_Debts_tableLayoutPanel.Enabled = true;

                if (P_HasEducationYes_radioButton.Checked)
                    Education_dataGridView.Enabled = true;

                if (P_HasIllnessYes_radioButton.Checked)
                    Illness_dataGridView.Enabled = true;

                if (P_HasIncomeSourceYes_radioButton.Checked)
                    P_HasIncomeSource_flowLayoutPanel.Enabled = true;

                if (P_HasLossesYes_radioButton.Checked)
                    Loss_dataGridView.Enabled = true;

            }
        }


        private void FamilyMember_dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //FamilyMember_dataGridView["F_Number", e.RowIndex].Value = FamilyNum_textBox1.Text;
            FamilyMember_dataGridView["P_State", e.RowIndex].Value = 1;
            FamilyMember_dataGridView.Refresh();
        }
        private void FamilyMember_dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Calculate_NumAtHome();
            FamilyMember_dataGridView.Refresh();
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
                //TODO
                CheckUserPermission();
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف بيانات المستفيد مع المحافظة على شركاء (إن وجد) والمحافظة على رقم المشروع؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //if (dialogResult == DialogResult.Yes)
                //{
                //    if (Person_ID != -1)
                //    {
                //        Delete_Person(Person_ID);
                //        l.Insert_Log("Delete " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ",
                //            Program.UserName, DateTime.Now);

                //        clear_Person_boxes();
                //        Person_ID = -1;
                //        Save_MP_ID_button.Visible = true;

                //        //hide delete-partner-extention buttons on Create
                //        AddExtention_button.Visible = AddPartner_button.Visible = Delete_MP_button.Visible = false;

                //        //update mode is on because we didn't delete the project
                //        Update_Mode = true;

                //        MessageBox.Show("تم حذف المستفيد مع كل بياناته الشخصية والعائلية بنجاح");
                //    }
                //}
                //else
                //{
                //    MessageBox.Show(
                //            "لا يمكن حذف المستفيد! الرجاء اختيار استمارة من قائمة البحث أولاً ثم المحاولة مرة ثانية.");
                //}
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
                //TODO
                CheckUserPermission();
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف الاستمارة بشكل كامل مع كل بيانات المشروع والمستفيدين والشركاء؟", "Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    //if (MicroProject_ID != -1)
                    //{
                    //    Delete_MP(MicroProject_ID);
                    //    l.Insert_Log("Delete the project " + MicroProject_ID, "Micro Project", Program.UserName, DateTime.Now);

                    //    if (Person_ID != -1)
                    //    {
                    //        Delete_Person(Person_ID);
                    //        l.Insert_Log("Delete " + P_FirstName_textBox.Text + " " + P_LastName_textBox.Text, " Benefeciary ",
                    //            Program.UserName, DateTime.Now);
                    //    }

                    //    clear_Person_boxes();
                    //    clear_Project_boxes();
                    //    Person_ID = -1;
                    //    MicroProject_ID = -1;

                    //    Save_MP_ID_button.Visible = true;
                    //    Save_MP_ID_button.BackgroundImage = Properties.Resources.Unchecked;
                    //    toolTip1.SetToolTip(Save_MP_ID_button, "لم يتم حجز الرقم بعد");

                    //    //hide delete-partner-extention buttons on Create
                    //    AddExtention_button.Visible = AddPartner_button.Visible = Delete_MP_button.Visible = false;

                    //    Update_Mode = false;
                    //    mainForm.TabName_label.Text = "Application";
                    //    mainForm.MP_ID_label.Text = MPID_textBox.Text;

                    //    MessageBox.Show("تم حذف الاستمارة بنجاح");
                    //}
                    //else
                    //{
                    //    MessageBox.Show(
                    //            "لا يمكن حذف المشروع! الرجاء اختيار مشروع من قائمة البحث أولاً ثم المحاولة مرة ثانية.");
                    //}
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
        #endregion


    }
}
