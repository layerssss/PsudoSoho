using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public class DeactiveProduct:MFLTransactionOpration
    {
        public override string ToString()
        {
            return string.Format("用户{0}反激活产品{1}", this.SubmitUser,ProductId);
        }

        public override string Type
        {
            get { return "反激活旅馆产品"; }
        }
        public int ProductId;
        public override void Execute()
        {
            var l = this.user.MFLUsers_product.First(tp => tp.id == ProductId).MFL_lodge.First();
            var lodgeName = l.ident;
            this.d.MFL_lodge.DeleteObject(l);
            ServiceDelegation.Lodge.DeleteFolder("~/" + lodgeName);
            ServiceDelegation.Admin.DeactiveLodge( lodgeName);
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(ProductId);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            this.ProductId = reader.ReadInt32();
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
