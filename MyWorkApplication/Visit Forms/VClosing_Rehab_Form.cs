using MyWorkApplication.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using MyWorkApplication.Properties;

namespace MyWorkApplication.Visit_Forms
{
    public partial class VClosing_Rehab_Form : Form
    {
        private const string _KIND = "R-cl"; //rehab
        private Log l;
        private readonly MainForm mainForm;
        private int MicroProject_ID, V_ID, User_ID, Person_ID;
        private MySqlComponents MySS;
        private NewTheme NewTheme;
        private UserNotification noti; 
        private Select s;
        private TasksOfProjects TasksOfProjects;
 
        private VisitRehab v;
        private User user;
        private DataTable Visit_Users_dt;
        private readonly List<int> User_IDs = new List<int>();
        private bool user_mode;
        private string Type;

        #region //Data
        string Ans1, Ans2, Ans3, Ans4, Ans5, Ans6, Ans7, Ans8, Ans9, Ans10, Ans11, Ans12, Ans13, Ans14, Ans15
             , Vistor1, Vistor2, Vistor3, Vistor4;

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            { 
                DialogResult dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف معلومات هذه الزيارة بشكل نهائي؟", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    v.Delete_Visit(V_ID, MicroProject_ID);

                    MessageBox.Show("تم حذف الزيارة بنجاح", "Confirmation");
                    Set_Controls();
                }

            }
            catch (Exception ex)
            {
                if (Program.MyConn != null && Program.MyConn.State == ConnectionState.Open)
                {
                    Program.MyConn.Close();
                }
                if (ex.Message.Contains("Cannot delete or update a parent row"))
                    MessageBox.Show("لا يمكن حذف هذه العائلة بسبب ارتباطها إما بتوزيعة قديمة أو بعائلة أخرى", "Error");
                else
                    if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        string CreatedBy, EditedBy, visitDate;

        int? Ans1_Value, Ans2_Value, Ans3_Value, Ans4_Value, Ans5_Value, Ans6_Value
            , Ans7_Value, Ans8_Value, Ans9_Value, Ans10_Value, Ans11_Value, Ans12_Value
            , Ans13_Value, Ans14_Value, Ans15_Value;

        int? user1_id, user2_id, user3_id, user4_id;

        string Visit_Query, Answers_Query, Visitors_Query;
        #endregion

        #region //IsDirty
        bool General_Information_IsDirty = false
            , Answers_IsDirty = false
            , Visitors_IsDirty = false;
        #endregion

        bool DataApproved = false;
        bool Update_Mode = false; 

        public VClosing_Rehab_Form()
        {
            InitializeComponent();
        }
        public VClosing_Rehab_Form(MainForm mainForm, int MicroProject_ID)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            V_ID = -1;
            Update_Mode = false;
        }
        public VClosing_Rehab_Form(int V_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.V_ID = V_ID;
            Update_Mode = true;
        }

        private void VClosing_Rehab_Form_Load(object sender, EventArgs e)
        {
            try
            {
                user_mode = false;
                
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.Visit_ToLight(this);
                else
                    NewTheme.Visit_ToNight(this);

                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Rehab. Closing Visit";

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                v = new VisitRehab();
                user = new User();
                

                switch (Program.role)
                {
                    case 1: //admin 
                    case 8: //admin_l0 
                        break;
                    case 2: //Data
                        {
                            addNotesToolStripMenuItem.Visible = false;
                        }
                        break;
                    case 3: //Financial 
                    case 4: //Guest
                    case 7: //Lawful
                    case 9: //Media
                        {
                            application_toolStripMenuItem.Visible
                            = addNotesToolStripMenuItem.Visible = false;
                        }
                        break;
                }

                //////////////////// handle auto scrolling for all comboBoxes //////////////////////
                //VisitType_comboBox1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel); 
                V1_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V2_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V3_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V4_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries(_KIND, "Yes");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs(_KIND, "Yes");

                users_bind();

                if (V_ID != -1)
                {
                    DataSet v_ds = v.Get_Visit(V_ID.ToString(), "", "");
                    Fill_Visit(v_ds); 
                }
                else if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    DataSet v_ds = v.Get_Visit("", "", MicroProject_ID_textBox.Text);
                    Fill_Visit(v_ds);
                    //user_mode = true;
                    //MicroProject_ID_textBox1_Leave(sender, e);
                }

                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                user_mode = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MicroProject_ID_textBox1_Leave(object sender, EventArgs e)
        {
            //if (user_mode)
            //{
            //    if (MicroProject_ID_textBox.Text != "")
            //    {
            //        Set_Controls();
            //        DataSet v_ds = v.Get_Visit("", "", MicroProject_ID_textBox.Text);
            //        Fill_Visit(v_ds);
            //    }
            //}
        }

        private void Fill_Visit(DataSet ds1)
        {
            try
            { 
                if (ds1 != null)
                {
                    user_mode = false;
                    MicroProject_ID = int.Parse(ds1.Tables[0].Rows[0]["MicroProject_ID"].ToString());
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    Person_Name_textBox.Text = (string)ds1.Tables[0].Rows[0]["Beneficiary Name"]; 
                    Person_ID = int.Parse(ds1.Tables[0].Rows[0]["Beneficiary_ID"].ToString());

                    if (ds1.Tables[0].Rows[0]["Loan Amount"] == DBNull.Value)
                        MessageBox.Show("لم يتم إدخال مبلغ القرض لهذا المشروع", "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        var dd = Convert.ToDouble(ds1.Tables[0].Rows[0]["Loan Amount"].ToString());
                        LoanAmount_textBox1.Text = dd.ToString("#,##0");
                    }

                    CreatedBy_User_label.Text = ds1.Tables[0].Rows[0]["Created_By"].ToString();
                    EditedBy_User_label.Text  = ds1.Tables[0].Rows[0]["Edited_By"].ToString();

                     
                    //check if there is a visit
                    if (ds1.Tables[0].Rows[0]["ID"] == DBNull.Value
                        || ds1.Tables[0].Rows[0]["ID"] == null)
                    {
                        Update_Mode = false;
                        //MessageBox.Show("انتبه! لم يتم إدخال زيارة لهذا المشروع.");
                    }
                    else
                    {
                        V_ID = int.Parse(ds1.Tables[0].Rows[0]["ID"].ToString());

                        var date1 = (DateTime)ds1.Tables[0].Rows[0]["Date"];
                        VisitDate_dateTimePicker.Value = date1;

                        Ans1_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans1"];
                        Ans2_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans2"];
                        Ans3_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans3"];
                        Ans4_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans4"];
                        Ans5_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans5"];
                        Ans6_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans6"];
                        Ans7_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans7"];
                        Ans8_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans8"];
                        Ans9_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans9"];
                        Ans10_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans10"];
                        Ans11_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans11"];
                        Ans12_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans12"];
                        Ans13_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans13"];
                        Ans14_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans14"];
                        Ans15_textBox.Text = (string)ds1.Tables[0].Rows[0]["Ans15"];

                        for (int i = 1; i <= 15; i++)
                        {
                            set_radioButtons(i, Convert.ToInt32(ds1.Tables[0].Rows[0]["Ans" + i + "_Value"]));
                        }

                        V1_comboBox.Text = V2_comboBox.Text =
                            V3_comboBox.Text = V4_comboBox.Text = "";
                        V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex =
                            V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1;

                        //Visit_Users_dt = ds1.Tables[1];
                        if (ds1.Tables[1].Rows.Count != 0)
                            for (var i = 0; i < ds1.Tables[1].Rows.Count; i++)
                            {
                                if (ds1.Tables[1].Rows[i][0] == null || ds1.Tables[1].Rows[i][0] == DBNull.Value) continue;

                                var u_id = ds1.Tables[1].Rows[i].Field<int>(0);
                                if (u_id != 0)
                                {
                                    var V = "V";
                                    var VV = "_comboBox";
                                    var fullName = V + (i + 1) + VV;
                                    var cbx = Controls.Find(fullName, true).FirstOrDefault() as ComboBox;
                                    if(cbx != null)
                                        cbx.SelectedValue = u_id;
                                }
                            }
                        Update_Mode = true;
                    }
                      
                    user_mode = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                user_mode = true;
            }
        }

        #region set get radio_buttons
        void set_radioButtons(int ansNo, int value)
        {
            string radio_name = "";
             
            if (value == -1)
            {
                radio_name = "ans" + ansNo + "_" + "N" + "_radioButton"; 
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if(rad_bx == null) //not found => make all radios unchecked
                {
                    string table_name = "";
                    table_name = "ans" + ansNo + "_tableLayoutPanel";
                    var table = Controls.Find(table_name, true).FirstOrDefault() as TableLayoutPanel;
                    foreach (RadioButton r in table.Controls.OfType<RadioButton>())
                        r.Checked = false;
                }
                else rad_bx.Checked = true; //found 
            }
            else
            {
                radio_name = "ans" + ansNo + "_" + value + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                rad_bx.Checked = true;
            }
        }
 
        int get_radioButtons_values(int ansNo)
        {
            int value = -1;
            string table_name = "ans" + ansNo + "_tableLayoutPanel";
            var table = Controls.Find(table_name, true).FirstOrDefault() as TableLayoutPanel;
            foreach (RadioButton r in table.Controls.OfType<RadioButton>())
            {
                if (r.Checked)
                {
                    string radio_name = "";
                    radio_name = r.Name;

                    int _index = radio_name.LastIndexOf("_");

                    string num = radio_name.Substring(_index - 1, 1);
                    if (num == "N") value = -1;
                    else value = Convert.ToInt32(num);
                }
            }
            return value;
        } 
        #endregion
         
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void users_bind()
        {
            try
            {
                var dt = s.select_visitors();
                V1_comboBox.DataSource = dt;
                V2_comboBox.BindingContext = new BindingContext();
                V2_comboBox.DataSource = dt;
                V3_comboBox.BindingContext = new BindingContext();
                V3_comboBox.DataSource = dt;
                V4_comboBox.BindingContext = new BindingContext();
                V4_comboBox.DataSource = dt;

                V1_comboBox.DisplayMember = V2_comboBox.DisplayMember =
                V3_comboBox.DisplayMember = V4_comboBox.DisplayMember = "Visitors";

                V1_comboBox.ValueMember = V2_comboBox.ValueMember =
                V3_comboBox.ValueMember = V4_comboBox.ValueMember = "U_ID";

                V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex =
                V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                DataApproved = Check_Data();

                if (DataApproved)
                {
                    Get_Data();
                    if (Update_Mode)
                    {
                        v.Update_Visit(V_ID, MicroProject_ID, Visit_Query, Answers_Query, Visitors_Query);
                        MessageBox.Show("تم تعديل الزيارة بنجاح", "Confirmation");

                    }
                    else
                    { 
                        V_ID = v.Insert_Visit(MicroProject_ID, _KIND, visitDate, CreatedBy, Answers_Query, Visitors_Query); 
                        Update_Mode = true; 
                        MessageBox.Show("تم إدخال الزيارة بنجاح", "Confirmation"); 
                    }
                      
                    //Set_Controls(); 
                    //DataSet v_ds = v.Get_Visit(V_ID.ToString(), "", "");
                    //Fill_Visit(v_ds);  
                }

            }
            catch (Exception ex)
            {
                Answers_Query = Visitors_Query = "";
                if (Program.MyConn != null && Program.MyConn.State == ConnectionState.Open)
                {
                    Program.MyConn.Close();
                }
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }
         
        private bool Check_Data()
        {
            if (string.IsNullOrWhiteSpace(MicroProject_ID_textBox.Text))
            {
                MessageBox.Show("الرجاء اختيار مشروع أولاً", "Error");
                return false;
            } 
              
            foreach (Control c in answers_tableLayoutPanel.Controls)
            {
                if (c.GetType() == typeof(TableLayoutPanel) && c.Name.Contains("ans")) /// inner table
                {
                    bool Answered = false;
                    foreach (Control rad in c.Controls )
                    { 
                        if (rad.GetType() == typeof(RadioButton)) /// answers radiobuttons
                        { 
                            var radio = rad as RadioButton;
                            if (radio.Checked)
                            {
                                Answered = true; break;
                            }
                        }
                    }
                    if(Answered == false)
                    {
                        MessageBox.Show("الرجاء التأكد من أنك قمت باختيار جميع الاجابات", "Error");
                        return false;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(V1_comboBox.Text) &&
                string.IsNullOrWhiteSpace(V2_comboBox.Text) &&
                string.IsNullOrWhiteSpace(V3_comboBox.Text) &&
                string.IsNullOrWhiteSpace(V4_comboBox.Text) )
            {
                MessageBox.Show("الرجاء التأكد من اختيار زائر واحد على الأقل", "Error");
                return false;
            }

            return true;
        }
        private void Get_Data()
        {
            #region
            if (!Update_Mode)
            {
                CreatedBy = Program.UserName;
                CreatedBy_User_label.Text = Program.UserName;
                visitDate = VisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd");

                Ans1 = Ans1_textBox.Text == null ? "" : Ans1_textBox.Text;
                Ans2 = Ans2_textBox.Text == null ? "" : Ans2_textBox.Text;
                Ans3 = Ans3_textBox.Text == null ? "" : Ans3_textBox.Text;
                Ans4 = Ans4_textBox.Text == null ? "" : Ans4_textBox.Text;
                Ans5 = Ans5_textBox.Text == null ? "" : Ans5_textBox.Text;
                Ans6 = Ans6_textBox.Text == null ? "" : Ans6_textBox.Text;
                Ans7 = Ans7_textBox.Text == null ? "" : Ans7_textBox.Text;
                Ans8 = Ans8_textBox.Text == null ? "" : Ans8_textBox.Text;
                Ans9 = Ans9_textBox.Text == null ? "" : Ans9_textBox.Text;
                Ans10 = Ans10_textBox.Text == null ? "" : Ans10_textBox.Text;
                Ans11 = Ans11_textBox.Text == null ? "" : Ans11_textBox.Text;
                Ans12 = Ans12_textBox.Text == null ? "" : Ans12_textBox.Text;
                Ans13 = Ans13_textBox.Text == null ? "" : Ans13_textBox.Text;
                Ans14 = Ans14_textBox.Text == null ? "" : Ans14_textBox.Text;
                Ans15 = Ans15_textBox.Text == null ? "" : Ans15_textBox.Text;

                Ans1_Value = get_radioButtons_values(1);
                Ans2_Value = get_radioButtons_values(2);
                Ans3_Value = get_radioButtons_values(3);
                Ans4_Value = get_radioButtons_values(4);
                Ans5_Value = get_radioButtons_values(5);
                Ans6_Value = get_radioButtons_values(6);
                Ans7_Value = get_radioButtons_values(7);
                Ans8_Value = get_radioButtons_values(8);
                Ans9_Value = get_radioButtons_values(9);
                Ans10_Value = get_radioButtons_values(10);
                Ans11_Value = get_radioButtons_values(11);
                Ans12_Value = get_radioButtons_values(12);
                Ans13_Value = get_radioButtons_values(13);
                Ans14_Value = get_radioButtons_values(14);
                Ans15_Value = get_radioButtons_values(15);

                Answers_Query += "(@Visit_ID,N'" + Ans1 + "',N'" + Ans2 + "',N'" + Ans3 + "',N'" + Ans4 + "',N'" + Ans5 + "'"
                    + ",N'" + Ans6 + "',N'" + Ans7 + "',N'" + Ans8 + "',N'" + Ans9 + "',N'" + Ans10 + "'"
                    + ",N'" + Ans11 + "',N'" + Ans12 + "',N'" + Ans13 + "',N'" + Ans14 + "',N'" + Ans15 + "'"
                    + "," +Ans1_Value +","+ Ans2_Value + "," + Ans3_Value + "," + Ans4_Value + "," + Ans5_Value 
                    + "," + Ans6_Value + "," + Ans7_Value + "," + Ans8_Value + "," + Ans9_Value + "," + Ans10_Value 
                    + "," + Ans11_Value + "," + Ans12_Value + "," + Ans13_Value + "," + Ans14_Value + "," + Ans15_Value +")";

            }
            #endregion

            #region
            else if (Update_Mode)
            { 
                EditedBy = Program.UserName;
                EditedBy_User_label.Text = Program.UserName;
                visitDate = VisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd");

                if(V_ID != -1)
                    Visit_Query += " UPDATE `visit` set Edited_By = '" + EditedBy + "'" +
                        ", Date = '" + visitDate + "'" +  
                        "  where ID = " + V_ID + ";"; 

                Ans1 = Ans1_textBox.Text == null ? "" : Ans1_textBox.Text;
                Ans2 = Ans2_textBox.Text == null ? "" : Ans2_textBox.Text;
                Ans3 = Ans3_textBox.Text == null ? "" : Ans3_textBox.Text;
                Ans4 = Ans4_textBox.Text == null ? "" : Ans4_textBox.Text;
                Ans5 = Ans5_textBox.Text == null ? "" : Ans5_textBox.Text;
                Ans6 = Ans6_textBox.Text == null ? "" : Ans6_textBox.Text;
                Ans7 = Ans7_textBox.Text == null ? "" : Ans7_textBox.Text;
                Ans8 = Ans8_textBox.Text == null ? "" : Ans8_textBox.Text;
                Ans9 = Ans9_textBox.Text == null ? "" : Ans9_textBox.Text;
                Ans10 = Ans10_textBox.Text == null ? "" : Ans10_textBox.Text;
                Ans11 = Ans11_textBox.Text == null ? "" : Ans11_textBox.Text;
                Ans12 = Ans12_textBox.Text == null ? "" : Ans12_textBox.Text;
                Ans13 = Ans13_textBox.Text == null ? "" : Ans13_textBox.Text;
                Ans14 = Ans14_textBox.Text == null ? "" : Ans14_textBox.Text;
                Ans15 = Ans15_textBox.Text == null ? "" : Ans15_textBox.Text;

                Ans1_Value = get_radioButtons_values(1);
                Ans2_Value = get_radioButtons_values(2);
                Ans3_Value = get_radioButtons_values(3);
                Ans4_Value = get_radioButtons_values(4);
                Ans5_Value = get_radioButtons_values(5);
                Ans6_Value = get_radioButtons_values(6);
                Ans7_Value = get_radioButtons_values(7);
                Ans8_Value = get_radioButtons_values(8);
                Ans9_Value = get_radioButtons_values(9);
                Ans10_Value = get_radioButtons_values(10);
                Ans11_Value = get_radioButtons_values(11);
                Ans12_Value = get_radioButtons_values(12);
                Ans13_Value = get_radioButtons_values(13);
                Ans14_Value = get_radioButtons_values(14);
                Ans15_Value = get_radioButtons_values(15);

                Answers_Query += " UPDATE `rehabvisit` set Ans1 = '" + Ans1 + "', Ans2 = '" + Ans2 
                    + "', Ans3 = '" + Ans3 + "', Ans4 = '" + Ans4 + "', Ans5 = '" + Ans5 
                    + "', Ans6 = '" + Ans6 + "', Ans7 = '" + Ans7 + "', Ans8 = '" + Ans8 
                    + "', Ans9 = '" + Ans9 + "', Ans10 = '" + Ans10 + "', Ans11 = '" + Ans11 
                    + "', Ans12 = '" + Ans12 + "',  Ans13 = '" + Ans13 + "', Ans14 = '" + Ans14  + "', Ans15 = '" + Ans15 
                        
                    + "', Ans1_Value = " + Ans1_Value + " , Ans2_Value = " + Ans2_Value + " , Ans3_Value = " + Ans3_Value 
                    + " , Ans4_Value = " + Ans4_Value + " , Ans5_Value = " + Ans5_Value + " , Ans6_Value = " + Ans6_Value 
                    + " , Ans7_Value = " + Ans7_Value + " , Ans8_Value = " + Ans8_Value + " , Ans9_Value = " + Ans9_Value 
                    + " , Ans10_Value = " + Ans10_Value + " , Ans11_Value = " + Ans11_Value + " , Ans12_Value = " + Ans12_Value 
                    + " , Ans13_Value = " + Ans13_Value + " , Ans14_Value = " + Ans14_Value + " , Ans15_Value = " + Ans15_Value  

                    + " where Visit_ID = " + V_ID + " ;";
   
            }
            #endregion

            //the same queries in update and insert => delete then add
            user1_id = (V1_comboBox.SelectedIndex == -1 ? -1 : int.Parse(V1_comboBox.SelectedValue.ToString()));
            user2_id = (V2_comboBox.SelectedIndex == -1 ? -1 : int.Parse(V2_comboBox.SelectedValue.ToString()));
            user3_id = (V3_comboBox.SelectedIndex == -1 ? -1 : int.Parse(V3_comboBox.SelectedValue.ToString()));
            user4_id = (V4_comboBox.SelectedIndex == -1 ? -1 : int.Parse(V4_comboBox.SelectedValue.ToString()));
            Visitors_Query += "(@Visit_ID," + (user1_id == -1 ? "NULL" : "" + user1_id + "") + "),"
                            + "(@Visit_ID," + (user2_id == -1 ? "NULL" : "" + user2_id + "") + "),"
                            + "(@Visit_ID," + (user3_id == -1 ? "NULL" : "" + user3_id + "") + "),"
                            + "(@Visit_ID," + (user4_id == -1 ? "NULL" : "" + user4_id + "") + ") ";
        }

        private void Set_Controls()
        {
            user_mode = false;

            Ans1= Ans2= Ans3= Ans4= Ans5= Ans6= Ans7
                = Ans8= Ans9= Ans10= Ans11= Ans12= Ans13= Ans14= Ans15
                = Vistor1= Vistor2= Vistor3= Vistor4 
                = CreatedBy= EditedBy= visitDate = "";
             
           Visit_Query = Answers_Query= Visitors_Query = "";

            Ans1_Value = Ans2_Value = Ans3_Value = Ans4_Value = Ans5_Value = Ans6_Value
                = Ans7_Value = Ans8_Value = Ans9_Value = Ans10_Value = Ans11_Value = Ans12_Value
                = Ans13_Value = Ans14_Value = Ans15_Value 
                = user1_id = user2_id = user3_id = user4_id = -1;

            //V_ID no need to clear
            MicroProject_ID = User_ID = Person_ID = -1;

            General_Information_IsDirty = Answers_IsDirty = Visitors_IsDirty
                = DataApproved = Update_Mode = false;

            Ans1_textBox.Text = 
            Ans2_textBox.Text = 
            Ans3_textBox.Text = 
            Ans4_textBox.Text = 
            Ans5_textBox.Text = 
            Ans6_textBox.Text = 
            Ans7_textBox.Text = 
            Ans8_textBox.Text = 
            Ans9_textBox.Text = 
            Ans10_textBox.Text =  
            Ans11_textBox.Text =  
            Ans12_textBox.Text =  
            Ans13_textBox.Text =  
            Ans14_textBox.Text =  
            Ans15_textBox.Text =  "" ;

            //MicroProject_ID_textBox.Text =
            Person_Name_textBox.Text = LoanAmount_textBox1.Text = "";
            CreatedBy_User_label.Text = EditedBy_User_label.Text = "";

            V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex = V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1;
            
            for (int i = 1; i <= 15; i++)  set_radioButtons(i, -1); 

            user_mode = true;
        }
    }
}
