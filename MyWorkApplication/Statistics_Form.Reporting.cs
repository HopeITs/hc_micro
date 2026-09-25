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
    public partial class Statistics_Form
    {
        private CancellationTokenSource reportCancellation;
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

        private string ReportSelected(CheckBoxComboBox combo)
        {
            return CrossCityReport ? reportParameters.Selected(combo) : Program.getSelectedItems_CheckboxComboBox(combo, true);
        }

        private void BeginReport()
        {
            if (reportingScope.SelectedCities.Length == 0)
                throw new InvalidOperationException("Select at least one city.");
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
            bool allCities = CrossCityReport;
            DataSet data = await Task.Run(() => reportResult.Combine(allCities, true, token), token);
            token.ThrowIfCancellationRequested();
            if (IsDisposed || Disposing) throw new OperationCanceledException();
            if (data.Tables[0].Columns.Contains("ReportGroupId")) data.Tables[0].Columns.Remove("ReportGroupId");
            reportReady = true;
            return data;
        }

        public void Check_Theme()
        {
            if (Settings.Default.theme == "Dark") new NewTheme().Statistics_ToNight(this, Hided);
            else new NewTheme().Statistics_ToLight(this, Hided);
            bool dark = Settings.Default.theme == "Dark";
            reportingPanel.BackColor = dark ? NewTheme.light_grey3 : NewTheme.light_grey1;
            reportingCoverage.ForeColor = dark ? NewTheme.light_white : NewTheme.light_grey4;
            reportingScope.BackColor = dark ? NewTheme.dark_grey2 : NewTheme.light_white;
            reportingScope.ForeColor = reportingCoverage.ForeColor;
            reportingCitiesLabel.ForeColor = reportingCoverage.ForeColor;
            reportingScope.Font = X_Axis_comboBox.Font;
            reportingScope.CheckBoxProperties = Category_comboBox.CheckBoxProperties;
            reportingScope.RightToLeft = RightToLeft.No;
        }

        private void ReportingScopeChanged(object sender, EventArgs e)
        {
            if (reportBusy) return;
            ClearReport(reportingScope.SelectedCities.Length == 0 ? "Select at least one city." : "Cities changed — close the list to load.");
            ApplyReportSafety();
        }

        private void ReportingScopeCommitted(object sender, EventArgs e)
        {
            if (!user_mode || reportBusy || reportingScope.SelectedCities.Length == 0) return;
            X_Axis_comboBox_SelectedIndexChanged(sender, e);
        }

        private void ClearReport(string message)
        {
            reportReady = false;
            reportCoverageText = message;
            if (IsDisposed || Disposing) return;
            reportingCoverage.Text = message;
            results_dataGridView.DataSource = null;
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.ChartAreas.Clear();
            Count_all_label.Text = Count_funded_label.Text = "";
        }

        private void ApplyReportSafety()
        {
            results_dataGridView.ReadOnly = true;
            results_dataGridView.AllowUserToDeleteRows = false;
            ExportToExcel_button.Enabled = reportReady && !reportBusy;
            Check_Theme();
        }

    }
}
