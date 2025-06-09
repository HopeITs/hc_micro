using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Notification_Box : Form
    {
        private DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private bool dragging;
        private Log l;
        private MainForm main_form;
        private MySqlComponents MySS;
        private int seen;
        private UserNotification userNotification;
        public static bool makeSeens;

        public Notification_Box(MainForm mainForm)
        {
            InitializeComponent();
            this.main_form = mainForm;
        }

        private void bind(int User_ID, string Role_ID)
        {
            if (Role_ID == "3") //financial
                MySS.query = @"select MicroProject_ID as 'ID'"
                             + ",P_Name as 'Title'"
                             + ",IF(CAST(Body AS UNSIGNED) = 0,Body,CAST(Body AS UNSIGNED)) as 'Body'"
                             + ",Date as 'Date'"
                             + ",User_ID as 'User_ID'"
                             + ",ID as 'N_ID'"
                             + ",user_notification.Pay_ID as 'Details'"
                             + ",user.UserName as 'Sent By'"
                             + " from `user_notification` left join user on  user_notification.User_ID = user.UserID "
                             + " where user.UserRoleID = " + Role_ID + " and Seen = 0 and Date <= CURDATE() "
                             + " order by Date desc";
            else
                MySS.query = "select MicroProject_ID as 'ID'" +
                             ",P_Name as 'Title'" +
                             ",Body as 'Body'" +
                             ",Date as 'Date'" +
                             ",User_ID as 'User_ID'" +
                             ",ID as 'N_ID'" +
                             ",user_notification.Pay_ID as 'Details'" +
                             ",(select UserName from user where user.UserID = user_notification.Sender_ID)  as 'Sent By'" +
                             " from `user_notification` " +
                             " where User_ID = " + User_ID + " and Seen = 0 and Date < CURDATE() + INTERVAL 4 DAY " +
                             " order by Date desc ";
            //// Date from just 3 Months till NOW-4 days //// Date > CURDATE() + INTERVAL -3 MONTH and 


            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var ds = new DataSet();
            MySS.da = new MySqlDataAdapter(MySS.query, Program.MyConn);
            MySS.da.Fill(ds, "projects");
            Program.MyConn.Close();

            Notification_dataGridView.ColumnHeadersVisible = false;
            Notification_dataGridView.DataSource = ds.Tables[0];
            Notification_dataGridView.Columns["Date"].DefaultCellStyle.Format = "dd-MM-yyyy";

            //body
            Notification_dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (Role_ID == "3") //financial
            {
                Notification_dataGridView.Columns["Body"].DefaultCellStyle.Format = "#,#";
                Notification_dataGridView.Columns["Body"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
            else
            {
                var dgc3 = Notification_dataGridView.Columns["Details"];
                dgc3.Visible = false;
            }

            Notification_dataGridView.ColumnHeadersVisible = true;
            var dgc1 = Notification_dataGridView.Columns["N_ID"];
            dgc1.Visible = false;
            var dgc2 = Notification_dataGridView.Columns["User_ID"];
            dgc2.Visible = false;

            //count rows
            Counter_textBox.Text =
                Notification_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
        }

        private void add_checkBox_Column()
        {
            foreach (DataGridViewColumn col in Notification_dataGridView.Columns) col.ReadOnly = true;
            var colCB = new DataGridViewCheckBoxColumn();
            colCB.Name = "Seen";
            colCB.HeaderText = "Seen";
            colCB.FalseValue = false;
            colCB.TrueValue = true;
            colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Notification_dataGridView.Columns.Add(colCB);

            Notification_dataGridView.Columns["ID"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            Notification_dataGridView.Columns["Date"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            Notification_dataGridView.Columns["Sent By"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            Notification_dataGridView.Columns["Details"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            Notification_dataGridView.Columns["Seen"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            Notification_dataGridView.Columns["Seen"].Width = 30;
            Notification_dataGridView.Columns["ID"].Width = 40;
            Notification_dataGridView.Columns["Sent By"].Width = 80;
            Notification_dataGridView.Columns["Date"].Width = 100;
            Notification_dataGridView.Columns["Title"].Width = 160;
        }

        private void Notification_Box_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.NotificationBox_ToNight(this);
                else
                    newTheme.NotificationBox_ToLight(this);

                MySS = new MySqlComponents();
                userNotification = new UserNotification();
                l = new Log();
                makeSeens = false;

                // make financial //
                if (Program.role == 3)
                    bind(Program.userID, "3");
                else
                    bind(Program.userID, "");

                CheckUserPermission();

                add_checkBox_Column();
                SeenCounter_textBox.Text = seen.ToString();
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
                case 8: //admin l0
                    {
                        applicationToolStripMenuItem.Visible =
                            checklistToolStripMenuItem.Visible =
                            loansToolStripMenuItem.Visible =
                            visitsToolStripMenuItem.Visible = true;
                    }
                    break;
                case 3: //Financial 
                    {
                        checklistToolStripMenuItem.Visible =
                            visitsToolStripMenuItem.Visible = false;
                        applicationToolStripMenuItem.Visible =
                            loansToolStripMenuItem.Visible = true;
                    }
                    break;
                case 2: //Data
                    {
                        checklistToolStripMenuItem.Visible =
                            visitsToolStripMenuItem.Visible =
                        applicationToolStripMenuItem.Visible = true;
                        loansToolStripMenuItem.Visible = false;
                    }
                    break;
                case 4: //Guest 
                case 6: //Out Of Service
                case 7: //Lawful
                case 9: //Media
                    {
                        applicationToolStripMenuItem.Visible = true;
                        checklistToolStripMenuItem.Visible =
                        loansToolStripMenuItem.Visible =
                        visitsToolStripMenuItem.Visible = false;
                    }
                    break;
            }
        }

        private void Notification_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Notification_dataGridView.CurrentRow.Index != null)
                    if (e.ColumnIndex == 8)
                    {
                        var r_index = Notification_dataGridView.CurrentRow.Index;

                        ch1 = Notification_dataGridView.Rows[r_index].Cells["Seen"] as DataGridViewCheckBoxCell;
                        ch1.ValueType = typeof(bool);
                        if (ch1.Value == null)
                        {
                            ch1.Value = true;
                            seen++;
                        }
                        else
                        {
                            ch1.Value = null;
                            seen--;
                        }

                        SeenCounter_textBox.Text = seen.ToString();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Close_button_MouseEnter(object sender, EventArgs e)
        {
            Close_button.BackgroundImage = Resources.Exit_L;
        }

        private void Close_button_MouseLeave(object sender, EventArgs e)
        {
            Close_button.BackgroundImage = Resources.Exit_D;
        }

        private void Notification_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void Notification_Box_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in Notification_dataGridView.Rows)
                {
                    var ch2 = row.Cells["Seen"] as DataGridViewCheckBoxCell;
                    ch2.ValueType = typeof(bool);
                    if (ch2.Value == null) continue;

                    if ((bool)ch2.Value)
                    {
                        var N_ID = row.Cells["N_ID"].Value.ToString();
                        var P_Name = row.Cells["Title"].Value.ToString();
                        var N_Body = row.Cells["Body"].Value.ToString();

                        N_Body = replaceQuotation(N_Body);
                        P_Name = replaceQuotation(P_Name);

                        if (Program.role == 3) //financial
                        {
                            var N_Pay_ID = row.Cells["Details"].Value.ToString();
                            userNotification.Update_NotificationWithPaymentID(int.Parse(N_Pay_ID), "", "", "1");
                        }
                        else
                        {
                            var N_Date = row.Cells["Date"].Value.ToString();
                            var N_MicroProject_ID = row.Cells["ID"].Value.ToString();
                            var N_Pay_ID = row.Cells["Details"].Value.ToString();

                            var oDate = Convert.ToDateTime(N_Date);

                            // clear this notification from other micro users //
                            userNotification.Update_MicroUsers_Notification(int.Parse(N_MicroProject_ID), oDate.ToString("yyyy/MM/dd"), N_Body, P_Name, int.Parse(N_Pay_ID), "1");
                        }
                        makeSeens = true;
                        l.Insert_Log("The Notification[" + N_ID + "]:" + P_Name + "-" + N_Body + ")",
                            "user_notification", Program.UserName, DateTime.Now);
                    }
                    else
                    {
                    }
                }
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Notification_Box_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void Notification_Box_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Notification_Box_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Project_label_Click(object sender, EventArgs e)
        {
        }

        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        private void Check_all_seen_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //Necessary to end the edit mode of the Cell.
                Notification_dataGridView.EndEdit();

                //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
                foreach (DataGridViewRow row in Notification_dataGridView.Rows)
                {
                    DataGridViewCheckBoxCell checkBox = (row.Cells["Seen"] as DataGridViewCheckBoxCell);
                    checkBox.Value = check_all_seen_checkBox.Checked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region context Item right click options 
        private void Notification_dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hti = Notification_dataGridView.HitTest(e.X, e.Y);
                    Notification_dataGridView.ClearSelection();
                    Notification_dataGridView.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(Notification_dataGridView, e.X, e.Y);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void GetSelectedRowIds(DataRow SelectedDataRow, bool with_P_ID)
        {
            MicroProject_ID = -1;
            P_ID = -1;
            P_Name = "";

            if (SelectedDataRow != null)
            {
                if (SelectedDataRow["ID"] == null ||
                    SelectedDataRow["ID"] == DBNull.Value)
                    MicroProject_ID = -1;
                else MicroProject_ID = int.Parse(SelectedDataRow["ID"].ToString());

                if (SelectedDataRow["Title"] == null ||
                    SelectedDataRow["Title"] == DBNull.Value)
                    P_Name = "";
                else P_Name = SelectedDataRow["Title"].ToString();
            }

            if (with_P_ID)
            {
                // check if it is a normal message or it is a notification to get MP_ID , P_Name and P_ID
                string query = @"SELECT person_microproject.Person_ID 
FROM person_microproject LEFT JOIN person on person_microproject.Person_ID = person.P_ID
WHERE person_microproject.MicroProject_ID = " + MicroProject_ID;

                if (P_Name.Contains("ابن/ة")) query += " AND concat(person.P_FirstName,' ',person.P_LastName, ' ابن/ة ', person.P_FatherName) like '" + P_Name + "'";
                else query += " AND concat(person.P_FirstName,' ',person.P_LastName) like '" + P_Name + "'";

                Program.buildConnection();
                MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
                int.TryParse(sc.ExecuteScalar().ToString(), out P_ID);
                Program.MyConn.Close();
            }
        }


        int MicroProject_ID, P_ID;
        string P_Name;

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)Notification_dataGridView.CurrentRow.DataBoundItem).Row;

                GetSelectedRowIds(row, true);

                if (P_ID != -1 && MicroProject_ID != -1)
                {
                    Form application_Form = new Application_Form(P_ID, MicroProject_ID, main_form);
                    main_form.showNewTab(application_Form, "Application", 0);
                }
                else
                    MessageBox.Show("لا يمكن استعراض معلومات هذا السطر");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void checklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)Notification_dataGridView.CurrentRow.DataBoundItem).Row;

                GetSelectedRowIds(row, false);

                if (MicroProject_ID != -1)
                {
                    Form Form = new CheckList_Form(MicroProject_ID, main_form);
                    main_form.showNewTab(Form, "Check List", 0);
                }
                else
                    MessageBox.Show("لا يمكن استعراض معلومات هذا السطر");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void loansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)Notification_dataGridView.CurrentRow.DataBoundItem).Row;

                GetSelectedRowIds(row, false);

                if (MicroProject_ID != -1)
                {
                    Form Form = new Loans_Form(MicroProject_ID, main_form);
                    main_form.showNewTab(Form, "Loans & Payments", 0);
                }
                else
                    MessageBox.Show("لا يمكن استعراض معلومات هذا السطر");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void visitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)Notification_dataGridView.CurrentRow.DataBoundItem).Row;

                GetSelectedRowIds(row, false);

                if (MicroProject_ID != -1)
                {
                    var _Form = new ChooseVisitKind_Form(MicroProject_ID, main_form);
                    _Form.ShowDialog();
                }
                else
                    MessageBox.Show("لا يمكن استعراض معلومات هذا السطر");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        #endregion


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
                    DataTable ex_dt = new DataTable();

                    //defaultView gets the visible rows only//
                    //if (bs.DataSource == null)
                    ex_dt = ((DataTable)Notification_dataGridView.DataSource).DefaultView.ToTable();
                    //else ex_dt = ((DataTable)bs.DataSource).DefaultView.ToTable();

                    //Remove invisible columns//
                    for (var j = 0; j <= Notification_dataGridView.ColumnCount - 1; j++)
                    {
                        if (Notification_dataGridView.Columns[j].Name == "Seen") continue;
                        if (Notification_dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(Notification_dataGridView.Columns[j].Name.ToString());
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

        private void ExportToExcel_button_MouseEnter(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_L;
        }

        private void ExportToExcel_button_MouseLeave(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_D;
        }

    }
}