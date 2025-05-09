using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWorkApplication.Visit_Forms
{
    public class M_and_E
    {
        private string query;

        public M_and_E()
        {
            query = "";
        }

        public void Insert_Visit(int Person_ID, string Number, string Kind, string Type, DateTime Date, double Profit,
            string Ans1, string Ans2, string Indicators, string Notes)
        {
            Program.buildConnection();
            query =
                "INSERT INTO `mevisit`(`Person_ID`,`Number`, `Kind`, `Type`, `Date`, `Profit`, `Ans1`, `Ans2`" +
                ", `Indicators`, `Notes`,`Result`, Created_By) VALUES  ("
                + Person_ID + ",'"
                + Number + "',N'"
                + Kind + "',N'"
                + Type + "',N'"
                + Date.Year + "/" + Date.Month + "/" + Date.Day + "',"
                + Profit + ",N'"
                + Ans1 + "',N'"
                + Ans2 + "',N'"
                + Indicators + "',N'"
                + Notes + "',"
                + 0 + ",'"+ Program.UserName + "')";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Visit(int ID, string Type,string Number, DateTime Date, double Profit,
            string Ans1, string Ans2, string Indicators, string Notes)
        {
            Program.buildConnection();
            query = "update `mevisit` set " +
                    " `Type`=N'" + Type + "'" +
                    ",`Profit`= " + Profit + " " +
                    ",`Date`=N'" + Date.Year + "/" + Date.Month + "/" + Date.Day + "'" +
                    ",`Ans1`=N'" + Ans1 + "'" +
                    ",`Ans2`=N'" + Ans2 + "'" +
                    ",`Indicators`=N'" + Indicators + "'" +
                    ",`Notes`=N'" + Notes + "'" +
                    ",`Number` = N'" + Number + "'" +
                    ",Edited_By = N'" + Program.UserName + "'" +
                    " where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Visit(DateTime Date, int Person_ID, int Profit)
        {
            Program.buildConnection();
            query = "update `mevisit` set  `Profit`= " + Profit + " " +
                    " where Person_ID = " + Person_ID + " and `Date`=N'" + Date.Year + "/" + Date.Month + "/" +
                    Date.Day + "'";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_VisitResult(int ID, int Result)
        {
            Program.buildConnection();
            query = "update `mevisit` set Result = " + Result + " where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Visit(int ID)
        {
            Program.buildConnection();
            query = "delete from `mevisit` where `ID` = " + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public int Get_Current_Visit(string Kind, string Number)
        {
            var V_ID = -1;
            Program.buildConnection();
            query = "select MAX(ID) from `mevisit` where `Kind` like '" + Kind + "' and Number = '" + Number + "'";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out V_ID);
                Program.MyConn.Close();
            }

            return V_ID;
        }

        public void Insert_Visit_User(int V_ID, int User_ID, string Mark)
        {
            Program.buildConnection();
            query = "INSERT INTO `mevisit_user`(`Visit_ID`, `User_ID`,`Mark`) values("
                    + V_ID + ","
                    + User_ID + ",N'"
                    + Mark + "' )";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_Visit_Users(int V_ID)
        {
            Program.buildConnection();
            query = "delete from `mevisit_user` where `Visit_ID` = " + V_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_Visit_Users(int V_ID)
        {
            Program.buildConnection();
            query =
                "SELECT `User_ID`,`UserName`,`Mark` FROM `mevisit_user` join `user` on user.UserID = mevisit_user.User_ID"
                + " WHERE `Visit_ID` = " + V_ID + "";
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable Get_Visit(int V_ID)
        {
            Program.buildConnection();
            query = @"SELECT v.Person_ID as 'Beneficiary_ID'
, person_microproject.MicroProject_ID as 'MicroProject_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, Loan_Amount as 'Loan Amount' 
, Loan_DateTaken as 'Loan Date'
, MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, MP_Street_ID 
, (case when (street.Name like 'متنقل') then
     street.Name
     else CONCAT(street.Name,' - ',MP_AddressAfterFund) END ) as 'Address'
, `Type`
, `Number`
, `Profit`
, `Date`
, `Kind`
, `Ans1`, `Ans2`, `Indicators` , `Notes` 
, Created_By , Edited_By

 FROM mevisit v
 left join person_microproject on person_microproject.Person_ID = v.Person_ID
 left join microproject on person_microproject.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = person_microproject.Person_ID
 left join loan on loan.MicroProject_ID = person_microproject.MicroProject_ID
 left join street on street.ID =  microproject.MP_Street_ID
 where v.ID = " + V_ID + "";
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable SearchByName(string name,string mp_id)
        {
             
            Program.buildConnection();
            var query = @"select PMP.MicroProject_ID as 'MicroProject_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, PMP.Person_ID as 'Beneficiary_ID'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date' 
, MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, MP_Street_ID 
, (case when (street.Name like 'متنقل') then
     street.Name
     else CONCAT(street.Name,' - ',MP_AddressAfterFund) END ) as 'Address'

, V.ID as 'ID'
,`Kind`, `Profit`, `Number`
,`Type`, `Date`, `Ans1`, `Ans2`, `Indicators` , `Notes`";

            var from = @" from person_microproject PMP 
 left join person P1 on P1.P_ID = PMP.Person_ID 
 left join microproject MP on MP.MP_ID = PMP.MicroProject_ID 
 left join street on street.ID =  MP.MP_Street_ID
 left join loan L on PMP.MicroProject_ID = L.MicroProject_ID
 left join mevisit V on PMP.Person_ID = V.Person_ID";

            var Condition =
                " WHERE CONCAT(TRIM(P1.P_FirstName),' ', TRIM(P1.P_LastName),' ابن/ة ',TRIM(P1.P_FatherName)) LIKE '%" + name + "%'";

            if(mp_id != "") Condition += " and PMP.MicroProject_ID = " + mp_id;

            query += from + Condition;
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable SearchByID(string MicroProject_ID)
        {
            
            Program.buildConnection();
            var query = @"select PMP.MicroProject_ID as 'MicroProject_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, PMP.Person_ID as 'Beneficiary_ID'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date' 
, MP_Name as 'Project Name'
, P_Mobile as 'Mobile'
, MP_Street_ID 
, (case when (street.Name like 'متنقل') then
     street.Name
     else CONCAT(street.Name,' - ',MP_AddressAfterFund) END ) as 'Address'

, V.ID as 'ID' 
,`Kind`, `Profit`, `Number`
,`Type`, `Date`, `Ans1`, `Ans2`, `Indicators` , `Notes` ";

            var from = @" from person_microproject PMP 
 left join person P1 on P1.P_ID = PMP.Person_ID
 left join microproject MP on MP.MP_ID = PMP.MicroProject_ID
 left join street on street.ID =  MP.MP_Street_ID
 left join loan L on PMP.MicroProject_ID = L.MicroProject_ID
 left join mevisit V on PMP.Person_ID = V.Person_ID ";


            var Condition = " where PMP.MicroProject_ID = " + MicroProject_ID;
            //if (V_Num != "")
            //    Condition += " and Number like '" + V_Num + "' ";

            query += from + Condition;

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        public DataTable Get_Questions_Answers(string type)
        {
            Program.buildConnection();
            query = @"SELECT mequestion.`Name` as 'Q_Name'
, mequestion.`Type` as 'Q_Type' 
, meanswer.`ID` as 'A_ID'
, meanswer.`Name` as 'A_Name'
, meanswer.`Mark` as 'A_Mark'
, `Question_ID` 
 from `mequestion` left join `meanswer` on mequestion.ID = meanswer.Question_ID ";

            var condition = "";
            if (type != "") condition = " where mequestion.`Type` like '%" + type + "%' ";
            query += condition + " order by A_ID";

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        private string Check_Conditions(bool onlyLastVisit
            , string visit_Kind, string visit_Num, string DateFrom, string DateTo
            , string mp_type, string category, string subCategory, string career)
        {
            //   Get the (last-first) visit of each funded beneficiary  
            string beneficiary_condtition = "";
            string orderBY = "";
            string mp_type_condtition = "";
            string category_condtition = "";
            string subCategory_condtition = "";
            string career_condtition = "";
            string visit_Number_condition = "", inner_visit_Number_condition = "";
            string visit_Kind_condition = "", inner_visit_Kind_condition = "";

            string Date_condition = "", inner_Date_condition = "";

            string cond = " and mevisit_answer.Visit_ID in (" +
            @" SELECT me.ID as 'V_ID' 
                FROM mevisit me
                LEFT JOIN person_microproject pmp on me.Person_ID = pmp.Person_ID
                LEFT JOIN microproject mp on mp.MP_ID = pmp.MicroProject_ID 
                WHERE me.Person_ID = pmp.Person_ID ";

            if (mp_type != "") mp_type_condtition = " and mp.MP_Type_ID = " + mp_type;
            if (category != "") category_condtition += " and mp.Category_ID = " + category;
            if (subCategory != "") subCategory_condtition += " and mp.SubCategory_ID = " + subCategory;
            if (career != "") career_condtition += " and mp.Career_ID = " + career;

            cond += mp_type_condtition + category_condtition + subCategory_condtition + career_condtition + " ) ";

            if (visit_Kind != "")
            { 
                visit_Kind_condition = @" and mevisit.Kind like '" + visit_Kind + "' ";
                inner_visit_Kind_condition = @" and me.Kind like '" + visit_Kind + "' "; 
            }
            if (visit_Num != "")
            {
                if (visit_Num.Length > 4) //all
                { }
                else //M1 - M5
                {
                    visit_Num = visit_Num.Substring(1, 1);
                    visit_Number_condition = @" and mevisit.Number like '" + visit_Num + "' ";
                    inner_visit_Number_condition = @" and me.Number like '" + visit_Num + "' ";
                }
            }
            if (DateFrom != "" && DateTo != "")
            {
                Date_condition = @" and mevisit.Date between '" + DateFrom + "' and '" + DateTo + "' ";
                inner_Date_condition = @" and me.Date between '" + DateFrom + "' and '" + DateTo + "' ";
            }
            if (onlyLastVisit)
            {
                beneficiary_condtition =
                " and mevisit_answer.Visit_ID in " +
                "( SELECT " +
                "   ( SELECT me.ID " +
                "       FROM mevisit me " +
                "       where me.Person_ID = mevisit.Person_ID "
                        + inner_Date_condition
                        + category_condtition + subCategory_condtition
                        + career_condtition + mp_type_condtition
                        + " ORDER BY mevisit.Number desc LIMIT 1) as 'V_ID' " +
                " FROM `mevisit` " +
                " left join person_microproject on person_microproject.Person_ID = mevisit.Person_ID  " +
                " left join microproject mp on mp.MP_ID = person_microproject.MicroProject_ID " +
                " WHERE 1 "
                        + Date_condition + visit_Number_condition
                        + category_condtition + subCategory_condtition
                        + career_condtition + mp_type_condtition +
                    "  GROUP by person_microproject.MicroProject_ID ) ";
            }

            string all_conditions = "";
            all_conditions = Date_condition + beneficiary_condtition
                + visit_Number_condition + visit_Kind_condition
                + cond;

            return all_conditions;
        }

        public DataTable Get_Questions_Answers_SideBySide(bool onlyLastVisit
            , string visit_Kind, string visit_Num , string DateFrom, string DateTo
            ,  string mp_type, string category, string subCategory, string career)
        {
            string conditions = Check_Conditions(onlyLastVisit
            , visit_Kind, visit_Num, DateFrom, DateTo
            , mp_type, category, subCategory, career);

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("count_mevisit_results", Program.MyConn);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@MyCondition", conditions);
            sc.Parameters.AddWithValue("@MyKind", visit_Kind); 
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt); 
                Program.MyConn.Close();
                return dt;
            }
        }

        public DataTable Get_Beneficiaries_Answers(bool onlyLastVisit
            , string visit_Kind, string visit_Num , string DateFrom , string DateTo
            , string mp_type , string category, string subCategory, string career)
        {
            string conditions = Check_Conditions(onlyLastVisit
            , visit_Kind, visit_Num, DateFrom, DateTo
            , mp_type, category, subCategory, career);

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("beneficiaries_me_answers", Program.MyConn);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@MyCondition", conditions);
            sc.Parameters.AddWithValue("@MyKind", visit_Kind);
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Program.MyConn.Close();
                return dt;
            }
        }
         
        public DataTable Get_Beneficaies(string Ans_IDs, string visitNumber, string DateFrom, string DateTo, string beneficiary, string category, string subCategory)
        {
            //TODO
            //   Get the (last-first) visit of each funded beneficiary  
            string beneficiary_condtition = "";
            string orderBY = "";
            if (beneficiary == "all") { beneficiary_condtition = ""; orderBY = ""; }
            else
            {
                if (beneficiary == "last") orderBY = " ORDER BY mevisit.Date desc ";
                if (beneficiary == "first") orderBY = " ORDER BY mevisit.Date asc ";

                beneficiary_condtition =
                    " and mevisit_answer.Visit_ID in ( " +
                    " SELECT (SELECT mevisit.ID FROM mevisit WHERE mevisit.Person_ID = person_microproject.Person_ID " +
                    orderBY + " LIMIT 1) as 'V_ID' " +
                    " FROM microproject JOIN person_microproject on microproject.MP_ID = person_microproject.MicroProject_ID " +
                    " WHERE microproject.MP_ID in (select MicroProject_ID from microproject_state " +
                                    " where MicroProject_ID = microproject.MP_ID " +
                                    " and State_ID in (1,4,5,7,8) " +
                                    " and Current_State = 1 ) "; 
                    //"(1, 4, 5) ) ";
            }
              
            Program.buildConnection();
            query = @"SELECT person_microproject.MicroProject_ID as 'رقم المشروع'
, CONCAT(P_FirstName, ' ', P_LastName) as 'المستفيد'
, mevisit.Number as 'رقم الزيارة'
 FROM mevisit_answer 
 join `mevisit` on mevisit.ID = mevisit_answer.Visit_ID
 join person on person.P_ID = mevisit.Person_ID
 join person_microproject on person_microproject.Person_ID =  mevisit.Person_ID
 join microproject on person_microproject.MicroProject_ID =  microproject.MP_ID ";

            string condition = " WHERE " + Ans_IDs ;
            if (visitNumber != "")
                condition += " and mevisit.Number like '" + visitNumber + "' ";

            if (DateFrom != "" && DateTo != "")
                condition += @" and mevisit.Date between '" + DateFrom + "' and '" + DateTo + "' ";

            if (category != "")
                condition += " and microproject.Category_ID = (select ID from category where Name like '" + category + "' limit 1) ";

            if (subCategory != "")
                condition += " and microproject.SubCategory_ID = (select ID from subcategory where Name like '" + subCategory + "' limit 1) ";

            string orderBy = " order by MicroProject_ID ";

            query += condition + orderBy;

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
         
        public void Insert_meVisit_Answers(int Visit_ID, int Question_ID, int Answer_ID, string Notes)
        {
            Program.buildConnection();
            query = "INSERT INTO `mevisit_answer`(`Visit_ID`, `Question_ID`, `Answer_ID`,`Notes`) VALUES ("
                    + Visit_ID + ","
                    + Question_ID + ","
                    + Answer_ID + ",N'"
                    + Notes + "' )";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_meVisit_Answers(int V_ID)
        {
            Program.buildConnection();
            query = "delete from `mevisit_answer` where `Visit_ID` = " + V_ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Get_meVisit_Answers(int V_ID, string Type)
        {
            Program.buildConnection();
            query = @"SELECT `Visit_ID`, `Question_ID`, `Answer_ID` , `Notes`
 FROM mevisit_answer left join mequestion on mevisit_answer.Question_ID = mequestion.ID ";

            var condition = " where Visit_ID = " + V_ID + " ";
            if (Type != "")
                condition += " and Type like '" + Type + "' ";

            query += condition;
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable GetVisitsOfBeneficiary(string MP_ID, string Name)
        {
            Program.buildConnection();
            var query = @"select MP.MP_ID as 'MicroProject_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, PMP.Person_ID as 'Person_ID'
, MP.SubCategory_ID as 'SubCategory_ID'
, microprojecttype.Name as 'Type' 
, V.Kind as 'Kind' 
, V.Date as 'Date'
, V.Number as 'Number'
, V.ID as 'V_ID'";

            var from = @"from microproject MP
 left outer join person_microproject PMP on MP.MP_ID = PMP.MicroProject_ID
 left outer join person P1 on P1.P_ID = PMP.Person_ID
 left outer join mevisit V on PMP.Person_ID = V.Person_ID 
 left outer join microprojecttype on microprojecttype.ID = MP.MP_Type_ID
 LEFT OUTER JOIN microproject_state mps on mps.MicroProject_ID = MP.MP_ID  ";

            var condition = " where Current_State = 1 and mps.State_ID in (1,4,5,7,8) ";
            if (Name != "")
                condition +=
                    " and CONCAT(TRIM(P1.P_FirstName),' ', TRIM(P1.P_LastName),' ابن/ة ',TRIM(P1.P_FatherName)) LIKE '%" +
                    Name + "%'";
            if (MP_ID != "")
                condition += " and MP.MP_ID = " + Convert.ToInt32(MP_ID);
            query += from + condition + " order by V.Date asc ";

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public int Get_Mark_Of_Profit(Double profit)
        {
            Program.buildConnection();
            int mark = 0;
            query = @"SELECT `Mark` FROM `meprofitmarks` ";

            string condition = " where IsActive = 1 ";

            condition += " and Min <= " + profit + " and Max >= " + profit + "";

            query += condition;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                int.TryParse(sc.ExecuteScalar().ToString(), out mark);
                Program.MyConn.Close();
            }

            return mark;
        }
    }
}