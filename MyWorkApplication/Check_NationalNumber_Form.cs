using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Check_NationalNumber_Form : Form
    {
        public Check_NationalNumber_Form()
        {
            InitializeComponent();
        }

        private Log l;
        private NewTheme newTheme;

        private void Check_NationalNumber_Form_Load(object sender, EventArgs e)
        {
            try
            {
                newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.MessageBox_ToNight(this);
                else
                    newTheme.MessageBox_ToLight(this);

                l = new Log();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private DataSet Check_National_Number(string PersonNationalNum)
        {
            //online
            Properties.Settings.Default.Online = false;
            Program.set_ConnectionString();
            Properties.Settings.Default.Save();

            var PersonNationalNum_without_zero = "";
            if (PersonNationalNum.ElementAt(0).ToString() == "0")
                PersonNationalNum_without_zero = PersonNationalNum.Remove(0, 1);
            else
                PersonNationalNum_without_zero = PersonNationalNum;

            //select databases
            string q1 = "SELECT schema_name FROM information_schema.schemata " +
                " WHERE schema_name LIKE 'hcsyria_micro%' " +
                " or schema_name LIKE 'hcsyria_f2f_ajudy' " +
                " or schema_name LIKE 'hcsyria_disc%'; ";
            Program.buildConnection();
            var sc = new MySqlCommand(q1, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();

            //build queries from all available databases
            string q2 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string db = dt.Rows[i].Field<string>(0);
                if (db.Contains("hcsyria_f2f_ajudy"))
                {
                    q2 += " select count(*)" +
                        " , (select schema_name FROM information_schema.schemata where schema_name LIKE '" + db + "' ) as 'database' " +
                        " , CONCAT(first_Name, ' ', father_Name, ' ', last_Name) as 'name' " +

                        " , (select (case " + db + ".`family_partner`.IsActive when 1 then 'فعال' else 'غير فعال' end) from " + db + ".`family_partner` " +
                        " left join " + db + ".`person_family` on " + db + ".`family_partner`.family_id = " + db + ".`person_family`.family_id " +
                        " where " + db + ".`person_family`.person_id = " + db + ".`person`.id " +
                        " order by " + db + ".`family_partner`.id desc limit 1 ) as 'state' " +

                        " from " + db + ".`person` " +
                        " where national_Number like '" + PersonNationalNum_without_zero + "' or national_Number like '" + PersonNationalNum + "';";
                }
                else if (db.Contains("hcsyria_disc"))
                {
                    q2 += "select count(*)" +
                        " , (select schema_name FROM information_schema.schemata where schema_name LIKE '" + db + "' ) as 'database' " +
                        " , CONCAT(FirstName, ' ', FatherName, ' ', LastName) as 'name' " +

                        // مستفيد بشو ? مقبول - مرفوض

                        " from " + db + ".`student` " +
                        " where NationalNumber like '" + PersonNationalNum_without_zero + "' or NationalNumber like '" + PersonNationalNum + "';";
                }
                else
                {
                    q2 += "select count(*) " +
                        " , (select schema_name FROM information_schema.schemata where schema_name LIKE '" + db + "' ) as 'database' " +
                        " , CONCAT(P_FirstName, ' ', P_FatherName, ' ', P_LastName) as 'name' " +

                        " , (select " + db + ".`state`.Name_ar from " + db + ".`microproject` " +
                        " left join " + db + ".`person_microproject` on " + db + ".`person_microproject`.MicroProject_ID = " + db + ".`microproject`.MP_ID " +
                        " left join " + db + ".`microproject_state` on " + db + ".`microproject_state`.MicroProject_ID = " + db + ".`microproject`.MP_ID " +
                        " left join " + db + ".`state` on " + db + ".`state`.ID = " + db + ".`microproject_state`.State_ID " +
                        " where " + db + ".`person_microproject`.Person_ID = " + db + ".`person`.P_ID " +
                        " and Current_State = 1 limit 1 ) as 'state' " +

                        " from " + db + ".`person` " +
                        " where P_NationalNumber like '" + PersonNationalNum_without_zero + "' or P_NationalNumber like '" + PersonNationalNum + "';";
                }
            }

            Program.buildConnection();
            sc = new MySqlCommand(q2, Program.MyConn);
            sc.ExecuteNonQuery();
            da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            //offline
            Properties.Settings.Default.Online = false;
            Program.set_ConnectionString();
            Properties.Settings.Default.Save();

            return ds;
        }

        private void Check_Button_Click(object sender, EventArgs e)
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                if (string.IsNullOrWhiteSpace(NationalNumber_textBox.Text))
                {
                    Result_label.Text = "";
                    Result_button.BackgroundImage = null;
                }
                else
                {
                    DataSet ds = Check_National_Number(NationalNumber_textBox.Text.Trim());
                    string msg = "";
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].Rows[0].Field<object>(0).ToString() == "0") continue;
                        else
                        {
                            string db1 = ds.Tables[i].Rows[0].Field<string>(1);
                            string project = "";
                            if (db1.Equals("hcsyria_f2f_ajudy")) project = "F2F - Aleppo";
                            else if (db1.Equals("hcsyria_micro")) project = "Micro Project - Aleppo";
                            else if (db1.Equals("hcsyria_micro_h")) project = "Micro Project - Homs";
                            else if (db1.Equals("hcsyria_micro_d")) project = "Micro Project - Damascus";
                            else if (db1.Equals("hcsyria_micro_l")) project = "Micro Project - Latakia";
                            else if (db1.Equals("hcsyria_micro_w")) project = "Micro Project - AlWadi";
                            else if (db1.Equals("hcsyria_micro_id")) project = "Micro Project - Idleb";
                            else if (db1.Equals("hcsyria_micro_lb")) project = "Micro Project - Lebanon";
                            else if (db1.Equals("hcsyria_micro_pts_aleppo")) project = "Micro Project - PTS Aleppo ";
                            else if (db1.Equals("hcsyria_disc_h")) project = "DISC - Homs";
                            else if (db1.Equals("hcsyria_disc_d")) project = "DISC - Damascus";
                            else if (db1.Equals("hcsyria_disc_a")) project = "DISC - Aleppo";

                            msg += "المستفيد/ة: "
                           + ds.Tables[i].Rows[0].Field<string>(2) + Environment.NewLine
                           + "المشروع: " + project + Environment.NewLine
                           + "الحالة: " + ds.Tables[i].Rows[0].Field<string>(3) + Environment.NewLine;
                        }
                    }

                    if (msg == "")
                    {
                        Result_label.Text = "لا يوجد معلومات عن الرقم الوطني المُدخَل.";
                        Result_button.BackgroundImage = Properties.Resources.Unchecked;
                    }
                    else
                    {
                        Result_label.Text = msg;
                        Result_button.BackgroundImage = Properties.Resources.Cheked;
                    }

                    l.Insert_Log("Check National Number: " + NationalNumber_textBox.Text.Trim(), "Check National Number"
                        , Program.UserName, DateTime.Now);
                }

                // Set cursor as defaul
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unable to connect to the server") || ex.Message.Contains("MySQL"))
                    MessageBox.Show("Unable to connect to the server please try again or check your connection",
                        "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Check_Button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                Check_Button.BackgroundImage = Properties.Resources.BtnBackground_D;
            else Check_Button.BackgroundImage = Properties.Resources.BtnBackground_L;
        }

        private void Check_Button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                Check_Button.BackgroundImage = Properties.Resources.BtnBackground_L;
            else Check_Button.BackgroundImage = Properties.Resources.BtnBackground_D;
        }

        private void NationalNumber_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Check_Button_Click(sender, e);
            }
        }
    }
}
