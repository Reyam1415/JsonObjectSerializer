
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;

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
            var result =  JsonElementValue.CreateString(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
        }

        [TestMethod]
        public void TestCreateString_WithNull()
        {
            string value = null;
            var result = JsonElementValue.CreateString(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
        }

        // number

        [TestMethod]
        public void TestCreateNumber_WithInt()
        {
            var value = 10;
            var result = JsonElementValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithDouble()
        {
            var value = 10.99;
            var result = JsonElementValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithoutANumber_Fail()
        {
            bool failed = false;
            var value = "my value";

            try
            {
                var result = JsonElementValue.CreateNumber(value);
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
            var result = JsonElementValue.CreateBool(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
        }

        [TestMethod]
        public void TestCreateBool_WithFalse()
        {
            var value = false;
            var result = JsonElementValue.CreateBool(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
        }

        // nullable

        [TestMethod]
        public void TestCreateNullable()
        {
            var value = 10;
            var result = JsonElementValue.CreateNullable(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNullable_WithNull()
        {
            var value = 10;
            var result = JsonElementValue.CreateNullable(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
        }

        // object

        [TestMethod]
        public void TestCreateObject()
        {
            var result = JsonElementValue.CreateObject()
                .AddString("mystring", "my value")
                .AddNumber("mynumber", 10)
                .AddBool("mybool", true)
                .AddNullable("mynullable", 100);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, result.Values["mystring"].ValueType);
            Assert.AreEqual("my value", ((JsonElementString)result.Values["mystring"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, result.Values["mynumber"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)result.Values["mynumber"]).Value);

            Assert.AreEqual(JsonElementValueType.Bool, result.Values["mybool"].ValueType);
            Assert.AreEqual(true, ((JsonElementBool)result.Values["mybool"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, result.Values["mynullable"].ValueType);
            Assert.AreEqual(100, ((JsonElementNullable)result.Values["mynullable"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithNullableNull()
        {
            var result = JsonElementValue.CreateObject()
                .AddNullable("mynullable", null);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Null, result.Values["mynullable"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result.Values["mynullable"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerObject()
        {
            var result = JsonElementValue.CreateObject()
                .AddString("mystring", "my value")
                .AddObject("myobj", JsonElementValue.CreateObject().AddString("key","string value"));

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((JsonElementString)result.Values["mystring"]).Value);
            Assert.AreEqual(JsonElementValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(JsonElementValueType.Object, result.Values["myobj"].ValueType);
            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject) result.Values["myobj"]).Values["key"].ValueType);
            Assert.AreEqual("string value", ((JsonElementString) ((JsonElementObject)result.Values["myobj"]).Values["key"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerArray()
        {
            var result = JsonElementValue.CreateObject()
                .AddString("mystring", "my value")
                .AddArray("myarray", JsonElementValue.CreateArray().AddString("a").AddString("b"));

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((JsonElementString)result.Values["mystring"]).Value);
            Assert.AreEqual(JsonElementValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(JsonElementValueType.Array, result.Values["myarray"].ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result.Values["myarray"]).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonElementString)((JsonElementArray)result.Values["myarray"]).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result.Values["myarray"]).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonElementString)((JsonElementArray)result.Values["myarray"]).Values[1]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithSameName_Fail()
        {
            bool failed = false;
            try
            {
                var result = JsonElementValue.CreateObject()
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
            var result = JsonElementValue.CreateArray()
                .AddString("my string")
                .AddNumber(10)
                .AddBool(true)
                .AddNullable(null);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual("my string", ((JsonElementString)result.Values[0]).Value);
            Assert.AreEqual(10, ((JsonElementNumber)result.Values[1]).Value);
            Assert.AreEqual(true, ((JsonElementBool)result.Values[2]).Value);
            Assert.AreEqual(null, ((JsonElementNullable)result.Values[3]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithObject()
        {
            var result = JsonElementValue.CreateArray()
                .AddObject(JsonElementValue.CreateObject().AddString("mystring","my value"));

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Object, result.Values[0].ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result.Values[0]).Values["mystring"].ValueType);
            Assert.AreEqual("my value", ((JsonElementString)((JsonElementObject)result.Values[0]).Values["mystring"]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithArray()
        {
            var result = JsonElementValue.CreateArray()
                .AddArray(JsonElementValue.CreateArray().AddString("my value"));

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Array, result.Values[0].ValueType);

            var innerArray = result.Values[0] as JsonElementArray;

            Assert.AreEqual(JsonElementValueType.String, innerArray.Values[0].ValueType);
            Assert.AreEqual("my value", ((JsonElementString) innerArray.Values[0]).Value);
        }      
        

    }
}
