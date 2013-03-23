using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cfs
{
    public partial class timespan
    {
        public string startTime
        {
            get
            {
                return this.start.ToShortTimeString();
            }
        }
        public string stopTime
        {
            get
            {
                return this.stop.ToShortTimeString();
            }
        }
    }
}