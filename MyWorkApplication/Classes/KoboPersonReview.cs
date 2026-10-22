using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class KoboPersonReviewRequiredException : InvalidOperationException
    {
        public readonly int Project;
        public readonly string Name;
        public KoboPersonReviewRequiredException(int project, string name)
            : base("تعذر تحديد المستفيد بشكل مؤكد. افتح مراجعة التعارضات ثم مطابقة المستفيدين لاختيار الشخص المرتبط بالمشروع.")
        { Project = project; Name = name; }
    }

    public class KoboPersonReview
    {
        private readonly string database = Program.selected_database;
        private readonly string connectionString = Program.ConnectionString;
        private void CheckContext()
        {
            if (database != Program.selected_database || connectionString != Program.ConnectionString)
                throw new InvalidOperationException("تغيرت قاعدة البيانات. أغلق الشاشة وافتحها مجدداً.");
            if (Program.role != 1 && Program.role != 8 && Program.role != 2)
                throw new UnauthorizedAccessException("ليس لديك صلاحية اعتماد المطابقة.");
        }

        internal static DataTable Candidates(MySqlConnection c, MySqlTransaction t, int project)
        {
            return KoboMonitoringImporter.Query(c,t,@"SELECT DISTINCT pmp.Person_ID,pmp.PersonType,
p.P_FirstName,p.P_FatherName,p.P_LastName FROM person_microproject pmp
LEFT JOIN person p ON p.P_ID=pmp.Person_ID WHERE pmp.MicroProject_ID=@p
AND pmp.Person_ID>0 AND TRIM(pmp.PersonType) IN ('مستفيد','شريك') ORDER BY pmp.Person_ID,pmp.PersonType", "@p",project);
        }
        public static string RelationshipHash(DataRow row)
        {
            return KoboConflictPolicy.Hash(JsonConvert.SerializeObject(new object[] {
                row["Person_ID"],row["PersonType"],row["P_FirstName"],row["P_FatherName"],row["P_LastName"] }));
        }
        public static bool ChoiceIsCurrent(DataRow review, int project, string sourceHash, DataTable candidates)
        {
            if (Convert.ToInt32(review["Project_ID"]) != project || review["Source_Hash"].ToString()!=sourceHash ||
                (review["State"].ToString()!="confirmed" && review["State"].ToString()!="auto_resolved") ||
                review["Person_ID"]==DBNull.Value) return false;
            foreach(DataRow candidate in candidates.Rows)
                if(Convert.ToInt32(candidate["Person_ID"])==Convert.ToInt32(review["Person_ID"]) &&
                    RelationshipHash(candidate)==review["Relationship_Hash"].ToString()) return true;
            return false;
        }
        internal static DataRow Resolve(MySqlConnection c, MySqlTransaction t, int submission, int project,
            string name, string raw, DataTable candidates)
        {
            DataTable saved;
            try { saved=KoboMonitoringImporter.Query(c,t,"SELECT * FROM kobo_person_review WHERE Submission_ID=@s FOR UPDATE","@s",submission); }
            catch(MySqlException ex) { if(ex.Number!=1146) throw; saved=new DataTable(); }
            if(saved.Rows.Count>0)
            {
                if(ChoiceIsCurrent(saved.Rows[0],project,KoboConflictPolicy.Hash(raw),candidates))
                    foreach(DataRow candidate in candidates.Rows)
                        if(Convert.ToInt32(candidate["Person_ID"])==Convert.ToInt32(saved.Rows[0]["Person_ID"]) &&
                            RelationshipHash(candidate)==saved.Rows[0]["Relationship_Hash"].ToString()) return candidate;
                if(saved.Rows[0]["State"].ToString()=="pending")
                {
                    DataRow automatic;
                    try { automatic=KoboMonitoringIdentity.ResolveNamedParticipant(candidates,name); }
                    catch(InvalidOperationException) { throw new KoboPersonReviewRequiredException(project,name); }
                    string relation=RelationshipHash(automatic), sourceHash=KoboConflictPolicy.Hash(raw);
                    KoboMonitoringImporter.Write(c,t,@"UPDATE kobo_person_review SET Project_ID=@p,Source_Hash=@h,Kobo_Name=@n,
Person_ID=@person,Relationship_Hash=@rh,State='auto_resolved',Revision=Revision+1 WHERE Submission_ID=@s",
                        "@p",project,"@h",sourceHash,"@n",name,"@person",Convert.ToInt32(automatic["Person_ID"]),"@rh",relation,"@s",submission);
                    KoboMonitoringImporter.Write(c,t,@"INSERT INTO kobo_person_review_audit
(Submission_ID,Project_ID,Person_ID,Decision,Reason,Actor,Actor_ID,Recorded_UTC,Source_Hash,Relationship_Hash,Kobo_Name,Candidate_JSON)
VALUES(@s,@p,@person,'auto_resolved',@r,@a,@u,UTC_TIMESTAMP(),@h,@rh,@n,@j)",
                        "@s",submission,"@p",project,"@person",Convert.ToInt32(automatic["Person_ID"]),
                        "@r","تمت مطابقة الاسم الواضح مع مستفيد واحد فقط ضمن أسماء الأشخاص المذكورين في إجابة كوبو.",
                        "@a",Program.UserName,"@u",Program.userID,"@h",sourceHash,"@rh",relation,"@n",name,
                        "@j",JsonConvert.SerializeObject(candidates));
                    return automatic;
                }
                throw new KoboPersonReviewRequiredException(project,name);
            }
            try { return KoboMonitoringIdentity.ResolveNamedParticipant(candidates,name); }
            catch(InvalidOperationException) { throw new KoboPersonReviewRequiredException(project,name); }
        }
        internal static void Register(int submission,int project,string name,string raw)
        {
            using(MySqlConnection c=new MySqlConnection(Program.ConnectionString))
            {
                c.Open();
                KoboMonitoringImporter.Write(c,null,@"INSERT INTO kobo_person_review
(Submission_ID,Project_ID,Source_Hash,Kobo_Name) VALUES(@s,@p,@h,@n)
ON DUPLICATE KEY UPDATE State=IF(State='confirmed','stale',State),Revision=Revision+1",
                    "@s",submission,"@p",project,"@h",KoboConflictPolicy.Hash(raw),"@n",name);
            }
        }
        private static DataRow Source(MySqlConnection c,MySqlTransaction t,int submission)
        {
            DataTable rows=KoboMonitoringImporter.Query(c,t,@"SELECT s.*,m.Project_Number_Field
FROM kobo_submission s JOIN kobo_form_mapping f ON BINARY f.Kobo_Form_UID=BINARY s.Kobo_Form_ID
JOIN kobo_monitoring_field_mapping m ON BINARY m.Kobo_Form_UID=BINARY s.Kobo_Form_ID
WHERE s.ID=@s AND f.LocalDatabase=DATABASE() AND f.Is_Active=1
AND f.Visit_Type IN ('monitoring','final') AND s.Kobo_Is_Deleted=0 FOR UPDATE","@s",submission);
            if(rows.Rows.Count!=1) throw new InvalidOperationException("المصدر محذوف أو غير متاح لهذه المحافظة.");
            return rows.Rows[0];
        }
        public DataTable List()
        {
            CheckContext();
            using(MySqlConnection c=new MySqlConnection(connectionString))
            {
                c.Open(); return KoboMonitoringImporter.Query(c,null,@"SELECT r.Submission_ID,r.Project_ID,r.Kobo_Name,r.State,s.Error_Message
FROM kobo_person_review r JOIN kobo_submission s ON s.ID=r.Submission_ID
WHERE s.Kobo_Is_Deleted=0 AND r.State IN ('pending','deferred','stale','confirmed')
AND (r.State<>'confirmed' OR s.Error_Message IS NOT NULL) ORDER BY r.Submission_ID DESC");
            }
        }
        public DataTable Load(int submission,out int project,out string name,out string hash,out int revision)
        {
            CheckContext();
            using(MySqlConnection c=new MySqlConnection(connectionString))
            {
                c.Open(); using(MySqlTransaction t=c.BeginTransaction())
                {
                    DataRow source=Source(c,t,submission);
                    string raw=source["Raw_JSON"].ToString();
                    Newtonsoft.Json.Linq.JObject data=KoboMonitoringImporter.ParseSubmission(raw);
                    if(!int.TryParse(KoboVisitorMapping.GetValue(data,source["Project_Number_Field"].ToString()),out project)||project<=0)
                        throw new InvalidOperationException("رقم المشروع في المصدر غير صالح.");
                    name=KoboVisitorMapping.GetValue(data,"group_mo5jr41/bene_name"); hash=KoboConflictPolicy.Hash(raw);
                    DataTable review=KoboMonitoringImporter.Query(c,t,"SELECT Revision FROM kobo_person_review WHERE Submission_ID=@s FOR UPDATE","@s",submission);
                    if(review.Rows.Count!=1) throw new InvalidOperationException("الحالة غير موجودة. أعد تحميل القائمة.");
                    revision=Convert.ToInt32(review.Rows[0][0]);
                    DataTable candidates=Candidates(c,t,project);
                    candidates.ExtendedProperties["Form"]=source["Kobo_Form_Name"].ToString();
                    candidates.ExtendedProperties["Kobo_ID"]=source["Kobo_Submission_ID"].ToString();
                    candidates.ExtendedProperties["Date"]=KoboMonitoringImporter.ResolveDate(data).ToString("yyyy-MM-dd");
                    t.Commit(); return candidates;
                }
            }
        }
        public string Decide(int submission,int project,string hash,int revision,int? person,string relationshipHash,string reason)
        {
            CheckContext();
            if(person.HasValue && string.IsNullOrWhiteSpace(reason)) throw new InvalidOperationException("يرجى كتابة سبب اختيار الشخص.");
            using(MySqlConnection c=new MySqlConnection(connectionString))
            {
                c.Open(); string key="micro_kobo_visit_sync_"+(string.IsNullOrWhiteSpace(database)?"unknown":database.Trim());
                if(Convert.ToInt32(KoboMonitoringImporter.Query(c,null,"SELECT GET_LOCK(@k,0)","@k",key).Rows[0][0])!=1)
                    throw new InvalidOperationException("توجد مزامنة قيد التنفيذ. حاول لاحقاً.");
                try
                {
                    using(MySqlTransaction t=c.BeginTransaction())
                    {
                        DataRow source=Source(c,t,submission);
                        string raw=source["Raw_JSON"].ToString();
                        Newtonsoft.Json.Linq.JObject data=KoboMonitoringImporter.ParseSubmission(raw);
                        int currentProject;
                        if(KoboConflictPolicy.Hash(raw)!=hash || !int.TryParse(KoboVisitorMapping.GetValue(data,source["Project_Number_Field"].ToString()),out currentProject) || currentProject!=project)
                            throw new InvalidOperationException("تغير المصدر. أعد تحميل الحالة ومراجعتها.");
                        DataTable review=KoboMonitoringImporter.Query(c,t,"SELECT * FROM kobo_person_review WHERE Submission_ID=@s FOR UPDATE","@s",submission);
                        if(review.Rows.Count!=1 || Convert.ToInt32(review.Rows[0]["Revision"])!=revision)
                            throw new InvalidOperationException("تغير القرار. أعد تحميل الحالة.");
                        DataTable candidates=Candidates(c,t,project); DataRow chosen=null;
                        foreach(DataRow candidate in candidates.Rows)
                            if(person.HasValue && Convert.ToInt32(candidate["Person_ID"])==person.Value && RelationshipHash(candidate)==relationshipHash) chosen=candidate;
                        if(person.HasValue && chosen==null) throw new InvalidOperationException("تغيرت بيانات الشخص أو علاقته بالمشروع. أعد تحميل الحالة.");
                        DataTable identity=KoboMonitoringImporter.Query(c,t,"SELECT Person_ID,MicroProject_ID FROM kobo_monitoring_record WHERE Submission_ID=@s FOR UPDATE","@s",submission);
                        if(person.HasValue && identity.Rows.Count>0 && (Convert.ToInt32(identity.Rows[0]["Person_ID"])!=person.Value || Convert.ToInt32(identity.Rows[0]["MicroProject_ID"])!=project))
                            throw new InvalidOperationException("للزيارة ربط سابق بشخص أو مشروع مختلف. لا يمكن تغييره من هذه الشاشة؛ راجع مسؤول النظام.");
                        string name=KoboVisitorMapping.GetValue(data,"group_mo5jr41/bene_name");
                        string decision=person.HasValue?"confirmed":"deferred";
                        KoboMonitoringImporter.Write(c,t,@"UPDATE kobo_person_review SET Project_ID=@p,Source_Hash=@h,Kobo_Name=@n,
Person_ID=@person,Relationship_Hash=@rh,State=@state,Revision=Revision+1 WHERE Submission_ID=@s",
                            "@p",project,"@h",hash,"@n",name,"@person",person.HasValue?(object)person.Value:DBNull.Value,"@rh",person.HasValue?(object)relationshipHash:DBNull.Value,"@state",decision,"@s",submission);
                        KoboMonitoringImporter.Write(c,t,@"INSERT INTO kobo_person_review_audit
(Submission_ID,Project_ID,Person_ID,Decision,Reason,Actor,Actor_ID,Recorded_UTC,Source_Hash,Relationship_Hash,Kobo_Name,Candidate_JSON)
VALUES(@s,@p,@person,@d,@r,@a,@u,UTC_TIMESTAMP(),@h,@rh,@n,@j)",
                            "@s",submission,"@p",project,"@person",person.HasValue?(object)person.Value:DBNull.Value,"@d",decision,"@r",reason??"","@a",Program.UserName,"@u",Program.userID,"@h",hash,"@rh",relationshipHash??"","@n",name,"@j",JsonConvert.SerializeObject(candidates));
                        t.Commit();
                    }
                    if(!person.HasValue) return "تم تأجيل القرار دون ربط الزيارة.";
                    KoboLocalVisitImportResult result=new KoboMonitoringImporter().ImportConflict(submission);
                    return result.ErrorCount==0 && result.ImportedCount+result.UpdatedCount>0
                        ?"تم اعتماد الشخص واستيراد الزيارة."
                        :"تم حفظ اختيار الشخص، لكن استيراد الزيارة يحتاج متابعة. راجع التعارضات ورسالة الخطأ في سجل المزامنة.";
                }
                finally { KoboMonitoringImporter.Query(c,null,"SELECT RELEASE_LOCK(@k)","@k",key); }
            }
        }
    }
}
