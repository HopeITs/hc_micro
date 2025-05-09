using MyWorkApplication.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWorkApplication
{
    public partial class Encrypt : Form
    {
        public Encrypt()
        {
            InitializeComponent();
        }

        private void Encrypt_btn_Click(object sender, EventArgs e)
        {
            //txtFileName.Text = Encryption.EncryptString(Result.Text);
        }

        private void Decrypt_btn_Click(object sender, EventArgs e)
        {
            //Result.Text = Encryption.DecryptString(txtFileName.Text);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            //txtFileName.Text = Program.UserName;
        }
    }
}
