using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class NewIdea_Form : Form
    {

        public NewIdea_Form(string Form_Name, string Project, List<string> emails)
        {
            InitializeComponent();
            this.Form_Name = Form_Name;
            this.Project = Project;
            this.emails = emails;
        }
        public NewIdea_Form(string Form_Name, string Project)
        {
            InitializeComponent();
            this.Form_Name = Form_Name;
            this.Project = Project;
        }

        User u;
        string Form_Name;
        string Project;
        List<string> emails;

        private void NewIdea_Form_Load(object sender, EventArgs e)
        {
            u = new User();
            var newTheme = new NewTheme();
            if (Settings.Default.theme == "Dark")
                newTheme.NotificationBox_ToNight(this);
            else
                newTheme.NotificationBox_ToLight(this);


            if (Form_Name == "New Idea")
            {
                Subject_TextBox.Visible = false;
                top_label.Visible = true;
                Subject_TextBox.Text = "Micro App. Notes From:" + Program.UserName;

                NewIdea_TextBox.Text = "أكتب رسالتك هنا";
                NewIdea_TextBox.ForeColor = Color.LightGray;
            }
            else
            {
                top_label.Visible = false;
                Subject_TextBox.Visible = true;
                Subject_TextBox.Text = "Micro App. Notes on:" + Form_Name + " From:" + Program.UserName;

                NewIdea_TextBox.Text = Project + Environment.NewLine;
                NewIdea_TextBox.SelectionStart = NewIdea_TextBox.Text.Length;
                NewIdea_TextBox.SelectionLength = 0;
            }
        }

        private void Send_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewIdea_TextBox.Text) || NewIdea_TextBox.Text == "أكتب رسالتك هنا")
                {
                    throw new InvalidOperationException("لا يمكن إرسال رسالة فارغة");
                }

                if (string.IsNullOrWhiteSpace(Subject_TextBox.Text))
                {
                    MessageBox.Show("لا يمكن إرسال الرسالة بدون عنوان", "Error");
                }
                else
                {
                    string Message = NewIdea_TextBox.Text + Environment.NewLine + "Date:" + DateTime.Now.ToString();
                    if (Form_Name == "New Idea")
                        u.Send_New_Idea(Subject_TextBox.Text, Message);
                    else u.Send_New_NoteEmail(Subject_TextBox.Text, Message, emails);

                    MessageBox.Show("تم الإرسال بنجاح", "Confirmation");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Send_Button_MouseEnter(object sender, EventArgs e)
        {
            Send_Button.BackgroundImage = Properties.Resources.BtnBackground_L;
        }

        private void Send_Button_MouseLeave(object sender, EventArgs e)
        {
            Send_Button.BackgroundImage = Properties.Resources.BtnBackground_D;
        }

        private void NewIdea_TextBox_Enter(object sender, EventArgs e)
        {
            if (NewIdea_TextBox.Text == "أكتب رسالتك هنا")
            {
                NewIdea_TextBox.Clear();
                NewIdea_TextBox.ForeColor = Color.Black;
            }
        }

        private void NewIdea_TextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewIdea_TextBox.Text))
            {
                NewIdea_TextBox.Text = "أكتب رسالتك هنا";
                NewIdea_TextBox.ForeColor = Color.LightGray;
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
