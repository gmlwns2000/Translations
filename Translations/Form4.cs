using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translations
{
    public delegate void Transfast(string text, int lang, bool clip);
    public partial class Form4 : Form
    {
        Transfast sender;
        bool close = false;
        public Form4(ref Transfast reciver, int lang, bool clip)
        {
            InitializeComponent();
            sender = reciver;
            checkBox1.Checked = clip;
            comboBox1.SelectedIndex = lang;
            this.KeyPreview = true;
            this.Focus();
            this.textBox1.Select();
        }
        private void Form4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.sender(textBox1.Text, comboBox1.SelectedIndex, checkBox1.Checked);
                timer2.Start();
            }
            else if(e.KeyCode == Keys.Escape)
            {
                timer2.Start();
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity*100.0f < 55)
            {
                Opacity = (Opacity * 100.0f + 12)/100.0f;
            }
            else
            {
                timer1.Stop();
                Console.WriteLine("TimerStopped!");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.Opacity * 100.0f > 0)
            {
                if (timer1.Enabled)
                {
                    timer1.Stop();
                }
                Opacity = (Opacity * 100.0f - 8) / 100.0f;
            }
            else
            {
                Console.WriteLine("TimerStoppedddd!");
                close = true;
                this.sender("", comboBox1.SelectedIndex, checkBox1.Checked);
                this.Close();
                timer2.Stop();
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if((e.CloseReason == CloseReason.UserClosing)&&(!close))
            {
                Console.WriteLine("Userd");
                e.Cancel = true;
            }
        }
    }
}
