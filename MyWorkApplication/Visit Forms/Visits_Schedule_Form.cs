using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;
using Point = System.Drawing.Point;

namespace MyWorkApplication
{
    public partial class Visits_Schedule_Form : Form
    {
        private Log l;
        private Street st;
        private MainForm main_form;
        private NewTheme NewTheme;
        private UserNotification noti;
        private Select s;
        private bool user_mode;
        string V_number = "", V_Kind = "";
        string Street = "", Type = "", Month = "";
        string[] MonthArray = new string[12];

        string Seen = "0";
        private string Original_Filter_String = "";
        BindingSource bs = new BindingSource();

        public Visits_Schedule_Form(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        private void Visits_Schedule_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                l = new Log();
                s = new Select();
                st = new Street();
                noti = new UserNotification();
                if (Settings.Default.theme == "Light") NewTheme.Search_ToLight(this, false);
                else NewTheme.Search_ToNight(this, false);

                MP_dataGridView.DoubleBuffered(true);
                bs.ListChanged += bs_ListChanged;

                user_mode = false;
                Year_comboBox.Items.Clear();
                for (int i = 2018; i <= DateTime.Today.Year + 2; i++)
                {
                    Year_comboBox.Items.Add(i.ToString());
                }
                Bind_All_ComboBoxes();
                Year_comboBox.SelectedItem = DateTime.Now.Year.ToString();

                bind_VisitNotifications(Month, (Year_checkBox.Checked ? Year_comboBox.SelectedItem.ToString() : "")
                    , V_Kind, V_number, Street, Type, Seen);
                user_mode = true;
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
            query += "SELECT ID,Name from microprojecttype order by ID ASC;"; //0 
            query += "SELECT ID, Name from `street` order by Name asc "; //1

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

            Street_comboBox.DataSource = null;
            Street_comboBox.DisplayMember = "Name";
            Street_comboBox.ValueMember = "ID";
            Street_comboBox.DataSource = ds.Tables[1];
            Street_comboBox.SelectedIndex = -1;
            user_mode = true;
        }

        private void bind_VisitNotifications(string month, string year, string V_Kind, string V_number, string Street, string Type, string Seen)
        {
            try
            {
                var dt = noti.Select_VisitNotifications(month, year, V_Kind, V_number, Street, Type, Seen);
                MP_dataGridView.ColumnHeadersVisible = false;
                MP_dataGridView.DataSource = null;
                MP_dataGridView.Columns.Clear();

                bs.DataSource = dt;
                MP_dataGridView.DataSource = bs;

                MP_dataGridView.Columns["التاريخ"].DefaultCellStyle.Format = "yyyy/MM/dd";

                DataGridViewColumn dgC1;
                if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
                {
                    dgC1 = MP_dataGridView.Columns["ID"];
                    dgC1.Visible = false;

                    dgC1 = MP_dataGridView.Columns["Pay_ID"];
                    dgC1.Visible = false;
                }

                MP_dataGridView.Columns["رقم المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["التاريخ"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["نوع الزيارة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["منطقة المشروع"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الموبايل"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["تاريخ الدفعة المستحقة"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["الممول"].DefaultCellStyle.Alignment =
                    MP_dataGridView.Columns["حالة الزيارة"].DefaultCellStyle.Alignment
                    = DataGridViewContentAlignment.MiddleCenter;

                //count rows
                Count_label.Text = "All: " + MP_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);

                var colCB = new DataGridViewButtonColumn();
                colCB.Name = "DeleteRow";
                colCB.HeaderText = "";
                colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colCB.FlatStyle = FlatStyle.Flat;
                MP_dataGridView.Columns.Add(colCB);

                MP_dataGridView.Columns["DeleteRow"].Width = 45;

                SearchToolBar.SetColumns(MP_dataGridView.Columns);

                MP_dataGridView.ColumnHeadersVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Month = string.Join(",", MonthArray.Where(s => !string.IsNullOrEmpty(s)));

                bind_VisitNotifications(Month
                    , (Year_checkBox.Checked ? Year_comboBox.SelectedItem.ToString() : "")
                    , V_Kind, V_number, Street, Type, Seen);
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

            Original_Filter_String = "";
            SearchToolBar.Text = "";
            V_Kind = V_number = Street = Type = Month = "";
            // Clear the values of the string array
            Array.Clear(MonthArray, 0, MonthArray.Length);

            VisitNumber_comboBox.SelectedIndex = Visits_comboBox.SelectedIndex
                = Street_comboBox.SelectedIndex = Type_comboBox.SelectedIndex = -1;

            bs.Filter = bs.Sort = "";
            Original_Filter_String = "";

            user_mode = true;

            refreshPageToolStripMenuItem_Click(sender, e);
        }

        #region datagridview functions
        private void dataGridView_FilterStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.FilterEventArgs e)
        {
            try
            {
                Original_Filter_String = MP_dataGridView.FilterString;
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
                Count_label.Text = "All: " + MP_dataGridView.Rows.Count.ToString("#,##0");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void MP_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var SelectedRowCount = MP_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                Selected_label.Text = "Selected: " + SelectedRowCount;
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
        private void MP_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == MP_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == MP_dataGridView.Columns["DeleteRow"].Index)
                {
                    Image image = null;
                    if (Deleted_checkBox.Checked)
                        image = Resources.Undo24;
                    else image = Resources.OK24;

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
        private void MP_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Check if click is on specific column 
                if (e.ColumnIndex == MP_dataGridView.Columns["DeleteRow"].Index)
                {
                    string message = "";
                    string seen = "";

                    if (Deleted_checkBox.Checked)
                    {
                        message = "هل أنت متأكد أنك تريد استعادة هذا الموعد؟";
                        seen = "0";
                    }
                    else
                    {
                        message = "هل أنت متأكد أنك تريد حذف هذا الموعد؟";
                        seen = "1";
                    }

                    var dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int ID = Convert.ToInt32(MP_dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());

                        string P_Name = MP_dataGridView.Rows[e.RowIndex].Cells["المستفيد"].Value.ToString();
                        string Visit = MP_dataGridView.Rows[e.RowIndex].Cells["نوع الزيارة"].Value.ToString();
                        string Date = MP_dataGridView.Rows[e.RowIndex].Cells["التاريخ"].Value.ToString();
                        int MP_ID = Convert.ToInt32(MP_dataGridView.Rows[e.RowIndex].Cells["رقم المشروع"].Value.ToString());
                        int Pay_ID = Convert.ToInt32(MP_dataGridView.Rows[e.RowIndex].Cells["Pay_ID"].Value.ToString());

                        DateTime Date_2 = Convert.ToDateTime(Date);

                        noti.Update_MicroUsers_Notification(MP_ID, Date_2.ToString("yyyy/MM/dd"), Visit, P_Name, Pay_ID, seen);
                        l.Insert_Log(
                            "Set seen=" + seen + " to Notification[" + ID + "] of " + Visit + ":" + P_Name + "-" + MP_ID,
                            "user_notification", Program.UserName, DateTime.Now);

                        refreshPageToolStripMenuItem_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

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
                    //defaultView gets the visible rows only//  
                    if (bs.DataSource == null)
                        ex_dt = ((DataTable)MP_dataGridView.DataSource).DefaultView.ToTable();
                    else ex_dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    ////Remove invisible columns//
                    //for (var j = MP_dataGridView.ColumnCount - 1; j >= 0; j--)
                    //    if (MP_dataGridView.Columns[j].Visible == false)
                    //        ex_dt.Columns.RemoveAt(j);
                    for (var j = 0; j <= MP_dataGridView.ColumnCount - 1; j++)
                    {
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

        private void Year_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshPageToolStripMenuItem_Click(sender, e);
        }

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

                //Month = "01";
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button1") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }

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

                //Month = "02"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button2") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                ////Month = "03";  
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button3") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }

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
                //Month = "04"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button4") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }

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
                //Month = "05"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button5") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }

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
                //Month = "06"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button6") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                ////Month = "07"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button7") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                //Month = "08";  
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button8") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                ////Month = "09"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button9") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                //Month = "10"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button10") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                //Month = "11";
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button11") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
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
                //Month = "12"; 
                //foreach (Control c in panel1.Controls)
                //    if (c.GetType() == typeof(Button))
                //    {
                //        var btn = c as Button;
                //        if (btn.Name == "button12") btn.BackColor = Color.FromArgb(104, 157, 202);
                //        else btn.BackColor = Color.Transparent;
                //    }
                refreshPageToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void Year_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (Year_checkBox.Checked)
                    Year_comboBox.Enabled = true;
                else Year_comboBox.Enabled = false;

                refreshPageToolStripMenuItem_Click(sender, e);
            }
        }

        private void Deleted_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Deleted_checkBox.Checked) Seen = "1";
            else Seen = "0";

            refreshPageToolStripMenuItem_Click(sender, e);
        }

        private void VisitNumber_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    // Check V_Number //
                    if (VisitNumber_comboBox.SelectedIndex == -1 || VisitNumber_comboBox.SelectedIndex == 0)
                        V_number = "";
                    else V_number = VisitNumber_comboBox.SelectedItem.ToString();

                    // Check V_Kind //
                    if (Visits_comboBox.SelectedIndex == -1 || Visits_comboBox.SelectedIndex == 0)
                        V_Kind = "";
                    else
                    {
                        if (Visits_comboBox.SelectedItem.ToString() == "مركبات")
                            V_Kind = "V";
                        else V_Kind = "O";
                    }

                    // Check Street //
                    if (Street_comboBox.Text == "")
                        Street = "";
                    else Street = Street_comboBox.SelectedValue.ToString();

                    // Check Type //
                    if (Type_comboBox.Text == "")
                        Type = "";
                    else Type = Type_comboBox.SelectedValue.ToString();

                    refreshPageToolStripMenuItem_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}