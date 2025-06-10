using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class Career
    {
        string query = "";

        public void Insert(string Name, int SubCategory_ID)
        {

            Program.buildConnection();
            query = "INSERT INTO `career` (`Name`, `SubCategory_ID`) VALUES ("
                        + "'" + Name + "'," + SubCategory_ID + ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string Name, int SubCategory_ID)
        {
            Program.buildConnection();
            query = "Update `career` set "
                         + " Name = N'" + Name + "',"
                         + " SubCategory_ID = " + SubCategory_ID + " "
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
            query = "delete From `career` where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string SubCategory_ID, string Name)
        {

            Program.buildConnection();
            query = "SELECT ID, Name, SubCategory_ID " +
                ",(select Name from subcategory where ID = career.SubCategory_ID) as 'SubCategory' " +
                " from `career`";

            string condition = " where 1 ";
            if (SubCategory_ID != "") condition += " and SubCategory_ID = " + SubCategory_ID;
            else if (Name != "") condition += " and Name like '" + Name + "' ";

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
