using System;
using System.Net;

namespace MyWorkApplication.Classes
{
    public class FtpConnector
    {
        public static void WriteFile(string FullName, string LocalName, Tuple<string, string> Credentials)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            ///Write
            client.UploadFile(FullName, LocalName);
        }

        public static void UploadFileToLocal(string FullName, string RemoteName, Tuple<string, string> Credentials)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            client.DownloadFile(FullName, RemoteName);
        }

        public static void DowloadToLocal(string FullName, string OfflinePath, Tuple<string, string> Credentials)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            client.DownloadFile(FullName, OfflinePath);
        }
        public static void DeleteFile(string FullName, Tuple<string, string> Credentials)
        {
            ///DELETE
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FullName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            //Set mode
            request.UsePassive = true;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }
        public static Boolean ExistFile(string FullName, Tuple<string, string> Credentials)
        {
            //select image type and read it always "lower"(jpg, png)
            ///////////////////////////////////////////////////////////////////////////
            //string NewName = "";
            //string NewFullName = "";
            //int LastSlash = FullName.LastIndexOf("\\");
            //if (LastSlash == -1) { LastSlash = FullName.LastIndexOf("/"); }
            //NewName = FullName.Substring(LastSlash + 1, FullName.Length - LastSlash - 1);
            //int LastDot = NewName.LastIndexOf(".");
            //string ImageType = NewName.Substring(LastDot + 1, NewName.Length - LastDot - 1);
            //ImageType = ImageType.ToLower();
            //NewFullName = FullName.Substring(0, FullName.Length - 3) + ImageType;
            ///////////////////////////////////////////////////////////////////////////

            //var request = (FtpWebRequest)WebRequest.Create(NewFullName);
            var request = (FtpWebRequest)WebRequest.Create(FullName);
            request.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            request.UseBinary = true;
            request.KeepAlive = false;
            request.UsePassive = false;
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
        }
    }
}