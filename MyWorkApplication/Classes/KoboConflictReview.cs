using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class KoboDuplicateReviewException : InvalidOperationException
    {
        public int ExistingId, VisitId;
        public KoboDuplicateReviewException(string message, int existingId, int visitId) : base(message)
        { ExistingId = existingId; VisitId = visitId; }
    }

    public class KoboConflictReview
    {
        private readonly string database = Program.selected_database;
        private readonly string connectionString = Program.ConnectionString;

        private void CheckContext()
        {
            if (database != Program.selected_database || connectionString != Program.ConnectionString)
                throw new InvalidOperationException("Database changed. Close and reopen Sync Conflicts.");
            if (Program.role != 1 && Program.role != 8 && Program.role != 2)
                throw new UnauthorizedAccessException("Your account cannot review Kobo sync conflicts.");
        }

        internal static DataTable Read(MySqlConnection c, MySqlTransaction t, string sql, params object[] args)
        { return KoboMonitoringImporter.Query(c, t, sql, args); }
        internal static void Write(MySqlConnection c, MySqlTransaction t, string sql, params object[] args)
        { KoboMonitoringImporter.Write(c, t, sql, args); }

        internal static DataTable Pair(MySqlConnection c, MySqlTransaction t, int incoming, int existing)
        {
            DataTable pair = Read(c, t, @"SELECT s.* FROM kobo_submission s
JOIN kobo_form_mapping f ON BINARY f.Kobo_Form_UID = BINARY s.Kobo_Form_ID
WHERE f.LocalDatabase = DATABASE() AND f.Is_Active = 1 AND f.Visit_Type IN ('monitoring','final')
AND s.ID IN (@incoming,@existing) ORDER BY s.ID FOR UPDATE", "@incoming", incoming, "@existing", existing);
            if (incoming == existing || pair.Rows.Count != 2) throw new InvalidOperationException("Conflict sources are missing or outside this database.");
            return pair;
        }

        internal static DataRow Source(DataTable pair, int id)
        {
            foreach (DataRow row in pair.Rows) if (Convert.ToInt32(row["ID"]) == id) return row;
            throw new InvalidOperationException("Missing source submission.");
        }

        internal static bool Current(DataRow conflict, DataTable pair)
        {
            if(Convert.ToInt32(Source(pair,Convert.ToInt32(conflict["Incoming_ID"]))["Kobo_Is_Deleted"])!=0 ||
                Convert.ToInt32(Source(pair,Convert.ToInt32(conflict["Existing_ID"]))["Kobo_Is_Deleted"])!=0) return false;
            return conflict["Incoming_Hash"].ToString() == KoboConflictPolicy.Hash(Source(pair, Convert.ToInt32(conflict["Incoming_ID"]))["Raw_JSON"].ToString()) &&
                conflict["Existing_Hash"].ToString() == KoboConflictPolicy.Hash(Source(pair, Convert.ToInt32(conflict["Existing_ID"]))["Raw_JSON"].ToString());
        }

        public static void Register(int incoming, int existing, int visit)
        {
            using (MySqlConnection c = new MySqlConnection(Program.ConnectionString))
            {
                c.Open(); using (MySqlTransaction t = c.BeginTransaction())
                {
                    DataTable pair = Pair(c, t, incoming, existing);
                    Write(c, t, @"INSERT INTO kobo_sync_conflict
(Incoming_ID,Existing_ID,Visit_ID,Incoming_Hash,Existing_Hash) VALUES(@i,@e,@v,@ih,@eh)
ON DUPLICATE KEY UPDATE ID = ID", "@i", incoming, "@e", existing, "@v", visit,
                        "@ih", KoboConflictPolicy.Hash(Source(pair,incoming)["Raw_JSON"].ToString()),
                        "@eh", KoboConflictPolicy.Hash(Source(pair,existing)["Raw_JSON"].ToString()));
                    t.Commit();
                }
            }
        }

        public DataTable List()
        {
            CheckContext();
            using (MySqlConnection c = new MySqlConnection(connectionString))
            {
                c.Open();
                DataTable rows = Read(c, null, @"SELECT r.*, i.Kobo_Submission_ID Incoming, e.Kobo_Submission_ID Existing,
i.Kobo_Form_Name Form, i.Raw_JSON Incoming_JSON, e.Raw_JSON Existing_JSON,
i.Kobo_Is_Deleted Incoming_Deleted,e.Kobo_Is_Deleted Existing_Deleted,
i.Kobo_Form_ID Incoming_UID,e.Kobo_Form_ID Existing_UID,
i.Error_Message Error, m.Project_Number_Field, p.MicroProject_ID Project, p.Person_ID Person,
v.Number Stage, v.Date Visit_Date
FROM kobo_sync_conflict r
JOIN kobo_submission i ON i.ID=r.Incoming_ID JOIN kobo_submission e ON e.ID=r.Existing_ID
JOIN kobo_form_mapping f ON BINARY f.Kobo_Form_UID=BINARY i.Kobo_Form_ID AND f.LocalDatabase=DATABASE()
LEFT JOIN kobo_monitoring_field_mapping m ON BINARY m.Kobo_Form_UID=BINARY i.Kobo_Form_ID
LEFT JOIN kobo_monitoring_record p ON p.Submission_ID=e.ID
LEFT JOIN mevisit v ON v.ID=r.Visit_ID ORDER BY r.ID DESC");
                rows.Columns.Add("Beneficiary");
                foreach (DataRow row in rows.Rows)
                {
                    string name=KoboVisitorMapping.GetValue(KoboMonitoringImporter.ParseSubmission(row["Incoming_JSON"].ToString()),"bene_name");
                    row["Beneficiary"]=string.IsNullOrWhiteSpace(name)?row["Person"].ToString():name;
                    if (row["Incoming_Hash"].ToString() != KoboConflictPolicy.Hash(row["Incoming_JSON"].ToString()) ||
                        row["Existing_Hash"].ToString() != KoboConflictPolicy.Hash(row["Existing_JSON"].ToString()) ||
                        Convert.ToInt32(row["Incoming_Deleted"])!=0 || Convert.ToInt32(row["Existing_Deleted"])!=0) row["State"] = "stale";
                    else if(row["State"].ToString()=="resolved" && !string.IsNullOrWhiteSpace(row["Error"].ToString())) row["State"]="blocked";
                }
                return rows;
            }
        }

        public DataTable History(int id)
        {
            CheckContext();
            using (MySqlConnection c = new MySqlConnection(connectionString))
            {
                c.Open(); return Read(c,null,@"SELECT Event AS 'الحدث',Decision AS 'القرار',Reason AS 'السبب',
Actor AS 'المستخدم',Actor_User_ID AS 'معرّف المستخدم',Recorded_At AS 'التاريخ (UTC)'
FROM kobo_sync_conflict_audit WHERE Conflict_ID=@id ORDER BY ID DESC", "@id",id);
            }
        }

        public void Decide(int id, string decision, string reason, string incomingHash, string existingHash, int revision)
        {
            CheckContext(); KoboConflictPolicy.ValidateDecision(decision, reason);
            using (MySqlConnection c = new MySqlConnection(connectionString))
            {
                c.Open(); string key = "micro_kobo_visit_sync_" + (string.IsNullOrWhiteSpace(database) ? "unknown" : database.Trim());
                DataTable acquired = Read(c,null,"SELECT GET_LOCK(@key,0) Acquired", "@key",key);
                if (acquired.Rows[0][0] == DBNull.Value || Convert.ToInt32(acquired.Rows[0][0]) != 1)
                    throw new InvalidOperationException("A sync or another review is running. Wait and refresh.");
                try
                {
                    using (MySqlTransaction t = c.BeginTransaction())
                    {
                        DataTable records=Read(c,t,"SELECT * FROM kobo_sync_conflict WHERE ID=@id FOR UPDATE","@id",id);
                        if(records.Rows.Count!=1) throw new InvalidOperationException("Conflict no longer exists.");
                        DataRow conflict=records.Rows[0];
                        if(Convert.ToInt32(conflict["Revision"])!=revision) throw new InvalidOperationException("Another reviewer saved a decision. Refresh before continuing.");
                        bool applied=conflict["Applied_At"]!=DBNull.Value;
                        if(applied && conflict["Decision"].ToString()!=decision)
                            throw new InvalidOperationException("Applied decisions cannot be reversed here. Changed sources may only be re-reviewed under the same decision.");
                        DataTable pair=Pair(c,t,Convert.ToInt32(conflict["Incoming_ID"]),Convert.ToInt32(conflict["Existing_ID"]));
                        DataRow incoming=Source(pair,Convert.ToInt32(conflict["Incoming_ID"]));
                        DataRow existing=Source(pair,Convert.ToInt32(conflict["Existing_ID"]));
                        if(KoboConflictPolicy.Hash(incoming["Raw_JSON"].ToString())!=incomingHash || KoboConflictPolicy.Hash(existing["Raw_JSON"].ToString())!=existingHash)
                            throw new InvalidOperationException("Submission changed since comparison. Refresh and review again.");
                        if(Convert.ToInt32(incoming["Kobo_Is_Deleted"])!=0 || Convert.ToInt32(existing["Kobo_Is_Deleted"])!=0)
                            throw new InvalidOperationException("A source was deleted. No decision applied.");
                        bool existingOwns=existing["Local_Table_Name"].ToString()=="mevisit" && existing["Local_Record_ID"]!=DBNull.Value &&
                            Convert.ToInt32(existing["Local_Record_ID"])==Convert.ToInt32(conflict["Visit_ID"]);
                        bool replacedOwns=decision=="replace" && incoming["Local_Table_Name"].ToString()=="mevisit" &&
                            incoming["Local_Record_ID"]!=DBNull.Value && Convert.ToInt32(incoming["Local_Record_ID"])==Convert.ToInt32(conflict["Visit_ID"]) &&
                            existing["Local_Record_ID"]==DBNull.Value;
                        if((!applied && (incoming["Local_Record_ID"]!=DBNull.Value || !existingOwns)) ||
                            (applied && !(decision=="replace"?replacedOwns:existingOwns)))
                            throw new InvalidOperationException("Visit ownership changed. Refresh and review before continuing.");
                        Write(c,t,@"UPDATE kobo_sync_conflict SET Decision=@d,Reason=@r,State=@s,
Incoming_Hash=@ih,Existing_Hash=@eh,Decided_By=@actor,Decided_User_ID=@user,Decided_At=UTC_TIMESTAMP(),Revision=Revision+1
WHERE ID=@id", "@d",decision,"@r",reason ?? "","@s",applied?"resolved":(decision=="defer"?"deferred":"approved"),
                            "@ih",incomingHash,"@eh",existingHash,"@actor",Program.UserName,"@user",Program.userID,"@id",id);
                        Audit(c,t,id,applied?"re-reviewed":"decision",decision,reason ?? "",incoming["Raw_JSON"].ToString(),existing["Raw_JSON"].ToString());
                        if(decision=="defer") Write(c,t,"UPDATE kobo_submission SET Sync_Status='review_deferred',Error_Message=NULL WHERE ID=@id","@id",conflict["Incoming_ID"]);
                        t.Commit();
                    }
                    // Local-only application, no Kobo API calls, under the same governorate lock.
                    if(decision!="defer")
                    {
                        DataTable selected=Read(c,null,"SELECT Incoming_ID FROM kobo_sync_conflict WHERE ID=@id","@id",id);
                        KoboLocalVisitImportResult result=new KoboMonitoringImporter().ImportConflict(Convert.ToInt32(selected.Rows[0]["Incoming_ID"]));
                        DataTable outcome=Read(c,null,@"SELECT r.State,s.Error_Message FROM kobo_sync_conflict r
JOIN kobo_submission s ON s.ID=r.Incoming_ID WHERE r.ID=@id","@id",id);
                        if(outcome.Rows[0]["State"].ToString()!="resolved" || !string.IsNullOrEmpty(outcome.Rows[0]["Error_Message"].ToString()))
                            throw new InvalidOperationException("Decision saved, but application needs review: " +
                                (string.IsNullOrWhiteSpace(outcome.Rows[0]["Error_Message"].ToString())?result.ErrorMessages:outcome.Rows[0]["Error_Message"].ToString()));
                    }
                }
                finally { Read(c,null,"SELECT RELEASE_LOCK(@key)","@key",key); }
            }
        }

        internal static void Audit(MySqlConnection c, MySqlTransaction t, int id, string action, string decision, string reason, string incoming, string existing)
        {
            Write(c,t,@"INSERT INTO kobo_sync_conflict_audit
(Conflict_ID,Event,Decision,Reason,Actor,Actor_User_ID,Recorded_At,Incoming_JSON,Existing_JSON)
VALUES(@id,@event,@decision,@reason,@actor,@user,UTC_TIMESTAMP(),@incoming,@existing)",
                "@id",id,"@event",action,"@decision",decision,"@reason",reason,"@actor",Program.UserName,
                "@user",Program.userID,"@incoming",incoming,"@existing",existing);
        }
    }
}
