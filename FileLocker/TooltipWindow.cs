using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileLocker
{
    public partial class TooltipWindow : Form
    {
        public TooltipWindow()
        {
            InitializeComponent();
        }

        private void TooltipWindow_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.ShowBalloonTip(10000, this.Text, this.Value, this.ToolTipIcon);
        }
        public string Value;
        public ToolTipIcon ToolTipIcon;

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            this.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
