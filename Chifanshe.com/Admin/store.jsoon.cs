using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chifanshe.com.Admin
{
    public class store:ispJs.IISPRenderer,ispJs.IISPAC
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {

            var sta = Convert.ToInt32(locals["$subPage"] as string);
            var d = Global.Entity;
            var s = d.stores.First(ts => ts.id == sta);
            locals["store"] = s;
            locals["foods"] = s.foods.ToArray();
            locals["timespans"] = s.timespans.ToArray();
            locals["stores"] = d.stores.ToArray();
            var orders=new Dictionary<string,cfs.order[]>();
            foreach(var g in d.orders.Where(to => to.store.id == sta).AsEnumerable().GroupBy(to=>to.time.Date)){
                orders.Add(g.Key.ToShortDateString(), g.ToArray());   
            }


            locals["orders"] = orders;
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