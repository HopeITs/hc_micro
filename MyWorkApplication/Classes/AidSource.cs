using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class AidSource
    {
        private string query;

        public void Insert(string table, string name, string name_en)
        {

            Program.buildConnection();

            query = "INSERT INTO `"+ table + "`(`Name`, `Name_en`) values (N'" + name + "',N'" + name_en + "')";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(string table, int ID, string name, string name_en)
        {

            Program.buildConnection();
            query = "Update `"+ table + "` set "
                    + " `Name` = N'" + name + "' "
                    + ",`Name_en` = N'" + name_en + "' "
                    + " where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete(string table, int ID)
        {

            Program.buildConnection();
            query = "delete From `"+ table + "` where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string table, string Name)
        {
            Program.buildConnection();
            query = "select `ID`, `Name`, `Name_en` from `"+ table + "`";
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
