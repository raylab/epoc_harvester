using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;

namespace Epoc_harvister
{
    static class My_Program
    {
        public static bool SRV = false;
        public static Form1 myForm;
        public static Form2 stimForm;
        public static Epoc_runner myEpoc;
        public static List<PictureBox> myElectrodes = new List<PictureBox>();
        public static MyWebSocket myWS;
        public delegate void ObjectDelegate(object obj);
        public static bool stimEnabled = false;
        public static List<string> myPics = new List<string>();
        public static string myPicPath;
        public static bool stimTime = false;

        //public static List<string> MyPics { get => myPics; set => myPics = value; }
        //public static My_License My_License;

        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new Form1();
            stimForm = new Form2();
            Application.Run(myForm);
        }   
        
        public static void Do_license()
        {
            My_License.activateLicense();
        }

        public static void Start_me()
        {
            foreach (var eegCq in myForm.pictureBoxBG.Controls.OfType<PictureBox>())
            {
                myElectrodes.Add(eegCq);
            }
            myForm.textBox1.Text = "Trying...";
            myEpoc = new Epoc_runner();
            myForm.timer1.Enabled = true ;
            myForm.textBox8.Text = $"{SRV}";
        }

        public static void UpdateMsgBox(object obj)
        {
            if (myForm.InvokeRequired)
            {
                ObjectDelegate method = new ObjectDelegate(UpdateMsgBox);
                myForm.Invoke(method, obj);
                return;
            }
            string text = (string)obj;
            myForm.textBox1.Text = text;
            var myJson = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(text);
            if (myJson["status"] == "record_started")
            {
                if (stimEnabled)
                {
                    string myDir = stimForm.textBox1.Text;
                    PlayStim(myDir);
                    //Console.WriteLine("Will be starting Sim");
                } 
            }
            else if (myJson["status"] == "record_stoped")
            {
                StopStim();
            }
            //Console.WriteLine("GOT JSON:");
            //Console.WriteLine(myJson["status"]);
            //foreach (KeyValuePair<string, string> item in myJson)
            // {
            //    Console.WriteLine("Key:{0}, Value:{1}", item.Key, item.Value);
            // }
            
        }

        public static void PlayStim(string arg)
        {
            myPicPath = arg;
            foreach (string s in Directory.GetFiles(arg, "*.jpg").Select(Path.GetFileName))
            {
                myPics.Add(s);
            }
            stimForm.textBox1.Visible = false;
            stimForm.button1.Visible = false;
            stimForm.button2.Visible = false;
            stimForm.KillCursor(true);
            stimForm.BackColor = Color.Black;
            stimForm.FormBorderStyle = FormBorderStyle.None;
            stimForm.WindowState = FormWindowState.Maximized;
            stimForm.pictureBox1.Dock = DockStyle.Fill;
            stimForm.timer1.Enabled = true;
            //Console.WriteLine(String.Join(", ", myPics.ToArray()));
        }

        public static void StimFlip(bool arg)
        { 
            if (myPics.Any() == true)
            {
                if (arg)
                {
                    Random rnd = new Random();
                    int gap = rnd.Next(1, 13);
                    stimTime = true;
                    string myPicFile = myPicPath + "\\" + myPics[0];
                    myPics.RemoveAt(0);
                    stimForm.pictureBox1.Image = Image.FromFile(myPicFile);
                    stimForm.timer1.Enabled = false;
                    stimForm.timer1.Interval = 4000 + (1000 * gap);
                    stimForm.timer2.Enabled = true;
                    //Console.WriteLine(stimForm.timer1.Interval);
                    //Console.WriteLine("Playing");
                }
                else if (!arg)
                {
                    stimTime = false;
                    stimForm.pictureBox1.Image = Image.FromFile("c:\\Lab\\Black.png");
                    stimForm.timer1.Enabled = true;
                    stimForm.timer2.Enabled = false;
                }
            }
            else
            {
                StopStim();
            }
        }

        public static void StopStim()
        {
            myPics.Clear() ;
            stimForm.timer1.Enabled = false;
            stimForm.timer2.Enabled = false;
            stimForm.textBox1.Visible = true;
            stimForm.button1.Visible = true;
            stimForm.button2.Visible = true;
            stimForm.pictureBox1.Image = null;
            stimForm.FormBorderStyle = FormBorderStyle.Sizable;
            stimForm.WindowState = FormWindowState.Normal;
            stimForm.pictureBox1.Dock = DockStyle.None;
            stimForm.KillCursor(false);
            stimForm.BackColor = SystemColors.Control;
        }

        public static void StartStim()
        {
            stimForm.Show(); 
           // Console.WriteLine(myJson);
        }

        public static void Stop_Server()
        {
            myWS.DisconnectWS();
            SRV = false;
            myForm.textBox8.Text = $"{SRV}";
        }

        public static void Start_Server()
        {
            SRV = true;
            myWS = new MyWebSocket();
            myForm.textBox8.Text = $"{SRV}";                
        }

        public static void Stop_me()
        {
            myForm.textBox1.Text = "Trying...";
            myForm.timer1.Enabled = false;
            myEpoc.Stop();
        }
    }  
}
