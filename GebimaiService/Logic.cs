using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    /// <summary>
    /// 
    /// </summary>
    public class Logic
    {
        internal static order placeOrder(string wbUID, int stockId, int num)
        {
            var d = Global.Entities;
            var n = DateTime.Now;
            var u = d.users.First(tu => tu.authId == wbUID && tu.authProvider == "weibo");
            var s = d.stocks.First(ts => ts.id == stockId);
            if (u.area == null || u.address == null || u.address2 == null)
            {
                throw (new Exception("亲，您还没有在我们这里记录收货地址呢，请登陆" + s.barcode.url + "记录您的收货地址和付款方式。"));
            }
            var tts = getFastest(u.area, u.address, d);
            if (u.alipay == null)
            {
                tts.cur = tts.lastAddress == u.address ? tts.cur.AddMinutes(tts.sender.intervalConnected) : tts.cur.AddMinutes(tts.sender.interval);
                WeiboProvider.SendNotification("有新的待发货订单了", tts.sender.user.id);
            }
            else
            {
                var ar = u.area;
                foreach(var a in d.admins.Where(ta=>ta.area==ar)){
                    WeiboProvider.SendNotification("有新的待付款订单了", a.user.id);
                }
            }
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            var o = new order()
            {
                address1 = u.address,
                address2=u.address2,
                alipay = u.alipay,
                state = u.alipay == null ? 1 : 0,
                stockid = s.id,
                sum = s.price * num,
                timespanid = tts.id,
                time = tts.cur,
                num=num 
            };
            d.orders.InsertOnSubmit(o);
            d.SubmitChanges();
            return o;
        }
        internal static timespan getFastest(string area, string address,gebimaicom d)
        {

            var n = DateTime.Now;
            var tts = d.timespans.Where(ttt =>
                ttt.sender.area == area &&
                ttt.sender.addresses.Contains(address) &&
                ttt.stop > n)
                .AsEnumerable()
                .OrderBy(ttt => ttt.lastAddress == address ? ttt.cur.AddMinutes(ttt.sender.intervalConnected) : ttt.cur.AddMinutes(ttt.sender.interval))
                .FirstOrDefault();

            if (tts == null)
            {
                throw (new Exception("亲，很抱歉，我们现在木有快递员在上班呀，没法给您送啊，伤不起，要不亲你来我们这里工作吧~"));
            }
            if (tts.cur < n)
            {
                tts.cur = n;
            }
            return tts;
        }
        internal static string[] Reply(string text, string wbUID)
        {
            text = text.Trim();
            var title = "";
            var num = 1;
            var d = Global.Entities;
            var user = d.users.FirstOrDefault(tu => tu.authId == wbUID && tu.authProvider == "weibo");
            if (user == null || user.area == null || user.address == null || user.address2 == null)
            {
                barcode b = null;
                if (text.Contains('#'))
                {
                    ParseString(text, out title, out num);
                    b = d.barcodes.FirstOrDefault(tb => tb.title.Contains(title));
                    if (b == null)
                    {

                        return new[] { "亲，您还没有在我们这里记录收货地址呢，请登陆 http://gebimai.com/ 购买以记录您的收货地址和付款方式。" };
                    }
                    else
                    {

                        return new[] { "亲，您要买#" + b.title + "#( " + b.url + " )吗，但是您还没有在我们这里记录收货地址呢，请登陆 " + b.url + " 购买以记录您的收货地址和付款方式。" };
                    }
                }
                else
                {
                    return Reply2(text);
                }
            }
            var ar = user.area;
            if (!text.Contains('#'))
            {//无语法
                var dlg = d.wbdialogs.FirstOrDefault(td => td.wbuid == wbUID);
                if (dlg != null)
                {//含有对话
                    var arr = dlg.data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (text.ToLower() == "ok")
                    {
                        if (arr.Length == 2)
                        {
                            var sid = Int32.Parse(arr[1]);
                            num = Convert.ToInt32(arr[0]);
                            var s = d.stocks.First(ts => ts.id == sid);
                            var o = placeOrder(wbUID, sid, num);
                            d.wbdialogs.DeleteAllOnSubmit(d.wbdialogs.Where(td => td.wbuid == wbUID));
                            d.SubmitChanges();
                            ispJs.WebApplication.SafeDelete("", "*.admin");
                            ispJs.WebApplication.SafeDelete("", "*.sender");
                            ispJs.WebApplication.SafeDelete("", "*.orders");
                            return new[] { o.state == 0 ? "亲，您的订单已经提交，我们会向您的支付宝发出AA付款信息，请确认付款信息中的订单编号后确认付款。付款成功后商品会送出。" : "亲，您的订单已经成功提交，俺预计会在" + o.time + "将商品送达。" };
                        }
                        else
                        {
                            return new[] { "不是吧亲，你还没选择要买哪个商品呢。" };
                        }
                    }
                    else
                    {
                        var id = 0;
                        if (!Int32.TryParse(text, out id))
                        {
                            return Logic.Reply2(text);
                        }
                        if (id < 1 || id > arr.Length)
                        {
                            return new[] { "亲你输入的数字我接受不了……" };
                        }
                        {
                            var sid = Int32.Parse(arr[id]);
                            var s = d.stocks.First(ts => ts.id == sid);
                            num = Convert.ToInt32(arr[0]);
                            dlg.data = num + "|" + sid;
                            d.SubmitChanges();
                            var ttt = getFastest(ar, user.address, d);
                            return new[] { string.Format("亲，你选择了购买{0}( {1} ){2}件，总共{3}元。俺们估计能在{4}送到您上次使用的收货地址，确认请回复ok",
                            s.barcode.title,
                            s.barcode.url,
                            num,
                            (double)(s.price*num)/10,
                            (ttt.lastAddress == user.address ? ttt.cur.AddMinutes(ttt.sender.intervalConnected) : ttt.cur.AddMinutes(ttt.sender.interval)).ToShortTimeString()
                            ) };
                        }
                    }

                }
                else
                {
                    //不含对话
                    return Logic.Reply2(text);
                }
            }
            else
            {//有语法
                try
                {
                    ParseString(text, out title, out num);
                }
                catch
                {
                    return new[] { "亲，你输入的命令俺没看懂啊~~"};
                }
                var ss = d.stocks.Where(ts => ts.area==ar && ts.barcode.title.Contains(title)).Take(5);
                if (!ss.Any())
                {
                    return new[] { "亲，俺找不到您搜索的商品#" + title + "#呀，有木有搞错！" };
                }
                d.wbdialogs.DeleteAllOnSubmit(d.wbdialogs.Where(td => td.wbuid == wbUID));
                d.SubmitChanges();
                var dlg = new wbdialog()
                {
                    wbuid = wbUID
                };
                d.wbdialogs.InsertOnSubmit(dlg);
                dlg.data = num.ToString();
                var str = "";
                var i = 1;
                foreach (var s in ss)
                {
                    str += string.Format("{3}.{0}( {1} )单价：{2}元;",
                        s.barcode.title,
                            s.barcode.url,
                            (double)(s.price * num) / 10,i
                        );
                    i++;
                    dlg.data += '|' + s.id.ToString();
                }
                if (ss.Count() == 1)
                {
                    var ttt = getFastest(ar, user.address, d);
                    str += "回复OK，即可购买以上物品" + num + "件,俺们估计能在" +
                        (ttt.lastAddress == user.address ? ttt.cur.AddMinutes(ttt.sender.intervalConnected) : ttt.cur.AddMinutes(ttt.sender.interval)).ToShortTimeString()
                        +"送到您上次使用的收货地址。";
                    if (text.Contains("给我来"))
                    {
                        d.SubmitChanges();
                        return Reply("ok", wbUID);
                    }
                }
                else
                {
                    str += "请回复序号啊亲。";
                }
                d.SubmitChanges();
                return new[] { str };
            }
        }

        private static string[] Reply2(string text)
        {
            System.IO.File.AppendAllText("/srv/service.gebimai.com/talk" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToLongTimeString() + "R:" + text + "\r\n");
            var d = Global.Entities;
            foreach (var c in d.rebot.OrderBy(tr => tr.sort).ThenBy(tr => tr.keyword))
            {
                if (c.sort >= 100)
                {
                    continue;
                }
                foreach (var kw in c.keyword.Split('|'))
                {
                    if (text.Contains(kw))
                    {
                        System.IO.File.AppendAllText("/srv/service.gebimai.com/talk" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToLongTimeString() + "T:" + c.content + "\r\n");
                        return new[] { c.content + " http://gebimai.com/maila-tua " };
                    }
                }
            }
            System.IO.File.AppendAllText("/srv/service.gebimai.com/talk" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToLongTimeString() + "T:亲你说啥？"+"\r\n");
            return new[] { "亲你说啥？" + " http://gebimai.com/maila-tua " };
        }
        public static ispJs.AccessDeniedException PageNotFoundExp
        {
            get
            {
                return new ispJs.AccessDeniedException("亲，这个页面不存在哦！");
            }
        }
        static void ParseString(string str, out string title, out int num)
        {
            num = 1;
            var f = new ispJs.Utility.StringFetcher(str);
            f.Fetch("#", true);
            title = f.Fetch("#", false);
            f.Position++;
            if (f.Fetch("#", true) != null)
            {
                var str2 = f.Fetch("#", false);
                num = str2num(str2);
                if (num == -1)
                {
                    num = str2num(title);
                    title = str2;
                    if (num == -1)
                    {
                        throw (new Exception(""));
                    }
                }
            }
            title = title.Trim();
        }
        static int str2num(string str)
        {
            str = str.Trim();
            var nc = new[] { "件", "袋", "包", "个", "麻袋", "箱", "盒", "台", "只" };
            foreach (var c in nc)
            {
                str = str.Replace(c, "");
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                return -1;
            }
        }
    }
}