using JsonLib;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonLibTest
{
    [TestClass]
    public class JsonValueToJsonTest
    {
        public JsonValueToJson GetService()
        {
            return new JsonValueToJson();
        }

        [TestMethod]
        public void TestGetString()
        {
            var service = this.GetService();

            var result = service.ToString(JsonElementValue.CreateString("my value"));

            Assert.AreEqual("\"my value\"", result);
        }

        [TestMethod]
        public void TestGetNumber()
        {
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

        [TestMethod]
        public void TestGetBool()
        {
            var service = this.GetService();

            var result = service.ToBool(JsonElementValue.CreateBool(true));

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGetArray()
        {
            var service = this.GetService();

            var result = service.ToArray(JsonElementValue.CreateArray().AddString("a").AddNumber(10).AddBool(true));

            Assert.AreEqual("[\"a\",10,true]", result);
        }

        [TestMethod]
        public void TestGetNullable()
        {
            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(10));

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetNullable_WithNull()
        {
            var service = this.GetService();

            var result = service.ToNullable(JsonElementValue.CreateNullable(null));

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestGetObject()
        {
            var service = this.GetService();

            var result = service.ToObject(JsonElementValue.CreateObject().AddString("mystring","a").AddNumber("mynumber",10).AddBool("mybool",true));

            Assert.AreEqual("{\"mystring\":\"a\",\"mynumber\":10,\"mybool\":true}", result);
        }

        [TestMethod]
        public void TestGetObject_WithNulls()
        {
            var service = this.GetService();

           

           var result = service.ToObject(JsonElementValue.CreateObject()
                .AddString("mystring", "a")
                .AddNullable("mynullable", null)
                .AddNullable("mynullablenotnull", 10)
                .AddString("mystringnull",null));

            Assert.AreEqual("{\"mystring\":\"a\",\"mynullable\":null,\"mynullablenotnull\":10,\"mystringnull\":null}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerObject()
        {
            var service = this.GetService();

            var result = service.ToObject(JsonElementValue.CreateObject()
                .AddString("mystring", "a")
                .AddObject("innerobj",JsonElementValue.CreateObject().AddString("innerstring","inner value").AddNumber("innernumber",20)));

            Assert.AreEqual("{\"mystring\":\"a\",\"innerobj\":{\"innerstring\":\"inner value\",\"innernumber\":20}}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerArray()
        {
            var service = this.GetService();

            var result = service.ToObject(JsonElementValue.CreateObject()
                .AddString("mystring", "a")
                .AddArray("innerarray", JsonElementValue.CreateArray().AddString("a").AddString("b")));

            Assert.AreEqual("{\"mystring\":\"a\",\"innerarray\":[\"a\",\"b\"]}", result);
        }

    }
}
