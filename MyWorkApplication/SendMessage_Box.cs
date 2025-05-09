using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class SendMessage_Box : Form
    {
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private bool dragging;
        private DataTable dt;
        private Log l;

        private Select s;
        private UserNotification u;

        public SendMessage_Box()
        {
            InitializeComponent();
        }
        private void SendMessage_Box_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.MessageBox_ToNight(this);
                else
                    newTheme.MessageBox_ToLight(this);

                CheckUserPermission();
                s = new Select();
                u = new UserNotification();
                l = new Log();

                bind_Users_into_ComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bind_Users_into_ComboBox()
        {
            dt = s.select_users();
            Users_comboBox.DataSource = dt;
            Users_comboBox.DisplayMember = "Users";
            Users_comboBox.ValueMember = "U_ID";
            Users_comboBox.SelectedIndex = -1;
        }

        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        private void CheckUserPermission()
        {
            if (Program.role == 1 || Program.role == 8) //admin
                MicroProject_ID_textBox.Visible = mp_label.Visible = true;
            else
                MicroProject_ID_textBox.Visible = mp_label.Visible = false;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                int user_ID;
                if (Users_comboBox.Text == "")
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        user_ID = int.Parse(dt.Rows[i]["U_ID"].ToString());
                        u.Insert_UserNotification(Date_dateTimePicker.Value.ToString("yyyy/MM/dd"), replaceQuotation(Body_richTextBox.Text),
                            replaceQuotation(P_Name_richTextBox.Text), -1, user_ID, Program.userID, -100);
                    }
                }
                else
                {
                    user_ID = Convert.ToInt32(Users_comboBox.SelectedValue);
                    u.Insert_UserNotification(Date_dateTimePicker.Value.ToString("yyyy/MM/dd"), replaceQuotation(Body_richTextBox.Text),
                        replaceQuotation(P_Name_richTextBox.Text), -1, user_ID, Program.userID, -100);
                }

                MessageBox.Show("Message sent successfully!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void Notification_Box_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void Notification_Box_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Notification_Box_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Save_button_MouseEnter(object sender, EventArgs e)
        { 
                if (Settings.Default.theme == "Dark")
                    Save_button.BackgroundImage = Properties.Resources.BtnBackground_D;
                else Save_button.BackgroundImage = Properties.Resources.BtnBackground_L; 
                 
        }

        private void Save_button_MouseLeave(object sender, EventArgs e)
        {
                if (Settings.Default.theme == "Dark")
                    Save_button.BackgroundImage = Properties.Resources.BtnBackground_L;
                else Save_button.BackgroundImage = Properties.Resources.BtnBackground_D;
        }
    }
}