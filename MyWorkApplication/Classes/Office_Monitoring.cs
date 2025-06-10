using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Visit_Forms
{
    public class Office_Monitoring
    {
        private string query;

        public Office_Monitoring()
        {
            query = "";
        }

        public void Insert_Office_Monitoring(int Visit_ID
            , string Ans1, string Ans2, string Ans3, string Ans4, string Ans5, string Ans6, string Other
            , int Ans1_Value, int Ans2_Value, int Ans3_Value
            , int Ans4_Value, int Ans5_Value, int Ans6_Value)
        {
            Program.buildConnection();
            query =
                "INSERT INTO `office_monitoring`(`Visit_ID`, `Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`, `OtherComments`" +
                ", `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value`) VALUES ("
                + Visit_ID + ",N'"
                + Ans1 + "',N'" + Ans2 + "',N'" + Ans3 + "',N'"
                + Ans4 + "',N'" + Ans5 + "',N'" + Ans6 + "',N'"
                + Other + "',"
                + Ans1_Value + "," + Ans2_Value + "," + Ans3_Value + ","
                + Ans4_Value + "," + Ans5_Value + "," + Ans6_Value + ")";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Office_Monitoring(int Visit_ID
            , string Ans1, string Ans2, string Ans3, string Ans4, string Ans5, string Ans6, string Other
            , int Ans1_Value, int Ans2_Value, int Ans3_Value
            , int Ans4_Value, int Ans5_Value, int Ans6_Value)
        {
            Program.buildConnection();
            query = "update `office_monitoring` set " +
                    " `Ans1`=N'" + Ans1 + "'" + ",`Ans2`=N'" + Ans2 + "'" +
                    ",`Ans3`=N'" + Ans3 + "'" + ",`Ans4`=N'" + Ans4 + "'" +
                    ",`Ans5`=N'" + Ans5 + "'" + ",`Ans6`=N'" + Ans6 + "'" +
                    ",`OtherComments`=N'" + Other + "'" +
                    ",`Ans1_Value`= " + Ans1_Value + ",`Ans2_Value`= " + Ans2_Value +
                    ",`Ans3_Value`= " + Ans3_Value + ",`Ans4_Value`= " + Ans4_Value +
                    ",`Ans5_Value`= " + Ans5_Value + ",`Ans6_Value`= " + Ans6_Value +
                    " where `Visit_ID` = " + Visit_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Office_Monitoring(int ID)
        {
            Program.buildConnection();
            query = "delete from `office_monitoring` where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public int Get_Last_ID_Office_Monitoring()
        {
            var O_ID = -1;
            Program.buildConnection();
            query = "select MAX(ID) from `office_monitoring` ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out O_ID);
                Program.MyConn.Close();
            }

            return O_ID;
        }

        public DataTable Get_Office_Monitoring(int V_ID)
        {
            Program.buildConnection();
            query = @"SELECT `ID`
,`Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`
, `OtherComments` 
, `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value` 
 FROM office_monitoring
 where Visit_ID = " + V_ID + "";
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
    }
}