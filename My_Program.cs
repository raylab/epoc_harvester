using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Epoc_harvister
{
    static class My_Program
    {
        public static bool SRV = false;
        public static Form1 myForm;
        public static Epoc_runner myEpoc;
        public static List<PictureBox> myElectrodes = new List<PictureBox>();
        public static WSServer myServer;
        public static string port;
        public static string myIP;

        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new Form1();
            Application.Run(myForm);
        }        

        //public static Form1 myForm;
        //public static Epoc_runner myEpoc;
        //public static List<PictureBox> myElectrodes = new List<PictureBox>();
        //public static WSServer myServer;
        //public static string port;
        //public static string myIP;
        //public static bool SRV = false;

        public static void Start_me()
        {
            foreach (var eegCq in myForm.pictureBoxBG.Controls.OfType<PictureBox>())
            {
                myElectrodes.Add(eegCq);
            }

            myForm.textBox1.Text = "Trying...";
            myEpoc = new Epoc_runner();
            myIP = GetLocalIPAddress();
            myForm.textBox10.Text = myIP;
            myForm.timer1.Enabled = true ;
            myForm.textBox8.Text = $"{SRV}";
        }

        public static void Stop_Server()
        {
            myServer.ws.Dispose();
            myServer = null;
            SRV = false;
            myForm.textBox8.Text = $"{SRV}";
        }

        public static void Start_Server()
        {
            if(myForm.textBox9.Text != "")
            {           
                port = myForm.textBox11.Text;
                string myhost = "ws://" + myIP + ":" + port;
                myForm.textBox1.Text = $"{myhost}";
                myServer = new WSServer(myhost);
                SRV = true;
                myForm.textBox8.Text = $"{SRV}";
            }
            else
            {
                MessageBox.Show("You need Record Number");
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void Stop_me()
        {
            myForm.textBox1.Text = "Trying...";
            myForm.timer1.Enabled = false;
            myEpoc.Stop();
        }
    }  
}
