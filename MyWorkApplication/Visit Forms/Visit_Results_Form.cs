using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace MyWorkApplication.Visit_Forms
{
    public partial class Visit_Results_Form : Form
    {
        private M_and_E me;
        private Visit v;
        private SubCategory sub;
        private Career c;
        private Select s;
        private MainForm main_form;
        private bool user_mode;
        private List<string> searchBy_list;
        private BindingSource bs = new BindingSource();
        private BindingSource b_bs = new BindingSource();
        private string Original_Filter_String;
        private string visitNum, visitKind, mp_typeID, mp_categoryID, mp_subcategoryID, mp_careerID;
        private string DateFrom = "", DateTo = "";
        private bool onlyLastVisit = false;

        private int PW;
        private bool Hided;

        public Visit_Results_Form(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        private void Monitoring_Visit_Results_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Check_Theme();

                me = new M_and_E();
                v = new Visit();
                sub = new SubCategory();
                c = new Career();
                s = new Select();

                clear_filters();

                PW = Search_panel.Height;
                Hided = true;
                Search_panel.Height = 0;

                user_mode = false;
                bind_comboBoxes();

                Answers_dataGridView.DoubleBuffered(true);
                //Benficiaries_dataGridView.DoubleBuffered(true);

                searchBy_list = new List<string>();
                searchBy_list.Add("عدد إجابات كل سؤال");
                searchBy_list.Add("إجابات المستفيدين");
                for (int i = 0; i < searchBy_list.Count; i++)
                    SearchBy_comboBox.Items.Add(searchBy_list.ElementAt<string>(i));

                bs.ListChanged += bs_ListChanged;
                b_bs.ListChanged += bs_ListChanged;

                DateFrom = DateFrom_dateTimePicker.Value.ToString("yyyy/MM/dd");
                DateTo = DateTo_dateTimePicker.Value.ToString("yyyy/MM/dd");

                //if (visit_statistics_tableLayoutPanel.Visible) visit_statistics_tableLayoutPanel.Visible = false;

                LastV_button.BackColor = Color.Transparent;
                visitKind = "v";
                VisitKind_comboBox.SelectedIndex = 0; // مشاريع أخرى 

                VisitNumber_comboBox.SelectedIndex = 0; //all Monitoring

                Type_comboBox.SelectedIndex = 0; //مشاريع صغيرة 
                Category_comboBox.SelectedIndex = -1; //no selection

                user_mode = true;
                SearchBy_comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Check_Theme()
        {
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.Search_ToNight(this, false);
            else
                newTheme.Search_ToLight(this, false);
        }

        private void bind_comboBoxes()
        {
            string query = "";
            query += "select ID,Name from `microprojecttype` order by ID ASC;"; //0  
            query += "select ID,Name from `category` ORDER BY Name ASC;"; //1

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            Type_comboBox.DataSource = null;
            Type_comboBox.DisplayMember = "Name";
            Type_comboBox.ValueMember = "ID";
            Type_comboBox.DataSource = ds.Tables[0];

            Category_comboBox.DataSource = null;
            Category_comboBox.DisplayMember = "Name";
            Category_comboBox.ValueMember = "ID";
            Category_comboBox.DataSource = ds.Tables[1];
        }
        private void SubCategory_bind(string Category_ID)
        {
            DataTable sub_st = sub.Select(Category_ID, "");
            SubCategory_comboBox.DataSource = null;
            SubCategory_comboBox.DisplayMember = "Name";
            SubCategory_comboBox.ValueMember = "ID";
            SubCategory_comboBox.DataSource = sub_st;
            SubCategory_comboBox.SelectedIndex = -1;
        }
        private void Career_bind(string SubCategory_ID)
        {
            DataTable c_st = c.Select(SubCategory_ID, "");
            Career_comboBox.DataSource = null;
            Career_comboBox.DisplayMember = "Name";
            Career_comboBox.ValueMember = "ID";
            Career_comboBox.DataSource = c_st;
            Career_comboBox.SelectedIndex = -1;
        }

        private void Category_comboBox_TextChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (Category_comboBox.Text != "")
                    SubCategory_bind(Category_comboBox.SelectedValue.ToString());
            }
        }
        private void SubCategory_comboBox_TextChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (SubCategory_comboBox.Text != "")
                    Career_bind(SubCategory_comboBox.SelectedValue.ToString());
            }
        }

        private void Select_AnswersNum(bool onlyLastVisit
            , string Visit_Kind, string visit_Num
            , string DateFrom, string DateTo
            , string Type, string category, string subCategory, string career)
        {
            try
            {
                DataTable dt = new DataTable();
                Answers_dataGridView.Columns.Clear();
                Answers_dataGridView.DataSource = null;
                Answers_dataGridView.ColumnHeadersVisible = false;

                Answers_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                bs.DataSource = null;

                if (visit_Num.StartsWith("M"))
                {
                    dt = me.Get_Questions_Answers_SideBySide(onlyLastVisit
                        , Visit_Kind, visit_Num
                        , DateFrom, DateTo
                        , Type, category, subCategory, career);

                    bs.DataSource = dt;
                    Answers_dataGridView.DataSource = bs;

                    DataGridViewColumn dgc = Answers_dataGridView.Columns["ans1_ID"]; dgc.Visible = false;
                    dgc = Answers_dataGridView.Columns["ans2_ID"]; dgc.Visible = false;
                    dgc = Answers_dataGridView.Columns["ans3_ID"]; dgc.Visible = false;
                    dgc = Answers_dataGridView.Columns["ID"]; dgc.Visible = false;

                    AllCount_label.Text = "All: " + bs.Count.ToString("#,##0");

                }
                else if (visit_Num.StartsWith("Opening") || visit_Num.StartsWith("Closing")) //Openeng & Closing according to visit_kind
                {
                    // // //
                }
                else
                {
                    //nothing
                }

                Answers_dataGridView.ColumnHeadersVisible = true;
                Answers_dataGridView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Select_BeneficiariesAnswers(bool onlyLastVisit
            , string Visit_Kind, string visit_Num
            , string DateFrom, string DateTo
            , string Type, string category, string subCategory, string career)
        {
            try
            {
                Answers_dataGridView.Columns.Clear();

                Answers_dataGridView.DataSource = null;
                Answers_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                Answers_dataGridView.ColumnHeadersVisible = false;

                bs.DataSource = null;


                if (visit_Num.StartsWith("M"))
                {
                    var dt = me.Get_Beneficiaries_Answers(onlyLastVisit
                    , Visit_Kind, visit_Num
                    , DateFrom, DateTo
                    , Type, category, subCategory, career);

                    bs.DataSource = dt;
                    Answers_dataGridView.DataSource = bs;
                    AllCount_label.Text = "All: " + bs.Count.ToString("#,##0");
                    Answers_dataGridView.Columns["الربح الصافي"].DefaultCellStyle.Format = "#,##0";
                }
                else if (visit_Num.StartsWith("Opening") || visit_Num.StartsWith("Closing")) //Openeng & Closing according to visit_kind
                {
                    DataSet ds = v.Get_Beneficiaries_Answers(
                          DateFrom, DateTo
                        , Visit_Kind
                        , Type, category, subCategory, career);

                    if (visit_Num.StartsWith("Opening") && visitKind == "o") bs.DataSource = ds.Tables[0];
                    else if (visit_Num.StartsWith("Opening") && visitKind == "v") bs.DataSource = ds.Tables[1];
                    else bs.DataSource = ds.Tables[2];

                    Answers_dataGridView.DataSource = bs;

                    DataGridViewColumn dgc = Answers_dataGridView.Columns["ID"]; dgc.Visible = false;
                    AllCount_label.Text = "All: " + bs.Count.ToString("#,##0");
                }
                else
                { }


                Answers_dataGridView.Columns["رقم المشروع"].Frozen =
                    Answers_dataGridView.Columns["المستفيد"].Frozen = true;
                Answers_dataGridView.ColumnHeadersVisible = true;

                Answers_dataGridView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchBy_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    // Set cursor as hourglass
                    Cursor.Current = Cursors.WaitCursor;
                    bs.Filter = bs.Sort = Original_Filter_String = "";
                    bs.DataSource = null;

                    check_filters();

                    if (SearchBy_comboBox.SelectedItem == null)
                    {
                        //user_mode = false;
                        SearchBy_comboBox.SelectedIndex = 0;
                        //user_mode = true;
                    }

                    // البحث حسب عدد اجوبة كل سؤال
                    if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                    {
                        Select_AnswersNum(onlyLastVisit
                            , visitKind, visitNum, DateFrom, DateTo
                            , mp_typeID, mp_categoryID, mp_subcategoryID, mp_careerID);
                    }
                    // البحث عن اجوبة المستفيدين
                    else if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(1))
                    {
                        Select_BeneficiariesAnswers(onlyLastVisit
                            , visitKind, visitNum, DateFrom, DateTo
                            , mp_typeID, mp_categoryID, mp_subcategoryID, mp_careerID);
                    }

                    SearchToolBar.SetColumns(Answers_dataGridView.Columns);

                    // Set cursor as default arrow
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void check_filters()
        {
            user_mode = false;
            if (VisitKind_comboBox.SelectedIndex == -1)
                visitKind = "";
            else
            {
                if (VisitKind_comboBox.SelectedItem.ToString() == "مركبات")
                    visitKind = "v";
                else visitKind = "o";
            }
            ////////////////////////////////////////////////////////////////////////////
            if (VisitNumber_comboBox.SelectedIndex == -1) //|| VisitNumber_comboBox.SelectedIndex == 0
                visitNum = "";
            else visitNum = VisitNumber_comboBox.SelectedItem.ToString();
            ////////////////////////////////////////////////////////////////////////////
            if (Type_comboBox.SelectedIndex == -1)
                mp_typeID = "";
            else mp_typeID = Type_comboBox.SelectedValue.ToString();
            ////////////////////////////////////////////////////////////////////////////
            if (Category_comboBox.SelectedIndex == -1)
                mp_categoryID = "";
            else mp_categoryID = Category_comboBox.SelectedValue.ToString();
            ////////////////////////////////////////////////////////////////////////////
            if (SubCategory_comboBox.SelectedIndex == -1)
                mp_subcategoryID = "";
            else mp_subcategoryID = SubCategory_comboBox.SelectedValue.ToString();
            /////////////////////////////////////////////////////////////////////////////
            if (Career_comboBox.SelectedIndex == -1)
                mp_careerID = "";
            else mp_careerID = Career_comboBox.SelectedValue.ToString();
            /////////////////////////////////////////////////////////////////////////////
            if (LastV_button.BackColor == Color.Transparent)
                onlyLastVisit = false;
            else onlyLastVisit = true;
            /////////////////////////////////////////////////////////////////////////////
            DateFrom = DateFrom_dateTimePicker.Value.ToString("yyyy/MM/dd");
            DateTo = DateTo_dateTimePicker.Value.ToString("yyyy/MM/dd");
            user_mode = true;
        }
        private void clear_filters()
        {
            user_mode = false;
            visitNum = mp_typeID = mp_categoryID = mp_subcategoryID = mp_careerID = "";
            LastV_button.BackColor = Color.Transparent;
            onlyLastVisit = false;
            VisitNumber_comboBox.SelectedItem = 0;
            VisitKind_comboBox.SelectedItem = 0; visitKind = "v";
            Type_comboBox.SelectedIndex = Category_comboBox.SelectedIndex
                = SubCategory_comboBox.SelectedIndex = Career_comboBox.SelectedIndex = -1;
            user_mode = true;
        }

        #region datagridview functions
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = Answers_dataGridView.FilterString;
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
                bs.Sort = Answers_dataGridView.SortString;
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
                var selectedCellCount = Answers_dataGridView.GetCellCount(DataGridViewElementStates.Selected);
                SelectedCount_label.Text = "Selected Count: " + selectedCellCount.ToString("#,##0");

                double judy = 0;
                foreach (DataGridViewCell cell in Answers_dataGridView.SelectedCells)
                {
                    try { double.Parse(cell.Value.ToString()); }
                    catch { continue; }
                    judy = judy + double.Parse(cell.Value.ToString());
                }
                SelectedSum_label.Text = "Selected Sum: " + judy.ToString("#,##0");

                // البحث حسب عدد اجوبة كل سؤال
                if (SearchBy_comboBox.SelectedItem.ToString() == searchBy_list.ElementAt<string>(0))
                {
                    if (selectedCellCount > 0)
                    {
                        var sb = new StringBuilder();

                        for (var i = 0; i < selectedCellCount; i++)
                        {
                            var selected_row = Answers_dataGridView.SelectedCells[i].RowIndex;
                            var selected_column = Answers_dataGridView.SelectedCells[i].ColumnIndex;

                            //if (selected_column != 2 && selected_column != 5 && selected_column != 8) continue;
                            //just allow to click on the answer title//
                            if (selected_column == 2 || selected_column == 3 || selected_column == 4)
                            {
                                //if answer 2 is empty//
                                if (Answers_dataGridView.SelectedCells[i].Value == DBNull.Value
                                    || Answers_dataGridView.SelectedCells[i].Value.ToString() == "") continue;

                                sb.Append("Answer_ID = ");
                                sb.Append(Answers_dataGridView.Rows[selected_row].Cells[selected_column + 3].Value); //edited selected_column + 2
                                sb.Append(" AND ");
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
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                        int VisibleColumnsCount = Answers_dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
                        int i = 0;
                        foreach (DataGridViewColumn C in Answers_dataGridView.Columns)
                        {
                            if (C.Visible == true)
                            {
                                if (e.WholeWord)
                                    NewFilter += "convert(`" + C.Name + "`,'System.String') = '" + e.ValueToSearch + "'";
                                else
                                    NewFilter += "convert(`" + C.Name + "`,'System.String') like '%" + e.ValueToSearch + "%'";

                                if (i < VisibleColumnsCount - 1)
                                    NewFilter += " or ";

                                i++;
                            }
                        }
                    }
                    string All_Filters = Original_Filter_String == "" ? NewFilter : Original_Filter_String + " and (" + NewFilter + ")";
                    bs.Filter = All_Filters;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        private void bs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            AllCount_label.Text = "All: " + Answers_dataGridView.Rows.Count.ToString("#,##0");
        }

        #endregion

        private void RefreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }

        private void Type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when selecting agriculture
            if (user_mode)
            {
                if (Type_comboBox.Text.Contains("زراع"))
                {
                    user_mode = false;
                    VisitKind_comboBox.SelectedIndex = 0; // مشاريع أخرى
                    //Category_comboBox.SelectedIndex = 0;  // Agriculture 
                    user_mode = true;
                }
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void Category_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when selecting agriculture
            if (user_mode)
            {
                user_mode = false;
                if (Category_comboBox.Text.Contains("Vehicles"))
                    VisitKind_comboBox.SelectedIndex = 1; // مركبات
                else VisitKind_comboBox.SelectedIndex = 0; // مشاريع أخرى
                user_mode = true;
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void ShowHide_button_Click(object sender, EventArgs e)
        {
            if (Hided)
            {
                Hided = false;
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Hide2_L;
                else ShowHide_button.BackgroundImage = Resources.Hide2_D;
                Search_panel.Height = PW;
            }
            else
            {
                Hided = true;
                if (Settings.Default.theme == "Light")
                    ShowHide_button.BackgroundImage = Resources.Show2_L;
                else ShowHide_button.BackgroundImage = Resources.Show2_D;
                Search_panel.Height = 0;
            }
        }
        private void ShowHide_button_MouseEnter(object sender, EventArgs e)
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

        private void ShowHide_button_MouseLeave(object sender, EventArgs e)
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


        private void ClearAllFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_filters();
            SearchBy_comboBox_SelectedIndexChanged(sender, e);
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
                        ex_dt = ((DataTable)Answers_dataGridView.DataSource).DefaultView.ToTable();
                    else ex_dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    ////Remove invisible columns//
                    //for (var j = Answers_dataGridView.ColumnCount - 1; j >= 0; j--)
                    //    if (Answers_dataGridView.Columns[j].Visible == false)
                    //        ex_dt.Columns.RemoveAt(j);
                    for (var j = 0; j <= Answers_dataGridView.ColumnCount - 1; j++)
                    {
                        if (Answers_dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(Answers_dataGridView.Columns[j].Name.ToString());
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

        private void VisitKind_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void LastV_button_Click(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (LastV_button.BackColor == Color.FromArgb(104, 157, 202))
                {
                    LastV_button.BackColor = Color.Transparent;
                    onlyLastVisit = false;
                }
                else
                {
                    LastV_button.BackColor = Color.FromArgb(104, 157, 202);
                    onlyLastVisit = true;
                }
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void DateFrom_bcDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }
    }
}
