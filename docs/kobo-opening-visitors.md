# Opening Visit Visitor Mapping

1. For a new installation, run the base `KoboSync_Schema.sql` first.
2. For both new and existing installations, select the correct governorate database in phpMyAdmin and run `MyWorkApplication/KoboVisitorMapping_Upgrade.sql`. Account status and the existing statistics routine remain unchanged: historical visit records show mapped former participants, while statistics retain their IsVisitor = 1 filter.
3. Rebuild the application and run Sync Kobo visits. Existing imported Opening visits are revisited automatically.
4. Open several Opening visits in the existing Other/Vehicle forms and check the visitor names.

The same upgrade can be run separately in Aleppo, Damascus, Homs, Wadi and Latakia. Seeds are filtered by `DATABASE()`; it never selects another database or creates user accounts. Lebanon and Idleb have no supplied Kobo form seeds. Re-running the script preserves assigned user links and disabled mappings, while filling missing links where the named account now exists.

## Stored Data

- `kobo_opening_field_mapping`: exact project, companion, free-text and plate fields for each supplied Opening form UID.
- `kobo_user_mapping`: form-specific companion codes and database-specific account/name aliases. An empty form UID applies the alias across forms in the current database.
- `kobo_visit_visitor`: ordered source values and display names for each locally stored submission, with an optional local user ID.
- `visit_user`: only real, matched local user IDs. No placeholder accounts or negative IDs are inserted.

Unknown names and genuinely missing accounts retain their original name without a user link and do not cause a sync failure. Explicit mappings may link existing Out of Service accounts regardless of IsVisitor. Rawan links to her own account, never Rawia. Sandra, Bashar and Mirvat also link to their existing local accounts, including confirmed spelling variants. Unconfirmed identities remain unlinked.

Opening forms use their existing visitor controls to display the stored Kobo names. Those controls are read-only for synced visitor lists, and saving other visit fields preserves the sync-managed visitor links. Names beyond the fourth slot are combined in the final slot. Manual visits keep their existing visitor selection behavior. No M&E import is added.

## New Users and Name Corrections

After creating the local account normally, add its exact Kobo alias in that governorate database:

```sql
INSERT INTO kobo_user_mapping
    (Kobo_Form_UID, Kobo_Name, Display_Name, User_ID, Is_Ignored, Is_Active)
VALUES
    ('', 'exact_kobo_username', 'Name to display', 123, 0, 1)
ON DUPLICATE KEY UPDATE
    Display_Name = VALUES(Display_Name), User_ID = VALUES(User_ID),
    Is_Ignored = 0, Is_Active = 1;
```

Replace the example values with verified values; use lowercase Kobo usernames/codes. For companion codes, use that form's UID rather than an empty UID. A NULL `User_ID` explicitly keeps a configured name unlinked. Explicit mappings accept historical accounts; automatic exact-username fallback still only considers IsVisitor = 1 accounts. Codes meaning 'no companion' use Is_Ignored = 1.

Run sync again after mapping changes. The combined-companion parser requires the updated application once; subsequent alias edits need no new release. Configured aliases are matched longest-first at name boundaries, including Arabic conjunctions and adjacent full names. Punctuation separates answers. Unrecognized fragments remain visible and unlinked; no fuzzy matching is used. Original submission text remains in Raw_JSON. Removed accounts stay unlinked. Apply the updated upgrade script to each governorate database before testing the new aliases.

Review preserved unlinked names with:

```sql
SELECT DISTINCT s.Kobo_Form_Name, v.Kobo_Name, v.Display_Name
FROM kobo_visit_visitor v
JOIN kobo_submission s ON s.ID = v.Submission_ID
LEFT JOIN `user` u ON u.UserID = v.User_ID
WHERE s.Kobo_Is_Deleted = 0 AND u.UserID IS NULL
ORDER BY s.Kobo_Form_Name, v.Display_Name;
```

This query is for review; unlinked names alone do not indicate failed imports. Actual import/API/database errors still count as sync errors.

## Verification

`tests/KoboOpeningVisitors.Tests.ps1` tests the supplied 16 Opening definitions, all choice-code mappings, account aliases, former employees, Rawan/Rawia separation, deleted accounts, duplicate visitors and display overflow without connecting to a database. Run it in 32-bit Windows PowerShell against a built executable, matching the application's runtime.

The SQL upgrade and live screens still require testing against the local governorate database. Do not import the supplied partial `user.sql` or error exports as migrations; they are reference data only.
