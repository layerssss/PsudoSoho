using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
namespace ResourcesDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var str = Clipboard.GetData(DataFormats.StringFormat) as string;
            if (str!=null&&str.StartsWith("http://"))
            {
                var wc = new WebClient();
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                var path = this.saveFileDialog1.FileName.Remove(this.saveFileDialog1.FileName.LastIndexOf('\\') + 1);
                try
                {
                    var filename=str.Substring(str.LastIndexOf('/') + 1);
                    try
                    {
                        filename = filename.Remove(filename.IndexOf('#'));
                    }
                    catch { }
                    try
                    {
                        filename = filename.Remove(filename.IndexOf('?'));
                    }
                    catch { }
                    if(filename==""){
                        filename="index.htm";
                    }
                    wc.DownloadFileAsync(new Uri(str), path + filename, "下载" + str + "成功\r\n");

                }
                catch (Exception ex)
                {

                    this.textBox1.Text += ex.Message + "\r\n";
                }
                wc.Dispose();
                Clipboard.Clear();
            }
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.textBox1.Text += e.UserState as string;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                timer1.Stop();
                this.Close();
            }
        }
    }
}
