using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace MyWorkApplication.Classes
{
    internal class Log
    {
        private string query;
        private MySqlCommand sc;

        public Log()
        {
            query = "";
            sc = new MySqlCommand();
        }

        public void Insert_Log(string Type, string OnTable, string User, DateTime Date)
        {
            Program.buildConnection();
            query = "INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (N'"
                    + Type + "',N'"
                    + OnTable + "',N'"
                    + User + "',N'"
                    + Date.Year + "/" + Date.Month + "/" + Date.Day + " " + Date.Hour + ":" + Date.Minute + ":" +
                    Date.Second + "',"
                    + "0" + ")"; //not seen

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }

        }

        public void Update_Log() //make all seen
        {
            Program.buildConnection();
            query = "update `log` set Log_Seen = 1 where Log_ID in "
                    + "(select Log_ID from `log` where Log_Seen = 0)"; //not seen
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Log(int[] ids) //make selected ids seen 
        {
            var sb = new StringBuilder();
            sb.Append('(');
            foreach (var i in ids) sb.Append(i).Append(',');
            // remove final ,
            sb.Length -= 1;
            sb.Append(')');
            var inValue = sb.ToString();

            Program.buildConnection();
            query = "update `log` set Log_Seen = 1 where Log_ID in " + inValue + " "; //not seen
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
    }
}