using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyWorkApplication.Classes
{
    internal class TasksOfProjects
    {
        private readonly MySqlComponents MySS;
        private string query;

        public TasksOfProjects()
        {
            MySS = new MySqlComponents();
        }

        //public void Insert_Task_MicroProject(int MicroProject_ID, int Task_ID, int State, DateTime Date)
        //{

        //    Program.buildConnection(); 
        //    MySS.query = "INSERT INTO `task_microproject`(`MicroProject_ID`, `Task_ID`, `State`, `Date`) VALUES ("
        //                 + MicroProject_ID + ","
        //                 + Task_ID + ","
        //                 + State + ",'"
        //                 + Date.Year + "/" + Date.Month + "/" + Date.Day + "' )";
        //    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        //    MySS.sc.ExecuteNonQuery();
        //    Program.MyConn.Close();
        //}

        //public void Insert_NewTasks_MicroProject(int MicroProject_ID, DateTime Date)
        //{

        //    Program.buildConnection();

        //    var task_IDs = new List<int>();
        //    task_IDs.Add(30);
        //    task_IDs.Add(31);

        //    string query = "";
        //    for (var i = 0; i < task_IDs.Count; i++)
        //    {
        //        query += "INSERT INTO `task_microproject`(`MicroProject_ID`, `Task_ID`, `State`, `Date`) VALUES ("
        //                + MicroProject_ID + ","
        //                + task_IDs.ElementAt(i) + ","
        //                + 0 + ",'"
        //                + Date.Year + "/" + Date.Month + "/" + Date.Day + "' );"; 
        //    }
        //    MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
        //    MySS.sc.ExecuteNonQuery();
        //    Program.MyConn.Close();
        //}

        public void Insert_AllTasks_MicroProject(int MicroProject_ID, DateTime Date, string Type)
        {

            Program.buildConnection();

            MySS.query = " SELECT `T_ID` FROM `task` where `T_Type` like '" + Type + "';";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var reader = MySS.sc.ExecuteReader();
            var task_IDs = new List<int>();
            while (reader.Read()) task_IDs.Add(Convert.ToInt32(reader.GetValue(0)));
            reader.Close();
            Program.MyConn.Close();
            int state;

            string query = "";
            Program.buildConnection();
            for (var i = 0; i < task_IDs.Count; i++)
            {
                if (task_IDs.ElementAt(i) == 1) //Project aprooved
                    state = 1;
                else state = 0;
                query += "INSERT INTO `task_microproject`(`MicroProject_ID`, `Task_ID`, `State`, `Date`) VALUES ("
                            + MicroProject_ID + ","
                            + task_IDs.ElementAt(i) + ","
                            + state + ",'"
                            + Date.Year + "/" + Date.Month + "/" + Date.Day + "' );";
            }
            var sc = new MySqlCommand(query, Program.MyConn);
            sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        public void Update_Task_MicroProject(int MicroProject_ID, int Task_ID, bool state, DateTime Date)
        {

            Program.buildConnection();
            MySS.query = "update `task_microproject` set `State` = " + (state ? 1 : 0)
                        + ", Date = '" + Date.Year + "/" + Date.Month
                        + "/" + Date.Day + "' "
                        + " where `MicroProject_ID` = " + MicroProject_ID
                        + " and  Task_ID = " + Task_ID + " ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }
        public void Update_Task_MicroProject(int MicroProject_ID, string Task, bool state, DateTime Date)
        {

            Program.buildConnection();
            MySS.query = "update `task_microproject` set `State` = " + (state ? 1 : 0)
                        + ", Date = '" + Date.Year + "/" + Date.Month
                        + "/" + Date.Day + "' "
                        + " where `MicroProject_ID` = " + MicroProject_ID
                        + " and  Task_ID = (select T_ID from task where T_Name like '" + Task + "') ";
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            MySS.sc.ExecuteNonQuery();
            Program.MyConn.Close();
        }

        public bool Get_Task_MicroProject_State(int MicroProject_ID, int Task_ID)
        {

            Program.buildConnection();

            MySS.query = "SELECT `State` FROM `task_microproject` WHERE `MicroProject_ID` = " + MicroProject_ID +
                         "  and `Task_ID` =" + Task_ID;
            MySS.sc = new MySqlCommand(MySS.query, Program.MyConn);
            var state = Convert.ToBoolean(MySS.sc.ExecuteScalar());

            Program.MyConn.Close();
            return state;
        }

        public DataTable Get_Tasks_Users()
        {
            Program.buildConnection();
            query = @"SELECT `Task_ID`
, `User_ID` as 'User_ID'
, UserName as 'Username'  
, `T_Name` as 'Task'
, `T_Type` as 'Type'
 FROM `task_user` left join `user` on user.UserID = task_user.User_ID 
 left join `task` on task.T_ID = task_user.Task_ID ";

            //if (type != "")
            //    query += " where T_Type like '" + type + "'";

            query += " ORDER BY `Task_ID` ";

            var da = new MySqlDataAdapter(query, Program.MyConn);
            var dt = new DataTable();
            da.Fill(dt);
            Program.MyConn.Close();
            return dt;
        }
    }
}