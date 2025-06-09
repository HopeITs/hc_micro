using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Visit_Forms
{
    public class Evaluation_Statistics
    {
        private string query;

        public Evaluation_Statistics()
        {
            query = "";
        }

        public void Insert_Statistics(int Person_ID)
        {
            Program.buildConnection();
            query = "INSERT INTO `person_statistics`(`Person_ID`, `AvgSal`, `AvgEval`, `EvalSal_Percentage`) VALUES ("
                    + Person_ID + ","
                    + "(SELECT IFNULL(avg(`W_Salary`),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` = " +
                    Person_ID + ")" + ","
                    + "(SELECT  IFNULL(avg(`Profit`),0) FROM `mevisit` WHERE `Person_ID` = " + Person_ID + ")" + ","
                    + "((SELECT IFNULL(avg(`Profit`),0) FROM `mevisit` WHERE `Person_ID` = " + Person_ID + ")" +
                    " -  (SELECT IFNULL(avg(`W_Salary`), 0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID`  = " +
                    Person_ID + "))" +
                    " /  (SELECT IFNULL(avg(`W_Salary`), 0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID`  = " +
                    Person_ID + ")" +
                    " *  100 " + ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Statistics(int Person_ID)
        {
            Program.buildConnection();
            query = "update `person_statistics` set " +
                    "  `AvgSal` = " +
                    "(SELECT IFNULL(avg(`W_Salary`),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` = " +
                    Person_ID + ")" +
                    ", `AvgEval` = " + "(SELECT  IFNULL(avg(`Profit`),0) FROM `mevisit` WHERE `Person_ID` = " +
                    Person_ID + ")" +
                    ", `EvalSal_Percentage` = " +
                    "((SELECT IFNULL(avg(`Profit`),0) FROM `mevisit` WHERE `Person_ID` = " + Person_ID + ")" +
                    " -  (SELECT IFNULL(avg(`W_Salary`), 0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID`  = " +
                    Person_ID + "))" +
                    " /  IF((SELECT IFNULL(avg(`W_Salary`), 0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID`  = " +
                    Person_ID + ") =0," +
                    "(SELECT  IFNULL(avg(`Profit`),0) FROM `mevisit` WHERE `Person_ID` = " + Person_ID +
                    ") , (SELECT IFNULL(avg(`W_Salary`), 0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID`  = " +
                    Person_ID + ")    )   " +
                    " *  100 " +
                    " where `Person_ID` = " + Person_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Statistics(int ID)
        {
            Program.buildConnection();
            query = "delete from `person_statistics` where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select_BeneficiaryWithEval_IDs()
        {

            Program.buildConnection();

            query = "SELECT `Person_ID` FROM `mevisit` ";
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
    }
}