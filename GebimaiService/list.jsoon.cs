using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class list:ispJs.IISPRenderer
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d=Global.Entities;
            var ar = Public.Areas[Convert.ToInt32(locals["$subPage"] as string)];
            locals["area"] = ar;
            var di=new Dictionary<string,string[]>();
            foreach(var s in d.stocks.Where(ts => ts.area == ar)){
                var str = s.price.ToString();
                di[s.barcode.title] = new[] { (str.Length == 1 ? "0" : str.Remove(str.Length - 1)) + '.' + str.Last() + "0", s.barcode.url };
            }
            locals["stocks"] = di;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}