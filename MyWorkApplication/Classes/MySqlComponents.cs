using System.Data;
using MySql.Data.MySqlClient;

namespace MyWorkApplication.Classes
{
    internal class MySqlComponents
    {
        public MySqlDataAdapter da;
        public DataSet ds;
        public DataTable dt;
        public string query;
        public MySqlDataReader reader;
        public MySqlCommand sc;
    }
}