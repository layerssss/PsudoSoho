using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public class JsonNumber:JsonString,IJson
    {
        public override string Text
        {
            get
            {
                return this.Value.ToString();
            }
            set
            {
                this.Value = double.Parse(value);
            }
        }
        public double Value;
        string IJson.ToString()
        {
            return this.Value.ToString();
        }

        string IJson.ToFormattedString(string indent)
        {
            return this.Value.ToString();
        }
    }
}
