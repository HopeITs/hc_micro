using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MyWorkApplication.Classes;
using PresentationControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static ClosedXML.Excel.XLPredefinedFormat;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MyWorkApplication
{
    internal static class Program
    {
        //CHARSET=utf8 for recognizing Arabic in c# 
        //public static MySqlConnection MyConn; 
        //public static string ConnectionString = ""; 
        //public static MySqlConnection MyFirstConn; //ADDED BY SARY
        //public static List<string> User_In_Database; //ADDED BY SARY 

        //CHARSET=utf8 for recognizing Arabic in c#
        public static string ConnectionString = ""; //PC-ADMIN        
        public static MySqlConnection MyConn;
        public static MySqlConnection MyFirstConn;
        public static string Selected_Date = "";
        public static List<string> User_In_Database;
        public static string database = "";

        public static string theme = "Light";
        public static int role = -1;
        public static int userID = -1;
        public static string selected_database = "";
        public static string UserName = "";
        public static string Password = "";

        public static string OnlineRoot;
        public static string OfflineRoot;

        public static Tuple<string, string> OnlineserversCredentials = new Tuple<string, string>("judy", "DAda1994@");
        public static Tuple<string, string> OfflineserversCredentials = new Tuple<string, string>("Admin", "admin54321");

        //SET Localhost = true for using local database;
        public static bool Localhost = true;

        public static bool close_app;

        public static string ReplaceQuotation(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }
        public static string CleanSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // \s+ تعني البحث عن فراغ واحد أو أكثر، واستبداله بفراغ واحد فقط
            // Trim() تزيل الفراغات من البداية والنهاية
            return Regex.Replace(text, @"\s+", " ").Trim();
        }

        public static string getSelectedItems_CheckboxComboBox(CheckBoxComboBox cbox, bool with_cotation)
        {
            string selected = "";
            foreach (var item in cbox.CheckBoxItems)
            {
                if (item.Checked)
                {
                    if (with_cotation)
                        selected += "'" + ReplaceQuotation(item.Text.ToString().Trim()) + "'";
                    else
                        selected += ReplaceQuotation(item.Text.ToString().Trim());
                    selected += ",";
                }
            }
            selected = selected.Remove(selected.Length - 1, 1);
            return selected;
        }

        public static string server = "";

        public static void set_ConnectionString()
        {
            //Added for the new encryption
            //string datalocation;
            //if (Properties.Settings.Default.Online)
            //    datalocation = "mysql.s1893.sureserver.com";
            //else datalocation = "localhost";
            //// else datalocation = "PC-ADMIN";
            //Encryption.DecryptCredentials(datalocation);

            Program.ConnectionString = "";
            if (Properties.Settings.Default.Online)
                Program.ConnectionString = "Server=mysql.s1893.sureserver.com;Port=3308;Uid=micro;Password=Center2018Hope;Convert Zero Datetime=True;CHARSET=utf8;Connect Timeout=21600;Allow User Variables=True;Pooling=false;";
            else
                //Program.ConnectionString = "Server=PC-ADMIN;Port=3306;Uid=jj;Password=pp;Convert Zero Datetime=True;CHARSET=utf8;Connect Timeout=21600;Allow User Variables=True;Pooling=false;";
                Program.ConnectionString = "server=localhost;Port=3306;Uid=jj;Password=pp;Convert Zero Datetime=True;CHARSET=utf8;Connect Timeout=21600;Allow User Variables=True;";

            OnlineRoot = "ftp://hcsyria.org/micro_images2/";
            OfflineRoot = "ftp://PC-ADMIN/micro_images/";

            if (selected_database == "Aleppo")
            {
                ConnectionString = "Database=hcsyria_micro;" + ConnectionString;
                OnlineRoot += "aleppo";
                OfflineRoot += "aleppo";
            }
            else if (selected_database == "Homs")
            {
                ConnectionString = "Database=hcsyria_micro_h;" + ConnectionString;
                OnlineRoot += "homs";
                OfflineRoot += "homs";
            }
            else if (selected_database == "Damascus")
            {
                ConnectionString = "Database=hcsyria_micro_d;" + ConnectionString;
                OnlineRoot += "damascus";
                OfflineRoot += "damascus";
            }
            else if (selected_database == "Latakia")
            {
                ConnectionString = "Database=hcsyria_micro_l;" + ConnectionString;
                OnlineRoot += "latakia";
                OfflineRoot += "latakia";
            }
            else if (selected_database == "AlWadi")
            {
                ConnectionString = "Database=hcsyria_micro_w;" + ConnectionString;
                OnlineRoot += "wadi";
                OfflineRoot += "wadi";
            }
            else if (selected_database == "Lebanon")
            {
                ConnectionString = "Database=hcsyria_micro_lb;" + ConnectionString;
                OnlineRoot += "lebanon";
                OfflineRoot += "lebanon";
            }
            else
            {
                OnlineRoot = "";
                OfflineRoot = "";
            }

        }

        public static bool buildConnection()
        {
            try
            {

                if (ConnectionString == "") return false;

                MyConn = new MySqlConnection(ConnectionString);
                if(MyConn.State == ConnectionState.Open)
                {
                    MyConn.Close();
                }
                if(MyConn.State == ConnectionState.Closed)
                {
                    MyConn.Open();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MySql") || ex.Message.Contains("MySQL"))
                {
                    MessageBox.Show("please check your network and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MySqlConnection.ClearPool(MyConn);
                    return false;
                }
                else if (ex.Message.Contains("host"))
                {
                    MessageBox.Show("please check the server or your network connection and try again", "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MySqlConnection.ClearPool(MyConn);
                    return false;
                }
                else
                {
                    MessageBox.Show(ex.Message, "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MySqlConnection.ClearPool(MyConn);

                    return false;
                }
            }
        }

        public static bool firstbuildConnection()
        {
            try
            {
                MyFirstConn = new MySqlConnection(ConnectionString);
                MySqlConnection.ClearPool(MyFirstConn);
                if (MyFirstConn.State == ConnectionState.Open)
                {
                    MyFirstConn.Close();
                }
                if (MyFirstConn.State == System.Data.ConnectionState.Closed)
                {
                    MyFirstConn.Open();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }


        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            close_app = false;
            Login_Form login_Form = new Login_Form();
            DialogResult result = login_Form.ShowDialog();
            if (result == DialogResult.OK)
            {
                close_app = false;
            }
            else close_app = true;

            System.Windows.Forms.Application.Run(new MainForm());

        }

    }
}