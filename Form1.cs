using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Epoc_harvister
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBoxBG.Controls.Add(IEE_CHAN_CMS);
            pictureBoxBG.Controls.Add(IEE_CHAN_DRL);
            pictureBoxBG.Controls.Add(IEE_CHAN_AF3);
            pictureBoxBG.Controls.Add(IEE_CHAN_F7);
            pictureBoxBG.Controls.Add(IEE_CHAN_F3);
            pictureBoxBG.Controls.Add(IEE_CHAN_FC5);
            pictureBoxBG.Controls.Add(IEE_CHAN_T7);
            pictureBoxBG.Controls.Add(IEE_CHAN_P7);
            pictureBoxBG.Controls.Add(IEE_CHAN_O1);
            pictureBoxBG.Controls.Add(IEE_CHAN_O2);
            pictureBoxBG.Controls.Add(IEE_CHAN_P8);
            pictureBoxBG.Controls.Add(IEE_CHAN_T8);
            pictureBoxBG.Controls.Add(IEE_CHAN_FC6);
            pictureBoxBG.Controls.Add(IEE_CHAN_F4);
            pictureBoxBG.Controls.Add(IEE_CHAN_F8);
            pictureBoxBG.Controls.Add(IEE_CHAN_AF4);

            IEE_CHAN_CMS.Location = new Point(36, 267);
            IEE_CHAN_DRL.Location = new Point(271, 268);
            IEE_CHAN_AF3.Location = new Point(89, 66);
            IEE_CHAN_F7.Location = new Point(43, 113);
            IEE_CHAN_F3.Location = new Point(121, 116);
            IEE_CHAN_FC5.Location = new Point(74, 158);
            IEE_CHAN_T7.Location = new Point(25, 206);
            IEE_CHAN_P7.Location = new Point(73, 328);
            IEE_CHAN_O1.Location = new Point(118, 385);
            IEE_CHAN_O2.Location = new Point(196, 386);
            IEE_CHAN_P8.Location = new Point(241, 328);
            IEE_CHAN_T8.Location = new Point(292, 208);
            IEE_CHAN_FC6.Location = new Point(240, 161);
            IEE_CHAN_F4.Location = new Point(196, 118);
            IEE_CHAN_F8.Location = new Point(275, 114);
            IEE_CHAN_AF4.Location = new Point(227, 69);

            IEE_CHAN_CMS.BackColor = Color.Transparent;
            IEE_CHAN_DRL.BackColor = Color.Transparent;
            IEE_CHAN_AF3.BackColor = Color.Transparent;
            IEE_CHAN_F7.BackColor = Color.Transparent;
            IEE_CHAN_F3.BackColor = Color.Transparent;
            IEE_CHAN_FC5.BackColor = Color.Transparent;
            IEE_CHAN_T7.BackColor = Color.Transparent;
            IEE_CHAN_P7.BackColor = Color.Transparent;
            IEE_CHAN_O1.BackColor = Color.Transparent;
            IEE_CHAN_O2.BackColor = Color.Transparent;
            IEE_CHAN_P8.BackColor = Color.Transparent;
            IEE_CHAN_T8.BackColor = Color.Transparent;
            IEE_CHAN_FC6.BackColor = Color.Transparent;
            IEE_CHAN_F4.BackColor = Color.Transparent;
            IEE_CHAN_F8.BackColor = Color.Transparent;
            IEE_CHAN_AF4.BackColor = Color.Transparent;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           My_Program.myEpoc.Run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start Engine")
            {
                button1.Text = "Stop Engine";
                My_Program.Start_me();
            }
            else if (button1.Text == "Stop Engine")
            {
                button1.Text = "Start Engine";
                My_Program.Stop_me();
            }

        }

        //private void Form1_Load(object sender, EventArgs e)
        //{

        //}

        private void button2_Click(object sender, EventArgs e)
        {
            if (My_Program.myForm.textBox9.Text != "" && My_Program.myForm.textBox10.Text != "")
            {
                if (button2.Text == "Connect")
                {
                    button2.Text = "Disconnect";
                    My_Program.Start_Server();
                }
                else if (button2.Text == "Disconnect")
                {
                    button2.Text = "Connect";
                    My_Program.Stop_Server();
                }
            }
            else
            {
                MessageBox.Show("You need Log address and Record Number");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start Engine")
            {
                MessageBox.Show("You need to start Emo Engine first");
            }
            else if (button1.Text == "Stop Engine")
            {
                My_Program.Do_license();
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            My_Program.StartStim();
        }
    }
}
