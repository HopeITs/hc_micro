using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class VisitRehab
    {
        private string query;
         
        public DataSet Get_Visit(string V_ID, string name, string MicroProject_ID)
        {
            Program.buildConnection();
            //string allquery = "START TRANSACTION;";

            query += @"SELECT PMP.`MicroProject_ID` as 'MicroProject_ID'
, PMP.Person_ID as 'Beneficiary_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, microproject.MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date' 
, microprojecttype.Name as 'Type'
, v.`ID`, v.`Kind`, v.`Date`, v.`Indicators`, v.`Created_By`, v.`Edited_By` 
, rehabvisit.*  "; 
            
            string from = @" FROM person_microproject PMP 
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID
 left join loan L on L.MicroProject_ID = microproject.MP_ID 
 left join visit v on PMP.MicroProject_ID = v.MicroProject_ID 
 left join rehabvisit on rehabvisit.Visit_ID = v.ID 
 left join street on street.ID =  microproject.MP_Street_ID 
 left join microprojecttype on microprojecttype.ID = microproject.MP_Type_ID ";

            string condition = " where 1 "; //Kind like 'R-cl'
            if (V_ID != "") condition += " and v.ID = " + V_ID + "";
            else if (name != "") condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + name + "%'";
            else if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;
             
            query += from + condition + ";";

            string q2 = @"SELECT `User_ID`,`UserName` 
    FROM  `visit_user` 
    left join `user` on user.UserID = visit_user.User_ID 
    left join `visit` on visit.ID = visit_user.Visit_ID ";
            if (V_ID != "") q2 += " WHERE `Visit_ID` = " + V_ID + "";
            else if (MicroProject_ID != "") q2 += " WHERE visit.MicroProject_ID = " + MicroProject_ID;

            q2 += ";";

            string allquery = query + q2;
            //allquery += " COMMIT;";

            var da = new MySqlDataAdapter(allquery, Program.MyConn);
            var ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();
            return ds;
        }

        public int Insert_Visit( int MicroProject_ID, string Kind, string Date, string CreatedBy
            , string Answers_Query, string Visitors_Query )
        {
            int Visit_ID;

            //check connection//
            Program.buildConnection();

            MySqlCommand sc = new MySqlCommand();
            sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
            sc.Parameters.AddWithValue("@Kind", Kind); 
            sc.Parameters.AddWithValue("@Date", Date);
            sc.Parameters.AddWithValue("@CreatedBy", CreatedBy);
             
            //visit
            query = " START TRANSACTION;"
                + " INSERT INTO `visit`(`MicroProject_ID`, `Kind`, `Date`, `Created_By`) "
                + " VALUES (@MicroProject_ID, @Kind, @Date, @CreatedBy);"
                + " SET @Visit_ID = Last_Insert_ID();";
              
            //rehabvisit
            if (Answers_Query != "")
                query += " INSERT INTO `rehabvisit`(`Visit_ID`, `Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`, `Ans7`, `Ans8`" +
                    ", `Ans9`, `Ans10`, `Ans11`, `Ans12`, `Ans13`, `Ans14`, `Ans15`" +
                    ", `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value`, `Ans7_Value`, `Ans8_Value`" +
                    ", `Ans9_Value`, `Ans10_Value`, `Ans11_Value`, `Ans12_Value`, `Ans13_Value`, `Ans14_Value`, `Ans15_Value`) VALUES " 
                    + Answers_Query + ";";

            //visit_user
            if (Visitors_Query != "")
                query += " INSERT INTO `visit_user`(`Visit_ID`, `User_ID`) VALUES " + Visitors_Query + ";";
              
            //log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
                " VALUES ('Insert Rehab Visit to the project: " + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen

            //taskboard --> Task IDs = 31 closing
            query += " update `task_microproject` set `State` = 1 "
            + ", `Date` = '" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "' "
            + " where `MicroProject_ID` = " + MicroProject_ID
            + " and `Task_ID` = 31;";

            //user_notification Delete UNSEEN notificattions of this project 
            query += " update `user_notification` set `Seen` = 1 where `MicroProject_ID` = " + MicroProject_ID + ";";
            //user_notification log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
                " VALUES ('Update Notifications set SEEN of project:" + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen

            //mp_state --> Make project state منتهي
            query += " update microproject_state set Current_State = 0 where MicroProject_ID = " + MicroProject_ID + ";"
              + " INSERT INTO microproject_state (`MicroProject_ID`, `State_ID`, `Date`, `Current_State`) Values ("
              + MicroProject_ID + ","
              + "(select ID from `state` where Name_ar like N'منتهي')" + ","
              + "N'" + DateTime.Now.ToString("yyyy/MM/dd") + "'" + ","
              + 1 + ");";
            //mp_state log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
               " VALUES ('Update state to منتهي of project:" + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen


            query += " SELECT @Visit_ID;"
                  + " COMMIT;";

            sc.CommandText = query;
            sc.Connection = Program.MyConn;
            Visit_ID = Convert.ToInt32(sc.ExecuteScalar());
            Program.MyConn.Close();

            return Visit_ID;
        }

        public void Update_Visit(int V_ID, int MicroProject_ID
            , string Visit_Query, string Answers_Query, string Visitors_Query)
        {
            //check connection//
            Program.buildConnection();

            MySqlCommand sc = new MySqlCommand();
            sc.Parameters.AddWithValue("@V_ID", V_ID);  
            query = " START TRANSACTION;";

            if (Visit_Query != null) 
                query += Visit_Query;

            query += " SET @Visit_ID = " + V_ID + ";";

            if (Answers_Query != null)
                query += Answers_Query;

            //visit_user
            if (Visitors_Query != "")
            {
                query += " Delete From `visit_user` where Visit_ID = " + V_ID + ";";
                query += " INSERT INTO `visit_user`(`Visit_ID`, `User_ID`) VALUES " + Visitors_Query + ";";
            }

            //log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
                " VALUES ('Update Rehab Visit["+V_ID+"] to the project: " + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen

            //taskboard --> Task IDs = 31 closing
            query += " update `task_microproject` set `State` = 1 " 
            + ", `Date` = '" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "' "
            + " where `MicroProject_ID` = " + MicroProject_ID
            + " and `Task_ID` = 31;";

            //user_notification Delete UNSEEN notificattions of this project 
            query += " update `user_notification` set `Seen` = 1 where `MicroProject_ID` = " + MicroProject_ID + ";";
            //user_notification log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
                " VALUES ('Update Notifications set SEEN of project:" + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen

            //mp_state --> Make project state منتهي
            query = " update microproject_state set Current_State = 0 where MicroProject_ID = " + MicroProject_ID + ";"
              + " INSERT INTO microproject_state (`MicroProject_ID`, `State_ID`, `Date`, `Current_State`) Values ("
              + MicroProject_ID + ","
              + "(select ID from `state` where Name_ar like N'منتهي')" + ","
              + "N'" + DateTime.Now.ToString("yyyy/MM/dd") + "'" + ","
              + 1 + ");";
            //mp_state log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
               " VALUES ('Update state to منتهي of project:" + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen
             

            query += " COMMIT;";

            sc.CommandText = query;
            sc.Connection = Program.MyConn; 
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }


        public void Delete_Visit(int V_ID,int MicroProject_ID)
        {
            //check connection//
            Program.buildConnection();

            MySqlCommand sc = new MySqlCommand();
            sc.Parameters.AddWithValue("@ID", V_ID);
            sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);

            query = " START TRANSACTION;";
            query += " DELETE FROM `visit` WHERE ID = @ID and MicroProject_ID = @MicroProject_ID;";
            //log
            query += " INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) " +
                " VALUES ('Delete Rehab Visit["+ V_ID + "] of the project: " + MicroProject_ID + "', 'Visit Rehab', '" + Program.UserName + "', 'DATE(NOW())', 0);"; //not seen
            query += " COMMIT;";
             
            sc.CommandText = query;
            sc.Connection = Program.MyConn;
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

    }
}
