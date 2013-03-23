using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL.Transactions
{
    public static class ServiceDelegation
    {
        public static class Admin{
            public static Action<string> ActiveLodge;
            public static Action<string> DeactiveLodge;
        }
        public static class Lodge
        {
            public static Action<string> DeleteFile;
            public static Action<string> DeleteFolder;
        }
    }
}
