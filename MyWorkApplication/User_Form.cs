using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class User_Form : Form
    {
        private int ID;
        private string imageFilePath, imageName;
        private Log l;


        /// ////////////////////////////
        private List<string> li;

        private List<string> li_user;
        private MainForm mainForm;
        private Thread myTh;

        private byte[] PicArr;

        //Color Pressed_color = Color.FromArgb(207, 20, 43);
        private readonly Color Pressed_BackColor = Color.Silver;
        private readonly Color Pressed_ForeColor = Color.Black;
        private DataRow SelectedDataRow;
        private int selectedIndex;
        private string selectedItemText;
        private User u;

        public User_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        /// ///////////////////////////
        private void User_Form_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.User_ToNight(this);
                else
                    newTheme.User_ToLight(this);

                l = new Log();
                u = new User();

                Bind_Roles();
                Bind_Users("");
                Clear_Boxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Bind_Users(string Username)
        {
            users_dataGridView.Columns.Clear();

            var dt = u.Select_Users(Username,"","");
            users_dataGridView.DataSource = dt;
            var dgC2 = users_dataGridView.Columns["ID"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["IsVisitor"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["IsME"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["IsCommunication"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["IsCashier"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["Password"];
            dgC2.Visible = false;
            dgC2 = users_dataGridView.Columns["Role_ID"];
            dgC2.Visible = false;

            // Adding delete Button //
            var colCB = new DataGridViewButtonColumn();
            colCB.Name = "Delete";
            colCB.HeaderText = "";
            colCB.FlatStyle = FlatStyle.Flat;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCB.MinimumWidth = 30;
            colCB.Width = 30;
            colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            users_dataGridView.Columns.Add(colCB);

            var imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn) users_dataGridView.Columns["#"];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 175;

            //users_dataGridView.Columns["Username"].Width = users_dataGridView.Columns["Role"].Width = 200;
            //users_dataGridView.RowTemplate.Height = 50;
        }

        private void Bind_Roles()
        {
            var dt = u.Select_Roles();
            Role_comboBox1.DataSource = dt;
            Role_comboBox1.DisplayMember = "Role_Name";
            Role_comboBox1.ValueMember = "Role_ID";
            Role_comboBox1.SelectedIndex = -1;
        }

        private void Bind_Tasks(int user_ID)
        {
            li_user = new List<string>();
            li_user = u.Select_User_Tasks(user_ID);

            li = new List<string>();
            li = u.Select_Tasks();

            for (var i = 0; i < li_user.Count; i++)
            {
                selectedItemText = li_user.ElementAt(i);
                li.Remove(selectedItemText);
            }

            AllTasks_listBox.DataSource = li;
            UserTasks_listBox.DataSource = li_user;
        }

        private void users_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void users_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == users_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == users_dataGridView.Columns["Delete"].Index)
                {
                    var dialogResult = MessageBox.Show("Are you sure you want to delete this User?", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // get row ID
                        var ID = Convert.ToInt32(users_dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                        u.Delete_User(ID);

                        l.Insert_Log("Delete The User: " + users_dataGridView.Rows[e.RowIndex].Cells["Username"].Value,
                            "user", Program.UserName, DateTime.Now);
                        users_dataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void users_dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                SelectedDataRow = ((DataRowView) users_dataGridView.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    ID = int.Parse(SelectedDataRow["ID"].ToString()); 
                    Username_textBox.Text = SelectedDataRow["Username"].ToString();
                    Password_textBox.Text = SelectedDataRow["Password"].ToString();
                    Email_textBox.Text = SelectedDataRow["Email"].ToString();
                    Role_comboBox1.Text = SelectedDataRow["Role"].ToString();

                    var IsVisitor = (int) SelectedDataRow["IsVisitor"];
                    var IsME = (int) SelectedDataRow["IsME"];
                    var IsCommunication = (int) SelectedDataRow["IsCommunication"];
                    var IsCashier = (int) SelectedDataRow["IsCashier"];

                    if (IsVisitor == 0)
                        No_V_button_Click(sender, e);
                    else Yes_V_button_Click(sender, e);
                    if (IsME == 0)
                        No_ME_button_Click(sender, e);
                    else Yes_ME_button_Click(sender, e);
                    if (IsCommunication == 0)
                        No_Com_button_Click(sender, e);
                    else Yes_Com_button_Click(sender, e);
                    if (IsCashier == 0)
                        No_Pay_button_Click(sender, e);
                    else Yes_Pay_button_Click(sender, e);

                    if (SelectedDataRow["#"] == DBNull.Value)
                    {
                        ProfilePicture_pictureBox.BackgroundImage = Resources.Unknown_User;
                    }
                    else
                    {
                        //profile picture
                        PicArr = u.Select_ProfilePicture(ID);
                        if (PicArr == null || PicArr.Length == 0)
                        {
                            ProfilePicture_pictureBox.BackgroundImage = Resources.Unknown_User;
                        }
                        else
                        {
                            var ms = new MemoryStream(PicArr);
                            ProfilePicture_pictureBox.BackgroundImage = Image.FromStream(ms);
                        }
                    }

                    Bind_Tasks(ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void users_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == users_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == users_dataGridView.Columns["Delete"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Dark")
                        image = Resources.KAKA_Alii_D;
                    else
                        image = Resources.KAKA_Alii;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new Point((e.CellBounds.Width - size.Width) / 2,
                        (e.CellBounds.Height - size.Height) / 2);
                    location.Offset(e.CellBounds.Location);
                    e.Graphics.DrawImage(image, location);
                    e.Handled = true;
                }
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
                if (Username_textBox.Text == "" || Password_textBox.Text == "" || Role_comboBox1.SelectedIndex == -1)
                    throw new InvalidOperationException("You Can't leave empty fields.. Please check and try again!");
                
                string email = Email_textBox.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                    throw new FormatException();

                string username = Username_textBox.Text;
                string password = Password_textBox.Text;
                string role = Role_comboBox1.SelectedText;


                PicArr = null;
                //////////////////// Convert_Picture(); ////////////////////
                PicArr = ImageToByte(ProfilePicture_pictureBox.BackgroundImage);
                ////////////////////////////////////////////////////////////


                var IsVisitor = -1;
                var IsME = -1;
                var IsCashier = -1;
                var IsCommunication = -1;

                if (Yes_V_button.BackColor == Pressed_BackColor)
                    IsVisitor = 1;
                else IsVisitor = 0;

                if (Yes_Pay_button.BackColor == Pressed_BackColor)
                    IsCashier = 1;
                else IsCashier = 0;

                if (Yes_Com_button.BackColor == Pressed_BackColor)
                    IsCommunication = 1;
                else IsCommunication = 0;

                if (Yes_ME_button.BackColor == Pressed_BackColor)
                    IsME = 1;
                else IsME = 0;



                if (SelectedDataRow != null && ID != -1)
                {
                    u.Update_User(ID, username, password, email,
                        Convert.ToInt32(Role_comboBox1.SelectedValue)
                        , IsVisitor, IsME, IsCommunication, IsCashier, PicArr);
                    l.Insert_Log("Update " + username + "-" + role, "User", Program.UserName, DateTime.Now);
                }
                else
                {
                    u.Insert_User(username, password, email,
                        Convert.ToInt32(Role_comboBox1.SelectedValue)
                        , IsVisitor, IsME, IsCommunication, IsCashier, PicArr);
                    l.Insert_Log("Insert " + username + "-" + role, "User", Program.UserName, DateTime.Now);
                }

                //Clear_Boxes();
                Bind_Users("");
            }
            
            catch (FormatException)
            {
                MessageBox.Show("الرجاء التأكد من كتابة البريد الالكتروني بشكل صحيح", "Email Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Email_textBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void Clear_Boxes()
        {
            Username_textBox.Text = Password_textBox.Text = Email_textBox.Text = "";
            Role_comboBox1.SelectedIndex = -1;
            PicArr = null;
            ProfilePicture_pictureBox.BackgroundImage = null;
            ID = -1;

            SelectedDataRow = null;
            users_dataGridView.ClearSelection();

            Yes_V_button.BackColor = Yes_ME_button.BackColor = Yes_Com_button.BackColor = Yes_Pay_button.BackColor = Color.Transparent;
            No_V_button.BackColor = No_ME_button.BackColor = No_Com_button.BackColor = No_Pay_button.BackColor = Pressed_BackColor;

            Yes_V_button.ForeColor = Yes_ME_button.ForeColor = Yes_Com_button.ForeColor = Yes_Pay_button.ForeColor = Color.Black;
            No_V_button.ForeColor = No_ME_button.ForeColor = No_Com_button.ForeColor = No_Pay_button.ForeColor = Pressed_ForeColor;

            AllTasks_listBox.DataSource = null;
            UserTasks_listBox.DataSource = null;

            li = new List<string>();
            li_user = new List<string>();
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            try
            {
                Clear_Boxes();
                Bind_Users("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Refresh_button.BackgroundImage = Resources.Refresh2_L;
            else Refresh_button.BackgroundImage = Resources.Refresh2_D;
        }

        private void Refresh_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Refresh_button.BackgroundImage = Resources.Refresh2_D;
            else Refresh_button.BackgroundImage = Resources.Refresh2_L;
        }

        #region image

        private void AddImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                myTh = new Thread(CallDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProfilePicture_pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddImage_button_Click(sender, e);
        }

        private void DeleteImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                ProfilePicture_pictureBox.BackgroundImage = Resources.Unknown_User;
                PicArr = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        private void CallDialog()
        {
            var open = new OpenFileDialog();
            open.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            var res = open.ShowDialog();
            if (res == DialogResult.OK)
            {
                imageFilePath = open.FileName;
                imageName = Path.GetFileName(open.FileName);
                //ProfilePicture_pictureBox.ImageLocation = imageFilePath;

                PicArr = null;
                var fs = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                PicArr = br.ReadBytes((int) fs.Length);

                if (PicArr == null || PicArr.Length == 0)
                {
                    ProfilePicture_pictureBox.BackgroundImage = Resources.Unknown_User;
                }
                else
                {
                    var ms = new MemoryStream(PicArr);
                    ProfilePicture_pictureBox.BackgroundImage = Image.FromStream(ms);
                }
            }
        }

        #endregion

        #region task

        private void AddTask_button_Click(object sender, EventArgs e)
        {
            try
            {
                selectedItemText = AllTasks_listBox.SelectedItem.ToString();
                selectedIndex = AllTasks_listBox.SelectedIndex;

                li_user.Add(selectedItemText);
                if (li != null) li.RemoveAt(selectedIndex);
                DataBinding();

                u.Insert_Task(ID, selectedItemText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataBinding()
        {
            //throw new NotImplementedException();
            AllTasks_listBox.DataSource = null;
            AllTasks_listBox.DataSource = li;

            UserTasks_listBox.DataSource = null;
            UserTasks_listBox.DataSource = li_user;
        }

        private void RemoveTask_button_Click(object sender, EventArgs e)
        {
            try
            {
                selectedItemText = UserTasks_listBox.SelectedItem.ToString();
                selectedIndex = UserTasks_listBox.SelectedIndex;

                li.Add(selectedItemText);
                li_user.Remove(selectedItemText);
                //UserTasks_listBox.Items.RemoveAt(UserTasks_listBox.Items.IndexOf(UserTasks_listBox.SelectedItem));
                DataBinding();

                u.Remove_Task(ID, selectedItemText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region btn check coloring

        private void Yes_ME_button_Click(object sender, EventArgs e)
        {
            No_ME_button.BackColor = Color.Transparent;
            No_ME_button.ForeColor = Color.Black;
            Yes_ME_button.BackColor = Pressed_BackColor;
            Yes_ME_button.ForeColor = Pressed_ForeColor;
        }
        private void No_ME_button_Click(object sender, EventArgs e)
        {
            No_ME_button.BackColor = Pressed_BackColor;
            No_ME_button.ForeColor = Pressed_ForeColor;
            Yes_ME_button.BackColor = Color.Transparent;
            Yes_ME_button.ForeColor = Color.Black;
        }

        private void Yes_Com_button_Click(object sender, EventArgs e)
        {
            No_Com_button.BackColor = Color.Transparent;
            No_Com_button.ForeColor = Color.Black;
            Yes_Com_button.BackColor = Pressed_BackColor;
            Yes_Com_button.ForeColor = Pressed_ForeColor;
        }
        private void No_Com_button_Click(object sender, EventArgs e)
        {
            No_Com_button.BackColor = Pressed_BackColor;
            No_Com_button.ForeColor = Pressed_ForeColor;
            Yes_Com_button.BackColor = Color.Transparent;
            Yes_Com_button.ForeColor = Color.Black;
        }

        private void Yes_Pay_button_Click(object sender, EventArgs e)
        {
            No_Pay_button.BackColor = Color.Transparent;
            No_Pay_button.ForeColor = Color.Black;
            Yes_Pay_button.BackColor = Pressed_BackColor;
            Yes_Pay_button.ForeColor = Pressed_ForeColor;
        } 
        private void No_Pay_button_Click(object sender, EventArgs e)
        {
            No_Pay_button.BackColor = Pressed_BackColor;
            No_Pay_button.ForeColor = Pressed_ForeColor;
            Yes_Pay_button.BackColor = Color.Transparent;
            Yes_Pay_button.ForeColor = Color.Black;
        }

        private void Yes_V_button_Click(object sender, EventArgs e)
        {
            No_V_button.BackColor = Color.Transparent;
            No_V_button.ForeColor = Color.Black;
            Yes_V_button.BackColor = Pressed_BackColor;
            Yes_V_button.ForeColor = Pressed_ForeColor;
        }
        private void No_V_button_Click(object sender, EventArgs e)
        {
            No_V_button.BackColor = Pressed_BackColor;
            No_V_button.ForeColor = Pressed_ForeColor;
            Yes_V_button.BackColor = Color.Transparent;
            Yes_V_button.ForeColor = Color.Black;
        }

        #endregion

        private void ShowPassword_button_Click(object sender, EventArgs e)
        { 
            if (Password_textBox.PasswordChar == '*')
            {
                Password_textBox.PasswordChar = '\0';
                string Pass = Password_textBox.Text;
                Password_textBox.Clear();
                Password_textBox.Text = Pass;
                ShowPassword_button.BackgroundImage = Properties.Resources.show_password_true;
                toolTip1.SetToolTip(ShowPassword_button, "Hide Password");
            }
            else if (Password_textBox.PasswordChar == '\0')
            {
                Password_textBox.PasswordChar = '*';
                string Pass = Password_textBox.Text;
                Password_textBox.Clear();
                Password_textBox.Text = Pass;
                ShowPassword_button.BackgroundImage = Properties.Resources.show_password_false;
                toolTip1.SetToolTip(ShowPassword_button, "Show Password");
            } 
        }
        
        

        private void Role_comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (Role_comboBox1.Text.Contains("Financial"))
                Yes_Pay_button_Click(sender, e);
            else No_Pay_button_Click(sender, e);
        }
    }
}