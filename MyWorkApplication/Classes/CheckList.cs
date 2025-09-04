using DocumentFormat.OpenXml.ExtendedProperties;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class CheckList
    {
        string query, condition, Ordered_By, log_query;
        MySqlCommand sc;

        public DataSet Get_NewChecklist(int Person_ID, int MicroProject_ID)
        {
            string query1 = @"SELECT checklist.*
,career.Name as 'Career'
,microprojectsubtype.Name as 'SubType'
,  CONCAT('حالة المشروع: '
    , IF(state.Name_ar = '-','N/A',state.Name_ar) 
    , IF(mps.Note = '' ,'' ,' - ') 
    , mps.Note
    , IF(MP.MP_Note = '' ,'' ,' - ') 
    , MP.MP_Note
)  as 'State_Reason'    

 from checklist 
 LEFT JOIN person_microproject PMP on checklist.Person_ID = PMP.Person_ID
 LEFT JOIN microproject MP on MP.MP_ID = PMP.MicroProject_ID  
 LEFT JOIN career on MP.Career_ID = career.ID 

 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT JOIN `state` on state.ID = mps.State_ID 
 left join `statereason` on statereason.ID = mps.StateReason_ID 

 LEFT JOIN microprojectsubtype on microprojectsubtype.ID = MP.MP_SubType_ID  ";

            condition = " where Current_State = 1 ";
            if (Person_ID != -1)
                condition += " and checklist.Person_ID = "+ Person_ID ;
            if (MicroProject_ID != -1)
                condition += " and PMP.MicroProject_ID = " + MicroProject_ID ;

            query1 += condition + ";";

            string query2 = @"select person_checklistmembers.* 
,checklist_member.Name as 'Name'
,checklist_position.Type as 'Type'
,checklist_member.Position_ID as 'Position_ID' 
,checklist_position.Name as 'Position'  

from person_checklistmembers 
LEFT JOIN checklist_member on person_checklistmembers.Member_ID = checklist_member.ID
LEFT JOIN checklist_position on checklist_member.Position_ID = checklist_position.ID 
where person_checklistmembers.Person_ID = " + Person_ID + ";";

            /////////// change this tables name /////////////

            query = query1 + query2;
            Program.buildConnection();
            MySqlDataAdapter Ad = new MySqlDataAdapter(query, Program.MyConn);
            DataSet ds = new DataSet();
            Ad.Fill(ds);
            Program.MyConn.Close();
            return ds;
        }
        public void Insert_NewChecklist(int Person_ID
                    , string HVisit_Date, string HVisit_Time, string HVisit_Notes, string isHVisited
                    , string PVisit_Date, string PVisit_Time, string PVisit_Notes, string isPVisited
                    , string VisitsReport, string Team_Date, string Commity_Date, string Notes
            , int H_visitor_id1, int H_visitor_id2, int P_visitor_id1, int P_visitor_id2
            , string NeedProTest, string NeedProTestNote
                     , string HasProTest  , string ProTest_Date, string ProTest_Note, string ProTest_Result
                     , string HasProTrain , string ProTrain_Trainer, string ProTrain_Place
                     , string ProTrain_BDate, string ProTrain_EDate, string ProTrain_Note, string ProTrain_Result
                     , string HasMngTrain , string MngTrain_Trainer, string MngTrain_Place, string MngTrain_Date, string MngTrain_Result
            , int ProTest_User_ID
            , string HasRejectMsg, string RejectMsg_Date
            , int RejectMsgSender_id
            )
        {
            query = " START TRANSACTION;";
            query += "INSERT INTO `checklist`(`Person_ID`" +
                ", `HasHomeVisit`   , `HVisit_Date`, `HVisit_Time`, `HVisit_Visitor1_ID`, `HVisit_Visitor2_ID`, `HVisit_Note`" +
                ", `HasProjectVisit`, `PVisit_Date`, `PVisit_Time`, `PVisit_Visitor1_ID`, `PVisit_Visitor2_ID`, `PVisit_Note`" +
                ", `VisitsReport`" +
                ", `NeedProTest`, `NeedProTest_Note`" +
                ", `HasProTest`, `ProTest_Date`, `ProTest_Note`, `ProTest_User_ID`, `ProTest_Result`" +
                ", `HasProTrain`, `ProTrain_Trainer`, `ProTrain_Place`, `ProTrain_BDate`, `ProTrain_EDate`, `ProTrain_Note`, `ProTrain_Result`" +
                ", `HasMngTrain`, `MngTrain_Trainer`, `MngTrain_Place`, `MngTrain_Date`, `MngTrain_Result`" +
                ", `Notes`, `TeamDate`, `CommityDate`" +
                ", `RejectMsg`, `RejectMsgDate`, `RejectMsgSender_ID`) VALUES (" +

                   Person_ID + "," + isHVisited + ","
                   + "N'" + HVisit_Date + "',"
                   + "N'" + HVisit_Time + "'," 
                   + H_visitor_id1 + "," + H_visitor_id2 + ","
                   + "N'" + HVisit_Notes + "',"
                   + isPVisited + ","
                   + "N'" + PVisit_Date + "',"
                   + "N'" + PVisit_Time + "',"
                   + P_visitor_id1 + "," + P_visitor_id2 + ","
                   + "N'" + PVisit_Notes + "',"
                   + "N'" + VisitsReport + "',"

                   + "N'" + NeedProTest + "'," + "N'" + NeedProTestNote + "',"

                   + HasProTest + ","
                   + "N'" + ProTest_Date + "',"
                   + "N'" + ProTest_Note + "',"
                   + ProTest_User_ID
                   + "N'" + ProTest_Result + "',"

                   + HasProTrain + ","
                   + "N'" + ProTrain_Trainer + "'," + "N'" + ProTrain_Place + "',"
                   + "N'" + ProTrain_BDate + "'," + "N'" + ProTrain_EDate + "',"
                   + "N'" + ProTrain_Note + "'," + "N'" + ProTrain_Result + "',"

                   + HasMngTrain + ","
                   + "N'" + MngTrain_Trainer + "'," + "N'" + MngTrain_Place + "',"
                   + "N'" + MngTrain_Date + "'," + "N'" + MngTrain_Result + "',"

                   + "N'" + Notes + "',"
                   + "N'" + Team_Date + "'," + "N'" + Commity_Date + "',"

                   + HasRejectMsg + ","
                   + "N'" + RejectMsg_Date + "',"
                   + RejectMsgSender_id 
            + ");";
            query += " COMMIT;";
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Update_NewChecklist(int Person_ID
                    , string HVisit_Date, string HVisit_Time, string HVisit_Notes, string isHVisited
                    , string PVisit_Date, string PVisit_Time, string PVisit_Notes, string isPVisited
                    , string VisitsReport, string Team_Date, string Commity_Date, string Notes
            , int H_visitor_id1, int H_visitor_id2, int P_visitor_id1, int P_visitor_id2
            , string NeedProTest, string NeedProTestNote
                     , string HasProTest, string ProTest_Date, string ProTest_Note, string ProTest_Result
                     , string HasProTrain, string ProTrain_Trainer, string ProTrain_Place
                     , string ProTrain_BDate, string ProTrain_EDate, string ProTrain_Note, string ProTrain_Result
                     , string HasMngTrain, string MngTrain_Trainer, string MngTrain_Place, string MngTrain_Date, string MngTrain_Result
            , int ProTest_User_ID
            , string HasRejectMsg, string RejectMsg_Date
            , int RejectMsgSender_id)
        {
            query = " START TRANSACTION;";
            query += "UPDATE `checklist` SET " + 
        "`HasHomeVisit`="   + isHVisited + "," +
        "`HVisit_Date`=N'"  + HVisit_Date + "'," +
        "`HVisit_Time`=N'"  + HVisit_Time + "'," +
        "`HVisit_Visitor1_ID`=" + H_visitor_id1 + "," +
        "`HVisit_Visitor2_ID`=" + H_visitor_id2 + "," +
        "`HVisit_Note`=N'" + HVisit_Notes + "'," +

        "`HasProjectVisit`=" + isPVisited + "," +
        "`PVisit_Date`=N'" + PVisit_Date + "'," +
        "`PVisit_Time`=N'" + PVisit_Time + "'," +
        "`PVisit_Visitor1_ID`=" + P_visitor_id1 + "," +
        "`PVisit_Visitor2_ID`=" + P_visitor_id2 + "," +
        "`PVisit_Note`=N'" + PVisit_Notes + "'," +

        "`VisitsReport`=N'" + VisitsReport + "'," +

        "`NeedProTest`=N'" + NeedProTest + "'," +
        "`NeedProTest_Note`=N'" + NeedProTestNote + "'," +

        "`HasProTest`=" + HasProTest + "," +
        "`ProTest_Date`=N'" + ProTest_Date + "'," +
        "`ProTest_Note`=N'" + ProTest_Note + "'," +
        "`ProTest_User_ID`=" + ProTest_User_ID + "," +
        "`ProTest_Result`=N'" + ProTest_Result + "'," +

        "`HasProTrain`=" + HasProTrain + "," +
        "`ProTrain_Trainer`=N'" + ProTrain_Trainer + "'," +
        "`ProTrain_Place`=N'" + ProTrain_Place + "'," +
        "`ProTrain_BDate`=N'" + ProTrain_BDate + "'," +
        "`ProTrain_EDate`=N'" + ProTrain_EDate + "'," +
        "`ProTrain_Note`=N'" + ProTrain_Note + "'," +
        "`ProTrain_Result`=N'" + ProTrain_Result + "'," +

        "`HasMngTrain`=" + HasMngTrain + "," +
        "`MngTrain_Trainer`=N'" + MngTrain_Trainer + "'," +
        "`MngTrain_Place`=N'" + MngTrain_Place + "'," +
        "`MngTrain_Date`=N'" + MngTrain_Date + "'," +
        "`MngTrain_Result`=N'" + MngTrain_Result + "'," +

        "`Notes`=N'" + Notes + "'," +
        "`TeamDate`=N'" + Team_Date + "'," +
        "`CommityDate`=N'" + Commity_Date + "'," +

        "`RejectMsg`=" + HasRejectMsg + "," +
        "`RejectMsgDate`=N'" + RejectMsg_Date + "'," +
        "`RejectMsgSender_ID`=" + RejectMsgSender_id +

            " WHERE `Person_ID`= " + Person_ID + ";";
            query += " COMMIT;";
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_NewChecklist(int Person_ID)
        {
            query = "DELETE FROM `checklist` where `Person_ID` = " + Person_ID + ";";
            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        public DataTable Get_Checklist(string MicroProject_ID, string P_Name)
        {
            query = @"SELECT distinct microproject_checklist.*
,checklist_member.Name as 'Name'
,checklist_position.Type as 'Type'
,checklist_member.Position_ID as 'Position_ID' 
,checklist_position.Name as 'Position'  
,MP.* 
,career.Name as 'Career'
,microprojectsubtype.Name as 'SubType'
,  CONCAT('حالة المشروع: '
    , IF(state.Name_ar = '-','N/A',state.Name_ar) 
    , IF(mps.Note = '' ,'' ,' - ') 
    , mps.Note
    , IF(MP.MP_Note = '' ,'' ,' - ') 
    , MP.MP_Note
)  as 'State_Reason'    

 from microproject MP  
 LEFT JOIN microproject_checklist on microproject_checklist.MicroProject_ID = MP.MP_ID
 LEFT JOIN person_microproject PMP on MP.MP_ID = PMP.MicroProject_ID  
 LEFT JOIN person P on P.P_ID = PMP.Person_ID 
 LEFT JOIN career on MP.Career_ID = career.ID 

 LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
 LEFT JOIN `state` on state.ID = mps.State_ID 
 left join `statereason` on statereason.ID = mps.StateReason_ID 

 LEFT JOIN microprojectsubtype on microprojectsubtype.ID = MP.MP_SubType_ID 
 LEFT JOIN checklist_member on microproject_checklist.Member_ID = checklist_member.ID
 LEFT JOIN checklist_position on checklist_member.Position_ID = checklist_position.ID ";

            condition = " where Current_State = 1 ";
            if (MicroProject_ID != "")
                condition += " and MP.MP_ID = '" + MicroProject_ID + "'";
            else if (P_Name != "")
                condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '" + P_Name + "'";

            query += condition;
            Program.buildConnection();
            MySqlDataAdapter Ad = new MySqlDataAdapter(query, Program.MyConn);
            DataTable dt = new DataTable();
            Ad.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable Select_Checklist_Members(string Member_ID, string Type)
        {
            query = @"select checklist_member.ID as 'ID'
, checklist_member.Name as 'Name'
, checklist_member.Position_ID as 'Position_ID'  
, checklist_position.Name as 'Position'
, checklist_position.Type as 'Type'
 from checklist_member 
 left JOIN checklist_position on checklist_member.Position_ID = checklist_position.ID ";
            condition = " where 1";

            if (Member_ID != "")
                condition += " and checklist_member.ID = " + Member_ID + " ";
            if (Type != "")
                condition += " and checklist_position.Type like '" + Type + "' ";

            Ordered_By = " ORDER BY checklist_member.Name ASC ";

            query += condition;
            query += Ordered_By;

            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            da.SelectCommand = sc;
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt;
        }

        public int Insert(int MicroProject_ID, int Member_ID, int Opinion, string Notes, string NameNotes, string RoleNotes, string Location)
        {
            query = " START TRANSACTION;";
            query += "INSERT INTO `microproject_checklist`(`MicroProject_ID`, `Member_ID`, `Opinion`, `Notes`, `NameNotes`, `RoleNotes`, `Location` ,`created_at`) values("
                + MicroProject_ID + "," + Member_ID + "," + Opinion + ",N'"
                + Notes + "',N'" + NameNotes + "',N'" + RoleNotes + "',N'" + Location + "','" 
                + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "');";
            query += " SET @row_id = Last_Insert_ID();";
            query += " SELECT @row_id;";
            query += " COMMIT;";

            int id = -1;
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                id = int.Parse(sc.ExecuteScalar().ToString());
                Program.MyConn.Close();
            }
            return id;
        }

        public void Delete(int Row_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from microproject_checklist where `ID` = " + Row_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_All(int MicroProject_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from microproject_checklist where `MicroProject_ID` = " + MicroProject_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int Row_ID, int Member_ID, int Opinion, string Notes, string NameNotes, string RoleNotes)
        {
            query = " START TRANSACTION;";
            query += "Update `microproject_checklist` set " +
                " `Member_ID` = " + Member_ID +
                ", `Opinion` = " + Opinion +
                ", `Notes` = '" + Notes + "' " +
                ", `NameNotes` = '" + NameNotes + "' " +
                ", `RoleNotes` = '" + RoleNotes + "' " +
                ", `created_at` = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' " +
            " where ID = " + Row_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Insert_Member(string Name, int Position_ID)
        {
            query = " START TRANSACTION;";
            query = " INSERT INTO `checklist_member`( `Name`, `Position_ID`) VALUES( N'" + Name + "'," + Position_ID + " );";
            query += " COMMIT;";
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Update_Member(int Member_ID, string Name, int Position_ID)
        {
            query = " START TRANSACTION;";
            query = "Update `checklist_member` set Name = N'" + Name + "',Position_ID = " + Position_ID + " " +
              " Where ID = " + Member_ID + ";";
            query += " COMMIT;";
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_Member(int Member_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from checklist_member where `ID` = " + Member_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select_Positions(string Name, string Type)
        {
            query = @"select checklist_position.`ID`  
, checklist_position.`Name` as 'Name'
, checklist_position.`Type` 
 from checklist_position ";
            condition = " where 1";

            if (Type != "")
                condition += " and checklist_position.Type like '" + Type + "' ";
            if (Name != "")
                condition += " and checklist_position.Name like '" + Name + "' ";

            Ordered_By = " ORDER BY checklist_position.`Name` ASC ";

            query += condition;
            query += Ordered_By;

            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            MySqlDataAdapter da = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            da.SelectCommand = sc;
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt;
        }
        public void Insert_Position(string Name, string Type)
        {
            query = " START TRANSACTION;";
            query += "INSERT INTO `checklist_position`(`Name`, `Type`) VALUES (" +
                "N'" + Name + "','" + Type + "');";
            query += " COMMIT;";
            Program.buildConnection();
            sc = new MySqlCommand(query, Program.MyConn);
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Update_Position(int ID, string Name)
        {
            query = " START TRANSACTION;";
            query += "Update `checklist_position` set " +
                " `Name` = '" + Name + "' " +
                " where ID = " + ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_Position(int Row_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from checklist_position where `ID` = " + Row_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Checklist_Details_Procedure(string condition)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("select_checklist_details", Program.MyConn);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@MyCondition", condition);

            using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Program.MyConn.Close();
                return dt;
            }
        }
    }
}
