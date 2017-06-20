using JsonLib;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToJsonValueWpfTest
    {
        public ObjectToJsonValue GetService()
        {
            return new ObjectToJsonValue();
        }

        [TestMethod]
        public void TestIsNumberWithValue()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumber(10));
            Assert.IsTrue(service.IsNumber(10.99));
            Assert.IsFalse(service.IsNumber("10"));
            Assert.IsFalse(service.IsNumber(true));
        }

        [TestMethod]
        public void TestIsNumberWithType()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumber(typeof(int)));
            Assert.IsTrue(service.IsNumber(typeof(uint)));
            Assert.IsTrue(service.IsNumber(typeof(UInt64)));
            Assert.IsFalse(service.IsNumber(typeof(string)));
        }

        [TestMethod]
        public void TestIsNullable()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNullable(typeof(int?)));
            Assert.IsFalse(service.IsNullable(typeof(int)));
        }

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var result = service.ToJsonElementValue("my string");

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual("my string", ((JsonElementString) result).Value);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var g = Guid.NewGuid();
            var result = service.ToJsonElementValue(g);

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual(g.ToString(), ((JsonElementString) result).Value);
        }

        [TestMethod]
        public void TestNumber()
        {
            var service = this.GetService();

            var result = service.ToJsonElementValue(10);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var result = service.ToJsonElementValue(10.99);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(10.99, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToJsonElementValue(true);

            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
            Assert.AreEqual(true, ((JsonElementBool)result).Value);
        }

        [TestMethod]
        public void TestNullable()
        {
            var service = this.GetService();

            var result = service.ToJsonElementValue(null);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 10,
                UserName = "Marie"
            };

            var result = service.ToJsonElementValue(user);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Id"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)((JsonElementObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 10,
                UserName = "Marie"
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id","map_id")
                .SetProperty("UserName","map_username")
                .SetProperty("Email","map_email");

            var result = service.ToJsonElementValue(user, mappings);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["map_id"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)((JsonElementObject)result).Values["map_id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["map_username"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["map_username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["map_email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)result).Values["map_email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithLowerStrategy()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 10,
                UserName = "Marie"
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJsonElementValue(user, mappings);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["id"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)((JsonElementObject)result).Values["id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["username"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)result).Values["email"]).Value);
        }

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };


            var result = service.ToJsonElementValue(array);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonElementString)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonElementString)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var array = new int[] { 1,2 };


            var result = service.ToJsonElementValue(array);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(2, ((JsonElementNumber)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var array = new double[] { 1.5, 2.5 };


            var result = service.ToJsonElementValue(array);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(1.5, ((JsonElementNumber)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(2.5, ((JsonElementNumber)((JsonElementArray)result).Values[1]).Value);
        }


        [TestMethod]
        public void TestList()
        {
            var service = this.GetService();

            var array = new List<string> { "a", "b" };

            var result = service.ToJsonElementValue(array);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonElementString)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonElementString)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestListOfObjects()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var result = service.ToJsonElementValue(array);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Id"].ValueType);
            Assert.AreEqual(1,((JsonElementNumber) ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Email"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Id"].ValueType);
            Assert.AreEqual(2, ((JsonElementNumber)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["UserName"].ValueType);
            Assert.AreEqual("Pat", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestListOfObjects_WithMappings()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToJsonElementValue(array, mappings);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_username"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["map_email"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_id"].ValueType);
            Assert.AreEqual(2, ((JsonElementNumber)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_username"].ValueType);
            Assert.AreEqual("Pat", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["map_email"]).Value);
        }

        [TestMethod]
        public void TestListOfObjects_WithLowerStrategy()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJsonElementValue(array, mappings);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["username"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["email"].ValueType);
            Assert.AreEqual(null, ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[0]).Values["email"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["id"].ValueType);
            Assert.AreEqual(2, ((JsonElementNumber)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["username"].ValueType);
            Assert.AreEqual("Pat", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["username"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["age"].ValueType);
            Assert.AreEqual(20, ((JsonElementNullable)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["email"]).Value);
        }
    }
}
