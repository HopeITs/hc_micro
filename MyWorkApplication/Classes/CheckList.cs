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
,  PMP.P_WorkYears as 'ExpYears'

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
                   + (HVisit_Date == "" ? "null" : "N'" + HVisit_Date + "'") + ","
                   + "N'" + HVisit_Time + "'," 
                   + (H_visitor_id1 == -1 ? "null" : H_visitor_id1.ToString()) + "," 
                   + (H_visitor_id2 == -1 ? "null" : H_visitor_id2.ToString()) + ","
                   + "N'" + HVisit_Notes + "',"
                   + isPVisited + ","
                   + (PVisit_Date == "" ? "null" : "N'" + PVisit_Date + "'") + ","
                   + "N'" + PVisit_Time + "',"
                   + (P_visitor_id1 == -1 ? "null" : P_visitor_id1.ToString()) + "," 
                   + (P_visitor_id2 == -1 ? "null" : P_visitor_id2.ToString()) + ","
                   + "N'" + PVisit_Notes + "',"
                   + "N'" + VisitsReport + "',"

                   + "N'" + NeedProTest + "'," + "N'" + NeedProTestNote + "',"

                   + HasProTest + ","
                   + (ProTest_Date == "" ? "null" : "N'" + ProTest_Date + "'") + ","
                   + "N'" + ProTest_Note + "',"
                   + (ProTest_User_ID == -1 ? "null" : ProTest_User_ID.ToString()) + ","
                   + "N'" + ProTest_Result + "',"

                   + HasProTrain + ","
                   + "N'" + ProTrain_Trainer + "'," + "N'" + ProTrain_Place + "',"
                   + (ProTrain_BDate == "" ? "null" : "N'" + ProTrain_BDate + "'") + "," 
                   + (ProTrain_EDate == "" ? "null" : "N'" + ProTrain_EDate + "'") + ","
                   + "N'" + ProTrain_Note + "'," + "N'" + ProTrain_Result + "',"

                   + HasMngTrain + ","
                   + "N'" + MngTrain_Trainer + "'," + "N'" + MngTrain_Place + "',"
                   + (MngTrain_Date == "" ? "null" : "N'" + MngTrain_Date + "'") + "," 
                   + "N'" + MngTrain_Result + "',"

                   + "N'" + Notes + "',"
                   + (Team_Date == "" ? "null" : "N'" + Team_Date + "'")       + "," 
                   + (Commity_Date == "" ? "null" : "N'" + Commity_Date + "'") + ","

                   + HasRejectMsg + ","
                   + (RejectMsg_Date == "" ? "null" : "N'" + RejectMsg_Date + "'") + ","
                   + (RejectMsgSender_id == -1 ? "null" : RejectMsgSender_id.ToString())
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
        "`HasHomeVisit`="       + isHVisited + "," +
        "`HVisit_Date`="        + (HVisit_Date == "" ? "null"   : "N'" + HVisit_Date +"'")  + "," +
        "`HVisit_Time`=N'"      + HVisit_Time + "'," +
        "`HVisit_Visitor1_ID`=" + (H_visitor_id1 == -1 ? "null" : H_visitor_id1.ToString()) + "," +
        "`HVisit_Visitor2_ID`=" + (H_visitor_id2 == -1 ? "null" : H_visitor_id2.ToString()) + "," +
        "`HVisit_Note`=N'"      + HVisit_Notes + "'," +

        "`HasProjectVisit`="    + isPVisited + "," +
        "`PVisit_Date`="        + (PVisit_Date == "" ? "null" : "N'" + PVisit_Date + "'") + "," +
        "`PVisit_Time`=N'"      + PVisit_Time + "'," +
        "`PVisit_Visitor1_ID`=" + (P_visitor_id1 == -1 ? "null" : P_visitor_id1.ToString()) + "," +
        "`PVisit_Visitor2_ID`=" + (P_visitor_id2 == -1 ? "null" : P_visitor_id2.ToString()) + "," +
        "`PVisit_Note`=N'"      + PVisit_Notes + "'," +

        "`VisitsReport`=N'"     + VisitsReport + "'," +

        "`NeedProTest`=N'"      + NeedProTest + "'," +
        "`NeedProTest_Note`=N'" + NeedProTestNote + "'," +

        "`HasProTest`="         + HasProTest + "," +
        "`ProTest_Date`="       + (ProTest_Date == "" ? "null" : "N'" + ProTest_Date + "'") + "," +
        "`ProTest_Note`=N'"     + ProTest_Note + "'," +
        "`ProTest_User_ID`="    + (ProTest_User_ID == -1 ? "null" : ProTest_User_ID.ToString()) + "," +
        "`ProTest_Result`=N'"   + ProTest_Result + "'," +

        "`HasProTrain`="        + HasProTrain + "," +
        "`ProTrain_Trainer`=N'" + ProTrain_Trainer + "'," +
        "`ProTrain_Place`=N'"   + ProTrain_Place + "'," +
        "`ProTrain_BDate`="     + (ProTrain_BDate == "" ? "null" : "N'" + ProTrain_BDate + "'")  + "," +
        "`ProTrain_EDate`="     + (ProTrain_EDate == "" ? "null" : "N'" + ProTrain_EDate + "'")  + "," +
        "`ProTrain_Note`=N'"    + ProTrain_Note + "'," +
        "`ProTrain_Result`=N'"  + ProTrain_Result + "'," +

        "`HasMngTrain`="        + HasMngTrain + "," +
        "`MngTrain_Trainer`=N'" + MngTrain_Trainer + "'," +
        "`MngTrain_Place`=N'"   + MngTrain_Place + "'," +
        "`MngTrain_Date`="      + (MngTrain_Date == "" ? "null" : "N'" + MngTrain_Date + "'") + "," +
        "`MngTrain_Result`=N'"  + MngTrain_Result + "'," +

        "`Notes`=N'"            + Notes + "'," +
        "`TeamDate`="           + (Team_Date == "" ? "null" : "N'" + Team_Date + "'")       + "," +
        "`CommityDate`="        + (Commity_Date == "" ? "null" : "N'" + Commity_Date + "'") + "," +

        "`RejectMsg`="          + HasRejectMsg + "," +
        "`RejectMsgDate`="      + (RejectMsg_Date == "" ? "null" : "N'" + RejectMsg_Date + "'") + "," +
        "`RejectMsgSender_ID`=" + (RejectMsgSender_id == -1 ? "null" : RejectMsgSender_id.ToString())  +

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


        #region checklist members
        public int Insert_Checklist_Members(int Person_ID
            , int Member_ID, int Opinion, string Notes, string NameNotes, string RoleNotes, string Location)
        {
            query = " START TRANSACTION;";
            query += "INSERT INTO `person_checklistmembers`(`Member_ID`, `Opinion`, `Notes`, `NameNotes`, `RoleNotes`, `Location` ,`created_at`,`Person_ID`) values("
                + Member_ID + "," + Opinion + ",N'"
                + Notes + "',N'" + NameNotes + "',N'" + RoleNotes + "',N'" + Location + "','" 
                + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," 
                + Person_ID + ");";
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
        public void Update_Checklist_Members(int Row_ID
            , int Member_ID, int Opinion, string Notes, string NameNotes, string RoleNotes)
        {
            query = " START TRANSACTION;";
            query += "Update `person_checklistmembers` set " +
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
        public void Delete_Checklist_Members(int Row_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from person_checklistmembers where `ID` = " + Row_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Delete_All_Checklist_Members(int Person_ID)
        {
            query = " START TRANSACTION;";
            query += "DELETE from person_checklistmembers where `Person_ID` = " + Person_ID + ";";
            query += " COMMIT;";

            Program.buildConnection();
            using (MySqlCommand sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        #endregion

        #region members
        public DataTable Select_Members(string Member_ID, string Type)
        {
            query = @"select checklist_member.ID as 'ID'
, checklist_member.Name as 'Name'
, checklist_member.Position_ID as 'Position_ID'  
, checklist_position.Name as 'Position'
, checklist_position.Type as 'Type'
, checklist_member.IsActive as 'IsActive'

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
        public void Update_Member(int Member_ID, string Name, int Position_ID, string IsActive)
        {
            query = " START TRANSACTION;";
            query = " Update `checklist_member` set " +
                " Name = N'"        + Name + "'," +
                " Position_ID = "   + Position_ID + "," +
                " IsActive = "      + IsActive +  
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
        #endregion

        #region positions
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
        #endregion

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


        public DataSet Get_NeedEvaluation(string in_type, string in_ids)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("GetNeedEvaluation", Program.MyConn);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@in_type", in_type);
            sc.Parameters.AddWithValue("@in_ids" , in_ids);

            using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);
                Program.MyConn.Close();
                return ds;
            }
        }
    }
}
