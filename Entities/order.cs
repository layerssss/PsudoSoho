using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    partial class order
    {
        public string address
        {
            get
            {
                return this.address1 + this.address2;
            }
        }
    }
