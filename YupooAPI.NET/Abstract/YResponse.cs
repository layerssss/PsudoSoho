using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Data.Linq;
namespace YupooAPI.NET.Abstract
{
    public abstract class YResponse
    {
        public Dictionary<string, string> Values;
        public Dictionary<string, Dictionary<string,string>> Dics;
        public void ReadREST(System.IO.Stream reader)
        {
            this.Values = new Dictionary<string, string>();
            this.Dics = new Dictionary<string, Dictionary<string, string>>();
            var xml = new XmlDocument();
            xml.Load(reader);
            var root=xml.SelectSingleNode("rsp").ChildNodes[0];
            if (root == null)
            {
                return;
            }
            if (root.Name == "err")
            {
                throw (new Exception(string.Format("code:{0} msg:{1}", root.Attributes["code"].Value, root.Attributes["msg"].Value)));
            }
            foreach (XmlElement e in root)
            {
                this.Values.Add(e.Name, e.InnerText);
                var adic=new Dictionary<string, string>();
                foreach(XmlAttribute a in e.Attributes){
                    adic.Add(a.Name,a.Value);
                }
                this.Dics.Add(e.Name, adic);
            } 
            foreach (XmlAttribute a in root.Attributes)
            {
                this.Values[a.Name] = a.Value;
            }
        }
    }
}
