using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class Donor 
    {
        private string query;

        public void Insert(string name, string name_abb)
        {
            
            Program.buildConnection();

            query = "INSERT INTO `donor`(`Name`, `Name_abb`) values (N'" + name + "',N'" + name_abb + "')";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string name, string name_abb)
        {
            
            Program.buildConnection();
            query = "Update `donor` set "
                    + " `Name` = N'" + name + "' "
                    + ",`Name_abb` = N'" + name_abb + "' "
                    + " where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete(int ID)
        {
            
            Program.buildConnection();
            query = "delete From `donor` where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string Name)
        {
            
            Program.buildConnection();
            query = "select `ID`, `Name`, `Name_abb` from `donor` ";
            var condition = "";
            if (Name != "")
                condition += " where lower(`Name`) like '%" + Name.ToLower() + "%'";

            query += condition;
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
