using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace GoClassing.Internal
{
    public static class GCTemplateReader
    {
        static Dictionary<string, string> templates = new Dictionary<string, string>();
        public static string Get(string templateFilename, string root)
        {
            lock (templates)
            {
                if (!templates.ContainsKey(templateFilename))
                {
                    templates.Add(templateFilename, File.ReadAllText(root + "Internal\\Templates\\" + templateFilename));
                }
                return templates[templateFilename];
            }
        }
        public static string Get(string templateFilename)
        {
            return Get(templateFilename, HttpContext.Current.Server.MapPath("/"));
        }
    }
}