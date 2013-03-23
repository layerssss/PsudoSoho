using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        /// <summary>
        /// 以新的地址和支付方式下订单
        /// </summary>
        /// <param name="area">用户所在区域</param>
        /// <param name="address">用户所在楼宇</param>
        /// <param name="address2">用户所在门牌号</param>
        /// <param name="alipay">用于付款的支付宝账号，可以留空，留空的话则为货到付款</param>
        /// <param name="url">商品URL</param>
        /// <param name="num">购买数目</param>
        [ispJs.Action]
        public void PlaceOrderWith(string area, string address, string address2, string alipay, string url, int num, out string redirectUrl)
        {
            alipay = "";
            var d = Global.Entities;
            var uname = Auth.Username;
            if (uname == null)
            {
                throw (new Exception("亲，要登录先啊有木有！")); 
            }
            var u = d.users.First(tu => tu.username == uname);
            if (area == "")
            {
                throw (new Exception("亲你还木有选择区域啊有木有！"));
            }
            if (address == "")
            {
                throw (new Exception("亲你还木有选择地址啊有木有！"));
            }
            if (!Public.Areas.Contains(area))
            {
                throw (new Exception("不支持亲所选择的区域哦("+area+")"));
            }
            if (!d.addresses.Any(ta => ta.area == area && ta.address1 == address))
            {
                throw (new Exception("不支持亲所选择的楼宇哦(" + address + ")"));
            }
            address2 = address2.Trim();
            if (address2 == "")
            {
                throw (new Exception("亲，必须填写房间号哦"));
            }
            var b = d.barcodes.FirstOrDefault(tb => tb.url == url);
            if (b == null)
            {
                throw (new Exception("亲，找不到该商品……"));
            }
            if (!b.stocks.Any(ts => ts.area == area))
            {
                throw (new Exception("亲，"+area+"当前没有该商品的库存了……"));
            }
            u.area = area;
            u.address = address;
            u.address2 = address2;
            alipay = alipay.Trim();
            u.alipay = alipay == "" ? null : alipay;
            d.SubmitChanges();
            PlaceOrder(url, num, out redirectUrl);
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
        }
        /// <summary>
        /// 下订单
        /// </summary>
        /// <param name="url">商品URL</param>
        /// <param name="num">购买数目</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        [ispJs.Action]
        public void PlaceOrder(string url, int num, out string redirectUrl)
        {

            var uname = Auth.Username;
            if (uname == null)
            {
                throw (new Exception("亲，要登录先啊有木有！"));
            }
            var d = Global.Entities;
            var u = d.users.First(tu => tu.username == uname);
            if (u.area == null)
            {
                throw (new Exception("亲，您还没有填写您的收货地址和付款方式……"));
            }
            var area = u.area;
            var b = d.barcodes.FirstOrDefault(tb => tb.url == url);
            if (b == null)
            {
                throw (new Exception("亲，找不到该商品……"));
            }
            if (!b.stocks.Any(ts => ts.area == area))
            {
                throw (new Exception("亲，" + area + "当前没有该商品的库存了……"));
            }
            try
            {
                redirectUrl = WeiboProvider.PostStatus(u.id, b.title, b.imgUrl, b.url, num);
            }
            catch (Exception ex)
            {
                throw (new Exception("亲，微博发布失败……" + ex.Message));
            }
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
        }
        /// <summary>
        /// Dismisses the message.
        /// </summary>
        [ispJs.Action]
        public void DismissMessage()
        {
            var uname = Auth.Username;
            if (uname == null)
            {
                throw (new Exception("亲，要登录先啊有木有！"));
            }

            var d = Global.Entities;
            var u = d.users.First(tu => tu.username == uname);
            u.message = null;
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
            d.SubmitChanges();
        }

    }
}