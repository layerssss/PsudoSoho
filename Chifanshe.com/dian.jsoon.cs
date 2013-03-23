using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chifanshe.com
{
    public class dian: ispJs.IISPRenderer
    {
        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var sta = locals["$subPage"] as string;
            var d = Global.Entity;
            var s = d.stores.First(ts => ts.alias == sta);
            locals["store"] = s;
            locals["foods"] = s.foods.ToArray();
            locals["timespans"] = s.timespans.ToArray();
            locals["stores"] = d.stores.ToArray();
            locals["isOpen"] = s.timespans.AsEnumerable().Any(ts =>
                DateTime.Now > DateTime.Today.AddHours(ts.start.Hour).AddMinutes(ts.start.Minute) &&
                DateTime.Now < DateTime.Today.AddHours(ts.stop.Hour).AddMinutes(ts.stop.Minute));
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