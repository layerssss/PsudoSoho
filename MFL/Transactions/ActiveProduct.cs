using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public class ActiveProduct:MFLTransactionOpration
    {
        public override string ToString()
        {
            return string.Format("用户{0}激活旅馆{1}", this.SubmitUser, this.LodgeName);
        }

        public override string Type
        {
            get { return "激活旅馆产品"; }
        }
        public int ProductId;
        public string LodgeName;
        public override void Execute()
        {

            var p = this.user.MFLUsers_product.First(tp => tp.id == ProductId);
            if (p.MFL_lodge.Any())
            {
                throw (new Exception("该旅馆已经激活"));
            }            
            MFLAccount.ValidateLodgeIdent(LodgeName);
            try
            {
                ServiceDelegation.Lodge.DeleteFile("/" + LodgeName);
                ServiceDelegation.Admin.ActiveLodge(LodgeName);
            }
            catch
            {
                throw (new Exception(string.Format("该旅馆标识属于保留关键字，请换用另一个标识重试。")));
            }
            this.d.MFL_lodge.AddObject(new MFL_lodge()
            {
                capPhoto = MFLAccount.GetProductCapPhoto(p.type),
                enabled = true,
                avaPhoto = MFLAccount.GetProductCapPhoto(p.type),
                ident = LodgeName,
                product_id = ProductId,
                MFLUsers_account_id = this.user.id,
                adminPwd = "xzvjkclnadm,afndsqkrheiryqhbfka",
                staHistory = 0,
                staToday = 0,
                staTodayDate = DateTime.Today
            });
            d.SaveChanges();
            var lodge = new MFLLodge("!NaN", LodgeName);
            foreach (var attr in TransactionData.OnlyInstance.Value.LodgeInitializationAttributes.Value)
            {
                var aid = lodge.NewAttribute(attr.Name, string.Format("background-position:-{0}px -96px;", attr.IconIndex * 32));
                foreach (var option in attr.Options)
                {
                    lodge.NewOption(aid, option, "");
                }
            }
        }


        public override bool NeedAudit
        {
            get { return false; }
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(ProductId);
            writer.Write(LodgeName);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            this.ProductId = reader.ReadInt32();
            this.LodgeName = reader.ReadString();
        }

        public override void RoleBack()
        {
            throw new NotImplementedException();
        }
    }
}
