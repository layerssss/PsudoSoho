using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using MFL.Transactions;
namespace MyFamilyLodge
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            {
                ServiceDelegation.Lodge.DeleteFile = (path) =>
                {
                    var lodge = new LodgeService.WebServiceSoapClient();
                    lodge.DeleteFile(GCServiceBase.Validator.GetHash(), path);
                };
                ServiceDelegation.Lodge.DeleteFolder = (path) =>
                {
                    var lodge = new LodgeService.WebServiceSoapClient();
                    lodge.DeleteFolder(GCServiceBase.Validator.GetHash(), path);
                };
                ServiceDelegation.Admin.ActiveLodge = (lodgeName) =>
                {
                    var admin = new AdminService.WebServiceSoapClient();
                    admin.ActiveLodge(GCServiceBase.Validator.GetHash(), lodgeName);
                }; 
                ServiceDelegation.Admin.DeactiveLodge = (lodgeName) =>
                {
                    var admin = new AdminService.WebServiceSoapClient();
                    admin.DeactiveLodge(GCServiceBase.Validator.GetHash(), lodgeName);
                };



            }
            var root=Server.MapPath("~/");
            TransactionData.OnlyInstance = new Lazy<TransactionData>(() =>
            {
                return new TransactionData(root + "App_Data\\TransactionData\\");
            }, LazyThreadSafetyMode.ExecutionAndPublication);
            transactionLocker = new object();
            transactionThread = new Thread(transactionThreadStart);
            transactionThread.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            transactionThreadStopping = true;
            do
            {
                Thread.Sleep(100);
            } while (transactionThreadStopping);
        }
        public static void CancelTransaction(int id)
        {
            lock (transactionLocker)
            {
                if (Global.transactionRunning != null && Global.transactionRunning.Id == id)
                {
                    throw (new Exception("抱歉，该操作当前正在执行中，无法进行取消。"));
                }
                var a = new MFL.MFLAccount(HttpContext.Current);
                a.CheckAccount();
                var t = a.D.MFLTC_transaction.First(tt => tt.id == id && tt.submitUser == a.MFLUsers_Account.username);
                if (t.executionTime.HasValue)
                {
                    throw (new Exception("抱歉，该操作已经被执行，不能进行取消。"));
                }
                a.D.MFLTC_transaction.DeleteObject(t);
                a.D.SaveChanges();
            }
        }
        static MFL.Transactions.MFLTransaction transactionRunning;
        static object transactionLocker;
        static Thread transactionThread;
        static bool transactionThreadStopping;
        static void transactionThreadStart()
        {
            transactionThreadStopping = false;
            while (!transactionThreadStopping)
            {
                lock (transactionLocker)
                {
                    try
                    {
                        transactionRunning = MFL.MFLTransactionCenter.GetLastPushingTransaction();
                    }
                    catch
                    {
                        transactionRunning = null;
                    }
                }
                if (transactionRunning != null)
                {
                    transactionRunning.Go();
                    lock (transactionLocker)
                    {
                        transactionRunning = null;
                    }
                }
                Thread.Sleep(5000);
            }
            transactionThreadStopping = false;
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

    }
}