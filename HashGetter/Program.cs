using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            LCmd.CmdLineHandler.SafeHandleArgs(typeof(Program).GetMethod("GetHash"), null, args);
        }
        public static void GetHash(
            string resource = "",
            string format = "{0}"
           )
        {
            Console.Write(format, GCServiceBase.Validator.GetHash(resource));
        }
    }
}
