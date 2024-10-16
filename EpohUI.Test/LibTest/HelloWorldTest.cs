using EpohUI.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpohUI.Test.LibTest
{
    [TestClass]
    public class HelloWorldTest
    {
        [TestMethod]
        public void TestSayHello()
        {
            var ret = HelloWorld.SayHello("Jack");
            Assert.IsTrue(ret == "Hello Jack");
        }
    }
}