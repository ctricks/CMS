using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CondoBillingSystem.Classes;
namespace CondoBillingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string IniPath = Environment.CurrentDirectory + "\\Settings.ini";

            if (!System.IO.File.Exists(IniPath))
            {
                MessageBox.Show("Error: Setting file is missing or corrupt. Please check this path \n" + IniPath,"Missing Ini File",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            clsIni ci = new clsIni(IniPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (ci.Read("LoginFirst", "SystemSettings").ToLower() == "true")
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Run(new FrmMain());
            }

        }
    }
}
