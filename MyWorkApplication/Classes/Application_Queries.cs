using MySql.Data.MySqlClient;
using System.Data;
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

        #region Select
        public DataSet Select_New_Application(int MicroProject_ID, int Person_ID)
        {
            string query = "";

            query += " select microproject.MP_ID as 'MicroProject_ID'"
                + ", microproject.MP_DateOfRequest as 'Date Of Request'"
                + ", microproject.MP_Name as 'Project Name'"
                + ", microproject.MP_RequestedAmount as 'Requested Amount'"
                + ", fundtype.name as 'fundtype'"
                + ", microproject.MP_Type_ID as 'Type_ID'"
                + ", microproject.MP_SubType_ID as 'SubType_ID'"
                + ", microproject.SubCategory_ID as 'SubCategory_ID'"
                + ", microproject.Career_ID as 'Career_ID'"
                + ", microproject.MP_Profit_Description as 'Profit_Description'"
                + ", microproject.MP_PaperUser_ID as 'PaperUser_ID'"
                + ", (select user.UserName from user where UserID = microproject.MP_ProgramUser_ID) as 'ProgramUser'"
                + ",(select user.UserName from user where UserID = microproject.MP_EditedByUser_ID) as 'EditedByUser'"
                + ", microproject.MP_PlaceType_ID"
                + ", microproject.MP_PlaceAddress"
                + ", microproject.MP_PlaceNote"
                + ", microproject.MP_AmperNumber"
                + ", microproject.MP_AmperType"
                + ", microproject.MP_Agreement1"
                + ", microproject.MP_Agreement2"
                + ", microproject.MP_Agreement3"
                + ", microproject.MP_Agreement4"
                + ", microproject.MP_Agreement5"
                + ", (select SUM(material.Price*material.Amount) from material where material.MicroProject_ID = @MicroProject_ID) AS 'OverallSyrian'"
                + ", microproject.IsContentUpdated"
                + " from microproject left join fundtype on fundtype.id = microproject.MP_FundType_ID"
                + " where microproject.MP_ID = @MicroProject_ID;";

            query += "select person.P_ID, person.P_FirstName, person.P_LastName,CONCAT(P_FirstName, ' ',person.P_FatherName ,' ', person.P_LastName) as 'FullName', person.P_Sex, person_microproject.PersonType, person.P_FullName_en, person.P_FatherName, person.P_MotherName," +
                " person.P_RegistrationPlace, person.P_NationalNumber, person.P_DOB, person.P_MaritalStatus_ID, person.P_Parish_ID, person.P_IntermidiarySide," +
                " person.P_Priest_ID, person.P_Priest2_ID, person.P_Mobile, person.P_HomeTel, person.P_Mobile2, person.Street_ID, person.P_HomeAddress, person.P_HomeType_ID," +
                " person.P_HomeState_Note, person.P_MilitaryService, person.P_MilitaryService_Note, person.P_MilitaryPeriod, person.P_MilitaryService_Date, person.P_IsMilitaryInWar, person_microproject.P_WorkYears," +
                " person_microproject.P_IsLivingWithFamily, person_microproject.P_LiveWithFamily_Note, person_microproject.P_HasAidSource, person_microproject.P_AidSourceNote," +
                " person_microproject.P_Loss, person_microproject.P_Goal, person_microproject.P_GoalHow, person_microproject.P_OtherNotes," +
                " person_microproject.P_HasCourses, person_microproject.P_HasEducation," +
                " person_microproject.P_HasLoss, person_microproject.P_HasIllness, person_microproject.P_HasAssets, person_microproject.P_HasDebts, person_microproject.P_DebtsAmount, person_microproject.P_DebtsNote, person_microproject.P_HasHelp, person_microproject.P_HasWork" +
                " from person left join person_microproject on person.P_ID = person_microproject.Person_ID where person_microproject.Person_ID = @Person_ID and person_microproject.MicroProject_ID = @MicroProject_ID;";

            query += " select image.Image_ID , image.image_NewPath " +
                " from image left join imagetype on imagetype.ID = image.ImageType_ID" +
                " where image.MicroProject_ID = @MicroProject_ID and image.Person_ID = @Person_ID and " +
                " imagetype.Name = 'صورة شخصية';";

            query += " select person_workexp.ID, person_workexp.Work_ID,person_workexp.W_Description, person_workexp.W_WorkType_ID, person_workexp.W_BeginDate, person_workexp.W_EndDate, person_workexp.W_PlaceState, person_workexp.W_Place, person_workexp.W_Contact, person_workexp.W_Salary, person_workexp.W_CauseOfLoseDescription, person_workexp.W_Status" +
                " from person_workexp where Person_ID = @Person_ID;";

            query += " select family.F_ID, family.F_Number, person_family.PF_ID, person_family.Person_ID, person.P_FirstName, person.P_LastName, person.P_FatherName, person.P_DOB, person.P_NationalNumber, person.P_State_ID," +
                "  person_family.Relation_ID, person_family.EducationLevel_ID, person_family.Education_ID, person.P_NationalNumber, education.Type_ID as 'Education_Type_ID', person_family.HasWork," +
                " person.P_NumAtHome, person.P_NumOfMembers" +
                " from person_family left join person on person.P_ID = person_family.Person_ID" +
                " left join family on family.F_ID = person_family.family_ID" +
                " left join education on education.ID = person_family.Education_ID" +
                " where P_Provider_ID = @Person_ID;";

            query += " select person_aidsource.ID, person_aidsource.Person_ID, relation.Name as 'Relation_Name', person_aidsource.AidSource_ID, person_aidsource.DateYear , person_aidsource.Amount, person_aidsource.ProjectType, person_aidsource.Notes" +
                " from person_aidsource left join person on person_aidsource.Person_ID = person.P_ID" +
                " left join person_family on person.P_ID = person_family.Person_ID" +
                " left join relation on relation.ID = person_family.Relation_ID" +
                " where person_family.P_Provider_ID = @Person_ID;";

            query += " select person_goals.Goal_ID , goal.Name as 'Goal_Name'" +
                " from person_goals left join goal on person_goals.Goal_ID = goal.ID" +
                " where Person_ID = @Person_ID;";

            query += " select person_education.ID, person_education.Level_ID , person_education.Education_ID, educationtype.ID as 'Educationtype_ID'" +
                " from person_education left join education on person_education.Education_ID = education.ID" +
                " left join educationtype on educationtype.id = education.Type_ID" +
                " where Person_ID = @Person_ID;";

            query += " select person_course.ID , person_course.Name, person_course.Place, person_course.Donor, person_course.Date, person_course.SubCategory_ID" +
             " from person_course" +
             " where Person_ID = @Person_ID;";

            query += " select microproject_powersource.ID, microproject_powersource.PowerSource_ID , microproject_powersource.Note, microproject_powersource.Cost, microproject_powersource.Responsability" +
                " from microproject_powersource" +
                " where microproject_powersource.MicroProject_ID = @MicroProject_ID;";

            query += " select material.ID, material.Name, material.Amount, material.Price, material.Comments" +
                " from material" +
                " where material.MicroProject_ID = @MicroProject_ID;";


            query += " select person_workexp.ID, person_workexp.Person_ID, person_workexp.W_Description, person_workexp.W_WorkType_ID, person_workexp.W_Salary, relation.Name as 'Relation_Name'" +
                " from person_workexp left join person on person_workexp.Person_ID = person.P_ID" +
                " left join person_family on person.P_ID = person_family.Person_ID" +
                " left join relation on relation.ID = person_family.Relation_ID" +
                " where person_family.P_Provider_ID = @Person_ID and person.P_ID <> @Person_ID;";


            query += " select person_loss.ID, person_loss.Loss_ID, person_loss.Year, person_loss.Note" +
                " from person_loss where Person_ID = @Person_ID;";


            query += " select person_illness.ID, person_illness.Person_ID, person_illness.Illness_ID, person_illness.Has_MedicalAid, person_illness.AidSource_ID, person_illness.Note, person_illness.AidPeriod_ID, person_illness.MedicalNeeds_ID, relation.Name as 'Relation_Name'" +
                " from person_illness left join person on person_illness.Person_ID = person.P_ID" +
                " left join person_family on person.P_ID = person_family.Person_ID" +
                " left join relation on relation.ID = person_family.Relation_ID" +
                " where person_family.P_Provider_ID = @Person_ID;";


            query += " select person_asset.ID, person_asset.Person_ID, person_asset.Asset_ID, person_asset.Asset_Type_ID, person_asset.Asset_Benefits_ID, person_asset.Year, person_asset.Description, person_asset.Note, relation.Name as 'Relation_Name'" +
                " from person_asset left join person on person_asset.Person_ID = person.P_ID" +
                " left join person_family on person.P_ID = person_family.Person_ID" +
                " left join relation on relation.ID = person_family.Relation_ID" +
                " where person_family.P_Provider_ID = @Person_ID;";

            query += " select person_help.ID, person_help.AidSource_ID, person_help.Aid_ID, person_help.Aidperiod_ID, person_help.Notes" +
                " from person_help where person_help.Person_ID = @Person_ID;";

            query += " select person_powersource.ID, person_powersource.PowerSource_ID, person_powersource.Note , person_powersource.Cost, person_powersource.Responsability" +
                " from person_powersource where person_powersource.Person_ID = @Person_ID;";

            var sc = new MySqlCommand();

            sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
            sc.Parameters.AddWithValue("@Person_ID", Person_ID);


            DataSet ds = new DataSet();

            Program.buildConnection();
            sc.CommandTimeout = 120;
            sc.CommandText = "SET SESSION wait_timeout = @t;" + query;
            sc.Parameters.AddWithValue("@t", 600);
            sc.Connection = Program.MyConn;
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            da.Fill(ds);
            Program.MyConn.Close();

            return ds;
        }
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

        public void Update_Person_Education(int ID, int Education_ID)
        {

            Program.buildConnection();
            query = "UPDATE `person_education` SET "
                    + " `Education_ID` = @Education_ID "
                    + " WHERE `ID`= @ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Education_ID", Education_ID);
                sc.Parameters.AddWithValue("@ID", ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Person_WorkExp(int ID, int Work_ID
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
                    + " WHERE `ID`= @ID ";
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
                sc.Parameters.AddWithValue("@ID", ID);
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
         



    }
}
