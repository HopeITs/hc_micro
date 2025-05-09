using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWorkApplication.Visit_Forms
{
    public class Visit
    {
        private string query;

        public Visit()
        {
            query = "";
        }

        public void Insert_Visit(int MicroProject_ID, string Kind, string Type, string Date,
            string Ans1, string Ans2, string Ans3, string Ans4, string Ans5, string Ans6, string Ans7, string Indicators
            , int Ans1_Value, int Ans2_Value, int Ans3_Value
            , int Ans4_Value, int Ans5_Value, int Ans6_Value, int Ans7_Value
            , string VehicleNumber
            , string CreatedBy)
        {
            Program.buildConnection();
            query = @"INSERT INTO `Visit`(`MicroProject_ID`, `Kind`, `Type`, `Date` , `Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`,`Ans7`, `Indicators` 
 , `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value`, `Ans7_Value` ,`VehicleNumber`, `Created_By` ) VALUES 
 (@MicroProject_ID ,@Kind ,@Type, @Date, @Ans1 ,@Ans2 ,@Ans3 ,@Ans4 ,@Ans5 ,@Ans6, @Ans7 ,@Indicators 
 ,@Ans1_Value,@Ans2_Value,@Ans3_Value,@Ans4_Value,@Ans5_Value,@Ans6_Value,@Ans7_Value
 ,@VehicleNumber
 ,@CreatedBy )";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@Kind", Kind);
                sc.Parameters.AddWithValue("@Type", Type);
                sc.Parameters.AddWithValue("@Date", Date);

                sc.Parameters.AddWithValue("@Ans1", Ans1);
                sc.Parameters.AddWithValue("@Ans2", Ans2);
                sc.Parameters.AddWithValue("@Ans3", Ans3);
                sc.Parameters.AddWithValue("@Ans4", Ans4);
                sc.Parameters.AddWithValue("@Ans5", Ans5);
                sc.Parameters.AddWithValue("@Ans6", Ans6);
                sc.Parameters.AddWithValue("@Ans7", Ans7);

                sc.Parameters.AddWithValue("@Indicators", Indicators);

                sc.Parameters.AddWithValue("@Ans1_Value", Ans1_Value);
                sc.Parameters.AddWithValue("@Ans2_Value", Ans2_Value);
                sc.Parameters.AddWithValue("@Ans3_Value", Ans3_Value);
                sc.Parameters.AddWithValue("@Ans4_Value", Ans4_Value);
                sc.Parameters.AddWithValue("@Ans5_Value", Ans5_Value);
                sc.Parameters.AddWithValue("@Ans6_Value", Ans6_Value);
                sc.Parameters.AddWithValue("@Ans7_Value", Ans7_Value);

                sc.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                sc.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
         
        public void Update_Visit(int ID, string Type, string Date,
            string Ans1, string Ans2, string Ans3, string Ans4, string Ans5, string Ans6,string Ans7
            , string Indicators
            , int Ans1_Value, int Ans2_Value, int Ans3_Value
            , int Ans4_Value, int Ans5_Value, int Ans6_Value, int Ans7_Value
            , string VehicleNumber
            , string Edited_By)
        {
            Program.buildConnection();
            query = "update `Visit` set " +
                    " `Type`= @Type "+ 
                    ",`Date`= @Date "+  
                    ",`Ans1`= @Ans1 "+ ",`Ans2`=@Ans2 " +
                    ",`Ans3`= @Ans3 "+ ",`Ans4`=@Ans4 " +
                    ",`Ans5`= @Ans5 "+ ",`Ans6`=@Ans6 " +
                    ",`Ans7`= @Ans7 "+ 
                    ",`Indicators`=@Indicators "+  
                    ",`Ans1_Value`=@Ans1_Value "+ ",`Ans2_Value`= @Ans2_Value" +
                    ",`Ans3_Value`=@Ans3_Value "+ ",`Ans4_Value`= @Ans4_Value" +
                    ",`Ans5_Value`=@Ans5_Value "+ ",`Ans6_Value`= @Ans6_Value" +
                    ",`Ans7_Value`=@Ans7_Value "+
                    ",`VehicleNumber`= @VehicleNumber" +
                    ",`Edited_By`= @Edited_By" +
                    " where `ID` = @ID";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID); 
                sc.Parameters.AddWithValue("@Type", Type);
                sc.Parameters.AddWithValue("@Date", Date);

                sc.Parameters.AddWithValue("@Ans1", Ans1);
                sc.Parameters.AddWithValue("@Ans2", Ans2);
                sc.Parameters.AddWithValue("@Ans3", Ans3);
                sc.Parameters.AddWithValue("@Ans4", Ans4);
                sc.Parameters.AddWithValue("@Ans5", Ans5);
                sc.Parameters.AddWithValue("@Ans6", Ans6);
                sc.Parameters.AddWithValue("@Ans7", Ans7);

                sc.Parameters.AddWithValue("@Indicators", Indicators);

                sc.Parameters.AddWithValue("@Ans1_Value", Ans1_Value);
                sc.Parameters.AddWithValue("@Ans2_Value", Ans2_Value);
                sc.Parameters.AddWithValue("@Ans3_Value", Ans3_Value);
                sc.Parameters.AddWithValue("@Ans4_Value", Ans4_Value);
                sc.Parameters.AddWithValue("@Ans5_Value", Ans5_Value);
                sc.Parameters.AddWithValue("@Ans6_Value", Ans6_Value);
                sc.Parameters.AddWithValue("@Ans7_Value", Ans7_Value);

                sc.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                sc.Parameters.AddWithValue("@Edited_By", Edited_By);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Visit(int ID)
        {
            Program.buildConnection();
            query = "delete from `Visit` where `ID` =@ID ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public int Get_Current_Visit(string Kind)
        {
            var V_ID = -1;
            Program.buildConnection();
            query = "select MAX(ID) from `Visit` where `Kind` like @Kind ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Kind", Kind);
                int.TryParse(sc.ExecuteScalar().ToString(), out V_ID);
                Program.MyConn.Close();
            }

            return V_ID;
        }

        public void Insert_Visit_User(int V_ID, int User_ID)
        {
            Program.buildConnection();
            query = "INSERT INTO `visit_user`(`Visit_ID`, `User_ID`) values(@V_ID,@User_ID)";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@V_ID", V_ID);
                sc.Parameters.AddWithValue("@User_ID", User_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Visit_Users(int V_ID)
        {
            Program.buildConnection();
            query = "delete from `visit_user` where `Visit_ID` = @V_ID" ;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@V_ID", V_ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Visit_Users(int V_ID)
        {
            Program.buildConnection();
            query = "SELECT `User_ID`,`UserName` FROM `visit_user` join `user` on user.UserID = visit_user.User_ID"
                    + " WHERE `Visit_ID` = @V_ID ";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@V_ID", V_ID);
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);
                Program.MyConn.Close();

                return dt;
            } 
        }

        public DataTable Get_Visit(string V_ID, string name, string MicroProject_ID,string _Kind)
        {
            Program.buildConnection();
            query = @"SELECT PMP.`MicroProject_ID` as 'MicroProject_ID'
, PMP.Person_ID as 'Beneficiary_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, microproject.MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date'
, v.ID as 'ID'
, MP_Street_ID 
, (case when (street.Name like 'متنقل') then
     street.Name
     else CONCAT(street.Name,' - ',MP_AddressAfterFund) END ) as 'Address'

, MP_AddressAfterFund as 'Short Address' 
, microprojecttype.Name as 'Type'

, `Type` , `Date` , `Kind`
, `Ans1`, `Ans2`, `Ans3`, `Ans4`, `Ans5`, `Ans6`, `Ans7`
, `Indicators`
, `Ans1_Value`, `Ans2_Value`, `Ans3_Value`, `Ans4_Value`, `Ans5_Value`, `Ans6_Value`, `Ans7_Value`
, VehicleNumber
, Created_By, Edited_By ";

            string condition = " where 1 ";
            string str = "";
            string from = @" FROM person_microproject PMP 
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID
 left join loan L on L.MicroProject_ID = microproject.MP_ID 
 left join visit v on PMP.MicroProject_ID = v.MicroProject_ID 
 left join street on street.ID =  microproject.MP_Street_ID 
 left join microprojecttype on microprojecttype.ID = microproject.MP_Type_ID ";

            if (V_ID != "") condition += " and v.ID = " + V_ID + ""; 
            if (name != "") condition+= " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + name + "%'";
            if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;
            if (_Kind != "") condition += " and Kind like '" + _Kind + "' ";
             
            query += from + condition;

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        } 
         
        public bool Has_Visit(string name, string MicroProject_ID, string _Kind)
        {
            var V_count = 0; 

            Program.buildConnection();
            query = @"SELECT count(*) ";

            string condition = " where 1 ";
            string str = "";
            string from = @" FROM visit v 
 left join person_microproject PMP on PMP.MicroProject_ID = v.MicroProject_ID
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID ";
             
            if (name != "") condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '%" + name + "%'";
            if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;
            if (_Kind != "") condition += " and Kind like '" + _Kind + "' ";

            query += from + condition;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out V_count);
                Program.MyConn.Close();
            }
            if (V_count == 0) return false;
            else return true;
        }

        public DataTable GetVisitsOfBeneficiary(string MP_ID, string Name)
        {
            Program.buildConnection();
            var query = @" select PMP.MicroProject_ID as 'MicroProject_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, PMP.Person_ID as 'Person_ID'
, MP.SubCategory_ID as 'SubCategory_ID' 
, microprojecttype.Name as 'Type' 
, fundtype.Name as 'FundType' 
, V.Date as 'Date'
, V.Kind as 'Kind'
, V.ID as 'V_ID'";

            var from = @" from person_microproject PMP 
 left join microproject MP on MP.MP_ID = PMP.MicroProject_ID 
 left join person P1 on P1.P_ID = PMP.Person_ID 
 left join visit V on PMP.MicroProject_ID = V.MicroProject_ID 
 left join microprojecttype on microprojecttype.ID = MP.MP_Type_ID
 left join fundtype on fundtype.ID = MP.MP_FundType_ID ";

            var condition = " where 1 ";
            if (Name != "")
                condition +=
                    " and CONCAT(TRIM(P1.P_FirstName),' ', TRIM(P1.P_LastName),' ابن/ة ',TRIM(P1.P_FatherName)) LIKE '%" +
                    Name + "%'";
            if (MP_ID != "")   // and PersonType like 'مستفيد' 
                condition += " and PMP.MicroProject_ID = " + Convert.ToInt32(MP_ID);
            query += from + condition;

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataSet Get_Visits_Statistics(string FromDate, string ToDate, string Type)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand();
            sc.Parameters.AddWithValue("@FromDate", FromDate);
            sc.Parameters.AddWithValue("@ToDate", ToDate);
            sc.Parameters.AddWithValue("@Type", Type);
            string query = " CALL `visit_user_statistics`(@FromDate, @ToDate, @Type); ";
            sc.CommandText = query;
            sc.Connection = Program.MyConn;
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyFirstConn.Close();
            return ds;
        }

        private string Check_Conditions(
              string DateFrom, string DateTo
            , string visit_Kind
            , string mp_type, string category, string subCategory, string career)
        {
            //string visit_Num, // string visit_Kind
            string mp_type_condtition = "";
            string category_condtition = "";
            string subCategory_condtition = "";
            string career_condtition = "";

            string visit_Number_condition = "";//  , inner_visit_Number_condition = "";
            string visit_Kind_condition = "";//  , inner_visit_Kind_condition = "";
            string visit_Date_condition = "";//  , inner_visit_Date_condition = "";

            string cond = " and visit.ID in ("
                + " SELECT v.ID FROM visit v "
                + " LEFT JOIN person_microproject pmp on v.MicroProject_ID = pmp.MicroProject_ID "
                + " LEFT JOIN microproject mp on mp.MP_ID = pmp.MicroProject_ID "
                + " WHERE 1 ";

            if (mp_type != "") mp_type_condtition = " and mp.MP_Type_ID = " + mp_type;
            if (category != "") category_condtition = " and mp.Category_ID = " + category;
            if (subCategory != "") subCategory_condtition = " and mp.SubCategory_ID = " + subCategory;
            if (career != "") career_condtition = " and mp.Career_ID = " + career;

            cond += mp_type_condtition + category_condtition + subCategory_condtition + career_condtition + " ) ";

            if (visit_Kind != "")
            {
                visit_Kind_condition = @" and visit.Kind like '" + visit_Kind.ToUpper() + "%' ";
                //inner_visit_Kind_condition = @" and v.Kind like '" + visit_Kind + "%' ";
            }
            if (DateFrom != "" && DateTo != "")
            {
                visit_Date_condition = @" and visit.Date between '" + DateFrom + "' and '" + DateTo + "' ";
                //inner_visit_Date_condition = @" and v.Date between '" + DateFrom + "' and '" + DateTo + "' ";
            }

            string all_conditions = " ";
            all_conditions = visit_Number_condition
                + visit_Kind_condition
                + visit_Date_condition
                + cond;

            return all_conditions;
        }

  
        public DataSet Get_Beneficiaries_Answers(
             string DateFrom, string DateTo
           , string visit_Kind
           , string mp_type, string category, string subCategory, string career)
        {
            string conditions = Check_Conditions(
                 DateFrom, DateTo
                 , visit_Kind
                 , mp_type, category, subCategory, career);

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand();
            sc.Parameters.AddWithValue("@MyCondition", conditions);
            //sc.Parameters.AddWithValue("@MyKind", visit_Kind);
            string query = " CALL `beneficiaries_visit_answers`(@MyCondition); ";
            sc.CommandText = query;
            sc.Connection = Program.MyConn;
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyFirstConn.Close();
            return ds;
        }
    }
}