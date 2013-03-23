using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace MFL.Transactions
{
    public abstract class MFLTransactionOpration : MFLTransaction
    {
        #region 抽象方法
        public abstract override string ToString();
        public abstract string Type
        {
            get;
        }
        public abstract void Execute();
        public abstract void Serialize(System.IO.BinaryWriter writer);
        public abstract void Deserialize(System.IO.BinaryReader reader);
        public abstract void RoleBack();
        #endregion
        #region 抽象属性
        public abstract bool NeedAudit
        {
            get;
        }
        #endregion
        public string ExecutionTime
        {
            get
            {
                if (this.mflTC_transaction.executionTime.HasValue)
                {
                    return this.mflTC_transaction.executionTime.Value.ToString();
                }
                return "未执行";
            }
        }
        public bool Pushing
        {
            get
            {
                if (!this.mflTC_transaction.executionTime.HasValue)
                {
                    return this.NeedAudit ?
                        this.mflTC_transaction.auditer != null :
                        true;
                }
                return false;
            }
        }
        private MFLTC_transaction mflTC_transaction;
        private MFLEntities transactionD;
        protected MFLEntities d;

        #region 静态方法
        public static MFLTransactionOpration LoadFrom(MFLTC_transaction src, MFLEntities d)
        {
            var type = System.Type.GetType("MFL.Transactions." + src.type);
            var constructor = type.GetConstructor(new Type[] { });
            var obj = constructor.Invoke(new Object[] { }) as MFLTransactionOpration;
            var reader = new System.IO.BinaryReader(
                new System.IO.MemoryStream(src.data));
            obj.Deserialize(reader);
            reader.Close();
            obj.mflTC_transaction = src;
            obj.transactionD = d;
            obj.d = new MFLEntities();
            return obj;
        }
        #endregion

        public override TransactionStatus Status
        {
            get
            {
                if (this.mflTC_transaction.executionTime.HasValue)
                {
                    return this.mflTC_transaction.error != null ? TransactionStatus.Error : TransactionStatus.Executted;
                }
                return this.NeedAudit ?
                    (this.mflTC_transaction.auditer != null ?
                    TransactionStatus.AudittedWaiting :
                    TransactionStatus.NeedAudit) :
                    TransactionStatus.Waiting;
            }
        }
        public override void Go()
        {
            if (!this.Pushing)
            {
                throw (new Exception("该操作的状态“" + this.Status.ToString() + "”当前不能被执行"));
            }
            try
            {
                this.mflTC_transaction.executionTime = DateTime.Now;
                this.Execute();
                this.d.SaveChanges();
                this.transactionD.SaveChanges();

            }
            catch (Exception ex)
            {
                var errorData = ex.ToString();
                try
                {
                    this.RoleBack();
                    this.d.SaveChanges();
                    errorData += "<br/>回滚成功。<br/>";
                }
                catch(Exception rbex) {
                    errorData += "<br/>回滚时触发异常：<br/>" + rbex.ToString();
                }
                this.mflTC_transaction.error = ex.Message;
                this.mflTC_transaction.errorData = errorData;
                transactionD.SaveChanges();
            }
        }

        public override void Audit(string auditter)
        {
            if (!this.NeedAudit)
            {
                throw (new Exception("该操作不需要审核"));
            }
            this.mflTC_transaction.auditer = auditter;
            this.transactionD.SaveChanges();
        }


        public override string SubmitUser
        {
            get { return this.mflTC_transaction.submitUser; }
        }
        protected MFLUsers_account user
        {
            get
            {
                return this.d.MFLUsers_account.FirstOrDefault(tu => tu.username == this.SubmitUser);
            }
        }
        public override int Id
        {
            get { return this.mflTC_transaction.id; }
        }
        public override string StatusInfo
        {
            get
            {
                return "错误详细信息：<br/>" + this.mflTC_transaction.errorData + "<br/>审核者：" + this.mflTC_transaction.auditer;
            }
        }

        public DateTime SubmitTime {
            get
            {
                return this.mflTC_transaction.submitTime;
            }
        }

        public override string ErrorInfo
        {
            get { return this.mflTC_transaction.error ?? "(无)"; }
        }

        public override void Reject(string rejecter,string reason)
        {
            if (!this.NeedAudit)
            {
                throw (new Exception("该操作不需要审核"));
            }
            this.mflTC_transaction.executionTime = DateTime.Now;
            this.mflTC_transaction.auditer = rejecter;
            this.mflTC_transaction.error = "没有通过审核，未通过原因："+reason;
            this.mflTC_transaction.errorData = "原因" + reason + "<br/>拒绝者:" + rejecter;
            this.transactionD.SaveChanges();
        }
    }
}
