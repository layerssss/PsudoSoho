using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace XLVExploreTracker
{
    public abstract class TrackerImplementation:TrackController
    {
        public override void Track(string arg)
        {
            this.d = new XLVExpEntities();
            
            this.arg = arg;

            switch (TrackController.Mode)
            {
                case TrackingMode.JustClear:
                    {
                        d.ExecuteStoreCommand("delete from XLVExp_lodge where tracker='" + this.Name + "' and trackerArg='" + arg + "'");
                    }
                    break;
                case TrackingMode.RefreshUrlList:
                    {
                        d.ExecuteStoreCommand("delete from XLVExp_lodge where tracker='" + this.Name + "' and trackerArg='" + arg + "'");
                        this.TrackArg(arg);
                    }
                    break;
                case TrackingMode.TrackPages:
                    {
                        var count = d.XLVExp_lodge.Count(tl => tl.tracker == this.Name && tl.trackerArg == arg);
                        for (var i = 0; i < count ; i++)
                        {
                            var entry = d.XLVExp_lodge.Where(tl => tl.tracker == this.Name && tl.trackerArg == arg).OrderBy(tl => tl.id).Skip(i).First();
                            try
                            {
                                this.TrackUrl(new TrackHelper(entry.url), entry);
                                d.SaveChanges();
                                this.Fire("Saved:{1} {0}", entry.title, entry.url);
                            }
                            catch (Exception ex)
                            {
                                this.Fire("Error Save Info({0}):{1} {2} {3}", ex.Message, entry.title, entry.tel, entry.url);
                                try
                                {
                                    this.Fire("Inner:{0}", ex.InnerException.Message);
                                }
                                catch { }
                                d.Refresh(System.Data.Objects.RefreshMode.StoreWins, entry);
                            }
                            //if (i > 10)//Debug
                            //{
                            //    break;
                            //}
                        }
                    }
                    break;

            }

        }
        string arg;
        public abstract string GetDescription(string arg);
        public abstract void TrackArg(string arg);
        public abstract void TrackUrl(TrackHelper helper, XLVExp_lodge entry);
        protected int PushedCount = 0;
        protected void Push(string url,string extradata,string imgurl="")
        {
            PushedCount++;
            var d = new XLVExpEntities();
            if (d.XLVExp_lodge.Any(tl => tl.url == url))
            {
                return;
            }
            var entry = new XLVExp_lodge()
            {
                city = "",
                extradata = extradata,
                imgurl = imgurl,
                url = url,
                prize = "",
                province = "",
                title = "",
                tel="",
                tracker = this.Name,
                trackerArg = arg
            };
            d.XLVExp_lodge.AddObject(entry);
            try
            {
                d.SaveChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public XLVExpEntities d { get; set; }
    }
}
