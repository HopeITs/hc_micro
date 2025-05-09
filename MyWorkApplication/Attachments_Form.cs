using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class Attachments_Form : Form
    {
        private byte[] arr;
        private FtpConnector c;
        private Document d;
        private Images img;
        private string[] FullPicNames;
        private string FilePicName;

        private string imageFilePath, imageName, Image_Path;
            //, image_type;
         
        // The current scale.
        private float ImageScale = 0.4f; 
        private float zoomLevel = 0.4f; // Initial zoom level

        private string saveFolderPath = "";
        private int ImageWidth, ImageHeight;
        private int index;
        private Log l; private User user;
        private readonly MainForm mainForm;
        private int MicroProject_ID, Image_ID, ImageType_ID;
        private readonly int Selected_Image_ID;
        private Point MouseDownLocation;
        private MySqlComponents MySS;
        private Thread myTh;

        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog open;
        private Select s;
        bool success;
        bool user_mode;
        DataSet img_types_ds;
        DataSet images_ds;
        string user_type_ids = "";

        public Attachments_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm; 
            MicroProject_ID = Image_ID = -1;
        }
        public Attachments_Form(int MicroProject_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            Image_ID = -1;
        } 
        public Attachments_Form(int MicroProject_ID, int I_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            Selected_Image_ID = I_ID;
        }

        private void Attachments_Form_Load(object sender, EventArgs e)
        {
            try
            {
                user_mode = false;
                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                c = new FtpConnector();
                d = new Document();
                user = new User();
                img = new Images();

                user_type_ids = LoadImageTypes();
                // Find the "all" RadioButton by its Name
                //RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                //allRadioButton.Checked = true; 

                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Attachment_ToNight(this);
                else
                    newTheme.Attachment_ToLight(this);

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                if (MicroProject_ID != -1 )
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    Program.buildConnection();
                    var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                                + " , CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'"
                                + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                                + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                                + " where PMP.MicroProject_ID = " + Convert.ToInt32(MicroProject_ID_textBox.Text);
                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    MySS.da = new MySqlDataAdapter(MySS.sc);
                    MySS.dt = new DataTable();
                    MySS.da.Fill(MySS.dt);

                    Program.MyConn.Close();
                    if (MySS.dt != null)
                        if (MySS.dt.Rows.Count > 0)
                        {
                            MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                            Person_Name_textBox.Text = MySS.dt.Rows[0]["Beneficiary Name"].ToString();

                            Image_bind(MicroProject_ID, user_type_ids);

                            mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                            mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                        }

                    if (Selected_Image_ID != -1)
                    {
                        for (var i = 0; i < ImageName_comboBox.Items.Count; i++)
                        {
                            ImageName_comboBox.SelectedIndex = i;
                            if (Convert.ToInt32(ImageName_comboBox.SelectedValue) == Selected_Image_ID) break;
                        }
                    }
                }

                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Attachments";
                mainForm.KeyPress += Attachments_Form_KeyPress;
                KeyPress += Attachments_Form_KeyPress;

                user_mode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #region imagetype bind radiobuttons
        private string LoadImageTypes()
        {
            img_types_ds = img.SelectImageTypes();
            int yOffset = 0;
            string type_ids = "";

            // Add the "All" RadioButton
            AddRadioButton("الكل", -1, yOffset, "All_radioButton");
            yOffset += 30; // Adjust spacing

            // Iterate over tables and add RadioButtons and Labels
            for (int tableIndex = 0; tableIndex < img_types_ds.Tables.Count; tableIndex++)
            {
                if (img_types_ds.Tables[tableIndex].Rows.Count > 0)
                {
                    yOffset += 5; // Adjust spacing
                    AddLabel(
                          img_types_ds.Tables[tableIndex].Rows[0]["Category"].ToString()
                        , yOffset);
                    yOffset += 30; // Adjust spacing
                    type_ids += img_types_ds.Tables[tableIndex].Rows[0]["ID"].ToString() + ",";

                    for (int i = 0; i < img_types_ds.Tables[tableIndex].Rows.Count; i++)
                    {
                        AddRadioButton(
                              img_types_ds.Tables[tableIndex].Rows[i]["Name"].ToString()
                            , img_types_ds.Tables[tableIndex].Rows[i]["ID"]
                            , yOffset,"");
                        yOffset += 30; // Adjust spacing
                    }
                }
            }

            if (type_ids.Length > 0)
                type_ids = type_ids.Remove(type_ids.Length - 1, 1);

            return type_ids;
        }
        // Add a method to create and add a RadioButton
        void AddRadioButton(string text, object tag, int yOffset,string Name)
        {
            RadioButton radioButton = new RadioButton();
            if(Name!="") radioButton.Name = Name;
            radioButton.Text = text;
            radioButton.Tag = tag;
            radioButton.AutoSize = true;
            radioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            ImageTypes_panel.Controls.Add(radioButton);
            radioButton.Location = new System.Drawing.Point(ImageTypes_panel.Width - radioButton.Width - 10, yOffset);
        }
        // Add a method to create and add a Label
        void AddLabel(string text, int yOffset)
        {
            Label MyLabel = new Label();
            MyLabel.AutoSize = true;
            MyLabel.Font = new Font("Rubik", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            MyLabel.Text = text;
            MyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            ImageTypes_panel.Controls.Add(MyLabel);
            MyLabel.Location = new System.Drawing.Point(ImageTypes_panel.Width - MyLabel.Width - 10, yOffset);
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton radioButton = sender as RadioButton;
                if (radioButton != null && radioButton.Checked)
                {
                    ImageType_ID = int.Parse(radioButton.Tag.ToString());

                    if(ImageType_ID == -1)
                        Image_bind(MicroProject_ID, "");
                    else 
                        Image_bind(MicroProject_ID, ImageType_ID.ToString());
                     
                    //عدم السماح باضافة صورة شخصية من هذه الواجهة - فقط للاستعراض او الحذف
                    if (radioButton.Text == "صورة شخصية")
                        AddImage_button.Visible = false;
                    else AddImage_button.Visible = true;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region zooming functions
        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) // Mouse wheel rolled up (Zoom in)
            {
                //if (zoomLevel < 3.0f) // Limit maximum zoom level to 3x
                //{
                    zoomLevel += 0.1f; // You can adjust the zoom increment as needed
                //}
            }
            else // Mouse wheel rolled down (Zoom out)
            {
                //if (zoomLevel > 0.2f) // Limit minimum zoom level to 20%
                //{
                    zoomLevel -= 0.1f; // You can adjust the zoom decrement as needed
                //}
            }

            // Apply zoom
            ApplyZoom();
        } 
        private void ApplyZoom()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            int newWidth = (int)(pictureBox1.Image.Width * zoomLevel);
            int newHeight = (int)(pictureBox1.Image.Height * zoomLevel);
            pictureBox1.ClientSize = new Size(newWidth, newHeight);

            // Update the TextBox with the zoom level in percentage
            int percentage = (int)((zoomLevel+0.6) * 100);
            Percentage_textBox.Text = percentage + "%";
        }

        private void FitToScreen_button_Click(object sender, EventArgs e)
        {
            zoomLevel = 0.4f; // Initial zoom level

            
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Size = new Size(UnderPicturBox_panel.Width - 5, UnderPicturBox_panel.Height - 5);
            pictureBox1.Location = new Point(0, 0);
            ImageScale = (UnderPicturBox_panel.Width - 5) / pictureBox1.Size.Width;
            Percentage_textBox.Text = ImageScale.ToString("p0");

        }
        #endregion

        //private void InsertImage(int MP_ID, string path, string type)
        //{ 
        //    Program.buildConnection();
        //    MySS.query = "Insert Into `image`(`Image_NewPath`, `Image_Type`, `MicroProject_ID`) values(N'" 
        //                 + path + "',N'"
        //                 + type + "',"
        //                 + MP_ID + ")";
        //    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        //    //MySS.sc.Parameters.Add("@Image_Content", MySqlDbType.Blob);
        //    //MySS.sc.Parameters["@Image_Content"].Value = array;
        //    MySS.sc.ExecuteNonQuery();
        //    Program.MyConn.Close();
        //}

        //private void DeleteImage(int image_ID)
        //{ 
        //    Program.buildConnection(); 
        //    var strCmd = "delete from `image` where `Image_ID` = " + image_ID + " "; 
        //    MySS.sc = new MySqlCommand(strCmd, Program.MyConn);
        //    MySS.sc.ExecuteNonQuery(); 
        //    Program.MyConn.Close();
        //}

        private void Image_bind(int MP_ID, string type_ids)
        {
            try
            {
                user_mode = false;

                imageFilePath = "";
                Image_ID = -1;
                pictureBox1.Image = null;

                images_ds = new DataSet();
                images_ds = img.Select_MP_Images(MP_ID, type_ids);
                ImageName_comboBox.DataSource = null;
                ImageName_comboBox.Text = "";
                ImageName_comboBox.DisplayMember = "Image Path";
                ImageName_comboBox.ValueMember = "ID";
                ImageName_comboBox.DataSource = images_ds.Tables[0];
                
                //fill count of images near image types
                UpdateRadioButtonCounts(images_ds.Tables[1]);
                RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                allRadioButton.Text = $"الكل ({images_ds.Tables[2].Rows[0][0].ToString()})";

                //if (ImageName_comboBox.Items.Count == 0)
                //{
                //    imageFilePath = "";
                //    Image_ID = -1;
                //    pictureBox1.Image = null;
                //    ImageName_comboBox.DataSource = null;
                //    ImageName_comboBox.Text = "";
                //}
                //else
                //{

                user_mode = true;
                if (ImageName_comboBox.Items.Count != 0)
                { 
                    ImagesCount_comboBox.Items.Clear();
                    for (var i = 0; i < ImageName_comboBox.Items.Count; i++)
                        ImagesCount_comboBox.Items.Add(i + 1); 

                    ImagesCount_comboBox.SelectedIndex = 0;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void UpdateRadioButtonCounts(DataTable countsTable)
        {
            foreach (DataRow row in countsTable.Rows)
            {
                string imageType = row["Type"].ToString();
                int count = int.Parse(row["Count"].ToString());

                foreach (Control control in ImageTypes_panel.Controls)
                {
                    if (control is RadioButton radioButton && radioButton.Text.StartsWith(imageType))
                    {
                        radioButton.Text = $"{imageType} ({count})";
                        break;
                    } 
                } 
            } 
        }

        private void ImagesCount_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                {
                    index = ImagesCount_comboBox.SelectedIndex;
                    ImageName_comboBox.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL"))
                { MessageBox.Show("please check your network and try again", "Error"); }
                else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        private void ImageName_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                if (ImageName_comboBox.SelectedValue == null)
                    return;

                Image_ID = Convert.ToInt32(ImageName_comboBox.SelectedValue.ToString());

                string PicName = img.Select_ImageName(Image_ID);
                if (PicName != null || PicName.Length > 0 && PicName != "")
                {
                    string OfflinePicPath = Program.OfflineRoot + @"/" + PicName;
                    string OnlinePicPath = Program.OnlineRoot + @"/" + PicName;

                    bool imageExist_offline = false;
                    bool imageExist_online = false;

                    string Type;
                    if (d.Offline_Exist(PicName))
                    {
                        d.GetNameFromPath(OfflinePicPath, out _, out Type);
                        imageExist_offline = true;
                    }
                    else if (d.Online_Exist(PicName))
                    {
                        d.GetNameFromPath(OnlinePicPath, out _, out Type);
                        imageExist_online = true;
                    }

                    if (imageExist_offline || imageExist_online)
                    {
                        string tempDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\micro_images";
                        System.IO.Directory.CreateDirectory(tempDirectory);

                        //clear pdf webBrowser to be able to delete files 
                        webBrowser1.Navigate("about:blank");
                        webBrowser1.DocumentText = "";
                        webBrowser1.Visible = false;

                        //Clean Temp Directory
                        System.IO.DirectoryInfo di = new DirectoryInfo(tempDirectory);
                        foreach (FileInfo allfiles in di.GetFiles())
                        {
                            try
                            {
                                allfiles.Delete();
                            }
                            catch
                            {

                            }
                        }
                        FileInfo file = new FileInfo(tempDirectory + "\\" + PicName);
                        if (imageExist_online)
                            d.CopyToLocal(OnlinePicPath, tempDirectory + "\\" + PicName , imageExist_online);
                        else
                            d.CopyToLocal(OfflinePicPath, tempDirectory + "\\" + PicName, imageExist_online);
                         
                        string extension = Path.GetExtension(tempDirectory + "\\" + PicName).ToLower();
                        if (extension == ".pdf")
                        {
                            pictureBox1.Visible = false;
                            webBrowser1.Visible = true;
                            webBrowser1.Navigate(tempDirectory + "\\" + PicName);
                        }
                        else
                        {
                            pictureBox1.Visible = true;
                            webBrowser1.Visible = false;
                            pictureBox1.ImageLocation = tempDirectory + "\\" + PicName;
                        }

                        zoomLevel = 0.4f; // Initial zoom level
                        pictureBox1.MouseWheel += PictureBox_MouseWheel;

                        FitToScreen_button_Click(sender, e);
                    }
                    else
                    {
                        img.DeleteImage(Image_ID, MicroProject_ID, PicName);

                        if (imageExist_online) d.DeleteOnline(OnlinePicPath);
                        if (imageExist_offline) d.DeleteOffline(OfflinePicPath);

                        MessageBox.Show("الملف مفقود - تم حذف الاسم من القائمة");

                        //clear poctureBox1
                        //pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                        pictureBox1.ImageLocation = "";
                        Image_bind(MicroProject_ID, user_type_ids);
                    }
                }
                else
                {
                    //clear poctureBox1
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                    pictureBox1.ImageLocation = "";
                }
            } 
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL"))
                { MessageBox.Show("please check your network and try again", "Error"); }
                else if (ex.Message.Contains("host"))
                { MessageBox.Show("please check the server or your network connection and try again", "Error"); }
                else if (ex.Message.Contains("Duplicate entry"))
                { MessageBox.Show("خطأ .. يوجد قيم مكررة", "Error"); }
                else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        //public string Select_Image(int ID)
        //{
        //    Program.buildConnection(); 
        //    string query = "select Image_NewPath from `image` where Image_ID = " + ID + " ";
        //    MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
        //    string result = sc.ExecuteScalar().ToString();
        //    Program.MyConn.Close();
        //    return result;
        //}

        private void CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //Admin_L0 
                case 3: //Financial
                case 7: //Lawful
                case 2: //Data 
                    break;
                case 4: //Guest
                case 9: //Media
                    throw new InvalidOperationException(" You don't have the permission for this action.");
            }
        }
        private void AddImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (MicroProject_ID_textBox.Text == "" || Person_Name_textBox.Text == "" || MicroProject_ID == -1)
                    throw new InvalidOperationException("Please Choose a project first.");
                if (ImageType_ID == -1)
                    throw new InvalidOperationException("Please Choose a type of the image you want to add.");

                //clear file names array
                FullPicNames = null;
                //Array.Clear(FullPicNames, 0, FullPicNames.Length);

                myTh = new Thread(CallDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();

                ImageLocation_textBox.Text = "";
                pictureBox1.ImageLocation = "";

                int success = 0;
                if (FullPicNames != null && FullPicNames.Length > 0)
                { 
                    foreach (var fileName in FullPicNames)
                    {
                        ImageLocation_textBox.Text += fileName + " , ";
                        pictureBox1.ImageLocation = fileName;
                        if (Save_Attachments(fileName)) success++;
                    }
                }

                if (FullPicNames != null && FullPicNames.Length > 0)
                    MessageBox.Show("تم حفظ المعلومات بنجاح" + Environment.NewLine + "المرفقات: نجح" + FullPicNames.Length.ToString() + "/" + success.ToString());
                else MessageBox.Show("تعذّر الحفظ، الرجاء التأكد من اختيار صورة ثم المحاولة مرة ثانية");

                // Find the "all" RadioButton by its Name
                RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                allRadioButton.Checked = true;
                Image_bind(MicroProject_ID, user_type_ids);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
  
        private bool Save_Attachments(string FullAttachName)
        {
            //insert image//
            if (ImageLocation_textBox.Text == "") 
                throw new InvalidOperationException("Please choose the image you want to add");
            if (ImageType_ID == -1) 
                throw new InvalidOperationException("Please choose the image type you want");

            bool sucess = false;
            //foreach (var FullPicName in FullPicNames)
            //{
            //Added By Maher 
            string t;
            d.GetNameFromPath(FullAttachName, out t, out _);

            string new_name = MicroProject_ID + "_img" + DateTime.Now.ToString("yyyyMMdd") + "_" + t;

            while (d.Offline_Exist(new_name) || d.Online_Exist(new_name))
            {
                t = "(1)" + t;
                new_name = MicroProject_ID + "_img" + DateTime.Now.ToString("yyyyMMdd") + "_" + t;
            } 
                 
            ////////////////// Upload Image to server ////////////////////
            if (d.SaveDocument(FullAttachName, new_name))
            {  
                ////////////////// Update/save Image path into image //////////////////// 
                img.InsertImage(MicroProject_ID, new_name, ImageType_ID);
                //l.Insert_Log("insert image:" + new_name + " to " + MicroProject_ID + " ", "Images", Program.UserName, DateTime.Now);

                sucess = true;
            } 
            return sucess;
        }
         
        private void DeleteImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (Image_ID == -1 || ImageName_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("Please choose the image you want to delete");
                var dialogResult = MessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                { 
                    //Added by Maher
                    string PicName = img.Select_ImageName(Image_ID);
                    if (PicName != null || PicName.Length > 0)
                    {
                        string OfflinePicPath = Program.OfflineRoot + @"/" + PicName; 
                        string OnlinePicPath = Program.OnlineRoot + @"/" + PicName;
                         
                        //clear poctureBox1
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                        pictureBox1.ImageLocation = "";

                        if (d.Online_Exist(OnlinePicPath)) d.DeleteOnline(OnlinePicPath);
                        if (d.Offline_Exist(OfflinePicPath)) d.DeleteOffline(OfflinePicPath);
                         
                        //Clear Path From Database
                        img.DeleteImage(Image_ID, MicroProject_ID, PicName);
                        //l.Insert_Log("Delete Image:" + PicName + " of : " + MicroProject_ID + " ", "Image", Program.UserName, DateTime.Now);

                        Image_ID = -1;
                        FilePicName = PicName = "";
                        // Find the "all" RadioButton by its Name
                        RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                        allRadioButton.Checked = true;
                        Image_bind(MicroProject_ID, user_type_ids);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CallDialog()
        {
            open = new OpenFileDialog();
            //open.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            open.Multiselect = true;
            var res = open.ShowDialog();
            if (res == DialogResult.OK)
                FullPicNames = open.FileNames;
            else
                return;
        }
         
        #region download image 
        private void DownloadSelected_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (MicroProject_ID_textBox.Text == "" || Person_Name_textBox.Text == "" || MicroProject_ID == -1)
                    throw new InvalidOperationException("Please Choose a project first.");

                if (pictureBox1.Image == null) throw new InvalidOperationException("Please choose the image you want to download.");

                myTh = new Thread(SaveDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();

                if (success)
                {
                    l.Insert_Log("download image " + ImageLocation_textBox.Text + " from the server: ", "Images",
                      Program.UserName, DateTime.Now);
                    MessageBox.Show("تم حفظ الصورة بنجاح");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DownloadAll_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CheckUserPermission();

                if (MicroProject_ID_textBox.Text == "" || Person_Name_textBox.Text == "" || MicroProject_ID == -1)
                    throw new InvalidOperationException("Please Choose a project first.");

                if (ImageName_comboBox.Items.Count == 0)
                    throw new InvalidOperationException("No images to download.");

                myTh = new Thread(FolderBrowserDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();

                if (success)
                {                
                    l.Insert_Log("download All images of:"+MicroProject_ID+" from the server"
                        , "Images"
                        , Program.UserName
                        , DateTime.Now);
                    // Open the save folder in File Explorer
                    System.Diagnostics.Process.Start("explorer.exe", saveFolderPath);
                    //MessageBox.Show("All images downloaded successfully.");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveDialog()
        {
            success = false;

            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            saveFileDialog1.Title = "Save Image File";
            string _, Type;
            d.GetNameFromPath(pictureBox1.ImageLocation.ToString(), out _, out Type);
            saveFileDialog1.FileName = _;

            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                fs.Close();
                success = true;
            }
            else
                return;
        }
        private void FolderBrowserDialog()
        {
            success = false;
            saveFolderPath = "";
            // Open folder browser dialog to select the save location
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFolderPath = folderDialog.SelectedPath; 

                    foreach (var item in ImageName_comboBox.Items)
                    {                        
                        // Cast item to DataRowView to access the data
                        DataRowView rowView = item as DataRowView;
                        if (rowView == null)
                        {
                            continue;
                        }
                        string imageName = rowView["Image Path"].ToString();
                        //string imageName = item.ToString();
                        //Image image = d.GetImageByName(imageName); // Replace with your method to get image by name
                        byte[] imageBytes = d.GetImageByName(imageName);// Get image bytes by path
                         
                        if (imageBytes != null)
                        {
                            string extension = Path.GetExtension(imageName);
                            string savePath = Path.Combine(saveFolderPath, imageName);

                            try
                            {
                                switch (extension.ToLower())
                                {
                                    case ".jpg":
                                    case ".jpeg":
                                        SaveImage(imageBytes, savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                    case ".png":
                                        SaveImage(imageBytes, savePath, System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                    case ".bmp":
                                        SaveImage(imageBytes, savePath, System.Drawing.Imaging.ImageFormat.Bmp);
                                        break;
                                    case ".gif":
                                        SaveImage(imageBytes, savePath, System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        MessageBox.Show($"Unsupported image format: {extension}");
                                        success = false;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to save image {imageName}: {ex.Message}");
                                success = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Image {imageName} not found.");
                            success = false;
                        }
                    }
                    success = true;
                    //if (success)
                    //{
                    //    MessageBox.Show("All images downloaded successfully.");
                    //} 
                } 
            }
        }
        // Method to save an image to a specified path with a specified format
        private void SaveImage(byte[] imageBytes, string savePath, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            image.Save(fs, format);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image: {ex.Message}");
            }
        }
        #endregion

        //private void ImageType_radioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (Contract_radioButton.Checked)
        //        image_type = "Contract";
        //    else if (Detention_radioButton.Checked)
        //        image_type = "Detention";
        //    else if (InsurancePolicy_radioButton.Checked)
        //        image_type = "InsurancePolicy";
        //    else if (Invoice_radioButton.Checked)
        //        image_type = "Invoice";
        //    else if (Reciept_radioButton.Checked)
        //        image_type = "Reciept";
        //    else if (Rent_radioButton.Checked)
        //        image_type = "Rent";
        //    else if (EconomicFeasibility_radioButton.Checked)
        //        image_type = "EconomicFeasibility";
        //    else if (All_radioButton.Checked) image_type = ""; 
        //    Image_bind(MicroProject_ID, image_type);
        //}

        private void Attachments_Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                var index = -1;
                if (ImageName_comboBox.Text != "")
                    if (ImageName_comboBox.SelectedIndex >= 0 &&
                        ImageName_comboBox.SelectedIndex < ImageName_comboBox.Items.Count)
                        if (e.KeyChar == 39)
                        {
                            index = ImageName_comboBox.SelectedIndex;
                            index++;
                            ImageName_comboBox.SelectedIndex = index;
                            e.Handled = true;
                        }
                        else if (ImageName_comboBox.SelectedIndex > 0 &&
                                 ImageName_comboBox.SelectedIndex <= ImageName_comboBox.Items.Count)
                        {
                            if (e.KeyChar == 37)
                            {
                                index = ImageName_comboBox.SelectedIndex;
                                index--;
                                ImageName_comboBox.SelectedIndex = index;
                                e.Handled = true;
                            }
                        }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseDownLocation = e.Location;
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox1.Left = e.X + pictureBox1.Left - MouseDownLocation.X;
                pictureBox1.Top = e.Y + pictureBox1.Top - MouseDownLocation.Y;
            }
        }
         
        private void First_button_Click(object sender, EventArgs e)
        {
            try
            {
                ImagesCount_comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Last_button_Click(object sender, EventArgs e)
        {
            try
            {
                ImagesCount_comboBox.SelectedIndex = ImageName_comboBox.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Next_button_Click(object sender, EventArgs e)
        {
            try
            {
                var index = ImagesCount_comboBox.SelectedIndex;
                if (index != ImagesCount_comboBox.Items.Count - 1)
                {
                    index++;
                    ImagesCount_comboBox.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Back_button_Click(object sender, EventArgs e)
        {
            try
            {
                var index = ImagesCount_comboBox.SelectedIndex;
                if (index != 0)
                {
                    index--;
                    ImagesCount_comboBox.SelectedIndex = index;
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
                    Program.buildConnection();
                    var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                                + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                                + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                                + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                                + " where PMP.MicroProject_ID = " + Convert.ToInt32(MicroProject_ID_textBox.Text);
                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    MySS.da = new MySqlDataAdapter(MySS.sc);
                    MySS.dt = new DataTable();
                    MySS.da.Fill(MySS.dt);

                    Program.MyConn.Close();
                    if (MySS.dt != null)
                        if (MySS.dt.Rows.Count > 0)
                        {
                            MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                            Person_Name_textBox.Text = MySS.dt.Rows[0]["Beneficiary Name"].ToString();

                            //All_radioButton.Checked = true;
                            // Find the "all" RadioButton by its Name
                            RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                            allRadioButton.Checked = true; 
                            Image_bind(MicroProject_ID, user_type_ids);

                            mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                            mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                        }
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
                    
                    Program.buildConnection();
                    var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                                + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                                + " from person_microproject PMP left outer join person P1 on P1.P_ID = PMP.Person_ID "
                                + " left outer join microproject MP on PMP.MicroProject_ID = MP.MP_ID"
                                + " WHERE CONCAT(TRIM(P1.P_FirstName),' ', TRIM(P1.P_LastName),' ابن/ة ',TRIM(P1.P_FatherName)) LIKE '%" +
                                Person_Name_textBox.Text + "%'";

                    MySS.sc = new MySqlCommand(query, Program.MyConn);
                    MySS.sc.ExecuteNonQuery();
                    MySS.da = new MySqlDataAdapter(MySS.sc);
                    MySS.dt = new DataTable();
                    MySS.da.Fill(MySS.dt);
                    Program.MyConn.Close();
                    if (MySS.dt != null)
                        if (MySS.dt.Rows.Count > 0)
                        {
                            MicroProject_ID = int.Parse(MySS.dt.Rows[0]["MicroProject_ID"].ToString());
                            MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                            Person_Name_textBox.Text = MySS.dt.Rows[0]["Beneficiary Name"].ToString();

                            //All_radioButton.Checked = true;
                            // Find the "all" RadioButton by its Name
                            RadioButton allRadioButton = this.ImageTypes_panel.Controls["All_radioButton"] as RadioButton;
                            allRadioButton.Checked = true; 
                            Image_bind(MicroProject_ID, user_type_ids);

                            mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                            mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region hover

        private void FitToScreen_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                FitToScreen_button.BackgroundImage = Resources.FitToScreen_L;
            else FitToScreen_button.BackgroundImage = Resources.FitToScreen_D;
        }
         
        private void FitToScreen_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                FitToScreen_button.BackgroundImage = Resources.FitToScreen_D;
            else FitToScreen_button.BackgroundImage = Resources.FitToScreen_L;
        }

        private void Download_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Download_button.BackgroundImage = Resources.Download_L;
            else Download_button.BackgroundImage = Resources.Download_D;
        }

        private void Download_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) 
            {
                System.Windows.Forms.Button btnSender = (System.Windows.Forms.Button)sender;
                System.Drawing.Point ptLowerLeft = new System.Drawing.Point(0, btnSender.Height);
                ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
                download_contextMenuStrip1.Show(ptLowerLeft);
            }
            //else
            //{
            //    ExportToExcel_button_Click(sender, e);
            //}
        }

        private void Download_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Download_button.BackgroundImage = Resources.Download_D;
            else Download_button.BackgroundImage = Resources.Download_L;
        }

        private void AddImage_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                AddImage_button.BackgroundImage = Resources.Plus_Sq_L;
            else AddImage_button.BackgroundImage = Resources.Plus_Sq_D;
        }

        private void AddImage_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                AddImage_button.BackgroundImage = Resources.Plus_Sq_D;
            else AddImage_button.BackgroundImage = Resources.Plus_Sq_L;
        }

        private void DeleteImage_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                DeleteImage_button.BackgroundImage = Resources.Trash_L;
            else DeleteImage_button.BackgroundImage = Resources.Trash_D;
        }

        private void DeleteImage_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                DeleteImage_button.BackgroundImage = Resources.Trash_D;
            else DeleteImage_button.BackgroundImage = Resources.Trash_L;
        }

        #endregion
    }
}