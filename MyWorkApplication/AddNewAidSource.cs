using ClosedXML.Excel;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class AddNewAidSource : Form
    {
        public AddNewAidSource()
        {
            InitializeComponent();
        }

        private int ID;
        private Log l;
        private AidSource aid;
        System.Data.DataTable dt;

        private void AddNewAidSource_Load(object sender, EventArgs e)
        {
            try
            {
                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Category_ToNight(this);
                else
                    newTheme.Category_ToLight(this);

                aid = new AidSource();
                l = new Log();
                dt = new System.Data.DataTable();

                bind("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void bind(string Name)
        {
            if (dt.Rows.Count == 0)
            {
                dt = aid.Select("");
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = dt;
            }
            else
            {
                DataRow[] rows = null;
                string condition = "";
                if (Name != "")
                {
                    condition += "Name='" + Name + "'";
                }

                rows = dt.Select(condition);
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();

                System.Data.DataTable selected_rows_dt = new System.Data.DataTable();

                selected_rows_dt = dt.Copy();
                selected_rows_dt.Rows.Clear();
                foreach (DataRow row in rows)
                    selected_rows_dt.ImportRow(row);

                dataGridView.DataSource = selected_rows_dt;
            }

            var dgC2 = dataGridView.Columns["ID"]; dgC2.Visible = false;

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
                if (Name_textBox.Text == "" || Name_en_textBox.Text == "")
                    throw new InvalidOperationException("لا يمكن ترك بعض الخلايا فارغة!");

                string name = replaceQuotation(Name_textBox.Text);
                string name_en = replaceQuotation(Name_en_textBox.Text);

                aid.Insert(name, name_en);
                l.Insert_Log("Insert " + name + ":" + name_en, "AidSource", Program.UserName, DateTime.Now);

                Name_textBox.Clear();
                Name_en_textBox.Clear();
                ID = -1;

                dt.Rows.Clear();
                bind("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string replaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        private void Name_textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bind(Name_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            Close();
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
                        string name = replaceQuotation(dataGridView.Rows[r].Cells["Name"].Value.ToString());
                        string name_en = replaceQuotation(dataGridView.Rows[r].Cells["Name_en"].Value.ToString());

                        aid.Update(ID, name, name_en);
                        l.Insert_Log("Update AidSource:" + name, "AidSource", Program.UserName, DateTime.Now);
                    }
                    if (e.ColumnIndex == 2) // name_en
                    {
                        string name = replaceQuotation(dataGridView.Rows[r].Cells["Name"].Value.ToString());
                        string name_en = replaceQuotation(dataGridView.Rows[r].Cells["Name_en"].Value.ToString());

                        aid.Update(ID, name, name_en);
                        l.Insert_Log("Update AidSource:" + name + ":" + name_en, "AidSource", Program.UserName, DateTime.Now);
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
                    var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف هذه الجهة؟", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var ID = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                        string name = replaceQuotation(dataGridView.Rows[e.RowIndex].Cells["Name"].Value.ToString());
                        string name_en = replaceQuotation(dataGridView.Rows[e.RowIndex].Cells["Name_en"].Value.ToString());

                        aid.Delete(ID);

                        l.Insert_Log("Delete AidSource: " + name + ":" + name_en, "AidSource", Program.UserName, DateTime.Now);
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

                    ////Remove invisible columns//
                    //for (var j = dataGridView.ColumnCount - 1; j >= 0; j--)
                    //    if (dataGridView.Columns[j].Visible == false)
                    //        ex_dt.Columns.RemoveAt(j);
                    //Remove invisible columns//
                    for (var j = 0; j <= dataGridView.ColumnCount - 1; j++)
                    {
                        if (dataGridView.Columns[j].Visible == false)
                            ex_dt.Columns.Remove(dataGridView.Columns[j].Name.ToString());
                    }

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
