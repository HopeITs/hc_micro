using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using MySql.Data.MySqlClient;
using System;
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
        private DataSet professionDataSet;
        private DataTable professionProjectsTable;
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
            table.Columns.Add("CareerName", typeof(string));
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
       IFNULL(career.Name, '') AS CareerName
FROM microproject MP
LEFT JOIN career ON MP.Career_ID = career.ID
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
                grid.CellContentClick -= ParticipantDataGridView_CellContentClick;
                grid.DataError -= ParticipantDataGridView_DataError;
                grid.CurrentCellDirtyStateChanged += ParticipantDataGridView_CurrentCellDirtyStateChanged;
                grid.CellValueChanged += ParticipantDataGridView_CellValueChanged;
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
            DataGridView grid = sender as DataGridView;
            if (grid != null && grid.IsCurrentCellDirty)
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void ParticipantDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (loadingProfessionProjects || e.RowIndex < 0 || e.ColumnIndex != 0)
                return;

            FillParticipantProjectRow(sender as DataGridView, e.RowIndex);
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

        private void FillParticipantProjectRow(DataGridView grid, int rowIndex)
        {
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count)
                return;

            DataGridViewRow row = grid.Rows[rowIndex];
            if (row.IsNewRow || row.Cells[0].Value == null)
                return;

            int projectId;
            if (!int.TryParse(row.Cells[0].Value.ToString(), out projectId))
                return;

            DataRow projectRow = FindProjectRow(projectId);
            row.Cells[2].Value = projectRow == null ? string.Empty : projectRow["CareerName"].ToString();

            DataTable beneficiaries = GetProjectBeneficiaries(projectId);
            if (beneficiaries.Rows.Count == 0)
            {
                row.Cells[1] = new DataGridViewTextBoxCell();
                row.Cells[1].Value = string.Empty;
            }
            else if (beneficiaries.Rows.Count == 1)
            {
                row.Cells[1] = new DataGridViewTextBoxCell();
                row.Cells[1].Value = beneficiaries.Rows[0]["Name"].ToString();
            }
            else
            {
                DataGridViewComboBoxCell beneficiaryCell = new DataGridViewComboBoxCell();
                beneficiaryCell.DataSource = beneficiaries;
                beneficiaryCell.DisplayMember = "Name";
                beneficiaryCell.ValueMember = "ID";
                beneficiaryCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                beneficiaryCell.FlatStyle = FlatStyle.Flat;
                row.Cells[1] = beneficiaryCell;
                row.Cells[1].Value = beneficiaries.Rows[0]["ID"];
            }
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
                grid.DefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.DefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid.RowsDefaultCellStyle.SelectionBackColor = selectionBackColor;
                grid.RowsDefaultCellStyle.SelectionForeColor = selectionForeColor;
                grid.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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
                return;
            }

            TextBox textBox = control as TextBox;
            if (textBox == null)
                return;

            textBox.RightToLeft = RightToLeft.Yes;
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

        private void surveyContent_tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void marketing_other_textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
