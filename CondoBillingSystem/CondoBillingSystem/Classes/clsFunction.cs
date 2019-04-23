using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CondoBillingSystem.Classes;
using MySql.Data.MySqlClient;
using System.Management;
using clsLic;
using System.Data;

namespace CondoBillingSystem.Classes
{
    public class clsFunction
    {
        clsDatabase cd = new clsDatabase();
        string CryptoKeys = "cbms-3bok-siri19";
        public bool CheckDatabaseInfo(clsIni ci, ref string ConnectionString, ref MySqlConnection myconn)
        {
            bool result = false;
            try
            {
                string myServerAddress = string.Empty;
                string myDataBase = string.Empty;
                string myUsername = string.Empty;
                string myPassword = string.Empty;

                myServerAddress = ci.Read("Servername", "DatabaseSettings");
                myDataBase = ci.Read("Databasename", "DatabaseSettings");
                myUsername = ci.Read("Username", "DatabaseSettings");
                myPassword = ci.Read("Password", "DatabaseSettings");

                if (!string.IsNullOrEmpty(myServerAddress) && !string.IsNullOrEmpty(myDataBase) &&
                    !string.IsNullOrEmpty(myUsername))
                {
                    ConnectionString = "Server = " + myServerAddress + "; Database = " + myDataBase + "; Uid = " + myUsername + "; Pwd =" + myPassword + ";";

                    result = cd.isDBConnected(ConnectionString, ref myconn);
                }
            }
            catch
            {
            }
            return result;
        }
        public DataTable GetUserRole()
        {
            DataTable dtResult = new DataTable();
            try
            {
                string Query = "Select RoleName,sysID from tbluserrole where isEnabled = 1 order by RoleName asc";
                dtResult = cd.ExecuteQuery(Query);
            }catch
            {
            }
            return dtResult;
        }
        public bool CheckUsers(string Username, string Password, string Role)
        {
            bool result = false;
            try
            {
                string Query = "Select ";
            }
            catch
            {
            }
            return result;
        }
        public bool CheckLicense(string LicenseCode)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(LicenseCode))
                {
                    result = CheckHardDriveLicense(LicenseCode);
                }
            }
            catch
            {
            }
            return result;
        }
        public bool CheckHardDriveLicense(string LicenseKey)
        {
            bool result = false;
            try
            {
                string HardDrive = GetHardDiskSerialNo();
                if (HardDrive == Decrypt(LicenseKey))
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }
        public string Decrypt(string Value)
        {
            string result = string.Empty;
            try
            {
                result = clsLic.CryptoEngine.Decrypt(Value, CryptoKeys);
            }
            catch
            {
            }
            return result;
        }
        public string Crypt(string Value)
        {
            string result = string.Empty;
            try
            {
                result = clsLic.CryptoEngine.Encrypt(Value, CryptoKeys);
            }
            catch
            {
            }
            return result;
        }
        public string GetHardDiskSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }
    }
}
