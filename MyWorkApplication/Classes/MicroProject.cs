using MySql.Data.MySqlClient;
using System.Data.SqlTypes;

namespace MyWorkApplication.Classes
{
    public class MicroProject
    {
        private string query;


        public MicroProject()
        {
            query = "";
        }

        public void Update_Project_State(int MicroProject_ID, string State, string StateDate)
        {
            query = " update microproject_state set Current_State = 0 where MicroProject_ID = " + MicroProject_ID + ";"
                + " INSERT INTO microproject_state (`MicroProject_ID`, `State_ID`, `Date`) Values ("
                + MicroProject_ID + ","
                + "(select ID from `state` where Name_ar like N'" + State + "')" + ","
                + "N'" + StateDate + "'" + ");";

            if (State == "منسحب" || State == "ملغى")
                query += " UPDATE user_notification set `Seen` = 1 WHERE user_notification.MicroProject_ID = " + MicroProject_ID + ";";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Project(int MicroProject_ID, string property, string value, string additional_updated_col)
        {
            string where1 = " where MP_ID = " + MicroProject_ID;

            query = " Update `microproject` set " + property + " = " + value;
            if (value == "-1") //insert null
                query = "Update `microproject` set " + property + " = " + SqlInt32.Null;

            if (additional_updated_col != "")
                query += "," + additional_updated_col;

            query += where1;

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        //string MP_ParishNotes,
        //, int ch_HouseTechCondition, int ch_ProjectExperience, int ch_ExpectedProjectSuccess, int ch_WorkAbility

        public void Update_Micro_VisitDetails(int MicroProject_ID, string isVisited
            , string Visit_Date, string Visit_Time
            , string Visit_Notes, string MP_KeyPersonNotes
            , string Team_Date, string Commity_Date)
        {
            query = "Update `microproject` set " +

                " MP_KeyPersonNotes = '" + MP_KeyPersonNotes + "' " +
                ",MP_TeamDate =  " + (Team_Date == "" ? "NULL" : "'" + Team_Date + "' ") +
                ",MP_VisitDate = " + (Visit_Date == "" ? "NULL" : "'" + Visit_Date + "' ") +
                ",MP_VisitTime = '" + Visit_Time + "' " +
                ",MP_VisitNotes = N'" + Visit_Notes + "' " +
                ",MP_CommityDate =  " + (Commity_Date == "" ? "NULL" : "'" + Commity_Date + "' ");

            //" MP_ParishNotes = '" + MP_ParishNotes + "' " +
            //",ch_HouseTechCondition = " + ch_HouseTechCondition + " " +
            //",ch_ProjectExperience =  " + ch_ProjectExperience + " " +
            //",ch_ExpectedProjectSuccess = " + ch_ExpectedProjectSuccess + " " +
            //",ch_WorkAbility = " + ch_WorkAbility + " "  

            if (isVisited != "")
                query += ",MP_Visited = " + isVisited + " ";

            query += " where MP_ID = " + MicroProject_ID;

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        public void Update_MP_Place(int MP_ID, int MP_Street_ID, string MP_AddressAfterFund)
        {
            var query = " Update `microproject` set "
                + " MP_Street_ID = " + (MP_Street_ID == -1 ? SqlInt32.Null : MP_Street_ID)
                + ",MP_AddressAfterFund = N'" + MP_AddressAfterFund + "'"
                + " where MP_ID = " + MP_ID;


            Program.buildConnection();
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

    }
}