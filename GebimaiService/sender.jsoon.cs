using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class sender : ispJs.IISPRenderer,ispJs.IISPAC
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d=Global.Entities;
            var s = Sender.Validate(d);
            var n=DateTime.Now;

            var tsp=s.timespans.FirstOrDefault(tt => tt.stop > n);
            locals.Add("area", s.area);
            locals.Add("addresses", s.addresses);
            locals.Add("activeInfo", tsp);
            locals.Add("orders", d.orders.Where(to => to.timespan.sender.id == s.id && to.state == 1).ToArray());

            locals["activeSenders"] = d.timespans.Where(ts => ts.sender.area == s.area && ts.stop > n).ToArray(); ;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
        }

        #endregion

        #region IISPAC 成员

        public void Page_Read(string subPage)
        {
            var d = Global.Entities;
            var s = Sender.Validate(d);
            if (s.alias != subPage)
            {
                throw (Logic.PageNotFoundExp);
            }
        }

        #endregion
    }
}