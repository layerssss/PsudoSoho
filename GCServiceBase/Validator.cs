using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCServiceBase
{
    public class Validator
    {
        public static void Validate(string hashString,string resourse="")
        {
            if(!TryValidate(hashString,resourse)){
                throw (new Exception("HASHWRONG"));
            }
        }

        public static bool TryValidate(string hashString, string resourse = "")
        {
            return (hashString??"").ToLower() == GetHash(resourse);
        }
        public static string GetHash(string resourse="")
        {
            return MD5(DateTime.UtcNow.DayOfYear + System.Configuration.ConfigurationManager.AppSettings["SharedSecret"] + resourse);
        }
        public static string MD5(string str)
        {
            var md5=System.Security.Cryptography.MD5.Create();
            var bytes=md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            return ByteArrayToHexString(bytes, 0, 16);

        }
        static string ByteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new System.Text.StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
