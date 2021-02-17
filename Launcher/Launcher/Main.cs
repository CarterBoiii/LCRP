using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_HWID
{
    public partial class Launcher : Form
    {
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        public Launcher()
        {
            InitializeComponent();
        }

        public class WebClientWithTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wc = base.GetWebRequest(address);
                wc.Timeout = 5000;
                return wc;
            }
        }

        public bool GetConnection()
        {
            try
            {
                using (WebClient wc = new WebClientWithTimeout())
                {
                    string json = wc.DownloadString($"http://127.0.0.1:30120/");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public int pCount()
        {
            try
            {
                using (WebClient wc = new WebClientWithTimeout())
                {
                    string json = wc.DownloadString($"http://127.0.0.1:30120/players.json");
                    int player = json.Length - json.Replace("{", "").Length;
                    return player;
                }
            }
            catch
            {
                return 0;
            }
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            if (GetConnection())
            {
                label22.Text = "ONLINE";
                label22.ForeColor = Color.FromArgb(25, 200, 25);
            }

            int playerCount = pCount();
            string playerCountString = Convert.ToString(playerCount);
            label15.Text = $"{playerCountString} / 32";
        }

        private void close_Click(object sender, EventArgs e)
        {
            
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            System.Diagnostics.Process.Start("fivem://connect/play.lucidcityrp.com");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            System.Diagnostics.Process.Start("https://www.lucidcityrp.com/");
        }
    }
}
