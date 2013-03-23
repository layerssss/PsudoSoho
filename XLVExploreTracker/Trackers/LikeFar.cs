using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace XLVExploreTracker.Trackers
{
    public class LikeFar:TrackerImplementation
    {

        public override void TrackArg(string arg)
        {
            var count = Convert.ToInt32(arg) ;
            var hAll = new TrackHelper("http://www.likefar.com/inn/");
            {
                Fire("Fecthed:{0}", hAll.url);
                hAll.FetchString("华东地区");
                while (true)
                {
                    try
                    {
                        hAll.FetchString("·<a target=\"_blank\"");
                    }
                    catch
                    {
                        break;
                    }
                    hAll.FetchString("<strong>");
                    var province = hAll.FetchString("</strong></a>");

                    Fire("P{0}", province);
                    var str = "<a target=\"_blank\" href=\"";
                    while (hAll.Buffer.Substring(hAll.Position, str.Length) == str)
                    {
                        hAll.Position += str.Length;
                        var cityUrl = hAll.FetchString("\">");
                        var city = hAll.FetchString("</a>");
                        Fire("C{0}", city);
                        try
                        {
                            var hCity = new TrackHelper(cityUrl);
                            Fire("Fecthed:{0}", hCity.url);
                            hCity.FetchString(">1/");
                            var pages = Convert.ToInt32(hCity.FetchString("</a>"));
                            for (var i = 1; i <= pages; i++)
                            {
                                Fire("CityPage {0} {1} {2}", province, city, i);

                                try
                                {
                                    var hPage = new TrackHelper(cityUrl + "/index" + (i == 1 ? "" : i.ToString()) + ".html");
                                    Fire("Fecthed:{0}", hPage.url);
                                    hPage.FetchString("<ul class=\"msgimglist\">");
                                    while (hPage.Buffer.Substring(hPage.Position, 3) == "<li")
                                    {
                                        hPage.FetchString("href=\"");
                                        var innurl = hPage.FetchString("\">");
                                        hPage.FetchString("</li>");
                                        this.Push(innurl, province + "|" + city);
                                        count--;
                                        if (count == 0)
                                        {
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Fire("ErrPage {0} {1} {2}", province, city, ex.Message);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Fire("ErrorCity {0} {1} {2}", province, city, ex.Message);
                        }
                    }

                }
            }
        }

        public override void TrackUrl(TrackHelper helper, XLVExp_lodge entry)
        {
            Fire("Fecthed:{0}", helper.url);
            helper.FetchString("<div class=\"viewbox\"><h1>");
            entry.title = helper.FetchString("</h1>");
            entry.province = entry.extradata.Split('|')[0];
            entry.city = entry.extradata.Split('|')[1];
            helper.FetchString("<div class=\"message\">");
            var mpos = helper.Position;
            helper.FetchString(" src=\"");
            entry.imgurl = helper.FetchString("\"");
            if (entry.imgurl.StartsWith("/") || entry.imgurl.StartsWith("\\"))
            {
                entry.imgurl = "http://www.likefar.com" + entry.imgurl;
            }
            helper.Position = mpos;
            var content = helper.FetchString("<div class=\"block mt1\">");
            var reg = new System.Text.RegularExpressions.Regex(@"\D(1(\d{10})|0(\d{2})(\s*)(-?)(\s*)(\d{8})|0(\d{3})(\s*)(-?)(\s*)(\d{7}))");
            entry.tel = "";
            foreach (System.Text.RegularExpressions.Match match in reg.Matches(content))
            {
                entry.tel += content.Substring(match.Index - 5, 5) + match.Value + ";";
            }
        }

        public override string GetDescription(string arg)
        {
            return "远方网-客栈联盟";
        }
    }
}
