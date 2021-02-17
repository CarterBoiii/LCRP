using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_HWID
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void LoginBTN_Click(object sender, EventArgs e)
        {
            string loginusername = Username2.Text;
            string loginpassword = Password2.Text;
            string rpassword = rpassword2.Text;

            System.Diagnostics.Process.Start("http://localhost/API/execute.php?action=registerUser&userName=" + Username2.Text + "&password=" + Password2.Text  + "&repassword=" + rpassword2.Text + "&registerKey=N0QxoXM");
            
            this.Hide();
            Login Login = new Login();
            Login.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Login = new Login();
            Login.Show();
        }

        private void Password2_TextChanged(object sender, EventArgs e)
        {

        }

        private void rpassword2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Username2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void register_Load(object sender, EventArgs e)
        {

        }
    }
}
