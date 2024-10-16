using EpohUI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpohUI.Test.CoreTest
{
    [TestClass]
    public class MethodHelperTest
    {
        [TestMethod]
        public void TestGetMethodId()
        {
            MethodHelper.GetMethodId("say-hello", out _);
        }
    }
}