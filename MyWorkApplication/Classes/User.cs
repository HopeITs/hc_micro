using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace MyWorkApplication.Classes
{
    internal class User
    {
        private string query;
        private MySqlCommand sc;

        public User()
        {
            query = "";
            sc = new MySqlCommand();
        }

        public int Insert_User(string Username, string Password, string Email, int UserRoleID
            , int IsVisitor, int IsME, int IsCommunication, int IsCashier
            , byte[] ProfilePicture)
        {
            query =
                @"INSERT  INTO `user`( `UserName`, `Password`, `Email`, `UserRoleID`, `IsVisitor`, `IsME`, `IsCommunication`,`IsCashier`, `ProfilePicture`) VALUES (
 @Username ,@Password, @Email, @UserRoleID, @IsVisitor, @IsME, @IsCommunication, @IsCashier ,@PicArr )";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Username", Username);
                sc.Parameters.AddWithValue("@Password", Password);
                sc.Parameters.AddWithValue("@Email", Email);
                sc.Parameters.AddWithValue("@UserRoleID", UserRoleID);
                sc.Parameters.AddWithValue("@IsVisitor", IsVisitor);
                sc.Parameters.AddWithValue("@IsME", IsME);
                sc.Parameters.AddWithValue("@IsCommunication", IsCommunication);
                sc.Parameters.AddWithValue("@IsCashier", IsCashier);

                sc.Parameters.Add(new MySqlParameter("@PicArr", ProfilePicture));
                sc.ExecuteNonQuery();

                // ✅ Get inserted ID 
                int ID = -1; 
                int.TryParse(sc.LastInsertedId.ToString(), out ID);

                Program.MyConn.Close(); 
                return ID;
            }
        }

        public void Update_User(int ID
            , string Username, string Password, string Email, int UserRoleID
            , int IsVisitor, int IsME, int IsCommunication, int IsCashier
            , byte[] ProfilePicture)
        {
            query = @" UPDATE `user` SET 
  `UserName` = @Username 
 ,`Password` = @Password 
 ,`Email` = @Email 
 ,`UserRoleID` =@UserRoleID 
 ,`IsVisitor` = @IsVisitor  
 ,`IsME` = @IsME 
 ,`IsCommunication` = @IsCommunication 
 ,`IsCashier` = @IsCashier 
 , ProfilePicture = @PicArr 
 WHERE UserID = @ID ";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);

                sc.Parameters.AddWithValue("@Username", Username);
                sc.Parameters.AddWithValue("@Password", Password);
                sc.Parameters.AddWithValue("@Email", Email);
                sc.Parameters.AddWithValue("@UserRoleID", UserRoleID);
                sc.Parameters.AddWithValue("@IsVisitor", IsVisitor);
                sc.Parameters.AddWithValue("@IsME", IsME);
                sc.Parameters.AddWithValue("@IsCommunication", IsCommunication);
                sc.Parameters.AddWithValue("@IsCashier", IsCashier);

                sc.Parameters.Add(new MySqlParameter("@PicArr", ProfilePicture));
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Delete_User(int ID)
        {
            query = "delete from `user` where `UserID` = @ID ";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@ID", ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public DataTable Select_Users(string Name, string RoleName, string special_condition)
        {
            Program.buildConnection();
            query = "select `UserID` as 'ID'" +
                    ", `UserName` as 'Username'" +
                    ", `Email`" +
                    ",  role.Role_Name as 'Role'" +
                    ", `ProfilePicture` as '#'" +
                    ", `IsVisitor`" +
                    ", `IsME`" +
                    ", `IsCommunication`" +
                    ", `IsCashier`" +
                    ", `Password`" +
                    ", `UserRoleID` as 'Role_ID'" +

                    " from `user` left join `role` on user.UserRoleID = role.Role_ID ";
            var condition = " where 1 "; 
            var orderBy = " order by UserName asc ";

            if (Name != "")
                condition += " and `UserName` like '" + Name + "'";
            if (RoleName != "")
                condition += " and role.Role_Name like '" + RoleName + "' ";
            if (special_condition != "")
                condition += special_condition;

            query += condition + orderBy;
            sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public byte[] Select_ProfilePicture(int ID)
        {
            byte[] arr = null;

            Program.buildConnection();
            MySqlDataReader reader;
            query = "select ProfilePicture from `user` where UserID = @ID  ";
            sc = new MySqlCommand(query, Program.MyConn);
            sc.Parameters.AddWithValue("@ID", ID);

            reader = sc.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                arr = (byte[])reader[0];
                reader.Close();
                Program.MyConn.Close();
            }

            return arr;
        }

        public List<string> Select_User_Tasks(int User_ID)
        {

            Program.buildConnection();
            query = @"select T_Name
 from `task` left join task_user on task.T_ID = task_user.Task_ID 
 where T_State = 1 and `User_ID` = @User_ID";
            sc = new MySqlCommand(query, Program.MyConn);
            sc.Parameters.AddWithValue("@User_ID", User_ID);

            using (var reader = sc.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetString(0));

                Program.MyConn.Close();
                return list;
            }
        }

        public List<string> Select_Tasks()
        {

            Program.buildConnection();
            query = @"select T_Name from `task` where T_State =1";
            sc = new MySqlCommand(query, Program.MyConn);
            using (var reader = sc.ExecuteReader())
            {
                var list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetString(0));

                Program.MyConn.Close();
                return list;
            }
        }

        public DataTable Select_Roles()
        {

            Program.buildConnection();
            query = @"select Role_ID,Role_Name from `role` ORDER BY Role_ID ASC ";

            sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public void Insert_Task(int User_ID, string Task)
        {
            query = @"INSERT INTO `task_user`(`Task_ID` , `User_ID`) VALUES ("
                    + "(select T_ID from `task` where T_Name like N'" + Task + "')"
                    + "," + User_ID
                    + ")";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Remove_Task(int User_ID, string Task)
        {
            query = @"DELETE from `task_user` 
where `User_ID` =  @User_ID ";

            if (Task != "all")
                query += " and `Task_ID` = (select T_ID from `task` where T_Name like @Task )";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                if (Task != "all")
                    sc.Parameters.AddWithValue("@Task", Task);
                sc.Parameters.AddWithValue("@User_ID", User_ID);

                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void Send_New_Idea(string Subject, string Message)
        {
            MailMessage msgMail = new MailMessage();
            msgMail.From = new MailAddress("feedback.userapp@hcsyria.org");
            msgMail.To.Add("judy.dadaghlian@hcsyria.org");

            msgMail.Subject = Subject;
            msgMail.Body = Message;

            SmtpClient mailClient = new SmtpClient("mail.s807.sureserver.com", 587);
            mailClient.EnableSsl = true;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = new NetworkCredential("feedback.userapp@hcsyria.org", "JS2021program");
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            mailClient.Send(msgMail);
            msgMail.Dispose();
        }

        public void Send_New_NoteEmail(string Subject, string Message, List<string> emails)
        {
            MailMessage msgMail = new MailMessage();
            msgMail.From = new MailAddress("micro.app@hcsyria.org");

            for (int i = 0; i < emails.Count; i++)
            {
                string email = emails[i];
                msgMail.To.Add(email);
            }

            msgMail.Subject = Subject;
            msgMail.Body = Message;

            SmtpClient mailClient = new SmtpClient("mail.s807.sureserver.com", 587);
            mailClient.EnableSsl = true;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = new NetworkCredential("micro.app@hcsyria.org", "HopeCenterMicro");
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            mailClient.Send(msgMail);
            msgMail.Dispose();
        }


        public DataTable Get_Version()
        {
            try
            {
                Program.buildConnection();
                query = @"select l_version from `version` ORDER BY id ASC ";
                sc = new MySqlCommand(query, Program.MyConn);
                sc.ExecuteNonQuery();
                var da = new MySqlDataAdapter(sc);
                var dt = new DataTable();
                da.Fill(dt);
                Program.MyConn.Close();
                return dt;
            }
            catch (Exception ex)
            { return null; }
        }

        public void Update_Version(string version)
        {
            query = "UPDATE `version` SET " +
                    " `l_version` = @version ";

            Program.buildConnection();
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@version", version);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public string Get_FTP_Path(string center)
        {

            Program.buildConnection();
            query = @"select `ftp_path` from `databases` where `center` like @center ";
            sc = new MySqlCommand(query, Program.MyConn);
            sc.Parameters.AddWithValue("@center", center);

            string path = sc.ExecuteScalar().ToString();
            Program.MyConn.Close();

            return path;
        }

    }
}