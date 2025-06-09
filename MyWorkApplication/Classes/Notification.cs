using MySql.Data.MySqlClient;
using System;

namespace MyWorkApplication.Classes
{
    internal class Notification
    {
        private MySqlComponents MySS;

        public Notification()
        {
            MySS = new MySqlComponents();
        }

        public void Insert_Notification(DateTime Date, int ExeMP_ID, int Payment_ID, string PName_Amount, int Type)
        {

            Program.buildConnection();

            var query =
                "Insert Into `notification`(`N_Date`, `N_ExeMP_ID`, `N_PaymentID`, `N_PName_Amount`, `N_Seen`, `N_Type`) values(N'"
                + Date.Year + "/" + Date.Month + "/" + Date.Day + "',"
                + ExeMP_ID + ","
                + Payment_ID + ",N'"
                + PName_Amount + "',"
                + "0" + ","
                + Type + ")";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_Notification(int N_ID)
        {

            Program.buildConnection();

            var query = "update `notification` set `N_Seen` = 1 where `N_ID` = " + N_ID + " ";
            //in "
            //+ "(select top 20 Log_ID from [dbo].[log] where Log_Seen = 0)";    //not seen
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_NotificationWithPaymentID(int Pay_ID)
        {

            Program.buildConnection();

            var query = "update `notification` set `N_Seen` = 1 where `N_PaymentID` = " + Pay_ID + " ";

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Update_NotificationWithExeFileID(int ExeF_ID)
        {

            Program.buildConnection();

            var query = "update `notification` set `N_Seen` = 1 where `N_PaymentID` = -5 and `N_ExeMP_ID` = " +
                        ExeF_ID + " ";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }
    }
}