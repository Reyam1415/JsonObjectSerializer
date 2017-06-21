using JsonLib;
using JsonLib.Mappings;
using JsonLibTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class JsonValueToJsonUwpTest
    {
        public JsonValueToJson GetService()
        {
            return new JsonValueToJson();
        }

        // string

        [TestMethod]
        public void TestGetString()
        {
            // json  string null : value to string

            var service = this.GetService();

            var result = service.ToString(JsonElementValue.CreateString("my value"));

            Assert.AreEqual("\"my value\"", result);
        }

        [TestMethod]
        public void TestGetString_EscapeString()
        {
            // json  string null : value to string

            var service = this.GetService();

            var result = service.ToString(JsonElementValue.CreateString("my \"escape\" value"));

            Assert.AreEqual("\"my \\\"escape\\\" value\"", result);
        }

        [TestMethod]
        public void TestGetString_WithNull()
        {
            var service = this.GetService();

            var result = service.ToString(JsonElementValue.CreateString(null));

            Assert.AreEqual("null", result);
        }

        // number

        [TestMethod]
        public void TestGetNumber_WithInt()
        {
            // json number => object number (int, double, enum int value)

            var service = this.GetService();

            var result = service.ToNumber(JsonElementValue.CreateNumber(10));

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetNumber_WithDoubleReplaceComma()
        {
            var service = this.GetService();

            var result = service.ToNumber(JsonElementValue.CreateNumber(10.99));

            Assert.AreEqual("10.99", result);
        }

        // bool

        [TestMethod]
        public void TestGetBool()
        {
            var service = this.GetService();

            var result = service.ToBool(JsonElementValue.CreateBool(true));

            Assert.AreEqual("true", result);
        }

        // nullable

        [TestMethod]
        public void TestGetNullable_WithInt()
        {
            // json nullable => value object int or double or bool or DateTime or Enum or Guid

            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(10));

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetNullable_WithDouble()
        {
            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(10.5));

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestGetNullable_WithBool()
        {
            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(true));

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGetNullable_WithDateTime()
        {
            var service = this.GetService();

            var value = new DateTime(1990, 12, 12);

            var result = service.ToNullable(JsonElementValue.CreateNullable(value));

            Assert.AreEqual("\"12/12/1990 00:00:00\"", result);
        }

        [TestMethod]
        public void TestGetNullable_WithGuid()
        {
            var service = this.GetService();

            var v = Guid.NewGuid();

            var value = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");

            var result = service.ToNullable(JsonElementValue.CreateNullable(value));

            Assert.AreEqual("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"", result);
        }

        [TestMethod]
        public void TestGetNullable_WithNull()
        {
            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(null));

            Assert.AreEqual("null", result);
        }


        // object

        [TestMethod]
        public void TestGetObject()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var jsonObject = new JsonElementObject()
                .AddString("MyGuid", g.ToString())
                .AddNumber("MyInt", 1)
                .AddNumber("MyDouble", 1.5)
                .AddBool("MyBool", true)
                .AddNumber("MyEnum", 1)
                .AddString("MyDate", t.ToString())
                .AddObject("MyObj", new JsonElementObject().AddString("MyInnerString", "my inner value"))
                .AddArray("MyList", new JsonElementArray().AddString("a").AddString("b"))
                .AddArray("MyArray", new JsonElementArray().AddString("y").AddString("z"));


            var result = service.ToObject(jsonObject);

            Assert.AreEqual("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my inner value\"},\"MyList\":[\"a\",\"b\"],\"MyArray\":[\"y\",\"z\"]}", result);
        }

        [TestMethod]
        public void TestGetObject_WithNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var jsonObject = new JsonElementObject()
                .AddNullable("MyGuid", g)
                .AddNullable("MyInt", 1)
                .AddNullable("MyDouble", 1.5)
                .AddNullable("MyBool", true)
                .AddNullable("MyEnum", 1)
                .AddNullable("MyDate", t);


            var result = service.ToObject(jsonObject);

            Assert.AreEqual("{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"}", result);
        }

        [TestMethod]
        public void TestGetObject_WithNullablesNull()
        {
            var service = this.GetService();

            var jsonObject = new JsonElementObject()
                .AddNullable("MyGuid", null)
                .AddNullable("MyInt", null)
                .AddNullable("MyDouble", null)
                .AddNullable("MyBool", null)
                .AddNullable("MyEnum", null)
                .AddNullable("MyDate", null);


            var result = service.ToObject(jsonObject);

            Assert.AreEqual("{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}", result);
        }

        // array

        [TestMethod]
        public void TestGetArray()
        {
            var service = this.GetService();

            var result = service.ToArray(JsonElementValue.CreateArray()
                .AddString("a")
                .AddNumber(10)
                .AddBool(true)
                .AddNullable(null));

            Assert.AreEqual("[\"a\",10,true,null]", result);
        }

        [TestMethod]
        public void TestGetArray_WithNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var t = new DateTime(1990, 12, 12);

            var jsonArray = new JsonElementArray()
                .AddNullable(g)
                .AddNullable(1)
                .AddNullable(1.5)
                .AddNullable(true)
                .AddNullable(1)
                .AddNullable(t);

            var result = service.ToArray(jsonArray);

            Assert.AreEqual("[\"344ac1a2-9613-44d7-b64c-8d45b4585176\",1,1.5,true,1,\"12/12/1990 00:00:00\"]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjects()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonElementArray();
            jsonArray.AddObject(new JsonElementObject()
                .AddString("MyGuid", g.ToString())
                .AddNumber("MyInt", 1)
                .AddNumber("MyDouble", 1.5)
                .AddBool("MyBool", true)
                .AddNumber("MyEnum", 1)
                .AddString("MyDate", t.ToString())
                .AddObject("MyObj", new JsonElementObject().AddString("MyInnerString", "my \"inner\" value 1"))
                .AddArray("MyList", new JsonElementArray().AddString("a1").AddString("b1"))
                .AddArray("MyArray", new JsonElementArray().AddString("y1").AddString("z1")));

            jsonArray.AddObject(new JsonElementObject()
             .AddString("MyGuid", g2.ToString())
             .AddNumber("MyInt", 2)
             .AddNumber("MyDouble", 2.5)
             .AddBool("MyBool", false)
             .AddNumber("MyEnum", 0)
             .AddString("MyDate", t2.ToString())
             .AddObject("MyObj", new JsonElementObject().AddString("MyInnerString", "my \"inner\" value 2"))
             .AddArray("MyList", new JsonElementArray().AddString("a2").AddString("b2"))
             .AddArray("MyArray", new JsonElementArray().AddString("y2").AddString("z2")));

            var result = service.ToArray(jsonArray);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 1\"},\"MyList\":[\"a1\",\"b1\"],\"MyArray\":[\"y1\",\"z1\"]},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\",\"MyObj\":{\"MyInnerString\":\"my \\\"inner\\\" value 2\"},\"MyList\":[\"a2\",\"b2\"],\"MyArray\":[\"y2\",\"z2\"]}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectNullables()
        {
            var service = this.GetService();

            var g = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176");
            var g2 = new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178");
            var t = new DateTime(1990, 12, 12);
            var t2 = new DateTime(1990, 10, 12);

            var jsonArray = new JsonElementArray();
            jsonArray.AddObject(new JsonElementObject()
                .AddNullable("MyGuid", g)
                .AddNullable("MyInt", 1)
                .AddNullable("MyDouble", 1.5)
                .AddNullable("MyBool", true)
                .AddNullable("MyEnum", 1)
                .AddNullable("MyDate", t));

            jsonArray.AddObject(new JsonElementObject()
                .AddNullable("MyGuid", g2)
                .AddNullable("MyInt", 2)
                .AddNullable("MyDouble", 2.5)
                .AddNullable("MyBool", false)
                .AddNullable("MyEnum", 0)
                .AddNullable("MyDate", t2));

            var result = service.ToArray(jsonArray);

            Assert.AreEqual("[{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585176\",\"MyInt\":1,\"MyDouble\":1.5,\"MyBool\":true,\"MyEnum\":1,\"MyDate\":\"12/12/1990 00:00:00\"},{\"MyGuid\":\"344ac1a2-9613-44d7-b64c-8d45b4585178\",\"MyInt\":2,\"MyDouble\":2.5,\"MyBool\":false,\"MyEnum\":0,\"MyDate\":\"12/10/1990 00:00:00\"}]", result);
        }

        [TestMethod]
        public void TestGetArray_OfObjectNullablesNull()
        {
            var service = this.GetService();

            var jsonArray = new JsonElementArray();
            jsonArray.AddObject(new JsonElementObject()
                .AddNullable("MyGuid", null)
                .AddNullable("MyInt", null)
                .AddNullable("MyDouble", null)
                .AddNullable("MyBool", null)
                .AddNullable("MyEnum", null)
                .AddNullable("MyDate", null));

            jsonArray.AddObject(new JsonElementObject()
                .AddNullable("MyGuid", null)
                .AddNullable("MyInt", null)
                .AddNullable("MyDouble", null)
                .AddNullable("MyBool", null)
                .AddNullable("MyEnum", null)
                .AddNullable("MyDate", null));

            var result = service.ToArray(jsonArray);

            Assert.AreEqual("[{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null},{\"MyGuid\":null,\"MyInt\":null,\"MyDouble\":null,\"MyBool\":null,\"MyEnum\":null,\"MyDate\":null}]", result);
        }

    }
}
