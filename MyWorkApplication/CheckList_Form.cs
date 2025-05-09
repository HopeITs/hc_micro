using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MyWorkApplication.Visit_Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Document = Microsoft.Office.Interop.Word.Document;
using System.Threading;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics; 

namespace MyWorkApplication
{
    public partial class CheckList_Form : Form
    {
        public CheckList_Form(MainForm mainForm)
        {
            InitializeComponent(); 
            this.mainForm = mainForm;
            this.MicroProject_ID = -1;
            MyDataGridViewInitializationMethod();
        }
        public CheckList_Form(int MicroProject_ID,MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.MicroProject_ID = MicroProject_ID;
            MyDataGridViewInitializationMethod();
        }

        MainForm mainForm;
        CheckList cl;
        MicroProject mp;
        Log l;
        Select s;
        int MicroProject_ID;
        bool Update_Mode, user_mode;
        System.Data.DataTable checklist_member_dt;
        System.Data.DataTable beneficiaries_dt;

        private void CheckList_Form_Load(object sender, EventArgs e)
        {
            try {
                mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                mainForm.TabName_label.Text = "Check List";

                var newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.Application_ToNight(this);
                else
                    newTheme.Application_ToLight(this);

                cl = new CheckList();
                l = new Log();
                s = new Classes.Select();
                mp = new MicroProject();

                user_mode = false;

                Person_Name_textBox.AutoCompleteCustomSource = s.select_beneficiaries("", "");
                MicroProject_ID_textBox.AutoCompleteCustomSource = s.select_project_IDs("", "");
                beneficiaries_dt = s.select_Beneficiaries_of_project("");


                keyperson_dataGridView.AutoGenerateColumns 
                    = team_dataGridView.AutoGenerateColumns 
                    = visit_dataGridView.AutoGenerateColumns
                    = commity_dataGridView.AutoGenerateColumns = false; 
                 
                Bind_Gridview_ComboBox();

                Update_Mode = false;

                if(MicroProject_ID != -1)
                {
                    MicroProject_ID_textBox.Text = MicroProject_ID.ToString();
                    MicroProject_ID_textBox_Leave(sender, e);
                }

                user_mode = true;

                CheckUserPermission();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CheckUserPermission()
        {
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //admin_l0
                    {
                        Save_button.Visible = Delete_button.Visible = Print_button.Visible
                            = Add_C_button.Visible = Add_K_button.Visible
                            = Add_T_button.Visible = Add_V_button.Visible 
                            = true;
                        return true;
                    }
                case 2: //Data
                    {
                        payments_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible
                            = Add_C_button.Visible = Add_K_button.Visible
                            = Add_T_button.Visible = Add_V_button.Visible 
                            = true; 
                        return true;
                    }
                case 3: //Financial 
                case 7: //Lawful
                    {
                        Visits_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible
                            = Add_C_button.Visible = Add_K_button.Visible
                            = Add_T_button.Visible = Add_V_button.Visible 
                            = false;
                        return false;
                    }
                case 4: //Guest
                case 9: //Media
                    {
                        payments_toolStripMenuItem.Visible = 
                            Visits_toolStripMenuItem.Visible = false;

                        Save_button.Visible = Delete_button.Visible = Print_button.Visible
                            = Add_C_button.Visible = Add_K_button.Visible
                            = Add_T_button.Visible = Add_V_button.Visible 
                           = false;
                        return false;
                    }
            }
            return false;
        }

        #region datagridview with comboxBoxe methods
        private void MyDataGridViewInitializationMethod()
        {
            keyperson_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(keyperson_dataGridView_EditingControlShowing);
            visit_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(visit_dataGridView_EditingControlShowing);
            team_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(team_dataGridView_EditingControlShowing);
            commity_dataGridView.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(commity_dataGridView_EditingControlShowing);

        } 

        private void team_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(team_dataGridView_Control_KeyPress);
        }
        private void commity_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(commity_dataGridView_Control_KeyPress);
        }
        private void keyperson_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(keyperson_dataGridView_Control_KeyPress); 
        } 
        private void visit_dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(visit_dataGridView_Control_KeyPress);
        }
         
        private void team_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && team_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void commity_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && commity_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        private void keyperson_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && keyperson_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true; 
            }
        }
        private void visit_dataGridView_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && visit_dataGridView.CurrentCellAddress.X == 2)
            {
                e.Handled = true;
            }
        }
        #endregion

        private void Bind_Gridview_ComboBox()
        {
            user_mode = false;

            checklist_member_dt = cl.Select_Checklist_Members("", "");

            System.Data.DataTable dt_k = checklist_member_dt.Select("Type='k'").CopyToDataTable();

            System.Data.DataTable dt_t = checklist_member_dt.Select("Type='t'").CopyToDataTable();

            System.Data.DataTable dt_c = checklist_member_dt.Select("Type='c'").CopyToDataTable(); 

            K_Name.DataSource = dt_k;
            K_Name.DisplayMember = "Name";
            K_Name.ValueMember = "ID";
              
            T_Name.DataSource = dt_t;
            T_Name.DisplayMember = "Name";
            T_Name.ValueMember = "ID";
              
            V_Name.DataSource = dt_t;
            V_Name.DisplayMember = "Name";
            V_Name.ValueMember = "ID";
             
            C_Name.DataSource = dt_c;
            C_Name.DisplayMember = "Name";
            C_Name.ValueMember = "ID";

            user_mode = true;
        }

        private void MicroProject_ID_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (user_mode)
                    if (MicroProject_ID_textBox.Text != "")
                    {
                        MicroProject_ID = Convert.ToInt32(MicroProject_ID_textBox.Text);
                        Fill_checklist(MicroProject_ID_textBox.Text ,"");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Person_Name_textBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if(user_mode)
                    if (Person_Name_textBox.Text != "")
                    {
                        Fill_checklist("", Person_Name_textBox.Text);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Fill_checklist(string MP_ID, string P_Name)
        {
            user_mode = false;
            System.Data.DataTable dt = cl.Get_Checklist(MP_ID, P_Name);
            if ( dt != null)
                if ( dt.Rows.Count > 0)
                {
                    MicroProject_ID = int.Parse(dt.Rows[0]["MP_ID"].ToString());

                    // if (MicroProject_ID_textBox.Text == "")
                        MicroProject_ID_textBox.Text = MicroProject_ID.ToString(); 
                    // Person_Name_textBox.Text = dt.Rows[0]["Beneficiary Name"].ToString();
                    string p_name = "";
                    DataRow[] rows = null;
                    rows = beneficiaries_dt.Select("MicroProject_ID=" + MicroProject_ID);  
                    for (int bb = 0; bb < rows.Length ; bb++)
                        p_name += rows[bb].Field<string>(1) + "-";
                    if (p_name.Last() == '-') p_name = p_name.Remove(p_name.Length-1,1);
                    Person_Name_textBox.Text = p_name;

                    KeyPersonNotes_textBox.Text = dt.Rows[0]["MP_KeyPersonNotes"].ToString();
                    //ParishNotes_textBox.Text = dt.Rows[0]["MP_ParishNotes"].ToString();
                    VisitNotes_textBox.Text = dt.Rows[0]["MP_VisitNotes"].ToString();
                    VisitTime_textBox.Text = dt.Rows[0]["MP_VisitTime"].ToString();

                    if (dt.Rows[0]["MP_VisitDate"] == null || dt.Rows[0]["MP_VisitDate"] == DBNull.Value)
                        VisitDate_checkBox.Checked = true;
                    else
                    {
                        VisitDate_checkBox.Checked = false;
                        VisitDate_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0]["MP_VisitDate"].ToString());
                    }

                    if (dt.Rows[0]["MP_TeamDate"] == null || dt.Rows[0]["MP_TeamDate"] == DBNull.Value)
                        TeamDate_checkBox.Checked = true;
                    else
                    {
                        TeamDate_checkBox.Checked = false;
                        TeamDate_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0]["MP_TeamDate"].ToString());
                    }

                    if (dt.Rows[0]["MP_CommityDate"] == null || dt.Rows[0]["MP_CommityDate"] == DBNull.Value)
                        CommityDate_checkBox.Checked = true;
                    else
                    {
                        CommityDate_checkBox.Checked = false;
                        CommityDate_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0]["MP_CommityDate"].ToString());
                    }


                    var visited = (int)dt.Rows[0]["MP_Visited"];
                    if (visited == 0) Visit_checkBox.Checked = false;
                    else Visit_checkBox.Checked = true;

                    //set radioButton answers//
                    //set_radioButtons(1, Convert.ToInt32(dt.Rows[0]["ch_HouseTechCondition"]));
                    //set_radioButtons(2, Convert.ToInt32(dt.Rows[0]["ch_ProjectExperience"]));
                    //set_radioButtons(3, Convert.ToInt32(dt.Rows[0]["ch_ExpectedProjectSuccess"]));
                    //set_radioButtons(4, Convert.ToInt32(dt.Rows[0]["ch_WorkAbility"]));
                     
                    keyperson_dataGridView.Rows.Clear();
                    team_dataGridView.Rows.Clear();
                    visit_dataGridView.Rows.Clear();
                    commity_dataGridView.Rows.Clear();

                    int keyperson_index, team_index,visit_index, commity_index;
                    keyperson_index= team_index= visit_index= commity_index = 0;

                    if (dt.Rows[0]["ID"] != DBNull.Value)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var opinion_text = "";
                            int opinion = dt.Rows[i].Field<int>("Opinion");
                            if (opinion == 1) opinion_text = "مع";
                            else if (opinion == 0) opinion_text = "ضد";
                            else if (opinion == -1) opinion_text = "N/A";

                            if (dt.Rows[i]["Location"].ToString() == "k") //keyperson
                            {
                                keyperson_dataGridView.Rows.Add();
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Checklist_ID"].Value = dt.Rows[i].Field<int>("ID");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_ID"].Value = dt.Rows[i].Field<int>("Member_ID");
                                 
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Name"].Value = dt.Rows[i].Field<int>("Member_ID");
                                //keyperson_dataGridView.Rows[keyperson_index].Cells["K_Position"].Value = dt.Rows[i].Field<string>("Position").ToString();

                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_Notes"].Value = dt.Rows[i].Field<string>("Notes");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_NameNotes"].Value = dt.Rows[i].Field<string>("NameNotes");
                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_RoleNotes"].Value = dt.Rows[i].Field<string>("RoleNotes");

                                //keyperson_dataGridView.Rows[keyperson_index].Cells["K_Opinion"].Value = opinion_text;
                                //if (opinion_text == "مع")
                                //{
                                //    keyperson_dataGridView.Rows[keyperson_index].DefaultCellStyle.BackColor = Color.FromArgb(104, 157, 202);
                                //    keyperson_dataGridView.Rows[keyperson_index].DefaultCellStyle.ForeColor = Color.Black;
                                //}
                                //else if (opinion_text == "ضد")
                                //{
                                //    keyperson_dataGridView.Rows[keyperson_index].DefaultCellStyle.BackColor = Color.FromArgb(220, 138, 94);
                                //    keyperson_dataGridView.Rows[keyperson_index].DefaultCellStyle.ForeColor = Color.Black;
                                //} 

                                keyperson_dataGridView.Rows[keyperson_index].Cells["K_InDataBase"].Value = "1";

                                keyperson_index++;
                            }
                            else if (dt.Rows[i]["Location"].ToString() == "t") //team
                            {
                                team_dataGridView.Rows.Add();
                                team_dataGridView.Rows[team_index].Cells["T_Checklist_ID"].Value = dt.Rows[i].Field<int>("ID");
                                team_dataGridView.Rows[team_index].Cells["T_ID"].Value = dt.Rows[i].Field<int>("Member_ID");
                                 
                                team_dataGridView.Rows[team_index].Cells["T_Name"].Value = dt.Rows[i].Field<int>("Member_ID");

                                team_dataGridView.Rows[team_index].Cells["T_Position"].Value = dt.Rows[i].Field<string>("Position").ToString();
                                team_dataGridView.Rows[team_index].Cells["T_Notes"].Value = dt.Rows[i].Field<string>("Notes");
                                team_dataGridView.Rows[team_index].Cells["T_Opinion"].Value = opinion_text;
                                if (opinion_text == "مع")
                                {
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.BackColor = Color.FromArgb(104, 157, 202);
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.ForeColor = Color.Black;
                                }
                                else if (opinion_text == "ضد")
                                {
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.BackColor = Color.FromArgb(220, 138, 94);
                                    team_dataGridView.Rows[team_index].DefaultCellStyle.ForeColor = Color.Black;
                                }

                                team_dataGridView.Rows[team_index].Cells["T_InDataBase"].Value = "1";

                                team_index++;
                            }
                            else if (dt.Rows[i]["Location"].ToString() == "v") //visit
                            {
                                visit_dataGridView.Rows.Add();
                                visit_dataGridView.Rows[visit_index].Cells["V_Checklist_ID"].Value = dt.Rows[i].Field<int>("ID");
                                visit_dataGridView.Rows[visit_index].Cells["V_ID"].Value = dt.Rows[i].Field<int>("Member_ID");
                                 
                                visit_dataGridView.Rows[visit_index].Cells["V_Name"].Value = dt.Rows[i].Field<int>("Member_ID");

                                visit_dataGridView.Rows[visit_index].Cells["V_Notes"].Value = dt.Rows[i].Field<string>("Notes");
                                visit_dataGridView.Rows[visit_index].Cells["V_InDataBase"].Value = "1";
                                visit_index++;
                            }
                            else if (dt.Rows[i]["Location"].ToString() == "c") //commity
                            {
                                commity_dataGridView.Rows.Add();
                                commity_dataGridView.Rows[commity_index].Cells["C_Checklist_ID"].Value = dt.Rows[i].Field<int>("ID");
                                commity_dataGridView.Rows[commity_index].Cells["C_ID"].Value = dt.Rows[i].Field<int>("Member_ID");
                                 
                                commity_dataGridView.Rows[commity_index].Cells["C_Name"].Value = dt.Rows[i].Field<int>("Member_ID");

                                commity_dataGridView.Rows[commity_index].Cells["C_Position"].Value = dt.Rows[i].Field<string>("Position").ToString();
                                commity_dataGridView.Rows[commity_index].Cells["C_Notes"].Value = dt.Rows[i].Field<string>("Notes");
                                commity_dataGridView.Rows[commity_index].Cells["C_Opinion"].Value = opinion_text;
                                if (opinion_text == "مع")
                                {
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.BackColor = Color.FromArgb(104, 157, 202);
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.ForeColor = Color.Black;
                                }
                                else if (opinion_text == "ضد")
                                {
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.BackColor = Color.FromArgb(220, 138, 94);
                                    commity_dataGridView.Rows[commity_index].DefaultCellStyle.ForeColor = Color.Black;
                                }

                                commity_dataGridView.Rows[commity_index].Cells["C_InDataBase"].Value = "1";
                                commity_index++;
                            }
                        } 
                        //Color_Tables();
                    }
                    keyperson_dataGridView.ClearSelection();
                    team_dataGridView.ClearSelection();
                    visit_dataGridView.ClearSelection();
                    commity_dataGridView.ClearSelection();
                     
                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = true;
                    mainForm.MP_ID_label.Text = MicroProject_ID.ToString();

                    Update_Mode = true;
                    user_mode = true;
                }
        } 
        private void Clear_Boxes()
        {
            MicroProject_ID = -1;
            Person_Name_textBox.Text = MicroProject_ID_textBox.Text = KeyPersonNotes_textBox.Text =
                //ParishNotes_textBox.Text = 
                VisitNotes_textBox.Text = VisitTime_textBox.Text = "";

            //clear radiobuttons
            //for (int i = 1; i <= 7; i++)
            //    set_radioButtons(i, -1);

            keyperson_dataGridView.Rows.Clear();
            team_dataGridView.Rows.Clear();
            visit_dataGridView.Rows.Clear();
            commity_dataGridView.Rows.Clear(); 
        }
         
        #region key persons table
        private void keyperson_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == keyperson_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;
                  

                //Check if click is on specific column 
                if (e.ColumnIndex == keyperson_dataGridView.Columns["K_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (keyperson_dataGridView["K_InDataBase", e.RowIndex].Value == null)
                            {
                                keyperson_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(keyperson_dataGridView.Rows[e.RowIndex].Cells["K_Checklist_ID"].Value.ToString());
                                var Name = keyperson_dataGridView.Rows[e.RowIndex].Cells["K_Name"].Value.ToString();

                                cl.Delete(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID
                                    , "CheckList", Program.UserName, DateTime.Now);
                                keyperson_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void keyperson_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //keyperson_dataGridView.EndEdit();
        }
         
        private void keyperson_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                keyperson_dataGridView.EndEdit();
                // No Need to fill position Just in KeyPerson
                
                //if (e.ColumnIndex == 2) //Name
                //{
                //    if (keyperson_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                //    {
                //        string id = keyperson_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                //        var row = checklist_member_dt.Select("ID = " + id);
                //        string position = row[0].Field<string>(3);
                //        keyperson_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                //    }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        private void keyperson_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == keyperson_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == keyperson_dataGridView.Columns["K_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
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
        private void keyperson_dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region team table
        private void team_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == team_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == team_dataGridView.Columns["T_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (team_dataGridView["T_InDataBase", e.RowIndex].Value == null)
                            {
                                team_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(team_dataGridView.Rows[e.RowIndex].Cells["T_Checklist_ID"].Value.ToString());
                                var Name = team_dataGridView.Rows[e.RowIndex].Cells["T_Name"].Value.ToString();

                                cl.Delete(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID,
                                        "CheckList", Program.UserName, DateTime.Now);
                                team_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void team_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2) //Name
                {
                    if (team_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        string id = team_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var row = checklist_member_dt.Select("ID = " + id);
                        string position = row[0].Field<string>(3);
                        team_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void team_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == team_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == team_dataGridView.Columns["T_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
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
        #endregion

        #region visit table
        private void visit_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == visit_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == visit_dataGridView.Columns["V_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (visit_dataGridView["V_InDataBase", e.RowIndex].Value == null)
                            {
                                visit_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(visit_dataGridView.Rows[e.RowIndex].Cells["V_Checklist_ID"].Value.ToString());
                                var Name = visit_dataGridView.Rows[e.RowIndex].Cells["V_Name"].Value.ToString();

                                cl.Delete(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID,
                                        "CheckList", Program.UserName, DateTime.Now);
                                visit_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        private void visit_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == visit_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == visit_dataGridView.Columns["V_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
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
        #endregion

        #region commity table
        private void commity_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if click is on new row or header row
                if (e.RowIndex == commity_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                //Check if click is on specific column 
                if (e.ColumnIndex == commity_dataGridView.Columns["C_DeleteRow"].Index)
                {
                    if (!CheckUserPermission()) //if the user don't have the permission to delete
                    {
                        MessageBox.Show("Sorry! You don't have the permission to do this action.");
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("تأكيد الحذف", "Delete", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (commity_dataGridView["C_InDataBase", e.RowIndex].Value == null)
                            {
                                commity_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                var Row_ID = Convert.ToInt32(commity_dataGridView.Rows[e.RowIndex].Cells["C_Checklist_ID"].Value.ToString());
                                var Name = commity_dataGridView.Rows[e.RowIndex].Cells["C_Name"].Value.ToString();

                                cl.Delete(Row_ID);
                                l.Insert_Log("Delete Member: " + Name + " from project: " + MicroProject_ID,
                                        "CheckList", Program.UserName, DateTime.Now);
                                commity_dataGridView.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        private void commity_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2) //Name
                {
                    if (commity_dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        string id = commity_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var row = checklist_member_dt.Select("ID = " + id);
                        string position = row[0].Field<string>(3);
                        commity_dataGridView.Rows[e.RowIndex].Cells[3].Value = position;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void commity_dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == commity_dataGridView.NewRowIndex || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == commity_dataGridView.Columns["C_DeleteRow"].Index)
                {
                    Image image = null;
                    if (Settings.Default.theme == "Light")
                        image = Resources.KAKA_Alii;
                    else image = Resources.KAKA_Alii_D;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    var size = image.Size;
                    var location = new System.Drawing.Point((e.CellBounds.Width - size.Width) / 2, (e.CellBounds.Height - size.Height) / 2);
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
        #endregion

        private void TeamDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TeamDate_checkBox.Checked) TeamDate_dateTimePicker.Visible = false;
            else TeamDate_dateTimePicker.Visible = true;
        } 
        private void Date_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (VisitDate_checkBox.Checked) VisitDate_dateTimePicker.Visible = false;
            else VisitDate_dateTimePicker.Visible = true;
        } 
        private void CommityDate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CommityDate_checkBox.Checked) CommityDate_dateTimePicker.Visible = false;
            else CommityDate_dateTimePicker.Visible = true;
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف جميع بيانات هذه الصفحة؟", "Delete",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (MicroProject_ID == -1)
                        throw new InvalidOperationException(
                            "لا يمكن حذف المشروع! الرجاء اختيار مشروع من قائمة البحث أولاً ثم المحاولة مرة ثانية.");

                    mp.Update_Micro_VisitDetails(MicroProject_ID, "", "", "", "", "" ,"" ,"" );
                    cl.Delete_All(MicroProject_ID);

                    Clear_Boxes();

                    Update_Mode = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            try
            {
                MicroProject_ID = Convert.ToInt32(MicroProject_ID_textBox.Text);
                 
                string Team_Date, Commity_Date, Visit_Date, Visit_Time, Visit_Notes, isVisited, ParishNotes, KeyPersonNotes;
                Team_Date = Commity_Date = Visit_Date = Visit_Time = Visit_Notes = isVisited= ParishNotes = KeyPersonNotes =""; 

                //Visit Details//
                if (VisitDate_dateTimePicker.Visible)
                {
                    Visit_Date = VisitDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                    isVisited = "1";
                }
                Visit_Time = VisitTime_textBox.Text;
                Visit_Notes = VisitNotes_textBox.Text;
                //ParishNotes = ParishNotes_textBox.Text;
                KeyPersonNotes = KeyPersonNotes_textBox.Text;

                if (TeamDate_dateTimePicker.Visible)
                {
                    Team_Date = TeamDate_dateTimePicker.Value.ToString("yyyy/MM/dd"); 
                }
                if (CommityDate_dateTimePicker.Visible)
                {
                    Commity_Date = CommityDate_dateTimePicker.Value.ToString("yyyy/MM/dd");
                }

                //ParishNotes,
                //,get_radioButtons_values(1),get_radioButtons_values(2)
                //,get_radioButtons_values(3),get_radioButtons_values(4)

                mp.Update_Micro_VisitDetails(MicroProject_ID, isVisited
                    , Visit_Date, Visit_Time
                    , Visit_Notes , KeyPersonNotes 
                    , Team_Date , Commity_Date);

                l.Insert_Log("Update Visit Details of Project:" +MicroProject_ID +":" + Person_Name_textBox.Text, "CheckList", Program.UserName, DateTime.Now);

                Insert_Update_ChecklistDetails();

                MessageBox.Show("تم حفظ كافة المعلومات بنجاح", "Save", MessageBoxButtons.OK, MessageBoxIcon.None);
                MicroProject_ID_textBox_Leave(sender, e);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void Insert_Update_ChecklistDetails()
        {
            string notes, opinion_text;
            int opinion, K_ID;
            string name_notes, role_notes;

            // KeyPerson //
            notes = opinion_text = "";
            name_notes = role_notes = ""; //Just for KeyPerson
            opinion = -1;
            for (var i = 0; i < keyperson_dataGridView.RowCount - 1; i++)
            { 
                if (keyperson_dataGridView.Rows[i].Cells["K_Name"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_Name"].Value.ToString() == null)
                    K_ID = -1;
                else K_ID = Convert.ToInt32(keyperson_dataGridView.Rows[i].Cells["K_Name"].Value.ToString());

                if (keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = keyperson_dataGridView.Rows[i].Cells["K_Notes"].Value.ToString();

                //Just for KeyPerson
                if (keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value.ToString() == null)
                    name_notes = "";
                else name_notes = keyperson_dataGridView.Rows[i].Cells["K_NameNotes"].Value.ToString();

                if (keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value == null || keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value.ToString() == null)
                    role_notes = "";
                else role_notes = keyperson_dataGridView.Rows[i].Cells["K_RoleNotes"].Value.ToString();

                //if (keyperson_dataGridView.Rows[i].Cells["K_Opinion"].Value.ToString() == "مع")
                //{ opinion = 1; }
                //else if (keyperson_dataGridView.Rows[i].Cells["K_Opinion"].Value.ToString() == "ضد")
                //    opinion = 0;
                //else opinion = -1;
                //opinion_text = keyperson_dataGridView.Rows[i].Cells["K_Opinion"].Value.ToString();

                ////set ID columns// 
                keyperson_dataGridView.Rows[i].Cells["K_ID"].Value = K_ID;
                if (K_ID != -1)
                {
                    if (keyperson_dataGridView.Rows[i].Cells["K_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert(MicroProject_ID, K_ID, opinion, notes, name_notes, role_notes, "k");

                        keyperson_dataGridView.Rows[i].Cells["K_InDataBase"].Value = "1";
                        keyperson_dataGridView.Rows[i].Cells["K_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (k)-" + name_notes + ":" + role_notes + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(keyperson_dataGridView.Rows[i].Cells["K_Checklist_ID"].Value);
                        cl.Update(RowID, K_ID, opinion, notes, name_notes, role_notes);
                        l.Insert_Log("Update (k)-" + name_notes + ":" + role_notes + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }

            // Team //
            notes = opinion_text = "";
            opinion = -1;
            int T_ID;
            for (var i = 0; i < team_dataGridView.RowCount - 1; i++)
            {
                if (team_dataGridView.Rows[i].Cells["T_Name"].Value == null || team_dataGridView.Rows[i].Cells["T_Name"].Value.ToString() == null)
                      T_ID = -1;
                else T_ID = Convert.ToInt32(team_dataGridView.Rows[i].Cells["T_Name"].Value.ToString());

                if (team_dataGridView.Rows[i].Cells["T_Notes"].Value == null || team_dataGridView.Rows[i].Cells["T_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = team_dataGridView.Rows[i].Cells["T_Notes"].Value.ToString();

                if (team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString() == "مع")
                { opinion = 1; }
                else if (team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString() == "ضد")
                    opinion = 0;
                else opinion = -1;
                opinion_text = team_dataGridView.Rows[i].Cells["T_Opinion"].Value.ToString();

                //set ID columns// 
                team_dataGridView.Rows[i].Cells["T_ID"].Value = T_ID;
                if (T_ID != -1)
                {
                    if (team_dataGridView.Rows[i].Cells["T_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert(MicroProject_ID, T_ID, opinion, notes, "", "", "t");

                        team_dataGridView.Rows[i].Cells["T_InDataBase"].Value = "1";
                        team_dataGridView.Rows[i].Cells["T_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (t)-" + T_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text, "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(team_dataGridView.Rows[i].Cells["T_Checklist_ID"].Value);
                        cl.Update(RowID, T_ID, opinion, notes, "", "");

                        l.Insert_Log("Update (t)-" + T_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text, "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }

            // Visit //
            notes = opinion_text = "";
            opinion = -1;
            int V_ID;
            for (var i = 0; i < visit_dataGridView.RowCount - 1; i++)
            { 
                if (visit_dataGridView.Rows[i].Cells["V_Name"].Value == null || visit_dataGridView.Rows[i].Cells["V_Name"].Value.ToString() == null)
                    V_ID = -1;
                else V_ID = Convert.ToInt32(visit_dataGridView.Rows[i].Cells["V_Name"].Value.ToString());

                if (visit_dataGridView.Rows[i].Cells["V_Notes"].Value == null || visit_dataGridView.Rows[i].Cells["V_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = visit_dataGridView.Rows[i].Cells["V_Notes"].Value.ToString();

                //set ID columns// 
                visit_dataGridView.Rows[i].Cells["V_ID"].Value = V_ID;
                if (V_ID != -1)
                {   if (visit_dataGridView.Rows[i].Cells["V_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert(MicroProject_ID, V_ID, opinion, notes, "", "", "v");

                        visit_dataGridView.Rows[i].Cells["V_InDataBase"].Value = "1";
                        visit_dataGridView.Rows[i].Cells["V_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (v)-" + V_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(visit_dataGridView.Rows[i].Cells["V_Checklist_ID"].Value);
                        cl.Update(RowID, V_ID, opinion, notes, "", "");

                        l.Insert_Log("Update (v)-" + V_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }

            // Commity //
            notes = opinion_text = "";
            opinion = -1;
            int C_ID;

            for (var i = 0; i < commity_dataGridView.RowCount - 1; i++)
            {
                if (commity_dataGridView.Rows[i].Cells["C_Name"].Value == null || commity_dataGridView.Rows[i].Cells["C_Name"].Value.ToString() == null)
                    C_ID = -1;
                else C_ID = Convert.ToInt32(commity_dataGridView.Rows[i].Cells["C_Name"].Value.ToString());

                if (commity_dataGridView.Rows[i].Cells["C_Notes"].Value == null || commity_dataGridView.Rows[i].Cells["C_Notes"].Value.ToString() == null)
                    notes = "";
                else notes = commity_dataGridView.Rows[i].Cells["C_Notes"].Value.ToString();

                if (commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString() == "مع")
                { opinion = 1; }
                else if (commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString() == "ضد")
                    opinion = 0;
                else opinion = -1;
                opinion_text = commity_dataGridView.Rows[i].Cells["C_Opinion"].Value.ToString();

                //set ID columns//
                commity_dataGridView.Rows[i].Cells["C_ID"].Value = C_ID;
                if (C_ID != -1)
                {
                    if (commity_dataGridView.Rows[i].Cells["C_InDataBase"].Value == null)
                    {
                        int RowID = cl.Insert(MicroProject_ID, C_ID, opinion, notes, "", "", "c");

                        commity_dataGridView.Rows[i].Cells["C_InDataBase"].Value = "1";
                        commity_dataGridView.Rows[i].Cells["C_Checklist_ID"].Value = RowID;
                        l.Insert_Log("Insert (c)-" + C_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                    else /// update
                    {
                        var RowID = Convert.ToInt32(commity_dataGridView.Rows[i].Cells["C_Checklist_ID"].Value);
                        cl.Update(RowID, C_ID, opinion, notes, "", "");

                        l.Insert_Log("Update (c)-" + C_ID + ":" + opinion_text + ":" + "to Project:" + MicroProject_ID + ":" + Person_Name_textBox.Text
                            , "CheckList", Program.UserName, DateTime.Now);
                    }
                }
            }
        }

        #region new checklist members
        private void Add_K_button_Click(object sender, EventArgs e)
        {
            try
            { 
                using (var form = new AddNewChecklistMember("Key Person", "k"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_T_button_Click(object sender, EventArgs e)
        {
            try
            { 
                using (var form = new AddNewChecklistMember("Team Member", "t"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_V_button_Click(object sender, EventArgs e)
        {
            try
            {  
                using (var form = new AddNewChecklistMember("Visitors", "t"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_C_button_Click(object sender, EventArgs e)
        {
            try
            {  
                using (var form = new AddNewChecklistMember("Commity Member", "c"))
                {
                    form.ShowDialog();
                    {
                        Bind_Gridview_ComboBox();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        //private void v1_pictureBox_Click(object sender, EventArgs e)
        //{
        //    foreach (RadioButton r in tableLayoutPanel_v1.Controls.OfType<RadioButton>())
        //        r.Checked = false;
        //}

        //private void v2_pictureBox_Click(object sender, EventArgs e)
        //{
        //    foreach (RadioButton r in tableLayoutPanel_v2.Controls.OfType<RadioButton>())
        //        r.Checked = false;
        //}

        //private void v3_pictureBox_Click(object sender, EventArgs e)
        //{
        //    foreach (RadioButton r in tableLayoutPanel_v3.Controls.OfType<RadioButton>())
        //        r.Checked = false;
        //}

        //private void v4_pictureBox_Click(object sender, EventArgs e)
        //{
        //    foreach (RadioButton r in tableLayoutPanel_v4.Controls.OfType<RadioButton>())
        //        r.Checked = false;
        //}

        void set_radioButtons(int ansNo, int value)
        {
            int max_i = 3;
            if (ansNo == 1) max_i = 3;
            else if (ansNo == 2) max_i = 4;
            else if (ansNo == 3 || ansNo == 4) max_i = 2; 

            if (value == -1)
            {
                for (int i = 0; i < max_i; i++)
                {
                    string radio_name = "v" + ansNo + "_" + i + "_radioButton";
                    var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                    rad_bx.Checked = false;
                }
            }
            else
            {
                string radio_name = "v" + ansNo + "_" + value + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                rad_bx.Checked = true;
            }
        }
        int get_radioButtons_values(int ansNo)
        {
            int value = -1;
            int max_i = 3;
            if (ansNo == 1) max_i = 3;
            else if (ansNo == 2) max_i = 4;
            else if (ansNo == 3 || ansNo == 4) max_i = 2;
             
            for (int i = 0; i < max_i; i++)
            {
                string radio_name = "v" + ansNo + "_" + i + "_radioButton";
                var rad_bx = Controls.Find(radio_name, true).FirstOrDefault() as RadioButton;
                if (rad_bx.Checked)
                {
                    value = i;
                    break;
                }
            }

            return value;
        }

        #region context menu strip
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new ChooseVisitKind_Form(MicroProject_ID, mainForm);
                _Form.ShowDialog();
            }
        }  
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new Loans_Form(MicroProject_ID, mainForm);
                mainForm.showNewTab(_Form, "Loans", 0);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new Attachments_Form(MicroProject_ID, mainForm);
                mainForm.showNewTab(_Form, "Attachments", 0);
            }
        }
        private void showChecklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MicroProject_ID_textBox.Text != "")
            {
                var _Form = new CheckList_Form(MicroProject_ID, mainForm);
                mainForm.showNewTab(_Form, "CheckList", 0);
            }
        }



        #endregion

        private void Visit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (MicroProject_ID == -1 && Visit_checkBox.Checked)
                {
                    Visit_checkBox.Checked = false;
                    throw new InvalidOperationException("الرجاء حفظ المشروع أولاً ثم يمكنك التعديل على هذه الخلية.");
                }

                //Update project just in visit cell// 
                var state = "";

                if (Visit_checkBox.Checked == false) //not visited
                {
                    if (user_mode) mp.Update_Project(MicroProject_ID, "MP_Visited", "0","");
                    toolTip1.SetToolTip(Visit_checkBox, "لم تتم بعد");
                    Visit_checkBox.Text = "لم تتم بعد";
                    state = "Not Visited";
                }
                else // visited
                {
                    if (user_mode) mp.Update_Project(MicroProject_ID, "MP_Visited", "1","");
                    toolTip1.SetToolTip(Visit_checkBox, "تمت");
                    Visit_checkBox.Text = "تمت";
                    state = "Visited";
                }

                if (user_mode) l.Insert_Log("Update the project: " + MicroProject_ID + " SET visited to:" + state + ""
                    , "CheckList", Program.UserName, DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         
        private void button1_Click(object sender, EventArgs e)
        { 
            try
            {
                if (MicroProject_ID_textBox.Text != "" && MicroProject_ID != -1)
                {
                    string p_name = Person_Name_textBox.Text;
                    int charLocation = Person_Name_textBox.Text.IndexOf("-", StringComparison.Ordinal); 
                    if (charLocation > 0)
                    {
                        p_name = p_name.Substring(0, charLocation);
                    }

                    string condition = " and MP_ID in (" + MicroProject_ID_textBox.Text + ")"
                        + " and CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName) like N'" + p_name + "' ;";

                    dt4444 = new System.Data.DataTable();
                    dt4444 = cl.Checklist_Details_Procedure(condition);

                    Thread myTh = new Thread(SaveCallDialog_2);
                    myTh.SetApartmentState(ApartmentState.STA);
                    myTh.Start();
                    myTh.Join();
                }
                else
                {
                    MessageBox.Show("الرجاء اختيار المشروع أولاً ثم إعادة المحاولة.");
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        System.Data.DataTable dt4444;
        private void SaveCallDialog_2()
        {
            try
            {
                //to close locked word files//
                Process[] workers = Process.GetProcessesByName("Winword.exe");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }

                FileInfo file; 
                string sourcedirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\DocPrint"; 
                string myfilename = @"cl";
                string word_path = sourcedirectory + @"\" + myfilename + ".docx"; 
                string filename = sourcedirectory + @"\" +  myfilename + ".xlsx";
                XLWorkbook wb = new XLWorkbook { RightToLeft = true };

                string appdata_path
                    = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\DocPrint";

                file = new FileInfo(appdata_path);
  

                // To copy a folder's contents to a new location:
                // Create a new target folder.
                // If the directory already exists, this method does not create a new directory.
                System.IO.Directory.CreateDirectory(appdata_path);  

                // To copy a file to another location and
                // overwrite the destination file if it already exists.
                System.IO.File.Copy( 
                      sourcedirectory + @"\" + myfilename + ".docx"
                    , appdata_path + @"\" + myfilename + ".docx"
                    , true);


                filename = appdata_path + @"\" + myfilename + ".xlsx";

                //DELETE old xls files
                FileInfo deletefile = new FileInfo(myfilename + ".xls");
                if (deletefile.Exists.Equals(true))
                {
                    deletefile.Delete();
                } 

                wb.Worksheets.Add(dt4444, "Sheet1");
                     
                wb.SaveAs(filename);
                wb.Dispose();
                  
                // convert .xlsx to .xls
                string newfilename = filename.Remove(filename.Length - 1, 1);
                System.IO.File.Delete(newfilename);
                System.IO.File.Move(filename, newfilename);
                 
                Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document document = ap.Documents.Open($@"{appdata_path}/{myfilename}.docx"); 
                document.MailMerge.MainDocumentType = WdMailMergeMainDocType.wdFormLetters;  
                document.MailMerge.OpenDataSource($@"{appdata_path}/{myfilename}.xls"
                    , WdMailMergeMainDocType.wdFormLetters
                    , false
                    , false
                    , true
                    , false
                    , ""
                    , ""
                    , false
                    , ""
                    , ""
                    , $@"Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source={appdata_path}/{myfilename}.xlsx;"
                    , ""
                    , ""); 
                document.MailMerge.Destination = WdMailMergeDestination.wdSendToNewDocument;
                document.MailMerge.SuppressBlankLines = true;
                document.MailMerge.DataSource.LastRecord = (int)WdMailMergeDefaultRecord.wdDefaultFirstRecord;
                document.MailMerge.DataSource.LastRecord = (int)WdMailMergeDefaultRecord.wdDefaultLastRecord;
                document.MailMerge.Execute();
                document.Close(WdSaveOptions.wdDoNotSaveChanges); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                //to close locked word files//
                Process[] workers = Process.GetProcessesByName("Winword.exe");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
        }

    }
}
