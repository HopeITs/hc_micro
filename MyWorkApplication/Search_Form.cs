using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml.Office;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using PresentationControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace MyWorkApplication
{
    public partial class Search_Form : Form
    {
        private DataGridViewColumnSelector cs;
        private string ApplyDate_condition = "";
        private string FundedAmount_condition = "";
        private string FundedDate_condition = "";
        private string RequestedAmount_condition = "";
        private string visitDate_condition = "";
        private string lastvisit_condition = "";
        private string mevisitDate_condition = "";
        private string Original_Filter_String = "";

        private bool Hided;
        private int[] IDs_array, PIDs_array;
        private int MicroProject_ID, Person_ID, Family_ID, Image_ID, ExeFile_ID;
        private string[] P_Names_array;
        private int PW;

        private DataRow SelectedDataRow, LoanSelectedDataRow, PersonSelectedDataRow;
        private bool show_images;
        private List<string> searchBy_list;
        private int total_bind_count;

        private MainForm main_form;
        MySqlComponents MySS;
        NewTheme NewTheme;
        Select s; Log l;
        Street st; SubCategory sub; Career career;
        bool user_mode;
        DataSet ds, combo_ds;
        private int tick;
        BindingSource bs = new BindingSource();
        string condition = "";

        public Search_Form(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        public Search_Form(MainForm main_form
            , string FundType, string Type, string SubType
            , string Partners, string State
            , string Donor, string DonorGroup
            , string Category, string SubCategory, string Career
            , string Parish, string Age, string Sex, string MaritalStatus
            , string h_street, string w_street)
        {
            InitializeComponent();
            this.main_form = main_form;

            user_mode = false;
            FundType_comboBox.Text = FundType;
            Type_comboBox.Text = Type;
            SubType_comboBox.Text = SubType;

            MP_Status_comboBox.Text = State;
            Partnership_comboBox.Text = Partners;

            Donor_comboBox.Text = Donor;
            DonorGroup_comboBox.Text = DonorGroup;

            Category_comboBox.Text = Category;
            SubCategory_comboBox.Text = SubCategory;
            Career_comboBox.Text = Career;

            P_Parish_comboBox.Text = Parish;
            Age_comboBox.Text = Age;
            Gender_comboBox.Text = Sex;
            MaritalStatus_comboBox.Text = MaritalStatus;

            HomeStreet_comboBox.Text = h_street;
            ProjectStreet_comboBox.Text = w_street;

            user_mode = true;
        }

        private void Search_Form_Load(object sender, EventArgs e)
        {
            try
            {
                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                st = new Street();
                sub = new SubCategory();
                career = new Career();
                NewTheme = new NewTheme();

                if (Settings.Default.theme == "Light") NewTheme.Search_ToLight(this, false);
                else NewTheme.Search_ToNight(this, false);

                PW = Search_panel.Height;
                Hided = true;
                Search_panel.Height = 0;
                MP_dataGridView.DoubleBuffered(true);

                //to scroll the show/hide lists 
                Hide_MenuItem.DropDown.Closing += Hide_MenuItem_Closing;
                Show_MenuItem.DropDown.Closing += Show_MenuItem_Closing;

                searchBy_list = new List<string>();
                searchBy_list.Add("Applications");
                searchBy_list.Add("Families");
                searchBy_list.Add("Loans and Payments");
                searchBy_list.Add("Attachments");
              //  searchBy_list.Add("Executive Files");
                searchBy_list.Add("M&E Visits");
                searchBy_list.Add("Monitoring Visits(1 to 5)");
                searchBy_list.Add("Check Lists");
                searchBy_list.Add("Special Cases");
                searchBy_list.Add("Aid Sources");

                for (int i = 0; i < searchBy_list.Count; i++)
                    SearchBy_comboBox.Items.Add(searchBy_list.ElementAt<string>(i));

                bs.ListChanged += bs_ListChanged;
                Bind_All_ComboBoxes();

                CheckUserPermission();
                SearchBy_comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin  
                case 3: //Financial 
                case 8: //admin l0
                    break;
                case 2: //Data
                case 4: //Guest 
                case 6: //Out Of Service
                case 7: //Lawful
                case 9: //Media
                    {
                        SearchBy_comboBox.Items.Remove("Loans and Payments");
                    }
                    break;
            }
        }

        private void Check_Visibility()
        {
            ApplyDateFrom_label.Text = "Apply Date:";
            if (Sum_FamilyMembers_label.Visible) Sum_FamilyMembers_label.Visible = false;
            if (visit_statistics_tableLayoutPanel.Visible) visit_statistics_tableLayoutPanel.Visible = false;
            if (Financial_tableLayoutPanel.Visible) Financial_tableLayoutPanel.Visible = false;
            if (Export_dolar_label.Visible) Export_dolar_label.Visible = false;
            if (showHideProfileImagesToolStripMenuItem.Visible) showHideProfileImagesToolStripMenuItem.Visible = false;
            if (Delete_button.Visible) Delete_button.Visible = false;
            LastVisit_checkBox.Visible = false;

            // applications //
            if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible =
                    P_Parish_comboBox.Visible = Age_comboBox.Visible =
                    Gender_comboBox.Visible = MaritalStatus_comboBox.Visible =
                    HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = true;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_comboBox.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = P_Parish_label.Visible = Age_label.Visible = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible
                    = true;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible =
                    ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;
                //Requested_checkBox.Visible = Funded_checkBox.Visible = 

                showHideProfileImagesToolStripMenuItem.Visible = true;
                Sum_FamilyMembers_label.Visible = true;
            }
            // families //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(1))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible =
                    P_Parish_comboBox.Visible = Age_comboBox.Visible = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible =
                    HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = P_Parish_label.Visible = Age_label.Visible = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible
                    = false;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible =
                    ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible = 

                //delete button//
                if (Program.role == 1 || Program.role == 8)
                    Delete_button.Visible = true;
                else Delete_button.Visible = false;
            }
            // Loans //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(2))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
                    = true;
                P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = false;


                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = true;

                P_Parish_label.Visible = Age_label.Visible
                    = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible
                    = false;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible =
                    from_label1.Visible = to_label1.Visible = year_label1.Visible =
                    from_label2.Visible = to_label2.Visible = year_label2.Visible =
                    ApplyDateFrom_label.Visible = FundedDateFrom_label.Visible =
                    ApplyDateFrom_dateTimePicker.Visible = ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_DateTimePicker.Visible = FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;
                //Requested_checkBox.Visible = Funded_checkBox.Visible =  

                Export_dolar_label.Visible = Export_val_label.Visible =
                Remaining_val_label.Visible = Import_val_label.Visible =
                Returned_val_label.Visible = Net_val_label.Visible = true;

                Financial_tableLayoutPanel.Visible = true;
            }
            // Attachments //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(3))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
                    = true;
                P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = true;

                P_Parish_label.Visible = Age_label.Visible = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible = false;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible =
                    from_label1.Visible = to_label1.Visible = year_label1.Visible =
                    from_label2.Visible = to_label2.Visible = year_label2.Visible =
                    ApplyDateFrom_label.Visible = FundedDateFrom_label.Visible =
                    ApplyDateFrom_dateTimePicker.Visible = ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_DateTimePicker.Visible = FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible =

                showHideProfileImagesToolStripMenuItem.Visible = true;
            }
            ////Executive Files //
            //else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
            //{
            //    FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
            //        MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
            //        Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
            //        FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
            //        = true;
            //    P_Parish_comboBox.Visible = Age_comboBox.Visible
            //        = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
            //        = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
            //        = false;

            //    FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
            //        = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
            //        = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
            //        = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
            //        = true;

            //    P_Parish_label.Visible = Age_label.Visible
            //        = Gender_label.Visible = MaritalStatus_label.Visible
            //        = HomeStreet_label.Visible = ProjectStreet_label.Visible
            //        = false;

            //    RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
            //        RequestedFrom_label.Visible =
            //        FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
            //        FundedFrom_label.Visible =
            //        from_label1.Visible = to_label1.Visible = year_label1.Visible =
            //        from_label2.Visible = to_label2.Visible = year_label2.Visible =
            //        ApplyDateFrom_label.Visible = FundedDateFrom_label.Visible =
            //        ApplyDateFrom_dateTimePicker.Visible = ApplyDateTo_DateTimePicker.Visible =
            //        FundedDateFrom_DateTimePicker.Visible = FundedDateTo_DateTimePicker.Visible =
            //        ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;
            //    //Requested_checkBox.Visible = Funded_checkBox.Visible = 

            //    Export_dolar_label.Visible = Export_val_label.Visible = false;
            //    Remaining_val_label.Visible = Import_val_label.Visible = false;
            //}
            // Visits //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
            {
                //statistics table
                visit_statistics_tableLayoutPanel.Visible = true;

                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
                 = true;
                P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = true;
                P_Parish_label.Visible = Age_label.Visible
                    = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible
                    = false;

                ApplyDateFrom_label.Text = "Last Visit Date:";
                from_label1.Visible = to_label1.Visible = year_label1.Visible =
                    ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;

                from_label2.Visible = to_label2.Visible = year_label2.Visible =
                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible = false;
            }
            // New Visit Forms //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(5))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible =
                    P_Parish_comboBox.Visible = Age_comboBox.Visible = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = true;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = P_Parish_label.Visible = Age_label.Visible = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible
                    = true;

                LastVisit_checkBox.Visible = true;
                ApplyDateFrom_label.Text = "Last Visit Date:";
                ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible =  
            }
            // Checklists //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(6))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                    MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                    Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                    FundDate_comboBox.Visible = ApplyDate_comboBox.Visible =
                    P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible
                    = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = P_Parish_label.Visible = Age_label.Visible
                    = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible = false;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible = RequestedFrom_label.Visible =
                        FundedFrom_textBox.Visible = FundedTo_textBox.Visible = FundedFrom_label.Visible =
                            ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                            ApplyDateTo_DateTimePicker.Visible =
                            FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                            FundedDateTo_DateTimePicker.Visible =
                            ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible = 
            }
            // Special Cases //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(7))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
                 = true;
                P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = true;
                P_Parish_label.Visible = Age_label.Visible
                    = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible = false;

                ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible = 
            }
            // Aid Sources //
            else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(8))
            {
                FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible = Partnership_comboBox.Visible =
                MP_Status_comboBox.Visible = Donor_comboBox.Visible = DonorGroup_comboBox.Visible =
                Category_comboBox.Visible = SubCategory_comboBox.Visible = Career_comboBox.Visible =
                FundDate_comboBox.Visible = ApplyDate_comboBox.Visible
                 = true;
                P_Parish_comboBox.Visible = Age_comboBox.Visible
                    = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                    = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible = false;

                FundType_label.Visible = Type_label.Visible = SubType_label.Visible = Partnership_label.Visible
                    = MP_Status_label.Visible = Donor_label.Visible = DonorGroup_label.Visible
                    = Category_label.Visible = SubCategory_label.Visible = Career_label.Visible
                    = FundedDateFrom_label.Visible = ApplyDateFrom_label.Visible
                    = true;
                P_Parish_label.Visible = Age_label.Visible = Gender_label.Visible = MaritalStatus_label.Visible
                    = HomeStreet_label.Visible = ProjectStreet_label.Visible = false;

                ApplyDateFrom_label.Visible = ApplyDateFrom_dateTimePicker.Visible =
                    ApplyDateTo_DateTimePicker.Visible =
                    FundedDateFrom_label.Visible = FundedDateFrom_DateTimePicker.Visible =
                    FundedDateTo_DateTimePicker.Visible =
                    ApplyDate_checkBox.Visible = FundedDate_checkBox.Visible = true;

                RequestedFrom_textBox.Visible = RequestedTo_textBox.Visible =
                    RequestedFrom_label.Visible =
                    FundedFrom_textBox.Visible = FundedTo_textBox.Visible =
                    FundedFrom_label.Visible = false;
                //Requested_checkBox.Visible = Funded_checkBox.Visible = 
            }
        }

        private void Calculate_ImportExportMoney(string str)
        {
            string donor = "";
            if (Donor_comboBox.Text != "")
            {
                donor = Program.ReplaceQuotation(Donor_comboBox.Text);
            }

            var dt2 = s.Select_Loan_Sum(str, donor);
            if (dt2.Rows.Count != 0)
            {
                Export_val_label.Text = "Exports:" + (dt2.Rows[0]["Export"] == DBNull.Value
                    ? "0"
                    : Convert.ToDecimal(dt2.Rows[0]["Export"]).ToString("#,##0"));
                Export_dolar_label.Text = "Exports($):" + (dt2.Rows[0]["Export($)"] == DBNull.Value
                    ? "0"
                    : Convert.ToDecimal(dt2.Rows[0]["Export($)"]).ToString("#,##0"));
                Import_val_label.Text = "Imports:" + (dt2.Rows[0]["Import"] == DBNull.Value
                    ? "0"
                    : Convert.ToDecimal(dt2.Rows[0]["Import"]).ToString("#,##0"));
                Remaining_val_label.Text = "Remaining:" + (dt2.Rows[0]["Remaining"] == DBNull.Value
                    ? "0"
                    : Convert.ToDecimal(dt2.Rows[0]["Remaining"]).ToString("#,##0"));

                Returned_val_label.Text = "Returned:" + (dt2.Rows[0]["Returned"] == DBNull.Value
                   ? "0"
                   : Convert.ToDecimal(dt2.Rows[0]["Returned"]).ToString("#,##0"));

                Net_val_label.Text = "Net:" + (dt2.Rows[0]["Net"] == DBNull.Value
                   ? "0"
                   : Convert.ToDecimal(dt2.Rows[0]["Net"]).ToString("#,##0"));
            }
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            save_visible_columns(SearchBy_comboBox.SelectedIndex);
            SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }

        private void showHideProfileImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (show_images)
                    show_images = false;
                else show_images = true;
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void RequestedFrom_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RequestedFrom_textBox.Text != "")
                {
                    RequestedFrom_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(RequestedFrom_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    RequestedFrom_textBox.SelectionStart = RequestedFrom_textBox.Text.Length;
                    RequestedFrom_textBox.SelectionLength = 0;
                }

                if (RequestedTo_textBox.Text != "")
                {
                    RequestedTo_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(RequestedTo_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    RequestedTo_textBox.SelectionStart = RequestedTo_textBox.Text.Length;
                    RequestedTo_textBox.SelectionLength = 0;
                }

                if (FundedFrom_textBox.Text != "")
                {
                    FundedFrom_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(FundedFrom_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    FundedFrom_textBox.SelectionStart = FundedFrom_textBox.Text.Length;
                    FundedFrom_textBox.SelectionLength = 0;
                }

                if (FundedTo_textBox.Text != "")
                {
                    FundedTo_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(FundedTo_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    FundedTo_textBox.SelectionStart = FundedTo_textBox.Text.Length;
                    FundedTo_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RequestedFrom_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                RequestedAmount_condition = "";
                if (RequestedFrom_textBox.Text != "" || RequestedTo_textBox.Text != "")
                {
                    var NeededFrom = RequestedFrom_textBox.Text.Replace(",", "");
                    var NeededTo = RequestedTo_textBox.Text.Replace(",", "");

                    double Needed_from_d, Needed_to_d;
                    if (NeededFrom == "")
                        Needed_from_d = 0;
                    else Needed_from_d = Convert.ToDouble(NeededFrom);

                    if (NeededTo == "")
                        Needed_to_d = 0;
                    else Needed_to_d = Convert.ToDouble(NeededTo);
                    RequestedAmount_condition = " and (`MP_RequestedAmount` BETWEEN "
                        + Needed_from_d + " and " + Needed_to_d + ") ";

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FundedFrom_textBox_Leave(object sender, EventArgs e)
        {
            FundedAmount_condition = "";
            if (FundedFrom_textBox.Text != "" || FundedTo_textBox.Text != "")
            {
                var FinancedFrom = FundedFrom_textBox.Text.Replace(",", "");
                var FinancedTo = FundedTo_textBox.Text.Replace(",", "");

                double Financed_from_d, Financed_To_d;
                if (FinancedFrom == "")
                    Financed_from_d = 0;
                else Financed_from_d = Convert.ToDouble(FinancedFrom);

                if (FinancedTo == "")
                    Financed_To_d = 0;
                else Financed_To_d = Convert.ToDouble(FinancedTo);
                FundedAmount_condition +=
                   // " and (L.Loan_Amount BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";
                   " and ( Round( (Loan_Amount / donorgroup.Rate) , 2 ) BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";

                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }
        ///////////////////////////////////////////////////////////////////////////

        private void ApplyDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ApplyDate_checkBox.Checked)
            {
                ApplyDateFrom_dateTimePicker.Enabled = ApplyDateFrom_label.Enabled
                    = ApplyDateTo_DateTimePicker.Enabled = true;

                ApplyDateFrom_dateTimePicker_ValueChanged(sender, e);
            }
            else
            {
                ApplyDate_condition = visitDate_condition = mevisitDate_condition = "";
                ApplyDateFrom_dateTimePicker.Enabled = ApplyDateFrom_label.Enabled
                    = ApplyDateTo_DateTimePicker.Enabled = false;

                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void FundedDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FundedDate_checkBox.Checked)
            {
                FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled
                    = FundedDateTo_DateTimePicker.Enabled = true;
                FundedDateFrom_bcDateTimePicker_ValueChanged(sender, e);
            }
            else
            {
                FundedDate_condition = "";
                FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled
                    = FundedDateTo_DateTimePicker.Enabled = false;
                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void LastVisit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            lastvisit_condition = "";
            if (LastVisit_checkBox.Checked)
            {
                lastvisit_condition = @" and mevisit.Date = (SELECT MAX(me.Date) FROM mevisit me WHERE me.Person_ID = P.P_ID ) Group By P.P_ID,MP.MP_ID ";
            }
            //SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }
        /////////////////  #############  /////////////////
        private void ApplyDateFrom_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ApplyDateFrom_dateTimePicker.Value > ApplyDateTo_DateTimePicker.Value)
                {
                    ApplyDate_condition = visitDate_condition = mevisitDate_condition = "";
                    throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");
                }

                string beg, end;
                beg = ApplyDateFrom_dateTimePicker.Value.ToString("yyyy/MM/dd");
                end = ApplyDateTo_DateTimePicker.Value.ToString("yyyy/MM/dd");

                ApplyDate_condition = " and ( MP_DateOfRequest between '" + beg + "' and '" + end + "' )";
                visitDate_condition = " and ( visit.Date between '" + beg + "' and '" + end + "' )";
                mevisitDate_condition = " and ( mevisit.Date between '" + beg + "' and '" + end + "')";

                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FundedDateFrom_bcDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FundedDateFrom_DateTimePicker.Value > FundedDateTo_DateTimePicker.Value)
                    throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");

                DateTime beg, end;
                beg = FundedDateFrom_DateTimePicker.Value;
                end = FundedDateTo_DateTimePicker.Value;

                FundedDate_condition = " and MP_ID in (select MicroProject_ID from loan where " +
                                       " Loan_DateTaken between '" + beg.Year + "/" + beg.Month + "/" + beg.Day + "' " +
                                       " and '" + end.Year + "/" + end.Month + "/" + end.Day + "' )";

                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            save_visible_columns(SearchBy_comboBox.SelectedIndex);
        }

        //private void ShowHideColumns_button_MouseClick(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        cs.mDataGridView_MouseClick(sender, e);
        //    }
        //    catch(Exception ex)
        //    { MessageBox.Show(ex.Message); }
        //} 
        private void show_hide_button_Click(object sender, EventArgs e)
        {
            if (Hided)
            {
                Hided = false;
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Hide2_L;
                else ShowHide_button.BackgroundImage = Resources.Hide2_D;
                Search_panel.Height = PW;
                ApplyFilter_button.Visible = ApplyFilter_button.Enabled = true;
            }
            else
            {
                Hided = true;
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Show2_L;
                else ShowHide_button.BackgroundImage = Resources.Show2_D;
                Search_panel.Height = 0;
                ApplyFilter_button.Visible = ApplyFilter_button.Enabled = false;
            }
        }


        public void Check_Conditions(string initial_cond)
        {
            condition = "";
            condition += initial_cond;
            //if (kind != "")
            //{
            //    if (kind == "Vehicles") condition += " and Category_ID in (1,2) ";
            //    else condition += " and Category_ID not in (1,2) ";
            //}

            //Fund Type
            if (FundType_comboBox.Text != "" && FundType_comboBox.Visible)
            {
                //condition += " and fundtype.Name like '" + FundType + "'";
                condition += " and fundtype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(FundType_comboBox, true) + ")";
            }

            //Type
            if (Type_comboBox.Text != "" && Type_comboBox.Visible)
            {
                //condition += " and microprojecttype.Name like '" + Type + "'";
                condition += " and microprojecttype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Type_comboBox, true) + ")";
            }

            //Sub Type
            if (SubType_comboBox.Text != "" && SubType_comboBox.Visible)
            {
                //condition += " and microprojectsubtype.Name like '" + SubType + "'";
                condition += " and microprojectsubtype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(SubType_comboBox, true) + ")";
            }

            //Partners
            if (Partnership_comboBox.Text != "" && Partnership_comboBox.Visible)
            {
                if (Partnership_comboBox.Text.Contains("شراكة"))
                    condition += " and Partnership > 1 ";

                if (Partnership_comboBox.Text.Contains("فردي"))
                    condition += " and Partnership = 1 ";
            }

            //MP Status
            if (MP_Status_comboBox.Text != "" && MP_Status_comboBox.Visible)
            { 
                condition += " and state.Name_Ar in ("
                    + Program.getSelectedItems_CheckboxComboBox(MP_Status_comboBox, true) + ")";
            }
            //Donor
            if (Donor_comboBox.Text != "" && Donor_comboBox.Visible)
            {
                //condition += " and donor.Name like '" + Donor + "'";
                condition += " and donor.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Donor_comboBox, true) + ")";
            }
            //Donor Group
            if (DonorGroup_comboBox.Text != "" && DonorGroup_comboBox.Visible)
            {
                condition += " and donorgroup.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(DonorGroup_comboBox, true) + ")";
            }
            //Parish
            if (P_Parish_comboBox.Text != "" && P_Parish_comboBox.Visible)
            {
                //if (Parish == "Christian")
                //    condition += " and parish.Name_en not like  N'Muslim'";
                //else condition += " and parish.Name_en like  N'" + Parish + "'";
                condition += " and parish.Name_en in ("
                    + Program.getSelectedItems_CheckboxComboBox(P_Parish_comboBox, true) + ")";
            }

            //Category
            if (Category_comboBox.Text != "" && Category_comboBox.Visible)
            {
                condition += " and C.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Category_comboBox, true) + ")";
            }
            //SUBCategory
            if (SubCategory_comboBox.Text != "" && SubCategory_comboBox.Visible)
            {
                condition += " and sub.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(SubCategory_comboBox, true) + ")";
            }
            //Career
            if (Career_comboBox.Text != "" && Career_comboBox.Visible)
            {
                condition += " and career.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Career_comboBox, true) + ")";
            }
            //Marital Status
            if (MaritalStatus_comboBox.Text != "" && MaritalStatus_comboBox.Visible)
            {
                condition += " and maritalstatus.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(MaritalStatus_comboBox, true) + ")";
            }

            //Apply Date
            if (ApplyDate_comboBox.Text != "" && ApplyDate_checkBox.Visible)
            {
                condition += " and Year(MP_DateOfRequest) in ("
                   + Program.getSelectedItems_CheckboxComboBox(ApplyDate_comboBox, true) + ")";
            }
            //Fund Date
            if (FundDate_comboBox.Text != "" && FundDate_comboBox.Visible)
            {
                condition += " and Year(Loan_DateTaken) in ("
                    + Program.getSelectedItems_CheckboxComboBox(FundDate_comboBox, true) + ")";
            }
            //Age 
            if (Age_comboBox.Text != "" && Age_comboBox.Visible)
            {
                DataRow[] rows
                    = combo_ds.Tables[9].Select("Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Age_comboBox, true) + ")");

                condition += " and TIMESTAMPDIFF(YEAR, `P_DOB`, MP_DateOfRequest) BETWEEN "
                    + rows[0][2].ToString() + " and " + rows[0][3].ToString();
            }
            //Gender
            if (Gender_comboBox.Text != "" && Gender_comboBox.Visible)
            {
                if (Gender_comboBox.Text.Contains("ذكر")) condition += " and P_Sex like N'%ذكر%'";
                if (Gender_comboBox.Text.Contains("أنثى")) condition += " and P_Sex like N'%أنثى%'";
            }

            //Home Street
            if (HomeStreet_comboBox.Text != "" && HomeStreet_comboBox.Visible)
            {
                condition += " and h_street.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(HomeStreet_comboBox, true) + ")";
            }

            //Project Street 
            if (ProjectStreet_comboBox.Text != "" && ProjectStreet_comboBox.Visible)
            {
                condition += " and w_street.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(ProjectStreet_comboBox, true) + ")";
            }

            //Apply Date
            if (ApplyDate_condition != "")
                condition += ApplyDate_condition;

            //Funded Date 
            if (FundedDate_condition != "")
                condition += FundedDate_condition;

            //Requested Amount
            if (RequestedAmount_condition != "")
                condition += RequestedAmount_condition;

            //Funded Amount 
            if (FundedAmount_condition != "")
                condition += FundedAmount_condition;
        }

        #region bind queries

        //string FundType, string Type, string SubType
        //    , string State, string Partners
        //    , string Donor, string DonorGroup
        //    , string Category, string SubCategory, string Career
        //    , string Parish, string Age, string Sex, string MaritalStatus
        //    , string ApplyYear, string FundYear, bool showImages

        
 ////// , (SELECT count(*)
 //////   From `person` p
 //////   left join `person_family` pf on p.P_ID = pf.Person_ID
 //////   where pf.P_Provider_ID = PMP.Person_ID
 //////   limit 1) as 'عدد الأفراد الكلي' 

 //////, (SELECT count(*)
 //////   From `person` p
 //////   left join `person_family` pf on p.P_ID = pf.Person_ID
 //////   left join `personstate` on personstate.ID = p.P_State_ID
 //////   where pf.P_Provider_ID = PMP.Person_ID
 //////   AND lower(personstate.Type) like 'active'
 //////   limit 1) as 'عدد الأفراد في المنزل' 



        private void bind_Applications()
        {
            try
            {
                string from = @" From `microproject` 
    LEFT JOIN `person_microproject` PMP on microproject.MP_ID = PMP.MicroProject_ID 
    LEFT JOIN `person` on person.P_ID = PMP.Person_ID  
    
    LEFT JOIN `maritalstatus` on person.P_MaritalStatus_ID = maritalstatus.ID 
    LEFT JOIN `parish` on person.P_Parish_ID = parish.ID 
    LEFT JOIN `category` C on microproject.Category_ID = C.ID 
    LEFT JOIN `subcategory` sub on microproject.SubCategory_ID = sub.ID  
    LEFT JOIN `career` on microproject.Career_ID = career.ID   
    LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID
    LEFT JOIN `state` on state.ID = mps.State_ID  
    
    LEFT JOIN `street` w_street on microproject.MP_Street_ID = w_street.ID 
    LEFT JOIN `street` h_street on person.Street_ID = h_street.ID 
  
    LEFT JOIN `priest` priest1 on priest1.Priest_ID = person.P_Priest_ID
    LEFT JOIN `priest` priest2 on priest2.Priest_ID = person.P_Priest2_ID

    LEFT JOIN `loan` on loan.MicroProject_ID = microproject.MP_ID
    LEFT JOIN `donor` on donor.ID = microproject.MP_Donor 
    LEFT JOIN `donorgroup` on donorgroup.ID = microproject.DonorGroup_ID 
    
    LEFT JOIN `fundtype` on microproject.MP_FundType_ID = fundtype.ID 
    LEFT JOIN `microprojecttype` on microproject.MP_Type_ID = microprojecttype.ID 
    LEFT JOIN `microprojectsubtype` on microproject.MP_SubType_ID = microprojectsubtype.ID ";

                string query1 = @" Select MP_ID as 'رقم المشروع'  
  , CONCAT(P_FirstName, ' ', P_LastName) as 'المستفيد'  
  , P_FatherName as 'اسم الأب'  
  , P_MotherName as 'اسم الأم'  
 
  , (SELECT CONCAT(p.P_FirstName, ' ', p.P_FatherName, ' ', p.P_LastName) 
      From `person` p 
      left join `person_family` pf on p.P_ID = pf.Person_ID 
      left join relation on relation.id = pf.Relation_ID
      WHERE relation.Name like N'زوج/ة' 
      and pf.P_Provider_ID = PMP.Person_ID limit 1) as 'الزوج / الزوجة' 
 
 , P_FullName_en as 'Beneficiary Name' 
 , career.Name_en as 'Career'
 , career.Name    as 'المهنة'
 , MP_Name as 'وصف المشروع'   

 , C.Name 'Category' 
 , sub.Name as 'Sub Category'

 , microprojecttype.Name as 'نوع المشروع'
 , microprojectsubtype.Name as 'تفصيل نوع المشروع' 

 , (select visit.VehicleNumber 
        from visit 
        where visit.Kind like 'V-op' 
        and visit.MicroProject_ID = MP_ID limit 1) as 'رقم لوحة المركبة'

 , fundtype.Name as 'نوع التمويل' 
 , MP_DateOfRequest as 'تاريخ التقديم'  
 
 , state.Name_Ar as 'حالة المشروع' 
 , mps.Date as 'تاريخ الحالة'  
 , (select count(*) 
        from microproject_state 
        where microproject_state.MicroProject_ID = MP_ID) as 'عدد الحالات'
 
 , IFNULL(Loan_Amount,0) as 'مبلغ التمويل'
 , Loan_DateTaken as 'تاريخ التمويل' 
 , IFNULL(donor.Name,'') as 'الجهة الممولة' 
 , IFNULL(donorgroup.Name,'') as 'المجموعة' 

 , (CASE IsContentUpdated WHEN 0 THEN N'لا' ELSE N'نعم' End) as 'تبديل المشروع' 

 , P_DOB as 'المواليد'  
 , TIMESTAMPDIFF(YEAR, P_DOB, MP_DateOfRequest) AS 'العمر عند تقديم الطلب'

 , parish.Name_en as 'الطائفة'

 , IFNULL(priest1.Priest_Name,'') as '1الكاهن'
 , IFNULL(priest2.Priest_Name,'') as '2الكاهن'
 
 , h_street.Name as 'منطقة السكن' 
 , P_HomeAddress as 'العنوان' 
 , P_RegistrationPlace as 'القيد' 
 , P_NationalNumber as 'الرقم الوطني' 
 , P_Mobile as 'الموبايل' 
 
 ,  IFNULL(P_NumAtHome,0) as 'عدد الأفراد الكلي'
 ,  IFNULL(P_NumOfMembers,0) as 'عدد الأفراد في المنزل'

  , (SELECT count(*) 
    From `person` p 
    left join `person_family` pf on p.P_ID = pf.Person_ID  
    left join `personstate` on personstate.ID = p.P_State_ID
    left join relation on relation.id = pf.Relation_ID
    WHERE relation.Name like N'ابن/ة' 
    AND lower(personstate.Type) like 'active'
    and pf.P_Provider_ID = PMP.Person_ID limit 1) as 'عدد الأبناء في المنزل' 

  , ( SELECT count(*) 
    From `person` p 
    left join `person_family` pf on p.P_ID = pf.Person_ID  
    left join `personstate` on personstate.ID = p.P_State_ID
    left join relation on relation.id = pf.Relation_ID
    WHERE relation.Name like N'ابن/ة'
    AND lower(personstate.Type) like 'active'    
 	and TIMESTAMPDIFF(YEAR, p.P_DOB, MP_DateOfRequest) < 18
    and pf.P_Provider_ID = person.P_ID limit 1 ) as 'عدد الأبناء تحت 18'
 
 , P_MilitaryService as 'خدمة العلم'
 , P_MilitaryService_Date as 'تاريخ التسريح'
 , P_MilitaryService_Note as 'وصف الحالة العسكرية' 

 , P_Sex as 'الجنس'
 , maritalstatus.Name as 'الحالة الاجتماعية'
  
 , w_street.Name as 'منطقة المشروع'
 , MP_AddressAfterFund  as 'موقع المشروع' 

 , (CASE MP_Visited WHEN 0 THEN N'غير مزار' ELSE N'مزار' End) as 'الزيارة' 
 , MP_VisitDate as 'تاريخ الزيارة'
 , MP_VisitTime as 'موعد الزيارة'

 , (CASE MP_Message WHEN 1 THEN N'تم الإرسال' ELSE 'لا يوجد' End) as 'رسالة الرفض' 

 , PMP.PersonType as 'صفة مقدم الطلب' 
  
 , PMP_ID as 'ID' 
 , P_ID as 'رقم المستفيد' 
 , microproject.ApplicationState as 'نوع الاستمارة'";

                if (show_images) query1 += @" ,(Select image.image_NewPath 
 from image left join imagetype on image.ImageType_ID = imagetype.ID
 where Person_ID = P_ID
 and MicroProject_ID = MP_ID 
 and imagetype.Name_en like 'Profile' limit 1 ) as 'الصورة' ";

                // use the public condition to use it when exporting applications
                string i_cond = " where Current_State = 1 ";
                Check_Conditions(i_cond);

                string order_by = " order by MP_ID DESC ";

                query1 += from + condition + order_by + ";";

                //count rows 
                //calculate sum of family members for all projects// 
                string query2 = "select IFNULL(count(DISTINCT MP_ID),0),IFNULL(sum(P_NumAtHome),0)" + from + condition + ";";

                ////////////////////////////////////////////////////////////////////
                string query = " ";
                query += query1 + query2;
                ////////////////////////////////////////////////////////////////////

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Program.MyConn.Close();

                MP_dataGridView.SuspendLayout();

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                MP_dataGridView.ResumeLayout();

                //////calculate sum of projects// 
                total_bind_count = 0;
                total_bind_count = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                Count_label.Text = "All:" + total_bind_count.ToString("#,##0");

                //////calculate sum of family members for all projects//  
                Decimal sumObject = 0;
                sumObject = Convert.ToDecimal(ds.Tables[1].Rows[0][1].ToString());
                Sum_FamilyMembers_label.Text = "مجموع الأفراد: " + sumObject.ToString("#,##0");

                MP_dataGridView.Columns["تاريخ التقديم"].DefaultCellStyle.Format = "dd/MM/yyyy";
                MP_dataGridView.Columns["تاريخ الحالة"].DefaultCellStyle.Format = "dd/MM/yyyy";
                MP_dataGridView.Columns["مبلغ التمويل"].DefaultCellStyle.Format = "#,##0";
                 
                MP_dataGridView.Columns["رقم المشروع"].Width = 70;
                MP_dataGridView.Columns["عدد الحالات"].Width = 50;

                MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الزيارة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["تاريخ التقديم"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["Category"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["Sub Category"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["Career"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["نوع المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["نوع التمويل"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الطائفة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الموبايل"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الرقم الوطني"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["حالة المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الجهة الممولة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["المجموعة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["خدمة العلم"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                MP_dataGridView.Columns["رقم المشروع"].Frozen = true;

                var dgC1 = MP_dataGridView.Columns["رقم المستفيد"];
                dgC1.Visible = false;
                var dgC2 = MP_dataGridView.Columns["ID"];
                dgC2.Visible = false;

                // Add State History Button //
                var colCB = new DataGridViewButtonColumn();
                colCB.Name = "StateHistory";
                colCB.HeaderText = "حالات المشروع";
                colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colCB.FlatStyle = FlatStyle.Flat;
                MP_dataGridView.Columns.Insert(17, colCB); // add after state
                MP_dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MP_dataGridView.Columns["StateHistory"].Width = 80;

                // /////////////////////////////////////////////////// 
                //if (show_images)
                //{
                //    var column = MP_dataGridView.Columns["#"];
                //    column.Width = 70;
                //    ((DataGridViewImageColumn)MP_dataGridView.Columns["#"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                //}  
                //foreach (var r in allrows.Except(dupRos))
                //    r.DefaultCellStyle.BackColor = Color.White;

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
         
        private void FamilyMember_bind()
        {
            string query = " select p.P_ID as 'رقم الفرد'"
                         + ",pf.Family_ID as 'رقم العائلة'"
                         + ",f.F_Number as 'رقم دفتر العائلة'"
                         + ",CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName) as 'المستفيد'"
                         + ",P_MotherName as 'اسم الأم'"
                         + ",maritalstatus.Name as 'الحالة الاجتماعية'"
                         + ",relation.Name as 'الصفة'"
                         + ",personstate.Name as 'الحالة'"
                         + ",pf.Work_Name as 'العمل'"
                         + ",p.P_DOB as 'تاريخ الولادة'"
                         + ",PMP.MicroProject_ID as 'رقم المشروع'"
                         + ",career.Name as 'المهنة'"
                         + ",career.Name_en as 'Career'"

                         + @",(select state.Name_Ar from state 
 left join microproject_state mps on state.ID = mps.State_ID
 where mps.MicroProject_ID = PMP.MicroProject_ID 
 and mps.Current_State = 1 limit 1) as 'حالة المشروع'
    
                            ,(select mps.Date from microproject_state mps 
 where mps.MicroProject_ID = PMP.MicroProject_ID
 and mps.Current_State = 1 limit 1) as 'تاريخ الحالة' "

                         + ",pf.P_Provider_ID as 'رقم المستفيد'"

                         + @" From `person` p 
    left join `person_family` pf on p.P_ID = pf.Person_ID  
    left join `family` f on f.F_ID = pf.Family_ID 
    left join `person_microproject` PMP on PMP.Person_ID = p.P_ID 
    left join `maritalstatus` on maritalstatus.ID = p.P_MaritalStatus_ID  
    left join `personstate` on personstate.ID = p.P_State_ID 
    left join `microproject` on PMP.MicroProject_ID = microproject.MP_ID
    left join `relation` on relation.ID = pf.Relation_ID 
    left join `career` on career.ID = microproject.Career_ID ";

            var i_cond = " where 1 ";
            //if (family_ID != "") 
            //    condition   = " where pf.Family_ID like CAST('" + family_ID + "' AS CHAR) "
            //                + " and mps.Current_State = 1 "; 
            Check_Conditions(i_cond);
            query += condition;

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            MP_dataGridView.DataSource = null;
            MP_dataGridView.Columns.Clear();
            MP_dataGridView.Rows.Clear();
            MP_dataGridView.ColumnHeadersVisible = false;
            MP_dataGridView.RowHeadersVisible = false;

            bs.DataSource = ds.Tables[0];
            MP_dataGridView.DataSource = bs;

            var dgC1 = MP_dataGridView.Columns["رقم المستفيد"];
            MP_dataGridView.Columns["تاريخ الولادة"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgC1.Visible = false;

            Program.buildConnection();
            //count rows
            var sel = "select count(*) FROM `person`";
            MySS.sc = new MySqlCommand(sel, Program.MyConn);
            decimal count = Convert.ToDecimal(MySS.sc.ExecuteScalar());
            Count_label.Text = "All:" + count.ToString("#,##0");
            Program.MyConn.Close();

            MP_dataGridView.Columns["رقم العائلة"].Width = 60;
            MP_dataGridView.Columns["رقم دفتر العائلة"].Width = MP_dataGridView.Columns["اسم الأم"].Width =
                MP_dataGridView.Columns["الصفة"].Width = MP_dataGridView.Columns["الحالة الاجتماعية"].Width = 100;

            MP_dataGridView.Columns["رقم العائلة"].DefaultCellStyle.Alignment =
            MP_dataGridView.Columns["الحالة"].DefaultCellStyle.Alignment =
                MP_dataGridView.Columns["رقم دفتر العائلة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["تاريخ الولادة"].DefaultCellStyle.Alignment =
                        MP_dataGridView.Columns["الحالة الاجتماعية"].DefaultCellStyle.Alignment =
                            MP_dataGridView.Columns["الصفة"].DefaultCellStyle.Alignment =
                                    DataGridViewContentAlignment.MiddleCenter;

            MP_dataGridView.ColumnHeadersVisible = true;
            MP_dataGridView.RowHeadersVisible = true;
        }

        private void Loan_bind()
        {
            MP_dataGridView.DataSource = null;
            MP_dataGridView.Columns.Clear();
            MP_dataGridView.Rows.Clear();
            MP_dataGridView.ColumnHeadersVisible = false;
            MP_dataGridView.RowHeadersVisible = false;

            var i_cond = "where Current_State = 1 ";
            Check_Conditions(i_cond);

            bs.DataSource = s.Select_Loan_All_Data("", condition);
            MP_dataGridView.DataSource = bs;

            MP_dataGridView.Columns["تاريخ استلام القرض"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["تاريخ آخر دفعة مدفوعة"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["تاريخ الدفعة المستحقة"].DefaultCellStyle.Format
                = "dd/MM/yyyy";

            MP_dataGridView.Columns["القرض"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["القرض ($)"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["القسط"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["المبلغ المدفوع"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["المبلغ المتبقي"].DefaultCellStyle.Format
                = MP_dataGridView.Columns["المبلغ المطلوب استرداده"].DefaultCellStyle.Format = "#,##0";

            MP_dataGridView.Columns["عدد الدفعات المتبقية"].DefaultCellStyle.Format = "N1";

            var dgc1 = MP_dataGridView.Columns["ID"];
            dgc1.Visible = false;

            decimal count = Convert.ToDecimal(s.Count_Distinct_Loan_IDs());
            Count_label.Text = "All:" + count.ToString("#,##0");

            MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["القسط"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["عدد الدفعات المتبقية"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["عدد الدفعات المدفوعة"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["عدد الدفعات الكلي"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["تاريخ استلام القرض"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["القرض"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["القرض ($)"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["النسبة"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["المجموعة"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["رقم الإيصال"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["المبلغ المطلوب استرداده"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["المبلغ المدفوع"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["تاريخ آخر دفعة مدفوعة"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["تاريخ الدفعة المستحقة"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["المبلغ المتبقي"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["أشهر التأخير"].DefaultCellStyle.Alignment
                = MP_dataGridView.Columns["الدفعات المستحقة والغير مدفوعة"].DefaultCellStyle.Alignment
                = DataGridViewContentAlignment.MiddleCenter;

            MP_dataGridView.ColumnHeadersVisible = true;
            MP_dataGridView.RowHeadersVisible = true;
        }

        private void bind_Images()
        {
            var from = @" from `image` 
 left OUTER join `imagetype` on image.ImageType_ID = imagetype.ID  
 left OUTER join `microproject` MP on image.MicroProject_ID = MP.MP_ID  
 LEFT OUTER join `person_microproject` PMP on PMP.MicroProject_ID = MP.MP_ID  
 LEFT OUTER join `person` P on P.P_ID = PMP.Person_ID  
 LEFT OUTER join `category` C on MP.Category_ID = C.ID  
 LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 LEFT OUTER join `career` on MP.Career_ID = career.ID 
 LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT OUTER JOIN `state` on state.ID = mps.State_ID 
 Left OUTER join `donor` on donor.ID = MP.MP_Donor
 LEFT OUTER join `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID
 LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID  ";

            string query = @"SELECT Image_ID as 'ID' 
 , image.MicroProject_ID as 'رقم المشروع' 
 , CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName) as 'المستفيد'
 , P_FullName_en as 'Beneficiary Name'
 , career.Name as 'المهنة' 
 , career.Name_en as 'Career'  
 , Image_NewPath as 'الصورة' 
 , imagetype.Name as 'النوع'
 , imagetype.Name_en as 'Image Type' ";

            var i_cond = " where Current_State = 1 ";
            Check_Conditions(i_cond);

            //get image types by user permissions//
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //Admin_L0 
                    {
                        break;
                    }
                case 3: //Financial
                    {
                        condition += " and imagetype.Category like 'أوراق مالية - Financial' ";
                        break;
                    }
                case 7: //Lawful
                    {
                        condition += " and imagetype.Category like 'أوراق قانونية - Legal' ";
                        break;
                    }
                case 2: //Data 
                    {
                        condition += " and (imagetype.Category like 'ثبوتيات شخصية - Personal' "
                                   + " or imagetype.Category like 'أوراق قانونية - Legal') ";
                        break;
                    }
            }


            query += from + condition + " order by image.MicroProject_ID asc ";

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            MP_dataGridView.DataSource = null;
            MP_dataGridView.Columns.Clear();
            MP_dataGridView.Rows.Clear();
            MP_dataGridView.ColumnHeadersVisible = false;
            MP_dataGridView.RowHeadersVisible = false;

            bs.DataSource = ds.Tables[0];
            MP_dataGridView.DataSource = bs;

            var dgC2 = MP_dataGridView.Columns["ID"];
            dgC2.Visible = false;

            Program.buildConnection();
            //count rows
            var sel = "select count(DISTINCT MP_ID) " + from + condition;
            MySS.sc = new MySqlCommand(sel, Program.MyConn);
            decimal count = Convert.ToDecimal(MySS.sc.ExecuteScalar());
            Count_label.Text = "All:" + count.ToString("#,##0");
            Program.MyConn.Close();

            MP_dataGridView.Columns["رقم المشروع"].Width = MP_dataGridView.Columns["النوع"].Width = 80;
            MP_dataGridView.Columns["الصورة"].Width = 200;

            MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                MP_dataGridView.Columns["النوع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الصورة"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //if (show_images)
            //{
            //    var column = MP_dataGridView.Columns["#"];
            //    column.Width = 70;
            //    ((DataGridViewImageColumn)MP_dataGridView.Columns["#"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            //}

            MP_dataGridView.ColumnHeadersVisible = true;
            MP_dataGridView.RowHeadersVisible = true;
        }

 //       private void bind_ExeFile()
 //       {
 //           var from = @" from `exefile` 
 //left OUTER join `microproject` MP on exefile.MicroProject_ID = MP.MP_ID  
 //LEFT OUTER join `person_microproject` PMP on PMP.MicroProject_ID = MP.MP_ID  
 //LEFT OUTER join `person` P on P.P_ID = PMP.Person_ID  
 //LEFT OUTER join `category` C on MP.Category_ID = C.ID  
 //LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 //LEFT OUTER join `career` on MP.Career_ID = career.ID
 //Left OUTER join `donor` on donor.ID = MP.MP_Donor 
 //LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 //LEFT OUTER JOIN `state` on state.ID = mps.State_ID 
 //LEFT OUTER join `loan` L on L.MicroProject_ID = MP.MP_ID
 //LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID
 //LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 //LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 //LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID  ";

 //           string query = " select PMP.MicroProject_ID as 'رقم المشروع'"
 //                        + ", CONCAT(P_FirstName,' ', P_LastName) as 'المستفيد'"
 //                        + ", P.P_FatherName as 'اسم الأب'"
 //                        + ", ExeF_No as 'رقم الملف'"
 //                        + ", ExeF_BeginDate as 'تاريخ البداية'"
 //                        + ", ExeF_CurrentDate as 'التاريخ الحالي'"
 //                        + ", ExeF_NumOfMonths as 'عدد الأشهر'"
 //                        + ", DATE_ADD(ExeF_CurrentDate, INTERVAL ExeF_NumOfMonths Month) as 'تاريخ الاستحقاق'"
 //                        + ", ExeF_ImpoundDate as 'تاريخ الحجز'"
 //                        + ", ExeF_ImpoundType as 'نوع الحجز' "
 //                        + ", ExeF_ID as 'ID'" + from;

 //           var i_cond = " where Current_State = 1 ";
 //           Check_Conditions(i_cond);

 //           query += condition;

 //           Program.buildConnection();
 //           MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
 //           sc.ExecuteNonQuery();
 //           MySqlDataAdapter da = new MySqlDataAdapter(sc);
 //           DataSet ds = new DataSet();
 //           da.Fill(ds);
 //           Program.MyConn.Close();

 //           MP_dataGridView.DataSource = null;
 //           MP_dataGridView.Columns.Clear();
 //           MP_dataGridView.Rows.Clear();
 //           MP_dataGridView.ColumnHeadersVisible = false;
 //           MP_dataGridView.RowHeadersVisible = false;

 //           bs.DataSource = ds.Tables[0];
 //           MP_dataGridView.DataSource = bs;

 //           MP_dataGridView.Columns["تاريخ البداية"].DefaultCellStyle.Format = "dd/MM/yyyy";
 //           MP_dataGridView.Columns["تاريخ الاستحقاق"].DefaultCellStyle.Format = "dd/MM/yyyy";
 //           MP_dataGridView.Columns["تاريخ الحجز"].DefaultCellStyle.Format = "dd/MM/yyyy";
 //           MP_dataGridView.Columns["التاريخ الحالي"].DefaultCellStyle.Format = "dd/MM/yyyy";

 //           var dgc1 = MP_dataGridView.Columns["ID"];
 //           dgc1.Visible = false;


 //           Program.buildConnection();
 //           //count rows
 //           var sel = "select count(DISTINCT MP_ID) " + from + condition;
 //           MySS.sc = new MySqlCommand(sel, Program.MyConn);
 //           decimal count = Convert.ToDecimal(MySS.sc.ExecuteScalar());
 //           Count_label.Text = "All:" + count.ToString("#,##0");
 //           Program.MyConn.Close();

 //           MP_dataGridView.Columns["رقم المشروع"].Width
 //               = MP_dataGridView.Columns["نوع الحجز"].Width
 //               = MP_dataGridView.Columns["رقم الملف"].Width = 80;

 //           MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["تاريخ البداية"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["التاريخ الحالي"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["تاريخ الاستحقاق"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["رقم الملف"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["عدد الأشهر"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["تاريخ الحجز"].DefaultCellStyle.Alignment
 //               = MP_dataGridView.Columns["نوع الحجز"].DefaultCellStyle.Alignment
 //               = DataGridViewContentAlignment.MiddleCenter;

 //           MP_dataGridView.ColumnHeadersVisible = true;
 //           MP_dataGridView.RowHeadersVisible = true;
 //       }

        private void bind_Visits()
        {
            try
            {
                var from =
                    @" from `microproject` MP 
 left OUTER join `person_microproject` PMP  on PMP.MicroProject_ID = MP.MP_ID
 left OUTER join `person` P on P.P_ID = PMP.Person_ID 
 left OUTER join `category` C on MP.Category_ID = C.ID 
 LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 LEFT OUTER join `career` on MP.Career_ID = career.ID

 LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT OUTER JOIN `state` on state.ID = mps.State_ID

 Left OUTER join `donor` on donor.ID = MP.MP_Donor
 LEFT OUTER join `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID
 LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID 
";

                string query = @"SELECT  MP.MP_ID as 'رقم المشروع'
, Group_Concat(CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName)) as 'المستفيد'
, P_FullName_en as 'Beneficiary Name'
, career.Name as 'المهنة' 
, career.Name_en as 'Career'  

, C.Name 'Category' 
, sub.Name as 'Sub Category' 

, (select Date from visit where visit.MicroProject_ID = PMP.MicroProject_ID and Kind like '%op' " + visitDate_condition + " limit 1) as 'Opening'"
+ ", (select Date from mevisit where mevisit.Person_ID = PMP.Person_ID and Number like '1' " + mevisitDate_condition + " limit 1) as 'M1'"
+ ", (select Date from mevisit where mevisit.Person_ID = PMP.Person_ID and Number like '2' " + mevisitDate_condition + " limit 1) as 'M2'"
+ ", (select Date from mevisit where mevisit.Person_ID = PMP.Person_ID and Number like '3' " + mevisitDate_condition + " limit 1) as 'M3'"
+ ", (select Date from mevisit where mevisit.Person_ID = PMP.Person_ID and Number like '4' " + mevisitDate_condition + " limit 1) as 'M4'"
+ ", (select Date from mevisit where mevisit.Person_ID = PMP.Person_ID and Number like '5' " + mevisitDate_condition + " limit 1) as 'M5'"
+ ", (select Date from visit where visit.MicroProject_ID = PMP.MicroProject_ID and Kind like '%cl' " + visitDate_condition + " limit 1) as 'Closing' "
+ from;

                string visit_query = @"SELECT  
      IFNULL(COUNT(case when visit.`Kind` like '%op' then 1 end ),0) as 'sum op' 
    , IFNULL(COUNT(case when visit.`Kind` like '%cl' then 1 end),0) as 'sum cl' 
    , IFNULL(COUNT(visit.ID),0) as 'total visit' 
   
    from visit
 LEFT OUTER join `microproject` MP on visit.MicroProject_ID = MP.MP_ID
 LEFT OUTER join `category` C on MP.Category_ID = C.ID 
 LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 LEFT OUTER join `career` on MP.Career_ID = career.ID
 LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT OUTER JOIN `state` on state.ID = mps.State_ID
 LEFT OUTER join `donor` on donor.ID = MP.MP_Donor 
 LEFT OUTER join `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
 LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID 
";

                string mevisit_query = @" SELECT
      IFNULL(COUNT(CASE WHEN `Number`= '1' THEN 1 end), 0) as 'sum m1'
    , IFNULL(COUNT(CASE WHEN `Number`= '2' THEN 1 end), 0) as 'sum m2'
    , IFNULL(COUNT(CASE WHEN `Number`= '3' THEN 1 end), 0) as 'sum m3'
    , IFNULL(COUNT(CASE WHEN `Number`= '4' THEN 1 end), 0) as 'sum m4'
    , IFNULL(COUNT(CASE WHEN `Number`= '5' THEN 1 end), 0) as 'sum m5'
    , IFNULL(COUNT(mevisit.ID), 0) as 'total mevisit' 

    from `microproject` MP
 LEFT OUTER join `person_microproject` PMP on MP.MP_ID = PMP.MicroProject_ID 
 LEFT OUTER join `mevisit` on PMP.Person_ID = mevisit.Person_ID 
 LEFT OUTER join `category` C on MP.Category_ID = C.ID 
 LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 LEFT OUTER join `career` on MP.Career_ID = career.ID
 
 LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT OUTER JOIN `state` on state.ID = mps.State_ID

 LEFT OUTER join `donor` on donor.ID = MP.MP_Donor  
 LEFT OUTER join `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
 LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID  ";

                var i_cond = " where Current_State = 1  ";
                Check_Conditions(i_cond);



                query += condition + " Group By MP_ID; ";
                visit_query += condition + visitDate_condition + ";";
                mevisit_query += condition + mevisitDate_condition + " and PMP.PersonType like 'مستفيد' ;";

                string all_query = query + visit_query + mevisit_query;

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(all_query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Program.MyConn.Close();

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                MP_dataGridView.Columns["Opening"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["M1"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["M2"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["M3"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["M4"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["M5"].DefaultCellStyle.Format =
                    MP_dataGridView.Columns["Closing"].DefaultCellStyle.Format = "dd/MM/yyyy";

                //count rows
                Program.buildConnection();
                var sel = "select count(DISTINCT MP_ID) " + from + condition;
                MySqlCommand sc1 = new MySqlCommand(sel, Program.MyConn);
                decimal count = Convert.ToDecimal(sc1.ExecuteScalar());
                Count_label.Text = "All:" + count.ToString("#,##0");
                Program.MyConn.Close();

                MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["Opening"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["M1"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["M2"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["M3"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["M4"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["M5"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["Closing"].DefaultCellStyle.Alignment
                        = DataGridViewContentAlignment.MiddleCenter;

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;

                sumOp_label.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["sum op"]).ToString("#,##0");
                sumCl_label.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["sum cl"]).ToString("#,##0");
                sumM1_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m1"]).ToString("#,##0");
                sumM2_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m2"]).ToString("#,##0");
                sumM3_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m3"]).ToString("#,##0");
                sumM4_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m4"]).ToString("#,##0");
                sumM5_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m5"]).ToString("#,##0");

                decimal total = Convert.ToDecimal(ds.Tables[1].Rows[0]["total visit"]) +
                    Convert.ToDecimal(ds.Tables[2].Rows[0]["total mevisit"]);
                sumAll_label.Text = total.ToString("#,##0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_NewVisits()
        {
            try
            {
                var from = @" FROM mevisit
 left join `person` P on P.P_ID = mevisit.Person_ID 
 left join `person_microproject` PMP on PMP.Person_ID = P.P_ID  
 left join `microproject` MP on MP.MP_ID = PMP.MicroProject_ID 
 left join `category` C on MP.Category_ID = C.ID 
 LEFT join `subcategory` sub on MP.SubCategory_ID = sub.ID  
 LEFT join `career` on MP.Career_ID = career.ID
 LEFT JOIN `maritalstatus` on P.P_MaritalStatus_ID = maritalstatus.ID 
 
 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT JOIN `state` on state.ID = mps.State_ID

 Left join `donor` on donor.ID = MP.MP_Donor
 Left join `loan` L on L.MicroProject_ID = MP.MP_ID
 Left join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
 LEFT JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID  ";

                string query = @"SELECT mevisit.`ID` as 'V_ID'
, MP.MP_ID as 'رقم المشروع'
, CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName) as 'المستفيد'
, P_FullName_en as 'Beneficiary Name'
, career.Name as 'المهنة' 
, career.Name_en as 'Career'  
   
, mevisit.`Date` as 'تاريخ الزيارة'
, `Number` as 'رقم الزيارة'
, (case `Kind` WHEN 'v' then 'مركبات' else 'مشاريع أخرى' End) as 'نوع الزيارة'
, `Profit` as 'الربح الصافي التقريبي' 
, `Result` as 'النتيجة'
, (select concat(meresultranges.Evaluation,'/',meresultranges.Evaluation_en) 
 from meresultranges where Result >= Min and  Result <= Max limit 1) as 'التقييم' 

, (select concat(user.UserName , ':' , mevisit_user.Mark) FROM mevisit_user 
 LEFT JOIN user on mevisit_user.User_ID = user.UserID 
 WHERE mevisit_user.Visit_ID = mevisit.ID 
 ORDER by mevisit_user.ID ASC LIMIT 1) as 'رأي الزائر1'
, (select concat(user.UserName , ':' , mevisit_user.Mark) FROM mevisit_user 
 LEFT JOIN user on mevisit_user.User_ID = user.UserID 
 WHERE mevisit_user.Visit_ID = mevisit.ID 
 ORDER by mevisit_user.ID DESC LIMIT 1) as 'رأي الزائر2'

, donor.Name as 'الجهة الممولة' 

, C.Name as 'Category'
, sub.Name as 'Sub Category'
, career.Name as 'Career'
 
, MP.`Category_ID` as 'Category_ID'
, mevisit.Person_ID as 'رقم المستفيد'";

                var i_cond = " where Current_State = 1 ";
                Check_Conditions(i_cond);

                string orderby = " Order By MP.MP_ID asc ";

                query += from
                      + condition + mevisitDate_condition + lastvisit_condition
                      + orderby;

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);
                Program.MyConn.Close();

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                MP_dataGridView.Columns["تاريخ الزيارة"].DefaultCellStyle.Format = "dd/MM/yyyy";
                MP_dataGridView.Columns["الربح الصافي التقريبي"].DefaultCellStyle.Format = "#,##0";

                var dgC1 = MP_dataGridView.Columns["V_ID"];
                dgC1.Visible = false;
                var dgC2 = MP_dataGridView.Columns["رقم المستفيد"];
                dgC2.Visible = false;
                var dgC3 = MP_dataGridView.Columns["Category_ID"];
                dgC3.Visible = false;

                //count rows
                string sel = "select count(*) " + from + condition + mevisitDate_condition;
                string outer_sel = "";
                //if the query has a count function so we need an external subQuery
                if (lastvisit_condition != "")
                {
                    outer_sel = " SELECT COUNT(*) from ( " + sel + lastvisit_condition + " ) AS subQuery; ";
                    sel = outer_sel;
                }
                Program.buildConnection();
                MySS.sc = new MySqlCommand(sel, Program.MyConn);
                decimal count = Convert.ToDecimal(MySS.sc.ExecuteScalar());
                Count_label.Text = "All:" + count.ToString("#,##0");
                Program.MyConn.Close();

                MP_dataGridView.Columns["رقم المشروع"].Width = 80;

                MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الربح الصافي التقريبي"].DefaultCellStyle.Alignment
                        = MP_dataGridView.Columns["تاريخ الزيارة"].DefaultCellStyle.Alignment =
                            MP_dataGridView.Columns["النتيجة"].DefaultCellStyle.Alignment
                                = MP_dataGridView.Columns["رقم الزيارة"].DefaultCellStyle.Alignment =
                                    MP_dataGridView.Columns["نوع الزيارة"].DefaultCellStyle.Alignment
                                        = DataGridViewContentAlignment.MiddleCenter;


                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_CheckList()
        {
            // , MP_ParishNotes as 'رأي الطائفة' 
            //, (case ch_HouseTechCondition when 0 then 'دون' when 1 then 'عادي' when 2 then 'جيد' else 'N/A' End) as 'الحالة الفنية للمنزل'
            //, (case ch_ProjectExperience when 0 then 'لا يوجد' when 1 then 'قليلة' when 2 then 'وسط' when 3 then 'ممتازة' else 'N/A' End) as 'الخبرة في المشروع'
            //, (case ch_ExpectedProjectSuccess when 0 then 'نعم' when 1 then 'لا' else 'N/A' End) as 'تعديل على الميزانية'
            //, (case ch_WorkAbility when 0 then 'غير قادر' when 1 then 'قادر' else 'N/A' End) as 'القدرة على العمل'

            try
            {
                var from = @"FROM `microproject` MP  
  left outer join `person_microproject` PMP on MP.MP_ID = PMP.MicroProject_ID 
  left outer join `person` P on PMP.Person_ID = P.P_ID
  left outer join `category` C on MP.Category_ID = C.ID 
  LEFT OUTER JOIN `subcategory` sub on MP.SubCategory_ID = sub.ID  
  LEFT OUTER JOIN `career` on MP.Career_ID = career.ID   
  LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
  LEFT OUTER JOIN `state` on state.ID = mps.State_ID ";

                string query = @"SELECT MP_ID as 'رقم المشروع'
 , GROUP_CONCAT(CONCAT(P_FirstName, ' ', P_LastName)) as 'المستفيد' 
 , P.P_FatherName as 'اسم الأب' 
 , P_FullName_en as 'Beneficiary Name'
 , career.Name as 'المهنة' 
 , career.Name_en as 'Career'  

 , C.Name as 'Category'
 , sub.Name as 'Sub Category' 
 
 ,(SELECT COUNT(ID) 
 FROM `microproject_checklist` 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like 'k') as 'الأشخاص المفتاحيين (عدد)'
 , ( Select GROUP_CONCAT( CONCAT_WS(':'
    , checklist_member.Name  
    , microproject_checklist.NameNotes  
    , microproject_checklist.RoleNotes  
    , microproject_checklist.Notes       
        )  SEPARATOR ' / ' ) 
 FROM `microproject_checklist` 
 left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID 
 AND Location like 'k') as 'الأشخاص المفتاحيين'

 , MP_KeyPersonNotes as 'آراء أخرى' 

 , MP_TeamDate as 'تاريخ اجتماع الفريق'
 ,(SELECT COUNT(ID) 
 FROM `microproject_checklist` 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like 't') as 'الفريق (عدد)'
 ,  ( Select GROUP_CONCAT( CONCAT_WS(':',
         checklist_member.Name 
         , (CASE microproject_checklist.Opinion WHEN 0 THEN 'ضد' WHEN 1 THEN 'مع' ELSE 'N/A' END)
         , microproject_checklist.Notes
    )  SEPARATOR ' / ' ) 
 FROM `microproject_checklist` 
 left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID 
 AND Location like 't') as 'الفريق'
 
 , (case MP_Visited when 1 then 'مزار' else 'لم تتم بعد' end) as 'الزيارة الأولية' 
 , MP_VisitTime as 'توقيت الزيارة الأولية'
 , MP_VisitDate as 'تاريخ الزيارة الأولية'
   
 , MP_VisitNotes as 'ملاحظات الزيارة الأولية'
 
 ,(SELECT COUNT(ID) 
FROM `microproject_checklist` 
WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like 'v') as 'الزائرون (عدد)'

 , ( Select GROUP_CONCAT( CONCAT_WS(':',
         checklist_member.Name 
        , microproject_checklist.Notes
        )  SEPARATOR ' / ' ) 
 FROM `microproject_checklist` 
 left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID 
 AND Location like 'v') as 'الزائرون'


, MP_CommityDate as 'تاريخ اجتماع اللجنة' 

,(SELECT COUNT(ID) 
FROM `microproject_checklist` 
WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like 'c') as 'اللجنة (عدد)'

, ( Select GROUP_CONCAT( CONCAT_WS(':',
         checklist_member.Name 
         , (CASE microproject_checklist.Opinion WHEN 0 THEN 'ضد' WHEN 1 THEN 'مع' ELSE 'N/A' END)
         , microproject_checklist.Notes
        )  SEPARATOR ' / ' ) 
 FROM `microproject_checklist` 
 left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
 WHERE microproject_checklist.MicroProject_ID = MP.MP_ID 
 AND Location like 'c') as 'اللجنة' ";

                var i_cond = " where Current_State = 1 ";
                Check_Conditions(i_cond);

                query += from + condition + " Group By MP_ID ";

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                //////////////////////////////////////////////////////////////////
                var s = new StringBuilder();
                s.Append("select MP_ID from microproject where MP_ID in ");
                s.Append("(");
                s.Append("select PMP.MicroProject_ID as 'MicroProject_ID'" + from + condition);
                s.Append(")");
                //count rows
                var sel = "select count(DISTINCT MP_ID) " + from + condition;
                sc = new MySqlCommand(sel, Program.MyConn);
                decimal count = Convert.ToDecimal(sc.ExecuteScalar());
                Program.MyConn.Close();
                Count_label.Text = "All:" + count.ToString("#,##0");
                //////////////////////////////////////////////////////////////////

                MP_dataGridView.Columns["رقم المشروع"].Width = 80;

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_SpecialCases()
        {
            try
            {
                var from = @"FROM specialcase sc
  left JOIN `microproject` MP on MP.MP_ID = sc.MicroProject_ID 
  left JOIN `user` on user.UserID = sc.User_ID
  left JOIN `specialcasetype` on specialcasetype.ID = sc.Type_ID 
  left JOIN `person_microproject` PMP on MP.MP_ID = PMP.MicroProject_ID 
  left JOIN `person` P on PMP.Person_ID = P.P_ID 
       
  left JOIN `category` C on MP.Category_ID = C.ID 
  LEFT JOIN `subcategory` sub on MP.SubCategory_ID = sub.ID  
  LEFT JOIN `career` on MP.Career_ID = career.ID   
  LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
  LEFT JOIN `state` on state.ID = mps.State_ID
       
  Left JOIN `donor` on donor.ID = MP.MP_Donor
  LEFT JOIN `loan` L on L.MicroProject_ID = MP.MP_ID
  LEFT JOIN `donorgroup` on donorgroup.ID = MP.DonorGroup_ID
  LEFT JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
  LEFT JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
  LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID 
";

                string query = @"SELECT MP_ID as 'رقم المشروع'
 , CONCAT(P_FirstName, ' ', P_LastName) as 'المستفيد' 
 , P.P_FatherName as 'اسم الأب' 
 , P_FullName_en as 'Beneficiary Name'
 , career.Name as 'المهنة' 
 , career.Name_en as 'Career'  

 , C.Name as 'Category'
 , sub.Name as 'Sub Category' 

 , sc.`Date` as 'تاريخ المراجعة'
 , user.UserName as 'مسجّل المراجعة'
 , specialcasetype.Name 'نوع المراجعة'
 , sc.`Reason` as 'السبب'
 , sc.`Description` as 'الوصف'
 , sc.`Action` as 'الإجراء' 
 , sc.ID as 'ID' ";

                var i_cond = " where Current_State = 1  ";
                Check_Conditions(i_cond);

                query += from + condition;
                query += " order by sc.Date Desc; ";

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                ////////////////////////////////////////////////////////////////////
                //var s = new StringBuilder();
                //s.Append("select MP_ID from microproject where MP_ID in ");
                //s.Append("(");
                //s.Append("select PMP.MicroProject_ID as 'MicroProject_ID'" + from + condition);
                //s.Append(")");
                ////count rows
                //var sel = "select count(DISTINCT MP_ID) " + from + condition;
                //sc = new MySqlCommand(sel, Program.MyConn);
                //decimal count = Convert.ToDecimal(sc.ExecuteScalar());
                //Program.MyConn.Close();
                //Count_label.Text = "All:" + count.ToString("#,##0");
                //////////////////////////////////////////////////////////////////// 
                //MP_dataGridView.Columns["رقم المشروع"].Width = 80; 

                //count rows
                Count_label.Text = "All:" + ds.Tables[0].Rows.Count.ToString("#,##0");

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_AidSources()
        {
            try
            {
                var from = @"FROM person_aidsource 
          left JOIN `person_microproject` PMP on person_aidsource.Person_ID = PMP.Person_ID and person_aidsource.MicroProject_ID = PMP.MicroProject_ID
          left JOIN `person` P on PMP.Person_ID = P.P_ID 
          left JOIN `microproject` MP on MP.MP_ID = PMP.MicroProject_ID  
          left JOIN `aidsource` on aidsource.ID = person_aidsource.AidSource_ID 
 
          left JOIN `category` C on MP.Category_ID = C.ID 
          LEFT JOIN `subcategory` sub on MP.SubCategory_ID = sub.ID  
          LEFT JOIN `career` on MP.Career_ID = career.ID   
          LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
          LEFT JOIN `state` on state.ID = mps.State_ID
       
          Left JOIN `donor` on donor.ID = MP.MP_Donor
          LEFT JOIN `loan` L on L.MicroProject_ID = MP.MP_ID
          LEFT JOIN `donorgroup` on donorgroup.ID = MP.DonorGroup_ID
          LEFT JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
          LEFT JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
          LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID 
        ";

                string query = @"SELECT MP_ID as 'رقم المشروع'
         , CONCAT(P_FirstName, ' ', P_LastName) as 'المستفيد' 
         , P.P_FatherName as 'اسم الأب'  
         , P_FullName_en as 'Beneficiary Name'
        
         , career.Name as 'المهنة' 
         , career.Name_en as 'Project Name'  

         , C.Name as 'Category'
         , sub.Name as 'Sub Category' 

         , aidsource.Name as 'الجهة الممولة'
         , person_aidsource.`DateYear` as 'سنة التمويل' 
         , person_aidsource.`Notes` as 'المواد' 

         , person_aidsource.Person_ID as 'رقم المستفيد'
         , person_aidsource.ID as 'ID' ";

                var i_cond = " where 1 ";
                Check_Conditions(i_cond);

                query += from + condition;
                query += " order by person_aidsource.DateYear Desc; ";

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                var dgc1 = MP_dataGridView.Columns["ID"];
                dgc1.Visible = false;

                var dgc2 = MP_dataGridView.Columns["رقم المستفيد"];
                dgc2.Visible = false;

                //count rows
                Count_label.Text = "All:" + ds.Tables[0].Rows.Count.ToString("#,##0");

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Bind_All_ComboBoxes()
        {
            user_mode = false;
            string query = "";

            query += @"SELECT ID, Name from `category` ORDER BY Name ASC;"; //0
            query += @"SELECT ID, Name, Category_ID ,(SELECT Name from `category` where ID = Category_ID) as Category_Name 
                    from `subcategory` ORDER BY Name ASC;"; //1
            query += @"SELECT ID, Name, SubCategory_ID ,(SELECT Name from `subcategory` where ID = SubCategory_ID) as SubCategory_Name 
                    from `career` ORDER BY Name ASC;"; //2 

            query += @"SELECT ID, Name from `donor` ORDER BY Name ASC;"; //3
            query += @"SELECT ID, Name, Donor_ID ,(SELECT Name from `donor` where ID = Donor_ID) as Donor_Name 
                    from `donorgroup` ORDER BY Name ASC;"; //4

            query += @"SELECT ID, Name from `fundtype` ORDER BY ID ASC;"; //5
            query += @"SELECT ID, Name from `microprojecttype` ORDER BY ID ASC;"; //6
            query += @"SELECT ID, Name, Type_ID ,(SELECT Name from `microprojecttype` where ID = Type_ID) as Type_Name     
                    from `microprojectsubtype` ORDER BY ID ASC;"; //7

            query += @"SELECT ID, Name from `maritalstatus` order by ID ASC;"; //8
            query += @"SELECT ID, Name, Min, Max from `age` where isActive =1 order by ID ASC; "; //9   
            query += @"SELECT ID, Name_ar as 'Name', Name_en from `state` order by ID ASC; "; //10  
            query += @"SELECT ID, Name_en as 'Name' from `parish` order by Name_en ASC; "; //11  
            query += @"SELECT ID, Name from `street` as h_street order by Name ASC; "; //12 
            query += @"SELECT ID, Name from `street` as w_street order by Name ASC; "; //13 

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            combo_ds = new DataSet();
            da.Fill(combo_ds);
            Program.MyConn.Close();

            //Set_comboBox_datasource(Category_comboBox, combo_ds.Tables[0]);
            //Set_comboBox_datasource(SubCategory_comboBox, combo_ds.Tables[1]);
            //Set_comboBox_datasource(Career_comboBox, combo_ds.Tables[2]);
            //Set_comboBox_datasource(Donor_comboBox, combo_ds.Tables[3]);
            //Set_comboBox_datasource(DonorGroup_comboBox, combo_ds.Tables[4]);
            //Set_comboBox_datasource(FundType_comboBox, combo_ds.Tables[5]);
            //Set_comboBox_datasource(Type_comboBox, combo_ds.Tables[6]);
            //Set_comboBox_datasource(SubType_comboBox, combo_ds.Tables[7]);
            //Set_comboBox_datasource(MaritalStatus_comboBox, combo_ds.Tables[8]);
            //Set_comboBox_datasource(Age_comboBox, combo_ds.Tables[9]);
            //Set_comboBox_datasource(P_Parish_comboBox, combo_ds.Tables[11]);

            Set_checkboxcomboBox_datasource(Category_comboBox, combo_ds.Tables[0]);
            Set_checkboxcomboBox_datasource(SubCategory_comboBox, combo_ds.Tables[1]);
            Set_checkboxcomboBox_datasource(Career_comboBox, combo_ds.Tables[2]);
            Set_checkboxcomboBox_datasource(Donor_comboBox, combo_ds.Tables[3]);
            Set_checkboxcomboBox_datasource(DonorGroup_comboBox, combo_ds.Tables[4]);
            Set_checkboxcomboBox_datasource(FundType_comboBox, combo_ds.Tables[5]);
            Set_checkboxcomboBox_datasource(Type_comboBox, combo_ds.Tables[6]);
            Set_checkboxcomboBox_datasource(SubType_comboBox, combo_ds.Tables[7]);
            Set_checkboxcomboBox_datasource(MaritalStatus_comboBox, combo_ds.Tables[8]);
            Set_checkboxcomboBox_datasource(Age_comboBox, combo_ds.Tables[9]);
            Set_checkboxcomboBox_datasource(MP_Status_comboBox, combo_ds.Tables[10]);
            Set_checkboxcomboBox_datasource(P_Parish_comboBox, combo_ds.Tables[11]);
            Set_checkboxcomboBox_datasource(HomeStreet_comboBox, combo_ds.Tables[12]);
            Set_checkboxcomboBox_datasource(ProjectStreet_comboBox, combo_ds.Tables[13]);

            ApplyDate_comboBox.Items.Clear();
            FundDate_comboBox.Items.Clear();
            for (int i = 2018; i <= DateTime.Today.Year; i++)
            {
                ApplyDate_comboBox.Items.Add(i.ToString());
                FundDate_comboBox.Items.Add(i.ToString());
            }

            ApplyDateTo_DateTimePicker.Value = FundedDateTo_DateTimePicker.Value = DateTime.Today;
            user_mode = true;
        }
        private void Set_comboBox_datasource(ComboBox cBox, DataTable c_dt)
        {
            cBox.DataSource = null;
            cBox.DisplayMember = "Name";
            cBox.ValueMember = "ID";
            cBox.DataSource = c_dt;
            cBox.SelectedIndex = -1;
        }
        private void Set_checkboxcomboBox_datasource(CheckBoxComboBox cBox, DataTable c_dt)
        {
            cBox.DataSource = null;
            cBox.DataSource =
                new ListSelectionWrapper<DataRow>(
                    c_dt.Rows,
                    "Name" // "SomePropertyOrColumnName" will populate the Name on ObjectSelectionWrapper.
                    );
            cBox.DisplayMemberSingleItem = "Name";
            cBox.DisplayMember = "NameConcatenated";
            cBox.ValueMember = "Selected";
        }
        private void SubCategory_bind(string Categories)
        {
            user_mode = false;
            SubCategory_comboBox.DataSource = null;
            //SubCategory_comboBox.DisplayMember = "Name";
            //SubCategory_comboBox.ValueMember = "ID";

            if (Categories != "")
            {
                DataRow[] rows = null;
                string[] categoryArray = Categories.Split(',');
                rows = combo_ds.Tables[1].AsEnumerable()
                    .Where(row => categoryArray.Contains(row.Field<string>("Category_Name"))).ToArray();
                //rows = combo_ds.Tables[1].Select("Category_ID=" + Category_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[1].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(SubCategory_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(SubCategory_comboBox, combo_ds.Tables[1]);

            }
            SubCategory_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void Career_bind(string SubCategories)
        {
            user_mode = false;
            Career_comboBox.DataSource = null;
            //Career_comboBox.DisplayMember = "Name";
            //Career_comboBox.ValueMember = "ID";

            if (SubCategories != "")
            {
                DataRow[] rows = null;
                //rows = combo_ds.Tables[2].Select("SubCategory_ID=" + SubCategory_ID);
                string[] Array = SubCategories.Split(',');
                rows = combo_ds.Tables[2].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("SubCategory_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[2].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(Career_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(Career_comboBox, combo_ds.Tables[2]);

            }
            Career_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void SubType_bind(string Types)
        {
            user_mode = false;
            SubType_comboBox.DataSource = null;

            if (Types != "")
            {
                DataRow[] rows = null;
                string[] Array = Types.Split(',');
                rows = combo_ds.Tables[7].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("Type_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[7].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(SubType_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(SubType_comboBox, combo_ds.Tables[7]);

            }
            SubType_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void DonorGroup_bind(string Donors)
        {
            user_mode = false;
            DonorGroup_comboBox.DataSource = null;

            if (Donors != "")
            {
                DataRow[] rows = null;
                //rows = combo_ds.Tables[4].Select("Donor_ID=" + Donor_ID);
                string[] Array = Donors.Split(',');
                rows = combo_ds.Tables[4].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("Donor_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[4].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(DonorGroup_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(DonorGroup_comboBox, combo_ds.Tables[4]);

            }
            DonorGroup_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        #endregion

        #region search and filter functions 
        private void SearchBy_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Check_Visibility();
                //MP_dataGridView.RowTemplate.Height = 28;

                tick = 0;
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tick == 0)
            {
                user_mode = false;
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;
                bs.Filter = bs.Sort = Original_Filter_String = "";
                bs.DataSource = null;
                user_mode = true;

                // applications //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                {
                    bind_Applications();
                }
                // families //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(1))
                {
                    FamilyMember_bind();
                }
                // Loans //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(2))
                {
                    Loan_bind();
                }
                // Attachments //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(3))
                {
                    bind_Images();
                }
                ////Executive Files //
                //else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
                //{
                //    bind_ExeFile();
                //}
                // Visits //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
                {
                    bind_Visits();
                }
                // New Visit Forms //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(5))
                {
                    bind_NewVisits();
                }
                // Check Lists //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(6))
                {
                    bind_CheckList();
                }
                // Special Cases //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(7))
                {
                    bind_SpecialCases();
                }
                // Aid Sources //
                else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(8))
                {
                    bind_AidSources();
                }

                tick++;
                // Use of the DataGridViewColumnSelector
                cs = new DataGridViewColumnSelector(MP_dataGridView);
                cs.MaxHeight = 100;
                cs.Width = 150;
                restore_visible_columns(SearchBy_comboBox.SelectedIndex);

                SearchToolBar.SetColumns(MP_dataGridView.Columns);
                Bind_Show_Hide_ToolStrip();

                Color_Duplicate_Beneficiaries();

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void Category_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (Category_comboBox.Text != "" && Category_comboBox.Visible)
                        SubCategory_bind(Program.getSelectedItems_CheckboxComboBox(Category_comboBox, false));

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SubCategory_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (SubCategory_comboBox.Text != "" && SubCategory_comboBox.Visible)
                        Career_bind(Program.getSelectedItems_CheckboxComboBox(SubCategory_comboBox, false));

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Donor_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (Donor_comboBox.Text != "" && Donor_comboBox.Visible)
                        DonorGroup_bind(Program.getSelectedItems_CheckboxComboBox(Donor_comboBox, false));

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Type_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (Type_comboBox.Text != "" && Type_comboBox.Visible)
                        SubType_bind(Program.getSelectedItems_CheckboxComboBox(Type_comboBox, false));

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ApplyFilter_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //private void Category_comboBox_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
        //    try
        //    {
        //        if (user_mode)
        //        { 
        //            //SubCategory_bind(Category_comboBox.SelectedValue.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void SubCategory_comboBox_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
        //    try
        //    {
        //        //if (user_mode)
        //        //    Career_bind(SubCategory_comboBox.SelectedValue.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}
        //private void Type_comboBox_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
        //    try
        //    {
        //        if (user_mode)
        //            SubType_bind(Type_comboBox.SelectedValue.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void Donor_comboBox_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
        //    try
        //    {
        //        if (user_mode)
        //           DonorGroup_bind(Donor_comboBox.SelectedValue.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        #endregion

        #region datagridview functions 
        private void MP_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                { //Check if click is on specific column 
                    if (e.ColumnIndex == MP_dataGridView.Columns["StateHistory"].Index)
                    {
                        var MP_ID = Convert.ToInt32(MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"].Value.ToString());
                        //, statereason.Name as 'سبب الحالة'
                        //, `Note` as 'ملاحظة الحالة'
                        string query = @" SELECT `MicroProject_ID` as 'رقم المشروع'
, state.Name_ar as 'الحالة'  
, `Date` as 'تاريخ الحالة' 
 FROM `microproject_state` mps left join state on mps.State_ID = state.ID
 left join statereason on mps.StateReason_ID = statereason.ID
 WHERE MicroProject_ID = " + MP_ID + " Order By Date DESC;";

                        Program.buildConnection();
                        MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                        sc.ExecuteNonQuery();
                        var da = new MySqlDataAdapter(sc);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        Program.MyConn.Close();


                        Form form = new ShowDetails_Form(dt, "حالات المشروع");
                        form.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MP_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (SearchBy_comboBox.SelectedIndex != 0) //just when search applications
                    return;

                if (e.RowIndex == MP_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex != -1)
                    if (MP_dataGridView.Columns.Contains("StateHistory"))
                        if (e.ColumnIndex == MP_dataGridView.Columns["StateHistory"].Index)
                        {
                            Image image = Resources.show_item16;
                            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                            var size = image.Size;
                            var location = new System.Drawing.Point(
                                (e.CellBounds.Width - size.Width) / 2,
                                (e.CellBounds.Height - size.Height) / 2
                                );
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
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = MP_dataGridView.FilterString;
                // bs.Filter = Original_Filter_String; 
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void dataGridView_SortStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.SortEventArgs e)
        {
            try
            {
                bs.Sort = MP_dataGridView.SortString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void MP_dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // Applications //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                    {
                        string ApplicationState = "";
                        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                        if (SelectedDataRow != null)
                        {
                            if (SelectedDataRow["رقم المشروع"] == null ||
                                SelectedDataRow["رقم المشروع"] == DBNull.Value)
                                MicroProject_ID = -1;
                            else MicroProject_ID = int.Parse(SelectedDataRow["رقم المشروع"].ToString());

                            if (SelectedDataRow["رقم المستفيد"] == null ||
                                SelectedDataRow["رقم المستفيد"] == DBNull.Value)
                                Person_ID = -1;
                            else Person_ID = int.Parse(SelectedDataRow["رقم المستفيد"].ToString());

                            if (SelectedDataRow["نوع الاستمارة"] == null ||
                              SelectedDataRow["نوع الاستمارة"] == DBNull.Value)
                                ApplicationState = "";
                            else ApplicationState = SelectedDataRow["نوع الاستمارة"].ToString();
                        }

                        Form Appplication_New_Form = new Application_New_Form(main_form, MicroProject_ID, Person_ID, ApplicationState);
                        main_form.showNewTab(Appplication_New_Form, "Application", 0);

                    }

                // families //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(1))
                {
                    SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                    if (SelectedDataRow != null)
                    {
                        if (SelectedDataRow["رقم العائلة"].ToString() == "" ||
                            SelectedDataRow["رقم العائلة"].ToString() == null || SelectedDataRow["رقم العائلة"] == null)
                            throw new InvalidOperationException("This Person Doesn't belong to a family");
                        Family_ID = int.Parse(SelectedDataRow["رقم العائلة"].ToString());
                        var P_ID = int.Parse(SelectedDataRow["رقم المستفيد"].ToString());

                        DataTable dt111 = s.Select_FamilyMembers(Family_ID.ToString(), P_ID.ToString());

                        using (var ShowDetails_Form = new ShowDetails_Form(dt111, "أفراد العائلة"))
                        {
                            ShowDetails_Form.ShowDialog();
                        }
                    }
                }

                // Attachments //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(3))
                    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                    {
                        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                        if (SelectedDataRow != null)
                        {
                            if (SelectedDataRow["رقم المشروع"] == null ||
                                SelectedDataRow["رقم المشروع"] == DBNull.Value)
                                MicroProject_ID = -1;
                            else MicroProject_ID = int.Parse(SelectedDataRow["رقم المشروع"].ToString());

                            if (SelectedDataRow["ID"] == null || SelectedDataRow["ID"] == DBNull.Value)
                                Image_ID = -1;
                            else Image_ID = int.Parse(SelectedDataRow["ID"].ToString());
                        }

                        Form attachments_Form = new Attachments_Form(MicroProject_ID, Image_ID, main_form);
                        main_form.showNewTab(attachments_Form, "Attachments", 0);
                    }

                //Executive Files
                //if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
                //    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                //    {
                //        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                //        if (SelectedDataRow != null)
                //        {
                //            if (SelectedDataRow["رقم المشروع"] == null ||
                //                SelectedDataRow["رقم المشروع"] == DBNull.Value)
                //                MicroProject_ID = -1;
                //            else MicroProject_ID = int.Parse(SelectedDataRow["رقم المشروع"].ToString());

                //            if (SelectedDataRow["ID"] == null || SelectedDataRow["ID"] == DBNull.Value)
                //                ExeFile_ID = -1;
                //            else ExeFile_ID = int.Parse(SelectedDataRow["ID"].ToString());
                //        }

                //        Form ExecutiveFiles_Form = new ExecutiveFiles_Form(ExeFile_ID, main_form);
                //        main_form.showNewTab(ExecutiveFiles_Form, "Executive File", 0);
                //    }

                // Visits //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(4))
                    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                    {
                        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                        if (SelectedDataRow != null)
                        {
                            if (SelectedDataRow["رقم المشروع"] == null ||
                                SelectedDataRow["رقم المشروع"] == DBNull.Value)
                                MicroProject_ID = -1;
                            else MicroProject_ID = Convert.ToInt32(SelectedDataRow["رقم المشروع"].ToString());


                            var _Form = new ChooseVisitKind_Form(MicroProject_ID, main_form);
                            _Form.ShowDialog();
                        }
                    }

                //New visit forms
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(5))
                {
                    SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                    if (SelectedDataRow != null)
                    {
                        var V_ID = int.Parse(SelectedDataRow["V_ID"].ToString());
                        var Beneficiary_ID = int.Parse(SelectedDataRow["رقم المستفيد"].ToString());
                        var Category_ID = int.Parse(SelectedDataRow["Category_ID"].ToString());
                        var V_Num = SelectedDataRow["رقم الزيارة"].ToString();

                        string V_NAME_TO_SHOW;
                        if (V_Num == "2") V_NAME_TO_SHOW = "Second visit";
                        else if (V_Num == "3*") V_NAME_TO_SHOW = "Third * visit";
                        else if (V_Num == "3") V_NAME_TO_SHOW = "Third visit";
                        else V_NAME_TO_SHOW = "";

                        if (Category_ID == 1 || Category_ID == 2)
                        {
                            Form Visit_V_Form = new V_ME_Taxi_Form(V_ID, main_form);
                            main_form.showNewTab(Visit_V_Form, V_NAME_TO_SHOW, 0);
                        }
                        else
                        {
                            Form Visit_O_Form = new V_ME_Other_Form(V_ID, main_form);
                            main_form.showNewTab(Visit_O_Form, V_NAME_TO_SHOW, 0);
                        }
                    }
                }

                // Checklists //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(6))
                    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                    {
                        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                        if (SelectedDataRow != null)
                        {
                            if (SelectedDataRow["رقم المشروع"] == null ||
                                SelectedDataRow["رقم المشروع"] == DBNull.Value)
                                MicroProject_ID = -1;
                            else MicroProject_ID = int.Parse(SelectedDataRow["رقم المشروع"].ToString());
                        }

                        Form Form = new CheckList_Form(MicroProject_ID, main_form);
                        main_form.showNewTab(Form, "Check List", 0);
                    }

                // SpecialCase //
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(7))
                    if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                    {
                        int SC_ID = -1;
                        SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                        if (SelectedDataRow != null)
                        {
                            if (SelectedDataRow["ID"] == null || SelectedDataRow["ID"] == DBNull.Value)
                                SC_ID = -1;
                            else SC_ID = int.Parse(SelectedDataRow["ID"].ToString());

                        }

                        Form Form = new SpecialCase_Form(SC_ID, main_form);
                        main_form.showNewTab(Form, "SpecialCase", 0);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MP_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    var SelectedRowCount = MP_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    Selected_label.Text = "عدد الأسطر المحددة: " + SelectedRowCount;

                    // Applications //
                    if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                    {
                        if (SelectedRowCount >= 1)
                        {
                            double sum = 0;
                            for (var i = 0; i < SelectedRowCount; i++)
                            {
                                var SelectedDataRow = ((DataRowView)MP_dataGridView.SelectedRows[i].DataBoundItem).Row;
                                sum += double.Parse(SelectedDataRow["عدد الأفراد في المنزل"].ToString());
                            }

                            Sum_FamilyMembers_label.Text = "عدد الأفراد في المنزل: " + sum.ToString("#,##0");
                        }
                    }

                    // Families //
                    if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(1))
                        if (SelectedRowCount >= 1)
                        {
                            PIDs_array = new int[SelectedRowCount];
                            P_Names_array = new string[SelectedRowCount];

                            for (var i = 0; i < SelectedRowCount; i++)
                            {
                                PersonSelectedDataRow = ((DataRowView)MP_dataGridView.SelectedRows[i].DataBoundItem).Row;
                                PIDs_array[i] = int.Parse(PersonSelectedDataRow["رقم الفرد"].ToString());
                                P_Names_array[i] = PersonSelectedDataRow["المستفيد"].ToString();
                            }
                        }

                    // Loans //
                    if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(2))
                    {
                        if (SelectedRowCount >= 1)
                        {
                            IDs_array = new int[SelectedRowCount];

                            for (var i = 0; i < SelectedRowCount; i++)
                            {
                                LoanSelectedDataRow = ((DataRowView)MP_dataGridView.SelectedRows[i].DataBoundItem).Row;
                                IDs_array[i] = int.Parse(LoanSelectedDataRow["ID"].ToString());
                            }

                            var str = "(" + string.Join(", ",
                                Array.ConvertAll(IDs_array, v => v.ToString(CultureInfo.InvariantCulture))) + ")";

                            Calculate_ImportExportMoney(str);
                        }
                        else
                        {
                            Calculate_ImportExportMoney("");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MP_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void SearchToolBar_Search(object sender, Zuby.ADGV.AdvancedDataGridViewSearchToolBarSearchEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(e.ValueToSearch))
                {
                    bs.Filter = Original_Filter_String;
                }
                else
                {
                    string NewFilter = "";

                    if (e.ColumnToSearch != null)
                    {
                        if (e.WholeWord)
                            NewFilter += "convert(`" + e.ColumnToSearch.Name + "`,'System.String') = '" + e.ValueToSearch + "'";
                        else
                            NewFilter += "convert(`" + e.ColumnToSearch.Name + "`,'System.String') like '%" + e.ValueToSearch + "%'";
                    }
                    else
                    {
                        //Colunms.GetRowCount(DataGridViewElementStates.Visible);
                        int VisibleColumnsCount = MP_dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
                        int i = 0;
                        foreach (DataGridViewColumn C in MP_dataGridView.Columns)
                        {
                            if (C.Visible == true)
                            {
                                if (C.Name != "StateHistory")
                                {
                                    if (e.WholeWord)
                                        NewFilter += "convert(`" + C.Name + "`,'System.String') = '" + e.ValueToSearch + "'";
                                    else
                                        NewFilter += "convert(`" + C.Name + "`,'System.String') like '%" + e.ValueToSearch + "%'";

                                    if (i < VisibleColumnsCount - 1)
                                        NewFilter += " or ";
                                }
                                i++;
                            }
                        }
                    }
                    string All_Filters = Original_Filter_String == "" ? NewFilter : Original_Filter_String + " and (" + NewFilter + ")";
                    //remove unnessecary 'or' at the end of the string
                    if (All_Filters.EndsWith("or "))
                        All_Filters = All_Filters.Substring(0, All_Filters.Length - 3);

                    bs.Filter = All_Filters;
                }

                /*
                           bool restartsearch = true;
                           int startColumn = 0;
                           int startRow = 0;
                           if (!e.FromBegin)
                           {
                               bool endcol; bool endrow;

                               endcol = Search_DataGridView.CurrentCell.ColumnIndex + 1 >= Search_DataGridView.ColumnCount;
                               endrow = Search_DataGridView.CurrentCell.RowIndex + 1 >= Search_DataGridView.RowCount;

                               if (endcol && endrow)
                               {
                                   startColumn = Search_DataGridView.CurrentCell.ColumnIndex;
                                   startRow = Search_DataGridView.CurrentCell.RowIndex;
                               }
                               else
                               {
                                   startColumn = endcol ? 0 : Search_DataGridView.CurrentCell.ColumnIndex + 1;
                                   startRow = Search_DataGridView.CurrentCell.RowIndex + (endcol ? 1 : 0);
                               }
                           }
                           DataGridViewCell c = Search_DataGridView.FindCell(
                               e.ValueToSearch,
                               e.ColumnToSearch != null ? e.ColumnToSearch.Name : null,
                               startRow,
                               startColumn,
                               e.WholeWord,
                               e.CaseSensitive);
                           if (c == null && restartsearch)
                           {
                               c = Search_DataGridView.FindCell(
                               e.ValueToSearch,
                               e.ColumnToSearch != null ? e.ColumnToSearch.Name : null,
                               0,
                               0,
                               e.WholeWord,
                              e.CaseSensitive);
                           }

                           if (c != null)
                           {
                               Search_DataGridView.CurrentCell = c;
                               if (e.FromBegin)
                                   SearchToolBar.Items["button_frombegin"].PerformClick();
                           }
                           else
                               MessageBox.Show("لا يوجد نتائج لعرضها"); 
               */
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void bs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                if (MP_dataGridView.DataSource != null && MP_dataGridView.Rows.Count != 0)
                {
                    BindingSource f_bs = (BindingSource)MP_dataGridView.DataSource;
                    DataTable filtered_dt = ((DataTable)f_bs.DataSource).DefaultView.ToTable(true, "رقم المشروع");
                    Count_label.Text = "عدد المشاريع: " + filtered_dt.Rows.Count.ToString("#,##0");
                }

                Color_Duplicate_Beneficiaries();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        private void Color_Duplicate_Beneficiaries()
        {
            
            //color duplicate rows//
            var allrows = MP_dataGridView.Rows.OfType<DataGridViewRow>().Reverse().Skip(1);//ignore the last empty line
            var dupRos = allrows.GroupBy(r => r.Cells["المستفيد"].Value.ToString())
                                .Where(g => g.Count() > 1)
                                .SelectMany(r => r.ToList());
            foreach (var r in dupRos)
            {
                r.Cells["المستفيد"].Style.BackColor = Color.LightCoral;
                if (Settings.Default.theme == "Light") { }
                else r.Cells["المستفيد"].Style.ForeColor = Color.Black;
            }
            
        }

        #region button clicks 
        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.role != 1 && Program.role != 8) throw new InvalidOperationException("You don't have permession for this action.");
                var dialogResult = MessageBox.Show("Are you sure you want to delete ??", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    Program.buildConnection();
                    var str = "(" + string.Join(", ",
                        Array.ConvertAll(PIDs_array, v => v.ToString(CultureInfo.InvariantCulture))) + ")";
                    var name_str = string.Join(", ",
                        Array.ConvertAll(P_Names_array, v => v.ToString(CultureInfo.InvariantCulture)));

                    var query = "DELETE FROM `person` WHERE `P_ID` in" + str;
                    var sc = new MySqlCommand(query, Program.MyConn);
                    sc.ExecuteNonQuery();
                    Program.MyConn.Close();
                    l.Insert_Log("Delete from Persons: " + name_str, "Person", Program.UserName, DateTime.Now);
                    FamilyMember_bind();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportToExcel_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && SearchBy_comboBox.SelectedIndex == 0)//application
            {
                System.Windows.Forms.Button btnSender = (System.Windows.Forms.Button)sender;
                System.Drawing.Point ptLowerLeft = new System.Drawing.Point(0, btnSender.Height);
                ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
                exportExcel_contextMenuStrip.Show(ptLowerLeft);
            }
            else
            {
                ExportToExcel_button_Click(sender, e);
            }
        }

        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
                //if Guest - Out of service - lawful - Media//
                if (Program.role == 4 ||
                    Program.role == 6 ||
                    Program.role == 7 ||
                    Program.role == 9)
                    throw new InvalidOperationException("Sorry ! You Don't have the permission for this action.");

                Thread myTh = new Thread(SaveCallDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void export_applications_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if Guest - Out of service - lawful - Media//
                if (Program.role == 4 ||
                    Program.role == 6 ||
                    Program.role == 7 ||
                    Program.role == 9)
                    throw new InvalidOperationException("Sorry ! You Don't have the permission for this action.");

                Thread myTh = new Thread(SaveCallDialog_2);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveCallDialog()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel File";
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog1.ValidateNames = true;

                var res = saveFileDialog1.ShowDialog();
                if (res == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    string filename = filename = saveFileDialog1.FileName;
                    XLWorkbook wb = new XLWorkbook { RightToLeft = true };
                    DataTable ex_dt = new DataTable();

                    //defaultView gets the visible rows only//
                    if (bs.DataSource == null)
                        ex_dt = ((DataTable)MP_dataGridView.DataSource).DefaultView.ToTable();
                    else ex_dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    //Remove invisible columns//
                    for (var j = 0; j <= MP_dataGridView.ColumnCount - 1; j++)
                    {
                        if (MP_dataGridView.Columns[j].Name == "StateHistory") continue;
                        if (MP_dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(MP_dataGridView.Columns[j].Name.ToString());
                    }
                    wb.Worksheets.Add(ex_dt, "Exported from App");

                    wb.SaveAs(filename);
                    wb.Dispose();

                    Process.Start(filename);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveCallDialog_2()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel File";
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog1.ValidateNames = true;

                var res = saveFileDialog1.ShowDialog();
                if (res == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    string filename = filename = saveFileDialog1.FileName;
                    XLWorkbook wb = new XLWorkbook { RightToLeft = true };

                    //ADD bs.filter to Condition String//
                    //get visible IDs from datagrid
                    string mp_cond = " and PMP_ID in (";
                    for (int i = 0; i < MP_dataGridView.Rows.Count; i++)
                    {
                        if (MP_dataGridView.Rows[i].Visible)
                        {
                            mp_cond += MP_dataGridView.Rows[i].Cells["ID"].Value.ToString();
                            mp_cond += ",";
                        }
                    }
                    mp_cond = mp_cond.Remove(mp_cond.Length - 1, 1);
                    mp_cond += ")";

                    string export_cond = this.condition;
                    if (bs.Filter != "")
                        export_cond += mp_cond;
                     
                    DataTable ex_dt = s.ZeroApplication_Details_Procedure(export_cond);

                    wb.Worksheets.Add(ex_dt, "Exported from App");

                    wb.SaveAs(filename);
                    wb.Dispose();

                    Process.Start(filename);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region mouse hover

        private void ExportToExcel_button_MouseEnter(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_L;
        }

        private void ExportToExcel_button_MouseLeave(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_D;
        }

        private void Refresh_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Refresh_button.BackgroundImage = Resources.Refresh2_L;
            else Refresh_button.BackgroundImage = Resources.Refresh2_D;
        }

        private void Refresh_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Refresh_button.BackgroundImage = Resources.Refresh2_D;
            else Refresh_button.BackgroundImage = Resources.Refresh2_L;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (Hided)
            {
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Show2_D;
                else ShowHide_button.BackgroundImage = Resources.Show2_L;
            }
            else
            {
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Hide2_D;
                else ShowHide_button.BackgroundImage = Resources.Hide2_L;
            }
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (Hided)
            {
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Show2_L;
                else ShowHide_button.BackgroundImage = Resources.Show2_D;
            }
            else
            {
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Hide2_L;
                else ShowHide_button.BackgroundImage = Resources.Hide2_D;
            }
        }

        private void ApplyFilter_button_MouseEnter(object sender, EventArgs e)
        {
            //if (Settings.Default.theme == "Light")
            ApplyFilter_button.BackgroundImage = Resources.Apply0;
            //else ShowHide_button.BackgroundImage = Resources.Show2_L; 
        }

        private void ApplyFilter_button_MouseLeave(object sender, EventArgs e)
        {
            //if (Settings.Default.theme == "Light")
            ApplyFilter_button.BackgroundImage = Resources.Apply1;
            //else ShowHide_button.BackgroundImage = Resources.Show2_D; 
        }

        #endregion

        #region right click menu 
        private void refreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh_button_Click(sender, e);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form Search_Form = new Search_Form(main_form);
                main_form.showNewTab(Search_Form, "Search", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form Statistics_Form = new Statistics_Form(main_form);
                main_form.showNewTab(Statistics_Form, "Statistics", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void taskBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var projectsTasks = new TaskBoard_Form(main_form);
                main_form.showNewTab(projectsTasks, "Task Board", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void showHideFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    show_hide_button_Click(sender, e);
        //}

        private void clearAllFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Visits_comboBox.Text 
            Category_comboBox.Text = SubCategory_comboBox.Text = Career_comboBox.Text
                = Type_comboBox.Text = SubType_comboBox.Text
                = Partnership_comboBox.Text = FundType_comboBox.Text
                = MP_Status_comboBox.Text = Donor_comboBox.Text = DonorGroup_comboBox.Text
                = P_Parish_comboBox.Text = Age_comboBox.Text = Gender_comboBox.Text = MaritalStatus_comboBox.Text
                = HomeStreet_comboBox.Text = ProjectStreet_comboBox.Text
                = ApplyDate_comboBox.Text = FundDate_comboBox.Text = "";

            RequestedFrom_textBox.Text = RequestedTo_textBox.Text
                = FundedFrom_textBox.Text = FundedTo_textBox.Text = "";

            lastvisit_condition =
            ApplyDate_condition = FundedDate_condition
                = RequestedAmount_condition = FundedAmount_condition = "";

            LastVisit_checkBox.Checked =
            ApplyDate_checkBox.Checked = FundedDate_checkBox.Checked = false;

            //SearchBy_textBox.Text = "";
            Refresh_button_Click(sender, e);
        }
        #endregion

        #region show/hide button
        private void Show_Hide_ToolStrip_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                Show_Hide_ToolStrip.ShowDropDown();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void Bind_Show_Hide_ToolStrip()
        {
            ToolStripMenuItem Hide_Columns = Show_Hide_ToolStrip.DropDownItems[0] as ToolStripMenuItem;
            ToolStripMenuItem Show_Columns = Show_Hide_ToolStrip.DropDownItems[1] as ToolStripMenuItem;

            Show_Columns.DropDownItems.Clear();
            Hide_Columns.DropDownItems.Clear();

            foreach (DataGridViewColumn C in MP_dataGridView.Columns)
            {
                //if (!C.Name.Contains("ID"))
                if (C.Visible)
                    Hide_Columns.DropDownItems.Add(C.Name);
                else
                    Show_Columns.DropDownItems.Add(C.Name);
            }
        }
        private void showHideFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            show_hide_button_Click(sender, e);
        }
        private void Show_MenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                MP_dataGridView.Columns[e.ClickedItem.Text].Visible = true;
                Bind_Show_Hide_ToolStrip();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void Hide_MenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                MP_dataGridView.Columns[e.ClickedItem.Text].Visible = false;
                Bind_Show_Hide_ToolStrip();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void showAllToolStripMenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < MP_dataGridView.Columns.Count; i++)
                    MP_dataGridView.Columns[i].Visible = true;
                Bind_Show_Hide_ToolStrip();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void Show_MenuItem_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            try
            {
                if (Show_MenuItem.DropDownItems.Count > 0)
                {
                    if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    {
                        e.Cancel = true;
                    }
                }
                SearchToolBar.SetColumns(MP_dataGridView.Columns);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }

        }
        private void Hide_MenuItem_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            try
            {
                if (Hide_MenuItem.DropDownItems.Count > 0)
                {
                    if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    {
                        e.Cancel = true;
                    }
                }
                SearchToolBar.SetColumns(MP_dataGridView.Columns);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }

        }
        #endregion

        #region save visible arrays for each tab in search 
        private string[] arr_str;
        private void save_visible_columns(int index)
        {
            arr_str = new string[MP_dataGridView.ColumnCount];

            for (var j = 0; j < MP_dataGridView.ColumnCount; j++)
                if (MP_dataGridView.Columns[j].Visible)
                    arr_str[j] = MP_dataGridView.Columns[j].Name;
                else arr_str[j] = "";

            var value_str = string.Join(",", arr_str.Select(i => i.ToString()).ToArray());

            if (index == 0) Settings.Default.Search0_Visible_arr = value_str;
            else if (index == 1) Settings.Default.Search1_Visible_arr = value_str;
            else if (index == 2) Settings.Default.Search2_Visible_arr = value_str;
            else if (index == 3) Settings.Default.Search3_Visible_arr = value_str;
            else if (index == 4) Settings.Default.Search4_Visible_arr = value_str;
            else if (index == 5) Settings.Default.Search5_Visible_arr = value_str;
            else if (index == 6) Settings.Default.Search6_Visible_arr = value_str;
            else if (index == 7) Settings.Default.Search7_Visible_arr = value_str;
            else if (index == 8) Settings.Default.Search8_Visible_arr = value_str;
            //else if (index == 9) Settings.Default.Search9_Visible_arr = value_str;

            Settings.Default.Save();
        }
        private void restore_visible_columns(int index)
        {
            try
            {
                arr_str = null;

                if (index == 0)
                    arr_str = (Settings.Default.Search0_Visible_arr == "" ? null : Settings.Default.Search0_Visible_arr.Split(','));
                else if (index == 1)
                    arr_str = (Settings.Default.Search1_Visible_arr == "" ? null : Settings.Default.Search1_Visible_arr.Split(','));
                else if (index == 2)
                    arr_str = (Settings.Default.Search2_Visible_arr == "" ? null : Settings.Default.Search2_Visible_arr.Split(','));
                else if (index == 3)
                    arr_str = (Settings.Default.Search3_Visible_arr == "" ? null : Settings.Default.Search3_Visible_arr.Split(','));
                else if (index == 4)
                    arr_str = (Settings.Default.Search4_Visible_arr == "" ? null : Settings.Default.Search4_Visible_arr.Split(','));
                else if (index == 5)
                    arr_str = (Settings.Default.Search5_Visible_arr == "" ? null : Settings.Default.Search5_Visible_arr.Split(','));
                else if (index == 6)
                    arr_str = (Settings.Default.Search6_Visible_arr == "" ? null : Settings.Default.Search6_Visible_arr.Split(','));
                else if (index == 7)
                    arr_str = (Settings.Default.Search7_Visible_arr == "" ? null : Settings.Default.Search7_Visible_arr.Split(','));
                else if (index == 8)
                    arr_str = (Settings.Default.Search8_Visible_arr == "" ? null : Settings.Default.Search8_Visible_arr.Split(','));
                //else if (index == 9)
                //    arr_str = (Settings.Default.Search9_Visible_arr == "" ? null : Settings.Default.Search9_Visible_arr.Split(','));

                if (arr_str == null) //using LINQ
                    MP_dataGridView.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = true);
                else
                    for (var i = 0; i < MP_dataGridView.ColumnCount; i++)
                    {
                        if (arr_str[i] != "") MP_dataGridView.Columns[i].Visible = true;
                        else MP_dataGridView.Columns[i].Visible = false;
                    }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Index was outside the bounds of the array")
                    || ex.Message.Contains("Input string was not in a correct format"))
                { }
                else MessageBox.Show(ex.Message);

                save_visible_columns(index);
            }
        }
        #endregion

    }
}