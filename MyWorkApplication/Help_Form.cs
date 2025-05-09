using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Help_Form : Form
    {
        public Help_Form()
        {
            InitializeComponent();
        }



        private void Help_Form_Load(object sender, EventArgs e)
        {
            try
            {
                Set_file();

                //byte[] buff = Properties.Resources.MP_Training_2021;
                //var assembly = Assembly.GetExecutingAssembly();
                //string resourceName = assembly.GetManifestResourceNames()
                //        .Single(str => str.EndsWith("MP Training 2021.pdf"));

                //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                //    using (StreamReader reader = new StreamReader(stream))
                //    {
                //        string result = reader.ReadToEnd();
                //        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

                //        webBrowser1.DocumentStream = ms;
                //        webBrowser1.Show();
                //    }

                ////// Create two different encodings.  
                ////Encoding ascii = Encoding.ASCII;
                ////Encoding unicode = Encoding.Unicode;
                ////// Convert unicode string into a byte array.  
                //////byte[] bytesInUni = unicode.GetBytes(authorName);
                ////// Convert unicode to ascii  
                ////byte[] bytesInAscii = Encoding.Convert(unicode, ascii, buff);
                //// Convert byte[] into a char[]  
                //char[] charsAscii = new char[ascii.GetCharCount(bytesInAscii, 0, bytesInAscii.Length)];
                //ascii.GetChars(bytesInAscii, 0, bytesInAscii.Length, charsAscii, 0);
                //// Convert char[] into a ascii string  
                //string asciiString = new string(charsAscii);


                //using (MemoryStream ms = new MemoryStream(buff))
                //{
                //    ms.Position = 0;
                //    using (StreamReader sr = new StreamReader(ms, Encoding.GetEncoding(srcEncodingStr), true))
                //    {
                //        Encoding enc = Encoding.GetEncoding(srcEncodingStr);

                //        if (enc.EncodingName.ToUpper() == "UTF-8")
                //        {
                //            enc = new UTF8Encoding(false);
                //        }
                //        if ((enc.EncodingName.ToUpper() == "UTF-16" || enc.EncodingName.ToUpper() == "UNICODE"))
                //        {
                //            enc = new UnicodeEncoding(false, false);
                //        }
                //        if (enc.EncodingName.ToUpper() == "UTF-32")
                //        {
                //            enc = new UTF32Encoding(false, false);
                //        }
                //        //using (StreamWriter sw = new StreamWriter(fs, enc))
                //        //{
                //        //    string filebody = sr.ReadToEnd();
                //        //    sw.Write(filebody, enc);
                //        //}

                //    }

                //}


                //webBrowser1.Navigate(@"D:\Micro Training\Damascus Training\MP Training 2021\MP Training 2021.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Set_file()
        {
            FileInfo file;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Micro Projects\Manual.pdf";

            //GET IMAGE IF NOT EXIST
            file = new FileInfo(path);
            if (file.Exists.Equals(false))
            {
                byte[] buff = Properties.Resources.MP_Training_2021_compressed;

                if (buff != null && buff.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(buff);

                    using (FileStream file2 = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
                        ms.CopyTo(file2);
                }
            }

            //SET IMAGE
            file = new FileInfo(path);
            if (file.Exists.Equals(true))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    webBrowser1.Navigate(path); 
                }

            }
            else
            { } 

            //DELETE IMAGE FILE
            file = new FileInfo(path);
            if (file.Exists.Equals(true) && !Properties.Settings.Default.RememberMe)
            {
                file.Delete();
            }
        }

    }
}
