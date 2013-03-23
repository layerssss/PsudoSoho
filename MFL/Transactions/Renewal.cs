using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public class Renewal:MFLTransactionOpration
    {
        public override string ToString()
        {
            return string.Format("用户{0}给产品ID{1}续费个{2}月，总价格￥{3}.00", this.SubmitUser, this.ProductId, this.Month, this.Balance);
        }

        public override string Type
        {
            get { return "旅馆产品续费"; }
        }
        public int ProductId;
        public int Balance;
        public int Month;
        public override void Execute()
        {
            MFLAccount.CheckBalance(this.user, this.Balance);
            var p = this.user.MFLUsers_product.First(tp => tp.id == ProductId);
            this.user.balance -= this.Balance;
            if (p.due_time < DateTime.Today||this.Balance==0)
            {
                p.due_time = DateTime.Today;
            }
            p.due_time = p.due_time.AddMonths(this.Month);
            this.d.SaveChanges();
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(ProductId);
            writer.Write(Balance);
            writer.Write(Month);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {

            this.ProductId = reader.ReadInt32();
            this.Balance = reader.ReadInt32();
            this.Month = reader.ReadInt32();
        }

        public override bool NeedAudit
        {
            get { return false; }
        }

        public override void RoleBack()
        {
            throw new NotImplementedException();
        }
    }
}
