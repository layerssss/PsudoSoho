using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFLJson;
using System.Web;
namespace MFL
{
    public class MFLAccount
    {
        private System.Web.HttpContext context;
        public MFLEntities D;
        public MFLAccount(System.Web.HttpContext context)
        {
            this.context = context;
            this.D = new MFLEntities();
            if (this.context.Request.Cookies["MFLAUTHX"] != null)
            {
                try
                {
                    var authx = this.context.Request.Cookies["MFLAUTHX"].Value.Split('|');
                    var salt = SharedConfig.PasswordSalt;
                    var uid = authx[0];
                    this.MFLUsers_Account = D.MFLUsers_account.First(ta => ta.username == uid);
                    if (common.MD5(salt+DateTime.Today.ToShortDateString() + this.MFLUsers_Account.password) != authx[1])
                    {
                        throw (new Exception());
                    }
                }
                catch
                {
                    this.context.Response.SetCookie(new System.Web.HttpCookie("MFLAUTHX")
                    {
                        Expires = DateTime.Now.AddDays(-1),
                        Domain = SharedConfig.CookieDomain
                    });
                    this.MFLUsers_Account = null;
                }
                
            }
        }
        public MFLUsers_account MFLUsers_Account;
        public void CheckAccount()
        {
            if (this.MFLUsers_Account == null)
            {
                context.Response.Redirect(SharedConfig.MFLBaseUrl + "Membership/Login.aspx?ReturnUrl=" +
                    context.Server.UrlEncode(context.Request.Url.ToString()));
            }
        }
        public IEnumerable<MFL_lodge> GetLodges()
        {
            CheckAccount();
            return this.MFLUsers_Account.MFLUsers_product.SelectMany(tp=>tp.MFL_lodge);
        }
        public void Login(string username, string password)
        {
            var acc = D.MFLUsers_account.FirstOrDefault(ta => ta.username == username && ta.password == password);
            if (acc == null)
            {
                throw (new Exception("登陆失败，请检查用户名和密码。"));
            }
            this.MFLUsers_Account = acc;
            var salt = SharedConfig.PasswordSalt;
            this.context.Response.SetCookie(new HttpCookie("MFLAUTHX")
            {
                Domain = SharedConfig.CookieDomain,
                Value = acc.username + "|" + common.MD5(salt + DateTime.Today.ToShortDateString() + this.MFLUsers_Account.password)
            });
        }
        public void Logout()
        {
            this.MFLUsers_Account = null;
            this.context.Response.SetCookie(new System.Web.HttpCookie("MFLAUTHX")
            {
                Expires = DateTime.Now.AddDays(-1),
                Domain = MFL.SharedConfig.CookieDomain
            });
        }
        public static string GetProductType(string typeStr)
        {
            switch (typeStr)
            {
                case "free":
                    return "迷你旅";
                case "premium":
                    return "白金旅";
                case "ultimate":
                    return "超级连锁旅";
            }
            return "特殊账户：" + typeStr;
        }
        public static long GetProductCapPhoto(string typeStr)
        {
            var p = Transactions.TransactionData.OnlyInstance.Value.Products.Value;
            return (p.ContainsKey(typeStr) ? p[typeStr].CapPhoto : 10) * 1024 * 1024;
        }
        public static Dictionary<int,int> GetPrize(string typeStr)
        {
            var p = Transactions.TransactionData.OnlyInstance.Value.Products.Value;
            return p.ContainsKey(typeStr) ? p[typeStr].Prizes : new Dictionary<int, int>();
        }

        private static List<char> letters;
        public static void ValidateLodgeIdent(string ident)
        {
            var d=new MFLEntities();
            if (ident.Length < 6)
            {
                throw (new Exception("旅馆标识的最小长度为6。"));
            }
            if (letters == null)
            {
                letters = new List<char>();
                for (var c = 'a'; c <= 'z'; c++)
                {
                    letters.Add(c);
                }
                for (var c = 'A'; c <= 'Z'; c++)
                {
                    letters.Add(c);
                }
            }
            if (!ident.All(tc => letters.Contains(tc) || char.IsDigit(tc) || tc == '_'))
            {
                throw (new Exception("旅馆标识只能使用英文字母、数字及下划线。"));
            }
            if (d.MFL_lodge.Any(tl => tl.ident.ToLower() == ident.ToLower()))
            {
                throw (new Exception("该旅馆标识（" + ident + "）已经被占用，请更换。"));
            }
        }

        public void Register(string username, string password)
        {
            var email = new System.Text.RegularExpressions.Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            
            if (letters == null)
            {
                letters = new List<char>();
                for (var c = 'a'; c <= 'z'; c++)
                {
                    letters.Add(c);
                }
            }
            if(!email.IsMatch(username)){
                throw(new Exception("电子邮件地址不符合格式。"));
            }
            if (password.Length < 6)
            {
                throw (new Exception("密码的最小长度为6。"));
            }
            if (!password.All(tc => letters.Contains(tc) || char.IsDigit(tc) || tc == '_'))
            {
                throw (new Exception("密码只能使用英文字母、数字及下划线。"));
            }
            this.D.MFLUsers_account.AddObject(new MFLUsers_account()
            {
                balance = 0,
                descriptions = "",
                isAdmin = false,
                password = password,
                username = username,
                pCity = "",
                pName = "",
                pProvince = "",
                pSex = true,
                pTraffic = ""
            });
            this.D.SaveChanges();
            this.Login(username, password);
        }

        public static void CheckBalance(MFLUsers_account user,int prize)
        {
            if (user.balance < prize)
            {
                throw (new Exception(string.Format("您的余额({0})已不足，不足以完成当前操作(需要{1})。", user.balance, prize)));
            }
        }
    }
}
