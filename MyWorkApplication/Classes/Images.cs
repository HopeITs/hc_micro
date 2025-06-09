using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyWorkApplication.Classes
{
    internal class Images
    {
        string query = "";
        public Images()
        {
            query = "";
        }

        public void InsertImage(int MicroProject_ID, string Image_NewPath, int ImageType_ID)
        {
            Program.buildConnection();
            query = @"Insert Into `image`(`Image_NewPath`, `ImageType_ID`, `MicroProject_ID`) values( 
 @Image_NewPath , 
 @ImageType_ID ,   
 @MicroProject_ID ); ";

            //insert log
            query += @"INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (
 N'insert image: @Image_NewPath to @MicroProject_ID'
,N'image' ,N'" + Program.UserName
      + "',N'" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
               + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second +
"',0)"; //not seen

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Image_NewPath", Image_NewPath);
                sc.Parameters.AddWithValue("@ImageType_ID", ImageType_ID);
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void InsertProfile(int MicroProject_ID, string Image_NewPath, int Person_ID, string P_Name)
        {
            Program.buildConnection();
            query = @"Insert Into `image`(`Image_NewPath`, `ImageType_ID`, `MicroProject_ID` , `Person_ID`) values( 
 @Image_NewPath , 
 (select ID from imagetype where imagetype.Name_en like 'Profile' limit 1) ,   
 @MicroProject_ID,
 @Person_ID); ";

            //insert log
            query += @"INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (
 N'insert image: @Image_NewPath to @MicroProject_ID : @P_Name '
,N'image' ,N'" + Program.UserName
      + "',N'" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
               + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second +
"',0)"; //not seen

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Image_NewPath", Image_NewPath);
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@Person_ID", Person_ID);
                sc.Parameters.AddWithValue("@P_Name", P_Name);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public void DeleteImage(int Image_ID, int MicroProject_ID, string Image_NewPath)
        {
            Program.buildConnection();
            query = "delete from `image` where `Image_ID` = @Image_ID ; ";
            //insert log
            query += @"INSERT INTO `log`(`Log_Type`, `Log_OnTable`, `Log_User`, `Log_Date`, `Log_Seen`) VALUES (
 N'Delete Image: @Image_NewPath of: @MicroProject_ID '
,N'image' ,N'" + Program.UserName
      + "',N'" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " "
               + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second +
"',0)"; //not seen

            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Image_ID", Image_ID);
                sc.Parameters.AddWithValue("@MicroProject_ID", MicroProject_ID);
                sc.Parameters.AddWithValue("@ImaImage_NewPathge_ID", Image_NewPath);
                sc.ExecuteNonQuery();
                Program.MyConn.Close();
            }
        }

        public string Select_ImageName(int Image_ID)
        {
            Program.buildConnection();
            string query = "select Image_NewPath from `image` where Image_ID = @Image_ID ;";
            string result = "";
            using (var sc = new MySqlCommand(query, Program.MyConn))
            {
                sc.Parameters.AddWithValue("@Image_ID", Image_ID);
                result = sc.ExecuteScalar().ToString();
                Program.MyConn.Close();
            }
            return result;
        }

        public DataTable Select_Image(int Image_ID, int Person_ID, string imageType)
        {
            Program.buildConnection();
            string query = "select * from `image` ";
            string condition = " where 1 ";
            if (Image_ID != -1) condition += " and Image_ID = @Image_ID ";
            if (Person_ID != -1) condition += " and Person_ID = @Person_ID ";
            if (imageType != "") condition += " and ImageType_ID in (select imagetype.ID from imagetype where imagetype.Name_en like @imageType) ";

            query += condition;
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.Parameters.AddWithValue("@Image_ID", Image_ID);
            sc.Parameters.AddWithValue("@Person_ID", Person_ID);
            sc.Parameters.AddWithValue("@imageType", imageType);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }

        public DataSet SelectImageTypes()
        {
            Program.buildConnection();
            query = "";
            switch (Program.role)
            {
                case 1: //admin 
                case 8: //Admin_L0 
                    {
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'ثبوتيات شخصية - Personal';";
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'أوراق قانونية - Legal';";
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'أوراق مالية - Financial';";
                        break;
                    }
                case 3: //Financial
                    {
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'أوراق مالية - Financial';";
                        break;
                    }
                case 7: //Lawful
                    {
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'أوراق قانونية - Legal';";
                        break;
                    }
                case 2: //Data 
                    {
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'ثبوتيات شخصية - Personal';";
                        query += " SELECT `ID`, `Name`, `Name_en`, `Category` FROM `imagetype` where Category like 'أوراق قانونية - Legal';";
                        break;
                    }
            }

            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            var da = new MySqlDataAdapter(sc);
            var ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();
            return ds;
        }

        public DataSet Select_MP_Images(int MP_ID, string type_ids)
        {
            Program.buildConnection();
            var query = "SELECT Image_ID as 'ID' " +
                ", Image_NewPath as 'Image Path' " +
                ", ImageType_ID " +
                ", imagetype.Name as 'Type' " +
                " FROM `image`" +
                " Left join imagetype on image.ImageType_ID = imagetype.ID ";

            var condition = " Where MicroProject_ID = " + MP_ID + " ";
            if (type_ids != "") condition += " and ImageType_ID in (" + type_ids + ")  ";

            query += condition + ";";

            query += @"SELECT imagetype.Name as 'Type', IFNULL(COUNT(ImageType_ID),0) as 'Count'
    FROM imagetype
    Left OUTER join `image` on image.ImageType_ID = imagetype.ID 
                            and image.MicroProject_ID = " + MP_ID
  + " GROUP BY imagetype.Name; ";

            query += @"SELECT IFNULL(COUNT(image.Image_ID),0) as 'AllCount'
    FROM `image` Where MicroProject_ID = " + MP_ID + " ;";

            MySqlCommand sc = new MySqlCommand(query, Program.MyConn);
            MySqlDataAdapter da = new MySqlDataAdapter(sc);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Program.MyConn.Close();

            return ds;
        }
    }
}
