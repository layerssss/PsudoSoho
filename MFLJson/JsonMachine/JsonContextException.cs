using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson.JsonMachine
{
    class JsonContextException:Exception
    {
        public JsonContextException(string message,JsonContextImplementation context)
        {
            this.message = message;
            this.context = context;
        }
        string message;
        private JsonContextImplementation context;
        public override string Message
        {
            get
            {
                var prev = "";
                var next = "";
                try
                {
                    prev = this.context.Template.Substring(this.context.Pos - 5, 5);
                }
                catch { }
                try
                {
                    next = this.context.Template.Substring(this.context.Pos, 5);
                }
                catch { }
                var lines = this.context.Template.Remove(this.context.Pos);
                var rows = lines.Count(tc => tc == '\r');
                var cols = lines.Length - lines.LastIndexOf('\r') - 1;
                return "Row:" + rows + ";Cols:" + cols + "\r\nSource:" + prev + "|" + next +"\r\nMessage:"+ message;
            }
        }

    }
}
