using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace YupooAPI.NET.Abstract
{
    public abstract class YRequest
    {
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        protected string urlBase;
        private string sharedSecret;
        public string SharedSecret
        {
            set
            {
                this.sharedSecret = value;
            }
        }
        protected string makeQuery()
        {
            var hashStr = sharedSecret;
            foreach (var key in Params.Keys.OrderBy(ts=>ts))
            {
                hashStr += key;
                hashStr += this.Params[key];
            }
            var query = new StringBuilder("");
            foreach (var key in Params.Keys)
            {
                query.Append(key);
                query.Append('=');
                query.Append(HttpUtility.UrlEncode(Params[key]));
                query.Append('&');
            }
            query.Append("api_sig=");
            query.Append(MD5(hashStr).ToLower());
            return query.ToString();
        }
        public static string MD5(string str)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
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
        abstract public void MakeResponse(YResponse response);
        public string UrlBase
        {
            set
            {
                this.urlBase = value.TrimEnd('/') + '/';
            }
        }
    }
}
