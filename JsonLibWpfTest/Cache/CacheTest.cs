using JsonLib;
using JsonLibTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest
{
    [TestClass]
    public class CacheWpfTest
    {
        public JsonCacheService GetService()
        {
            return new JsonCacheService();
        }

        [TestMethod]
        public void TestCache()
        {
            var service = this.GetService();

            service.Set<Item>("json value", new Item { Value = "result value" });

            Assert.IsTrue(service.Has<Item>("json value"));

            Assert.IsFalse(service.Has<Item>("json value not registered"));
        }

        [TestMethod]
        public void TestGetCache()
        {
            var service = this.GetService();

            service.Set<Item>("json value", new Item { Value = "result value" });

            var result = service.GetResult<Item>("json value");

            Assert.AreEqual("result value", ((Item)result).Value);
        }
    }

    public class Item
    {
        public string Value { get; set; }
    }
}
