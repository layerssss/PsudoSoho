using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
namespace FileLocker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        RegistryKey shell;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.shell = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("CLASSES").OpenSubKey("*").OpenSubKey("shell", true);
            
            if (!(this.textBox1.Enabled = this.button1.Enabled = !(this.button2.Enabled = shell.GetSubKeyNames().Contains("FileLocker"))))
            {
                var str = this.shell.OpenSubKey("FileLocker").GetValue("") as string;
                this.textBox1.Text = str.Substring(2, str.Length - 12);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddLocker("*");
            AddLocker("Directory");
            this.Close();
        }
        public void AddLocker(string type)
        {
            this.shell = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("CLASSES").OpenSubKey(type).OpenSubKey("shell", true);
            this.textBox1.Text = this.textBox1.Text.Trim();
            var fileLocker = shell.CreateSubKey("FileLocker");
            fileLocker.SetValue("", "以“" + this.textBox1.Text + "”的身份锁定该文件。");
            var command = fileLocker.CreateSubKey("command");
            command.SetValue("", Application.ExecutablePath + " lock u" + this.textBox1.Text + " \"%1\"");
            var fileUnlocker = shell.CreateSubKey("FileUnlocker");
            fileUnlocker.SetValue("", "以“" + this.textBox1.Text + "”的身份解锁该文件。");
            command = fileUnlocker.CreateSubKey("command");
            command.SetValue("", Application.ExecutablePath + " unlock u" + this.textBox1.Text + " \"%1\"");


            var lockedFiles = shell.CreateSubKey("LockedFiles");
            lockedFiles.SetValue("", "查看所有被锁定的文件。");
            command = lockedFiles.CreateSubKey("command");
            command.SetValue("", Application.ExecutablePath + " locked u" + this.textBox1.Text + " \"dummy\"");
        }
        public void RemoveLocker(string type)
        {

            this.shell = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("CLASSES").OpenSubKey(type).OpenSubKey("shell", true);
            try
            {
                shell.OpenSubKey("FileLocker", true).DeleteSubKey("command");
                shell.DeleteSubKey("FileLocker");
                shell.OpenSubKey("FileUnlocker", true).DeleteSubKey("command");
                shell.DeleteSubKey("FileUnlocker");
                shell.OpenSubKey("LockedFiles", true).DeleteSubKey("command");
                shell.DeleteSubKey("LockedFiles");
            }
            catch { }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RemoveLocker("*");
            RemoveLocker("Directory");
            this.Close();
        }
    }
}
