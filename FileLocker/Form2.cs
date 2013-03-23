using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace FileLocker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var dev = new DevService.WebServiceSoapClient();
            var list = dev.GetLockedFile();
            foreach (var fi in list)
            {
                var btn = new Button()
                {
                    Text = fi[0] + "已锁定" + fi[1],
                    Tag = fi[1],
                    TextAlign=ContentAlignment.MiddleLeft
                };
                this.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.Click += new EventHandler(btn_Click);
                
            }
        }
        public string Username;
        void btn_Click(object sender, EventArgs e)
        {
            var path = (sender as Button).Tag as string;
            try
            {
                var dev = new DevService.WebServiceSoapClient();
                var msg="";
                if ((msg = dev.UnlockFile(path, Username)) != "")
                {
                    throw (new Exception(msg));
                }
                this.Controls.Remove(sender as Button);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作未成功：" + ex.Message, "文件锁定器", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
