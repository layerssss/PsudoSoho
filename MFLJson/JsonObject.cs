using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public class JsonObject:Dictionary<string,IJson>,IJson
    {
        public JsonObject Clone()
        {
            var obj = Json.Object();
            foreach (var key in this.Keys)
            {
                obj.Add(key, this[key]);
            }
            return obj;
        }
        string IJson.ToString()
        {
            var ls=this.Select(s1 => "\"" + Json.EscapeString(s1.Key) + "\":" + (s1.Value??JsonString.Empty).ToString());
            return "{" + (ls.Any() ? ls.Aggregate((s1, s2) => s1 + "," + s2) : "") + "}";
        }

        string IJson.ToFormattedString(string indent)
        {
            var ls = this.Select(s1 => indent + "    " + "\"" + Json.EscapeString(s1.Key) + "\":" + (s1.Value ?? JsonString.Empty).ToFormattedString(indent + "    "));
            return "{\r\n" + (ls.Any() ? ls.Aggregate((s1, s2) => s1 + ",\r\n" + s2) : "") + "\r\n" + indent + "}";
        }
        public new IJson this[string key]
        {
            get
            {
                if (!this.ContainsKey(key))
                {
                    throw (new Exception(string.Format("Key \"{0}\" not found! ({1})", key, this.Keys.Aggregate((s1, s2) => s1 + "," + s2))));
                }
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }
        public void Embed(JsonObject child, string prefix)
        { 
            if (prefix == "")
            {
                this.Embed(child);
                return;
            }
            foreach (var kvp in child)
            {
                this[prefix + kvp.Key.Remove(1).ToUpper() + kvp.Key.Substring(1)] = kvp.Value;
            }
        }
        public void Embed(JsonObject child)
        {
            foreach (var kvp in child)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
    }
}
