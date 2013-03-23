using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace GebimaiService
{
    public class Admin
    {


        public static global::admin Validate(gebimaicom d)
        {
            var u = Auth.Username;
            try
            {
                return d.admins.First(ta => ta.user.username == u);
            }
            catch
            {
                throw (new Exception("没有找到你的管理员资料。"));
            }
        }
        [ispJs.Action]
        public void Refresh()
        {
            var d = Global.Entities;
            var s = Validate(d);
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
        }
        /// <summary>
        /// 确认一个订单的支付宝已经付款.
        /// </summary>
        /// <param name="orderId">The order id.</param>
        [ispJs.Action]
        public void VerifyPayment(int orderId)
        {
            var d = Global.Entities;
            var n=DateTime.Now;
            var ar = Validate(d).area;
            var o = d.orders.FirstOrDefault(to => to.id == orderId);
            if (o == null)
            {
                throw (new Exception("找不到该订单"));
            }
            if (o.state != 0)
            {
                throw (new InvalidOperationException());
            }
            if (o.timespan.cur < n)
            {
                o.timespan.cur = n;
            }
            o.timespan.cur=o.address1==o.timespan.lastAddress?
                o.timespan.cur.AddMinutes(o.timespan.sender.intervalConnected):
                o.timespan.cur.AddMinutes(o.timespan.sender.interval);
            o.time = o.timespan.cur;
            o.timespan.lastAddress = o.address1;
            o.state = 1;
            d.SubmitChanges();
            WeiboProvider.SendNotification("有新的待发货订单了", o.timespan.sender.user.id);
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
        }
        /// <summary>
        /// Notifies the payment.
        /// </summary>
        /// <param name="orderId">The order id.</param>
        [ispJs.Action]
        public void NotifyPayment(int orderId)
        {
            var d = Global.Entities;
            var n = DateTime.Now;
            var ar = Validate(d).area;
            var o = d.orders.FirstOrDefault(to => to.id == orderId);
            if (o == null)
            {
                throw (new Exception("找不到该订单"));
            }
            if (o.state != 0)
            {
                throw (new InvalidOperationException());
            }
            throw (new Exception("这个功能暂时无法实现呀！！！想想办法！"));
        }
        /// <summary>
        /// 增加一个送货员.
        /// </summary>
        /// <param name="weiboName">送货员的微博昵称.</param>
        /// <param name="alias">送货员标号(英文).</param>
        /// <param name="interval">送货员可接受两个不同楼宇之间订单的时间间隔（分钟）.</param>
        /// <param name="intervalConnected">送货员可接受同一个楼宇之间订单的时间间隔（分钟）.</param>
        /// <param name="addresses">The addresses.</param>
        [ispJs.Action]
        public void AddSender(string weiboName,
             string alias,
             int interval,
             int intervalConnected,
            string addresses)
        {
            if (alias.Length < 3)
            {
                throw (new Exception("代号至少有3个英文字母"));
            }
            var d = Global.Entities;
            var ar = Validate(d).area;
            if (d.senders.Any(ts => ts.alias == alias))
            {
                throw (new Exception("该代号已经被占用"));
            }
            var u=d.users.FirstOrDefault(tu=>tu.name==weiboName);
            if(u==null)
            {
                throw (new Exception("找不到该用户"));
            }
            if (u.senders.Any())
            {
                throw (new Exception("该用户已经是送货员了"));
            }
            d.senders.InsertOnSubmit(new global::sender()
            {
                area = ar,
                addresses = addresses,
                alias = alias,
                interval = interval,
                intervalConnected = intervalConnected,
                userid = u.id
            });
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
        }
        /// <summary>
        /// 删除一个送货员
        /// </summary>
        /// <param name="alias">The alias.</param>
        [ispJs.Action]
        public void DelSender(string alias)
        {
            var d = Global.Entities;
            var ar = Validate(d).area;
            var s = d.senders.FirstOrDefault(ts => ts.alias == alias);
            if (s==null)
            {
                throw (new Exception("找不到该送货员"));
            }
            if (s.area!=ar)
            {
                throw (new Exception("您没有权限删除该送货员"));
            }
            if (s.timespans.Any(tt => tt.orders.Any(to => to.state < 2)))
            {
                throw (new Exception("改送货员尚有未完成的订单，无法删除"));
            }
            d.senders.DeleteOnSubmit(s);
            d.SubmitChanges();

            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
        }
        /// <summary>
        /// 【暂不使用】【未实现】增加一种商品.
        /// </summary>
        /// <param name="barcode">商品的包装条形码.</param>
        /// <param name="title">商品名称.</param>
        /// <param name="picNum">商品图片的数目.</param>
        /// <param name="productId">商品种类ID.</param>
        [ispJs.Action]
        public void AddBarcode(
            string barcode,
            string title,
            int picNum,
            int productId)
        {
            throw (new NotImplementedException());
        }
        /// <summary>
        /// 【仅服务器端调用】同步指定产品.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> 则为增加或修改，否则为删除.</param>
        /// <param name="url">产品展示地址</param>
        /// <param name="imgUrl">产品缩略图URL(删除时可以不用指定)</param>
        /// <param name="title">产品标题(删除时可以不用指定)</param>
        [ispJs.Action]
        public void Sync(
            bool enable,
            string url,
            string imgUrl,
            string title)
        {
            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != "106.187.38.100")
            {
                throw (new Exception("Access Denied!"));
            }
            var d = Global.Entities;
            if (enable)
            {
                var b = d.barcodes.FirstOrDefault(tb => tb.productid == 1 && tb.url == url);
                if (b == null)
                {
                    var i = 0;
                    var barcode = "";
                    while (true)
                    {
                        barcode = '1' + i.ToString().PadLeft(12, '0');
                        if (!d.barcodes.Any(tb => tb.bc == barcode))
                        {
                            break;
                        }
                        i++;
                    }
                    b = new barcode()
                    {
                        url = url,
                        bc = barcode,
                        productid = 1,
                        price = 0,
                        num = 1
                    };
                    d.barcodes.InsertOnSubmit(b);
                }
                b.imgUrl = imgUrl;
                b.title = title;
                
            }
            else
            {
                var b = d.barcodes.FirstOrDefault(tb => tb.productid == 1 && tb.url == url);
                if (b == null)
                {
                    throw (new Exception("找不到指定URL对应的产品"));
                }
                d.barcodes.DeleteOnSubmit(b);
                
            }
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
        }


        /// <summary>
        /// 在当前区域将一种商品上架.
        /// </summary>
        /// <param name="barcode">商品条形码.</param>
        /// <param name="url">The URL.</param>
        /// <param name="price">商品价格（角）.</param>
        [ispJs.Action]
        public void AddStock(
            string barcode,
            int price,int importprice)
        {
            var d = Global.Entities;
            var a = Validate(d);
            var ar=a.area;
            var b = d.barcodes.FirstOrDefault(tb => tb.url == barcode);
            if (b == null)
            {
                b = d.barcodes.FirstOrDefault(tb => tb.bc == barcode);
            }
            if (b == null)
            {
                throw (new Exception("找不到该商品"));
            }
            var bc = b.bc;
            var s = d.stocks.FirstOrDefault(ts => ts.area == ar && ts.barcodebc == bc);
            if (s==null)
            {

                s = new stock()
                {
                    area=ar,
                    barcodebc=b.bc,
                    importprice=0,
                    price=0,
                    enabled=false

                };
                d.stocks.InsertOnSubmit(s);
                d.SubmitChanges();
                s = d.stocks.First(ts => ts.area == ar && ts.barcodebc == bc);
            }
            s.importprice = importprice;
            s.price = price;
            s.enabled = true;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
        }
        /// <summary>
        /// 在当前区域将一种商品下架.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="barcode">商品条形码.</param>
        [ispJs.Action]
        public void DelStock(
            string url,
            string barcode)
        {
            var d = Global.Entities;
            var a = Validate(d);
            var ar = a.area;
            var b = d.barcodes.FirstOrDefault(tb => tb.bc == barcode|| tb.url == url);
            if (b == null)
            {
                throw (new Exception("找不到该商品"));
            }
            var s = d.stocks.FirstOrDefault(ts => ts.area == ar && ts.barcodebc == b.bc);
            if (s == null)
            {
                throw (new Exception("该商品未在当前区域上架"));
            }
            if(s.orders.Any(to=>to.state<2)){
                throw (new Exception("该商品仍有未付款或者待发货的订单，不能下架"));
            }
            s.enabled = false;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
        }

        /// <summary>
        /// 在当前区域增加一个楼宇.
        /// </summary>
        /// <param name="address">楼宇名称.</param>
        [ispJs.Action]
        public void AddAddress(string address)
        {

            var d = Global.Entities;
            var ar = Validate(d).area;
            if (d.addresses.Any(ta => ta.area == ar && ta.address1 == address))
            {
                throw (new Exception("该楼宇已经存在"));
            }
            d.addresses.InsertOnSubmit(new address()
            {
                address1 = address,
                area = ar
            });
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
        }
        /// <summary>
        /// 在当前区域删除一个楼宇.
        /// </summary>
        /// <param name="address">楼宇名称.</param>
        [ispJs.Action]
        public void DelAddress(string address)
        {

            var d = Global.Entities;
            var ar = Validate(d).area;
            d.addresses.DeleteAllOnSubmit(d.addresses.Where(ta => ta.area == ar && ta.address1 == address));
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
        }
        /// <summary>
        /// 【单一形式接口】使用户跳转到今天的订单
        /// </summary>
        [ispJs.Action]
        public void JumpOrdersToday()
        {
            var d = Global.Entities;
            var a = Validate(d);
            var n = DateTime.Now;
            ispJs.WebApplication.Response.Redirect(string.Format("/{0}-{1:yyyyMMdd}.orders",
                HttpUtility.UrlEncode( a.alias), n));
        }
    }
}