using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class AddNewPriest : Form
    {
        private Log l;

        private MySqlComponents MySS;
        private int Priest_ID;
        private DataRow SelectedDataRow;
        private readonly string username;

        public AddNewPriest()
        {
            InitializeComponent();
        }

        public AddNewPriest(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void AddNewPriest_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);

                SelectedDataRow = null;
                if (SelectedDataRow != null) Delete_button.Enabled = true;
                else Delete_button.Enabled = false;

                MySS = new MySqlComponents();
                l = new Log();
                Priest_bind("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Priest_bind(string Name)
        {
            
            Program.buildConnection();

            MySS.query = "select `Priest_ID` as 'ID',`Priest_Name` as 'Name' from `priest`";

            var condition = "";
            if (Name != "")
                condition += " where `Priest_Name` like '%" + Name + "%'";
            MySS.query += condition;

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Priest_dataGridView.DataSource = MySS.dt;
            var dgC2 = Priest_dataGridView.Columns["ID"];
            dgC2.Visible = false;

            Program.MyConn.Close();
        }

        private void InsertPriest_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (PriestName_textBox.Text == "") throw new NoNullAllowedException();
                insertPriest();
                l.Insert_Log("Insert " + PriestName_textBox.Text, " Priest ", username, DateTime.Now);
                PriestName_textBox.Clear();
                Priest_bind(PriestName_textBox.Text);
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("You can't leave empty fields");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeletePriest_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (PriestName_textBox.Text == "") throw new NoNullAllowedException();
                var dialogResult =
                    MessageBox.Show("Are you sure you want to delete ?", "Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    deletePriest(Priest_ID);
                    l.Insert_Log("Delete " + PriestName_textBox.Text, " Priest ", username, DateTime.Now);

                    PriestName_textBox.Clear();
                    SelectedDataRow = null;
                    Priest_bind(PriestName_textBox.Text);
                }
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Please select the field you want to delete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 

        private void Priest_dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                SelectedDataRow = ((DataRowView) Priest_dataGridView.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    Priest_ID = int.Parse(SelectedDataRow["ID"].ToString());
                    PriestName_textBox.Text = SelectedDataRow["Name"].ToString();

                    Delete_button.Enabled = true;
                }
                else
                {
                    Delete_button.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Priest_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedDataRow = ((DataRowView) Priest_dataGridView.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    Priest_ID = int.Parse(SelectedDataRow["ID"].ToString());
                    PriestName_textBox.Text = SelectedDataRow["Name"].ToString();

                    Delete_button.Enabled = true;
                }
                else
                {
                    Delete_button.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PriestName_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (PriestName_textBox.Text != "")
                    Save_button.Enabled = true;
                else
                    Save_button.Enabled = false;

                Priest_bind(PriestName_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        #region insert update delete Priest

        private void insertPriest()
        {
            
            Program.buildConnection();

            MySS.query = "Insert Into `priest`(`Priest_Name`) values(N'" + PriestName_textBox.Text + "' )";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            Program.MyConn.Close();
        }

        private void updatePriest(int PriestID)
        {
            
            Program.buildConnection();

            MySS.query = "Update `priest` set "
                         + "`Priest_Name` = N'" + PriestName_textBox.Text + "'"
                         + "where `Priest_ID` =" + PriestID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            Program.MyConn.Close();
        }

        private void deletePriest(int PriestID)
        {
            
            Program.buildConnection();

            var delPriestQuery = "delete From `priest` where `Priest_ID` =" + PriestID;
            MySS.sc = new MySqlCommand(delPriestQuery, Program.MyConn);
            MySS.sc.ExecuteNonQuery();

            Program.MyConn.Close();
        }

        #endregion insert update delete Priest

        #region mouse hover

        private void delete_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Delete_button.BackgroundImage = Resources.Delete_CL;
            else Delete_button.BackgroundImage = Resources.Delete_CD;
        }

        private void delete_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Delete_button.BackgroundImage = Resources.Delete_CD;
            else Delete_button.BackgroundImage = Resources.Delete_CL;
        }

        private void Add_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Save_button.BackgroundImage = Resources.Save_CL;
            else Save_button.BackgroundImage = Resources.Save_CD;
        }

        private void Add_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Save_button.BackgroundImage = Resources.Save_CD;
            else Save_button.BackgroundImage = Resources.Save_CL;
        }

        private void Close_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Close_button.BackgroundImage = Resources.Exit_L;
            else Close_button.BackgroundImage = Resources.Exit_D;
        }

        private void Close_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Light")
                Close_button.BackgroundImage = Resources.Exit_D;
            else Close_button.BackgroundImage = Resources.Exit_L;
        }

        #endregion mouse hover
    }
}