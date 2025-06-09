using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class CheckList
    {
        string query, condition, Ordered_By, log_query;
        MySqlCommand sc;

        public DataTable Get_Checklist(string MicroProject_ID, string P_Name)
        {
            query = @"SELECT distinct microproject_checklist.*
,checklist_member.Name as 'Name'
,checklist_position.Type as 'Type'
,checklist_member.Position_ID as 'Position_ID' 
,checklist_position.Name as 'Position'  
,MP.* 

 from microproject MP  
 left JOIN microproject_checklist on microproject_checklist.MicroProject_ID = MP.MP_ID
 left join person_microproject PMP on MP.MP_ID = PMP.MicroProject_ID  
 left join person P on P.P_ID = PMP.Person_ID  
 left JOIN checklist_member on microproject_checklist.Member_ID = checklist_member.ID
 left JOIN checklist_position on checklist_member.Position_ID = checklist_position.ID ";

            if (MicroProject_ID != "")
                condition = " Where MP.MP_ID = '" + MicroProject_ID + "'";
            else if (P_Name != "")
                condition = " Where CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '" + P_Name + "'";

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
            query += "INSERT INTO `microproject_checklist`(`MicroProject_ID`, `Member_ID`, `Opinion`, `Notes`, `NameNotes`, `RoleNotes`, `Location`) values("
                + MicroProject_ID + "," + Member_ID + "," + Opinion + ",N'"
                + Notes + "',N'" + NameNotes + "',N'" + RoleNotes + "',N'" + Location + "');";
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
