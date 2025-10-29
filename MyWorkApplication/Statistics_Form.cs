using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using PresentationControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.AxHost;
using Application = Microsoft.Office.Interop.Excel.Application;
using ChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using Color = System.Drawing.Color;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using Legend = System.Windows.Forms.DataVisualization.Charting.Legend;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace MyWorkApplication
{
    public partial class Statistics_Form : Form
    {
        private string ApplyDate_condition = "";
        private string FundedDate_condition = "";

        private string RequestedAmount_condition = "";
        private string FundedAmount_condition = "";

        private string mevisitDate_condition = "";

        private SeriesChartType Chart_type = SeriesChartType.Column;
        private Evaluation_Statistics es;

        private bool user_mode;
        private DataSet ds, combo_ds;
        private string from = "", inner_from = "", same_from = "";
        private string from3 = "";
        private string condition = " where 1 ";

        private bool Hided;
        private readonly MainForm main_form;
        private List<string> X_Axis_list;

        private MySqlComponents MySS;
        private int PW;
        private Application xlApp;
        private Workbook xlWorkBook;
        private Worksheet xlWorkSheet;
        private List<double> xValues = new List<double>();
        private List<double> yValues = new List<double>();
        private List<Color> colors = new List<Color>();
        NewTheme newTheme;
        Street st;
        SubCategory sub;
        Select s;

        public Statistics_Form(MainForm main_form)
        {
            InitializeComponent();
            this.main_form = main_form;
        }

        private void Statistics_Form_Load(object sender, EventArgs e)
        {
            try
            {
                newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Statistics_ToNight(this, Hided);
                else
                    newTheme.Statistics_ToLight(this, Hided);

                user_mode = false;

                MySS = new MySqlComponents();
                es = new Evaluation_Statistics();
                st = new Street();
                sub = new SubCategory();
                s = new Select();

                PW = Search_panel.Height;
                Hided = true;
                Search_panel.Height = 0;

                //X_Axis_list.Add("Experience VS Project");
                //X_Axis_list.Add("Profit Rate");
                //X_Axis_list.Add("Success Rate");

                X_Axis_list = new List<string>();
                X_Axis_list.Add("Project Status");              //0
                X_Axis_list.Add("Funding Type");                //1
                X_Axis_list.Add("Project Type");                //2 طاقة زراعي ترميم
                X_Axis_list.Add("New Project VS Developing");   //3
                X_Axis_list.Add("Categories");                  //4
                X_Axis_list.Add("Sub Categories");              //5
                X_Axis_list.Add("Partnership");                 //6
                X_Axis_list.Add("Parishes");                    //7
                X_Axis_list.Add("Gender");                      //8
                X_Axis_list.Add("Marital Status");              //9
                X_Axis_list.Add("Age");                         //10
                X_Axis_list.Add("Donors");                      //11
                X_Axis_list.Add("Fund Year");                   //12
                X_Axis_list.Add("Fund Amount ($)");             //13
                X_Axis_list.Add("Sustainability of projects");  //14 مدة استدامة المشاريع الممولة


                for (int i = 0; i < X_Axis_list.Count; i++)
                    X_Axis_comboBox.Items.Add(X_Axis_list.ElementAt<string>(i));

                //clear
                chart1.ChartAreas.Clear();
                chart1.Series.Clear();

                Bind_All_ComboBoxes();

                fillChartTypesComboBox();
                comboBox1.SelectedIndex = 0;

                user_mode = true;
                X_Axis_comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void X_Axis_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (X_Axis_comboBox.SelectedIndex != -1)
                    {
                        Check_Visibility();

                        from =
                            @" From `microproject` MP
 LEFT JOIN `person_microproject` PMP on PMP.MicroProject_ID = MP.MP_ID 
 LEFT JOIN `person` P on P.P_ID = PMP.Person_ID 
 LEFT JOIN `priest` on Priest_ID = P.P_Priest_ID
 left JOIN `parish` on P.P_Parish_ID = parish.ID 
 Left join `maritalstatus` on maritalstatus.ID = P.P_MaritalStatus_ID
 LEFT JOIN `category` C on C.ID = MP.Category_ID
 LEFT JOIN `subcategory` sub on sub.ID =  MP.SubCategory_ID  
 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT JOIN `state` on state.ID = mps.State_ID  
 LEFT JOIN `donor` on donor.ID = MP.MP_Donor
 LEFT JOIN `street` w_street on w_street.ID = MP.MP_Street_ID
 LEFT JOIN `street` h_street on h_street.ID = P.Street_ID  
 LEFT JOIN `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT JOIN `donorgroup` on donorgroup.ID = MP.DonorGroup_ID  
 LEFT JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
 LEFT JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
 LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID ";

                        inner_from =
                            @" From `microproject`
 LEFT JOIN `person_microproject` on person_microproject.MicroProject_ID = microproject.MP_ID 
 LEFT JOIN `person` on person.P_ID = person_microproject.Person_ID 
 LEFT JOIN `priest` on Priest_ID = person.P_Priest_ID
 left JOIN `parish` on person.P_Parish_ID = parish.ID 
 Left join `maritalstatus` on maritalstatus.ID = person.P_MaritalStatus_ID
 LEFT JOIN `category` C on C.ID = microproject.Category_ID
 LEFT JOIN `subcategory` sub on sub.ID =  microproject.SubCategory_ID  
 LEFT JOIN `microproject_state` on microproject_state.MicroProject_ID = microproject.MP_ID
 LEFT JOIN `state` on state.ID = microproject_state.State_ID  
 LEFT JOIN `donor` on donor.ID = microproject.MP_Donor
 LEFT JOIN `street` w_street on w_street.ID = microproject.MP_Street_ID
 LEFT JOIN `street` h_street on h_street.ID = person.Street_ID  
 LEFT JOIN `loan` on loan.MicroProject_ID = microproject.MP_ID
 LEFT JOIN `donorgroup` on donorgroup.ID = microproject.DonorGroup_ID  
 LEFT JOIN `fundtype` on fundtype.ID = microproject.MP_FundType_ID 
 LEFT JOIN `microprojecttype` on microprojecttype.ID = microproject.MP_Type_ID
 LEFT JOIN `microprojectsubtype` on microprojectsubtype.ID = microproject.MP_SubType_ID ";

                        Check_Conditions();

                        //clear
                        chart1.ChartAreas.Clear();
                        chart1.Series.Clear();

                        user_mode = false;
                        withFunded_checkBox.Visible = false;

                        //build
                        Build_ChartArea(0);

                        //profit rate - success rate SelectedIndex != 13
                        if (X_Axis_comboBox.SelectedIndex != 11
                         && X_Axis_comboBox.SelectedIndex != 12
                         && X_Axis_comboBox.SelectedIndex != 13
                         //&& X_Axis_comboBox.SelectedIndex != 14
                         )    
                        {
                            withFunded_checkBox.Visible = true;
                            user_mode = true;

                            if (Chart_type == SeriesChartType.Doughnut)
                            {
                                Build_ChartArea(1);

                                chart1.ChartAreas[0].Position = new ElementPosition(2, 5, 48, 80);
                                chart1.ChartAreas[1].Position = new ElementPosition(50, 5, 48, 80);
                            }
                        }

                        user_mode = true;

                        // Project Status //
                        if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(0))
                        {
                            Project_Status_BindChart();

                            Fill_ChartArea("Status");
                        }

                        // Fund Type //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(1))
                        {
                            Project_FundType_BindChart();

                            Fill_ChartArea("Fund Type");
                        }

                        // Project Type //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(2))
                        {
                            Project_Type_BindChart();

                            Fill_ChartArea("Project Type");
                        }

                        // SubType //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(3))
                        {
                            Project_SubType_BindChart();

                            Fill_ChartArea("New Project vs Develpment");
                        }

                        // Categories //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(4))
                        {
                            Project_Category_BindChart();

                            Fill_ChartArea("Categories");
                        }
                        // Sub Categories //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(5))
                        {
                            Project_SubCategory_BindChart();

                            Fill_ChartArea("Sub Categories");
                        }

                        // Partnership //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(6))
                        {
                            Project_Partnership_BindChart();

                            Fill_ChartArea("Partnership");
                        }

                        // Parishes //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(7))
                        {
                            Project_Parish_BindChart();

                            Fill_ChartArea("Parishes");
                        }

                        // Gender //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(8))
                        {
                            Project_Gender_BindChart();

                            Fill_ChartArea("Gender");
                        }

                        // Marital status //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(9))
                        {
                            Project_MaritalStatus_BindChart();

                            Fill_ChartArea("Marital status");
                        }

                        //   age  //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(10))
                        {
                            Project_Age_BindChart();

                            Fill_ChartArea("Age");
                        }

                        // profit rate //
                        //else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(11))
                        //{
                        //    condition += mevisitDate_condition;
                        //    Project_LastProfitRate_BindChart();

                        //    Fill_ChartArea("Profit Rate");
                        //}

                        //// success rate //
                        //else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(12))
                        //{
                        //    condition += mevisitDate_condition;
                        //    Project_LastMEResults_BindChart();

                        //    Fill_ChartArea("Success Rate");
                        //}

                        // Donors //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(11))
                        {
                            Project_Donor_BindChart();

                            Fill_ChartArea("Donors");
                        }

                        // Fund Year //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(12))
                        {
                            Project_FundYear_BindChart();

                            Fill_ChartArea("Fund Year");
                        }

                        // Fund Year //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(13))
                        {
                            Project_FundAmount_BindChart();

                            Fill_ChartArea("Fund Amount");
                        }

                        // Sustainability //
                        else if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(14))
                        {
                            Project_Sustainability_BindChart();

                            Fill_ChartArea("Sustainability");
                        }

                        //to show/hide columns
                        if (withFunded_checkBox.Visible)
                            WithFunded_checkBox_CheckStateChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void ApplyFilter_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    X_Axis_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AdvancedSearch_button_Click(object sender, EventArgs e)
        {
            if (Hided)
            {
                Hided = false;
                if (Settings.Default.theme == "Light")
                    AdvancedSearch_button.BackgroundImage = Resources.Hide2_L;
                else AdvancedSearch_button.BackgroundImage = Resources.Hide2_D;
                Search_panel.Height = PW;

                ApplyFilter_button.Visible = ApplyFilter_button.Enabled = true;
            }
            else
            {
                Hided = true;
                if (Settings.Default.theme == "Light")
                    AdvancedSearch_button.BackgroundImage = Resources.Show2_L;
                else AdvancedSearch_button.BackgroundImage = Resources.Show2_D;
                Search_panel.Height = 0;

                ApplyFilter_button.Visible = ApplyFilter_button.Enabled = false;
            }
        }

        private void Check_Visibility()
        {
            FundType_comboBox.Visible = Type_comboBox.Visible = SubType_comboBox.Visible
                = MP_Status_comboBox.Visible = Partnership_comboBox.Visible
                = Donor_comboBox.Visible = DonorGroup_comboBox.Visible
                = Category_comboBox.Visible = SubCategory_comboBox.Visible
                = P_Parish_comboBox.Visible = Age_comboBox.Visible
                = Gender_comboBox.Visible = MaritalStatus_comboBox.Visible
                = HomeStreet_comboBox.Visible = ProjectStreet_comboBox.Visible = true;

            FundType_label.Visible = Type_label.Visible = SubType_label.Visible
                = MP_Status_label.Visible = Partnership_label.Visible
                = Donor_label.Visible = DonorGroup_label.Visible
                = Category_label.Visible = SubCategory_label.Visible
                = P_Parish_label.Visible = Age_label.Visible
                = Gender_label.Visible = MaritalStatus_label.Visible
                = HomeStreet_label.Visible = ProjectStreet_label.Visible = true;

            ApplyDateFrom_label.Text = "Apply Date:";
            //// profit rate //
            //if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(12))
            //{
            //    ApplyDateFrom_label.Text = "Visit Date:";
            //}
            //// success rate //
            //if (X_Axis_comboBox.SelectedItem.ToString() == X_Axis_list.ElementAt<string>(13))
            //{
            //    ApplyDateFrom_label.Text = "Visit Date:";
            //}
        }
        public void Check_Conditions()
        {
            condition = " where Current_State = 1 ";

            //Fund Type
            if (FundType_comboBox.Text != "")
            {
                //condition += " and fundtype.Name like '" + FundType + "'";
                condition += " and fundtype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(FundType_comboBox, true) + ")";
            }

            //Type
            if (Type_comboBox.Text != "")
            {
                //condition += " and microprojecttype.Name like '" + Type + "'";
                condition += " and microprojecttype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Type_comboBox, true) + ")";
            }

            //Sub Type
            if (SubType_comboBox.Text != "")
            {
                //condition += " and microprojectsubtype.Name like '" + SubType + "'";
                condition += " and microprojectsubtype.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(SubType_comboBox, true) + ")";
            }

            //Partners
            if (Partnership_comboBox.Text != "")
            {
                if (Partnership_comboBox.Text.Contains("شراكة"))
                    condition += " and Partnership > 1 ";

                if (Partnership_comboBox.Text.Contains("فردي"))
                    condition += " and Partnership = 1 ";
            }

            //MP Status
            if (MP_Status_comboBox.Text != "")
            {
                condition += " and state.Name_Ar in ("
                    + Program.getSelectedItems_CheckboxComboBox(MP_Status_comboBox, true) + ")";
            }
            //Donor
            if (Donor_comboBox.Text != "")
            {
                //condition += " and donor.Name like '" + Donor + "'";
                condition += " and donor.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Donor_comboBox, true) + ")";
            }
            //Donor Group
            if (DonorGroup_comboBox.Text != "")
            {
                condition += " and donorgroup.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(DonorGroup_comboBox, true) + ")";
            }
            //Parish
            if (P_Parish_comboBox.Text != "")
            {
                //if (Parish == "Christian")
                //    condition += " and parish.Name_en not like  N'Muslim'";
                //else condition += " and parish.Name_en like  N'" + Parish + "'";
                condition += " and parish.Name_en in ("
                    + Program.getSelectedItems_CheckboxComboBox(P_Parish_comboBox, true) + ")";
            }

            //Category
            if (Category_comboBox.Text != "")
            {
                condition += " and C.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Category_comboBox, true) + ")";
            }
            //SUBCategory
            if (SubCategory_comboBox.Text != "")
            {
                condition += " and sub.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(SubCategory_comboBox, true) + ")";
            }
            //Career
            if (Career_comboBox.Text != "")
            {
                condition += " and career.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Career_comboBox, true) + ")";
            }
            //Marital Status
            if (MaritalStatus_comboBox.Text != "")
            {
                condition += " and maritalstatus.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(MaritalStatus_comboBox, true) + ")";
            }

            //Apply Date
            if (ApplyDate_comboBox.Text != "")
            {
                condition += " and Year(MP_DateOfRequest)in ("
                   + Program.getSelectedItems_CheckboxComboBox(ApplyDate_comboBox, true) + ")";
            }
            //Fund Date
            if (FundDate_comboBox.Text != "")
            {
                condition += " and Year(Loan_DateTaken) in ("
                    + Program.getSelectedItems_CheckboxComboBox(FundDate_comboBox, true) + ")";
            }
            //Age 
            if (Age_comboBox.Text != "")
            {
                DataRow[] rows
                    = combo_ds.Tables[9].Select("Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(Age_comboBox, true) + ")");

                condition += " and TIMESTAMPDIFF(YEAR, `P_DOB`, MP_DateOfRequest) BETWEEN "
                    + rows[0][2].ToString() + " and " + rows[0][3].ToString();
            }
            //Gender
            if (Gender_comboBox.Text != "")
            {

                if (Gender_comboBox.Text.Contains("ذكر")) condition += " and P_Sex like N'%ذكر%'";
                if (Gender_comboBox.Text.Contains("أنثى")) condition += " and P_Sex like N'%أنثى%'";
            }

            //Home Street
            if (HomeStreet_comboBox.Text != "")
            {
                condition += " and h_street.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(HomeStreet_comboBox, true) + ")";
            }

            //Project Street 
            if (ProjectStreet_comboBox.Text != "")
            {
                condition += " and w_street.Name in ("
                    + Program.getSelectedItems_CheckboxComboBox(ProjectStreet_comboBox, true) + ")";
            }

            //Apply Date
            if (ApplyDate_condition != "")
            {
                //profit rate - success rate  
                if (X_Axis_comboBox.SelectedIndex != 11
                 && X_Axis_comboBox.SelectedIndex != 12
                 && X_Axis_comboBox.SelectedIndex != 13
                 //&& X_Axis_comboBox.SelectedIndex != 14
                 )  
                    condition += ApplyDate_condition;
            }

            //Funded Date 
            if (FundedDate_condition != "") condition += FundedDate_condition;

            //Requested Amount
            if (RequestedAmount_condition != "")
                condition += RequestedAmount_condition;

            //Funded Amount 
            if (FundedAmount_condition != "")
                condition += FundedAmount_condition;
        }

        #region export to excel

        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
                //Just the roles : Admin - admin_l0 - manager - data
                if (Program.role != 1 && Program.role != 8 && Program.role != 5 && Program.role != 2)
                    throw new InvalidOperationException("Sorry ! You Don't have the permission for this action.");


                object misValue = Missing.Value;
                xlApp = new Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
                // see the excel sheet behind the program  
                xlApp.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.  
                // store its reference to worksheet  
                xlWorkSheet = xlWorkBook.Sheets[1];
                xlWorkSheet = xlWorkBook.ActiveSheet;
                // changing the name of active sheet  
                xlWorkSheet.Name = "Exported from App";

                for (var x = 1; x < results_dataGridView.Columns.Count + 1; x++)
                    xlWorkSheet.Cells[1, x] = results_dataGridView.Columns[x - 1].HeaderText;

                for (var i = 0; i < results_dataGridView.Rows.Count; i++)
                {
                    for (var j = 0; j < results_dataGridView.Columns.Count; j++)
                        xlWorkSheet.Cells[i + 2, j + 1] = results_dataGridView.Rows[i].Cells[j].Value.ToString();
                }
                //1.create the Excel Chart object
                var xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
                //2. Set the position of chart where you need to place inside the Excel sheet
                var myChart = xlCharts.Add(200, 5, 300, 250);
                //3. create a new chart page to display your value
                var chartPage = myChart.Chart;
                //4.Set the X & Y axis Range of data columns   
                //4.1 it takes Excel A Column as as X axis; Data value is from A20-A30
                //4.2 it takes Excel B Column as as Y axis; Data value is from A20-A30
                var rowCount = results_dataGridView.Rows.Count + 1;

                var chartRange = xlWorkSheet.get_Range("A1", "B" + rowCount);
                if (results_dataGridView.Columns.Count > 2)
                    chartRange = xlWorkSheet.get_Range("A1", "C" + rowCount);

                //5.Set the chart Source data from your chart range
                chartPage.SetSourceData(chartRange, Type.Missing);
                //6.select the chart type to render your data values

                if (comboBox1.SelectedItem.ToString() == SeriesChartType.Column.ToString())
                    chartPage.ChartType = XlChartType.xlColumnClustered;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.BoxPlot.ToString()
                         || comboBox1.SelectedItem.ToString() == SeriesChartType.Bubble.ToString())
                    chartPage.ChartType = XlChartType.xlBubble;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.Bar.ToString())
                    chartPage.ChartType = XlChartType.xlBarClustered;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.SplineRange.ToString()
                         || comboBox1.SelectedItem.ToString() == SeriesChartType.Line.ToString())
                    chartPage.ChartType = XlChartType.xlLine;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.Area.ToString())
                    chartPage.ChartType = XlChartType.xlArea;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.Point.ToString())
                    chartPage.ChartType = XlChartType.xlXYScatter;

                else if (comboBox1.SelectedItem.ToString() == SeriesChartType.Doughnut.ToString()
                         || comboBox1.SelectedItem.ToString() == SeriesChartType.Pie.ToString())
                    chartPage.ChartType = XlChartType.xlPie;

                else chartPage.ChartType = XlChartType.xl3DLine;

                //colong the series  

                //chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
                //7.If you need to declare the chart title please follow the two steps
                myChart.Chart.HasTitle = true;
                chartPage.ChartTitle.Text = "Column Chart";

                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();

                //releaseObject(xlWorkSheet);
                //releaseObject(xlWorkBook);
                //releaseObject(xlApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Chart_type = (SeriesChartType)comboBox1.SelectedItem;
                refreshPageToolStripMenuItem_Click(sender, e);
                //Category_comboBox_Leave(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void fillChartTypesComboBox()
        {
            comboBox1.Items.Add(SeriesChartType.Column);
            comboBox1.Items.Add(SeriesChartType.BoxPlot);
            comboBox1.Items.Add(SeriesChartType.Bar);
            comboBox1.Items.Add(SeriesChartType.SplineRange);
            comboBox1.Items.Add(SeriesChartType.Area);
            comboBox1.Items.Add(SeriesChartType.Line);
            comboBox1.Items.Add(SeriesChartType.Bubble);
            comboBox1.Items.Add(SeriesChartType.Point);

            comboBox1.Items.Add(SeriesChartType.StepLine);
            comboBox1.Items.Add(SeriesChartType.Doughnut);

            comboBox1.Items.Add(SeriesChartType.StackedBar);
            comboBox1.Items.Add(SeriesChartType.StackedColumn);

            //    comboBox1.Items.Add(SeriesChartType.Kagi); 
            //    comboBox1.Items.Add(SeriesChartType.Renko);
            //    comboBox1.Items.Add(SeriesChartType.Pie);
            //    comboBox1.Items.Add(SeriesChartType.Pyramid);
            //    comboBox1.Items.Add(SeriesChartType.Candlestick);
            //    comboBox1.Items.Add(SeriesChartType.ErrorBar);
            //    comboBox1.Items.Add(SeriesChartType.FastLine);
            //    comboBox1.Items.Add(SeriesChartType.FastPoint);
            //    comboBox1.Items.Add(SeriesChartType.PointAndFigure);
            //    comboBox1.Items.Add(SeriesChartType.Polar);
            //    comboBox1.Items.Add(SeriesChartType.Radar);
            //    comboBox1.Items.Add(SeriesChartType.Range);
            //    comboBox1.Items.Add(SeriesChartType.RangeColumn);
            //    comboBox1.Items.Add(SeriesChartType.RangeBar); 
            //    comboBox1.Items.Add(SeriesChartType.Funnel);
            //    comboBox1.Items.Add(SeriesChartType.Spline);
            //    comboBox1.Items.Add(SeriesChartType.SplineArea); 
            //    comboBox1.Items.Add(SeriesChartType.StackedArea);  
        }

        private void executeQuery_countAll(string query)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            results_dataGridView.DataSource = ds.Tables[0];
            Count_all_label.Text = Count_funded_label.Text = "";

            if (X_Axis_comboBox.SelectedIndex == 14)
            {
                var sumObject = ds.Tables[0].Compute("Sum(Num_Funded)", string.Empty);
                var sumObject2 = ds.Tables[0].Compute("Sum(Num_Closed)", string.Empty);

                Count_all_label.Text = "Ongoing: " + sumObject.ToString();
                Count_funded_label.Text = "Closed: " + sumObject2.ToString();
            }
            else
            {
                var sumObject = ds.Tables[0].Compute("Sum(numOfProjects)", string.Empty);
                var sumObject2 = ds.Tables[0].Compute("Sum(numOfFundedProjects)", string.Empty);

                Count_all_label.Text = "All: " + sumObject.ToString();
                Count_funded_label.Text = "Funded: " + sumObject2.ToString();
            }
        }

        #region select project statistics 
        private void Project_Status_BindChart()
        {
            try
            {
                var query = " Select state.Name_en as 'Project State' " +
                             ", count(DISTINCT MP_ID) as numOfProjects " +
                             ", (select count(DISTINCT microproject.MP_ID) " +
                             inner_from + condition +                       //add the conditions to the sub query// 
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and microproject_state.State_ID = mps.State_ID ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = "";
                GroupBycondition += " group by state.Name_en ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_FundType_BindChart()
        {
            try
            {

                string query =
                    " select fundtype.Name_en " +
                    ", count(DISTINCT MP_ID) as numOfProjects" +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                            inner_from + condition +
                            " and State_ID in (4,5,7,8) and Current_State = 1 " +
                            " and MP.MP_FundType_ID = microproject.MP_FundType_ID " +
                      ") as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by fundtype.Name ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Type_BindChart()
        {
            try
            {
                string query = " Select microprojecttype.Name_en as 'Type'" +
                    ", count(DISTINCT MP_ID) as numOfProjects " +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                             inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and MP.MP_SubType_ID = microproject.MP_SubType_ID ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = "  group by microprojecttype.Name ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_SubType_BindChart()
        {
            try
            {
                string query = " Select microprojectsubtype.Name_en as 'sub Type'" +
                    ", count(DISTINCT MP_ID) as numOfProjects " +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                             inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and MP.MP_SubType_ID = microproject.MP_SubType_ID ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = @" group by microprojectsubtype.Name_en 
    order by Field(microprojectsubtype.Name_en
,'New Project','Development/Expand'
,'Renovation' ,'Renovation - New Project' ,'Renovation - Development/Expand')";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Category_BindChart()
        {
            try
            {

                string query = "select C.Name " +
                    ", count(DISTINCT MP_ID) as numOfProjects " +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                    inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1" +
                    " and MP.Category_ID = microproject.Category_ID  ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by C.Name ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_SubCategory_BindChart()
        {
            try
            {

                string query = "select sub.Name " +
                    ", count(DISTINCT MP_ID) as numOfProjects " +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                    inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                    " and MP.SubCategory_ID = microproject.SubCategory_ID  ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by sub.Name ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Partnership_BindChart()
        {
            try
            {
                string query = "Select case WHEN Partnership = 1 THEN 'Individual' ELSE CONCAT(Partnership, ' Partners') END as PartnershipRange "
                             + ", count(DISTINCT MP_ID) as numOfProjects "
                             + ", (select count(DISTINCT microproject.MP_ID) "
                             + inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and MP.Partnership = microproject.Partnership ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = "  group by PartnershipRange ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Donor_BindChart()
        {
            try
            {
                condition += " and MP_Donor is not null ";

                string query = "Select donor.Name " +
                    ", count(DISTINCT MP_ID) as numOfProjects " +
                    ", (select count(DISTINCT microproject.MP_ID) " +
                             inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                " and MP.MP_Donor = microproject.MP_Donor ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by donor.Name ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_FundYear_BindChart()
        {
            try
            {
                condition += " and YEAR(Loan_DateTaken) is not null ";

                string query = "Select YEAR(Loan_DateTaken) as 'Fund Year'"
                             + ", count(DISTINCT MP_ID) as numOfProjects "
                             + ", (select count(DISTINCT microproject.MP_ID) "
                             + inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and YEAR(L.Loan_DateTaken) = YEAR(loan.Loan_DateTaken) ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = "  group by YEAR(Loan_DateTaken) ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_FundAmount_BindChart()
        {
            try
            {
                 
                string query = @"Select case 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 0 AND 1000 THEN '0-1000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 1001 AND 2000 THEN '1001-2000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 2001 AND 3000 THEN '2001-3000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 3001 AND 4000 THEN '3001-4000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 4001 AND 5000 THEN '4001-5000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 5001 AND 5000 THEN '5001-5000' 
 WHEN Round( (CASE Currency 
    WHEN 'SP'  THEN Loan_Amount / ExchangeRate
    WHEN 'USD' THEN Loan_Amount 
    END) , 2 ) BETWEEN 6001 AND 7000 THEN '6001-7000' 
ELSE '>7000' END as FundAmountRange "

                    + ", count(DISTINCT MP_ID) as numOfProjects "
                    + ", (select count(DISTINCT microproject.MP_ID) "
                            + inner_from + condition +
                            " and State_ID in (4,5,7,8) " +
                            " and Current_State = 1 " +
                            " and YEAR(L.Loan_Amount) = YEAR(loan.Loan_Amount) ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = @" 
    GROUP BY FundAmountRange 
    ORDER BY FIELD(FundAmountRange ,
    '0-1000', '1001-2000', '2001-3000', '3001-4000', '4001-5000', '5001-6000', '6001-7000', '>7000')";

                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Sustainability_BindChart()
        {
            try
            {
                string query = @"SELECT
CASE WHEN(
        CASE
            WHEN state.Name_Ar IN('مغلق', 'ملغى')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, mps.Date)
            WHEN state.Name_Ar IN('ممول', 'منتهي')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, NOW())
            ELSE NULL
        END
            ) < 1 THEN '<1 year' 
    WHEN(
        CASE
            WHEN state.Name_Ar IN('مغلق', 'ملغى')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, mps.Date)
            WHEN state.Name_Ar IN('ممول', 'منتهي')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, NOW())
            ELSE NULL
        END
            ) BETWEEN 1 AND 2 THEN '1-2 years'
    WHEN(
        CASE
            WHEN state.Name_Ar IN('مغلق', 'ملغى')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, mps.Date)
            WHEN state.Name_Ar IN('ممول', 'منتهي')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, NOW())
            ELSE NULL
        END
    ) BETWEEN 2 AND 3 THEN '2-3 years'
    WHEN(
        CASE
            WHEN state.Name_Ar IN('مغلق', 'ملغى')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, mps.Date)
            WHEN state.Name_Ar IN('ممول', 'منتهي')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, NOW())
            ELSE NULL
        END
    ) BETWEEN 3 AND 4 THEN '3-4 years'
    WHEN(
        CASE
            WHEN state.Name_Ar IN('مغلق', 'ملغى')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, mps.Date)
            WHEN state.Name_Ar IN('ممول', 'منتهي')
                THEN TIMESTAMPDIFF(YEAR, Loan_DateTaken, NOW())
            ELSE NULL
        END
    ) BETWEEN 4 AND 5 THEN '4-5 years'
    ELSE '>5 years'
END AS SustainabilityRange "

        + ", COUNT(DISTINCT CASE WHEN state.Name_Ar IN('ممول','منتهي') THEN MP_ID END) AS Num_Funded "
        + ", COUNT(DISTINCT CASE WHEN state.Name_Ar IN('ملغى','مغلق') THEN MP_ID END)  AS Num_Closed "; //بس ملغى ومغلق

        //+ ", COUNT(DISTINCT MP_ID) AS numOfProjects "
        //+ ", (select count(DISTINCT microproject.MP_ID) "
        //+ inner_from + condition +
        //" and State_ID in (4,5,7,8) " + 
        //" and Current_State = 1 " +
        //" AND YEAR(L.Loan_DateTaken) = YEAR(loan.Loan_DateTaken) ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = @" 
    GROUP BY SustainabilityRange 
    ORDER BY FIELD(SustainabilityRange ,
    '<1 year', '1-2 years', '2-3 years', '3-4 years', '4-5 years', '>5 years')";

                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        //        private void Project_Experience_BindChart()
        //        {
        //            //try
        //            //{

        ////                condition += " and Score_ID = 3 ";

        ////                var from3 = " left join microproject_score ms on MP.MP_ID = ms.MicroProject_ID ";
        ////                var inner_from3 = " left join microproject_score on microproject.MP_ID = microproject_score.MicroProject_ID ";

        ////                string query = @"SELECT case ms.value when 1 then 'Yes' when 0 then 'No' else 'N/A' end as 'Experience'
        ////, count(DISTINCT MP_ID) as numOfProjects 
        ////, (select count(DISTINCT microproject.MP_ID) " +
        ////        inner_from + inner_from3 + condition +
        ////                             " and State_ID in (4,5,7,8) " +
        ////                             " and Current_State = 1 " +
        ////        " and ms.value = microproject_score.value ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

        ////                var GroupBycondition = "  group by ms.value ";

        //                //query += from + from3 + condition + GroupBycondition + ";";

        //                //executeQuery_countAll(query);
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    MessageBox.Show(ex.Message);
        //            //}
        //        }

        #endregion
        #region select beneficiary statistics 
        private void Project_Parish_BindChart()
        {
            try
            {
                condition += " and lower(IsProjectOwner) like 'yes' ";
                string query = "Select parish.Name_en " +
                    ", count(P_ID) as numOfProjects " +
                    ", (select count(person.P_ID) " +
                        inner_from + condition +
                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                        " and  P.P_Parish_ID = person.P_Parish_ID ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by parish.Name_en ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Gender_BindChart()
        {
            try
            {
                string query =
                    " SELECT (CASE P_Sex WHEN N'أنثى' THEN N'Female' WHEN N'ذكر' THEN N'Male' ELSE N'N/A' End) " +
                    ", COUNT(P_ID) as numOfProjects " +
                    ", (select count(person.P_ID) " +
                             inner_from + condition +  // يحسب مستفيدين  

                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                    " and  P.P_Sex = person.P_Sex  ) as numOfFundedProjects ";      //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by P_Sex ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_MaritalStatus_BindChart()
        {
            try
            {
                condition += " and lower(IsProjectOwner) like 'yes' ";
                string query =
                    "SELECT maritalstatus.Name_en " +
                    ", COUNT(P_ID) as numOfProjects " +
                    ", (select count(person.P_ID) " +
                             inner_from + condition +                           // يحسب مستفيدين  

                             " and State_ID in (4,5,7,8) " +
                             " and Current_State = 1 " +
                             " and P.P_MaritalStatus_ID = person.P_MaritalStatus_ID  ) as numOfFundedProjects "; //مقسمين نفس التقسيمة 

                var GroupBycondition = " group by maritalstatus.Name_en ";
                query += from + condition + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Project_Age_BindChart()
        {
            try
            {
                condition += " and lower(IsProjectOwner) like 'yes' ";
                string query = " Select a.Name as 'Age'"
                    + ", count(P_ID) as numOfProjects "
                    + ", (select count(person.P_ID) "
                    + inner_from
                    + " LEFT JOIN age ON TIMESTAMPDIFF(YEAR, person.P_DOB, microproject.MP_DateOfRequest) >= age.Min "
                                 + " AND TIMESTAMPDIFF(YEAR, person.P_DOB, microproject.MP_DateOfRequest) < age.Max "
                    + condition
                    + " and State_ID in (4,5,7,8) "
                    + " and Current_State = 1 "
                    + " and age.Name = a.Name "
                    + ") as numOfFundedProjects ";      //مقسمين نفس التقسيمة  

                var GroupBycondition = " Group by `Age` ORDER BY `P_DOB` desc";
                query += from
                    + " LEFT JOIN age a ON TIMESTAMPDIFF(YEAR, P_DOB, MP_DateOfRequest) >= a.Min "
                                   + " AND TIMESTAMPDIFF(YEAR, P_DOB, MP_DateOfRequest) <  a.Max "
                    + condition
                    + GroupBycondition + ";";

                executeQuery_countAll(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void Project_LastProfitRate_BindChart()
        //{
        //    try
        //    {
        //        MySqlCommand sc = new MySqlCommand("calculate_profit_rates", Program.MyConn);
        //        sc.CommandType = CommandType.StoredProcedure;
        //        sc.Parameters.AddWithValue("@MyCondition", condition);

        //        using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);
        //            results_dataGridView.DataSource = dt;
        //            Program.MyConn.Close();

        //            Count_all_label.Text = Count_funded_label.Text = "";

        //            var sumObject = dt.Compute("Sum(numOfProjects)", string.Empty);
        //            Count_funded_label.Text = "Funded:" + sumObject.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void Project_LastMEResults_BindChart()
        //{
        //    try
        //    {
        //        MySqlCommand sc = new MySqlCommand("calculate_last_me_results", Program.MyConn);
        //        sc.CommandType = CommandType.StoredProcedure;
        //        sc.Parameters.AddWithValue("@MyCondition", condition);

        //        using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);
        //            results_dataGridView.DataSource = dt;
        //            Program.MyConn.Close();

        //            Count_all_label.Text = Count_funded_label.Text = "";

        //            var sumObject = dt.Compute("Sum(numOfProjects)", string.Empty);

        //            Count_funded_label.Text = "Funded:" + sumObject.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        #endregion

        #region comboBox bind and events
        private void Bind_All_ComboBoxes()
        {
            string query = "";

            query += @"SELECT ID, Name from `category` ORDER BY Name ASC;"; //0
            query += @"SELECT ID, Name, Category_ID ,(SELECT Name from `category` where ID = Category_ID) as Category_Name 
                    from `subcategory` ORDER BY Name ASC;"; //1
            query += @"SELECT ID, Name, SubCategory_ID ,(SELECT Name from `subcategory` where ID = SubCategory_ID) as SubCategory_Name 
                    from `career` ORDER BY Name ASC;"; //2 

            query += @"SELECT ID, Name from `donor` ORDER BY Name ASC;"; //3
            query += @"SELECT ID, Name, Donor_ID ,(SELECT Name from `donor` where ID = Donor_ID) as Donor_Name 
                    from `donorgroup` ORDER BY Name ASC;"; //4

            query += @"SELECT ID, Name from `fundtype` ORDER BY ID ASC;"; //5
            query += @"SELECT ID, Name from `microprojecttype` ORDER BY ID ASC;"; //6
            query += @"SELECT ID, Name, Type_ID ,(SELECT Name from `microprojecttype` where ID = Type_ID) as Type_Name     
                    from `microprojectsubtype` ORDER BY ID ASC;"; //7

            query += @"SELECT ID, Name from `maritalstatus` order by ID ASC;"; //8
            query += @"SELECT ID, Name, Min, Max from `age` where isActive =1 order by ID ASC; "; //9   

            query += @"SELECT ID, Name_ar as 'Name', Name_en from `state` order by ID ASC; "; //10  
            query += @"SELECT ID, Name_en as 'Name' from `parish` order by Name_en ASC; "; //11  
            query += @"SELECT ID, Name from `street` as h_street order by Name ASC; "; //12 
            query += @"SELECT ID, Name from `street` as w_street order by Name ASC; "; //13 

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            combo_ds = new DataSet();
            da.Fill(combo_ds);
            Program.MyConn.Close();

            //Set_comboBox_datasource(Category_comboBox,          combo_ds.Tables[0]);
            //Set_comboBox_datasource(SubCategory_comboBox,       combo_ds.Tables[1]);
            //Set_comboBox_datasource(Career_comboBox,            combo_ds.Tables[2]);
            //Set_comboBox_datasource(Donor_comboBox,             combo_ds.Tables[3]);
            //Set_comboBox_datasource(DonorGroup_comboBox,        combo_ds.Tables[4]);
            //Set_comboBox_datasource(FundType_comboBox, combo_ds.Tables[5]);
            //Set_comboBox_datasource(Type_comboBox,              combo_ds.Tables[6]);
            //Set_comboBox_datasource(SubType_comboBox,           combo_ds.Tables[7]);
            //Set_comboBox_datasource(MaritalStatus_comboBox,     combo_ds.Tables[8]);
            //Set_comboBox_datasource(Age_comboBox,               combo_ds.Tables[9]);
            //Set_comboBox_datasource(HomeStreet_comboBox,        combo_ds.Tables[10]);
            //Set_comboBox_datasource(ProjectStreet_comboBox,     combo_ds.Tables[11]);
            //Set_comboBox_datasource(P_Parish_comboBox,          combo_ds.Tables[12]);

            Set_checkboxcomboBox_datasource(Category_comboBox, combo_ds.Tables[0]);
            Set_checkboxcomboBox_datasource(SubCategory_comboBox, combo_ds.Tables[1]);
            Set_checkboxcomboBox_datasource(Career_comboBox, combo_ds.Tables[2]);
            Set_checkboxcomboBox_datasource(Donor_comboBox, combo_ds.Tables[3]);
            Set_checkboxcomboBox_datasource(DonorGroup_comboBox, combo_ds.Tables[4]);
            Set_checkboxcomboBox_datasource(FundType_comboBox, combo_ds.Tables[5]);
            Set_checkboxcomboBox_datasource(Type_comboBox, combo_ds.Tables[6]);
            Set_checkboxcomboBox_datasource(SubType_comboBox, combo_ds.Tables[7]);
            Set_checkboxcomboBox_datasource(MaritalStatus_comboBox, combo_ds.Tables[8]);
            Set_checkboxcomboBox_datasource(Age_comboBox, combo_ds.Tables[9]);

            Set_checkboxcomboBox_datasource(MP_Status_comboBox, combo_ds.Tables[10]);
            Set_checkboxcomboBox_datasource(P_Parish_comboBox, combo_ds.Tables[11]);
            Set_checkboxcomboBox_datasource(HomeStreet_comboBox, combo_ds.Tables[12]);
            Set_checkboxcomboBox_datasource(ProjectStreet_comboBox, combo_ds.Tables[13]);



            ApplyDate_comboBox.Items.Clear();
            FundDate_comboBox.Items.Clear();

            for (int i = 2018; i <= DateTime.Today.Year; i++)
            {
                ApplyDate_comboBox.Items.Add(i.ToString());
                FundDate_comboBox.Items.Add(i.ToString());
            }

            ApplyDateTo_DateTimePicker.Value = FundedDateTo_DateTimePicker.Value = DateTime.Today;
        }
        private void Set_comboBox_datasource(ComboBox cBox, DataTable c_dt)
        {
            cBox.DataSource = null;
            cBox.DisplayMember = "Name";
            cBox.ValueMember = "ID";
            cBox.DataSource = c_dt;
            cBox.SelectedIndex = -1;
        }
        private void Set_checkboxcomboBox_datasource(CheckBoxComboBox cBox, DataTable c_dt)
        {
            cBox.DataSource = null;
            cBox.DataSource =
                new ListSelectionWrapper<DataRow>(
                    c_dt.Rows,
                    "Name" // "SomePropertyOrColumnName" will populate the Name on ObjectSelectionWrapper.
                    );
            cBox.DisplayMemberSingleItem = "Name";
            cBox.DisplayMember = "NameConcatenated";
            cBox.ValueMember = "Selected";
        }

        private void SubCategory_bind(string Categories)
        {
            user_mode = false;
            SubCategory_comboBox.DataSource = null;
            //SubCategory_comboBox.DisplayMember = "Name";
            //SubCategory_comboBox.ValueMember = "ID";

            if (Categories != "")
            {
                DataRow[] rows = null;
                string[] categoryArray = Categories.Split(',');
                rows = combo_ds.Tables[1].AsEnumerable()
                    .Where(row => categoryArray.Contains(row.Field<string>("Category_Name"))).ToArray();
                //rows = combo_ds.Tables[1].Select("Category_ID=" + Category_ID);

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[1].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(SubCategory_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(SubCategory_comboBox, combo_ds.Tables[1]);

            }
            SubCategory_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void Career_bind(string SubCategories)
        {
            user_mode = false;
            Career_comboBox.DataSource = null;
            //Career_comboBox.DisplayMember = "Name";
            //Career_comboBox.ValueMember = "ID";

            if (SubCategories != "")
            {
                DataRow[] rows = null;
                //rows = combo_ds.Tables[2].Select("SubCategory_ID=" + SubCategory_ID);
                string[] Array = SubCategories.Split(',');
                rows = combo_ds.Tables[2].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("SubCategory_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[2].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 && selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(Career_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(Career_comboBox, combo_ds.Tables[2]);

            }
            Career_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void SubType_bind(string Types)
        {
            user_mode = false;
            SubType_comboBox.DataSource = null;

            if (Types != "")
            {
                DataRow[] rows = null;
                string[] Array = Types.Split(',');
                rows = combo_ds.Tables[7].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("Type_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[7].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(SubType_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(SubType_comboBox, combo_ds.Tables[7]);

            }
            SubType_comboBox.SelectedIndex = -1;
            user_mode = true;
        }
        private void DonorGroup_bind(string Donors)
        {
            user_mode = false;
            DonorGroup_comboBox.DataSource = null;

            if (Donors != "")
            {
                DataRow[] rows = null;
                //rows = combo_ds.Tables[4].Select("Donor_ID=" + Donor_ID);
                string[] Array = Donors.Split(',');
                rows = combo_ds.Tables[4].AsEnumerable()
                    .Where(row => Array.Contains(row.Field<string>("Donor_Name"))).ToArray();

                DataTable selected_rows_dt = new DataTable();
                selected_rows_dt = combo_ds.Tables[4].Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                if (selected_rows_dt.Rows.Count != 0 || selected_rows_dt != null)
                    Set_checkboxcomboBox_datasource(DonorGroup_comboBox, selected_rows_dt);
            }
            else
            {
                Set_checkboxcomboBox_datasource(DonorGroup_comboBox, combo_ds.Tables[4]);

            }
            DonorGroup_comboBox.SelectedIndex = -1;
            user_mode = true;
        }

        private void DonorGroup_comboBox_Enter(object sender, EventArgs e)
        {
            if (Donor_comboBox.SelectedIndex != -1)
            {
                int donor_id = Convert.ToInt32(Donor_comboBox.SelectedValue);
                DonorGroup_bind(donor_id.ToString());
            }
            else
            {
                // set the default one //
                DonorGroup_comboBox.DataSource = null;
                DonorGroup_comboBox.DisplayMember = "Name";
                DonorGroup_comboBox.ValueMember = "ID";
                DonorGroup_comboBox.DataSource = combo_ds.Tables[4];
            }
        }
        private void SubCategory_comboBox_Enter(object sender, EventArgs e)
        {
            if (Category_comboBox.SelectedIndex != -1)
            {
                int id = Convert.ToInt32(Category_comboBox.SelectedValue);
                SubCategory_bind(id.ToString());
            }
            else
            {
                // set the default one //
                SubCategory_comboBox.DataSource = null;
                SubCategory_comboBox.DisplayMember = "Name";
                SubCategory_comboBox.ValueMember = "ID";
                SubCategory_comboBox.DataSource = combo_ds.Tables[1];
            }
        }
        private void Career_comboBox_Enter(object sender, EventArgs e)
        {
            if (SubCategory_comboBox.SelectedIndex != -1)
            {
                int id = Convert.ToInt32(SubCategory_comboBox.SelectedValue);
                Career_bind(id.ToString());
            }
            else
            {
                // set the default one //
                Career_comboBox.DataSource = null;
                Career_comboBox.DisplayMember = "Name";
                Career_comboBox.ValueMember = "ID";
                Career_comboBox.DataSource = combo_ds.Tables[2];
            }
        }
        private void Type_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    SubType_bind(Type_comboBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Donor_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //add the type_comboBox_selectedValue_changed to fill the data before binding subType  
            try
            {
                if (user_mode)
                    DonorGroup_bind(Donor_comboBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region chart methods 
        private void SetChartAreaProperties(int max_value)
        {
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#,#";
        }

        private void Build_ChartArea(int index)
        {
            try
            {
                var chartArea1 = new ChartArea();
                chartArea1.AxisY.LabelStyle.Font = new Font("Avenir LT Std 65 Bold", 12f, FontStyle.Regular);
                chartArea1.AxisX.LabelStyle.Font = new Font("Avenir LT Std 65 Bold", 12f, FontStyle.Regular);
                chartArea1.AxisX.IsLabelAutoFit = true;
                chartArea1.AxisX.LabelAutoFitMaxFontSize = 11;
                chartArea1.AxisX.LabelAutoFitMinFontSize = 8;
                chartArea1.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;

                chart1.ChartAreas.Insert(index, chartArea1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Build_Legend(int ChartAreas_index)
        {
            try
            {
                var legend1 = new Legend();
                legend1.Font = new Font("Avenir LT Std 65 Medium", 10f, FontStyle.Regular);
                legend1.TitleAlignment = StringAlignment.Center;
                legend1.DockedToChartArea = chart1.ChartAreas[ChartAreas_index].Name;
                legend1.Docking = Docking.Top;
                legend1.LegendStyle = LegendStyle.Table;
                legend1.TableStyle = LegendTableStyle.Wide;
                legend1.IsEquallySpacedItems = true;
                legend1.IsDockedInsideChartArea = false;
                legend1.Alignment = StringAlignment.Far;

                if (Settings.Default.theme == "Light")
                {
                    legend1.BackColor = Color.FromArgb(189, 189, 189);
                    legend1.ForeColor = Color.Black;
                }
                else
                {
                    legend1.ForeColor = Color.White;
                    legend1.BackColor = Color.FromArgb(75, 75, 75);
                }

                chart1.Legends.Add(legend1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Build_Series(string Name)
        {
            try
            {
                Series s1 = new Series();
                s1.ShadowColor = Color.FromArgb(128, 128, 128);
                s1.ShadowOffset = 1;
                s1.IsXValueIndexed = true;
                s1.Name = Name;
                s1.XValueType = ChartValueType.Auto;
                s1.YValuesPerPoint = 1;
                s1.YValueType = ChartValueType.Auto;
                s1.IsValueShownAsLabel = true;
                s1.Font = new Font("Century", 10f, FontStyle.Bold);
                s1.IsVisibleInLegend = true;
                s1.CustomProperties = "DrawSideBySide=True" +
                    ",MinPixelPointWidth=40" +
                    ",MaxPixelPointWidth=80" +
                    ",LabelStyle=Top" +
                    ",EmptyPointValue=Zero";

                chart1.Series.Add(s1);
                chart1.Series[Name].ChartType = Chart_type;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Fill_ChartArea(string Name)
        {
            if(X_Axis_comboBox.SelectedIndex == 14)
                Build_Series("Ongoing projects - " + Name);
            else 
                Build_Series("all projects - " + Name);

            //not profit rate , success rate
            if (X_Axis_comboBox.SelectedIndex != 11
             && X_Axis_comboBox.SelectedIndex != 12
             && X_Axis_comboBox.SelectedIndex != 13)
            {
                if (X_Axis_comboBox.SelectedIndex == 14)
                    Build_Series("Closed projects - " + Name);
                else 
                    Build_Series("funded projects - " + Name);
            }


            if (Chart_type == SeriesChartType.Doughnut)
            {
                chart1.Palette = ChartColorPalette.BrightPastel;
                chart1.Legends[0].Enabled = false;
                chart1.Legends[1].Enabled = true;
            }
            else
            {
                chart1.Palette = ChartColorPalette.None;
                chart1.PaletteCustomColors = new Color[] { Color.FromArgb(192, 0, 0), Color.Gray };
                chart1.Legends[0].Enabled = true;
                chart1.Legends[1].Enabled = false;
            }

            //if there is more than chartarea specially for doughnut type
            for (int i = 0; i < chart1.ChartAreas.Count; i++)
            {
                chart1.Series[i].ChartArea = chart1.ChartAreas[i].Name;
                chart1.Series[i].Legend = chart1.Legends[i].Name;
            }

            var max_value = 0;
            for (var i = 0; i < results_dataGridView.Rows.Count; i++)
            {
                //fill chart
                if (X_Axis_comboBox.SelectedIndex == 14)
                    chart1.Series["Ongoing projects - " + Name].Points.AddXY(
                        results_dataGridView.Rows[i].Cells[0].Value.ToString(), Convert.ToInt32(results_dataGridView.Rows[i].Cells[1].Value.ToString()));
                else
                    chart1.Series["all projects - " + Name].Points.AddXY(
                    results_dataGridView.Rows[i].Cells[0].Value.ToString(), Convert.ToInt32(results_dataGridView.Rows[i].Cells[1].Value.ToString()));

                //not profit rate ,success rate
                if (X_Axis_comboBox.SelectedIndex != 11
                 && X_Axis_comboBox.SelectedIndex != 12
                 && X_Axis_comboBox.SelectedIndex != 13)
                {
                    if (X_Axis_comboBox.SelectedIndex == 14) 
                        chart1.Series["Closed projects - " + Name].Points.AddXY(
                            results_dataGridView.Rows[i].Cells[0].Value.ToString(), Convert.ToInt32(results_dataGridView.Rows[i].Cells[2].Value.ToString()));
                    else
                        chart1.Series["funded projects - " + Name].Points.AddXY(
                            results_dataGridView.Rows[i].Cells[0].Value.ToString(), Convert.ToInt32(results_dataGridView.Rows[i].Cells[2].Value.ToString()));
                }
                //save the maximum value
                if (Convert.ToInt32(results_dataGridView.Rows[i].Cells[1].Value) > max_value)
                    max_value = Convert.ToInt32(results_dataGridView.Rows[i].Cells[1].Value);
            }

            SetChartAreaProperties(max_value);

            //color chart
            if (Settings.Default.theme == "Dark")
                newTheme.Statistics_ToNight(this, Hided);
            else
                newTheme.Statistics_ToLight(this, Hided);
        }
        #endregion

        #region mouse hover

        private void AdvancedSearch_button_MouseEnter(object sender, EventArgs e)
        {
            if (Hided)
                AdvancedSearch_button.BackgroundImage = Resources.Show2_D;
            else
                AdvancedSearch_button.BackgroundImage = Resources.Hide2_D;
        }

        private void AdvancedSearch_button_MouseLeave(object sender, EventArgs e)
        {
            if (Hided)
                AdvancedSearch_button.BackgroundImage = Resources.Show2_L;
            else
                AdvancedSearch_button.BackgroundImage = Resources.Hide2_L;
        }

        private void ExportToExcel_button_MouseEnter(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_L;
        }

        private void ExportToExcel_button_MouseLeave(object sender, EventArgs e)
        {
            ExportToExcel_button.BackgroundImage = Resources.Excel_D;
        }

        private void ApplyFilter_button_MouseEnter(object sender, EventArgs e)
        {
            ApplyFilter_button.BackgroundImage = Resources.Apply0;
        }

        private void ApplyFilter_button_MouseLeave(object sender, EventArgs e)
        {
            ApplyFilter_button.BackgroundImage = Resources.Apply1;
        }
        #endregion

        #region right click menu

        private void refreshPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_Axis_comboBox_SelectedIndexChanged(sender, e);
        }

        private void showHideFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdvancedSearch_button_Click(sender, e);
        }

        private void clearAllFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            user_mode = false;
            FundType_comboBox.Text = Type_comboBox.Text
                = SubType_comboBox.Text = Partnership_comboBox.Text
                = MP_Status_comboBox.Text = Donor_comboBox.Text
                = DonorGroup_comboBox.Text = Career_comboBox.Text
                = ApplyDate_comboBox.Text = FundDate_comboBox.Text
                = Category_comboBox.Text = SubCategory_comboBox.Text
                = P_Parish_comboBox.Text = Age_comboBox.Text
                = Gender_comboBox.Text = MaritalStatus_comboBox.Text
                = HomeStreet_comboBox.Text = ProjectStreet_comboBox.Text = "";

            FundedDate_checkBox.Checked = ApplyDate_checkBox.Checked = false;
            //Funded_checkBox.Checked = Requested_checkBox.Checked = 

            RequestedFrom_textBox.Text = RequestedTo_textBox.Text
                = FundedFrom_textBox.Text = FundedTo_textBox.Text = "";

            ApplyDate_condition = FundedDate_condition
                = RequestedAmount_condition = FundedAmount_condition = "";

            user_mode = true;

            X_Axis_comboBox_SelectedIndexChanged(sender, e);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = s.Select_Applications(condition, false);
                using (var ShowFamilyMembers = new ShowDetails_Form(ds.Tables[0], "المستفيدين"))
                {
                    ShowFamilyMembers.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        //  string FundType, string Type, string SubType
        //, string Partners, string State
        //, string Donor, string DonorGroup
        //, string Category, string SubCategory
        //, string Parish, string Age, string Sex, string MaritalStatus
        //, string ApplyDate, string FundDate
        //, string ApplyDate_Condition, string FundedDate_Condition
        //, string HomeStreet,string ProjectStreet


        #region requested and fund date 
        private void ApplyDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (ApplyDate_checkBox.Checked)
                {
                    ApplyDateFrom_DateTimePicker.Enabled = ApplyDateFrom_label.Enabled
                    = ApplyDateTo_DateTimePicker.Enabled = true;
                    //= ApplyDateTo_label.Enabled

                    //ApplyDateFrom_dateTimePicker_ValueChanged(sender, e);
                }
                else
                {
                    ApplyDate_condition = "";
                    ApplyDateFrom_DateTimePicker.Enabled = ApplyDateFrom_label.Enabled
                    = ApplyDateTo_DateTimePicker.Enabled = false;
                    //= ApplyDateTo_label.Enabled 

                    //X_Axis_comboBox_SelectedIndexChanged(sender, e);
                }
            }
        }
        private void FundedDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (user_mode)
            {
                if (FundedDate_checkBox.Checked)
                {
                    FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled
                        = FundedDateTo_DateTimePicker.Enabled = true;
                    // = FundedDateTo_label.Enabled 
                    //FundedDateTo_bcDateTimePicker_ValueChanged(sender, e);
                }
                else
                {
                    FundedDate_condition = "";
                    FundedDateFrom_DateTimePicker.Enabled = FundedDateFrom_label.Enabled
                        = FundedDateTo_DateTimePicker.Enabled = false;
                    // = FundedDateTo_label.Enabled 
                    //X_Axis_comboBox_SelectedIndexChanged(sender, e);
                }
            }
        }

        //private void Requested_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (user_mode)
        //    {
        //        if (Requested_checkBox.Checked)
        //            RequestedFrom_label.Enabled = RequestedFrom_textBox.Enabled
        //                //= RequestedTo_label.Enabled
        //                = RequestedTo_textBox.Enabled = true;
        //        else
        //            RequestedFrom_label.Enabled = RequestedFrom_textBox.Enabled
        //                //= RequestedTo_label.Enabled
        //                = RequestedTo_textBox.Enabled = false;
        //    }
        //}

        //private void Funded_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (user_mode)
        //    {
        //        if (Funded_checkBox.Checked)
        //            FundedFrom_label.Enabled = FundedFrom_textBox.Enabled
        //                //= FundedTo_label.Enabled 
        //                = FundedTo_textBox.Enabled = true;
        //        else
        //            FundedFrom_label.Enabled = FundedFrom_textBox.Enabled
        //                //= FundedTo_label.Enabled 
        //                = FundedTo_textBox.Enabled = false;
        //    }
        //}
        private void ApplyDateFrom_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (ApplyDateFrom_DateTimePicker.Value > ApplyDateTo_DateTimePicker.Value)
                    {
                        ApplyDate_condition = mevisitDate_condition = "";
                        throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");
                    }

                    string beg, end;
                    beg = ApplyDateFrom_DateTimePicker.Value.ToString("yyyy/MM/dd");
                    end = ApplyDateTo_DateTimePicker.Value.ToString("yyyy/MM/dd");

                    ApplyDate_condition = " and ( MP_DateOfRequest between '" + beg + "' and '" + end + "' )";
                    mevisitDate_condition = " and PMP.Person_ID in " +
                        " (select mevisit.Person_ID from mevisit " +
                        " where mevisit.Date between '" + beg + "' and '" + end + "' )";

                    //X_Axis_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FundedDateTo_bcDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    if (FundedDateFrom_DateTimePicker.Value > FundedDateTo_DateTimePicker.Value)
                        throw new InvalidOperationException("لا يمكن أن يكون تاريخ البداية أكبر من تاريخ النهاية");
                    if (user_mode)
                    {
                        DateTime beg, end;
                        beg = FundedDateFrom_DateTimePicker.Value;
                        end = FundedDateTo_DateTimePicker.Value;

                        FundedDate_condition = " and MP_ID in (select MicroProject_ID from loan where " +
                                               " Loan_DateTaken between '" + beg.Year + "/" + beg.Month + "/" + beg.Day + "' " +
                                               " and '" + end.Year + "/" + end.Month + "/" + end.Day + "' )";

                        //X_Axis_comboBox_SelectedIndexChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RequestedFrom_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RequestedFrom_textBox.Text != "")
                {
                    RequestedFrom_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(RequestedFrom_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    RequestedFrom_textBox.SelectionStart = RequestedFrom_textBox.Text.Length;
                    RequestedFrom_textBox.SelectionLength = 0;
                }

                if (RequestedTo_textBox.Text != "")
                {
                    RequestedTo_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(RequestedTo_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    RequestedTo_textBox.SelectionStart = RequestedTo_textBox.Text.Length;
                    RequestedTo_textBox.SelectionLength = 0;
                }

                if (FundedFrom_textBox.Text != "")
                {
                    FundedFrom_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(FundedFrom_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    FundedFrom_textBox.SelectionStart = FundedFrom_textBox.Text.Length;
                    FundedFrom_textBox.SelectionLength = 0;
                }

                if (FundedTo_textBox.Text != "")
                {
                    FundedTo_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDecimal(FundedTo_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    FundedTo_textBox.SelectionStart = FundedTo_textBox.Text.Length;
                    FundedTo_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RequestedFrom_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                RequestedAmount_condition = "";
                if (RequestedFrom_textBox.Text != "" || RequestedTo_textBox.Text != "")
                {
                    var NeededFrom = RequestedFrom_textBox.Text.Replace(",", "");
                    var NeededTo = RequestedTo_textBox.Text.Replace(",", "");

                    double Needed_from_d, Needed_to_d;
                    if (NeededFrom == "")
                        Needed_from_d = 0;
                    else Needed_from_d = Convert.ToDouble(NeededFrom);

                    if (NeededTo == "")
                        Needed_to_d = 0;
                    else Needed_to_d = Convert.ToDouble(NeededTo);
                    RequestedAmount_condition = " and (`MP_RequestedAmount` BETWEEN "
                        + Needed_from_d + " and " + Needed_to_d + ") ";

                    //SearchBy_comboBox_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FundedFrom_textBox_Leave(object sender, EventArgs e)
        {
            FundedAmount_condition = "";
            if (FundedFrom_textBox.Text != "" || FundedTo_textBox.Text != "")
            {
                var FinancedFrom = FundedFrom_textBox.Text.Replace(",", "");
                var FinancedTo = FundedTo_textBox.Text.Replace(",", "");

                double Financed_from_d, Financed_To_d;
                if (FinancedFrom == "")
                    Financed_from_d = 0;
                else Financed_from_d = Convert.ToDouble(FinancedFrom);

                if (FinancedTo == "")
                    Financed_To_d = 0;
                else Financed_To_d = Convert.ToDouble(FinancedTo);
                FundedAmount_condition +=
                   // " and (L.Loan_Amount BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";
                   @" and ( Round( (CASE Currency 
   WHEN 'SP'  THEN Loan_Amount / ExchangeRate
   WHEN 'USD' THEN Loan_Amount 
   END) , 2 ) BETWEEN " + Financed_from_d + " and " + Financed_To_d + ") ";

                //SearchBy_comboBox_SelectedIndexChanged(sender, e);
            }
        }

        #endregion

        private void WithFunded_checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (withFunded_checkBox.CheckState == CheckState.Checked) //العمودين
            {
                chart1.Series[0].IsVisibleInLegend = chart1.Series[0].Enabled = true;
                chart1.Series[1].IsVisibleInLegend = chart1.Series[1].Enabled = true;

                withFunded_checkBox.Text = "The Columns: All + Funded";
            }
            else if (withFunded_checkBox.CheckState == CheckState.Unchecked) // الكل فقط
            {
                chart1.Series[0].IsVisibleInLegend = chart1.Series[0].Enabled = true;
                chart1.Series[1].IsVisibleInLegend = chart1.Series[1].Enabled = false;

                withFunded_checkBox.Text = "The Columns: All Projects";
            }
            else if (withFunded_checkBox.CheckState == CheckState.Indeterminate) // fund فقط
            {
                chart1.Series[0].IsVisibleInLegend = chart1.Series[0].Enabled = false;
                chart1.Series[1].IsVisibleInLegend = chart1.Series[1].Enabled = true;

                withFunded_checkBox.Text = "The Columns: Funded Projects";
            }
        }



    }
}