using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MyWorkApplication
{
    public partial class Timeline_Form : Form
    {
        private DateTime date;
        private int[] IDs_array;
        private Log l;

        private readonly MainForm mainForm;
        private int MicroProject_ID, Loan_ID;
        private MySqlComponents MySS;
        private int NumOfAllPayments;
        private string[,] PayDate;
        private string ReceiveDate, ApplyDate;
        private Select s;
        private DataRow SelectedDataRow;
        private string state, MP_State;

        public Timeline_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void Timeline_Form_Load(object sender, EventArgs e)
        {
            try
            {
                var NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                {
                    NewTheme.Timeline_ToLight(this);
                    chart1.ChartAreas[0].BackColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].BackHatchStyle = ChartHatchStyle.DarkVertical;
                    chart1.ChartAreas[0].BorderColor = Color.FromArgb(75, 75, 75);

                    chart1.ChartAreas[0].AxisX.InterlacedColor = Color.Black;
                    chart1.ChartAreas[0].AxisX.LineColor = Color.Black;
                    chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
                    chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
                    chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);

                    chart1.ChartAreas[0].AxisY.InterlacedColor = Color.Black;
                    chart1.ChartAreas[0].AxisY.LineColor = Color.Black;
                    chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
                    chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                    chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);
                }
                else
                {
                    NewTheme.Timeline_ToNight(this);
                    chart1.ChartAreas[0].BackColor = Color.FromArgb(75, 75, 75);
                    chart1.ChartAreas[0].BackHatchStyle = ChartHatchStyle.None;
                    chart1.ChartAreas[0].BorderColor = Color.FromArgb(189, 189, 189);

                    chart1.ChartAreas[0].AxisX.InterlacedColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisX.LineColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisX.TitleForeColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);

                    chart1.ChartAreas[0].AxisY.InterlacedColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisY.LineColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisY.TitleForeColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(189, 189, 189);
                    chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(96, 96, 96);
                    chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.FromArgb(96, 96, 96);
                }

                bind_Category_into_ComboBox();

                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Timeline";

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                //fill_beneficiary_box();
                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                PayDate = new string[30, 2];

                bind_Applications(MP_Category_comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bind_Category_into_ComboBox()
        {

            Program.buildConnection();

            var query = "select ID,Name from `category`";
            var sc = new MySqlCommand(query, Program.MyConn);
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var reader = sc.ExecuteReader();
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Load(reader);

            reader.Close();
            Program.MyConn.Close();
            MP_Category_comboBox.DisplayMember = "Name";
            MP_Category_comboBox.ValueMember = "ID";
            MP_Category_comboBox.DataSource = dt;
            MP_Category_comboBox.Text = "";
        }

        public DataTable Loan_bind(int MP_ID)
        {
            Program.MyConn.Close();

            Program.buildConnection();

            MySS.query = "select `Loan_ID` as 'ID'" +
                         ",`Loan_Amount` as 'Loan Amount'" +
                         ",`Loan_DateTaken` as 'Receive Date'" +
                         ",`Loan_PaymentsCount` as 'Payments Count'" +
                         "from `loan` where `MicroProject_ID` = " + MP_ID + " ";

            var sc = new MySqlCommand(MySS.query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable Payment_bind(int Loan_ID)
        {
            Program.MyConn.Close();

            Program.buildConnection();

            MySS.query = "select `Pay_Amount` as 'Amount'" +
                         ",`Pay_DueDate` as 'Pay Date'" +
                         ",`Pay_IsPaid` as 'State'" +
                         ",`Pay_RecievedOnDate` as 'Actual Pay Date'" +
                         "\n from `payment` where `Loan_ID` = " + Loan_ID + " " +
                         "\n order by `Pay_DueDate`";
            var sc = new MySqlCommand(MySS.query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DateTime GetApplyDate(int MP_ID)
        {

            Program.buildConnection();

            MySS.query = " select MP.MP_DateOfRequest as 'Apply Date'"
                         + " from  `microproject` MP where MP.MP_ID = " + MP_ID;

            var sc = new MySqlCommand(MySS.query, Program.MyConn);
            return Convert.ToDateTime(sc.ExecuteScalar());
        }

        private void MP_Category_comboBox_Leave(object sender, EventArgs e)
        {
            try
            {
                bind_Applications(MP_Category_comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MicroProject_ID_textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox.Text != "")
                {

                    Program.buildConnection();
                    var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                        + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                        + " ,L.Loan_ID as 'Loan_ID'"
                        + " ,L.Loan_Amount as 'Loan Amount'"
                        + " ,L.Loan_DateTaken as 'Loan Date'"
                        + " ,L.Loan_PaymentsCount as 'Payments Count'"
                        + " ,state.Name_Ar as 'Project State'"
                        + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                        + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                        + " left outer join loan L on MP.MP_ID = L.MicroProject_ID "
                        + " LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID "
                        + " LEFT OUTER JOIN `state` on state.ID = mps.State_ID "
                        + " where PMP.MicroProject_ID = " + Convert.ToInt32(MicroProject_ID_textBox.Text);

                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    MySS.da = new MySqlDataAdapter(MySS.sc);
                    MySS.dt = new DataTable();
                    MySS.da.Fill(MySS.dt);
                    if (MySS.dt != null)
                        if (MySS.dt.Rows.Count > 0)
                        {
                            MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                            Person_Name_textBox.Text = MySS.dt.Rows[0]["Beneficiary Name"].ToString();
                            date = GetApplyDate(MicroProject_ID);
                            var Ap_date = GetApplyDate(MicroProject_ID);
                            ApplyDate = date.ToString("dd-MM-yyyy");
                            MP_State = MySS.dt.Rows[0]["Project State"].ToString();


                            var ddd = Loan_bind(MicroProject_ID);
                            if (ddd.Rows.Count > 0)
                            {
                                Loan_ID = (int)ddd.Rows[0]["ID"];
                                var Re_date = (DateTime)ddd.Rows[0]["Receive Date"];
                                date = (DateTime)ddd.Rows[0]["Receive Date"];
                                ReceiveDate = date.ToString("dd-MM-yyyy");

                                NumOfAllPayments = (int)ddd.Rows[0]["Payments Count"];

                                ddd = Payment_bind(Loan_ID);
                                if (ddd.Rows.Count > 0)
                                {
                                    var i = 0;
                                    foreach (DataRow row in ddd.Rows)
                                    {
                                        date = (DateTime)row["Pay Date"];
                                        state = row["State"].ToString();
                                        PayDate[i, 0] = date.ToString();
                                        PayDate[i, 1] = state == "Paid" ? "Black" : "Red";
                                        i++;
                                    }

                                    //build the chart++++++++++++++++++
                                    //Build_Chart_Series(ApplyDate, ReceiveDate, PayDate);
                                    Build_Chart_Series(Ap_date, Re_date, MP_State, 1);
                                }
                                else //the project has a loan but doesn't have payments yet
                                {
                                    Build_Chart_Series(Ap_date, Re_date, "", 1);
                                    //build the chart
                                    //  Build_Chart_Series(ApplyDate, ReceiveDate);
                                }
                            }
                            else //the project doesn't have a loan
                            {
                                ReceiveDate = "";

                                //build the chart
                                Build_Chart_Series(Ap_date, Ap_date, "", 1);
                            }

                            mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                            mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                        }
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

                    Program.buildConnection();
                    var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                        + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                        + " ,L.Loan_ID as 'Loan_ID'"
                        + " ,L.Loan_Amount as 'Loan Amount'"
                        + " ,L.Loan_DateTaken as 'Loan Date'"
                        + " ,L.Loan_PaymentsCount as 'Payments Count'"
                        + " ,state.Name_Ar as 'Project State'"
                        + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                        + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                        + " left outer join loan L on MP.MP_ID = L.MicroProject_ID "
                        + " LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID "
                        + " LEFT OUTER JOIN `state` on state.ID = mps.State_ID "
                        + " WHERE CONCAT(TRIM(P1.P_FirstName), ' ', TRIM(P1.P_LastName), ' ابن/ة ', TRIM(P1.P_FatherName)) LIKE '%" +
                        Person_Name_textBox.Text + "%'";

                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    MySS.da = new MySqlDataAdapter(MySS.sc);
                    MySS.dt = new DataTable();
                    MySS.da.Fill(MySS.dt);
                    if (MySS.dt != null)
                        if (MySS.dt.Rows.Count > 0)
                        {
                            MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                            MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                            Person_Name_textBox.Text = MySS.dt.Rows[0]["Beneficiary Name"].ToString();

                            mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                            mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Build_Chart_Series(DateTime ApplyDate, DateTime ReceiveDate, string payments, int mp_index)
        {
            var list = new List<DateTime>();
            var payArray = column(PayDate, 0);

            var s1 = new Series("Apply Date");
            s1.ChartType = SeriesChartType.Point;
            s1.BorderWidth = 5;
            s1.Color = Color.Blue;

            var s2 = new Series("Recieve Date");
            s2.ChartType = SeriesChartType.Point;
            s2.BorderWidth = 5;
            s2.Color = Color.Green;

            var s3 = new Series("Payments");
            s3.ChartType = SeriesChartType.Point;
            s3.BorderWidth = 5;
            s3.Color = Color.Red;

            var s4 = new Series("End Of Project");
            s4.ChartType = SeriesChartType.Point;
            s4.BorderWidth = 5;
            s4.Color = Color.Black;

            s1.Points.AddXY(ApplyDate, mp_index);

            if (payments != "")
            {
                s2.Points.AddXY(ReceiveDate, mp_index);

                for (var i = 0; i < payArray.Length - 1; i++)
                    if (payArray[i] != null)
                        list.Add(Convert.ToDateTime(payArray[i]));
                for (var i = 0; i < list.Count; i++)
                    if (i == list.Count - 1) //last item in the list
                    {
                        if (payments == "مغلق")
                            s4.Points.AddXY(list[i], mp_index);
                    }
                    else
                    {
                        s3.Points.AddXY(list[i], mp_index);
                    }
            }

            chart1.Series.Clear();
            chart1.Annotations.Clear();

            chart1.Series.Add(s1);
            chart1.Series.Add(s2);
            chart1.Series.Add(s3);
            chart1.Series.Add(s4);

            //  put_Annotiation(s1); put_Annotiation(s2); put_Annotiation(s3); put_Annotiation(s4);


            chart1.Series[0].XValueType = chart1.Series[1].XValueType =
                chart1.Series[2].XValueType = chart1.Series[3].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-yyyy";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart1.Series[0].Font = chart1.Series[1].Font = chart1.Series[2].Font =
                chart1.Series[3].Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);

            var minDate = new DateTime(2018, 01, 01);
            var maxDate = DateTime.Now.AddMonths(1);
            chart1.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();

            chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 90;

            var blockSize = 10;
            // enable autoscroll
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            // let's zoom to [0,blockSize] (e.g. [0,100])
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Months;
            var position = 0;
            var size = blockSize;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(position, size);
            // disable zoom-reset button (only scrollbar's arrows are available)
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            // set scrollbar small change to blockSize (e.g. 100)
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
        }

        public T[] column<T>(T[,] multidimArray, int wanted_column)
        {
            var l = multidimArray.GetLength(0);
            var columnArray = new T[l];
            for (var i = 0; i < l; i++) columnArray[i] = multidimArray[i, wanted_column];
            return columnArray;
        }

        private void getDetailsOfProject_BuildChart(int[] MP_IDs)
        {
            var str = "(" + string.Join(", ", Array.ConvertAll(MP_IDs, v => v.ToString(CultureInfo.InvariantCulture))) +
                      ")";

            Program.buildConnection();
            var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                + " ,L.Loan_ID as 'Loan_ID'"
                + " ,L.Loan_Amount as 'Loan Amount'"
                + " ,L.Loan_DateTaken as 'Loan Date'"
                + " ,L.Loan_PaymentsCount as 'Payments Count'"
                + " ,state.Name_Ar as 'Project State'"
                + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                + " left outer join loan L on MP.MP_ID = L.MicroProject_ID "
                + " LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID "
                + " LEFT OUTER JOIN `state` on state.ID = mps.State_ID "
                + " where PMP.MicroProject_ID in " + str + "";
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                Build_Series();
                for (var j = 0; j < dt.Rows.Count; j++)
                {
                    MicroProject_ID = int.Parse(dt.Rows[j]["MicroProject_ID"].ToString());
                    Person_Name_textBox.Text = dt.Rows[j]["Beneficiary Name"].ToString();
                    var Ap_date = GetApplyDate(MicroProject_ID);
                    // Ap_date = Convert.ToDateTime(Ap_date.ToString("dd-MM-yyyy"));

                    MP_State = dt.Rows[j]["Project State"].ToString();

                    var ddd = Loan_bind(MicroProject_ID);
                    if (ddd.Rows.Count > 0)
                    {
                        Loan_ID = (int)ddd.Rows[0]["ID"];
                        var Re_date = (DateTime)ddd.Rows[0]["Receive Date"];
                        //    Re_date = Convert.ToDateTime(Re_date.ToString("dd-MM-yyyy"));

                        NumOfAllPayments = (int)ddd.Rows[0]["Payments Count"];

                        ddd = Payment_bind(Loan_ID);
                        if (ddd.Rows.Count > 0)
                        {
                            var i = 0;
                            foreach (DataRow row in ddd.Rows)
                            {
                                date = (DateTime)row["Pay Date"];
                                state = row["State"].ToString();
                                PayDate[i, 0] = date.ToString();
                                PayDate[i, 1] = state == "Paid" ? "Black" : "Red";
                                i++;
                            }
                            //build the chart

                            var ss = Create_New_Line_Series(MicroProject_ID);
                            Fill_Series(Ap_date, Re_date, MP_State, j + 1, ss);
                        }
                        else //the project has a loan but doesn't have payments yet
                        {
                            //build the chart
                            var ss = Create_New_Line_Series(MicroProject_ID);
                            Fill_Series(Ap_date, Re_date, "", j + 1, ss);
                        }
                    }
                    else //the project doesn't have a loan
                    {
                        ReceiveDate = "";

                        //build the chart
                        var ss = Create_New_Line_Series(MicroProject_ID);
                        Fill_Series(Ap_date, Ap_date, "", j + 1, ss);
                    }
                }

                /////// after adding all points in all series
                //  put_Annotiation(chart1.Series["Apply Date"]); put_Annotiation(chart1.Series["Recieve Date"]);
                //  put_Annotiation(chart1.Series["Payments"]); put_Annotiation(chart1.Series["End Of Project"]);
            }
        }

        public void Build_Series()
        {
            var s1 = new Series("Apply Date");
            s1.ChartType = SeriesChartType.Point;
            s1.BorderWidth = 5;
            s1.Color = Color.Blue;

            var s2 = new Series("Recieve Date");
            s2.ChartType = SeriesChartType.Point;
            s2.BorderWidth = 5;
            s2.Color = Color.Green;

            var s3 = new Series("Payments");
            s3.ChartType = SeriesChartType.Point;
            s3.BorderWidth = 5;
            s3.Color = Color.Red;

            var s4 = new Series("End Of Project");
            s4.ChartType = SeriesChartType.Point;
            s4.BorderWidth = 5;
            s4.Color = Color.Black;

            //var line_Series = new Series("Project timeLine");
            //line_Series.ChartType = SeriesChartType.Line;
            //line_Series.BorderWidth = 2;
            //line_Series.Color = Color.Purple;
            //line_Series.MarkerStyle = MarkerStyle.Diamond;
            //line_Series.MarkerSize = 12;
            //line_Series.MarkerColor = Color.Purple;

            chart1.Series.Clear();
            chart1.Annotations.Clear();

            chart1.Series.Add(s1);
            chart1.Series.Add(s2);
            chart1.Series.Add(s3);
            chart1.Series.Add(s4);

            //int blockSize = 100;
            //// enable autoscroll
            //chart1.ChartAreas[0].CursorX.AutoScroll = true;
            //// let's zoom to [0,blockSize] (e.g. [0,100])
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Years;
            //int position = 0;
            //int size = blockSize;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(position, size);
            //// disable zoom-reset button (only scrollbar's arrows are available)
            //chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            //// set scrollbar small change to blockSize (e.g. 100)
            //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
        }

        private Series Create_New_Line_Series(int MP_ID)
        {
            var colorDialog1 = new ColorDialog();

            var line_Series = new Series("Project" + MP_ID);
            line_Series.ChartType = SeriesChartType.Line;
            line_Series.BorderWidth = 1;
            //if (colorDialog1.ShowDialog() == DialogResult.OK)
            //    line_Series.Color = colorDialog1.Color;

            line_Series.MarkerStyle = MarkerStyle.Diamond;
            line_Series.MarkerSize = 14;

            return line_Series;
        }

        public void Fill_Series(DateTime ApplyDate, DateTime ReceiveDate, string payments, int mp_index, Series ss)
        {
            var list = new List<DateTime>();
            var payArray = column(PayDate, 0);

            ss.SmartLabelStyle = new SmartLabelStyle { CalloutLineAnchorCapStyle = LineAnchorCapStyle.Diamond };

            chart1.Series[0].XValueType = chart1.Series[1].XValueType =
                chart1.Series[2].XValueType = chart1.Series[3].XValueType = ChartValueType.DateTime;
            ss.XValueType = ChartValueType.DateTime;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-yyyy";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;

            chart1.Series[0].Font = chart1.Series[1].Font = chart1.Series[2].Font =
                chart1.Series[3].Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);
            ss.Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);

            var minDate = new DateTime(2018, 01, 01);
            var maxDate = DateTime.Now.AddMonths(1);
            chart1.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();

            chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Avenir LT Std 35 Light", 11f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels;
            //  chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 90;

            // fill data points //
            chart1.Series["Apply Date"].Points.AddXY(ApplyDate, mp_index);

            ss.Points.AddXY(ApplyDate, mp_index);
            ss.Points[0].MarkerStyle = MarkerStyle.Cross;

            if (payments != "")
            {
                chart1.Series["Recieve Date"].Points.AddXY(ReceiveDate, mp_index);
                ss.Points.AddXY(ReceiveDate, mp_index);
                ss.Points[1].MarkerStyle = MarkerStyle.Star5;

                for (var i = 0; i < payArray.Length - 1; i++)
                    if (payArray[i] != null)
                        list.Add(Convert.ToDateTime(payArray[i]));
                for (var i = 0; i < list.Count; i++)
                    if (i == list.Count - 1) //last item in the list
                    {
                        if (payments == "مغلق")
                        {
                            chart1.Series["End Of Project"].Points.AddXY(list[i], mp_index);
                            ss.Points.AddXY(list[i], mp_index);
                        }
                    }
                    else
                    {
                        chart1.Series["Payments"].Points.AddXY(list[i], mp_index);
                        ss.Points.AddXY(list[i], mp_index);
                    }
            }

            /////////////////////////
            //add the new series to chart
            chart1.Series.Add(ss);


            var blockSize = 10;
            // enable autoscroll
            chart1.ChartAreas[0].CursorX.AutoScroll = true;

            //// let's zoom to [0,blockSize] (e.g. [0,100])
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Months;
            //int position = 0;
            //int size = blockSize;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(position, size);

            //// disable zoom-reset button (only scrollbar's arrows are available)
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            //// set scrollbar small change to blockSize (e.g. 100)
            //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = blockSize;
        }

        private void Show_button_Click(object sender, EventArgs e)
        {
            getDetailsOfProject_BuildChart(IDs_array);
        }

        private void put_Annotiation(Series s)
        {
            foreach (var point in s.Points)
            {
                // directly anchored to a point
                var TA1 = new CalloutAnnotation();
                TA1.Text = "#VALX";
                TA1.TextStyle = TextStyle.Default;
                TA1.SetAnchor(point);
                //  TA1.AnchorX = 50;  // 50% of chart width
                //   TA1.AnchorY = 20;  // 20% of chart height, from top!
                TA1.Alignment = ContentAlignment.MiddleCenter;
                if (s.Name == "Apply Date") TA1.LineColor = Color.Blue;
                else if (s.Name == "Recieve Date") TA1.LineColor = Color.Green;
                else if (s.Name == "Payments") TA1.LineColor = Color.Red;
                else if (s.Name == "End Of Project") TA1.LineColor = Color.Black;
                TA1.LineWidth = 1;
                TA1.BackColor = Color.FromArgb(216, 217, 214);
                TA1.ForeColor = Color.Black;
                TA1.Font = new Font("Avenir LT Std 35 Light", 10f, FontStyle.Regular);
                chart1.Annotations.Add(TA1);
            }

            //// anchored to a point but shifted down
            //TextAnnotation TA2 = new TextAnnotation();
            //TA2.Text = "5/12/2018";
            //TA2.AnchorDataPoint = chart1.Series[0].Points[0];
            //TA2.AnchorY = 0;
            //chart1.Annotations.Add(TA2);

            //// this one is not anchored on a point:
            //TextAnnotation TA3 = new TextAnnotation();
            //TA3.Text = "At 50% width BC";
            //TA3.AnchorX = 50;  // 50% of chart width
            //TA3.AnchorY = 20;  // 20% of chart height, from top!
            //TA3.Alignment = ContentAlignment.BottomCenter;  // try a few!
            //chart2.Annotations.Add(TA3);
        }

        private void bind_Applications(string Category)
        {

            Program.buildConnection();
            var from = "  from `microproject` MP "
                + " left join `person_microproject` PMP  on PMP.MicroProject_ID = MP.MP_ID "
                + " LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID "
                + " LEFT JOIN `state` on state.ID = mps.State_ID "
                + " left join `person` P1 on P1.P_ID = PMP.Person_ID "
                + " left join `category` C on MP.Category_ID = C.ID  ";

            MySS.query = "select PMP.PMP_ID as 'PMP_ID'"
                         + " ,PMP.MicroProject_ID as 'رقم المشروع'"
                         + " ,PMP.Person_ID as 'Beneficiary_ID'"
                         + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'المستفيد'"
                         + " ,MP.MP_Name as 'اسم المشروع'"
                         + " ,state.Name_Ar as 'حالة المشروع'"
                         + from;
            var condition = " where 1 ";

            //// category  ////
            if (Category != "")
                condition += " and c.Name like N'%" + Category + "%'";

            MySS.query += condition;
            MySS.query += " order by PMP.MicroProject_ID ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            MP_dataGridView.ColumnHeadersVisible = false;
            MP_dataGridView.DataSource = MySS.dt;
            MP_dataGridView.ColumnHeadersVisible = true;
            Program.MyConn.Close();

            DataGridViewColumn dgC1;
            if (MP_dataGridView.RowCount > 0 && MP_dataGridView.CurrentRow != null)
            {
                dgC1 = MP_dataGridView.Columns["Beneficiary_ID"];
                dgC1.Visible = false;
                dgC1 = MP_dataGridView.Columns["PMP_ID"];
                dgC1.Visible = false;
            }
        }

        private void MP_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var SelectedRowCount = MP_dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (SelectedRowCount >= 1)
                {
                    IDs_array = new int[SelectedRowCount];
                    for (var i = 0; i < SelectedRowCount; i++)
                    {
                        SelectedDataRow = ((DataRowView)MP_dataGridView.SelectedRows[i].DataBoundItem).Row;
                        IDs_array[i] = int.Parse(SelectedDataRow["رقم المشروع"].ToString());
                    }

                    //getDetailsOfProject_BuildChart(IDs_array);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}