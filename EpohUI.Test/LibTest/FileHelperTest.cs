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
        public void TestRead()
        {
            var req = new FileReq
            {
                File = Path.Combine(Directory.GetCurrentDirectory(), $"{typeof(FileHelperTest).FullName}.TestRead"),
                Text = "1"
            };
            var reqStr = JsonConvert.SerializeObject(req);
            
            File.WriteAllText(req.File, req.Text);
            var read = FileHelper.Read(reqStr);
            Assert.IsTrue(read == "1");
            File.Delete(req.File);
            Assert.ThrowsException<FileNotFoundException>(() => FileHelper.Read(reqStr));
        }
        
        
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