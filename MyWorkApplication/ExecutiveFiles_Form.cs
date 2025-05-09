using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class ExecutiveFiles_Form : Form
    {
        private DateTime ImpoundDate, BeginDate, CurrentDate, ExpiredDate;
        private Log l;
        private readonly MainForm mainForm;
        private int MicroProject_ID, ExeFile_ID;

        private MySqlComponents MySS;
        private NewTheme NewTheme;
        private Select s;
        private bool update_mode;
        private readonly List<int> User_IDs = new List<int>();
        private UserNotification userNotification;

        public ExecutiveFiles_Form(MainForm mainForm)
        {
            InitializeComponent();
            update_mode = false;
            this.mainForm = mainForm;
        }

        public ExecutiveFiles_Form(int ExeFile_ID, MainForm mainForm)
        {
            InitializeComponent();
            update_mode = true;
            this.mainForm = mainForm;
            this.ExeFile_ID = ExeFile_ID;
        }

        private void insertExeFile(int MicroProject_ID, DateTime BeginDate1, DateTime ImpoundDate1,
            DateTime CurrentDate1)
        {
            
            Program.buildConnection();

            var query =
                "Insert Into `exefile`(`ExeF_No`, `ExeF_BeginDate`, `ExeF_CurrentDate`, `ExeF_ImpoundDate`, `ExeF_ImpoundType`,`ExeF_NumOfMonths`, `MicroProject_ID`) values(N'"
                + ExeF_No_textBox.Text + "',N'"
                + BeginDate1.Year + "/"
                + BeginDate1.Month + "/"
                + BeginDate1.Day + "',N'"
                + CurrentDate1.Year + "/"
                + CurrentDate1.Month + "/"
                + CurrentDate1.Day + "',N'"
                + ImpoundDate1.Year + "/"
                + ImpoundDate1.Month + "/"
                + ImpoundDate1.Day + "',N'"
                + ExeF_ImpoundType_textBox.Text + "',"
                + Convert.ToInt32(Months_numericUpDown.Value.ToString()) + "," +
                +MicroProject_ID + " ) ";
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void updateExeFile(int ExeF_ID, DateTime BeginDate1, DateTime ImpoundDate1, DateTime CurrentDate1)
        {
            
            Program.buildConnection();

            MySS.query = "update `exefile` set " +
                         "ExeF_No = N'" + ExeF_No_textBox.Text + "'," +
                         "ExeF_BeginDate = N'" + BeginDate1.Year + "/"
                         + BeginDate1.Month + "/"
                         + BeginDate1.Day + "'," +
                         "ExeF_CurrentDate = N'" + CurrentDate1.Year + "/"
                         + CurrentDate1.Month + "/"
                         + CurrentDate1.Day + "'," +
                         "ExeF_ImpoundDate = N'" + ImpoundDate1.Year + "/"
                         + ImpoundDate1.Month + "/"
                         + ImpoundDate1.Day + "'," +
                         "ExeF_ImpoundType = N'" + ExeF_ImpoundType_textBox.Text + "'," +
                         "ExeF_NumOfMonths = " + Convert.ToInt32(Months_numericUpDown.Value.ToString()) + "," +
                         "MicroProject_ID = " + MicroProject_ID + " " +
                         "\n where ExeF_ID = " + ExeF_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            Program.MyConn.Close();
        }

        private void deleteExeFile(int ExeF_ID)
        {
            
            Program.buildConnection();
            MySS.query = "delete from `exefile` where ExeF_ID = " + ExeF_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        private void ExecutiveFiles_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.Loan_ToLight(this);
                else
                    NewTheme.Loan_ToNight(this);

                MySS = new MySqlComponents();
                l = new Log();
                s = new Select();
                userNotification = new UserNotification();

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");

                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Executive File";

                var user_db = s.select_Lawful();
                if (user_db != null)
                    for (var i = 0; i < user_db.Rows.Count; i++)
                        User_IDs.Add(user_db.Rows[i].Field<int>(0));

                if (update_mode)
                {
                    fill_ExeFiles_boxes(ExeFile_ID);
                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fill_ExeFiles_boxes(int ExeFile_ID)
        {
            try
            {
                var DRow = s.ExeFile_bind(ExeFile_ID);
                if (DRow != null)
                {
                    ExeFile_ID = int.Parse(DRow.Rows[0]["ID"].ToString());

                    ExeF_No_textBox.Text = DRow.Rows[0]["Number"].ToString();

                    var date1 = (DateTime) DRow.Rows[0]["Begin Date"];
                    ExeF_BeginDate_maskedTextBox.Text = date1.ToString("dd-MM-yyyy");
                    date1 = (DateTime) DRow.Rows[0]["Current Date"];
                    ExeF_CurrentDate_maskedTextBox.Text = date1.ToString("dd-MM-yyyy");

                    Calculate_Set_ExpiredDate();

                    date1 = (DateTime) DRow.Rows[0]["Impound Date"];
                    ExeF_ImpoundDate_maskedTextBox.Text = date1.ToString("dd-MM-yyyy");
                    ExeF_ImpoundType_textBox.Text = DRow.Rows[0]["Impound Type"].ToString();

                    var months = (int) DRow.Rows[0]["Months"];
                    Months_numericUpDown.Value = months;

                    Person_Name_textBox.Text = DRow.Rows[0]["Beneficiary Name"].ToString();
                    MicroProject_ID = (int) DRow.Rows[0]["MicroProject_ID"];
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();

                    update_mode = true;
                }
                else
                {
                    clear_ExeFile_boxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Fill_ExeFile_Boxes(string MP_ID, string P_Name)
        {
            
            Program.buildConnection();
            var query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                        + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                        + " ,`ExeF_ID` as 'ID'"
                        + ",`ExeF_No` as 'Number'"
                        + ",`ExeF_BeginDate` as 'Begin Date'"
                        + ",`ExeF_CurrentDate` as 'Current Date'"
                        + ",`ExeF_ImpoundDate` as 'Impound Date'"
                        + ",`ExeF_ImpoundType` as 'Impound Type'"
                        + ",`ExeF_NumOfMonths` as 'Months'"
                        + " FROM `exefile` E right outer join  person_microproject PMP on E.MicroProject_ID = PMP.MicroProject_ID "
                        + " right outer join person P1 on P1.P_ID = PMP.Person_ID ";

            var condition = " Where lower(IsProjectOwner) like 'yes' ";
            if (MP_ID != "")
                condition += " and PMP.MicroProject_ID = " + MP_ID;
            else if (P_Name != "")
                condition +=
                    " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" +
                    Person_Name_textBox.Text + "%'";

            query += condition;
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();

            clear_ExeFile_boxes();
            if (dt != null && dt.Rows.Count > 0)
            {
                MicroProject_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
                MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                Person_Name_textBox.Text = dt.Rows[0]["Beneficiary Name"].ToString();

                if (dt.Rows[0]["ID"] != DBNull.Value)
                {
                    update_mode = true;
                    ExeFile_ID = int.Parse(dt.Rows[0]["ID"].ToString());

                    ExeF_No_textBox.Text = dt.Rows[0]["Number"].ToString();

                    var beg_date = (DateTime) dt.Rows[0]["Begin Date"];
                    ExeF_BeginDate_maskedTextBox.Text = beg_date.ToString("dd-MM-yyyy");
                    var cur_date = (DateTime) dt.Rows[0]["Current Date"];
                    ExeF_CurrentDate_maskedTextBox.Text = cur_date.ToString("dd-MM-yyyy");

                    if (dt.Rows[0]["Impound Date"] != DBNull.Value)
                    {
                        var imp_date = (DateTime) dt.Rows[0]["Impound Date"];
                        ExeF_ImpoundDate_maskedTextBox.Text = imp_date.ToString("dd-MM-yyyy");
                        ExeF_ImpoundType_textBox.Text = dt.Rows[0]["Impound Type"].ToString();
                    }

                    Months_numericUpDown.Value = Convert.ToDecimal(dt.Rows[0]["Months"]);
                    Calculate_Set_ExpiredDate();
                }

                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
            }
        }

        private void MicroProject_ID_textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox.Text != "") Fill_ExeFile_Boxes(MicroProject_ID_textBox.Text, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Person_Name_textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Person_Name_textBox.Text != "") Fill_ExeFile_Boxes("", Person_Name_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getCurrentExeFileID()
        {
            
            Program.buildConnection();

            MySS.query = "select MAX(`ExeF_ID`) from `exefile` ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            int.TryParse(MySS.sc.ExecuteScalar().ToString(), out ExeFile_ID);
        }

        private DateTime GetDateFromMaskedTextBox(MaskedTextBox textBox)
        {
            var d = textBox.Text.Substring(0, 2);
            var m = textBox.Text.Substring(3, 2);
            var y = textBox.Text.Substring(6, 4);
            var Date = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            return Date;
        }


        private void AddExeFile_button_Click(object sender, EventArgs e)
        {
            try
            {
                BeginDate = GetDateFromMaskedTextBox(ExeF_BeginDate_maskedTextBox);
                ImpoundDate = GetDateFromMaskedTextBox(ExeF_ImpoundDate_maskedTextBox);
                CurrentDate = GetDateFromMaskedTextBox(ExeF_CurrentDate_maskedTextBox);
                ExpiredDate = GetDateFromMaskedTextBox(ExeF_ExpiredDate_maskedTextBox);

                if (update_mode)
                {
                    updateExeFile(ExeFile_ID, BeginDate, ImpoundDate, CurrentDate);
                    l.Insert_Log("Update Executive file of : " + MicroProject_ID + " ", "Executive file",
                        Program.UserName, DateTime.Now);

                    //Send to Lawful//
                    if (User_IDs.Count == 0)
                        MessageBox.Show(
                            "لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");
                    else
                        foreach (var u in User_IDs)
                            // Update Expired Date and User for the same ExeFile_ID //
                            userNotification.Update_NotificationW_ExpiredDateOfExeFile(ExeFile_ID, ExpiredDate.ToString("yyyy/MM/dd"),
                                Program.userID);
                    clear_ExeFile_boxes();
                }
                else
                {
                    if (ExeF_No_textBox.Text == "")
                        throw new InvalidOperationException("You Can't leave the file number empty ! please check and try again.");
                    insertExeFile(MicroProject_ID, BeginDate, ImpoundDate, CurrentDate);
                    l.Insert_Log("insert Executive file to: " + MicroProject_ID + " ", " Executive file",
                        Program.UserName, DateTime.Now);
                    getCurrentExeFileID();

                    //Send to Lawful//
                    if (User_IDs.Count == 0)
                        MessageBox.Show(
                            "لا يوجد مستخدمين لإرسال إشعار الزيارة لهم، يرجى تحديد أدوار المستخدمين والمحاولة لاحقاً");
                    else
                        foreach (var u in User_IDs)
                            userNotification.Insert_UserNotification(ExpiredDate.ToString("yyyy/MM/dd"), "الملف التنفيذي",
                                Person_Name_textBox.Text, MicroProject_ID, u, Program.userID, -1199);

                    clear_ExeFile_boxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteExeFile_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExeF_No_textBox.Text == "")
                    throw new InvalidOperationException("Please choose Executive file that you want to delete.");
                var dialogResult =
                    MessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    deleteExeFile(ExeFile_ID);
                    l.Insert_Log("Delete Executive file of : " + Person_Name_textBox.Text + " ", "Executive file",
                        Program.UserName, DateTime.Now);
                    clear_ExeFile_boxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Calculate_Set_ExpiredDate()
        {
            try
            {
                string d, m, y;
                d = ExeF_CurrentDate_maskedTextBox.Text.Substring(0, 2);
                m = ExeF_CurrentDate_maskedTextBox.Text.Substring(3, 2);
                y = ExeF_CurrentDate_maskedTextBox.Text.Substring(6, 4);
                var date = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));

                var months = Convert.ToInt32(Months_numericUpDown.Value);
                ExeF_ExpiredDate_maskedTextBox.Text = date.AddMonths(months).ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExeF_BeginDate_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!update_mode)
                    ExeF_CurrentDate_maskedTextBox.Text = ExeF_BeginDate_maskedTextBox.Text;

                if (ExeF_CurrentDate_maskedTextBox.Text != null || ExeF_CurrentDate_maskedTextBox.Text != "")
                    Calculate_Set_ExpiredDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExeF_CurrentDate_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExeF_CurrentDate_maskedTextBox.Text != null || ExeF_CurrentDate_maskedTextBox.Text != "")
                    Calculate_Set_ExpiredDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Months_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExeF_CurrentDate_maskedTextBox.Text != null || ExeF_CurrentDate_maskedTextBox.Text != "")
                    Calculate_Set_ExpiredDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clear_ExeFile_boxes()
        {
            ExeF_No_textBox.Text = ExeF_ImpoundType_textBox.Text = "";
            ExeF_BeginDate_maskedTextBox.ResetText();
            ExeF_CurrentDate_maskedTextBox.ResetText();
            ExeF_ImpoundDate_maskedTextBox.ResetText();
            ExeF_ExpiredDate_maskedTextBox.ResetText();
            ExeFile_ID = -1;
            update_mode = false;
        }


        #region hover

        private void AddExeFile_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                AddExeFile_button.BackgroundImage = Resources.Save2_D;
            else AddExeFile_button.BackgroundImage = Resources.Save2_L;
        }

        private void AddExeFile_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                AddExeFile_button.BackgroundImage = Resources.Save2_L;
            else AddExeFile_button.BackgroundImage = Resources.Save2_D;
        }

        private void DeleteExeFile_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                DeleteExeFile_button.BackgroundImage = Resources.Delete2_D;
            else DeleteExeFile_button.BackgroundImage = Resources.Delete2_L;
        }

        private void DeleteExeFile_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                DeleteExeFile_button.BackgroundImage = Resources.Delete2_L;
            else DeleteExeFile_button.BackgroundImage = Resources.Delete2_D;
        }

        #endregion
    }
}