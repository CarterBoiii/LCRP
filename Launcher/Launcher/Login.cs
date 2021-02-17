using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Login_HWID
{
    public partial class Login : Form
    {
        public static Point newpoint = new Point();
        public static int x;
        public static int y;
        public static string passInfo;
        public static string userInfo;
        public Login()
        {
            InitializeComponent();
        }

        
        #region "Button Login"

        private void LoginBTN_Click(object sender, EventArgs e) 
        {
            LoginBTN.Text = "Login"; 

            try
            {
                if (Execute("accessAccount", "userName=" + Username.Text + "&password=" + Password.Text + "&registerKey=" + "LCRP") == 1)
                {
                    Username.Text = Username.Text; 
                    

                    WebClient fetchInfo = new WebClient();
                    string allowedState = fetchInfo.DownloadString("http://localhost/API/execute.php?action=isALLOWED&userName=" + Username.Text); 

                    if (allowedState == "ALLOWED") 
                    {
                        HWIDReset(); 
                        HWIDAllowed(); 
                        GETIP(); 
                    }
                    else if (allowedState == "ADMIN")
                    {
                        HWIDReset();
                        HWIDAllowed();
                        GETIP();
                    }
                    else if (allowedState == "BANNED") 
                    {
                        MessageBox.Show("You are banned from Lucid City Roleplay. To request an unban please head over to our forums https://www.lucidcityrp.com/", "Login HWID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoginBTN.Text = "Login"; 
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("API is offline, contact Sofia M | Sofia_XD#7705 or wait.", "SERVER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginBTN.Text = "Login"; 
            }
        }

        #endregion


        
        #region "HWID + Encryptions"

        private static string para3() 
        {
            string str = "";
            ManagementObjectCollection.ManagementObjectEnumerator objA = new ManagementObjectSearcher("Select * From Win32_processor").Get().GetEnumerator();
            try
            {
                while (true)
                {
                    if (!objA.MoveNext())
                    {
                        break;
                    }
                    ManagementObject current = (ManagementObject)objA.Current;
                    str = current["ProcessorID"].ToString();
                }
            }
            finally
            {
                if (!ReferenceEquals(objA, null))
                {
                    objA.Dispose();
                }
            }
            ManagementObject obj3 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            obj3.Get();
            return (str + obj3["VolumeSerialNumber"].ToString());
        }

        
        public static string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        #endregion 


        
        #region "HWID Reset"

        void HWIDReset()
        {
            try
            {
                string MDR = ("http://localhost/API/execute.php?action=sendhwid&userName=" + Username.Text + "&registerKey=" + para3()); 
                WebClient Check = new WebClient();
                string mdr = Check.DownloadString("http://localhost/API/execute.php?action=hwid&userName=" + Username.Text);

                if (mdr == "RESET") 
                {
                    WebClient LOL = new WebClient();
                    LOL.DownloadString(MDR);

                    MessageBox.Show("" + Username.Text + " HWID set, your computer is now linked to your account", "HWID set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to get / change HWID, contact Sofia M | Sofia_XD#7705 !");
                Application.Exit();
            }
        }
        #endregion


        
        #region "Void HWID Is Allowed"

        void HWIDAllowed()
        {
            string HWID1 = para3();
            try
            {
                WebClient HWID = new WebClient();
                string secure = HWID.DownloadString("http://localhost/API/execute.php?action=hwid&userName=" + Username.Text); 

                if (secure == HWID1)
                {
                    AllowAccess(); 
                }
                else
                {
                    MessageBox.Show("This account is registered to a another PC !", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("API is offline, contact Sofia M | Sofia_XD#7705.", "SERVER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        
        #region "Get Last IP + Send New"

        void GETIP() 
        {
            try
            {
                string externalip = new WebClient().DownloadString("http://ipinfo.io/ip");

                string GET = new WebClient().DownloadString("http://localhost/API/execute.php?action=GETIP&userName=" + Username.Text + "");
                string SEND = new WebClient().DownloadString("http://localhost/API/execute.php?action=IP&userName=" + Username.Text + "&IP=" + externalip + ""); 
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to get / change IP, contact Sofia M | Sofia_XD#7705 !");
                Application.Exit();
            }
        }

        #endregion

        #region "Some APIs"

        public static int Execute(string action, string args)
        {
            WebClient requests = new WebClient();
            string url = "http://localhost/API/execute.php";
            string urlaction = "?action=" + action;
            string urlargs = "&" + args;
            string buildurl = url + urlaction + urlargs;

            string response = requests.DownloadString(buildurl);
            if (response == null)
            {
                return 0;
            }

            if (!response.StartsWith("LOGIN_GOOD"))
            {
                CheckError(response);
                return 0;
            }

            return 1;
        }

        public static void RaiseError(string error)
        {
            MessageBox.Show(error, "Oops..", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }

        public static int CheckError(string error)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            Errors.Add("MISSING_PARAMETERS", "Missing parameters"); 
            Errors.Add("INVALID_KEY", "The registration key is not valid"); 
            Errors.Add("USERNAME_TOO_SHORT", "Your username is too short");
            Errors.Add("PASSWORD_TOO_SHORT", "Your password is too short"); 
            Errors.Add("USERNAME_TAKEN", "The username you choose is already taken"); 
            Errors.Add("PASSWORDS_NOT_MATCH", "Passwords do not match"); 
            Errors.Add("USER_BANNED", "You are banned from Lucid City Roleplay. To request an unban please head over to our forums https://www.lucidcityrp.com/"); 
            Errors.Add("NO_ACTION", "No action"); 
            Errors.Add("NOT_ENOUGH_PRIVILEGES", "You do not have enough privileges");
            Errors.Add("INVALID_CREDENTIALS", "Invalid Username or Password."); 

            if (!error.StartsWith("ERROR")) 
            {
                RaiseError(error);
                return 0;
            }

            string message = "Undefined error";
            string[] array = error.Split(':');
            if (array.Length == 2 && Errors.ContainsKey(array[1]))
            {
                string key = array[1];
                message = Errors[key];
            }

            RaiseError(message);
            return 1;
        }

        private void textBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginBTN_Click(null, null);
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginBTN_Click(null, null);
        }

        #endregion

        #region "AllowAccess"

        void AllowAccess()
        {

            MessageBox.Show("Welcome " + Username.Text + " Enjoy your stay with us");
            Launcher ALLOWED = new Launcher();
            ALLOWED.Show();
            this.Hide();
        }


        #endregion


































        //For move application
        #region "System Move Title Panel"
        private void xMouseDown(object sender, MouseEventArgs e)
        {
            x = Control.MousePosition.X - base.Location.X;
            y = Control.MousePosition.Y - base.Location.Y;
        }
        private void xMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                newpoint = Control.MousePosition;
                newpoint.X -= x;
                newpoint.Y -= y;
                base.Location = newpoint;
            }
        }
        #endregion 

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            register registerform = new register();
            registerform.Show();
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {

        }

        private void Password_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
