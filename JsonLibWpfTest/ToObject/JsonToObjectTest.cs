using JsonLib;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest.ToObject
{
    [TestClass]
    public class JsonToObjectWpfTest
    {
        public JsonToObject GetService()
        {
            return new JsonToObject();
        }

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var result = service.ToObject<string>("\"my value\"");

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestNumber()
        {
            var service = this.GetService();

            var result = service.ToObject<int>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToObject<bool>("true");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<int?>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNullable_WithNull()
        {
            var service = this.GetService();

            var result = service.ToObject<int?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}";

            var result = service.ToObject<User>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetProperty("Id","map_id").SetProperty("UserName","map_username");

            var json = "{\"map_id\":1,\"map_username\":\"Marie\",\"age\":null,\"email\":null}";

            var result = service.ToObject<User>(json, mappings);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithLowerStrategy()
        {
            var service = this.GetService();

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var json = "{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null}";

            var result = service.ToObject<User>(json, mappings);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestEnumerable_WithListOfObjects()
        {
            var service = this.GetService();

            var mappings = new MappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var json = "[{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null},{\"id\":2,\"username\":\"Pat\",\"age\":null,\"email\":null}]";

            var result = service.ToObject<List<User>>(json,mappings);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }


        [TestMethod]
        public void TestObject_WithDateTime()
        {
            var service = this.GetService();

            var json = "{\"UserName\":\"Marie\",\"Birth\":\"12/12/1990 00:00:00\"}";

            var result = service.ToObject<UserWithDate>(json);

            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(new DateTime(1990, 12, 12), result.Birth);
        }

        [TestMethod]
        public void TestObject_WithEscapeString()
        {
            var service = this.GetService();

            var json = "{\"UserName\":\"Marie \\\"Bellin\\\"\"}";

            var result = service.ToObject<User>(json);

            Assert.AreEqual("Marie \"Bellin\"", result.UserName);
        }
    }
}
