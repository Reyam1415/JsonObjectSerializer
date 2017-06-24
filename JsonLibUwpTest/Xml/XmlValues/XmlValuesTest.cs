using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib.Xml;

namespace JsonLibTest.Xml.XmlValues
{
    [TestClass]
    public class XmlValuesUwpTest
    {
        // string

        [TestMethod]
        public void TestCreateString()
        {
            var key = "MyString";
            var value = "my string";

            var result = XmlValue.CreateString(key,value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.String, result.ValueType);
        }

        [TestMethod]
        public void TestCreateString_WithNull()
        {
            var key = "MyString";
            string value = null;
            var result = XmlValue.CreateString(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.String, result.ValueType);
        }

        [TestMethod]
        public void TestSetNilString()
        {
            var key = "MyString";
            var key2 = "MyString2";
            var value = "my string";

            var result = XmlValue.CreateString(key, value) as XmlString;

            Assert.IsFalse(result.IsNil);

            var result2 = XmlValue.CreateString(key2, null) as XmlString;

            Assert.IsTrue(result2.IsNil);
        }

        // number

        [TestMethod]
        public void TestCreateNumber_WithInt()
        {
            var key = "MyNumber";
            var value = 10;
            var result = XmlValue.CreateNumber(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithDouble()
        {
            var key = "MyNumber";
            var value = 10.99;
            var result = XmlValue.CreateNumber(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Number, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNumber_WithoutANumber_Fail()
        {
            var key = "MyNumber";
            bool failed = false;
            var value = "my value";

            try
            {
                var result = XmlValue.CreateNumber(key, value);
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
            var key = "MyBool";
            var value = true;
            var result = XmlValue.CreateBool(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Bool, result.ValueType);
        }

        [TestMethod]
        public void TestCreateBool_WithFalse()
        {
            var key = "MyBool";
            var value = false;
            var result = XmlValue.CreateBool(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Bool, result.ValueType);
        }


        // nullable

        [TestMethod]
        public void TestCreateNullable()
        {
            var key = "MyNullable";
            var value = 10;
            var result = XmlValue.CreateNullable<int?>(key,value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
        }

        [TestMethod]
        public void TestCreateNullable_WithNull()
        {
            var key = "MyNullable";
            var value = 10;
            var result = XmlValue.CreateNullable<int?>(key, value);

            Assert.AreEqual(key, result.NodeName);
            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
        }


        [TestMethod]
        public void TestSetNilNullable()
        {
            var key = "MyNullable";
            var key2 = "MyNullable";
            var value = 10;

            var result = XmlValue.CreateNullable<int?>(key,value);

            Assert.IsFalse(result.IsNil);

            var result2 = XmlValue.CreateNullable<int?>(key2, null);

            Assert.IsTrue(result2.IsNil);

        }

        // object

        [TestMethod]
        public void TestCreateObject()
        {
            var result = XmlValue.CreateObject("myitem")
                .AddString("mystring", "my value")
                .AddNumber("mynumber", 10)
                .AddBool("mybool", true)
                .AddNullable<int?>("mynullable", 100);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("myitem", result.NodeName);

            Assert.IsFalse(result.HasNils);

            Assert.AreEqual(XmlValueType.String, result.Values["mystring"].ValueType);
            Assert.AreEqual("my value", ((XmlString)result.Values["mystring"]).Value);

            Assert.AreEqual(XmlValueType.Number, result.Values["mynumber"].ValueType);
            Assert.AreEqual(10, ((XmlNumber)result.Values["mynumber"]).Value);

            Assert.AreEqual(XmlValueType.Bool, result.Values["mybool"].ValueType);
            Assert.AreEqual(true, ((XmlBool)result.Values["mybool"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, result.Values["mynullable"].ValueType);
            Assert.AreEqual(100, ((XmlNullable)result.Values["mynullable"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithNullableNull()
        {
            var result = XmlValue.CreateObject("myitem")
                .AddNullable<int?>("mynullable", null);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual(true, result.HasNils);

            Assert.IsTrue(result.HasNils);

            Assert.AreEqual(XmlValueType.Nullable, result.Values["mynullable"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)result.Values["mynullable"]).Value);
            Assert.AreEqual(true, ((XmlNullable)result.Values["mynullable"]).IsNil);
        }

        [TestMethod]
        public void TestCreateObjectNil()
        {
            var result = XmlValue.CreateObject("myitem");

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.IsFalse(result.IsNil);

            var result2 = XmlValue.CreateObject("myitem");
            result2.SetNil();

            Assert.AreEqual(XmlValueType.Object, result2.ValueType);
            Assert.IsTrue(result2.IsNil);
        }

        [TestMethod]
        public void TestCreateObject_WithStringNull_SetHasNilsTrue()
        {
            var result = XmlValue.CreateObject("MyItem");

            result.AddString("MyString1", "My Value 1");

            Assert.IsFalse(result.HasNils);

            result.AddString("MyString2", null);

            Assert.IsTrue(result.HasNils);
        }

        [TestMethod]
        public void TestCreateObject_WithNullableNull_SetHasNilsTrue()
        {
            var result = XmlValue.CreateObject("MyItem");

            result.AddNullable<int?>("MyNullable1", 10);

            Assert.IsFalse(result.HasNils);

            result.AddNullable<int?>("MyNullable2", null);

            Assert.IsTrue(result.HasNils);
        }

        [TestMethod]
        public void TestCreateObject_WithObjNull_SetHasNilsTrue()
        {
            var result = XmlValue.CreateObject("MyItem");

            result.AddObject("MyObj1", XmlValue.CreateObject("MyObj1"));

            Assert.IsFalse(result.HasNils);

            result.AddObject("MyObj2", XmlValue.CreateObject("MyObj2").SetNil());

            Assert.IsTrue(result.HasNils);
        }

        [TestMethod]
        public void TestCreateObject_WithArrayNull_SetHasNilsTrue()
        {
            var result = XmlValue.CreateObject("MyItem");

            result.AddArray("MyArray1", XmlValue.CreateArray("MyArray1"));

            Assert.IsFalse(result.HasNils);

            result.AddArray("MyArray2", XmlValue.CreateArray("MyArray2").SetNil());

            Assert.IsTrue(result.HasNils);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerObject()
        {
            var result = XmlValue.CreateObject("myitem")
                .AddString("mystring", "my value")
                .AddObject("myobj", XmlValue.CreateObject("obj").AddString("key", "string value"));

            Assert.AreEqual(XmlValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((XmlString)result.Values["mystring"]).Value);
            Assert.AreEqual(XmlValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(XmlValueType.Object, result.Values["myobj"].ValueType);
            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values["myobj"]).Values["key"].ValueType);
            Assert.AreEqual("string value", ((XmlString)((XmlObject)result.Values["myobj"]).Values["key"]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithInnerArray()
        {
            var result = XmlValue.CreateObject("myitem")
                .AddString("mystring", "my value")
                .AddArray("myarray", XmlValue.CreateArray("items").Add(new XmlString("InnerString","a")).Add(new XmlString("InnerString", "b")));

            Assert.AreEqual(XmlValueType.Object, result.ValueType);

            Assert.AreEqual("my value", ((XmlString)result.Values["mystring"]).Value);
            Assert.AreEqual(XmlValueType.String, result.Values["mystring"].ValueType);

            Assert.AreEqual(XmlValueType.Array, result.Values["myarray"].ValueType);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result.Values["myarray"]).Values[0].ValueType);
            Assert.AreEqual("a", ((XmlString)((XmlArray)result.Values["myarray"]).Values[0]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result.Values["myarray"]).Values[1].ValueType);
            Assert.AreEqual("b", ((XmlString)((XmlArray)result.Values["myarray"]).Values[1]).Value);
        }

        [TestMethod]
        public void TestCreateObject_WithSameName_Fail()
        {
            bool failed = false;
            try
            {
                var result = XmlValue.CreateObject("item")
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
        public void TestAddToArray_WithNotSameType_Fail()
        {
            bool failed = false;

            var result = XmlValue.CreateArray("items")
                .Add(new XmlString("MyString", "my value 1"));

            try
            {
                result.Add(new XmlNumber("MyNumber", 10));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestAddToArray_WithNotSameNodeName_Fail()
        {
            bool failed = false;

            var result = XmlValue.CreateArray("items")
                .Add(new XmlString("MyString", "my value 1"));

            try
            {
                result.Add(new XmlString("MyString2", "my value 2"));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestAddToArray_WithSameTypeAndNodeName_Success()
        {
            bool failed = false;

            var result = XmlValue.CreateArray("items")
                .Add(new XmlString("MyString", "my value 1"));

            try
            {
                result.Add(new XmlString("MyString", "my value 2"));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestCreateArray()
        {
            var result = XmlValue.CreateArray("items")
                .Add(new XmlString("MyString","my value 1"))
                .Add(new XmlString("MyString", "my value 2"));

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("items", result.NodeName);

            Assert.AreEqual("my value 1", ((XmlString)result.Values[0]).Value);
            Assert.AreEqual("my value 2", ((XmlString)result.Values[1]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithObject()
        {
            var result = XmlValue.CreateArray("Items")
                .Add(XmlValue.CreateObject("Obj").AddString("MyString", "my value"));

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.Object, result.Values[0].ValueType);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values[0]).Values["MyString"].ValueType);
            Assert.AreEqual("my value", ((XmlString)((XmlObject)result.Values[0]).Values["MyString"]).Value);
        }

        [TestMethod]
        public void TestCreateArray_WithArray()
        {
            var result = XmlValue.CreateArray("items")
                .Add(XmlValue.CreateArray("InnerItems").Add(new XmlString("MyString", "my value 1")));

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.Array, result.Values[0].ValueType);

            var innerArray = result.Values[0] as XmlArray;

            Assert.AreEqual(XmlValueType.String, innerArray.Values[0].ValueType);
            Assert.AreEqual("my value 1", ((XmlString)innerArray.Values[0]).Value);
        }

    }
}
