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
        private int currentMeetingId = -1;
        private bool updateMode;
        private bool userMode;
        private bool professionMeeting_IsDirty;
        private bool updatingGuildOptions;
        private bool loadingProfessionProjects;
        private bool fillingParticipantProjectRow;
        private bool refreshingParticipantOptions;
        private bool professionMeetingDeleted;
        private DataSet professionDataSet;
        private DataTable professionProjectsTable;
        private readonly Dictionary<string, int> questionIds = new Dictionary<string, int>();
        private readonly Dictionary<string, int> optionIds = new Dictionary<string, int>();
        private readonly Dictionary<string, int> userIdsByName = new Dictionary<string, int>();
        private readonly Dictionary<int, DataTable> projectBeneficiaries = new Dictionary<int, DataTable>();

        public ProfessionMeetings_Form()
            : this(null)
        {
        }

        public ProfessionMeetings_Form(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            PopulateDefaultChallengeRows();
            ConfigureDataGridViews();
            BindProfessionMeetingDirtyEvents();
            BindAbsenceCountCalculation();
        }

        public ProfessionMeetings_Form(MainForm mainForm, int meetingId)
            : this(mainForm)
        {
            currentMeetingId = meetingId;
            updateMode = meetingId > 0;
        }

        private void ProfessionMeetings_Form_Load(object sender, EventArgs e)
        {
            bool loadedSuccessfully = false;
            try
            {
                userMode = false;
                if (mainForm != null)
                {
                    mainForm.Project_label.Visible = mainForm.TabName_label.Visible = true;
                    mainForm.ProjectNumber_label.Visible = mainForm.MP_ID_label.Visible = false;
                    mainForm.TabName_label.Text = "Profession Meetings";
                }

                ApplyTheme();
                PopulateProfessionComboBox();
                UpdateGuildDetailsVisibility();
                ConfigureParticipantProjectGrids();
                ConfigureDataGridViews();
                PopulateRegistrarFields();
                if (updateMode)
                    LoadProfessionMeeting(currentMeetingId);

                ResetProfessionMeetingDirty();
                BeginInvoke((MethodInvoker)delegate
                {
                    ResetInitialScrollPosition();
                    UpdateDeleteButtonVisibility();
                    ResetProfessionMeetingDirty();
                    userMode = true;
                });
                loadedSuccessfully = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (!loadedSuccessfully)
                    userMode = true;
            }
        }

        private void ProfessionMeetings_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (professionMeetingDeleted)
                        return;

                    CommitProfessionMeetingGridEdits();

                    if (professionMeeting_IsDirty)
                    {
                        if (MessageBox.Show("يوجد بعض التعديلات غير المحفوظة، هل تود حفظها قبل إغلاق الصفحة؟؟", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            this.ActiveControl = Save_button;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL")) { MessageBox.Show("please check your network and try again", "Error"); } else if (ex.Message.Contains("host")) { MessageBox.Show("please check the server or your network connection and try again", "Error"); } else { MessageBox.Show(ex.Message, "Error"); }
            }
        }

        public void Check_Theme()
        {
            ApplyTheme();
            ConfigureDataGridViews();
        }

        private void ResetInitialScrollPosition()
        {
            content_tabControl.SelectedIndex = 0;
            content_tabControl.Focus();

            survey_scrollPanel.AutoScrollPosition = Point.Empty;
            minutes_scrollPanel.AutoScrollPosition = Point.Empty;
        }

        private void BindProfessionMeetingDirtyEvents()
        {
            BindProfessionMeetingDirtyEvents(this);
        }

        private void BindProfessionMeetingDirtyEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBoxBase)
                {
                    ((TextBoxBase)control).TextChanged += ProfessionMeetingControl_Changed;
                    if (control is TextBox)
                        control.Enter += TextBox_SelectAllOnEnter;
                }
                else if (control is ComboBox)
                    ((ComboBox)control).SelectedIndexChanged += ProfessionMeetingControl_Changed;
                else if (control is DateTimePicker)
                    ((DateTimePicker)control).ValueChanged += ProfessionMeetingControl_Changed;
                else if (control is NumericUpDown)
                    ((NumericUpDown)control).ValueChanged += ProfessionMeetingControl_Changed;
                else if (control is CheckBox)
                    ((CheckBox)control).CheckedChanged += ProfessionMeetingControl_Changed;

                if (control is DataGridView)
                {
                    DataGridView grid = control as DataGridView;
                    grid.CellValueChanged += ProfessionMeetingGrid_CellValueChanged;
                    grid.RowsAdded += ProfessionMeetingGrid_RowsAdded;
                    grid.RowsRemoved += ProfessionMeetingGrid_RowsRemoved;
                    grid.UserDeletedRow += ProfessionMeetingGrid_UserDeletedRow;
                }

                if (control.HasChildren)
                    BindProfessionMeetingDirtyEvents(control);
            }
        }

        private void TextBox_SelectAllOnEnter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
                BeginInvoke((MethodInvoker)delegate { textBox.SelectAll(); });
        }

        private void BindAbsenceCountCalculation()
        {
            invitedCount_numericUpDown.ValueChanged -= AttendanceCount_ValueChanged;
            attendanceCount_numericUpDown.ValueChanged -= AttendanceCount_ValueChanged;
            invitedCount_numericUpDown.ValueChanged += AttendanceCount_ValueChanged;
            attendanceCount_numericUpDown.ValueChanged += AttendanceCount_ValueChanged;
        }

        private void AttendanceCount_ValueChanged(object sender, EventArgs e)
        {
            RecalculateAbsenceCount();
        }

        private void RecalculateAbsenceCount()
        {
            if (!userMode)
                return;

            decimal absenceCount = invitedCount_numericUpDown.Value - attendanceCount_numericUpDown.Value;
            if (absenceCount < absenceCount_numericUpDown.Minimum)
                absenceCount = absenceCount_numericUpDown.Minimum;
            if (absenceCount > absenceCount_numericUpDown.Maximum)
                absenceCount = absenceCount_numericUpDown.Maximum;

            if (absenceCount_numericUpDown.Value != absenceCount)
                absenceCount_numericUpDown.Value = absenceCount;
        }

        private void ResetProfessionMeetingDirty()
        {
            professionMeeting_IsDirty = false;
        }

        private void SetProfessionMeetingDirty()
        {
            if (refreshingParticipantOptions)
                return;

            if (userMode)
                professionMeeting_IsDirty = true;
        }

        private void ProfessionMeetingControl_Changed(object sender, EventArgs e)
        {
            SetProfessionMeetingDirty();
        }

        private void ProfessionMeetingGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                SetProfessionMeetingDirty();
        }

        private void ProfessionMeetingGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SetProfessionMeetingDirty();
        }

        private void ProfessionMeetingGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetProfessionMeetingDirty();
        }

        private void ProfessionMeetingGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            SetProfessionMeetingDirty();
        }

        private void CommitProfessionMeetingGridEdits()
        {
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
                if (grid == null)
                    continue;

                if (grid.IsCurrentCellDirty)
                    grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

                grid.EndEdit();
            }
        }

        private bool HasCurrentDirtyGridCell()
        {
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
                if (grid != null && grid.IsCurrentCellDirty)
                    return true;
            }

            return false;
        }

        private void ApplyTheme()
        {
            try
            {
                NewTheme newTheme = new NewTheme();
                if (Settings.Default.theme == "Dark")
                    newTheme.ApplicationNew_ToNight(this);
                else
                    newTheme.ApplicationNew_ToLight(this);

                ApplyApplicationNewStyle();
            }
            catch
            {
                ApplyApplicationNewStyle();
            }
        }

        private void ApplyApplicationNewStyle()
        {
            bool lightTheme = Settings.Default.theme == "Light";
            Color formBackColor = lightTheme ? NewTheme.light_grey1 : NewTheme.light_grey3;
            Color textColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
            Color inputBackColor = lightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
            Color inputForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;

            BackColor = formBackColor;
            ForeColor = textColor;
            ApplyApplicationNewStyleToControl(mainLayoutPanel, formBackColor, textColor, inputBackColor, inputForeColor);

            title_label.ForeColor = lightTheme ? NewTheme.light_red : NewTheme.light_white;

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
                headerLabel.BackColor = lightTheme ? NewTheme.light_red : NewTheme.light_grey2;
                headerLabel.ForeColor = lightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
            }

            Save_button.BackgroundImage = lightTheme ? Resources.Save2_D : Resources.Save2_L;
            Save_button.ForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
            DeleteMeeting_button.BackgroundImage = lightTheme ? Resources.Delete2_D : Resources.Delete2_L;
            DeleteMeeting_button.ForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
            UpdateDeleteButtonVisibility();
        }

        private void ApplyApplicationNewStyleToControl(Control control, Color backColor, Color foreColor, Color inputBackColor, Color inputForeColor)
        {
            if (control == null)
                return;

            if (control is TabControl)
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;
            }
            else if (control is Panel || control is TableLayoutPanel || control is FlowLayoutPanel || control is TabPage)
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;
            }
            else if (control is Label || control is CheckBox || control is RadioButton)
            {
                control.ForeColor = foreColor;
                if (control.BackColor != NewTheme.light_red && control.BackColor != NewTheme.light_grey2)
                    control.BackColor = Color.Transparent;
            }
            else if (control is Button)
            {
                control.ForeColor = foreColor;
            }
            else if (control is TextBox || control is ComboBox || control is RichTextBox || control is NumericUpDown)
            {
                control.BackColor = inputBackColor;
                control.ForeColor = inputForeColor;
            }
            else if (control is DateTimePicker)
            {
                DateTimePicker dateTimePicker = control as DateTimePicker;
                dateTimePicker.BackColor = inputBackColor;
                dateTimePicker.ForeColor = inputForeColor;
                dateTimePicker.CalendarForeColor = inputForeColor;
                dateTimePicker.CalendarMonthBackground = inputBackColor;
            }
            else if (control is DataGridView)
            {
                ApplyApplicationNewDataGridViewStyle(control as DataGridView);
            }

            foreach (Control child in control.Controls)
                ApplyApplicationNewStyleToControl(child, backColor, foreColor, inputBackColor, inputForeColor);
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
            bool lightTheme = Settings.Default.theme == "Light";
            Color backColor = selected
                ? (lightTheme ? NewTheme.light_grey2 : NewTheme.light_grey3)
                : comboBox.BackColor;
            Color foreColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;

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

        private DataTable CreateBeneficiaryLookupTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
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
                projectBeneficiaries.Clear();
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
                LoadProfessionBeneficiaries(professionId);
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

        private void LoadProfessionBeneficiaries(int professionId)
        {
            projectBeneficiaries.Clear();

            if (professionProjectsTable != null)
            {
                foreach (DataRow row in professionProjectsTable.Rows)
                {
                    int emptyProjectId;
                    if (int.TryParse(GetStringValue(row, "ProjectId"), out emptyProjectId))
                        projectBeneficiaries[emptyProjectId] = CreateBeneficiaryLookupTable();
                }
            }

            Program.buildConnection();
            try
            {
                string query = @"
                        SELECT person_microproject.MicroProject_ID AS ProjectId,
                               person.P_ID AS ID,
                               CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) AS Name
                        FROM person_microproject
                        LEFT JOIN person ON person.P_ID = person_microproject.Person_ID
                        LEFT JOIN microproject MP ON MP.MP_ID = person_microproject.MicroProject_ID
                        WHERE MP.Career_ID = @CareerId
                        ORDER BY Name ASC";

                MySqlCommand command = new MySqlCommand(query, Program.MyConn);
                command.Parameters.AddWithValue("@CareerId", professionId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    int projectId;
                    int personId;
                    if (!int.TryParse(GetStringValue(row, "ProjectId"), out projectId) ||
                        !int.TryParse(GetStringValue(row, "ID"), out personId))
                        continue;

                    if (!projectBeneficiaries.ContainsKey(projectId))
                        projectBeneficiaries[projectId] = CreateBeneficiaryLookupTable();

                    projectBeneficiaries[projectId].Rows.Add(personId, GetStringValue(row, "Name"));
                }
            }
            finally
            {
                Program.MyConn.Close();
            }
        }

        private DataTable GetProjectBeneficiaries(int projectId)
        {
            if (projectBeneficiaries.ContainsKey(projectId))
                return projectBeneficiaries[projectId];

            DataTable beneficiaries = LoadProjectBeneficiaries(projectId);
            projectBeneficiaries[projectId] = beneficiaries;
            return beneficiaries;
        }

        private DataTable LoadProjectBeneficiaries(int projectId)
        {
            DataTable beneficiaries = CreateBeneficiaryLookupTable();
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

                foreach (DataRow row in table.Rows)
                {
                    int personId;
                    if (!int.TryParse(GetStringValue(row, "ID"), out personId))
                        continue;

                    beneficiaries.Rows.Add(personId, GetStringValue(row, "Name"));
                }

                return beneficiaries;
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
                grid.CellBeginEdit -= ParticipantDataGridView_CellBeginEdit;
                grid.CellEndEdit -= ParticipantDataGridView_CellEndEdit;
                grid.EditingControlShowing -= ParticipantDataGridView_EditingControlShowing;
                grid.CellContentClick -= ParticipantDataGridView_CellContentClick;
                grid.CellParsing -= ParticipantDataGridView_CellParsing;
                grid.DataError -= ParticipantDataGridView_DataError;
                grid.CurrentCellDirtyStateChanged += ParticipantDataGridView_CurrentCellDirtyStateChanged;
                grid.CellValueChanged += ParticipantDataGridView_CellValueChanged;
                grid.CellBeginEdit += ParticipantDataGridView_CellBeginEdit;
                grid.CellEndEdit += ParticipantDataGridView_CellEndEdit;
                grid.EditingControlShowing += ParticipantDataGridView_EditingControlShowing;
                grid.CellContentClick += ParticipantDataGridView_CellContentClick;
                grid.CellParsing += ParticipantDataGridView_CellParsing;
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

                object[] projectItems = GetProjectIdItems(projects);

                projectColumn.DataSource = null;
                projectColumn.DisplayMember = string.Empty;
                projectColumn.ValueMember = string.Empty;
                projectColumn.Items.Clear();
                projectColumn.Items.AddRange(projectItems);
                projectColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                projectColumn.FlatStyle = FlatStyle.Flat;
                ApplyApplicationNewGridCellStyle(projectColumn.DefaultCellStyle);

                foreach (DataGridViewRow row in grid.Rows)
                    SetParticipantProjectCellItems(row.Cells[0] as DataGridViewComboBoxCell, projectItems);
            }
        }

        private object[] GetProjectIdItems(DataTable projects)
        {
            List<object> projectItems = new List<object>();
            if (projects == null)
                return projectItems.ToArray();

            foreach (DataRow row in projects.Rows)
            {
                int projectId;
                if (!int.TryParse(GetStringValue(row, "ProjectId"), out projectId))
                    continue;

                if (!projectItems.Contains(projectId))
                    projectItems.Add(projectId);
            }

            return projectItems.ToArray();
        }

        private void SetParticipantProjectCellItems(DataGridViewComboBoxCell projectCell, object[] projectItems)
        {
            if (projectCell == null)
                return;

            object currentValue = NormalizeProjectValue(projectItems, projectCell.Value);

            projectCell.DataSource = null;
            projectCell.DisplayMember = string.Empty;
            projectCell.ValueMember = string.Empty;
            projectCell.Items.Clear();
            projectCell.Items.AddRange(projectItems);
            projectCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            projectCell.FlatStyle = FlatStyle.Flat;
            ApplyApplicationNewGridCellStyle(projectCell.Style);

            if (!Equals(projectCell.Value, currentValue))
                projectCell.Value = currentValue;
        }

        private object NormalizeProjectValue(object[] projectItems, object value)
        {
            if (value == null || projectItems == null)
                return null;

            int projectId;
            if (!int.TryParse(value.ToString(), out projectId))
                return null;

            foreach (object projectItem in projectItems)
            {
                if (projectItem != null && projectItem.ToString() == projectId.ToString())
                    return projectId;
            }

            return null;
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

                    row.Tag = null;
                    row.Cells[0].Value = null;
                    row.Cells[1] = new DataGridViewTextBoxCell();
                    row.Cells[1].Value = string.Empty;
                    row.Cells[2].Value = string.Empty;
                }
            }
        }

        private void ParticipantDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (fillingParticipantProjectRow || refreshingParticipantOptions)
                return;

            DataGridView grid = sender as DataGridView;
            if (grid != null && grid.IsCurrentCellDirty && grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == 0)
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void ParticipantDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateParticipantProjectRowFromCell(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void ParticipantDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateParticipantProjectRowFromCell(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void ParticipantDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (fillingParticipantProjectRow || refreshingParticipantOptions)
                return;

            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count)
                return;

            if (e.ColumnIndex == 0)
                RefreshParticipantProjectCellOptions(grid, e.RowIndex);
            else if (e.ColumnIndex == 1)
                RefreshParticipantPersonCellOptions(grid, e.RowIndex);
        }

        private void RefreshParticipantProjectCellOptions(DataGridView grid, int rowIndex)
        {
            refreshingParticipantOptions = true;
            try
            {
                DataGridViewRow row = grid.Rows[rowIndex];
                DataGridViewComboBoxCell projectCell = row.Cells[0] as DataGridViewComboBoxCell;
                if (projectCell == null)
                {
                    object currentValue = row.Cells[0].Value;
                    projectCell = new DataGridViewComboBoxCell();
                    row.Cells[0] = projectCell;
                    projectCell.Value = currentValue;
                }

                SetParticipantProjectCellItems(projectCell, GetAvailableProjectItems(grid, rowIndex));
            }
            catch
            {
            }
            finally
            {
                refreshingParticipantOptions = false;
            }
        }

        private void RefreshParticipantPersonCellOptions(DataGridView grid, int rowIndex)
        {
            DataGridViewRow row = grid.Rows[rowIndex];
            DataGridViewComboBoxCell personCell = row.Cells[1] as DataGridViewComboBoxCell;
            if (personCell == null)
                return;

            int projectId;
            if (row.Cells[0].Value == null || !int.TryParse(row.Cells[0].Value.ToString(), out projectId))
                return;

            refreshingParticipantOptions = true;
            try
            {
                DataTable beneficiaries = GetAvailableBeneficiaries(grid, rowIndex, projectId, GetParticipantPersonKey(row));
                object currentValue = NormalizeBeneficiaryValue(beneficiaries, personCell.Value);

                personCell.DataSource = beneficiaries;
                personCell.DisplayMember = "Name";
                personCell.ValueMember = "ID";
                personCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                personCell.FlatStyle = FlatStyle.Flat;
                ApplyApplicationNewGridCellStyle(personCell.Style);

                if (!Equals(personCell.Value, currentValue))
                    personCell.Value = currentValue;
            }
            catch
            {
            }
            finally
            {
                refreshingParticipantOptions = false;
            }
        }

        private object NormalizeBeneficiaryValue(DataTable beneficiaries, object value)
        {
            if (beneficiaries == null || value == null)
                return null;

            int personId;
            if (!int.TryParse(value.ToString(), out personId))
                return null;

            foreach (DataRow row in beneficiaries.Rows)
            {
                if (GetStringValue(row, "ID") == personId.ToString())
                    return personId;
            }

            return null;
        }

        private object[] GetAvailableProjectItems(DataGridView grid, int rowIndex)
        {
            List<object> availableProjects = new List<object>();
            if (professionProjectsTable == null)
                return availableProjects.ToArray();

            string currentProjectId = GetParticipantCellKey(grid.Rows[rowIndex].Cells[0]);
            List<string> usedProjectIds = GetUsedParticipantProjectIds(grid, rowIndex);
            List<string> usedPersonIds = GetUsedParticipantPersonIds(grid, rowIndex);

            foreach (DataRow row in professionProjectsTable.Rows)
            {
                string projectKey = GetStringValue(row, "ProjectId");
                if (projectKey != currentProjectId && !HasAvailableBeneficiary(projectKey, usedProjectIds, usedPersonIds))
                    continue;

                int projectId;
                if (!int.TryParse(projectKey, out projectId) || availableProjects.Contains(projectId))
                    continue;

                availableProjects.Add(projectId);
            }

            return availableProjects.ToArray();
        }

        private bool HasAvailableBeneficiary(string projectKey, List<string> usedProjectIds, List<string> usedPersonIds)
        {
            int projectId;
            if (!int.TryParse(projectKey, out projectId))
                return false;

            DataTable beneficiaries = GetProjectBeneficiaries(projectId);
            if (beneficiaries == null || beneficiaries.Rows.Count == 0)
                return !usedProjectIds.Contains(projectKey);

            foreach (DataRow row in beneficiaries.Rows)
            {
                string personId = GetStringValue(row, "ID");
                if (string.IsNullOrWhiteSpace(personId))
                    continue;

                if (!usedPersonIds.Contains(personId))
                    return true;
            }

            return false;
        }

        private DataTable GetAvailableBeneficiaries(DataGridView grid, int rowIndex, int projectId, string currentPersonId)
        {
            DataTable beneficiaries = GetProjectBeneficiaries(projectId);
            DataTable availableBeneficiaries = beneficiaries.Clone();
            List<string> usedPersonIds = GetUsedParticipantPersonIds(grid, rowIndex);

            foreach (DataRow row in beneficiaries.Rows)
            {
                string personId = GetStringValue(row, "ID");
                if (usedPersonIds.Contains(personId) && personId != currentPersonId)
                    continue;

                availableBeneficiaries.ImportRow(row);
            }

            return availableBeneficiaries;
        }

        private List<string> GetUsedParticipantProjectIds(DataGridView grid, int excludedRowIndex)
        {
            List<string> usedProjectIds = new List<string>();

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (i == excludedRowIndex || grid.Rows[i].IsNewRow)
                    continue;

                string projectId = GetParticipantCellKey(grid.Rows[i].Cells[0]);
                if (!string.IsNullOrWhiteSpace(projectId) && !usedProjectIds.Contains(projectId))
                    usedProjectIds.Add(projectId);
            }

            return usedProjectIds;
        }

        private List<string> GetUsedParticipantPersonIds(DataGridView grid, int excludedRowIndex)
        {
            List<string> usedPersonIds = new List<string>();

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (i == excludedRowIndex || grid.Rows[i].IsNewRow)
                    continue;

                string personId = GetParticipantPersonKey(grid.Rows[i]);
                if (!string.IsNullOrWhiteSpace(personId) && !usedPersonIds.Contains(personId))
                    usedPersonIds.Add(personId);
            }

            return usedPersonIds;
        }

        private string GetParticipantCellKey(DataGridViewCell cell)
        {
            if (cell == null || cell.Value == null)
                return string.Empty;

            return cell.Value.ToString().Trim();
        }

        private string GetParticipantPersonKey(DataGridViewRow row)
        {
            if (row == null)
                return string.Empty;

            DataGridViewCell personCell = row.Cells[1];
            if (personCell is DataGridViewComboBoxCell)
                return GetParticipantCellKey(personCell);

            if (personCell.Tag != null)
                return personCell.Tag.ToString().Trim();

            return string.Empty;
        }

        private void ParticipantDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewComboBoxEditingControl comboBox = e.Control as DataGridViewComboBoxEditingControl;
            if (comboBox == null)
                return;

            DataGridView grid = comboBox.EditingControlDataGridView;

            comboBox.SelectionChangeCommitted -= ParticipantProjectComboBox_SelectionChangeCommitted;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.AutoCompleteMode = AutoCompleteMode.None;
            comboBox.AutoCompleteSource = AutoCompleteSource.None;

            if (grid != null && grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == 0)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            }

            comboBox.SelectionChangeCommitted += ParticipantProjectComboBox_SelectionChangeCommitted;
        }

        private void ParticipantProjectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataGridViewComboBoxEditingControl comboBox = sender as DataGridViewComboBoxEditingControl;
            DataGridView grid = comboBox == null ? null : comboBox.EditingControlDataGridView;
            if (grid == null || grid.CurrentCell == null)
                return;

            int columnIndex = grid.CurrentCell.ColumnIndex;
            if (columnIndex != 0 && columnIndex != 1)
                return;

            int rowIndex = grid.CurrentCell.RowIndex;
            object selectedValue = comboBox.SelectedValue ?? comboBox.SelectedItem;

            comboBox.EditingControlValueChanged = true;
            grid.NotifyCurrentCellDirty(true);
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (columnIndex == 0)
                QueueFillParticipantProjectRow(grid, rowIndex, selectedValue);
        }

        private void ParticipantDataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count || e.ColumnIndex != 0)
                return;

            DataGridViewComboBoxCell projectCell = grid.Rows[e.RowIndex].Cells[0] as DataGridViewComboBoxCell;
            if (projectCell == null)
                return;

            string typedText = e.Value == null ? string.Empty : e.Value.ToString().Trim();
            if (string.IsNullOrWhiteSpace(typedText))
            {
                e.Value = null;
                e.ParsingApplied = true;
                return;
            }

            object matchedItem = FindProjectItem(projectCell, typedText);

            e.Value = matchedItem == null ? projectCell.Value : matchedItem;
            e.ParsingApplied = true;
        }

        private object FindProjectItem(DataGridViewComboBoxCell projectCell, string typedText)
        {
            if (projectCell == null || string.IsNullOrWhiteSpace(typedText))
                return null;

            foreach (object item in projectCell.Items)
            {
                if (item != null && item.ToString() == typedText)
                    return item;
            }

            object prefixMatch = null;
            foreach (object item in projectCell.Items)
            {
                if (item == null || !item.ToString().StartsWith(typedText))
                    continue;

                if (prefixMatch != null)
                    return null;

                prefixMatch = item;
            }

            return prefixMatch;
        }

        private void UpdateParticipantProjectRowFromCell(DataGridView grid, int rowIndex, int columnIndex)
        {
            if (fillingParticipantProjectRow || refreshingParticipantOptions || grid == null || rowIndex < 0 || columnIndex != 0)
                return;

            if (rowIndex >= grid.Rows.Count)
                return;

            object selectedProjectId = grid.Rows[rowIndex].Cells[0].Value;
            if (selectedProjectId == null || string.IsNullOrWhiteSpace(selectedProjectId.ToString()))
            {
                if (!grid.Rows[rowIndex].IsNewRow)
                    ResetParticipantDependentCells(grid.Rows[rowIndex]);

                return;
            }

            QueueFillParticipantProjectRow(grid, rowIndex, selectedProjectId);
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
            FillParticipantProjectRow(grid, rowIndex, selectedProjectId, null);
        }

        private void FillParticipantProjectRow(DataGridView grid, int rowIndex, object selectedProjectId, int? preferredPersonId)
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

                int projectId;
                if (row.Cells[0].Value == null ||
                    !int.TryParse(row.Cells[0].Value.ToString(), out projectId))
                {
                    ResetParticipantDependentCells(row);
                    return;
                }

                if (!preferredPersonId.HasValue && IsParticipantRowFilledFor(row, projectId))
                    return;

                ResetParticipantDependentCells(row);

                DataRow projectRow = FindProjectRow(projectId);
                row.Cells[2].Value = projectRow == null ? string.Empty : projectRow["ProjectSpeciality"].ToString();

                string preferredPersonKey = preferredPersonId.HasValue ? preferredPersonId.Value.ToString() : string.Empty;
                DataTable beneficiaries = GetAvailableBeneficiaries(grid, rowIndex, projectId, preferredPersonKey);
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
                    ApplyApplicationNewGridCellStyle(beneficiaryCell.Style);
                    row.Cells[1] = beneficiaryCell;
                    row.Cells[1].Value = FindBeneficiaryValue(beneficiaries, preferredPersonKey);
                    row.Cells[1].Tag = null;
                }

                row.Tag = projectId;
                ApplyApplicationNewGridCellStyle(row.Cells[0].Style);
                ApplyApplicationNewGridCellStyle(row.Cells[2].Style);
                grid.InvalidateRow(rowIndex);
            }
            finally
            {
                fillingParticipantProjectRow = false;
            }
        }

        private object FindBeneficiaryValue(DataTable beneficiaries, string preferredPersonId)
        {
            if (beneficiaries == null || beneficiaries.Rows.Count == 0)
                return null;

            if (!string.IsNullOrWhiteSpace(preferredPersonId))
            {
                object preferredValue = NormalizeBeneficiaryValue(beneficiaries, preferredPersonId);
                if (preferredValue != null)
                    return preferredValue;
            }

            return NormalizeBeneficiaryValue(beneficiaries, GetStringValue(beneficiaries.Rows[0], "ID"));
        }

        private void QueueFillParticipantProjectRow(DataGridView grid, int rowIndex, object selectedProjectId)
        {
            if (grid == null || rowIndex < 0 || grid.IsDisposed || !grid.IsHandleCreated)
                return;

            grid.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    if (grid.IsDisposed || rowIndex >= grid.Rows.Count)
                        return;

                    bool resumeCellEdit = IsEditingParticipantDependentCell(grid, rowIndex);
                    if (resumeCellEdit)
                        grid.EndEdit();

                    FillParticipantProjectRow(grid, rowIndex, selectedProjectId);

                    if (resumeCellEdit && IsCurrentParticipantDependentCell(grid, rowIndex))
                        grid.BeginEdit(true);
                }
                catch
                {
                }
            });
        }

        private bool IsCurrentParticipantDependentCell(DataGridView grid, int rowIndex)
        {
            if (grid == null || grid.CurrentCell == null || grid.CurrentCell.RowIndex != rowIndex)
                return false;

            return grid.CurrentCell.ColumnIndex == 1 || grid.CurrentCell.ColumnIndex == 2;
        }

        private bool IsEditingParticipantDependentCell(DataGridView grid, int rowIndex)
        {
            return IsCurrentParticipantDependentCell(grid, rowIndex) && grid.IsCurrentCellInEditMode;
        }

        private bool IsParticipantRowFilledFor(DataGridViewRow row, int projectId)
        {
            return row != null && row.Tag != null && row.Tag.ToString() == projectId.ToString();
        }

        private void ResetParticipantDependentCells(DataGridViewRow row)
        {
            row.Tag = null;
            row.Cells[1] = new DataGridViewTextBoxCell();
            row.Cells[1].Value = string.Empty;
            row.Cells[1].Tag = null;
            ApplyApplicationNewGridCellStyle(row.Cells[1].Style);

            row.Cells[2].Value = string.Empty;
            ApplyApplicationNewGridCellStyle(row.Cells[2].Style);
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

        private void ConfigureDataGridViews()
        {
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
                grid.EnableHeadersVisualStyles = false;
                ApplyApplicationNewDataGridViewStyle(grid);

                foreach (DataGridViewColumn column in grid.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    ApplyApplicationNewGridCellStyle(column.DefaultCellStyle);
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    ApplyApplicationNewGridCellStyle(column.HeaderCell.Style);
                }

                grid.CellClick -= dataGridView_CellClick;
                grid.CellEnter -= dataGridView_CellEnter;
                grid.CellContentClick -= ParticipantDataGridView_CellContentClick;
                grid.CellPainting -= DataGridView_DeleteButtonCellPainting;
                grid.EditingControlShowing -= dataGridView_EditingControlShowing;
                grid.CellClick += dataGridView_CellClick;
                grid.CellEnter += dataGridView_CellEnter;
                grid.CellContentClick += ParticipantDataGridView_CellContentClick;
                grid.CellPainting += DataGridView_DeleteButtonCellPainting;
                grid.EditingControlShowing += dataGridView_EditingControlShowing;
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ApplyRtlToGridEditingControl(e.Control);
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StartRtlCellEdit(sender as DataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
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
                bool lightTheme = Settings.Default.theme == "Light";
                if (comboBox.RightToLeft != RightToLeft.Yes)
                    comboBox.RightToLeft = RightToLeft.Yes;

                comboBox.BackColor = lightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
                comboBox.ForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
                return;
            }

            TextBox textBox = control as TextBox;
            if (textBox == null)
                return;

            bool isLightTheme = Settings.Default.theme == "Light";
            textBox.RightToLeft = RightToLeft.Yes;
            textBox.BackColor = isLightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
            textBox.ForeColor = isLightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
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

        private void SetProfessionMeetingUpdateMode(int meetingId)
        {
            currentMeetingId = meetingId;
            updateMode = meetingId > 0;
            UpdateDeleteButtonVisibility();
        }

        private void UpdateDeleteButtonVisibility()
        {
            if (DeleteMeeting_button != null)
                DeleteMeeting_button.Visible = updateMode && currentMeetingId > 0;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            bool suppressGridCommitDirty = updateMode && !professionMeeting_IsDirty && !HasCurrentDirtyGridCell();
            bool previousUserMode = userMode;
            if (suppressGridCommitDirty)
                userMode = false;

            try
            {
                CommitProfessionMeetingGridEdits();
            }
            finally
            {
                if (suppressGridCommitDirty)
                {
                    ResetProfessionMeetingDirty();
                    userMode = previousUserMode;
                }
            }

            if (updateMode && !professionMeeting_IsDirty)
            {
                MessageBox.Show("لا يوجد أي تغييرات لحفظها ", "Error");
                return;
            }

            if (!ValidateBeforeSave())
                return;

            bool wasUpdate = updateMode;
            int meetingId = updateMode ? currentMeetingId : -1;

            try
            {
                Program.buildConnection();
                using (MySqlTransaction transaction = Program.MyConn.BeginTransaction())
                {
                    LoadProfessionSaveLookups(transaction);
                    if (updateMode)
                    {
                        UpdateProfessionMeeting(transaction, meetingId);
                        DeleteProfessionMeetingDetails(transaction, meetingId);
                    }
                    else
                    {
                        meetingId = InsertProfessionMeeting(transaction);
                        SetProfessionMeetingUpdateMode(meetingId);
                    }

                    SaveSurveyAnswers(transaction, meetingId);
                    SaveMeetingMinutes(transaction, meetingId);
                    transaction.Commit();
                }

                Program.MyConn.Close();
                UpdateDeleteButtonVisibility();
                ResetProfessionMeetingDirty();

                try
                {
                    Log log = new Log();
                    log.Insert_Log((wasUpdate ? "Update profession meeting:" : "Insert profession meeting:") + meetingId, "Profession Meeting", Program.UserName, DateTime.Now);
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

        private void DeleteMeeting_button_Click(object sender, EventArgs e)
        {
            if (!updateMode || currentMeetingId <= 0)
            {
                MessageBox.Show("لا يمكن حذف اللقاء قبل حفظه");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد أنك تريد حذف لقاء المهن بشكل كامل؟", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult != DialogResult.Yes)
                return;

            try
            {
                Program.buildConnection();
                using (MySqlTransaction transaction = Program.MyConn.BeginTransaction())
                {
                    DeleteProfessionMeetingDetails(transaction, currentMeetingId);
                    ExecuteNonQuery(transaction, "DELETE FROM profession_meeting WHERE ID = @MeetingId;", Parameter("@MeetingId", currentMeetingId));
                    transaction.Commit();
                }

                Program.MyConn.Close();

                try
                {
                    Log log = new Log();
                    log.Insert_Log("Delete profession meeting:" + currentMeetingId, "Profession Meeting", Program.UserName, DateTime.Now);
                }
                catch
                {
                }

                professionMeetingDeleted = true;
                MessageBox.Show("تم حذف لقاء المهن بنجاح");
                SetProfessionMeetingUpdateMode(-1);
                Close();
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

        private void DeleteMeeting_button_MouseEnter(object sender, EventArgs e)
        {
            DeleteMeeting_button.BackgroundImage = Settings.Default.theme == "Light" ? Resources.Delete2_L : Resources.Delete2_D;
        }

        private void DeleteMeeting_button_MouseLeave(object sender, EventArgs e)
        {
            DeleteMeeting_button.BackgroundImage = Settings.Default.theme == "Light" ? Resources.Delete2_D : Resources.Delete2_L;
        }

        private void LoadProfessionMeeting(int meetingId)
        {
            if (meetingId <= 0)
                return;

            try
            {
                DataSet dataSet = SelectProfessionMeeting(meetingId);
                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Profession meeting was not found");
                    SetProfessionMeetingUpdateMode(-1);
                    return;
                }

                ClearProfessionMeetingInputs();
                LoadMeetingHeader(dataSet.Tables[0].Rows[0]);
                LoadTextAnswers(dataSet.Tables[1]);
                LoadOptionAnswers(dataSet.Tables[2]);
                LoadCompetitionRows(dataSet.Tables[3]);
                LoadSeasonRows(dataSet.Tables[4]);
                LoadBrandRows(dataSet.Tables[5]);
                LoadEquipmentRows(dataSet.Tables[6]);
                LoadChallengeRows(dataSet.Tables[7]);
                LoadParticipantRows(dataSet.Tables[8], "19", participant1_dataGridView);
                LoadParticipantRows(dataSet.Tables[8], "20", participant2_dataGridView);
                LoadParticipantRows(dataSet.Tables[8], "21", participant3_dataGridView);
                LoadParticipantRows(dataSet.Tables[8], "22", participant4_dataGridView);
                LoadAttendeeRows(dataSet.Tables[9]);
                LoadMinuteRows(dataSet.Tables[10]);

                SetProfessionMeetingUpdateMode(meetingId);
                UpdateGuildDetailsVisibility();
                ConfigureDataGridViews();
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

        private DataSet SelectProfessionMeeting(int meetingId)
        {
            Program.buildConnection();

            string query = @"
SELECT pm.*,
       paperUser.UserName AS PaperRegistrarName,
       programUser.UserName AS ProgramRegistrarName,
       editorUser.UserName AS EditedByName
FROM profession_meeting pm
LEFT JOIN `user` paperUser ON paperUser.UserID = pm.PaperRegistrar_UserID
LEFT JOIN `user` programUser ON programUser.UserID = pm.ProgramRegistrar_UserID
LEFT JOIN `user` editorUser ON editorUser.UserID = pm.Edited_By_UserID
WHERE pm.ID = @MeetingId;

SELECT q.Number AS QuestionNumber, a.Value_Text, a.Value_Number
FROM profession_meeting_answer a
LEFT JOIN profession_question q ON q.ID = a.Question_ID
WHERE a.Meeting_ID = @MeetingId;

SELECT q.Number AS QuestionNumber, o.Sort_Order, oa.Is_Selected, oa.Rank_Value, oa.Other_Text
FROM profession_meeting_option_answer oa
LEFT JOIN profession_option o ON o.ID = oa.Option_ID
LEFT JOIN profession_question q ON q.ID = o.Question_ID
WHERE oa.Meeting_ID = @MeetingId;

SELECT Competition_Level, CountValue, Reason, Sort_Order
FROM profession_meeting_competition
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT Season_Count, From_Period, To_Period, Reason, Sort_Order
FROM profession_meeting_season
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT Brand_Name, Sort_Order
FROM profession_meeting_brand
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT Specialization, Equipment, Sort_Order
FROM profession_meeting_equipment
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT Challenge, Challenge_Count, Suggested_Solution, Sort_Order
FROM profession_meeting_challenge
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT q.Number AS QuestionNumber, p.MicroProject_ID, p.Person_ID, p.Person_Name, p.Speciality, p.Sort_Order
FROM profession_meeting_participant p
LEFT JOIN profession_question q ON q.ID = p.Question_ID
WHERE p.Meeting_ID = @MeetingId
ORDER BY q.Number, p.Sort_Order;

SELECT MicroProject_ID, Person_ID, Attendee_Name, Speciality, Sort_Order
FROM profession_meeting_attendee
WHERE Meeting_ID = @MeetingId
ORDER BY Sort_Order;

SELECT Section_ID, Notes
FROM profession_meeting_minute
WHERE Meeting_ID = @MeetingId
ORDER BY Section_ID;";

            MySqlCommand command = new MySqlCommand(query, Program.MyConn);
            command.Parameters.AddWithValue("@MeetingId", meetingId);

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            Program.MyConn.Close();
            return dataSet;
        }

        private void ClearProfessionMeetingInputs()
        {
            countOption1_numericUpDown.Value = countOption2_numericUpDown.Value = countOption3_numericUpDown.Value = 0;
            countOption4_numericUpDown.Value = countOption5_numericUpDown.Value = countOption6_numericUpDown.Value = 0;
            countOption7_numericUpDown.Value = countOption8_numericUpDown.Value = countOption9_numericUpDown.Value = 0;
            countOption10_numericUpDown.Value = countOption11_numericUpDown.Value = countOption12_numericUpDown.Value = 0;
            countOption13_numericUpDown.Value = countOption14_numericUpDown.Value = countOption15_numericUpDown.Value = 0;
            countOption16_numericUpDown.Value = countOption17_numericUpDown.Value = countOption18_numericUpDown.Value = countOption19_numericUpDown.Value = 0;
            countOption20_numericUpDown.Value = countOption21_numericUpDown.Value = countOption22_numericUpDown.Value = countOption23_numericUpDown.Value = 0;
            marketingOption1_numericUpDown.Value = marketingOption2_numericUpDown.Value = marketingOption3_numericUpDown.Value = marketingOption4_numericUpDown.Value = 0;
            marketingOption5_numericUpDown.Value = marketingOption6_numericUpDown.Value = marketingOption7_numericUpDown.Value = marketingOption8_numericUpDown.Value = 0;
            numericUpDown1.Value = numericUpDown2.Value = numericUpDown3.Value = 0;

            CheckBox[] checkBoxes =
            {
                targetAudience_checkBox1, targetAudience_checkBox2, targetAudience_checkBox3,
                targetAudience_checkBox4, targetAudience_checkBox5, targetAudience_checkBox6,
                targetAudience_checkBox7, targetAudience_checkBox8, targetAudience_checkBox9,
                targetAudience_other_checkBox, guild_checkBox1, guild_checkBox2, guild_checkBox3
            };

            foreach (CheckBox checkBox in checkBoxes)
                checkBox.Checked = false;

            targetAudience_other_textBox.Clear();
            countOption15_other_textBox.Clear();
            marketing_other_textBox.Clear();
            guildName_textBox.Clear();
            guildReferences_textBox.Clear();
            competitionSevereReason_textBox.Clear();
            competitionMediumReason_textBox.Clear();
            competitionWeakReason_textBox.Clear();

            memo1_richTextBox.Clear();
            memo2_richTextBox.Clear();
            memo3_richTextBox.Clear();
            memo4_richTextBox.Clear();
            memo6_richTextBox.Clear();
            memo7_richTextBox.Clear();
            memo8_richTextBox.Clear();
            memo9_richTextBox.Clear();
            memo10_richTextBox.Clear();
            memo11_richTextBox.Clear();

            season_dataGridView.Rows.Clear();
            equipment_dataGridView.Rows.Clear();
            challenges_dataGridView.Rows.Clear();
            participant1_dataGridView.Rows.Clear();
            participant2_dataGridView.Rows.Clear();
            participant3_dataGridView.Rows.Clear();
            participant4_dataGridView.Rows.Clear();
            attendees_dataGridView.Rows.Clear();
        }

        private void LoadMeetingHeader(DataRow row)
        {
            dateTimePicker1.Value = GetDateValue(row, "MeetingDate", DateTime.Today);
            place_textBox.Text = GetStringValue(row, "Place");
            SetNumericValue(invitedCount_numericUpDown, GetIntValue(row, "InvitedCount"));
            SetNumericValue(attendanceCount_numericUpDown, GetIntValue(row, "AttendanceCount"));
            SetNumericValue(absenceCount_numericUpDown, GetIntValue(row, "AbsenceCount"));

            loadingProfessionProjects = true;
            try
            {
                int? subCategoryId = GetIntValue(row, "SubCategory_ID");
                int? careerId = GetIntValue(row, "Career_ID");

                SetComboBoxSelectedValue(professionSubCategory_comboBox, subCategoryId);
                BindCareerComboBox(subCategoryId.HasValue ? subCategoryId.Value.ToString() : string.Empty);
                SetComboBoxSelectedValue(professionCareer_comboBox, careerId);

                if (careerId.HasValue)
                {
                    professionProjectsTable = GetProjectsByProfession(careerId.Value);
                    LoadProfessionBeneficiaries(careerId.Value);
                }
                else
                {
                    professionProjectsTable = CreateProjectLookupTable();
                    projectBeneficiaries.Clear();
                }

                ConfigureProjectComboColumns(professionProjectsTable);
            }
            finally
            {
                loadingProfessionProjects = false;
            }

            string paperRegistrarName = GetStringValue(row, "PaperRegistrarName");
            if (!string.IsNullOrWhiteSpace(paperRegistrarName))
            {
                if (!paperRegistrar_comboBox.Items.Contains(paperRegistrarName))
                    paperRegistrar_comboBox.Items.Add(paperRegistrarName);
                paperRegistrar_comboBox.SelectedItem = paperRegistrarName;
            }

            programRegistrar_textBox.Text = GetStringValue(row, "ProgramRegistrarName");
            if (string.IsNullOrWhiteSpace(programRegistrar_textBox.Text))
                programRegistrar_textBox.Text = Program.UserName ?? string.Empty;

            editedBy_textBox.Text = Program.UserName ?? string.Empty;
        }

        private void LoadTextAnswers(DataTable table)
        {
            memo1_richTextBox.Text = GetTextAnswer(table, "5");
            memo3_richTextBox.Text = GetTextAnswer(table, "14");
            memo4_richTextBox.Text = GetTextAnswer(table, "15");
            memo6_richTextBox.Text = GetTextAnswer(table, "18");
        }

        private void LoadOptionAnswers(DataTable table)
        {
            LoadOptionNumbers(table, "1", new NumericUpDown[] { countOption1_numericUpDown, countOption2_numericUpDown, countOption3_numericUpDown });
            LoadOptionNumbers(table, "2", new NumericUpDown[] { countOption4_numericUpDown, countOption5_numericUpDown, countOption6_numericUpDown });
            LoadOptionNumbers(table, "3", new NumericUpDown[] { countOption7_numericUpDown, countOption8_numericUpDown, countOption9_numericUpDown });
            LoadCheckOptions(table, "4",
                new CheckBox[] { targetAudience_checkBox1, targetAudience_checkBox2, targetAudience_checkBox3, targetAudience_checkBox4, targetAudience_checkBox5, targetAudience_checkBox6, targetAudience_checkBox7, targetAudience_checkBox8, targetAudience_checkBox9 },
                targetAudience_other_checkBox,
                targetAudience_other_textBox,
                10);
            LoadOptionNumbers(table, "7", new NumericUpDown[] { countOption10_numericUpDown, countOption11_numericUpDown, countOption12_numericUpDown, countOption13_numericUpDown, countOption14_numericUpDown, countOption15_numericUpDown }, 6, countOption15_other_textBox);
            LoadGuildOptions(table);
            LoadOptionNumbers(table, "11", new NumericUpDown[] { marketingOption1_numericUpDown, marketingOption2_numericUpDown, marketingOption3_numericUpDown, marketingOption4_numericUpDown, marketingOption5_numericUpDown, marketingOption6_numericUpDown, marketingOption7_numericUpDown, marketingOption8_numericUpDown }, 8, marketing_other_textBox);
            LoadOptionNumbers(table, "13", new NumericUpDown[] { countOption16_numericUpDown, countOption17_numericUpDown, countOption18_numericUpDown, countOption19_numericUpDown });
            LoadOptionNumbers(table, "17", new NumericUpDown[] { countOption20_numericUpDown, countOption21_numericUpDown, countOption22_numericUpDown, countOption23_numericUpDown });
        }

        private void LoadOptionNumbers(DataTable table, string questionNumber, NumericUpDown[] controls)
        {
            LoadOptionNumbers(table, questionNumber, controls, -1, null);
        }

        private void LoadOptionNumbers(DataTable table, string questionNumber, NumericUpDown[] controls, int otherSortOrder, TextBox otherTextBox)
        {
            foreach (DataRow row in table.Rows)
            {
                if (GetStringValue(row, "QuestionNumber") != questionNumber)
                    continue;

                int? sortOrder = GetIntValue(row, "Sort_Order");
                if (!sortOrder.HasValue || sortOrder.Value <= 0 || sortOrder.Value > controls.Length)
                    continue;

                SetNumericValue(controls[sortOrder.Value - 1], GetIntValue(row, "Rank_Value"));

                if (otherSortOrder == sortOrder.Value && otherTextBox != null)
                    otherTextBox.Text = GetStringValue(row, "Other_Text");
            }
        }

        private void LoadCheckOptions(DataTable table, string questionNumber, CheckBox[] checkBoxes, CheckBox otherCheckBox, TextBox otherTextBox, int otherSortOrder)
        {
            foreach (DataRow row in table.Rows)
            {
                if (GetStringValue(row, "QuestionNumber") != questionNumber)
                    continue;

                int? sortOrder = GetIntValue(row, "Sort_Order");
                if (!sortOrder.HasValue)
                    continue;

                if (sortOrder.Value >= 1 && sortOrder.Value <= checkBoxes.Length)
                    checkBoxes[sortOrder.Value - 1].Checked = true;

                if (sortOrder.Value == otherSortOrder)
                {
                    otherCheckBox.Checked = true;
                    otherTextBox.Text = GetStringValue(row, "Other_Text");
                }
            }
        }

        private void LoadGuildOptions(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                if (GetStringValue(row, "QuestionNumber") != "9")
                    continue;

                int? sortOrder = GetIntValue(row, "Sort_Order");
                if (!sortOrder.HasValue)
                    continue;

                if (sortOrder.Value == 1)
                {
                    guild_checkBox1.Checked = true;
                    guildName_textBox.Text = GetStringValue(row, "Other_Text");
                }
                else if (sortOrder.Value == 2)
                {
                    guild_checkBox2.Checked = true;
                    guildReferences_textBox.Text = GetStringValue(row, "Other_Text");
                }
                else if (sortOrder.Value == 3)
                {
                    guild_checkBox3.Checked = true;
                }
            }
        }

        private void LoadCompetitionRows(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                int? sortOrder = GetIntValue(row, "Sort_Order");
                if (!sortOrder.HasValue)
                    continue;

                if (sortOrder.Value == 1)
                {
                    SetNumericValue(numericUpDown1, GetIntValue(row, "CountValue"));
                    competitionSevereReason_textBox.Text = GetStringValue(row, "Reason");
                }
                else if (sortOrder.Value == 2)
                {
                    SetNumericValue(numericUpDown2, GetIntValue(row, "CountValue"));
                    competitionMediumReason_textBox.Text = GetStringValue(row, "Reason");
                }
                else if (sortOrder.Value == 3)
                {
                    SetNumericValue(numericUpDown3, GetIntValue(row, "CountValue"));
                    competitionWeakReason_textBox.Text = GetStringValue(row, "Reason");
                }
            }
        }

        private void LoadSeasonRows(DataTable table)
        {
            season_dataGridView.Rows.Clear();
            foreach (DataRow row in table.Rows)
                season_dataGridView.Rows.Add(GetStringValue(row, "Season_Count"), GetStringValue(row, "From_Period"), GetStringValue(row, "To_Period"), GetStringValue(row, "Reason"));
        }

        private void LoadBrandRows(DataTable table)
        {
            List<string> brands = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                string brand = GetStringValue(row, "Brand_Name");
                if (!string.IsNullOrWhiteSpace(brand))
                    brands.Add(brand);
            }

            memo2_richTextBox.Text = string.Join(Environment.NewLine, brands.ToArray());
        }

        private void LoadEquipmentRows(DataTable table)
        {
            equipment_dataGridView.Rows.Clear();
            foreach (DataRow row in table.Rows)
                equipment_dataGridView.Rows.Add(GetStringValue(row, "Specialization"), GetStringValue(row, "Equipment"));
        }

        private void LoadChallengeRows(DataTable table)
        {
            challenges_dataGridView.Rows.Clear();
            foreach (DataRow row in table.Rows)
                challenges_dataGridView.Rows.Add(GetStringValue(row, "Challenge"), GetStringValue(row, "Challenge_Count"), GetStringValue(row, "Suggested_Solution"));

            if (challenges_dataGridView.Rows.Count <= 1)
                PopulateDefaultChallengeRows();
        }

        private void LoadParticipantRows(DataTable table, string questionNumber, DataGridView grid)
        {
            grid.Rows.Clear();
            foreach (DataRow row in table.Rows)
            {
                if (GetStringValue(row, "QuestionNumber") != questionNumber)
                    continue;

                AddPersonProjectRow(grid, GetIntValue(row, "MicroProject_ID"), GetIntValue(row, "Person_ID"), GetStringValue(row, "Person_Name"), GetStringValue(row, "Speciality"));
            }
        }

        private void LoadAttendeeRows(DataTable table)
        {
            attendees_dataGridView.Rows.Clear();
            foreach (DataRow row in table.Rows)
                AddPersonProjectRow(attendees_dataGridView, GetIntValue(row, "MicroProject_ID"), GetIntValue(row, "Person_ID"), GetStringValue(row, "Attendee_Name"), GetStringValue(row, "Speciality"));
        }

        private void AddPersonProjectRow(DataGridView grid, int? projectId, int? personId, string personName, string speciality)
        {
            int rowIndex = grid.Rows.Add();
            DataGridViewRow row = grid.Rows[rowIndex];

            if (projectId.HasValue)
                FillParticipantProjectRow(grid, rowIndex, projectId.Value, personId);

            DataGridViewComboBoxCell personCell = row.Cells[1] as DataGridViewComboBoxCell;
            if (personId.HasValue && personCell != null)
            {
                object personValue = NormalizeBeneficiaryValue(personCell.DataSource as DataTable, personId.Value);
                if (personValue != null)
                    personCell.Value = personValue;
            }
            else
            {
                row.Cells[1] = new DataGridViewTextBoxCell();
                row.Cells[1].Value = personName;
                row.Cells[1].Tag = personId.HasValue ? personId.Value.ToString() : null;
                ApplyApplicationNewGridCellStyle(row.Cells[1].Style);
            }

            row.Cells[2].Value = speciality;
            ApplyApplicationNewGridCellStyle(row.Cells[2].Style);
        }

        private void LoadMinuteRows(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                int? sectionId = GetIntValue(row, "Section_ID");
                if (!sectionId.HasValue)
                    continue;

                string notes = GetStringValue(row, "Notes");
                if (sectionId.Value == 1)
                    memo7_richTextBox.Text = notes;
                else if (sectionId.Value == 2)
                    memo8_richTextBox.Text = notes;
                else if (sectionId.Value == 3)
                    memo9_richTextBox.Text = notes;
                else if (sectionId.Value == 4)
                    memo10_richTextBox.Text = notes;
                else if (sectionId.Value == 5)
                    memo11_richTextBox.Text = notes;
            }
        }

        private string GetTextAnswer(DataTable table, string questionNumber)
        {
            foreach (DataRow row in table.Rows)
            {
                if (GetStringValue(row, "QuestionNumber") == questionNumber)
                    return GetStringValue(row, "Value_Text");
            }

            return string.Empty;
        }

        private void SetComboBoxSelectedValue(ComboBox comboBox, int? value)
        {
            if (!value.HasValue)
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            comboBox.SelectedValue = value.Value;
        }

        private void SetNumericValue(NumericUpDown control, int? value)
        {
            decimal numericValue = value.HasValue ? value.Value : 0;
            if (numericValue < control.Minimum)
                numericValue = control.Minimum;
            if (numericValue > control.Maximum)
                numericValue = control.Maximum;

            control.Value = numericValue;
        }

        private string GetStringValue(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == null || row[columnName] == DBNull.Value)
                return string.Empty;

            return row[columnName].ToString();
        }

        private int? GetIntValue(DataRow row, string columnName)
        {
            string value = GetStringValue(row, columnName);
            int intValue;
            if (int.TryParse(value, out intValue))
                return intValue;

            return null;
        }

        private DateTime GetDateValue(DataRow row, string columnName, DateTime defaultValue)
        {
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == null || row[columnName] == DBNull.Value)
                return defaultValue;

            DateTime value;
            if (DateTime.TryParse(row[columnName].ToString(), out value))
                return value;

            return defaultValue;
        }

        private void ApplyApplicationNewDataGridViewStyle(DataGridView grid)
        {
            if (grid == null)
                return;

            bool lightTheme = Settings.Default.theme == "Light";
            Color backColor = lightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
            Color foreColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
            Color gridColor = lightTheme ? NewTheme.light_grey3 : NewTheme.light_grey2;

            grid.BackgroundColor = backColor;
            grid.GridColor = gridColor;
            grid.ForeColor = foreColor;

            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ApplyApplicationNewGridCellStyle(grid.DefaultCellStyle);
            ApplyApplicationNewGridCellStyle(grid.RowsDefaultCellStyle);
            ApplyApplicationNewGridCellStyle(grid.AlternatingRowsDefaultCellStyle);
            ApplyApplicationNewGridCellStyle(grid.ColumnHeadersDefaultCellStyle);
            ApplyApplicationNewGridCellStyle(grid.RowHeadersDefaultCellStyle);
            ApplyApplicationNewGridCellStyle(grid.RowTemplate.DefaultCellStyle);

            foreach (DataGridViewColumn column in grid.Columns)
            {
                ApplyApplicationNewGridCellStyle(column.DefaultCellStyle);
                ApplyApplicationNewGridCellStyle(column.HeaderCell.Style);
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                    ApplyApplicationNewGridCellStyle(cell.Style);
            }
        }

        private void ApplyApplicationNewGridCellStyle(DataGridViewCellStyle style)
        {
            if (style == null)
                return;

            bool lightTheme = Settings.Default.theme == "Light";
            style.BackColor = lightTheme ? NewTheme.light_white : NewTheme.dark_grey2;
            style.ForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
            style.SelectionBackColor = lightTheme ? NewTheme.light_grey2 : NewTheme.light_grey3;
            style.SelectionForeColor = lightTheme ? NewTheme.light_grey4 : NewTheme.light_white;
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

            if (string.IsNullOrWhiteSpace(place_textBox.Text))
            {
                MessageBox.Show("الرجاء إدخال مكان اللقاء");
                return false;
            }

            if (!ValidateMeetingCounts())
                return false;

            if (attendanceCount_numericUpDown.Value > invitedCount_numericUpDown.Value)
            {
                MessageBox.Show("عدد الحضور لا يمكن أن يكون أكبر من عدد المدعوين");
                return false;
            }

            if (!ValidateQuestionTotal("1", new NumericUpDown[] { countOption1_numericUpDown, countOption2_numericUpDown, countOption3_numericUpDown }))
                return false;

            if (!ValidateQuestionTotal("2", new NumericUpDown[] { countOption4_numericUpDown, countOption5_numericUpDown, countOption6_numericUpDown }))
                return false;

            if (!ValidateQuestionTotal("3", new NumericUpDown[] { countOption7_numericUpDown, countOption8_numericUpDown, countOption9_numericUpDown }))
                return false;

            if (!ValidateTargetAudienceAnswer())
                return false;

            if (targetAudience_other_checkBox.Checked && string.IsNullOrWhiteSpace(targetAudience_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 4");
                return false;
            }

            if (!ValidateRequiredRichTextAnswer("5", memo1_richTextBox))
                return false;

            if (!ValidateQuestionTotal("6", new NumericUpDown[] { numericUpDown1, numericUpDown2, numericUpDown3 }))
                return false;

            if (!ValidateCompetitionReasons())
                return false;

            if (!ValidateQuestionTotal("7", new NumericUpDown[] { countOption10_numericUpDown, countOption11_numericUpDown, countOption12_numericUpDown, countOption13_numericUpDown, countOption14_numericUpDown, countOption15_numericUpDown }))
                return false;

            if (countOption15_numericUpDown.Value > 0 && string.IsNullOrWhiteSpace(countOption15_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 7");
                return false;
            }

            if (!ValidateQuestionGridTotal("8", season_dataGridView, 0))
                return false;

            if (!ValidateSeasonRows())
                return false;

            if (!ValidateGuildAnswer())
                return false;

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

            if (!ValidateRequiredRichTextAnswer("10", memo2_richTextBox))
                return false;

            if (!ValidateQuestionTotal("11", new NumericUpDown[] { marketingOption1_numericUpDown, marketingOption2_numericUpDown, marketingOption3_numericUpDown, marketingOption4_numericUpDown, marketingOption5_numericUpDown, marketingOption6_numericUpDown, marketingOption7_numericUpDown, marketingOption8_numericUpDown }))
                return false;

            if (marketingOption8_numericUpDown.Value > 0 && string.IsNullOrWhiteSpace(marketing_other_textBox.Text))
            {
                MessageBox.Show("الرجاء تحديد خيار غير ذلك في السؤال 11");
                return false;
            }

            if (!ValidateEquipmentRows())
                return false;

            if (!ValidateQuestionTotal("13", new NumericUpDown[] { countOption16_numericUpDown, countOption17_numericUpDown, countOption18_numericUpDown, countOption19_numericUpDown }))
                return false;

            if (!ValidateRequiredRichTextAnswer("14", memo3_richTextBox))
                return false;

            if (!ValidateRequiredRichTextAnswer("15", memo4_richTextBox))
                return false;

            if (!ValidateQuestionGridTotal("16", challenges_dataGridView, 1))
                return false;

            if (!ValidateChallengeRows())
                return false;

            if (!ValidateQuestionTotal("17", new NumericUpDown[] { countOption20_numericUpDown, countOption21_numericUpDown, countOption22_numericUpDown, countOption23_numericUpDown }))
                return false;

            if (!ValidateRequiredRichTextAnswer("18", memo6_richTextBox))
                return false;

            if (!ValidateAttendeeRowsTotal())
                return false;

            if (!ValidateMeetingMinuteAnswers())
                return false;

            return true;
        }

        private bool ValidateTargetAudienceAnswer()
        {
            if (!targetAudience_checkBox1.Checked &&
                !targetAudience_checkBox2.Checked &&
                !targetAudience_checkBox3.Checked &&
                !targetAudience_checkBox4.Checked &&
                !targetAudience_checkBox5.Checked &&
                !targetAudience_checkBox6.Checked &&
                !targetAudience_checkBox7.Checked &&
                !targetAudience_checkBox8.Checked &&
                !targetAudience_checkBox9.Checked &&
                !targetAudience_other_checkBox.Checked)
            {
                MessageBox.Show("الرجاء الإجابة على السؤال 4");
                return false;
            }

            return true;
        }

        private bool ValidateGuildAnswer()
        {
            if (!guild_checkBox1.Checked &&
                !guild_checkBox2.Checked &&
                !guild_checkBox3.Checked)
            {
                MessageBox.Show("الرجاء الإجابة على السؤال 9");
                return false;
            }

            return true;
        }

        private bool ValidateRequiredRichTextAnswer(string questionNumber, RichTextBox richTextBox)
        {
            if (string.IsNullOrWhiteSpace(richTextBox.Text))
            {
                MessageBox.Show("الرجاء الإجابة على السؤال " + questionNumber);
                return false;
            }

            return true;
        }

        private bool ValidateCompetitionReasons()
        {
            if (!ValidateCompetitionReason("6", numericUpDown1, competitionSevereReason_textBox))
                return false;

            if (!ValidateCompetitionReason("6", numericUpDown2, competitionMediumReason_textBox))
                return false;

            if (!ValidateCompetitionReason("6", numericUpDown3, competitionWeakReason_textBox))
                return false;

            return true;
        }

        private bool ValidateCompetitionReason(string questionNumber, NumericUpDown countControl, TextBox reasonTextBox)
        {
            if (countControl.Value > 0 && string.IsNullOrWhiteSpace(reasonTextBox.Text))
            {
                MessageBox.Show("عند إدخال عدد في السؤال " + questionNumber + " يجب إدخال السبب");
                return false;
            }

            return true;
        }

        private bool ValidateMeetingMinuteAnswers()
        {
            if (string.IsNullOrWhiteSpace(memo7_richTextBox.Text) ||
                string.IsNullOrWhiteSpace(memo8_richTextBox.Text) ||
                string.IsNullOrWhiteSpace(memo9_richTextBox.Text) ||
                string.IsNullOrWhiteSpace(memo10_richTextBox.Text) ||
                string.IsNullOrWhiteSpace(memo11_richTextBox.Text))
            {
                MessageBox.Show("الرجاء تعبئة محضر الاجتماع لكل المحاور");
                return false;
            }

            return true;
        }

        private bool ValidateMeetingCounts()
        {
            if (invitedCount_numericUpDown.Value <= 0)
            {
                MessageBox.Show("الرجاء إدخال عدد المدعوين");
                return false;
            }

            if (attendanceCount_numericUpDown.Value <= 0)
            {
                MessageBox.Show("الرجاء إدخال عدد الحضور");
                return false;
            }

            decimal expectedAbsenceCount = invitedCount_numericUpDown.Value - attendanceCount_numericUpDown.Value;
            if (expectedAbsenceCount >= absenceCount_numericUpDown.Minimum &&
                expectedAbsenceCount <= absenceCount_numericUpDown.Maximum &&
                absenceCount_numericUpDown.Value != expectedAbsenceCount)
            {
                absenceCount_numericUpDown.Value = expectedAbsenceCount;
            }

            return true;
        }

        private bool ValidateSeasonRows()
        {
            foreach (DataGridViewRow row in season_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string countValue = GetCellText(row, 0);
                if (string.IsNullOrWhiteSpace(countValue))
                    continue;

                if (string.IsNullOrWhiteSpace(GetCellText(row, 1)) ||
                    string.IsNullOrWhiteSpace(GetCellText(row, 2)) ||
                    string.IsNullOrWhiteSpace(GetCellText(row, 3)))
                {
                    MessageBox.Show("عند إدخال العدد في السؤال 8 يجب إدخال من وإلى والسبب");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateEquipmentRows()
        {
            bool hasAnsweredRow = false;

            foreach (DataGridViewRow row in equipment_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string specialization = GetCellText(row, 0);
                string equipment = GetCellText(row, 1);

                if (string.IsNullOrWhiteSpace(specialization) && string.IsNullOrWhiteSpace(equipment))
                    continue;

                if (string.IsNullOrWhiteSpace(specialization) || string.IsNullOrWhiteSpace(equipment))
                {
                    MessageBox.Show("الرجاء إكمال الاختصاص والمعدات في السؤال 12");
                    return false;
                }

                hasAnsweredRow = true;
            }

            if (!hasAnsweredRow)
            {
                MessageBox.Show("الرجاء الإجابة على السؤال 12");
                return false;
            }

            return true;
        }

        private bool ValidateChallengeRows()
        {
            foreach (DataGridViewRow row in challenges_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string countValue = GetCellText(row, 1);
                string solution = GetCellText(row, 2);

                if (string.IsNullOrWhiteSpace(countValue))
                    continue;

                int count;
                if (!int.TryParse(countValue, out count) || count <= 0)
                    continue;

                if (string.IsNullOrWhiteSpace(solution))
                {
                    MessageBox.Show("عند إدخال عدد في السؤال 16 يجب إدخال اقتراحات الحلول");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateQuestionGridTotal(string questionNumber, DataGridView grid, int countColumnIndex)
        {
            int total = 0;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string value = GetCellText(row, countColumnIndex);
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                int countValue;
                if (!int.TryParse(value, out countValue))
                {
                    MessageBox.Show("الرجاء إدخال أرقام صحيحة في السؤال " + questionNumber);
                    return false;
                }

                total += countValue;
            }

            int attendanceCount = Convert.ToInt32(attendanceCount_numericUpDown.Value);
            if (total != attendanceCount)
            {
                MessageBox.Show("مجموع الأعداد في السؤال " + questionNumber + " يجب أن يساوي عدد الحضور");
                return false;
            }

            return true;
        }

        private bool ValidateAttendeeRowsTotal()
        {
            int total = 0;
            foreach (DataGridViewRow row in attendees_dataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int? projectId = GetNullableIntFromCell(row, 0);
                string attendeeName = GetCellText(row, 1);
                string speciality = GetCellText(row, 2);

                if (!projectId.HasValue &&
                    string.IsNullOrWhiteSpace(attendeeName) &&
                    string.IsNullOrWhiteSpace(speciality))
                    continue;

                total++;
            }

            int attendanceCount = Convert.ToInt32(attendanceCount_numericUpDown.Value);
            if (total != attendanceCount)
            {
                MessageBox.Show("عدد الأسماء في جدول أسماء الحضور يجب أن يساوي عدد الحضور");
                return false;
            }

            return true;
        }

        private bool ValidateQuestionTotal(string questionNumber, NumericUpDown[] controls)
        {
            int total = 0;
            foreach (NumericUpDown control in controls)
                total += Convert.ToInt32(control.Value);

            int attendanceCount = Convert.ToInt32(attendanceCount_numericUpDown.Value);
            if (total != attendanceCount)
            {
                MessageBox.Show("مجموع الأعداد في السؤال " + questionNumber + " يجب أن يساوي عدد الحضور");
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

        private void UpdateProfessionMeeting(MySqlTransaction transaction, int meetingId)
        {
            int careerId = Convert.ToInt32(professionCareer_comboBox.SelectedValue);
            object subCategoryId = DbValue(GetSelectedIntValue(professionSubCategory_comboBox));
            object paperRegistrarId = DbValue(GetUserIdByName(paperRegistrar_comboBox.Text));

            string query = @"
UPDATE profession_meeting SET
    Career_ID = @CareerId,
    SubCategory_ID = @SubCategoryId,
    MeetingDate = @MeetingDate,
    Place = @Place,
    InvitedCount = @InvitedCount,
    AttendanceCount = @AttendanceCount,
    AbsenceCount = @AbsenceCount,
    PaperRegistrar_UserID = @PaperRegistrarUserId,
    Edited_By_UserID = @EditedByUserId,
    Updated_At = @UpdatedAt
WHERE ID = @MeetingId;";

            ExecuteNonQuery(transaction, query,
                Parameter("@CareerId", careerId),
                Parameter("@SubCategoryId", subCategoryId),
                Parameter("@MeetingDate", dateTimePicker1.Value.Date),
                Parameter("@Place", DbString(place_textBox.Text)),
                Parameter("@InvitedCount", Convert.ToInt32(invitedCount_numericUpDown.Value)),
                Parameter("@AttendanceCount", Convert.ToInt32(attendanceCount_numericUpDown.Value)),
                Parameter("@AbsenceCount", Convert.ToInt32(absenceCount_numericUpDown.Value)),
                Parameter("@PaperRegistrarUserId", paperRegistrarId),
                Parameter("@EditedByUserId", Program.userID),
                Parameter("@UpdatedAt", DateTime.Now),
                Parameter("@MeetingId", meetingId));
        }

        private void DeleteProfessionMeetingDetails(MySqlTransaction transaction, int meetingId)
        {
            string[] tables =
            {
                "profession_meeting_answer",
                "profession_meeting_option_answer",
                "profession_meeting_competition",
                "profession_meeting_season",
                "profession_meeting_brand",
                "profession_meeting_equipment",
                "profession_meeting_challenge",
                "profession_meeting_participant",
                "profession_meeting_attendee",
                "profession_meeting_minute"
            };

            foreach (string table in tables)
                ExecuteNonQuery(transaction, "DELETE FROM " + table + " WHERE Meeting_ID = @MeetingId;", Parameter("@MeetingId", meetingId));
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
