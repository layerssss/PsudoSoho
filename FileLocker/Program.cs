using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FileLocker
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 3)
            {
                var dev = new DevService.WebServiceSoapClient();
                var msg = "";
                try
                {
                    switch (args[0])
                    {
                        case "lock":
                            msg=dev.LockFile(args[2].Substring(3), args[1].Substring(1));
                            break;
                        case "unlock":
                            msg = dev.UnlockFile(args[2].Substring(3), args[1].Substring(1));
                            break;
                        case "locked":

                            Application.Run(new Form2() { Username = args[1].Substring(1) });
                            break;
                    }
                    if (msg != "")
                    {
                        throw (new Exception(msg));
                    }
                    if (args[0] != "locked")
                    {
                        new TooltipWindow()
                        {
                            Text = "文件锁定器",
                            ToolTipIcon = ToolTipIcon.Info,
                            Value = "操作成功"
                        }.ShowDialog();
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    new TooltipWindow()
                    {
                        Text = "文件锁定器",
                        ToolTipIcon = ToolTipIcon.Error,
                        Value = "操作未成功：" + ex.Message
                    }.ShowDialog();
                    return 1;
                }
            }
            else
            {
                Application.Run(new Form1());
                return 0;
            }
        }
    }
}
