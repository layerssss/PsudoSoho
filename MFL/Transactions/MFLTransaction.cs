using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public abstract class MFLTransaction
    {
        public abstract void Go();
        public abstract void Audit(string auditter);
        public abstract void Reject(string rejecter,string reason);
        public abstract override string ToString();
        public abstract TransactionStatus Status
        {
            get;
        }
        public abstract int Id
        {
            get;
        }
        public abstract string SubmitUser
        {
            get;
        }
        public string StatusDescription
        {
            get
            {
                switch (this.Status)
                {
                    case TransactionStatus.AudittedWaiting:
                        return "已审核通过，正在执行";
                    case TransactionStatus.Error:
                        return "执行失败";
                    case TransactionStatus.Executted:
                        return "已执行成功";
                    case TransactionStatus.NeedAudit:
                        return "等待被审核";
                    case TransactionStatus.Waiting:
                        return "正在执行";
                }
                return "未知状态（" + this.Status.ToString() + "）";
            }
        }
        public abstract string StatusInfo
        {
            get;
        }
        public abstract string ErrorInfo
        {
            get;
        }
        public enum TransactionStatus
        {
            NeedAudit,
            AudittedWaiting,
            Waiting,
            Executted,
            Error
        }
    }
}
