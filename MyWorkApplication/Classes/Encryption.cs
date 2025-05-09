using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    static class Encryption
    {
        private static string password = "5tCzcEL2wS";
        public static string DecryptedData = "";
        private static byte[] entropy = Encoding.Unicode.GetBytes("YZbtvE5nW679sCUFYGxH");
        private static readonly string fileName = @"Connections.txt";
         
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);
        public static byte[] GenerateSalt()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider rgnCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rgnCryptoServiceProvider.GetBytes(data);
            }
            return data;
        }

        public static void FileEncrypt()
        {
            GCHandle gCHandle = GCHandle.Alloc(password, GCHandleType.Pinned);
            byte[] salt = GenerateSalt();
            byte[] passwords = Encoding.UTF8.GetBytes(password);
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;//aes 256 bit encryption c#
            AES.BlockSize = 128;//aes 128 bit encryption c#
            AES.Padding = PaddingMode.PKCS7;
            var key = new Rfc2898DeriveBytes(passwords, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = CipherMode.CFB;
            using (FileStream fsCrypt = new FileStream(fileName + ".aes", FileMode.Create))
            {
                fsCrypt.Write(salt, 0, salt.Length);
                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (FileStream fsIn = new FileStream(fileName, FileMode.Open))
                    {
                        byte[] buffer = new byte[1048576];
                        int read;
                        while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cs.Write(buffer, 0, read);
                        }
                    }
                    cs.Close();
                }
                fsCrypt.Close();
            }
            ZeroMemory(gCHandle.AddrOfPinnedObject(), password.Length * 2);
            gCHandle.Free();
        }

        public static void FileDecrypt()
        {
            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);

            string inputFileName = Directory.GetCurrentDirectory() + @"\" + fileName + ".aes"; ;

            string appdata_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\IncData";

            System.IO.Directory.CreateDirectory(appdata_path);

            FileInfo Copidfile;
            Copidfile = new FileInfo(appdata_path + @"\" + fileName + ".aes");
            if (Copidfile.Exists.Equals(true))
            {
                Copidfile.Delete();

            }

            System.IO.File.Copy( inputFileName
            , appdata_path + @"\" + fileName + ".aes"
            , true);
             
            byte[] passwords = Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];
            using (FileStream fsCrypt = new FileStream(appdata_path + @"\" + fileName + ".aes", FileMode.Open))
            {
                fsCrypt.Read(salt, 0, salt.Length);
                RijndaelManaged AES = new RijndaelManaged();
                AES.KeySize = 256;//aes 256 bit encryption c#
                AES.BlockSize = 128;//aes 128 bit encryption c#
                var key = new Rfc2898DeriveBytes(passwords, salt, 50000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CFB;
                using (CryptoStream cryptoStream = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    int read;
                    DecryptedData = "";
                    byte[] buffer = new byte[1048576];
                    while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string lines = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        DecryptedData = lines;
                    }
                }
            }
            ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();
        }

        public static void DecryptCredentials(string datalocation)
        {

            if (DecryptedData == "") FileDecrypt(); 

            int Name_start = DecryptedData.IndexOf(datalocation);
            int Connection_Start = DecryptedData.IndexOf(';', Name_start) + 1;
            int length = DecryptedData.IndexOf("\n", Connection_Start) - Connection_Start - 1;
            //***************************************//
            int RemotelyPassword_start = DecryptedData.IndexOf("RemotelyPassword");
            int RemotelyPassword_Connection_Start = DecryptedData.IndexOf(';', RemotelyPassword_start) + 1;
            int RemotelyPassword_length = DecryptedData.IndexOf("\n", RemotelyPassword_Connection_Start) - RemotelyPassword_Connection_Start - 1;
            //*******************************//
            int RemotelyUser_start = DecryptedData.IndexOf("RemotelyUser");
            int RemotelyUser_Connection_Start = DecryptedData.IndexOf(';', RemotelyUser_start) + 1;
            int RemotelyUser_length = DecryptedData.IndexOf("\n", RemotelyUser_Connection_Start) - RemotelyUser_Connection_Start - 1;
            //*******************************//
            int RemotelyName_start = DecryptedData.IndexOf("RemotelyName");
            int RemotelyName_Connection_Start = DecryptedData.IndexOf(';', RemotelyName_start) + 1;
            int RemotelyName_length = DecryptedData.IndexOf("\n", RemotelyName_Connection_Start) - RemotelyName_Connection_Start - 1;
            //*******************************//
            int OnlineserversName_start = DecryptedData.IndexOf("OnlineserversName");
            int OnlineserversName_Connection_Start = DecryptedData.IndexOf(';', OnlineserversName_start) + 1;
            int OnlineserversName_length = DecryptedData.IndexOf("\n", OnlineserversName_Connection_Start) - OnlineserversName_Connection_Start - 1;
            //*******************************//
            int OnlineserversPassword_start = DecryptedData.IndexOf("OnlineserversPassword");
            int OnlineserversPassword_Connection_Start = DecryptedData.IndexOf(';', OnlineserversPassword_start) + 1;
            int OnlineserversPassword_length = DecryptedData.IndexOf("\n", OnlineserversPassword_Connection_Start) - OnlineserversPassword_Connection_Start - 1;
            //*******************************//
            int OfflineserversName_start = DecryptedData.IndexOf("OfflineserversName");
            int OfflineserversName_Connection_Start = DecryptedData.IndexOf(';', OfflineserversName_start) + 1;
            int OfflineserversName_length = DecryptedData.IndexOf("\n", OfflineserversName_Connection_Start) - OfflineserversName_Connection_Start - 1;
            //*******************************//
            int OfflineserversPassword_start = DecryptedData.IndexOf("OfflineserversPassword");
            int OfflineserversPassword_Connection_Start = DecryptedData.IndexOf(';', OfflineserversPassword_start) + 1;
            int OfflineserversPassword_length = DecryptedData.IndexOf("\n", OfflineserversPassword_Connection_Start) - OfflineserversPassword_Connection_Start - 1;
            //*******************************//
            int RemoteRoot_start = DecryptedData.IndexOf("RemoteRoot");
            int RemoteRoot_Connection_Start = DecryptedData.IndexOf(';', RemoteRoot_start) + 1;
            int RemoteRoot_length = DecryptedData.IndexOf("\n", RemoteRoot_Connection_Start) - RemoteRoot_Connection_Start - 1;
            //*******************************//
            int OfflineRoot_start = DecryptedData.IndexOf("OfflineRoot");
            int OfflineRoot_Connection_Start = DecryptedData.IndexOf(';', OfflineRoot_start) + 1;
            int OfflineRoot_length = DecryptedData.IndexOf("\n", OfflineRoot_Connection_Start) - OfflineRoot_Connection_Start - 1;
            //*******************************//
            int OnlineRoot_start = DecryptedData.IndexOf("OnlineRoot");
            int OnlineRoot_Connection_Start = DecryptedData.IndexOf(';', OnlineRoot_start) + 1;
            int OnlineRoot_length = DecryptedData.IndexOf("\n", OnlineRoot_Connection_Start) - OnlineRoot_Connection_Start - 1;

            Program.ConnectionString = DecryptedData.Substring(Connection_Start, length);
            
            //Program.RemotelyCredentials = new Tuple<string, string, string>(DecryptedData.Substring(RemotelyName_Connection_Start, RemotelyName_length), DecryptedData.Substring(RemotelyUser_Connection_Start, RemotelyUser_length), DecryptedData.Substring(RemotelyPassword_Connection_Start, RemotelyPassword_length));
            //Program.OnlineserversCredentials = new Tuple<string, string>(DecryptedData.Substring(OnlineserversName_Connection_Start, OnlineserversName_length), DecryptedData.Substring(OnlineserversPassword_Connection_Start, OnlineserversPassword_length));
            //Program.OfflineserversCredentials = new Tuple<string, string>(DecryptedData.Substring(OfflineserversName_Connection_Start, OfflineserversName_length), DecryptedData.Substring(OfflineserversPassword_Connection_Start, OfflineserversPassword_length));
            //Program.OfflineRoot = DecryptedData.Substring(OfflineRoot_Connection_Start, OfflineRoot_length);
            //Program.RemoteRoot = DecryptedData.Substring(RemoteRoot_Connection_Start, RemoteRoot_length);
            //Program.OnlineRoot = DecryptedData.Substring(OnlineRoot_Connection_Start, OnlineRoot_length); 
        }

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

    }
}
