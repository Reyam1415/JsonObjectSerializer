using JsonLib.Json.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonLibTest
{
    [TestClass]
    public class CacheTest
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

            service.Set<Item>("json value", new Item { Value = "property value" });

            var result = service.Get<Item>("json value");
            var value = service.GetResult<Item>("json value");

            Assert.AreEqual("json value", result.Json);
            Assert.AreEqual(typeof(Item), result.Result.GetType());
            Assert.AreEqual("property value", ((Item)result.Result).Value);
            Assert.AreEqual("property value", ((Item)value).Value);
        }

        [TestMethod]
        public void TestGetCache_WithValues()
        {
            var service = this.GetService();

            service.Set<Item>("json value a", new Item { Value = "property value a" });
            service.Set<ItemB>("json value b", new ItemB { Value = "property value b" });

            var result = service.Get<Item>("json value a");
            var value = service.GetResult<Item>("json value a");

            Assert.AreEqual("json value a", result.Json);
            Assert.AreEqual(typeof(Item), result.Result.GetType());
            Assert.AreEqual("property value a", ((Item)result.Result).Value);
            Assert.AreEqual("property value a", ((Item)value).Value);

            var resultB = service.Get<ItemB>("json value b");
            var valueB = service.GetResult<ItemB>("json value b");

            Assert.AreEqual("json value b", resultB.Json);
            Assert.AreEqual(typeof(ItemB), resultB.Result.GetType());
            Assert.AreEqual("property value b", ((ItemB)resultB.Result).Value);
            Assert.AreEqual("property value b", ((ItemB)valueB).Value);
        }

        [TestMethod]
        public void TestNewReplaceOldValue()
        {
            var service = this.GetService();

            service.Set<Item>("json value", new Item { Value = "property value a" });

            var result = service.Get<Item>("json value");
            var value = service.GetResult<Item>("json value");

            Assert.AreEqual("json value", result.Json);
            Assert.AreEqual(typeof(Item), result.Result.GetType());
            Assert.AreEqual("property value a", ((Item)result.Result).Value);
            Assert.AreEqual("property value a", ((Item)value).Value);

            service.Set<ItemB>("json value", new ItemB { Value = "property value b" });

            Assert.AreEqual(1, service.Count);

            var resultB = service.Get<ItemB>("json value");
            var valueB = service.GetResult<ItemB>("json value");

            Assert.AreEqual("json value", resultB.Json);
            Assert.AreEqual(typeof(ItemB), resultB.Result.GetType());
            Assert.AreEqual("property value b", ((ItemB)resultB.Result).Value);
            Assert.AreEqual("property value b", ((ItemB)valueB).Value);
        }

        [TestMethod]
        public void TestNewReplaceOldValue_WithSameType()
        {
            var service = this.GetService();

            service.Set<Item>("json value", new Item { Value = "property value a" });

            var result = service.Get<Item>("json value");
            var value = service.GetResult<Item>("json value");

            Assert.AreEqual("json value", result.Json);
            Assert.AreEqual(typeof(Item), result.Result.GetType());
            Assert.AreEqual("property value a", ((Item)result.Result).Value);
            Assert.AreEqual("property value a", ((Item)value).Value);

            service.Set<ItemB>("json value", new Item { Value = "property value b" });

            Assert.AreEqual(1, service.Count);

            var resultB = service.Get<ItemB>("json value");
            var valueB = service.GetResult<ItemB>("json value");

            Assert.AreEqual("json value", resultB.Json);
            Assert.AreEqual(typeof(Item), resultB.Result.GetType());
            Assert.AreEqual("property value b", ((Item)resultB.Result).Value);
            Assert.AreEqual("property value b", ((Item)valueB).Value);
        }

        [TestMethod]
        public void TestClearCache()
        {
            var service = this.GetService();

            service.Set<Item>("jsonA", new Item { Value = "property value A" });
            service.Set<ItemB>("jsonB", new Item { Value = "property value B" });

            Assert.AreEqual(2,service.Count);
            Assert.IsTrue(service.Has<Item>("jsonA"));
            Assert.IsTrue(service.Has<ItemB>("jsonB"));

            service.Clear();

            Assert.AreEqual(0, service.Count);
            Assert.IsFalse(service.Has<Item>("jsonA"));
            Assert.IsFalse(service.Has<ItemB>("jsonB"));
        }
    }

    public class Item
    {
        public string Value { get; set; }
    }

    public class ItemB
    {
        public string Value { get; set; }
    }
}
