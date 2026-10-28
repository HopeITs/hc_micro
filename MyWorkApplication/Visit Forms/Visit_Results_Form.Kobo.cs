using MyWorkApplication.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MyWorkApplication.Visit_Forms
{
    public partial class Visit_Results_Form
    {
        private bool koboModeVisible;
        private bool koboLoading;
        private bool koboReloadRequested;
        private bool koboBinding;
        private int[] legacyFilterSelections;
        private DataTable koboSources;
        private string koboSourceUid;

        private bool IsKoboMode { get { return SearchBy_comboBox.SelectedIndex >= 2; } }

        private ComboBox[] ProjectFilterControls()
        {
            return new ComboBox[] { VisitNumber_comboBox, VisitKind_comboBox, Type_comboBox, Category_comboBox, SubCategory_comboBox, Career_comboBox };
        }

        private void SetKoboMode(bool enabled)
        {
            if (enabled == koboModeVisible) return;
            bool wasUserMode = user_mode;
            user_mode = false;
            try
            {
                ComboBox[] controls = ProjectFilterControls();
                if (enabled)
                {
                    legacyFilterSelections = new int[controls.Length];
                    for (int i = 0; i < controls.Length; i++) legacyFilterSelections[i] = controls[i].SelectedIndex;
                    foreach (ComboBox control in controls) control.SelectedIndex = -1;
                    ReloadKoboForms();
                }
                else if (legacyFilterSelections != null)
                {
                    for (int i = 0; i < controls.Length; i++)
                        controls[i].SelectedIndex = legacyFilterSelections[i] < controls[i].Items.Count ? legacyFilterSelections[i] : -1;
                    Answers_dataGridView.RowTemplate.Height = 27;
                    Answers_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    Answers_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                }
                koboModeVisible = enabled;
                KoboFilters_panel.Visible = enabled && !Hided;
            }
            finally { user_mode = wasUserMode; }
        }

        private void ResetKoboFilters()
        {
            bool wasUserMode = user_mode;
            user_mode = false;
            try
            {
                foreach (ComboBox control in ProjectFilterControls()) control.SelectedIndex = -1;
                if (KoboVersion_comboBox.Items.Count > 0) KoboVersion_comboBox.SelectedIndex = 0;
            }
            finally { user_mode = wasUserMode; }
        }

        private void ReloadKoboForms()
        {
            string selected = Convert.ToString(KoboForm_comboBox.SelectedValue);
            koboLoading = true;
            try
            {
                koboSources = null;
                KoboForm_comboBox.DisplayMember = "Name";
                KoboForm_comboBox.ValueMember = "UID";
                KoboForm_comboBox.DataSource = KoboVisitResults.GetForms();
                if (!string.IsNullOrEmpty(selected)) KoboForm_comboBox.SelectedValue = selected;
                if (KoboForm_comboBox.SelectedIndex < 0 && KoboForm_comboBox.Items.Count > 0) KoboForm_comboBox.SelectedIndex = 0;
            }
            finally { koboLoading = false; }
        }

        private void KoboForm_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (koboLoading || !user_mode) return;
            koboSources = null;
            SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }

        private void KoboVersion_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!koboLoading && user_mode) SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }

        private void LoadKoboResults()
        {
            bool raiseEvents = bs.RaiseListChangedEvents;
            bool filterEnabled = Answers_dataGridView.FilterAndSortEnabled;
            koboBinding = true;
            Answers_dataGridView.SuspendLayout();
            try
            {
                // Configure once, without remeasuring the entire grid after every generated column.
                Answers_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                Answers_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                Answers_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                Answers_dataGridView.FilterAndSortEnabled = false;
                bs.RaiseListChangedEvents = false;
                LoadKoboResultsCore();
            }
            finally
            {
                bs.RaiseListChangedEvents = raiseEvents;
                Answers_dataGridView.FilterAndSortEnabled = filterEnabled;
                if (SearchBy_comboBox.SelectedIndex == 2)
                    Answers_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                else
                {
                    Answers_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    Answers_dataGridView.ColumnHeadersHeight = 126;
                }
                Answers_dataGridView.ResumeLayout(false);
                if (Answers_dataGridView.DataSource != null && SearchBy_comboBox.SelectedIndex == 2)
                    Answers_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                koboBinding = false;
            }
            UpdateKoboSelectionTotals();
        }

        private void LoadKoboResultsCore()
        {
            if (koboReloadRequested)
            {
                ReloadKoboForms();
                koboReloadRequested = false;
            }
            Answers_dataGridView.DataSource = null;
            Answers_dataGridView.Columns.Clear();
            KoboSummary_label.Text = "";
            string uid = Convert.ToString(KoboForm_comboBox.SelectedValue);
            if (KoboForm_comboBox.Items.Count == 0) return;
            if (koboSources == null || uid != koboSourceUid)
            {
                koboSources = KoboVisitResults.GetSources(uid);
                koboSourceUid = uid;
                koboLoading = true;
                try
                {
                    KoboVersion_comboBox.DisplayMember = "Name";
                    KoboVersion_comboBox.ValueMember = "Key";
                    KoboVersion_comboBox.DataSource = KoboVisitResults.GetVersions(koboSources);
                }
                finally { koboLoading = false; }
            }
            string version = Convert.ToString(KoboVersion_comboBox.SelectedValue);
            KoboResultsFilter filter = new KoboResultsFilter
            {
                From = DateFrom_dateTimePicker.Value, To = DateTo_dateTimePicker.Value,
                Version = version.StartsWith("v:", StringComparison.Ordinal) ? version.Substring(2) : null,
                Stage = visitNum, Kind = visitKind, ProjectType = mp_typeID,
                Category = mp_categoryID, SubCategory = mp_subcategoryID, Career = mp_careerID,
                LastVisitOnly = onlyLastVisit
            };
            bool counts = SearchBy_comboBox.SelectedIndex == 2;
            KoboResultsReport report = KoboVisitResults.BuildCombined(koboSources, filter, counts);
            bs.DataSource = report.Table;
            Answers_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            Answers_dataGridView.RowTemplate.Height = 27;
            Answers_dataGridView.RowTemplate.MinimumHeight = 27;
            Answers_dataGridView.DataSource = bs;
            foreach (DataColumn source in report.Table.Columns)
            {
                DataGridViewColumn column = Answers_dataGridView.Columns[source.ColumnName];
                column.HeaderText = source.Caption;
                bool narrow = Answers_dataGridView.ClientSize.Width < 1150;
                column.Width = counts ? (source.ColumnName == "Question" ? (narrow ? 300 : 420) : source.ColumnName.StartsWith("Answer", StringComparison.Ordinal) && source.ColumnName != "Answered" ? (narrow ? 95 : 110) : 75) : 145;
                column.MinimumWidth = 60;
                column.DefaultCellStyle.WrapMode = counts ? DataGridViewTriState.True : DataGridViewTriState.False;
                column.ToolTipText = source.Caption;
            }
            if (counts)
            {
                Answers_dataGridView.Columns["Question"].Frozen = true;
                Answers_dataGridView.Columns["Answered"].Width = 115;
                Answers_dataGridView.Columns["Type"].Visible = false;
                Answers_dataGridView.Columns["Unanswered"].Visible = false;
                Answers_dataGridView.Columns["Visits"].Visible = false;
            }
            if (!counts)
            {
                Answers_dataGridView.Columns["Project_ID"].Width = 90;
                Answers_dataGridView.Columns["Beneficiary"].Width = 200;
                Answers_dataGridView.Columns["Stage"].Width = 85;
                Answers_dataGridView.Columns["Visit_Date"].Width = 110;
                Answers_dataGridView.Columns["SubCategory_Name"].Width = 120;
                Answers_dataGridView.Columns["Career_Name"].Width = 130;
                Answers_dataGridView.Columns["Project_ID"].Frozen = true;
                Answers_dataGridView.Columns["Beneficiary"].Frozen = true;
                Answers_dataGridView.Columns["Visit_Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
            Answers_dataGridView.ColumnHeadersVisible = true;
            AllCount_label.Text = "All: " + bs.Count.ToString("#,##0");
            KoboSummary_label.Text = "الزيارات المؤكدة: " + report.Visits.ToString("#,##0") +
                "    |    المستبعدة من النتائج: " + report.Excluded.ToString("#,##0") +
                (report.InvalidJson == 0 ? "" : "    |    تعذر قراءة إجابتها: " + report.InvalidJson.ToString("#,##0")) +
                (report.InvalidDate == 0 ? "" : "    |    تاريخها غير صالح: " + report.InvalidDate.ToString("#,##0")) +
                (report.UnmappedFields == 0 ? "" : "    |    حقول إضافية متاحة في عارض المصدر: " + report.UnmappedFields.ToString("#,##0"));
        }

        private void UpdateKoboSelectionTotals()
        {
            int count = 0;
            double sum = 0;
            foreach (DataGridViewCell cell in Answers_dataGridView.SelectedCells)
            {
                if (!cell.Visible) continue;
                count++;
                double value;
                if (double.TryParse(Convert.ToString(cell.Value), out value)) sum += value;
            }
            SelectedCount_label.Text = "Selected Count: " + count.ToString("#,##0");
            SelectedSum_label.Text = "Selected Sum: " + sum.ToString("#,##0");
        }

        private static void PrepareKoboExport(DataTable table)
        {
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] labels = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++) labels[i] = table.Columns[i].Caption;
            for (int i = 0; i < table.Columns.Count; i++) table.Columns[i].ColumnName = Guid.NewGuid().ToString("N");
            foreach (DataColumn column in table.Columns)
            {
                string label = labels[column.Ordinal], unique = label;
                int suffix = 2;
                while (!seen.Add(unique)) unique = label + " (" + suffix++ + ")";
                column.ColumnName = unique;
            }
        }

        private void SearchKoboResults(string value, string columnName, bool wholeWord)
        {
            if (string.IsNullOrWhiteSpace(value)) { bs.Filter = Original_Filter_String; return; }
            StringBuilder literal = new StringBuilder();
            foreach (char character in value)
            {
                if (character == '\'') literal.Append("''");
                else if (!wholeWord && (character == '[' || character == ']' || character == '%' || character == '*')) literal.Append('[').Append(character).Append(']');
                else literal.Append(character);
            }
            List<string> expressions = new List<string>();
            foreach (DataGridViewColumn column in Answers_dataGridView.Columns)
            {
                if (!column.Visible || (columnName != null && column.Name != columnName)) continue;
                string field = "Convert([" + column.Name + "], 'System.String')";
                expressions.Add(field + (wholeWord ? " = '" + literal + "'" : " LIKE '%" + literal + "%'"));
            }
            if (expressions.Count == 0) return;
            string search = "(" + string.Join(" OR ", expressions) + ")";
            bs.Filter = string.IsNullOrEmpty(Original_Filter_String) ? search : "(" + Original_Filter_String + ") AND " + search;
        }
    }
}
