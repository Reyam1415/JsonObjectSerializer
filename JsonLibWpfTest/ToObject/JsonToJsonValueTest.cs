using JsonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest
{
    [TestClass]
    public class JsonToJsonValueWpfTest
    {
        public JsonToJsonValue GetService()
        {
            return new JsonToJsonValue();
        }

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

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var json = "\"my value\"";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementString), result.GetType());
            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual("my value", ((JsonElementString)result).Value);
        }

        [TestMethod]
        public void TestString_WithNumberString()
        {
            var service = this.GetService();

            var json = "\"10\"";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementString), result.GetType());
            Assert.AreEqual(JsonElementValueType.String, result.ValueType);
            Assert.AreEqual("10", ((JsonElementString)result).Value);
        }

        [TestMethod]
        public void TestNumber()
        {
            var service = this.GetService();

            var json = "10";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementNumber), result.GetType());
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var json = "10.99";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementNumber), result.GetType());
            Assert.AreEqual(JsonElementValueType.Number, result.ValueType);
            Assert.AreEqual((double)10.99, ((JsonElementNumber)result).Value);
        }

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var json = "true";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementBool), result.GetType());
            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
            Assert.AreEqual(true, ((JsonElementBool)result).Value);
        }

        [TestMethod]
        public void TestBool_WithFalse()
        {
            var service = this.GetService();

            var json = "false";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementBool), result.GetType());
            Assert.AreEqual(JsonElementValueType.Bool, result.ValueType);
            Assert.AreEqual(false, ((JsonElementBool)result).Value);
        }

        [TestMethod]
        public void TestNull()
        {
            var service = this.GetService();

            var json = "null";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(typeof(JsonElementNullable), result.GetType());
            Assert.AreEqual(JsonElementValueType.Null, result.ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)result).Value);
        }


        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":20,\"Email\":\"marie@domain.com\"}";

            var result = service.ToJsonValue(json);


            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Age"].ValueType);
            Assert.AreEqual(20, ((JsonElementNumber)((JsonElementObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["Email"].ValueType);
            Assert.AreEqual("marie@domain.com", ((JsonElementString)((JsonElementObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithNulls()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}";

            var result = service.ToJsonValue(json);


            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["Age"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, ((JsonElementObject)result).Values["Email"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)((JsonElementObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithInnerObject()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":\"Admin\",\"Status\":100}}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementObject)result).Values["Role"].ValueType);

            var role = ((JsonElementObject)result).Values["Role"] as JsonElementObject;

            Assert.AreEqual(JsonElementValueType.Number, role.Values["RoleId"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)role.Values["RoleId"]).Value);

            Assert.AreEqual(JsonElementValueType.String, role.Values["Name"].ValueType);
            Assert.AreEqual("Admin", ((JsonElementString)role.Values["Name"]).Value);

            Assert.AreEqual(JsonElementValueType.Number, role.Values["Status"].ValueType);
            Assert.AreEqual(100, ((JsonElementNumber)role.Values["Status"]).Value);
        }

        [TestMethod]
        public void TestObject_WithInnerObjectAndNulls()
        {
            var service = this.GetService();

            var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":null,\"Status\":null}}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementObject)result).Values["Id"].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementObject)result).Values["Id"]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementObject)result).Values["Role"].ValueType);

            var role = ((JsonElementObject)result).Values["Role"] as JsonElementObject;

            Assert.AreEqual(JsonElementValueType.Number, role.Values["RoleId"].ValueType);
            Assert.AreEqual(10, ((JsonElementNumber)role.Values["RoleId"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, role.Values["Name"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)role.Values["Name"]).Value);

            Assert.AreEqual(JsonElementValueType.Null, role.Values["Status"].ValueType);
            Assert.AreEqual(null, ((JsonElementNullable)role.Values["Status"]).Value);
        }

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var json = "[\"a\",\"b\"]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual("a", ((JsonElementString)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual("b", ((JsonElementString)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var json = "[1,2]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(1, ((JsonElementNumber)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(2, ((JsonElementNumber)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var json = "[1.5,2.5]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(1.5, ((JsonElementNumber)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.Number, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(2.5, ((JsonElementNumber)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithBools()
        {
            var service = this.GetService();

            var json = "[true,false]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Bool, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(true, ((JsonElementBool)((JsonElementArray)result).Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.Bool, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(false, ((JsonElementBool)((JsonElementArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithObjects()
        {
            var service = this.GetService();

            var json = "[{\"Name\":\"a\"},{\"Name\":\"b\"}]";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Array, result.ValueType);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementArray)result).Values[0].ValueType);
            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Name"].ValueType);
            Assert.AreEqual("a", ((JsonElementString) ((JsonElementObject)((JsonElementArray)result).Values[0]).Values["Name"]).Value);

            Assert.AreEqual(JsonElementValueType.Object, ((JsonElementArray)result).Values[1].ValueType);
            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Name"].ValueType);
            Assert.AreEqual("b", ((JsonElementString)((JsonElementObject)((JsonElementArray)result).Values[1]).Values["Name"]).Value);

        }

        [TestMethod]
        public void TestObject_WithInnerArray()
        {
            var service = this.GetService();

            var json = "{\"UserName\":\"Marie\",\"Strings\":[\"a\",\"b\"]}";

            var result = service.ToJsonValue(json);

            Assert.AreEqual(JsonElementValueType.Object, result.ValueType);

            Assert.AreEqual(JsonElementValueType.String, ((JsonElementObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((JsonElementString)((JsonElementObject)result).Values["UserName"]).Value);

            Assert.AreEqual(JsonElementValueType.Array, ((JsonElementObject)result).Values["Strings"].ValueType);

            var array = ((JsonElementObject)result).Values["Strings"] as JsonElementArray;

            Assert.AreEqual(2,array.Values.Count);

            Assert.AreEqual(JsonElementValueType.String,array.Values[0].ValueType);
            Assert.AreEqual("a",((JsonElementString) array.Values[0]).Value);

            Assert.AreEqual(JsonElementValueType.String, array.Values[1].ValueType);
            Assert.AreEqual("b", ((JsonElementString)array.Values[1]).Value);

        }
    }
}
