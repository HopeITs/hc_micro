using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class AgricultureApplication
    {
        string query;

        public DataTable Get_Questions_Answers()
        {
            Program.buildConnection();
            query = @"SELECT agquestion.`Name` as 'Q_Name' 
, aganswer.`ID` as 'A_ID'
, aganswer.`Name` as 'A_Name' 
, `Question_ID` 
 from `agquestion` left join `aganswer` on agquestion.ID = aganswer.Question_ID ";

            var condition = "";
            query += condition + " order by A_ID ";

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public void Insert_Agriculture_Answers(int MicroProject_ID, int Question_ID, int Answer_ID)
        {
            Program.buildConnection();
            query = "INSERT INTO `microproject_ag_answer`(`MicroProject_ID`, `Question_ID`, `Answer_ID`) VALUES ("
                    + MicroProject_ID + ","
                    + Question_ID + ","
                    + Answer_ID + " )";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Insert_Agriculture_Answers(string myQuery)
        {
            Program.buildConnection();
            query = myQuery;

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        public void Delete_Agriculture_Answers(int MP_ID)
        {
            Program.buildConnection();
            query = "DELETE FROM `microproject_ag_answer` where `MicroProject_ID` = " + MP_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Agriculture_Answers(int MP_ID)
        {
            Program.buildConnection();
            query = @"SELECT microproject_ag_answer.*
,aganswer.Name as 'A_Name' 
,agquestion.`Name` as 'Q_Name' 
 FROM microproject_ag_answer 
 left join agquestion on microproject_ag_answer.Question_ID = agquestion.ID
 left join aganswer on microproject_ag_answer.Answer_ID = aganswer.ID ";

            var condition = " where MicroProject_ID = " + MP_ID + " ";

            query += condition;
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
        
    }
}
