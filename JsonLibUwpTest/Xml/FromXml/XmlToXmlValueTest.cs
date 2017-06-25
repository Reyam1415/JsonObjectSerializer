using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Xml;
using System.Collections.Generic;
using JsonLib.Json;

namespace JsonLibTest
{
    [TestClass]
    public class XmlToXmlValueUwpTest
    {
        public XmlToXmlValue GetService()
        {
            return new XmlToXmlValue();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();
            var xml = "<String>my value</String>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("String", result.NodeName);
            Assert.AreEqual("my value",((XmlString) result).Value);
        }

        [TestMethod]
        public void TestStringEmpty()
        {
            var service = this.GetService();
            var xml = "<String />";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("String", result.NodeName);
            Assert.AreEqual("", ((XmlString)result).Value);
            Assert.AreEqual(false, ((XmlString)result).IsNil);
        }

        [TestMethod]
        public void TestStringNil()
        {
            var service = this.GetService();
            var xml = "<String xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("String", result.NodeName);
            Assert.AreEqual(null, ((XmlString)result).Value);
            Assert.AreEqual(true, ((XmlString)result).IsNil);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            var service = this.GetService();
            var xml = "<Int32>10</Int32>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Number, result.ValueType);
            Assert.AreEqual("Int32", result.NodeName);
            Assert.AreEqual(10, ((XmlNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();
            var xml = "<Double>10.5</Double>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Number, result.ValueType);
            Assert.AreEqual("Double", result.NodeName);
            Assert.AreEqual(10.5, ((XmlNumber)result).Value);
        }

        // bool

        [TestMethod]
        public void TestBool_WithTrue()
        {
            var service = this.GetService();
            var xml = "<Boolean>true</Boolean>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Bool, result.ValueType);
            Assert.AreEqual("Boolean", result.NodeName);
            Assert.AreEqual(true, ((XmlBool)result).Value);
        }

        [TestMethod]
        public void TestBool_WithFalse()
        {
            var service = this.GetService();
            var xml = "<Boolean>false</Boolean>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Bool, result.ValueType);
            Assert.AreEqual("Boolean", result.NodeName);
            Assert.AreEqual(false, ((XmlBool)result).Value);
        }

        // Guid

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var xml = "<Guid>344ac1a2-9613-44d7-b64c-8d45b4585176</Guid>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("Guid", result.NodeName);
            Assert.AreEqual("344ac1a2-9613-44d7-b64c-8d45b4585176", ((XmlString)result).Value);
            Assert.AreEqual(false, ((XmlString)result).IsNil);
        }

        // DateTime

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var xml = "<DateTime>12/12/1990 00:00:00</DateTime>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("DateTime", result.NodeName);
            Assert.AreEqual("12/12/1990 00:00:00", ((XmlString)result).Value);
            Assert.AreEqual(false, ((XmlString)result).IsNil);
        }

        // enum

        [TestMethod]
        public void TestEnum()
        {
            var service = this.GetService();
            var xml = "<MyEnum>Other</MyEnum>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("MyEnum", result.NodeName);
            Assert.AreEqual("Other", ((XmlString)result).Value);
            Assert.AreEqual(false, ((XmlString)result).IsNil);
        }

        // array

        [TestMethod]
        public void TestArrayOfInt()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32><Int32>2</Int32></ArrayOfInt32>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            var xmlArray = result as XmlArray;

            Assert.AreEqual(2, xmlArray.Values.Count);

            Assert.AreEqual(XmlValueType.Number, xmlArray.Values[0].ValueType);
            Assert.AreEqual(1, ((XmlNumber) xmlArray.Values[0]).Value);

            Assert.AreEqual(XmlValueType.Number, xmlArray.Values[1].ValueType);
            Assert.AreEqual(2, ((XmlNumber)xmlArray.Values[1]).Value);
        }

        [TestMethod]
        public void TestArrayOfDoubles()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double><Double>2.5</Double></ArrayOfDouble>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfDouble", result.NodeName);

            var xmlArray = result as XmlArray;

            Assert.AreEqual(2, xmlArray.Values.Count);

            Assert.AreEqual(XmlValueType.Number, xmlArray.Values[0].ValueType);
            Assert.AreEqual(1.5, ((XmlNumber)xmlArray.Values[0]).Value);

            Assert.AreEqual(XmlValueType.Number, xmlArray.Values[1].ValueType);
            Assert.AreEqual(2.5, ((XmlNumber)xmlArray.Values[1]).Value);
        }

        [TestMethod]
        public void TestArrayOfString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfString><String>my value 1</String><String>my value 2</String></ArrayOfString>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfString", result.NodeName);

            var xmlArray = result as XmlArray;

            Assert.AreEqual(2, xmlArray.Values.Count);

            Assert.AreEqual(XmlValueType.String, xmlArray.Values[0].ValueType);
            Assert.AreEqual("my value 1", ((XmlString)xmlArray.Values[0]).Value);

            Assert.AreEqual(XmlValueType.String, xmlArray.Values[1].ValueType);
            Assert.AreEqual("my value 2", ((XmlString)xmlArray.Values[1]).Value);
        }

        [TestMethod]
        public void TestArrayOfBool()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfBoolean", result.NodeName);

            var xmlArray = result as XmlArray;

            Assert.AreEqual(2, xmlArray.Values.Count);

            Assert.AreEqual(XmlValueType.Bool, xmlArray.Values[0].ValueType);
            Assert.AreEqual(true, ((XmlBool)xmlArray.Values[0]).Value);

            Assert.AreEqual(XmlValueType.Bool, xmlArray.Values[1].ValueType);
            Assert.AreEqual(false, ((XmlBool)xmlArray.Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithOnlyOneItem_GuessIsObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfString><String>my value 1</String></ArrayOfString>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("ArrayOfString", result.NodeName);

            var xmlValue = result as XmlObject;

            Assert.AreEqual(1, xmlValue.Values.Count);

            Assert.AreEqual(XmlValueType.String, xmlValue.Values["String"].ValueType);
            Assert.AreEqual("my value 1", ((XmlString)xmlValue.Values["String"]).Value);
        }

        // object

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var model = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var xml = "<MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value</MyString><MyBool>true</MyBool><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value</MyInnerString></MyObj><MyList><String>a</String><String>b</String></MyList><MyArray><String>y</String><String>z</String></MyArray></MyItem>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("MyItem", result.NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid.ToString(), ((XmlString)((XmlObject)result).Values["MyGuid"]).Value);
            Assert.AreEqual("MyGuid", ((XmlString)((XmlObject)result).Values["MyGuid"]).NodeName);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((XmlNumber)((XmlObject)result).Values["MyInt"]).Value);
            Assert.AreEqual("MyInt", ((XmlNumber)((XmlObject)result).Values["MyInt"]).NodeName);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((XmlNumber)((XmlObject)result).Values["MyDouble"]).Value);
            Assert.AreEqual("MyDouble", ((XmlNumber)((XmlObject)result).Values["MyDouble"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyString"].ValueType);
            Assert.AreEqual(model.MyString, ((XmlString)((XmlObject)result).Values["MyString"]).Value);
            Assert.AreEqual("MyString", ((XmlString)((XmlObject)result).Values["MyString"]).NodeName);

            Assert.AreEqual(XmlValueType.Bool, ((XmlObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((XmlBool)((XmlObject)result).Values["MyBool"]).Value);
            Assert.AreEqual("MyBool", ((XmlBool)((XmlObject)result).Values["MyBool"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyEnumValue"].ValueType);
            Assert.AreEqual("Other", ((XmlString)((XmlObject)result).Values["MyEnumValue"]).Value);
            Assert.AreEqual("MyEnumValue", ((XmlString)((XmlObject)result).Values["MyEnumValue"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate.ToString(), ((XmlString)((XmlObject)result).Values["MyDate"]).Value);
            Assert.AreEqual("MyDate", ((XmlString)((XmlObject)result).Values["MyDate"]).NodeName);

            Assert.AreEqual(XmlValueType.Object, ((XmlObject)result).Values["MyObj"].ValueType);
            Assert.AreEqual("MyObj", ((XmlObject)result).Values["MyObj"].NodeName);

            var myObj = ((XmlObject)result).Values["MyObj"] as XmlObject;

            Assert.AreEqual(XmlValueType.String, myObj.Values["MyInnerString"].ValueType);
            Assert.AreEqual("MyInnerString", myObj.Values["MyInnerString"].NodeName);
            Assert.AreEqual(model.MyObj.MyInnerString, ((XmlString)myObj.Values["MyInnerString"]).Value);

            Assert.AreEqual(XmlValueType.Array, ((XmlObject)result).Values["MyList"].ValueType);
            Assert.AreEqual("MyList", ((XmlObject)result).Values["MyList"].NodeName);

            var myList = ((XmlObject)result).Values["MyList"] as XmlArray;

            Assert.AreEqual("String", myList.Values[0].NodeName);
            Assert.AreEqual(XmlValueType.String, myList.Values[0].ValueType);
            Assert.AreEqual("a", ((XmlString)myList.Values[0]).Value);

            Assert.AreEqual("String", myList.Values[1].NodeName);
            Assert.AreEqual(XmlValueType.String, myList.Values[1].ValueType);
            Assert.AreEqual("b", ((XmlString)myList.Values[1]).Value);

            Assert.AreEqual(XmlValueType.Array, ((XmlObject)result).Values["MyArray"].ValueType);
            Assert.AreEqual("MyArray", ((XmlObject)result).Values["MyArray"].NodeName);

            var myArray = ((XmlObject)result).Values["MyArray"] as XmlArray;

            Assert.AreEqual("String", myArray.Values[0].NodeName);
            Assert.AreEqual(XmlValueType.String, myArray.Values[0].ValueType);
            Assert.AreEqual("y", ((XmlString)myArray.Values[0]).Value);

            Assert.AreEqual("String", myArray.Values[1].NodeName);
            Assert.AreEqual(XmlValueType.String, myArray.Values[1].ValueType);
            Assert.AreEqual("z", ((XmlString)myArray.Values[1]).Value);
        }


        [TestMethod]
        public void TestObjectNullablesNull()
        {
            var service = this.GetService();

            var xml = "<MyItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyGuid xsi:nil=\"true\" /><MyInt xsi:nil=\"true\" /><MyDouble xsi:nil=\"true\" /><MyString xsi:nil=\"true\" /><MyBool xsi:nil=\"true\" /><MyEnumValue xsi:nil=\"true\" /><MyDate xsi:nil=\"true\" /><MyObj xsi:nil=\"true\" /><MyList xsi:nil=\"true\" /><MyArray xsi:nil=\"true\" /></MyItem>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("MyItem", result.NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyGuid"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyGuid"]).IsNil);
            Assert.AreEqual("MyGuid", ((XmlString)((XmlObject)result).Values["MyGuid"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyInt"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyInt"]).IsNil);
            Assert.AreEqual("MyInt", ((XmlString)((XmlObject)result).Values["MyInt"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyDouble"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyDouble"]).IsNil);
            Assert.AreEqual("MyDouble", ((XmlString)((XmlObject)result).Values["MyDouble"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyString"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyString"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyString"]).IsNil);
            Assert.AreEqual("MyString", ((XmlString)((XmlObject)result).Values["MyString"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyBool"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyBool"]).IsNil);
            Assert.AreEqual("MyBool", ((XmlString)((XmlObject)result).Values["MyBool"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyEnumValue"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyEnumValue"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyEnumValue"]).IsNil);
            Assert.AreEqual("MyEnumValue", ((XmlString)((XmlObject)result).Values["MyEnumValue"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["MyDate"]).Value);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyDate"]).IsNil);
            Assert.AreEqual("MyDate", ((XmlString)((XmlObject)result).Values["MyDate"]).NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyObj"].ValueType);
            Assert.AreEqual("MyObj", ((XmlObject)result).Values["MyObj"].NodeName);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyList"].ValueType);
            Assert.AreEqual("MyList", ((XmlObject)result).Values["MyList"].NodeName);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyList"]).IsNil);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyArray"].ValueType);
            Assert.AreEqual("MyArray", ((XmlObject)result).Values["MyArray"].NodeName);
            Assert.AreEqual(true, ((XmlString)((XmlObject)result).Values["MyArray"]).IsNil);
        }

        // dictionary

        [TestMethod]
        public void TestDictionary_WithStringKeyAndIntValue()
        {
            var service = this.GetService();

            //var value = new Dictionary<string, int>
            //{
            //    {"key1", 10 },
            //    {"key2", 20 },
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfInt32><Int32><Key>key1</Key><Value>10</Value></Int32><Int32><Key>key2</Key><Value>20</Value></Int32></ArrayOfInt32>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            Assert.AreEqual(2, ((XmlArray)result).Values.Count);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            var result1 = ((XmlArray)result).Values[0] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("key1", ((XmlString)result1.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result1.Values["Value"].ValueType);
            Assert.AreEqual("Value", result1.Values["Value"].NodeName);
            Assert.AreEqual(10, ((XmlNumber)result1.Values["Value"]).Value);

            var result2 = ((XmlArray)result).Values[1] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result2.Values["Key"].ValueType);
            Assert.AreEqual("Key", result2.Values["Key"].NodeName);
            Assert.AreEqual("key2", ((XmlString)result2.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result2.Values["Value"].ValueType);
            Assert.AreEqual("Value", result2.Values["Value"].NodeName);
            Assert.AreEqual(20, ((XmlNumber)result2.Values["Value"]).Value);
        }

        [TestMethod]
        public void TestDictionary_WithStringKeyAndIntValueGuesObject()
        {
            var service = this.GetService();

            //var value = new Dictionary<string, int>
            //{
            //    {"key1", 10 },
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfInt32><Int32><Key>key1</Key><Value>10</Value></Int32></ArrayOfInt32>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            Assert.AreEqual(XmlValueType.Object, ((XmlObject)result).Values["Int32"].ValueType);

            var result1 = ((XmlObject)result).Values["Int32"] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("key1", ((XmlString)result1.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result1.Values["Value"].ValueType);
            Assert.AreEqual("Value", result1.Values["Value"].NodeName);
            Assert.AreEqual(10, ((XmlNumber)result1.Values["Value"]).Value);
        }

        [TestMethod]
        public void TestDictionary_WithTypeKeyAndIntValue()
        {
            var service = this.GetService();

            //var value = new Dictionary<Type, int>
            //{
            //    {typeof(MyItem), 10 },
            //    {typeof(MyItem2), 20 },
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfInt32><Int32><Key>JsonLibTest.MyItem</Key><Value>10</Value></Int32><Int32><Key>JsonLibTest.MyItem2</Key><Value>20</Value></Int32></ArrayOfInt32>";

            var result = service.ToXmlValue(xml);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            Assert.AreEqual(2, ((XmlArray)result).Values.Count);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            var result1 = ((XmlArray)result).Values[0] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("JsonLibTest.MyItem", ((XmlString)result1.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result1.Values["Value"].ValueType);
            Assert.AreEqual("Value", result1.Values["Value"].NodeName);
            Assert.AreEqual(10, ((XmlNumber)result1.Values["Value"]).Value);

            var result2 = ((XmlArray)result).Values[1] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result2.Values["Key"].ValueType);
            Assert.AreEqual("Key", result2.Values["Key"].NodeName);
            Assert.AreEqual("JsonLibTest.MyItem2", ((XmlString)result2.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result2.Values["Value"].ValueType);
            Assert.AreEqual("Value", result2.Values["Value"].NodeName);
            Assert.AreEqual(20, ((XmlNumber)result2.Values["Value"]).Value);
        }
    }
}
