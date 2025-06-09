using ClosedXML.Excel;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class ShowDetails_Form : Form
    {
        DataTable dt;
        BindingSource bs = new BindingSource();
        string Original_Filter_String = "";

        public ShowDetails_Form(DataTable dt, string form_lable)
        {
            InitializeComponent();
            this.dt = dt;
            Form_label.Text = form_lable;

            dataGridView.DoubleBuffered(true);
            bs.ListChanged += bs_ListChanged;
        }

        private void ShowDetails_Form_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.ShowDetails_Form_ToNight(this);
                else
                    newTheme.ShowDetails_Form_ToLight(this);

                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.Rows.Clear();
                dataGridView.ColumnHeadersVisible = false;
                dataGridView.RowHeadersVisible = false;

                bs.DataSource = null;
                bs.Filter = bs.Sort = Original_Filter_String = "";
                bs.DataSource = dt;

                dataGridView.DataSource = bs;
                dataGridView.ColumnHeadersVisible = true;
                dataGridView.RowHeadersVisible = true;

                SearchToolBar.SetColumns(dataGridView.Columns);
                Count_label.Text = "All:" + dataGridView.Rows.Count.ToString("#,##0");
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

        #region ADGV datagridview events
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = dataGridView.FilterString;
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
                bs.Sort = dataGridView.SortString;
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
                var SelectedRowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                Selected_label.Text = "Selected: " + SelectedRowCount.ToString("#,##0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
                        int VisibleColumnsCount = dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
                        int i = 0;
                        foreach (DataGridViewColumn C in dataGridView.Columns)
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
            //            List<int> list = new List<int>();
            //Then add all the rows of the column to this list.
            //Then try list.Distinct().ToList()
            //This list will give the required distinct values 
            //var distinctRows = (from DataGridViewRow row in dataGridView.Rows
            //                    select row.Cells["رقم المشروع"]).Distinct().Count();
            //Count_label.Text = "All: " + distinctRows.ToString("#,##0");
            Count_label.Text = "All: " + dataGridView.Rows.Count.ToString("#,##0");
        }
        #endregion


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
                        ex_dt = ((DataTable)dataGridView.DataSource).DefaultView.ToTable();
                    else ex_dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    ////Remove invisible columns//
                    //for (var j = dataGridView.ColumnCount - 1; j >= 0; j--)
                    //    if (dataGridView.Columns[j].Visible == false)
                    //        ex_dt.Columns.RemoveAt(j);
                    for (var j = 0; j <= dataGridView.ColumnCount - 1; j++)
                    {
                        if (dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(dataGridView.Columns[j].Name.ToString());
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


        #region mouse hover

        private void ExportToExcel_button_MouseEnter(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_L;
        }
        private void ExportToExcel_button_MouseLeave(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_D;
        }
        #endregion

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDetails_Form_Load(sender, e);
        }
    }
}