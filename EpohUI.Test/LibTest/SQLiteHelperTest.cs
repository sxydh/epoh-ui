using EpohUI.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EpohUI.Test.LibTest
{

    [TestClass]
    public class SQLiteHelperTest
    {

        [TestMethod]
        public void SelectTest()
        {
            var req = new Dictionary<string, object>
            {
                ["sql"] = "select #p1 as i, #p2 as s, #p3 as n",
                ["args"] = new object[] { 1, "1", null },
                ["file"] = $"{typeof(SQLiteHelperTest).FullName}.sqlite"
            };
            var ret = SQLiteHelper.Execute(JsonConvert.SerializeObject(req));
            Assert.IsNotNull(ret);
        }

    }
}
