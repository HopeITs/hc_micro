using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyWorkApplication.Classes;
using MyWorkApplication.Properties;
using PresentationControls;

namespace MyWorkApplication
{
    public partial class Search_Form
    {
        private CancellationTokenSource reportCancellation;
        private CancellationTokenSource selectionCancellation;
        private Action<string, int, DataRow> openCityRecord = CityRecordWindow.Open;
        private bool reportBusy;
        private bool reportReadOnly;
        private bool reportReady;
        private string reportCoverageText = "No report loaded";
        private string reportConnection;
        private string[] reportCities;
        private string[] reportAllowedCities;
        private ReportFilterParameters reportParameters = new ReportFilterParameters();
        private readonly Dictionary<Control, bool> reportControlStates = new Dictionary<Control, bool>();
        private CityReportResult reportResult;
        private bool CrossCityReport
        {
            get
            {
                string[] cities = reportingScope.SelectedCities;
                return cities.Length != 1 || cities[0] != Program.selected_database;
            }
        }

        private async Task UpdateSelectedLoanSummaryAsync()
        {
            var cancellation = new CancellationTokenSource();
            selectionCancellation = cancellation;
            try
            {
                await Task.Delay(200, cancellation.Token);
                var rows = MP_dataGridView.SelectedRows.Cast<DataGridViewRow>()
                    .Where(row => row.DataBoundItem is DataRowView).Select(row => ((DataRowView)row.DataBoundItem).Row).ToArray();
                if (rows.Length == 0)
                {
                    if (reportResult != null) BindLoanSummary(reportResult.Combine(true, false).Tables[1]);
                    Financial_tableLayoutPanel.Visible = true;
                    reportingCoverage.Text = reportCoverageText;
                    return;
                }
                Financial_tableLayoutPanel.Visible = false;
                var totals = new CityReportResult { RequestedCities = rows.Select(RecordCity).Distinct().ToArray() };
                foreach (var group in rows.GroupBy(RecordCity))
                {
                    var values = new Dictionary<string, object>();
                    foreach (long id in group.Select(row => Convert.ToInt64(row["ID"])).Distinct()) values.Add("@loan" + values.Count, id);
                    string sql = new Select().BuildLoanSummaryQuery("(" + string.Join(",", values.Keys) + ")", "", true);
                    var result = await new CityReportingService().QueryAsync(reportConnection, reportAllowedCities,
                        new[] { group.Key }, sql, values, cancellation.Token);
                    foreach (var city in result.Cities) totals.Cities.Add(city.Key, city.Value);
                    totals.FailedCities.AddRange(result.FailedCities);
                    foreach (var reason in result.FailureReasons) totals.FailureReasons.Add(reason.Key, reason.Value);
                }
                cancellation.Token.ThrowIfCancellationRequested();
                if (IsDisposed || Disposing || reportBusy || !reportReady) return;
                // These are scalar totals, not category statistics or detail rows.
                var tables = totals.Cities.Values.Select(value => value.Tables[0]).ToArray();
                if (totals.FailedCities.Count == 0 && tables.Length > 0)
                {
                    DataTable sum = tables[0].Clone();
                    foreach (var table in tables) foreach (DataRow row in table.Rows) sum.ImportRow(row);
                    BindLoanSummary(CityReportResult.Sum(sum, null));
                    Financial_tableLayoutPanel.Visible = true;
                }
                reportingCoverage.Text = reportCoverageText + "; Selected-row financial totals: " + totals.Coverage;
                toolTip1.SetToolTip(reportingCoverage, reportingCoverage.Text);
            }
            catch (OperationCanceledException) { }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                {
                    Financial_tableLayoutPanel.Visible = false;
                    reportingCoverage.Text = reportCoverageText + "; Selected-row totals unavailable: " + error.Message;
                }
            }
            finally
            {
                if (ReferenceEquals(selectionCancellation, cancellation)) selectionCancellation = null;
                cancellation.Dispose();
            }
        }

        private int CurrentReportType
        {
            get { return searchBy_list == null ? SearchBy_comboBox.SelectedIndex : searchBy_list.IndexOf(SearchBy_comboBox.Text); }
        }

        private string RecordCity(DataRow row)
        {
            string city = row.Table.Columns.Contains("City") ? Convert.ToString(row["City"]) : reportCities.Single();
            if (!CityReportingService.PermittedCities(Program.User_In_Database).Contains(city))
                throw new InvalidOperationException("You no longer have access to " + city + ".");
            return city;
        }

        private string ReportSelected(CheckBoxComboBox combo)
        {
            return CrossCityReport ? reportParameters.Selected(combo) : Program.getSelectedItems_CheckboxComboBox(combo, true);
        }

        private void BeginReport()
        {
            if (reportingScope.SelectedCities.Length == 0)
                throw new InvalidOperationException("Select at least one city.");
            if (selectionCancellation != null) selectionCancellation.Cancel();
            reportBusy = true;
            reportReady = false;
            reportReadOnly = CrossCityReport;
            reportParameters = new ReportFilterParameters();
            reportCancellation = new CancellationTokenSource();
            reportConnection = Program.ConnectionString;
            reportAllowedCities = CityReportingService.PermittedCities(Program.User_In_Database);
            reportCities = reportingScope.SelectedCities;
            reportControlStates.Clear();
            foreach (Control control in Controls)
            {
                if (control == reportingPanel) continue;
                reportControlStates.Add(control, control.Enabled);
                control.Enabled = false;
            }
            reportingScope.Enabled = false;
            reportingCoverage.Text = "Loading: " + string.Join(", ", reportCities);
        }

        private void EndReport()
        {
            reportBusy = false;
            if (reportCancellation != null) { reportCancellation.Dispose(); reportCancellation = null; }
            if (IsDisposed || Disposing) return;
            foreach (var control in reportControlStates) control.Key.Enabled = control.Value;
            reportingScope.Enabled = true;
            Cursor.Current = Cursors.Default;
            ApplyReportSafety();
            if (reportReady)
                MP_dataGridView_SelectionChanged(this, EventArgs.Empty);
        }

        private async Task<DataSet> ReadReportAsync(string query)
        {
            if (CrossCityReport) query = reportParameters.Range(query);
            CancellationToken token = reportCancellation.Token;
            reportResult = await new CityReportingService().QueryAsync(reportConnection, reportAllowedCities,
                reportCities, query, reportParameters.Values, token);
            token.ThrowIfCancellationRequested();
            if (IsDisposed || Disposing) throw new OperationCanceledException();
            reportCoverageText = reportResult.Coverage;
            reportingCoverage.Text = reportCoverageText;
            toolTip1.SetToolTip(reportingCoverage, reportCoverageText);
            bool allCities = CrossCityReport;
            DataSet data = await Task.Run(() => reportResult.Combine(allCities, false, token), token);
            token.ThrowIfCancellationRequested();
            if (IsDisposed || Disposing) throw new OperationCanceledException();
            if (data.Tables[0].Columns.Contains("ReportGroupId")) data.Tables[0].Columns.Remove("ReportGroupId");
            reportReady = true;
            return data;
        }

        public void Check_Theme()
        {
            if (Settings.Default.theme == "Dark") new NewTheme().Search_ToNight(this, Hided);
            else new NewTheme().Search_ToLight(this, Hided);
            bool dark = Settings.Default.theme == "Dark";
            reportingPanel.BackColor = dark ? NewTheme.light_grey3 : NewTheme.light_grey1;
            reportingCoverage.ForeColor = dark ? NewTheme.light_white : NewTheme.light_grey4;
            reportingScope.BackColor = dark ? NewTheme.dark_grey2 : NewTheme.light_white;
            reportingScope.ForeColor = reportingCoverage.ForeColor;
            reportingCitiesLabel.ForeColor = reportingCoverage.ForeColor;
            reportingScope.Font = SearchBy_comboBox.Font;
            reportingScope.CheckBoxProperties = Category_comboBox.CheckBoxProperties;
            reportingScope.RightToLeft = RightToLeft.No;
        }

        private void ReportingScopeChanged(object sender, EventArgs e)
        {
            if (reportBusy) return;
            timer1.Stop();
            show_images = false;
            ClearReport(reportingScope.SelectedCities.Length == 0 ? "Select at least one city." : "Cities changed — close the list to load.");
            ApplyReportSafety();
        }

        private void ReportingScopeCommitted(object sender, EventArgs e)
        {
            if (searchBy_list == null || reportBusy || reportingScope.SelectedCities.Length == 0) return;
            SearchBy_comboBox_SelectedIndexChanged(sender, e);
        }

        private bool reportResetting;

        private void ResetSearchBinding()
        {
            reportResetting = true;
            try
            {
                Original_Filter_String = "";
                ReportBinding.Clear(bs);
                MP_dataGridView.CleanFilterAndSort();
                MP_dataGridView.DataSource = null;
            }
            finally { reportResetting = false; }
        }

        private void ClearReport(string message)
        {
            if (selectionCancellation != null) selectionCancellation.Cancel();
            reportReady = false;
            reportCoverageText = message;
            if (IsDisposed || Disposing) return;
            reportingCoverage.Text = message;
            ResetSearchBinding();
            Count_label.Text = "All: 0";
            Selected_label.Text = "Selected: 0";
            Sum_FamilyMembers_label.Text = "";
            Financial_tableLayoutPanel.Visible = false;
            visit_statistics_tableLayoutPanel.Visible = false;
        }

        private void ApplyReportSafety()
        {
            MP_dataGridView.ReadOnly = true;
            MP_dataGridView.AllowUserToDeleteRows = false;
            bool readOnly = CrossCityReport || reportReadOnly;
            Delete_button.Enabled = reportReady && !reportBusy && !readOnly && CurrentReportType == 1;
            showHideProfileImagesToolStripMenuItem.Enabled = reportReady && !reportBusy && !readOnly;
            export_applications_ToolStripMenuItem.Enabled = reportReady && !reportBusy;
            ExportToExcel_button.Enabled = reportReady && !reportBusy;
            if (MP_dataGridView.Columns.Contains("StateHistory"))
                MP_dataGridView.Columns["StateHistory"].Visible = !readOnly;
        }

        private void FinishSearchReport()
        {
            if (CrossCityReport && MP_dataGridView.Columns.Contains("City"))
            {
                // An unfrozen column cannot move ahead of the existing frozen project
                // column. Reapply freezing after establishing the combined column order.
                foreach (DataGridViewColumn column in MP_dataGridView.Columns) column.Frozen = false;
                MP_dataGridView.Columns["City"].Visible = true;
                MP_dataGridView.Columns["City"].DisplayIndex = 0;
                MP_dataGridView.Columns["City"].Frozen = true;
                if (MP_dataGridView.Columns.Contains("رقم المشروع")) MP_dataGridView.Columns["رقم المشروع"].Frozen = true;
                DataTable table = (DataTable)bs.DataSource;
                if (table.Columns.Contains("رقم الاجتماع")) bs.Sort = "[تاريخ الاجتماع] DESC, [رقم الاجتماع] DESC, [City] ASC";
                else if (table.Columns.Contains("رقم المشروع")) bs.Sort = "[رقم المشروع] DESC, [City] ASC";
                else bs.Sort = "[City] ASC";
            }
            bs_ListChanged(this, new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, -1));
            if (CrossCityReport && SearchBy_comboBox.Text == "Loans and Payments")
            {
                reportCoverageText += "; financial totals follow selection (all loaded rows when none selected)";
                reportingCoverage.Text = reportCoverageText;
            }
            Check_Theme();
            ApplyReportSafety();
        }

        private static int CountReportRows(DataTable table)
        {
            string id = table.Columns.Contains("رقم المشروع") ? "رقم المشروع" : null;
            if (id == null) return table.Rows.Count;
            return table.DefaultView.ToTable(true, table.Columns.Contains("City") ? new[] { "City", id } : new[] { id }).Rows.Count;
        }

        private string[] ReportIdentityColumns(string id)
        {
            return MP_dataGridView.Columns.Contains("City") ? new[] { "City", id } : new[] { id };
        }

        private void AddReportCoverage(ClosedXML.Excel.XLWorkbook workbook)
        {
            var sheet = workbook.Worksheets.Add("Report coverage");
            sheet.Cell(1, 1).Value = reportCoverageText;
            sheet.Cell(2, 1).Value = "Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sheet.Cell(3, 1).Value = "Scope: " + (reportReadOnly ? "Selected cities (read-only)" : "Current city");
            sheet.Cell(4, 1).Value = "Report: " + SearchBy_comboBox.Text;
            sheet.Column(1).Width = 100;
            sheet.Column(1).Style.Alignment.WrapText = true;
        }

        private async Task ExportSelectedCityApplicationsAsync()
        {
            string coverage = reportCoverageText;
            string connection = reportConnection;
            string[] sourceCities = reportCities;
            string[] sourceAllowedCities = reportAllowedCities;
            DataTable rows = ((DataTable)bs.DataSource).DefaultView.ToTable();
            if (rows.Rows.Count == 0) throw new InvalidOperationException("There are no visible applications to export.");
            if (!rows.Columns.Contains("City"))
            {
                rows.Columns.Add("City", typeof(string));
                foreach (DataRow row in rows.Rows) row["City"] = sourceCities.Single();
            }
            using (var dialog = new SaveFileDialog { Title = "Save Excel File", Filter = "Excel files (*.xlsx)|*.xlsx", ValidateNames = true })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                BeginReport();
                try
                {
                    var token = reportCancellation.Token;
                    CityReportResult exported = await new CityReportingService().ExportApplicationsAsync(connection, sourceAllowedCities, rows, token, Program.UserName);
                    DataSet data = await Task.Run(() => exported.Combine(true, false, token), token);
                    token.ThrowIfCancellationRequested();
                    if (IsDisposed || Disposing) throw new OperationCanceledException();
                    reportCoverageText = coverage + "; Detailed export: " + exported.Coverage;
                    using (var workbook = new ClosedXML.Excel.XLWorkbook { RightToLeft = true })
                    {
                        workbook.Worksheets.Add(data.Tables[0], "Exported from App");
                        AddReportCoverage(workbook);
                        workbook.SaveAs(dialog.FileName);
                    }
                    System.Diagnostics.Process.Start(dialog.FileName);
                    if (exported.FailedCities.Count > 0)
                        MessageBox.Show(this, exported.Coverage, "Incomplete detailed export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (OperationCanceledException) { }
                finally
                {
                    reportCoverageText = coverage;
                    reportConnection = connection;
                    reportCities = sourceCities;
                    reportAllowedCities = sourceAllowedCities;
                    reportReady = true;
                    if (!IsDisposed && !Disposing) reportingCoverage.Text = coverage;
                    EndReport();
                }
            }
        }

    }
}
