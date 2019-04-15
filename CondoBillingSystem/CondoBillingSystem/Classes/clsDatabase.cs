using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace CondoBillingSystem.Classes
{
    public class clsDatabase
    {
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
    }
}
