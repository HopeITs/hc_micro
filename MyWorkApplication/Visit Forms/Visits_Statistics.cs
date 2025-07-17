using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace MyWorkApplication.Visit_Forms
{
    public partial class Visits_Statistics : Form
    {
        public Visits_Statistics(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
            bs.ListChanged += bs_ListChanged;

            Hide_MenuItem.DropDown.Closing += Hide_MenuItem_Closing;
            Show_MenuItem.DropDown.Closing += Show_MenuItem_Closing;
        }

        private M_and_E me;
        private Visit v;
        private Select s;
        private MainForm main_form;
        BindingSource bs = new BindingSource();
        string Date_condition = "";
        string beginDate, endDate, type;
        bool user_mode;

        private void Visits_Statistics_Load(object sender, EventArgs e)
        {
            try
            {
                Check_Theme();

                me = new M_and_E();
                v = new Visit();
                s = new Classes.Select();

                user_mode = false;

                Search_DataGridView.DoubleBuffered(true);
                Type_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                Bind_All_ComboBoxes();

                Type_comboBox.SelectedIndex = -1;

                DateFrom_dateTimePicker.Value = DateTo_dateTimePicker.Value.AddYears(-1);
                
                user_mode = true;

                DateFrom_dateTimePicker_ValueChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        public void Check_Theme()
        {
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.Visit_User_ToNight(this);
            else
                newTheme.Visit_User_ToLight(this);
        }

        private void Bind_All_ComboBoxes()
        {
            string query = "";
            query += "select ID,Name from microprojecttype order by ID ASC;"; //0  
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            Type_comboBox.DataSource = null;
            Type_comboBox.DisplayMember = "Name";
            Type_comboBox.ValueMember = "ID";
            Type_comboBox.DataSource = ds.Tables[0];
            Type_comboBox.SelectedIndex = -1;
        }

        private void bind(string beginDate, string endDate, string Type)
        {
            DataSet ds = v.Get_Visits_Statistics(beginDate, endDate, Type);

            bs.DataSource = ds.Tables[0];
            Search_DataGridView.ColumnHeadersVisible = false;
            Search_DataGridView.DataSource = null;
            Search_DataGridView.Columns.Clear();
            Search_DataGridView.DataSource = bs;
            Search_DataGridView.ColumnHeadersVisible = true;

            Bind_Show_Hide_ToolStrip();
            NumOfRows1.Text = Search_DataGridView.Rows.Count.ToString();
            SearchToolBar.SetColumns(Search_DataGridView.Columns);


            sumOp_label.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["sum op"]).ToString("#,##0");
            sumCl_label.Text = Convert.ToDecimal(ds.Tables[1].Rows[0]["sum cl"]).ToString("#,##0");
            sumM1_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m1"]).ToString("#,##0");
            sumM2_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m2"]).ToString("#,##0");
            sumM3_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m3"]).ToString("#,##0");
            sumM4_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m4"]).ToString("#,##0");
            sumM5_label.Text = Convert.ToDecimal(ds.Tables[2].Rows[0]["sum m5"]).ToString("#,##0");
            decimal total = Convert.ToDecimal(ds.Tables[1].Rows[0]["total visit"]) + Convert.ToDecimal(ds.Tables[2].Rows[0]["total mevisit"]);
            sumAll_label.Text = total.ToString("#,##0");

        }

        private void Search_DataGridView_SortStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.SortEventArgs e)
        {
            try
            {
                bs.Sort = Search_DataGridView.SortString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void Search_DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int sumNumbers = 0;
                for (int i = 0; i < Search_DataGridView.SelectedCells.Count; i++)
                {
                    //if (!Search_DataGridView.SelectedCells.Contains(Search_DataGridView.Rows[i].Cells[i]))
                    //{
                    int nextNumber = 0;
                    if (int.TryParse(Search_DataGridView.SelectedCells[i].FormattedValue.ToString(), out nextNumber))
                        sumNumbers += nextNumber;
                    //}
                }
                NumOfSelected1.Text = Search_DataGridView.SelectedCells.Count.ToString("#,##0");
                SumOfSelected1.Text = sumNumbers.ToString("#,##0");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void bs_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            NumOfRows1.Text = Search_DataGridView.Rows.Count.ToString();
        }

        private void DateFrom_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DateFrom_dateTimePicker.Value > DateTo_dateTimePicker.Value)
                {
                    Date_condition = "";
                    throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");
                }

                DateTime beg, end;
                beg = DateFrom_dateTimePicker.Value;
                end = DateTo_dateTimePicker.Value;

                beginDate = beg.Year + "/" + beg.Month + "/" + beg.Day;
                endDate = end.Year + "/" + end.Month + "/" + end.Day;

                bind(beginDate, endDate, Type_comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bind(beginDate, endDate, Type_comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        #region excel
        private void Export_Exel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel |*.xlsx";
                saveFileDialog1.Title = "Export as Excel";
                DialogResult res = saveFileDialog1.ShowDialog();
                string filepath = "";

                if (res == DialogResult.OK)
                {
                    // If the file name is not an empty string open it for saving.  
                    if (!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                    {
                        // Saves the Image via a FileStream created by the OpenFile method.  
                        System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                        filepath = saveFileDialog1.FileName;
                        fs.Close();
                    }
                }

                if (!string.IsNullOrWhiteSpace(filepath))
                {

                    XLWorkbook wb = new XLWorkbook();

                    DataTable dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    //DataTable dt = SignitaureList_DataGridView.DataSource.ToTable();

                    wb.Worksheets.Add(dt, "Visit-Visitors Statistics");
                    wb.SaveAs(filepath);
                    wb.Dispose();
                    Process.Start(filepath);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void Excel_Button_MouseEnter(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Properties.Resources.Excel_L;
        }
        private void Excel_Button_MouseLeave(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Properties.Resources.Excel_D;

        }
        #endregion

        #region show-hide columns
        private void Bind_Show_Hide_ToolStrip()
        {
            ToolStripMenuItem Hide_Columns = Show_Hide_ToolStrip.DropDownItems[0] as ToolStripMenuItem;
            ToolStripMenuItem Show_Columns = Show_Hide_ToolStrip.DropDownItems[1] as ToolStripMenuItem;

            Show_Columns.DropDownItems.Clear();
            Hide_Columns.DropDownItems.Clear();

            foreach (DataGridViewColumn C in Search_DataGridView.Columns)
            {
                if (C.Visible)
                    Hide_Columns.DropDownItems.Add(C.Name);
                else
                    Show_Columns.DropDownItems.Add(C.Name);
            }
        }
        private void toolStripMenuItem2_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                Search_DataGridView.Columns[e.ClickedItem.Text].Visible = false;
                Bind_Show_Hide_ToolStrip();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void toolStripMenuItem3_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                Search_DataGridView.Columns[e.ClickedItem.Text].Visible = true;
                Bind_Show_Hide_ToolStrip();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void Show_Hide_ToolStrip_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                Show_Hide_ToolStrip.ShowDropDown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        #endregion

        private void Type_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                    bind(beginDate, endDate, Type_comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

    }
}
