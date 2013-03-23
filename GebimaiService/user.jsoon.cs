using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class user:ispJs.IISPRenderer,ispJs.IISPAC
    {

        #region IISPRenderer 成员

        public void Page_Load(Dictionary<string, object> locals)
        {
            var d = Global.Entities;
            var username = locals["$subPage"] as string;
            var u=d.users.First(tu => tu.username == username);
            locals.Add("userInfo", u);
            locals.Add("adminInfo", u.admins.FirstOrDefault());
            locals.Add("senderInfo", u.senders.FirstOrDefault());
            locals.Add("devInfo", u.dev.FirstOrDefault());
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
            if (subPage != Auth.Username)
            {
                new _Dev.Devs().Page_Read(subPage);
            }
        }

        #endregion
    }
}