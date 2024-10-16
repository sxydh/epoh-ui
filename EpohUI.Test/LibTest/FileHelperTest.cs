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
                ["file"] = Path.Combine(Directory.GetCurrentDirectory(), $"{typeof(FileHelperTest).FullName}.TestWrite"),
                ["text"] = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            FileHelper.Write(JsonConvert.SerializeObject(dict));
            Assert.IsTrue(File.Exists(dict["file"]));
            File.Delete(dict["file"]);
            Assert.IsTrue(!File.Exists(dict["file"]));
        }
    }
}