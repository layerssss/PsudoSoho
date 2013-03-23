using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public class BuyProduct:MFLTransactionOpration
    {
        public string ProductionType;
        public int Balance;
        public int Month;
        
        public override string ToString()
        {
            return string.Format("用户{0}购买旅馆{1}，总价￥{2}.00，时长{3}个月", SubmitUser, MFLAccount.GetProductType(ProductionType), Balance, Month);
        }

        public override void Execute()
        {
            if (Balance > this.user.balance)
            {
                throw (new Exception("您的账户余额不足以完成当前的操作，请充值后重试。"));
            }
            if (Balance == 0 && this.user.MFLUsers_product.Count(tp => tp.type== "free") >= 5)
            {
                throw (new Exception("您已经拥有5个免费旅，不能够再添加免费旅了，请选择添加其他类型的旅馆。"));
            }
            d.MFLUsers_product.AddObject(new MFL.MFLUsers_product()
            {
                descriptions = "",
                due_time = DateTime.Today.AddMonths(this.Month),
                initiate_time = DateTime.Today,
                type = ProductionType,
                user_id = this.user.id
            });
            this.user.balance -= Balance;
        }

        public override bool NeedAudit
        {
            get { return false; }
        }

        public override string Type
        {
            get { return "购买旅馆产品"; }
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(ProductionType);
            writer.Write(Balance);
            writer.Write(Month);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            ProductionType = reader.ReadString();
            Balance = reader.ReadInt32();
            Month = reader.ReadInt32();
        }

        public override void RoleBack()
        {
            throw new NotImplementedException();
        }
    }
}
