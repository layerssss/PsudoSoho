using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{

    public class TransactionTable : System.Data.DataTable
    {
        private System.Data.DataColumn CSubmitTime;
        private System.Data.DataColumn CExecutionTime;
        private System.Data.DataColumn CDescription;
        private System.Data.DataColumn CType;
        private System.Data.DataColumn CId;
        private System.Data.DataColumn CSubmitter;
        private System.Data.DataColumn CStatus;
        private System.Data.DataColumn CSInfo;
        private System.Data.DataColumn CErrorInfo;
        public TransactionTable()
        {
            var dt = this;
            this.CId = dt.Columns.Add("ID");
            this.CType = dt.Columns.Add("操作类型");
            this.CSubmitter = dt.Columns.Add("操作发起用户");
            this.CStatus = dt.Columns.Add("操作状态");
            this.CSInfo = dt.Columns.Add("操作状态信息");
            this.CSubmitTime = dt.Columns.Add("提交时间");
            this.CExecutionTime = dt.Columns.Add("执行时间");
            this.CDescription = dt.Columns.Add("操作描述");
            this.CErrorInfo = dt.Columns.Add("错误原因");
            dt.PrimaryKey = new[] { CId };
        }
        public void Add(MFLTransactionOpration item)
        {
            var r = this.NewRow();
            r[CId] = item.Id;
            r[CType] = item.Type;
            r[CSubmitter] = item.SubmitUser;
            r[CStatus] = item.StatusDescription;
            r[CSInfo] = item.StatusInfo;
            r[CSubmitTime] = item.SubmitTime;
            r[CExecutionTime] = item.ExecutionTime;
            r[CDescription] = item.ToString();
            r[CErrorInfo] = item.ErrorInfo;
            this.Rows.Add(r);
        }
    }
}
