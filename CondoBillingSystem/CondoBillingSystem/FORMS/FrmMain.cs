using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CondoBillingSystem
{
    public partial class FrmMain : Form
    {
        public bool LoginFirst;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (!LoginFirst)
            {
                FORMS.FrmLogin fl = new FORMS.FrmLogin();
                fl.ShowDialog();
            }
        }
    }
}
