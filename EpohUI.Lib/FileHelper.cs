﻿using System.IO;

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

        public static string GetMethodIdMap()
        {
            return $"lib/file-read={typeof(FileHelper).FullName}#Read";
        }

    }
}
