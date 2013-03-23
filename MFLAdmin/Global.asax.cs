using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Threading;
using MFLJson;
using MFLJson.JsonMachine;
using MFL.Buffered;
namespace MFLAdmin
{
    public class Global : System.Web.HttpApplication
    {
        public static Queue<string> Pending;
        public static string Publishing;
        protected static Thread thread;
        protected static bool threadStopping;
        public static string LogPath;
        public static string Root;
        public static bool IsDebugging
        {
            get
            {
                return MFL.SharedConfig.Debugging;
            }
        }
        protected static void threadStart()
        {
            Global.threadStopping = false;
            byte[] buffer = new byte[1000000];
            var ms = new MemoryStream(buffer);
            var rootLength=(Root + "templates\\" ).Length;
            while (!Global.threadStopping)
            {
                var lodgeName = "";
                lock (Global.Pending)
                {
                    if (Global.Pending.Any())
                    {
                        lodgeName = Global.Pending.Dequeue();
                        Publishing = lodgeName;
                    }
                }
                if (lodgeName != "")
                {

                    if (lodgeName.StartsWith("~"))
                    {
                        try
                        {
                            var monthStr = "";
                            var arr = lodgeName.Split(new[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                            var lodge = new MFLLodge(Root, arr[0]);
                            monthStr = arr[1];
                            var c = new LodgeService.WebServiceSoapClient();

                            {
                                var jMonth = Json.Object();
                                for (var i = 0; i < 31; i++)
                                {
                                    var count = 0;
                                    var jDay = Json.Object();
                                    foreach (var r in lodge.MFL_lodge.MFL_room)
                                    {
                                        if (File.Exists(string.Format(Root + "static\\{0}-room{1:D4}-state-{2}{3:D2}.txt", lodge.MFL_lodge.ident, r.id, monthStr, i + 1)))
                                        {
                                            jDay["room" + r.id] = Json.True();
                                            count++;
                                        }
                                        else
                                        {
                                            jDay["room" + r.id] = Json.False();
                                        }

                                    }
                                    jDay["sum"] = Json.Number(lodge.MFL_lodge.MFL_room.Count - count);
                                    jMonth["day" + (i + 1)] = jDay;
                                }
                                c.WriteFile(GCServiceBase.Validator.GetHash(), lodge.MFL_lodge.ident +
                                    '/' + monthStr + ".js", (jMonth as IJson).ToString());
                            }
                        }
                        catch(Exception ex)
                        {
                            if (IsDebugging)
                            {
                                File.AppendAllLines(LogPath, new[] { ex.ToString() + ex.StackTrace ?? "" });
                            }
                            lock (Global.Pending)
                            {
                                Global.Pending.Enqueue(lodgeName);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (IsDebugging)
                            {
                                JsonContext.TemplatesBuffer.Clear();
                            }
                            var c = new LodgeService.WebServiceSoapClient();
                            var lodge = new MFLLodge(Root, lodgeName);
                            var templateName = lodge.GetTemplateName();
                            c.EmptyFolder(GCServiceBase.Validator.GetHash(), lodgeName, "*.htm");
                            c.EmptyFolder(GCServiceBase.Validator.GetHash(), lodgeName, "*.css");
                            c.EmptyFolder(GCServiceBase.Validator.GetHash(), lodgeName, "*.txt");

                            if (IsDebugging)
                            {
                                foreach (var file in Directory.GetFiles(Root + "Messages\\RenderCache"))
                                {
                                    File.Delete(file);
                                }
                            }

                            var baseObject = Json.Object();

                            Func<string, string> templateReader = (filename) =>
                            {
                                var tM = JsonContext.Initial("T-" + filename, "Include", (fname) =>
                                {
                                    try
                                    {
                                        return File.ReadAllText(fname.Substring(2));
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        return "";
                                    }
                                });
                                foreach (var file in Directory.GetFiles(Root + "templates\\" + templateName + "\\files\\", "*.Include"))
                                {
                                    var incFilename = file.Substring(file.LastIndexOf('\\') + 1);
                                    var incM = JsonContext.Initial("INC-" + file, "Element", (incname) =>
                                    {
                                        return File.ReadAllText(incname.Substring(4));
                                    });
                                    foreach (var ele in Directory.GetFiles(Root + "templates\\" + templateName + "\\files\\", "*.Element"))
                                    {
                                        var eleFilename = ele.Substring(ele.LastIndexOf('\\') + 1);
                                        incM[eleFilename.Remove(eleFilename.LastIndexOf('.'))] = Json.String(File.ReadAllText(ele));
                                    }
                                    ms.Seek(0, SeekOrigin.Begin);
                                    incM.RenderToStream(ms);
                                    tM[incFilename.Remove(incFilename.LastIndexOf('.'))] = Json.String(System.Text.Encoding.UTF8.GetString(buffer, 0, (int)ms.Position));
                                }
                                ms.Seek(0, SeekOrigin.Begin);
                                tM.RenderToStream(ms);
                                var tmp = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)ms.Position);
                                if (IsDebugging)
                                {
                                    File.WriteAllText(Root + "Messages\\RenderCache\\" + filename.Substring(filename.LastIndexOf('\\') + 1) + ".MFL", tmp);
                                }
                                return tmp;
                            };

                            var specialTypes = new Dictionary<string, int>();
                            var specialTypesKeys = new Dictionary<string, string>();
                            foreach (var line in File.ReadAllLines(Root + "templates\\" + templateName + "\\specialPhotoTypes.txt"))
                            {
                                var ar = line.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                if (ar.Length == 3)
                                {
                                    var type = ar[1];
                                    try
                                    {
                                        type = type.Remove(type.IndexOf('('));
                                    }
                                    catch { }
                                    specialTypes.Add(type, Convert.ToInt32(ar[2]));
                                    specialTypesKeys.Add(type, ar[0]);
                                }
                            }
                            var g = new PinyinGlossary();
                            {
                                baseObject["baseUrl"] = Json.String(MFL.SharedConfig.LodgeBaseUrl);
                                baseObject["adminBaseUrl"] = Json.String(MFL.SharedConfig.AdminBaseUrl);
                                baseObject["MFLBaseUrl"] = Json.String(MFL.SharedConfig.MFLBaseUrl);
                                baseObject.Embed(lodge.GetAllProperties());
                                foreach (JsonObject tP in lodge.GetTemplateProperties(templateName))
                                {
                                    baseObject[(tP["templatePropertyKey"] as JsonString).Text] = tP["templatePropertyValue"];
                                }
                                baseObject["lodgeAlbum"] = lodge.LodgeGetPhotos("照片展示");
                                baseObject["lodgeRooms"] = lodge.GetRooms();
                                foreach (JsonObject roomObj in baseObject["lodgeRooms"] as JsonArray)
                                {
                                    roomObj["roomCurrent"] = Json.False();
                                }
                                baseObject["lodgePhotoTypes"] = Json.Array(lodge.LodgeGetPhotoTypes().Where(tjson =>
                                {
                                    return !specialTypes.Keys.Any(ts => (tjson as JsonString).Text.StartsWith(ts));
                                })
                                    .Select(tjson =>
                                {
                                    var tstr = (tjson as JsonString).Text;
                                    g.Append(tstr);
                                    return Json.Object(
                                        "type", tjson,
                                        "typeCurrent", Json.False(),
                                        "typePinyin", Json.String(g[tstr]));
                                }));
                                foreach (var stype in specialTypes.Keys)
                                {
                                    var sphotos = lodge.LodgeGetPhotos(stype);
                                    if (specialTypes[stype] < 0 && sphotos.Count + specialTypes[stype] < 0)
                                    {
                                        throw (new Exception() { HelpLink = "当前使用的模版要求放置在“" + stype + "”类型的照片至少要" + (-specialTypes[stype]) + "张,当前已有" + sphotos.Count + "张，请打开“旅馆相册”页面修改图片分类或上传图片后重试。" });
                                    }
                                    if (specialTypes[stype] > 0 && sphotos.Count != specialTypes[stype])
                                    {
                                        throw (new Exception() { HelpLink = "当前使用的模版要求放置在“" + stype + "”类型的照片数目为" + specialTypes[stype] + ",当前已有" + sphotos.Count + "张，请打开“旅馆相册”页面修改图片分类或上传图片后重试。" });
                                    }
                                    baseObject["specialPhotos" + specialTypesKeys[stype]] = sphotos;
                                    for (var i = 0; i < sphotos.Count; i++)
                                    {
                                        baseObject["specialPhoto" + specialTypesKeys[stype] + i] = (sphotos[i] as JsonObject)["lodgePhotoUrl"];
                                    }
                                }
                                baseObject["ImportScripts"] = Json.Array();
                                baseObject["RenderIt"] = Json.True();
                                runJsOnRoot(Root + "templates\\" + templateName + "\\files\\root.js", baseObject, baseObject);
                                runJsOnRoot(Root + "App_Data\\templateScripts\\root.js", baseObject, baseObject);
                                foreach (JsonObject script in baseObject["ImportScripts"] as JsonArray)
                                {
                                    runJsOnRoot(Root + "App_Data\\templateScripts\\" + (script["FileName"] as JsonString).Text, baseObject, baseObject);
                                }
                            }
                            baseObject["ImportScripts"] = Json.Array();
                            var renderedTypes = Json.Array();
                            {
                                var typeTemplateFI = new FileInfo(Root + "templates\\" + templateName + "\\files\\type.htm");
                                //if (typeTemplateFI.Exists)
                                //{
                                foreach (var type in (baseObject["lodgePhotoTypes"] as JsonArray).Select(tjson => ((tjson as JsonObject)["type"] as JsonString).Text))
                                {
                                    var m = JsonContext.Initial(typeTemplateFI.FullName, "MFL", templateReader);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    m.Embed(baseObject);
                                    m["FileName"] = Json.String(typeTemplateFI.Name);
                                    foreach (JsonObject typeObj in m["lodgePhotoTypes"] as JsonArray)
                                    {
                                        typeObj["typeCurrent"] = Json.Bool(
                                            (typeObj["type"] as JsonString).Text == type);
                                    }
                                    m["type"] = Json.String(type);
                                    m["typePinyin"] = Json.String(g[type]);
                                    m["lodgeTypeAlbum"] = lodge.LodgeGetPhotos(type);

                                    runJsOnRoot(Root + "templates\\" + templateName + "\\files\\type.htm.js", m, m);
                                    runJsOnRoot(Root + "App_Data\\templateScripts\\type.htm.js", m, m);
                                    foreach (JsonObject script in m["ImportScripts"] as JsonArray)
                                    {
                                        runJsOnRoot(Root + "App_Data\\templateScripts\\" + (script["FileName"] as JsonString).Text, m, m);
                                    }
                                    if (IsDebugging)
                                    {
                                        File.WriteAllText(Root + "Messages\\RenderCache\\" + g[type] + ".htm.js",
                                            File.ReadAllText(Root + "App_Data\\comments\\root.js").Replace("{DateTime.Now}", DateTime.Now.ToString()) +
                                            "root=" + (m as IJson).ToFormattedString("") + ";", System.Text.Encoding.Default);
                                    }
                                    try
                                    {
                                        m.RenderToStream(ms);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw (new Exception("渲染“旅馆照片分类页面”时发生错误:" + ex.ToString()));
                                    }
                                    var text = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)ms.Position);
                                    m["content"] = Json.String(text);
                                    renderedTypes.Add(m);
                                    if ((m["RenderIt"] as JsonBool).Value && text.Length != 0)
                                    {
                                        c.WriteFile(
                                            GCServiceBase.Validator.GetHash(),
                                            lodgeName + "/" + g[type] + ".htm",
                                            text);
                                    }
                                }
                                //}
                            }

                            foreach (JsonObject roomObj in baseObject["lodgePhotoTypes"] as JsonArray)
                            {
                                roomObj["typeCurrent"] = Json.False();
                            }
                            var renderedRooms = Json.Array();
                            {
                                var roomTemplateFI = new FileInfo(Root + "templates\\" + templateName + "\\files\\room.htm");
                                //if (roomTemplateFI.Exists)
                                //{

                                foreach (JsonObject typeObj in baseObject["lodgePhotoTypes"] as JsonArray)
                                {
                                    typeObj["typeCurrent"] = Json.False();
                                }
                                foreach (JsonObject room in lodge.GetRooms())
                                {
                                    var m = JsonContext.Initial(roomTemplateFI.FullName, "MFL", templateReader);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    m.Embed(baseObject);
                                    m.Embed(room);

                                    foreach (JsonObject roomObj in baseObject["lodgeRooms"] as JsonArray)
                                    {
                                        roomObj["roomCurrent"] = Json.Bool(
                                            (roomObj["roomId"] as JsonNumber).Value == (m["roomId"] as JsonNumber).Value
                                            );
                                    }
                                    m["FileName"] = Json.String(roomTemplateFI.Name);
                                    var roomId = (int)(room["roomId"] as JsonNumber).Value;
                                    m["roomAlbum"] = lodge.RoomGetPhotos(roomId);
                                    m["roomAttributes"] = lodge.GetRoomAttributes(roomId, true);

                                    runJsOnRoot(Root + "App_Data\\templateScripts\\room.htm.js", m, m);
                                    runJsOnRoot(Root + "templates\\" + templateName + "\\files\\room.htm.js", m, m);
                                    foreach (JsonObject script in m["ImportScripts"] as JsonArray)
                                    {
                                        runJsOnRoot(Root + "App_Data\\templateScripts\\" + (script["FileName"] as JsonString).Text, m, m);
                                    }

                                    if (IsDebugging)
                                    {
                                        File.WriteAllText(Root + "Messages\\RenderCache\\" + roomId + ".htm.root.js",
                                            File.ReadAllText(Root + "App_Data\\comments\\root.js").Replace("{DateTime.Now}", DateTime.Now.ToString()) +
                                            "root=" + (m as IJson).ToFormattedString("") + ";", System.Text.Encoding.Default);
                                    }
                                    try
                                    {
                                        m.RenderToStream(ms);
                                    }

                                    catch (Exception ex)
                                    {
                                        throw (new Exception("渲染“房间页面”时发生错误:" + ex.ToString()));
                                    }
                                    var text = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)ms.Position);
                                    m["content"] = Json.String(text);
                                    renderedRooms.Add(m);
                                    if ((m["RenderIt"] as JsonBool).Value && text.Length != 0)
                                    {
                                        c.WriteFile(
                                            GCServiceBase.Validator.GetHash(),
                                            lodgeName + "/room" + roomId + ".htm",
                                            text + (m["RenderIt"] as JsonBool).Value);
                                    }
                                }
                                //}
                            }

                            baseObject["lodgeRooms"] = renderedRooms;
                            baseObject["lodgePhotoTypes"] = renderedTypes;
                            foreach (JsonObject roomObj in baseObject["lodgeRooms"] as JsonArray)
                            {
                                roomObj["roomCurrent"] = Json.False();
                            }
                            foreach (JsonObject roomObj in baseObject["lodgePhotoTypes"] as JsonArray)
                            {
                                roomObj["typeCurrent"] = Json.False();
                            }
                            foreach (var file in Directory.GetFiles(Root + "templates\\" + templateName + "\\files\\"))
                            {
                                if (file.ToLower().EndsWith("room.htm") || file.ToLower().EndsWith("type.htm"))
                                {
                                    continue;
                                }
                                if (!(file.ToLower().EndsWith(".htm") || file.ToLower().EndsWith(".css") || file.ToLower().EndsWith(".txt")))
                                {
                                    continue;
                                }
                                var m = JsonContext.Initial(file, "MFL", templateReader);
                                ms.Seek(0, SeekOrigin.Begin);
                                m.Embed(baseObject);
                                m["FileName"] = Json.String((new FileInfo(file)).Name);
                                
                                runJsOnRoot(file + ".js", m, m);
                                runJsOnRoot(Root + "App_Data\\templateScripts\\" + file.Substring(rootLength + templateName.Length + 7) + ".js", m, m);
                                foreach (JsonObject script in m["ImportScripts"] as JsonArray)
                                {
                                    runJsOnRoot(Root + "App_Data\\templateScripts\\" + (script["FileName"] as JsonString).Text, m, m);
                                }
                                if (IsDebugging)
                                {
                                    File.WriteAllText(Root + "Messages\\RenderCache\\" + file.Substring(rootLength + templateName.Length + "\\files\\".Length) + ".root.js",
                                        File.ReadAllText(Root + "App_Data\\comments\\root.js").Replace("{DateTime.Now}",DateTime.Now.ToString()) +
                                        "root=" + (m as IJson).ToFormattedString("") + ";", System.Text.Encoding.Default);
                                }
                                try
                                {
                                    m.RenderToStream(ms);
                                }
                                catch (Exception ex)
                                {
                                    throw (new Exception("渲染" + file.Substring(rootLength + templateName.Length + "\\files\\".Length) + "时发生错误:" + ex.ToString()));
                                }
                                if ((m["RenderIt"] as JsonBool).Value && ms.Position != 0)
                                {
                                    c.WriteFile(
                                        GCServiceBase.Validator.GetHash(),
                                        lodgeName + "/" + file.Substring(rootLength + templateName.Length + "\\files\\".Length),
                                        System.Text.Encoding.UTF8.GetString(buffer, 0, (int)ms.Position));
                                }
                            }
                            lodge.LodgeChanged = false;
                            File.WriteAllText(Root + "static\\" + lodgeName + ".lastPublishedTime", DateTime.Now.ToString()+"(更新成功)");
                        }
                        catch (Exception ex)
                        {
                            //if (IsDebugging)
                            //{
                                File.AppendAllLines(LogPath, new[] { ex.ToString() + ex.StackTrace ?? "" });
                            //}
                            File.WriteAllText(Root + "static\\" + lodgeName + ".lastPublishedTime", DateTime.Now.ToString() + "(更新失败:" + (ex.HelpLink ?? "模版错误") + ")");
                        }
                        //Thread.Sleep(2000);
                        lock (Global.Pending)
                        {
                            Publishing = null;
                        }
                    }
                }
                Thread.Sleep(500);
                //Thread.Sleep(5000);
            }
            ms.Close();
            ms.Dispose();
            Global.threadStopping = false;
        }
        protected static void runJsOnRoot(string jsPath, JsonObject root, JsonObject newRoot)
        {
            var fi = new FileInfo(jsPath);
            var renderCachePath = "";
            if (!fi.Exists)
            {
                return;
            }
            var filename = jsPath.Substring(jsPath.LastIndexOf('\\') + 1);
            var jc = new Noesis.Javascript.JavascriptContext();
            var js = "var target;\r\nvar root=" + (IsDebugging ? (root as IJson).ToFormattedString("") : (root as IJson).ToString()) + ";\r\n" + File.ReadAllText(jsPath)
                + ";\r\n"
                + File.ReadAllText(Root + "App_Data\\comments\\RootDumper.js").Replace("{DateTime.Now}", DateTime.Now.ToString()) + File.ReadAllText(Root + "App_Data\\RootDumper.js");
            if (IsDebugging)
            {
                renderCachePath=Root + "Messages\\RenderCache\\" + (fi.Directory.Name.ToLower() == "templatescripts" ? "public." : "") + filename
                    + ".compiled.js";
                File.WriteAllText(renderCachePath,
                    File.ReadAllText(Root + "App_Data\\comments\\compiled.js").Replace("{DateTime.Now}", DateTime.Now.ToString()) +
                    js, System.Text.Encoding.Default);
            }
            try
            {
                jc.Run(js);
            }
            catch (Noesis.Javascript.JavascriptException ex)
            {
                throw (new Exception(string.Format("运行脚本“{0}”时发生错误：{1}；\r\n行号：{2}", jsPath.Substring(Root.Length), ex.Message, ex.Line)));
            }
            var newStrings = jc.GetParameter("rootNewStrings") as string;
            var newArrays = jc.GetParameter("rootNewArrays") as string;
            if (newStrings.Length != 0)
            {
                foreach (var str in newStrings.TrimEnd(',').Split(','))
                {
                    switch (str[0])
                    {
                        case 'n':
                            {
                                js = "target=root." + str.Substring(2) + ";//导出Number\r\n";
                                if (IsDebugging)
                                {
                                    File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                }
                                jc.Run(js);

                                try
                                {
                                    newRoot[str.Substring(2)] = Json.Number((int)jc.GetParameter("target"));
                                }
                                catch
                                {
                                    newRoot[str.Substring(2)] = Json.Number((double)jc.GetParameter("target"));
                                }
                            }
                            break;
                        case 's':
                            {
                                js = "target=root." + str.Substring(2) + ";//导出String\r\n";
                                if (IsDebugging)
                                {
                                    File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                }
                                jc.Run(js);
                                newRoot[str.Substring(2)] = Json.String(jc.GetParameter("target") as string);
                            }
                            break;
                        case 'b':
                            {
                                js = "target=root." + str.Substring(2) + ";//导出Boolean\r\n";
                                if (IsDebugging)
                                {
                                    File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                }
                                jc.Run(js);
                                newRoot[str.Substring(2)] = Json.Bool((bool)jc.GetParameter("target"));
                            }
                            break;
                    }
                }
            }
            if (newArrays.Length != 0)
            {
                var arrs=newArrays.TrimEnd(',').Split(',');
                for (var k = 0; k < arrs.Length; k += 3)
                {
                    var arrName=arrs[k];
                    var keys = arrs[k + 1].TrimEnd('|').Split('|');
                    var length = Convert.ToInt32(arrs[k + 2]);
                    var arr = Json.Array();
                    for (var i = 0; i < length; i++)
                    {
                        var obj = Json.Object();
                        foreach(var str in keys){
                            switch (str[0])
                            {
                                case 'n':
                                    {
                                        js = "target=root." + arrName + "[" + i + "]." + str.Substring(2) + ";//导出Number\r\n";
                                        if (IsDebugging)
                                        {
                                            File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                        }
                                        jc.Run(js);
                                        try
                                        {
                                            obj[str.Substring(2)] = Json.Number((int)jc.GetParameter("target"));
                                        }
                                        catch {
                                            obj[str.Substring(2)] = Json.Number((double)jc.GetParameter("target"));
                                        }
                                    }
                                    break;
                                case 's':
                                    {
                                        js = "target=root." + arrName + "[" + i + "]." + str.Substring(2) + ";//导出String\r\n";
                                        if (IsDebugging)
                                        {
                                            File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                        }
                                        jc.Run(js);
                                        obj[str.Substring(2)] = Json.String(jc.GetParameter("target") as string);
                                    }
                                    break;
                                case 'b':
                                    {
                                        js = "target=root." + arrName + "[" + i + "]." + str.Substring(2) + ";//导出Boolean\r\n";
                                        if (IsDebugging)
                                        {
                                            File.AppendAllText(renderCachePath, js, System.Text.Encoding.Default);
                                        }
                                        jc.Run(js);
                                        obj[str.Substring(2)] = Json.Bool((bool)jc.GetParameter("target"));
                                    }
                                    break;
                            }
                        }
                        arr.Add(obj);
                    }
                    newRoot[arrName]=arr;
                }
            }

        }
        protected void Application_Start(object sender, EventArgs e)
        {
            Global.LogPath=Server.MapPath("~/Messages/log.txt");
            Global.Root = Server.MapPath("~/");
            MFL.Transactions.TransactionData.OnlyInstance = new Lazy<MFL.Transactions.TransactionData>(() =>
            {
                return new MFL.Transactions.TransactionData(Global.Root + "App_Data\\TransactionData\\");
            }, LazyThreadSafetyMode.ExecutionAndPublication);
            Global.Pending = new Queue<string>();
            Global.thread = new Thread(Global.threadStart);
            Global.thread.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            Global.threadStopping = true;
            while (Global.threadStopping)
            {
                Thread.Sleep(100);
            }
        }
    }
}