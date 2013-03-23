using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MFLJson.JsonMachine
{
    class JsonContextImplementation:JsonContext
    {
        string tag;
        string data;
        public override void RenderToStream(System.IO.Stream outputStream)
        {
            this.Pos = 0;
            var data="";
            var noRender = false;
            this.writer = new StreamWriter(outputStream);
            if ((tag=FetchTag(out data, false)) != "Render")
            {
                this.Pos = 0;
                noRender = true;
            }
            try
            {
                while (true)
                {
                    switch ((tag = FetchTag(out data, true)))
                    {
                        case "RenderEnds":
                            writer.Flush();
                            return;
                        case null:
                            if (noRender)
                            {
                                writer.Flush();
                                return;
                            }
                            throw (new JsonContextException("找不到RenderEnds", this));
                        case "Array":
                            processArray(data);
                            break;
                        case "Array?":
                            processArrayOptional(data);
                            break;
                        case "Object":
                            processObject(data);
                            break;
                        default:
                            var dataarr = data.Split(',');
                            this.processData(tag, this[dataarr[0]], dataarr.Skip(1));
                            break;
                    }
                }
            }
            catch(Exception ex) {
                throw (new JsonContextException(ex.Message, this));
            }
        }
        private void processObject(string d)
        {
            var obj = this[d] as JsonObject;
            if (obj == null)
            {
                while (FetchTag(out data, false) != "ObjectEnds") ;
                return;
            }
            var objPos = Pos;
            var et=obj.GetEnumerator();
            if (!et.MoveNext())
            {
                writer.Write(JsonContext.TagEmpty);
                while (FetchTag(out data, false) != "ObjectEnds") ;
                return;
            }
            while (true)
            {
                switch ((tag = FetchTag(out data, true)))
                {
                    case "ObjectEnds":
                        if (et.MoveNext())
                        {
                            Pos = objPos;
                            break;
                        }
                        obj = null;
                        return;
                    case "ObjectKey":
                        writer.Write(et.Current.Key);
                        break;
                    case "ObjectText":
                        this.processData("Text", et.Current.Value);
                        break;
                    case "ObjectHtml":
                        this.processData("Html", et.Current.Value);
                        break;
                    default:
                        var dataarr = data.Split(',');
                        this.processData(tag, this[dataarr[0]], dataarr.Skip(1));
                        break;
                }
            }
        }
        private void processArray(string d)
        {
            var arr = this[d] as JsonArray;
            if (arr == null)
            {
                writer.Write(JsonContext.TagNull.Replace("$NullMessage$", (this[d] as JsonString).Text));
                while (FetchTag(out data, false) != "ArrayEnds") ;
                return;
            }
            var arrPos = Pos;
            var et = arr.GetEnumerator();
            if (!et.MoveNext())
            {
                writer.Write(JsonContext.TagEmpty);
                while (FetchTag(out data, false) != "ArrayEnds") ;
                return;
            }
            while (true)
            {
                switch ((tag = FetchTag(out data, true)))
                {
                    case "ArrayEnds":
                        if (et.MoveNext())
                        {
                            Pos = arrPos;
                            break;
                        }
                        arr = null;
                        return;
                    case "ArrayTags":
                        this.processTags((et.Current as JsonObject)[data] as JsonString);
                        break;
                    case "ArrayText":
                        this.processData("Text", (et.Current as JsonObject)[data]);
                        break;
                    case "ArrayHtml":
                        this.processData("Html", (et.Current as JsonObject)[data]);
                        break;
                    case "ArrayShowIf":
                        {
                            var dataarr = data.Split(',');
                            this.processData("ShowIf", (et.Current as JsonObject)[dataarr[0]], dataarr.Skip(1));
                        }
                        break;
                    case "ArrayShowIfEqual":
                        {
                            var dataarr = data.Split(',');
                            this.processData("ShowIfEqual", (et.Current as JsonObject)[dataarr[0]], dataarr.Skip(1));
                        }
                        break;
                    default:
                        {
                            var dataarr = data.Split(',');
                            this.processData(tag, this[dataarr[0]], dataarr.Skip(1));
                        }
                        break;
                }
            }
        }
        private void processTags(JsonString d)
        {
            var arr = d.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var arrPos = Pos;
            var et = arr.GetEnumerator();
            if (!et.MoveNext())
            {
                while (FetchTag(out data, false) != "ArrayTagsEnds") ;
                return;
            }
            while (true)
            {
                switch ((tag = FetchTag(out data, true)))
                {
                    case "ArrayTagsEnds":
                        if (et.MoveNext())
                        {
                            Pos = arrPos;
                            break;
                        }
                        arr = null;
                        return;
                    case "ArrayTagsText":
                        writer.Write(et.Current as string);
                        break;
                    default:
                        var dataarr = data.Split(',');
                        this.processData(tag, this[dataarr[0]], dataarr.Skip(1));
                        break;
                }
            }
        }
        private void processArrayOptional(string d)
        {
            var t = JsonContext.TagEmpty;
            JsonContext.TagEmpty = "";
            this.processArray(d);
            JsonContext.TagEmpty = t;
        }
        void processData(string tag, IJson d,IEnumerable<string> moredata)
        {
            switch (tag)
            {
                case "Text":
                    if (d is JsonString)
                    {
                        writer.Write((d as JsonString).Text);
                    }
                    else
                    {
                        writer.Write(d.ToString());
                    }
                    return;
                case "Tags":
                    this.processTags(d as JsonString);
                    break;
                case "Html":
                    writer.Write((d as JsonString).Html);
                    return;
                case "ShowIf":
                    writer.Write((d as JsonString).Text == "true" ? moredata.First() : "");
                    return;
                case "ShowIfEqual":
                    writer.Write((d as JsonString).Text == moredata.ElementAt(1) ? moredata.ElementAt(0) : "");
                    return;
                default:
                    throw (new JsonContextException("不识别的"+tag, this));
            }
        }
        void processData(string tag, IJson d)
        {
            processData(tag,d,new string[]{});
        }
        StreamWriter writer;
        string FetchTag(out string data,bool write)
        {
            var lastPos = Pos;
            var tagStartOffset = 0;
            var dataStartOffset = 0;
            while (Pos != Template.Length)
            {
                switch (Template[Pos])
                {
                    case '$':
                        if (tagStartOffset == 0)
                        {
                            try
                            {
                                if (this.Prefix != this.Template.Substring(Pos + 1, Prefix.Length))
                                {//prefix error
                                    break;
                                }
                            }
                            catch
                            {
                                break;
                            }
                            tagStartOffset = Pos - lastPos + 1 + Prefix.Length;
                        }
                        else
                        {
                            
                            if (write)
                            {
                                writer.Write(this.Template.Substring(lastPos, tagStartOffset - Prefix.Length - 1));
                            }
                            data = this.Template.Substring(lastPos + dataStartOffset, Pos - lastPos - dataStartOffset);
                            Pos++;
                            return this.Template.Substring(lastPos + tagStartOffset, dataStartOffset - tagStartOffset - 1);
                        }
                        break;
                    case ':':
                        dataStartOffset = Pos - lastPos + 1;
                        break;
                }
                Pos++;
            }
            if (write)
            {
                writer.Write(this.Template.Substring(lastPos, Pos - lastPos));
            }
            data = null;
            return null;
        }
        public int Pos = 0;
    }
}
