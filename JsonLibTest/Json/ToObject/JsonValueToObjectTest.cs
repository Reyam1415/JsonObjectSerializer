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
    public class JsonValueToObjectTest
    {
        public JsonValueToObject GetService()
        {
            return new JsonValueToObject();
        }

        // json string

        [TestMethod]
        public void TestValue_WithString()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateString("my value");

            var result = service.ToValue(typeof(string), jsonValue);

            Assert.AreEqual("my value", result);
        }


        [TestMethod]
        public void TestValue_WtithStringNull()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateString(null);

            var result = service.ToValue(typeof(string), jsonValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestValue_WithGuid()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var jsonValue = JsonValue.CreateString(g.ToString());

            var result = service.ToValue(typeof(Guid), jsonValue);

            Assert.AreEqual(g, result);
        }

        [TestMethod]
        public void TestValue_WithGuidNullable()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var jsonValue = JsonValue.CreateString(g.ToString());

            var result = service.ToValue(typeof(Guid?), jsonValue);

            Assert.AreEqual(g, result);
        }

        [TestMethod]
        public void TestValue_WithGuidNullableAndNull()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateString(null);

            var result = service.ToValue(typeof(Guid?), jsonValue);

            Assert.AreEqual(null, result);
        }


        [TestMethod]
        public void TestValue_WithDateTime()
        {
            var service = this.GetService();

            // "\"12/12/1990 00:00:00\""
            var value = new DateTime(1990, 12, 12);

            var jsonValue = JsonValue.CreateString(value.ToString());

            var result = service.ToValue(typeof(DateTime), jsonValue);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TestValue_WithDateTimeNullable()
        {
            var service = this.GetService();

            var value = new DateTime(1990, 12, 12);

            var jsonValue = JsonValue.CreateString(value.ToString());

            var result = service.ToValue(typeof(DateTime?), jsonValue);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TestValue_WithDateTimeNullableNull()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateString(null);

            var result = service.ToValue(typeof(DateTime?), jsonValue);

            Assert.AreEqual(null, result);
        }


        // json  number

        [TestMethod]
        public void TestNumber()
        {
            // json value (object) int or double (no null) => property could be numeric, nullable, or not numeric

            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10);

            var result = service.ToValue(typeof(int), jsonValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithNullable()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10);

            var result = service.ToValue(typeof(int?), jsonValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10.5);

            var result = service.ToValue(typeof(double), jsonValue);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNumber_WithDoubleNullable()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10.5);

            var result = service.ToValue(typeof(double?), jsonValue);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNumber_WithIntConversion()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10);

            var result = service.ToValue(typeof(Int64), jsonValue);

            Assert.AreEqual((Int64)10, result);
        }

        [TestMethod]
        public void TestGuessNumberFromJson_ToPropertyString()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNumber(10);

            var result = service.ToValue(typeof(string), jsonValue);

            Assert.AreEqual("10", result);
        }

        // bool

        [TestMethod]
        public void TestBool_WithTrue()
        {
            // json value true | false => property could be bool, nullable, or not bool
            // can not be null else is JsonElementNullable from Json

            var service = this.GetService();

            var jsonValue = JsonValue.CreateBool(true);

            var result = service.ToValue(typeof(bool), jsonValue);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestBool_WithFalse()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateBool(false);

            var result = service.ToValue(typeof(bool), jsonValue);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestGuessBool_ToPropertyString()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateBool(true);

            var result = service.ToValue(typeof(string), jsonValue);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGuessBool_ToPropertyString_WithFalse()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateBool(false);

            var result = service.ToValue(typeof(string), jsonValue);

            Assert.AreEqual("false", result);
        }

        [TestMethod]
        public void TestBoolNullable()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateBool(true);

            var result = service.ToValue(typeof(bool?), jsonValue);

            Assert.AreEqual(true, result);
        }

        // nullable

        [TestMethod]
        public void TestNullable()
        {
            // int, Int64, double ... DateTime, Guid,Enum

            var service = this.GetService();

            var jsonValue = JsonValue.CreateNullable(10);

            var result = service.ToValue(typeof(int?), jsonValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNullable_WithNull()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNullable(null);

            var result = service.ToValue(typeof(int?), jsonValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNullable_ConvertNumber()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateNullable(10);

            var result = service.ToValue(typeof(Int64?), jsonValue);

            Assert.AreEqual((Int64)10, result);
        }

        [TestMethod]
        public void TestNullable_WithGuid()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var jsonValue = JsonValue.CreateNullable(g);

            var result = service.ToValue(typeof(Guid?), jsonValue);

            Assert.AreEqual(g, result);
        }

        [TestMethod]
        public void TestNullable_WithDateTime()
        {
            var service = this.GetService();

            var t = new DateTime(1990, 12, 12);

            var jsonValue = JsonValue.CreateNullable(t);

            var result = service.ToValue(typeof(DateTime?), jsonValue);

            Assert.AreEqual(t, result);
        }

        // object

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var jsonObject = new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString())
               .AddObject("MyObj", new JsonObject().AddString("MyInnerString", "my inner value"))
               .AddArray("MyList", new JsonArray().AddString("a").AddString("b"))
               .AddArray("MyArray", new JsonArray().AddString("y").AddString("z"));

            var result = service.ToObject(typeof(AssemblyItem), jsonObject) as AssemblyItem;

            Assert.IsNotNull(result);
            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
            Assert.AreEqual("my inner value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }

        [TestMethod]
        public void TestObject_WithNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var jsonObject = new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString());

            var result = service.ToObject(typeof(ItemNullable), jsonObject) as ItemNullable;

            Assert.IsNotNull(result);
            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
        }

        [TestMethod]
        public void TestObject_WithNullablesNull()
        {
            var service = this.GetService();

            var jsonObject = new JsonObject()
               .AddNullable("MyGuid", null)
               .AddNullable("MyInt", null)
               .AddNullable("MyDouble", null)
               .AddNullable("MyBool", null)
               .AddNullable("MyEnum", null)
               .AddNullable("MyDate", null);

            var result = service.ToObject(typeof(ItemNullable), jsonObject) as ItemNullable;

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnum);
            Assert.AreEqual(null, result.MyDate);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateObject()
                .AddNumber("map_id", 1)
                .AddString("map_username", "Marie");

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithMappingNotFound_Fail()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie");

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
            Assert.AreEqual(null, result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithLowerStrategy()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie");

            var mappings = new JsonMappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithInnerList()
        {
            var service = this.GetService();

            var jsonValue = JsonValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie")
                .AddArray("Strings", JsonValue.CreateArray().AddString("a").AddString("b"));

            var mappings = new JsonMappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var result = service.ToObject(typeof(UserWithInnerAndList), jsonValue, mappings) as UserWithInnerAndList;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual("a", result.Strings[0]);
            Assert.AreEqual("b", result.Strings[1]);

        }

        [TestMethod]
        public void TestObjectGuessTypes_ToPropertyTypes()
        {
            // guess number, bool

            var service = this.GetService();

            var jsonObject = new JsonObject()
               .AddNumber("MyIntString", 10)
               .AddBool("MyBoolString", true);

            var result = service.ToObject(typeof(MyItemGuess), jsonObject) as MyItemGuess;

            Assert.IsNotNull(result);
            Assert.AreEqual("10", result.MyIntString);
            Assert.AreEqual("true", result.MyBoolString);
        }

        // find property

        [TestMethod]
        public void TestFindProperty_WithAllLower()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "username", true);

            Assert.IsNotNull(result);
            Assert.AreEqual("UserName", result.Name);
            Assert.AreEqual(typeof(string), result.PropertyType);
        }

        [TestMethod]
        public void TestFindProperty_WithTypeLower()
        {
            var service = this.GetService();

            var mapping = new JsonTypeMapping(typeof(User)).SetToLowerCaseStrategy();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "username", false, mapping);

            Assert.IsNotNull(result);
            Assert.AreEqual("UserName", result.Name);
            Assert.AreEqual(typeof(string), result.PropertyType);
        }

        [TestMethod]
        public void TestFindProperty_WithMapping()
        {
            var service = this.GetService();

            var mapping = new JsonTypeMapping(typeof(User)).SetProperty("UserName","map_username");

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "map_username", false, mapping);

            Assert.IsNotNull(result);
            Assert.AreEqual("UserName", result.Name);
            Assert.AreEqual(typeof(string), result.PropertyType);
        }

        [TestMethod]
        public void TestFindProperty_NoAllLowerAndNoMappingIfFound_ReturnProperty()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "UserName", false);

            Assert.IsNotNull(result);
            Assert.AreEqual("UserName", result.Name);
            Assert.AreEqual(typeof(string), result.PropertyType);
        }

        [TestMethod]
        public void TestFindProperty_NoAllLowerAndNoMappingIfNotFound_ReturnNull()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "map_username", false);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestFindPropertySimpleFind()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "UserName");

            Assert.IsNotNull(result);
            Assert.AreEqual("UserName", result.Name);
            Assert.AreEqual(typeof(string), result.PropertyType);
        }


        [TestMethod]
        public void TestFindPropertySimpleNotFind()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var result = service.FindProperty(properties, "map_username");

            Assert.IsNull(result);
        }

        // list

        [TestMethod]
        public void TestList_WithStrings()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToList(typeof(List<string>), jsonValue) as List<string>;

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestList_WithDateTimes()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("12/12/1990 00:00:00")
                .AddString("12/10/1990 00:00:00");

            var result = service.ToList(typeof(List<DateTime>), jsonValue) as List<DateTime>;

            Assert.AreEqual(new DateTime(1990,12,12), result[0]);
            Assert.AreEqual(new DateTime(1990, 10, 12), result[1]);
        }

        [TestMethod]
        public void TestList_WithGuids()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585176")
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585178");

            var result = service.ToList(typeof(List<Guid>), jsonValue) as List<Guid>;

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result[0]);
            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178"), result[1]);
        }

        [TestMethod]
        public void TestList_WithInts()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1)
                .AddNumber(2);

            var result = service.ToList(typeof(List<int>), jsonValue) as List<int>;

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestList_WithDoubles()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToList(typeof(List<double>), jsonValue) as List<double>;

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestList_WithBools()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddBool(true)
                .AddBool(false);

            var result = service.ToList(typeof(List<bool>), jsonValue) as List<bool>;

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestList_WithIntsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1)
                .AddNumber(2);

            var result = service.ToList(typeof(List<int?>), jsonValue) as List<int?>;

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestList_WithDoublesNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToList(typeof(List<double?>), jsonValue) as List<double?>;

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestList_WithBoolsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddBool(true)
                .AddBool(false);

            var result = service.ToList(typeof(List<bool?>), jsonValue) as List<bool?>;

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestList_WithDateTimesNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("12/12/1990 00:00:00")
                .AddString("12/10/1990 00:00:00");

            var result = service.ToList(typeof(List<DateTime?>), jsonValue) as List<DateTime?>;

            Assert.AreEqual(new DateTime(1990, 12, 12), result[0]);
            Assert.AreEqual(new DateTime(1990, 10, 12), result[1]);
        }

        [TestMethod]
        public void TestList_WithGuidsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585176")
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585178");

            var result = service.ToList(typeof(List<Guid?>), jsonValue) as List<Guid?>;

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result[0]);
            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178"), result[1]);
        }

        // null

        [TestMethod]
        public void TestList_WithIntsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNullable(null)
                .AddNullable(null);

            var result = service.ToList(typeof(List<int?>), jsonValue) as List<int?>;

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestList_WithDoublesNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
              .AddNullable(null)
              .AddNullable(null);

            var result = service.ToList(typeof(List<double?>), jsonValue) as List<double?>;

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestList_WithBoolsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
               .AddNullable(null)
               .AddNullable(null);

            var result = service.ToList(typeof(List<bool?>), jsonValue) as List<bool?>;

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestList_WithDateTimesNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
               .AddNullable(null)
               .AddNullable(null);

            var result = service.ToList(typeof(List<DateTime?>), jsonValue) as List<DateTime?>;

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestList_WithGuidsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNullable(null)
                .AddNullable(null);

            var result = service.ToList(typeof(List<Guid?>), jsonValue) as List<Guid?>;

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }


        // array

        [TestMethod]
        public void TestArray_WithStrings()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToArray(typeof(string[]), jsonValue) as string[];

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestArray_WithDateTimes()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("12/12/1990 00:00:00")
                .AddString("12/10/1990 00:00:00");

            var result = service.ToArray(typeof(DateTime[]), jsonValue) as DateTime[];

            Assert.AreEqual(new DateTime(1990, 12, 12), result[0]);
            Assert.AreEqual(new DateTime(1990, 10, 12), result[1]);
        }

        [TestMethod]
        public void TestArray_WithGuids()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585176")
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585178");

            var result = service.ToArray(typeof(Guid[]), jsonValue) as Guid[];

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result[0]);
            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178"), result[1]);
        }

        [TestMethod]
        public void TestArray_WithInts()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1)
                .AddNumber(2);

            var result = service.ToArray(typeof(int[]), jsonValue) as int[];

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToArray(typeof(double[]), jsonValue) as double[];

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestArray_WithBools()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddBool(true)
                .AddBool(false);

            var result = service.ToArray(typeof(bool[]), jsonValue) as bool[];

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestArray_WithIntsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1)
                .AddNumber(2);

            var result = service.ToArray(typeof(int?[]), jsonValue) as int?[];

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestArray_WithDoublesNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToArray(typeof(double?[]), jsonValue) as double?[];

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestArray_WithBoolsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddBool(true)
                .AddBool(false);

            var result = service.ToArray(typeof(bool?[]), jsonValue) as bool?[];

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestArray_WithDateTimesNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("12/12/1990 00:00:00")
                .AddString("12/10/1990 00:00:00");

            var result = service.ToArray(typeof(DateTime?[]), jsonValue) as DateTime?[];

            Assert.AreEqual(new DateTime(1990, 12, 12), result[0]);
            Assert.AreEqual(new DateTime(1990, 10, 12), result[1]);
        }

        [TestMethod]
        public void TestArray_WithGuidsNullables()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585176")
                .AddString("344ac1a2-9613-44d7-b64c-8d45b4585178");

            var result = service.ToArray(typeof(Guid?[]), jsonValue) as Guid?[];

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result[0]);
            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178"), result[1]);
        }

        // null

        [TestMethod]
        public void TestArray_WithIntsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNullable(null)
                .AddNullable(null);

            var result = service.ToArray(typeof(int?[]), jsonValue) as int?[];

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestArray_WithDoublesNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
              .AddNullable(null)
              .AddNullable(null);

            var result = service.ToArray(typeof(double?[]), jsonValue) as double?[];

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestArray_WithBoolsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
               .AddNullable(null)
               .AddNullable(null);

            var result = service.ToArray(typeof(bool?[]), jsonValue) as bool?[];

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestArray_WithDateTimesNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
               .AddNullable(null)
               .AddNullable(null);

            var result = service.ToArray(typeof(DateTime?[]), jsonValue) as DateTime?[];

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestArray_WithGuidsNullablesNull()
        {
            // list of number, string bool, nullables, object, array

            var service = this.GetService();

            var jsonValue = JsonValue.CreateArray()
                .AddNullable(null)
                .AddNullable(null);

            var result = service.ToArray(typeof(Guid?[]), jsonValue) as Guid?[];

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }


        [TestMethod]
        public void TestArrayGuessTypes_ToPropertyTypes()
        {
            // guess number, bool

            var service = this.GetService();

            var jsonArray = new JsonArray()
                .AddObject(new JsonObject()
                   .AddNumber("MyIntString", 10)
                   .AddBool("MyBoolString", true))
               .AddObject(new JsonObject()
                   .AddNumber("MyIntString", 20)
                   .AddBool("MyBoolString", false));

            var result = service.ToArray(typeof(MyItemGuess[]), jsonArray) as MyItemGuess[];

            Assert.IsNotNull(result);

            Assert.AreEqual("10", result[0].MyIntString);
            Assert.AreEqual("true", result[0].MyBoolString);

            Assert.AreEqual("20", result[1].MyIntString);
            Assert.AreEqual("false", result[1].MyBoolString);
        }


        [TestMethod]
        public void TestListGuessTypes_ToPropertyTypes()
        {
            // guess number, bool

            var service = this.GetService();

            var jsonArray = new JsonArray()
                .AddObject(new JsonObject()
                   .AddNumber("MyIntString", 10)
                   .AddBool("MyBoolString", true))
               .AddObject(new JsonObject()
                   .AddNumber("MyIntString", 20)
                   .AddBool("MyBoolString", false));

            var result = service.ToList(typeof(List<MyItemGuess>), jsonArray) as List<MyItemGuess>;

            Assert.IsNotNull(result);

            Assert.AreEqual("10", result[0].MyIntString);
            Assert.AreEqual("true", result[0].MyBoolString);

            Assert.AreEqual("20", result[1].MyIntString);
            Assert.AreEqual("false", result[1].MyBoolString);
        }

        // list of objects


        [TestMethod]
        public void TestListOfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString())
               .AddObject("MyObj", new JsonObject().AddString("MyInnerString", "my \"inner\" value 1"))
               .AddArray("MyList", new JsonArray().AddString("a1").AddString("b1"))
               .AddArray("MyArray", new JsonArray().AddString("y1").AddString("z1")));

            jsonArray.AddObject(new JsonObject()
              .AddString("MyGuid", g2.ToString())
              .AddNumber("MyInt", 2)
              .AddNumber("MyDouble", 2.5)
              .AddBool("MyBool", false)
              .AddNumber("MyEnum", 0)
              .AddString("MyDate", t2.ToString())
              .AddObject("MyObj", new JsonObject().AddString("MyInnerString", "my \"inner\" value 2"))
              .AddArray("MyList", new JsonArray().AddString("a2").AddString("b2"))
              .AddArray("MyArray", new JsonArray().AddString("y2").AddString("z2")));

            var results = service.ToList(typeof(List<AssemblyItem>), jsonArray) as List<AssemblyItem>;

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
            Assert.AreEqual("my \"inner\" value 1", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a1", result.MyList[0]);
            Assert.AreEqual("b1", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y1", result.MyArray[0]);
            Assert.AreEqual("z1", result.MyArray[1]);

            var result2 = results[1];

            Assert.AreEqual(g2, result2.MyGuid);
            Assert.AreEqual(2, result2.MyInt);
            Assert.AreEqual(2.5, result2.MyDouble);
            Assert.AreEqual(false, result2.MyBool);
            Assert.AreEqual(AssemblyEnum.Default, result2.MyEnum);
            Assert.AreEqual(t2, result2.MyDate);
            Assert.AreEqual("my \"inner\" value 2", result2.MyObj.MyInnerString);
            Assert.AreEqual(2, result2.MyList.Count);
            Assert.AreEqual("a2", result2.MyList[0]);
            Assert.AreEqual("b2", result2.MyList[1]);
            Assert.AreEqual(2, result2.MyArray.Length);
            Assert.AreEqual("y2", result2.MyArray[0]);
            Assert.AreEqual("z2", result2.MyArray[1]);
        }


        [TestMethod]
        public void TestListOfObjectsWithNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString()));

            jsonArray.AddObject(new JsonObject()
              .AddString("MyGuid", g2.ToString())
              .AddNumber("MyInt", 2)
              .AddNumber("MyDouble", 2.5)
              .AddBool("MyBool", false)
              .AddNumber("MyEnum", 0)
              .AddString("MyDate", t2.ToString()));

            var results = service.ToList(typeof(List<ItemNullable>), jsonArray) as List<ItemNullable>;

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);

            var result2 = results[1];

            Assert.AreEqual(g2, result2.MyGuid);
            Assert.AreEqual(2, result2.MyInt);
            Assert.AreEqual(2.5, result2.MyDouble);
            Assert.AreEqual(false, result2.MyBool);
            Assert.AreEqual(AssemblyEnum.Default, result2.MyEnum);
            Assert.AreEqual(t2, result2.MyDate);
        }

        [TestMethod]
        public void TestListOfObjectsWithNullablesNull()
        {
            var service = this.GetService();

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddNullable("MyGuid", null)
               .AddNullable("MyInt", null)
               .AddNullable("MyDouble", null)
               .AddNullable("MyBool", null)
               .AddNullable("MyEnum", null)
               .AddNullable("MyDate", null));

            jsonArray.AddObject(new JsonObject()
                .AddNullable("MyGuid", null)
                .AddNullable("MyInt", null)
                .AddNullable("MyDouble", null)
                .AddNullable("MyBool", null)
                .AddNullable("MyEnum", null)
                .AddNullable("MyDate", null));

            var results = service.ToList(typeof(List<ItemNullable>), jsonArray) as List<ItemNullable>;

            var result = results[0];

            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnum);
            Assert.AreEqual(null, result.MyDate);

            var result2 = results[1];

            Assert.AreEqual(null, result2.MyGuid);
            Assert.AreEqual(null, result2.MyInt);
            Assert.AreEqual(null, result2.MyDouble);
            Assert.AreEqual(null, result2.MyBool);
            Assert.AreEqual(null, result2.MyEnum);
            Assert.AreEqual(null, result2.MyDate);
        }

        [TestMethod]
        public void TestArrayOfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString())
               .AddObject("MyObj", new JsonObject().AddString("MyInnerString", "my \"inner\" value 1"))
               .AddArray("MyList", new JsonArray().AddString("a1").AddString("b1"))
               .AddArray("MyArray", new JsonArray().AddString("y1").AddString("z1")));

            jsonArray.AddObject(new JsonObject()
              .AddString("MyGuid", g2.ToString())
              .AddNumber("MyInt", 2)
              .AddNumber("MyDouble", 2.5)
              .AddBool("MyBool", false)
              .AddNumber("MyEnum", 0)
              .AddString("MyDate", t2.ToString())
              .AddObject("MyObj", new JsonObject().AddString("MyInnerString", "my \"inner\" value 2"))
              .AddArray("MyList", new JsonArray().AddString("a2").AddString("b2"))
              .AddArray("MyArray", new JsonArray().AddString("y2").AddString("z2")));

            var results = service.ToArray(typeof(AssemblyItem[]), jsonArray) as AssemblyItem[];

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
            Assert.AreEqual("my \"inner\" value 1", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a1", result.MyList[0]);
            Assert.AreEqual("b1", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y1", result.MyArray[0]);
            Assert.AreEqual("z1", result.MyArray[1]);

            var result2 = results[1];

            Assert.AreEqual(g2, result2.MyGuid);
            Assert.AreEqual(2, result2.MyInt);
            Assert.AreEqual(2.5, result2.MyDouble);
            Assert.AreEqual(false, result2.MyBool);
            Assert.AreEqual(AssemblyEnum.Default, result2.MyEnum);
            Assert.AreEqual(t2, result2.MyDate);
            Assert.AreEqual("my \"inner\" value 2", result2.MyObj.MyInnerString);
            Assert.AreEqual(2, result2.MyList.Count);
            Assert.AreEqual("a2", result2.MyList[0]);
            Assert.AreEqual("b2", result2.MyList[1]);
            Assert.AreEqual(2, result2.MyArray.Length);
            Assert.AreEqual("y2", result2.MyArray[0]);
            Assert.AreEqual("z2", result2.MyArray[1]);
        }


        [TestMethod]
        public void TestArrayOfObjectsWithNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddString("MyGuid", g.ToString())
               .AddNumber("MyInt", 1)
               .AddNumber("MyDouble", 1.5)
               .AddBool("MyBool", true)
               .AddNumber("MyEnum", 1)
               .AddString("MyDate", t.ToString()));

            jsonArray.AddObject(new JsonObject()
              .AddString("MyGuid", g2.ToString())
              .AddNumber("MyInt", 2)
              .AddNumber("MyDouble", 2.5)
              .AddBool("MyBool", false)
              .AddNumber("MyEnum", 0)
              .AddString("MyDate", t2.ToString()));

            var results = service.ToArray(typeof(ItemNullable[]), jsonArray) as ItemNullable[];

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);

            var result2 = results[1];

            Assert.AreEqual(g2, result2.MyGuid);
            Assert.AreEqual(2, result2.MyInt);
            Assert.AreEqual(2.5, result2.MyDouble);
            Assert.AreEqual(false, result2.MyBool);
            Assert.AreEqual(AssemblyEnum.Default, result2.MyEnum);
            Assert.AreEqual(t2, result2.MyDate);
        }

        [TestMethod]
        public void TestArrayOfObjectsWithNullablesNull()
        {
            var service = this.GetService();

            var jsonArray = new JsonArray();
            jsonArray.AddObject(new JsonObject()
               .AddNullable("MyGuid", null)
               .AddNullable("MyInt", null)
               .AddNullable("MyDouble", null)
               .AddNullable("MyBool", null)
               .AddNullable("MyEnum", null)
               .AddNullable("MyDate", null));

            jsonArray.AddObject(new JsonObject()
                .AddNullable("MyGuid", null)
                .AddNullable("MyInt", null)
                .AddNullable("MyDouble", null)
                .AddNullable("MyBool", null)
                .AddNullable("MyEnum", null)
                .AddNullable("MyDate", null));

            var results = service.ToArray(typeof(ItemNullable[]), jsonArray) as ItemNullable[];

            var result = results[0];

            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnum);
            Assert.AreEqual(null, result.MyDate);

            var result2 = results[1];

            Assert.AreEqual(null, result2.MyGuid);
            Assert.AreEqual(null, result2.MyInt);
            Assert.AreEqual(null, result2.MyDouble);
            Assert.AreEqual(null, result2.MyBool);
            Assert.AreEqual(null, result2.MyEnum);
            Assert.AreEqual(null, result2.MyDate);
        }

        [TestMethod]
        public void TestEnumerable_WithListAndAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("Id", 1).AddString("UserName", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("Id", 2).AddString("UserName", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue, mappings) as List<User>;

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
        public void TestEnumerable_WithArrayAndAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("Id", 1).AddString("UserName", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("Id", 2).AddString("UserName", "Pat"));

            var result = service.ToEnumerable(typeof(User[]), jsonValue, mappings) as User[];

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
        public void TestEnumerable_WithListAndMapping()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("map_id", 1).AddString("map_username", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("map_id", 2).AddString("map_username", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue, mappings) as List<User>;

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
        public void TestEnumerable_WithArrayAndMapping()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("map_id", 1).AddString("map_username", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("map_id", 2).AddString("map_username", "Pat"));

            var result = service.ToEnumerable(typeof(User[]), jsonValue, mappings) as User[];

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
        public void TestEnumerable_WitListAndhLowerStrategy()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("id", 1).AddString("username", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("id", 2).AddString("username", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue, mappings) as List<User>;

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
        public void TestEnumerable_WithArrayAndLowerStrategy()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var jsonValue = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddNumber("id", 1).AddString("username", "Marie"))
                .AddObject(JsonValue.CreateObject().AddNumber("id", 2).AddString("username", "Pat"));

            var result = service.ToEnumerable(typeof(User[]), jsonValue, mappings) as User[];

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }
    }

    
}
