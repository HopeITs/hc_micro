using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class SpecialCase_Form : Form
    {
        public SpecialCase_Form()
        {
            InitializeComponent();
        }

        private Log l;
        private readonly MainForm mainForm;
        private int MicroProject_ID, SC_ID, User_ID, Type_ID, Person_ID;
        private MicroProject mp;
        private NewTheme NewTheme;
        private UserNotification noti;
        private Select s;
        private SpecialCase sc;
        private bool Update_Mode;
        private bool user_mode;
        private User user;
        private DataTable Visit_Users_dt;
        private DataSet combo_ds;
        private string previous_mp = "";
        private string previous_p_name = "";

        public SpecialCase_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.SC_ID = -1;
            Update_Mode = false;
        }

        public SpecialCase_Form(int SC_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.SC_ID = SC_ID;
            Update_Mode = true;
        }

        private void SpecialCase_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.ApplicationNew_ToLight(this);
                else
                    NewTheme.ApplicationNew_ToNight(this);
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Special Case";

                l = new Log();
                s = new Select();
                sc = new SpecialCase();
                user = new User();
                mp = new MicroProject();

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
                User_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                Type_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                Bind_All_ComboBoxes();

                Date_dateTimePicker.Value = DateTime.Now;
                Type_comboBox.SelectedIndex = 0; //default هاتفية
                User_comboBox.SelectedValue = Program.userID; //this user

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "Yes");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "Yes");

                if (SC_ID != -1)
                {
                    DataTable sc_dt = sc.Select_SpecialCase(SC_ID.ToString(), "", "");
                    fill_boxes(sc_dt);

                    User_comboBox.Focus();

                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Bind_All_ComboBoxes()
        {
            user_mode = false;
            string query = "";

            query += "SELECT UserID as 'ID', UserName as 'Name' from `user` where IsVisitor = 1 ORDER BY Name ASC;"; //0 visitors
            query += "SELECT ID, Name from `specialcasetype` ORDER BY Name ASC;"; //1 

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            combo_ds = new DataSet();
            da.Fill(combo_ds);
            Program.MyConn.Close();

            Set_comboBox_datasource(User_comboBox, combo_ds.Tables[0]);
            Set_comboBox_datasource(Type_comboBox, combo_ds.Tables[1]);

            user_mode = true;
        }

        private void Set_comboBox_datasource(ComboBox cBox, DataTable c_dt)
        {
            cBox.DataSource = null;
            cBox.DisplayMember = "Name";
            cBox.ValueMember = "ID";
            cBox.DataSource = c_dt;
            cBox.SelectedIndex = -1;
        }

        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        string ApplicationState = "";

        private void fill_boxes(DataTable dt)
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
                    ApplicationState = (string)dt.Rows[0]["نوع الاستمارة"];

                    if (dt.Rows[0]["Address"] != DBNull.Value)
                        WorkAddress_textBox.Text = (string)dt.Rows[0]["Address"];
                    Person_ID = int.Parse(dt.Rows[0]["Beneficiary_ID"].ToString());

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

                    if (Update_Mode)
                    {
                        SC_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                        var date1 = (DateTime)dt.Rows[0]["Date"];
                        Date_dateTimePicker.Value = date1;

                        User_comboBox.SelectedValue = (int)dt.Rows[0]["User_ID"];
                        Type_comboBox.SelectedValue = (int)dt.Rows[0]["Type_ID"];

                        Reason_textBox.Text = (string)dt.Rows[0]["Reason"];
                        Description_textBox.Text = (string)dt.Rows[0]["Description"];
                        Action_textBox.Text = (string)dt.Rows[0]["Action"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MicroProject_ID_textBox_Leave(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                DataTable dt = new DataTable();
                if (Update_Mode) // يوجد بيانات على الصفحة
                {
                    var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد اختيار مشروع جديد لتسجيل مراجعة جديدة؟", "Attention", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Update_Mode = false;
                        clear_Visit_boxes();

                        dt = sc.Select_SpecialCase("", "", MicroProject_ID_textBox.Text);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fill_boxes(dt);
                        }
                        mainForm.TabName_label.Text = "SpecialCase";
                        mainForm.MP_ID_label.Text = MicroProject_ID_textBox.Text;
                    }
                    else
                    {
                        Update_Mode = true;
                        MicroProject_ID_textBox.Text = previous_mp;
                        //leave the content as it is//
                    }
                }
                else
                {
                    dt = sc.Select_SpecialCase("", "", MicroProject_ID_textBox.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        fill_boxes(dt);
                    }
                    mainForm.TabName_label.Text = "SpecialCase";
                    mainForm.MP_ID_label.Text = MicroProject_ID_textBox.Text;
                }
            }
        }



        private void MicroProject_ID_textBox_Enter(object sender, EventArgs e)
        {
            //save previous text//
            previous_mp = MicroProject_ID_textBox.Text;
        }

        private void Person_Name_textBox_Enter(object sender, EventArgs e)
        {
            //save previous text//
            previous_p_name = Person_Name_textBox.Text;
        }


        private void Person_Name_textBox1_Leave(object sender, EventArgs e)
        {
            if (Person_Name_textBox.Text != "")
            {
                DataTable dt = new DataTable();
                if (Update_Mode) // يوجد بيانات على الصفحة
                {
                    var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد اختيار مشروع جديد لتسجيل مراجعة جديدة؟", "Attention", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Update_Mode = false;
                        clear_Visit_boxes();

                        dt = sc.Select_SpecialCase("", Person_Name_textBox.Text, "");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fill_boxes(dt);
                        }
                        mainForm.TabName_label.Text = "SpecialCase";
                        mainForm.MP_ID_label.Text = MicroProject_ID_textBox.Text;
                    }
                    else
                    {
                        Update_Mode = true;
                        Person_Name_textBox.Text = previous_p_name;
                        //leave the content as it is//
                    }
                }
                else
                {
                    dt = sc.Select_SpecialCase("", Person_Name_textBox.Text, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        fill_boxes(dt);
                    }
                    mainForm.TabName_label.Text = "SpecialCase";
                    mainForm.MP_ID_label.Text = MicroProject_ID_textBox.Text;
                }
            }
        }

        private void clear_Visit_boxes()
        {
            SC_ID = -1;

            Type_ID = User_ID = -1;
            Type_comboBox.SelectedIndex = User_comboBox.SelectedIndex = -1;

            LoanDate_dateTimePicker.Value = Date_dateTimePicker.Value = DateTime.Now;
            Reason_textBox.Text = Description_textBox.Text = Action_textBox.Text = "";

            LoanAmount_textBox1.Text = "";

            MP_Name_textBox.Text = Mobile_textBox.Text = WorkAddress_textBox.Text
                = MicroProject_ID_textBox.Text = Person_Name_textBox.Text = "";
            MicroProject_ID = -1;

            mainForm.TabName_label.Text = "SpecialCase";
            mainForm.MP_ID_label.Text = "";

            previous_mp = "";
            previous_p_name = "";

            Type_comboBox.SelectedIndex = 0; //default هاتفية
            User_comboBox.SelectedValue = Program.userID; //this user

            Update_Mode = false;

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
                        throw new InvalidOperationException(" نعتذر ! لا تملك الصلاحية للقيام بهذه العملية.");
                    }
                    break;

            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();
                if (MicroProject_ID_textBox.Text == "" || Person_Name_textBox.Text == "" || MicroProject_ID == -1)
                    throw new InvalidOperationException("الرجاء التأكد من اختيار المشروع ثم إعادة المحاولة.");

                if (User_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("الرجاء التأكد من اختيار مسجل الطلب ثم إعادة المحاولة.");

                if (Type_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("الرجاء التأكد من نوع المراجعة ثم إعادة المحاولة.");

                if (Reason_textBox.Text == "")
                    throw new InvalidOperationException("الرجاء التأكد من كتابة سبب المراجعة ثم إعادة المحاولة.");

                User_ID = int.Parse(User_comboBox.SelectedValue.ToString());
                Type_ID = int.Parse(Type_comboBox.SelectedValue.ToString());

                ////insert Section////
                if (Update_Mode == false && SC_ID == -1)
                {
                    SC_ID = sc.Insert(MicroProject_ID, User_ID, Type_ID
                        , Date_dateTimePicker.Value.ToString("yyyy/MM/dd")
                        , Reason_textBox.Text, Description_textBox.Text, Action_textBox.Text);

                    l.Insert_Log(
                        "Insert SpecialCase to " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "SpecialCase", Program.UserName, DateTime.Now);
                }
                else ////update section ////
                {
                    sc.Update(SC_ID, User_ID, Type_ID
                        , Date_dateTimePicker.Value.ToString("yyyy/MM/dd")
                        , Reason_textBox.Text, Description_textBox.Text, Action_textBox.Text);

                    l.Insert_Log(
                        "Update SpecialCase of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "SpecialCase", Program.UserName, DateTime.Now);
                }

                var dialogResult = MessageBox.Show("تم تسجيل المراجعة بنجاح. هل تريد إضافة مراجعة جديدة ؟", "Success", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    clear_Visit_boxes();
                    Update_Mode = false;

                    mainForm.TabName_label.Text = "Special Case";
                    mainForm.MP_ID_label.Text = "";

                    MicroProject_ID_textBox.Focus();
                }
                else
                {
                    Update_Mode = true;

                    mainForm.TabName_label.Text = "Special Case";
                    mainForm.MP_ID_label.Text = MicroProject_ID_textBox.Text;
                }
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

                if (SC_ID == -1)
                    throw new InvalidOperationException("الرجاء التأكد من اختيار مراجعة لحذفها!");

                var dialogResult = MessageBox.Show("هل أنت متأكد من أنك تريد حذف هذه المراجعة؟", "Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    sc.Delete(SC_ID);

                    l.Insert_Log(
                        "Delete SpecialCase of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "SpecialCase", Program.UserName, DateTime.Now);

                    clear_Visit_boxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void Save_button_MouseLeave(object sender, EventArgs e)
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

        #region context menu items click
        private void clear_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد مسح كل محتوى الصفحة؟", "Attention", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                clear_Visit_boxes();
            }
        }

        private void application_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Appplication_New_Form = new Application_New_Form(mainForm, MicroProject_ID, Person_ID, ApplicationState);
            mainForm.showNewTab(Appplication_New_Form, "Application", 0);
        }

        private void addNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Find the admin in this database + who entered the application//
                string my_condition = " and ( UserName like '" + User_comboBox.Text + "'";
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
                var _Form = new NewIdea_Form("SpecialCase", project, emails);
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion
    }
}
