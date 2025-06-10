using MySql.Data.MySqlClient;
using System.Data.SqlTypes;

namespace MyWorkApplication.Classes
{
    public class Application_Queries
    {

        private string query;

        public Application_Queries()
        {
            query = "";
        }

        #region insert

        #endregion

        #region update
        public void Update_Material(int M_ID, string name, double amount, double price, double local, string comment)
        {

            Program.buildConnection();
            query = @"UPDATE `material` SET 
  `Name`  = @name   
 ,`Amount`= @amount 
 ,`Price` = @price  
 ,`LocalContribution`= @local 
 ,`Comments`= @comment 
 WHERE `ID`= @M_ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@name", name);
                sc.Parameters.AddWithValue("@amount", amount);
                sc.Parameters.AddWithValue("@price", price);
                sc.Parameters.AddWithValue("@local", local);
                sc.Parameters.AddWithValue("@comment", comment);
                sc.Parameters.AddWithValue("@M_ID", M_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Person_Education(int PEdu_ID, int Education_ID)
        {

            Program.buildConnection();
            query = "UPDATE `person_education` SET "
                    + " `Education_ID` = @Education_ID "
                    + " WHERE `PEdu_ID`= @PEdu_ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Education_ID", Education_ID);
                sc.Parameters.AddWithValue("@PEdu_ID", PEdu_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Person_WorkExp(int PW_ID, int Work_ID
            , string W_Description, string W_BeginDate, string W_EndDate
            , string W_Place, string W_PlaceState
            , string W_CauseOfLoseDescription, double W_Salary)
        {

            Program.buildConnection();
            query = "UPDATE `person_workexp` SET "
                    + " `Work_ID` =@Work_ID "
                    + ",`W_Description`= @W_Description "
                    + ",`W_BeginDate`= @W_BeginDate "
                    + ",`W_EndDate`= @W_EndDate "
                    + ",`W_Place`= @W_Place "
                    + ",`W_PlaceState`= @W_PlaceState "
                    + ",`W_CauseOfLoseDescription`= @W_CauseOfLoseDescription "
                    + ",`W_Salary`= @W_Salary "
                    + " WHERE `PW_ID`= @PW_ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Work_ID", Work_ID);
                sc.Parameters.AddWithValue("@W_Description", W_Description);
                sc.Parameters.AddWithValue("@W_BeginDate", W_BeginDate);
                sc.Parameters.AddWithValue("@W_EndDate", W_EndDate);
                sc.Parameters.AddWithValue("@W_Place", W_Place);
                sc.Parameters.AddWithValue("@W_PlaceState", W_PlaceState);
                sc.Parameters.AddWithValue("@W_CauseOfLoseDescription", W_CauseOfLoseDescription);
                sc.Parameters.AddWithValue("@W_Salary", W_Salary);
                sc.Parameters.AddWithValue("@PW_ID", PW_ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Person_AidSource(int ID, int AidSource_ID, string DateYear, string Notes)
        {
            Program.buildConnection();
            query = "UPDATE `person_aidsource` SET "
                    + " `AidSource_ID` =@AidSource_ID "
                    + ",`DateYear`= @DateYear "
                    + ",`Notes`= @Notes "
                    + " WHERE `ID`= @ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@AidSource_ID", AidSource_ID);
                sc.Parameters.AddWithValue("@DateYear", DateYear);
                sc.Parameters.AddWithValue("@Notes", Notes);
                sc.Parameters.AddWithValue("@ID", ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Person_FMember(int P_ID, string P_FirstName, string P_LastName, string P_DOB, int P_state)
        {

            Program.buildConnection();
            query = "UPDATE `person` SET "
                    + " `P_FirstName`= N'" + P_FirstName + "'"
                    + ",`P_LastName` = N'" + (P_LastName != "" ? P_LastName : "") + "'"
                    + ",`P_DOB` = N'" + (P_DOB == "" ? null : P_DOB + "/01/01") + "'"
                    + ",`P_State_ID` =" + (P_state == -1 ? SqlInt32.Null : P_state)
                    + " WHERE `P_ID`= " + P_ID + "";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }


        #endregion

        #region delete

        #endregion




    }
}
