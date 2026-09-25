using MySql.Data.MySqlClient;
using MyWorkApplication.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    private static int assertions;
    private static void Assert(bool value, string message)
    {
        if (!value) throw new Exception(message);
        assertions++;
    }

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            TestCombining();
            TestParameters();
            TestApplicationExport();
            string joins = " FROM microproject MP LEFT JOIN person P ON P.ProjectId = MP.MP_ID LEFT JOIN microproject_state mps ON mps.ProjectId = MP.MP_ID ";
            System.IO.File.WriteAllLines(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "statistics-queries.sql"), new[] {
                StatisticsReportQuery.Build("MP.Category", "Category", "MP.Category", joins, " WHERE mps.Current_State = 1 "),
                StatisticsReportQuery.Build("MP.Category", "Category", "MP.Category", joins, " WHERE mps.Current_State = 1 ", true),
                StatisticsReportQuery.Build("MP.Category", "Category", "MP.Category", joins, " WHERE mps.Current_State = 1 AND MP.Category = 1 ") });
            Task.Run(() => TestExecution()).GetAwaiter().GetResult();
            if (args.Length > 0) TestForms(args[0]);
            Console.WriteLine("PASS: " + assertions + " reporting assertions; no live database used.");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }

    private static DataSet Detail(int id, decimal total, bool empty = false)
    {
        var data = new DataSet();
        var table = data.Tables.Add("Rows");
        table.Columns.Add("ID", typeof(int));
        table.PrimaryKey = new[] { table.Columns[0] };
        if (!empty) table.Rows.Add(id);
        var summary = data.Tables.Add("Summary");
        summary.Columns.Add("Total", typeof(decimal));
        summary.Rows.Add(total);
        return data;
    }

    private static DataSet Statistics(string label, int id, long count, long funded)
    {
        var data = new DataSet();
        var table = data.Tables.Add("Statistics");
        table.Columns.Add("Label", typeof(string));
        table.Columns.Add("numOfProjects", typeof(long));
        table.Columns.Add("numOfFundedProjects", typeof(long));
        table.Columns.Add("ReportGroupId", typeof(int));
        table.Rows.Add(label, count, funded, id);
        return data;
    }

    private static void TestCombining()
    {
        var report = new CityReportResult();
        report.Cities.Add("Aleppo", Detail(123, 12.75m));
        report.Cities.Add("Homs", Detail(123, 4.50m));
        var combined = report.Combine(true, false);
        Assert(combined.Tables[0].Rows.Count == 2, "Overlapping IDs must survive merging.");
        Assert((string)combined.Tables[0].Rows[1]["City"] == "Homs", "Rows must retain source city.");
        Assert((decimal)combined.Tables[1].Rows[0][0] == 17.25m, "Scalar summaries must add without losing precision.");
        Assert(combined.Tables[0].DefaultView.ToTable(true, "City", "ID").Rows.Count == 2, "Distinct identity includes city.");
        combined.Tables[0].DefaultView.Sort = "City DESC";
        Assert((string)combined.Tables[0].DefaultView[0]["City"] == "Homs", "Global sorting must operate on combined rows.");
        combined.Tables[0].DefaultView.RowFilter = "City = 'Homs'";
        Assert(combined.Tables[0].DefaultView.Count == 1, "City filtering must preserve correct rows.");
        Assert(report.Cities["Aleppo"].Tables[0].Columns.Count == 1, "Merging must not mutate city inputs.");

        var stats = new CityReportResult();
        stats.Cities.Add("Aleppo", Statistics("Category", 1, 10, 2));
        stats.Cities["Aleppo"].Tables[0].Rows.Add("Category", 3L, 1L, 2);
        stats.Cities.Add("Homs", Statistics("Category translated", 1, 30, 18));
        DataTable totals = stats.Combine(false, true).Tables[0];
        Assert(totals.Rows.Count == 2, "Different IDs sharing a label must remain distinct.");
        Assert((decimal)totals.Rows[0][1] == 40 && (decimal)totals.Rows[0][2] == 20, "Same category ID must aggregate despite label differences.");
        Assert((decimal)totals.Rows[0][2] / (decimal)totals.Rows[0][1] == 0.5m, "Percentage denominator must be the combined total.");
        var empty = new CityReportResult();
        empty.Cities.Add("Aleppo", Detail(0, 0, true));
        Assert(empty.Combine(true, false).Tables[0].Columns.Contains("City"), "Empty results retain city schema.");
        Assert(empty.Combine(true, false).Tables[0].Rows.Count == 0, "Empty results must not invent records.");
        var allFailed = new CityReportResult();
        allFailed.FailedCities.Add("Aleppo");
        try { allFailed.Combine(true, false); throw new Exception("Expected no-data failure."); }
        catch (InvalidOperationException) { assertions++; }
    }

    private static void TestParameters()
    {
        var filterRows = new DataTable();
        filterRows.Columns.Add("Name", typeof(string));
        filterRows.Columns.Add("City", typeof(string));
        filterRows.Rows.Add("O'Brien [50%]*", "Homs");
        filterRows.Rows.Add("Other", "Aleppo");
        filterRows.DefaultView.RowFilter = ReportFilterParameters.GridText(new[] { "Name", "City" }, "O'Brien [50%]*", false, "City = 'Homs'");
        Assert(filterRows.DefaultView.Count == 1, "Search text must safely match apostrophes and literal wildcard/bracket characters.");
        filterRows.DefaultView.RowFilter = ReportFilterParameters.GridText(new[] { "Name", "City" }, "Other", true, "City = 'Homs'");
        Assert(filterRows.DefaultView.Count == 0, "Text search must retain the existing city/grid filter.");

        var parameters = new ReportFilterParameters();
        using (var combo = new PresentationControls.CheckBoxComboBox())
        {
            combo.Items.Add("O'Brien");
            foreach (var item in combo.CheckBoxItems) if (item.Text == "O'Brien") item.Checked = true;
            string selected = parameters.Selected(combo);
            Assert(!selected.Contains("O'Brien") && parameters.Values.Values.Contains("O'Brien"), "Apostrophes must be bound as values, never interpolated.");
        }
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            string sql = parameters.Range("amount BETWEEN 1,25 and 8,50 AND date BETWEEN '2026/01/01' and '2026/09/23'");
            Assert(!sql.Contains("2026/") && !sql.Contains("1,25"), "Dates and amount endpoints must be parameterized.");
            Assert(parameters.Values.Values.Contains(1.25m) && parameters.Values.Values.Contains(8.50m), "Localized decimal endpoints must keep their value.");
            Assert(parameters.Values.Values.Contains(new DateTime(2026, 9, 23)), "Date endpoint must be a typed date.");
        }
        finally { CultureInfo.CurrentCulture = previous; }
    }

    private static async Task TestExecution()
    {
        var timeout = new System.IO.IOException("transport", new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.TimedOut));
        Assert(CityReportingService.DescribeFailure(timeout).Contains("timed out"), "Nested socket timeout must be described accurately.");
        var failedService = new CityReportingService((connection, city, sql, parameters, token) => { throw timeout; });
        var failed = await failedService.QueryAsync("unused", new[] { "Aleppo" }, new[] { "Aleppo" }, "SELECT 1", new Dictionary<string, object>(), CancellationToken.None);
        Assert(failed.Coverage.Contains("Aleppo") && failed.Coverage.Contains("timed out") && failed.Cities.Count == 0, "Transport failure must become a named incomplete report.");
        try { failed.Combine(true, false); throw new Exception("All-failed report should reject aggregation."); }
        catch (InvalidOperationException) { assertions++; }
        var cities = CityReportingService.Databases.Keys.ToArray();
        int active = 0, maxActive = 0, called = 0;
        var service = new CityReportingService((connection, city, sql, parameters, token) =>
        {
            Interlocked.Increment(ref called);
            int concurrent = Interlocked.Increment(ref active);
            lock (cities) maxActive = Math.Max(maxActive, concurrent);
            try
            {
                Thread.Sleep(40);
                token.ThrowIfCancellationRequested();
                if (city == "Homs") throw (MySqlException)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlException));
                return Detail(123, 1);
            }
            finally { Interlocked.Decrement(ref active); }
        });
        var result = await service.QueryAsync("unchanged", cities, cities, "SELECT 1", new Dictionary<string, object>(), CancellationToken.None);
        Assert(maxActive <= 2 && maxActive > 1, "Queries must have bounded parallelism.");
        Assert(result.FailedCities.SequenceEqual(new[] { "Homs" }), "Failed city must be identified.");
        Assert(result.Coverage.StartsWith("Incomplete") && result.Coverage.Contains("Homs"), "Partial results must be explicitly labelled.");
        Assert(result.Combine(true, false).Tables[0].Rows.Count == cities.Length - 1, "Only successful cities contribute rows.");
        int previousCalls = called;
        try
        {
            await service.QueryAsync("unchanged", new[] { "Aleppo" }, new[] { "Homs" }, "SELECT 1", new Dictionary<string, object>(), CancellationToken.None);
            throw new Exception("Expected authorization rejection.");
        }
        catch (InvalidOperationException) { Assert(called == previousCalls, "Unauthorized city must not be queried."); }
        Assert(CityReportingService.PermittedCities(new[] { "Aleppo", "unknown", "Aleppo" }).SequenceEqual(new[] { "Aleppo" }), "City whitelist must reject unknown entries and duplicates.");
        using (var cancellation = new CancellationTokenSource())
        {
            cancellation.CancelAfter(10);
            try
            {
                await service.QueryAsync("unchanged", cities, cities, "SELECT 1", new Dictionary<string, object>(), cancellation.Token);
                throw new Exception("Expected cancellation.");
            }
            catch (OperationCanceledException) { assertions++; }
        }
        Assert(active == 0, "Cancellation must wait for workers to stop before completing.");

        var applicationRows = new DataTable();
        applicationRows.Columns.Add("City", typeof(string));
        applicationRows.Columns.Add("ID", typeof(long));
        for (int i = 1; i <= 1001; i++) applicationRows.Rows.Add("Aleppo", (long)i);
        applicationRows.Rows.Add("Homs", 1L);
        int exportCalls = 0;
        var batchedExporter = new CityReportingService((connection, city, sql, values, token) =>
        {
            Interlocked.Increment(ref exportCalls);
            if (sql != "select_fullapplication_details") throw new Exception("Export must use the stored-procedure command path.");
            string filter = (string)values["@MyCondition"];
            string[] ids = filter.Substring(filter.IndexOf('(') + 1).TrimEnd(')').Split(',');
            if (ids.Length > 500) throw new Exception("Export batch exceeded its limit.");
            var ds = new DataSet();
            var details = ds.Tables.Add("Details");
            details.Columns.Add("Application", typeof(long));
            details.Columns.Add("Notes", typeof(string));
            foreach (string id in ids) details.Rows.Add(long.Parse(id), city + " details");
            var extra = ds.Tables.Add("ProcedureMetadata");
            extra.Columns.Add("Message", typeof(string));
            extra.Rows.Add("This must not be summed as a financial total.");
            return ds;
        });
        var allApplications = await batchedExporter.ExportApplicationsAsync("unused", new[] { "Aleppo", "Homs" }, applicationRows, CancellationToken.None);
        var exportedDetails = allApplications.Combine(true, false);
        Assert(exportCalls == 4, "1001 applications must be split into three bounded batches, plus the other city.");
        Assert(exportedDetails.Tables.Count == 1 && exportedDetails.Tables[0].Rows.Count == 1002, "Detailed exports must preserve every city's records and ignore auxiliary procedure results.");
        Assert(exportedDetails.Tables[0].Select("Application = 1").Length == 2, "Overlapping application IDs must remain separate in detailed export.");

        var cityValues = new Dictionary<string, IDictionary<string, object>>
        {
            { "Aleppo", new Dictionary<string, object> { { "@MyCondition", "WHERE PMP_ID IN (1,2)" } } },
            { "Homs", new Dictionary<string, object> { { "@MyCondition", "WHERE PMP_ID IN (1,3)" } } }
        };
        var exportService = new CityReportingService((connection, city, sql, parameters, token) =>
        {
            string expected = city == "Aleppo" ? "WHERE PMP_ID IN (1,2)" : "WHERE PMP_ID IN (1,3)";
            if ((string)parameters["@MyCondition"] != expected) throw new Exception("Export parameters crossed city boundaries.");
            return Detail(123, 1);
        });
        var exported = await exportService.QueryAsync("unchanged", cityValues.Keys, cityValues.Keys,
            "CALL select_fullapplication_details(@MyCondition)", new Dictionary<string, object>(), CancellationToken.None, cityValues);
        Assert(exported.Cities.Count == 2, "Detailed exports must send each city's own parameters.");
    }

    private static void TestApplicationExport()
    {
        var rows = new DataTable();
        rows.Columns.Add("City", typeof(string));
        rows.Columns.Add("ID", typeof(long));
        rows.Rows.Add("Aleppo", 123L);
        rows.Rows.Add("Aleppo", 124L);
        rows.Rows.Add("Homs", 123L);
        rows.Rows.Add("Homs", 125L);
        rows.DefaultView.RowFilter = "ID <> 124";
        var parameters = ReportFilterParameters.ForApplicationExport(rows.DefaultView.ToTable(), new[] { "Aleppo", "Homs", "Damascus" });
        Assert((string)parameters["Aleppo"]["@MyCondition"] == " WHERE Current_State = 1 AND PMP_ID IN (123)", "Detailed export must honor grid filtering.");
        Assert((string)parameters["Homs"]["@MyCondition"] == " WHERE Current_State = 1 AND PMP_ID IN (123,125)", "Same ID in another city must retain its source.");
        Assert((string)parameters["Damascus"]["@MyCondition"] == " WHERE 1 = 0 ", "A city without visible rows must export no records.");
    }

    private static void TestForms(string applicationPath)
    {
        var assembly = System.Reflection.Assembly.LoadFrom(System.IO.Path.GetFullPath(applicationPath));
        var globals = assembly.GetType("MyWorkApplication.Program", true);
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
        globals.GetField("User_In_Database").SetValue(null, new List<string> { "Aleppo", "Homs" });
        globals.GetField("selected_database").SetValue(null, "Aleppo");
        globals.GetField("ConnectionString").SetValue(null, "Server=localhost;Database=hcsyria_micro");
        object connectionBefore = globals.GetField("MyConn").GetValue(null);
        foreach (string name in new[] { "Search_Form", "Statistics_Form" })
        {
            Type formType = assembly.GetType("MyWorkApplication." + name, true);
            using (var form = (System.Windows.Forms.Form)Activator.CreateInstance(formType, new object[] { null }))
            {
                var scope = (System.Windows.Forms.ComboBox)formType.GetField("reportingScope", flags).GetValue(form);
                var citiesProperty = scope.GetType().GetProperty("SelectedCities");
                Assert(((string[])citiesProperty.GetValue(scope)).SequenceEqual(new[] { "Aleppo" }), name + " must default to current city.");
                Assert(scope.Text.Contains("Aleppo"), "Checked cities must also appear in the collapsed selector: index=" + scope.SelectedIndex + "; text=" + scope.Text + "; items=" + string.Join("|", scope.Items.Cast<object>()));
                Assert(scope.RightToLeft == System.Windows.Forms.RightToLeft.No && scope.FlatStyle == System.Windows.Forms.FlatStyle.Flat, "City selector must match flat styling and be LTR.");
                Assert(formType.GetField("reportingCancel", flags) == null, "Cancel button must be removed.");
                var cityItems = ((System.Collections.IEnumerable)scope.GetType().GetProperty("CheckBoxItems").GetValue(scope)).Cast<object>().ToArray();
                Action<string, bool> checkCity = (city, check) =>
                {
                    object item = cityItems.Single(i => Equals(i.GetType().GetProperty("ComboBoxItem").GetValue(i), city));
                    item.GetType().GetProperty("Checked").SetValue(item, check);
                };
                checkCity("All permitted cities", true);
                Assert(((string[])citiesProperty.GetValue(scope)).SequenceEqual(new[] { "Aleppo", "Homs" }), "Select-all must select only permitted cities.");
                checkCity("Aleppo", false);
                Assert(((string[])citiesProperty.GetValue(scope)).SequenceEqual(new[] { "Homs" }), "A single other city must be selectable.");
                Assert((bool)formType.GetProperty("CrossCityReport", flags).GetValue(form), "A single non-active city must be read-only.");
                checkCity("Aleppo", true);
                Assert(scope.Text.Contains("Aleppo") && scope.Text.Contains("Homs"), "Collapsed selector must display multiple checked cities.");
                int commits = 0;
                EventHandler committed = (sender, e) => commits++;
                scope.GetType().GetEvent("CitiesCommitted").AddEventHandler(scope, committed);
                scope.GetType().GetMethod("CommitSelection", flags).Invoke(scope, null);
                scope.GetType().GetMethod("CommitSelection", flags).Invoke(scope, null);
                Assert(commits == 1, "Several checkbox changes must commit exactly one refresh.");
                formType.GetMethod("BeginReport", flags).Invoke(form, null);
                Assert(!(bool)scope.Enabled, "Scope changes must be disabled during a load.");
                string[] actions = name == "Search_Form"
                    ? new[] { "Delete_button_Click", "MP_dataGridView_CellContentClick", "MP_dataGridView_RowHeaderMouseClick", "MP_dataGridView_SelectionChanged", "showHideProfileImagesToolStripMenuItem_Click" }
                    : new[] { "showDetailsToolStripMenuItem_Click" };
                foreach (string action in actions)
                    formType.GetMethod(action, flags).Invoke(form, new object[] { null, null });
                Assert(ReferenceEquals(connectionBefore, globals.GetField("MyConn").GetValue(null)), "Read-only actions must not replace the global connection.");
                Assert((string)globals.GetField("selected_database").GetValue(null) == "Aleppo", "Read-only actions must not switch city.");
                var token = (CancellationTokenSource)formType.GetField("reportCancellation", flags).GetValue(form);
                token.Cancel();
                Assert(token.IsCancellationRequested, "Cancel must signal the report token.");
                formType.GetMethod("EndReport", flags).Invoke(form, null);
                Assert(scope.Enabled, "Scope must be restored after cancellation.");
                form.PerformLayout();
                var panel = (System.Windows.Forms.Control)formType.GetField("tableLayoutPanel1", flags).GetValue(form);
                Assert(scope.Top >= 0 && scope.Bottom <= panel.Height, "City selector must fit vertically in the reporting bar.");
                var primarySelector = (System.Windows.Forms.ComboBox)formType.GetField(name == "Search_Form" ? "SearchBy_comboBox" : "X_Axis_comboBox", flags).GetValue(form);
                Assert(scope.Parent == primarySelector.Parent && scope.Width >= 150, "City selector must fit alongside the primary selector, without a column-span collision.");
                Assert(Math.Abs((scope.Top + scope.Height / 2) - (primarySelector.Top + primarySelector.Height / 2)) <= 2, "City and report selectors must align vertically.");
                using (var bitmap = new System.Drawing.Bitmap(panel.Width, panel.Height))
                {
                    panel.Parent = null;
                    panel.CreateControl();
                    Assert(scope.Text.Contains("Aleppo") && scope.Text.Contains("Homs"), "Creating the native city control must preserve displayed selection.");
                    panel.DrawToBitmap(bitmap, panel.ClientRectangle);
                    bitmap.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + "-reporting-bar.png"));
                    panel.Parent = form;
                }
                var export = (System.Windows.Forms.Button)formType.GetField("ExportToExcel_button", flags).GetValue(form);
                Assert(!export.Enabled, "Cancelled / unready reports cannot be exported.");
                if (name == "Search_Form")
                {
                    var delete = (System.Windows.Forms.Button)formType.GetField("Delete_button", flags).GetValue(form);
                    Assert(!delete.Enabled, "Unready reports cannot delete records.");
                    var grid = (System.Windows.Forms.DataGridView)formType.GetField("MP_dataGridView", flags).GetValue(form);
                    var binding = (System.Windows.Forms.BindingSource)formType.GetField("bs", flags).GetValue(form);
                    var table = new DataTable();
                    table.Columns.Add("رقم المشروع", typeof(int));
                    table.Columns.Add("City", typeof(string));
                    table.Rows.Add(123, "Aleppo");
                    table.Rows.Add(123, "Homs");
                    binding.DataSource = table;
                    grid.DataSource = binding;
                    grid.Columns["رقم المشروع"].Frozen = true;
                    formType.GetMethod("FinishSearchReport", flags).Invoke(form, null);
                    Assert(grid.Columns["City"].Visible && grid.Columns["City"].DisplayIndex == 0, "City must be visible ahead of the frozen project column.");
                    var count = (System.Windows.Forms.Label)formType.GetField("Count_label", flags).GetValue(form);
                    Assert(count.Text.Contains("2"), "UI counts must include overlapping project IDs from both cities.");
                    grid.ClearSelection();
                    grid.Rows[0].Selected = true;
                    grid.Rows[1].Selected = true;
                    formType.GetMethod("MP_dataGridView_SelectionChanged", flags).Invoke(form, new object[] { null, EventArgs.Empty });
                    var selected = (System.Windows.Forms.Label)formType.GetField("Selected_label", flags).GetValue(form);
                    Assert(selected.Text == "Selected: 2", "Selected rows in different cities must both count, even with the same ID.");
                    grid.ClearSelection();
                    formType.GetMethod("MP_dataGridView_SelectionChanged", flags).Invoke(form, new object[] { null, EventArgs.Empty });
                    Assert(selected.Text == "Selected: 0", "Clearing selection must reset the count.");
                    var searchBy = (System.Windows.Forms.ComboBox)formType.GetField("SearchBy_comboBox", flags).GetValue(form);
                    searchBy.Items.Add("Applications");
                    formType.GetField("reportBusy", flags).SetValue(form, true);
                    searchBy.SelectedIndex = 0;
                    formType.GetField("reportBusy", flags).SetValue(form, false);
                    formType.GetField("reportReady", flags).SetValue(form, true);
                    string routedCity = null;
                    Action<string, int, DataRow> route = (city, reportType, record) => routedCity = city;
                    formType.GetField("openCityRecord", flags).SetValue(form, route);
                    var homsRow = grid.Rows.Cast<System.Windows.Forms.DataGridViewRow>().Single(row => (string)row.Cells["City"].Value == "Homs");
                    formType.GetMethod("MP_dataGridView_RowHeaderMouseClick", flags).Invoke(form, new object[] { grid, new System.Windows.Forms.DataGridViewCellMouseEventArgs(-1, homsRow.Index, 0, 0, new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0)) });
                    Assert(routedCity == null, "Read-only cross-city reports must not open an editable city window.");
                    searchBy.Items.Add("Family Members");
                    formType.GetField("reportBusy", flags).SetValue(form, true);
                    searchBy.SelectedIndex = 1;
                    formType.GetField("reportBusy", flags).SetValue(form, false);
                    formType.GetMethod("ApplyReportSafety", flags).Invoke(form, null);
                    Assert(!delete.Enabled, "Loaded cross-city family reports must disable deletion.");
                    formType.GetMethod("Delete_button_Click", flags).Invoke(form, new object[] { null, EventArgs.Empty });
                    Assert(export.Enabled, "Viewed-data export must remain available in a ready cross-city report.");
                    var fullExport = (System.Windows.Forms.ToolStripMenuItem)formType.GetField("export_applications_ToolStripMenuItem", flags).GetValue(form);
                    Assert(fullExport.Enabled, "Full application export must remain available in a ready cross-city report.");
                    Assert((string)globals.GetField("selected_database").GetValue(null) == "Aleppo", "Opening another city must preserve the active city.");
                    formType.GetField("reportBusy", flags).SetValue(form, true);
                    searchBy.Items.Add("Attachments");
                    searchBy.SelectedIndex = 2;
                    formType.GetField("searchBy_list", flags).SetValue(form, new List<string> { "Applications", "Family Members", "Loans and Payments", "Attachments" });
                    formType.GetField("reportBusy", flags).SetValue(form, false);
                    Assert((int)formType.GetProperty("CurrentReportType", flags).GetValue(form) == 3, "Role-filtered dropdown positions must not change record types.");
                    binding.Sort = "City ASC";
                    binding.Filter = "City = 'Homs'";
                    formType.GetMethod("ClearReport", flags).Invoke(form, new object[] { "Test reset" });
                    Assert(binding.DataSource == null && string.IsNullOrEmpty(binding.Sort) && string.IsNullOrEmpty(binding.Filter), "Sorted and filtered reports must clear without the IBindingList exception.");
                    var replacement = new DataTable();
                    replacement.Columns.Add("DifferentColumn", typeof(int));
                    binding.DataSource = replacement;
                    Assert(binding.Count == 0, "A different report schema must bind after clearing.");
                }
                else
                {
                    var grid = (System.Windows.Forms.DataGridView)formType.GetField("results_dataGridView", flags).GetValue(form);
                    var axis = (System.Windows.Forms.ComboBox)formType.GetField("X_Axis_comboBox", flags).GetValue(form);
                    var chart = (System.Windows.Forms.DataVisualization.Charting.Chart)formType.GetField("chart1", flags).GetValue(form);
                    formType.GetField("user_mode", flags).SetValue(form, false);
                    for (int i = 0; i < 15; i++) axis.Items.Add("Axis " + i);
                    var table = new DataTable();
                    table.Columns.Add("Category", typeof(string));
                    table.Columns.Add("numOfProjects", typeof(decimal));
                    table.Columns.Add("numOfFundedProjects", typeof(decimal));
                    table.Rows.Add("A", 3000000000m, 2000000000m);
                    grid.DataSource = table;
                    foreach (int axisIndex in new[] { 0, 11, 14 })
                    {
                        axis.SelectedIndex = axisIndex;
                        foreach (var type in new[] { System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column, System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut })
                        {
                            formType.GetField("Chart_type", flags).SetValue(form, type);
                            formType.GetMethod("Fill_ChartArea", flags).Invoke(form, new object[] { "Test" });
                            formType.GetMethod("Fill_ChartArea", flags).Invoke(form, new object[] { "Test" });
                            Assert(chart.Series.Count == (axisIndex == 11 ? 1 : 2), "Single/two-series chart must render without stale series.");
                            Assert(chart.Legends.Count == (type == System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut && axisIndex != 11 ? 2 : 1), "Repeated rendering must not accumulate legends.");
                            Assert(chart.Series[0].Points[0].YValues[0] == 3000000000d, "Chart totals must not overflow Int32.");
                            using (var image = new System.IO.MemoryStream())
                                chart.SaveImage(image, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                        }
                    }
                    formType.GetMethod("ClearReport", flags).Invoke(form, new object[] { "Test failure" });
                    formType.GetMethod("WithFunded_checkBox_CheckStateChanged", flags).Invoke(form, new object[] { null, EventArgs.Empty });
                    Assert(chart.Series.Count == 0 && chart.Legends.Count == 0 && grid.DataSource == null, "Failure must clear stale chart and rows safely.");
                }
            }
        }
    }
}
