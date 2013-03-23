using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cfs
{
    public partial class food
    {
        public string priceString
        {
            get
            {   var str=this.price.ToString();
             return str.Remove(str.Length - 1) + '.' + str.Substring(str.Length - 1)+'元';
            }
        }
    }
}