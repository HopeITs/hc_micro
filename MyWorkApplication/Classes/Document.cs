using MySql.Data.MySqlClient;
using MyWorkApplication.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    internal class Document
    {
        public Document()
        {

        }

        public void GetNameFromPath(string FullName, out string name, out string Type)
        {
            int LastSlash = FullName.LastIndexOf("\\");
            if (LastSlash == -1) { LastSlash = FullName.LastIndexOf("/"); }
            name = FullName.Substring(LastSlash + 1, FullName.Length - LastSlash - 1);
            int LastDot = name.LastIndexOf(".");
            Type = name.Substring(LastDot + 1, name.Length - LastDot - 1);
        }
          
        public bool SaveDocument(String FullName, string Name)
        {
            try
            {
                string OfflinePath = Program.OfflineRoot + $@"/{Name}";
                string OnlinePath = Program.OnlineRoot + $@"/{Name}";

                //Saving to offline Server
                FtpConnector.WriteFile(OfflinePath, FullName, Program.OfflineserversCredentials);
             
                //Saving to online Server 
                FtpConnector.WriteFile(OnlinePath, FullName, Program.OnlineserversCredentials);
                return true;
            }
            catch { return false; }

        }
            
        public bool Online_Exist(string Name)
        {
            try
            {
                string Path = Program.OnlineRoot + @"/" + Name;
                return FtpConnector.ExistFile(Path, Program.OnlineserversCredentials);
            }
            catch (Exception ex)
            {
                return false;
            }
            //   return FtpConnector.ExistFile(OnlinePath, Program.OnlineserversCredentials); 
        }
        public bool Offline_Exist(string Name)
        {
            try
            {
                string Path = Program.OfflineRoot + @"/" + Name;
                return FtpConnector.ExistFile(Path, Program.OfflineserversCredentials);
            }
            catch (Exception ex)
            {
                return false;
            }
            // return FtpConnector.ExistFile(OfflinePath, Program.OfflineserversCredentials); 
        }
         
        public void CopyToLocal(string SourcePath, string DestinationPath, bool imageExistsOnline)
        {
            if (imageExistsOnline)
                FtpConnector.DowloadToLocal(SourcePath, DestinationPath, Program.OnlineserversCredentials);
            else 
                FtpConnector.DowloadToLocal(SourcePath, DestinationPath, Program.OfflineserversCredentials);
        }

        public void DeleteOnline(string FullName)
        {
            FtpConnector.DeleteFile(FullName, Program.OnlineserversCredentials);
        }
        public void DeleteOffline(string FullName)
        { 
            FtpConnector.DeleteFile(FullName, Program.OfflineserversCredentials); 
        }

        // Method to get an image by its name
        public byte[] GetImageByName(string PicName)
        {
            if (PicName != null || PicName.Length > 0 && PicName != "")
            {
                string OfflinePicPath = Program.OfflineRoot + @"/" + PicName;
                string OnlinePicPath = Program.OnlineRoot + @"/" + PicName;


                WebClient request = new WebClient();
                if (Offline_Exist(PicName))
                { 
                    // If your FTP server requires credentials, set them here
                    request.Credentials = new NetworkCredential("Admin", "admin54321");
                    byte[] imageBytes = request.DownloadData(OfflinePicPath);
                    //using (MemoryStream ms = new MemoryStream(imageBytes))
                    //{
                    //    return Image.FromStream(ms);
                    //}
                    return imageBytes;
                }
                else if (Online_Exist(PicName))
                {
                    // If your FTP server requires credentials, set them here
                    request.Credentials = new NetworkCredential("judy", "DAda1994@");
                    byte[] imageBytes = request.DownloadData(OfflinePicPath);
                    //using (MemoryStream ms = new MemoryStream(imageBytes))
                    //{
                    //    return Image.FromStream(ms);
                    //}
                    return imageBytes;
                }
                else return null;
            }
            else return null;
        }
    }
}
