using DocumentFormat.OpenXml.Office.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class Purchasing
    { 
        public void Insert(int microProjectId, string itemName, string brand,
                                 int? itemCategoryId, int quantity, decimal price,
                                 string currency, DateTime? invoiceDate,
                                 string invoiceNumber, string traderName)
        {
            Program.buildConnection();
            string query = @"INSERT INTO purchasing
                        (MicroProject_ID, ItemName, Brand, ItemCategory_ID,
                         Quantity, Price, Currency, InvoiceDate,
                         InvoiceNumber, TraderName)
                        VALUES (@MicroProject_ID, @ItemName, @Brand, @ItemCategory_ID,
                                @Quantity, @Price, @Currency, @InvoiceDate,
                                @InvoiceNumber, @TraderName);";
            //long lastID = -1;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@MicroProject_ID", microProjectId);
                sc.Parameters.AddWithValue("@ItemName", itemName);
                sc.Parameters.AddWithValue("@Brand", brand);
                sc.Parameters.AddWithValue("@ItemCategory_ID", (object)itemCategoryId ?? DBNull.Value);
                sc.Parameters.AddWithValue("@Quantity", quantity);
                sc.Parameters.AddWithValue("@Price", price);
                sc.Parameters.AddWithValue("@Currency", currency);
                sc.Parameters.AddWithValue("@InvoiceDate", (object)invoiceDate ?? DBNull.Value);
                sc.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                sc.Parameters.AddWithValue("@TraderName", traderName);
                 
                sc.ExecuteNonQuery();
                //lastID = sc.LastInsertedId;

                Program.MyConn.Close();
            }
            //return int.Parse(lastID.ToString());
        }

        public void Update(int id, int microProjectId, string itemName, string brand,
                                 int? itemCategoryId, int quantity, decimal price,
                                 string currency, DateTime? invoiceDate,
                                 string invoiceNumber, string traderName)
        {
            Program.buildConnection();
            string query = @"UPDATE purchasing SET
                         MicroProject_ID = @MicroProject_ID,
                         ItemName = @ItemName,
                         Brand = @Brand,
                         ItemCategory_ID = @ItemCategory_ID,
                         Quantity = @Quantity,
                         Price = @Price,
                         Currency = @Currency,
                         InvoiceDate = @InvoiceDate,
                         InvoiceNumber = @InvoiceNumber,
                         TraderName = @TraderName
                         WHERE ID = @ID;";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", id);
                sc.Parameters.AddWithValue("@MicroProject_ID", microProjectId);
                sc.Parameters.AddWithValue("@ItemName", itemName);
                sc.Parameters.AddWithValue("@Brand", brand);
                sc.Parameters.AddWithValue("@ItemCategory_ID", (object)itemCategoryId ?? DBNull.Value);
                sc.Parameters.AddWithValue("@Quantity", quantity);
                sc.Parameters.AddWithValue("@Price", price);
                sc.Parameters.AddWithValue("@Currency", currency);
                sc.Parameters.AddWithValue("@InvoiceDate", (object)invoiceDate ?? DBNull.Value);
                sc.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                sc.Parameters.AddWithValue("@TraderName", traderName);
                 
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete(int ID)
        {
            Program.buildConnection();
            string query = "DELETE FROM purchasing WHERE ID = @ID;";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select_PurchasingItems(string MicroProject_ID,string name)
        {
            Program.buildConnection();
            string query = @"SELECT PMP.MicroProject_ID as 'MicroProject_ID'
, PMP.Person_ID as 'Person_ID'
, CONCAT(P_FirstName, ' ', P_LastName, ' ابن/ة ', P_FatherName) as 'Beneficiary Name'
, microproject.MP_Name as 'Project Name' 
, career.Name as 'Career'
, L.Loan_Amount as 'Loan Amount'
, L.Loan_DateTaken as 'Loan Date'  
, microproject.ApplicationState as 'نوع الاستمارة'
, purchasing.* ";
             
            string condition = " where 1 ";
            string str = "";
            string from = @" FROM person_microproject PMP 
 left join microproject on PMP.MicroProject_ID = microproject.MP_ID
 left join person P on P.P_ID = PMP.Person_ID
 left join loan L on L.MicroProject_ID = microproject.MP_ID  
 left join career on career.ID =  microproject.Career_ID
 left join microprojecttype on microprojecttype.ID = microproject.MP_Type_ID 

 left join purchasing on PMP.MicroProject_ID = purchasing.MicroProject_ID  
 left join purchasingcategory on purchasingcategory.ID = purchasing.ItemCategory_ID ";
             
            if (MicroProject_ID != "") condition += " and PMP.MicroProject_ID = " + MicroProject_ID;
            if (name != "") condition += " and CONCAT(TRIM(P_FirstName),' ', TRIM(P_LastName),' ابن/ة ',TRIM(P_FatherName)) LIKE '" + name + "'";

            query += from + condition;

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable Select_PurchasingCategories()
        {
            Program.buildConnection();
            string query = @"SELECT * FROM purchasingcategory;"; 
            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
    }
}
