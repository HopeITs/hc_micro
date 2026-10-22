# Kobo Person Review

Apply `MyWorkApplication/KoboPersonReview_Upgrade.sql` separately to each local governorate database, in addition to the existing monitoring and conflict upgrades. No application startup migration runs automatically.

Sync again to queue ambiguous beneficiary matches. In the sync screen open **مراجعة التعارضات** then **مطابقة المستفيدين**. Select a source row, compare the Kobo name against the people linked to its project, explicitly select the correct person, enter a reason, and confirm. **القرار لاحقاً** leaves it unlinked. If nobody is suitable, correct the project relationships or source data first; do not choose a random candidate.

Only project beneficiaries and partners are eligible. If a Kobo beneficiary answer contains multiple names, the importer compares complete database first-and-last (or first-father-last) names as whole name segments. If exactly one matching person is the beneficiary and other matches are partners, it selects that beneficiary. Multiple matching beneficiaries or an uncertain name still require review. No aliases, fuzzy selection or hardcoded project exceptions are added.

Visitor recording is separate from beneficiary identification: the Kobo submitter is included; a named choice adds that companion; **لا يوجد مرافق** suppresses only that same-team companion choice; and the separate companion-name field is still read for a person from another team. A yes/no answer that another team attended does not create an unnamed visitor.

Decisions are per submission, not per name. The source JSON hash and chosen project relationship/name snapshot must still match on every import. Changed source data or changed/deleted relationships require fresh review. Existing imported project/person identities cannot be reassigned here. Candidate and source snapshots, actor, UTC time, decision and reason are recorded in `kobo_person_review_audit`; queue/decision state is in `kobo_person_review`.

Confirmation retries only the selected local submission, under the same database sync lock. It does not fetch Kobo, generate notifications, change manual visits, or bypass duplicate-review/overwrite protections. A person decision can succeed while a subsequent duplicate visit still needs separate review.

The SQL upgrades and end-to-end database import need testing against a disposable copy of each governorate database before production use.
