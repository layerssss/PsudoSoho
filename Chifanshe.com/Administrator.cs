using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.todaynic.ScpClient;
namespace Chifanshe.com
{
    public class Administrator
    {
        [ispJs.Action]
        public void AddStore(string title)
        {
            Auth.ValidateAdmin();
            var d = Global.Entity;
            var s = new cfs.store()
            {
                alias = "null",
                phone = "",
                speed = "",
                pricelimit=0,
                title = ""
            };
            d.stores.InsertOnSubmit(s);
            d.SubmitChanges();
            var t = new cfs.timespan()
            {
                start=DateTime.Today,
                stop=DateTime.Today,
                storeId=s.id
            };
            d.timespans.InsertOnSubmit(t);
            d.SubmitChanges();

            
        }
        [ispJs.Action]
        public void EditStore(int id, string phone,int pricelimit,string title, string startTime, string stopTime, string speed, string alias)
        {
            
            Auth.ValidateAdmin();
            var d = Global.Entity;
            var s = d.stores.First(ts => ts.id == id);
            s.title = title;
            s.speed = speed;
            s.alias = alias;
            s.pricelimit = pricelimit;
            s.phone = phone;
            s.timespans.First().start = DateTime.Parse(DateTime.Today.ToShortDateString() + " " + startTime);
            s.timespans.First().stop = DateTime.Parse(DateTime.Today.ToShortDateString() + " " + stopTime);
            d.SubmitChanges();
        }
        [ispJs.Action]
        public void AddFood(string title,int sid)
        {
            Auth.ValidateAdmin();
            var d = Global.Entity;
            d.foods.InsertOnSubmit(new cfs.food()
            {
                description="",
                optiondata = "{'属性':['选项1','选项2']}",
                price=0,
                title=title,
                storeId=sid
            });
            d.SubmitChanges();

        }
        [ispJs.Action]
        public void EditFood(int id, string title, int price, string optiondata)
        {
            Auth.ValidateAdmin();
            var d = Global.Entity;
            var f = d.foods.First(tf => tf.id == id);
            f.title = title;
            f.price = price;
            f.optiondata = optiondata;
            d.SubmitChanges();
        }

        [ispJs.Action]
        public void ChangeBackground(HttpPostedFile file, int id)
        {

            Auth.ValidateAdmin();
            file.SaveAs(ispJs.WebApplication.Server.MapPath("/storeImg/" + id + ".jpg"));

        }
    }
}