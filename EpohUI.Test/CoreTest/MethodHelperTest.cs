using EpohUI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpohUI.Test.CoreTest
{

    [TestClass]
    public class MethodHelperTest
    {

        [TestMethod]
        public void InitTest()
        {
            MethodHelper.GetMethodId("", out var methodId);
            Assert.IsNotNull(methodId);
        }

    }
}
