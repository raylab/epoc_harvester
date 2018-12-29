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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form_FormClosing;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolderDialog1 = new FolderBrowserDialog();// OpenFileDialog();
            //folderBrowserDialog1.RootFolder = "";
            //openFileDialog1.ShowDialog();
            if (openFolderDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFolderDialog1.SelectedPath;
            }else if (String.IsNullOrEmpty(textBox1.Text) == false)
            {
                string foo = textBox1.Text;
            }
            else
            {
                MessageBox.Show("Please select program to play.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) == false)
            {
                if (button2.Text == "Set")
                {
                   
                    My_Program.stimEnabled = true;
                    //My_Program.PlayStim(textBox1.Text);
                    button2.Text = "Ready";
                }
                else if (button2.Text == "Ready")
                {
                    My_Program.stimEnabled = false;
                    button2.Text = "Set";
                }

            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool flag = true;
            My_Program.StimFlip(flag);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            bool flag = false;
            My_Program.StimFlip(flag);
        }
        public void KillCursor(bool flag)
        {
            if (flag)
            {
                Cursor.Hide();
            }
            else if (!flag)
            {
                Cursor.Show();
            }
        }
    }
}
