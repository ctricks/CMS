using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CondoBillingSystem.Classes;
using System.Threading;
using CondoBillingSystem.Classes;
using MySql.Data.MySqlClient;

namespace CondoBillingSystem
{
    public partial class Form1 : Form
    {
        clsIni ci;
        int counter = 0;
        List<string> PicLogin = new List<string>();
        clsFunction cf = new clsFunction();
        string ConnectionString = string.Empty;

        MySqlConnection myConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Preparing()
        {
            string ApplicationBackground = Environment.CurrentDirectory + "\\Resources\\Background.jpg";
            if(File.Exists(ApplicationBackground))
            {
                ApplicationBackground = ci.Read("ApplicationBackground", "SystemSettings") == "" ? ApplicationBackground : ci.Read("ApplicationBackground", "SystemSettings");
                Image imgbg = Image.FromFile(ApplicationBackground);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackgroundImage = imgbg;
            }
            tmrLoading.Start();
        }

        private void ResizeForm()
        {

        }

        private void GetCompanyInfo()
        {
            lblCompanyName.Text = ci.Read("CompanyName", "CompanyInfo");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string IniPath = Environment.CurrentDirectory + "\\Settings.ini";
            if (!File.Exists(IniPath))
            {
                MessageBox.Show("Error: File Setting is missing. Please check\nPath:" + IniPath,"File not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            ci = new clsIni(IniPath);
            GetCompanyInfo();
            string LoginPic = ci.Read("LoginPicLocation","SystemSettings");
            if (!string.IsNullOrEmpty(LoginPic) && Path.GetExtension(LoginPic) == ".jpg")
            {
                pbImageLoading.Image = Image.FromFile(LoginPic);
            }
                PicLogin = GetPicFromCondoPics();
            Preparing();
        }
        private List<string> GetPicFromCondoPics()
        {
            List<string> PicStr = new List<string>();
            try
            {
                string PicExt = ci.Read("LoginPicExtension", "SystemSettings");
                string PicPath = Environment.CurrentDirectory + @"\Resources\CondoPics\";
                int PicCount = 0;
                if (Directory.Exists(PicPath))
                {
                    if(!string.IsNullOrEmpty(PicExt))
                     PicCount = Directory.GetFiles(PicPath, "*." + PicExt).Count(); 
                }

                if (PicCount > 0)
                {
                    foreach (string PicLocation in Directory.GetFiles(PicPath, "*." + PicExt))
                    {
                        PicStr.Add(PicLocation);
                    }
                }
            }
            catch
            {
            }
            return PicStr;
        }
        private void LoadLoginImage(int ctr)
        {
            if (PicLogin.Count > 0)
            {
                pbImageLoading.Image = Image.FromFile(PicLogin[ctr].ToString());
                pbImageLoading.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void tmrLoading_Tick(object sender, EventArgs e)
        {   
            Thread.Sleep(500);

            if (progressBar1.Value == 10)
            {
                lblProcessStatus.Text = "Checking Database...";
                if (!cf.CheckDatabaseInfo(ci, ref ConnectionString,ref myConnection))
                {
                    lblProcessStatus.ForeColor = Color.Red;
                    lblProcessStatus.Text = "Error: Please check your database connection settings.";
                    tmrLoading.Stop();
                    return;
                }
            }

            if(progressBar1.Value == 50)
            {
                lblProcessStatus.Text = "Checking License...";
                if(!cf.CheckLicense(ci.Read("LicenseCode","Licensing")))
                {
                    lblProcessStatus.ForeColor = Color.Red;
                    lblProcessStatus.Text = "Error: Please check your license settings.";
                    tmrLoading.Stop();
                    DialogResult drQuestion = MessageBox.Show("License is Invalid. Would you like update it today?","Update License",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (drQuestion == System.Windows.Forms.DialogResult.Yes)
                    {
                        Unregistered uv = new Unregistered();
                        uv.ShowDialog();
                        if (uv.isMatch)
                        {
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Application will close. Please check with your vendor \n(unregistered copy)", "Unable to use the Application.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Application will close. Please check with your vendor (unregistered copy)", "Unable to use the Application.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (progressBar1.Value >= 100)
            {
                tmrLoading.Stop();
                progressBar1.Value = 100;
                return;
            }
            progressBar1.Value += 5;
            if (progressBar1.Value % 10 == 0)
            {
                counter++;
                if (counter > PicLogin.Count -1)
                {
                    counter = 1;
                }
                LoadLoginImage(counter);
            }
        }
    }
}
