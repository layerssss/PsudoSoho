using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public class JsonBool : JsonString, IJson
    {
        public override string Text
        {
            get
            {
                return this.Value ? "true" : "false";
            }
            set
            {
                this.Value = bool.Parse(value);
            }
        }
        public bool Value;
        string IJson.ToString()
        {
            return this.Text;
        }

        string IJson.ToFormattedString(string indent)
        {
            return this.Text;
        }
    }
}
