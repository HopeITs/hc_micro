using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class WebSection : Form
    {
        public WebSection()
        {
            InitializeComponent();
        }

        public WebSection(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        MySqlComponents MySS;
        private DataRow SelectedDataRow;
        private int MicroProjectEnglish_ID = -1;
        private int MicroProject_ID = -1;
        private int MPI_ID;
        private Log l;
        private string username;
        private int type;
        AllWebProjects AllWebProjects;

        private void fill_boxes(DataRow dr)
        {
            if (dr != null)
            {
                MicroProjectEnglish_ID = Convert.ToInt32(dr["MicroProjectEnglishID"].ToString());
                MicroProject_ID = Convert.ToInt32(dr["Project Number"].ToString());
                MPE_MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                
                MPI_MicroProjectID_textBox.Text = MicroProject_ID.ToString();

                MPE_BeneficiaryName_textBox.Text = dr["Beneficiary name"].ToString();
                MPE_PartnerName_textBox.Text = (string)dr["Partner name"];
                MPE_Country_textBox.Text = (string)dr["Country"];
                MPE_City_textBox.Text = (string)dr["City"];
                MPE_DateOfRequest_dateTimePicker.Value = (DateTime)dr["Date of application"];
                MPE_Name_textBox.Text = (string)dr["Project name"];
                int amount_requested = 0;
                amount_requested = Convert.ToInt32(dr["Amount requested"].ToString());
                MPE_AllMoneyNeeded_textBox.Text = amount_requested.ToString();
                MPE_PeriodOfExecution_textBox.Text = (string)dr["Duration for execution"];
                MPE_Suffering_textBox.Text = (string)dr["Suffering and Need"];
                MPE_Description_textBox.Text = (string)dr["Project description"];
                MPE_VideoOut_textBox.Text = (string)dr["Video Out"];
                MPE_VideoIn_textBox.Text = (string)dr["Video In"];
                int amount_donated = 0;
                amount_donated = Convert.ToInt32(dr["Amount received"].ToString());
                MPE_MoneyDonated_textBox.Text = amount_donated.ToString();

              //  MPE_VideoIn_textBox.Text = dr["Video In"].ToString();

                string funded = (string)dr["Project funded"];
                if (funded.Equals("1"))
                    MPE_Done_checkBox.Checked = true;
                else
                    MPE_Done_checkBox.Checked = false;
                MPE_SuccessStory_textBox1.Text = (string)dr["Success Story"];

                MPE_Image_textBox.Text = (string)dr["Image Path"];
                MPE_Location_textBox.Text = (string)dr["Project Location"];
            }
        }

        #region insert - update - delete
        private void insertMicroProjectEnglish(string story)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            int zero = 0;
            MySS.query = "Insert Into `microprojectenglish`(`MicroProject_ID`,`MPE_Image`, `MPE_Country`, `MPE_SuccessStory`, `MPE_City`, `MPE_DateOfRequest`, `MPE_Name`, `MPE_AllMoneyNeeded`, `MPE_PeriodOfExecution`, `MPE_Description`, `MPE_VideoOut`, `MPE_BeneficiaryName`, `MPE_PartnerName`, `MPE_Suffering`, `MPE_Done`, `MPE_MoneyDonated`, `MPE_VideoIn`,  `MPE_Location`) values("
                + Convert.ToInt32(MPE_MicroProject_ID_textBox.Text) + ",'"
                + null + "',N'"
                + MPE_Country_textBox.Text + "',N'"
                + (story == "" ? "empty" : replaceQuotation(story)) + "',N'"
                + MPE_City_textBox.Text + "',N'"
                                //+ MPE_DateOfRequest_dateTimePicker.Value.Month.ToString() + "/"
                                //+ MPE_DateOfRequest_dateTimePicker.Value.Day.ToString() + "/"
                                //+ MPE_DateOfRequest_dateTimePicker.Value.Year.ToString() + "',N'"

                + MPE_DateOfRequest_dateTimePicker.Value.Year.ToString() + "/"
                + MPE_DateOfRequest_dateTimePicker.Value.Month.ToString() + "/"
                + MPE_DateOfRequest_dateTimePicker.Value.Day.ToString() + "',N'"

                + replaceQuotation(MPE_Name_textBox.Text) + "',"
                + (MPE_AllMoneyNeeded_textBox.Text == "" ? zero : Convert.ToInt32(MPE_AllMoneyNeeded_textBox.Text)) + ",N'"
                + MPE_PeriodOfExecution_textBox.Text + "',N'"
                + replaceQuotation(MPE_Description_textBox.Text) + "',N'"
                + (MPE_VideoOut_textBox.Text == "" ? "0" : MPE_VideoOut_textBox.Text) + "',N'"  //Video out link
                + MPE_BeneficiaryName_textBox.Text + "',N'"
                + (MPE_PartnerName_textBox.Text == "" ? "0" : MPE_PartnerName_textBox.Text) + "',N'"

                + replaceQuotation(MPE_Suffering_textBox.Text) + "',N'"
                + (MPE_Done_checkBox.Checked ? "1" : "0") + "',"
                + (MPE_MoneyDonated_textBox.Text == "" ? zero : Convert.ToInt32(MPE_MoneyDonated_textBox.Text)) + ",N'"
                + (MPE_VideoIn_textBox.Text == "" ? "0" : MPE_VideoIn_textBox.Text) + "','"    //Video in link
                + null + "')";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }
        private void Update_MPE(int MPE_ID, string story)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            MySS.query = "update `microprojectenglish` set " +
                " MicroProject_ID = " + Convert.ToInt32(MPE_MicroProject_ID_textBox.Text) +
                ",MPE_Country = N'" + MPE_Country_textBox.Text + "'" +
                ",MPE_SuccessStory = N'" + (story == "" ? "empty" : replaceQuotation(story)) + "'" +
                ",MPE_City = N'" + MPE_City_textBox.Text + "'" +
                ",MPE_DateOfRequest = N'" + MPE_DateOfRequest_dateTimePicker.Value.Year.ToString() + "/" + MPE_DateOfRequest_dateTimePicker.Value.Month.ToString() + "/"
                                + MPE_DateOfRequest_dateTimePicker.Value.Day.ToString() + "'" +
                ",MPE_Name = N'" + replaceQuotation(MPE_Name_textBox.Text) + "'" +
                ",MPE_AllMoneyNeeded= N'" + Convert.ToInt32(MPE_AllMoneyNeeded_textBox.Text) + "'" +
                ",MPE_PeriodOfExecution= N'" + MPE_PeriodOfExecution_textBox.Text + "'" +
                ",MPE_Description = N'" + replaceQuotation(MPE_Description_textBox.Text) + "'" +
            //    ",MPE_VideoOut = N'" + (MPE_VideoOut_textBox.Text == "" ? "0" : MPE_VideoOut_textBox.Text) + "'" +
                ",MPE_Suffering = N'" + replaceQuotation(MPE_Suffering_textBox.Text) + "'" +
                ",MPE_Done = N'" + (MPE_Done_checkBox.Checked ? "1" : "0") + "'" +
                ",MPE_MoneyDonated = N'" + Convert.ToInt32(MPE_MoneyDonated_textBox.Text) + "'" +
            //    ",MPE_VideoIn =N'" + (MPE_VideoIn_textBox.Text == "" ? "0" : MPE_VideoIn_textBox.Text) + "'" +
                ",MPE_BeneficiaryName = N'" + MPE_BeneficiaryName_textBox.Text + "'" +
                ",MPE_PartnerName = N'" + (MPE_PartnerName_textBox.Text == "" ? "0" : MPE_PartnerName_textBox.Text) + "'" +

                "  where MPE_ID = " + MPE_ID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }

        private void Update_MPE(int MPE_ID, string imagePath, string ProjectLocation, string VideoInLink, string VideoOutLink)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            MySS.query = "update `microprojectenglish` set " +
                " MicroProject_ID = " + Convert.ToInt32(MPI_MicroProjectID_textBox.Text) +

                " ,MPE_Image = N'" + imagePath + "'" +
                " ,MPE_Location = N'" + ProjectLocation + "'" +
                ",MPE_VideoIn = N'" + (VideoInLink == "" ? "0" : VideoInLink) + "'" +
                ",MPE_VideoOut = N'" + (VideoOutLink == "" ? "0" : VideoOutLink) + "'" +
                "  where MPE_ID = " + MPE_ID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }

        private void Insert_MPI()
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            string query = "insert into `microprojectimages`(`MPI_Path`, `MPI_Number`, `MPI_IsFirst`, `MicroProject_ID`) values ("
                            + "'" + MPI_Path_textBox.Text + "'"
                            + "," + Int32.Parse(MPI_Number_textBox.Text) + " "
                            + "," + (MPI_IsFirst_checkBox.Checked ? 1 : 0)  + " "
                            + "," + Int32.Parse(MPI_MicroProjectID_textBox.Text) + ") ";
            MySS.sc = new MySqlCommand(query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }
        private void Update_MPI(int MPI_ID)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            string updMP = " update `microprojectimages` set "
                            + "`MPI_Path` = N'" + MPI_Path_textBox.Text + "'"
                            + ",MPI_Number = " + Int32.Parse(MPI_Number_textBox.Text) + " "
                            + ",MPI_IsFirst = " + (MPI_IsFirst_checkBox.Checked ? 1 : 0) + " "
                            + ",MicroProject_ID = " + Int32.Parse(MPI_MicroProjectID_textBox.Text) + " "
                            + " where MPI_ID = " + MPI_ID + " ";
            MySS.sc = new MySqlCommand(updMP, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }
        private void Delete_MPI(int MPI_ID)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            string query = "delete from `microprojectimages` where MPI_ID = " + MPI_ID + " ";
            MySS.sc = new MySqlCommand(query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
        }
        #endregion

        public bool IsNumeric(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        #region btn-clicks
        private void Add_MPE_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (MPE_Country_textBox.Text == "" || MPE_BeneficiaryName_textBox.Text == "" || MPE_City_textBox.Text == "" || MPE_Name_textBox.Text == "" ||
                        MPE_AllMoneyNeeded_textBox.Text == "" || MPE_PeriodOfExecution_textBox.Text == "" || MPE_Description_textBox.Text == "" ||
                        MPE_MicroProject_ID_textBox.Text == "" || MPE_Suffering_textBox.Text == "")
                {
                    throw new Exception("There are empty values. Please Complete all information");
                }
                else if (!IsNumeric(MPE_AllMoneyNeeded_textBox.Text))
                {
                    MPE_AllMoneyNeeded_textBox.Select();
                    throw new Exception("Please enter only digits values for Amount requested");
                }
                else if (!IsNumeric(MPE_MoneyDonated_textBox.Text))
                {
                    MPE_MoneyDonated_textBox.Select();
                    throw new Exception("Please enter only digits values for Amount received");
                }
                else
                {
                    insertMicroProjectEnglish(replaceQuotation(MPE_SuccessStory_textBox1.Text));

                    l.Insert_Log("insert the project " + replaceQuotation(MPE_Name_textBox.Text), "Micro Project English", username, DateTime.Now);

                    clear_boxes();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("There are empty values. Please Complete all information"))
                    MessageBox.Show("There are empty values. Please Complete all information");
                else MessageBox.Show(ex.Message);
            }
        }
        private void Update_MPE_button_Click(object sender, EventArgs e)
        {
            try
            {
                //if (SelectedDataRow == null || MicroProjectEnglish_ID == -1)
                //    throw new Exception("Please choose the project you want to update");
                Update_MPE(MicroProjectEnglish_ID, replaceQuotation(MPE_SuccessStory_textBox1.Text));

                l.Insert_Log("Update the project " + replaceQuotation(MPE_Name_textBox.Text), "Micro Project English", username, DateTime.Now);

                clear_boxes();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                //insert image
                Insert_MPI();
                //select
                bind_images(MPI_MicroProjectID_textBox.Text);
                //save to log
                l.Insert_Log("insert the image: " + MPI_Number_textBox.Text + " to project :" + MPI_MicroProjectID_textBox.Text, "Micro Project Images", username, DateTime.Now);
                //clear
                MPI_Path_textBox.Text = MPI_Number_textBox.Text = "";
                MPI_IsFirst_checkBox.Checked = false;

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void UpdateImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedDataRow == null || MicroProjectEnglish_ID == -1)
                    throw new Exception("Please choose the Project you want to update");
                //update image
                Update_MPI(MPI_ID);

                //select
                bind_images(MPI_MicroProjectID_textBox.Text);
                //save to log
                l.Insert_Log("update the image: " + MPI_Number_textBox.Text + " of the project :" + MPI_MicroProjectID_textBox.Text, "Micro Project Images", username, DateTime.Now);
                //clear
                MPI_Path_textBox.Text = MPI_MicroProjectID_textBox.Text = MPI_Number_textBox.Text = "";
                MPI_IsFirst_checkBox.Checked = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void DeleteImage_button_Click(object sender, EventArgs e)
        {
            try
            {
                //update image
                Delete_MPI(MPI_ID);
                //select
                bind_images(MPI_MicroProjectID_textBox.Text);
                //save to log
                l.Insert_Log("delete the image: " + MPI_Number_textBox.Text + " from the project :" + MPI_MicroProjectID_textBox.Text, "Micro Project Images", username, DateTime.Now);
                //clear
                MPI_Path_textBox.Text = MPI_MicroProjectID_textBox.Text = MPI_Number_textBox.Text = "";
                MPI_IsFirst_checkBox.Checked = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void save_button_Click(object sender, EventArgs e)
        {
            try
            {
                //update project
                Update_MPE(MicroProjectEnglish_ID, MPE_Image_textBox.Text, MPE_Location_textBox.Text, MPE_VideoIn_textBox.Text, MPE_VideoOut_textBox.Text);
                l.Insert_Log("update the project : " + MPI_MicroProjectID_textBox.Text, "Micro Project English", username, DateTime.Now);
                MPE_Location_textBox.Clear();
                MPE_Image_textBox.Clear();
                MPE_VideoIn_textBox.Clear();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void AllWebBack_button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region mouse hover

        private void AddSave_button_MouseEnter(object sender, EventArgs e)
        {
            Add_MPE_button.BackgroundImage = Save_button.BackgroundImage = Properties.Resources.save;
        }
        private void AddSave_button_MouseLeave(object sender, EventArgs e)
        {
            Add_MPE_button.BackgroundImage = Save_button.BackgroundImage = Properties.Resources.save0;
        }
        private void UpdateSave_button_MouseEnter(object sender, EventArgs e)
        {
            Update_MPE_button.BackgroundImage = Properties.Resources.save;
        }
        private void UpdateSave_button_MouseLeave(object sender, EventArgs e)
        {
            Update_MPE_button.BackgroundImage = Properties.Resources.save0;
        }

        private void Add_button_MouseEnter(object sender, EventArgs e)
        {
            AddImage_button.BackgroundImage = Properties.Resources.add;
        }
        private void Add_button_MouseLeave(object sender, EventArgs e)
        {
            AddImage_button.BackgroundImage = Properties.Resources.add0;
        }
        private void Update_button_MouseEnter(object sender, EventArgs e)
        {
            UpdateImage_button.BackgroundImage = Properties.Resources.update;
        }
        private void Update_button_MouseLeave(object sender, EventArgs e)
        {
            UpdateImage_button.BackgroundImage = Properties.Resources.update0;
        }
        private void Delete_button_MouseEnter(object sender, EventArgs e)
        {
            DeleteImage_button.BackgroundImage = Properties.Resources.delete;
        }
        private void Delete_button_MouseLeave(object sender, EventArgs e)
        {
            DeleteImage_button.BackgroundImage = Properties.Resources.delete0;
        }

        private void Images_button_MouseEnter(object sender, EventArgs e)
        {
            ProjectImages_button.BackColor = Color.FromArgb(40, 40, 40);
        }
        private void Images_button_MouseLeave(object sender, EventArgs e)
        {
            ProjectImages_button.BackColor = Color.Transparent;
        }
        private void MainInf_button_MouseEnter(object sender, EventArgs e)
        {
            MainInf_button.BackColor = Color.FromArgb(40, 40, 40);
        }
        private void MainInf_button_MouseLeave(object sender, EventArgs e)
        {
            MainInf_button.BackColor = Color.Transparent;
        }
        #endregion mouse hover
        private void MicroProject_formForWeb_Load(object sender, EventArgs e)
        {
            MySS = new MySqlComponents();
            l = new Log();
            clear_boxes();
            Add_MPE_button.Visible = true;
            Update_MPE_button.Visible = false;
        }

        private void bind_images(string MicroProject_ID)
        {
            //check connection//
            if (Program.MyConn.State != ConnectionState.Open)
            {
                Program.buildConnection();
            }
            MySS.query = "select MPI_ID as 'Image ID'"
                            + ",MPI_Path as 'Image Path'"
                            + ",MPI_Number as 'Image Number'"

                            + ",CASE MPI_IsFirst WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' End as 'IsItFirstImage'"
                            //                            + ",'IsItFirstImage' = CASE WHEN MPI_IsFirst = 0 THEN 'No' WHEN MPI_IsFirst = 1 THEN 'Yes' END"
                            + ",MicroProject_ID as 'Project ID'"
                            + "from `microprojectimages` ";


            string condition = "\n";
            if (MicroProject_ID != "")
            {
                condition = " where MicroProject_ID =" + Int32.Parse(MicroProject_ID) + " ";
            }
            MySS.query += condition;

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            MPI_DataGridView.DataSource = MySS.dt;
            DataGridViewColumn dgC2 = MPI_DataGridView.Columns["Image ID"];
            dgC2.Visible = false;
        }

        private void clear_boxes()
        {
            MPE_BeneficiaryName_textBox.Text = MPE_Name_textBox.Text =
            MPE_AllMoneyNeeded_textBox.Text = MPE_PeriodOfExecution_textBox.Text = MPE_Description_textBox.Text =
            MPE_VideoOut_textBox.Text = MPE_VideoIn_textBox.Text = MPE_MicroProject_ID_textBox.Text = MPE_Suffering_textBox.Text = "";
            MPE_PartnerName_textBox.Text = MPE_MoneyDonated_textBox.Text = "";
            MPE_SuccessStory_textBox1.Text = "";
            MPE_DateOfRequest_dateTimePicker.Value = DateTime.Now;
            MPE_Done_checkBox.Checked = false;
            MPE_Image_textBox.Text = MPI_Path_textBox.Text = MPI_MicroProjectID_textBox.Text = MPI_Number_textBox.Text
                = MPE_Location_textBox.Text = "";
            MPI_IsFirst_checkBox.Checked = false;
            MPE_Country_textBox.Text = "Syria";
            MPE_City_textBox.Text = "Aleppo";
        }

        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        private void MainInformation_button_Click(object sender, EventArgs e)
        {
            Web_TabControl.SelectedIndex = 0;
            ProjectImages_button.BackColor = Color.Transparent;
            MainInf_button.BackColor = Color.FromArgb(40, 40, 40); 
        }
        private void ProjectImages_button_Click(object sender, EventArgs e)
        {
            Web_TabControl.SelectedIndex = 1;
            bind_images(MPI_MicroProjectID_textBox.Text);
            ProjectImages_button.BackColor = Color.FromArgb(40, 40, 40);
            MainInf_button.BackColor = Color.Transparent;
        }

        private void MPI_IsFirst_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (MPI_IsFirst_checkBox.Checked)
            {
                MPE_Image_textBox.Enabled = true;
                MPE_Image_textBox.Text = MPI_Path_textBox.Text;
            }
            else
            {
                MPE_Image_textBox.Enabled = false;
                MPE_Image_textBox.Text = "";
            }

        }
        private void MPI_DataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SelectedDataRow = ((DataRowView)MPI_DataGridView.CurrentRow.DataBoundItem).Row;
            if (SelectedDataRow != null)
            {
                MPI_ID = Convert.ToInt32(SelectedDataRow["Image ID"].ToString());
                MPI_Path_textBox.Text = SelectedDataRow["Image Path"].ToString();
                int MPI_Number = Convert.ToInt32(SelectedDataRow["Image Number"].ToString());
                MPI_Number_textBox.Text = MPI_Number.ToString();
                string IsItFirst = SelectedDataRow["IsItFirstImage"].ToString();
                if (IsItFirst == "No")
                { MPI_IsFirst_checkBox.Checked = false; }
                else
                { MPI_IsFirst_checkBox.Checked = true; }
                MicroProject_ID = Convert.ToInt32(SelectedDataRow["Project ID"].ToString());
                MPI_MicroProjectID_textBox.Text = MicroProject_ID.ToString();
            }
        }
        private void AllWeb_button_Click(object sender, EventArgs e)
        {
            using (AllWebProjects AllWebProjects = new AllWebProjects(username))
            {
               if (AllWebProjects.ShowDialog() == DialogResult.OK)
               {
                   fill_boxes(AllWebProjects.SelectedDataRow);
                   Add_MPE_button.Visible = false;
                   Update_MPE_button.Visible = true;
               }
               else
               {
                   clear_boxes();
                   Add_MPE_button.Visible = true;
                   Update_MPE_button.Visible = false;
               }
            }
        }


    }
}