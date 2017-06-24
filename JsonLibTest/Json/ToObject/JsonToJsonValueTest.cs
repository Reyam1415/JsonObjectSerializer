using JsonLib;
using JsonLib.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JsonLibTest
{
    [TestClass]
    public class JsonToJsonValueTest
    {
        public JsonToJsonValue GetService()
        {
            return new JsonToJsonValue();
        }

        // check

        [TestMethod]
        public void TestFailWithDoubleString()
        {
            bool failed = false;
            var service = this.GetService();

            var json = "\"my value\"\"my value 2\"";

            try
            {
                var result = service.ToJsonValue(json);
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestFailWithStringAndOtherValue()
        {
            bool failed = false;
            var service = this.GetService();

            var json = "\"my value\"10";

            try
            {
                var result = service.ToJsonValue(json);
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestFailWithInvalid()
        {
            bool failed = false;
            var service = this.GetService();

            var json = "1,2,3";

            try
            {
                var result = service.ToJsonValue(json);
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        // string

        [TestMethod]
        public void TestString()
        {
            // Json: string, Guid, DateTime => Json Value String + value string
            // (null)

            var service = this.GetService();

            var json = "\"my value\"";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonString), result.GetType());
            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual("my value", ((JsonString)result).Value);
        }

        [TestMethod]
        public void TestString_WithNumberString()
        {
            var service = this.GetService();

            var json = "\"10\"";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonString), result.GetType());
            Assert.AreEqual(JsonValueType.String, result.ValueType);
            Assert.AreEqual("10", ((JsonString)result).Value);
        }

        // number

        [TestMethod]
        public void TestNumber()
        {
            // int or double ...

            var service = this.GetService();

            var json = "10";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonNumber), result.GetType());
            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual(10, ((JsonNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var json = "10.99";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonNumber), result.GetType());
            Assert.AreEqual(JsonValueType.Number, result.ValueType);
            Assert.AreEqual((double)10.99, ((JsonNumber)result).Value);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var json = "true";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonBool), result.GetType());
            Assert.AreEqual(JsonValueType.Bool, result.ValueType);
            Assert.AreEqual(true, ((JsonBool)result).Value);
        }

        [TestMethod]
        public void TestBool_WithFalse()
        {
            var service = this.GetService();

            var json = "false";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonBool), result.GetType());
            Assert.AreEqual(JsonValueType.Bool, result.ValueType);
            Assert.AreEqual(false, ((JsonBool)result).Value);
        }

        // null

        [TestMethod]
        public void TestNull()
        {
            // null => Guess Json Value Nullable bu check conversion to object
            var service = this.GetService();

            var json = "null";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonNullable), result.GetType());
            Assert.AreEqual(JsonValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((JsonNullable)result).Value);
        }

        // object

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":20,\"Email\":\"marie@domain.com\"}";

            var result = service.ToJsonValue(json);


            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonNumber)((JsonObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["Email"].ValueType);
            Assert.AreEqual("marie@domain.com", ((JsonString)((JsonObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithNulls()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}";

            var result = service.ToJsonValue(json);


            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, ((JsonObject)result).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)((JsonObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithInnerObject()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":\"Admin\",\"Status\":100}}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Object, ((JsonObject)result).Values["Role"].ValueType);

            var role = ((JsonObject)result).Values["Role"] as JsonObject;

            Assert.AreEqual(JsonValueType.Number, role.Values["RoleId"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)role.Values["RoleId"]).Value);

            Assert.AreEqual(JsonValueType.String, role.Values["Name"].ValueType);
            Assert.AreEqual("Admin", ((JsonString)role.Values["Name"]).Value);

            Assert.AreEqual(JsonValueType.Number, role.Values["Status"].ValueType);
            Assert.AreEqual(100, ((JsonNumber)role.Values["Status"]).Value);
        }

        [TestMethod]
        public void TestObject_WithInnerObjectAndNulls()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":null,\"Status\":null}}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Object, ((JsonObject)result).Values["Role"].ValueType);

            var role = ((JsonObject)result).Values["Role"] as JsonObject;

            Assert.AreEqual(JsonValueType.Number, role.Values["RoleId"].ValueType);
            Assert.AreEqual(10, ((JsonNumber)role.Values["RoleId"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, role.Values["Name"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)role.Values["Name"]).Value);

            Assert.AreEqual(JsonValueType.Nullable, role.Values["Status"].ValueType);
            Assert.AreEqual(null, ((JsonNullable)role.Values["Status"]).Value);
        }

        // array

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var json = "[\"a\",\"b\"]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonString)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.String, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonString)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var json = "[1,2]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(1, ((JsonNumber)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(2, ((JsonNumber)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var json = "[1.5,2.5]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(1.5, ((JsonNumber)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.Number, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(2.5, ((JsonNumber)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithBools()
        {
            var service = this.GetService();

            var json = "[true,false]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Bool, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(true, ((JsonBool)((JsonArray)result).Values[0]).Value);

            Assert.AreEqual(JsonValueType.Bool, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(false, ((JsonBool)((JsonArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithObjects()
        {
            var service = this.GetService();

            var json = "[{\"Name\":\"a\"},{\"Name\":\"b\"}]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Array, result.ValueType);

            Assert.AreEqual(JsonValueType.Object, ((JsonArray)result).Values[0].ValueType);
            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[0]).Values["Name"].ValueType);
            Assert.AreEqual("a", ((JsonString) ((JsonObject)((JsonArray)result).Values[0]).Values["Name"]).Value);

            Assert.AreEqual(JsonValueType.Object, ((JsonArray)result).Values[1].ValueType);
            Assert.AreEqual(JsonValueType.String, ((JsonObject)((JsonArray)result).Values[1]).Values["Name"].ValueType);
            Assert.AreEqual("b", ((JsonString)((JsonObject)((JsonArray)result).Values[1]).Values["Name"]).Value);

        }

        [TestMethod]
        public void TestObject_WithInnerArray()
        {
            var service = this.GetService();

            var json = "{\"UserName\":\"Marie\",\"Strings\":[\"a\",\"b\"]}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonValueType.Object, result.ValueType);

            Assert.AreEqual(JsonValueType.String, ((JsonObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonString)((JsonObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonValueType.Array, ((JsonObject)result).Values["Strings"].ValueType);

            var array = ((JsonObject)result).Values["Strings"] as JsonArray;

            Assert.AreEqual(2,array.Values.Count);

            Assert.AreEqual(JsonValueType.String,array.Values[0].ValueType);
            Assert.AreEqual("a",((JsonString) array.Values[0]).Value);

            Assert.AreEqual(JsonValueType.String, array.Values[1].ValueType);
            Assert.AreEqual("b", ((JsonString)array.Values[1]).Value);

        }
    }
}
