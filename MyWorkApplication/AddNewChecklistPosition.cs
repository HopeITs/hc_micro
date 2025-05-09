using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class AddNewChecklistPosition : Form
    {
        public AddNewChecklistPosition(string Type)
        {
            InitializeComponent();
            this.Type = Type;
        }

        string Type;
        private int ID;
        private Log l;
        CheckList cl;  
        DataTable checklist_position_dt;

        private void AddNewChecklistPosition_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);
                 
                checklist_position_dt = new System.Data.DataTable();

                l = new Log();
                cl = new CheckList();

                checklist_position_dt = cl.Select_Positions("",Type);

                bind(Type); 
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

        public void bind(string Type)
        {

            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            dataGridView.DataSource = cl.Select_Positions("", Type);

            var dgC2 = dataGridView.Columns["Type"]; dgC2.Visible = false;
            dgC2 = dataGridView.Columns["ID"]; dgC2.Visible = false;

            var colCB = new DataGridViewButtonColumn();
            colCB.Name = "DeleteRow";
            colCB.HeaderText = "";
            colCB.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCB.FlatStyle = FlatStyle.Flat;
            dataGridView.Columns.Add(colCB);
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns["DeleteRow"].Width = 40;

            Count_label.Text = "All:" + dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);
        }

        #region grid events 
        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل تريد حفظ التعديلات التي قمت بها؟", "Confirm before save",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var r = e.RowIndex;
                    ID = Convert.ToInt32(dataGridView.Rows[r].Cells["ID"].Value);

                    if (e.ColumnIndex == 1) // name 
                    {
                        string Name = dataGridView.Rows[r].Cells["Name"].Value.ToString(); 

                        cl.Update_Position(ID, Name);
                        l.Insert_Log("Update Position:" + Type + ":" + Name, "Position", Program.UserName, DateTime.Now);
                    }
                    dataGridView.EndEdit();
                }
                else
                {
                    dataGridView.CancelEdit(); // Set cell value back to what it was prior to user's change 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        var ID = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                        cl.Delete_Position(ID);
                        l.Insert_Log("Delete Position: " + dataGridView.Rows[e.RowIndex].Cells["Name"].Value ,
                            "Position", Program.UserName, DateTime.Now);
                        dataGridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
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

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_textBox.Text == "" )
                    throw new InvalidOperationException("الرجاء التأكد من أنّ جميع الخلايا تم تعبئتها قبل عملية الحفظ");

                cl.Insert_Position(Name_textBox.Text,Type);
                l.Insert_Log("Insert " + Name_textBox.Text, "Position", Program.UserName, DateTime.Now);
 
                Name_textBox.Clear(); 

                bind(Type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
