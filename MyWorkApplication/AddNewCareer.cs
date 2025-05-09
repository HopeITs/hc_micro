using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Data;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Threading;

namespace MyWorkApplication
{
    public partial class AddNewCareer : Form
    {
        public AddNewCareer()
        {
            InitializeComponent();
        }

        private int ID;
        private Log l; 
        private Career career;
        private SubCategory sub;
        System.Data.DataTable SubCategory_dt,Career_dt;

        private void AddNewSubCategory_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);
                 
                l = new Log();
                sub = new SubCategory();
                career = new Career();

                SubCategory_dt = new System.Data.DataTable();
                Career_dt = new System.Data.DataTable();
                 
                SubCategory_bind();
                Career_bind("", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SubCategory_bind()
        {
            SubCategory_dt = sub.Select("","");
            SubCategory_comboBox.DisplayMember = "Name";
            SubCategory_comboBox.ValueMember = "ID";
            SubCategory_comboBox.DataSource = SubCategory_dt; 
        }

        public void Career_bind(string SubCategory_ID, string Name)
        {
            if (Career_dt.Rows.Count == 0)
            {
                Career_dt = career.Select("", "");
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = Career_dt; 
            }
            else
            {
                DataRow[] rows = null;
                string condition = "";
                if (Name != "")
                {
                    condition += "Name='" + Name + "'";
                    if (SubCategory_ID != "")
                        condition += " AND SubCategory_ID=" + SubCategory_ID + " ";
                }
                else if (SubCategory_ID != "")
                {
                    condition += "SubCategory_ID=" + SubCategory_ID + " ";
                    if (Name != "")
                        condition += " AND Name='" + Name + "'";
                }

                rows = Career_dt.Select(condition);
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();

                System.Data.DataTable selected_rows_dt =new System.Data.DataTable();
                
                selected_rows_dt = Career_dt.Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                dataGridView.DataSource = selected_rows_dt;
            }

            var dgC2 = dataGridView.Columns["ID"]; dgC2.Visible = false;
            dgC2 = dataGridView.Columns["SubCategory_ID"]; dgC2.Visible = false;
            dataGridView.Columns["SubCategory"].ReadOnly = true;

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

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_textBox.Text == "" || SubCategory_comboBox.SelectedIndex == -1)
                    throw new InvalidOperationException("لا يمكن ترك بعض الخلايا فارغة!");

                career.Insert(Name_textBox.Text, Convert.ToInt32(SubCategory_comboBox.SelectedValue.ToString())); 
                l.Insert_Log("Insert Career:" + Name_textBox.Text, "Career", Program.UserName, DateTime.Now);

                Name_textBox.Clear();
                ID = -1; 

                Career_dt.Rows.Clear();
                Career_bind(SubCategory_comboBox.SelectedValue.ToString(), "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
          
        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubCategory_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubCategory_comboBox.SelectedIndex != -1)
                    Career_bind(SubCategory_comboBox.SelectedValue.ToString(), Name_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Name_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubCategory_comboBox.SelectedIndex != -1)
                    Career_bind(SubCategory_comboBox.SelectedValue.ToString(), Name_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region mouse move
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

        #endregion
         

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

                    if (e.ColumnIndex == 1 || e.ColumnIndex == 2) // name - category_id
                    {
                        int SubCategoryID = Convert.ToInt32(dataGridView.Rows[r].Cells["SubCategory_ID"].Value.ToString());
                        string Name = dataGridView.Rows[r].Cells["Name"].Value.ToString();

                        career.Update(ID, Name, SubCategoryID);
                        l.Insert_Log("Update Career:" + Name + ":" + SubCategoryID,
                            "Career", Program.UserName, DateTime.Now);
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
                        var ID = Convert.ToInt32( dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                        career.Delete(ID);
                        l.Insert_Log("Delete Career: " + dataGridView.Rows[e.RowIndex].Cells["Name"].Value,
                            "Career", Program.UserName, DateTime.Now);
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

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var SelectedRowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                Selected_label.Text = "Selected: " + SelectedRowCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportToExcel_button_Click(object sender, EventArgs e)
        {
            try
            {
                //if Guest or Out of service or lawful//
                if (Program.role == 4 ||
                    Program.role == 6 ||
                    Program.role == 7)
                    throw new InvalidOperationException("Sorry ! You Don't have the permission for this action.");

                Thread myTh = new Thread(SaveCallDialog);
                myTh.SetApartmentState(ApartmentState.STA);
                myTh.Start();
                myTh.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveCallDialog()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save Excel File";
                saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog1.ValidateNames = true;

                var res = saveFileDialog1.ShowDialog();
                if (res == DialogResult.OK && saveFileDialog1.FileName != "")
                {
                    string filename = filename = saveFileDialog1.FileName;
                    XLWorkbook wb = new XLWorkbook { RightToLeft = true };
                    //defaultView gets the visible rows only//
                    System.Data.DataTable ex_dt = ((System.Data.DataTable)dataGridView.DataSource).DefaultView.ToTable();

                    //Remove invisible columns//
                    for (var j = 0; j <= dataGridView.ColumnCount - 1; j++)
                    {
                        if (dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(dataGridView.Columns[j].Name.ToString());
                    }
                    ////Remove invisible columns//
                    //for (var j = dataGridView.ColumnCount - 1; j >= 0; j--)
                    //    if (dataGridView.Columns[j].Visible == false)
                    //        ex_dt.Columns.RemoveAt(j);

                    wb.Worksheets.Add(ex_dt, "Exported from App");

                    wb.SaveAs(filename);
                    wb.Dispose();

                    Process.Start(filename);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
