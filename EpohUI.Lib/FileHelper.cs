using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EpohUI.Lib
{
    public class FileHelper
    {
        public static string Read(string reqBody)
        {
            return File.ReadAllText(reqBody);
        }

        public static Stream ReadStream(string reqBody)
        {
            return new FileStream(reqBody, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096);
        }

        public static void Write(string reqBody)
        {
            var reqJon = JObject.Parse(reqBody);
            var file = reqJon["file"]?.ToString() ?? throw new ArgumentException("Args file cannot be null");
            var text = reqJon["text"]?.ToString() ?? throw new ArgumentException("Args text cannot be null");
            var isAbsolute = reqJon["isAbsolute"]?.ToString() ?? "0";
            File.WriteAllText(
                isAbsolute == "1" ? file : Path.Combine(Directory.GetCurrentDirectory(), file),
                text);
        }

        public static string GetMethodIdMap()
        {
            var ret = "";
            ret += $"lib/file-read={typeof(FileHelper).FullName}#Read";
            ret += $"\r\nlib/file-read_stream={typeof(FileHelper).FullName}#ReadStream";
            return ret;
        }
    }
}