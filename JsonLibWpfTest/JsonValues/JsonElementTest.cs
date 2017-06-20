
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;

namespace JsonLibTest
{
    [TestClass]
    public class JsonElementWpfTest
    {
        /*
         1
        "a"
        true
        {
          "item":"value"
        }
        [1,2]
         */
        [TestMethod]
        public void TestCreateString()
        {
            var value = "my string";
            var result =  JsonElementValue.CreateString(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber()
        {
            var value = 10.99;
            var result = JsonElementValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithInt()
        {
            var value = 10;
            var result = JsonElementValue.CreateNumber(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateBool()
        {
            var value = true;
            var result = JsonElementValue.CreateBool(value);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
        }

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

        [TestMethod]
        public void TestCreateArray()
        {
            var result = JsonElementValue.CreateArray()
                .AddString("my string")
                .AddNumber(10)
                .AddBool(true);

            Assert.AreEqual("my string", ((JsonElementString) result.Values[0]).Value);
            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(10, ((JsonElementNumber)result.Values[1]).Value);
            Assert.AreEqual(true, ((JsonElementBool)result.Values[2]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithObject()
        {
            var result = JsonElementValue.CreateArray().AddObject(JsonElementValue.CreateObject().AddString("mystring","my value"));

            Assert.AreEqual("my value", ((JsonElementString)((JsonElementObject)result.Values[0]).Values["mystring"]).Value);
            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);
        }

        [TestMethod]
        public void TestCreateArray_WithArray()
        {
            var result = JsonElementValue.CreateArray().AddArray(JsonElementValue.CreateArray().AddString("my value"));

            Assert.AreEqual("my value", ((JsonElementString)((JsonElementArray)result.Values[0]).Values[0]).Value);
        }

        [TestMethod]
        public void TestCreateObject()
        {
            var result = JsonElementValue.CreateObject()
                .AddString("mystring", "my value")
                .AddNumber("mynumber",10)
                .AddBool("mybool",true);

            Assert.AreEqual("my value", ((JsonElementString)result.Values["mystring"]).Value);
            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(10, ((JsonElementNumber)result.Values["mynumber"]).Value);
            Assert.AreEqual(true, ((JsonElementBool)result.Values["mybool"]).Value);
        }
    }
}
