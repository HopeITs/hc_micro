# Kobo Submissions Viewer

Open **إجابات كوبو** from the Kobo sync window. The viewer reads the selected governorate database and does not edit submissions, local visits, or statistics.

The upper grid lists submissions, form, submission time, submitter, sync status, local record link, and any import error. Search checks submission ID, form name, submitter, and error text. Status filters distinguish errors, imported records, unlinked active records, and deleted Kobo submissions.

Selecting a submission loads its saved `Raw_JSON` and displays non-metadata answers in the lower grid. Nested fields and repeat-group rows stay distinguishable. Question and choice labels come from the bundled `KoboMonitoringFieldLabels.json` when a mapping exists; an unmapped field displays its Kobo field path and original response. Choice responses include the stored code with the human label. This is a source-response viewer, not a translation into legacy `mevisit_answer` records.

No database upgrade is needed. A database must already contain `kobo_submission`, which the Kobo sync setup creates. Offline parsing coverage is in `tests/KoboSubmissionViewer.Tests.ps1`; visual RTL behavior and a live local database still need testing in Visual Studio.
