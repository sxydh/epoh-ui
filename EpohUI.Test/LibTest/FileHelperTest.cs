using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using EpohUI.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace EpohUI.Test.LibTest
{
    [TestClass]
    public class FileHelperTest
    {
        [TestMethod]
        public void TestWrite()
        {
            var dict = new Dictionary<string, string>
            {
                ["file"] = $"{typeof(FileHelperTest).FullName}.TestWrite",
                ["text"] = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            FileHelper.Write(JsonConvert.SerializeObject(dict));

            var file = Path.Combine(Directory.GetCurrentDirectory(), dict["file"]);
            Assert.IsTrue(File.Exists(file));
            File.Delete(file);
            Assert.IsTrue(!File.Exists(file));

            dict = new Dictionary<string, string>
            {
                ["file"] = $"{typeof(FileHelperTest).FullName}.TestWrite",
                ["text"] = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ["isAbsolute"] = "1"
            };
            FileHelper.Write(JsonConvert.SerializeObject(dict));

            file = dict["file"];
            Assert.IsTrue(File.Exists(file));
            File.Delete(file);
            Assert.IsTrue(!File.Exists(file));
        }
    }
}