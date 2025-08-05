using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyWorkApplication.Visit_Forms
{
    public partial class V_ME_Taxi_Form : Form
    {
        private const string _KIND = "v";
        private DataTable ans_ques_dt;
        private Log l;
        private M_and_E m_And_E;

        private readonly MainForm mainForm;
        private int MicroProject_ID, V_ID, User_ID, Person_ID;
        private MySqlComponents MySS;
        private NewTheme NewTheme;
        private UserNotification noti;
        private string P_Name;
        private readonly int Partners;
        private Select s;
        private User user;
        private DataRow SelectedDataRow;
        private int Sum_Of_Marks;
        private TasksOfProjects TasksOfProjects;
        private bool Update_Mood;
        private string V_Num;
        private DataTable Visit_Users_dt;

        public V_ME_Taxi_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            V_ID = -1;
            Partners = 0;
        }

        public V_ME_Taxi_Form(int V_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.V_ID = V_ID;
            Partners = 0;
        }

        public V_ME_Taxi_Form(MainForm mainForm, string V_Num, string P_Name, int MP_ID, int Partners)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.P_Name = P_Name;
            this.MicroProject_ID = MP_ID;
            V_ID = -1;
            Visit_Num_textBox.Text = this.V_Num = V_Num;
            this.Partners = Partners;
            // if partner = 0 .. there is no partners 
            // if partner = 1 .. there is partner but we are adding first beneficiary (insert all querstions)
            // if partner = 2 .. there is partner but we are adding the partner now (select mutual questions and insert just partner querstions)
        }

        private void Visit_V_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light") NewTheme.ME_Visit_ToLight(this);
                else NewTheme.ME_Visit_ToNight(this);

                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Vehical Visit";

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                m_And_E = new M_and_E();
                user = new User();
                TasksOfProjects = new TasksOfProjects();
                noti = new UserNotification();

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
                V1_Eval_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                V2_Eval_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////

                users_bind(V1_comboBox);
                users_bind(V2_comboBox); //users_bind(V3_comboBox); users_bind(V4_comboBox);
                Person_Name_textBox1.AutoCompleteCustomSource = s.select_beneficiaries(_KIND, "Yes");
                MicroProject_ID_textBox1.AutoCompleteCustomSource = s.select_project_IDs(_KIND, "Yes");

                ans_ques_dt = new DataTable();
                ans_ques_dt = m_And_E.Get_Questions_Answers(_KIND);
                Fill_form_answers_questions(ans_ques_dt);

                if (V_ID != -1)
                {
                    fill_visit_boxes(V_ID);

                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                }
                else if (P_Name != "")
                {
                    Person_Name_textBox1.Text = P_Name;
                    MicroProject_ID_textBox1.Text = MicroProject_ID.ToString();
                    Person_Name_textBox1_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Fill_form_answers_questions(DataTable ans_ques_dt)
        {
            try
            {
                var Q_ID = -1;
                foreach (Control table in panel1.Controls)
                    if (table.GetType() == typeof(TableLayoutPanel))
                        if (table.Name.Contains("ans"))
                            foreach (Control c in table.Controls)
                                if (c.GetType() == typeof(TableLayoutPanel))
                                {
                                    Q_ID = -1;

                                    var n1 = c as TableLayoutPanel;
                                    //find the question
                                    if (n1.ColumnCount == 4) continue; //skip table because don't have answers !

                                    var lbl = n1.GetControlFromPosition(5, 0) as Label;
                                    Q_ID = Convert.ToInt32(lbl.Text);
                                    var row = ans_ques_dt.Select("Question_ID = " + Q_ID, "A_ID");

                                    //find Answer of this question
                                    var index = 0;
                                    for (var i = 0; i < row.Length; i++)
                                    {
                                        var rado = n1.GetControlFromPosition(i, 1) as RadioButton;
                                        rado.Text = row[index++].Field<string>("A_Name");
                                    }
                                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void clear_Visit_boxes(int j)
        {
            V_ID = -1;
            SelectedDataRow = null;

            CreatedBy_User_label.Text = EditedBy_User_label.Text = "";

            V1_comboBox.Text = V2_comboBox.Text =
                    V1_Eval_comboBox.Text = V2_Eval_comboBox.Text = ""; // V3_comboBox.Text = V4_comboBox.Text = "";
            V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex =
                V1_Eval_comboBox.SelectedIndex = V2_Eval_comboBox.SelectedIndex = -1; // V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1;
            Update_Mood = false;

            LoanDate_dateTimePicker.Value = VisitDate_dateTimePicker.Value = DateTime.Now;
            Ans1_textBox.Text = Indicators_textBox.Text = Notes_textBox.Text = "";

            LoanAmount_textBox1.Text = "";
            if (j != 0)
            {
                MicroProject_ID_textBox1.Text = Person_Name_textBox1.Text = "";
                MicroProject_ID = -1;
                Person_ID = -1;
                P_Name = "";
            }

            foreach (Control table in panel1.Controls)
                if (table.GetType() == typeof(TableLayoutPanel))
                    foreach (Control c in table.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                            foreach (Control bb in c.Controls)
                            {
                                if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox)) bb.Text = "";
                                if (bb.GetType() == typeof(RadioButton))
                                {
                                    var rad = bb as RadioButton;
                                    rad.Checked = false;
                                }
                            }

            Sum_label.Text = "المجموع = ";
        }

        private void fill_visit_boxes(int V_ID)
        {
            try
            {
                var dt = m_And_E.Get_Visit(V_ID);
                if (dt != null)
                {
                    Update_Mood = true;

                    MicroProject_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                    MicroProject_ID_textBox1.Text = MicroProject_ID.ToString();
                    Person_Name_textBox1.Text = (string)dt.Rows[0]["Beneficiary Name"];
                    Person_ID = int.Parse(dt.Rows[0]["Beneficiary_ID"].ToString());
                    MP_Name_textBox.Text = (string)dt.Rows[0]["Project Name"];
                    Mobile_textBox.Text = (string)dt.Rows[0]["Mobile"];
                    if (dt.Rows[0]["Address"] != DBNull.Value)
                        WorkAddress_textBox.Text = (string)dt.Rows[0]["Address"];

                    var dd = Convert.ToDouble(dt.Rows[0]["Loan Amount"].ToString());
                    LoanAmount_textBox1.Text = dd.ToString();
                    var date = (DateTime)dt.Rows[0]["Loan Date"];
                    LoanDate_dateTimePicker.Value = date;

                    CreatedBy_User_label.Text = dt.Rows[0]["Created_By"].ToString();
                    EditedBy_User_label.Text = dt.Rows[0]["Edited_By"].ToString();

                    //VisitType_comboBox1.Text = (string) dt.Rows[0]["Type"];
                    var date1 = (DateTime)dt.Rows[0]["Date"];
                    VisitDate_dateTimePicker.Value = date1;
                    var Profit = double.Parse(dt.Rows[0]["Profit"].ToString());
                    Profit_textBox.Text = Profit.ToString();
                    Ans1_textBox.Text = (string)dt.Rows[0]["Ans1"];
                    //Ans2_textBox.Text = (string)dt.Rows[0]["Ans2"];
                    Indicators_textBox.Text = (string)dt.Rows[0]["Indicators"];
                    Notes_textBox.Text = (string)dt.Rows[0]["Notes"];

                    Visit_Num_textBox.Text = dt.Rows[0]["Number"].ToString();

                    ///////////////////////////////////////////////////////////////////
                    // fill answers of questions // 

                    #region fill answers of questions

                    var ans_dt = m_And_E.Get_meVisit_Answers(V_ID, "");
                    int Q_ID, A_ID;
                    string Notes;
                    Sum_Of_Marks = 0;
                    foreach (Control table in panel1.Controls)
                        if (table.GetType() == typeof(TableLayoutPanel))
                            if (table.Name.Contains("ans"))
                                foreach (Control c in table.Controls)
                                    if (c.GetType() == typeof(TableLayoutPanel))
                                    {
                                        var n1 = c as TableLayoutPanel;
                                        Q_ID = A_ID = -1;
                                        Notes = "";
                                        DataRow[] row = null;
                                        //find Q_ID
                                        foreach (Control bb in n1.Controls)
                                            if (bb.GetType() == typeof(Label))
                                                if (bb.Name.Contains("id"))
                                                {
                                                    Q_ID = Convert.ToInt32(bb.Text);
                                                    row = ans_dt.Select("Question_ID = " + Q_ID + " ");
                                                    if (row == null || row.Length == 0) break;
                                                    A_ID = row[0].Field<int>("Answer_ID");
                                                    Notes = row[0].Field<string>("Notes");
                                                }

                                        if (row == null || row.Length == 0)
                                            continue;
                                        foreach (Control bb in n1.Controls)
                                        {
                                            var row2 = ans_ques_dt.Select("A_ID = " + A_ID + " ");
                                            if (bb.GetType() == typeof(TextBox))
                                                bb.Text = Notes;
                                            if (bb.GetType() == typeof(RadioButton))
                                            {
                                                var rad = bb as RadioButton;
                                                if (rad.Text == row2[0].Field<string>("A_Name"))
                                                {
                                                    rad.Checked = true;
                                                    Sum_Of_Marks += row2[0].Field<int>("A_Mark");
                                                }
                                            }
                                        }
                                    }

                    //Calculate Profit Mark and add it to sum//
                    var Profit_Mark = m_And_E.Get_Mark_Of_Profit(Profit);
                    Sum_Of_Marks += Profit_Mark;
                    Sum_label.Text = "المجموع = " + Sum_Of_Marks;

                    #endregion

                    ///////////////////////////////////////////////////////////////////

                    //Fill visitors// 
                    V1_comboBox.Text =
                        V2_comboBox.Text =
                            V1_Eval_comboBox.Text =
                                V2_Eval_comboBox.Text = ""; // V3_comboBox.Text = V4_comboBox.Text = "";
                    V1_comboBox.SelectedIndex = V2_comboBox.SelectedIndex =
                        V1_Eval_comboBox.SelectedIndex =
                            V2_Eval_comboBox.SelectedIndex =
                                -1; // V3_comboBox.SelectedIndex = V4_comboBox.SelectedIndex = -1;

                    Visit_Users_dt = m_And_E.Get_Visit_Users(V_ID);
                    if (Visit_Users_dt.Rows.Count != 0)
                        for (var i = 0; i < Visit_Users_dt.Rows.Count; i++)
                        {
                            var u_id = Visit_Users_dt.Rows[i].Field<int>(0);
                            var u_mark = Visit_Users_dt.Rows[i].Field<string>(2);

                            var V = "V";
                            var VV = "_comboBox";
                            var E = "_Eval";
                            var fullName = V + (i + 1) + VV;
                            var cbx = Controls.Find(fullName, true).FirstOrDefault() as ComboBox;
                            cbx.SelectedValue = u_id;

                            var fullName2 = V + (i + 1) + E + VV;
                            var cbx2 = Controls.Find(fullName2, true).FirstOrDefault() as ComboBox;
                            cbx2.SelectedItem = u_mark;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MicroProject_ID_textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void Person_Name_textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void Profit_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Profit_textBox.Text != "")
                {
                    Profit_textBox.Text =
                        Regex.Replace(string.Format("{0:n" + 4 + "}", Convert.ToDouble(Profit_textBox.Text)),
                            @"[" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");

                    Profit_textBox.SelectionStart = Profit_textBox.Text.Length;
                    Profit_textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        if (Update_Mood)
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

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (Person_ID == -1 || MicroProject_ID == -1 || Person_Name_textBox1.Text == "" ||
                    MicroProject_ID_textBox1.Text == "")
                    throw new InvalidOperationException("الرجاء اختيار المستفيد أولاً ثم إعادة المحاولة ");
                if (Profit_textBox.Text == "")
                    throw new InvalidOperationException("لا يمكن ترك الربح الصافي فارغاً ! الرجاء تعبئته والمحاولة مرة أخرى ");
                if (!CheckAllQuestionsHaveAnswers())
                    throw new InvalidOperationException("لا يمكن ترك سؤال بدون إجابة ! الرجاء تعبئته والمحاولة مرة أخرى ");

                var Profit = Convert.ToDouble(Profit_textBox.Text.Replace(",", ""));
                if (Update_Mood)
                {
                    //update basic information of visit
                    m_And_E.Update_Visit(V_ID, "", Visit_Num_textBox.Text.Trim(), VisitDate_dateTimePicker.Value, Profit
                        , Ans1_textBox.Text, "", Indicators_textBox.Text, Notes_textBox.Text);
                    l.Insert_Log(
                        "Update m&e visit:" + V_Num + ":" + _KIND + " of " + MicroProject_ID + " : " +
                        Person_Name_textBox1.Text + " ", " Visit ", Program.UserName, DateTime.Now);
                    EditedBy_User_label.Text = Program.UserName;

                    // Update answers and marks //
                    //remove all anwers of this visit from database
                    m_And_E.Delete_meVisit_Answers(V_ID);

                    //update users
                    //remove all users of this visit from database
                    m_And_E.Delete_Visit_Users(V_ID);
                }
                else
                {
                    V_Num = Visit_Num_textBox.Text;
                    m_And_E.Insert_Visit(Person_ID, V_Num, _KIND, "",
                        VisitDate_dateTimePicker.Value, Profit
                        , Ans1_textBox.Text, "", Indicators_textBox.Text, Notes_textBox.Text);

                    V_ID = m_And_E.Get_Current_Visit(_KIND, V_Num);
                    l.Insert_Log(
                        "Insert m&e visit:" + V_Num + ":" + _KIND + " of " + MicroProject_ID + " : " +
                        Person_Name_textBox1.Text + " ", " Visit ", Program.UserName, DateTime.Now);
                    CreatedBy_User_label.Text = Program.UserName;

                    ///////////////////////////   Update Visit Task on Taskboard   ///////////////////////////// 
                    var Task_ID = -1;
                    if (V_Num == "1") Task_ID = 26;
                    else if (V_Num == "2") Task_ID = 27;
                    else if (V_Num == "3") Task_ID = 28;
                    else if (V_Num == "4") Task_ID = 29;
                    else if (V_Num == "5") Task_ID = 30;

                    if (Task_ID != -1)
                        TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, Task_ID, true, DateTime.Now);
                    //////////////////////////////////////////////////////////////////////////////////////////

                    //update notification
                    noti.Update_MicroUsers_Notification(MicroProject_ID, "", "Visit M" + V_Num + "-V", Person_Name_textBox1.Text, -21, "1");
                    l.Insert_Log("Set seen=1 to Notification of Visit M" + V_Num + "-V:" + Person_Name_textBox1.Text + "-" + MicroProject_ID,
                        "user_notification visit", Program.UserName, DateTime.Now);
                    ////////////////////////////////////////////////////////////////////////////////////////// 

                    Update_Mood = true;
                }

                // Insert answers and marks //
                string Note;
                int Q_ID, A_ID;
                Note = "";
                Sum_Of_Marks = 0;
                foreach (Control table in panel1.Controls)
                    if (table.GetType() == typeof(TableLayoutPanel))
                        if (table.Name.Contains("ans"))
                            foreach (Control c in table.Controls)
                                if (c.GetType() == typeof(TableLayoutPanel))
                                {
                                    Q_ID = A_ID = -1;

                                    var n1 = c as TableLayoutPanel;
                                    //find the question
                                    if (n1.ColumnCount == 4) continue; //skip table because don't have answers !

                                    var lbl = n1.GetControlFromPosition(5, 0) as Label;
                                    Q_ID = Convert.ToInt32(lbl.Text);
                                    //find Note of this question
                                    var txt = n1.GetControlFromPosition(4, 1) as TextBox;
                                    Note = txt.Text;
                                    //find Answer of this question
                                    foreach (Control bb in n1.Controls)
                                        if (bb.GetType() == typeof(RadioButton))
                                        {
                                            var rad = bb as RadioButton;
                                            if (rad.Checked)
                                            {
                                                var row = ans_ques_dt.Select("A_Name like '" + rad.Text +
                                                                             "' and Question_ID = " + Q_ID);
                                                A_ID = row[0].Field<int>("A_ID");
                                                Sum_Of_Marks += row[0].Field<int>("A_Mark");
                                                break;
                                            }
                                        }

                                    //insert to visit
                                    if (A_ID != -1 && Q_ID != -1)
                                        m_And_E.Insert_meVisit_Answers(V_ID, Q_ID, A_ID, Note);
                                }

                l.Insert_Log(
                    "Insert answers of the visit-" + _KIND + " of " + MicroProject_ID + " : " +
                    Person_Name_textBox1.Text + " ", " Visit ", Program.UserName, DateTime.Now);

                //Calculate Profit Mark and add it to sum//
                var Profit_Mark = m_And_E.Get_Mark_Of_Profit(Profit);
                Sum_Of_Marks += Profit_Mark;
                Sum_label.Text = "المجموع = " + Sum_Of_Marks;
                //save result in database
                m_And_E.Update_VisitResult(V_ID, Sum_Of_Marks);

                var users_of_visit = "";
                //check user comboBoxs and insert
                if (V1_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V1_comboBox.SelectedValue);
                    m_And_E.Insert_Visit_User(V_ID, User_ID, V1_Eval_comboBox.Text);
                    users_of_visit += V1_comboBox.Text + " ";
                }

                if (V2_comboBox.Text != "")
                {
                    User_ID = Convert.ToInt32(V2_comboBox.SelectedValue);
                    m_And_E.Insert_Visit_User(V_ID, User_ID, V2_Eval_comboBox.Text);
                    users_of_visit += V2_comboBox.Text + " ";
                }
                l.Insert_Log(
                    "Insert visitors-" + users_of_visit + " of the visit-" + _KIND + " of " + MicroProject_ID + " : " +
                    Person_Name_textBox1.Text + " ", " Visit ", Program.UserName, DateTime.Now);

                if (Partners != 0)
                {
                    var partners_MessageBox = new Partners_MessageBox(mainForm, MicroProject_ID, Person_ID, V_Num);
                    partners_MessageBox.ShowDialog();
                }
                else
                {
                    //create Evaluation form of this beneficiary //
                    var dialogResult = MessageBox.Show("Do you want to add another visit to this beneficiary visit ? ",
                        " ", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) clear_Visit_boxes(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (V_ID == -1)
                    throw new InvalidOperationException(
                        "لا يوجد زيارة محددة ليتم حذفها ! الرجاء إعادة تحديد الزيارة ثم المحاولة مرة أخرى");
                var dialogResult = MessageBox.Show("Are you sure you want to delete this visit?", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    m_And_E.Delete_Visit(V_ID);
                    ///////////////////////////   Update Visit Task on Taskboard   /////////////////////////////
                    TasksOfProjects = new TasksOfProjects();
                    var Task_ID = -1;
                    if (V_Num == "1") Task_ID = 26;
                    else if (V_Num == "2") Task_ID = 27;
                    else if (V_Num == "3") Task_ID = 28;
                    else if (V_Num == "4") Task_ID = 29;
                    else if (V_Num == "5") Task_ID = 30;

                    if (Task_ID != -1)
                        TasksOfProjects.Update_Task_MicroProject(MicroProject_ID, Task_ID, false, DateTime.Now);
                    //////////////////////////////////////////////////////////////////////////////////////////

                    l.Insert_Log(
                        "Delete visit-" + V_Num + "-" + _KIND + " to " + MicroProject_ID + " : " +
                        Person_Name_textBox1.Text + " ", " Visit ", Program.UserName, DateTime.Now);
                    clear_Visit_boxes(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CheckAllQuestionsHaveAnswers()
        {
            bool answer_checked;
            foreach (Control table in panel1.Controls)
                if (table.GetType() == typeof(TableLayoutPanel))
                    if (table.Name.Contains("ans"))
                        foreach (Control c in table.Controls)
                            if (c.GetType() == typeof(TableLayoutPanel))
                            {
                                answer_checked = false;
                                var n1 = c as TableLayoutPanel;
                                //find the question
                                if (n1.ColumnCount == 4) continue; //skip table because don't have answers ! 
                                //find Answer of this question
                                foreach (Control bb in n1.Controls)
                                    if (bb.GetType() == typeof(RadioButton))
                                    {
                                        var rad = bb as RadioButton;
                                        if (rad.Checked) answer_checked = true;
                                    }

                                if (!answer_checked)
                                    return false;
                            }

            return true;
        }

        #region context menu items click
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (V_ID != -1)
            {
                fill_visit_boxes(V_ID);

                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
            }
            else if (P_Name != "")
            {
                Person_Name_textBox1.Text = P_Name;
                Person_Name_textBox1_Leave(sender, e);
            }
        }

        private void Visit_Num_textBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Visit_Num_textBox.Text))
            {
                int num = -1;
                int.TryParse(Visit_Num_textBox.Text, out num);
                if (num == -1 || num <= 0)
                {
                    MessageBox.Show("الرجاء ادخال رقم زيارة صحيح", "Alert");
                    Visit_Num_textBox.Text = "";
                }
            }
        }

        private void Visit_Num_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != '.' && !(char.IsNumber(e.KeyChar) || e.KeyChar == 8))
                e.Handled = true;
        }

        private void application_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Appplication_New_Form = new Application_New_Form(mainForm, MicroProject_ID, Person_ID, "project");
            mainForm.showNewTab(Appplication_New_Form, "Application", 0);
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

                string project = MicroProject_ID.ToString() + ":" + Person_Name_textBox1.Text;
                var _Form = new NewIdea_Form("M&E Visit-" + Visit_Num_textBox.Text, project, emails);
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