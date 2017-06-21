using JsonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonLibTest
{
    [TestClass]
    public class JsonValueToJsonServiceTest
    {

        public JsonValueToJsonService GetService()
        {
            return new JsonValueToJsonService();
        }

        // get key

        [TestMethod]
        public void TestGetKey()
        {
            var service = this.GetService();

            var result = service.GetKey("mykey");

            Assert.AreEqual("\"mykey\"", result);
        }

        // string

        [TestMethod]
        public void TestGetString()
        {
            var service = this.GetService();

            var result = service.GetString("my value");

            Assert.AreEqual("\"my value\"", result);
        }

        [TestMethod]
        public void TestGetString_EscapeString()
        {
            var service = this.GetService();

            var result = service.GetString("my \"escape\" value");

            Assert.AreEqual("\"my \\\"escape\\\" value\"", result);
        }

        [TestMethod]
        public void TestGetStringObject()
        {
            var service = this.GetService();

            var result = service.GetString("mykey","my value");

            Assert.AreEqual("\"mykey\":\"my value\"", result);
        }

        // number

        [TestMethod]
        public void TestGetNumber()
        {
            var service = this.GetService();

            var result = service.GetNumber(10);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetNumberObject()
        {
            var service = this.GetService();

            var result = service.GetNumber("mykey", 10);

            Assert.AreEqual("\"mykey\":10", result);
        }


        // bool

        [TestMethod]
        public void TestGetBool()
        {
            var service = this.GetService();

            var result = service.GetBool(true);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGetBoolObject()
        {
            var service = this.GetService();

            var result = service.GetBool("mykey", true);

            Assert.AreEqual("\"mykey\":true", result);
        }

        // nullable

        [TestMethod]
        public void TestGetNullable()
        {
            var service = this.GetService();

            var result = service.GetNullable(10);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetNullable_WithNull()
        {
            var service = this.GetService();

            var result = service.GetNullable(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestGetNullable_WithBool()
        {
            var service = this.GetService();

            var result = service.GetNullable(true);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGetNullableObject()
        {
            var service = this.GetService();

            var result = service.GetNullable("mykey", 10);

            var result2 = service.GetNullable("mykey", null);

            Assert.AreEqual("\"mykey\":10", result);
            Assert.AreEqual("\"mykey\":null", result2);
        }
    }
}
