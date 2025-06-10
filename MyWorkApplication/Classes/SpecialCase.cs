using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class SpecialCase
    {
        private string query;

        public SpecialCase()
        {
            query = "";
        }

        public int Insert(int MicroProject_ID, int User_ID, int Type_ID, string Date
            , string Reason, string Description, string Action)
        {
            Program.buildConnection();
            query = @"INSERT INTO `specialcase` (`MicroProject_ID`, `User_ID`, `Type_ID`, `Date`, `Reason`, `Description`, `Action`) 
VALUES (@MicroProject_ID, @User_ID , @Type_ID , @Date , @Reason , @Description , @Action );";

            long lastID = -1;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@User_ID", User_ID);
                sc.Parameters.AddWithValue("@Type_ID", Type_ID);
                sc.Parameters.AddWithValue("@Date", Date);

                sc.Parameters.AddWithValue("@Reason", Reason);
                sc.Parameters.AddWithValue("@Description", Description);
                sc.Parameters.AddWithValue("@Action", Action);

                sc.ExecuteNonQuery();
                lastID = sc.LastInsertedId;

                Program.MyConn.Close();
            }
            return int.Parse(lastID.ToString());
        }

        public void Update(int ID, int User_ID, int Type_ID, string Date
            , string Reason, string Description, string Action)
        {
            Program.buildConnection();
            query = "update `specialcase` set " +
                    " `Type_ID`= @Type_ID " +
                    ",`User_ID`= @User_ID " +
                    ",`Date`= @Date " +
                    ",`Reason`= @Reason " +
                    ",`Description`= @Description " +
                    ",`Action`= @Action " +
                    " where `ID` = @ID";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);
                sc.Parameters.AddWithValue("@User_ID", User_ID);
                sc.Parameters.AddWithValue("@Type_ID", Type_ID);
                sc.Parameters.AddWithValue("@Date", Date);

                sc.Parameters.AddWithValue("@Reason", Reason);
                sc.Parameters.AddWithValue("@Description", Description);
                sc.Parameters.AddWithValue("@Action", Action);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete(int ID)
        {
            Program.buildConnection();
            query = "delete from `specialcase` where `ID` =@ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select_SpecialCase(string SC_ID, string name, string MicroProject_ID)
        {
            Program.buildConnection();
            query = @"SELECT PMP.`MicroProject_ID` as 'MicroProject_ID'
, PMP.Person_ID as 'Beneficiary_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, microproject.MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date'

, MP_Street_ID 
, (case when (street.Name like 'متنقل') then
     street.Name
     else CONCAT(street.Name,' - ',MP_AddressAfterFund) END ) as 'Address'

, MP_AddressAfterFund as 'Short Address' 
, microprojecttype.Name as 'Type' ";

            if (SC_ID != "")
                query += @"  , sc.ID as 'ID'
 , `User_ID` , user.UserName
 , `Type_ID` , specialcasetype.Name 'SpecialCase Type'
 , `Date`, `Reason`, `Description`, `Action` ";


            string condition = " where 1 ";
            string str = "";
            string from = @" FROM person_microproject PMP 
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID
 left join loan L on L.MicroProject_ID = microproject.MP_ID  
 left join street on street.ID =  microproject.MP_Street_ID 
 left join microprojecttype on microprojecttype.ID = microproject.MP_Type_ID 

 left join specialcase sc on PMP.MicroProject_ID = sc.MicroProject_ID 
 left join user on user.UserID = sc.User_ID
 left join specialcasetype on specialcasetype.ID = sc.Type_ID ";

            if (SC_ID != "") condition += " and sc.ID = " + SC_ID + "";
            if (name != "") condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + name + "%'";
            if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;

            query += from + condition;

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public bool Has_SpecialCase(string name, string MicroProject_ID)
        {
            var V_count = 0;

            Program.buildConnection();
            query = @"SELECT count(*) ";

            string condition = " where 1 ";
            string str = "";
            string from = @" FROM specialcase sc 
 left join person_microproject PMP on PMP.MicroProject_ID = sc.MicroProject_ID
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID ";

            if (name != "") condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + name + "%'";
            if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;

            query += from + condition;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out V_count);
                Program.MyConn.Close();
            }
            if (V_count == 0) return false;
            else return true;
        }
    }
}
