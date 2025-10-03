using MySql.Data.MySqlClient;
using System.Data;

namespace MyWorkApplication.Classes
{
    public class DonorGroup
    {
        private string query;

        public void Insert(string name, int Donor_ID)
        {

            Program.buildConnection();

            query = "INSERT INTO `donorgroup`(`Name`, `Donor_ID`) values (N'" + name + "'," + Donor_ID + ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string name, int Donor_ID)
        {

            Program.buildConnection();
            query = "Update `donorgroup` set "
                    + " `Name` = N'" + name + "'" 
                    + ",`Donor_ID` = " + Donor_ID + " "
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
            query = "delete From `donorgroup` where `ID` =" + ID;
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select(string Name)
        {
            //, Round(Rate,3) as 'Rate' 
            Program.buildConnection();
            query = "select `ID`, `Name` " +
                " , (select donor.Name from donor where donor.ID = Donor_ID limit 1) as 'Donor'" +
                " , Donor_ID from `donorgroup` ";
            var condition = "";
            if (Name != "")
                condition += " where lower(`Name`) like '%" + Name + "%'";
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