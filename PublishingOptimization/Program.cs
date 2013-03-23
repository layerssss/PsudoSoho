using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using LCmd;
namespace PublishingOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            CmdLineHandler.SafeHandleArgs(typeof(Program).GetMethod("Optimize"), null, args);
        }
        public static List<string> jsMarked =new List<string>();
        public static string top = null;
        public static void Optimize(
            string pages = "*.php|*.html|*.aspx|*.master",
            string root=".\\",
            string absolutePath=".\\",
            string gcPath = ".\\compiler.jar",
            string gcCompilationLevel = "ADVANCED_OPTIMIZATIONS",

            bool cssMerge = true,
            bool cssDeleteOrigin = true,
            string cssTargetPath = ".\\Style\\{0}.css",
            string cssTargetAbsoluteUrl="/Style/{0}.css",

            bool jsMerge = true,
            bool jsDeleteOrigin = true,
            string jsTargetPath = ".\\Scripts\\{0}.js",
            string jsTargetAbsoluteUrl="/Scripts/{0}.js"
            )
        {
            if (top == null)
            {
                top = root;
            }
            foreach (var s in pages.Split('|'))
            {
                foreach (var f in Directory.GetFiles(root, s))
                {
                    var js = "";
                    Console.WriteLine(f);
                    var str2 = "";
                    var css = "";
                    bool jsAdded = false;
                    {
                        var str = File.ReadAllText(f);
                        while (true)
                        {
                            {
                                var i = str.IndexOf("<script src=\"");
                                var i2 = str.IndexOf("<script type=\"text/javascript\"");
                                if (i == -1 && i2 == -1)
                                {
                                    break;
                                }
                                if (i != -1)
                                {
                                    str2 += str.Remove(i);
                                    str = str.Substring(i);
                                }
                                else
                                {
                                    str2 += str.Remove(i2);
                                    str = str.Substring(i2);
                                }
                            }
                            if (str.StartsWith("<script src=\""))
                            {
                                str = str.Substring("<script src=\"".Length);
                                var p = str.Remove(str.IndexOf('"'));
                                if (!p.StartsWith("http://")&&!p.StartsWith("https://"))
                                {
                                    p = p.Replace('/', '\\');
                                    p = p.StartsWith("\\") ? absolutePath + p.TrimStart('\\') : root + p;

                                    js += "\r\n" + File.ReadAllText(p);
                                    jsMarked.Add(p);
                                    if (!jsAdded)
                                    {
                                        jsAdded = true;
                                        str2 += "<script src=\"" + string.Format(jsTargetAbsoluteUrl, f.Substring(top.Length).Replace('\\', '_')) + "\" type=\"text/javascript\"></script>";
                                    }
                                }
                                else
                                {
                                    str2 += "<script src=\"" + p + "\" type=\"text/javascript\"></script>";
                                }
                                str = str.Substring(str.IndexOf("</script>") + "</script>".Length);
                            }
                            if (str.StartsWith("<script type=\"text/javascript\""))
                            {
                                str = str.Substring(str.IndexOf(">") + 1);
                                js += "\r\n" + str.Remove(str.IndexOf("</script>"));
                                str = str.Substring(str.IndexOf("</script>") + "</script>".Length);

                                if (!jsAdded)
                                {
                                    jsAdded = true;
                                    str2 += "<script src=\"" + string.Format(jsTargetAbsoluteUrl, f.Substring(top.Length).Replace('\\', '_')) + "\" type=\"text/javascript\"></script>";
                                }
                            }
                        }
                        str2 += str;
                    }
                    if (js != "")
                    {
                        File.WriteAllText(gcPath + ".input.js", js, Encoding.UTF8);
                        var p = Process.Start(new ProcessStartInfo(
                            "java.exe",
                            "-jar \"" + gcPath + "\" --js \"" + gcPath + ".input.js\" --js_output_file \"" + gcPath + ".output.js\"")
                            {
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError=true
                            });
                        p.WaitForExit();
                        if (p.ExitCode != 0)
                        {
                            Console.WriteLine("GoogleClosureError:");
                            Console.WriteLine(p.StandardOutput.ReadToEnd());
                            Console.WriteLine(p.StandardError.ReadToEnd());
                            Console.ReadKey();
                        }

                        js = File.ReadAllText(gcPath + ".output.js", Encoding.UTF8);
                    }
                    {//css
                        jsAdded = false;
                        var str = str2;
                        str2 = "";
                        while (true)
                        {
                            {
                                var i = str.IndexOf("<link href=\"");
                                var i2 = str.IndexOf("<style type=\"text/css\">");
                                if (i == -1 && i2 == -1)
                                {
                                    break;
                                }
                                if (i != -1)
                                {
                                    str2 += str.Remove(i);
                                    str = str.Substring(i);
                                }
                                else
                                {
                                    str2 += str.Remove(i2);
                                    str = str.Substring(i2);
                                }
                            }
                            if (str.StartsWith("<link href=\""))
                            {
                                str = str.Substring("<link href=\"".Length);
                                var p = str.Remove(str.IndexOf('"'));
                                if (!p.StartsWith("http://"))
                                {
                                    p = p.Replace('/', '\\');
                                    p = p.StartsWith("\\") ? absolutePath + p.TrimStart('\\') : root + p;

                                    css += "\r\n" + File.ReadAllText(p);
                                    jsMarked.Add(p);
                                    if (!jsAdded)
                                    {
                                        jsAdded = true;
                                        str2 += "<link href=\"" + string.Format(cssTargetAbsoluteUrl, f.Substring(top.Length).Replace('\\', '_')) + "\" rel=\"stylesheet\" type=\"text/css\" />";
                                    }
                                }
                                else
                                {
                                    str2 += "<link href=\"" + p + "\" rel=\"stylesheet\" type=\"text/css\" />";
                                }
                                str = str.Substring(str.IndexOf("/>") + "/>".Length);
                            }
                            if (str.StartsWith("<style type=\"text/css\""))
                            {
                                str = str.Substring(str.IndexOf(">") + 1);
                                css += "\r\n" + str.Remove(str.IndexOf("</style>"));
                                str = str.Substring(str.IndexOf("</style>") + "</style>".Length);

                                if (!jsAdded)
                                {
                                    jsAdded = true;
                                    str2 += "<link href=\"" + string.Format(cssTargetAbsoluteUrl, f.Substring(top.Length).Replace('\\', '_')) + "\" rel=\"stylesheet\" type=\"text/css\" />";
                                }
                            }
                        }
                        str2 += str;
                    }
                    {
                        var last=0;
                        do
                        {
                            last=css.Length;
                            css = css.Replace('\r', ' ');
                            css = css.Replace('\n', ' ');
                            css = css.Replace('\t', ' ');
                            css = css.Replace("  ", " ");
                        }
                        while (css.Length != last);
                        css = css.Replace("{ ", "{");
                        css = css.Replace(" {", "{");

                        css = css.Replace("} ", "}");
                        css = css.Replace(" }", "}");

                        css = css.Replace("; ", ";");
                        css = css.Replace(" ;", ";");
                    }
                    if (jsMerge && (js != "" || css != ""))
                    {
                        File.WriteAllText(string.Format(jsTargetPath, f.Substring(top.Length).Replace('\\', '_')), js, Encoding.UTF8);

                        File.WriteAllText(string.Format(cssTargetPath, f.Substring(top.Length).Replace('\\', '_')), css, Encoding.UTF8);
                        File.WriteAllText(f, str2, Encoding.UTF8);
                    }


                }
            }
            foreach (var d in Directory.GetDirectories(root))
            {
                Optimize(pages, d, absolutePath, gcPath, gcCompilationLevel, cssMerge, false, cssTargetPath,cssTargetAbsoluteUrl, jsMerge, false, jsTargetPath,jsTargetAbsoluteUrl);
            }
            if (jsDeleteOrigin)
            {
                foreach (var p in jsMarked)
                {
                    File.Delete(p);
                }
            }
        }
    }
}
