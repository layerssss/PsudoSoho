using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.todaynic.ScpClient;
namespace Chifanshe.com
{
    public class Actions
    {
        [ispJs.Action]
        public void SubmitOrder(out int id, int sid, string data, string addNote, string building, string phNo)
        {
            //throw (new Exception("不要点啊亲，我们现在还木有上线啊，亲，关注我们微博随时掌握上线动态啊有木有啊亲！！！"));
            id = 0;

            
            var d = Global.Entity;
            var s = d.stores.First(ts => ts.id == sid);
            if(!s.timespans.AsEnumerable().Any(ts=>
                DateTime.Now>DateTime.Today.AddHours(ts.start.Hour).AddMinutes(ts.start.Minute)&&
                DateTime.Now<DateTime.Today.AddHours(ts.stop.Hour).AddMinutes(ts.stop.Minute))){
                throw(new Exception("亲，我们打烊了啊！"));
            }
            var o = new cfs.order()
            {
                address=building,
                ip=ispJs.WebApplication.Request.ServerVariables["REMOTE_ADDR"],
                num=0,
                phone=phNo,
                storeId=sid,
                time=DateTime.Now,
                content=""
            };

            var arr=data.Split(new[] { '|' });
            var total = 0;
            for (var i = 0; i+1 < arr.Length; i++)
            {
                var fid=Convert.ToInt32(arr[i]);
                var option=arr[++i];
                var num = Convert.ToInt32(arr[++i]);
                var f = d.foods.First(tf => tf.id == fid && tf.storeId == sid);
                o.num += num;
                o.content += (f.title.Length > 4 ? f.title.Remove(4) + "…" : f.title)
                    + option + "x" + num + ";";
                total += f.price * num;
            }
            o.content += "共" +  ((double)total/10).ToString("N1") + "元;";
            o.content += "到"+o.address+";";
            o.content += "电" + o.phone + ";";
            o.content += addNote;
            o.content += "-吃饭社-";
            if (total <s.pricelimit*10||o.address.Length<3||o.phone.Any(tc=>!char.IsNumber(tc)&&tc!=' '&&tc!='-')||o.phone.Length<6)
            {
                throw (new Exception("亲，你输入的电话号码格式不正确！"));
            }
            try
            {
                var c = new SMSClient("sms.todaynic.com", 20002, "ms32657", "mtqwnd");
                if (!c.sendSMS(s.phone, o.content, "3"))
                {
                    throw (new Exception(""));
                }
            }
            catch
            {
                throw (new Exception("Sorry!"));
            }
            d.orders.InsertOnSubmit(o);
            d.SubmitChanges();
            id = o.id;
        }
    }
}