using System.Data;
using MySql.Data.MySqlClient;

namespace MyWorkApplication.Classes
{
    public class DonorGroup
    {
        private string query;

        public void Insert(string name, double rate, int Donor_ID)
        {
            
            Program.buildConnection();

            query = "INSERT INTO `donorgroup`(`Name`, `Rate`, `Donor_ID`) values (N'" + name + "'," + rate + "," +Donor_ID+ ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update(int ID, string name, double rate, int Donor_ID)
        {
            
            Program.buildConnection();
            query = "Update `donorgroup` set "
                    + " `Name` = N'" + name + "'"
                    + ",`Rate` = " + rate + " "
                    + ",`Donor_ID` = " + Donor_ID+ " "
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
            
            Program.buildConnection();
            query = "select `ID`, `Name`, Round(Rate,3) as 'Rate' " +
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