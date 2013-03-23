using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YupooAPI.NET
{
    class yRedirectRequest:Abstract.YRequest
    {
        public override void MakeResponse(Abstract.YResponse response)
        {
            throw new NotImplementedException();
        }
        public string GetRedirectUrl(string absoluteUrl)
        {

            var url = this.urlBase + absoluteUrl;
            return url + "?" + this.makeQuery();
        }
    }
}
