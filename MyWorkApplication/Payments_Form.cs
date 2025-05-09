using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using Application = Microsoft.Office.Interop.Excel.Application;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;

namespace MyWorkApplication
{
    public partial class Payments_Form : Form
    {
        private Log l; 
        private readonly MainForm main_form;
        private NewTheme NewTheme;
        private UserNotification noti;
        private Select s;
        private bool user_mode;
        private string Month = "";
        private string[] MonthArray = new string[12];

        private string Original_Filter_String = "";
        private BindingSource bs = new BindingSource();

        public Payments_Form(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        private void Payments_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                l = new Log();
                s = new Select();
                noti = new UserNotification();
                bs.ListChanged += bs_ListChanged;
                dataGridView.DoubleBuffered(true);

                if (Settings.Default.theme == "Light") NewTheme.Search_ToLight(this, false);
                else NewTheme.Search_ToNight(this, false);

                //Default view pending//
                Pending_button.BackColor = Color.FromArgb(104, 157, 202);
                Paid_button.BackColor = Color.Transparent;
                ////////////////////////

                user_mode = false;
                Year_comboBox.Items.Clear();
                for (int i = 2018; i <= DateTime.Today.Year + 2; i++)
                {
                    Year_comboBox.Items.Add(i.ToString());
                }
                //Default view This Year//
                Year_comboBox.SelectedItem = DateTime.Now.Year.ToString();
                //////////////////////////

                Group_bind();

                bind_PendingPayments("", (Year_checkBox.Checked ? Year_comboBox.SelectedItem.ToString() : ""), Group_comboBox.Text.ToLower());
                user_mode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_PendingPayments(string month, string year, string Group)
        {
            try
            {
                user_mode = false;

                var paid = "Not Paid";
                if (Paid_button.BackColor == Color.FromArgb(104, 157, 202)) paid = "Paid";
                else paid = "Not Paid";

                DataSet ds1 = new DataSet();

                ds1 = s.Payments_bind(month, year, Group, paid);
                dataGridView.ColumnHeadersVisible = false;
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                //dataGridView.DataSource = ds1.Tables[0];

                bs.DataSource = ds1.Tables[0];
                dataGridView.DataSource = bs;

                SearchToolBar.SetColumns(dataGridView.Columns);

                dataGridView.Columns["تاريخ الاستحقاق"].DefaultCellStyle.Format =
                      dataGridView.Columns["تاريخ الدفع الفعلي"].DefaultCellStyle.Format ="yyyy/MM/dd";
                dataGridView.Columns["القسط"].DefaultCellStyle.Format = "#,##0";

                DataGridViewColumn dgC1;
                if (dataGridView.RowCount > 0 && dataGridView.CurrentRow != null)
                {
                    dgC1 = dataGridView.Columns["Pay_ID"];
                    dgC1.Visible = false;
                }

                dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                    dataGridView.Columns["تاريخ الاستحقاق"].DefaultCellStyle.Alignment =
                        dataGridView.Columns["تاريخ الدفع الفعلي"].DefaultCellStyle.Alignment =
                            dataGridView.Columns["المجموعة"].DefaultCellStyle.Alignment =
                                dataGridView.Columns["القسط"].DefaultCellStyle.Alignment =
                                    DataGridViewContentAlignment.MiddleCenter;

                //count rows
                Count_label.Text = "All: " + dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);

                double sum = 0;
                double.TryParse(ds1.Tables[1].Rows[0][0].ToString(), out sum);

                Sum_label.Text = "Sum: " + sum.ToString("#,##0");
                dataGridView.ColumnHeadersVisible = true;

                dataGridView.ClearSelection();

                user_mode = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Year_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                refreshPageToolStripMenuItem_Click(sender, e);
            }
        }

        private void Group_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_mode)
            { 
                refreshPageToolStripMenuItem_Click(sender, e);
            }
        }

        private void Paid_button_Click(object sender, EventArgs e)
        { 
            Paid_button.BackColor = Color.FromArgb(104, 157, 202);
            Pending_button.BackColor = Color.Transparent;

            refreshPageToolStripMenuItem_Click(sender, e);
        }

        private void Pending_button_Click(object sender, EventArgs e)
        { 
            Pending_button.BackColor = Color.FromArgb(104, 157, 202);
            Paid_button.BackColor = Color.Transparent;

            refreshPageToolStripMenuItem_Click(sender, e);
        }

        private void Year_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Year_checkBox.Checked)
                Year_comboBox.Enabled = true;
            else Year_comboBox.Enabled = false;

            refreshPageToolStripMenuItem_Click(sender, e);
        }

        #region menustrip functions
        private void refreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            try
            {
                Month = string.Join(",", MonthArray.Where(s => !string.IsNullOrEmpty(s)));

                bind_PendingPayments(Month
                    , (Year_checkBox.Checked ? Year_comboBox.SelectedItem.ToString() : "")
                    , Group_comboBox.Text.ToLower());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void clearAllFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control c in panel1.Controls)
                if (c.GetType() == typeof(Button))
                {
                    var btn = c as Button;
                    btn.BackColor = Color.Transparent;
                }

            user_mode = false;
            Month = "";
            // Clear the values of the string array
            Array.Clear(MonthArray, 0, MonthArray.Length);
            Original_Filter_String = "";
            SearchToolBar.Text = "";
            bs.Filter = bs.Sort = "";
            Original_Filter_String = "";
            Group_comboBox.SelectedIndex = -1;
            user_mode = true;

            refreshPageToolStripMenuItem_Click(sender, e);
        }
        #endregion

        private void Group_bind()
        {
            var donorGroup = new DonorGroup();
            var dt_Group = donorGroup.Select("");
            Group_comboBox.DisplayMember = "Name";
            Group_comboBox.ValueMember = "ID";
            Group_comboBox.DataSource = dt_Group;
            Group_comboBox.SelectedIndex = -1;
        }

        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
                //if Guest or Out of service or lawful//
                if (Program.role == 4 ||
                    Program.role == 6 ||
                    Program.role == 7)
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
        

        #region datagridview functions
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = dataGridView.FilterString;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) 
                { MessageBox.Show("please check your network and try again", "Error"); } 
                else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } 
                else { 
                    MessageBox.Show(ex.Message, "Error"); 
                }
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
                                if (C.Name != "DeleteRow")
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

                    //remove last 'or'
                    if (NewFilter.Substring(NewFilter.Length - 4, 4) == " or ")
                        NewFilter = NewFilter.Substring(0, NewFilter.Length - 4);

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
                Count_label.Text = "All: " + dataGridView.Rows.Count.ToString("#,##0");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dataGridView.RowCount > 0 && dataGridView.CurrentRow != null)
                {
                    var SelectedDataRow = ((DataRowView)dataGridView.CurrentRow.DataBoundItem).Row;
                    var MicroProject_ID = -1;

                    if (SelectedDataRow != null)
                    {
                        if (SelectedDataRow["رقم المشروع"] == null || SelectedDataRow["رقم المشروع"] == DBNull.Value)
                            MicroProject_ID = -1;
                        else MicroProject_ID = int.Parse(SelectedDataRow["رقم المشروع"].ToString());
                    }

                    Form Loan_Form = new Loans_Form(MicroProject_ID, main_form);
                    main_form.showNewTab(Loan_Form, "Loans & Payments", 0);
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
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    var SelectedRowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    Selected_label.Text = "Selected: " + SelectedRowCount;

                    if (SelectedRowCount >= 1)
                    {
                        double sum = 0;

                        //IDs_array = new int[SelectedRowCount];
                        for (var i = 0; i < SelectedRowCount; i++)
                        {
                            var SelectedDataRow = ((DataRowView)dataGridView.SelectedRows[i].DataBoundItem).Row;
                            sum += double.Parse(SelectedDataRow["القسط"].ToString());
                        }

                        Sum_label.Text = "Sum: " + sum.ToString("#,##0");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region months button clicks

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[0] == "" || string.IsNullOrEmpty(MonthArray[0]))
                {
                    MonthArray[0] = "01";
                    button1.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[0] = "";
                    button1.BackColor = Color.Transparent;
                } 
                refreshPageToolStripMenuItem_Click(sender, e);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[1] == "" || string.IsNullOrEmpty(MonthArray[1]))
                {
                    MonthArray[1] = "02";
                    button2.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[1] = "";
                    button2.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[2] == "" || string.IsNullOrEmpty(MonthArray[2]))
                {
                    MonthArray[2] = "03";
                    button3.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[2] = "";
                    button3.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[3] == "" || string.IsNullOrEmpty(MonthArray[3]))
                {
                    MonthArray[3] = "04";
                    button4.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[3] = "";
                    button4.BackColor = Color.Transparent;
                }

                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[4] == "" || string.IsNullOrEmpty(MonthArray[4]))
                {
                    MonthArray[4] = "05";
                    button5.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[4] = "";
                    button5.BackColor = Color.Transparent;
                }

                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[5] == "" || string.IsNullOrEmpty(MonthArray[5]))
                {
                    MonthArray[5] = "06";
                    button6.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[5] = "";
                    button6.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[6] == "" || string.IsNullOrEmpty(MonthArray[6]))
                {
                    MonthArray[6] = "07";
                    button7.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[6] = "";
                    button7.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[7] == "" || string.IsNullOrEmpty(MonthArray[7]))
                {
                    MonthArray[7] = "08";
                    button8.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[7] = "";
                    button8.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[8] == "" || string.IsNullOrEmpty(MonthArray[8]))
                {
                    MonthArray[8] = "09";
                    button9.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[8] = "";
                    button9.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[9] == "" || string.IsNullOrEmpty(MonthArray[9]))
                {
                    MonthArray[9] = "10";
                    button10.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[9] = "";
                    button10.BackColor = Color.Transparent;
                }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[10] == "" || string.IsNullOrEmpty(MonthArray[10]))
                {
                    MonthArray[10] = "11";
                    button11.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[10] = "";
                    button11.BackColor = Color.Transparent;
                } 
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (MonthArray[11] == "" || string.IsNullOrEmpty(MonthArray[11]))
                {
                    MonthArray[11] = "12";
                    button12.BackColor = Color.FromArgb(104, 157, 202);
                }
                else
                {
                    MonthArray[11] = "";
                    button12.BackColor = Color.Transparent;
                } 
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


    }
}