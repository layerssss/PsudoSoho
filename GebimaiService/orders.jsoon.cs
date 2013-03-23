using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class orders:ispJs
        .IISPAC,ispJs.IISPRenderer
    {
        #region IISPAC 成员

        public void Page_Read(string subPage)
        {
            var d=Global.Entities;
            var arr = subPage.Split('-');
            var ar = arr[0];
            var a = Admin.Validate(d);
            if (a.alias != ar)
            {
                throw (new ispJs.AccessDeniedException("没有权限"));
            }
        }

        #endregion

        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var subPage = (string)locals["$subPage"];
            var d = Global.Entities;
            var arr = subPage.Split('-');
            var alia = arr[0];
            var ar = Admin.Validate(d).area;
            var a = Admin.Validate(d);
            var date = new DateTime(Convert.ToInt32(arr[1].Substring(0, 4)),
                Convert.ToInt32(arr[1].Substring(4, 2)),
                Convert.ToInt32(arr[1].Substring(6, 2))).Date;
            var ndate=date.AddDays(1);
            locals["orders"] = d.orders.Where(to => to.timespan.sender.area == ar
                && to.time < ndate && to.time > date).ToArray();
            locals["date"] = arr[1];
            locals["area"] = ar;
            locals["alias"] = alia;
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