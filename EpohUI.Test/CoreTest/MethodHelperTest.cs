using EpohUI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpohUI.Test.CoreTest
{

    [TestClass]
    public class MethodHelperTest
    {

        [TestMethod]
        public void GetMethodIdTest()
        {
            MethodHelper.GetMethodId("say-hello", out var methodId);
            Assert.IsNotNull(methodId);
        }

        [TestMethod]
        public void InvokeTest()
        {
            MethodHelper.GetMethodId("say-hello", out var methodId);
            Assert.IsNotNull(methodId);
            var ret = MethodHelper.Invoke(methodId, "Jack");
            Assert.IsNotNull(ret);
        }

    }
}
