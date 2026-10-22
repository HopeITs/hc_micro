using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    internal static class KoboConflictImport
    {
        private static DataTable Decisions(MySqlConnection c, MySqlTransaction t, int source)
        {
            try { return KoboConflictReview.Read(c,t,@"SELECT * FROM kobo_sync_conflict
WHERE (Incoming_ID=@id OR Existing_ID=@id) AND Decision IS NOT NULL FOR UPDATE","@id",source); }
            catch(MySqlException ex) { if(ex.Number==1146) return new DataTable(); throw; }
        }

        internal static bool Before(MySqlConnection c, MySqlTransaction t, int source)
        {
            foreach(DataRow r in Decisions(c,t,source).Rows)
            {
                int incoming=Convert.ToInt32(r["Incoming_ID"]), existing=Convert.ToInt32(r["Existing_ID"]);
                DataTable pair=KoboConflictReview.Pair(c,t,incoming,existing);
                if(!KoboConflictReview.Current(r,pair))
                    throw new InvalidOperationException("Reviewed conflict data changed. Open Sync Conflicts and review again.");
                string decision=r["Decision"].ToString();
                if(existing==source && decision=="replace" && r["Applied_At"]!=DBNull.Value)
                {
                    KoboConflictReview.Write(c,t,"UPDATE kobo_submission SET Sync_Status='review_replaced', Error_Message=NULL WHERE ID=@id","@id",source);
                    return true;
                }
                if(incoming!=source || (decision!="ignore" && decision!="defer")) continue;
                KoboConflictReview.Write(c,t,"UPDATE kobo_submission SET Sync_Status=@status,Error_Message=NULL WHERE ID=@id",
                    "@status",decision=="ignore"?"review_ignored":"review_deferred","@id",source);
                if(decision=="ignore") Finish(c,t,source);
                return true;
            }
            return false;
        }

        internal static void PrepareReplacement(MySqlConnection c, MySqlTransaction t, int source,
            int project, int person, string number, string kind, DateTime date)
        {
            foreach(DataRow r in Decisions(c,t,source).Rows)
            {
                if(Convert.ToInt32(r["Incoming_ID"])!=source || r["Decision"].ToString()!="replace" || r["Applied_At"]!=DBNull.Value) continue;
                int existing=Convert.ToInt32(r["Existing_ID"]), visit=Convert.ToInt32(r["Visit_ID"]);
                DataTable pair=KoboConflictReview.Pair(c,t,source,existing);
                if(!KoboConflictReview.Current(r,pair)) throw new InvalidOperationException("Conflict comparison is stale.");
                DataRow owner=KoboConflictReview.Source(pair,existing);
                if(owner["Local_Table_Name"].ToString()!="mevisit" || owner["Local_Record_ID"]==DBNull.Value ||
                    Convert.ToInt32(owner["Local_Record_ID"])!=visit || Convert.ToInt32(owner["Kobo_Is_Deleted"])!=0)
                    throw new InvalidOperationException("Existing source ownership changed; replacement stopped.");
                DataTable identity=KoboConflictReview.Read(c,t,@"SELECT * FROM kobo_monitoring_record
WHERE Submission_ID=@id AND Legacy_Visit_ID=@visit FOR UPDATE","@id",existing,"@visit",visit);
                if(identity.Rows.Count!=1 || Convert.ToInt32(identity.Rows[0]["MicroProject_ID"])!=project ||
                    Convert.ToInt32(identity.Rows[0]["Person_ID"])!=person || identity.Rows[0]["Visit_Number"].ToString()!=number ||
                    identity.Rows[0]["Visit_Kind"].ToString()!=kind || Convert.ToDateTime(identity.Rows[0]["Visit_Date"]).Date!=date.Date)
                    throw new InvalidOperationException("Replacement requires the same verified project, person, stage, category and date.");
                KoboConflictReview.Write(c,t,"UPDATE kobo_monitoring_record SET Legacy_Visit_ID=NULL WHERE Submission_ID=@id","@id",existing);
                KoboConflictReview.Write(c,t,@"UPDATE kobo_submission SET Local_Table_Name=NULL,Local_Record_ID=NULL,
Sync_Status='review_replaced',Error_Message=NULL WHERE ID=@id","@id",existing);
                KoboConflictReview.Write(c,t,"UPDATE kobo_submission SET Local_Table_Name='mevisit',Local_Record_ID=@visit WHERE ID=@id",
                    "@visit",visit,"@id",source);
            }
        }

        internal static bool AllowsDuplicate(MySqlConnection c, MySqlTransaction t, int source, int visit)
        {
            foreach(DataRow r in Decisions(c,t,source).Rows)
            {
                if(r["Decision"].ToString()!="keep_both") continue;
                int incoming=Convert.ToInt32(r["Incoming_ID"]), existing=Convert.ToInt32(r["Existing_ID"]);
                DataTable pair=KoboConflictReview.Pair(c,t,incoming,existing);
                if(!KoboConflictReview.Current(r,pair)) continue;
                if(source==incoming && visit==Convert.ToInt32(r["Visit_ID"])) return true;
                DataRow added=KoboConflictReview.Source(pair,incoming);
                if(source==existing && added["Local_Table_Name"].ToString()=="mevisit" && added["Local_Record_ID"]!=DBNull.Value &&
                    visit==Convert.ToInt32(added["Local_Record_ID"])) return true;
            }
            return false;
        }

        internal static void Finish(MySqlConnection c, MySqlTransaction t, int source)
        {
            foreach(DataRow r in Decisions(c,t,source).Rows)
            {
                if(Convert.ToInt32(r["Incoming_ID"])!=source || r["Applied_At"]!=DBNull.Value || r["Decision"].ToString()=="defer") continue;
                DataTable pair=KoboConflictReview.Pair(c,t,source,Convert.ToInt32(r["Existing_ID"]));
                if(!KoboConflictReview.Current(r,pair)) throw new InvalidOperationException("Conflict changed before application.");
                KoboConflictReview.Write(c,t,"UPDATE kobo_sync_conflict SET State='resolved',Applied_At=UTC_TIMESTAMP() WHERE ID=@id","@id",r["ID"]);
                KoboConflictReview.Audit(c,t,Convert.ToInt32(r["ID"]),"applied",r["Decision"].ToString(),r["Reason"].ToString(),
                    KoboConflictReview.Source(pair,source)["Raw_JSON"].ToString(), KoboConflictReview.Source(pair,Convert.ToInt32(r["Existing_ID"]))["Raw_JSON"].ToString());
            }
        }
    }
}
