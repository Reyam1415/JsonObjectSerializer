using JsonLib;
using JsonLib.Json;
using JsonLib.Json.Mappings;
using JsonLib.Mappings;
using JsonLibTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToJsonValueUwpTest
    {
        public ObjectToJsonValue GetService()
        {
            return new ObjectToJsonValue();
        }
       
        // get property name

        [TestMethod]
        public void TestGetJsonPropertyName_WithAllLowerAndNoMapping_ReturnsLowerName()
        {
            var service = this.GetService();

            var result = service.GetJsonPropertyName("UserName", true, null);

            Assert.AreEqual("username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithAllLowerAndMapping_DontInterfer()
        {
            var service = this.GetService();

            var mapping = new JsonTypeMapping(typeof(User)).SetProperty("UserName","map_username");

            var result = service.GetJsonPropertyName("UserName", true, mapping);

            Assert.AreEqual("username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithLowerMapping()
        {
            var service = this.GetService();

            var mapping = new JsonTypeMapping(typeof(User)).SetToLowerCaseStrategy();

            var result = service.GetJsonPropertyName("UserName", false, mapping);

            Assert.AreEqual("username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithMapping()
        {
            var service = this.GetService();

            var mapping = new JsonTypeMapping(typeof(User)).SetProperty("UserName", "map_username");

            var result = service.GetJsonPropertyName("UserName", false, mapping);

            Assert.AreEqual("map_username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithAllLowerFalseAndNoMapping_ReturnsPropertyName()
        {
            var service = this.GetService();

            var result = service.GetJsonPropertyName("UserName", false, null);

            Assert.AreEqual("UserName", result);
        }

        // to json value


        [TestMethod]
        public void TestToJsonValue_WithNoValueAndNullable()
        {
            var service = this.GetService();

            var result = service.ToJsonValue<int?>(null);
            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }

        // string

        [TestMethod]
        public void TestString()
        {
            // (System)
            // string => Json String ... value null ? null : value to string
            // Guid => Json String .. value to string
            // DateTime => Json String .. value to string

            var service = this.GetService();

            var result = service.ToJsonValue("my string");

            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual("my string", ((JsonString) result).Value);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var result = service.ToJsonValue<string>(null);

            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual(null, ((JsonString)result).Value);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var value = Guid.NewGuid();
            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual(value.ToString(), ((JsonString) result).Value);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var value = new DateTime(1990,12,12);
            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual(value.ToString(), ((JsonString)result).Value);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            // number int | double | enum value 
            var service = this.GetService();

            int value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithInt64()
        {
            // number int | double | enum value 
            var service = this.GetService();

            Int64 value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            // number int | double | enum value 
            var service = this.GetService();

            double value = 10.5;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithEnum()
        {
            // number int | double | enum value 
            var service = this.GetService();

            AssemblyEnum value = AssemblyEnum.Other;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual(1, ((JsonNumber)result).Value);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToJsonValue(true);

            Assert.AreEqual(JsonValueType.Bool, result.ValueType);
            Assert.AreEqual(true, ((JsonBool)result).Value);
        }

        // nullable

        [TestMethod]
        public void TestNullable_WithInt()
        {
            var service = this.GetService();

            int? value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(10, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithIntNull()
        {
            var service = this.GetService();

            int? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuid()
        {
            var service = this.GetService();

            Guid? value = Guid.NewGuid();

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(value, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuidNull()
        {
            var service = this.GetService();

            Guid? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBool()
        {
            var service = this.GetService();

            bool? value = true;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(value, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBoolNull()
        {
            var service = this.GetService();

            bool? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTime()
        {
            var service = this.GetService();

            DateTime? value = new DateTime(1990,12,12);

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(value, ((JsonNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTimeNull()
        {
            var service = this.GetService();

            DateTime? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }


        // to json object

        [TestMethod]
        public void TestObjectComplete()
        {
            var service = this.GetService();

            var model = new AssemblyItem
            {
                MyGuid = Guid.NewGuid(),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new AssemblyInner { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var result = service.ToJsonValue(model);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid.ToString(), ((JsonString)((JsonObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((JsonNumber)((JsonObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((JsonNumber)((JsonObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["MyString"].ValueType);
            Assert.AreEqual(model.MyString, ((JsonString)((JsonObject)result).Values["MyString"]).Value);

            Assert.AreEqual(JsonValueType.Bool, ((JsonObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((JsonBool)((JsonObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate.ToString(), ((JsonString)((JsonObject)result).Values["MyDate"]).Value);

            Assert.AreEqual(JsonValueType.Object, ((JsonObject)result).Values["MyObj"].ValueType);

            var myObj = ((JsonObject)result).Values["MyObj"] as JsonObject;

            Assert.AreEqual(JsonValueType.String, myObj.Values["MyInnerString"].ValueType);
            Assert.AreEqual(model.MyObj.MyInnerString, ((JsonString) myObj.Values["MyInnerString"]).Value);

        }

        [TestMethod]
        public void TestObject_WithNullablesNotNull()
        {
            // Json nullable => value object

            var service = this.GetService();

            var model = new ItemNullable
            {
                MyGuid = Guid.NewGuid(),
                MyInt = 1,
                MyDouble = 1.5,
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12)
            };

            var result = service.ToJsonValue(model);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid, ((JsonNullable)((JsonObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((JsonNullable)((JsonObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((JsonNullable)((JsonObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((JsonNullable)((JsonObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(model.MyEnum, ((JsonNullable)((JsonObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate, ((JsonNullable)((JsonObject)result).Values["MyDate"]).Value);
        }

        [TestMethod]
        public void TestObject_WithNullablesNull()
        {
            var service = this.GetService();

            var model = new ItemNullable();

            var result = service.ToJsonValue(model);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["MyDate"]).Value);
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

            var result = service.ToJsonValue(user);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Id"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)((JsonObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)result).Values["Email"]).Value);
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

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id","map_id")
                .SetProperty("UserName","map_username")
                .SetProperty("Email","map_email");

            var result = service.ToJsonValue(user, mappings);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["map_id"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)((JsonObject)result).Values["map_id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["map_username"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["map_username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["map_email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)result).Values["map_email"]).Value);
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

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJsonValue(user, mappings);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["id"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)((JsonObject)result).Values["id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["username"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)result).Values["email"]).Value);
        }

        // array

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };


            var result = service.ToJsonValue(array);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonString)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonString)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var array = new int[] { 1,2 };


            var result = service.ToJsonValue(array);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(2, ((JsonNumber)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var array = new double[] { 1.5, 2.5 };


            var result = service.ToJsonValue(array);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(1.5, ((JsonNumber)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(2.5, ((JsonNumber)((JsonArray)result).Values[1]).Value);
        }


        [TestMethod]
        public void TestList()
        {
            var service = this.GetService();

            var array = new List<string> { "a", "b" };

            var result = service.ToJsonValue(array);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonString)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonString)((JsonArray)result).Values[1]).Value);
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

            var result = service.ToJsonValue(array);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Object, ((JsonArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[0]).Values["Id"].ValueType);
            Assert.AreEqual(1,((JsonNumber) ((JsonObject)((JsonArray)result).Values[0]).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[0]).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)((JsonArray)result).Values[0]).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["Email"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[1]).Values["Id"].ValueType);
            Assert.AreEqual(2, ((JsonNumber)((JsonObject)((JsonArray)result).Values[1]).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["UserName"].ValueType);
            Assert.AreEqual("Pat", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[1]).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonNullable)((JsonObject)((JsonArray)result).Values[1]).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["Email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["Email"]).Value);
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

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToJsonValue(array, mappings);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Object, ((JsonArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[0]).Values["map_id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)((JsonArray)result).Values[0]).Values["map_id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["map_username"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["map_username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[0]).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)((JsonArray)result).Values[0]).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["map_email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["map_email"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[1]).Values["map_id"].ValueType);
            Assert.AreEqual(2, ((JsonNumber)((JsonObject)((JsonArray)result).Values[1]).Values["map_id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["map_username"].ValueType);
            Assert.AreEqual("Pat", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["map_username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[1]).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonNullable)((JsonObject)((JsonArray)result).Values[1]).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["map_email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["map_email"]).Value);
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

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJsonValue(array, mappings);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Object, ((JsonArray)result).Values[0].ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[0]).Values["id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)((JsonArray)result).Values[0]).Values["id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["username"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[0]).Values["age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)((JsonArray)result).Values[0]).Values["age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["email"].ValueType);
            Assert.AreEqual(null, ((JsonString)((JsonObject)((JsonArray)result).Values[0]).Values["email"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)((JsonArray)result).Values[1]).Values["id"].ValueType);
            Assert.AreEqual(2, ((JsonNumber)((JsonObject)((JsonArray)result).Values[1]).Values["id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["username"].ValueType);
            Assert.AreEqual("Pat", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["username"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)((JsonArray)result).Values[1]).Values["age"].ValueType);
            Assert.AreEqual(20, ((JsonNullable)((JsonObject)((JsonArray)result).Values[1]).Values["age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["email"]).Value);
        }

        // dictionary

        [TestMethod]
        public void TestDictionaryIntString()
        {
            var service = this.GetService();

            var value = new Dictionary<int, string>
            {
                { 10,  "value 1" },
                { 20,  "value 2" }
            };

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(2, ((JsonObject)result).Values.Count);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["10"].ValueType);
            Assert.AreEqual("value 1", ((JsonString) ((JsonObject)result).Values["10"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["20"].ValueType);
            Assert.AreEqual("value 2", ((JsonString)((JsonObject)result).Values["20"]).Value);
        }

        [TestMethod]
        public void TestDictionaryStringString()
        {
            var service = this.GetService();

            var value = new Dictionary<string, string>
            {
                { "key1",  "value 1" },
                { "key2",  "value 2" }
            };

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(2, ((JsonObject)result).Values.Count);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["key1"].ValueType);
            Assert.AreEqual("value 1", ((JsonString)((JsonObject)result).Values["key1"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["key2"].ValueType);
            Assert.AreEqual("value 2", ((JsonString)((JsonObject)result).Values["key2"]).Value);
        }

        [TestMethod]
        public void TestDictionaryStringBool()
        {
            var service = this.GetService();

            var value = new Dictionary<string, bool>
            {
                { "key1",  true },
                { "key2",  false }
            };

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(2, ((JsonObject)result).Values.Count);

            Assert.AreEqual(JsonValueType.Bool, ((JsonObject)result).Values["key1"].ValueType);
            Assert.AreEqual(true, ((JsonBool)((JsonObject)result).Values["key1"]).Value);

            Assert.AreEqual(JsonValueType.Bool, ((JsonObject)result).Values["key2"].ValueType);
            Assert.AreEqual(false, ((JsonBool)((JsonObject)result).Values["key2"]).Value);
        }

        [TestMethod]
        public void TestDictionaryStringEnum()
        {
            var service = this.GetService();

            var value = new Dictionary<string, MyEnum>
            {
                { "key1",  MyEnum.Other },
                { "key2",  MyEnum.Default }
            };

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(2, ((JsonObject)result).Values.Count);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["key1"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["key1"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["key2"].ValueType);
            Assert.AreEqual(0, ((JsonNumber)((JsonObject)result).Values["key2"]).Value);
        }

        [TestMethod]
        public void TestDictionaryStringDateTime()
        {
            var service = this.GetService();

            var value = new Dictionary<string, DateTime>
            {
                { "key1",  new DateTime(1990,12,12) },
                { "key2",  new DateTime(1990,10,12) }
            };

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(2, ((JsonObject)result).Values.Count);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["key1"].ValueType);
            Assert.AreEqual("12/12/1990 00:00:00", ((JsonString)((JsonObject)result).Values["key1"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["key2"].ValueType);
            Assert.AreEqual("12/10/1990 00:00:00", ((JsonString)((JsonObject)result).Values["key2"]).Value);
        }

    }


    public class ItemNullable
    {
        public Guid? MyGuid { get; set; }
        public int? MyInt { get; set; }
        public double? MyDouble { get; set; }
        public bool? MyBool { get; set; }
        public AssemblyEnum? MyEnum { get; set; }
        public DateTime? MyDate { get; set; }
    }

    public class MyItemGeneric<T>
    {

    }
}
