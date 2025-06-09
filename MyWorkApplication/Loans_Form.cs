using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Loans_Form : Form
    {
        private string SubCategory = "";
        private int SubCategory_ID = -1;
        private readonly List<int> Communication_IDs = new List<int>();
        private DateTimePicker dateTimePicker1;
        private DataTable dt_Group, dt_Donor;
        private DateTime dueTo_date, Receive_date;
        private readonly List<int> Financial_IDs = new List<int>();
        private Log l;
        //private int late_num;
        private readonly MainForm mainForm;
        private int MicroProject_ID, Loan_ID, Payment_ID;
        private MicroProject mp;
        private MySqlComponents MySS;
        private string pay, Loan, returned_amount;
        private double Return_amount;
        private Select s;
        private TasksOfProjects TasksOfProjects;
        private bool Update_Mood;
        private readonly List<int> User_IDs = new List<int>();
        private UserNotification userNotification;
        private string type, fundtype;

        public Loans_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            MicroProject_ID = -1;
        }

        public Loans_Form(int MicroProject_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
        }

        private void LoanPayments_Form_Load(object sender, EventArgs e)
        {
            try
            {
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Loans and Payments";

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                userNotification = new UserNotification();
                mp = new MicroProject();

                CheckUserPermission();

                //////////////////// handle auto scrolling for all comboBoxes //////////////////////
                Group_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                Loan_ID = -1;
                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "Yes");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "Yes");
                Group_bind();

                dateTimePicker1 = new DateTimePicker();
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd/MM/yyyy";
                dateTimePicker1.Visible = false;
                var oRectangle = Payment_dataGridView.GetCellDisplayRectangle(3, 0, true);
                // Setting area for dateTimePicker1.
                dateTimePicker1.Size = new Size(oRectangle.Width, oRectangle.Height);
                // Setting location for dateTimePicker1.
                dateTimePicker1.Location = new Point(oRectangle.X, oRectangle.Y);

                Payment_dataGridView.Controls.Add(dateTimePicker1);
                dateTimePicker1.ValueChanged += DateTimePickerChange;

                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                {
                    newTheme.Loan_ToNight(this);
                    dateTimePicker1.BackColor = Color.FromArgb(75, 75, 75);
                    dateTimePicker1.ForeColor = Color.White;
                }
                else newTheme.Loan_ToLight(this);

                Payment_dataGridView.DoubleBuffered(true);
                //Payment_dataGridView.Focus();

                var user_db = s.select_M_E();
                if (user_db != null)
                    for (var i = 0; i < user_db.Rows.Count; i++)
                        User_IDs.Add(user_db.Rows[i].Field<int>(0));
                var com_db = s.select_Communication();
                if (com_db != null)
                    for (var i = 0; i < com_db.Rows.Count; i++)
                        Communication_IDs.Add(com_db.Rows[i].Field<int>(0));
                var financial_db = s.select_Financial();
                if (financial_db != null)
                    for (var i = 0; i < financial_db.Rows.Count; i++)
                        Financial_IDs.Add(financial_db.Rows[i].Field<int>(0));

                ///// Select From Payments Form /////
                if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox2_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void insertPayment(int Loan_ID, string pay_amount, DateTime Pay_DueDate, string Pay_IsPaid,
            DateTime Pay_RecievedOnDate, string pay_note)
        {

            Program.buildConnection();

            pay = pay_amount.Replace(",", "");
            MySS.query =
                "Insert Into `payment`(`Pay_Amount`, `Pay_DueDate`, `Pay_IsPaid`, `Loan_ID`, `Pay_RecievedOnDate`,`Pay_Note`) values("
                + pay
                + ",N'" + Pay_DueDate.Year + "/" + Pay_DueDate.Month + "/" + Pay_DueDate.Day + "',N'"
                + Pay_IsPaid + "',"
                + Loan_ID + ",N'"
                + (Pay_IsPaid == "Paid"
                    ? Pay_RecievedOnDate.Year + "/"
                                              + Pay_RecievedOnDate.Month + "/"
                                              + Pay_RecievedOnDate.Day
                    : Pay_DueDate.Year + "/"
                                       + Pay_DueDate.Month + "/"
                                       + Pay_DueDate.Day) + "',N'"
                + pay_note + "' ) ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void updatePayment(int P_ID, string pay_amount, DateTime Pay_DueDate, string Pay_IsPaid,
            DateTime Pay_RecievedOnDate, string pay_note)
        {

            Program.buildConnection();

            var pay = pay_amount.Replace(",", "");
            MySS.query = "update `payment` set " +
                         "Pay_Amount = " + double.Parse(pay) + "," +
                         "Pay_DueDate = N'" + Pay_DueDate.Year + "/" + Pay_DueDate.Month + "/" + Pay_DueDate.Day +
                         "'," +
                         "Pay_IsPaid = N'" + Pay_IsPaid + "'," +
                         "Pay_RecievedOnDate =N' " + (Pay_IsPaid == "Paid"
                             ? Pay_RecievedOnDate.Year + "/"
                                                       + Pay_RecievedOnDate.Month + "/"
                                                       + Pay_RecievedOnDate.Day
                             : Pay_DueDate.Year + "/"
                                                + Pay_DueDate.Month + "/"
                                                + Pay_DueDate.Day) + "', " +
                         "Pay_Note = N'" + pay_note + "' " +
                         "where Pay_ID = " + P_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private int getCurrentPaymentID()
        {

            Program.buildConnection();
            MySS.query = "select MAX(`Pay_ID`) from `payment`  ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var ID = int.Parse(MySS.sc.ExecuteScalar().ToString());
            Program.MyConn.Close();
            return ID;
        }

        private int Insert_Loan(int MP_ID)
        {
            double rate, count;

            Loan = LoanAmount_textBox.Text.Replace(",", "");
            pay = PaymentAmount_textBox.Text.Replace(",", "");
            returned_amount = ReturnedAmount1_textBox.Text.Replace(",", "");

            if (PaymentAmount_textBox.Text == "" || PaymentAmount_textBox.Text == "0") pay = "0";

            if (ReturnedAmount1_textBox.Text == "" || ReturnedAmount1_textBox.Text == "0") returned_amount = "0";

            if (Rate_textBox.Text == "" || Rate_textBox.Text == "0") rate = 0;
            else rate = Convert.ToDouble(Rate_textBox.Text);

            if (PaymentsCount_label.Text == "" || PaymentsCount_label.Text == "0") count = 0;
            else count = Convert.ToDouble(PaymentsCount_label.Text);

            MySS.query =
                " START TRANSACTION; "
                + "Insert Into `loan`(`Loan_Amount`, `Loan_DateTaken`, `Loan_PaymentsCount`,`Loan_Rate`,`Loan_ReturnedAmount`,`Loan_PaymentsAmount`,`Loan_Note`,`Loan_ReceiptID`, `MicroProject_ID`) values("
                + Loan + ",N'"
                + LoanDate_DateTimePicker.Value.Year + "/"
                + LoanDate_DateTimePicker.Value.Month + "/"
                + LoanDate_DateTimePicker.Value.Day + "',"
                + count + ","
                + rate + ","
                + returned_amount + ","
                + pay + ",N'"
                + Note_textBox.Text + "',N'"
                + ReceiptNo_textBox.Text + "',"
                + MP_ID + " );"
                + " SET @loan_id = Last_Insert_ID();"
                + " SELECT @loan_id;"
                + " COMMIT;";

            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            //MySS.sc.ExecuteNonQuery();
            int id = Convert.ToInt32(MySS.sc.ExecuteScalar());
            Program.MyConn.Close();
            return id;
        }

        private void Update_Loan(int L_ID)
        {

            double rate, count;
            var Loan = LoanAmount_textBox.Text.Replace(",", "");
            pay = PaymentAmount_textBox.Text.Replace(",", "");
            returned_amount = ReturnedAmount1_textBox.Text.Replace(",", "");

            if (PaymentAmount_textBox.Text == "" || PaymentAmount_textBox.Text == "0") pay = "0";
            if (ReturnedAmount1_textBox.Text == "" || ReturnedAmount1_textBox.Text == "0") returned_amount = "0";

            if (Rate_textBox.Text == "" || Rate_textBox.Text == "0") rate = 0;
            else rate = Convert.ToDouble(Rate_textBox.Text);

            if (PaymentsCount_label.Text == "" || PaymentsCount_label.Text == "0") count = 0;
            else count = Convert.ToDouble(PaymentsCount_label.Text);

            MySS.query = " Update `loan` set " +
                         "`Loan_Amount` = " + double.Parse(Loan) +
                         ",`Loan_DateTaken` = N'" + LoanDate_DateTimePicker.Value.ToString("yyyy/MM/dd") + "'" +
                         ",`Loan_PaymentsCount` = " + count +
                         ",`Loan_Rate` = " + rate +
                         ",`Loan_ReturnedAmount` = " + returned_amount +
                         ",`Loan_PaymentsAmount` = " + pay +
                         ",`Loan_Note` = N'" + Note_textBox.Text + "'" +
                         ",`Loan_ReceiptID` = N'" + ReceiptNo_textBox.Text + "' " +

                         " where `Loan_ID` = " + L_ID + " ";


            Program.buildConnection();
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void Delete_Loan(int L_ID)
        {

            Program.buildConnection();
            MySS.query = "delete from `loan` where `Loan_ID` = " + L_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private int getCurrentLoanID()
        {
            Program.buildConnection();
            MySS.query = "select MAX(`Loan_ID`) from `loan` ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var id = int.Parse(MySS.sc.ExecuteScalar().ToString());
            Program.MyConn.Close();
            return id;
        }

        private void Calculate_RemainedAmount(int LoanID)
        {
            decimal paid_pay_count, remain_Pay_Count, all_pay_count;
            double Paid_amount, Remain_Amount;

            // المبلغ الكلي
            var Amount = LoanAmount_textBox.Text.Replace(",", "");
            var loan_amount = Convert.ToDouble(Amount);

            if (PaymentAmount_textBox.Text == ""
             || PaymentAmount_textBox.Text == "0"
             || ReturnedAmount1_textBox.Text == ""
             || ReturnedAmount1_textBox.Text == "0") return;

            // القسط
            Amount = PaymentAmount_textBox.Text.Replace(",", "");
            var payment_amount = Convert.ToDouble(Amount);

            // المبلغ المطلوب استرداده
            Amount = ReturnedAmount1_textBox.Text.Replace(",", "");
            Return_amount = Convert.ToDouble(Amount);

            // النسبة
            var rate = Return_amount * 100 / loan_amount;

            ////// النسبة
            ////Double rate = Convert.ToDouble(Rate_textBox.Text.ToString()); 
            ////// المبلغ المطلوب استرداده
            ////Return_amount = loan_amount * rate / 100;

            // عدد الدفعات الكلي //
            all_pay_count = (decimal)Return_amount / (decimal)payment_amount;

            var loan_dt = s.Select_Loan_All_Data(LoanID.ToString(), "");
            if (loan_dt.Rows.Count != 0)
            {
                //// عدد الدفعات المدفوعة //
                if (loan_dt.Rows[0]["عدد الدفعات المدفوعة"] == null ||
                    loan_dt.Rows[0]["عدد الدفعات المدفوعة"] == DBNull.Value) paid_pay_count = 0;
                else paid_pay_count = Convert.ToDecimal(loan_dt.Rows[0]["عدد الدفعات المدفوعة"]);

                // مجموع المبالغ المدفوعة //
                if (loan_dt.Rows[0]["المبلغ المدفوع"] == null || loan_dt.Rows[0]["المبلغ المدفوع"] == DBNull.Value)
                    Paid_amount = 0;
                else Paid_amount = Convert.ToDouble(loan_dt.Rows[0]["المبلغ المدفوع"]);

                // المتبقي من المبلغ المسترد //
                Remain_Amount = Return_amount - Paid_amount;

                // عدد الدفعات المتبقية
                //remain_Pay_Count = Convert.ToInt32(all_pay_count - paid_pay_count);
                remain_Pay_Count = Convert.ToDecimal(Remain_Amount / payment_amount);


                ////fill textboxes////
                PaidAmount_label.Text = Paid_amount.ToString("#,##0");
                RemainOfReturned_label.Text = Remain_Amount.ToString("#,##0");
                //تلوين في حال كان المبلغ المتبقي اصغر من مبلغ قسط ونصف القسط
                if (Remain_Amount < (payment_amount + 0.5 * payment_amount))
                {
                    RemainOfReturned_label.BackColor = Color.FromArgb(190, 23, 23);
                    RemainOfReturned_label.ForeColor = Color.White;
                }
                else
                {
                    RemainOfReturned_label.BackColor = Color.Transparent;
                    if (Settings.Default.theme == "Dark")
                        RemainOfReturned_label.ForeColor = Color.WhiteSmoke;
                    else RemainOfReturned_label.ForeColor = Color.Black;
                }
                /////////////////////////////////////////////////////////////////////

                PaidPaymentsCount_label.Text = paid_pay_count.ToString("N1");
                RemainOfPaymentsCount_label.Text = remain_Pay_Count.ToString("N1");
            }
            else
            {
                PaidAmount_label.Text = "0";
                RemainOfReturned_label.Text = "0";
                PaidPaymentsCount_label.Text = "0";
                RemainOfPaymentsCount_label.Text = "المتبقية = " + "0";
            }

            ReturnedAmount2_label.Text = Return_amount.ToString("#,##0");
            PaymentsCount_label.Text = all_pay_count.ToString("N1");
            Rate_textBox.Text = rate.ToString("N1");
        }

        private void Payment_bind(int L_ID)
        {

            Program.buildConnection();

            MySS.query = "select Pay_ID as 'ID'" +
                         ",Pay_Amount as 'Amount'" +
                         ",Pay_DueDate as 'Pay Date'" +
                         ",Pay_IsPaid as 'State'" +
                         ",Pay_RecievedOnDate as 'Actual Pay Date'" +
                         ",Pay_Note as 'Pay Note'" +
                         ",Loan_ID as 'Loan_ID'" +
                         " from `payment` ";
            var condition = " ";
            condition = " where Loan_ID = " + L_ID + " ";
            MySS.query += condition;

            var sc = new MySqlCommand(MySS.query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);

            Program.MyConn.Close();
            Payment_dataGridView.Rows.Clear();

            //late_num = 0;
            if (dt != null)
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    Payment_dataGridView.Rows.Add();
                    // get the data of 1 row
                    var Paid = "Not Paid";

                    Payment_dataGridView.Rows[i].Cells["Pay_ID"].Value = dt.Rows[i].Field<int>("ID").ToString();
                    Payment_ID = Convert.ToInt32(Payment_dataGridView.Rows[i].Cells["Pay_ID"].Value);

                    Payment_dataGridView.Rows[i].Cells["Pay_Loan_ID"].Value =
                        dt.Rows[i].Field<int>("Loan_ID").ToString();
                    Loan_ID = Convert.ToInt32(Payment_dataGridView.Rows[i].Cells["Pay_Loan_ID"].Value);

                    if (dt.Rows[i].Field<string>("Pay Note") == null)
                        Payment_dataGridView.Rows[i].Cells["Pay_Note"].Value = "";
                    else Payment_dataGridView.Rows[i].Cells["Pay_Note"].Value = dt.Rows[i].Field<string>("Pay Note");

                    if (dt.Rows[i].Field<float>("Amount") == null ||
                        dt.Rows[i].Field<float>("Amount").ToString() == null)
                        Payment_dataGridView.Rows[i].Cells["Pay_Amount"].Value = 0;
                    else
                        Payment_dataGridView.Rows[i].Cells["Pay_Amount"].Value =
                            dt.Rows[i].Field<float>("Amount").ToString();

                    Paid = dt.Rows[i].Field<string>("State");
                    var chkchecking = Payment_dataGridView.Rows[i].Cells["Pay_IsPaid"] as DataGridViewCheckBoxCell;
                    chkchecking.ValueType = typeof(bool);
                    if (Paid == "Paid")
                        chkchecking.Value = true;
                    else chkchecking.Value = false;

                    Payment_dataGridView.Rows[i].Cells["Pay_DueDate"].Value =
                        dt.Rows[i].Field<DateTime>("Pay Date").Date;

                    Payment_dataGridView.Rows[i].Cells["Pay_RecievedOnDate"].Value =
                        dt.Rows[i].Field<DateTime>("Actual Pay Date").Date;

                    Payment_dataGridView.Rows[i].Cells["P_InDataBase"].Value = "1";

                    var realDate = (DateTime)Payment_dataGridView.Rows[i].Cells["Pay_RecievedOnDate"].Value;
                    var dueDate = (DateTime)Payment_dataGridView.Rows[i].Cells["Pay_DueDate"].Value;
                    //if ((realDate - dueDate).Days > 10)
                    //    late_num++;
                }

            Payment_dataGridView.Columns["Pay_RecievedOnDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            Payment_dataGridView.Columns["Pay_DueDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

        }

        private void Payment_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == Payment_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == Payment_dataGridView.Columns["P_DeleteRow"].Index)
                {
                    CheckUserPermission();
                    var dialogResult = MessageBox.Show("Are you sure you want to delete ?", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (Payment_dataGridView.Rows[e.RowIndex].Cells["Pay_ID"].Value != null)
                        {
                            var P_ID = Convert.ToInt32(Payment_dataGridView.Rows[e.RowIndex].Cells["Pay_ID"].Value
                                .ToString());
                            var L_ID = Convert.ToInt32(Payment_dataGridView.Rows[e.RowIndex].Cells["Pay_Loan_ID"].Value
                                .ToString());


                            Program.buildConnection();
                            MySS.query = "delete from `payment` where Pay_ID = " + P_ID + " ";
                            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
                            MySS.sc.ExecuteNonQuery();
                            Program.MyConn.Close();

                            l.Insert_Log("Delete Payment of : " + MicroProject_ID + " ", "Payment",
                                Program.UserName, DateTime.Now);

                            Payment_dataGridView.Rows.RemoveAt(e.RowIndex);
                            Calculate_RemainedAmount(L_ID);
                        }
                        else
                        {
                            Payment_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Payment_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == Payment_dataGridView.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == Payment_dataGridView.Columns["P_DeleteRow"].Index)
            {
                Image image = null;
                if (Settings.Default.theme == "Dark")
                    image = Resources.KAKA_Alii_D;
                else
                    image = Resources.KAKA_Alii;
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                var size = image.Size;
                var location = new Point((e.CellBounds.Width - size.Width) / 2,
                    (e.CellBounds.Height - size.Height) / 2);
                location.Offset(e.CellBounds.Location);
                e.Graphics.DrawImage(image, location);
                e.Handled = true;
            }
        }

        private void Payment_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void DateTimePickerChange(object sender, EventArgs e)
        {
            Payment_dataGridView.CurrentCell.Value = dateTimePicker1.Text;
        }

        private void DateTimePickerClose(object sender, EventArgs e)
        {
            Payment_dataGridView.CurrentCell.Value = dateTimePicker1.Text;
            dateTimePicker1.Visible = false;
        }

        private void DeleteLoan_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();
                if (Loan_ID == -1)
                    throw new InvalidOperationException("Please choose the loan you want to delete");
                var dialogResult =
                    MessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Delete_Loan(Loan_ID);
                    l.Insert_Log("Delete Loan of : " + MicroProject_ID + " ", "Loan", Program.UserName,
                        DateTime.Now);
                    Loan_ID = -1;

                    userNotification.Delete_Notification(
                          MicroProject_ID
                        , ""
                        , "Visit Opening-" + SubCategory
                        , Person_Name_textBox.Text
                        , -21);

                    //////////////////   task = تسليم كامل المبلغ و تحديد الأقساط ///////////
                    string task = "تسليم كامل المبلغ و تحديد الأقساط";
                    TasksOfProjects = new TasksOfProjects();
                    TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, task, false, DateTime.Now);

                    clear_Boxes();

                    Update_Mood = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SavePayments_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();
                for (var i = 0; i < Payment_dataGridView.RowCount - 1; i++)
                {
                    var Pay_Amount = "0";
                    var Paid = "Not Paid";
                    var Note = "";

                    if (Payment_dataGridView.Rows[i].Cells["Pay_Note"].Value == null)
                        Note = "";
                    else Note = Payment_dataGridView.Rows[i].Cells["Pay_Note"].Value.ToString();

                    Pay_Amount = Payment_dataGridView.Rows[i].Cells["Pay_Amount"].Value.ToString();
                    Pay_Amount = Pay_Amount.Replace(",", "");

                    var chkchecking = Payment_dataGridView.Rows[i].Cells["Pay_IsPaid"] as DataGridViewCheckBoxCell;
                    chkchecking.ValueType = typeof(bool);

                    if (Convert.ToBoolean(chkchecking.Value))
                        Paid = "Paid";
                    else Paid = "Not Paid";

                    dueTo_date = (DateTime)Payment_dataGridView.Rows[i].Cells["Pay_DueDate"].Value;
                    if (Payment_dataGridView.Rows[i].Cells["Pay_RecievedOnDate"].Value == null)
                        Receive_date = dueTo_date;
                    else Receive_date = (DateTime)Payment_dataGridView.Rows[i].Cells["Pay_RecievedOnDate"].Value;

                    if (Payment_dataGridView.Rows[i].Cells["P_InDataBase"].Value == null) //insert new payment
                    {
                        insertPayment(Loan_ID, Pay_Amount, dueTo_date, Paid, Receive_date, Note);
                        l.Insert_Log("insert Payment to: " + MicroProject_ID + " ", "Payment",
                            Program.UserName, DateTime.Now);
                        Payment_ID = getCurrentPaymentID();
                        Payment_dataGridView.Rows[i].Cells["Pay_Loan_ID"].Value = Loan_ID;
                        Payment_dataGridView.Rows[i].Cells["Pay_ID"].Value = Payment_ID;
                        if (Paid == "Not Paid")
                            foreach (var u in Financial_IDs)
                                userNotification.Insert_UserNotification(dueTo_date.ToString("yyyy/MM/dd"), pay, Person_Name_textBox.Text,
                                    MicroProject_ID, u,
                                    Program.userID, Payment_ID);
                        Payment_dataGridView.Rows[i].Cells["P_InDataBase"].Value = "1";
                    }
                    else //update payment
                    {
                        Loan_ID = Convert.ToInt32(Payment_dataGridView.Rows[i].Cells["Pay_Loan_ID"].Value.ToString());
                        Payment_ID = Convert.ToInt32(Payment_dataGridView.Rows[i].Cells["Pay_ID"].Value.ToString());

                        updatePayment(Payment_ID, Pay_Amount, dueTo_date, Paid, Receive_date, Note);
                        l.Insert_Log("Update payment of: " + MicroProject_ID + " ", "Payment",
                            Program.UserName, DateTime.Now);

                        int seen0 = 0;
                        if (Paid == "Not Paid")
                        {
                            seen0 = 0;
                        }
                        else
                        {
                            seen0 = 1;
                        }
                        userNotification.Update_NotificationWithPaymentID(Payment_ID
                            , Pay_Amount
                            , dueTo_date.ToString("yyyy/MM/dd")
                            , seen0.ToString());

                        Payment_dataGridView.Rows[i].Cells["P_InDataBase"].Value = "1";
                    }
                }

                //update loan details //update all payments number//
                Update_Loan(Loan_ID);
                Calculate_RemainedAmount(Loan_ID);

                string Paid1, Paid2;
                //check if last payment has paid and there is no other payments
                if (Payment_dataGridView.RowCount > 1)
                {
                    //check if last payment has paid and there is no other payments//
                    /////////////////////////////////////////////////////////////////
                    var ch2 = Payment_dataGridView.Rows[Payment_dataGridView.RowCount - 2].Cells["Pay_IsPaid"] as DataGridViewCheckBoxCell;
                    ch2.ValueType = typeof(bool);
                    if (Convert.ToBoolean(ch2.Value))
                        Paid2 = "Paid";
                    else Paid2 = "Not Paid";

                    //Paid1 == "Paid" && Paid2 == "Paid"  

                    // => اذا كان المتبقي اقل من نص دفعة
                    if (Paid2 == "Paid" && RemainOfReturned_label.BackColor == Color.FromArgb(190, 23, 23))
                    {
                        Send_Trophy_Notifications();
                    }
                }

                ////////////////////////////////////////////////////////////////////
                MessageBox.Show("Payments saved successfuly");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Send_Trophy_Notifications()
        {
            var dialogResult = MessageBox.Show("سيتم إرسال إشعار بالزيارة الختامية للمشروع، هل أنت متأكد من أن المستفيد انتهى من دفع أقساطه؟", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //زيارة الختام بعد أسبوع من تاريخ آخر دفعة
                var Final_Visit_date = (DateTime)Payment_dataGridView.Rows[Payment_dataGridView.RowCount - 2]
                .Cells["Pay_RecievedOnDate"].Value;
                Final_Visit_date = Final_Visit_date.AddDays(7);

                //Send to M&E//
                if (User_IDs.Count == 0)
                    throw new InvalidOperationException("لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");

                foreach (var u in User_IDs)
                    userNotification.Insert_UserNotification(Final_Visit_date.ToString("yyyy/MM/dd"), "Visit Closing-" + SubCategory,
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);

                //Send to Conmunication//
                if (Communication_IDs.Count == 0)
                    throw new InvalidOperationException("لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");

                foreach (var u in Communication_IDs)
                    userNotification.Insert_UserNotification(Final_Visit_date.ToString("yyyy/MM/dd"), "Visit (Trophy)" + SubCategory,
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -10);

                l.Insert_Log("Insert Closing Visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Loan", Program.UserName, DateTime.Now);
            }
        }

        private void Payment_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (Payment_dataGridView.CurrentCell.ColumnIndex == 3)
                {
                    dateTimePicker1.Location = Payment_dataGridView
                        .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                    dateTimePicker1.Visible = true;
                    if (Payment_dataGridView.CurrentCell.Value != DBNull.Value &&
                        Payment_dataGridView.CurrentCell.Value != null)
                        dateTimePicker1.Value = (DateTime)Payment_dataGridView.CurrentCell.Value;
                    else
                        dateTimePicker1.Value = DateTime.Today;
                }
                else if (Payment_dataGridView.CurrentCell.ColumnIndex == 4)
                {
                    dateTimePicker1.Location = Payment_dataGridView
                        .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                    dateTimePicker1.Visible = true;
                    if (Payment_dataGridView.CurrentCell.Value != DBNull.Value &&
                        Payment_dataGridView.CurrentCell.Value != null)
                        dateTimePicker1.Value = (DateTime)Payment_dataGridView.CurrentCell.Value;
                    else
                        dateTimePicker1.Value = DateTime.Today;
                }
                else
                {
                    dateTimePicker1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Payment_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Payment_dataGridView.Focused && Payment_dataGridView.CurrentCell.ColumnIndex == 3)
                {
                    Payment_dataGridView.CurrentCell.Value = dateTimePicker1.Value.Date;
                    Payment_dataGridView.CurrentCell.Style.Format = "dd/MM/yyyy";
                    dueTo_date = dateTimePicker1.Value;
                }
                else if (Payment_dataGridView.Focused && Payment_dataGridView.CurrentCell.ColumnIndex == 4)
                {
                    Payment_dataGridView.CurrentCell.Value = dateTimePicker1.Value.Date;
                    Payment_dataGridView.CurrentCell.Style.Format = "dd/MM/yyyy";
                    Receive_date = dateTimePicker1.Value;
                }
                else if (Payment_dataGridView.Focused && Payment_dataGridView.CurrentCell.ColumnIndex == 2)
                {
                    if (Payment_dataGridView.CurrentCell.Value.ToString() != "")
                        Payment_dataGridView.CurrentCell.Value =
                            Regex.Replace(
                                string.Format("{0:n" + 4 + "}",
                                    Convert.ToDecimal(Payment_dataGridView.CurrentCell.Value.ToString())),
                                @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
                }
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
                case 8: //Admin_L0 
                    {
                        DeleteLoan_button.Visible = true;
                        break;
                    }
                case 3: //Financial
                case 7: //Lawful
                    {
                        DeleteLoan_button.Visible = false;
                        break;
                    }
                case 2: //Data 
                case 4: //Guest
                case 9: //Media
                    {
                        DeleteLoan_button.Visible = false;
                        throw new InvalidOperationException(" You don't have the permission for this action.");
                    }
            }
        }

        private void PaymentAmount_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (LoanAmount_textBox.Text != ""
                        && ReturnedAmount1_textBox.Text != ""
                        && PaymentAmount_textBox.Text != "")
                    Calculate_RemainedAmount(Loan_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void generate_me_visits_notifications(bool deleteAndInsert)
        {
            try
            {
                // استلام القرض //
                // زيارة الافتتاحية مع تاريخ التمويل
                DateTime OpeningVisit_date = LoanDate_DateTimePicker.Value.AddDays(5);

                //Generate only Opening Visit on saving the loan//
                //generate the M1 to M5 visits AFTER saving the opening visit//
                ///////////////////////////////////////////////////////////////

                if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                {
                    if (deleteAndInsert)
                        //delete old notification
                        userNotification.Delete_Notification(
                            MicroProject_ID
                            , ""
                            , "Visit Closing-R"
                            , Person_Name_textBox.Text
                            , -21);
                }
                else
                {
                    if (deleteAndInsert)
                        //delete old notification
                        userNotification.Delete_Notification(
                           MicroProject_ID
                           , ""
                           , "Visit Opening-" + SubCategory
                           , Person_Name_textBox.Text
                           , -21);
                }

                //Send to M&E//
                if (User_IDs.Count == 0)
                    MessageBox.Show(
                        "لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");
                else
                    foreach (var u in User_IDs)
                    {
                        if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                        {
                            //generate only 1 rehabilitation closing visit
                            userNotification.Insert_UserNotification(OpeningVisit_date.ToString("yyyy/MM/dd"), "Visit Closing-R",
                                Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                        }
                        else
                        {
                            userNotification.Insert_UserNotification(OpeningVisit_date.ToString("yyyy/MM/dd"), "Visit Opening-" + SubCategory,
                               Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                        }
                    }

                l.Insert_Log("Insert Opening Visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Loan", Program.UserName, DateTime.Now);

                MessageBox.Show("تم توليد إشعار زيارة الافتتاحية الخاصة بهذا المشروع بنجاح");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void AddLoan_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();
                if (LoanAmount_textBox.Text == "") throw new InvalidOperationException("Loan Amount");
                MicroProject_ID = int.Parse(MicroProject_ID_textBox.Text);

                var date = DateTime.Now.Year + "/"
                         + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
                         + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":"
                         + DateTime.Now.Second;

                if (!Update_Mood)
                {
                    Loan_ID = Insert_Loan(MicroProject_ID);
                    l.Insert_Log("insert Loan to: " + MicroProject_ID + " ", "Loan", Program.UserName, DateTime.Now);

                    // change project state to Financed ممول // 

                    mp.Update_Project_State(MicroProject_ID, "ممول", date);
                    /////////////////////////////////////////////////////
                    l.Insert_Log("Update Project State of:" + MicroProject_ID + " to Financed ممول", "MicroProject",
                        Program.UserName, DateTime.Now);

                    //////////////////   task = تسليم كامل المبلغ و تحديد الأقساط ///////////
                    string task = "تسليم كامل المبلغ و تحديد الأقساط";
                    TasksOfProjects = new TasksOfProjects();
                    TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, task, true, DateTime.Now);

                    Update_Mood = true;
                }
                else
                {
                    Update_Loan(Loan_ID);
                    l.Insert_Log("Update the Loan of: " + MicroProject_ID + " ", "Loan", Program.UserName,
                        DateTime.Now);
                }

                //update DonorGroup_ID and Donor_ID in microproject Table//
                string DonorGroup_ID = "-1";
                string Donor_ID = "-1";
                string Donor = "";
                if (Group_comboBox.SelectedValue != null && Group_comboBox.SelectedIndex != -1)
                {
                    DonorGroup_ID = Group_comboBox.SelectedValue.ToString();
                    var row = dt_Group.Select("ID=" + DonorGroup_ID);
                    Donor_ID = row[0].Field<int>("Donor_ID").ToString();
                    Donor = row[0].Field<string>("Donor").ToString();
                    Donor = Program.ReplaceQuotation(Donor);

                    mp.Update_Project(MicroProject_ID, "DonorGroup_ID", DonorGroup_ID, "MP_Donor=" + Donor_ID);

                    l.Insert_Log("Update the project: " + MicroProject_ID
                        + " SET DonorGroup:" + Group_comboBox.Text + ","
                        + " SET Donor:" + Donor + ",",
                        "Loan", Program.UserName, DateTime.Now);
                }

                //check if the project has opening visit notification
                //then genereate if it hasn't have one (in both cases insert & update)
                int count = userNotification.Has_Visit_Notifications(MicroProject_ID, "Op");
                if (count <= 0) //توليد اشعار افتتاحية لكل المشاريع حتى الزراعية
                    generate_me_visits_notifications(false);
                else
                    generate_me_visits_notifications(true);

                Calculate_RemainedAmount(Loan_ID);
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Please choose the Project you want to add the Loan to it");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Loan Amount"))
                    MessageBox.Show("You can't leave empty fields, Please enter the Loan Amount.");
                else
                    MessageBox.Show(ex.Message);
            }
        }

        private void clear_Boxes()
        {
            Loan_ID = -1;
            LoanAmount_textBox.Text = PaymentsCount_label.Text = ReturnedAmount1_textBox.Text =
                ReturnedAmount2_label.Text = RemainOfReturned_label.Text = RemainOfPaymentsCount_label.Text =
                    PaidPaymentsCount_label.Text = PaidAmount_label.Text = "";

            RemainOfReturned_label.BackColor = Color.Transparent;
            if (Settings.Default.theme == "Dark")
                RemainOfReturned_label.ForeColor = Color.WhiteSmoke;
            else RemainOfReturned_label.ForeColor = Color.Black;

            Rate_textBox.Text = PaymentAmount_textBox.Text = ReceiptNo_textBox.Text = Note_textBox.Text = "";
            LoanDate_DateTimePicker.Value = DateTime.Now;
            Group_comboBox.SelectedIndex = -1;
            Payment_dataGridView.Rows.Clear();
            dolarAmount_textBox.Text = "";
        }

        private void MicroProject_ID_textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox.Text != "")
                {
                    var where = " where PMP.MicroProject_ID = " + Convert.ToInt32(MicroProject_ID_textBox.Text)
                              + " and PMP.MicroProject_ID in (select MicroProject_ID from microproject_state " +
                 " where MicroProject_ID = MP.MP_ID " +
                 " and State_ID in (1,4,5,7,8) " +
                 " and Current_State = 1 ) ";
                    fill_loan(where);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Person_Name_textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Person_Name_textBox.Text != "")
                {
                    var where =
                        " Where lower(IsProjectOwner) like 'yes' " +
                        " and CONCAT(TRIM(P1.P_FirstName),' ', TRIM(P1.P_LastName),' ابن/ة ',TRIM(P1.P_FatherName)) LIKE '%" + Person_Name_textBox.Text + "%'" +
                        " and PMP.MicroProject_ID in (select MicroProject_ID from microproject_state " +
                 " where MicroProject_ID = MP.MP_ID " +
                 " and State_ID in (1,4,5,7,8) " +
                 " and Current_State = 1  ) ";
                    fill_loan(where);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataTable project_dt;
        public void fill_loan(string where)
        {
            Program.buildConnection();
            var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                        + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                        + " ,L.Loan_ID as 'Loan_ID'"
                        + " ,L.Loan_Amount as 'Loan Amount'"
                        + " ,L.Loan_DateTaken as 'Loan Date'"
                        + " ,L.Loan_PaymentsCount as 'Payments Count'"
                        + " ,L.Loan_Rate as 'Rate'"
                        + " ,L.Loan_ReturnedAmount as 'Returned Amount'"
                        + " ,L.Loan_PaymentsAmount as 'Payments Amount'"
                        + " ,L.Loan_Note as 'Note'"
                        + " ,L.Loan_ReceiptID  as 'ReceiptID'"
                        + " ,SubCategory_ID as 'SubCategory_ID'"
                        + " ,MP.DonorGroup_ID as 'Group_ID'"
                        + " ,MP.MP_Donor as 'Donor_ID'"
                        + " , microprojecttype.Name as 'Type' "
                        + " , fundtype.Name as 'FundType' "

                        + @" From person_microproject PMP
 left join person P1 on P1.P_ID = PMP.Person_ID 
 left join microproject MP on PMP.MicroProject_ID = MP.MP_ID
 left join loan L on MP.MP_ID = L.MicroProject_ID 
 left join microprojecttype on microprojecttype.ID = MP.MP_Type_ID
 left join fundtype on fundtype.ID = MP.MP_FundType_ID ";

            query += where;
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            project_dt = new DataTable();
            da.Fill(project_dt);
            Program.MyConn.Close();
            // button1.Visible = false;

            clear_Boxes();

            if (project_dt != null)
                if (project_dt.Rows.Count > 0)
                {
                    if (project_dt.Rows[0]["Donor_ID"] == null || project_dt.Rows[0]["Donor_ID"] == DBNull.Value)
                    {
                        MessageBox.Show("لا يمكن إضافة قرض لهذا المشروع لأنه ليس بحالة مقبول");
                        SavePayments_button.Visible = false;
                    }
                    else
                    {
                        int donor_id = int.Parse(project_dt.Rows[0]["Donor_ID"].ToString());
                        //select the groups of this donor and bind
                        try
                        {
                            System.Data.DataTable dt_selected = dt_Group.Select("Donor_ID=" + donor_id).CopyToDataTable();
                            Group_comboBox.DataSource = null;
                            Group_comboBox.DataSource = dt_selected;
                            Group_comboBox.DisplayMember = "Name";
                            Group_comboBox.ValueMember = "ID";
                        }
                        catch
                        {
                            //Fill all donor groups because there isn't a group according to this donor
                            MessageBox.Show("انتبه! لا يوجد مجموعة خاصة لممول هذا المشروع، لذا سيتم عرض كل المجموعات", "Info", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }

                        MicroProject_ID = int.Parse(project_dt.Rows[0]["MicroProject_ID"].ToString());
                        MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                        Person_Name_textBox.Text = project_dt.Rows[0]["Beneficiary Name"].ToString();

                        if (project_dt.Rows[0]["SubCategory_ID"] != DBNull.Value)
                        {
                            SubCategory_ID = int.Parse(project_dt.Rows[0]["SubCategory_ID"].ToString());
                            if (SubCategory_ID == 1 || SubCategory_ID == 2) SubCategory = "V";
                            else SubCategory = "O";
                        }
                        clear_Boxes();

                        Update_Mood = false;
                        if (project_dt.Rows[0]["Loan_ID"] != DBNull.Value)
                        {
                            Loan_ID = int.Parse(project_dt.Rows[0]["Loan_ID"].ToString());
                            Payment_bind(Loan_ID);

                            Update_Mood = true;
                        }

                        if (project_dt.Rows[0]["Loan Amount"] != DBNull.Value)
                        {
                            var dd = Convert.ToDouble(project_dt.Rows[0]["Loan Amount"].ToString());
                            LoanAmount_textBox.Text = dd.ToString();
                            Loan = LoanAmount_textBox.Text;
                        }

                        if (project_dt.Rows[0]["Loan Date"] != DBNull.Value)
                        {
                            var date = (DateTime)project_dt.Rows[0]["Loan Date"];
                            LoanDate_DateTimePicker.Value = date;
                        }

                        if (project_dt.Rows[0]["Payments Count"] != DBNull.Value)
                        {
                            var Count = Convert.ToDouble(project_dt.Rows[0]["Payments Count"].ToString());
                            PaymentsCount_label.Text = Count.ToString();
                        }

                        ///////////////////// new columns /////////////////////////
                        if (project_dt.Rows[0]["Rate"] != DBNull.Value)
                        {
                            var dd = Convert.ToDouble(project_dt.Rows[0]["Rate"].ToString());
                            Rate_textBox.Text = dd.ToString();
                        }

                        if (project_dt.Rows[0]["Returned Amount"] != DBNull.Value)
                        {
                            var dd = Convert.ToDouble(project_dt.Rows[0]["Returned Amount"].ToString());
                            ReturnedAmount1_textBox.Text = dd.ToString();
                        }

                        if (project_dt.Rows[0]["Payments Amount"] != DBNull.Value)
                        {
                            var dd = Convert.ToDouble(project_dt.Rows[0]["Payments Amount"].ToString());
                            PaymentAmount_textBox.Text = dd.ToString();

                            Calculate_RemainedAmount(Loan_ID);
                        }

                        // to generate notifications by type (opening or closing)
                        type = fundtype = "";
                        type = project_dt.Rows[0]["Type"].ToString();
                        fundtype = project_dt.Rows[0]["FundType"].ToString();


                        if (project_dt.Rows[0]["Note"] != DBNull.Value) Note_textBox.Text = project_dt.Rows[0]["Note"].ToString();
                        if (project_dt.Rows[0]["ReceiptID"] != DBNull.Value)
                            ReceiptNo_textBox.Text = project_dt.Rows[0]["ReceiptID"].ToString();
                        ////////////////////////////////////////////////////////////
                        if (project_dt.Rows[0]["Group_ID"] == DBNull.Value)
                        {
                            Group_comboBox.SelectedIndex = -1;
                        }
                        else
                        {
                            var result = project_dt.Rows[0]["Group_ID"];

                            result = result == DBNull.Value ? null : result;
                            var countDis = Convert.ToInt32(result);
                            var Selected = -1;
                            var count = Group_comboBox.Items.Count;
                            for (var i = 0; i <= count - 1; i++)
                            {
                                Group_comboBox.SelectedIndex = i;
                                if (Group_comboBox.SelectedValue.ToString() == countDis.ToString())
                                {
                                    Selected = i;
                                    break;
                                }
                            }

                            Group_comboBox.SelectedIndex = Selected;
                            Group_comboBox.SelectedValue = countDis;

                            SavePayments_button.Visible = true;
                        }


                        mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                        mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("لا يمكن إضافة قرض لهذا المشروع لأنه ليس بحالة مقبول");
                    SavePayments_button.Visible = false;
                }
        }

        private void AddGroup_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var AddNewGroup = new AddNewGroup())
                {
                    var r = AddNewGroup.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        Group_bind();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Group_bind()
        {
            var donorGroup = new DonorGroup();
            dt_Group = donorGroup.Select("");
            Group_comboBox.DisplayMember = "Name";
            Group_comboBox.ValueMember = "ID";
            Group_comboBox.DataSource = dt_Group;
            Group_comboBox.SelectedIndex = -1;
        }
        private void Donor_bind()
        {
            var d = new Donor();
            dt_Donor = d.Select("");
        }

        private void Group_comboBox_Enter(object sender, EventArgs e)
        {
            Group_bind();
        }

        private void Group_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoanAmount_textBox.Text != "" && Group_comboBox.SelectedIndex != -1)
                {
                    var row = dt_Group.Select("ID = " + Group_comboBox.SelectedValue);
                    var rate = row[0].Field<double>("Rate");
                    Loan = LoanAmount_textBox.Text.Replace(",", "");
                    var syr_Amount = double.Parse(Loan);

                    var dolarAmount = syr_Amount / rate;
                    dolarAmount_textBox.Text = dolarAmount.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region textbox change value

        private void Loan_Amount_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoanAmount_textBox.Text != "")
                {
                    LoanAmount_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(LoanAmount_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    LoanAmount_textBox.SelectionStart = LoanAmount_textBox.Text.Length;
                    LoanAmount_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Payment_Amount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (PaymentAmount_textBox.Text != "")
                {
                    PaymentAmount_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(PaymentAmount_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    PaymentAmount_textBox.SelectionStart = PaymentAmount_textBox.Text.Length;
                    PaymentAmount_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReturnedAmount_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ReturnedAmount2_label.Text != "")
                {
                    ReturnedAmount2_label.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(ReturnedAmount2_label.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
                }

                if (ReturnedAmount1_textBox.Text != "")
                {
                    ReturnedAmount1_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(ReturnedAmount1_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    ReturnedAmount1_textBox.SelectionStart = ReturnedAmount1_textBox.Text.Length;
                    ReturnedAmount1_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void RemainOfReturned_textBox_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (RemainOfReturned_textBox.Text != "")
        //        {
        //            RemainOfReturned_textBox.Text =
        //                Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(RemainOfReturned_textBox.Text)),
        //                    @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

        //            RemainOfReturned_textBox.SelectionStart = RemainOfReturned_textBox.Text.Length;
        //            RemainOfReturned_textBox.SelectionLength = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        #endregion

        #region hover

        private void AddLoan_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                AddLoan_button.BackgroundImage = Resources.Save2_D;
            else AddLoan_button.BackgroundImage = Resources.Save2_L;
        }

        private void AddLoan_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                AddLoan_button.BackgroundImage = Resources.Save2_L;
            else AddLoan_button.BackgroundImage = Resources.Save2_D;
        }

        private void DeleteLoan_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                DeleteLoan_button.BackgroundImage = Resources.Delete2_D;
            else DeleteLoan_button.BackgroundImage = Resources.Delete2_L;
        }

        private void DeleteLoan_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                DeleteLoan_button.BackgroundImage = Resources.Delete2_L;
            else DeleteLoan_button.BackgroundImage = Resources.Delete2_D;
        }

        private void SavePayments_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                SavePayments_button.BackgroundImage = Resources.Save2_D;
            else SavePayments_button.BackgroundImage = Resources.Save2_L;
        }

        private void SavePayments_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                SavePayments_button.BackgroundImage = Resources.Save2_L;
            else SavePayments_button.BackgroundImage = Resources.Save2_D;
        }

        #endregion

        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        #region context menu items click
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MicroProject_ID_textBox2_Leave(sender, e);
        }

        private void attachments_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _Form = new Attachments_Form(MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "Attachments", 0);
        }

        private void addNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Find the admin in this database + who entered the application//
                string my_condition = " and ( IsCashier = 1 ";
                my_condition += " or role.Role_Name like 'Admin' ) ";

                User user = new User();

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

                string project = MicroProject_ID.ToString() + ":" + Person_Name_textBox.Text;
                var _Form = new NewIdea_Form("Loan & Payments", project, emails);
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    CheckUserPermission(); 
            //    var dialogResult = MessageBox.Show("سيتم إعادة توليد إشعارات زيارات المراقبة والتقييم وذلك حسب تاريخ تسليم القرض، وحذف الإشعارات القديمة في حال تم توليدها سابقاً",
            //            " ", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        //Delete this project Visit notifications//
            //        userNotification.Update_MicroUsers_Notification(MicroProject_ID, "", "Visit%", Person_Name_textBox.Text,"1");
            //        /////////////////////////////////////////// 
            //        generate_me_visits_notifications();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        #region Prevent vertical scroll
        // Prevent vertical and horizontal scroll messages from being processed 
        int previousVerticalScrollValue;


        private void Payment_dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                previousVerticalScrollValue = Payment_dataGridView.FirstDisplayedScrollingRowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Payment_dataGridView_Enter(object sender, EventArgs e)
        {
            try
            {
                if (previousVerticalScrollValue >= 0 && previousVerticalScrollValue < Payment_dataGridView.Rows.Count)
                {
                    Payment_dataGridView.FirstDisplayedScrollingRowIndex = previousVerticalScrollValue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

    }
}