using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFL
{
    class common
    {

        public  static string MD5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = get_md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            get_md5.Dispose();
            return ByteArrayToHexString(bytes, 0, 16);
        }
        public static string ByteArrayToHexString(byte[] buf, int offset, int len)
        {
            len += offset;
            var sb = new StringBuilder();
            for (int i = offset; i < len; i++)
            {
                sb.Append(buf[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
