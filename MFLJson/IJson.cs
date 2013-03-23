using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public interface IJson
    {
        string ToString();
        string ToFormattedString(string indent);
    }
}
