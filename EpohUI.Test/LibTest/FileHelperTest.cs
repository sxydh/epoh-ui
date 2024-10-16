using System;
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
            /* 相对路径 */
            var req = new FileReq
            {
                File = $"{typeof(FileHelperTest).FullName}.TestWrite",
                Text = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            FileHelper.Write(JsonConvert.SerializeObject(req));

            var file = Path.Combine(Directory.GetCurrentDirectory(), req.File);
            Assert.IsTrue(File.Exists(file));
            File.Delete(file);
            Assert.IsTrue(!File.Exists(file));

            /* 绝对路径 */
            req = new FileReq
            {
                File = $"{typeof(FileHelperTest).FullName}.TestWrite",
                Text = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                IsAbsolute = "1"
            };
            FileHelper.Write(JsonConvert.SerializeObject(req));

            Assert.IsTrue(File.Exists(req.File));
            File.Delete(req.File);
            Assert.IsTrue(!File.Exists(req.File));
        }

        [TestMethod]
        public void TestDelete()
        {
            var req = new FileReq
            {
                File = $"{typeof(FileHelperTest).FullName}.TestDelete",
                Text = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            File.WriteAllText(req.File, req.Text);
            Assert.IsTrue(File.Exists(req.File));
            
            FileHelper.Delete(JsonConvert.SerializeObject(req));
            Assert.IsTrue(!File.Exists(req.File));
        }
        
        [TestMethod]
        public void TestExists()
        {
            var req = new FileReq
            {
                File = $"{typeof(FileHelperTest).FullName}.TestExists",
                Text = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            File.WriteAllText(req.File, req.Text);
            Assert.IsTrue(File.Exists(req.File));
            
            var exists = FileHelper.Exists(JsonConvert.SerializeObject(req));
            Assert.IsTrue(exists == "1");
            File.Delete(req.File);
            Assert.IsTrue(!File.Exists(req.File));
        }
    }
}