using MyWorkApplication.Visit_Forms;
using System;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Partners_MessageBox : Form
    {
        private int Category_ID;
        private M_and_E m_And_E;
        private readonly MainForm mainForm;
        private int MicroProject_ID, Person_ID;
        private string Person_Name;
        private readonly string V_Num;
        private DataGridViewRow SelectedDataRow;

        public Partners_MessageBox(MainForm mainForm, int MicroProject_ID, int Person_ID, string V_Num)
        {
            InitializeComponent();
            this.MicroProject_ID = MicroProject_ID;
            this.Person_ID = Person_ID;
            this.V_Num = V_Num;
            this.mainForm = mainForm;
        }

        private void New_button_Click(object sender, EventArgs e)
        {
            try
            {
                ///////////// if row selected /////////////
                if (DataGridView.RowCount > 0 && DataGridView.CurrentRow != null)
                {
                    //SelectedDataRow = ((DataRowView)DataGridView.CurrentRow.DataBoundItem).Row;
                    SelectedDataRow = DataGridView.CurrentRow;
                    if (SelectedDataRow != null)
                    {
                        if (SelectedDataRow.Cells["MP_ID"] == null ||
                            SelectedDataRow.Cells["MP_ID"].Value == DBNull.Value)
                            MicroProject_ID = -1;
                        else MicroProject_ID = int.Parse(SelectedDataRow.Cells["MP_ID"].Value.ToString());

                        if (SelectedDataRow.Cells["P_ID"] == null ||
                            SelectedDataRow.Cells["P_ID"].Value == DBNull.Value)
                            Person_ID = -1;
                        else Person_ID = int.Parse(SelectedDataRow.Cells["P_ID"].Value.ToString());

                        if (SelectedDataRow.Cells["Beneficiary_Name"] == null ||
                            SelectedDataRow.Cells["Beneficiary_Name"].Value == DBNull.Value)
                            Person_Name = "";
                        else Person_Name = SelectedDataRow.Cells["Beneficiary_Name"].Value.ToString();
                    }

                    string V_NAME_TO_SHOW;
                    if (V_Num == "2") V_NAME_TO_SHOW = "Second visit";
                    else if (V_Num == "3*") V_NAME_TO_SHOW = "Third * visit";
                    else if (V_Num == "3") V_NAME_TO_SHOW = "Third visit";
                    else V_NAME_TO_SHOW = "";

                    if (Category_ID == 1 || Category_ID == 2)
                    {
                        Form Visit_V_Form = new V_ME_Taxi_Form(mainForm, V_Num, Person_Name, MicroProject_ID, 1);
                        mainForm.showNewTab(Visit_V_Form, V_NAME_TO_SHOW, 0);
                    }
                    else
                    {
                        Form Visit_O_Form = new V_ME_Other_Form(mainForm, V_Num, Person_Name, MicroProject_ID, 1);
                        mainForm.showNewTab(Visit_O_Form, V_NAME_TO_SHOW, 0);
                    }

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void Back_button_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Partners_MessageBox_Load(object sender, EventArgs e)
        {
            try
            {
                m_And_E = new M_and_E();

                DataGridView.Rows.Clear();
                var grid_index = 0;

                var dt = m_And_E.GetVisitsOfBeneficiary(MicroProject_ID.ToString(), "");
                if (dt != null)
                    for (var i = 0; i < dt.Rows.Count; i++)
                        if (int.Parse(dt.Rows[i]["Person_ID"].ToString()) == Person_ID)
                        {
                        }
                        else
                        {
                            MicroProject_ID = int.Parse(dt.Rows[i]["MicroProject_ID"].ToString());
                            Category_ID = int.Parse(dt.Rows[i]["Category_ID"].ToString());

                            DataGridView.Rows.Add();

                            DataGridView.Rows[grid_index].Cells["MP_ID"].Value = MicroProject_ID;
                            DataGridView.Rows[grid_index].Cells["P_ID"].Value =
                                int.Parse(dt.Rows[i]["Person_ID"].ToString());
                            DataGridView.Rows[grid_index].Cells["Beneficiary_Name"].Value =
                                (string)dt.Rows[i]["Beneficiary Name"];
                            grid_index++;
                        }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}