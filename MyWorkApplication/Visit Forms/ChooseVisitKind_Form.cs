using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MyWorkApplication.Visit_Forms
{
    public partial class ChooseVisitKind_Form : Form
    {
        private DataTable dt_old, dt_new;
        private M_and_E m_And_E;
        private readonly MainForm mainForm;
        private int MicroProject_ID, Person_ID, SubCategory_ID, V_ID;
        private string type, fundtype;
        private int partners;
        private Select s;
        private Visit v;

        public ChooseVisitKind_Form(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            MicroProject_ID = -1;
        }

        public ChooseVisitKind_Form(int MicroProject_ID, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
        }

        private void ChooseVisitKind_Form_Load(object sender, EventArgs e)
        {
            try
            {
                v = new Visit();
                m_And_E = new M_and_E();
                s = new Select();
                //FundType_label.Text = "";
                FundType_textBox.Text = "";

                NewTheme NewTheme = new NewTheme();
                if (Settings.Default.theme == "Light") NewTheme.ChooseVisit_ToLight(this);
                else NewTheme.ChooseVisit_ToNight(this);

                //Person_Name_textBox1.AutoCompleteCustomSource = s.select_beneficiaries("", "Yes");
                MicroProject_ID_textBox1.AutoCompleteCustomSource = s.select_project_IDs("", "Yes");

                if (MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox1.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox1_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MicroProject_ID_textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox1.Text != "")
                {
                    fill_beneficiaries_comboBox(MicroProject_ID_textBox1.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void fill_beneficiaries_comboBox(string MP_ID)
        {
            DataTable b_dt = new DataTable();
            b_dt = s.select_Beneficiaries_of_project(MP_ID);

            Beneficiary_comboBox.DisplayMember = "Name";
            Beneficiary_comboBox.ValueMember = "ID";
            Beneficiary_comboBox.DataSource = b_dt;
            Beneficiary_comboBox.SelectedIndex = 0;
        }
        private void Beneficiary_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Beneficiary_comboBox.SelectedIndex != -1)
                {
                    dt_old = v.GetVisitsOfBeneficiary(MicroProject_ID_textBox1.Text, Beneficiary_comboBox.Text);
                    dt_new = m_And_E.GetVisitsOfBeneficiary(MicroProject_ID_textBox1.Text, Beneficiary_comboBox.Text);

                    // MicroProject_ID = dt_old.Rows[0].Field<int>("MicroProject_ID");
                    MicroProject_ID = Convert.ToInt32(MicroProject_ID_textBox1.Text);
                    partners = s.HasPartner(MicroProject_ID);
                    if (partners > 1) partners = 1;
                    else partners = 0;

                    color_buttons(dt_old, dt_new);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void color_buttons(DataTable dt_old1, DataTable dt_new1)
        {
            dataGridView_M.Visible = dataGridView_O.Visible = dataGridView_C.Visible = false;
            dataGridView_M.DataSource = dataGridView_O.DataSource = dataGridView_C.DataSource = null;
            dataGridView_M.Columns.Clear(); dataGridView_O.Columns.Clear(); dataGridView_C.Columns.Clear();
            Kind_button.Visible = false;

            if (dt_old1 != null)
                if (dt_old1.Rows.Count > 0)
                {
                    MicroProject_ID = int.Parse(dt_old1.Rows[0]["MicroProject_ID"].ToString());
                    Person_ID = int.Parse(dt_old1.Rows[0]["Person_ID"].ToString());

                    if (dt_old1.Rows[0]["SubCategory_ID"] == null || dt_old1.Rows[0]["SubCategory_ID"] == DBNull.Value)
                        SubCategory_ID = -1;
                    else SubCategory_ID = int.Parse(dt_old1.Rows[0]["SubCategory_ID"].ToString());

                    type = fundtype = "";
                    type = dt_old1.Rows[0]["Type"].ToString();
                    fundtype = dt_old1.Rows[0]["FundType"].ToString();
                    FundType_textBox.Text = fundtype + " - " + type;

                    // Put background image of project kind
                    Kind_button.Visible = true;
                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                        Kind_button.BackgroundImage = Properties.Resources.rehab3;
                    else if (SubCategory_ID == 1 || SubCategory_ID == 2)
                        Kind_button.BackgroundImage = Properties.Resources.taxi;
                    else if (type.Contains("زراع"))
                        Kind_button.BackgroundImage = Properties.Resources.agriculture;
                    else
                        Kind_button.BackgroundImage = Properties.Resources.warehouse;

                    VOpening_button.Enabled = VMonitoring_button.Enabled = VClosing_button.Enabled = true;
                    VOpening_button.BackColor = VMonitoring_button.BackColor = VClosing_button.BackColor = Color.FromArgb(189, 189, 189);
                    VOpening_button.ForeColor = VMonitoring_button.ForeColor = VClosing_button.ForeColor = Color.FromArgb(96, 96, 96);

                    for (var i = 0; i < dt_old1.Rows.Count; i++)
                        if (dt_old1.Rows[i]["Kind"] == DBNull.Value)
                        {

                        }
                        else
                        {
                            if (dt_old1.Rows[i]["Kind"].ToString().Contains("op"))
                            {
                                // Can't Add new visit //
                                VOpening_button.Enabled = false;
                                VOpening_button.BackColor = Color.FromArgb(96, 96, 96);
                                VOpening_button.ForeColor = Color.White;

                                DataTable selected_rows_dt = dt_old1.Copy();
                                selected_rows_dt.Rows.Clear();
                                selected_rows_dt.ImportRow(dt_old1.Rows[i]);

                                dataGridView_O.DataSource = selected_rows_dt;
                                dataGridView_O.Columns["MicroProject_ID"].Visible
                                    = dataGridView_O.Columns["Beneficiary Name"].Visible
                                    = dataGridView_O.Columns["Person_ID"].Visible
                                    = dataGridView_O.Columns["SubCategory_ID"].Visible
                                    = dataGridView_O.Columns["Kind"].Visible
                                    = dataGridView_O.Columns["Type"].Visible
                                    = dataGridView_O.Columns["FundType"].Visible
                                    = dataGridView_O.Columns["V_ID"].Visible = false;

                                dataGridView_O.Visible = true;
                            }
                            if (dt_old1.Rows[i]["Kind"].ToString().Contains("cl"))
                            {
                                // Can't Add new visit //
                                VClosing_button.Enabled = false;
                                VClosing_button.BackColor = Color.FromArgb(96, 96, 96);
                                VClosing_button.ForeColor = Color.White;

                                DataTable selected_rows_dt = dt_old1.Copy();
                                selected_rows_dt.Rows.Clear();
                                selected_rows_dt.ImportRow(dt_old1.Rows[i]);

                                dataGridView_C.DataSource = selected_rows_dt;
                                dataGridView_C.Columns["MicroProject_ID"].Visible
                                    = dataGridView_C.Columns["Beneficiary Name"].Visible
                                    = dataGridView_C.Columns["Person_ID"].Visible
                                    = dataGridView_C.Columns["SubCategory_ID"].Visible
                                    = dataGridView_C.Columns["Kind"].Visible
                                    = dataGridView_C.Columns["Type"].Visible
                                    = dataGridView_C.Columns["FundType"].Visible
                                    = dataGridView_C.Columns["V_ID"].Visible = false;

                                dataGridView_C.Visible = true;
                            }
                        }
                }
                else
                { }

            if (dt_new1 != null && dt_new1.Rows.Count != 0)
            {
                if (dt_new1.Rows[0]["Kind"] == DBNull.Value)
                {
                    // No thing // 
                }
                else
                {
                    //Fill Regular Visits//
                    dataGridView_M.DataSource = dt_new1;
                    dataGridView_M.Columns["MicroProject_ID"].Visible
                        = dataGridView_M.Columns["Beneficiary Name"].Visible
                        = dataGridView_M.Columns["Person_ID"].Visible
                        = dataGridView_M.Columns["SubCategory_ID"].Visible
                        = dataGridView_M.Columns["Type"].Visible
                        = dataGridView_M.Columns["Kind"].Visible
                        = dataGridView_M.Columns["V_ID"].Visible = false;
                    dataGridView_M.Visible = true;

                    //hide monitoring because they are linked to beneficiary not project 
                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                        dataGridView_M.Visible = false;
                }
            }

            dataGridView_M.ClearSelection();
            dataGridView_O.ClearSelection();
            dataGridView_C.ClearSelection();
        }

        private void VOpening_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox1.Text != "" && Beneficiary_comboBox.Text != "")
                {
                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                    {
                        MessageBox.Show("لا يوجد امكانية لاضافة زيارة افتتاحية لمشاريع الترتيم");
                    }
                    else if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form Taxi_Visit1_Form = new VOpening_Taxi_Form(mainForm, MicroProject_ID);
                        mainForm.showNewTab(Taxi_Visit1_Form, "Opening visit", 0);
                    }
                    else
                    {
                        Form Other_Visit1_Form = new VOpening_Other_Form(mainForm, MicroProject_ID);
                        mainForm.showNewTab(Other_Visit1_Form, "Opening visit", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VMonitoring_button_Click(object sender, EventArgs e)
        {
            try
            {
                string visit_num = "";
                if (MicroProject_ID_textBox1.Text != "" && Beneficiary_comboBox.Text != "")
                {
                    // if no regular visits yet //
                    if (dataGridView_M.Rows.Count == 0 || dataGridView_M.Visible == false)
                        visit_num = "1";
                    else
                    {
                        int rowsCount = dataGridView_M.Rows.Count;
                        int MaxNumber = 0;
                        for (var i = 0; i < rowsCount; i++)
                        {
                            int value = Convert.ToInt32(dataGridView_M.Rows[i].Cells["Number"].Value.ToString());
                            if (value > MaxNumber)
                                MaxNumber = value;
                        }
                        visit_num = (MaxNumber + 1).ToString();
                    }
                    ////////////////////////////////

                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                    {
                        MessageBox.Show("لا يوجد امكانية لاضافة زيارة مراقبة وتقييم لمشاريع الترتيم");
                    }
                    else if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form Visit_V_Form = new V_ME_Taxi_Form(mainForm, visit_num, Beneficiary_comboBox.Text
                            , Convert.ToInt32(MicroProject_ID_textBox1.Text), partners);
                        mainForm.showNewTab(Visit_V_Form, "Vehicles Monitoring visit", 0);
                    }
                    else
                    {
                        Form Visit_O_Form = new V_ME_Other_Form(mainForm, visit_num, Beneficiary_comboBox.Text
                            , Convert.ToInt32(MicroProject_ID_textBox1.Text), partners);
                        mainForm.showNewTab(Visit_O_Form, "Other Projects Monitoring visit", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VClosing_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID_textBox1.Text != "" && Beneficiary_comboBox.Text != "")
                {
                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                    {
                        //MessageBox.Show("نعتذر عن تحميل الصفحة، لأننا لم ننته من برمجتها بعد"
                        //    + Environment.NewLine
                        //    + "الرجاء انتظار النسخة القادمة");
                        Form rehab_visit_Form = new VClosing_Rehab_Form(mainForm, MicroProject_ID);
                        mainForm.showNewTab(rehab_visit_Form, "Rehabilitation Closing visit", 0);
                    }
                    else if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form Taxi_Visit4_Form = new VClosing_Form(mainForm, MicroProject_ID, "V-cl");
                        mainForm.showNewTab(Taxi_Visit4_Form, "Vehicles Closing visit", 0);
                    }
                    else
                    {
                        Form Other_Visit4_Form = new VClosing_Form(mainForm, MicroProject_ID, "O-cl");
                        mainForm.showNewTab(Other_Visit4_Form, "Other Projects Closing visit", 0);
                    }
                }
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataRow SelectedDataRow = ((DataRowView)dataGridView_O.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    int V_ID = -1;
                    if (SelectedDataRow["V_ID"] == null ||
                        SelectedDataRow["V_ID"] == DBNull.Value)
                        V_ID = -1;
                    else V_ID = Convert.ToInt32(SelectedDataRow["V_ID"].ToString());

                    if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form Visit1_Form = new VOpening_Taxi_Form(V_ID, mainForm);
                        mainForm.showNewTab(Visit1_Form, "Vehicles Opening Visit", 0);
                    }
                    else
                    {
                        Form Visit1_Form = new VOpening_Other_Form(V_ID, mainForm);
                        mainForm.showNewTab(Visit1_Form, "Other Projects Opening Visit", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataRow SelectedDataRow = ((DataRowView)dataGridView_C.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    int V_ID = -1;
                    if (SelectedDataRow["V_ID"] == null ||
                        SelectedDataRow["V_ID"] == DBNull.Value)
                        V_ID = -1;
                    else V_ID = Convert.ToInt32(SelectedDataRow["V_ID"].ToString());

                    if (type.Contains("ترميم") && fundtype.Contains("منحة"))
                    {
                        Form rehab_visit_Form = new VClosing_Rehab_Form(V_ID, mainForm);
                        mainForm.showNewTab(rehab_visit_Form, "Rehabilitation Closing visit", 0);
                    }
                    else if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form VisitClosing_Form = new VClosing_Form(V_ID, mainForm, "V-cl");
                        mainForm.showNewTab(VisitClosing_Form, "Vehicles Closing Visit", 0);
                    }
                    else
                    {
                        Form VisitClosing_Form = new VClosing_Form(V_ID, mainForm, "O-cl");
                        mainForm.showNewTab(VisitClosing_Form, "Other Closing Visit", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataRow SelectedDataRow = ((DataRowView)dataGridView_M.CurrentRow.DataBoundItem).Row;
                if (SelectedDataRow != null)
                {
                    int V_ID = -1;
                    if (SelectedDataRow["V_ID"] == null ||
                        SelectedDataRow["V_ID"] == DBNull.Value)
                        V_ID = -1;
                    else V_ID = Convert.ToInt32(SelectedDataRow["V_ID"].ToString());

                    string V_NUMBER = SelectedDataRow["Number"].ToString();

                    if (SubCategory_ID == 1 || SubCategory_ID == 2)
                    {
                        Form Visit_V_Form = new V_ME_Taxi_Form(V_ID, mainForm);
                        mainForm.showNewTab(Visit_V_Form, "Visit:" + V_NUMBER, 0);
                    }
                    else
                    {
                        Form Visit_O_Form = new V_ME_Other_Form(V_ID, mainForm);
                        mainForm.showNewTab(Visit_O_Form, "Visit:" + V_NUMBER, 0);
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