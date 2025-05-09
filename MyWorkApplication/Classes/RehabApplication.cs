using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class RehabApplication
    {
        string query, condition;

        public void Insert_MicroProjectRehab(int MicroProject_ID, string PropertyOwnership, string PropertyArea,string Created_By)
        {
            Program.buildConnection();
            query = "INSERT INTO `microproject_rehab`(`MicroProject_ID`, `PropertyOwnership`, `PropertyArea`, `Created_By`) VALUES ("
                + MicroProject_ID + ",N'" + PropertyOwnership + "',N'" + PropertyArea + "',N'" + Created_By +"' )";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_MicroProjectRehab(int ID, int MicroProject_ID, string PropertyOwnership, string PropertyArea,string Edited_By)
        {
            Program.buildConnection();
            query = " UPDATE `microproject_rehab` SET " +
"  `PropertyOwnership`= N'" + PropertyOwnership + "'" +
", `PropertyArea`= N'" + PropertyArea + "'" +
", `Edited_By`= N'" + Edited_By + "'" +
" WHERE `MicroProject_ID`= " + MicroProject_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_MicroProjectRehab(int ID)
        {
            Program.buildConnection();
            query = " Delete from `microproject_rehab` where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_MicroProjectRehab(string MicroProject_ID, string P_Name)
        {
            query = @"SELECT microproject_rehab.*  
 ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'
 ,MP.* 

 from microproject MP 
 left JOIN microproject_rehab on microproject_rehab.MicroProject_ID = MP.MP_ID
 left join person_microproject PMP on PMP.MicroProject_ID = MP.MP_ID 
 left join person P on P.P_ID = PMP.Person_ID ";

            if (MicroProject_ID != "")
                condition = " Where PMP.MicroProject_ID = '" + MicroProject_ID + "' ";
            else if (P_Name != "")
                condition = " Where CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + P_Name + "%'";

            query += condition;
            Program.buildConnection();
            MySqlDataAdapter Ad = new MySqlDataAdapter(query, Program.MyConn);
            DataTable dt = new DataTable();
            Ad.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable Get_Questions_Answers()
        {
            Program.buildConnection();
            query = @"SELECT rehabquestion.`Name` as 'Q_Name'
, rehabquestion.`Type` as 'Q_Type' 
, rehabanswer.`ID` as 'A_ID'
, rehabanswer.`Name` as 'A_Name' 
, `Question_ID` 
 from `rehabquestion` left join `rehabanswer` on rehabquestion.ID = rehabanswer.Question_ID ";

            var condition = ""; 
            query += condition + " order by A_ID ";

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public void Insert_Rehab_Answers(int Rehab_ID, int Question_ID, int Answer_ID
            , string Notes , int Number, double Price, string Result)
        {
            Program.buildConnection();
            query = "INSERT INTO `microproject_rehab_answer`(`MicroRehab_ID`, `Question_ID`, `Answer_ID`, `Note`, `Number`, `Price`, `Result`) VALUES ("
                    + Rehab_ID + ","
                    + Question_ID + ","
                    + Answer_ID + ",N'"
                    + Notes + "',"  
                    + Number + ","  
                    + Price + ",N'"  
                    + Result + "' )";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
        public void Insert_Rehab_Answers(string myQuery)
        {
            Program.buildConnection();
            query = myQuery;

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        public void Delete_Rehab_Answers(int Rehab_ID)
        {
            Program.buildConnection();
            query = "DELETE FROM `microproject_rehab_answer` where `MicroRehab_ID` = " + Rehab_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Rehab_Answers(int Rehab_ID)
        {
            Program.buildConnection();
            query = @"SELECT microproject_rehab_answer.*
 FROM microproject_rehab_answer left join rehabquestion on microproject_rehab_answer.Question_ID = rehabquestion.ID ";

            var condition = " where MicroRehab_ID = " + Rehab_ID + " ";

            query += condition;
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public int Get_Rehab_ID(int MicroProject_ID)
        {
            var ID = -1;
            Program.buildConnection();
            query = "select ID from `microproject_rehab` where MicroProject_ID = " + MicroProject_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out ID);
                Program.MyConn.Close();
            } 
            return ID;
        }
    }
}
