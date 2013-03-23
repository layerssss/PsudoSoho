using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFL.Transactions;
namespace MFL
{
    public class MFLTransactionCenter
    {
        public MFLEntities D;
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
        }
        public MFLTransactionCenter()
        {
            this.D = new MFLEntities(); 
            var a = new MFL.MFLAccount(System.Web.HttpContext.Current);
            if (a.MFLUsers_Account.isAdmin)
            {
                this.username = a.MFLUsers_Account.username;
            }
        }
        public static void SubmitTransaction(MFLTransactionOpration t)
        {
            var a = new MFLAccount(System.Web.HttpContext.Current);
            a.CheckAccount();
            var mflt = new MFLTC_transaction()
            {
                submitUser = a.MFLUsers_Account.username,
                submitTime=DateTime.Now
            };
            convertDataAndType(t, mflt);
            a.D.MFLTC_transaction.AddObject(mflt);
            a.D.SaveChanges();
            System.Web.HttpContext.Current.Response.Redirect("/Account/Wizards/Success.aspx?audit=" + t.NeedAudit);
        }
        public TransactionTable GetPushingTransactions()
        {

            return getTable(this.D.MFLTC_transaction.Where(tt => !tt.executionTime.HasValue)
                .OrderBy(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, this.D))
                .Where(ttd => ttd.Pushing));//
        }
        public static MFLTransaction GetLastPushingTransaction()
        {
            var d = new MFLEntities();
            return d.MFLTC_transaction.Where(tt => !tt.executionTime.HasValue)
                .OrderBy(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, d))
                .FirstOrDefault(tt => tt.Pushing);
        }
        public TransactionTable GetTransactionsOfDay(DateTime day)
        {
            day=day.Date;
            var dayp=day.AddDays(1);
            return getTable(this.D.MFLTC_transaction.Where(tt => tt.submitTime > day && tt.submitTime < dayp)
                .OrderBy(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, this.D)));
        }
        public TransactionTable GetTransactionsOfUser(string user)
        {
            return getTable(this.D.MFLTC_transaction.Where(tt => tt.submitUser.Contains(user??"kezhan"))
                .OrderBy(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, this.D)));
        }
        public TransactionTable GetTransactionsNeedAudit()
        {
            return getTable(this.D.MFLTC_transaction.Where(tt => !tt.executionTime.HasValue)
                .OrderBy(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, this.D))
                .Where(ttd => ttd.Status == MFLTransaction.TransactionStatus.NeedAudit));
        }
        public TransactionTable GetTransaction(int id)
        {

            var a = new MFLAccount(System.Web.HttpContext.Current);
            a.CheckAccount();
            return getTable(D.MFLTC_transaction.Where(tt => tt.submitUser == a.MFLUsers_Account.username && tt.id == id)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, D)));
        }
        public void Audit(int ID)
        {
            var t = MFLTransactionOpration.LoadFrom(this.D.MFLTC_transaction.First(tt => tt.id == ID), this.D);
            t.Audit(this.username);
            this.D.SaveChanges();

        }
        public TransactionTable GetMyTransactions()
        {
            var a = new MFLAccount(System.Web.HttpContext.Current);
            a.CheckAccount();
            return getTable(D.MFLTC_transaction.Where(tt => tt.submitUser == a.MFLUsers_Account.username)
                .OrderByDescending(tt => tt.submitTime)
                .AsEnumerable()
                .Select(tt => MFLTransactionOpration.LoadFrom(tt, D)));
        }
        static TransactionTable getTable(IEnumerable<MFLTransactionOpration> items)
        {
            var dt = new TransactionTable();

            foreach (var item in items)
            {
                dt.Add(item);
            }
            return dt;
            
        }
        static void convertDataAndType(MFLTransactionOpration src, MFLTC_transaction dst)
        {
            dst.type = src.GetType().Name;
            var ms = new System.IO.MemoryStream();
            src.Serialize(new System.IO.BinaryWriter(ms));
            ms.Flush();
            dst.data = ms.ToArray();
            ms.Close();
            
        }

    }
}
