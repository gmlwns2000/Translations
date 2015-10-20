using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Translations
{
    public delegate void SetOpacity(double opacity);
    public delegate void SaveSetting(double opacity, int language, bool save_positon, bool mosttop, bool copyclip);
    public delegate HtmlDocument NoneArg();
    public delegate void SetClipB(string text);
    public partial class Form1 : Form
    {
        private Point mCurrentPosition = new Point(0, 0);
        private bool moved = false;
        private bool save_pos = true;
        private bool copyclip = false;
        public int lang = 0;
        public int hot_iddd = 0;
        public int hot_idd = 0;
        public int hot_id = 0;
        private bool mosttop = false;
        private bool fasttrans = false;
        private double opacc = 0.0f;
        public string[,] commands = { { "/hello","Hello World!","msg"} , { "/hello", "Test Command", "pop" } };

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);

        public Form1()
        {
            InitializeComponent();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Translations";
            Console.WriteLine(path);
            if (new DirectoryInfo(path).Exists)
            {
                FileInfo fi = new FileInfo(path + "\\setting.cfg");
                if (!fi.Exists)
                {
                    fi.Create().Close();
                }
                
            }
            else
            {
                new DirectoryInfo(path).Create();
                new FileInfo(path + "\\setting.cfg").Create().Close(); ;
            }

            if(new FileInfo(path + "\\setting.cfg").Exists)
            {
                string[] config = File.ReadAllLines(path + "\\setting.cfg");
                if(config.Length > 0)
                {
                    for(int i = 0; i < config.Length; i++)
                    {
                        Console.WriteLine(config[i]);
                        if (i == 0)             //lang
                        {
                            this.lang = Convert.ToInt32(config[i]);
                        }
                        else if (i == 1)        //all top
                        {
                            if (config[i] == "true")
                            {
                                this.TopMost = true;
                                this.mosttop = true;
                            }
                            else
                            {
                                this.TopMost = false;
                                this.mosttop = false;
                            }
                        }
                        else if (i == 2)        //all copy
                        {
                            if (config[i] == "true")
                            {
                                copyclip = true;
                            }
                            else
                            {
                                copyclip = false;
                            }
                        }
                        else if (i == 3)        //alpha
                        {
                            this.Opacity = 1;
                            this.opacc = 1;
                            if (this.Opacity <= 0.25)
                            {
                                this.Opacity = 0.25;
                                this.opacc = 0.25;
                            }
                        }else if(i == 5)        //y
                        {
                            if (!(config[i] == "-1"))
                            {
                                this.Location = new Point(
                                    Convert.ToInt32(config[i-1]),
                                    Convert.ToInt32(config[i]));
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("io error");
                Application.Exit();
            }

            hot_id = Hotkey.RegisterHotKey(Keys.F1, KeyModifiers.Alt);
            hot_idd = Hotkey.RegisterHotKey(Keys.F2, KeyModifiers.Alt);
            hot_iddd = Hotkey.RegisterHotKey(Keys.F3, KeyModifiers.Alt);
            Hotkey.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
        }

        void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if(e.Key == Keys.F1)
            {
                get_word();
                go_trans();
            }
            else if(e.Key == Keys.F2)
            {
                if (!fasttrans)
                {
                    Console.WriteLine("FastTransOpen");
                    Transfast tf = new Transfast(go_trans);
                    Form4 f4 = new Form4(ref tf, lang, copyclip);
                    fasttrans = true;
                    f4.Focus();
                    f4.Show();
                }
            }
            else
            {
                go_trans();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //get_word();
        }

        private void get_word()
        {
            richTextBox1.Text = Clipboard.GetText();
        }
        private void go_trans()
        {
            if(richTextBox1.Text == "")
            {
                Console.WriteLine("HI!");
                return;
            }else if((richTextBox1.Text == "/exit")|| (richTextBox1.Text == "/ㄷ턋") || (richTextBox1.Text == "/EXIT") || (richTextBox1.Text == "/나가기"))
            {
                Application.Exit();
                return;
            }else if(richTextBox1.Text == "/option")
            {
                SetOpacity opacitysetting = new SetOpacity(set_opacity);
                SaveSetting sav = new SaveSetting(save_setting);
                Form2 f2 = new Form2(ref opacitysetting, ref sav, this.Opacity * 100, ref lang, save_pos, mosttop, copyclip);
                f2.Show();
                return;
            }else if(richTextBox1.Text == "/clip")
            {
                get_word();
                Thread.Sleep(2);
            }
            else if (richTextBox1.Text == "/clip")
            {
                Form5 f5 = new Form5(ref commands);
            }
            else
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    if (richTextBox1.Text == commands[i,0])
                    {
                        if (commands[i, 2] == "cmd")
                        {
                            
                        }else if (commands[i, 2] == "msg")
                        {
                            MessageBox.Show(commands[i, 1]);
                        }
                        break;
                    }
                }
            }
            string lang_str="ERROR";
            if (lang==0)
            {
                lang_str = "ko";
                Console.WriteLine("Get Korean");
            }
            else if (lang == 1)
            {
                lang_str = "en";
            }
            else if (lang == 2)
            {
                lang_str = "ja";
            }
            else if (lang == 3)
            {
                lang_str = "fr";
            }
            else if (lang == 4)
            {
                lang_str = "es";
            }
            else if (lang == 5)
            {
                lang_str = "ar";
            }
            else
            {
                lang_str = "ERROR";
            }
            if (lang_str == "ERROR")
            {
                notifyIcon1.ShowBalloonTip(1,"Errored", "Translation Errored Can't Find Language Code / Code:"+Convert.ToString(lang),ToolTipIcon.Error);
            }
            else
            {
                string url = "https://translate.google.com/#auto/" + lang_str + "/" + richTextBox1.Text;
                Console.WriteLine(url);
                Console.WriteLine("navigate");
                webBrowser1.Navigate(url);
            }
        }
        public void go_trans(string str)
        {
            richTextBox1.Text = str;
            go_trans();
        }
        public void go_trans(string str, int lang, bool clip)
        {
            if (str == "")
            {
                this.lang = lang;
                copyclip = clip;
                this.fasttrans = false;
                return;
            }
            this.lang = lang;
            copyclip = clip;
            this.richTextBox1.Text = str;
            go_trans();
            this.fasttrans = false;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Translations";
            if (new DirectoryInfo(path).Exists)
            {
                if (!new FileInfo(path + "\\setting.cfg").Exists)
                {
                    new FileInfo(path + "\\setting.cfg").Create().Close();
                }
            }
            else
            {
                new DirectoryInfo(path).Create();
                new FileInfo(path + "\\setting.cfg").Create().Close();
            }
            string text = Convert.ToString(lang) + "\n" + bool2string(mosttop) + "\n" + bool2string(copyclip) + "\n" + Convert.ToString((int)((float)this.Opacity * 100.0f)) + "\n";
            if (save_pos)
            {
                text = text + Convert.ToString(this.Location.X) + "\n" + Convert.ToString(this.Location.Y);
            }
            else
            {
                text = text + "-1\n-1";
            }
            Console.WriteLine(text);
            File.WriteAllText(path + "\\setting.cfg", text);
        }
        public void go_trans(ref WebBrowser wb)
        {
            Thread tr = new Thread(new ThreadStart(get_result));
            tr.Start();
        }

        public HtmlDocument get_web_document()
        {
            return webBrowser1.Document;
        }

        private void get_result()
        {
            Thread.Sleep(300);
            string tableId = "result_box";
            HtmlDocument doc = (HtmlDocument)webBrowser1.Invoke(new NoneArg(get_web_document));
            HtmlElement rb = doc.GetElementById(tableId);
            if (rb == null)
            {
                notifyIcon1.ShowBalloonTip(1, "Errored", "Errored on HTML Parsing, It can be not up to dated.", ToolTipIcon.Error);
                return;
            }

            HtmlElementCollection spans = rb.GetElementsByTagName("span");
            String result = "";
            foreach (HtmlElement el in spans)
            {
                el.Focus();
                result = result + el.InnerText + " ";
            }
            Console.WriteLine("Get result");
            notifyIcon1.ShowBalloonTip(100, "Translated", "Result: "+result, ToolTipIcon.Info);
            if (copyclip)
            {
                object[] t = new object[] { result };
                Invoke(new SetClipB(set_clipboard),t);
            }
        }

        private void set_clipboard(string str)
        {
            Clipboard.SetText(str);
            Console.WriteLine("Copy to Clipboard");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            set_opacity(trackBar1.Value);
        }
        
        private void go_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(
                this.Location.X + (mCurrentPosition.X + e.X),
                this.Location.Y + (mCurrentPosition.Y + e.Y));
                this.moved = true;
            }
        }

        private void go_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mCurrentPosition = new Point(-e.X, -e.Y);
            }
            else if(e.Button == MouseButtons.Right)
            {
                trackBar1.Visible = !trackBar1.Visible;
                
            }
        }

        private void go_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (moved)
                {
                    moved = false;
                    Console.WriteLine(Convert.ToString(this.Location.X) + "," + Convert.ToString(this.Location.Y));
                }
                else
                {
                    go_trans(richTextBox1.Text);
                }
            }
        }
        
        public void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.TopMost = true;
            Thread.Sleep(1);
            this.TopMost = false;
        }
        
        void set_opacity(double opacity)
        {
            this.Opacity = (float)opacity/100.0f;
            this.opacc = this.Opacity;
            Console.WriteLine(this.opacc);
        }

        void save_setting(double opacity, int language, bool save_positon, bool mmt, bool cc)
        {
            set_opacity(opacity);
            lang = language;
            save_pos = save_positon;
            mosttop = mmt;
            this.TopMost = mmt;
            copyclip = cc;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Translations";
            if (new DirectoryInfo(path).Exists)
            {
                if (!new FileInfo(path + "\\setting.cfg").Exists)
                {
                    new FileInfo(path + "\\setting.cfg").Create().Close();
                }
            }
            else
            {
                new DirectoryInfo(path).Create();
                new FileInfo(path + "\\setting.cfg").Create().Close();
            }
            Console.WriteLine(this.opacc);
            string text = Convert.ToString(lang) + "\n" + bool2string(mosttop) + "\n" + bool2string(copyclip) + "\n" + Convert.ToString((int)((float)this.opacc * 100.0f)) + "\n";
            if (save_pos)
            {
                text = text + Convert.ToString(this.Location.X) + "\n" + Convert.ToString(this.Location.Y);
            }
            else
            {
                text = text + "-1\n-1";
            }
            Console.WriteLine(text);
            File.WriteAllText(path + "\\setting.cfg", text);
        }

        string bool2string(bool check)
        {
            if (check)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 옵션ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetOpacity opacitysetting = new SetOpacity(set_opacity);
            SaveSetting sav = new SaveSetting(save_setting);
            Form2 f2 = new Form2(ref opacitysetting, ref sav, this.Opacity * 100, ref lang, save_pos, mosttop, copyclip);
            Console.WriteLine(lang);
            f2.Show();
            Console.WriteLine("f");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 번역ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            get_word();
            go_trans();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                go_trans(toolStripTextBox1.Text);
                contextMenuStrip1.Close();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsoluteUri == webBrowser1.Url.AbsoluteUri)
            {
                go_trans(ref this.webBrowser1);
            }
        }

        private void creditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 help = new Form3();
            help.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hotkey.UnregisterHotKey(hot_id);
            Hotkey.UnregisterHotKey(hot_idd);
            Hotkey.UnregisterHotKey(hot_iddd);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine(this.opacc);
            save_setting(this.opacc, this.lang, this.save_pos, this.mosttop, this.copyclip);
        }
    }
}
