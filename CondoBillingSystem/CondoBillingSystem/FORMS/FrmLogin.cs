using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CondoBillingSystem.Classes;
using System.IO;

namespace CondoBillingSystem.FORMS
{
    public partial class FrmLogin : Form
    {
        clsFunction cf = new clsFunction();
        clsIni ci;
        string strDBType = string.Empty;
        string DBFile = string.Empty;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private bool checkDatabase()
        {
            bool result = false;
            try
            {
                string SettingFilePath = Environment.CurrentDirectory + "\\Settings.ini";
                if (File.Exists(SettingFilePath))
                {
                    ci = new clsIni(SettingFilePath);
                    strDBType = ci.Read("DatabaseType", "SystemSettings");
                    if (!string.IsNullOrEmpty(strDBType))
                    {
                        DBFile = ci.Read("DBFilePath", "SQLLite");
                    }

                    result = string.IsNullOrEmpty(DBFile) ? false : File.Exists(DBFile) ? true : false;

                }
            }
            catch
            {
            }
            return result;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (!checkDatabase())
            {
                MessageBox.Show("Error: Please check your database file.", "DB File is invalid or missing.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return;
            }
        }
    }
}
