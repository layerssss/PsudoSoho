using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public class JsonString:IJson
    {
        public virtual string Text
        {
            get;
            set;
        }
        public string Html
        {
            get
            {
                return System.Web.HttpUtility.HtmlEncode(this.Text).Replace(" ", "&nbsp;").Replace("\n", "<br />");
            }
        }
        public static JsonString Empty = new JsonString() { Text = "" };

        string IJson.ToString()
        {
            return "\"" + Json.EscapeString(this.Text).Replace("</script>","</sc\u0063ript>") + "\"";
        }

        string IJson.ToFormattedString(string indent)
        {
            return "\"" + Json.EscapeString(this.Text) + "\"";
        }
        public string EscapedValue
        {
            get
            {
                return Json.EscapeString(this.Text);
            }
        }
    }
}
