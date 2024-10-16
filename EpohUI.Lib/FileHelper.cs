using System;
using System.IO;
using Newtonsoft.Json;

namespace EpohUI.Lib
{
    public class FileHelper
    {
        public static string Read(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<FileReq>(reqBody);
            return File.ReadAllText(req.File);
        }

        public static Stream ReadStream(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<FileReq>(reqBody);
            return new FileStream(req.File, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096);
        }

        public static void Write(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<FileReq>(reqBody);
            File.WriteAllText(req.File, req.Text);
        }

        public static void Delete(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<FileReq>(reqBody);
            File.Delete(req.File);
        }

        public static string Exists(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<FileReq>(reqBody);
            return File.Exists(req.File) ? "1" : "0";
        }

        public static string GetMethodIdMap()
        {
            var ret = "";
            ret += $"lib/file-read={typeof(FileHelper).FullName}#Read";
            ret += $"\r\nlib/file-read_stream={typeof(FileHelper).FullName}#ReadStream";
            ret += $"\r\nlib/file-write={typeof(FileHelper).FullName}#Write";
            ret += $"\r\nlib/file-delete={typeof(FileHelper).FullName}#Delete";
            ret += $"\r\nlib/file-exists={typeof(FileHelper).FullName}#Exists";
            return ret;
        }
    }

    public class FileReq
    {
        private string _file;
        private string _isAbsolute;

        public string File
        {
            get => IsAbsolute == "1" ? _file : Path.Combine(Directory.GetCurrentDirectory(), _file);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _file = value;
            }
        }

        public string Text { get; set; }

        public string IsAbsolute
        {
            get => string.IsNullOrEmpty(_isAbsolute) ? "0" : _isAbsolute;
            set => _isAbsolute = value;
        }
    }
}