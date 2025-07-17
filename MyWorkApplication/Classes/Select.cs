using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal class Select
    {
        private byte[] arr;
        private string Loan_from, Loan_condition;
        private readonly MySqlComponents MySS;

        public Select()
        {
            MySS = new MySqlComponents();
        }

        public DataSet Select_Applications(string condition, bool show_Images)
        {
            string from = @" From microproject 
LEFT OUTER JOIN person_microproject on microproject.MP_ID = person_microproject.MicroProject_ID 
LEFT OUTER JOIN person on person.P_ID = person_microproject.Person_ID  
LEFT OUTER JOIN priest on person.P_Priest_ID = priest.Priest_ID 
left outer join parish on person.P_Parish_ID = parish.ID 
LEFT OUTER JOIN maritalstatus on person.P_MaritalStatus_ID = maritalstatus.ID 
LEFT OUTER JOIN category on microproject.Category_ID = category.ID 
LEFT OUTER JOIN subcategory on microproject.SubCategory_ID = subcategory.ID  

LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = microproject.MP_ID
LEFT OUTER JOIN `state` on state.ID = mps.State_ID 

LEFT OUTER JOIN donor on microproject.MP_Donor = donor.ID 
LEFT OUTER JOIN street w_street on microproject.MP_Street_ID = w_street.ID 
LEFT OUTER JOIN street h_street on person.Street_ID = h_street.ID 
LEFT OUTER JOIN loan on microproject.MP_ID = loan.MicroProject_ID 
LEFT OUTER JOIN donorgroup on microproject.DonorGroup_ID = donorgroup.ID  
LEFT OUTER JOIN fundtype on microproject.MP_FundType_ID = fundtype.ID 
LEFT OUTER JOIN microprojecttype on microproject.MP_Type_ID = microprojecttype.ID 
LEFT OUTER JOIN microprojectsubtype on microproject.MP_SubType_ID = microprojectsubtype.ID ";

            string query1 = @" Select MP_ID as 'رقم المشروع'  
, CONCAT(P_FirstName, ' ', P_LastName) as 'المستفيد'  
, P_FatherName as 'اسم الأب'  
, P_MotherName as 'اسم الأم'  
, (SELECT CONCAT(p.P_FirstName, ' ', p.P_FatherName, ' ', p.P_LastName) 
    From `person` p 
    left join `person_family` pf on p.P_ID = pf.Person_ID  
    WHERE pf.Relation like N'زوج/ة' 
    and pf.P_Provider_ID = person.P_ID limit 1) as 'الزوج / الزوجة' 
, MP_Name as 'اسم المشروع' 
, category.Name 'الصنف' 
, subcategory.Name as 'المهنة' 
 
, parish.Name_ar as 'الطائفة'

, IFNULL(Priest_Name,'') as 'الكاهن'
, h_street.Name as 'منطقة السكن' 
, P_HomeAddress as 'العنوان' 
, P_RegistrationPlace as 'القيد' 
, P_NationalNumber as 'الرقم الوطني' 
, P_Mobile as 'الموبايل' 
  
, (SELECT IFNULL(COUNT(person_family.Person_ID),0) from person_family 
left join person p on person_family.Person_ID = p.P_ID 
left join personstate on p.P_State_ID = personstate.ID 
WHERE person_family.P_Provider_ID = person.P_ID and lower(personstate.Type) like 'active' ) as 'عدد الأفراد' 

, (SELECT IFNULL(COUNT(person_family.Person_ID),0) from person_family  
WHERE person_family.P_Provider_ID = person.P_ID and person_family.Relation like 'ابن/ة' ) as 'عدد الأبناء' 

, P_MilitaryService as 'خدمة العلم'
, P_MilitaryService_Note as 'وصف الحالة العسكرية' 
, MP_DateOfRequest as 'تاريخ التقديم'  
, fundtype.Name as 'نوع التمويل' 
, microprojecttype.Name as 'نوع المشروع'
, microprojectsubtype.Name as 'تفصيل نوع المشروع' 
, state.Name_Ar as 'حالة المشروع' 
, mps.Date as 'تاريخ الحالة'
, IFNULL(Loan_Amount,0) as 'مبلغ التمويل'
, Loan_DateTaken as 'تاريخ التمويل' 
, IFNULL(donor.Name,'') as 'الجهة الممولة' 
, IFNULL(donorgroup.Name,'') as 'المجموعة' 
, w_street.Name 'منطقة المشروع'
, MP_AddressAfterFund  as 'موقع المشروع' 
, (CASE MP_Visited WHEN 0 THEN N'غير مزار' ELSE N'مزار' End) as 'الزيارة' 
, (CASE MP_Message WHEN 1 THEN N'تم الإرسال' ELSE 'لا يوجد' End) as 'رسالة الرفض' 
, (CASE IsContentUpdated WHEN 1 THEN N'نعم' ELSE 'لا' End) as 'تبديل المشروع' 
, person_microproject.PersonType as 'صفة مقدم الطلب' 
, PMP_ID as 'ID' 
, P_ID as 'رقم المستفيد' ";

            if (show_Images) query1 += " ,P_Picture as '#' ,P_PicturePath as 'Path' ";

            string order_by = " order by MP_ID DESC ";

            query1 += from + condition + order_by + ";";

            //calculate sum of family members for all projects// 
            string query2 = "select IFNULL(count(DISTINCT MP_ID),0),IFNULL(sum(P_NumAtHome),0)" + from + condition + ";";

            ////////////////////////////////////////////////////////////////////
            string query = " ";
            query += query1 + query2;
            query += " ";
            ////////////////////////////////////////////////////////////////////

            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            return ds;
        }

        #region family

        public DataTable FamilyMembers_bind(int Family_ID, int P_ID)
        {

            Program.buildConnection();
            var query = "select p.P_ID as 'ID'" +
                        ",p.P_FirstName as 'FirstName'" +
                        ",p.P_LastName as 'LastName'" +
                        ",p.P_MotherName as 'MotherName'" +
                        ",p.P_FatherName as 'FatherName'" +
                        ",p.P_NationalNumber as 'National Number'" +
                        ",pf.IsInNow as 'IsInNow'" +
                        ",pf.Relation as 'Relation'" +
                        ",p.P_Sex as 'Gender'" +
                        ",p.P_MaritalStatus_ID as 'Marital Status'" +
                        ",pf.Work_Name as 'Work'" +
                        ",pf.P_Provider_ID as 'Provider_ID'" +
                        ",p.P_DOB as 'Birth Date'" +
                        ",F.F_Number as 'Family Number' " +
                        ",pf.Family_ID as 'Family_ID'" +
                        ",p.P_State_ID as 'State_ID'" +
                        ",personstate.Name as 'State'" +

                        " from `person` p " +
                        " left outer join `person_family` pf on p.P_ID = pf.Person_ID " +
                        " left outer join `family` F on F.F_ID = pf.Family_ID " +
                        " left outer join `personstate` on p.P_State_ID = personstate.ID ";

            var condition = " where pf.P_Provider_ID = " + P_ID;
            if (Family_ID != -1)
                condition += " and pf.Family_ID = " + Family_ID;
            query += condition;

            var sc1 = new MySqlCommand(query, Program.MyConn);
            sc1.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc1);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable Select_FamilyMembers(string Family_ID, string Provider_ID)
        {
            Program.buildConnection();
            var query = "select p.P_ID as 'ID'"
                        + ",pf.Family_ID as 'رقم العائلة'"
                        + ",f.F_Number as 'رقم دفتر العائلة'"
                        + ",CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName) as 'الاسم الثلاثي'"
                        + ",P_MotherName as 'الأم'"
                        + ",maritalstatus.Name as 'الحالة الاجتماعية'"
                        + ",pf.IsInNow as 'من دفتر العائلة'"
                        + ",pf.Relation as 'علاقته مع المستفيد'"
                        + ",pf.Work_Name as 'العمل'"
                        + ",p.P_DOB as 'تاريخ الولادة'"
                        + " From `person` p " +
                        " left outer join `person_family` pf on p.P_ID = pf.Person_ID " +
                        " left outer join `family` f on f.F_ID = pf.Family_ID " +
                        " LEFT outer join `maritalstatus` on p.P_MaritalStatus_ID = maritalstatus.ID ";
            var condition = " where 1 ";
            if (Family_ID != "")
                condition += " and pf.Family_ID like CAST('" + Family_ID + "' AS CHAR) ";
            if (Family_ID != "")
                condition += " and pf.P_Provider_ID = " + Provider_ID + " ";
            query += condition;

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();

            return dt;
        }

        #endregion

        #region ExeFiles

        public DataTable ExeFile_bind(int ExeFile_ID)
        {

            Program.buildConnection();
            MySS.query = "select PMP.MicroProject_ID as 'MicroProject_ID'"
                         + " ,CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name'"
                         + " ,`ExeF_ID` as 'ID'"
                         + ",`ExeF_No` as 'Number'"
                         + ",`ExeF_BeginDate` as 'Begin Date'"
                         + ",`ExeF_CurrentDate` as 'Current Date'"
                         + ",`ExeF_ImpoundDate` as 'Impound Date'"
                         + ",`ExeF_ImpoundType` as 'Impound Type'"
                         + ",`ExeF_NumOfMonths` as 'Months'"
                         + " FROM `exefile` E right outer join  person_microproject PMP on E.MicroProject_ID = PMP.MicroProject_ID "
                         + " right outer join person P1 on P1.P_ID = PMP.Person_ID "
                         + " where ExeF_ID = " + ExeFile_ID + " and PMP.MicroProject_ID is not NULL ";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        #endregion

        public AutoCompleteStringCollection select_beneficiaries(string category, string Financed)
        {

            Program.buildConnection();
            var where = "";
            if (category == "")
            {
                where = "";
                if (Financed == "Yes")
                    where += " where MP_ID in (select MicroProject_ID from microproject_state " +
                                    " where MicroProject_ID = microproject.MP_ID " +
                                    " and State_ID in (1,4,5,7,8) " +
                                    " and Current_State = 1 ) ";
            }
            else if (category.Contains("V"))
            {
                where = " where microproject.Category_ID in (1, 2) ";
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                                    " where MicroProject_ID = microproject.MP_ID " +
                                    " and State_ID in (1,4,5,7,8) " +
                                    " and Current_State = 1 ) ";
            }
            else if (category.Contains("O"))
            {
                where = " where microproject.Category_ID not in (1, 2) ";
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                                    " where MicroProject_ID = microproject.MP_ID " +
                                    " and State_ID in (1,4,5,7,8) " +
                                    " and Current_State = 1 ) ";
            }
            else if (category.Contains("R"))
            {
                where = " where microproject.MP_Type_ID = 2 "; //ترميم
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                                    " where MicroProject_ID = microproject.MP_ID " +
                                    " and State_ID in (1,4,5,7,8) " +
                                    " and Current_State = 1 ) ";
            }

            var query = @"select CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Beneficiary Name' 
 from microproject 
 join person_microproject on person_microproject.MicroProject_ID = microproject.MP_ID  
 join person on person.P_ID = person_microproject.Person_ID "
            + where;

            MySqlCommand mySqlCommand = new MySqlCommand(query, Program.MyConn);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            while (reader.Read()) collection.Add(reader.GetString(0));
            return collection;
        }

        public AutoCompleteStringCollection select_project_IDs(string category, string Financed)
        {

            Program.buildConnection();
            var where = "";
            if (category == "")
            {
                where = "";
                if (Financed == "Yes")
                    where += " where MP_ID in (select MicroProject_ID from microproject_state " +
                 " where MicroProject_ID = microproject.MP_ID " +
                 " and State_ID in (1,4,5,7,8) " +
                 " and Current_State = 1  ) ";
            }
            else if (category.Contains("V"))
            {
                where = " where microproject.Category_ID in (1, 2) ";
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                " where MicroProject_ID = microproject.MP_ID " +
                " and State_ID in (1,4,5,7,8) " +
                " and Current_State = 1 ) ";
            }
            else if (category.Contains("O"))
            {
                where = " where microproject.Category_ID not in (1, 2) ";
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                " where MicroProject_ID = microproject.MP_ID " +
                " and State_ID in (1,4,5,7,8) " +
                " and Current_State = 1 ) ";
            }
            else if (category.Contains("R"))
            {
                where = " where microproject.MP_Type_ID = 2 "; //ترميم
                if (Financed == "Yes")
                    where += " and MP_ID in (select MicroProject_ID from microproject_state " +
                " where MicroProject_ID = microproject.MP_ID " +
                " and State_ID in (1,4,5,7,8) " +
                " and Current_State = 1 ) ";
            }
            var query = @"select MP_ID as 'MicroProject_ID' from microproject " + where;

            MySqlCommand mySqlCommand = new MySqlCommand(query, Program.MyConn);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            while (reader.Read()) collection.Add(reader.GetString(0));
            return collection;
        }

        public DataTable select_visitors()
        {

            Program.buildConnection();
            var query =
                @"select UserID as 'U_ID',UserName as 'Visitors' from `user` where IsVisitor = 1 order by UserName ASC";
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable select_users()
        {

            Program.buildConnection();
            var da = new MySqlDataAdapter(
                "select UserID as 'U_ID',UserName as 'Users' from `user` where IsVisitor != -1 order by UserName ASC",
                Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable select_M_E()
        {
            Program.buildConnection();
            var da = new MySqlDataAdapter(" select UserID as 'U_ID',UserName as 'Users' from `user` where IsME = 1 ",
                Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable select_Communication()
        {

            Program.buildConnection();
            var da = new MySqlDataAdapter(
                " select UserID as 'U_ID',UserName as 'Users' from `user` where IsCommunication = 1 ", Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        // to send the payments notifications must be with role financial OR isCashier = 1 //
        public DataTable select_Financial()
        {

            Program.buildConnection();
            var da = new MySqlDataAdapter(
                " select UserID as 'U_ID',UserName as 'Users' from `user` where UserRoleID = 3 or IsCashier = 1 ", Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable select_Lawful()
        {

            Program.buildConnection();
            var da = new MySqlDataAdapter(
                " select UserID as 'U_ID',UserName as 'Users' from `user` where UserRoleID = 7 ", Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable select_Beneficiaries_of_project(string MicroProject_ID)
        {

            Program.buildConnection();
            string query = @"select Person_ID as 'ID'
 , CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) as 'Name'
 , person_microproject.MicroProject_ID as 'MicroProject_ID'

  from person_microproject left join person on person.P_ID = person_microproject.Person_ID ";
            string condition = "";
            if (MicroProject_ID != "")
                condition += " where person_microproject.MicroProject_ID = " + MicroProject_ID;

            query += condition;

            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Program.MyConn.Close();
            return dt;
        }

        public int HasPartner(int MicroProject_ID)
        {

            Program.buildConnection();
            MySS.query = "SELECT count(Person_ID) FROM `person_microproject` WHERE MicroProject_ID = " +
                         MicroProject_ID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var count = Convert.ToInt32(MySS.sc.ExecuteScalar());
            Program.MyConn.Close();
            return count;
        }

        #region images

        public DataTable Image_bind(int MP_ID, string type)
        {

            Program.buildConnection();

            MySS.query = "SELECT `Image_ID` " +
                         ",`Image_Path`" +
                         " FROM `image` " +
                         " Where `MicroProject_ID` = " + MP_ID + " and `Image_Type` like '" + type + "'";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);

            MySS.da = new MySqlDataAdapter(MySS.query, Program.MyConn);
            MySS.reader = MySS.sc.ExecuteReader();
            MySS.dt = new DataTable();
            MySS.dt.Load(MySS.reader);
            Program.MyConn.Close();

            return MySS.dt;
        }

        public byte[] Show_image(int image_ID)
        {

            Program.buildConnection();

            byte[] arr = null;
            MySS.query = "select `Image_Content` from `image` where `Image_ID` = " + image_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.reader = MySS.sc.ExecuteReader();
            MySS.reader.Read();
            if (MySS.reader.HasRows) arr = (byte[])MySS.reader[0];
            MySS.reader.Close();
            Program.MyConn.Close();
            return arr;
        }

        #endregion

        #region loan

        public DataSet Payments_bind(string month, string year, string group, string paid)
        {
            var q = @"select 
 MP_ID as 'رقم المشروع' 
 ,(select CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) from person 
 left join `person_microproject` on person_microproject.Person_ID = person.P_ID 
 where person_microproject.PersonType like 'مستفيد' AND loan.MicroProject_ID = person_microproject.MicroProject_ID LIMIT 1) as 'المستفيد' 

 ,IFNULL((select CONCAT(P_FirstName,' ', P_LastName,' ابن/ة ',P_FatherName) from person 
 left join `person_microproject` on person_microproject.Person_ID = person.P_ID 
 where person_microproject.PersonType like 'شريك' AND loan.MicroProject_ID = person_microproject.MicroProject_ID LIMIT 1),'') as 'الشريك'

,`Pay_Amount` as 'القسط' 
,`Pay_DueDate` as 'تاريخ الاستحقاق' 
,`Pay_RecievedOnDate` as 'تاريخ الدفع الفعلي' 
, donorgroup.Name as 'المجموعة'
,`Pay_ID` as 'Pay_ID' ";

            string from = @" from `payment` left join `loan` on payment.Loan_ID = loan.Loan_ID  
 left join `microproject` on microproject.MP_ID = loan.MicroProject_ID  
 left join `donorgroup` on donorgroup.ID = microproject.DonorGroup_ID ";

            var filtered_col = "Pay_DueDate";
            if (paid == "Paid") filtered_col = "Pay_RecievedOnDate";
            else filtered_col = "Pay_DueDate";

            var condition = "  Where 1 ";
            if (month != "") condition += " and Month(" + filtered_col + ") in (" + month + ") ";
            if (year != "") condition += " and Year(" + filtered_col + ") like " + year;
            if (group != "") condition += " and lower(donorgroup.Name) Like lower('" + @group + "') ";
            if (paid != "") condition += " and Pay_IsPaid Like '" + paid + "' ";
            q += from + condition + " order by " + filtered_col + " asc;";

            q += " select IFNULL(SUM(Pay_Amount),0) " + from + condition + ";";

            Program.buildConnection();
            var sc = new MySqlCommand(q, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();
            return ds;
        }

        public DataTable Select_Loan_All_Data(string Loan_ID, string cond)
        {
            try
            {
                var from = @" from `loan` L 
     LEFT OUTER join `microproject` MP on L.MicroProject_ID = MP.MP_ID  
     LEFT OUTER join `person_microproject` PMP on PMP.MicroProject_ID = MP.MP_ID  
     LEFT OUTER join `person` P on P.P_ID = PMP.Person_ID  
     LEFT OUTER join `category` C on MP.Category_ID = C.ID  
     LEFT OUTER join `subcategory` sub on MP.SubCategory_ID = sub.ID  
     LEFT OUTER join `career` on MP.Career_ID = career.ID  

     LEFT OUTER JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID
     LEFT OUTER JOIN `state` on state.ID = mps.State_ID  

     LEFT OUTER join `donor` on donor.ID = MP.MP_Donor  
     LEFT OUTER join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
     LEFT OUTER JOIN `fundtype` on fundtype.ID = MP.MP_FundType_ID 
     LEFT OUTER JOIN `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID
     LEFT OUTER JOIN `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID ";

                var query = @"select L.Loan_ID as 'ID'
    ,L.MicroProject_ID as 'رقم المشروع' 
    , Group_Concat(CONCAT(P_FirstName, ' ' , P_LastName , ' ابن/ة ', P_FatherName)) as 'المستفيد' 
    , career.Name as 'المهنة' 
    , P_FullName_en as 'Beneficiary Name'
    , career.Name_en as 'Career'  

    , C.Name as 'Category'
    , sub.Name as 'Sub Category' 

    , state.Name_Ar as 'حالة المشروع'
    , P_Mobile as 'الموبايل' 

    ,L.Loan_DateTaken as 'تاريخ استلام القرض'
    ,L.Loan_Amount as 'القرض'
    ,Round( ( Loan_Amount / (SELECT Rate from donorgroup where donorgroup.ID = MP.DonorGroup_ID)) , 2 ) as 'القرض ($)'
    ,Round((L.Loan_ReturnedAmount * 100)/L.Loan_Amount, 2 ) as 'النسبة' 
    ,L.Loan_ReturnedAmount as 'المبلغ المطلوب استرداده'
    ,L.Loan_PaymentsAmount as 'القسط'

    ,(select sum(`Pay_Amount`) from `payment` where `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like N'Paid') as 'المبلغ المدفوع'

    ,IF((L.Loan_ReturnedAmount - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))<0
        ,0,(L.Loan_ReturnedAmount - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))) as 'المبلغ المتبقي'

    ,Round( (L.Loan_ReturnedAmount / L.Loan_PaymentsAmount),2) as 'عدد الدفعات الكلي'

    ,(select count(Pay_Amount) from `payment` where `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like N'Paid') as 'عدد الدفعات المدفوعة'

    ,((IF(((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))<0 
        ,0,((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))) )
        / L.Loan_PaymentsAmount ) as 'عدد الدفعات المتبقية'

    ,(SELECT `Pay_RecievedOnDate` FROM `payment` WHERE `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like 'Paid' ORDER BY `payment`.`Pay_RecievedOnDate` DESC LIMIT 1) as 'تاريخ آخر دفعة مدفوعة'

    ,(SELECT `Pay_DueDate` FROM `payment` WHERE `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like 'Not Paid'  AND state.Name_ar like 'ممول' ORDER BY `payment`.`Pay_DueDate` DESC LIMIT 1) as 'تاريخ الدفعة المستحقة'

    ,IF( 
        IF((L.Loan_ReturnedAmount - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID)) < 0
        ,0,(L.Loan_ReturnedAmount - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))) = 0 
        ,0,
        (SELECT TIMESTAMPDIFF(MONTH, payment.Pay_DueDate , DATE_ADD( Now(),INTERVAL 1 MONTH)) 
            from payment 
            where payment.Loan_ID = L.Loan_ID and payment.Pay_IsPaid = 'Not Paid' 
            and  state.ID != 7 
            ORDER by Pay_DueDate desc LIMIT 1 )) as 'أشهر التأخير'

    ,(select count(Pay_Amount) from `payment` where `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like N'Not Paid') as 'الدفعات المستحقة والغير مدفوعة'

    ,Loan_Note as 'ملاحظات'
    ,donorgroup.Name as 'المجموعة'
    ,L.Loan_ReceiptID as 'رقم الإيصال'";

                string condition = "";
                //= " where Current_State = 1 ";
                if (cond != "")
                    condition += cond;

                if (Loan_ID != "")
                    if (condition == "") condition += " where L.Loan_ID = " + Loan_ID;
                    else condition += " and L.Loan_ID = " + Loan_ID;

                //for group_concat//
                string groupBy = " Group By MP_ID ";

                query += from + condition + groupBy;

                Loan_from = from;
                Loan_condition = condition;

                Program.buildConnection();
                var sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);
                Program.MyConn.Close();
                return dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public DataTable Select_Loan_Sum(string id_loan_str, string donor)
        {
            var net_query = ""; string query = "";

            if (id_loan_str == "")
                net_query = @" ,IFNULL((select sum(`Pay_Amount`) from `payment` where Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0)";
            else
                net_query = @" ,IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount < 0 and `Pay_IsPaid` = N'Paid') ,0) + IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0)";

            if (donor != "") net_query += @"-(select SUM(lulu.Loan_Amount) from loan lulu join microproject m on lulu.MicroProject_ID = m.MP_ID left join donor on donor.ID = m.MP_Donor left join donorgroup on donorgroup.ID = m.DonorGroup_ID where donor.Name like '" + donor + "' and donorgroup.Name like '%Installment%') ";
            net_query += " as 'Remaining'";


            if (id_loan_str == "")
                query = @"select SUM(`Loan_Amount`) as 'Export'
 ,IFNULL((select sum(`Pay_Amount`) from `payment` where Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Import' " +
",IFNULL((select sum(`Pay_Amount`) from `payment` where Pay_Amount < 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Returned' " +
",IFNULL((select sum(`Pay_Amount`) from `payment` where Pay_Amount < 0 and `Pay_IsPaid` = N'Paid') ,0) + IFNULL((select sum(`Pay_Amount`) from `payment` where Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Net'" +
",IFNULL( SUM( ( Loan_Amount / (SELECT Rate from donorgroup where donorgroup.ID = microproject.DonorGroup_ID))) ,0) as 'Export($)'"
 + net_query
 + " from `loan` left join `microproject` on loan.MicroProject_ID = microproject.MP_ID ";
            else
                query = @"select SUM(`Loan_Amount`) as 'Export'
 ,IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Import' " +
",IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount < 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Returned' " +
",IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount < 0 and `Pay_IsPaid` = N'Paid') ,0) + IFNULL((select sum(`Pay_Amount`) from `payment` where payment.Loan_ID in " + id_loan_str + " and Pay_Amount >= 0 and `Pay_IsPaid` = N'Paid') ,0) as 'Net'" +
",IFNULL( SUM( ( Loan_Amount / (SELECT Rate from donorgroup where donorgroup.ID = microproject.DonorGroup_ID))) ,0) as 'Export($)'"
  + net_query
  + " from `loan` left join `microproject` on loan.MicroProject_ID = microproject.MP_ID where `Loan_ID` in " + id_loan_str + " ";

            //        ,SUM(IF(((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID)) < 0
            //,0,((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID)))) as 'Remaining'

            Program.buildConnection();
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public int Count_Distinct_Loan_IDs()
        {

            Program.buildConnection();
            //count rows
            var sel = "select count(DISTINCT MP_ID) " + Loan_from + Loan_condition;
            var sc = new MySqlCommand(sel, Program.MyConn);
            var count = Convert.ToInt32(sc.ExecuteScalar());
            Program.MyConn.Close();
            return count;
        }

        #endregion

        #region project

        //micro project bind//
        public DataTable MicroProject_bind(int Person_ID, int MicroProject_ID)
        {

            Program.buildConnection();

            var query = "select MP.MP_ID as 'MicroProject_ID'"
                + ", MP_Name as 'Project Name'" 
                 
                + ", MP_RequestedAmount as 'Requested Amount'"
                + ", MP_PeriodOfExecution as 'Requested Time'"
                + ", MP_DateOfRequest as 'Application Date'"
                + ", MP_Description as 'Description'"

                + ", MP_FundType_ID as 'FundType_ID'"
                + ", MP_Type_ID as 'Type_ID'"
                + ", MP_SubType_ID as 'SubType_ID'"

                + ", (select Name from microprojecttype where microprojecttype.ID = MP_Type_ID limit 1) as 'Type'"

                + ", Category_ID "
                + ", SubCategory_ID "
                + ", Career_ID "

                + ", (select Name from `category` where ID = MP.Category_ID) as 'Category'"
                + ", (select Name from `subcategory` where ID = MP.SubCategory_ID) as 'SubCategory'"
                + ", (select Name from `career` where ID = MP.Career_ID) as 'Career'"

                + ", MP_HasPreviousDonors as 'Has Previous Donors'"
                + ", MP_OtherDonors as 'Other Donors'"
                + ", MP_IsNeedLicense as 'needs a license'"
                + ", MP_LicenseSide as 'License Side'"
                + ", MP_HasCompetitors as 'Has Competitors'"
                + ", MP_Competitors as 'Competitors'"
                + ", MP_SuppliesInsurance as 'Supplies Insurance'"
                + ", MP_Suppliers as 'Suppliers'"
                + ", MP_Risk as 'Risk'"
                + ", MP_Protection as 'Protection'"
                + ", MP_OtherNotes as 'Other Notes'"
                + ", MP_Marketing as 'Marketing'"
                + ", MP_SimpleProfit as 'Simple Profit'"
                + ", MP_Profit_Description as 'Profit Comment'"
                + ", MP_IncomeKind as 'Income Kind'"
                + ", MP_IncomeKind_Note as 'Income Kind Note'"
                + ", MP_HasLedger as 'Has Ledger'"
                + ", MP_AmperNumber as 'Amper Number'"

                + ", MP_Donor as 'Donor_ID'"
                + ", (select donor.Name from donor where donor.ID = MP_Donor) as 'Donor'"

                + ", state.Name_ar as 'Project State' "
                + ", mps.Date as 'Project State Date' "

                + ", MP_Visited as 'Visited'"
                + ", MP_Message as 'Message'"
                + ", MP_MessageDate as 'Message Date'"

                + ", IsContentUpdated as 'IsContentUpdated'"

                + ", MP_PaperUser_ID as    'PaperUser_ID'"
                + ", MP_ProgramUser_ID as  'ProgramUser_ID'"
                + ", MP_EditedByUser_ID as 'EditedByUser_ID'"

                + ", (select UserName from user where user.UserID = MP_PaperUser_ID limit 1) as 'PaperUser'"
                + ", (select UserName from user where user.UserID = MP_ProgramUser_ID limit 1) as 'ProgramUser'"
                + ", (select UserName from user where user.UserID = MP_EditedByUser_ID limit 1) as 'EditedByUser'"

                + ", (SELECT IFNULL(COUNT(ID),0) FROM `microproject_checklist` WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like 'k') as 'KeyPerson'"

                + " from `microproject` MP"
                + " LEFT JOIN `microproject_state` mps on mps.MicroProject_ID = MP.MP_ID "
                + " LEFT OUTER JOIN `state` on state.ID = mps.State_ID "

                + " where Current_State = 1 and MP.MP_ID = " + MicroProject_ID
                + " Group By MP.MP_ID ";

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable MicroProject_PowerSource_bind(int MicroProject_ID)
        {
            Program.buildConnection();
            string query = " select microproject_powersource.ID as 'ID'" +
                         " ,powersource.Name as 'Name'" +
                         " ,microproject_powersource.Note as 'Note'" +
                         " ,microproject_powersource.Cost as 'Cost'" +
                         " ,microproject_powersource.Responsability as 'Responsability'" +
                         " ,microproject_powersource.PowerSource_ID as 'Power_ID'" +

                         " from `microproject_powersource` left join `powersource` on microproject_powersource.PowerSource_ID = powersource.ID" +
                         " where microproject_powersource.MicroProject_ID = " + MicroProject_ID + "";
            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        //micro project details bind// 
        public DataTable MicroProject_Priest_bind(int MicroProject_ID)
        {

            Program.buildConnection();
            MySS.query =
                "select Priest_Name as 'Name' from `priest` right outer join `microproject` on priest.Priest_ID = microproject.MP_Priest_ID "
                + " where microproject.MP_ID = " + MicroProject_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Budget_Item_bind(int MicroProject_ID)
        {

            Program.buildConnection();

            MySS.query = "select I.I_Name as 'Item Name'" +
                         ",I.I_Price as 'Item Price'" +
                         ",BI.Item_Amount as 'Amount'" +
                         "\n from `item` I right outer join `budget_item` BI on BI.Item_ID = I.I_ID " +
                         "left outer join `budget` B on BI.Budget_ID = B.B_ID" +
                         "\n where B.MicroProject_ID = " + MicroProject_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Materials_bind(int MicroProject_ID)
        {

            Program.buildConnection();

            MySS.query = "select `ID` as 'ID'" +
                         ",`Name` as 'Name'" +
                         ",`Amount` as 'Amount'" +
                         ",`Price` as 'Price'" +
                         ",`LocalContribution` as 'LocalContribution'" +
                         ",`Comments` as 'Comments'" +
                         ",`MicroProject_ID` as 'MicroProject_ID'" +
                         " FROM `material` " +
                         " WHERE MicroProject_ID = " + MicroProject_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Risks_bind(int MicroProject_ID)
        {

            Program.buildConnection();
            MySS.query = "select risk.Name as 'Name'" +
                         ",`Risk_ID` as 'Risk_ID'" +
                         ",`MicroProject_ID` as 'MicroProject_ID'" +
                         " FROM `microproject_risk` left join `risk` on microproject_risk.Risk_ID = risk.ID " +
                         " WHERE MicroProject_ID = " + MicroProject_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Score_bind(int MicroProject_ID)
        {

            Program.buildConnection();
            MySS.query = "select score.Name as 'Name'" +
                         ",`Score_ID` as 'Score_ID'" +
                         ",`value` as 'value'" +
                         ",`notes` as 'notes'" +
                         ",`MicroProject_ID` as 'MicroProject_ID'" +
                         " FROM `microproject_score` left join `score` on microproject_score.Score_ID = score.ID " +
                         " WHERE MicroProject_ID = " + MicroProject_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }


        public DataTable Application_Details_Procedure(string condition)
        {
            Program.buildConnection();
            MySqlCommand sc = new MySqlCommand("select_application_details", Program.MyConn);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@MyCondition", condition);

            using (MySqlDataAdapter sda = new MySqlDataAdapter(sc))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Program.MyConn.Close();
                return dt;
            }
        }
        #endregion

        #region beneficiary 
        //benefeciary bind//
        public DataTable Person_bind(int Person_ID)
        {
            Program.buildConnection();

            MySS.query = "select P_ID as 'ID'"
                         + ",P_FirstName as 'First Name'"
                         + ",P_LastName as 'Last Name'"
                         + ",P_FatherName as 'Father Name'"
                         + ",P_MotherName as 'Mother Name'"
                         + ",P_FullName_en as 'Full Name En'"
                         + ",P_DOB as 'Birth Date'"
                         + ",P_MilitaryService as 'Military Services'"
                         + ",P_MilitaryService_Date as 'Military Services Date'"
                         + ",P_MilitaryService_Note as 'Military Services Note'"
                         + ",P_Mobile as 'Mobile'"
                         + ",P_HomeTel as 'Land Line'"
                         + ",Street_ID as 'Street_ID'"
                         + ",P_HomeAddress as 'Home Address'"
                         + ",P_NationalNumber as 'National Number'"
                         + ",P_RegistrationPlace as 'Nationality'"
                         + ",P_Sex as 'Gender'"
                         + ",P_MaritalStatus_ID as 'Marital Status'"
                         + ",P_IsLivingWithFamily as 'Is Living With Family'"
                         + ",P_LiveWithFamily_Note as 'Living With Family Note'"
                         + ",P_NumAtHome as 'Family members at home'"
                         + ",IsProjectOwner as 'Project Owner'"
                         + ",IsProvider as 'Provider'"
                         + ",P_SourceOfIncome as 'Source Of Income'"
                         + ",P_MedicalCond as 'Medical Conditions'"
                         + ",P_MedicalHelp as 'Medical Help'"
                         + ",P_MedicalHelp_Note as 'Medical Help Note'"
                         + ",P_Loss as 'Loss'"
                         + ",P_IntermidiarySide as 'Intermidiary Side'"
                         + ",P_HomeState as 'Home State'"
                         + ",P_HomeState_Note as 'Home State Note'"
                         + ",P_OtherProperties as 'Other Properties'"
                         + ",P_OtherProperties_Note as 'Other Properties Note'"
                         + ",P_OtherIncomeSources as 'Other Income Sources'"
                         + ",P_OtherIncomeSources_Note as 'Other Income Sources Note'"
                         + ",P_Courses_Note as 'Other Courses'"
                         + ",P_MaristesCourse as 'Maristes Course'"
                         + ",P_OtherCourses as 'Other Courses'"
                         + ",P_PowerSource_Note as 'PowerSource_Note'"

                         + ",P_Parish_ID as 'Parish_ID'"
                         + ",parish.Name_ar as 'Parish'"

                         + ",P_Priest_ID as 'Priest_ID'"
                         + ",Priest_Name as 'Priest'"

                         + ",P_State_ID as 'State_ID'"
                         + ",personstate.Name as 'State'"

                         //+ ",P_ProfilePath as 'Picture Path'"
                         //+ ",image.image_NewPath as 'Picture Path'"
                         //+ ",image.Image_ID as 'Image_ID'"

                         + @",(select image.image_NewPath 
    from `image` left join `imagetype` on image.ImageType_ID = imagetype.ID 
    where image.Person_ID = person.P_ID 
    and imagetype.Name_en like 'Profile' limit 1) as 'Picture Path' "

                         + @",(select image.Image_ID 
    from `image` left join `imagetype` on image.ImageType_ID = imagetype.ID 
    where image.Person_ID = person.P_ID 
    and imagetype.Name_en like 'Profile' limit 1) as 'Image_ID' "

                         + ",person_microproject.PersonType as 'PersonType' "

                         + " From `person` "
                         + "  left outer join `priest` on priest.Priest_ID = person.P_Priest_ID "
                         + "  left outer join `parish` on parish.ID = person.P_Parish_ID "
                         + "  left outer join `person_microproject` on person_microproject.Person_ID = person.P_ID "
                         + "  left outer join `personstate` on personstate.ID = person.P_State_ID "

                         + " Where P_ID= " + Person_ID + "  ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        ////public MySqlDataReader Person_Image(int Person_ID)
        ////{ 
        ////    Program.buildConnection(); 
        ////    MySqlDataReader reader;
        ////    MySS.query = "select P_Picture, P_PicturePath from `person` where P_ID = " + Person_ID + " ";
        ////    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        ////    reader = MySS.sc.ExecuteReader(); 
        ////    return reader;
        ////}

        //public string Person_Image(int Person_ID)
        //{
        //    Program.buildConnection();  
        //    MySS.query = "select `P_ProfilePath` from `person` where `P_ID` = " + Person_ID + " ";
        //    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        //    string result = MySS.sc.ExecuteScalar().ToString();
        //    Program.MyConn.Close(); 
        //    return result;
        //}

        //benefeciary details bind//
        public DataTable Person_Education_bind(int Person_ID)
        {

            Program.buildConnection();

            MySS.query = "select PEdu_ID as 'ID'" +
                         ",E.E_Type as 'E_Type'" +
                         ",E.E_Name as 'Name'" +
                         ",Education_ID " +


                         "\n from `person_education` PE left outer join `education` E on PE.Education_ID = E.E_ID" +
                         "\n where PE.Person_ID=" + Person_ID + "";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);

            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_Experience_bind(int Person_ID, int MicroProject_ID)
        {

            Program.buildConnection();

            MySS.query = "select PW_ID as 'ID'" +
                         ",W.W_Name as 'Name'" +
                         ",PWE.W_Description as 'Description'" +
                         ",PWE.W_BeginDate as 'Begin Date'" +
                         ",PWE.W_EndDate as 'End Date'" +
                         ",PWE.W_Place as 'Place'" +
                         ",PWE.W_PlaceState as 'Place State'" +
                         ",PWE.W_Status as 'Status'" +
                         ",PWE.W_CauseOfLose as 'Reason Of Lose'" +
                         ",PWE.W_CauseOfLoseDescription as 'Description Reason Of Lose'" +
                         ",PWE.W_Salary as 'Salary'" +
                         ",Work_ID " +

                         " from `person_workexp` PWE left outer join `work` W on PWE.Work_ID = W.W_ID " +
                         " where PWE.Person_ID=" + Person_ID + " and PWE.MicroProject_ID=" + MicroProject_ID +
                         " and PWE.W_Status='No'";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);

            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_Work_bind(int Person_ID, int MicroProject_ID)
        {

            Program.buildConnection();

            MySS.query = "select PW_ID as 'ID'" +
                         ",W.W_Name as 'Name'" +
                         ",PWE.W_Description as 'Description'" +
                         ",PWE.W_BeginDate as 'Begin Date'" +
                         ",PWE.W_EndDate as 'End Date'" +
                         ",PWE.W_Place as 'Place'" +
                         ",PWE.W_PlaceState as 'Place State'" +
                         ",PWE.W_Status as 'Status'" +
                         ",PWE.W_CauseOfLose as 'Reason Of Lose'" +
                         ",PWE.W_CauseOfLoseDescription as 'Description Reason Of Lose'" +
                         ",PWE.W_Salary as 'Salary'" +
                         ",Work_ID " +
                         " from `person_workexp` PWE left outer join `work` W on PWE.Work_ID = W.W_ID " +
                         " where PWE.Person_ID=" + Person_ID + " and PWE.MicroProject_ID=" + MicroProject_ID +
                         " and PWE.W_Status='Yes' ";

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);

            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_AidSource_bind(int Person_ID, int MicroProject_ID)
        {

            Program.buildConnection();
            MySS.query = " select person_aidsource.`ID` " +
                         " ,aidsource.Name as 'Name'" +
                         " ,person_aidsource.DateYear " +
                         " ,person_aidsource.Notes " +
                         " ,person_aidsource.`AidSource_ID`" +
                         " ,person_aidsource.`MicroProject_ID`" +
                         " ,person_aidsource.`Person_ID` "

                         + " from `person_aidsource` left join `aidsource` on person_aidsource.AidSource_ID = aidsource.ID"
                         + " where person_aidsource.Person_ID=" + Person_ID
                         + " and person_aidsource.MicroProject_ID=" + MicroProject_ID;

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_PowerSource_bind(int Person_ID)
        {
            Program.buildConnection();
            MySS.query = " select person_powersource.PowerSource_ID as 'ID' " +
                        " ,powersource.Name as 'Name' " +
                        " from `person_powersource` " +
                        " left join `powersource` on person_powersource.PowerSource_ID = powersource.ID " +
                        " where person_powersource.Person_ID = " + Person_ID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);
            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_Courses_bind(int Person_ID)
        {

            Program.buildConnection();

            MySS.query = "select PC.Course_ID as 'ID' " +
                         " ,C.Name as 'Name'" +
                         " from `person_course` PC left outer join `course` C on PC.Course_ID = C.ID " +
                         " where PC.Person_ID = " + Person_ID;

            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            MySS.da = new MySqlDataAdapter(MySS.sc);
            MySS.dt = new DataTable();
            MySS.da.Fill(MySS.dt);

            Program.MyConn.Close();
            return MySS.dt;
        }

        public DataTable Person_Loss_bind(int Person_ID)
        {

            Program.buildConnection();

            var query = "select PL.Loss_ID as 'ID' " +
                        " ,L.Name as 'Name'" +
                        " from `person_loss` PL left outer join `loss` L on PL.Loss_ID = L.ID " +
                        " where PL.Person_ID = " + Person_ID;

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);

            Program.MyConn.Close();
            return dt;
        }

        //public DataTable Person_Language_bind(int Person_ID)
        //{
        //    
        //                   Program.buildConnection();

        //    MySS.query = "select PL_ID as 'ID'" +
        //       ",L.L_Name as 'Language'" +
        //       ",PL.L_Level as 'Level'" +
        //       "\n from `person_language` PL left outer join `language` L on PL.Language_ID = L.L_ID " +
        //       "\n where PL.Person_ID=" + Person_ID + "";

        //    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        //    MySS.sc.ExecuteNonQuery();
        //    MySS.da = new MySqlDataAdapter(MySS.sc);
        //    MySS.dt = new DataTable();
        //    MySS.da.Fill(MySS.dt);

        //    Program.MyConn.Close();
        //    return MySS.dt;
        //}
        //public DataTable Person_Skill_bind(int Person_ID)
        //{
        //    
        //                   Program.buildConnection();

        //    string strCmd = "select PS_ID as 'ID'" +
        //       ",S.S_Name as 'Skill'" +
        //       ",PS.S_Describtion as 'Description'" +
        //       "\n from `person_skill` PS left outer join `skill` S on PS.Skill_ID = S.S_ID " +
        //       "\n where PS.Person_ID=" + Person_ID + "";

        //    MySS.sc = new MySqlCommand(strCmd, Program.MyConn);
        //    MySS.sc.ExecuteNonQuery();
        //    MySS.da = new MySqlDataAdapter(MySS.sc);
        //    MySS.dt = new DataTable();
        //    MySS.da.Fill(MySS.dt);

        //    Program.MyConn.Close();
        //    return MySS.dt;
        //}

        #endregion


        public DataTable Category_Select()
        {

            Program.buildConnection();
            string query = "select ID,Name from `category` ORDER BY Name ASC ";
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