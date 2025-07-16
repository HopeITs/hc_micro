using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class UserNotification
    {

        public void Insert_UserNotification(string Date, string Body, string P_Name, int MicroProject_ID, int User_ID,
            int Sender_ID, int Pay_ID)
        {

            Program.buildConnection();
            string query = "Insert Into `user_notification`(`Date`, `Body`, `Seen`, `P_Name`, `MicroProject_ID`, `User_ID`, `Sender_ID`,`Pay_ID`) values("
               + "@Date ,@Body, 0 , @P_Name ,@MicroProject_ID ,@User_ID ,@Sender_ID ,@Pay_ID )";

            //save null if no MicroProject_id
            object MP_ID = (MicroProject_ID == -1) ? (object)DBNull.Value : MicroProject_ID;

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Body", Body);
                sc.Parameters.AddWithValue("@P_Name", P_Name);
                sc.Parameters.AddWithValue("@MicroProject_ID", MP_ID);
                sc.Parameters.AddWithValue("@User_ID", User_ID);
                sc.Parameters.AddWithValue("@Sender_ID", Sender_ID);
                sc.Parameters.AddWithValue("@Pay_ID", Pay_ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_MicroUsers_Notification(int MicroProject_ID, string Date, string Body, string P_Name
            , int Pay_ID, string Seen)
        {
            //save null if no MicroProject_id
            object MP_ID = (MicroProject_ID == -1) ? (object)DBNull.Value : MicroProject_ID;

            Program.buildConnection();
            string condition = " where 1 ";
            string query = "update `user_notification` set `Seen` = @Seen ";
            if (MicroProject_ID != -1)
                condition += " and `MicroProject_ID` = @MicroProject_ID ";
            if (Date != "")
                condition += " and `Date` = @Date ";
            if (Body != "")
                condition += " and `Body` like @Body ";
            if (P_Name != "")
                condition += " and `P_Name` like @P_Name ";
            if (Pay_ID != -1)
                condition += " and `Pay_ID` = @Pay_ID ";

            query += condition;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MP_ID);
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Body", Body);
                sc.Parameters.AddWithValue("@P_Name", P_Name);
                sc.Parameters.AddWithValue("@Pay_ID", Pay_ID);
                sc.Parameters.AddWithValue("@Seen", Seen);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Notification(int MicroProject_ID, string Date, string Body, string P_Name, int Pay_ID)
        {

            Program.buildConnection();
            string condition = " where 1 ";
            string query = " Delete from `user_notification` ";
            if (MicroProject_ID != -1)
                condition += " and `P_Name` = @P_Name ";
            if (MicroProject_ID != -1)
                condition += " and `MicroProject_ID` = @MicroProject_ID ";
            if (Date != "")
                condition += " and `Date` = @Date ";
            if (Body != "")
                condition += " and `Body` = @Body ";
            if (Pay_ID != -1)
                condition += " and `Pay_ID` = @Pay_ID ";

            query += condition;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Body", Body);
                sc.Parameters.AddWithValue("@P_Name", P_Name);
                sc.Parameters.AddWithValue("@Pay_ID", Pay_ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        public void Update_NotificationWithPaymentID(int Pay_ID, string Body, string Date, string Seen)
        {

            Program.buildConnection();
            string query = "update `user_notification` set `Seen` =@Seen ";
            if (Date != "") query += " ,`Date` = @Date ";
            if (Body != "") query += " ,`Body` = @Body ";

            query += " where `Pay_ID` = @Pay_ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Pay_ID", Pay_ID);
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Body", Body);
                sc.Parameters.AddWithValue("@Seen", Seen);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_NotificationW_ExpiredDateOfExeFile(int ExeFileID, string Date, int Sender_ID)
        {

            Program.buildConnection();
            string query = "update `user_notification` set " +
                         " Date = @Date " +
                         ",Sender_ID = @Sender_ID " +
                         " where `Body` like N'الملف التنفيذي' and `Pay_ID` = @ExeFileID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ExeFileID", ExeFileID);
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Sender_ID", Sender_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_UserNotification(int N_ID)
        {

            Program.buildConnection();

            string query = "update `user_notification` set `Seen` = 1 where `ID` = @N_ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@N_ID", N_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_UserNotification(int MicroProject_ID, int User_ID)
        {

            Program.buildConnection();
            string query = " update `user_notification` set `Seen` = 1 where `MicroProject_ID` = @MicroProject_ID ";
            if (User_ID != -1)
                query += " and `User_ID` = @User_ID ";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@User_ID", User_ID);
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_VisitNotification(int MicroProject_ID, int Seen)
        {
            Program.buildConnection();
            string query = " Delete from `user_notification` " +
                " where `MicroProject_ID` = @MicroProject_ID " +
                " and `Pay_ID` = -21 " +
                " and `Seen` = @Seen ";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@Seen", Seen);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public bool Has_Notifications(int MicroProject_ID, string Date, string Body, string P_Name, int Pay_ID, string Seen)
        {
            Program.buildConnection();
            string condition = " where 1 ";
            string query = "select count(*) from `user_notification` ";
            if (Seen != "")
                condition += " and `Seen` = @Seen ";
            if (MicroProject_ID != -1)
                condition += " and `MicroProject_ID` = @MicroProject_ID ";
            if (Date != "")
                condition += " and `Date` = @Date ";
            if (Body != "")
                condition += " and `Body` like @Body ";
            if (P_Name != "")
                condition += " and `P_Name` like @P_Name ";
            if (Pay_ID != -1)
                condition += " and `Pay_ID` = @Pay_ID ";

            query += condition;
            int count = 0;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@Date", Date);
                sc.Parameters.AddWithValue("@Body", Body);
                sc.Parameters.AddWithValue("@P_Name", P_Name);
                sc.Parameters.AddWithValue("@Pay_ID", Pay_ID);
                sc.Parameters.AddWithValue("@Seen", Seen);
                var val = sc.ExecuteScalar();
                Program.MyConn.Close();
                if (val == null || val == DBNull.Value) return false;
                int.TryParse(val.ToString(), out count);
                if (count == 0) return false;
                else return true;
            }
        }

        public DataTable Select_VisitNotifications(string month, string year, string V_Kind, string V_number, string Street_ID, string Type_ID, string seen)
        {
            //  , (select Pay_DueDate from payment left join loan on payment.Loan_ID = loan.Loan_ID where Pay_IsPaid like 'Paid' and loan.MicroProject_ID = MP.MP_ID order by Pay_DueDate Desc limit 1) 'تاريخ آخر دفعة مدفوعة'

            Program.buildConnection();
            var q = @"select user_notification.`ID` as 'ID' 
 , user_notification.`MicroProject_ID` as 'رقم المشروع' 
 , MP.MP_Name as 'المشروع' 
 , user_notification.`P_Name` as 'المستفيد'
 , state.Name_ar as 'حالة المشروع'
 , donor.Name_abb as 'الممول'

 , user_notification.`Body` as 'نوع الزيارة'
 , user_notification.`Date` as 'التاريخ'
 
 , h_street.Name as 'منطقة السكن' 
 , w_street.Name 'منطقة المشروع'
 , (select group_concat(P_Mobile) from person where person.P_ID = PMP.Person_ID limit 1) 'الموبايل'
 
 , (select Pay_DueDate from payment left join loan on payment.Loan_ID = loan.Loan_ID where Pay_IsPaid like 'Not Paid' and loan.MicroProject_ID = MP.MP_ID order by Pay_DueDate Desc limit 1) 'تاريخ الدفعة المستحقة'
 
 , ( CASE 
    WHEN lower(`Body`) like '%op%' THEN (SELECT IF(visit.Date = NULL,'Not Yet','Done') from visit WHERE visit.MicroProject_ID = PMP.MicroProject_ID and visit.Kind like '%op%' limit 1)
 	WHEN lower(`Body`) like '%cl%' THEN (SELECT IF(visit.Date = NULL,'Not Yet','Done') from visit WHERE visit.MicroProject_ID = PMP.MicroProject_ID and visit.Kind like '%cl%' limit 1)

    WHEN lower(`Body`) like '%m1%' THEN (SELECT IF(mevisit.Date = NULL,'Not Yet','Done') from mevisit WHERE mevisit.Person_ID = PMP.Person_ID and mevisit.Number like '1' limit 1)
    WHEN lower(`Body`) like '%m2%' THEN (SELECT IF(mevisit.Date = NULL,'Not Yet','Done') from mevisit WHERE mevisit.Person_ID = PMP.Person_ID and mevisit.Number like '2' limit 1)
    WHEN lower(`Body`) like '%m3%' THEN (SELECT IF(mevisit.Date = NULL,'Not Yet','Done') from mevisit WHERE mevisit.Person_ID = PMP.Person_ID and mevisit.Number like '3' limit 1)
    WHEN lower(`Body`) like '%m4%' THEN (SELECT IF(mevisit.Date = NULL,'Not Yet','Done') from mevisit WHERE mevisit.Person_ID = PMP.Person_ID and mevisit.Number like '4' limit 1)
    WHEN lower(`Body`) like '%m5%' THEN (SELECT IF(mevisit.Date = NULL,'Not Yet','Done') from mevisit WHERE mevisit.Person_ID = PMP.Person_ID and mevisit.Number like '5' limit 1)
    ELSE 'N/A' END 
 ) as 'حالة الزيارة'

 , Pay_ID 
 
 from `user_notification` 
 left join `microproject` MP on MP.MP_ID = user_notification.MicroProject_ID
 left join `person_microproject` PMP on MP.MP_ID = PMP.MicroProject_ID
 left join `person` on person.P_ID = PMP.Person_ID
 LEFT JOIN `street` h_street on person.Street_ID = h_street.ID 
 LEFT join `street` w_street on w_street.ID = MP.MP_Street_ID 
 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 Left Join `donor` on donor.ID = MP.MP_Donor
 LEFT JOIN `state` on state.ID = mps.State_ID ";

            var condition =
                " where Current_State = 1 and `Pay_ID` = -21 and `User_ID` =  (select UserID from user where IsME = 1 limit 1) "; //28
            if (month != "") condition += " and Month(user_notification.`Date`) in (" + month + ")";
            if (year != "") condition += " and Year(user_notification.`Date`) like " + year;
            if (V_Kind != "") condition += " and Body like '%-" + V_Kind + "'";
            if (V_number != "") condition += " and Body like 'Visit " + V_number + "%'";
            if (Street_ID != "") condition += " and w_street.ID = " + Street_ID + " ";
            if (Type_ID != "") condition += " and MP_Type_ID = " + Type_ID + " ";
            if (seen != "") condition += " and Seen = " + seen;
            q += condition + "  GROUP BY user_notification.ID ORDER BY user_notification.`Date` asc  ";

            var sc = new MySqlCommand(q, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public int Has_Visit_Notifications(int MicroProject_ID, string Visit_Type)
        {
            Program.buildConnection();
            string q1 = "SELECT count(*) " +
                " FROM user_notification " +
                " WHERE user_notification.MicroProject_ID = " + MicroProject_ID +
                " and user_notification.Body like '%" + Visit_Type + "%' " +
                " GROUP BY user_notification.MicroProject_ID;";

            MySqlCommand sc1 = new MySqlCommand(q1, Program.MyConn);
            var count = Convert.ToInt32(sc1.ExecuteScalar());
            Program.MyConn.Close();
            return count;
        }
    }
}