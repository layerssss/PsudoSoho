using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFLAdmin
{
    public class PinyinGlossary:Dictionary<string,string>
    {
        public void Append(string hz)
        {
            var py = PinyinConverter.Convert(hz);
            if (this.ContainsValue(py))
            {
                var i = 2;
                while (this.ContainsValue(py + i))
                {
                    i++;
                }
                this.Add(hz, py + i);
            }
            else
            {
                this.Add(hz, py);
            }
        }
    }
}