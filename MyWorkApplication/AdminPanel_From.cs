using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using PresentationControls;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace MyWorkApplication
{
    public partial class AdminPanel_From : Form
    {
        private int[] arr;

        private string[] coloursArr =
            {"Red", "Green", "Black", "White", "Orange", "Yellow", "Blue", "Maroon", "Pink", "Purple"};

        private DataGridViewColumnSelector cs;
        private ListSortDirection direction;

        private string direction_1;
        private bool Hided;
        private Log l;
        private readonly MainForm main_form;
        private int MicroProject_ID;
        private DataGridViewColumn oldColumn, newColumn;
        private int Person_ID;
        private int PW;
        private DataRow SelectedDataRow;
        private TasksOfProjects TasksOfProjects;
        private string State = "";
        private Street st;
        private SubCategory sub;
        private DataSet ds;
        private int tick;
        private string username;
        private bool user_mode;
        private int total_bind_count;
        private string ApplyDate_condition = "";
        private string FundedAmount_condition = "";
        private string FundedDate_condition = "";
        private string RequestedAmount_condition = "";
        private string Original_Filter_String = "";
        BindingSource bs = new BindingSource();
        string condition = "";
        string ApplicationState = "";

        public AdminPanel_From(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        public AdminPanel_From(string username, MainForm main_form)
        {
            InitializeComponent();
            this.username = username;
            this.main_form = main_form;
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            try
            {
                l = new Log();
                st = new Street(); sub = new SubCategory();
                user_mode = false;

                PW = Search_panel.Height;
                Hided = true;

                Search_panel.Height = 0;
                MP_dataGridView.DoubleBuffered(true);

                bs.ListChanged += bs_ListChanged;

                //to scroll the show/hide lists 
                Hide_MenuItem.DropDown.Closing += Hide_MenuItem_Closing;
                Show_MenuItem.DropDown.Closing += Show_MenuItem_Closing;

                Bind_All_ComboBoxes();
                

                // Use of the DataGridViewColumnSelector
                cs = new DataGridViewColumnSelector(MP_dataGridView);
                cs.MaxHeight = 100;
                cs.Width = 110;

                Check_Theme();
                timer1.Start(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //    ,string FundType, string Type, string SubType
        //    , string State, string Donor,string DonorGroup
        //    , string Category,string SubCategory, string Career
        //    , string ApplyYear, string FundYear, string reason
        //    , string Parish, string Age, string Sex, string MaritalStatus, string Partners

        #region bind queries
        private void bind(string value)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor; 
                bs.Filter = bs.Sort = Original_Filter_String = "";
                bs.DataSource = null;
                //LEFT JOIN `street` on street.ID = P.Street_ID

                var from =
                    @" from `microproject` MP 
 LEFT JOIN `person_microproject` PMP on PMP.MicroProject_ID = MP.MP_ID 
 LEFT JOIN `person` P on P.P_ID = PMP.Person_ID
 Left join `maritalstatus` on maritalstatus.ID = P.P_MaritalStatus_ID
 left join `parish` on P.P_Parish_ID = parish.ID 
 LEFT JOIN `loan` l on l.MicroProject_ID = MP.MP_ID
 LEFT JOIN `category` C on MP.Category_ID = C.ID
 LEFT JOIN `subcategory` Sub on MP.SubCategory_ID = Sub.ID
 LEFT JOIN `career` on MP.Career_ID = career.ID
 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT JOIN `state` on state.ID = mps.State_ID
 LEFT JOIN `statereason` on statereason.ID = mps.StateReason_ID
 
 LEFT JOIN `street` w_street on MP.MP_Street_ID = w_street.ID 
 LEFT JOIN `street` h_street on P.Street_ID = h_street.ID 
 
 LEFT JOIN `priest` priest1 on priest1.Priest_ID = P.P_Priest_ID
 LEFT JOIN `priest` priest2 on priest2.Priest_ID = P.P_Priest2_ID

 LEFT JOIN `donor` on donor.ID = MP.MP_Donor 
 LEFT JOIN `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 

 LEFT JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID ";

                var query = @"SELECT MP.MP_ID as 'رقم المشروع'
,CONCAT(P_FirstName,' ', P_LastName) as 'المستفيد'
,P_FatherName as 'اسم الأب'
,P_MotherName as 'اسم الأم'
,( SELECT CONCAT(p.P_FirstName, ' ', p.P_FatherName, ' ', p.P_LastName) 
    From `person` p 
    left join `person_family` pf on p.P_ID = pf.Person_ID 
    left join relation on relation.id = pf.Relation_ID
    WHERE relation.Name like N'زوج/ة' 
    and pf.P_Provider_ID = PMP.Person_ID limit 1) as 'الزوج / الزوجة' 

,P.P_NationalNumber as 'الرقم الوطني'
,P.P_RegistrationPlace as 'القيد'

,( SELECT f.F_Number 
    From `family` f 
    left join `person_family` pf on f.F_ID = pf.Family_ID  
    left join relation on relation.id = pf.Relation_ID
    WHERE relation.Name like N'مستفيد'  
    and pf.P_Provider_ID = PMP.Person_ID limit 1) as 'رقم دفتر العائلة'

,P_DOB as 'المواليد'  
,P.P_Mobile as 'الموبايل' 

,career.Name    as 'المهنة'
,career.Name_en as 'Career'
,MP.MP_Name as 'وصف المشروع'  
,MP.MP_DateOfRequest as 'تاريخ التقديم'
,MP.MP_RequestedAmount as 'المبلغ المطلوب'
,l.Loan_Amount as 'مبلغ التمويل'

,parish.Name_en as 'الطائفة' 
,priest1.Priest_Name as '1الكاهن'
,priest2.Priest_Name as '2الكاهن' 
,h_street.Name as 'منطقة السكن'
,P_HomeAddress as 'العنوان' 

,microprojecttype.Name as 'نوع المشروع'  
,MP.MP_Note as 'ملاحظات المشروع'
,P.P_MaristesCourse as 'دورة تدريبية'  
,state.Name_ar as 'حالة المشروع'
,statereason.Name as 'سبب الحالة'
,mps.Note as 'ملاحظة الحالة' 
,donor.Name as 'الجهة الممولة' 
,(CASE IsContentUpdated WHEN 0 THEN N'لا' ELSE N'نعم' End) as 'تبديل المشروع' 

,MP.ApplicationState as 'نوع الاستمارة' 
,P.P_ID as 'رقم المستفيد' ";
                 
                var orderBy = " order by MP.MP_ID DESC ";
                if (value != "") orderBy = " order by mps.State_ID, PMP.MicroProject_ID DESC ";

                Check_Conditions(" where Current_State = 1 ");
                query += from + condition + orderBy + ";";

                //count rows
                string query2 = "select IFNULL(count(DISTINCT MP_ID),0) "
                    + from + condition + ";";

                string all_query = "";
                all_query += query + query2;

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(all_query, Program.MyConn);
                sc.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                DataSet main_ds = new DataSet();
                da.Fill(main_ds);
                Program.MyConn.Close();
                 
                MP_dataGridView.SuspendLayout();

                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();
                MP_dataGridView.Rows.Clear();
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.RowHeadersVisible = false;

                bs.DataSource = main_ds.Tables[0];
                MP_dataGridView.DataSource = bs;

                MP_dataGridView.ResumeLayout();

                //////calculate sum of projects// 
                total_bind_count = 0; 
                total_bind_count = Convert.ToInt32(main_ds.Tables[1].Rows[0][0].ToString());
                Count_label.Text = "All:" + total_bind_count.ToString("#,##0");

                MP_dataGridView.Columns["المبلغ المطلوب"].DefaultCellStyle.Format = "#,##0";
                MP_dataGridView.Columns["مبلغ التمويل"].DefaultCellStyle.Format = "#,##0";
                MP_dataGridView.Columns["تاريخ التقديم"].DefaultCellStyle.Format = "yyyy/MM/dd";
                 
                MP_dataGridView.Columns["رقم المشروع"].ReadOnly =
                    MP_dataGridView.Columns["المستفيد"].ReadOnly =
                    MP_dataGridView.Columns["اسم الأب"].ReadOnly =
                    MP_dataGridView.Columns["اسم الأم"].ReadOnly =
                    MP_dataGridView.Columns["الزوج / الزوجة"].ReadOnly =
                    MP_dataGridView.Columns["الرقم الوطني"].ReadOnly =
                    MP_dataGridView.Columns["القيد"].ReadOnly =
                    MP_dataGridView.Columns["رقم دفتر العائلة"].ReadOnly =
                    MP_dataGridView.Columns["المواليد"].ReadOnly =
                    MP_dataGridView.Columns["المهنة"].ReadOnly =
                    MP_dataGridView.Columns["Career"].ReadOnly =
                    MP_dataGridView.Columns["تاريخ التقديم"].ReadOnly = 
                    MP_dataGridView.Columns["مبلغ التمويل"].ReadOnly =
                    MP_dataGridView.Columns["الطائفة"].ReadOnly =
                    MP_dataGridView.Columns["1الكاهن"].ReadOnly =
                    MP_dataGridView.Columns["2الكاهن"].ReadOnly =
                    MP_dataGridView.Columns["منطقة السكن"].ReadOnly =
                    MP_dataGridView.Columns["العنوان"].ReadOnly = 
                    MP_dataGridView.Columns["نوع المشروع"].ReadOnly = 
                    MP_dataGridView.Columns["نوع الاستمارة"].ReadOnly = true;

                ConvertCellToComboBoxAndColor();
                 
                addStateButtonToGrid();

                restore_visible_columns();

                MP_dataGridView.Columns["رقم المشروع"].Frozen = true;
                var dgC1 = MP_dataGridView.Columns["رقم المستفيد"];
                dgC1.Visible = false;

                MP_dataGridView.ColumnHeadersVisible = true;
                MP_dataGridView.RowHeadersVisible = true;
                MP_dataGridView.ClearSelection();

                SearchToolBar.SetColumns(MP_dataGridView.Columns);
                Bind_Show_Hide_ToolStrip();

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
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

            query += "SELECT ID, Name                   from `category` ORDER BY Name ASC;"; //0
            query += "SELECT ID, Name, Category_ID      from `subcategory` ORDER BY Name ASC;"; //1
            query += "SELECT ID, Name, SubCategory_ID   from `career` order by Name ASC;"; //2
            query += "SELECT ID, Name                   from `donor` ORDER BY Name ASC;"; //3
            query += "SELECT ID, Name, Donor_ID         from `donorgroup` ORDER BY Name ASC;"; //4
            query += "SELECT ID, Name                   from `fundtype` ORDER BY ID ASC;"; //5
            query += "SELECT ID, Name                   from `microprojecttype` ORDER BY ID ASC;"; //6
            query += "SELECT ID, Name, Type_ID          from `microprojectsubtype` ORDER BY ID ASC;"; //7 
            query += "select ID, Name                   from `maritalstatus` order by ID ASC;"; //8 
            query += "SELECT ID, Name, Min, Max         from `age` where isActive=1 order by ID ASC; "; //9 
            query += "select ID, Name                   from `statereason` ORDER BY Name ASC;"; //10

            query += "select ID, Name_en as 'Name'      from `parish` ORDER BY Name_en ASC;"; //11
            query += "SELECT ID, Name                   from `street` as h_street order by Name ASC; "; //12 
            query += "SELECT ID, Name                   from `street` as w_street order by Name ASC; "; //13 
            query += "select ID, Name_ar ,Name_en       from `state` ORDER BY Name_ar ASC;"; //14 

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();
              
            Set_checkboxcomboBox_datasource(Category_comboBox, ds.Tables[0]);
            Set_checkboxcomboBox_datasource(SubCategory_comboBox, ds.Tables[1]);
            Set_checkboxcomboBox_datasource(Career_comboBox, ds.Tables[2]);
            Set_checkboxcomboBox_datasource(Donor_comboBox, ds.Tables[3]);
            Set_checkboxcomboBox_datasource(DonorGroup_comboBox, ds.Tables[4]);
            Set_checkboxcomboBox_datasource(FundType_comboBox, ds.Tables[5]);
            Set_checkboxcomboBox_datasource(Type_comboBox, ds.Tables[6]);
            Set_checkboxcomboBox_datasource(SubType_comboBox, ds.Tables[7]);
            Set_checkboxcomboBox_datasource(MaritalStatus_comboBox, ds.Tables[8]);
            Set_checkboxcomboBox_datasource(Age_comboBox, ds.Tables[9]);

            Set_checkboxcomboBox_datasource(StateReason_comboBox, ds.Tables[10]);
            Set_checkboxcomboBox_datasource(P_Parish_comboBox, ds.Tables[11]);
            Set_checkboxcomboBox_datasource(HomeStreet_comboBox, ds.Tables[12]);
            Set_checkboxcomboBox_datasource(ProjectStreet_comboBox, ds.Tables[13]);

            ApplyDate_comboBox.Items.Clear();
            FundDate_comboBox.Items.Clear();
            for (int i = 2018; i <= DateTime.Today.Year; i++)
            {
                ApplyDate_comboBox.Items.Add(i.ToString());
                FundDate_comboBox.Items.Add(i.ToString());
            }

            user_mode = true;
        }

        //private void Set_comboBox_datasource(ComboBox cBox, DataTable c_dt)
        //{
        //    cBox.DataSource = null;
        //    cBox.DisplayMember = "Name";
        //    cBox.ValueMember = "ID";
        //    cBox.DataSource = c_dt;
        //    cBox.SelectedIndex = -1;
        //}
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
        private void SubCategory_bind(string Category_ID)
        {
            user_mode = false;
            SubCategory_comboBox.DataSource = null;
            SubCategory_comboBox.DisplayMember = "Name";
            SubCategory_comboBox.ValueMember = "ID";

            if (Category_ID != "")
            {
                DataRow[] rows = null;
                rows = ds.Tables[1].Select("Category_ID=" + Category_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[1].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    SubCategory_comboBox.DataSource = selected_rows_dt;
            }
            else
            {
                SubCategory_comboBox.DataSource = ds.Tables[1];

            }
            SubCategory_comboBox.SelectedIndex = -1;
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
                rows = ds.Tables[2].Select("SubCategory_ID=" + SubCategory_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[2].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Career_comboBox.DataSource = selected_rows_dt;
            }
            else
            {
                Career_comboBox.DataSource = ds.Tables[2];

            }
            Career_comboBox.SelectedIndex = -1;
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
                rows = ds.Tables[7].Select("Type_ID=" + Type_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[7].Copy();
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
                SubType_comboBox.DataSource = ds.Tables[7];

            }
            SubType_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void DonorGroup_bind(string Donor_ID)
        {
            user_mode = false;
            DonorGroup_comboBox.DataSource = null;
            DonorGroup_comboBox.DisplayMember = "Name";
            DonorGroup_comboBox.ValueMember = "ID";

            if (Donor_ID != "")
            {
                DataRow[] rows = null;
                rows = ds.Tables[4].Select("Donor_ID=" + Donor_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = ds.Tables[4].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    DonorGroup_comboBox.DataSource = selected_rows_dt;
                else
                {

                }
            }
            else
            {
                DonorGroup_comboBox.DataSource = ds.Tables[4];

            }
            DonorGroup_comboBox.SelectedIndex = -1;
            user_mode = true;
        }

        #endregion

        #region state buttons
        private void Accepted_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "مقبول";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Financed_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "ممول";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Regected_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "مرفوض";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delayed_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "مؤجل";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnHold_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "تسجيل أولي";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Waiting_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "بالانتظار";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Finished_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "منتهي";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Closed_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "مغلق";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UnderReview_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "قيد الدراسة";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Withdrew_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "منسحب";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cancelled_button_Click(object sender, EventArgs e)
        {
            try
            {
                State = "ملغى";
                //FundedBy_comboBox_Leave(sender, e);
                ApplyFilter_button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

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
        private void ApplyFilter_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    bind("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region save-restore columns
        private string[] arr_str;
        private void save_visible_columns()
        { 
            ////(store 1/0 instead of names).
            var values = MP_dataGridView.Columns
                .Cast<DataGridViewColumn>()
                .Select(c => c.Visible ? "1" : "0");
            Settings.Default.Admin_Visible_arr = string.Join(",", values);
            Settings.Default.Save();
        }
        private void restore_visible_columns()
        {
            try
            {  
                ////(store 1/0 instead of names).
                string saved = Settings.Default.Admin_Visible_arr; 
                if (string.IsNullOrWhiteSpace(saved))
                {
                    // fallback → show all
                    foreach (DataGridViewColumn col in MP_dataGridView.Columns)
                        col.Visible = true;

                    save_visible_columns(); // keep consistency
                    return;
                } 
                string[] values = saved.Split(','); 
                for (int i = 0; i < MP_dataGridView.ColumnCount && i < values.Length; i++)
                    MP_dataGridView.Columns[i].Visible = (values[i] == "1");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Index was outside the bounds of the array")
                    || ex.Message.Contains("Input string was not in a correct format"))
                { }
                else MessageBox.Show(ex.Message);
                 
                save_visible_columns();
            }
        }
        //private void ShowHideFilters_button_Click(object sender, EventArgs e)
        //{
        //    if (Hided)
        //    {
        //        Hided = false;
        //        if (Settings.Default.theme == "Light")
        //            ShowHideFilters_button.BackgroundImage = Resources.Hide2_L;
        //        else ShowHideFilters_button.BackgroundImage = Resources.Hide2_D;
        //        Search_panel.Height = PW;
        //    }
        //    else
        //    {
        //        Hided = true;
        //        if (Settings.Default.theme == "Light")
        //            ShowHideFilters_button.BackgroundImage = Resources.Show2_L;
        //        else ShowHideFilters_button.BackgroundImage = Resources.Show2_D;
        //        Search_panel.Height = 0;
        //    }
        //}

        //private void ShowHideColumns_button_MouseClick(object sender, MouseEventArgs e)
        //{
        //    cs.mDataGridView_MouseClick(sender, e);
        //}

        //private void ShowHideColumns_button_Click(object sender, EventArgs e)
        //{
        //    //
        //}
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
                //if (SearchFor == "Family")
                //{
                //    Family_HiddenColumns.Add(e.ClickedItem.Text);
                //}
                //else if (SearchFor == "Member")
                //{
                //    Member_HiddenColumns.Add(e.ClickedItem.Text);
                //}
                //else if (SearchFor == "Partner")
                //{
                //    Partner_HiddenColumns.Add(e.ClickedItem.Text);
                //}
                //else if (SearchFor == "Distribution")
                //{
                //    Distribution_HiddenColumns.Add(e.ClickedItem.Text);
                //}

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tick == 0)
            { 
                bind(""); 
                tick++;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void ConvertCellToComboBoxAndColor()
        {

            for (var i = 0; i < MP_dataGridView.RowCount; i++)
            {
                //حالة المشروع//
                var ComboBoxCell1 = new DataGridViewComboBoxCell();
                for (int dd = 0; dd < ds.Tables[14].Rows.Count; dd++)
                {
                    ComboBoxCell1.Items.Add(ds.Tables[14].Rows[dd].Field<string>(1));
                }
                ComboBoxCell1.Value = MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value;
                ComboBoxCell1.FlatStyle = FlatStyle.Flat;
                MP_dataGridView["حالة المشروع", i] = ComboBoxCell1;

                //الجهة الممولة//
                var ComboBoxCell2 = new DataGridViewComboBoxCell();
                ComboBoxCell2.Items.Add("None");
                for (int dd = 0; dd < ds.Tables[3].Rows.Count; dd++)
                {
                    ComboBoxCell2.Items.Add(ds.Tables[3].Rows[dd].Field<string>(1));
                }
                ComboBoxCell2.Value = MP_dataGridView.Rows[i].Cells["الجهة الممولة"].Value;
                ComboBoxCell2.FlatStyle = FlatStyle.Flat;
                MP_dataGridView["الجهة الممولة", i] = ComboBoxCell2;

                //نوع المشروع//
                var ComboBoxCell3 = new DataGridViewComboBoxCell();
                for (int dd = 0; dd < ds.Tables[6].Rows.Count; dd++)
                {
                    ComboBoxCell3.Items.Add(ds.Tables[6].Rows[dd].Field<string>(1));
                }
                ComboBoxCell3.Value = MP_dataGridView.Rows[i].Cells["نوع المشروع"].Value;
                ComboBoxCell3.FlatStyle = FlatStyle.Flat;
                MP_dataGridView["نوع المشروع", i] = ComboBoxCell3;

                //حالة المستفيد// سبب الحالة
                var ComboBoxCell4 = new DataGridViewComboBoxCell();
                for (int jj = 0; jj < ds.Tables[10].Rows.Count; jj++)
                {
                    ComboBoxCell4.Items.AddRange(ds.Tables[10].Rows[jj].Field<string>(1));
                }
                ComboBoxCell4.Value = MP_dataGridView.Rows[i].Cells["سبب الحالة"].Value;
                ComboBoxCell4.FlatStyle = FlatStyle.Flat;
                MP_dataGridView["سبب الحالة", i] = ComboBoxCell4;

                //تبديل المشروع//
                var ComboBoxCell5 = new DataGridViewComboBoxCell();
                ComboBoxCell5.Items.AddRange("نعم", "لا");
                ComboBoxCell5.Value = MP_dataGridView.Rows[i].Cells["تبديل المشروع"].Value;
                ComboBoxCell5.FlatStyle = FlatStyle.Flat;
                MP_dataGridView["تبديل المشروع", i] = ComboBoxCell5;

                //color
                if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "بالانتظار")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Waiting_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "مرفوض")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Regected_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "مغلق")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Closed_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "مؤجل")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Delayed_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "مقبول")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Accepted_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "قيد الدراسة")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Prospecting_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "ممول")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Financed_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "منتهي")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor = Finished_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "منسحب")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Withdrew_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "ملغى")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            Cancelled_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (MP_dataGridView.Rows[i].Cells["حالة المشروع"].Value.ToString() == "تسجيل أولي")
                {
                    MP_dataGridView.Rows[i].DefaultCellStyle.BackColor =
                        MP_dataGridView.Rows[i].Cells["حالة المشروع"].Style.BackColor =
                            OnHold_button.BackColor;
                    MP_dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            MP_dataGridView.Columns["اسم الأم"].Width = MP_dataGridView.Columns["اسم الأب"].Width = 60;
            MP_dataGridView.Columns["رقم المشروع"].Width = 70;

            MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                MP_dataGridView.Columns["الموبايل"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["حالة المشروع"].DefaultCellStyle.Alignment =
                        MP_dataGridView.Columns["مبلغ التمويل"].DefaultCellStyle.Alignment =
                            MP_dataGridView.Columns["المبلغ المطلوب"].DefaultCellStyle.Alignment =
                                MP_dataGridView.Columns["الرقم الوطني"].DefaultCellStyle.Alignment =
                                    MP_dataGridView.Columns["رقم دفتر العائلة"].DefaultCellStyle.Alignment =
                                        MP_dataGridView.Columns["الجهة الممولة"].DefaultCellStyle.Alignment =
                                            MP_dataGridView.Columns["نوع المشروع"].DefaultCellStyle.Alignment =
                                                DataGridViewContentAlignment.MiddleCenter;
        }
        private void addStateButtonToGrid()
        {
            // Add State History Button //
            var colCB = new DataGridViewButtonColumn();
            colCB.Name = "StateHistory";
            colCB.HeaderText = "حالات المشروع";
            colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCB.FlatStyle = FlatStyle.Flat;
            MP_dataGridView.Columns.Add(colCB);
            MP_dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MP_dataGridView.Columns["StateHistory"].Width = 80;
            // ///////////////////////////////////////////////////
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            save_visible_columns();
        }
        private void AdminPanel_Activated(object sender, EventArgs e)
        {
            Check_Theme();
        }

        private void Refresh_button_Click_1(object sender, EventArgs e)
        {
            save_visible_columns();
            var NeededFrom = RequestedFrom_textBox.Text.Replace(",", "");
            var NeededTo = RequestedTo_textBox.Text.Replace(",", "");
            var FinancedFrom = FundedFrom_textBox.Text.Replace(",", "");
            var FinancedTo = FundedTo_textBox.Text.Replace(",", "");

            State = "";

            bind("");  
        }
        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
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
                    //defaultView gets the visible rows only//
                    DataTable ex_dt = new DataTable();

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
        private void Check_Theme()
        {
            var NewTheme = new NewTheme();
            if (Settings.Default.theme == "Light")
                NewTheme.AdminPanel_ToLight(this, Hided);
            else
                NewTheme.AdminPanel_ToNight(this, Hided);
        }
        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        #region datagridview functions
        private void MP_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {  
                pre_data = "";
                //3a talab tony // حالة المشروع
                if (e.ColumnIndex == 26)
                    pre_data = MP_dataGridView.Rows[e.RowIndex].Cells["حالة المشروع"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string pre_data = "";

        private void MP_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل تريد حفظ التعديلات التي قمت بها؟", "Confirm before save",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    Program.buildConnection();
                    var r = e.RowIndex;

                    MicroProject_ID = Convert.ToInt32(MP_dataGridView.Rows[r].Cells["رقم المشروع"].Value);
                    Person_ID = Convert.ToInt32(MP_dataGridView.Rows[r].Cells["رقم المستفيد"].Value);
                    var query = "";
                    var Updated_col_name = "";

                    // حالة المشروع
                    if (e.ColumnIndex == 24) // if we update just the 
                    {
                        var state = -1;
                        string edited_column = MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString();
                        if (edited_column == "مقبول")
                        {
                            if (Program.role != 1 && Program.role != 8)
                                throw new InvalidOperationException("Can't edit this value from this window...");

                            //insert the tasks of this project//
                            TasksOfProjects = new TasksOfProjects();
                            TasksOfProjects.Insert_AllTasks_MicroProject(MicroProject_ID, DateTime.Now, "f");
                            state = 1;
                            l.Insert_Log("Insert all (f) tasks of project: " + MicroProject_ID + " ", " Task_Project ",
                                Program.UserName, DateTime.Now);
                        }
                        if (edited_column == "منسحب")
                        {
                            // Delete all notificattions of this project (Mark Seen)
                            //user_ID = -1 //// clear for all users 
                            var noti = new UserNotification();
                            noti.Update_UserNotification(MicroProject_ID, -1);

                            //remove donor if it is not N/A//
                            if (replaceQuotation(MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value.ToString()) == "None"
                                || replaceQuotation(MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value.ToString()) == "")
                            { //do nothing 
                            }
                            else
                            {
                                query += "update `microproject` set MP_Donor = " + SqlInt32.Null
                                + " where MP_ID = " + MicroProject_ID + ";";
                                MessageBox.Show("انتبه! سيتم حذف الجهة الممولة من المشروع");
                            }
                        }
                        if (edited_column == "ملغى" || edited_column == "منتهي" || edited_column == "مغلق")
                        {
                            // Delete UNSEEN notificattions of this project 
                            //user_ID = -1 //// clear for all users 
                            var noti = new UserNotification();
                            noti.Update_UserNotification(MicroProject_ID, -1);
                            l.Insert_Log("Update Notifications set SEEN of project:"
                                + MicroProject_ID + " : " + MP_dataGridView.Rows[r].Cells["المستفيد"].Value
                                , "Admin Panel", Program.UserName, DateTime.Now);
                        }
                        if (edited_column != "مرفوض" && edited_column != "بالانتظار")
                        {
                            //Send Notification to all users if the status changned - except rejected & waiting 
                            User user1 = new User();
                            UserNotification noti1 = new UserNotification();
                            var user_db = user1.Select_Users("", "", " and role.Role_Name Not like 'Out of Service' and role.Role_Name Not like 'Guest'");
                            if (user_db != null)
                                for (var i = 0; i < user_db.Rows.Count; i++)
                                    noti1.Insert_UserNotification(
                                        DateTime.Now.ToString("yyyy/MM/dd")
                                        , "تم تبديل حالة المشروع إلى: " + edited_column
                                        , MP_dataGridView.Rows[r].Cells["المستفيد"].Value.ToString()
                                        , MicroProject_ID
                                        , user_db.Rows[i].Field<int>(0)
                                        , Program.userID
                                        , -998 - i);
                        }
                        if (edited_column == "ممول")
                        {
                            if (pre_data == "مقبول")
                            {
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value = pre_data;
                                MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change
                                throw new InvalidOperationException("لا يمكن تعديل المشروع المقبول الى حالة ممول من هذه الواجهة، حتى يقوم مسؤول المالية بادخال القرض من واجهة Loans and Payments ");
                            }
                        }
                        //NEW
                        if (edited_column == "بالانتظار")
                        {
                            // لا نسمح بتبديل الحالة من تسجيل اولي الى انتظار من هذه الواجهة
                            // بل يجب عليه تعبئة الاستمارة وستتعدل لوحدها
                            if (pre_data == "تسجيل أولي")
                            {
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value = pre_data;
                                MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change
                                throw new InvalidOperationException("لا يمكن تعديل المسجل على الاستمارة الأولية الى حالة انتظار من هذه الواجهة، حتى يتم ادخال استمارة المشروع");
                            }
                        }
                        //NEW
                        if (edited_column == "تسجيل أولي")
                        {
                            //اقفال امكانية اختيار حالة تسجيل اولي   
                            MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value = pre_data;
                            MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change
                            throw new InvalidOperationException("لا يمكن تعديل حالة المشروع لتصبح تسجيل اولي لانها الحالة الافتراضية التي يتم اخذها اوتوماتيكيا عند تعبئة استمارة اولية    \r\n");
                        }

                        query += " update microproject_state set Current_State = 0 where MicroProject_ID = " + MicroProject_ID + ";"
                            + " INSERT INTO microproject_state (`MicroProject_ID`, `State_ID`, `Date`) Values ("
                            + MicroProject_ID + ","
                            + "(select ID from `state` where Name_ar like N'" + edited_column + "')" + ","
                            + "N'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'" + ");";

                        Updated_col_name = "State:" + edited_column;
                    }
                    // الموبايل
                    else if (e.ColumnIndex == 9)
                    {
                        query = "update `person` set" +
                                " P_Mobile = N'" + MP_dataGridView.Rows[r].Cells["الموبايل"].Value + "' " +
                                " where P_ID = " + Person_ID + " ";
                        Updated_col_name = "Mobile:" + MP_dataGridView.Rows[r].Cells["الموبايل"].Value;
                    }
                    // وصف المشروع - المبلغ المطلوب
                    else if (e.ColumnIndex == 12 || e.ColumnIndex == 14)
                    {
                        query = "update `microproject` set" +
                                " MP_Name = N'" + MP_dataGridView.Rows[r].Cells["وصف المشروع"].Value + "'," +
                                " MP_RequestedAmount = N'" + MP_dataGridView.Rows[r].Cells["المبلغ المطلوب"].Value +
                                "'" +
                                " where MP_ID = " + MicroProject_ID + " ";

                        Updated_col_name = "Project Name/Requested money";
                    }
                    // ملاحظات المشروع
                    else if (e.ColumnIndex == 22)
                    {
                        query = "update `microproject` set MP_Note = N'" +
                                replaceQuotation(MP_dataGridView.Rows[r].Cells["ملاحظات المشروع"].Value.ToString()) + "' "
                                + " where MP_ID = " + MicroProject_ID + " ";

                        Updated_col_name = "Comments";
                    }
                    //دورة تدريبية 
                    else if (e.ColumnIndex == 23)
                    {
                        query = "update `person` set" +
                                " P_MaristesCourse = N'" + replaceQuotation(MP_dataGridView.Rows[r].Cells["دورة تدريبية"].Value.ToString()) +
                                " where P_ID = " + Person_ID + " ";
                        Updated_col_name = "Maristes Course";
                    }
                    // سبب الحالة - ملاحظة الحالة
                    else if (e.ColumnIndex == 25 || e.ColumnIndex == 26)
                    {
                        string edited_column = MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString();
                        query = " update microproject_state set "
                            + " StateReason_ID = (select ID from `statereason` where Name like N'" + MP_dataGridView.Rows[r].Cells["سبب الحالة"].Value + "')"
                            + ",Note = N'" + MP_dataGridView.Rows[r].Cells["ملاحظة الحالة"].Value.ToString() + "' "

                            + " where MicroProject_ID = " + MicroProject_ID
                            + " and State_ID = (select ID from `state` where Name_ar like N'" + edited_column + "')"
                            + " ORDER BY Date DESC LIMIT 1 ";
                        Updated_col_name = "State Reason:" + MP_dataGridView.Rows[r].Cells["سبب الحالة"].Value;
                    }
                    // الجهة الممولة
                    else if (e.ColumnIndex == 27)
                    {
                        string edited_column = Program.ReplaceQuotation(
                            MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value.ToString());

                        if (edited_column == "None")
                        {
                            // مشاريع ممولة
                            if (
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "مقبول" ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "ممول"  ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "منتهي" ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "مغلق"  ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "ملغى")
                            {
                                MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                                throw new Exception("يجب تحديد الجهة الممولة للمشاريع الممولة");
                            }
                            // باقي المشاريع 
                            else
                            {
                                query = "update `microproject` set MP_Donor = " + SqlInt32.Null
                                      + " where MP_ID = " + MicroProject_ID + " ";
                            }
                        }
                        // بحال اختيار جهة ممولة
                        else
                        {
                            // مشاريع ممولة
                            if (
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "مقبول" ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "ممول"  ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "منتهي" ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "مغلق"  ||
                                MP_dataGridView.Rows[r].Cells["حالة المشروع"].Value.ToString() == "ملغى")
                            {
                                query = "update `microproject` " +
                               " set MP_Donor = (Select ID from donor where donor.Name like '" + edited_column + "') " +
                               " where MP_ID = " + MicroProject_ID + " ";
                            }
                            // باقي المشاريع
                            else
                            {
                                MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value = "None";
                                MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                                throw new Exception("لا يمكن اختيار جهة ممولة للمشاريع بحالة التسجيل الأولي أو الانتظار أو قيد الدراسة أو الرفض أو الانسحاب");
                            }
                        }

                        Updated_col_name = "Donor:" + edited_column;
                    }
                    // تبديل المشروع
                    else if (e.ColumnIndex == 28)
                    {
                        query = "update `microproject` set" +
                                " IsContentUpdated = " + (MP_dataGridView.Rows[r].Cells["تبديل المشروع"].Value.ToString() == "نعم" ? 1 : 0) +
                                " where MP_ID = " + MicroProject_ID + " ";
                        Updated_col_name = "IsContentUpdated";
                    }

                    Program.buildConnection();
                    using (var sc = new MySqlCommand(query, Program.MyConn))
                    {
                        sc.ExecuteNonQuery();
                        Program.MyConn.Close();
                    }

                    l.Insert_Log(
                        "Update " + Updated_col_name + " of " + MP_dataGridView.Rows[r].Cells["المستفيد"].Value +
                        " : " + MicroProject_ID + " ", "Admin Panel", Program.UserName, DateTime.Now);

                    MP_dataGridView.EndEdit();
                    ConvertCellToComboBoxAndColor();

                    //if we change the state to مقبول
                    if (Updated_col_name == "State:" + "مقبول")
                    {
                        if (MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value == DBNull.Value
                            || MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value.ToString() == ""
                            || MP_dataGridView.Rows[r].Cells["الجهة الممولة"].Value.ToString() == "None")
                        {
                            MessageBox.Show("انتبه! يجب عليك اختيار الجهة الممولة للمشروع");
                            //DataGridViewCell cell = MP_dataGridView.Rows[r].Cells["الجهة الممولة"];
                            //MP_dataGridView.CurrentCell = cell;
                            //MP_dataGridView.BeginEdit(true);
                        }
                    }
                }
                else
                {
                    MP_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MP_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void MP_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Check if click is on specific column 
                if (e.ColumnIndex == MP_dataGridView.Columns["StateHistory"].Index)
                {
                    var MP_ID = Convert.ToInt32(MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"].Value.ToString());

                    string query = @" SELECT `MicroProject_ID` as 'رقم المشروع'
, state.Name_ar as 'الحالة'
, statereason.Name as 'سبب الحالة'
, `Note` as 'ملاحظة الحالة'
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MP_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == MP_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

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
        private void MP_dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                SelectedDataRow = ((DataRowView)MP_dataGridView.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
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


                        Form Appplication_New_Form = new Application_New_Form(main_form, MicroProject_ID, Person_ID, ApplicationState);
                        main_form.showNewTab(Appplication_New_Form, "Application", 0);

                    }
                }
                else
                {
                    throw new InvalidOperationException("Please Select a row first");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Please Select a row first"))
                    MessageBox.Show("Please Select a row first");
                else
                    MessageBox.Show(ex.Message);
            }
        }
        private void MP_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0) // mp_id
                {
                    if (MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"] == null ||
                        MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"].Value == DBNull.Value)
                        MicroProject_ID = -1;
                    else
                        MicroProject_ID = int.Parse(MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"].Value
                            .ToString());

                    if (MP_dataGridView.Rows[e.RowIndex].Cells["رقم المستفيد"] == null ||
                        MP_dataGridView.Rows[e.RowIndex].Cells["رقم المستفيد"].Value == DBNull.Value)
                        Person_ID = -1;
                    else
                        Person_ID = int.Parse(MP_dataGridView.Rows[e.RowIndex].Cells["رقم المستفيد"].Value
                            .ToString());


                    //NEW
                    if (SelectedDataRow["حالة المشروع"].ToString() == "تسجيل أولي")
                    { 
                        Form Appplication_New_Form = new Application_New_Form(main_form, MicroProject_ID, Person_ID, "zero");
                        main_form.showNewTab(Appplication_New_Form, "Application", 0);
                    }
                    else
                    {
                        Form Appplication_New_Form = new Application_New_Form(main_form, MicroProject_ID, Person_ID, "project");
                        main_form.showNewTab(Appplication_New_Form, "Application", 0);
                    }
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
                bs.Filter = Original_Filter_String = MP_dataGridView.FilterString;
                ConvertCellToComboBoxAndColor();
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
                ConvertCellToComboBoxAndColor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var SelectedRowCount = MP_dataGridView.SelectedRows.Count;
                Selected_label.Text = "Selected: " + SelectedRowCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    if (All_Filters.EndsWith("or ")) All_Filters = All_Filters.Substring(0, All_Filters.Length - 3);

                    bs.Filter = All_Filters;
                }

                ConvertCellToComboBoxAndColor();

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
            Count_label.Text = "All: " + MP_dataGridView.Rows.Count.ToString("#,##0");
        }
        #endregion

        #region mouse enter-leave
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
        private void ExportToExcel_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                ExportToExcel_button.BackgroundImage = Resources.Excel_L;
            else ExportToExcel_button.BackgroundImage = Resources.Excel_D;
        }
        private void ExportToExcel_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                ExportToExcel_button.BackgroundImage = Resources.Excel_D;
            else ExportToExcel_button.BackgroundImage = Resources.Excel_L;
        }
        private void ShowHideFilters_button_MouseEnter(object sender, EventArgs e)
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
        private void ShowHideFilters_button_MouseLeave(object sender, EventArgs e)
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

        public void Check_Conditions(string initial_cond)
        {
            condition = "";
            condition += initial_cond;

            //State
            if (State != "")
            {
                condition += " and state.Name_Ar in ('" + State + "')";
            }
            //Reason
            if (StateReason_comboBox.Text != "" && StateReason_comboBox.Visible)
            {
                condition += " and statereason.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(StateReason_comboBox, true) + ")";
            }
            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////

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

            ////MP Status
            //if (MP_Status_comboBox.Text != "" && MP_Status_comboBox.Visible)
            //{
            //    //if (State == "ممول+منتهي+ملغى+مغلق")
            //    //     condition += " and state.ID in (4,5,7,8) ";
            //    //else condition += " and state.Name_Ar like N'" + State + "' ";
            //    condition += " and state.Name_Ar in ("
            //        + Program.getSelectedItems_CheckboxComboBox(MP_Status_comboBox, true) + ")";
            //}

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
                    = ds.Tables[9].Select("Name in ("
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


        #region textbox , combobox events
        private void FundedBy_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                var NeededFrom = RequestedFrom_textBox.Text.Replace(",", "");
                var NeededTo = RequestedTo_textBox.Text.Replace(",", "");
                var FinancedFrom = FundedFrom_textBox.Text.Replace(",", "");
                var FinancedTo = FundedTo_textBox.Text.Replace(",", "");

                //bind("");


                //, FundType_comboBox.Text, Type_comboBox.Text, SubType_comboBox.Text, State, replaceQuotation(Donor_comboBox.Text), replaceQuotation(DonorGroup_comboBox.Text), Category_comboBox.Text
                //, SubCategory_comboBox.Text, Career_comboBox.Text, ApplyDate_comboBox.Text, FundDate_comboBox.Text, StateReason_comboBox.Text
                //, P_Parish_comboBox.Text, Age_comboBox.Text, Gender_comboBox.Text, MaritalStatus_comboBox.Text,Partnership_comboBox.Text
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Category_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    SubCategory_bind(Category_comboBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SubCategory_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    Career_bind(SubCategory_comboBox.SelectedValue.ToString());
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
        private void Donor_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    DonorGroup_bind(Donor_comboBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region right click menu
        private void refreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh_button_Click_1(sender, e);
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
        private void clearAllFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type_comboBox.Text = Donor_comboBox.Text = DonorGroup_comboBox.Text
                = Category_comboBox.Text = SubCategory_comboBox.Text
                = Career_comboBox.Text = ApplyDate_comboBox.Text = FundDate_comboBox.Text = StateReason_comboBox.Text
                = P_Parish_comboBox.Text = Age_comboBox.Text = Gender_comboBox.Text = MaritalStatus_comboBox.Text = "";
            RequestedFrom_textBox.Text = RequestedTo_textBox.Text = FundedFrom_textBox.Text = FundedTo_textBox.Text = "";
            State = "";

            ApplyDate_condition = FundedDate_condition
               = RequestedAmount_condition = FundedAmount_condition = "";
            ApplyDate_checkBox.Checked = FundedDate_checkBox.Checked = false;

            Refresh_button_Click_1(sender, e);
        }
        #endregion

        #region filters date checkboxs functions
        //private void Requested_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (Requested_checkBox.Checked)
        //    {
        //        RequestedFrom_label.Enabled = RequestedFrom_textBox.Enabled
        //            = RequestedTo_textBox.Enabled = true;
        //        RequestedFrom_textBox_Leave(sender, e);
        //    }
        //    else
        //    {
        //        RequestedAmount_condition = "";
        //        RequestedFrom_label.Enabled = RequestedFrom_textBox.Enabled 
        //            = RequestedTo_textBox.Enabled = false;

        //        Refresh_button_Click_1(sender, e);
        //    }
        //}
        //private void Funded_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (Funded_checkBox.Checked)
        //    {
        //        FundedFrom_label.Enabled = FundedFrom_textBox.Enabled
        //            //= FundedTo_label.Enabled
        //            = FundedTo_textBox.Enabled = true;
        //        FundedFrom_textBox_Leave(sender, e);
        //    }
        //    else
        //    {
        //        FundedAmount_condition = "";
        //        FundedFrom_label.Enabled = FundedFrom_textBox.Enabled
        //            = FundedTo_textBox.Enabled = false;

        //        Refresh_button_Click_1(sender, e);
        //    }
        //}
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
        private void ApplyDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ApplyDate_checkBox.Checked)
            {

                ApplyDateFrom_dateTimePicker.Enabled = ApplyDateFrom_label.Enabled =
                ApplyDateTo_DateTimePicker.Enabled = true;

                ApplyDateFrom_dateTimePicker_ValueChanged(sender, e);
            }
            else
            {
                ApplyDate_condition = "";

                ApplyDateFrom_dateTimePicker.Enabled = ApplyDateFrom_label.Enabled =
                ApplyDateTo_DateTimePicker.Enabled = false;

                //Refresh_button_Click_1(sender, e);
            }
        }
        private void FundedDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FundedDate_checkBox.Checked)
            {

                FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled =
                    FundedDateTo_DateTimePicker.Enabled = true;

                FundedDateFrom_bcDateTimePicker_ValueChanged(sender, e);
            }
            else
            {
                FundedDate_condition = "";

                FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled =
                    FundedDateTo_DateTimePicker.Enabled = false;


                //Refresh_button_Click_1(sender, e);
            }
        }
        private void ApplyDateFrom_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ApplyDateFrom_dateTimePicker.Value > ApplyDateTo_DateTimePicker.Value)
                {
                    ApplyDate_condition = "";
                    throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");
                }

                string beg, end;
                beg = ApplyDateFrom_dateTimePicker.Value.ToString("yyyy/MM/dd");
                end = ApplyDateTo_DateTimePicker.Value.ToString("yyyy/MM/dd");

                ApplyDate_condition = " and ( MP_DateOfRequest between '" + beg + "' and '" + end + "' )";

                //Refresh_button_Click_1(sender, e);
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

                //Refresh_button_Click_1(sender, e);
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

                    //Refresh_button_Click_1(sender, e);
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
                    //" and (L.Loan_Amount BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";
                    " and ( Round( (Loan_Amount / donorgroup.Rate) , 2 ) BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";

                //Refresh_button_Click_1(sender, e);
            }
        }
        #endregion

    }
}