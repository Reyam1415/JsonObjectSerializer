using JsonLib;
using JsonLib.Json;
using JsonLib.Json.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToJsonTest
    {
        public ObjectToJson GetService()
        {
            return new ObjectToJson();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var result = service.ToJson("my string");

            Assert.AreEqual("\"my string\"", result);
        }

        [TestMethod]
        public void TestString_EascapeInnerString()
        {
            var service = this.GetService();

            var result = service.ToJson("my \"escape\" string");

            Assert.AreEqual("\"my \\\"escape\\\" string\"", result);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var result = service.ToJson<string>(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var value = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var result = service.ToJson(value);

            Assert.AreEqual("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"", result);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var value = new DateTime(1990, 12, 12);
            var result = service.ToJson(value);

            Assert.AreEqual("\"12/12/1990 00:00:00\"", result);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            // number int | double | enum value 
            var service = this.GetService();

            int value = 10;

            var result = service.ToJson(value);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestNumber_WithInt64()
        {
            // number int | double | enum value 
            var service = this.GetService();

            Int64 value = 10;

            var result = service.ToJson(value);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            // number int | double | enum value 
            var service = this.GetService();

            double value = 10.5;

            var result = service.ToJson(value);

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestNumber_WithEnum()
        {
            // number int | double | enum value 
            var service = this.GetService();

            AssemblyEnum value = AssemblyEnum.Other;

            var result = service.ToJson(value);

            Assert.AreEqual("1", result);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToJson(true);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestBoolFalse()
        {
            var service = this.GetService();

            var result = service.ToJson(false);

            Assert.AreEqual("false", result);
        }

        // nullable

        [TestMethod]
        public void TestNullable_WithInt()
        {
            var service = this.GetService();

            int? value = 10;

            var result = service.ToJson(value);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestNullable_WithIntNull()
        {
            var service = this.GetService();

            int? value = null;

            var result = service.ToJson(value);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestNullable_WithGuid()
        {
            var service = this.GetService();

            Guid? value = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var result = service.ToJson(value);

            Assert.AreEqual("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"", result);
        }

        [TestMethod]
        public void TestNullable_WithGuidNull()
        {
            var service = this.GetService();

            Guid? value = null;

            var result = service.ToJson(value);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestNullable_WithBool()
        {
            var service = this.GetService();

            bool? value = true;

            var result = service.ToJson(value);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestNullable_WithBoolNull()
        {
            var service = this.GetService();

            bool? value = null;

            var result = service.ToJson(value);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestNullable_WithDateTime()
        {
            var service = this.GetService();

            DateTime? value = new DateTime(1990, 12, 12);

            var result = service.ToJson(value);

            Assert.AreEqual("\"12/12/1990 00:00:00\"", result);
        }

        [TestMethod]
        public void TestNullable_WithDateTimeNull()
        {
            var service = this.GetService();

            DateTime? value = null;

            var result = service.ToJson(value);

            Assert.AreEqual("null", result);
        }



        // to json object

        [TestMethod]
        public void TestObjectComplete()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var model = new AssemblyItem
            {
                MyGuid = g,
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var result = service.ToJson(model);

            Assert.AreEqual("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value\"},\"MyList\":[\"a\",\"b\"],\"MyArray\":[\"y\",\"z\"]}", result);
        }

        [TestMethod]
        public void TestObject_WithNullablesNotNull()
        {
            // Json nullable => value object

            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var model = new ItemNullable
            {
                MyGuid = g,
                MyInt = 1,
                MyDouble = 1.5,
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12)
            };

            var result = service.ToJson(model);

            Assert.AreEqual("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"}", result);

        }

        [TestMethod]
        public void TestObject_WithNullablesNull()
        {
            var service = this.GetService();

            var model = new ItemNullable();

            var result = service.ToJson(model);

            Assert.AreEqual("{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}", result);
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

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":10,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}", result);
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
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"map_id\":10,\"map_username\":\"Marie\",\"Age\":null,\"map_email\":null}", result);
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

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"id\":10,\"username\":\"Marie\",\"age\":null,\"email\":null}", result);
        }

        // array

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };

            var result = service.ToJson(array);

            Assert.AreEqual("[\"a\",\"b\"]", result);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var array = new int[] { 1, 2 };


            var result = service.ToJson(array);

            Assert.AreEqual("[1,2]", result);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var array = new double[] { 1.5, 2.5 };


            var result = service.ToJson(array);

            Assert.AreEqual("[1.5,2.5]", result);
        }


        [TestMethod]
        public void TestList()
        {
            var service = this.GetService();

            var array = new List<string> { "a", "b" };

            var result = service.ToJson(array);

            Assert.AreEqual("[\"a\",\"b\"]", result);
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

            var result = service.ToJson(array);

            Assert.AreEqual("[{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},{\"Id\":2,\"UserName\":\"Pat\",\"Age\":20,\"Email\":\"pat@domain.com\"}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var list = new List<AssemblyItem> {
                new AssemblyItem
                {
                    MyGuid = g,
                    MyInt = 1,
                    MyDouble = 1.5,
                    MyString = "my \"escape\" value",
                    MyBool = true,
                    MyEnum = AssemblyEnum.Other,
                    MyDate = new DateTime(1990, 12, 12),
                    MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value 1" },
                    MyList = new List<string> { "a1", "b1" },
                    MyArray = new string[] { "y1", "z1" }
                },
                new AssemblyItem
                {
                    MyGuid = g2,
                    MyInt = 2,
                    MyDouble = 2.5,
                    MyString = "my \"escape\"value 2",
                    MyBool = true,
                    MyEnum = AssemblyEnum.Default,
                    MyDate = new DateTime(1990, 10, 12),
                    MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value 2" },
                    MyList = new List<string> { "a2", "b2" },
                    MyArray = new string[] { "y2", "z2" }
                },
            };


            var result = service.ToJson(list);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my \\\"escape\\\" value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyString\":\"my \\\"escape\\\"value 2\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]", result);
        }


        [TestMethod]
        public void TestGetArray_WithArray()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var list = new AssemblyItem[] {
                new AssemblyItem
                {
                    MyGuid = g,
                    MyInt = 1,
                    MyDouble = 1.5,
                    MyString = "my \"escape\" value",
                    MyBool = true,
                    MyEnum = AssemblyEnum.Other,
                    MyDate = new DateTime(1990, 12, 12),
                    MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value 1" },
                    MyList = new List<string> { "a1", "b1" },
                    MyArray = new string[] { "y1", "z1" }
                },
                new AssemblyItem
                {
                    MyGuid = g2,
                    MyInt = 2,
                    MyDouble = 2.5,
                    MyString = "my \"escape\"value 2",
                    MyBool = true,
                    MyEnum = AssemblyEnum.Default,
                    MyDate = new DateTime(1990, 10, 12),
                    MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value 2" },
                    MyList = new List<string> { "a2", "b2" },
                    MyArray = new string[] { "y2", "z2" }
                },
            };


            var result = service.ToJson(list);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my \\\"escape\\\" value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyString\":\"my \\\"escape\\\"value 2\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var list = new List<ItemNullable> {
                new ItemNullable
                {
                    MyGuid = g,
                    MyInt = 1,
                    MyDouble = 1.5,
                    MyBool = true,
                    MyEnum = AssemblyEnum.Other,
                    MyDate = new DateTime(1990, 12, 12)
                },
                new ItemNullable
                {
                    MyGuid = g2,
                    MyInt = 2,
                    MyDouble = 2.5,
                    MyBool = true,
                    MyEnum = AssemblyEnum.Default,
                    MyDate = new DateTime(1990, 10, 12)
                },
            };


            var result = service.ToJson(list);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":true,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\"}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectNullablesNull()
        {
            var service = this.GetService();


            var list = new List<ItemNullable> {
                new ItemNullable
                {},
                new ItemNullable
                {},
            };

            var result = service.ToJson(list);

            Assert.AreEqual("[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]", result);
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

            var result = service.ToJson(array, mappings);

            Assert.AreEqual("[{\"map_id\":1,\"map_username\":\"Marie\",\"Age\":null,\"map_email\":null},{\"map_id\":2,\"map_username\":\"Pat\",\"Age\":20,\"map_email\":\"pat@domain.com\"}]", result);
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

            var result = service.ToJson(array, mappings);

            Assert.AreEqual("[{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null},{\"id\":2,\"username\":\"Pat\",\"age\":20,\"email\":\"pat@domain.com\"}]", result);
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

            var result = service.ToJson(value);

            Assert.AreEqual("{\"10\":\"value 1\",\"20\":\"value 2\"}", result);
        }

        [TestMethod]
        public void TestDictionaryIntObject()
        {
            var service = this.GetService();

            var value = new Dictionary<int, User>
            {
                { 10,  new User{ Id=1, UserName="Marie"} },
                { 20,  new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"} }
            };

            var r = JsonObjectSerializer.Stringify(value);

            var x = JsonObjectSerializer.Parse<Dictionary<int,User>>(r);

            var result = service.ToJson(value);

            Assert.AreEqual("{\"10\":{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},\"20\":{\"Id\":2,\"UserName\":\"Pat\",\"Age\":20,\"Email\":\"pat@domain.com\"}}", result);
        }

        // nillables

        [TestMethod]
        public void TestStringNillable()
        {
            var service = this.GetService();

            var result = service.ToJson<string>("my value");

            var result2 = service.ToJson<string>(null);

            Assert.AreEqual("\"my value\"", result);
            Assert.AreEqual("null", result2);
        }

        [TestMethod]
        public void TestNullableNillable()
        {
            var service = this.GetService();

            var result = service.ToJson<int?>(10);

            var result2 = service.ToJson<int?>(null);

            Assert.AreEqual("10", result);
            Assert.AreEqual("null", result2);
        }

        [TestMethod]
        public void TestObjectNillable()
        {
            var service = this.GetService();

            var result = service.ToJson<User>(new User { Id = 1, UserName = "Marie" });

            var result2 = service.ToJson<User>(null);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}", result);
            Assert.AreEqual("null", result2);
        }

        [TestMethod]
        public void TestDictionaryNillable()
        {
            var service = this.GetService();

            var result = service.ToJson<Dictionary<int, string>>(new Dictionary<int, string> { { 1, "a" } });

            var result2 = service.ToJson<Dictionary<int, string>>(null);

            var result3 = service.ToJson<Dictionary<int, int?>>(new Dictionary<int, int?> { { 1,10 }, { 2, null } });

            Assert.AreEqual("{\"1\":\"a\"}", result);
            Assert.AreEqual("null", result2);
            Assert.AreEqual("{\"1\":10,\"2\":null}", result3);
        }

        [TestMethod]
        public void TestArrayNillable()
        {
            var service = this.GetService();

            var result = service.ToJson<string[]>(new string[] { "a", "b" });

            var result2 = service.ToJson<string[]>(null);

            var result3 = service.ToJson<string[]>(new string[] { "a", "b", null });

            var result4 = service.ToJson<User[]>(new User[] { new User { Id = 1, UserName = "Marie" }, null });

            Assert.AreEqual("[\"a\",\"b\"]", result);
            Assert.AreEqual("null", result2);
            Assert.AreEqual("[\"a\",\"b\",null]", result3);
            Assert.AreEqual("[{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},null]", result4);
        }

    }

}
