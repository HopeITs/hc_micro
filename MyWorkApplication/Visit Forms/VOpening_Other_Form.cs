using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication.Visit_Forms
{
    public partial class VOpening_Other_Form : Form
    {
        private const string _KIND = "O-op";
        private Log l;
        private readonly MainForm mainForm;
        private int MicroProject_ID, V_ID, User_ID, Person_ID;
        private MySqlComponents MySS;
        private NewTheme NewTheme;
        private UserNotification noti;
        private Office_Monitoring O;
        private int Office_Monitoring_ID;
        private Select s; 
        private TasksOfProjects TasksOfProjects;
        private bool Update_Mode;
        private Visit v;
        private User user;
        private DataTable Visit_Users_dt;
        private readonly List<int> User_IDs = new List<int>();
        private bool mevisits_generated = false;
        private string Type;
         
        public VOpening_Other_Form(int V_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.V_ID = V_ID;
            Update_Mode = true;
        }

        public VOpening_Other_Form(MainForm mainForm, int MicroProject_ID)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            V_ID = -1;
            Update_Mode = false;
        }

        private void Other_Visit1_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.Visit_ToLight(this);
                else
                    NewTheme.Visit_ToNight(this);
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "First Visit of Other Project";

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                v = new Visit();
                user = new User();
                O = new Office_Monitoring();
                noti = new UserNotification();
                Type = "";

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
                Street_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V1_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V2_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V3_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V4_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                bind_Street_into_ComboBox();
                users_bind(V1_comboBox);
                users_bind(V2_comboBox);
                users_bind(V3_comboBox);
                users_bind(V4_comboBox);
                
                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries(_KIND, "Yes");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs(_KIND, "Yes");

                var user_db = s.select_M_E();
                if (user_db != null)
                    for (var i = 0; i < user_db.Rows.Count; i++)
                        User_IDs.Add(user_db.Rows[i].Field<int>(0));
                 
                if (V_ID != -1)
                { 
                    DataTable v_dt = v.Get_Visit(V_ID.ToString(), "", "", _KIND);
                    fill_visit_boxes(v_dt);

                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();

                    check_visit_notifications();
                }
                else if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox1_Leave(sender, e);


                }
                 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void check_visit_notifications()
        {
            //select the visits not to ignore generate notifications
            UserNotification U_noti = new UserNotification();
            mevisits_generated = U_noti.Has_Notifications(MicroProject_ID, "", "%Visit M%", "", -1, "");

            if (!mevisits_generated)
            {
                var dialogResult = MessageBox.Show(
                    "انتبه !! هذا المشروع لا يملك أي إشعار لزيارات  المراقبة والتقييم" + Environment.NewLine +
                    "هل تريد توليد إشعارات الزيارات وذلك حسب تاريخ زيارة الافتتاحية التالية؟؟" + Environment.NewLine,
                            "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    //Delete this project Visit notifications//
                    noti.Delete_Notification(MicroProject_ID, "", "Visit M%", Person_Name_textBox.Text, -1);
                    l.Insert_Log("Delete visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ", "Visit", Program.UserName, DateTime.Now);
                      
                    if (Type.Contains("زراع"))
                        generate_me_agricultural_visits_notifications();
                    else
                        generate_me_visits_notifications();
                } 
            }
        }

        private void users_bind(ComboBox comboBox)
        {
            try
            {
                var dt = s.select_visitors();
                comboBox.DataSource = dt;
                comboBox.DisplayMember = "Visitors";
                comboBox.ValueMember = "U_ID";
                comboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fill_visit_boxes(DataTable dt)
        {
            try
            { 
                if (dt != null)
                {
                    MicroProject_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    Person_Name_textBox.Text = (string)dt.Rows[0]["Beneficiary Name"];
                    MP_Name_textBox.Text = (string)dt.Rows[0]["Project Name"];
                    Mobile_textBox.Text = (string)dt.Rows[0]["Mobile"];

                    if(dt.Rows[0]["Address"] != DBNull.Value)
                        WorkAddress_textBox.Text = (string)dt.Rows[0]["Address"];
                    Person_ID = int.Parse(dt.Rows[0]["Beneficiary_ID"].ToString());

                    Type = (string)dt.Rows[0]["Type"];

                    if (dt.Rows[0]["Loan Amount"] == DBNull.Value)
                        MessageBox.Show("لم يتم إدخال مبلغ القرض لهذا المشروع", "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        var dd = Convert.ToDouble(dt.Rows[0]["Loan Amount"].ToString());
                        LoanAmount_textBox1.Text = dd.ToString();

                        if (dt.Rows[0]["Loan Date"] != DBNull.Value)
                        {
                            var date = Convert.ToDateTime(dt.Rows[0]["Loan Date"].ToString());
                            LoanDate_dateTimePicker.Value = date;
                        }
                    }

                    CreatedBy_User_label.Text = dt.Rows[0]["Created_By"].ToString();
                    EditedBy_User_label.Text = dt.Rows[0]["Edited_By"].ToString();

                    if (Update_Mode)
                    {
                        V_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                         
                        var date1 = (DateTime)dt.Rows[0]["Date"];
                        VisitDate_dateTimePicker.Value = date1;

                        if (dt.Rows[0]["MP_Street_ID"] == DBNull.Value || dt.Rows[0]["MP_Street_ID"] == null)
                            Street_comboBox.SelectedIndex = -1;
                        else
                        {
                            Street_comboBox.SelectedValue = int.Parse(dt.Rows[0]["MP_Street_ID"].ToString());
                        }

                        if (dt.Rows[0]["Short Address"] != DBNull.Value && dt.Rows[0]["Short Address"] != null)
                            Place_Note_textBox.Text = (string)dt.Rows[0]["Short Address"];

                        Ans1_textBox.Text = (string)dt.Rows[0]["Ans1"];
                        Ans2_textBox.Text = (string)dt.Rows[0]["Ans2"];
                        Ans3_textBox.Text = (string)dt.Rows[0]["Ans3"];
                        Ans4_textBox.Text = (string)dt.Rows[0]["Ans4"];
                        Ans5_textBox.Text = (string)dt.Rows[0]["Ans5"];
                        Ans6_textBox.Text = (string)dt.Rows[0]["Ans6"];
                        Ans7_textBox.Text = (string)dt.Rows[0]["Ans7"];

                        for (int i = 1; i <= 7; i++)
                        {
                            set_radioButtons(i, Convert.ToInt32(dt.Rows[0]["Ans" + i + "_Value"]));
                        }

                        Indicatores_textBox.Text = (string)dt.Rows[0]["Indicators"];

                        var office_dt = O.Get_Office_Monitoring(V_ID);
                        if (office_dt.Rows.Count > 0)
                        {
                            Office_Monitoring_ID = int.Parse(office_dt.Rows[0]["ID"].ToString());
                            Office1_textBox.Text = (string)office_dt.Rows[0]["Ans1"];
                            Office2_textBox.Text = (string)office_dt.Rows[0]["Ans2"];
                            Office3_textBox.Text = (string)office_dt.Rows[0]["Ans3"];
                            Office4_textBox.Text = (string)office_dt.Rows[0]["Ans4"];
                            Office5_textBox.Text = (string)office_dt.Rows[0]["Ans5"];
                            Office6_textBox.Text = (string)office_dt.Rows[0]["Ans6"];
                            OtherComments_textBox.Text = (string)office_dt.Rows[0]["OtherComments"];

                            for (int i = 1; i <= 6; i++)
                            {
                                set_office_radioButtons(i, Convert.ToInt32(office_dt.Rows[0]["Ans" + i + "_Value"]));
                            }
                        }

                        V1_comboBox.Text = V2_comboBox.Text = V3_comboBox.Text = V4_comboBox.Text = "";
                        V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex =
                            V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1;

                        Visit_Users_dt = v.Get_Visit_Users(V_ID);
                        if (Visit_Users_dt.Rows.Count != 0)
                            for (var i = 0; i < Visit_Users_dt.Rows.Count; i++)
                            {
                                var u_id = Visit_Users_dt.Rows[i].Field<int>(0);
                                var V = "V";
                                var VV = "_comboBox";
                                var fullName = V + (i + 1) + VV;
                                var cbx = Controls.Find(fullName, true).FirstOrDefault() as ComboBox;
                                cbx.SelectedValue = u_id;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         

        #region set get radio_buttons
        void set_radioButtons(int ansNo, int value)
        {
            string name = "Ans" + ansNo + "_Value";
            if (ansNo != 4) //other projects
            {
                if (value == -1)
                {
                    string radio_name = "yes" + ansNo + "_radioButton";
                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;

                    radio_name = "no" + ansNo + "_radioButton";
                    rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;
                }
                else if (value == 1)
                {
                    string radio_name = "yes" + ansNo + "_radioButton";
                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = true;

                    radio_name = "no" + ansNo + "_radioButton";
                    rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;
                }
                else if (value == 0)
                {
                    string radio_name = "yes" + ansNo + "_radioButton";
                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;

                    radio_name = "no" + ansNo + "_radioButton";
                    rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = true;
                }
            }
            else //ans4 (other projects) values are 25-50-75-100 and -1 for not seted value//  

            {
                if (value == -1)
                {
                    foreach (RadioButton r in ans4_tableLayoutPanel.Controls.OfType<RadioButton>())
                        r.Checked = false;
                }
                else
                {
                    string radio_name = "";
                    if (value == 1) radio_name = "ans" + ansNo + "_25_radioButton";
                    else if (value == 2) radio_name = "ans" + ansNo + "_50_radioButton";
                    else if (value == 3) radio_name = "ans" + ansNo + "_75_radioButton";
                    else if (value == 4) radio_name = "ans" + ansNo + "_100_radioButton";

                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = true;
                }
            }
        }
        void set_office_radioButtons(int ansNo, int value)
        {
            string name = "Ans" + ansNo + "_Value";
            if (value == -1)
            {
                for (int i = 0; i < 3; i++)
                {
                    string radio_name = "o" + ansNo + "_" + i + "_radioButton";
                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;
                }
            }
            else
            {
                string radio_name = "o" + ansNo + "_" + value + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                rad_bx.Checked = true;
            }
        }
        int get_radioButtons_values(int ansNo)
        {
            int value = -1;
            string name = "Ans" + ansNo + "_Value";
            if (ansNo != 4) //other projects
            {
                string radio_name = "yes" + ansNo + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 1;

                radio_name = "no" + ansNo + "_radioButton";
                rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 0;
            }
            else //ans4 (other projects) values are 25-50-75-100 and -1 for not seted value//  
            {
                string radio_name = "";
                radio_name = "ans" + ansNo + "_25_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 1;

                radio_name = "ans" + ansNo + "_50_radioButton";
                rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 2;

                radio_name = "ans" + ansNo + "_75_radioButton";
                rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 3;

                radio_name = "ans" + ansNo + "_100_radioButton";
                rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked) value = 4;
            }
            return value;
        }
        int get_office_radioButtons_values(int ansNo)
        {
            int value = -1;
            for (int i = 0; i < 3; i++)
            {
                string radio_name = "o" + ansNo + "_" + i + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked)
                {
                    value = i;
                    break;
                }
            }

            return value;
        }
        #endregion

        private void MicroProject_ID_textBox1_Leave(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                DataTable dt = new DataTable();
                if (v.Has_Visit("", MicroProject_ID_textBox.Text, _KIND))
                {
                    Update_Mode = true;
                    dt = v.Get_Visit("", "", MicroProject_ID_textBox.Text, _KIND);
                }
                else
                {
                    dt = v.Get_Visit("", "", MicroProject_ID_textBox.Text, "");
                    Update_Mode = false;
                }
                clear_Visit_boxes(0);

                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        fill_visit_boxes(dt);
                    }
            }
        }

        private void Person_Name_textBox1_Leave(object sender, EventArgs e)
        {
            if (Person_Name_textBox.Text != "")
            {
                DataTable dt = new DataTable();
                if (v.Has_Visit("", Person_Name_textBox.Text, _KIND))
                {
                    Update_Mode = true;
                    dt = v.Get_Visit("", Person_Name_textBox.Text, "", _KIND);
                }
                else
                {
                    Update_Mode = false;
                    dt = v.Get_Visit("", Person_Name_textBox.Text, "", "");
                }

                clear_Visit_boxes(0);

                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        fill_visit_boxes(dt);
                    }
            }
       
        }

        private void generate_me_visits_notifications()
        {
            // زيارة الافتتاحية //updated
            DateTime OpeningVisit_date = VisitDate_dateTimePicker.Value;

            // أول زيارة M&E
            // تبدأ بعد شهرين وأسبوع من زيارة الافتتاحية
            //updated
            DateTime M1_Visit_date = OpeningVisit_date.AddMonths(2);
            //M1_Visit_date = M1_Visit_date.AddDays(7);

            int visit_interval;
            DateTime M2_Visit_date, M3_Visit_date, M4_Visit_date, M5_Visit_date;
            M2_Visit_date = M3_Visit_date = M4_Visit_date = M5_Visit_date = DateTime.Now;

            // المسافة بين كل زيارة هي 4 أشهر ثابتة
            visit_interval = 4;
            M2_Visit_date = M1_Visit_date.AddMonths(visit_interval);
            M3_Visit_date = M2_Visit_date.AddMonths(visit_interval);
            M4_Visit_date = M3_Visit_date.AddMonths(visit_interval);
            M5_Visit_date = M4_Visit_date.AddMonths(visit_interval);
            
            //Send to M&E//
            if (User_IDs.Count == 0)
                MessageBox.Show(
                    "لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");
            else
                foreach (var u in User_IDs)
                {
                    noti.Insert_UserNotification(M1_Visit_date.ToString("yyyy/MM/dd"), "Visit M1-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M2_Visit_date.ToString("yyyy/MM/dd"), "Visit M2-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M3_Visit_date.ToString("yyyy/MM/dd"), "Visit M3-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M4_Visit_date.ToString("yyyy/MM/dd"), "Visit M4-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M5_Visit_date.ToString("yyyy/MM/dd"), "Visit M5-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                }
            l.Insert_Log("Insert visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                    "Visit", Program.UserName, DateTime.Now);

            MessageBox.Show("تم توليد إشعارات زيارات المراقبة والتقييم الخاصة بهذا المشروع بنجاح");
        }
        private void generate_me_agricultural_visits_notifications()
        { 
            // زيارة الافتتاحية //updated
            DateTime OpeningVisit_date = VisitDate_dateTimePicker.Value;

            // أول زيارة M&E
            //في الشهر الاقرب للافتتاحية اما رابع او حادي عشر 

            OpeningVisit_date = OpeningVisit_date.AddMonths(2); // كرمال تكون ابعد
            DateTime M1_Visit_date = OpeningVisit_date;
            DateTime M2_Visit_date, M3_Visit_date, M4_Visit_date, M5_Visit_date;
            M2_Visit_date = M3_Visit_date = M4_Visit_date = M5_Visit_date = DateTime.Now;
             
            string day, month, year;
            day = OpeningVisit_date.Day.ToString();
            if (OpeningVisit_date.Day == 31) day = "30";
            year = OpeningVisit_date.Year.ToString();

            if (OpeningVisit_date.Month >= 1 && OpeningVisit_date.Month < 4)
            { 
                month = "04"; 
                string dateString = year + "-" + month + "-" + day; 
                M1_Visit_date = DateTime.Parse(dateString);  

                M2_Visit_date = M1_Visit_date.AddMonths(7); //11
                M3_Visit_date = M2_Visit_date.AddMonths(5); //4
                M4_Visit_date = M3_Visit_date.AddMonths(7); //11
                M5_Visit_date = M4_Visit_date.AddMonths(5); //4 
            }  
            else if (OpeningVisit_date.Month >= 4 && OpeningVisit_date.Month < 11)
            {
                month = "11";
                string dateString = year + "-" + month + "-" + day;
                M1_Visit_date = DateTime.Parse(dateString);

                M2_Visit_date = M1_Visit_date.AddMonths(5); //4
                M3_Visit_date = M2_Visit_date.AddMonths(7); //11
                M4_Visit_date = M3_Visit_date.AddMonths(5); //4
                M5_Visit_date = M4_Visit_date.AddMonths(7); //11 
            }
            else if (OpeningVisit_date.Month >= 11) //should turn to the next year
            {
                month = "04";
                OpeningVisit_date = OpeningVisit_date.AddYears(1);
                year = OpeningVisit_date.Year.ToString();
                string dateString = year + "-" + month + "-" + day;
                M1_Visit_date = DateTime.Parse(dateString);

                M2_Visit_date = M1_Visit_date.AddMonths(7); //11
                M3_Visit_date = M2_Visit_date.AddMonths(5); //4
                M4_Visit_date = M3_Visit_date.AddMonths(7); //11
                M5_Visit_date = M4_Visit_date.AddMonths(5); //4 
            }

            //Send to M&E//
            if (User_IDs.Count == 0)
                MessageBox.Show(
                    "لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");
            else
                foreach (var u in User_IDs)
                {
                    noti.Insert_UserNotification(M1_Visit_date.ToString("yyyy/MM/dd"), "Visit M1-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M2_Visit_date.ToString("yyyy/MM/dd"), "Visit M2-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M3_Visit_date.ToString("yyyy/MM/dd"), "Visit M3-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M4_Visit_date.ToString("yyyy/MM/dd"), "Visit M4-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                    noti.Insert_UserNotification(M5_Visit_date.ToString("yyyy/MM/dd"), "Visit M5-" + _KIND.Substring(0, 1),
                        Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -21);
                }
            l.Insert_Log("Insert visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                    "Visit", Program.UserName, DateTime.Now);

            MessageBox.Show("تم توليد إشعارات زيارات المراقبة والتقييم الخاصة بهذا المشروع بنجاح");
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            { 
                CheckUserPermission();
                if (MicroProject_ID_textBox.Text == "" || Person_Name_textBox.Text == "" || MicroProject_ID == -1)
                    throw new InvalidOperationException("You should choose a project first ..!!");
                //if (VisitType_comboBox1.Text == "") throw new InvalidOperationException("You can't leave empty fields ..!!");

                ////insert Section////
                if (Update_Mode == false && V_ID == -1)
                {
                    v.Insert_Visit(MicroProject_ID, _KIND, "", VisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd"),
                        Ans1_textBox.Text, Ans2_textBox.Text, Ans3_textBox.Text,
                        Ans4_textBox.Text, Ans5_textBox.Text, Ans6_textBox.Text, Ans7_textBox.Text, Indicatores_textBox.Text
                        , get_radioButtons_values(1), get_radioButtons_values(2), get_radioButtons_values(3)
                        , get_radioButtons_values(4), get_radioButtons_values(5), get_radioButtons_values(6)
                        , get_radioButtons_values(7)
                        , "" , Program.UserName);

                    l.Insert_Log("Insert visit-" + _KIND + " to " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Visit", Program.UserName, DateTime.Now);
                    CreatedBy_User_label.Text = Program.UserName;

                    ////////////// GET current V_ID //////////////////
                    V_ID = v.Get_Current_Visit(_KIND);

                    /////////// insert office monitoring ////////////////
                    O.Insert_Office_Monitoring(V_ID, Office1_textBox.Text, Office2_textBox.Text,
                        Office3_textBox.Text, Office4_textBox.Text, Office5_textBox.Text, Office6_textBox.Text,
                        OtherComments_textBox.Text
                        , get_office_radioButtons_values(1), get_office_radioButtons_values(2)
                        , get_office_radioButtons_values(3), get_office_radioButtons_values(4)
                        , get_office_radioButtons_values(5), get_office_radioButtons_values(6));


                    //make the visit 1 task of this project ==> checked
                    //update task ^_^
                    ///////////////////////////   Task IDs = 25 opening  ////////////////////////////////////////////
                    TasksOfProjects = new TasksOfProjects();
                    TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, 25, true, DateTime.Now);
                    ////////////////////////////////////////////////////////////////////////////////////////// 

                    //update notification
                    noti.Update_MicroUsers_Notification(MicroProject_ID , "", "Visit Opening-O" , "", -21, "1");
                    l.Insert_Log( "Set seen=1 to Notification of Visit Opening-O:" + Person_Name_textBox.Text + "-" + MicroProject_ID ,
                        "user_notification visit", Program.UserName, DateTime.Now);
                    //////////////////////////////////////////////////////////////////////////////////////////   

                    MessageBox.Show("سيتم توليد إشعارات زيارات المراقبة والتقييم وذلك حسب تاريخ زيارة الافتتاحية التالية"); 
                    //Delete this project Visit notifications//
                    noti.Delete_Notification(MicroProject_ID, "", "Visit M%", Person_Name_textBox.Text, -1);
                    l.Insert_Log("Delete visit notifications of:" + MicroProject_ID + " : " + Person_Name_textBox.Text + " ", "Visit", Program.UserName, DateTime.Now);

                    /////////////////////////////////////////// 
                    // توليد اشعارات الزيارات للمشاريع الزراعية فقط في الشهرين الرابع والحادي عشر //
                    if (Type.Contains("زراع"))
                        //MessageBox.Show("ملاحظة:" + Environment.NewLine + "لن يتم توليد أي اشعار بالزيارات للمشروع لأنه من المشاريع الزراعية", "Alert");
                        generate_me_agricultural_visits_notifications();
                    else
                        generate_me_visits_notifications();
                }
                else ////update section ////
                {
                    v.Update_Visit(V_ID, "", VisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd"),
                         Ans1_textBox.Text, Ans2_textBox.Text, Ans3_textBox.Text,
                         Ans4_textBox.Text, Ans5_textBox.Text, Ans6_textBox.Text, Ans7_textBox.Text, Indicatores_textBox.Text
                        , get_radioButtons_values(1), get_radioButtons_values(2), get_radioButtons_values(3)
                        , get_radioButtons_values(4), get_radioButtons_values(5), get_radioButtons_values(6)
                        , get_radioButtons_values(7)
                        , "" , Program.UserName);
                    l.Insert_Log(
                        "Update visit-" + _KIND + " of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Visit", Program.UserName, DateTime.Now);
                    EditedBy_User_label.Text = Program.UserName;

                    /////////// update office monitoring ////////////////
                    O.Update_Office_Monitoring(V_ID, Office1_textBox.Text, Office2_textBox.Text,
                        Office3_textBox.Text, Office4_textBox.Text, Office5_textBox.Text, Office6_textBox.Text,
                        OtherComments_textBox.Text
                         , get_office_radioButtons_values(1), get_office_radioButtons_values(2)
                        , get_office_radioButtons_values(3), get_office_radioButtons_values(4)
                        , get_office_radioButtons_values(5), get_office_radioButtons_values(6));

                    //remove all users of this visit from database
                    v.Delete_Visit_Users(V_ID);
                }

                var users_of_visit = "";
                //check user comboBoxs and insert
                if (V1_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V1_comboBox.SelectedValue);
                    v.Insert_Visit_User(V_ID, User_ID);
                    users_of_visit += V1_comboBox.Text + " ";
                }

                if (V2_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V2_comboBox.SelectedValue);
                    v.Insert_Visit_User(V_ID, User_ID);
                    users_of_visit += V2_comboBox.Text + " ";
                }

                if (V3_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V3_comboBox.SelectedValue);
                    v.Insert_Visit_User(V_ID, User_ID);
                    users_of_visit += V3_comboBox.Text + " ";
                }

                if (V4_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V4_comboBox.SelectedValue);
                    v.Insert_Visit_User(V_ID, User_ID);
                    users_of_visit += V4_comboBox.Text + " ";
                }

                l.Insert_Log(
                    "Insert visitors-" + users_of_visit + " of the visit-" + _KIND + " of " + MicroProject_ID + " : " +
                    Person_Name_textBox.Text + " ", "Visit", Program.UserName, DateTime.Now);

                Update_MP_Place(MicroProject_ID);
                l.Insert_Log("Update place after fund of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                    "Visit", Program.UserName, DateTime.Now);

                //check عدا الزراعي
                if(!Type.Contains("زراع"))
                    check_visit_notifications();

                MessageBox.Show("Visit saved successfuly");
                clear_Visit_boxes(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (V_ID == -1) throw new InvalidOperationException("Please choose a vosot first to delete !");
                var dialogResult = MessageBox.Show("Are you sure you want to delete this visit?", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    v.Delete_Visit(V_ID);

                    ///////////////////////////   Task IDs = opening 25   ////////////////////////////////////////////
                    TasksOfProjects = new TasksOfProjects();
                    TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, 25, false, DateTime.Now);

                    l.Insert_Log(
                        "Delete visit-" + _KIND + " to " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Visit", Program.UserName, DateTime.Now);
                    clear_Visit_boxes(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CheckUserPermission()
        {
            addNotesToolStripMenuItem.Visible = false;

            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        if (Update_Mode)
                            addNotesToolStripMenuItem.Visible = true;
                    }
                    break;
                case 2: //Data 
                    break;
                case 3: //Financial  
                case 4: //Guest
                case 7: //Lawful 
                case 9: //Media
                    {
                        throw new InvalidOperationException(" نعتذر ! لا تملك الصلاحية للقيام بهذا الفعل.");
                    } 
            }
        }

        private void clear_Visit_boxes(int j)
        {
            V_ID = -1;
             
            V1_comboBox.Text = V2_comboBox.Text = V3_comboBox.Text = V4_comboBox.Text = Place_Note_textBox.Text= "";
            V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex
                = V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex 
                = Street_comboBox.SelectedIndex = -1;

            CreatedBy_User_label.Text = EditedBy_User_label.Text = "";

            LoanDate_dateTimePicker.Value = VisitDate_dateTimePicker.Value = DateTime.Now;
            Ans1_textBox.Text = Ans2_textBox.Text = Indicatores_textBox.Text
            = Ans3_textBox.Text = Ans4_textBox.Text = Ans5_textBox.Text = Ans6_textBox.Text = "";

            //clear radiobuttons
            for (int i = 1; i <=7; i++)
                set_radioButtons(i, -1);
            for (int i = 1; i <=6; i++)
                set_office_radioButtons(i, -1);
         
            Office1_textBox.Text = Office2_textBox.Text = Office3_textBox.Text =
                Office4_textBox.Text = Office5_textBox.Text = Office6_textBox.Text = OtherComments_textBox.Text = "";

            LoanAmount_textBox1.Text = "";
            if (j != 0)
            {
                MP_Name_textBox.Text = Mobile_textBox.Text = WorkAddress_textBox.Text
                     = MicroProject_ID_textBox.Text = Person_Name_textBox.Text = "";
                MicroProject_ID = -1;
                Update_Mode = false;
            }
        }

        private void Update_MP_Place(int MP_ID)
        {
            MicroProject mp = new MicroProject();

            mp.Update_MP_Place(MP_ID
                , (Street_comboBox.SelectedValue != null ? int.Parse(Street_comboBox.SelectedValue.ToString()) : -1)
                , Place_Note_textBox.Text);
        }

        private void bind_Street_into_ComboBox()
        {
            
            Program.buildConnection();

            MySS.query = "select ID, Name from street ORDER BY Name ASC";
            var sc = new MySqlCommand(MySS.query, Program.MyConn);
            var da = new MySqlDataAdapter(MySS.query, Program.MyConn);
            var reader = sc.ExecuteReader();
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Load(reader);
            Street_comboBox.DisplayMember = "Name";
            Street_comboBox.ValueMember = "ID";
            Street_comboBox.DataSource = dt;
            Street_comboBox.SelectedIndex = -1;
        }

        private void ClearPlace_button_Click(object sender, EventArgs e)
        {
            Street_comboBox.SelectedIndex = -1;
        }

        #region mouse hover 
        private void Save_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Save2_L;
            else image = Resources.Save2_D;

            Save_button.BackgroundImage = image;
        }
        private void Save_button_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Save2_D;
            else image = Resources.Save2_L;

            Save_button.BackgroundImage = image;
        }
        private void Delete_button_MouseEnter(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Delete2_L;
            else image = Resources.Delete2_D;

            Delete_button.BackgroundImage = image;
        }
        private void Delete_button_MouseLeave(object sender, EventArgs e)
        {
            Image image = null;
            if (Settings.Default.theme == "Light")
                image = Resources.Delete2_D;
            else image = Resources.Delete2_L;

            Delete_button.BackgroundImage = image;
        }
        #endregion

        #region x pictureBox clicks 
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans1_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans2_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans3_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans4_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans5_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans6_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }
         
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in ans7_tableLayoutPanel.Controls.OfType<RadioButton>())
                r.Checked = false;
        }
        /// ////////////////////////////////////////////////////////////////
        private void o1_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o1.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void o2_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o2.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void o3_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o3.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void o4_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o4.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        

        private void o5_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o5.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        private void o6_pictureBox_Click(object sender, EventArgs e)
        {
            foreach (RadioButton r in tableLayoutPanel_o6.Controls.OfType<RadioButton>())
                r.Checked = false;
        }

        #endregion

        #region context menu items click
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (V_ID != -1)
            {
                DataTable v_dt = v.Get_Visit(V_ID.ToString(), "", "", _KIND);
                fill_visit_boxes(v_dt);

                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
            }
            else if (MicroProject_ID != -1)
            {
                MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                MicroProject_ID_textBox1_Leave(sender, e);
            }
        }

        private void application_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _Form = new Application_Form(Person_ID, MicroProject_ID, mainForm);
            mainForm.showNewTab(_Form, "Application", 0);
        }

        private void addNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Find the admin in this database + who entered the application//
                string my_condition = " and ( UserName like '" + CreatedBy_User_label.Text + "'";
                if (CreatedBy_User_label.Text != EditedBy_User_label.Text)
                    my_condition += " or UserName like '" + EditedBy_User_label.Text + "'";
                my_condition += " or role.Role_Name like 'Admin' ) ";

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
                var _Form = new NewIdea_Form("Opening Visit", project, emails);
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion


        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
    }
}