using JsonLib;
using JsonLib.Json;
using JsonLib.Json.Mappings;
using JsonLib.Mappings;
using JsonLibTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest.ToObject
{
    [TestClass]
    public class JsonToObjectTest
    {
        public JsonToObject GetService()
        {
            return new JsonToObject();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var result = service.ToObject<string>("\"my value\"");

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestString_WithEscape()
        {
            var service = this.GetService();

            var result = service.ToObject<string>("\"my \\\"escape\\\" value\"");

            Assert.AreEqual("my \"escape\" value", result);
        }

        [TestMethod]
        public void TestString_WithGuid()
        {
            var service = this.GetService();

            var result = service.ToObject<Guid>("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"");

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        [TestMethod]
        public void TestString_WithDateTime()
        {
            var service = this.GetService();

            var result = service.ToObject<DateTime>("\"12/12/1990 00:00:00\"");

            Assert.AreEqual(new DateTime(1990,12,12), result);
        }


        [TestMethod]
        public void TestString_WithGuidNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<Guid?>("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"");

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        [TestMethod]
        public void TestString_WithDateTimeNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<DateTime?>("\"12/12/1990 00:00:00\"");

            Assert.AreEqual(new DateTime(1990, 12, 12), result);
        }

        [TestMethod]
        public void TestString_WithGuidNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<Guid?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestString_WithDateTimeNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<DateTime?>("null");

            Assert.AreEqual(null, result);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            var service = this.GetService();

            var result = service.ToObject<int>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var result = service.ToObject<double>("10.5");

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNumber_WithEnum()
        {
            var service = this.GetService();

            var result = service.ToObject<AssemblyEnum>("1");

            Assert.AreEqual(AssemblyEnum.Other, result);
        }

        [TestMethod]
        public void TestNumber_WithIntNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<int?>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithDoubleNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<double?>("10.5");

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNumber_WithEnumNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<AssemblyEnum?>("1");

            Assert.AreEqual(AssemblyEnum.Other, result);
        }

        [TestMethod]
        public void TestNumber_WithIntNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<int?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNumber_WithDoubleNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<double?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNumber_WithEnumNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<AssemblyEnum?>("null");

            Assert.AreEqual(null, result);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToObject<bool>("true");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestBoolNullable()
        {
            var service = this.GetService();

            var result = service.ToObject<bool?>("true");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestBoolNullableNull()
        {
            var service = this.GetService();

            var result = service.ToObject<bool?>("null");

            Assert.AreEqual(null, result);
        }

        // nullable

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

        // object

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
        public void TestObject_WithEscapeString()
        {
            var service = this.GetService();

            var json = "{\"UserName\":\"Marie \\\"Bellin\\\"\"}";

            var result = service.ToObject<User>(json);

            Assert.AreEqual("Marie \"Bellin\"", result.UserName);
        }

        [TestMethod]
        public void TestGetObject()
        {
            var service = this.GetService();

            var json = "{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my inner value\"},\"MyList\":[\"a\",\"b\"],\"MyArray\":[\"y\",\"z\"]}";

            var result = service.ToObject<AssemblyItem>(json);

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

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
        public void TestGetObject_WithNullables()
        {
            var service = this.GetService();

            var json = "{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"}";

            var result = service.ToObject<ItemNullable>(json);

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            Assert.IsNotNull(result);
            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
        }

        [TestMethod]
        public void TestGetObject_WithNullablesNull()
        {
            var service = this.GetService();

            var json = "{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}";

            var result = service.ToObject<ItemNullable>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnum);
            Assert.AreEqual(null, result.MyDate);
        }


        [TestMethod]
        public void TestObject_WithAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var json = "{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null}";

            var result = service.ToObject<User>(json, mappings);

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

            var mappings = new JsonMappingContainer();
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

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var json = "{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null}";

            var result = service.ToObject<User>(json, mappings);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        // array

        [TestMethod]
        public void TestList_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]";

            var results = service.ToObject<List<AssemblyItem>>(json);

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
        public void TestArray_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]";

            var results = service.ToObject<AssemblyItem[]>(json);

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
        public void TestList_OfObjectsNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\"}]";

            var results = service.ToObject<List<ItemNullable>>(json);

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
        public void TestArray_OfObjectsNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\"}]";

            var results = service.ToObject<ItemNullable[]>(json);

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
        public void TestList_OfObjectsNullablesNull()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]";

            var results = service.ToObject<List<ItemNullable>>(json);

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
        public void TestArray_OfObjectsNullablesNull()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]";

            var results = service.ToObject<ItemNullable[]>(json);

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
        public void TestList_OfObjectsWithAllLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":false,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var results = service.ToObject<AssemblyItem[]>(json, mappings);

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
        public void TestList_OfObjectsWithTypeLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":false,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();
            // AssemblyInner to upper

            var results = service.ToObject<List<AssemblyItem>>(json, mappings);

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
        public void TestList_OfObjectsWithMapping()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my \\\"escape\\\" value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"map_mylist\":[\"a1\",\"b1\"],\"map_myarray\":[\"y1\",\"z1\"]},{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"map_myint\":2,\"map_mydouble\":2.5,\"map_mystring\":\"my \\\"escape\\\"value 2\",\"map_mybool\":false,\"map_mynullable\":null,\"map_myenum\":0,\"map_mydate\":\"12/10/1990 00:00:00\",\"map_myobj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"map_mylist\":[\"a2\",\"b2\"],\"map_myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>()
                .SetProperty("MyGuid","map_myguid")
                .SetProperty("MyInt","map_myint")
                .SetProperty("MyDouble","map_mydouble")
                .SetProperty("MyString", "map_mystring")
                .SetProperty("MyBool", "map_mybool")
                .SetProperty("MyEnum", "map_myenum")
                .SetProperty("MyDate", "map_mydate")
                .SetProperty("MyNullable", "map_mynullable")
                .SetProperty("MyObj", "map_myobj")
                .SetProperty("MyList", "map_mylist")
                .SetProperty("MyArray", "map_myarray");

            var results = service.ToObject<List<AssemblyItem>>(json, mappings);

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
        public void TestArray_OfObjectsWithAllLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":false,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var results = service.ToObject<AssemblyItem[]>(json,mappings);

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
        public void TestArray_OfObjectsWithTypeLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":false,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();

            var results = service.ToObject<AssemblyItem[]>(json, mappings);

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
        public void TestArray_OfObjectsWithMapping()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my \\\"escape\\\" value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"map_mylist\":[\"a1\",\"b1\"],\"map_myarray\":[\"y1\",\"z1\"]},{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"map_myint\":2,\"map_mydouble\":2.5,\"map_mystring\":\"my \\\"escape\\\"value 2\",\"map_mybool\":false,\"map_mynullable\":null,\"map_myenum\":0,\"map_mydate\":\"12/10/1990 00:00:00\",\"map_myobj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"map_mylist\":[\"a2\",\"b2\"],\"map_myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>()
                .SetProperty("MyGuid", "map_myguid")
                .SetProperty("MyInt", "map_myint")
                .SetProperty("MyDouble", "map_mydouble")
                .SetProperty("MyString", "map_mystring")
                .SetProperty("MyBool", "map_mybool")
                .SetProperty("MyEnum", "map_myenum")
                .SetProperty("MyDate", "map_mydate")
                .SetProperty("MyNullable", "map_mynullable")
                .SetProperty("MyObj", "map_myobj")
                .SetProperty("MyList", "map_mylist")
                .SetProperty("MyArray", "map_myarray");

            var results = service.ToObject<AssemblyItem[]>(json, mappings);

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


        // dictionary

        [TestMethod]
        public void TestDictionaryIntString()
        {
            var service = this.GetService();

            var json = "{\"10\":\"value 1\",\"20\":\"value 2\"}";

            var result = service.ToObject<Dictionary<int, string>>(json);

            Assert.AreEqual("value 1", result[10]);
            Assert.AreEqual("value 2", result[20]);
        }

        [TestMethod]
        public void TestDictionaryStringString()
        {
            var service = this.GetService();

            var json = "{\"key1\":\"value 1\",\"key2\":\"value 2\"}";

            var result = service.ToObject<Dictionary<string, string>>(json);

            Assert.AreEqual("value 1", result["key1"]);
            Assert.AreEqual("value 2", result["key2"]);
        }

        [TestMethod]
        public void TestDictionaryStringString_WithKeyNumber()
        {
            var service = this.GetService();

            var json = "{\"10\":\"value 1\",\"20\":\"value 2\"}";

            var result = service.ToObject<Dictionary<string, string>>(json);

            Assert.AreEqual("value 1", result["10"]);
            Assert.AreEqual("value 2", result["20"]);
        }

        [TestMethod]
        public void TestDictionaryIntObject()
        {
            var service = this.GetService();

            var json = "{\"10\":{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},\"20\":{\"Id\":2,\"UserName\":\"Pat\",\"Age\":20,\"Email\":\"pat@domain.com\"}}";

            var result = service.ToObject<Dictionary<int, User>>(json);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result[10].Id);
            Assert.AreEqual("Marie", result[10].UserName);
            Assert.AreEqual(null, result[10].Age);
            Assert.AreEqual(null, result[10].Email);

            Assert.AreEqual(2, result[20].Id);
            Assert.AreEqual("Pat", result[20].UserName);
            Assert.AreEqual(20, result[20].Age);
            Assert.AreEqual("pat@domain.com", result[20].Email);
        }

        [TestMethod]
        public void TestDictionaryIntObject_WithAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();


            var json = "{\"10\":{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null},\"20\":{\"id\":2,\"username\":\"Pat\",\"age\":20,\"email\":\"pat@domain.com\"}}";

            var result = service.ToObject<Dictionary<int, User>>(json, mappings);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result[10].Id);
            Assert.AreEqual("Marie", result[10].UserName);
            Assert.AreEqual(null, result[10].Age);
            Assert.AreEqual(null, result[10].Email);

            Assert.AreEqual(2, result[20].Id);
            Assert.AreEqual("Pat", result[20].UserName);
            Assert.AreEqual(20, result[20].Age);
            Assert.AreEqual("pat@domain.com", result[20].Email);
        }

        [TestMethod]
        public void TestDictionaryIntObject_WithMapping()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<User>().SetProperty("Id", "MapId")
                .SetProperty("UserName","MapUserName")
                .SetProperty("Email", "MapEmail");

            var json = "{\"10\":{\"MapId\":1,\"MapUserName\":\"Marie\",\"Age\":null,\"MapEmail\":null},\"20\":{\"MapId\":2,\"MapUserName\":\"Pat\",\"Age\":20,\"MapEmail\":\"pat@domain.com\"}}";

            var result = service.ToObject<Dictionary<int, User>>(json, mappings);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result[10].Id);
            Assert.AreEqual("Marie", result[10].UserName);
            Assert.AreEqual(null, result[10].Age);
            Assert.AreEqual(null, result[10].Email);

            Assert.AreEqual(2, result[20].Id);
            Assert.AreEqual("Pat", result[20].UserName);
            Assert.AreEqual(20, result[20].Age);
            Assert.AreEqual("pat@domain.com", result[20].Email);
        }
    }
}
