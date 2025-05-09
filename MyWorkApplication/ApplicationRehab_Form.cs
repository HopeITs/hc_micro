using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class ApplicationRehab_Form : Form
    {
        public ApplicationRehab_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        public ApplicationRehab_Form(MainForm mainForm,int MicroProject_ID)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
        }

        private MainForm mainForm; 

        private Log l;
        private Select s;
        private User user;
        private Street st;
        private NewTheme NewTheme;
        private RehabApplication ra;
        private MicroProject mp;

        private int MicroProject_ID, R_ID; 
        private bool Update_Mode,user_mode;

        string answer_query = "";
        private DataTable ans_ques_dt; 
         
        private void ApplicationRehab_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Properties.Settings.Default.theme == "Light") NewTheme.ME_Visit_ToLight(this);
                else NewTheme.ME_Visit_ToNight(this);
                
               
                l = new Log();
                s = new Select();
                user = new User();
                st = new Street();
                ra = new RehabApplication();
                mp = new MicroProject();

                switch ( Program.role)
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
                    case 7: //Lawful
                    case 4: //Guest
                        {
                            addNotesToolStripMenuItem.Visible = false;
                        }
                        break;
                }

                //////////////////// handle auto scrolling for all comboBoxes //////////////////////
                Street_comboBox.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                ///////////////////////////////////////////////////////////////////////////////////////
                 
                Street_bind();

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                ans_ques_dt = new DataTable();
                ans_ques_dt = ra.Get_Questions_Answers();
                Fill_form_answers_questions(ans_ques_dt);

                Update_Mode = false;

                if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Street_bind()
        {
            DataTable st_st = st.Select("");
            Street_comboBox.DataSource = null;
            Street_comboBox.DisplayMember = "Name";
            Street_comboBox.ValueMember = "ID";
            Street_comboBox.DataSource = st_st;
            Street_comboBox.SelectedIndex = -1;
        }
        //prevent auto scrolling in comboboxs//
        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
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

                                    //ignore the table that has the lables//
                                    if (n1.Name.Contains("_h")) continue;

                                    var lbl =  n1.GetControlFromPosition(0 ,1) as Label; 

                                    Q_ID = Convert.ToInt32(lbl.Text);
                                    var row = ans_ques_dt.Select("Question_ID = " + Q_ID, "A_ID");

                                    //find Answer of this question
                                    var index = 0;
                                    for (var i = 1; i < row.Length; i++)
                                    {
                                        var rado = n1.GetControlFromPosition(i, 0) as CheckBox;
                                        rado.Text = row[index++].Field<string>("A_Name");
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
            try
            {
                if (MicroProject_ID_textBox.Text != "")
                {
                    MicroProject_ID = Convert.ToInt32(MicroProject_ID_textBox.Text);
                    Fill_RehabilitationApp(MicroProject_ID_textBox.Text, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Person_Name_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Person_Name_textBox.Text != "")
                {
                    Fill_RehabilitationApp("", Person_Name_textBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Fill_RehabilitationApp(string MP_ID, string P_Name)
        {
            user_mode = false;
            clear_boxes(0);

            DataTable dt = ra.Get_MicroProjectRehab(MP_ID, P_Name);
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    MicroProject_ID = int.Parse(dt.Rows[0]["MP_ID"].ToString());

                    if (MicroProject_ID_textBox.Text == "")
                        MicroProject_ID_textBox.Text = MicroProject_ID.ToString();

                    Person_Name_textBox.Text = dt.Rows[0]["Beneficiary Name"].ToString();

                    PropertyOwner_textBox.Text = dt.Rows[0]["PropertyOwnership"].ToString();
                    PropertyArea_textBox.Text = dt.Rows[0]["PropertyArea"].ToString();

                    WorkAddress_textBox.Text = dt.Rows[0]["MP_AddressAfterFund"].ToString();

                    if (dt.Rows[0]["MP_Street_ID"] == null || dt.Rows[0]["MP_Street_ID"] == DBNull.Value)
                        Street_comboBox.SelectedIndex = -1;
                    else
                    {
                        int val = (int)dt.Rows[0]["MP_Street_ID"];
                        Street_comboBox.SelectedValue = (int)dt.Rows[0]["MP_Street_ID"];
                    }

                    CreatedBy_User_label.Text = dt.Rows[0]["Created_By"].ToString();
                    EditedBy_User_label.Text = dt.Rows[0]["Edited_By"].ToString();

                    if (dt.Rows[0]["ID"] != DBNull.Value)
                    {
                         R_ID = int.Parse(dt.Rows[0]["ID"].ToString());
                         Fill_Rehabilitation_Answers(R_ID); 

                        mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                        mainForm.MP_ID_label.Text = MicroProject_ID.ToString();

                        Update_Mode = true;
                    }
                     
                    user_mode = true;
                }
        }

        private void Fill_Rehabilitation_Answers(int R_ID)
        {
            #region fill answers of questions

            var ans_dt = ra.Get_Rehab_Answers(R_ID);
            int Q_ID, A_ID;
            string Note, Result;
            int Number;
            double Price;

            foreach (Control table in panel1.Controls)
                if (table.GetType() == typeof(TableLayoutPanel))
                    if (table.Name.Contains("ans"))
                        foreach (Control c in table.Controls)
                            if (c.GetType() == typeof(TableLayoutPanel))
                            {
                                var n1 = c as TableLayoutPanel;
                                if (n1.Name.Contains("_h")) continue;

                                Q_ID = A_ID = -1;

                                Note = Result = "";
                                Price = 0;
                                Number = 0;
                                  
                                DataRow[] row = null;
                                List<int> A_IDs = new List<int>();
                                
                                //find Q_ID
                                var lbl = n1.GetControlFromPosition(0, 1) as Label;
                                Q_ID = Convert.ToInt32(lbl.Text);
                                 
                                //get the answers
                                row = ans_dt.Select("Question_ID = " + Q_ID + " ");
                                if (row == null || row.Length == 0) continue;
                                             
                                for (int i = 0; i < row.Count(); i++)
                                    A_IDs.Add(row[i].Field<int>("Answer_ID")); 
                                     
                                Note = row[0].Field<string>("Note");
                                Number = row[0].Field<int>("Number");
                                Price = row[0].Field<double>("Price");
                                Result = row[0].Field<string>("Result");
                                 
                                var textBox = n1.GetControlFromPosition(4, 0) as TextBox;
                                textBox.Text = Note;
                                textBox = n1.GetControlFromPosition(6, 0) as TextBox;
                                textBox.Text= Number.ToString();
                                textBox = n1.GetControlFromPosition(7, 0) as TextBox;
                                textBox.Text = Price.ToString("N2");
                                textBox = n1.GetControlFromPosition(8, 0) as TextBox;
                                textBox.Text = Result;
                                 
                                //if we have more than 1 answer to the same question//
                                for (int i = 0; i < A_IDs.Count; i++)
                                { 
                                    var row2 = ans_ques_dt.Select("A_ID = " + A_IDs[i] + " ");
                                    foreach(Control bb in n1.Controls)
                                        if (bb.GetType() == typeof(CheckBox))
                                        {
                                            var rad = bb as CheckBox;
                                            if (rad.Text == row2[0].Field<string>("A_Name"))
                                            {
                                                rad.Checked = true;
                                            }
                                        } 
                                }
                            }
              
            #endregion
        }

        private void clear_boxes(int j)
        {
            R_ID = -1; 

            CreatedBy_User_label.Text = EditedBy_User_label.Text = "";
            Update_Mode = false;

            PropertyOwner_textBox.Text = PropertyArea_textBox.Text = WorkAddress_textBox.Text = "";
            Street_comboBox.SelectedIndex = -1;
             
            if (j != 0)
            {
                MicroProject_ID_textBox.Text = Person_Name_textBox.Text = "";
                MicroProject_ID = -1; 
            }

            foreach (Control table in panel1.Controls)
                if (table.GetType() == typeof(TableLayoutPanel))
                    foreach (Control c in table.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                            foreach (Control bb in c.Controls)
                            {
                                if (bb.GetType() == typeof(TextBox) || bb.GetType() == typeof(ComboBox)) bb.Text = "";
                                if (bb.GetType() == typeof(CheckBox))
                                {
                                    var rad = bb as CheckBox;
                                    rad.Checked = false;
                                }
                            } 
        }

        private void CheckUserPermission()
        {
            addNotesToolStripMenuItem.Visible = false;

            switch ( Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        if (Update_Mode)
                            addNotesToolStripMenuItem.Visible = true;
                    }
                    break;
                case 2: //Data
                    {
                    }
                    break;
                case 3: //Financial 
                case 7: //Lawful
                    {
                        throw new InvalidOperationException(" نعتذر ! لا تملك الصلاحية للقيام بهذا الفعل.");
                    }
                    break;
                case 4: //Guest
                    {
                        throw new InvalidOperationException(" نعتذر ! لا تملك الصلاحية للقيام بهذا الفعل.");
                    }
                    break;
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (MicroProject_ID == -1 || Person_Name_textBox.Text == "" || MicroProject_ID_textBox.Text == "")
                    throw new InvalidOperationException("الرجاء اختيار المستفيد أولاً ثم إعادة المحاولة ");
                if (!CheckAllQuestionsHaveAnswers())
                    throw new InvalidOperationException("لا يمكن ترك سؤال بدون إجابة ! الرجاء تعبئته والمحاولة مرة أخرى ");

                //update microproject and add fundAddress and street//
                mp.Update_MP_Place(MicroProject_ID
                    , (Street_comboBox.SelectedValue != null ? int.Parse(Street_comboBox.SelectedValue.ToString()) : -1)
                    , WorkAddress_textBox.Text);
                l.Insert_Log("Update place after fund of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                    "Rehabilitation Application", Program.UserName, DateTime.Now);
                
                if (Update_Mode)
                {
                    //update basic information of rehab app
                    ra.Update_MicroProjectRehab(R_ID, MicroProject_ID, PropertyOwner_textBox.Text, PropertyArea_textBox.Text, Program.UserName);
                    l.Insert_Log(
                        "Update RehabApp of " + MicroProject_ID + " : " +
                        Person_Name_textBox.Text + " ", "Rehabilitation Application", Program.UserName, DateTime.Now);
                    EditedBy_User_label.Text = Program.UserName; 
                }
                else
                { 
                    ra.Insert_MicroProjectRehab(MicroProject_ID, PropertyOwner_textBox.Text, PropertyArea_textBox.Text, Program.UserName);

                    R_ID = ra.Get_Rehab_ID(MicroProject_ID);
                    l.Insert_Log(
                        "Insert RehabApp of " + MicroProject_ID + " : " +
                        Person_Name_textBox.Text + " ", "Rehabilitation Application", Program.UserName, DateTime.Now);
                    CreatedBy_User_label.Text = Program.UserName; 
                }
                 
                // Insert answers and marks //
                string Note,Result;
                int Number = 0;
                double Price = 0;
                int Q_ID, A_ID;
                Note = Result = "";
                answer_query = "START TRANSACTION;";
                if (Update_Mode)
                    //remove all anwers of this visit from database
                    answer_query += "DELETE FROM `microproject_rehab_answer` where `MicroRehab_ID` = " + R_ID + ";";

                foreach (Control table in panel1.Controls)
                    if (table.GetType() == typeof(TableLayoutPanel))
                        if (table.Name.Contains("ans"))
                            foreach (Control c in table.Controls)
                                if (c.GetType() == typeof(TableLayoutPanel))
                                {
                                    Q_ID = A_ID = -1;

                                    var n1 = c as TableLayoutPanel;
                                    if (n1.Name.Contains("_h")) continue;

                                    var lbl = n1.GetControlFromPosition(0, 1) as Label;
                                    Q_ID = Convert.ToInt32(lbl.Text);

                                    //find Note of this question
                                    /////////////////////////////////////////////////////
                                    var txt = n1.GetControlFromPosition(4, 0) as TextBox;
                                    Note = txt.Text; 
                                    txt = n1.GetControlFromPosition(6, 0) as TextBox;
                                    Number = (txt.Text=="" ? 0 : Convert.ToInt32(txt.Text));
                                    txt = n1.GetControlFromPosition(7, 0) as TextBox;
                                    Price = (txt.Text == "" ? 0 : Convert.ToDouble(txt.Text));
                                    txt = n1.GetControlFromPosition(8, 0) as TextBox;
                                    Result = txt.Text;
                                    //////////////////////////////////////////////////////

                                    //find Answer of this question
                                    foreach (Control bb in n1.Controls)
                                        if (bb.GetType() == typeof(CheckBox))
                                        {
                                            var rad = bb as CheckBox;
                                            if (rad.Checked)
                                            {
                                                var row = ans_ques_dt.Select("A_Name like '" + rad.Text +
                                                                             "' and Question_ID = " + Q_ID);
                                                A_ID = row[0].Field<int>("A_ID");

                                                if (A_ID != -1 && Q_ID != -1)
                                                    answer_query += "INSERT INTO `microproject_rehab_answer`(`MicroRehab_ID`, `Question_ID`, `Answer_ID`, `Note`, `Number`, `Price`, `Result`) VALUES ("
                                                          + R_ID + "," + Q_ID + "," + A_ID + ",N'"
                                                          + Note + "'," + Number + "," + Price + ",N'" + Result + "' );";
                                            }
                                        }
                                }
                  
                answer_query += "COMMIT;";
                ra.Insert_Rehab_Answers(answer_query); 
                l.Insert_Log(
                    "Insert answers in RehabApp-" + R_ID + " of " + MicroProject_ID + " : " +
                    Person_Name_textBox.Text,"Rehabilitation Application", Program.UserName, DateTime.Now);
                  
                Update_Mode = true; 
                MessageBox.Show("تم حفظ المعلومات بنجاح", "Save");
               
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

                if (R_ID == -1)
                    throw new InvalidOperationException(
                        "لا يوجد زيارة محددة ليتم حذفها ! الرجاء إعادة تحديد الزيارة ثم المحاولة مرة أخرى");
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف كل معلومات ملحق الترميم الخاص بهذا المشروع?", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ra.Delete_MicroProjectRehab(R_ID); 
                    l.Insert_Log(
                        "Delete RehabApp-" + R_ID + " of " + MicroProject_ID + " : " +
                        Person_Name_textBox.Text, "Rehabilitation Application", Program.UserName, DateTime.Now);

                    //update microproject and add fundAddress and street//
                    mp.Update_MP_Place(MicroProject_ID, -1 , "");
                    l.Insert_Log("Delete place after fund of " + MicroProject_ID + " : " + Person_Name_textBox.Text + " ",
                        "Rehabilitation Application", Program.UserName, DateTime.Now);
                    
                    clear_boxes(1);
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
                                //ignore the table that has the titles
                                if (n1.Name.Contains("_h")) continue;

                                var lbl = n1.GetControlFromPosition(0, 1) as Label;
                                int Q_ID = Convert.ToInt32(lbl.Text);
                                int ans_count = 0, max_ans_count = 10;
                                // all the questions above 9 have just 1 answer 
                                if (Q_ID >= 9) max_ans_count = 1;
                                // this questions can be without answers
                                if (Q_ID == 1 || Q_ID == 4 || Q_ID == 11)
                                {
                                    answer_checked = true;
                                    break; //ignore checking this questions
                                }

                                //find Answer of this question
                                foreach (Control bb in n1.Controls)
                                    if (bb.GetType() == typeof(CheckBox))
                                    {
                                        var rad = bb as CheckBox;
                                        if (rad.Checked)
                                        {
                                            //عنا بس مجموعة من الاجوبة اللي مسموح نختارن سوا بنفس السؤال// 
                                            ans_count++;
                                            if(ans_count > max_ans_count)
                                            {
                                                answer_checked = false;
                                                continue;
                                            }
                                            answer_checked = true; 
                                        }
                                    } 

                                if (!answer_checked)
                                    return false;
                            }

            return true;
        }

        private void IntNumbers_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                var txtBox = sender as TextBox;
                if (txtBox != null)
                {
                    string txt = txtBox.Text.Trim();
                    if (txt != "" && txt != "0")
                    {
                        int value = 0;
                        int.TryParse(txt,out value);

                        if (value == 0)
                        {
                            txtBox.Text = "";
                            txtBox.Focus();
                        }
                        else txtBox.Text = value.ToString();
                    }
                }
            }
            catch(Exception ex)
            { MessageBox.Show("الرجاء إدخال قيمة رقمية"); }
        }
        private void DoubleNumbers_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                var txtBox = sender as TextBox;
                if (txtBox != null)
                {
                    string txt = txtBox.Text.Trim();
                    if (txt != "" && txt != "0")
                    {
                        double value = 0;
                        double.TryParse(txt, out value);

                        if (value == 0)
                        {
                            txtBox.Text = "";
                            txtBox.Focus();
                        }
                        else txtBox.Text = value.ToString("N");
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show("الرجاء إدخال قيمة رقمية"); }
        }


        #region context menu items click
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                MicroProject_ID_textBox_Leave(sender, e);
            }
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
                var _Form = new NewIdea_Form("Rehabilitaion Form", project, emails);
                _Form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion

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
    }
}
