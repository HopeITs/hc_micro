using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MyWorkApplication.Controls;

namespace MyWorkApplication
{
    public partial class Purchasing_Form: Form
    {
        public Purchasing_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = -1;
            Update_Mode = false;
        }

        public Purchasing_Form(int MicroProject_ID,MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            Update_Mode = true;
        }

        private Log l;
        private readonly MainForm mainForm;
        private int MicroProject_ID, Person_ID;
        private NewTheme NewTheme;
        private UserNotification noti;
        private Select s;
        private Purchasing p;
        private DataTable category_dt;
        private bool Update_Mode;
        private bool user_mode;
        private string ApplicationState = ""; 
         
        private void Purchasing_Form_Load(object sender, EventArgs e)
        {
            try
            {
                NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    NewTheme.ApplicationNew_ToLight(this);
                else
                    NewTheme.ApplicationNew_ToNight(this);
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Purchasing";

                l = new Log();
                s = new Select();
                p = new Purchasing();

                CheckUserPermission();
                Bind_Gridview_ComboBox();
                Add_DateTimeColumn_Gridview();

                Items_dataGridView.DoubleBuffered(true);

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "Yes");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "Yes");

                if (MicroProject_ID != -1)
                {
                    Fill_Purchasing(MicroProject_ID.ToString(),"");
                     
                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Add_DateTimeColumn_Gridview()
        {
            CalendarColumn col = new CalendarColumn();
            col.HeaderText = "تاريخ الفاتورة";
            col.Name = "InvoiceDate";
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //col.FillWeight = 10;
            Items_dataGridView.Columns.Insert(8, col); 
        }

        private void Fill_Purchasing(string microProjectId,string p_name)
        {
            user_mode = false;
            Items_dataGridView.DataSource = null;
            DataTable dt = p.Select_PurchasingItems(microProjectId, p_name);

            //Items_dataGridView.DataSource = dt;
            clear_Boxes(false);

            if (dt == null) return;

            //fill project details
            MicroProject_ID = int.Parse(dt.Rows[0]["MicroProject_ID"].ToString());
            MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
            Person_Name_textBox.Text = (string)dt.Rows[0]["Beneficiary Name"];
            Person_ID = int.Parse(dt.Rows[0]["Person_ID"].ToString());
            MP_Name_textBox.Text = (string)dt.Rows[0]["Project Name"];
            Career_textBox.Text = (string)dt.Rows[0]["Career"]; 
            ApplicationState = (string)dt.Rows[0]["نوع الاستمارة"]; 

            if (dt.Rows[0]["Loan Amount"] == DBNull.Value)
                MessageBox.Show("لم يتم إدخال مبلغ القرض لهذا المشروع", "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                var dd = Convert.ToDouble(dt.Rows[0]["Loan Amount"].ToString());
                LoanAmount_textBox1.Text = dd.ToString();

                if (dt.Rows[0]["Loan Date"] != DBNull.Value)
                {
                    var date = Convert.ToDateTime(dt.Rows[0]["Loan Date"].ToString());
                    LoanDate_dateTimePicker.Value = date;
                }
            }

            //fill items table
            for (var i = 0; i < dt.Rows.Count; i++)
            { 
                // ID
                if (!dt.Rows[i].IsNull("ID"))
                {
                    Items_dataGridView.Rows.Add();
                    Items_dataGridView.Rows[i].Cells["ID"].Value = dt.Rows[i].Field<int>("ID").ToString();

                    // MicroProject_ID
                    if (dt.Rows[i].IsNull("MicroProject_ID"))
                        Items_dataGridView.Rows[i].Cells["MP_ID"].Value = "";
                    else
                        Items_dataGridView.Rows[i].Cells["MP_ID"].Value = dt.Rows[i].Field<int>("MicroProject_ID").ToString();

                    // ItemName
                    Items_dataGridView.Rows[i].Cells["ItemName"].Value =
                        dt.Rows[i].IsNull("ItemName") ? "" : dt.Rows[i].Field<string>("ItemName");

                    // Brand
                    Items_dataGridView.Rows[i].Cells["Brand"].Value =
                        dt.Rows[i].IsNull("Brand") ? "" : dt.Rows[i].Field<string>("Brand");

                    // ItemCategory_ID
                    if (!dt.Rows[i].IsNull("ItemCategory_ID"))
                        Items_dataGridView.Rows[i].Cells["ItemCategory_ID"].Value =
                            dt.Rows[i].Field<int>("ItemCategory_ID");

                    // Quantity
                    if (dt.Rows[i].IsNull("Quantity"))
                        Items_dataGridView.Rows[i].Cells["Quantity"].Value = 0;
                    else
                        Items_dataGridView.Rows[i].Cells["Quantity"].Value =
                            dt.Rows[i].Field<int>("Quantity").ToString();

                    // Price
                    if (dt.Rows[i].IsNull("Price"))
                        Items_dataGridView.Rows[i].Cells["Price"].Value = 0;
                    else
                        Items_dataGridView.Rows[i].Cells["Price"].Value =
                            dt.Rows[i].Field<decimal>("Price").ToString();

                    // Currency
                    Items_dataGridView.Rows[i].Cells["Currency"].Value =
                        dt.Rows[i].IsNull("Currency") ? "" : dt.Rows[i].Field<string>("Currency");

                    // InvoiceDate
                    if (dt.Rows[i].IsNull("InvoiceDate"))
                        Items_dataGridView.Rows[i].Cells["InvoiceDate"].Value = null;
                    else
                        Items_dataGridView.Rows[i].Cells["InvoiceDate"].Value =
                            dt.Rows[i].Field<DateTime>("InvoiceDate").Date;

                    // InvoiceNumber
                    Items_dataGridView.Rows[i].Cells["InvoiceNumber"].Value =
                        dt.Rows[i].IsNull("InvoiceNumber") ? "" : dt.Rows[i].Field<string>("InvoiceNumber");

                    // TraderName
                    Items_dataGridView.Rows[i].Cells["TraderName"].Value =
                        dt.Rows[i].IsNull("TraderName") ? "" : dt.Rows[i].Field<string>("TraderName");

                    // Mark the row as from database
                    Items_dataGridView.Rows[i].Cells["P_InDataBase"].Value = "1";
                } 
            }
            user_mode = true;
        }
        private void Bind_Gridview_ComboBox()
        {
            user_mode = false; 
            category_dt = p.Select_PurchasingCategories();   
            ItemCategory_ID.DataSource = category_dt;
            ItemCategory_ID.DisplayMember = "Name";
            ItemCategory_ID.ValueMember = "ID";  
            user_mode = true;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try {
                foreach (DataGridViewRow row in Items_dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    // قراءة القيم من الجدول
                    string itemName = row.Cells["ItemName"].Value?.ToString();
                    string brand = row.Cells["Brand"].Value?.ToString();
                    int? categoryId = row.Cells["ItemCategory_ID"].Value == null
                                        ? (int?)null
                                        : Convert.ToInt32(row.Cells["ItemCategory_ID"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    string currency = row.Cells["Currency"].Value?.ToString();
                    DateTime? invoiceDate = row.Cells["InvoiceDate"].Value == null
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(row.Cells["InvoiceDate"].Value);
                    string invoiceNumber = row.Cells["InvoiceNumber"].Value?.ToString();
                    string traderName = row.Cells["TraderName"].Value?.ToString();

                    // لو عندك عمود ID في الـ DataGridView
                    if (row.Cells["ID"].Value == null || row.Cells["ID"].Value == DBNull.Value)
                    {
                        // INSERT
                        p.Insert(MicroProject_ID, itemName, brand, categoryId,
                                              quantity, price, currency, invoiceDate,
                                              invoiceNumber, traderName);

                        l.Insert_Log("Insert Purchasing Item:" + itemName + " of: " + MicroProject_ID, "Purchasing",
                                   Program.UserName, DateTime.Now);
                    }
                    else
                    {
                        // UPDATE
                        int id = Convert.ToInt32(row.Cells["ID"].Value);
                        p.Update(id, MicroProject_ID, itemName, brand, categoryId,
                                              quantity, price, currency, invoiceDate,
                                              invoiceNumber, traderName);

                        l.Insert_Log("Update Purchasing Item:" + itemName + " of: " + MicroProject_ID, "Purchasing",
                                   Program.UserName, DateTime.Now);
                    }
                }
                MessageBox.Show("تم حفظ المواد بنجاح", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clear_Boxes(bool withproject)
        {
            if (withproject) {
                MicroProject_ID = -1;
                Person_ID = -1;
                MicroProject_ID_textBox.Text = Person_Name_textBox.Text = "";
            }

            Career_textBox.Text = MP_Name_textBox.Text = LoanAmount_textBox1.Text = ""; 
            LoanDate_dateTimePicker.Value = DateTime.Now;

            Items_dataGridView.Rows.Clear(); 
        }

        private void MicroProject_ID_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox.Text != "")
                {
                    int.TryParse(MicroProject_ID_textBox.Text, out MicroProject_ID);
                    Fill_Purchasing(MicroProject_ID.ToString(),"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Person_Name_textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Person_Name_textBox.Text != "")
                {
                    string name = Person_Name_textBox.Text;
                    Fill_Purchasing("",name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0  
                    break;
                case 2: //Data
                case 7: //Lawful
                    {
                        addNotesToolStripMenuItem.Visible = false;

                    }
                    break;
                case 3: //Financial 
                case 4: //Guest 
                case 9: //Media
                    {
                        application_toolStripMenuItem.Visible
                        = addNotesToolStripMenuItem.Visible = false;
                        Save_button.Visible = false; 

                        throw new InvalidOperationException(" You don't have the permission for this action.");
                    } 
            }
        }

        #region datagrid functions
        private void Items_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == Items_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == Items_dataGridView.Columns["P_DeleteRow"].Index)
                {
                    CheckUserPermission();

                    var dialogResult = MessageBox.Show("Are you sure you want to delete ?", "Delete",
                        MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (Items_dataGridView.Rows[e.RowIndex].Cells["ID"].Value == null
                            || Items_dataGridView.Rows[e.RowIndex].Cells["ID"].Value == DBNull.Value
                            || Items_dataGridView.Rows[e.RowIndex].Cells["ID"].Value == "")
                        {
                            Items_dataGridView.Rows.RemoveAt(e.RowIndex); 
                        }
                        else
                        {
                            var ID = Convert.ToInt32(Items_dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString());

                            p.Delete(ID);

                            l.Insert_Log("Delete Purchasing Item of: " + MicroProject_ID, "Purchasing",
                                Program.UserName, DateTime.Now);

                            Items_dataGridView.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Items_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == Items_dataGridView.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == Items_dataGridView.Columns["P_DeleteRow"].Index)
            {
                Image image = null;
                if (Settings.Default.theme == "Dark")
                    image = Resources.KAKA_Alii_D;
                else
                    image = Resources.KAKA_Alii;
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                var size = image.Size;
                var location = new Point((e.CellBounds.Width - size.Width) / 2,
                    (e.CellBounds.Height - size.Height) / 2);
                location.Offset(e.CellBounds.Location);
                e.Graphics.DrawImage(image, location);
                e.Handled = true;
            }
        }
        private void Items_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void Items_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colIndex = Items_dataGridView.CurrentCell.ColumnIndex;

            if (colIndex == Items_dataGridView.Columns["Price"].Index ||
                colIndex == Items_dataGridView.Columns["Quantity"].Index)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= Numeric_KeyPress;
                    tb.KeyPress += Numeric_KeyPress;
                }
            }
            else
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                    tb.KeyPress -= Numeric_KeyPress;
            }
        }

        private void Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            int colIndex = Items_dataGridView.CurrentCell.ColumnIndex;

            if (colIndex == Items_dataGridView.Columns["Quantity"].Index)
            {
                // العدد: فقط أعداد صحيحة
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
            else if (colIndex == Items_dataGridView.Columns["Price"].Index)
            {
                // السعر: أعداد عشرية
                TextBox tb = sender as TextBox;

                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // منع إدخال أكثر من فاصلة عشرية
                if (e.KeyChar == '.' && tb.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region hover  
        private void Save_button_MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                Save_button.BackgroundImage = Resources.Save2_D;
            else Save_button.BackgroundImage = Resources.Save2_L;
        } 
        private void Save_button_MouseLeave(object sender, EventArgs e)
        {
            if (Settings.Default.theme == "Dark")
                Save_button.BackgroundImage = Resources.Save2_L;
            else Save_button.BackgroundImage = Resources.Save2_D;
        }
        #endregion
    }
}
