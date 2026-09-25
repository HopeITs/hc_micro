# City reporting checks

This dependency-free .NET Framework 4.8 console suite uses synthetic city data. It never connects to a database. Build the application solution first, then run:

```powershell
& 'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe' tests/CityReporting.Tests/CityReporting.Tests.csproj /t:Build /v:minimal
& './tests/CityReporting.Tests/bin/CityReporting.Tests.exe' 'MyWorkApplication/bin/Debug/Micro Projects.exe'
```

The optional application path enables form-construction and action-guard checks. The tests cover overlapping record IDs, global sorting and filtering, category IDs versus labels, scalar totals, empty/partial results, authorization, concurrency, cancellation, parameter values, and preservation of the active connection/city.

## Live acceptance checks

Use a test account authorized for at least two city databases with identical schemas and lookup IDs.

1. Open Search and Statistics. Confirm the active city is checked by default. Check individual cities or **All permitted cities**, then close the popup and compare results with independent city reports using identical filters.
2. Exercise every Search category, including loans, visits, checklists, and profession meetings. Use date, amount, and lookup filters. Projects sharing the same ID in two cities must count separately.
3. Check all statistics axes and chart types. Counts must equal city totals for the same lookup ID. Pie/doughnut proportions must use combined counts.
4. Select rows in multiple cities and verify selected counts, including identical IDs. Verify deletion and navigation to editable forms are blocked even when handlers are invoked directly. Confirm viewed-data exports in Search/Statistics and full application exports in Search remain available. Switch to only the active city and confirm normal actions are restored.
5. Export filtered search rows, detailed applications, and statistics. Confirm search exports include City even if that grid column was hidden, and all workbooks include a Report coverage worksheet. Detailed application exports must query the selected IDs in their original cities, including when two cities share the same ID. This export reuses the existing `select_fullapplication_details` procedure in each city.
6. In cross-city loan reports, selected-loan totals must be queried using City + Loan ID. With no rows selected, totals cover the loaded report query. Verify SP and USD totals separately and verify that rapid selection changes cannot display stale totals.
7. Make one test city unavailable, refresh, and verify **Incomplete**, the failed city name, and the same coverage in exports. If every city fails, previous rows/charts must disappear and export must be disabled.
8. Close a tab while loading to cancel its work. Reopen afterward and select only the active city. Confirm no stale data appears and other open forms retain their active city.
9. Check light/dark themes, Arabic labels, the reporting bar at supported window sizes, and Visual Studio Designer loading.

City access comes from the existing login session's `Program.User_In_Database` list. A city that was unavailable during login is not in that session's permitted list; sign in again after it becomes available. Reporting uses the existing server credentials with a separate connection per city; no schema migration is required.

The city selector uses the existing checkbox combobox style, left-to-right. Empty selection clears the report. Selection commits once when the popup closes; there is no Cancel button. Closing/disposal still cancels in-flight work. Failed cities include a connection/query failure reason in coverage and exports.

The suite also checks sorted-binding reset, a different replacement schema, checkbox selection/commit behavior, nested socket timeouts, and chart rebuilding with large totals. After running the console suite, run `python tests/CityReporting.Tests/test_statistics_sql.py` to check generated aggregate SQL with independent fixtures (SQLite COUNT/CASE semantics; live MySQL compatibility and timings require the acceptance checks above).

Statistics computes total/funded counts in one grouped query instead of correlated joined queries for each bucket. Changing chart type redraws cached data. Reporting connections use pooling and at most two concurrent workers. This reduces repeated work; it cannot repair an unavailable server.

## Cross-city read-only behavior

Cross-city Search is read-only again, including selection of a single city other than the active city. Row navigation to editors, family-member deletion and state-history/profile actions are blocked. Selected-row counts, filters, sorting, charts and exports remain available. Current-city Search keeps its normal behavior. Statistics stays read-only, with viewed-data export available. Both viewed-column and full application exports remain available in Search.

Detailed application export uses the stored-procedure command path, splits ID lists into batches of 500, and excludes auxiliary procedure resultsets from numeric aggregation. Test with over 500 applications, identical IDs in different cities, filtered and empty grids, a missing procedure, denied export role, lost connection, and a locked destination workbook. The source snapshot and per-city coverage must remain intact after an export failure.

The city selector shares the first row with Search By / X-Axis. Check light/dark themes and the supported display scaling. Automated checks validate row counts, role-filtered report-type routing, city isolation, export batching, and selector alignment; live MySQL procedures, current-city writes still require a test environment.
