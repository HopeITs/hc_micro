# Kobo M&E Import

## Local Installation

Back up the selected governorate database. Apply KoboVisitorMapping_Upgrade.sql, KoboMonitoringStage_Upgrade.sql, then KoboMonitoringImport_Upgrade.sql. Rebuild the application and test sync against a disposable local copy before production. No live migration or sync was run by implementation tests.

Import runs after Opening import under the existing governorate-level sync lock. Scripts seed only the selected database and preserve reviewed configurations on reruns. Configured monitoring/Final forms route to mevisit; existing Closing records are never converted.

## Imported Information

- General monitoring: __010, exact M&E_1 through M&E_4.
- Vehicle monitoring: vist_no, M1-M4.
- Energy monitoring: vist_, currently M1/M2 in supplied definitions.
- Separate Final forms: M4, never M5 or Closing.
- Agriculture/livestock: unnumbered records (Number is empty), without invented M1-M4 stages. Production answers stay in the source response.
- mevisit: beneficiary, kind, date, stage and source ownership.
- mevisit_user: distinct confirmed participant IDs.
- kobo_visit_visitor: ordered names, including former staff and unmatched people.
- kobo_submission: full Raw_JSON and exact local record link.

Questionnaire codes are not yet translated into legacy mevisit_answer question IDs, scores, profit or evaluations. New rows use required legacy numeric defaults, which are placeholders, not extracted financial results. Synced forms hide placeholder profit and expose the original JSON response in the existing Notes area, read-only. Names use existing visitor controls, combining overflow in the second slot. No new interface is created.

## Safety Rules

No tasks, notifications, schedules or future visits are generated. Statistics retain IsVisitor = 1.

A sole eligible project participant continues to be selected by the existing project relationship, regardless of spelling differences in Kobo. Name comparison is only used to distinguish multiple eligible people. When Kobo supplies bene_name for such a project, its full name is compared only to that project's beneficiaries/partners. Database first/last fields are combined; verified father-name formats are also supported. Exactly one person ID must match. Whitespace, Arabic vowel marks, tatweel and directional formatting are cleaned; spelling differences (such as ديما versus ديمة) are not automatically equated. There are no name aliases or fuzzy auto-links. Missing names retain the previous sole-beneficiary/sole-partner fallback. Ambiguous/mismatched multi-person names stay errors. Partner status is preserved. Every successful import stores its explicit project, person, original role, stage, date and optional legacy visit ID in kobo_monitoring_record.

If the person belongs to multiple projects, the record stays ONLY in kobo_monitoring_record and its visitor snapshots, with Local_Table_Name = 'kobo_monitoring_record'. No mevisit row is created. This prevents person-only legacy searches/statistics from assigning the new visit to another project. Such records are not added to legacy visit screens/statistics; review them using the Kobo-only query below. Existing historical legacy records are unchanged.

```sql
SELECT r.*, s.Kobo_Submission_ID, s.Kobo_Form_Name, s.Sync_Status
FROM kobo_monitoring_record r
JOIN kobo_submission s ON s.ID = r.Submission_ID
WHERE r.MicroProject_ID = 828;
```

Repeat sync updates only owned Kobo-linked records. Manual edits, legacy answers/evaluations, changed project/person identities, cross-table links and duplicate beneficiary/date/stage/category matches stop that record with a review error. Duplicate diagnostics list the candidate visit IDs and their linked source submissions/forms, including deleted sources. Created_By = 'Kobo Sync' alone is never accepted as proof that a candidate is the same visit. A general M4 and separate Final on the same date remain flagged rather than merged. Separate dates stay separate source visits.

Invalid/missing dates raise errors instead of becoming today's date. JSON date strings retain the source timezone/calendar day.

Deleted Kobo sources retain local historical visits and produce a review error. Historical source responses stay accessible read-only. Retained deleted visits can still affect reports until reviewed; nothing is silently removed.

## Coverage and Verification

25 supplied definitions cover five governorates. Latakia's general M&E definition was not supplied; it remains raw-only with a missing-mapping error. Companion identities use reviewed labels, not choice codes that may mean different people in different forms. Unknown choices remain named and unlinked.

Run tests/KoboMonitoringStages.Tests.ps1 and tests/KoboMonitoringImport.Tests.ps1 in 32-bit Windows PowerShell against the built executable. Offline checks cover fields, choices, stage validation, source calendar dates and Opening regressions. Live SQL constraints, migration/transaction behavior and rendered forms still require testing against a backed-up local database.
