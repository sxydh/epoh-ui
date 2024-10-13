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
                ["sql"] = "select #0 as i, #1 as s, #2 as n",
                ["args"] = new object[] { 1, "1", null },
                ["file"] = $"{typeof(SQLiteHelperTest).FullName}.sqlite"
            };
            var ret = SQLiteHelper.Select(JsonConvert.SerializeObject(req));
            Assert.IsNotNull(ret);
        }

    }
}
