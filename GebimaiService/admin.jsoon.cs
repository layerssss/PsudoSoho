using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class admin : ispJs.IISPRenderer, ispJs.IISPAC
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {

            var d = Global.Entities;
            var a = Admin.Validate(d);
            var ar=a.area;
            var n = DateTime.Now;
            locals["area"] = ar;
            locals["stocks"] = d.stocks.Where(ts => ts.area == ar&&ts.enabled).ToArray();
            locals["senders"] = d.senders.Where(ts => ts.area == ar).ToArray();
            locals["payingOrders"] = d.orders.Where(to => to.timespan.sender.area == ar && to.state == 0).ToArray();
            locals["activeSenders"] = d.timespans.Where(ts => ts.sender.area == ar && ts.stop > n).ToArray();
            locals["addresses"] = d.addresses.Where(ta => ta.area == ar).Select(ta => ta.address1).ToArray();

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
            var d = Global.Entities;
            var a = Admin.Validate(d); 
            if (a.alias != subPage)
            {
                throw (Logic.PageNotFoundExp);
            }
        }

        #endregion
    }
}