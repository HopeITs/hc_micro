using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class AddNewChecklistMember : Form
    {
        public AddNewChecklistMember(string Table, string Type)
        {
            InitializeComponent();
            this.Type = Type;
            this.Type_lable.Text = Table;
        }

        string Type;
        private int ID;
        private Log l;
        CheckList cl;
        DataTable checklist_member_dt, positions_dt;

        private void AddNewChecklistMember_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);

                checklist_member_dt = new System.Data.DataTable();

                l = new Log();
                cl = new CheckList();

                bind(Type);
                bind_Positions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bind_Positions()
        {
            positions_dt = cl.Select_Positions("", Type);

            Position_comboBox.DataSource = null;
            Position_comboBox.DataSource = positions_dt;
            Position_comboBox.DisplayMember = "Name";
            Position_comboBox.ValueMember = "ID";
            Position_comboBox.SelectedIndex = -1;
        }

        public void bind(string Type)
        {
            checklist_member_dt = cl.Select_Members("", Type);

            dataGridView.Rows.Clear();
            for (int i = 0; i < checklist_member_dt.Rows.Count; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells["Member_ID"].Value   = checklist_member_dt.Rows[i].Field<int>("ID");
                dataGridView.Rows[i].Cells["Member_Name"].Value = checklist_member_dt.Rows[i].Field<string>("Name");
                dataGridView.Rows[i].Cells["Position_ID"].Value = checklist_member_dt.Rows[i].Field<int>("Position_ID");

                string P_Name, P_Type;
                P_Name = checklist_member_dt.Rows[i].Field<string>("Position");
                P_Type = checklist_member_dt.Rows[i].Field<string>("Type");

                dataGridView.Rows[i].Cells["Position_Name"] = Load_Position_ComboBox(P_Name, P_Type);
                dataGridView.Rows[i].Cells["Position_Name"].Value = P_Name;
                dataGridView.Rows[i].Cells["Position_Type"].Value = P_Type;

                dataGridView.Rows[i].Cells["IsActive"].Value = checklist_member_dt.Rows[i].Field<int>("IsActive");
            }
            Count_label.Text = "All:" + dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);
        }

        private DataGridViewComboBoxCell Load_Position_ComboBox(string Name, string Type)
        {
            var dt = new DataTable();
            dt = cl.Select_Positions(Name, Type);

            var cell = new DataGridViewComboBoxCell();

            var row = new ArrayList();
            //add items to array list from datatable
            foreach (DataRow dr in dt.Rows) row.Add(dr[1].ToString());
            cell.Items.AddRange(row.ToArray());
            cell.MaxDropDownItems = 8;
            cell.AutoComplete = true;
            cell.FlatStyle = FlatStyle.Flat;
            cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            return cell;
        }

        #region grid events
        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل تريد حفظ التعديلات التي قمت بها؟", "Confirm before save",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var r = e.RowIndex;
                    ID = Convert.ToInt32(dataGridView.Rows[r].Cells["Member_ID"].Value);

                    string Name, Position, IsActive; 
                    int Position_ID = -1;

                    if (e.ColumnIndex == 1
                        || (e.ColumnIndex == 3)
                        || (e.ColumnIndex == 5)) // member name // position  // isActive
                    {
                        if (dataGridView.Rows[r].Cells["Member_Name"].Value == null || dataGridView.Rows[r].Cells["Member_Name"].Value.ToString() == "")
                            throw new Exception("لا يمكن ترك الخلية فارغة، يرجى التأكد وإعادة المحاولة");

                        Name = dataGridView.Rows[r].Cells["Member_Name"].Value.ToString();

                        if (dataGridView.Rows[r].Cells["Position_Name"].Value == null || dataGridView.Rows[r].Cells["Position_Name"].Value.ToString() == "")
                            throw new Exception("لا يمكن ترك الخلية فارغة، يرجى التأكد وإعادة المحاولة");

                        Position = dataGridView.Rows[r].Cells["Position_Name"].Value.ToString();
                        DataRow[] rows = positions_dt.Select("Name = '" + Position + "'");
                        Position_ID = Convert.ToInt32(rows[0].Field<int>("ID"));

                        if (dataGridView.Rows[r].Cells["IsActive"].Value == null || dataGridView.Rows[r].Cells["IsActive"].Value.ToString() == "False")
                            IsActive = "0";
                        else IsActive = "1";
                        
                        cl.Update_Member(ID, Name, Position_ID, IsActive);
                        l.Insert_Log("Update " + Name + ":" + Position + ":"
                            + (IsActive=="1"?"Active":"Not Active") , "Checklist", Program.UserName, DateTime.Now);

                        dataGridView.EndEdit();
                    }
                }
                else
                {
                    dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change
            }
        }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Check if click is on specific column 
                if (e.ColumnIndex == dataGridView.Columns["DeleteRow"].Index)
                {
                    var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف هذه المجموعة؟", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var ID = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["Member_ID"].Value.ToString());
                        cl.Delete_Member(ID);
                        l.Insert_Log("Delete Checklist Member: " + dataGridView.Rows[e.RowIndex].Cells["Member_Name"].Value
                            + ":" + dataGridView.Rows[e.RowIndex].Cells["Position_Name"].Value,
                            "Checklist", Program.UserName, DateTime.Now);
                        dataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot delete or update a parent row"))
                    MessageBox.Show("لا يمكنك حذف هذا الخيار ! بسبب وجود معلومات متعلقة به..");
                else
                    MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == dataGridView.Columns["DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;

                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2,
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
        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3) //type updated and enter to name
                {
                    dataGridView.Rows[e.RowIndex].Cells[3] = Load_Position_ComboBox("", Type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_textBox.Text == "" || Position_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("الرجاء التأكد من أنّ جميع الخلايا تم تعبئتها قبل عملية الحفظ");

                cl.Insert_Member(Name_textBox.Text, Convert.ToInt32(Position_comboBox.SelectedValue));
                l.Insert_Log("Insert " + Name_textBox.Text + ":" + Position_comboBox.Text, "Checklist Member", Program.UserName, DateTime.Now);

                Name_textBox.Clear();
                Position_comboBox.SelectedIndex = -1;

                bind(Type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddPosition_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AddNewChecklistPosition(Type))
                {
                    form.ShowDialog();
                    {
                        bind_Positions();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
