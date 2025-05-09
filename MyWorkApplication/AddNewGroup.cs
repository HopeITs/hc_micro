using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;

namespace MyWorkApplication
{
    public partial class AddNewGroup : Form
    {
        private DonorGroup group;
        private int ID;
        private Log l; 
        DataTable donors_dt;

        public AddNewGroup()
        {
            InitializeComponent();
        }


        private void AddNewGroup_Load(object sender, EventArgs e)
        {
            try
            {
                group = new DonorGroup();
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);

                l = new Log();
                Donor_bind();
                Fill_Groups("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Fill_Groups(string Name)
        {
            var dt = group.Select(Name.ToLower());
            if (dt.Rows.Count != 0)
            {
                group_dataGridView.DataSource = null;
                group_dataGridView.Columns.Clear();
                group_dataGridView.DataSource = dt;

                var dgC2 = group_dataGridView.Columns["ID"];
                var dgC1 = group_dataGridView.Columns["Donor_ID"];
                dgC2.Visible =dgC1.Visible = false;

                var colCB = new DataGridViewButtonColumn();
                colCB.Name = "DeleteRow";
                colCB.HeaderText = "";
                colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colCB.FlatStyle = FlatStyle.Flat;
                group_dataGridView.Columns.Add(colCB);
                group_dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                group_dataGridView.Columns["DeleteRow"].Width = 40;


                for (var i = 0; i < group_dataGridView.RowCount; i++)
                {
                    //الجهة الممولة//
                    var ComboBoxCell2 = new DataGridViewComboBoxCell();
                    //ComboBoxCell2.Items.Add("None");

                    for (int dd = 0; dd< donors_dt.Rows.Count; dd++)
                    { 
                        ComboBoxCell2.Items.Add(donors_dt.Rows[dd].Field<string>(1));
                    }

                    ComboBoxCell2.Value = group_dataGridView.Rows[i].Cells["Donor"].Value;
                    ComboBoxCell2.FlatStyle = FlatStyle.Flat;
                    group_dataGridView["Donor", i] = ComboBoxCell2;
                }

            }
        }
                    
                                    
                   

        private void Name_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Name_textBox.Text != "")
                    Save_button.Enabled = true;
                else
                    Save_button.Enabled = false;

                Fill_Groups(Name_textBox.Text);
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

        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_textBox.Text == "") throw new InvalidOperationException("لا يمكن ترك بعض الخلايا فارغة!");
                if (Donor_comboBox.Text == "" || Donor_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("لا يمكن ترك بعض الخلايا فارغة!");

                double rate;
                if (Rate_textBox.Text == "") rate = 0;
                else double.TryParse(Rate_textBox.Text, out rate);

                int Donor_ID = Convert.ToInt32(Donor_comboBox.SelectedValue.ToString());
                string donor = replaceQuotation(Donor_comboBox.Text);

                group.Insert(Name_textBox.Text, rate, Donor_ID);
                l.Insert_Log("Insert " + Name_textBox.Text + ":" + donor + ":" + rate, " Group ", Program.UserName,
                    DateTime.Now);
                Name_textBox.Clear();
                Rate_textBox.Clear();
                Donor_comboBox.SelectedIndex = -1;

                Fill_Groups("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Donor_bind()
        {
            
            Program.buildConnection();
            var sc = new MySqlCommand("select ID,Name from `donor` ORDER BY Name ASC ", Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            donors_dt = new DataTable();
            da.Fill(donors_dt);
            Program.MyConn.Close();
            Donor_comboBox.DataSource = null;
            Donor_comboBox.DisplayMember = "Name";
            Donor_comboBox.ValueMember = "ID";
            Donor_comboBox.DataSource = donors_dt;
            Donor_comboBox.SelectedIndex = -1;
        }

        private void group_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void group_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل تريد حفظ التعديلات التي قمت بها؟", "Confirm before save",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var r = e.RowIndex;
                    ID = Convert.ToInt32(group_dataGridView.Rows[r].Cells["ID"].Value);

                    if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3) // name - rate - donor
                    {
                        double rate;
                        if (group_dataGridView.Rows[r].Cells["Rate"].Value == null) rate = 0;
                        else double.TryParse(group_dataGridView.Rows[r].Cells["Rate"].Value.ToString(), out rate);
                         
                        string donor = replaceQuotation(group_dataGridView.Rows[r].Cells["Donor"].Value.ToString());
                        DataRow[] rows = null;
                        rows = donors_dt.Select("Name='" + donor+ "'");
                        int Donor_ID = rows[0].Field<int>("ID");
                        //var dValue = from row in donors_dt.AsEnumerable()
                        //             where row.Field<string>("Name") == donor
                        //             select row.Field<int>("ID"); 

                        group.Update(ID, group_dataGridView.Rows[r].Cells["Name"].Value.ToString(), rate, Donor_ID);
                        l.Insert_Log("Update Group:" + group_dataGridView.Rows[r].Cells["Name"].Value + ":" + donor+ ":" + rate,
                            "Group", Program.UserName, DateTime.Now);
                    }

                    group_dataGridView.EndEdit();
                }
                else
                {
                    group_dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void group_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Check if click is on specific column 
                if (e.ColumnIndex == group_dataGridView.Columns["DeleteRow"].Index)
                {
                    var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف هذه المجموعة؟", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var ID = Convert.ToInt32(group_dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                        group.Delete(ID);
                        l.Insert_Log("Delete group: " + group_dataGridView.Rows[e.RowIndex].Cells["Name"].Value,
                            "Group", Program.UserName, DateTime.Now);
                        group_dataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void group_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == group_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == group_dataGridView.Columns["DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;

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

        #region mouse hover

      
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