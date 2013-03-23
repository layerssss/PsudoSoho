using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MFLJson;
using System.IO;
namespace MFL
{
    public class MFLStatic
    {
        protected string root;
        protected MFLStatic() { 
        }
        public MFLStatic(string root)
        {
            this.root = root;
        }
        public virtual JsonArray GetIcons()
        {
            return Transactions.TransactionData.OnlyInstance.Value.Icons.Value;

        }
        public virtual JsonArray GetTemplates()
        {
            return Json.Array(Directory.GetDirectories(root + "templates\\").Select(prefix =>
            {
                var name = prefix.Substring(prefix.LastIndexOf('\\') + 1);
                var previewUrl = File.ReadAllText(prefix + "\\previewUrl.txt");
                return Json.Object(
                    "templateName", Json.String(name),
                    "templatePreviewIcon", Json.String("/templates/" + name + "/previewIcon.png?" + "_=" + (new Random()).Next(1000000)),
                    "templatePreviewUrl", Json.String(previewUrl)
                    );
            }).ToArray());
        }
    }
}
