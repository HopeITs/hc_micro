using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class Education
    {
        private string query;

        public void Insert(string Name, string Type_ID)
        {

            Program.buildConnection();
            query = "INSERT INTO `education` (`Name`, `Type_ID`) VALUES " +
                "(N'"+ Name + "'," + Type_ID + ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string Name, string Type_ID)
        {

            Program.buildConnection();
            query = "Update `education` set "
                         + " `Name` = N'" + Name + "',"
                         + " `Type_ID` = " + Type_ID + " "
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
            query = "delete From `education` where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string Type_ID, string Name)
        {

            Program.buildConnection();
            query = @"select `ID` 
    ,(select Name from educationtype where ID = Type_ID ) as 'Type' 
    ,`Name` 
    ,`Type_ID` 
    from `education`";
            string condition = " where 1 ";
            if (Name != "") condition += " and Name like '" + Name + "' ";
            else if (Type_ID != "") condition += " and Type_ID = " + Type_ID + " ";
            query += condition + " order by Name asc ";
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataTable SelectEducationTypes()
        {
            Program.buildConnection();
            string query1 = "select ID,Name from `educationtype` order by ID Asc;";

            MySqlCommand sc = new MySqlCommand(query1, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
    }
}
