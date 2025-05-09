using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class Education
    {
        private string query;

        public void Insert(string Name, string Type)
        {
            
            Program.buildConnection();
            query = "INSERT INTO `education` (`E_Name`, `E_Type`) VALUES (N'"
                         + Name + "',N'" + Type + "' )";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string Name, string Type)
        {
            
            Program.buildConnection();
            query = "Update `education` set "
                         + "`E_Name` = N'" + Name + "',"
                         + "`E_Type` = N'" + Type + "'"
                         + "where `E_ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete(int ID)
        {
            
            Program.buildConnection();
            query = "delete From `education` where `E_ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string Type, string Name)
        {
            
            Program.buildConnection();
            query = "select `E_ID` as 'ID' " +
                ",`E_Type` as 'Type'" +
                ",`E_Name` as 'Name' from `education`";
            string condition = " where 1 ";
            if (Name != "") condition += " and E_Name like '" + Name + "' ";
            else if (Type != "") condition += " and E_Type like '" + Type + "' "; 
            query += condition + " order by Name asc ";
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
