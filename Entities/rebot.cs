using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    partial class rebot
    {
        public bool locked
        {
            get
            {
                return (DateTime.Now - this.locktime).TotalSeconds < 30;
            }
        }
    }
