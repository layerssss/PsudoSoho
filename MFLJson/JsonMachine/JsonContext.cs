using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MFLJson.JsonMachine
{
    public abstract class JsonContext:JsonObject
    {
        public string Template;
        public string Prefix;
        public static Dictionary<string, string> TemplatesBuffer = new Dictionary<string, string>();
        public static JsonContext Initial(string templateFilename, string prefix,Func<string,string> templateReadDelegate)
        {

            lock (JsonContext.TemplatesBuffer)
            {
                if (!TemplatesBuffer.ContainsKey(templateFilename))
                {
                    TemplatesBuffer.Add(templateFilename, templateReadDelegate(templateFilename));
                }
            }
            var o = new JsonContextImplementation();
            o.Template = TemplatesBuffer[templateFilename];
            o.Prefix = prefix;
            return o;
        }
        [Obsolete("读取文件的初始化方法只应用于调试时")]
        public static JsonContext Initial(Stream templateStream,string prefix)
        {
            var o=new JsonContextImplementation();
            var reader = new StreamReader(templateStream);
            o.Template = reader.ReadToEnd();
            o.Prefix = prefix;
            reader.Close();
            return o;
        }
        public static string TagEmpty="";
        public static string TagNull="";
        public abstract void RenderToStream(Stream outputStream);
        protected JsonContext()
        {
        }
    }
}
