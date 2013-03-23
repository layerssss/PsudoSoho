using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chifanshe.com.Admin
{
    public class Stores:ispJs.IISPRenderer,ispJs.IISPAC
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d = Global.Entity;
            locals["stores"] = d.stores.ToArray();
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IISPAC 成员

        public void Page_Read(string subPage)
        {
            Auth.ValidateAdmin();
        }

        #endregion
    }
}