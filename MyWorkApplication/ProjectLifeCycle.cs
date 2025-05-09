using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;

namespace MyWorkApplication
{
    public partial class ProjectLifeCycle : Form
    {
        //string[,] PayDate = new string[30, 2];
        private DateTime date;
        private int MicroProject_ID, Loan_ID;

        private MySqlComponents MySS;
        private int NumOfAllPayments;
        private string[,] PayDate;
        private string ReceiveDate, ApplyDate, TodayDate;
        private DataRow SelectedDataRow;
        private string state;

        public ProjectLifeCycle()
        {
            InitializeComponent();
        }

        private void Project_bind()
        {
            
            Program.buildConnection();

            MySS.query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                         //+ " ,PMP.Person_ID as 'Beneficiary_ID'"
                         + "  ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                         + "  ,MP.MP_Name as 'Project Name'"
                         + " ,CASE MP.MP_State WHEN 0 THEN N'Rejected' WHEN 1 THEN N'Accepted' WHEN 2 THEN N'Delayed' WHEN 4 THEN N'Financed' WHEN 5 THEN N'Closed' ELSE N'On Hold' End as 'Project State'"
                         + "  ,MP.MP_DateOfRequest as 'Apply Date'"
                         + "  from `person_microproject` PMP left outer join `person` P1 on P1.P_ID = PMP.Person_ID"
                         + "  left outer join `microproject` MP on PMP.MicroProject_ID = MP.MP_ID";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            MP_dataGridView.ColumnHeadersVisible = false;
            MP_dataGridView.DataSource = MySS.dt;
            MP_dataGridView.ColumnHeadersVisible = true;
            MP_dataGridView.Columns["Apply Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        public DataTable Loan_bind(int MP_ID)
        {
            
            Program.buildConnection();

            MySS.query = "select `Loan_ID` as 'ID'" +
                         ",`Loan_Amount` as 'Loan Amount'" +
                         ",`Loan_DateTaken` as 'Receive Date'" +
                         ",`Loan_PaymentsCount` as 'Payments Count'" +
                         "from `loan` where `MicroProject_ID` = " + MP_ID + " ";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            return MySS.dt;
        }

        public DataTable Payment_bind(int Loan_ID)
        {
            
            Program.buildConnection();

            MySS.query = "select `Pay_Amount` as 'Amount'" +
                         ",`Pay_DueDate` as 'Pay Date'" +
                         ",`Pay_IsPaid` as 'State'" +
                         ",`Pay_RecievedOnDate` as 'Actual Pay Date'" +
                         "\n from `payment` where `Loan_ID` = " + Loan_ID + " " +
                         "\n order by `Pay_DueDate`";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            return MySS.dt;
        }

        public void Build_Chart_Series(string ApplyDate, string ReceiveDate, string[,] PayDate)
        {
            var list = new List<string>();
            var payArray = column(PayDate, 0);
            var ZeroList = new double[1];

            foreach (var series in chart1.Series)
                //clear chart
                series.Points.Clear();
            chart1.ChartAreas[0].AxisX.Interval = 1; //to ahow all labels in x-axis
            chart1.ChartAreas[0].AxisX.CustomLabels.Clear();
            //list.Add(ApplyDate);
            //ZeroList[0] = 0;

            chart1.Series[0].Points.AddXY(1, 0);
            chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(0.7, 1.3, ApplyDate);
            //list.Clear();

            if (ReceiveDate != "")
            {
                //list.Add(ReceiveDate);
                chart1.Series[1].Points.AddXY(2, 0);
                chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(1.7, 2.3, ReceiveDate);
                //list.Clear();
            }

            for (var i = 0; i < payArray.Length - 1; i++)
                if (payArray[i] != null)
                    list.Add(payArray[i]);
            ZeroList = new double[list.Count];
            for (var i = 0; i < ZeroList.Length - 1; i++) ZeroList[i] = 0;

            for (var i = 0; i < ZeroList.Length - 1; i++)
                if (i == ZeroList.Length - 2) //last item in the list
                {
                    if (payArray.Length - 1 == NumOfAllPayments) //is this payment the last one in this project
                    {
                        chart1.Series[3].Points.AddXY(i + 3, ZeroList[i]);
                        chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(i + 2.7, i + 3.3, payArray[i]);
                    }
                    else
                    {
                        chart1.Series[2].Points.AddXY(i + 3, ZeroList[i]);
                        chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(i + 2.7, i + 3.3, payArray[i]);
                    }
                }
                else
                {
                    chart1.Series[2].Points.AddXY(i + 3, ZeroList[i]);
                    chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(i + 2.7, i + 3.3, payArray[i]);
                }

            // chart1.Series[2].Points.AddXY(list,ZeroList);
        }

        public void Build_Chart_Series(string ApplyDate, string ReceiveDate)
        {
            var list = new List<string>();
            var ZeroList = new double[1];

            foreach (var series in chart1.Series)
                //clear chart
                series.Points.Clear();
            chart1.ChartAreas[0].AxisX.Interval = 1; //to ahow all labels in x-axis
            chart1.ChartAreas[0].AxisX.CustomLabels.Clear();

            chart1.Series[0].Points.AddXY(1, 0);
            chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(0.7, 1.3, ApplyDate);

            if (ReceiveDate != "")
            {
                chart1.Series[1].Points.AddXY(2, 0);
                chart1.ChartAreas["ChartArea1"].AxisX.CustomLabels.Add(1.7, 2.3, ReceiveDate);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProjectLifeCycle_Load(sender, e);
        }

        private void SearchBy_textBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in MP_dataGridView.Rows)
            {
                var found = 0;
                for (var i = 0; i < MP_dataGridView.ColumnCount; i++)
                    if (r.Cells[i].Value.ToString().Contains(SearchBy_textBox.Text))
                    {
                        MP_dataGridView.Rows[r.Index].Visible = true;
                        found = 1;
                    }

                if (found == 0)
                {
                    MP_dataGridView.CurrentCell = null;
                    MP_dataGridView.Rows[r.Index].Visible = false;
                }

                //if (SearchBy_textBox.Text != "")
                //{ Counter_textBox.Text = MP_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString(); }
            }
        }

        public T[] column<T>(T[,] multidimArray, int wanted_column)
        {
            var l = multidimArray.GetLength(0);
            var columnArray = new T[l];
            for (var i = 0; i < l; i++) columnArray[i] = multidimArray[i, wanted_column];
            return columnArray;
        }

        private void ProjectLifeCycle_Load(object sender, EventArgs e)
        {
            try
            {
                //MyTheme myTheme = new MyTheme();
                //if (Properties.Settings.Default.theme == "Light")
                //    myTheme.ShowAllForm_ToLight(this);
                //else
                //    myTheme.ShowAllForm_ToNight(this);

                MySS = new MySqlComponents();
                PayDate = new string[20, 2];
                Project_bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MP_dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                SelectedDataRow = ((DataRowView) MP_dataGridView.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    MicroProject_ID = int.Parse(SelectedDataRow["MicroProject_ID"].ToString());
                    date = (DateTime) SelectedDataRow["Apply Date"];
                    ApplyDate = date.ToShortDateString();

                    // bring the dates of this project life cycle
                    MySS.dt = Loan_bind(MicroProject_ID);
                    if (MySS.dt.Rows.Count > 0)
                    {
                        Loan_ID = (int) MySS.dt.Rows[0]["ID"];
                        date = (DateTime) MySS.dt.Rows[0]["Receive Date"];
                        NumOfAllPayments = (int) MySS.dt.Rows[0]["Payments Count"];
                        ReceiveDate = date.ToShortDateString();
                        MySS.dt = Payment_bind(Loan_ID);
                        if (MySS.dt.Rows.Count > 0)
                        {
                            var i = 0;
                            foreach (DataRow row in MySS.dt.Rows)
                            {
                                date = (DateTime) row["Pay Date"];
                                state = row["State"].ToString();
                                PayDate[i, 0] = date.ToShortDateString();
                                PayDate[i, 1] = state == "Paid" ? "Black" : "Red";
                                i++;
                            }

                            //build the chart
                            Build_Chart_Series(ApplyDate, ReceiveDate, PayDate);
                        }
                        else //the project has a loan but doesn't have payments yet
                        {
                            //build the chart
                            Build_Chart_Series(ApplyDate, ReceiveDate);
                        }
                    }
                    else //the project doesn't have a loan
                    {
                        ReceiveDate = "";

                        //build the chart
                        Build_Chart_Series(ApplyDate, ReceiveDate);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}