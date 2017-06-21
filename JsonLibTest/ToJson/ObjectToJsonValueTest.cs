using JsonLib;
using JsonLib.Mappings;
using JsonLibTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToJsonValueTest
    {
        public ObjectToJsonValue GetService()
        {
            return new ObjectToJsonValue();
        }

        // check is system type

        [TestMethod]
        public void TestIsSystemType()
        {
            var service = this.GetService();

            // namespace => System
            Assert.IsTrue(service.IsSystemType(typeof(int)));
            Assert.IsTrue(service.IsSystemType(typeof(double)));
            Assert.IsTrue(service.IsSystemType(typeof(string)));
            Assert.IsTrue(service.IsSystemType(typeof(bool)));
            Assert.IsTrue(service.IsSystemType(typeof(string[])));
            Assert.IsTrue(service.IsSystemType(typeof(Guid)));
            Assert.IsTrue(service.IsSystemType(typeof(DateTime)));

            Assert.IsTrue(service.IsSystemType(typeof(int?)));

            // namespace => System.Collections.Generic
            Assert.IsFalse(service.IsSystemType(typeof(List<string>)));
            Assert.IsFalse(service.IsSystemType(typeof(List<User>)));

            Assert.IsFalse(service.IsSystemType(typeof(User)));
            Assert.IsFalse(service.IsSystemType(typeof(AssemblyEnum)));
        }

        // check numeric

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

        // check nullable

        [TestMethod]
        public void TestIsNullable()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNullable(typeof(int?)));
            Assert.IsFalse(service.IsNullable(typeof(int)));
        }

        // check generic

        [TestMethod]
        public void TestIsGeneric()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsGenericType(typeof(MyItemGeneric<string>)));
            Assert.IsTrue(service.IsGenericType(typeof(List<string>)));
            Assert.IsTrue(service.IsGenericType(typeof(List<User>)));
            Assert.IsFalse(service.IsGenericType(typeof(User)));
            Assert.IsFalse(service.IsGenericType(typeof(int)));
        }

        // check enum

        [TestMethod]
        public void TestIsEnum()
        {
            var service = this.GetService();

            var propertyMyEnum = typeof(AssemblyItem).GetProperty("MyEnum");
            var propertyMyString = typeof(AssemblyItem).GetProperty("MyString");

            Assert.IsTrue(service.IsEnum(propertyMyEnum.PropertyType));
            Assert.IsFalse(service.IsEnum(propertyMyString.PropertyType));
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

            var mapping = new TypeMapping(typeof(User)).SetProperty("UserName","map_username");

            var result = service.GetJsonPropertyName("UserName", true, mapping);

            Assert.AreEqual("username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithLowerMapping()
        {
            var service = this.GetService();

            var mapping = new TypeMapping(typeof(User)).SetToLowerCaseStrategy();

            var result = service.GetJsonPropertyName("UserName", false, mapping);

            Assert.AreEqual("username", result);
        }

        [TestMethod]
        public void TestGetJsonPropertyName_WithMapping()
        {
            var service = this.GetService();

            var mapping = new TypeMapping(typeof(User)).SetProperty("UserName", "map_username");

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
            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
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

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual("my string", ((JsonElementString) result).Value);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var result = service.ToJsonValue<string>(null);

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual(null, ((JsonElementString)result).Value);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var value = Guid.NewGuid();
            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual(value.ToString(), ((JsonElementString) result).Value);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var value = new DateTime(1990,12,12);
            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual(value.ToString(), ((JsonElementString)result).Value);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            // number int | double | enum value 
            var service = this.GetService();

            int value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithInt64()
        {
            // number int | double | enum value 
            var service = this.GetService();

            Int64 value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            // number int | double | enum value 
            var service = this.GetService();

            double value = 10.5;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithEnum()
        {
            // number int | double | enum value 
            var service = this.GetService();

            AssemblyEnum value = AssemblyEnum.Other;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)result).Value);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToJsonValue(true);

            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
            Assert.AreEqual(true, ((JsonElementBool)result).Value);
        }

        // nullable

        [TestMethod]
        public void TestNullable_WithInt()
        {
            var service = this.GetService();

            int? value = 10;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(10, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithIntNull()
        {
            var service = this.GetService();

            int? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuid()
        {
            var service = this.GetService();

            Guid? value = Guid.NewGuid();

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuidNull()
        {
            var service = this.GetService();

            Guid? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBool()
        {
            var service = this.GetService();

            bool? value = true;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBoolNull()
        {
            var service = this.GetService();

            bool? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTime()
        {
            var service = this.GetService();

            DateTime? value = new DateTime(1990,12,12);

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(value, ((JsonElementNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTimeNull()
        {
            var service = this.GetService();

            DateTime? value = null;

            var result = service.ToJsonValue(value);

            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
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

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid.ToString(), ((JsonElementString)((JsonElementObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((JsonElementNumber)((JsonElementObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((JsonElementNumber)((JsonElementObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["MyString"].ValueType);
            Assert.AreEqual(model.MyString, ((JsonElementString)((JsonElementObject)result).Values["MyString"]).Value);

            Assert.AreEqual(JsonElementValueType.Bool, ((JsonElementObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((JsonElementBool)((JsonElementObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate.ToString(), ((JsonElementString)((JsonElementObject)result).Values["MyDate"]).Value);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementObject)result).Values["MyObj"].ValueType);

            var myObj = ((JsonElementObject)result).Values["MyObj"] as JsonElementObject;

            Assert.AreEqual(JsonElementValueType.String, myObj.Values["MyInnerString"].ValueType);
            Assert.AreEqual(model.MyObj.MyInnerString, ((JsonElementString) myObj.Values["MyInnerString"]).Value);

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

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid, ((JsonElementNullable)((JsonElementObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((JsonElementNullable)((JsonElementObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((JsonElementNullable)((JsonElementObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((JsonElementNullable)((JsonElementObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(model.MyEnum, ((JsonElementNullable)((JsonElementObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate, ((JsonElementNullable)((JsonElementObject)result).Values["MyDate"]).Value);
        }

        [TestMethod]
        public void TestObject_WithNullablesNull()
        {
            var service = this.GetService();

            var model = new ItemNullable();

            var result = service.ToJsonValue(model);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyGuid"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyInt"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyDouble"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyBool"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyEnum"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["MyDate"]).Value);
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

            var result = service.ToJsonValue(user, mappings);

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

            var result = service.ToJsonValue(user, mappings);

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

        // array

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };


            var result = service.ToJsonValue(array);

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


            var result = service.ToJsonValue(array);

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


            var result = service.ToJsonValue(array);

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

            var result = service.ToJsonValue(array);

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

            var result = service.ToJsonValue(array);

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

            var result = service.ToJsonValue(array, mappings);

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

            var result = service.ToJsonValue(array, mappings);

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
