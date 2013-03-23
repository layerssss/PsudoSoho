using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFLJson
{
    public static class Json
    {
        public static JsonString String(string value)
        {
            return new JsonString() { Text = value };
        }
        public static JsonString String(double value)
        {
            return new JsonString() { Text = value.ToString() };
        }
        public static JsonString String(bool value)
        {
            return new JsonString() { Text = value.ToString().ToLower() };
        }
        public static JsonArray Array(IEnumerable<IJson> values)
        {
            return Array(values.ToArray());
        }
        public static JsonArray Array(params IJson[] values)
        {
            var arr = new JsonArray();
            arr.AddRange(values);
            return arr;
        }
        public static string EscapeString(string value)
        {
            return (value ?? "").Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
        }
        public static JsonObject Object(params object[] kvps)
        {
            var obj = new JsonObject();
            for (int i = 0; i < kvps.Length; i += 2)
            {
                obj.Add(kvps[i] as string, kvps[i + 1] as IJson);
            }
            return obj;
        }
        public static JsonNumber Number(double value)
        {
            return new JsonNumber() { Value = value };
        }
        public static JsonBool True()
        {
            return new JsonBool() { Value = true };
        }
        public static JsonBool False()
        {
            return new JsonBool() { Value = false };
        }
        public static JsonBool Bool(bool value)
        {
            return new JsonBool() { Value = value };
        }
    }
}
