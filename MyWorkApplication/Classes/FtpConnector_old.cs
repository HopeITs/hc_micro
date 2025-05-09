using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;

namespace MyWorkApplication.Classes
{
    public class FtpConnector
    {
        public FtpConnector()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;
        }

        private byte[] Convert_Picture(string img_path)
        {
            byte[] arr;
            arr = null;
            var fs = new FileStream(img_path, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            arr = br.ReadBytes((int) fs.Length);
            return arr;
        }

        public void Upload(string ftpFullPath, string imgFilePath)
        {
            var ftp = (FtpWebRequest) WebRequest.Create(ftpFullPath);
            ftp.Credentials = new NetworkCredential("judy", "DAda1994@");
            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            ftp.Proxy = new WebProxy();

            var fs = File.OpenRead(imgFilePath);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            var ftpStream = ftp.GetRequestStream();
            ftpStream.Write(buffer, 0, buffer.Length);
            ftpStream.Close();
        }

        public Image Download(string URL)
        {
            Image img;
            var request = (FtpWebRequest) WebRequest.Create(URL);
            request.Credentials = new NetworkCredential("judy", "DAda1994@");
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            var response = (FtpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();

            if (stream != null && stream != Stream.Null)
            {
                img = Image.FromStream(stream);
                response.Close();
                stream.Close();
                return img;
            }

            return img = null;
        }

        public void Delete(string URL)
        {
            var request = (FtpWebRequest) WebRequest.Create(URL);
            request.Credentials = new NetworkCredential("judy", "DAda1994@");
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            var result = string.Empty;
            var response = (FtpWebResponse) request.GetResponse();
            var size = response.ContentLength;
            var datastream = response.GetResponseStream();
            var sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
        }

        public void DownloadFileAsync(string URL)
        {
            Process.Start(URL, @"/secondary /username:judy /password:DAda1994@");
        }

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
            ///READ 
            client.DownloadFile(FullName, RemoteName);


        }
        public static void DowloadToLocal(string FullName, string Type, Tuple<string, string> Credentials)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            ///READ 
            client.DownloadFile(FullName, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DISC App\StudentPic\temp." + Type);
        }
        public static void DeleteFile(string FullName, Tuple<string, string> Credentials)
        {
            ///DELETE
            System.Net.FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FullName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                response.Close();
            }
        }
        public static Boolean ExistFile(string FullName, Tuple<string, string> Credentials)
        {
            var request = (FtpWebRequest)WebRequest.Create(FullName);
            request.Credentials = new NetworkCredential(Credentials.Item1, Credentials.Item2);
            request.UseBinary = true;
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