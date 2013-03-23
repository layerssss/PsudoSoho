using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class Public
    {
        /// <summary>
        /// 获取所有可用的区域列表.
        /// </summary>
        /// <param name="areas">区域列表.</param>
        [ispJs.Action]
        public void GetAreas(out string[] areas)
        {
            areas = Areas;
        }
        public static string[] Areas=new[] { "暨南大学珠海校区","暨南大学广州本部" };
        /// <summary>
        /// 获取一个区域可用的楼宇列表.
        /// </summary>
        /// <param name="area">当前区域名.</param>
        /// <param name="addresses">楼宇列表.</param>
        [ispJs.Action]
        public void GetAddresses(string area, out string[] addresses)
        {
            addresses = Global.Entities.addresses.Where(ta => ta.area == area).Select(ta => ta.address1).ToArray();
            if (!addresses.Any())
            {
                throw (new Exception("目前不支持该区域"));
            }
        }
        [ispJs.Action]
        public void GetPrices(string url, out Dictionary<string, StockInfo> prices,out string url2,out string time)
        {
            url2 = url;
            prices=new Dictionary<string,StockInfo>();
            var d=Global.Entities;
            var n = DateTime.Now;
            
            var origint=n.AddDays(1);
            var t = origint;
            foreach (var area in Areas)
            {
                var s = d.stocks.FirstOrDefault(ts => ts.area == area && ts.barcode.url == url&&ts.enabled);
                if (s == null)
                {
                    prices[area] = new StockInfo()
                    {
                        available = false,
                        price = 0,
                        time = "暂时无法送达"
                    };
                }
                else
                {
                    
                    var t2 = d.timespans.Where(ts => ts.sender.area == area && ts.stop > n)
                        .AsEnumerable().Select(ts => (ts.cur < n ? n : ts.cur).AddMinutes(ts.sender.interval)).OrderBy(tt => tt).FirstOrDefault();
                    prices[area] = new StockInfo()
                    {
                        available = true,
                        price = s.price,
                        time="暂时无法送达"
                    };
                    if (t2 != default(DateTime))
                    {
                        t = t > t2 ? t2 : t;
                        prices[area].time = "现在买最快" + t2.ToShortTimeString() + "送达";
                    }
                }
            }
            time = (t == origint) ? "暂时无法送达" : "现在买最快" + t.ToShortTimeString()+"送达";
        }
        public class StockInfo
        {
            public bool available;
            public int price;
            public string priceString
            {
                get
                {
                    var str=this.price.ToString();
                    return (str.Length==1?"0": str.Remove(str.Length - 1)) + '.' + str.Last()+"0";
                }
            }
            public string time;
        }
    }
}