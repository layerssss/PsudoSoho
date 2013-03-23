using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    public class Sender
    {
        public static global::sender Validate(gebimaicom d)
        {
            var u = Auth.Username;
            try
            {
                return d.senders.First(ts => ts.user.username == u );
            }
            catch
            {
                throw (new Exception("没有找到你的送货员资料。"));
            }
        }


        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        [ispJs.Action]
        public void Refresh()
        {
            var d = Global.Entities;
            var s = Validate(d);
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender"); 
        }


        [ispJs.Action]
        public void ResetLastAddress()
        {
            var d = Global.Entities;
            var n = DateTime.Now;
            var s = Validate(d);
            
            var t = s.timespans.FirstOrDefault(tt => tt.stop > n);
            if (t == null)
            {
                throw (new InvalidOperationException());
            }
            t.lastAddress = "";
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");

        }
        /// <summary>
        /// 快递员上班.
        /// </summary>
        /// <param name="stop">计划下班的时间.</param>
        [ispJs.Action]
        public void Activate(string stop)
        {
            var d = Global.Entities;
            var s = Validate(d);
            var n = DateTime.Now;
            var st = DateTime.Parse(DateTime.Today.ToShortDateString() + ' ' + stop.Replace('：',':'));
            if (s.timespans.Any(tt => tt.stop > n))
            {
                throw (new Exception("你当前正在上班"));
            }
            if (st <= n)
            {
                throw (new Exception("亲你输入的这个时间" + st.ToShortTimeString() + "我接受不了呀有木有"));
            }
            d.timespans.InsertOnSubmit(new timespan()
            {
                start=n,
                cur=n,
                lastAddress="",
                senderid=s.id,
                stop = st
            });
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
        }
        /// <summary>
        /// 快递员提前下班.
        /// </summary>
        [ispJs.Action]
        public void Deactivate(string stop)
        {
            var d = Global.Entities;
            var s = Validate(d);
            var n = DateTime.Now;
            var st = DateTime.Parse(DateTime.Today.ToShortDateString() + ' ' + stop.Replace('：', ':'));
            var t = s.timespans.FirstOrDefault(tt => tt.stop > n);
            if (t == null)
            {
                throw (new Exception("亲你输入的这个时间"+st.ToShortTimeString()+"我接受不了呀有木有"));
            }
            if (st <= n)
            {
                throw (new Exception("亲你输入的这个时间" + st.ToShortTimeString() + "我接受不了呀有木有"));
            }
            if (t.orders.Any(to => to.state < 2&&st<to.time))
            {
                throw (new Exception("无法提前下班，因为在该时间之后存在未付款或者待发货的订单"));
            }
            t.stop = st;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
        }
        /// <summary>
        /// 对指定的订单进行反馈.
        /// </summary>
        /// <param name="orderId">订单ID.</param>
        [ispJs.Action]
        public void Feedback(int orderId)
        {
            var d = Global.Entities;
            var s = Validate(d);
            var o = d.orders.FirstOrDefault(to => to.id == orderId);
            if (o == null)
            {
                throw (new Exception("找不到指定订单。"));
            }
            if (o.timespan.senderid != s.id)
            {
                throw (new Exception("该订单不是分配给您的。"));
            }
            if (o.state != 1)
            {
                throw (new InvalidOperationException());
            }
            o.state = 2;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("", "*.admin");
            ispJs.WebApplication.SafeDelete("", "*.sender");
            ispJs.WebApplication.SafeDelete("", "*.orders");
        }




        /// <summary>
        /// Kills the specified man.
        /// </summary>
        /// <param name="man">The man.</param>
        public void Kill(string man)
        {
            
        }
    }
}