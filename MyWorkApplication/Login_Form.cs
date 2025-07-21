using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {

            InitializeComponent();
            Version_label.Text
                = "Micro Projects 2018 - " + DateTime.Now.Year.ToString()
                + " |  V" + Assembly.GetExecutingAssembly().GetName().Version;

            Copyright_label.Text
                = "2018 - " + DateTime.Now.Year.ToString()
                + " CHRISTIAN HOPE CENTER. All Rights Reserved";

            Screen screen = Screen.FromControl(this);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private Log l;
        private User u;
        private MySqlComponents MySS;
        private byte[] PersonPicArr;
        private char selected_hope;
        private bool program_expired = false;

        bool connection = false; //ADDED BY SARY
        List<string> User_In_Database = new List<string>(); //ADDED BY SARY (User_In_Database)
        DataTable User_info = new DataTable(); //ADDED BY SARY 
        //ADD ANY NEW CENTER HERE AFTER CREATE ITS DATABASE
        List<string> Centers = new List<string> { "Aleppo", "Homs", "Damascus", "Latakia", "AlWadi", "Lebanon"
            , "PTS Aleppo" }; //ADDED BY SARY 

        private void Login_Form_Load(object sender, EventArgs e)
        {
            try
            {
                //Check Expire Date//
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                if (today == Resources.Expired_Date)
                {
                    MessageBox.Show("Program Expired - Contact Administrator", "Micro Projects", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    program_expired = true;
                    Application.Exit();
                }

                MySS = new MySqlComponents();
                l = new Log();
                u = new User();
                connection = false;
                User_In_Database.Clear();

                // By Maher
                ///FOR MAKE ENCRYPTED FILE JUST PUT THE "Connections.txt" IN THE DEBUGE FOLDER
                ///THEN UNCOMMENT THE "Encryption.FileEncrypt();"
                ///THEN COPY the "Connections.txt.aes" TO THE "...\\
                ///DELETE THE "Connections.txt" FROM DEBUGE
                //Encryption.FileEncrypt();  

                if (Properties.Settings.Default.first_login)
                {
                    Properties.Settings.Default.profile_picture = "";
                    Properties.Settings.Default.Admin_Visible_arr = "";

                    Properties.Settings.Default.Search0_Visible_arr = "";
                    Properties.Settings.Default.Search1_Visible_arr = "";
                    Properties.Settings.Default.Search2_Visible_arr = "";
                    Properties.Settings.Default.Search3_Visible_arr = "";
                    Properties.Settings.Default.Search4_Visible_arr = "";
                    Properties.Settings.Default.Search5_Visible_arr = "";
                    Properties.Settings.Default.Search6_Visible_arr = "";
                    Properties.Settings.Default.Search7_Visible_arr = "";
                    Properties.Settings.Default.Search8_Visible_arr = "";
                    Properties.Settings.Default.Search9_Visible_arr = "";

                    Properties.Settings.Default.Task_Visible_arr = "";
                    Properties.Settings.Default.Task1_Visible_arr = "";

                    Properties.Settings.Default.RememberMe = false;
                    Properties.Settings.Default.first_login = false;
                    Properties.Settings.Default.Save();
                }
                if (Settings.Default.update_version)
                {
                    Settings.Default.Upgrade();
                    Settings.Default.update_version = false;
                    Settings.Default.Save();
                }

                //Save the profile image in AppData
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "Micro Projects", "Team");
                if (Directory.Exists(specificFolder).Equals(false))
                {
                    Directory.CreateDirectory(specificFolder);
                }

                //Added By Maher
                if (Properties.Settings.Default.RememberMe)
                {
                    Remember_checkBox.Checked = true;
                    if (Settings.Default.username != "")
                    {
                        //By Maher
                        //SecureString UName = Encryption.DecryptString(Settings.Default.username);
                        //UserName_textBox.Text = Encryption.ToInsecureString(UName); 
                        //SecureString PWord = Encryption.DecryptString(Settings.Default.password);
                        //Password_textBox.Text = Encryption.ToInsecureString(PWord);

                        UserName_textBox.Text = Settings.Default.username;
                        Password_textBox.Text = Settings.Default.password;

                        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\Team\" + UserName_textBox.Text + ".png";
                        FileInfo file = new FileInfo(path);
                        if (file.Exists.Equals(true))
                        {
                            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                                ProfilePicture_pictureBox.BackgroundImage = Image.FromStream(stream);
                        }
                        else ProfilePicture_pictureBox.BackgroundImage = Resources.login_profile;
                    }
                    //ProfilePicture_pictureBox.BackgroundImage = Resources.login_profile;
                }
                else
                {

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\Team\" + UserName_textBox.Text + ".png";
                    FileInfo file = new FileInfo(path);
                    //DELETE IMAGE FILE
                    file = new FileInfo(path);
                    if (file.Exists.Equals(true) && !Settings.Default.RememberMe)
                        file.Delete();
                    ProfilePicture_pictureBox.BackgroundImage = Resources.login_profile;

                    Remember_checkBox.Checked = false;
                    UserName_textBox.Text = "username";
                    Password_textBox.Text = "password";
                }
                UserName_textBox.Select(UserName_textBox.Text.Length, UserName_textBox.Text.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Connect_button_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                DataTable dt = new DataTable();

                if (!Online_checkBox.Checked) //offline
                    Properties.Settings.Default.Online = false;
                else if (Online_checkBox.Checked) //online
                    Properties.Settings.Default.Online = true;

                Properties.Settings.Default.Save();

                for (int i = 0; i < Centers.Count; i++)
                {
                    //Properties.Settings.Default.selected_hope = Centers[i].ToString();
                    //Properties.Settings.Default.Save();
                    Program.selected_database = Centers[i].ToString();
                    Program.set_ConnectionString();

                    if (Program.firstbuildConnection())
                    {
                        connection = true;

                        dt = u.Get_Version();
                        if (dt == null)
                            continue;

                        var version1 = new Version(Application.ProductVersion.ToString());
                        var version2 = new Version(dt.Rows[0].Field<string>("l_version"));

                        var result = version1.CompareTo(version2);
                        if (result > 0)
                        {
                            u.Update_Version(Application.ProductVersion.ToString());
                        }
                        else if (result < 0)
                        {
                            // Using old version
                            MessageBox.Show("أنت تستخدم نسخة قديمة من البرنامج" + Environment.NewLine + "الرجاء التحديث إلى النسخة ذات الرقم" + Environment.NewLine + version2.ToString(), "Error");
                            return;
                        }
                        else
                        {
                            // Versions are equal - Do nothing
                        }

                        dt = Check_User();
                        if (dt.Rows.Count >= 1) //user exsist
                        {
                            User_In_Database.Add(Centers[i].ToString());
                            User_info = dt;
                        }
                    }
                }

                if (!connection)
                {
                    //Properties.Settings.Default.selected_hope = "";
                    //Properties.Settings.Default.Save(); 

                    Program.selected_database = "";
                    Program.set_ConnectionString();

                    MessageBox.Show("please check your network connection and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (User_In_Database.Count < 1)
                {
                    //Properties.Settings.Default.selected_hope = "";
                    //Properties.Settings.Default.Save();

                    Program.selected_database = "";
                    Program.set_ConnectionString();

                    MessageBox.Show("please check username or password or check if the database is available and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (User_In_Database.Count >= 1)
                {
                    Set_Properties();
                    Set_User_Image();
                }
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL"))
                { MessageBox.Show("please check your network and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else if (ex.Message.Contains("host"))
                { MessageBox.Show("please check the server or your network connection and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Check_Version()
        {
            DataTable dt = u.Get_Version();

            var version1 = new Version(Application.ProductVersion.ToString());
            var version2 = new Version(dt.Rows[0].Field<string>("l_version"));

            var result = version1.CompareTo(version2);
            if (result > 0)
            {
                u.Update_Version(Application.ProductVersion.ToString());
            }
            else if (result < 0)
            {
                // Using old version
                MessageBox.Show("أنت تستخدم نسخة قديمة من البرنامج" + Environment.NewLine + "الرجاء التحديث إلى النسخة ذات الرقم" + Environment.NewLine + version2.ToString(), "Error");
                return;
            }
            else
            {
                // Versions are equal - Do nothing
            }
        }

        private DataTable Check_User()
        {
            string query = "select * from `user` " +
                " where UserName like '" + UserName_textBox.Text + "' " +
                " and Password like '" + Password_textBox.Text + "' ";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Program.MyFirstConn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Program.MyFirstConn.Close();
            return dt;

        }

        private void Set_Properties()
        {
            Program.User_In_Database = User_In_Database;
            Program.selected_database = User_In_Database[0];

            Program.set_ConnectionString();

            User_info = u.Select_Users(UserName_textBox.Text, "", "");
            Program.userID = User_info.Rows[0].Field<int>("ID");
            Program.UserName = User_info.Rows[0].Field<string>("Username");
            Program.Password = User_info.Rows[0].Field<string>("Password");
            Program.role = User_info.Rows[0].Field<int>("Role_ID");

            if (Remember_checkBox.Checked)
            {
                //save properties settings
                Settings.Default.RememberMe = true;
                //var x = Encryption.EncryptString(Encryption.ToSecureString(Program.UserName));
                //Settings.Default.username = x; 
                //var y = Encryption.EncryptString(Encryption.ToSecureString(Program.Password));
                //Settings.Default.password = y;
                Settings.Default.username = Program.UserName;
                Settings.Default.password = Program.Password;
            }
            else
            {
                //clear properties settings
                Settings.Default.RememberMe = false;
                Settings.Default.username = Settings.Default.password = "";
            }
            Properties.Settings.Default.Save();
        }

        private void Select_User_Profile()
        {
            PersonPicArr = null;
            Program.buildConnection();
            var query = "select ProfilePicture from `user` where UserName like '" + UserName_textBox.Text +
                        "' and Password like '" + Password_textBox.Text + "' ";
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            byte[] query_array = dt.Rows[0].Field<byte[]>(0);
            if (query_array != null && query_array.Length > 0)
            {
                MySqlDataReader reader = sc.ExecuteReader();
                reader.Read();
                PersonPicArr = (byte[])reader[0];

                reader.Close();
            }
            Program.MyConn.Close();
        }
        private void Set_User_Image()
        {
            FileInfo file;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\Team\" + Program.UserName + ".png";

            //GET IMAGE IF NOT EXIST
            file = new FileInfo(path);
            if (file.Exists.Equals(false))
            {
                //PersonPicArr = u.Select_ProfilePicture(dt.Rows[0].Field<int>("ID"));
                Select_User_Profile();

                if (PersonPicArr != null && PersonPicArr.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(PersonPicArr);

                    using (FileStream file2 = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
                        ms.CopyTo(file2);
                }
            }

            //SET IMAGE
            file = new FileInfo(path);
            if (file.Exists.Equals(true))
            {

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    ProfilePicture_pictureBox.BackgroundImage = Image.FromStream(stream);
                }

            }
            else
                ProfilePicture_pictureBox.BackgroundImage = Properties.Resources.login_profile;

            //DELETE IMAGE FILE
            file = new FileInfo(path);
            if (file.Exists.Equals(true) && !Properties.Settings.Default.RememberMe)
            {
                file.Delete();
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            //Added By Maher
            Program.selected_database = "";
            Program.set_ConnectionString();

            //
            Application.Exit();
        }

        private void Close_button_MouseEnter(object sender, EventArgs e)
        {
            Close_button.BackgroundImage = Resources.login_close2;
        }

        private void Close_button_MouseLeave(object sender, EventArgs e)
        {
            Close_button.BackgroundImage = Resources.login_close1;
        }

        private void Connect_button_MouseEnter(object sender, EventArgs e)
        {
            Connect_button.BackgroundImage = Resources.login01;
        }

        private void Connect_button_MouseLeave(object sender, EventArgs e)
        {
            Connect_button.BackgroundImage = Resources.login00;
        }

        private void ShowPassword_button_Click(object sender, EventArgs e)
        {
            if (Password_textBox.PasswordChar == '*')
            {
                Password_textBox.PasswordChar = '\0';
                string Pass = Password_textBox.Text;
                Password_textBox.Clear();
                Password_textBox.Text = Pass;
                ShowPassword_button.BackgroundImage = Properties.Resources.login_eye11;
                toolTip1.SetToolTip(ShowPassword_button, "Hide Password");
            }
            else if (Password_textBox.PasswordChar == '\0')
            {
                Password_textBox.PasswordChar = '*'; //•
                string Pass = Password_textBox.Text;
                Password_textBox.Clear();
                Password_textBox.Text = Pass;
                ShowPassword_button.BackgroundImage = Properties.Resources.login_eye00;
                toolTip1.SetToolTip(ShowPassword_button, "Show Password");
            }
        }


        //private void checkBox1_Paint(object sender, PaintEventArgs e)
        //{
        //    Point pt = new Point(e.ClipRectangle.Left + 2, e.ClipRectangle.Top + 4);
        //    Rectangle rect = new Rectangle(pt, new Size(12, 12));
        //    if (Remember_checkBox.Checked)
        //    {
        //        using (Font wing = new Font("Wingdings", 10f))
        //            e.Graphics.DrawString("ü", wing, Brushes.White, rect);
        //    }
        //    else if (Online_checkBox.Checked)
        //    {
        //        using (Font wing = new Font("Wingdings", 10f))
        //            e.Graphics.DrawString("ü", wing, Brushes.White, rect);
        //    }
        //    e.Graphics.DrawRectangle(Pens.Red, rect);
        //}
    }
}