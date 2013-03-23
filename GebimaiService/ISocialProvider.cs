using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GebimaiService
{
    public interface ISocialProvider
    {
        void Validate(
            string code,
            bool remember,
            out string id,
            out string authId,
            out string authToken,
            out DateTime authTokenExpre,
            out string name,
            out string avatarUrl,
            out string gender,
            out string data);
        void Notify();
        string GetLoginUrl(bool remember);
    }
}
