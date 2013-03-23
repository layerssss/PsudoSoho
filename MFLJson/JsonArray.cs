using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public class JsonArray:List<IJson>,IJson
    {
        string IJson.ToString()
        {
            var ls= this.Select(s1 => s1.ToString());
            return "[" + (ls.Any() ? ls.Aggregate((s1, s2) => s1 + "," + s2) : "") + "]";
        }

        string IJson.ToFormattedString(string indent)
        {
            var ls=this.Select(s1 => indent + "    "+s1.ToFormattedString(indent + "    "));
            return "[\r\n" + (ls.Any() ? ls.Aggregate((s1, s2) => s1 + ",\r\n" + s2) : "") + "\r\n" + indent + "]";
        }
    }
}
