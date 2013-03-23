using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFLJson;
namespace MFL.Buffered
{
    public class MFLStatic:MFL.MFLStatic
    {
        protected MFLStatic()
        {
        }
        public MFLStatic(string root)
        {
            if (instance == null)
            {
                instance = new MFL.MFLStatic(root);
                icons = instance.GetIcons();
                templates = instance.GetTemplates();
            }
        }
        static MFL.MFLStatic instance;
        static JsonArray icons;
        static JsonArray templates;
        public override MFLJson.JsonArray GetIcons()
        {
            return icons;
        }
        public override JsonArray GetTemplates()
        {
            return templates;
        }
    }
}
