using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace CondoBillingSystem.Classes
{
    public class clsDatabase
    {
        public string SQLConnString;
        string DBPath = Environment.CurrentDirectory + "\\Settings.ini"; 
        MySqlConnection myConnection;

        public bool isDBConnected(string ConnectionString, ref MySqlConnection myConn)
        {
            bool result = false;
            try
            {
                using (MySqlConnection mycon = new MySqlConnection(ConnectionString))
                {
                    mycon.Open();
                    myConn = mycon;
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        public string GetConnString()
        {
            clsIni ini = new clsIni(DBPath);
            string result = string.Empty;
            try
            {
                if (DBPath.Contains(".ini"))
                {
                    result = "Server = " + ini.Read("ServerName", "DatabaseSettings") + "; Database = " + ini.Read("DatabaseName", "DatabaseSettings") + "; User Id = " + ini.Read("Username", "DatabaseSettings") + ";" +
                             "Password = " + ini.Read("Password", "DatabaseSettings") + "; ";
                }
            }
            catch
            {
            }
            return result;
        }
        public string SQLConnBuilder(string DBPath)
        {
            string result = string.Empty;
            try
            {
                string ConnViaIni = string.Empty;
                if (DBPath.ToLower().Contains("server"))
                {
                    ConnViaIni = DBPath;
                }
                else
                {
                    ConnViaIni = GetConnString();
                }
                SQLConnString = ConnViaIni;//"Data Source=" + DBPath + ";Version=3;New=True;Compress=True;";
                result = SQLConnString;
            }
            catch
            {
            }
            return result;
        }
        public bool ExecuteNonQuery(string SQLStatement)
        {
            bool result = false;
            try
            {
                //string SqlConnstr = SQLConnBuilder(DBPath);
                string sqlConstr = SQLConnBuilder(DBPath);
                MySqlConnection sconn = new MySqlConnection(sqlConstr);
                sconn.Open();
                if (sconn.State == ConnectionState.Open)
                {
                    MySqlCommand sqlcom = new MySqlCommand(SQLStatement, sconn);
                    sqlcom.ExecuteReader();
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        public DataTable ExecuteQuery(string SQLStatement)
        {
            DataTable dtResult = new DataTable();
            try
            {
                string SqlConnstr = SQLConnBuilder(DBPath);
                MySqlConnection sconn = new MySqlConnection(SqlConnstr);
                sconn.Open();
                if (sconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter sda = new MySqlDataAdapter(SQLStatement, sconn);
                    sda.Fill(dtResult);
                }
            }
            catch
            {
            }
            return dtResult;
        }
    }
}
