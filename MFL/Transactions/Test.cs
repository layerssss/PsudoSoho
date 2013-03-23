using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public class Test:MFLTransactionOpration
    {
        public override void Execute()
        {
            Console.WriteLine("Executed:" + this.A);
        }
        public string A;

        public override bool NeedAudit
        {
            get { return false; }
        }

        public override string ToString()
        {
            return this.A;
        }

        public override string Type
        {
            get { return "测试"; }
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(A);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            A = reader.ReadString();
        }

        public override void RoleBack()
        {
            throw new NotImplementedException();
        }
    }
}
