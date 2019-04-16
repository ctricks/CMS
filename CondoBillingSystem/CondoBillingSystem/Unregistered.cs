using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CondoBillingSystem.Classes;

namespace CondoBillingSystem
{
    public partial class Unregistered : Form
    {
        clsFunction cf = new clsFunction();
        public bool isMatch = false;
        public string MatchWord = string.Empty;
        public Unregistered()
        {
            InitializeComponent();
        }

        private void Unregistered_Load(object sender, EventArgs e)
        {
            tbMachineID.Text = cf.GetHardDiskSerialNo();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSerialNo.Text))
            {
                MessageBox.Show("Error: Cannot process blank value. Please provide a good one.","Unable to process",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            isMatch = cf.GetHardDiskSerialNo() == cf.Decrypt(tbSerialNo.Text) ? true : false;

            if (!isMatch)
            {
                MessageBox.Show("Error: Invalid Serial Number , Please provide a good one.", "Unable to process", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MatchWord = tbSerialNo.Text;
                MessageBox.Show("License is valid.","Applying License",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
