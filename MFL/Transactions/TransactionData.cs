using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MFLJson;
namespace MFL.Transactions
{
    public class TransactionData
    {
        string[][] getCommands(string filePath)
        {
            return File.ReadAllLines(filePath).Select(ts => ts.Split(new[] { "," }, StringSplitOptions.None)).ToArray();
        }
        public TransactionData(string path)
        {
            this.Products = new Lazy<ProductCollection>(() =>
            {
                var pc = new ProductCollection();
                ProductCollection.Product p=null;
                foreach (var cmd in this.getCommands(path+"Products.txt"))
                {
                    switch (cmd[0])
                    {
                        case "product":
                            p = new ProductCollection.Product();
                            p.Type = cmd[1];
                            pc.Add(cmd[1], p);
                            break;
                        case "name":
                            p.Name = cmd[1];
                            break;
                        case "prize":
                            p.Prizes.Add(Convert.ToInt32(cmd[1]), Convert.ToInt32(cmd[2]));
                            break;
                        case "capPhoto":
                            p.CapPhoto = Convert.ToInt32(cmd[1]);
                            break;
                    }
                }
                return pc;
            }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            this.Icons = new Lazy<MFLJson.JsonArray>(() =>
            {
                var icons = MFLJson.Json.Array();
                MFLJson.JsonObject icon = null; 
                foreach (var cmd in this.getCommands(path + "Icons.txt"))
                {
                    switch (cmd[0])
                    {
                        case "icon":
                            icon = Json.Object(
                                "icon", Json.String(string.Format("background-position:-{0}px -96px;", Convert.ToInt32(cmd[1])*32)),
                                "text", Json.String(string.Format("图标：{0}", cmd[2])));
                            icons.Add(icon);
                            break;
                    }
                }
                return icons;
            }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            this.LodgeInitializationAttributes = new Lazy<AttributeCollection>(() =>
            {
                var ac = new AttributeCollection();
                AttributeCollection.Attribute attr = null;
                foreach (var cmd in this.getCommands(path + "LodgeInitializationAttributes.txt"))
                {
                    switch (cmd[0])
                    {
                        case "attribute":
                            attr = new AttributeCollection.Attribute()
                            {
                                Name = cmd[1]
                            };
                            ac.Add(attr);
                            break;
                        case "iconIndex":
                            attr.IconIndex = Convert.ToInt32(cmd[1]);
                            break;
                        case "option":
                            attr.Options.Add(cmd[1]);
                            break;
                    }
                }
                return ac;
            }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }
        TransactionData()
        {
        }
        public static Lazy<TransactionData> OnlyInstance;
        public Lazy<ProductCollection> Products;
        public Lazy<MFLJson.JsonArray> Icons;
        public Lazy<AttributeCollection> LodgeInitializationAttributes;

        public class ProductCollection : Dictionary<string, ProductCollection.Product>
        {
            public class Product
            {
                public string Name;
                public string Type;
                public int CapPhoto;
                public Dictionary<int, int> Prizes = new Dictionary<int, int>();
            }
        }
        public class AttributeCollection : List<AttributeCollection.Attribute>
        {
            public class Attribute
            {
                public string Name;
                public int IconIndex;
                public List<string> Options = new List<string>();
            }
        }
    }
}
