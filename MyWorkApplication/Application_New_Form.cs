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
        DataSet ds, grid_ds;

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
            MyDataGridViewInitializationMethod();

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
                Get_Last_MicroProjectID();

                FamilyMember_dataGridView.AutoGenerateColumns =
                Work_dataGridView.AutoGenerateColumns = Exp_dataGridView.AutoGenerateColumns =
                Education_dataGridView.AutoGenerateColumns = Course_dataGridView.AutoGenerateColumns =
                Loss_dataGridView.AutoGenerateColumns = Illness_dataGridView.AutoGenerateColumns =
                Assets_dataGridView.AutoGenerateColumns =
                HomePowerSource_dataGridView.AutoGenerateColumns = PowerSource_dataGridView.AutoGenerateColumns =
                Materials_dataGridView.AutoGenerateColumns = false;

                Bind_All_ComboBoxes(-1);
                Bind_Gridview_ComboBox(-1);

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
                AidA_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                AidPeriodA_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                AidR_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                AidPeriodR_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                PaperApplicationUser_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

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

                Stop_Highlighting_Combobox();

                CheckUserPermission();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show("لا يمكن فتح هذه الاستمارة بسبب خطأ ما", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ////clear_Person_boxes();
                ////clear_Project_boxes();
                this.Close();
            }
        }

        private void Application_Form_Shown(object sender, EventArgs e)
        {
            Stop_Highlighting_Combobox();
        }

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

                //object Ob = ((ComboBox)sender).Items[e.Index];
                //// Draw the text.
                //if (Properties.Settings.Default.theme == "Light")
                //    e.Graphics.DrawString(((ComboBox)sender).GetItemText(((ComboBox)sender).Items[e.Index]), e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
                //else e.Graphics.DrawString(((ComboBox)sender).GetItemText(((ComboBox)sender).Items[e.Index]), e.Font, Brushes.LightGray, e.Bounds.X, e.Bounds.Y);

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
            AidA_comboBox.SelectionLength = 0; AidA_comboBox.SelectionStart = AidA_comboBox.Text.Length;
            AidPeriodA_comboBox.SelectionLength = 0; AidPeriodA_comboBox.SelectionStart = AidA_comboBox.Text.Length;
            AidR_comboBox.SelectionLength = 0; AidR_comboBox.SelectionStart = AidR_comboBox.Text.Length;
            AidPeriodA_comboBox.SelectionLength = 0; AidPeriodA_comboBox.SelectionStart = AidR_comboBox.Text.Length;
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
        #endregion

        #region datagridview with comboxBoxes methods
        private void MyDataGridViewInitializationMethod()
        {
            Work_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Work_dataGridView_EditingControlShowing);
            Exp_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Exp_dataGridView_EditingControlShowing);
            Education_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Education_dataGridView_EditingControlShowing);
            Course_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Course_dataGridView_EditingControlShowing);
            PowerSource_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(PowerSource_dataGridView_EditingControlShowing);
            Materials_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Materials_dataGridView_EditingControlShowing);
            FamilyMember_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(FamilyMember_dataGridView_EditingControlShowing);
            Loss_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Loss_dataGridView_EditingControlShowing);
            Illness_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Illness_dataGridView_EditingControlShowing);
            Assets_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(Assets_dataGridView_EditingControlShowing);
            AidSource_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(AidSource_dataGridView_EditingControlShowing);
            HomePowerSource_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(HomePowerSource_dataGridView_EditingControlShowing);
        }

        private void Work_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Exp_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Education_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Course_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void PowerSource_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Materials_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void FamilyMember_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Loss_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Illness_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void Assets_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void AidSource_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void HomePowerSource_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = (ComboBox)e.Control;
                cb.DrawMode = DrawMode.OwnerDrawVariable;
                cb.DrawItem -= ComboBox_DrawItem;
                cb.DrawItem += ComboBox_DrawItem;

                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
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

        private void HomePowerSource_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && HomePowerSource_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        private void Assets_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Assets_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void Illness_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Illness_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        private void Loss_dataGridView_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Loss_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void Course_dataGridView_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Make a specific column only accept numeric value in datagridview in Keypress event
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Course_dataGridView.CurrentCellAddress.X == 3)
            {
                e.Handled = true;
            }
            if (!char.IsNumber(e.KeyChar)
                && !char.IsControl(e.KeyChar)
                && Course_dataGridView.CurrentCellAddress.X == 4)
            {
                e.Handled = true;
            }
        }
        #endregion

        private void Get_Last_MicroProjectID()
        {
            string query = "select MAX(MP_ID) from `microproject`";
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            var n = 0;
            int.TryParse(sc.ExecuteScalar().ToString(), out n);
            MPID_textBox.Text = (++n).ToString();
            Program.MyConn.Close();
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

        private void Bind_All_ComboBoxes(int updated_id)
        {
            user_mode = false;
            string query = "";

            query += "SELECT ID, Name, Category_ID from `subcategory` ORDER BY Name ASC;"; //0
            query += "SELECT ID, Name, SubCategory_ID from `career` ORDER BY Name ASC;"; //1 
            query += "SELECT ID, Name from `street` order by Name ASC;"; //2
            query += "select Priest_ID as 'ID',Priest_Name as 'Name' from `priest` order by Priest_Name ASC;"; //3
            query += "select ID, Name from microprojecttype order by ID ASC;"; //4
            query += "select ID, Name, Type_ID from microprojectsubtype order by ID ASC;"; //5
            query += "select ID, Name, Type from `personstate` order by ID ASC;"; //6
            query += "select ID, Name from `maritalstatus` order by ID ASC;"; //7
            query += "select ID, Name_ar as 'Name' from `parish` order by ID ASC;"; //8

            query += "select ID, Name from `assettype` order by ID ASC;"; //9
            query += "select UserID as 'ID', UserName as 'Name' from `user` "
                  + " where IsVisitor = 1 order by UserName ASC;"; //10
            query += "SELECT ID, Name from `goal` order by ID ASC;"; //11


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
                //Multiple Combo Boxes With The Same Data Source (C#)
                P_Priest_comboBox.BindingContext = new BindingContext();   //create a new context
                P_Priest2_comboBox.DataSource = P_Priest_comboBox.DataSource = null;
                P_Priest2_comboBox.DisplayMember = P_Priest_comboBox.DisplayMember = "Name";
                P_Priest2_comboBox.ValueMember = P_Priest_comboBox.ValueMember = "ID";
                P_Priest2_comboBox.DataSource = P_Priest_comboBox.DataSource = ds.Tables[3];
                P_Priest2_comboBox.SelectedIndex = P_Priest_comboBox.SelectedIndex = -1;

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
            //if (updated_id == -1 || updated_id == 6) // -1 bind all 
            //{
            ////Multiple Combo Boxes With The Same Data Source (C#)
            //FPersonState_comboBox1.BindingContext = new BindingContext();   //create a new context
            //P_State_comboBox.DataSource = FPersonState_comboBox1.DataSource = null;
            //P_State_comboBox.DisplayMember = FPersonState_comboBox1.DisplayMember = "Name";
            //P_State_comboBox.ValueMember = FPersonState_comboBox1.ValueMember = "ID";
            //P_State_comboBox.DataSource = FPersonState_comboBox1.DataSource = ds.Tables[6];
            //P_State_comboBox.SelectedIndex = FPersonState_comboBox1.SelectedIndex = -1;
            //}
            if (updated_id == -1 || updated_id == 7) // -1 bind all 
            {
                P_MaritalState_comboBox.DataSource = null;
                P_MaritalState_comboBox.DisplayMember = "Name";
                P_MaritalState_comboBox.ValueMember = "ID";
                P_MaritalState_comboBox.DataSource = ds.Tables[7];
                P_MaritalState_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 8) // -1 bind all 
            {
                P_Parish_comboBox.DataSource = null;
                P_Parish_comboBox.DisplayMember = "Name";
                P_Parish_comboBox.ValueMember = "ID";
                P_Parish_comboBox.DataSource = ds.Tables[8];
                P_Parish_comboBox.SelectedIndex = -1;
            }

            if (updated_id == -1 || updated_id == 9) // -1 bind all 
            {
                //Multiple Combo Boxes With The Same Data Source (C#)
                P_HomeType_comboBox.BindingContext = new BindingContext();   //create a new context
                MP_PlaceType_comboBox.DataSource = P_HomeType_comboBox.DataSource = null;
                MP_PlaceType_comboBox.DisplayMember = P_HomeType_comboBox.DisplayMember = "Name";
                MP_PlaceType_comboBox.ValueMember = P_HomeType_comboBox.ValueMember = "ID";
                MP_PlaceType_comboBox.DataSource = P_HomeType_comboBox.DataSource = ds.Tables[9];
                MP_PlaceType_comboBox.SelectedIndex = P_HomeType_comboBox.SelectedIndex = -1;
            }
            if (updated_id == -1 || updated_id == 10) // -1 bind all 
            {
                PaperApplicationUser_comboBox.DataSource = null;
                PaperApplicationUser_comboBox.DisplayMember = "Name";
                PaperApplicationUser_comboBox.ValueMember = "ID";
                PaperApplicationUser_comboBox.DataSource = ds.Tables[10];
                PaperApplicationUser_comboBox.SelectedIndex = -1;
            }

            //bind goal checkboxes
            Bind_GoalsCheckBoxes(ds.Tables[11]);

            user_mode = true;
        }
        private void Bind_Gridview_ComboBox(int updated_id)
        {
            user_mode = false;
            string query = "";

            //education edited
            query += "select ID, Name, Type from `personstate` ORDER BY ID;";  //0 
            query += "select ID ,Name, Type_ID from `education` ORDER BY Name ASC;"; //1 
            query += "select W_ID as 'ID',W_Name as 'Name' from `work` ORDER BY W_Name ASC;"; //2
            query += "select ID ,Name from `powersource` ORDER BY ID ASC;"; //3
            query += "select ID ,Name from `aidsource` ORDER BY ID ASC;"; //4

            query += "select ID, Name from `maritalstatus` ORDER BY ID ASC;";  //5
            query += "select ID, Name from `relation` ORDER BY ID ASC;";  //6
            query += "select ID, Name from `worktype` ORDER BY ID ASC;";  //7
            query += "SELECT ID, Name, Category_ID from `subcategory` ORDER BY Name ASC;"; //8
            query += "SELECT ID, Name from `loss` ORDER BY ID ASC;"; //9
            query += "SELECT ID, Name from `illness` ORDER BY ID ASC;"; //10
            query += "SELECT ID, Name from `asset` ORDER BY ID ASC;"; //11

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
                E_Name.DisplayMember = "Name";
                E_Name.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 2) // -1 bind all 
            {
                W_Name.DataSource = grid_ds.Tables[2];
                W_Name.DisplayMember = "Name";
                W_Name.ValueMember = "ID";

                Exp_Name.DataSource = grid_ds.Tables[2];
                Exp_Name.DisplayMember = "Name";
                Exp_Name.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 3) // -1 bind all 
            {
                DataTable sourceTable = grid_ds.Tables[3];
                // Clone the table for independent binding
                DataTable source1 = sourceTable.Copy();
                DataTable source2 = sourceTable.Copy();

                Power_Name.DataSource = source1;
                Power_Name.DisplayMember = "Name";
                Power_Name.ValueMember = "ID";

                HPower_Name.DataSource = source2;
                HPower_Name.DisplayMember = "Name";
                HPower_Name.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 4) // -1 bind all 
            {
                //AidSource_Name
                Aid_Name.DataSource = grid_ds.Tables[4];
                Aid_Name.DisplayMember = "Name";
                Aid_Name.ValueMember = "ID";
            }

            //if (updated_id == -1 || updated_id == 5) // -1 bind all 
            //{
            //    P_MaritalStatus.DataSource = grid_ds.Tables[5];
            //    P_MaritalStatus.DisplayMember = "Name";
            //    P_MaritalStatus.ValueMember = "ID";
            //}
            if (updated_id == -1 || updated_id == 6) // -1 bind all 
            {
                P_Relation.DataSource = grid_ds.Tables[6];
                P_Relation.DisplayMember = "Name";
                P_Relation.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 7) // -1 bind all 
            {
                DataTable sourceTable = grid_ds.Tables[7];
                // Clone the table for independent binding
                DataTable source1 = sourceTable.Copy();
                DataTable source2 = sourceTable.Copy();
                DataTable source3 = sourceTable.Copy();

                P_WorkType.DataSource = source1;
                P_WorkType.DisplayMember = "Name";
                P_WorkType.ValueMember = "ID";

                W_WorkType.DataSource = source2;
                W_WorkType.DisplayMember = "Name";
                W_WorkType.ValueMember = "ID";

                Exp_WorkType.DataSource = source3;
                Exp_WorkType.DisplayMember = "Name";
                Exp_WorkType.ValueMember = "ID";

            }
            if (updated_id == -1 || updated_id == 8) // -1 bind all 
            {
                C_SubCategory.DataSource = grid_ds.Tables[8];
                C_SubCategory.DisplayMember = "Name";
                C_SubCategory.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 9) // -1 bind all 
            {
                L_Type.DataSource = grid_ds.Tables[9];
                L_Type.DisplayMember = "Name";
                L_Type.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 10) // -1 bind all 
            {
                Ill_Name.DataSource = grid_ds.Tables[10];
                Ill_Name.DisplayMember = "Name";
                Ill_Name.ValueMember = "ID";
            }
            if (updated_id == -1 || updated_id == 11) // -1 bind all 
            {
                Asset_Name.DataSource = grid_ds.Tables[11];
                Asset_Name.DisplayMember = "Name";
                Asset_Name.ValueMember = "ID";
            }
            user_mode = true;
        }
        private void Bind_GoalsCheckBoxes(DataTable dt)
        {
            P_Goals_panel2.Controls.Clear(); // Clear any old checkboxes
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

                P_Goals_panel2.Controls.Add(chk);
            }
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
                int NumOfActiveMembers = 0;
                int NumOfMembers = FamilyMember_dataGridView.Rows.Count; //all

                if (!string.IsNullOrWhiteSpace(P_FirstName_textBox.Text))
                {
                    NumOfActiveMembers++; NumOfMembers++;
                }
                ////  husband/wife
                //if (!string.IsNullOrWhiteSpace(FPersonFName_textBox1.Text)
                //    && FPersonState_comboBox1.SelectedIndex != -1)
                //{
                //    string value = FPersonState_comboBox1.SelectedValue.ToString();
                //    DataRow[] rows = null; int id;
                //    if (int.TryParse(value, out id)) rows = ds.Tables[6].Select("ID=" + id);
                //    else rows = ds.Tables[6].Select("Name='" + value + "'");
                //    if (rows.Length != 0)
                //    {
                //        if (rows[0].Field<string>(2).ToLower() == "active")
                //            NumberOfMembers++;
                //    }
                //}

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
                                NumOfActiveMembers++;
                        }
                    }
                P_NumAtHome_textBox.Text = NumOfActiveMembers.ToString();
                P_NumOfMembers_textBox.Text = NumOfMembers.ToString();
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


        #region education Work Exp course material family

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


                            var query = "delete from `person_education` where PEdu_ID = " + ID;
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
            /*
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
            }*/
        }
        private void Education_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4) // select the type to fill the education
                {
                }
                if (e.ColumnIndex == 5) //check if type iserted before the name
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
                if (e.ColumnIndex == 5) //type updated and enter to name
                {
                    var Edu_Type = "";
                    if (Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value == null ||
                        Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString() == "")
                        throw new Exception("الرجاء قم باختيار درجة التحصيل العلمي أولا ثم حاول إدخال مجال الدراسة");
                    Edu_Type = Education_dataGridView.Rows[e.RowIndex].Cells["E_Type"].Value.ToString();

                    Education_dataGridView.Rows[e.RowIndex].Cells[5] = Load_Education_ComboBox(Edu_Type, "");
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
                                var PW_ID = Convert.ToInt32(Work_dataGridView.Rows[e.RowIndex].Cells["W_RowID"].Value
                                    .ToString());

                                var query = "delete from `person_workexp` where W_Status like 'Yes' and PW_ID = " + PW_ID +
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
                                var PW_ID = Convert.ToInt32(Exp_dataGridView.Rows[e.RowIndex].Cells["Exp_RowID"].Value
                                    .ToString());

                                var query = "delete from `person_workexp` " +
                                            " where W_Status like 'No' and PW_ID = " + PW_ID + "";
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
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void Course_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //TODO
        }
        private void Course_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                /*
                if (e.RowIndex == Exp_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == Exp_dataGridView.Columns["C_DeleteRow"].Index)
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
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Course_dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //TODO
        }

        /// //////////////////////////////////////////////////////////////////////

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
                /*
                if (!string.IsNullOrWhiteSpace(Materials_dataGridView.CurrentCell.Value.ToString()))
                {
                    if (e.ColumnIndex == Materials_dataGridView.Columns["M_Price"].Index
                         //   || e.ColumnIndex == Materials_dataGridView.Columns["M_LocalContribution"].Index
                         || e.ColumnIndex == Materials_dataGridView.Columns["M_Amount"].Index)
                    {
                        string value = Materials_dataGridView.CurrentCell.Value.ToString();
                        double deci_val = 0;
                        double.TryParse(value, out deci_val);
                        Materials_dataGridView.CurrentCell.Value = deci_val.ToString("#,##0");

                        calculate_SumOfBudgetItems();
                    }
                }
                else Materials_dataGridView.CurrentCell.Value = "";*/
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

        private void AidSource_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == AidSource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == AidSource_dataGridView.Columns["Aid_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        if (AidSource_dataGridView["Aid_InDataBase", e.RowIndex].Value == null)
                        {
                            AidSource_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف مصدر الطاقة للمشروع؟", "Delete",
                                MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var Row_ID = Convert.ToInt32(AidSource_dataGridView.Rows[e.RowIndex].Cells["Aid_RowID"].Value.ToString());

                                var query = "DELETE FROM `person_aidsource` WHERE ID = " + Row_ID + "";
                                Program.buildConnection();
                                var sc = new MySqlCommand(query, Program.MyConn);
                                sc.ExecuteNonQuery();
                                Program.MyConn.Close();

                                l.Insert_Log("DELETE aidsource: " + AidSource_dataGridView.Rows[e.RowIndex].Cells["Aid_Name"].Value
                                    + " from project: " + MicroProject_ID, "aidsource", Program.UserName, DateTime.Now);

                                AidSource_dataGridView.Rows.RemoveAt(e.RowIndex);
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
                if (e.RowIndex == AidSource_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == AidSource_dataGridView.Columns["Aid_DeleteRow"].Index)
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
            AidSource_dataGridView.EndEdit();
            if (e.ColumnIndex == AidSource_dataGridView.Columns["Aid_DateYear"].Index)
            {
                if (AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                    AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value &&
                    AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    string value = AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    int year = 0;
                    if (!int.TryParse(value, out year))
                    {
                        MessageBox.Show("الرجاء إدخال السنة على الشكل التالي 2024!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    else
                        AidSource_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = year.ToString("");
                }
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
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2,
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
