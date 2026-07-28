using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
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
                UpdateGuildDetailsVisibility();
                ConfigureDataGridViewsForRtlEditing();
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
                grid.EditingControlShowing -= DataGridView_EditingControlShowing;
                grid.CellClick += DataGridView_CellClick;
                grid.CellEnter += DataGridView_CellEnter;
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
