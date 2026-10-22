# Sync Conflicts

Apply KoboSyncConflicts_Upgrade.sql to the selected governorate database after the existing monitoring upgrades. Rebuild, sync once to populate the queue, then choose مراجعة التعارضات on the Kobo sync form. The database still needs a backup and disposable-copy testing before production.

## Review

The top grid identifies the project, participant, stage/date and source submission IDs. The comparison tab lists source fields and values side by side; missing fields differ from empty strings and JSON null. A differences-only filter is available. The history tab shows who decided what, when (UTC), and why.

Only a single, verifiable Kobo-owned candidate enters this queue. Manual visits, multiple candidate owners and unresolved beneficiary/project matches remain ordinary errors, not automatic replacement candidates.

- Replace: incoming source takes ownership of the same local visit ID; the former source response and visitor snapshots remain stored. The former source is excluded from subsequent imports. Project, person, stage, category, date and registry ownership must match; manual edits, legacy answers/evaluations remain protected.
- Keep both: create a separate visit, then allow only this reviewed duplicate pair on subsequent syncs. Both visits count independently.
- Ignore: keep the source and audit, but do not create a local visit. Future syncs continue excluding it.
- Decide later: retain an unresolved/deferred review without changing visits.

Every non-deferred decision needs a reason and explicit confirmation. Approving applies only the selected incoming submission locally; it does not fetch Kobo data, generate future visits or notifications, or change statistics procedures. If application fails, the recorded approval and error remain visible rather than reporting success.

## Concurrency and Evidence

Review uses the same per-governorate advisory lock as sync. A revision check rejects a second reviewer saving over another decision. Both JSON snapshots are re-hashed immediately before approval/application; changed or deleted sources require renewed review. Applied decisions cannot be reversed here. Changed responses may be re-reviewed under the same decision, without repeating ownership transfer or duplicating the visit.

The decision and full source snapshots are appended to kobo_sync_conflict_audit. Application and its audit event commit together with the visit changes. Both raw submissions remain in kobo_submission. Replacement releases only the former source's legacy link; it does not delete raw data or the historical project registry record.

Access follows the existing Sync Kobo roles (Admin, L0 Admin, Data), checked in the service as well as the entry point. The screen captures its database context and refuses operations after a database switch.

## Verification

Offline policy tests cover source comparison, missing/null fields, stable hashes, changed/deleted evidence, valid decisions, required reasons and Designer control creation/rendering at two sizes. Existing import, identity and stage regression suites must pass.

Live transaction/migration behavior, two-user contention, rollback after failed replacement and real-data permissions still require a disposable database test. No real conflict decisions were applied by the offline tests.
