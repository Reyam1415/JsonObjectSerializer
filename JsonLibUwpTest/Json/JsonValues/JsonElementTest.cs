
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Json;

namespace JsonLibTest
{
    [TestClass]
    public class JsonElementUwpTest
    {
        /*
        "a" | null (+ datetime string, guid string)
        1 | 1.5 (+enum number)
        true | false
        null
        {"key":VALUE1,...} => VALUE1 is Json Value
        [VALUE1,...] => VALUE1 is Json Value
         */

        // string

        [TestMethod]
        public void TestCreateString()
        {
            var value = "my string";
            var result =  JsonValue.CreateString(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.String, result.ValueType);
        }

        [TestMethod]
        public void TestCreateString_WithNull()
        {
            string value = null;
            var result = JsonValue.CreateString(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.String, result.ValueType);
        }

        // number

        [TestMethod]
        public void TestCreateNumber_WithInt()
        {
            var value = 10;
            var result = JsonValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithDouble()
        {
            var value = 10.99;
            var result = JsonValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithoutANumber_Fail()
        {
            bool failed = false;
            var value = "my value";

            try
            {
                var result = JsonValue.CreateNumber(value);
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }

        // bool

        [TestMethod]
        public void TestCreateBool()
        {
            var value = true;
            var result = JsonValue.CreateBool(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Bool, result.ValueType);
        }

        [TestMethod]
        public void TestCreateBool_WithFalse()
        {
            var value = false;
            var result = JsonValue.CreateBool(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Bool, result.ValueType);
        }

        // nullable

        [TestMethod]
        public void TestCreateNullable()
        {
            var value = 10;
            var result = JsonValue.CreateNullable(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNullable_WithNull()
        {
            var value = 10;
            var result = JsonValue.CreateNullable(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
        }

        // object

        [TestMethod]
        public void TestCreateObject()
        {
            var result = JsonValue.CreateObject()
                .AddString("mystring", "my value")
                .AddNumber("mynumber", 10)
                .AddBool("mybool", true)
                .AddNullable("mynullable", 100);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.String, result.Values["mystring"].ValueType);
            Assert.AreEqual("my value", ((JsonString)result.Values["mystring"]).Value);

            Assert.AreEqual(JsonValueType.Number, result.Values["mynumber"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)result.Values["mynumber"]).Value);

            Assert.AreEqual(JsonValueType.Bool, result.Values["mybool"].ValueType);
            Assert.AreEqual(true, ((JsonBool)result.Values["mybool"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, result.Values["mynullable"].ValueType);
            Assert.AreEqual(100, ((JsonNullable)result.Values["mynullable"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithNullableNull()
        {
            var result = JsonValue.CreateObject()
                .AddNullable("mynullable", null);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Nullable, result.Values["mynullable"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)result.Values["mynullable"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerObject()
        {
            var result = JsonValue.CreateObject()
                .AddString("mystring", "my value")
                .AddObject("myobj", JsonValue.CreateObject().AddString("key","string value"));

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((JsonString)result.Values["mystring"]).Value);
            Assert.AreEqual(JsonValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(JsonValueType.Object, result.Values["myobj"].ValueType);
            Assert.AreEqual(JsonValueType.String, ((JsonObject) result.Values["myobj"]).Values["key"].ValueType);
            Assert.AreEqual("string value", ((JsonString) ((JsonObject)result.Values["myobj"]).Values["key"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerArray()
        {
            var result = JsonValue.CreateObject()
                .AddString("mystring", "my value")
                .AddArray("myarray", JsonValue.CreateArray().AddString("a").AddString("b"));

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((JsonString)result.Values["mystring"]).Value);
            Assert.AreEqual(JsonValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(JsonValueType.Array, result.Values["myarray"].ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result.Values["myarray"]).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonString)((JsonArray)result.Values["myarray"]).Values[0]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result.Values["myarray"]).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonString)((JsonArray)result.Values["myarray"]).Values[1]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithSameName_Fail()
        {
            bool failed = false;
            try
            {
                var result = JsonValue.CreateObject()
                            .AddString("myvalue", "my value")
                            .AddNumber("myvalue", 10);
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }

        // Array

        [TestMethod]
        public void TestCreateArray()
        {
            var result = JsonValue.CreateArray()
                .AddString("my string")
                .AddNumber(10)
                .AddBool(true)
                .AddNullable(null);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual("my string", ((JsonString)result.Values[0]).Value);
            Assert.AreEqual(10, ((JsonNumber)result.Values[1]).Value);
            Assert.AreEqual(true, ((JsonBool)result.Values[2]).Value);
            Assert.AreEqual(null, ((JsonNullable)result.Values[3]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithObject()
        {
            var result = JsonValue.CreateArray()
                .AddObject(JsonValue.CreateObject().AddString("mystring","my value"));

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Object, result.Values[0].ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result.Values[0]).Values["mystring"].ValueType);
            Assert.AreEqual("my value", ((JsonString)((JsonObject)result.Values[0]).Values["mystring"]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithArray()
        {
            var result = JsonValue.CreateArray()
                .AddArray(JsonValue.CreateArray().AddString("my value"));

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Array, result.Values[0].ValueType);

            var innerArray = result.Values[0] as JsonArray;

            Assert.AreEqual(JsonValueType.String, innerArray.Values[0].ValueType);
            Assert.AreEqual("my value", ((JsonString) innerArray.Values[0]).Value);
        }      
        

    }
}
