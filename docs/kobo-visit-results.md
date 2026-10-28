# Kobo Visit Results

The existing **Visit Results** dropdown now has four modes. The first two remain the original legacy reports. The new modes are:

- **عدد إجابات كل سؤال (كوبو)**: a combined frequency table for choice questions across all selected visits and forms. Column order is question, all answer options in numeric order, all corresponding counts in numeric order, then total answers. This ordering includes every option beyond three; horizontal scrolling is available when needed.
- **إجابات المستفيدين (كوبو)**: one combined table with one row per confirmed source submission, with project, person, visit date/stage, category/career, and the saved questionnaire answers in shared question columns. Technical form name, form-version ID and submission-ID columns are omitted from both the grid and Excel export. They remain in the stored source records and the separate source-response viewer.

Beneficiary columns follow the original screen's compact layout: project number, beneficiary, subcategory, career, visit stage/number, visit date, then readable questionnaire columns. Question headers wrap within a fixed 126px header band; question columns are 145px wide with full human-readable tooltips. Entirely empty question columns are omitted for the current selection, but visit rows are never removed for having unanswered questions. These omissions also apply to Excel export.

The default is **جميع نماذج كوبو**, not the first available form. The existing Show button reveals the usual filters plus optional form/version selectors; no form selection is required to see the combined table. Existing date, project, visit-stage, last-visit, grid search/filter, and Excel export controls are reused. Entering Kobo mode clears the legacy project/stage selections; returning restores them. Refresh reloads saved submissions from the selected governorate database. It does not contact Kobo or perform a sync.

## Shared Questions

Both Kobo result modes exclude the audited basic-information groups (`group_mo5jr41`, `group_vv6uu79`), the vehicle-identification group (`group_vz9zl11`), and visit-number/date questions. These contain donor, project/person identifiers, occupation/address/GPS, companions and other-team attendance, vehicle type/card/plate identifiers, and the duplicate visit-stage question. The ordinary project, beneficiary, date, stage, category and career context columns remain. Assessment questions about location impact, services, debts, profit, employment, project conditions, agriculture and livestock are retained; this is not a broad keyword exclusion. Newly encountered raw fields inside the excluded groups also stay out of the result grids. Original responses and sync/visitor mappings remain unchanged and inspectable in the separate source-response viewer.

The legacy mode calls `count_mevisit_results` / `beneficiaries_me_answers`, which report against shared legacy question/answer IDs. Kobo has form-specific paths and codes instead. The Kobo modes construct a read-only shared question view from the original definitions and saved JSON, without inserting translations into legacy answer tables or creating a table for each form.

Identical question wording and compatible question types share one report question even when field paths and choice-list names differ. Whitespace, invisible direction marks, Arabic vowel marks/tatweel and trailing question punctuation are normalized; different wording/spelling is not guessed to match. Direct numeric/text answers can share one question, while single-choice and multiple-choice formats remain distinct. Choice codes are translated using each source form's own definition; matching answer labels then share a count. Options offered by only one form remain in the union of options. Generic follow-ups such as "لماذا؟" use their parent question, and repeated fields use their section, to distinguish context. Equivalent contexts can share a column across forms; unresolved ambiguities remain separate with readable headers, never form IDs or field paths. Unknown choice codes stay form-specific and visibly identify their form.

## Source And Identity

Reports read `kobo_submission.Raw_JSON`; they do not translate answers into `mevisit_answer`, edit local visits, resolve conflicts, or change visitor statistics.

Monitoring records use the explicit submission/project/person relationship in `kobo_monitoring_record`, with the matching local link. A person associated with several projects is never joined to all those projects. Opening records use their linked `visit` project; their visit dates come from each source JSON rather than a potentially shared legacy opening row. Local project category/career and linked person names are current database values; original questionnaire values remain separate answer columns.

Eligible submissions have `local_inserted` or `local_updated` status, are not deleted, and have a confirmed project/visit link. Errors, pending imports, deferred/ignored/replaced sources, and unresolved links are excluded. Malformed JSON and missing/invalid opening source dates are reported separately. The exclusions are form/version-level diagnostics; the confirmed count also applies the chosen date and project filters. These totals describe saved Kobo submissions, not all legacy visits or staff participations.

Each source ID counts once, even if a join supplies a repeated row. Different confirmed source IDs remain separate visits, including an explicitly approved "keep both" pair on the same project/stage/date. Last-visit mode keeps the latest source per project/person across all selected forms, within the version and other filters, using source ID to break same-date ties. It does not choose an extra latest visit for every form.

## Answer Counts

- Answered plus blank equals the confirmed source count from forms that define that question. Submissions from unrelated forms that never ask it are not counted as blanks. Blank includes questions skipped by form conditions or absent from an older version of a form in the bundled snapshot; it is not an answer of "No".
- Single-choice counts use the stored choice code. Multiple-choice options count at most once per submission; adding all option counts can exceed the number of answered submissions.
- Numeric and free-text questions appear with their original values in beneficiary mode. The compact frequency report contains choice questions, not invented answer categories for arbitrary text or continuous numeric values.
- Repeat-group answers stay in the source row, separated by ` | `, and do not multiply visits. Repeated occurrences of one option count once for that source.

## Question Definitions

`KoboQuestionCatalog.json` bundles ordered field paths, types, Arabic labels, section/parent-question context, and choice codes/labels extracted from the 41 supplied XLSForm definitions. Label extraction checks populated language cells per row and prefers actual Arabic text, including Arabic text stored in an English-labelled column when the Arabic column is blank. Extraction fails instead of silently generating an empty form. All available options are supported, not just three. Notes, calculations, hidden fields and transport metadata are not questionnaire rows.

Reading each source uses exact field paths/codes, with no guessed aliases. Shared report columns then match by question wording/type and translated answer labels as described above. Undefined fields do not become technical column headers in the clean reports; a unique-field diagnostic count is shown in the report's optional summary panel, and original values remain inspectable in the separate source-response viewer. Unknown historical choice codes for defined questions remain visible as form-specific codes. The snapshot is not a complete archive of every historical label: a code reused with a different meaning in an older form version requires that version's definition before claiming a historically exact label. Use the version selector and source-response viewer when reviewing such cases.

To regenerate the catalog from updated local XLSForm exports, use `tests/Export-KoboQuestionCatalog.ps1`. This is a development step, not a runtime dependency on the Downloads folder.

## Verification

No new database migration is introduced. Existing Kobo sync and monitoring-registry upgrades must already be applied. The original legacy queries are unchanged.

If the report says `kobo_monitoring_record` is missing, the selected database has not completed the monitoring-import setup. Back it up, then apply `KoboVisitorMapping_Upgrade.sql`, `KoboMonitoringStage_Upgrade.sql`, and `KoboMonitoringImport_Upgrade.sql` in that order to that same database. Re-sync and refresh the report afterward. The report checks for the registry before querying and provides these instructions in Arabic; it does not create an empty registry automatically or guess project/person links from legacy visits. Each governorate database needs its own upgrade.

`tests/KoboVisitResults.Tests.ps1` exercises catalog consistency, eligibility, identity, dates, cross-form question aggregation, different codes for the same answer, form-specific unknown codes, question-specific denominators, option counts, repeats, cross-form latest selection, filters, literal search, export headings, return to legacy mode, and Designer rendering at 1000px/1500px widths without a database. Live database query execution and end-to-end sync reconciliation still need to be checked against the local test database before real-data rollout.

## Loading Performance

Grid loading is batched: binding notifications, repeated header/row measurements, layout passes and ADGV filter initialization are deferred while columns are configured. Selection totals are calculated once afterward and use numeric parsing without throwing exceptions for ordinary text cells. The legacy modes keep their original selection behavior.

`tests/KoboVisitResults.Performance.ps1 -AssemblyPath <built-exe> -Visits 1000 -WithGrid` generates repeatable synthetic responses from the supplied Aleppo definitions and times calculation separately from rendering. A local before/after comparison measured summary grid loading at 13,221 ms vs 1,464 ms and beneficiary grid loading at 5,363 ms vs 1,853 ms. Calculation itself took approximately 0.1-0.2 seconds. Timings are observations, not CI thresholds, and exclude database/network time. No records are loaded from or written to a database by the benchmark.
