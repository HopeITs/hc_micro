using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace MyWorkApplication
{
    public partial class Log_Form : Form
    {
        private Log l; private User u;
        private int[] Log_IDs;
        private DataRow LogSelectedDataRow;
        BindingSource bs = new BindingSource();
        string condition = "";
        private string Original_Filter_String = "";
        private MySqlComponents MySS;

        public Log_Form()
        {
            InitializeComponent();
        }
         
        private void Log_Form_Load(object sender, EventArgs e)
        {
            try
            {
                MySS = new MySqlComponents();
                l = new Log();
                u = new User();

                Log_dataGridView.DoubleBuffered(true);
                bs.ListChanged += bs_ListChanged;

                log_bind("","");
                //user_bind("");

                var NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.Search_ToLight(this, true);
                else
                    NewTheme.Search_ToNight(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region datagridview functions
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = Log_dataGridView.FilterString;
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
                bs.Sort = Log_dataGridView.SortString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        } 
        private void Log_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var SelectedRowCount = Log_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                Selected_label.Text = "عدد الأسطر المحددة: " + SelectedRowCount.ToString("#,##0"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Log_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                        int VisibleColumnsCount = Log_dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
                        int i = 0;
                        foreach (DataGridViewColumn C in Log_dataGridView.Columns)
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
            try
            {
                //if (Log_dataGridView.DataSource != null && Log_dataGridView.Rows.Count != 0)
                //{
                //    BindingSource f_bs = (BindingSource)Log_dataGridView.DataSource;
                //    System.Data.DataTable filtered_dt = ((System.Data.DataTable)f_bs.DataSource).DefaultView.ToTable(true, "رقم المشروع");
                //    Count_label.Text = "عدد المشاريع: " + filtered_dt.Rows.Count.ToString("#,##0");
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
        #endregion

        //private void user_bind(string username)
        //{
        //    var dt = u.Select_Users(username,"",""); 
        //    User_comboBox.DisplayMember = "Username";
        //    User_comboBox.ValueMember = "ID";
        //    User_comboBox.DataSource = dt;
        //    User_comboBox.Text = "";
        //}

        //private void Search_TxtBox_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        log_bind(Search_TxtBox.Text, User_comboBox.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void Search_TxtBox_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        log_bind(Search_TxtBox.Text,User_comboBox.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void User_comboBox_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        log_bind(Search_TxtBox.Text, User_comboBox.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void log_bind(string search, string username)
        {

            Program.buildConnection();
            search = search.ToLower();
            username = username.ToLower();

            MySS.query =
                "select `Log_ID`,`Log_Type`,`Log_User`,`Log_Date`,`Log_OnTable` from `log` where `Log_Seen` = 0 ";
            var condition = "";
            if (search != "")
                condition += " and (lower(`Log_Type`) like '%" + search + "%' or lower(`Log_OnTable`) like '%" + search +
                             "%' or lower(`Log_User`) like '%" + search + "%' or lower(`Log_Date`) like '%" + search + "%')";
            if (username != "")
                condition += " and lower(`Log_User`) like '%" + username + "%' ";

            MySS.query += condition + " order by Log_Date DESC ";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new System.Data.DataTable();
            MySS.da.Fill(MySS.dt);

            bs.DataSource = MySS.dt;
            Log_dataGridView.DataSource = bs;

            var dgC1 = Log_dataGridView.Columns["Log_ID"];
            dgC1.Visible = false;
            Program.MyConn.Close();

            Count_label.Text = Log_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString("#,##0");

            Log_dataGridView.Columns["Log_User"].DefaultCellStyle.Alignment =
                Log_dataGridView.Columns["Log_Date"].DefaultCellStyle.Alignment =
                    Log_dataGridView.Columns["Log_OnTable"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
        }

        private void DeleteLog_button_Click(object sender, EventArgs e)
        {
            try
            {
                //select specific rows
                var SelectedRowCount = Log_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (SelectedRowCount > 0)
                {
                    Log_IDs = new int[SelectedRowCount];
                    for (var i = 0; i < SelectedRowCount; i++)
                    {
                        LogSelectedDataRow = ((DataRowView) Log_dataGridView.SelectedRows[i].DataBoundItem).Row;
                        Log_IDs[i] = int.Parse(LogSelectedDataRow["Log_ID"].ToString());
                    }

                    l.Update_Log(Log_IDs);
                }
                else
                {
                    l.Update_Log();
                }

                log_bind("","");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            try
            {
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_button_MouseEnter(object sender, EventArgs e)
        {
            Refresh_button.BackgroundImage = Resources.Refresh2_L;
        }

        private void Refresh_button_MouseLeave(object sender, EventArgs e)
        {
            Refresh_button.BackgroundImage = Resources.Refresh2_D;
        }

        private void DeleteImage_button_MouseEnter(object sender, EventArgs e)
        {
            DeleteImage_button.BackgroundImage = Resources.Trash_L;
        }

        private void DeleteImage_button_MouseLeave(object sender, EventArgs e)
        {
            DeleteImage_button.BackgroundImage = Resources.Trash_D;
        }
         
        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.role != 1 && Program.role != 8 && Program.role != 5 && Program.role != 3)
                    throw new InvalidOperationException("Sorry ! You Don't have the permission for this action.");

                _Application app = new Application();
                // creating new WorkBook within Excel application  
                _Workbook workbook = app.Workbooks.Add(Type.Missing);
                // creating new Excelsheet in workbook  
                _Worksheet worksheet = null;
                // see the excel sheet behind the program  
                app.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.  
                // store its reference to worksheet  
                worksheet = workbook.Sheets[1];
                worksheet = workbook.ActiveSheet;
                // changing the name of active sheet  
                worksheet.Name = "Exported from App";

                // storing header part in Excel  
                for (var i = 1; i < Log_dataGridView.Columns.Count + 1; i++)
                    worksheet.Cells[1, i] = Log_dataGridView.Columns[i - 1].HeaderText;
                // storing Each row and column value to excel sheet  
                for (var i = 0; i < Log_dataGridView.Rows.Count; i++)
                    for (var j = 0; j < Log_dataGridView.Columns.Count; j++)
                        if (Log_dataGridView.Rows[i].Visible)
                            worksheet.Cells[i + 2, j + 1] = Log_dataGridView.Rows[i].Cells[j].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}