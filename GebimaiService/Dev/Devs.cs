using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService._Dev
{
    public class Devs:ispJs.IISPRenderer,ispJs.IISPAC
    {
        #region IISPAC 成员

        public void Page_Read(string subPage)
        {

            var username= Auth.Username;
            if (username == null)
            {
                throw (new ispJs.AccessDeniedException("亲你还没登陆！"));
            }
            if (!Global.Entities.users.First(tu => tu.username == username).dev.Any())
            {
                throw (new ispJs.AccessDeniedException("亲，这个页面不存在哦！"));
            }
            ispJs.WebApplication.SafeDelete("Dev/Default.htm");
        }

        #endregion

        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d=Global.Entities;
            locals["codex"] = d.rebot.OrderBy(tr=>tr.sort).ThenBy(tr=>tr.keyword).ToArray();
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