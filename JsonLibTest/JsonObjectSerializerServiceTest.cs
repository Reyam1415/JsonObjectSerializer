using JsonLib;
using JsonLib.Json.Mappings;
using JsonLib.Mappings;
using JsonLib.Mappings.Xml;
using JsonLib.Xml;
using JsonLibTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest
{
    [TestClass]
    public class JsonObjectSerializerServiceTest
    {
        public JsonObjectSerializerService GetService()
        {
            return new JsonObjectSerializerService();
        }


        // stringify

        [TestMethod]
        public void TestStringify_WithString()
        {
            var service = this.GetService();

            var result = service.Stringify("my value");

            Assert.AreEqual("\"my value\"", result);
        }

        [TestMethod]
        public void TestStringify_WithEscapeString()
        {
            var service = this.GetService();

            var result = service.Stringify("my \"escape\" value");

            Assert.AreEqual("\"my \\\"escape\\\" value\"", result);
        }

        [TestMethod]
        public void TestStringify_WithDateTime()
        {
            var service = this.GetService();

            var t = new DateTime(1990, 12, 12);

            var result = service.Stringify(t);

            Assert.AreEqual("\"12/12/1990 00:00:00\"", result);
        }

        [TestMethod]
        public void TestStringify_WithGuid()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var result = service.Stringify(g);

            Assert.AreEqual("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"", result);
        }

        [TestMethod]
        public void TestStringify_WithInt()
        {
            var service = this.GetService();

            var result = service.Stringify(10);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestStringify_WithDouble()
        {
            var service = this.GetService();

            var result = service.Stringify(10.5);

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestStringify_WithEnum()
        {
            var service = this.GetService();

            var result = service.Stringify(AssemblyEnum.Other);

            Assert.AreEqual("1", result);
        }

        // bool

        [TestMethod]
        public void TestStringify_WithBool()
        {
            var service = this.GetService();

            var result = service.Stringify(true);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestStringify_WithIntNullable()
        {
            var service = this.GetService();

            var result = service.Stringify<int?>(10);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestStringify_WithInthNullableNull()
        {
            var service = this.GetService();

            var result = service.Stringify<int?>(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestStringify_WithDoubleNullable()
        {
            var service = this.GetService();

            var result = service.Stringify<double?>(10.5);

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestStringify_WithDoubleNullableNull()
        {
            var service = this.GetService();

            var result = service.Stringify<double?>(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestStringify_WithEnumNullable()
        {
            var service = this.GetService();

            var result = service.Stringify<AssemblyEnum?>(AssemblyEnum.Other);

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void TestStringify_WithEnumNullableNull()
        {
            var service = this.GetService();

            var result = service.Stringify<AssemblyEnum?>(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestStringify_WithDateTimeNullable()
        {
            var service = this.GetService();

            var t = new DateTime(1990, 12, 12);

            var result = service.Stringify<DateTime?>(t);

            Assert.AreEqual("\"12/12/1990 00:00:00\"", result);
        }

        [TestMethod]
        public void TestStringify_WithGuidNullable()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var result = service.Stringify<Guid?>(g);

            Assert.AreEqual("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"", result);
        }

        [TestMethod]
        public void TestStringify_WithDateTimeNullableNull()
        {
            var service = this.GetService();

            var result = service.Stringify<DateTime?>(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestStringify_WithGuidNullableNull()
        {
            var service = this.GetService();

            var result = service.Stringify<Guid?>(null);

            Assert.AreEqual("null", result);
        }

        // stringify object

        [TestMethod]
        public void TestStringify_WithObject()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var item = new AssemblyItem
            {
                MyGuid = g,
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

            var result = service.Stringify(item);

            Assert.AreEqual("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my inner value\"},\"MyList\":[\"a\",\"b\"],\"MyArray\":[\"y\",\"z\"]}", result);
        }


        [TestMethod]
        public void TestStringify_WithObjectAndAllLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var mappings = new JsonMappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var item = new AssemblyItem
            {
                MyGuid = g,
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

            var result = service.Stringify(item,mappings);

            Assert.AreEqual("{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my inner value\"},\"mylist\":[\"a\",\"b\"],\"myarray\":[\"y\",\"z\"]}", result);
        }

        [TestMethod]
        public void TestStringify_WithObjectAndTypeLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();

            // InnerItem no mapping

            var item = new AssemblyItem
            {
                MyGuid = g,
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

            var result = service.Stringify(item, mappings);

            Assert.AreEqual("{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"MyInnerString\":\"my inner value\"},\"mylist\":[\"a\",\"b\"],\"myarray\":[\"y\",\"z\"]}", result);
        }

        [TestMethod]
        public void TestStringify_WithObjectAndMapping()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

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

            mappings.SetType<AssemblyInner>().SetProperty("MyInnerString", "inner_map");

            var item = new AssemblyItem
            {
                MyGuid = g,
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

            var result = service.Stringify(item, mappings);

            Assert.AreEqual("{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my inner value\"},\"map_mylist\":[\"a\",\"b\"],\"map_myarray\":[\"y\",\"z\"]}", result);
        }


        // array

        [TestMethod]
        public void TestList_OfObjects()
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


            var result = service.Stringify(list);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my \\\"escape\\\" value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyString\":\"my \\\"escape\\\"value 2\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestList_OfObjectsWithAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetLowerStrategyForAllTypes();

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":true,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestList_OfObjectsWithTypeLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();
            mappings.SetType<AssemblyInner>().SetToLowerCaseStrategy();

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":true,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestList_OfObjectsWithMapping()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

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

            mappings.SetType<AssemblyInner>().SetProperty("MyInnerString", "inner_map");

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my \\\"escape\\\" value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my \\\"inner\\\" value 1\"},\"map_mylist\":[\"a1\",\"b1\"],\"map_myarray\":[\"y1\",\"z1\"]},{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"map_myint\":2,\"map_mydouble\":2.5,\"map_mystring\":\"my \\\"escape\\\"value 2\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":0,\"map_mydate\":\"12/10/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my \\\"inner\\\" value 2\"},\"map_mylist\":[\"a2\",\"b2\"],\"map_myarray\":[\"y2\",\"z2\"]}]", result);
        }


        [TestMethod]
        public void TestGetArray_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var list = new AssemblyItem[]{
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


            var result = service.Stringify(list);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my \\\"escape\\\" value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyString\":\"my \\\"escape\\\"value 2\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectsWithAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetLowerStrategyForAllTypes();

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":true,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectsWithTypeLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();
            mappings.SetType<AssemblyInner>().SetToLowerCaseStrategy();

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":true,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectsWithMapping()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

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

            mappings.SetType<AssemblyInner>().SetProperty("MyInnerString", "inner_map");

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


            var result = service.Stringify(list, mappings);

            Assert.AreEqual("[{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my \\\"escape\\\" value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my \\\"inner\\\" value 1\"},\"map_mylist\":[\"a1\",\"b1\"],\"map_myarray\":[\"y1\",\"z1\"]},{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"map_myint\":2,\"map_mydouble\":2.5,\"map_mystring\":\"my \\\"escape\\\"value 2\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":0,\"map_mydate\":\"12/10/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my \\\"inner\\\" value 2\"},\"map_mylist\":[\"a2\",\"b2\"],\"map_myarray\":[\"y2\",\"z2\"]}]", result);
        }

        // parse

        [TestMethod]
        public void TestParse_WithString()
        {
            var service = this.GetService();

            var result = service.Parse<string>("\"my value\"");

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestParse_WithStringEscape()
        {
            var service = this.GetService();

            var result = service.Parse<string>("\"my \\\"escape\\\" value\"");

            Assert.AreEqual("my \"escape\" value", result);
        }

        [TestMethod]
        public void TestParse_WithDateTime()
        {
            var service = this.GetService();

            var result = service.Parse<DateTime>("\"12/12/1990 00:00:00\"");

            Assert.AreEqual(new DateTime(1990, 12, 12), result);
        }

        [TestMethod]
        public void TestParse_WithGuid()
        {
            var service = this.GetService();

            var result = service.Parse<Guid>("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"");

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        [TestMethod]
        public void TestParse_WithInt()
        {
            var service = this.GetService();

            var result = service.Parse<int>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestParse_WithDouble()
        {
            var service = this.GetService();

            var result = service.Parse<double>("10.5");

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestParse_WithEnum()
        {
            var service = this.GetService();

            var result = service.Parse<AssemblyEnum>("1");

            Assert.AreEqual(AssemblyEnum.Other, result);
        }

        [TestMethod]
        public void TestParse_WithBool()
        {
            var service = this.GetService();

            var result = service.Parse<bool>("false");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestParse_WithIntNullable()
        {
            var service = this.GetService();

            var result = service.Parse<int?>("10");

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestParse_WithDoubleNullable()
        {
            var service = this.GetService();

            var result = service.Parse<double?>("10.5");

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestParse_WithIntNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<int?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestParse_WithDoubleNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<double?>("null");

            Assert.AreEqual(null, result);
        }


        [TestMethod]
        public void TestParse_WithDateTimeNullable()
        {
            var service = this.GetService();

            var result = service.Parse<DateTime?>("\"12/12/1990 00:00:00\"");

            Assert.AreEqual(new DateTime(1990, 12, 12), result);
        }

        [TestMethod]
        public void TestParse_WithGuidNullable()
        {
            var service = this.GetService();

            var result = service.Parse<Guid?>("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"");

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        [TestMethod]
        public void TestParse_WithDateTimeNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<DateTime?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestParse_WithGuidNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<Guid?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestParse_WithEnumNullable()
        {
            var service = this.GetService();

            var result = service.Parse<AssemblyEnum?>("1");

            Assert.AreEqual(AssemblyEnum.Other, result);
        }

        [TestMethod]
        public void TestParse_WithEnumNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<AssemblyEnum?>("null");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestParse_WithBoolNullable()
        {
            var service = this.GetService();

            var result = service.Parse<bool?>("false");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestParse_WithBoolNullableNull()
        {
            var service = this.GetService();

            var result = service.Parse<bool?>("null");

            Assert.AreEqual(null, result);
        }

        // parse object

        [TestMethod]
        public void TestParse_WithObject()
        {
            var service = this.GetService();

            var result = service.Parse<AssemblyItem>("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyString\":\"my value\",\"MyBool\":true,\"MyNullable\":null,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my inner value\"},\"MyList\":[\"a\",\"b\"],\"MyArray\":[\"y\",\"z\"]}");

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

            var result = service.Parse<ItemNullable>(json);

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

            var result = service.Parse<ItemNullable>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnum);
            Assert.AreEqual(null, result.MyDate);
        }

        [TestMethod]
        public void TestParse_WithObjectAndAllLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var result = service.Parse<AssemblyItem>("{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value\"},\"mylist\":[\"a\",\"b\"],\"myarray\":[\"y\",\"z\"]}", mappings);

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            Assert.IsNotNull(result);
            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
            Assert.AreEqual("my \"inner\" value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }

        [TestMethod]
        public void TestParse_WithObjectAndTypeLower()
        {
            var service = this.GetService();

            var mappings = new JsonMappingContainer();
            mappings.SetType<AssemblyItem>().SetToLowerCaseStrategy();
            mappings.SetType<AssemblyInner>().SetToLowerCaseStrategy();

            var result = service.Parse<AssemblyItem>("{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value\"},\"mylist\":[\"a\",\"b\"],\"myarray\":[\"y\",\"z\"]}", mappings);

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            Assert.IsNotNull(result);
            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(AssemblyEnum.Other, result.MyEnum);
            Assert.AreEqual(t, result.MyDate);
            Assert.AreEqual("my \"inner\" value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }


        [TestMethod]
        public void TestParse_WithObjectAndMapping()
        {
            var service = this.GetService();

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

            mappings.SetType<AssemblyInner>().SetProperty("MyInnerString", "inner_map");

            var result = service.Parse<AssemblyItem>("{\"map_myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"map_myint\":1,\"map_mydouble\":1.5,\"map_mystring\":\"my value\",\"map_mybool\":true,\"map_mynullable\":null,\"map_myenum\":1,\"map_mydate\":\"12/12/1990 00:00:00\",\"map_myobj\":{\"inner_map\":\"my inner value\"},\"map_mylist\":[\"a\",\"b\"],\"map_myarray\":[\"y\",\"z\"]}", mappings);

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

        // parse list / array

        [TestMethod]
        public void TestParseList_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]";

            var results = service.Parse<List<AssemblyItem>>(json);

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

            var results = service.Parse<AssemblyItem[]>(json);

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

            var results = service.Parse<List<ItemNullable>>(json);

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

            var results = service.Parse<ItemNullable[]>(json);

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
        public void TestParseList_OfObjectsNullablesNull()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]";

            var results = service.Parse<List<ItemNullable>>(json);

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
        public void TestParseArray_OfObjectsNullablesNull()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]";

            var results = service.Parse<ItemNullable[]>(json);

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
        public void TestParseList_OfObjectsWithAllLower()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t2 = new DateTime(1990, 10, 12);

            var json = "[{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"myint\":1,\"mydouble\":1.5,\"mystring\":\"my \\\"escape\\\" value\",\"mybool\":true,\"mynullable\":null,\"myenum\":1,\"mydate\":\"12/12/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 1\"},\"mylist\":[\"a1\",\"b1\"],\"myarray\":[\"y1\",\"z1\"]},{\"myguid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"myint\":2,\"mydouble\":2.5,\"mystring\":\"my \\\"escape\\\"value 2\",\"mybool\":false,\"mynullable\":null,\"myenum\":0,\"mydate\":\"12/10/1990 00:00:00\",\"myobj\":{\"myinnerstring\":\"my \\\"inner\\\" value 2\"},\"mylist\":[\"a2\",\"b2\"],\"myarray\":[\"y2\",\"z2\"]}]";

            var mappings = new JsonMappingContainer().SetLowerStrategyForAllTypes();

            var results = service.Parse<AssemblyItem[]>(json, mappings);

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
        public void TestParseList_OfObjectsWithTypeLower()
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

            var results = service.Parse<List<AssemblyItem>>(json, mappings);

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
        public void TestParseList_OfObjectsWithMapping()
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

            var results = service.Parse<List<AssemblyItem>>(json, mappings);

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

            var results = service.Parse<AssemblyItem[]>(json, mappings);

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

            var results = service.Parse<AssemblyItem[]>(json, mappings);

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

            var results = service.Parse<AssemblyItem[]>(json, mappings);

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

        // indented


        [TestMethod]
        public void TestSkipWhithspacesString()
        {
            var service = this.GetService();

            var json = "\r\r\n\t\"my value\"\r\r\n\t";

            var result = service.Parse<string>(json);

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestDontRemoveSpacesInString()
        {
            var service = this.GetService();

            var json = "\r\r\n\t\"my value\\r\\r\\n\\t\"";

            var result = service.Parse<string>(json);

            Assert.AreEqual("my value\r\r\n\t", result);
        }

        [TestMethod]
        public void TestSkipWhithspacesAtStartInt()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   10";

            var result = service.Parse<int>(json);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestSkipWhithspacesAtEndInt()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   10  \r\r\n\t   ";

            var result = service.Parse<int>(json);

            Assert.AreEqual(10, result);
        }


        [TestMethod]
        public void TestSkipWhithspacesWithDouble()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   10.5  \r\r\n\t   ";

            var result = service.Parse<double>(json);

            Assert.AreEqual(10.5, result);
        }


        [TestMethod]
        public void TestSkipWhithspacesWithBool()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   true  \r\r\n\t   ";

            var result = service.Parse<bool>(json);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestSkipWhithspacesWithNull()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   null  \r\r\n\t   ";

            var result = service.Parse<bool?>(json);

            Assert.AreEqual(null, result);
        }

        // object

        [TestMethod]
        public void TestLoadBeautifiedJson()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var item = new AssemblyItem
            {
                MyGuid = g,
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = t,
                MyObj = new AssemblyInner { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var json = service.StringifyAndBeautify(item);

            var result = service.Parse<AssemblyItem>(json);

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
        public void TestSkipWhithspacesWithObject()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var json = "\r\r\n\t   {\r\n   \"MyGuid\" : \"344ac1a2-9613-44d7-b64c-8d45b4585176\",\r\n   \"MyInt\" : 1,\r\n   \"MyDouble\" : 1.5,\r\n   \"MyString\" : \"my value\",\r\n   \"MyBool\" : true,\r\n   \"MyNullable\" : null,\r\n   \"MyEnum\" : 1,\r\n   \"MyDate\" : \"12/12/1990 00:00:00\",\r\n   \"MyObj\" : {\r\n      \"MyInnerString\" : \"my inner value\"\r\n   },\r\n   \"MyList\" : [\r\n      \"a\",\r\n      \"b\"\r\n   ],\r\n   \"MyArray\" : [\r\n      \"y\",\r\n      \"z\"\r\n   ]\r\n}  \r\r\n\t   ";

            var result = service.Parse<AssemblyItem>(json);

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
        public void TestLoadIndentedPlusWithSpacesJsonFile()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var json = File.ReadAllText("obj.json");

            var result = service.Parse<AssemblyItem>(json);

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


        // array

        [TestMethod]
        public void TestSkipWhithspacesWithArrayOfString()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [\"a\",\"b\"]  \r\r\n\t   ";

            var result = service.Parse<string[]>(json);

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestSkipWhithspacesWithArrayOfInt()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [1,2]  \r\r\n\t   ";

            var result = service.Parse<int[]>(json);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestSkipWhithspacesIntInnnerArrayItems()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [  \t\r1    \r\n,   \r 2\t\r\n        ]  \r\r\n\t   ";

            var result = service.Parse<int[]>(json);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestSkipWhithspacesInnnerStringArrayItems()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [  \t\r  \"a\"    \r\n,   \r \"b\"\t\r\n        ]  \r\r\n\t   ";

            var result = service.Parse<string[]>(json);

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }


        [TestMethod]
        public void TestSkipWhithspacesInnnerBoolArrayItems()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [  \t\r  true    \r\n,   \r false\t\r\n        ]  \r\r\n\t   ";

            var result = service.Parse<bool[]>(json);

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }


        [TestMethod]
        public void TestSkipWhithspacesInnnerNullArrayItems()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [  \t\r  null    \r\n,   \r null\t\r\n        ]  \r\r\n\t   ";

            var result = service.Parse<bool?[]>(json);

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }


        [TestMethod]
        public void TestSkipWhithspacesWithArrayOfBool()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [true,false]  \r\r\n\t   ";

            var result = service.Parse<bool[]>(json);

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }


        [TestMethod]
        public void TestSkipWhithspacesWithArrayOfNull()
        {
            var service = this.GetService();

            var json = "\r\r\n\t   [null,null]  \r\r\n\t   ";

            var result = service.Parse<bool?[]>(json);

            Assert.AreEqual(null, result[0]);
            Assert.AreEqual(null, result[1]);
        }

        [TestMethod]
        public void TestParseArrayBeautified()
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
                    MyString = "my \"escape\" value 2",
                    MyBool = true,
                    MyEnum = AssemblyEnum.Default,
                    MyDate = new DateTime(1990, 10, 12),
                    MyObj = new AssemblyInner { MyInnerString = "my \"inner\" value 2" },
                    MyList = new List<string> { "a2", "b2" },
                    MyArray = new string[] { "y2", "z2" }
                },
            };


            var json = service.StringifyAndBeautify(list);

            var results = service.Parse<List<AssemblyItem>>(json);

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual("my \"escape\" value", result.MyString);
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
            Assert.AreEqual("my \"escape\" value 2", result2.MyString);
            Assert.AreEqual(true, result2.MyBool);
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
        public void TestArrayLoadIndentedPlusWithSpacesJsonFile()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var json = File.ReadAllText("data.json");


            var results = service.Parse<List<AssemblyItem>>(json);

            var result = results[0];

            Assert.AreEqual(g, result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual("my \"escape\" value", result.MyString);
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
            Assert.AreEqual("my \"escape\" value 2", result2.MyString);
            Assert.AreEqual(true, result2.MyBool);
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

        // to xml

        [TestMethod]
        public void TestToXml_WithArray()
        {
            var service = this.GetService();

            var value = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var result =  service.ToXml(value);

            Assert.AreEqual("<?xml version=\"1.0\"?>\r<ArrayOfUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><User><Id>1</Id><UserName>Marie</UserName><Age xsi:nil=\"true\" /><Email xsi:nil=\"true\" /></User><User><Id>2</Id><UserName>Pat</UserName><Age>20</Age><Email>pat@domain.com</Email></User></ArrayOfUser>", result);
        }

        [TestMethod]
        public void TestToXml_WithArrayAndMapping()
        {
            var service = this.GetService();

            var value = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser")
                .SetArrayName("MapMyUsers")
                .SetProperty("Id", "MapId")
                .SetProperty("UserName", "MapUserName")
                .SetProperty("Email", "MapEmail");

            var result = service.ToXml(value, mappings);

            Assert.AreEqual("<?xml version=\"1.0\"?>\r<MapMyUsers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MapUser><MapId>1</MapId><MapUserName>Marie</MapUserName><Age xsi:nil=\"true\" /><MapEmail xsi:nil=\"true\" /></MapUser><MapUser><MapId>2</MapId><MapUserName>Pat</MapUserName><Age>20</Age><MapEmail>pat@domain.com</MapEmail></MapUser></MapMyUsers>", result);
        }

        // from xml

        [TestMethod]
        public void TestFromXml_WithArray()
        {
            var service = this.GetService();

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><User><Id>1</Id><UserName>Marie</UserName><Age xsi:nil=\"true\" /><Email xsi:nil=\"true\" /></User><User><Id>2</Id><UserName>Pat</UserName><Age>20</Age><Email>pat@domain.com</Email></User></ArrayOfUser>";

            var results = service.FromXml<List<User>>(xml);

            var result = results[0];

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);

            var result2 = results[1];

            Assert.AreEqual(2, result2.Id);
            Assert.AreEqual("Pat", result2.UserName);
            Assert.AreEqual(20, result2.Age);
            Assert.AreEqual("pat@domain.com", result2.Email);
        }

        [TestMethod]
        public void TestFromXml_WithArrayAndMapping()
        {
            var service = this.GetService();

            var xml = "<?xml version=\"1.0\"?>\r<MapMyUsers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MapUser><MapId>1</MapId><MapUserName>Marie</MapUserName><Age xsi:nil=\"true\" /><MapEmail xsi:nil=\"true\" /></MapUser><MapUser><MapId>2</MapId><MapUserName>Pat</MapUserName><Age>20</Age><MapEmail>pat@domain.com</MapEmail></MapUser></MapMyUsers>";

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser")
                .SetArrayName("MapMyUsers")
                .SetProperty("Id", "MapId")
                .SetProperty("UserName", "MapUserName")
                .SetProperty("Email", "MapEmail");

            var results = service.FromXml<List<User>>(xml, mappings);

            var result = results[0];

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);

            var result2 = results[1];

            Assert.AreEqual(2, result2.Id);
            Assert.AreEqual("Pat", result2.UserName);
            Assert.AreEqual(20, result2.Age);
            Assert.AreEqual("pat@domain.com", result2.Email);
        }

        [TestMethod]
        public void TestDictionaryOfStringInt32()
        {
            var service = this.GetService();

            //var value = new Dictionary<string, int>
            //{
            //    {"key1", 10 },
            //    {"key2", 20 },
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfInt32><Int32><Key>key1</Key><Value>10</Value></Int32><Int32><Key>key2</Key><Value>20</Value></Int32></ArrayOfInt32>";

            var result = service.FromXml<Dictionary<string, int>>(xml);

            Assert.AreEqual(10, result["key1"]);
            Assert.AreEqual(20, result["key2"]);
        }

        [TestMethod]
        public void TestDictionary_WithExoticKeyAndIntValue()
        {
            var service = this.GetService();

            //var value = new Dictionary<Type, int>
            //{
            //    {typeof(MyItem), 10 },
            //    {typeof(MyItem2), 20 },
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfInt32><Int32><Key>JsonLibTest.MyItem, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Key><Value>10</Value></Int32><Int32><Key>JsonLibTest.MyItem2, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</Key><Value>20</Value></Int32></ArrayOfInt32>";

            var result = service.FromXml<Dictionary<Type,int>>(xml);

            Assert.AreEqual(10, result[typeof(MyItem)]);
            Assert.AreEqual(20, result[typeof(MyItem2)]);
        }

        [TestMethod]
        public void TestDictionary_WithMapping()
        {
            var service = this.GetService();

            //var value = new Dictionary<int, User>
            //{
            //    { 1, new User { Id = 1, UserName = "Marie" } },
            //    { 2, new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" } }
            //};

            var xml = "<?xml version=\"1.0\"?>\r<MyUsers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><User><Key>1</Key><Value><map_id>1</map_id><map_username>Marie</map_username><Age xsi:nil=\"true\" /><map_email xsi:nil=\"true\" /></Value></User><User><Key>2</Key><Value><map_id>2</map_id><map_username>Pat</map_username><Age>20</Age><map_email>pat@domain.com</map_email></Value></User></MyUsers>";

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("user")
                    .SetArrayName("MyUsers")
                    .SetProperty("Id", "map_id")
                    .SetProperty("UserName", "map_username")
                    .SetProperty("Email", "map_email");

            var results = service.FromXml<Dictionary<int,User>>(xml, mappings);

            var result = results[1]; // key 1

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);

            var result2 = results[2]; // key 2

            Assert.AreEqual(2, result2.Id);
            Assert.AreEqual("Pat", result2.UserName);
            Assert.AreEqual(20, result2.Age);
            Assert.AreEqual("pat@domain.com", result2.Email);
        }


        [TestMethod]
        public void TestTFromXml_WithCommands()
        {
            var service = this.GetService();

            var xml = File.ReadAllText("comment1.xml");

            var result = service.FromXml<string[]>(xml);

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        // cache

        //[TestMethod]
        //public void TestAddToCache()
        //{
        //    var service = this.GetService();

        //    Assert.IsTrue(service.CacheIsActive);

        //    string json = "\"my value\"";

        //    service.Parse<string>(json);

        //    Assert.IsTrue(service.CacheHas<string>(json));
        //}
    }
}
