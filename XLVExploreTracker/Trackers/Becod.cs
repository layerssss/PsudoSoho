using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLVExploreTracker.Trackers
{
    public class Becod:TrackerImplementation
    {
        public override void TrackArg(string arg)
        {
            var hprovince = new TrackHelper("http://{0}.becod.com/", arg);
            while (true)
            {
                try
                {
                    hprovince.FetchString(" class=\"scenery2_top\"");
                }
                catch
                {
                    break;
                }
                hprovince.FetchString(">");
                var province = hprovince.FetchString("<");
                this.Fire("P {0}", province);
                hprovince.FetchString("<ul>");
                while (hprovince.BufferTailedWith("<li>"))
                {
                    hprovince.FetchString(" href=\"");
                    var hcity = new TrackHelper("http://{1}.becod.com{0}", hprovince.FetchString("\""), arg);
                    hprovince.FetchString(">");
                    var city = hprovince.FetchString("<").TrimStart('[').TrimEnd(']','网');
                    if (city == "")
                    {
                        hprovince.FetchString(" href=\"");
                        hprovince.FetchString(">");
                        city = hprovince.FetchString("<").TrimStart('[').TrimEnd(']', '网');
                    }
                    this.Fire("PC {0} {1}", province, city);
                    hprovince.FetchString("</li>");
                    int page;
                    try
                    {
                        hcity.FetchString("\"page_info\"><b>");
                        hcity.FetchString(" / ");
                        page = (Convert.ToInt32(hcity.FetchString("条")) - 1) / 15 + 1;
                    }
                    catch
                    {
                        page = 1;
                    }
                    for (var i = 1; i <= page; i++)
                    {
                        var hpage = new TrackHelper(hcity.url.Remove(hcity.url.Length - 5) + (i == 1 ? "" : "_" + i) + ".html");
                        this.Fire("PCP {0} {1} {2} {3}", province, city, i, hpage.url);
                        var count = 0;
                        while (true)
                        {
                            try
                            {
                                hpage.FetchString(" class=\"hotel_left\"");
                            }
                            catch
                            {
                                break;
                            }
                            hpage.FetchString("href=\"");
                            var url = "http://" + arg + ".becod.com" + hpage.FetchString("\"");
                            hpage.FetchString(" src=\"");
                            var imgurl = hpage.FetchString("\"");
                            this.Fire("Push {0}", url);
                            this.Push(url, province + "|" + city, imgurl);
                            //if (this.PushedCount > 10)
                            //{
                            //    return;
                            //}
                            count++;
                        }
                        this.Fire("Count:{0}", count);
                    }

                }
            }
        }

        public override void TrackUrl(TrackHelper helper, XLVExp_lodge entry)
        {
            helper.FetchString("<title>");
            entry.title = helper.FetchString("<");
            entry.title = entry.title.Remove(entry.title.Length - 2);
            var ar = entry.extradata.Split('|');
            entry.province = ar[0];
            entry.city = ar[1];
            helper.FetchString(" id=\"guzhen_left\"");
            helper.FetchString("<ul>");
            var content = helper.FetchString("</ul>");
            helper.FetchString(" id=\"guzhen_right\"");
            try
            {
                helper.FetchString("元起");
                helper.Position -= 6;
                helper.FetchString(">");
                entry.prize = helper.FetchString("元");
                if (entry.prize.Length > 10)
                {
                    throw (new Exception());
                }
            }
            catch
            {
                entry.prize = "不可用";
            }

            var reg = new System.Text.RegularExpressions.Regex(@"\D(1(\d{10})|0(\d{2})(\s*)(-?)(\s*)(\d{8})|0(\d{3})(\s*)(-?)(\s*)(\d{7}))");
            entry.tel = "";
            foreach (System.Text.RegularExpressions.Match match in reg.Matches(content))
            {
                entry.tel += content.Substring(match.Index - 5, 5) + match.Value + ";";
            }
        }

        public override string GetDescription(string arg)
        {
            switch (arg)
            {
                case "guzhen":
                    return "百酷-古镇";
                case "gongyu":
                    return "百酷-酒店式公寓";
                case "lvguan":
                    return "百酷-旅馆";
                case "lvshe":
                    return "百酷-青年旅社";
                case "njle":
                    return "百酷-农家乐";
            }
            return "百酷-未知类型(" + arg + ")";
        }
    }
}
