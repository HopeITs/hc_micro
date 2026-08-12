using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class ProfessionMeetings_Form : Form
    {
        private readonly MainForm mainForm;
        private bool updatingGuildOptions;
        private bool loadingProfessionProjects;
        private bool fillingParticipantProjectRow;
        private DataSet professionDataSet;
        private DataTable professionProjectsTable;
        private readonly Dictionary<string, int> questionIds = new Dictionary<string, int>();
        private readonly Dictionary<string, int> optionIds = new Dictionary<string, int>();
        private readonly Dictionary<string, int> userIdsByName = new Dictionary<string, int>();
        private readonly Color accentColor = Color.FromArgb(165, 54, 48);

        public ProfessionMeetings_Form()
            : this(null)
        {
        }

        public ProfessionMeetings_Form(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            PopulateDefaultChallengeRows();
            ConfigureDataGridViewsForRtlEditing();
        }

        private void ProfessionMeetings_Form_Load(object sender, EventArgs e)
        {
            try
            {
                if (mainForm != null)
                {
                    mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                    mainForm.TabName_label.Text = "Profession Meetings";
                }

                ApplyTheme();
                PopulateProfessionComboBox();
                UpdateGuildDetailsVisibility();
                ConfigureDataGridViewsForRtlEditing();
                ConfigureParticipantProjectGrids();
                PopulateRegistrarFields();
                BeginInvoke((MethodInvoker)ResetInitialScrollPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetInitialScrollPosition()
        {
            content_tabControl.SelectedIndex = 0;
            content_tabControl.Focus();

            survey_scrollPanel.AutoScrollPosition = Point.Empty;
            minutes_scrollPanel.AutoScrollPosition = Point.Empty;
        }

        private void ApplyTheme()
        {
            try
            {
                NewTheme newTheme = new NewTheme();
                if (Settings.Default.theme == "Light")
                    newTheme.ApplicationNew_ToLight(this);
                else
                    newTheme.ApplicationNew_ToNight(this);

                ApplyConsistentAccentColor();
            }
            catch
            {
                BackColor = Color.FromArgb(240, 240, 240);
                ForeColor = Color.FromArgb(60, 60, 60);
                ApplyConsistentAccentColor();
            }
        }

        private void ApplyConsistentAccentColor()
        {
            title_label.ForeColor = accentColor;

            Label[] headerLabels =
            {
                section1_label,
                section15_label,
                section31_label,
                section41_label,
                section44_label,
                section56_label,
                question57_label,
                question58_label,
                question59_label,
                question60_label,
                question61_label
            };

            foreach (Label headerLabel in headerLabels)
            {
                headerLabel.BackColor = accentColor;
                headerLabel.ForeColor = Color.White;
            }
        }

        private void PopulateRegistrarFields()
        {
            string userName = Program.UserName ?? string.Empty;
            programRegistrar_textBox.Text = userName;
            editedBy_textBox.Text = userName;

            userIdsByName.Clear();
            if (!string.IsNullOrWhiteSpace(userName) && Program.userID > 0)
                userIdsByName[userName] = Program.userID;

            paperRegistrar_comboBox.Items.Clear();
            if (!string.IsNullOrWhiteSpace(userName))
                paperRegistrar_comboBox.Items.Add(userName);

            try
            {
                User user = new User();
                DataTable users = user.Select_Users("", "", "");
                if (users != null)
                {
                    foreach (DataRow row in users.Rows)
                    {
                        if (row.Table.Columns.Contains("Username") && row["Username"] != null)
                        {
                            string name = row["Username"].ToString();
                            if (!string.IsNullOrWhiteSpace(name) && !paperRegistrar_comboBox.Items.Contains(name))
                                paperRegistrar_comboBox.Items.Add(name);

                            if (!string.IsNullOrWhiteSpace(name) &&
                                row.Table.Columns.Contains("ID") &&
                                row["ID"] != null &&
                                row["ID"] != DBNull.Value)
                                userIdsByName[name] = Convert.ToInt32(row["ID"]);
                        }
                    }
                }
            }
            catch
            {
            }

            if (paperRegistrar_comboBox.Items.Count > 0)
                paperRegistrar_comboBox.SelectedIndex = 0;
        }

        private void paperRegistrar_comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawCenteredComboBoxItem(sender, e);
        }

        private void professionSubCategory_comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawCenteredComboBoxItem(sender, e);
        }

        private void professionCareer_comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawCenteredComboBoxItem(sender, e);
        }

        private void DrawCenteredComboBoxItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color backColor = selected ? Color.FromArgb(217, 216, 214) : comboBox.BackColor;
            Color foreColor = Color.Black;

            using (SolidBrush backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, e.Bounds);

            string text = e.Index >= 0
                ? comboBox.GetItemText(comboBox.Items[e.Index])
                : comboBox.Text;

            TextRenderer.DrawText(
                e.Graphics,
                text,
                comboBox.Font,
                e.Bounds,
                foreColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.RightToLeft);

            e.DrawFocusRectangle();
        }

        private void PopulateProfessionComboBox()
        {
            professionDataSet = GetProfessionDataSet();

            professionSubCategory_comboBox.SelectedIndexChanged -= professionSubCategory_comboBox_SelectedIndexChanged;
            professionCareer_comboBox.SelectedIndexChanged -= professionCareer_comboBox_SelectedIndexChanged;

            professionSubCategory_comboBox.DataSource = null;
            professionSubCategory_comboBox.DisplayMember = "Name";
            professionSubCategory_comboBox.ValueMember = "ID";
            professionSubCategory_comboBox.DataSource = professionDataSet.Tables[0];
            professionSubCategory_comboBox.SelectedIndex = -1;

            BindCareerComboBox(string.Empty);

            professionSubCategory_comboBox.SelectedIndexChanged += professionSubCategory_comboBox_SelectedIndexChanged;
            professionCareer_comboBox.SelectedIndexChanged += professionCareer_comboBox_SelectedIndexChanged;

            professionProjectsTable = CreateProjectLookupTable();
            ConfigureProjectComboColumns(professionProjectsTable);
        }

        private DataTable CreateProjectLookupTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ProjectId", typeof(int));
            table.Columns.Add("ProjectName", typeof(string));
            table.Columns.Add("ProjectSpeciality", typeof(string));
            return table;
        }

        private DataSet GetProfessionDataSet()
        {
            Program.buildConnection();
            try
            {
                string query = "";
                query += "SELECT ID, Name, Category_ID FROM subcategory ORDER BY Name ASC;";
                query += "SELECT ID, Name, SubCategory_ID FROM career ORDER BY Name ASC;";

                MySqlCommand command = new MySqlCommand(query, Program.MyConn);
                command.ExecuteNonQuery();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        private void BindCareerComboBox(string subCategoryId)
        {
            if (professionDataSet == null || professionDataSet.Tables.Count < 2)
                return;

            professionCareer_comboBox.DataSource = null;
            professionCareer_comboBox.DisplayMember = "Name";
            professionCareer_comboBox.ValueMember = "ID";

            if (string.IsNullOrWhiteSpace(subCategoryId))
            {
                professionCareer_comboBox.DataSource = professionDataSet.Tables[1];
                professionCareer_comboBox.SelectedIndex = -1;
                return;
            }

            DataRow[] rows = professionDataSet.Tables[1].Select("SubCategory_ID=" + subCategoryId);
            DataTable selectedRows = professionDataSet.Tables[1].Copy();
            selectedRows.Rows.Clear();

            foreach (DataRow row in rows)
                selectedRows.ImportRow(row);

            professionCareer_comboBox.DataSource = selectedRows;
            professionCareer_comboBox.SelectedIndex = -1;
        }

        private void professionSubCategory_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (professionSubCategory_comboBox.SelectedValue == null ||
                professionSubCategory_comboBox.SelectedValue.GetType() == typeof(DataRowView))
                return;

            loadingProfessionProjects = true;
            try
            {
                BindCareerComboBox(professionSubCategory_comboBox.SelectedValue.ToString());
                professionProjectsTable = CreateProjectLookupTable();
                ConfigureProjectComboColumns(professionProjectsTable);
                ClearParticipantProjectRows();
            }
            finally
            {
                loadingProfessionProjects = false;
            }
        }

        private void professionCareer_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingProfessionProjects)
                return;

            if (professionCareer_comboBox.SelectedValue == null ||
                professionCareer_comboBox.SelectedValue.GetType() == typeof(DataRowView))
                return;

            int professionId;
            if (!int.TryParse(professionCareer_comboBox.SelectedValue.ToString(), out professionId))
                return;

            loadingProfessionProjects = true;
            try
            {
                professionProjectsTable = GetProjectsByProfession(professionId);
                ConfigureProjectComboColumns(professionProjectsTable);
                ClearParticipantProjectRows();
            }
            finally
            {
                loadingProfessionProjects = false;
            }
        }

        private DataTable GetProjectsByProfession(int professionId)
        {
            Program.buildConnection();
            try
            {
                string query = @"
                        SELECT MP.MP_ID AS ProjectId,
                               IFNULL(MP.MP_Name, '') AS ProjectName,
                               IFNULL(MP.MP_Speciality, '') AS ProjectSpeciality
                        FROM microproject MP
                        WHERE MP.Career_ID = @CareerId
                        ORDER BY MP.MP_ID DESC";

                MySqlCommand command = new MySqlCommand(query, Program.MyConn);
                command.Parameters.AddWithValue("@CareerId", professionId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        private DataTable GetProjectBeneficiaries(int projectId)
        {
            Program.buildConnection();
            try
            {
                string query = @"
                        SELECT person.P_ID AS ID,
                               CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) AS Name
                        FROM person_microproject
                        LEFT JOIN person ON person.P_ID = person_microproject.Person_ID
                        WHERE person_microproject.MicroProject_ID = @ProjectId
                        ORDER BY Name ASC";

                MySqlCommand command = new MySqlCommand(query, Program.MyConn);
                command.Parameters.AddWithValue("@ProjectId", projectId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        private void ConfigureParticipantProjectGrids()
        {
            DataGridView[] participantGrids =
            {
                participant1_dataGridView,
                participant2_dataGridView,
                participant3_dataGridView,
                participant4_dataGridView,
                attendees_dataGridView
            };

            foreach (DataGridView grid in participantGrids)
            {
                grid.CurrentCellDirtyStateChanged -= ParticipantDataGridView_CurrentCellDirtyStateChanged;
                grid.CellValueChanged -= ParticipantDataGridView_CellValueChanged;
                grid.EditingControlShowing -= ParticipantDataGridView_EditingControlShowing;
                grid.CellContentClick -= ParticipantDataGridView_CellContentClick;
                grid.DataError -= ParticipantDataGridView_DataError;
                grid.CurrentCellDirtyStateChanged += ParticipantDataGridView_CurrentCellDirtyStateChanged;
                grid.CellValueChanged += ParticipantDataGridView_CellValueChanged;
                grid.EditingControlShowing += ParticipantDataGridView_EditingControlShowing;
                grid.CellContentClick += ParticipantDataGridView_CellContentClick;
                grid.DataError += ParticipantDataGridView_DataError;
            }
        }

        private void ConfigureProjectComboColumns(DataTable projects)
        {
            DataGridView[] participantGrids =
            {
                participant1_dataGridView,
                participant2_dataGridView,
                participant3_dataGridView,
                participant4_dataGridView,
                attendees_dataGridView
            };

            foreach (DataGridView grid in participantGrids)
            {
                DataGridViewComboBoxColumn projectColumn = grid.Columns[0] as DataGridViewComboBoxColumn;
                if (projectColumn == null)
                    continue;

                projectColumn.DataSource = projects == null ? CreateProjectLookupTable() : projects.Copy();
                projectColumn.DisplayMember = "ProjectId";
                projectColumn.ValueMember = "ProjectId";
                projectColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                projectColumn.FlatStyle = FlatStyle.Flat;
                projectColumn.DefaultCellStyle.BackColor = Color.White;
                projectColumn.DefaultCellStyle.ForeColor = Color.Black;
                projectColumn.DefaultCellStyle.SelectionBackColor = Color.FromArgb(217, 216, 214);
                projectColumn.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void ClearParticipantProjectRows()
        {
            DataGridView[] participantGrids =
            {
                participant1_dataGridView,
                participant2_dataGridView,
                participant3_dataGridView,
                participant4_dataGridView,
                attendees_dataGridView
            };

            foreach (DataGridView grid in participantGrids)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    row.Cells[0].Value = null;
                    row.Cells[1] = new DataGridViewTextBoxCell();
                    row.Cells[1].Value = string.Empty;
                    row.Cells[2].Value = string.Empty;
                }
            }
        }

        private void ParticipantDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (fillingParticipantProjectRow)
                return;

            DataGridView grid = sender as DataGridView;
            if (grid != null && grid.IsCurrentCellDirty && grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == 0)
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void ParticipantDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ParticipantDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewComboBoxEditingControl comboBox = e.Control as DataGridViewComboBoxEditingControl;
            if (comboBox == null)
                return;

            comboBox.SelectionChangeCommitted -= ParticipantProjectComboBox_SelectionChangeCommitted;

            DataGridView grid = sender as DataGridView;
            if (grid == null || grid.CurrentCell == null || grid.CurrentCell.ColumnIndex != 0)
                return;

            comboBox.SelectionChangeCommitted += ParticipantProjectComboBox_SelectionChangeCommitted;
        }

        private void ParticipantProjectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CommitParticipantProjectSelection(sender as DataGridViewComboBoxEditingControl);
        }

        private void CommitParticipantProjectSelection(DataGridViewComboBoxEditingControl comboBox)
        {
            if (comboBox == null || comboBox.EditingControlDataGridView == null || fillingParticipantProjectRow)
                return;

            DataGridView grid = comboBox.EditingControlDataGridView;
            if (grid.CurrentCell == null || grid.CurrentCell.ColumnIndex != 0)
                return;

            int rowIndex = grid.CurrentCell.RowIndex;
            object selectedProjectId = GetSelectedProjectId(comboBox);
            if (selectedProjectId == null)
                return;

            comboBox.EditingControlValueChanged = true;
            grid.CurrentCell.Value = selectedProjectId;
            grid.NotifyCurrentCellDirty(true);
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            grid.EndEdit();

            QueueFillParticipantProjectRow(grid, rowIndex, selectedProjectId);
        }

        private object GetSelectedProjectId(ComboBox comboBox)
        {
            if (comboBox == null)
                return null;

            if (comboBox.SelectedValue != null && comboBox.SelectedValue.GetType() != typeof(DataRowView))
                return comboBox.SelectedValue;

            DataRowView rowView = comboBox.SelectedItem as DataRowView;
            if (rowView != null && rowView.Row.Table.Columns.Contains("ProjectId"))
                return rowView["ProjectId"];

            if (!string.IsNullOrWhiteSpace(comboBox.Text))
                return comboBox.Text.Trim();

            return null;
        }

        private void ParticipantDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ParticipantDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (!(grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
                return;

            if (!grid.Rows[e.RowIndex].IsNewRow)
                grid.Rows.RemoveAt(e.RowIndex);
        }

        private void DataGridView_DeleteButtonCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex == grid.NewRowIndex || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (!(grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
                return;

            Image image = Settings.Default.theme == "Light" ? Resources.KAKA_Alii : Resources.KAKA_Alii_D;
            if (image == null)
                return;

            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
            Size size = image.Size;
            Point location = new Point(
                (e.CellBounds.Width - size.Width) / 2,
                (e.CellBounds.Height - size.Height) / 2);
            location.Offset(e.CellBounds.Location);
            e.Graphics.DrawImage(image, location);
            e.Handled = true;
        }

        private void FillParticipantProjectRow(DataGridView grid, int rowIndex, object selectedProjectId)
        {
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count)
                return;

            if (fillingParticipantProjectRow)
                return;

            fillingParticipantProjectRow = true;
            try
            {
                DataGridViewRow row = grid.Rows[rowIndex];
                if (row.IsNewRow)
                    return;

                if (selectedProjectId != null)
                {
                    row.Cells[0].Value = selectedProjectId;
                }

                ResetParticipantDependentCells(row);

                if (row.Cells[0].Value == null)
                    return;

                int projectId;
                if (!int.TryParse(row.Cells[0].Value.ToString(), out projectId))
                    return;

                DataRow projectRow = FindProjectRow(projectId);
                row.Cells[2].Value = projectRow == null ? string.Empty : projectRow["ProjectSpeciality"].ToString();

                DataTable beneficiaries = GetProjectBeneficiaries(projectId);
                if (beneficiaries.Rows.Count == 0)
                {
                    row.Cells[1].Value = string.Empty;
                    row.Cells[1].Tag = null;
                }
                else
                {
                    DataGridViewComboBoxCell beneficiaryCell = new DataGridViewComboBoxCell();
                    beneficiaryCell.DataSource = beneficiaries;
                    beneficiaryCell.DisplayMember = "Name";
                    beneficiaryCell.ValueMember = "ID";
                    beneficiaryCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    beneficiaryCell.FlatStyle = FlatStyle.Flat;
                    beneficiaryCell.Style.BackColor = Color.White;
                    beneficiaryCell.Style.ForeColor = Color.Black;
                    beneficiaryCell.Style.SelectionBackColor = Color.FromArgb(217, 216, 214);
                    beneficiaryCell.Style.SelectionForeColor = Color.Black;
                    row.Cells[1] = beneficiaryCell;
                    row.Cells[1].Value = beneficiaries.Rows[0]["ID"];
                    row.Cells[1].Tag = null;
                }

                row.Cells[0].Style.BackColor = Color.White;
                row.Cells[0].Style.ForeColor = Color.Black;
                row.Cells[0].Style.SelectionBackColor = Color.FromArgb(217, 216, 214);
                row.Cells[0].Style.SelectionForeColor = Color.Black;
                row.Cells[2].Style.BackColor = Color.White;
                row.Cells[2].Style.ForeColor = Color.Black;
                row.Cells[2].Style.SelectionBackColor = Color.FromArgb(217, 216, 214);
                row.Cells[2].Style.SelectionForeColor = Color.Black;
            }
            finally
            {
                fillingParticipantProjectRow = false;
            }
        }

        private void QueueFillParticipantProjectRow(DataGridView grid, int rowIndex, object selectedProjectId)
        {
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count)
                return;

            if (grid.IsHandleCreated)
            {
                    FillParticipantProjectRow(grid, rowIndex, selectedProjectId);
            }
        }

        private void ResetParticipantDependentCells(DataGridViewRow row)
        {
            row.Cells[1] = new DataGridViewTextBoxCell();
            row.Cells[1].Value = string.Empty;
            row.Cells[1].Tag = null;
            row.Cells[1].Style.BackColor = Color.White;
            row.Cells[1].Style.ForeColor = Color.Black;
            row.Cells[1].Style.SelectionBackColor = Color.FromArgb(217, 216, 214);
            row.Cells[1].Style.SelectionForeColor = Color.Black;

            row.Cells[2].Value = string.Empty;
            row.Cells[2].Style.BackColor = Color.White;
            row.Cells[2].Style.ForeColor = Color.Black;
            row.Cells[2].Style.SelectionBackColor = Color.FromArgb(217, 216, 214);
            row.Cells[2].Style.SelectionForeColor = Color.Black;
        }

        private DataRow FindProjectRow(int projectId)
        {
            if (professionProjectsTable == null)
                return null;

            foreach (DataRow row in professionProjectsTable.Rows)
            {
                if (row["ProjectId"] != null && row["ProjectId"].ToString() == projectId.ToString())
                    return row;
            }

            return null;
        }

        private void PopulateDefaultChallengeRows()
        {
            if (challenges_dataGridView.Rows.Count > 1)
                return;

            string[] challenges =
            {
                "الوضع الاقتصادي",
                "اليد العاملة",
                "التسويق",
                "مكان المشروع",
                "نقص المعدات",
                "تنفيذ مرحلة من مراحل عملك خارج مكان العمل"
            };

            foreach (string challenge in challenges)
                challenges_dataGridView.Rows.Add(challenge, string.Empty, string.Empty);
        }

        private void ConfigureDataGridViewsForRtlEditing()
        {
            Color selectionBackColor = Color.FromArgb(217, 216, 214);
            Color alternateSelectionBackColor = Color.FromArgb(216, 217, 214);
            Color selectionForeColor = Color.Black;

            DataGridView[] grids =
            {
                season_dataGridView,
                equipment_dataGridView,
                challenges_dataGridView,
                participant1_dataGridView,
                participant2_dataGridView,
                participant3_dataGridView,
                participant4_dataGridView,
                attendees_dataGridView
            };

            foreach (DataGridView grid in grids)
            {
                grid.RightToLeft = RightToLeft.Yes;
                grid.EditMode = DataGridViewEditMode.EditOnEnter;
                grid.GridColor = Color.FromArgb(64, 64, 64);
                grid.EnableHeadersVisualStyles = false;
                grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.DefaultCellStyle.BackColor = Color.White;
                grid.DefaultCellStyle.ForeColor = Color.Black;
                grid.DefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.DefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.RowsDefaultCellStyle.BackColor = Color.White;
                grid.RowsDefaultCellStyle.ForeColor = Color.Black;
                grid.RowsDefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.RowsDefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
                grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = alternateSelectionBackColor;
                grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.RowHeadersDefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.RowHeadersDefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.RowTemplate.DefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.RowTemplate.DefaultCellStyle.SelectionForeColor = selectionForeColor;

                foreach (DataGridViewColumn column in grid.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.BackColor = Color.White;
                    column.DefaultCellStyle.ForeColor = Color.Black;
                    column.DefaultCellStyle.SelectionBackColor = selectionBackColor;
                    column.DefaultCellStyle.SelectionForeColor = selectionForeColor;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.HeaderCell.Style.SelectionBackColor = selectionBackColor;
                    column.HeaderCell.Style.SelectionForeColor = selectionForeColor;
                }

                grid.CellClick -= DataGridView_CellClick;
                grid.CellEnter -= DataGridView_CellEnter;
                grid.CellContentClick -= ParticipantDataGridView_CellContentClick;
                grid.CellPainting -= DataGridView_DeleteButtonCellPainting;
                grid.EditingControlShowing -= DataGridView_EditingControlShowing;
                grid.CellClick += DataGridView_CellClick;
                grid.CellEnter += DataGridView_CellEnter;
                grid.CellContentClick += ParticipantDataGridView_CellContentClick;
                grid.CellPainting += DataGridView_DeleteButtonCellPainting;
                grid.EditingControlShowing += DataGridView_EditingControlShowing;
            }
        }

        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ApplyRtlToGridEditingControl(e.Control);
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StartRtlCellEdit(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void DataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            StartRtlCellEdit(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void StartRtlCellEdit(DataGridView grid, int rowIndex, int columnIndex)
        {
            if (grid == null || rowIndex < 0 || columnIndex < 0 || grid.ReadOnly)
                return;

            DataGridViewCell cell = grid.Rows[rowIndex].Cells[columnIndex];
            if (cell.ReadOnly)
                return;

            if (grid.Columns[columnIndex] is DataGridViewButtonColumn)
                return;

            grid.BeginInvoke((MethodInvoker)delegate
            {
                if (grid.CurrentCell == null || grid.CurrentCell.RowIndex < 0 || grid.CurrentCell.ColumnIndex < 0)
                    return;

                grid.BeginEdit(true);
                ApplyRtlToGridEditingControl(grid.EditingControl);
            });
        }

        private void ApplyRtlToGridEditingControl(Control control)
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.RightToLeft = RightToLeft.Yes;
                comboBox.BackColor = Color.White;
                comboBox.ForeColor = Color.Black;
                return;
            }

            TextBox textBox = control as TextBox;
            if (textBox == null)
                return;

            textBox.RightToLeft = RightToLeft.Yes;
            textBox.BackColor = Color.White;
            textBox.ForeColor = Color.Black;
            textBox.TextAlign = HorizontalAlignment.Left;
            textBox.SelectionStart = 0;
            textBox.SelectionLength = 0;
        }

        private void OtherCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null || checkBox.Tag == null)
                return;

            TextBox textBox = checkBox.Tag as TextBox;
            if (textBox == null)
                return;

            textBox.Visible = checkBox.Checked;
            if (checkBox.Checked)
                textBox.Focus();
        }

        private void countOption15_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            bool showOtherTextBox = countOption15_numericUpDown.Value > 0;
            countOption15_other_textBox.Visible = showOtherTextBox;

            if (!showOtherTextBox)
                countOption15_other_textBox.Clear();
        }

        private void GuildCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (updatingGuildOptions)
                return;

            updatingGuildOptions = true;
            try
            {
                if (sender == guild_checkBox3 && guild_checkBox3.Checked)
                {
                    guild_checkBox1.Checked = false;
                    guild_checkBox2.Checked = false;
                }
                else if ((sender == guild_checkBox1 || sender == guild_checkBox2) &&
                         (guild_checkBox1.Checked || guild_checkBox2.Checked))
                {
                    guild_checkBox3.Checked = false;
                }
            }
            finally
            {
                updatingGuildOptions = false;
            }

            UpdateGuildDetailsVisibility();
        }

        private void UpdateGuildDetailsVisibility()
        {
            bool showGuildName = guild_checkBox1.Checked && !guild_checkBox3.Checked;
            bool showReferences = guild_checkBox2.Checked && !guild_checkBox3.Checked;

            guildName_label.Visible = showGuildName;
            guildName_textBox.Visible = showGuildName;
            guildReferences_label.Visible = showReferences;
            guildReferences_textBox.Visible = showReferences;

            if (!showGuildName)
                guildName_textBox.Clear();

            if (!showReferences)
                guildReferences_textBox.Clear();
        }

        private void Memo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            RichTextBox memo = sender as RichTextBox;
            if (memo == null)
                return;

            int lineIndex = memo.GetLineFromCharIndex(memo.SelectionStart);
            if (lineIndex < 0 || lineIndex >= memo.Lines.Length)
                return;

            string currentLine = memo.Lines[lineIndex];
            Match match = Regex.Match(currentLine, @"^\s*(\d+)([.-])\s*");
            if (!match.Success)
                return;

            int number;
            if (!int.TryParse(match.Groups[1].Value, out number))
                return;

            e.SuppressKeyPress = true;
            memo.SelectedText = Environment.NewLine + (number + 1).ToString() + match.Groups[2].Value + " ";
            memo.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void Memo_Enter(object sender, EventArgs e)
        {
            RichTextBox memo = sender as RichTextBox;
            if (memo != null)
                memo.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforeSave())
                return;

            int meetingId = -1;

            try
            {
                Program.buildConnection();
                using (MySqlTransaction transaction = Program.MyConn.BeginTransaction())
                {
                    LoadProfessionSaveLookups(transaction);
                    meetingId = InsertProfessionMeeting(transaction);
                    SaveSurveyAnswers(transaction, meetingId);
                    SaveMeetingMinutes(transaction, meetingId);
                    transaction.Commit();
                }

                Program.MyConn.Close();

                try
                {
                    Log log = new Log();
                    log.Insert_Log("Insert profession meeting:" + meetingId, "Profession Meeting", Program.UserName, DateTime.Now);
                }
                catch
                {
                }

                MessageBox.Show("تم حفظ لقاء المهن بنجاح");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Program.MyConn.State == ConnectionState.Open)
                    Program.MyConn.Close();
            }
        }

        private bool ValidateBeforeSave()
        {
            if (professionCareer_comboBox.SelectedValue == null ||
                professionCareer_comboBox.SelectedValue.GetType() == typeof(DataRowView))
            {
                MessageBox.Show("الرجاء اختيار المهنة");
                return false;
            }

            if (Program.userID <= 0)
            {
                MessageBox.Show("لا يمكن تحديد المستخدم الحالي، الرجاء تسجيل الدخول من جديد");
                return false;
            }

            if (attendanceCount_numericUpDown.Value > invitedCount_numericUpDown.Value)
            {
                MessageBox.Show("عدد الحضور لا يمكن أن يكون أكبر من عدد المدعوين");
                return false;
            }

            if (targetAudience_other_checkBox.Checked && string.IsNullOrWhiteSpace(targetAudience_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 4");
                return false;
            }

            if (countOption15_numericUpDown.Value > 0 && string.IsNullOrWhiteSpace(countOption15_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 7");
                return false;
            }

            if (marketingOption8_numericUpDown.Value > 0 && string.IsNullOrWhiteSpace(marketing_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 11");
                return false;
            }

            if (guild_checkBox1.Checked && string.IsNullOrWhiteSpace(guildName_textBox.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم النقابة");
                return false;
            }

            if (guild_checkBox2.Checked && string.IsNullOrWhiteSpace(guildReferences_textBox.Text))
            {
                MessageBox.Show("الرجاء إدخال أسماء المرجعيات أو شيوخ الكار");
                return false;
            }

            return true;
        }

        private void LoadProfessionSaveLookups(MySqlTransaction transaction)
        {
            questionIds.Clear();
            optionIds.Clear();

            string query = @"
SELECT ID, Number FROM profession_question;
SELECT o.ID, q.Number, o.Sort_Order
FROM profession_option o
LEFT JOIN profession_question q ON q.ID = o.Question_ID;";

            MySqlCommand command = new MySqlCommand(query, Program.MyConn, transaction);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            foreach (DataRow row in dataSet.Tables[0].Rows)
                questionIds[row["Number"].ToString()] = Convert.ToInt32(row["ID"]);

            foreach (DataRow row in dataSet.Tables[1].Rows)
                optionIds[row["Number"].ToString() + "|" + row["Sort_Order"].ToString()] = Convert.ToInt32(row["ID"]);
        }

        private int InsertProfessionMeeting(MySqlTransaction transaction)
        {
            int careerId = Convert.ToInt32(professionCareer_comboBox.SelectedValue);
            object subCategoryId = DbValue(GetSelectedIntValue(professionSubCategory_comboBox));
            object paperRegistrarId = DbValue(GetUserIdByName(paperRegistrar_comboBox.Text));

            string query = @"
INSERT INTO profession_meeting
    (Career_ID, SubCategory_ID, MeetingDate, Place, InvitedCount, AttendanceCount, AbsenceCount,
     PaperRegistrar_UserID, ProgramRegistrar_UserID, Created_At, Edited_By_UserID, Updated_At)
VALUES
    (@CareerId, @SubCategoryId, @MeetingDate, @Place, @InvitedCount, @AttendanceCount, @AbsenceCount,
     @PaperRegistrarUserId, @ProgramRegistrarUserId, @CreatedAt, @EditedByUserId, @UpdatedAt);";

            using (MySqlCommand command = new MySqlCommand(query, Program.MyConn, transaction))
            {
                command.Parameters.AddWithValue("@CareerId", careerId);
                command.Parameters.AddWithValue("@SubCategoryId", subCategoryId);
                command.Parameters.AddWithValue("@MeetingDate", dateTimePicker1.Value.Date);
                command.Parameters.AddWithValue("@Place", DbString(place_textBox.Text));
                command.Parameters.AddWithValue("@InvitedCount", Convert.ToInt32(invitedCount_numericUpDown.Value));
                command.Parameters.AddWithValue("@AttendanceCount", Convert.ToInt32(attendanceCount_numericUpDown.Value));
                command.Parameters.AddWithValue("@AbsenceCount", Convert.ToInt32(absenceCount_numericUpDown.Value));
                command.Parameters.AddWithValue("@PaperRegistrarUserId", paperRegistrarId);
                command.Parameters.AddWithValue("@ProgramRegistrarUserId", Program.userID);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@EditedByUserId", Program.userID);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = new MySqlCommand("SELECT LAST_INSERT_ID();", Program.MyConn, transaction))
                return Convert.ToInt32(command.ExecuteScalar());
        }

        private void SaveSurveyAnswers(MySqlTransaction transaction, int meetingId)
        {
            SaveOptionNumberAnswers(transaction, meetingId, "1", new NumericUpDown[] { countOption1_numericUpDown, countOption2_numericUpDown, countOption3_numericUpDown });
            SaveOptionNumberAnswers(transaction, meetingId, "2", new NumericUpDown[] { countOption4_numericUpDown, countOption5_numericUpDown, countOption6_numericUpDown });
            SaveOptionNumberAnswers(transaction, meetingId, "3", new NumericUpDown[] { countOption7_numericUpDown, countOption8_numericUpDown, countOption9_numericUpDown });

            SaveCheckedOptions(transaction, meetingId, "4",
                new CheckBox[]
                {
                    targetAudience_checkBox1,
                    targetAudience_checkBox2,
                    targetAudience_checkBox3,
                    targetAudience_checkBox4,
                    targetAudience_checkBox5,
                    targetAudience_checkBox6,
                    targetAudience_checkBox7,
                    targetAudience_checkBox8,
                    targetAudience_checkBox9
                },
                targetAudience_other_checkBox,
                targetAudience_other_textBox,
                10);

            SaveTextAnswer(transaction, meetingId, "5", memo1_richTextBox.Text);
            SaveCompetitionRows(transaction, meetingId);
            SaveOptionNumberAnswers(transaction, meetingId, "7", new NumericUpDown[] { countOption10_numericUpDown, countOption11_numericUpDown, countOption12_numericUpDown, countOption13_numericUpDown, countOption14_numericUpDown, countOption15_numericUpDown }, 6, countOption15_other_textBox);
            SaveSeasonRows(transaction, meetingId);
            SaveGuildOptions(transaction, meetingId);
            SaveBrands(transaction, meetingId);
            SaveOptionNumberAnswers(transaction, meetingId, "11", new NumericUpDown[] { marketingOption1_numericUpDown, marketingOption2_numericUpDown, marketingOption3_numericUpDown, marketingOption4_numericUpDown, marketingOption5_numericUpDown, marketingOption6_numericUpDown, marketingOption7_numericUpDown, marketingOption8_numericUpDown }, 8, marketing_other_textBox);
            SaveEquipmentRows(transaction, meetingId);
            SaveOptionNumberAnswers(transaction, meetingId, "13", new NumericUpDown[] { countOption16_numericUpDown, countOption17_numericUpDown, countOption18_numericUpDown, countOption19_numericUpDown });
            SaveTextAnswer(transaction, meetingId, "14", memo3_richTextBox.Text);
            SaveTextAnswer(transaction, meetingId, "15", memo4_richTextBox.Text);
            SaveChallengeRows(transaction, meetingId);
            SaveOptionNumberAnswers(transaction, meetingId, "17", new NumericUpDown[] { countOption20_numericUpDown, countOption21_numericUpDown, countOption22_numericUpDown, countOption23_numericUpDown });
            SaveTextAnswer(transaction, meetingId, "18", memo6_richTextBox.Text);
            SaveParticipantRows(transaction, meetingId, "19", participant1_dataGridView);
            SaveParticipantRows(transaction, meetingId, "20", participant2_dataGridView);
            SaveParticipantRows(transaction, meetingId, "21", participant3_dataGridView);
            SaveParticipantRows(transaction, meetingId, "22", participant4_dataGridView);
        }

        private void SaveMeetingMinutes(MySqlTransaction transaction, int meetingId)
        {
            SaveAttendeeRows(transaction, meetingId);
            SaveMinuteText(transaction, meetingId, 1, memo7_richTextBox.Text);
            SaveMinuteText(transaction, meetingId, 2, memo8_richTextBox.Text);
            SaveMinuteText(transaction, meetingId, 3, memo9_richTextBox.Text);
            SaveMinuteText(transaction, meetingId, 4, memo10_richTextBox.Text);
            SaveMinuteText(transaction, meetingId, 5, memo11_richTextBox.Text);
        }

        private void SaveTextAnswer(MySqlTransaction transaction, int meetingId, string questionNumber, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            string query = @"
INSERT INTO profession_meeting_answer (Meeting_ID, Question_ID, Value_Text, Value_Number)
VALUES (@MeetingId, @QuestionId, @ValueText, NULL);";

            ExecuteNonQuery(transaction, query,
                Parameter("@MeetingId", meetingId),
                Parameter("@QuestionId", GetQuestionId(questionNumber)),
                Parameter("@ValueText", text.Trim()));
        }

        private void SaveOptionNumberAnswers(MySqlTransaction transaction, int meetingId, string questionNumber, NumericUpDown[] controls)
        {
            SaveOptionNumberAnswers(transaction, meetingId, questionNumber, controls, -1, null);
        }

        private void SaveOptionNumberAnswers(MySqlTransaction transaction, int meetingId, string questionNumber, NumericUpDown[] controls, int otherSortOrder, TextBox otherTextBox)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                int value = Convert.ToInt32(controls[i].Value);
                if (value <= 0)
                    continue;

                int sortOrder = i + 1;
                string otherText = otherSortOrder == sortOrder && otherTextBox != null ? otherTextBox.Text.Trim() : string.Empty;
                InsertOptionAnswer(transaction, meetingId, questionNumber, sortOrder, true, value, otherText);
            }
        }

        private void SaveCheckedOptions(MySqlTransaction transaction, int meetingId, string questionNumber, CheckBox[] checkBoxes, CheckBox otherCheckBox, TextBox otherTextBox, int otherSortOrder)
        {
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (checkBoxes[i].Checked)
                    InsertOptionAnswer(transaction, meetingId, questionNumber, i + 1, true, null, string.Empty);
            }

            if (otherCheckBox.Checked)
                InsertOptionAnswer(transaction, meetingId, questionNumber, otherSortOrder, true, null, otherTextBox.Text.Trim());
        }

        private void SaveGuildOptions(MySqlTransaction transaction, int meetingId)
        {
            if (guild_checkBox1.Checked)
                InsertOptionAnswer(transaction, meetingId, "9", 1, true, null, guildName_textBox.Text.Trim());

            if (guild_checkBox2.Checked)
                InsertOptionAnswer(transaction, meetingId, "9", 2, true, null, guildReferences_textBox.Text.Trim());

            if (guild_checkBox3.Checked)
                InsertOptionAnswer(transaction, meetingId, "9", 3, true, null, string.Empty);
        }

        private void InsertOptionAnswer(MySqlTransaction transaction, int meetingId, string questionNumber, int sortOrder, bool isSelected, int? rankValue, string otherText)
        {
            string query = @"
INSERT INTO profession_meeting_option_answer (Meeting_ID, Option_ID, Is_Selected, Rank_Value, Other_Text)
VALUES (@MeetingId, @OptionId, @IsSelected, @RankValue, @OtherText);";

            ExecuteNonQuery(transaction, query,
                Parameter("@MeetingId", meetingId),
                Parameter("@OptionId", GetOptionId(questionNumber, sortOrder)),
                Parameter("@IsSelected", isSelected ? 1 : 0),
                Parameter("@RankValue", DbValue(rankValue)),
                Parameter("@OtherText", DbString(otherText)));
        }

        private void SaveCompetitionRows(MySqlTransaction transaction, int meetingId)
        {
            SaveCompetitionRow(transaction, meetingId, competitionSevere_label.Text, numericUpDown1, competitionSevereReason_textBox, 1);
            SaveCompetitionRow(transaction, meetingId, competitionMedium_label.Text, numericUpDown2, competitionMediumReason_textBox, 2);
            SaveCompetitionRow(transaction, meetingId, competitionWeak_label.Text, numericUpDown3, competitionWeakReason_textBox, 3);
        }

        private void SaveCompetitionRow(MySqlTransaction transaction, int meetingId, string level, NumericUpDown countControl, TextBox reasonTextBox, int sortOrder)
        {
            string query = @"
INSERT INTO profession_meeting_competition (Meeting_ID, Competition_Level, CountValue, Reason, Sort_Order)
VALUES (@MeetingId, @Level, @CountValue, @Reason, @SortOrder);";

            ExecuteNonQuery(transaction, query,
                Parameter("@MeetingId", meetingId),
                Parameter("@Level", CleanLabelText(level)),
                Parameter("@CountValue", Convert.ToInt32(countControl.Value)),
                Parameter("@Reason", DbString(reasonTextBox.Text)),
                Parameter("@SortOrder", sortOrder));
        }

        private void SaveSeasonRows(MySqlTransaction transaction, int meetingId)
        {
            int sortOrder = 1;
            foreach (DataGridViewRow row in season_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int? countValue = GetNullableIntFromCell(row, 0);
                string fromPeriod = GetCellText(row, 1);
                string toPeriod = GetCellText(row, 2);
                string reason = GetCellText(row, 3);

                if (!countValue.HasValue && string.IsNullOrWhiteSpace(fromPeriod) &&
                    string.IsNullOrWhiteSpace(toPeriod) && string.IsNullOrWhiteSpace(reason))
                    continue;

                string query = @"
INSERT INTO profession_meeting_season (Meeting_ID, Season_Count, From_Period, To_Period, Reason, Sort_Order)
VALUES (@MeetingId, @SeasonCount, @FromPeriod, @ToPeriod, @Reason, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@SeasonCount", DbValue(countValue)),
                    Parameter("@FromPeriod", DbString(fromPeriod)),
                    Parameter("@ToPeriod", DbString(toPeriod)),
                    Parameter("@Reason", DbString(reason)),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveBrands(MySqlTransaction transaction, int meetingId)
        {
            int sortOrder = 1;
            string[] lines = memo2_richTextBox.Lines;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string query = @"
INSERT INTO profession_meeting_brand (Meeting_ID, Brand_Name, Sort_Order)
VALUES (@MeetingId, @BrandName, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@BrandName", line.Trim()),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveEquipmentRows(MySqlTransaction transaction, int meetingId)
        {
            int sortOrder = 1;
            foreach (DataGridViewRow row in equipment_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string specialization = GetCellText(row, 0);
                string equipment = GetCellText(row, 1);

                if (string.IsNullOrWhiteSpace(specialization) && string.IsNullOrWhiteSpace(equipment))
                    continue;

                string query = @"
INSERT INTO profession_meeting_equipment (Meeting_ID, Specialization, Equipment, Sort_Order)
VALUES (@MeetingId, @Specialization, @Equipment, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@Specialization", DbString(specialization)),
                    Parameter("@Equipment", DbString(equipment)),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveChallengeRows(MySqlTransaction transaction, int meetingId)
        {
            int sortOrder = 1;
            foreach (DataGridViewRow row in challenges_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string challenge = GetCellText(row, 0);
                int? challengeCount = GetNullableIntFromCell(row, 1);
                string solution = GetCellText(row, 2);

                if (string.IsNullOrWhiteSpace(challenge) && !challengeCount.HasValue && string.IsNullOrWhiteSpace(solution))
                    continue;

                string query = @"
INSERT INTO profession_meeting_challenge (Meeting_ID, Challenge, Challenge_Count, Suggested_Solution, Sort_Order)
VALUES (@MeetingId, @Challenge, @ChallengeCount, @SuggestedSolution, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@Challenge", DbString(challenge)),
                    Parameter("@ChallengeCount", DbValue(challengeCount)),
                    Parameter("@SuggestedSolution", DbString(solution)),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveParticipantRows(MySqlTransaction transaction, int meetingId, string questionNumber, DataGridView grid)
        {
            int questionId = GetQuestionId(questionNumber);
            int sortOrder = 1;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int? projectId = GetNullableIntFromCell(row, 0);
                int? personId = GetPersonIdFromRow(row, transaction);
                string personName = GetCellText(row, 1);
                string speciality = GetCellText(row, 2);

                if (!projectId.HasValue && !personId.HasValue &&
                    string.IsNullOrWhiteSpace(personName) && string.IsNullOrWhiteSpace(speciality))
                    continue;

                string query = @"
INSERT INTO profession_meeting_participant
    (Meeting_ID, Question_ID, Person_ID, MicroProject_ID, Person_Name, Speciality, Notes, Sort_Order)
VALUES
    (@MeetingId, @QuestionId, @PersonId, @MicroProjectId, @PersonName, @Speciality, NULL, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@QuestionId", questionId),
                    Parameter("@PersonId", DbValue(personId)),
                    Parameter("@MicroProjectId", DbValue(projectId)),
                    Parameter("@PersonName", DbString(personName)),
                    Parameter("@Speciality", DbString(speciality)),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveAttendeeRows(MySqlTransaction transaction, int meetingId)
        {
            int sortOrder = 1;
            foreach (DataGridViewRow row in attendees_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int? projectId = GetNullableIntFromCell(row, 0);
                int? personId = GetPersonIdFromRow(row, transaction);
                string attendeeName = GetCellText(row, 1);
                string speciality = GetCellText(row, 2);

                if (!projectId.HasValue && !personId.HasValue &&
                    string.IsNullOrWhiteSpace(attendeeName) && string.IsNullOrWhiteSpace(speciality))
                    continue;

                string query = @"
INSERT INTO profession_meeting_attendee
    (Meeting_ID, Person_ID, MicroProject_ID, Attendee_Name, Speciality, Sort_Order)
VALUES
    (@MeetingId, @PersonId, @MicroProjectId, @AttendeeName, @Speciality, @SortOrder);";

                ExecuteNonQuery(transaction, query,
                    Parameter("@MeetingId", meetingId),
                    Parameter("@PersonId", DbValue(personId)),
                    Parameter("@MicroProjectId", DbValue(projectId)),
                    Parameter("@AttendeeName", DbString(attendeeName)),
                    Parameter("@Speciality", DbString(speciality)),
                    Parameter("@SortOrder", sortOrder));

                sortOrder++;
            }
        }

        private void SaveMinuteText(MySqlTransaction transaction, int meetingId, int sectionId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            string query = @"
INSERT INTO profession_meeting_minute (Meeting_ID, Section_ID, Notes)
VALUES (@MeetingId, @SectionId, @Notes);";

            ExecuteNonQuery(transaction, query,
                Parameter("@MeetingId", meetingId),
                Parameter("@SectionId", sectionId),
                Parameter("@Notes", text.Trim()));
        }

        private int? GetSelectedIntValue(ComboBox comboBox)
        {
            if (comboBox.SelectedValue == null || comboBox.SelectedValue.GetType() == typeof(DataRowView))
                return null;

            int value;
            if (int.TryParse(comboBox.SelectedValue.ToString(), out value))
                return value;

            return null;
        }

        private int? GetUserIdByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            if (userIdsByName.ContainsKey(userName))
                return userIdsByName[userName];

            return null;
        }

        private int GetQuestionId(string questionNumber)
        {
            if (questionIds.ContainsKey(questionNumber))
                return questionIds[questionNumber];

            throw new InvalidOperationException("لم يتم العثور على السؤال رقم " + questionNumber + " في قاعدة البيانات");
        }

        private int GetOptionId(string questionNumber, int sortOrder)
        {
            string key = questionNumber + "|" + sortOrder;
            if (optionIds.ContainsKey(key))
                return optionIds[key];

            throw new InvalidOperationException("لم يتم العثور على خيار السؤال رقم " + questionNumber + " في قاعدة البيانات");
        }

        private string GetCellText(DataGridViewRow row, int columnIndex)
        {
            if (row.Cells[columnIndex].FormattedValue != null)
                return row.Cells[columnIndex].FormattedValue.ToString().Trim();

            if (row.Cells[columnIndex].Value != null)
                return row.Cells[columnIndex].Value.ToString().Trim();

            return string.Empty;
        }

        private int? GetNullableIntFromCell(DataGridViewRow row, int columnIndex)
        {
            if (row.Cells[columnIndex].Value == null)
                return null;

            int value;
            if (int.TryParse(row.Cells[columnIndex].Value.ToString(), out value))
                return value;

            return null;
        }

        private int? GetPersonIdFromRow(DataGridViewRow row, MySqlTransaction transaction)
        {
            DataGridViewCell cell = row.Cells[1];
            int? personId = null;

            if (cell is DataGridViewComboBoxCell)
            {
                personId = GetPersonIdFromComboBoxCell(cell as DataGridViewComboBoxCell, GetCellText(row, 1));
                if (personId.HasValue && PersonExists(personId.Value, transaction))
                    return personId;
            }
            else if (cell.Tag != null)
            {
                int value;
                if (int.TryParse(cell.Tag.ToString(), out value))
                    personId = value;
            }

            if (personId.HasValue && PersonExists(personId.Value, transaction))
                return personId;

            return null;
        }

        private int? GetPersonIdFromComboBoxCell(DataGridViewComboBoxCell cell, string selectedName)
        {
            if (cell == null)
                return null;

            int value;
            if (cell.Value != null && int.TryParse(cell.Value.ToString(), out value))
                return value;

            DataTable table = cell.DataSource as DataTable;
            if (table == null || string.IsNullOrWhiteSpace(selectedName))
                return null;

            foreach (DataRow row in table.Rows)
            {
                if (row["Name"] != null &&
                    row["Name"].ToString() == selectedName &&
                    row["ID"] != null &&
                    row["ID"] != DBNull.Value &&
                    int.TryParse(row["ID"].ToString(), out value))
                    return value;
            }

            return null;
        }

        private bool PersonExists(int personId, MySqlTransaction transaction)
        {
            string query = "SELECT COUNT(*) FROM person WHERE P_ID = @PersonId;";
            using (MySqlCommand command = new MySqlCommand(query, Program.MyConn, transaction))
            {
                command.Parameters.AddWithValue("@PersonId", personId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private string CleanLabelText(string text)
        {
            return (text ?? string.Empty).Trim().TrimEnd(':', '؟');
        }

        private object DbString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return DBNull.Value;

            return text.Trim();
        }

        private object DbValue(int? value)
        {
            if (value.HasValue)
                return value.Value;

            return DBNull.Value;
        }

        private MySqlParameter Parameter(string name, object value)
        {
            return new MySqlParameter(name, value ?? DBNull.Value);
        }

        private void ExecuteNonQuery(MySqlTransaction transaction, string query, params MySqlParameter[] parameters)
        {
            using (MySqlCommand command = new MySqlCommand(query, Program.MyConn, transaction))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        private void surveyContent_tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void marketing_other_textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
