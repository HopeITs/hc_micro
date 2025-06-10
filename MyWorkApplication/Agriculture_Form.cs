using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Agriculture_Form : Form
    {
        public Agriculture_Form(MainForm mainForm, int MicroProject_ID, string PName
            , string CreatedBy, string EditedBy)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            this.Person_Name_textBox.Text = PName;
            this.CreatedBy_User_label.Text = CreatedBy;
            this.EditedBy_User_label.Text = EditedBy;
        }

        private MainForm mainForm;

        private Log l;
        private Select s;
        private User user;
        private NewTheme NewTheme;
        private AgricultureApplication ag;
        private MicroProject mp;

        private int MicroProject_ID;
        private bool Update_Mode, user_mode;

        string answer_query = "";
        private DataTable ans_ques_dt;



        private void Agriculture_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Properties.Settings.Default.theme == "Light") NewTheme.ME_Visit_ToLight(this);
                else NewTheme.ME_Visit_ToNight(this);

                l = new Log();
                s = new Select();
                user = new User();
                ag = new AgricultureApplication();
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
                    case 7: //Lawful
                    case 4: //Guest
                        {
                            addNotesToolStripMenuItem.Visible = false;
                        }
                        break;
                }

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                ans_ques_dt = new DataTable();
                ans_ques_dt = ag.Get_Questions_Answers();
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

        private void Fill_form_answers_questions(DataTable ans_ques_dt)
        {
            try
            {
                var Q_ID = -1;
                foreach (Control table in panel1.Controls)
                    if (table.GetType() == typeof(TableLayoutPanel) && table.Name.Contains("ans"))
                        foreach (Control c in table.Controls)
                            if (c.GetType() == typeof(TableLayoutPanel))
                            {
                                var n1 = c as TableLayoutPanel;
                                var lbl = n1.GetControlFromPosition(0, 0) as Label;

                                //ignore (-) in 1-
                                if (lbl.Text.Length == 3)
                                    Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 2));
                                else Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 1));
                                var row = ans_ques_dt.Select("Question_ID = " + Q_ID, "A_ID");

                                //find Answers of this question 
                                foreach (Control bb in n1.Controls)
                                    if (bb.GetType() == typeof(FlowLayoutPanel)) //if the answers were in the flow panel//
                                    {
                                        bb.Controls.Clear();

                                        for (int i = 0; i < row.Length; i++)
                                        {
                                            CheckBox cbox = new CheckBox();
                                            cbox.AutoSize = false;
                                            cbox.Checked = false;
                                            cbox.MinimumSize = new Size(140, 30);
                                            cbox.MaximumSize = new Size(140, 30);
                                            cbox.Text = row[i].Field<string>("A_Name");
                                            bb.Controls.Add(cbox);
                                        }
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
                    Fill_AgricultureApp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Fill_AgricultureApp()
        {
            user_mode = false;
            clear_boxes(0);

            Fill_Agriculture_Answers(MicroProject_ID);
            Update_Mode = true;
            user_mode = true;
        }

        private void clear_boxes(int j)
        {
            Update_Mode = false;
            CreatedBy_User_label.Text = EditedBy_User_label.Text = "";

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
                                if (bb.GetType() == typeof(FlowLayoutPanel))
                                    foreach (Control cbox in bb.Controls)
                                        if (cbox.GetType() == typeof(CheckBox))
                                        {
                                            var rad = cbox as CheckBox;
                                            rad.Checked = false;
                                        }

        }

        private void Fill_Agriculture_Answers(int MP_ID)
        {
            var ans_dt = ag.Get_Agriculture_Answers(MP_ID);
            int Q_ID, A_ID;

            foreach (Control table in panel1.Controls)
                if (table.GetType() == typeof(TableLayoutPanel) && table.Name.Contains("ans"))
                    foreach (Control c in table.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            var n1 = c as TableLayoutPanel;
                            Q_ID = A_ID = -1;

                            DataRow[] row = null;

                            var lbl = n1.GetControlFromPosition(0, 0) as Label;

                            //ignore (-) in 1-
                            if (lbl.Text.Length == 3)
                                Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 2));
                            else Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 1));

                            //get the answers
                            row = ans_dt.Select("Question_ID = " + Q_ID + " ");
                            if (row == null || row.Length == 0) continue;

                            //find Answers of this question 
                            foreach (Control bb in n1.Controls)
                                if (bb.GetType() == typeof(FlowLayoutPanel)) //if the answers were in the flow panel//
                                    foreach (Control cBox in bb.Controls)
                                        if (cBox.GetType() == typeof(CheckBox))
                                        {
                                            var rad = cBox as CheckBox;
                                            for (int i = 0; i < row.Count(); i++)
                                            {
                                                if (rad.Text == row[i].Field<string>("A_Name"))
                                                {
                                                    rad.Checked = true;
                                                }
                                            }
                                        }
                        }

        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (MicroProject_ID == -1 || MicroProject_ID_textBox.Text == "")
                    throw new InvalidOperationException("الرجاء اختيار المستفيد أولاً ثم إعادة المحاولة ");

                if (!CheckAllQuestionsHaveAnswers())
                    throw new InvalidOperationException("لا يمكن ترك سؤال بدون إجابة ! الرجاء تعبئته والمحاولة مرة أخرى ");

                // Insert answers and marks // 
                int Q_ID, A_ID;
                answer_query = "START TRANSACTION;";
                if (Update_Mode)
                    //remove all anwers of this visit from database
                    answer_query += "DELETE FROM `microproject_ag_answer` where `MicroProject_ID` = " + MicroProject_ID + ";";


                foreach (Control table in panel1.Controls)
                    if (table.GetType() == typeof(TableLayoutPanel) && table.Name.Contains("ans"))
                        foreach (Control c in table.Controls)
                            if (c.GetType() == typeof(TableLayoutPanel))
                            {
                                var n1 = c as TableLayoutPanel;
                                Q_ID = A_ID = -1;

                                var lbl = n1.GetControlFromPosition(0, 0) as Label;

                                //ignore (-) in 1-
                                if (lbl.Text.Length == 3)
                                    Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 2));
                                else Q_ID = Convert.ToInt32(lbl.Text.Substring(0, 1));

                                //find Answers of this question 
                                foreach (Control bb in n1.Controls)
                                    if (bb.GetType() == typeof(FlowLayoutPanel)) //if the answers were in the flow panel//
                                        foreach (Control cBox in bb.Controls)
                                            if (cBox.GetType() == typeof(CheckBox))
                                            {
                                                var rad = cBox as CheckBox;
                                                if (rad.Checked)
                                                {
                                                    var row = ans_ques_dt.Select("A_Name like '" + rad.Text +
                                                                            "' and Question_ID = " + Q_ID);
                                                    A_ID = row[0].Field<int>("A_ID");

                                                    if (A_ID != -1 && Q_ID != -1)
                                                        answer_query += "INSERT INTO `microproject_ag_answer`(`MicroProject_ID`, `Question_ID`, `Answer_ID`) VALUES ("
                                                                + MicroProject_ID + "," + Q_ID + "," + A_ID + ");";

                                                }

                                            }
                            }

                answer_query += "COMMIT;";
                ag.Insert_Agriculture_Answers(answer_query);
                l.Insert_Log(
                    "Insert answers in AgricultureApp of " + MicroProject_ID + " : " +
                    Person_Name_textBox.Text, "Agriculture Application", Program.UserName, DateTime.Now);

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

                if (MicroProject_ID == -1)
                    throw new InvalidOperationException(
                        "لا يوجد زيارة محددة ليتم حذفها ! الرجاء إعادة تحديد الزيارة ثم المحاولة مرة أخرى");

                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف كل معلومات ملحق المشروع الزراعي?", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ag.Delete_Agriculture_Answers(MicroProject_ID);
                    l.Insert_Log(
                        "Delete AgricultureApp of " + MicroProject_ID + " : " +
                        Person_Name_textBox.Text, "Agriculture Application", Program.UserName, DateTime.Now);

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
                if (table.GetType() == typeof(TableLayoutPanel) && table.Name.Contains("ans"))
                    foreach (Control c in table.Controls)
                        if (c.GetType() == typeof(TableLayoutPanel))
                        {
                            answer_checked = false;
                            var n1 = c as TableLayoutPanel;

                            //find Answer of this question
                            foreach (Control bb in n1.Controls)
                            {
                                if (bb.GetType() == typeof(FlowLayoutPanel))
                                    foreach (Control cBox in bb.Controls)
                                        if (cBox.GetType() == typeof(CheckBox))
                                        {
                                            CheckBox rad = cBox as CheckBox;
                                            if (rad.Checked)
                                            {
                                                //عنا بس مجموعة من الاجوبة اللي مسموح نختارن سوا بنفس السؤال//
                                                answer_checked = true;
                                                break;
                                            }
                                        }
                            }
                            if (!answer_checked)
                                return false;
                        }
            return true;
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
