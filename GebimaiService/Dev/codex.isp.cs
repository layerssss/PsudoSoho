using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService._Dev
{
    public class codex:ispJs.IISPAC,ispJs.IISPRenderer
    {
        #region IISPAC 成员

        public void Page_Read(string subPage)
        {
            var username = Auth.Username;
            if (username == null)
            {
                throw (new ispJs.AccessDeniedException("亲你还没登陆！"));
            }
            if (!Global.Entities.users.First(tu => tu.username == username).dev.Any())
            {
                throw (new ispJs.AccessDeniedException("亲，这个页面不存在哦！"));
            }
            var d = Global.Entities;
            var cid = Convert.ToInt32(subPage);
            var c = d.rebot.First(tr => tr.id == cid);
            c.lockdev = d.dev.First(td => td.user.username == username).alias;
            c.locktime = DateTime.Now;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("Dev/" + subPage + ".codex");
        }

        #endregion

        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d = Global.Entities;
            var username = Auth.Username;
            var cid=Convert.ToInt32(locals["$subPage"] as string);
            var c = d.rebot.First(tr => tr.id == cid);
            locals["c"] = c;
            c.lockdev = d.dev.First(td => td.user.username == username).alias;
            c.locktime = DateTime.Now;
            d.SubmitChanges();

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