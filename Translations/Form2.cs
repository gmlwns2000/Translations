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
    public partial class Form2 : Form
    {
        SetOpacity opacity;
        SaveSetting save_set;
        public Form2(ref SetOpacity opacity,ref SaveSetting savset, double nowopa, ref int lang, bool savepos, bool mosttop, bool copyclip)
        {
            InitializeComponent();
            this.opacity = opacity;
            this.save_set = savset;
            trackBar1.Value = (int)nowopa;
            comboBox1.SelectedIndex = lang;
            checkBox1.Checked = savepos;
            checkBox2.Checked = mosttop;
            checkBox3.Checked = copyclip;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save_set(trackBar1.Value, comboBox1.SelectedIndex, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked);
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            opacity(trackBar1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            save_set(trackBar1.Value, comboBox1.SelectedIndex, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked);
        }
    }
}
