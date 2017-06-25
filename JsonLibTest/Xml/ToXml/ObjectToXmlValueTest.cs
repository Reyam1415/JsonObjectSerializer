using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib.Xml;
using JsonLibTest.Services;
using System.Collections.Generic;
using JsonLib.Mappings.Xml;
using JsonLib;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToXmlValueTest
    {

        public ObjectToXmlValue GetService()
        {
            return new ObjectToXmlValue();
        }

      
        // get property name

        [TestMethod]
        public void TestGetXmlPropertyName_WithMapping()
        {
            var service = this.GetService();

            var mapping = new XmlTypeMapping(typeof(User), "MapUser")
                .SetProperty("UserName", "map_username");

            var result = service.GetXmlPropertyName("UserName", mapping);

            Assert.AreEqual("map_username", result);
        }

        [TestMethod]
        public void TestGetXmlPropertyName_WithMappingNotHaveProperty_ReturnsPropertyName()
        {
            var service = this.GetService();

            var mapping = new XmlTypeMapping(typeof(User), "MapUser")
                .SetProperty("UserName", "map_username");

            var result = service.GetXmlPropertyName("Id", mapping);

            Assert.AreEqual("Id", result);
        }


        // to xml value


        [TestMethod]
        public void TestToXmlValue_WithNoValueAndNullable()
        {
            var service = this.GetService();

            var result = service.ToXmlValue<int?>(null);
            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual(null, ((XmlNullable)result).Value);
        }

        //[TestMethod]
        //public void TestType()
        //{
        //    var service = this.GetService();

        //    var result = service.ToXmlValue(typeof(MyItem));

        //    //Assert.AreEqual(XmlValueType.String, result.ValueType);
        //    //Assert.AreEqual(typeof(XmlString), result.GetType());
        //    //Assert.AreEqual("String", result.NodeName);
        //    //Assert.AreEqual("my string", ((XmlString)result).Value);
        //}

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var result = service.ToXmlValue("my string");

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual(typeof(XmlString), result.GetType());
            Assert.AreEqual("String", result.NodeName);
            Assert.AreEqual("my string", ((XmlString)result).Value);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var result = service.ToXmlValue<string>(null);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("String", result.NodeName);
            Assert.AreEqual(null, ((XmlString)result).Value);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var value = Guid.NewGuid();
            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("Guid", result.NodeName);
            Assert.AreEqual(value.ToString(), ((XmlString)result).Value);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var value = new DateTime(1990, 12, 12);
            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("DateTime", result.NodeName);
            Assert.AreEqual(value.ToString(), ((XmlString)result).Value);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            // number int | double | enum value 
            var service = this.GetService();

            int value = 10;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Number, result.ValueType);
            Assert.AreEqual("Int32", result.NodeName);
            Assert.AreEqual(value, ((XmlNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithInt64()
        {
            // number int | double | enum value 
            var service = this.GetService();

            Int64 value = 10;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Number, result.ValueType);
            Assert.AreEqual("Int64", result.NodeName);
            Assert.AreEqual(value, ((XmlNumber)result).Value);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            // number int | double | enum value 
            var service = this.GetService();

            double value = 10.5;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Number, result.ValueType);
            Assert.AreEqual("Double", result.NodeName);
            Assert.AreEqual(value, ((XmlNumber)result).Value);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var result = service.ToXmlValue(true);

            Assert.AreEqual(XmlValueType.Bool, result.ValueType);
            Assert.AreEqual("Boolean", result.NodeName);
            Assert.AreEqual(true, ((XmlBool)result).Value);
        }

        // nullable

        [TestMethod]
        public void TestNullable_WithInt()
        {
            var service = this.GetService();

            int? value = 10;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Int32", result.NodeName);
            Assert.AreEqual(10, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithIntNull()
        {
            var service = this.GetService();

            int? value = null;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Int32", result.NodeName);
            Assert.AreEqual(null, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuid()
        {
            var service = this.GetService();

            Guid? value = Guid.NewGuid();

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Guid", result.NodeName);
            Assert.AreEqual(value, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithGuidNull()
        {
            var service = this.GetService();

            Guid? value = null;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Guid", result.NodeName);
            Assert.AreEqual(null, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBool()
        {
            var service = this.GetService();

            bool? value = true;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Boolean", result.NodeName);
            Assert.AreEqual(value, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithBoolNull()
        {
            var service = this.GetService();

            bool? value = null;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("Boolean", result.NodeName);
            Assert.AreEqual(null, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTime()
        {
            var service = this.GetService();

            DateTime? value = new DateTime(1990, 12, 12);

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("DateTime", result.NodeName);
            Assert.AreEqual(value, ((XmlNullable)result).Value);
        }

        [TestMethod]
        public void TestNullable_WithDateTimeNull()
        {
            var service = this.GetService();

            DateTime? value = null;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Nullable, result.ValueType);
            Assert.AreEqual("DateTime", result.NodeName);
            Assert.AreEqual(null, ((XmlNullable)result).Value);
        }

        // enum

        [TestMethod]
        public void TestEnum()
        {
            var service = this.GetService();

            var value = AssemblyEnum.Other;

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.String, result.ValueType);
            Assert.AreEqual("AssemblyEnum", result.NodeName);
            Assert.AreEqual("Other", ((XmlString)result).Value);
        }


        // to json object

        [TestMethod]
        public void TestObjectComplete()
        {
            var service = this.GetService();

            var model = new AssemblyItem
            {
                MyGuid = Guid.NewGuid(),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new AssemblyInner { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var result = service.ToXmlValue(model);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("AssemblyItem", result.NodeName);

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

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual("Other", ((XmlString)((XmlObject)result).Values["MyEnum"]).Value);
            Assert.AreEqual("MyEnum", ((XmlString)((XmlObject)result).Values["MyEnum"]).NodeName);

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
            Assert.AreEqual("a", ((XmlString) myList.Values[0]).Value);

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
        public void TestObject_WithNullablesNotNull()
        {
            // Json nullable => value object

            var service = this.GetService();

            var model = new ItemNullable
            {
                MyGuid = Guid.NewGuid(),
                MyInt = 1,
                MyDouble = 1.5,
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12)
            };

            var result = service.ToXmlValue(model);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("ItemNullable", result.NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(model.MyGuid, ((XmlNullable)((XmlObject)result).Values["MyGuid"]).Value);
            Assert.AreEqual("MyGuid", ((XmlNullable)((XmlObject)result).Values["MyGuid"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(model.MyInt, ((XmlNullable)((XmlObject)result).Values["MyInt"]).Value);
            Assert.AreEqual("MyInt", ((XmlNullable)((XmlObject)result).Values["MyInt"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(model.MyDouble, ((XmlNullable)((XmlObject)result).Values["MyDouble"]).Value);
            Assert.AreEqual("MyDouble", ((XmlNullable)((XmlObject)result).Values["MyDouble"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(model.MyBool, ((XmlNullable)((XmlObject)result).Values["MyBool"]).Value);
            Assert.AreEqual("MyBool", ((XmlNullable)((XmlObject)result).Values["MyBool"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(AssemblyEnum.Other, ((XmlNullable)((XmlObject)result).Values["MyEnum"]).Value);
            Assert.AreEqual("MyEnum", ((XmlNullable)((XmlObject)result).Values["MyEnum"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(model.MyDate, ((XmlNullable)((XmlObject)result).Values["MyDate"]).Value);
            Assert.AreEqual("MyDate", ((XmlNullable)((XmlObject)result).Values["MyDate"]).NodeName);
        }

        [TestMethod]
        public void TestObject_WithNullablesNull()
        {
            var service = this.GetService();

            var model = new ItemNullable();

            var result = service.ToXmlValue(model);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);
            Assert.AreEqual("ItemNullable", result.NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyGuid"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyGuid"]).Value);
            Assert.AreEqual("MyGuid", ((XmlNullable)((XmlObject)result).Values["MyGuid"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyInt"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyInt"]).Value);
            Assert.AreEqual("MyInt", ((XmlNullable)((XmlObject)result).Values["MyInt"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyDouble"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyDouble"]).Value);
            Assert.AreEqual("MyDouble", ((XmlNullable)((XmlObject)result).Values["MyDouble"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyBool"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyBool"]).Value);
            Assert.AreEqual("MyBool", ((XmlNullable)((XmlObject)result).Values["MyBool"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyEnum"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyEnum"]).Value);
            Assert.AreEqual("MyEnum", ((XmlNullable)((XmlObject)result).Values["MyEnum"]).NodeName);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["MyDate"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["MyDate"]).Value);
            Assert.AreEqual("MyDate", ((XmlNullable)((XmlObject)result).Values["MyDate"]).NodeName);
        }

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 10,
                UserName = "Marie"
            };

            var result = service.ToXmlValue(user);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result).Values["Id"].ValueType);
            Assert.AreEqual(10, ((XmlNumber)((XmlObject)result).Values["Id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((XmlString)((XmlObject)result).Values["UserName"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["Age"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["Email"].ValueType);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 10,
                UserName = "Marie"
            };

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("user")
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToXmlValue(user, mappings);

            Assert.AreEqual(XmlValueType.Object, result.ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result).Values["map_id"].ValueType);
            Assert.AreEqual("map_id", ((XmlObject)result).Values["map_id"].NodeName);
            Assert.AreEqual(10, ((XmlNumber)((XmlObject)result).Values["map_id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["map_username"].ValueType);
            Assert.AreEqual("map_username", ((XmlObject)result).Values["map_username"].NodeName);
            Assert.AreEqual("Marie", ((XmlString)((XmlObject)result).Values["map_username"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result).Values["Age"].ValueType);
            Assert.AreEqual("Age", ((XmlObject)result).Values["Age"].NodeName);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result).Values["map_email"].ValueType);
            Assert.AreEqual("map_email", ((XmlObject)result).Values["map_email"].NodeName);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result).Values["map_email"]).Value);
        }

        // array

        [TestMethod]
        public void TestArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };


            var result = service.ToXmlValue(array);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result).Values[0].ValueType);
            Assert.AreEqual("String", ((XmlArray)result).Values[0].NodeName);
            Assert.AreEqual("a", ((XmlString)((XmlArray)result).Values[0]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result).Values[1].ValueType);
            Assert.AreEqual("String", ((XmlArray)result).Values[1].NodeName);
            Assert.AreEqual("b", ((XmlString)((XmlArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var array = new int[] { 1, 2 };

            var result = service.ToXmlValue(array);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlArray)result).Values[0].ValueType);
            Assert.AreEqual("Int32", ((XmlArray)result).Values[0].NodeName);
            Assert.AreEqual(1, ((XmlNumber)((XmlArray)result).Values[0]).Value);

            Assert.AreEqual(XmlValueType.Number, ((XmlArray)result).Values[1].ValueType);
            Assert.AreEqual("Int32", ((XmlArray)result).Values[1].NodeName);
            Assert.AreEqual(2, ((XmlNumber)((XmlArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestArray_WithDoubles()
        {
            var service = this.GetService();

            var array = new double[] { 1.5, 2.5 };

            var result = service.ToXmlValue(array);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlArray)result).Values[0].ValueType);
            Assert.AreEqual("Double", ((XmlArray)result).Values[0].NodeName);
            Assert.AreEqual(1.5, ((XmlNumber)((XmlArray)result).Values[0]).Value);

            Assert.AreEqual(XmlValueType.Number, ((XmlArray)result).Values[1].ValueType);
            Assert.AreEqual("Double", ((XmlArray)result).Values[1].NodeName);
            Assert.AreEqual(2.5, ((XmlNumber)((XmlArray)result).Values[1]).Value);
        }


        [TestMethod]
        public void TestList()
        {
            var service = this.GetService();

            var array = new List<string> { "a", "b" };

            var result = service.ToXmlValue(array);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result).Values[0].ValueType);
            Assert.AreEqual("String", ((XmlArray)result).Values[0].NodeName);
            Assert.AreEqual("a", ((XmlString)((XmlArray)result).Values[0]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlArray)result).Values[1].ValueType);
            Assert.AreEqual("String", ((XmlArray)result).Values[1].NodeName);
            Assert.AreEqual("b", ((XmlString)((XmlArray)result).Values[1]).Value);
        }

        [TestMethod]
        public void TestListOfObjects()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var result = service.ToXmlValue(array);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfUser", result.NodeName);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)((XmlArray)result).Values[0]).Values["Id"].ValueType);
            Assert.AreEqual("Id", ((XmlObject)((XmlArray)result).Values[0]).Values["Id"].NodeName);
            Assert.AreEqual(1, ((XmlNumber)((XmlObject)((XmlArray)result).Values[0]).Values["Id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)((XmlArray)result).Values[0]).Values["UserName"].ValueType);
            Assert.AreEqual("UserName", ((XmlObject)((XmlArray)result).Values[0]).Values["UserName"].NodeName);
            Assert.AreEqual("Marie", ((XmlString)((XmlObject)((XmlArray)result).Values[0]).Values["UserName"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)((XmlArray)result).Values[0]).Values["Age"].ValueType);
            Assert.AreEqual("Age", ((XmlObject)((XmlArray)result).Values[0]).Values["Age"].NodeName);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)((XmlArray)result).Values[0]).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)((XmlArray)result).Values[0]).Values["Email"].ValueType);
            Assert.AreEqual("Email", ((XmlObject)((XmlArray)result).Values[0]).Values["Email"].NodeName);
            Assert.AreEqual(null, ((XmlString)((XmlObject)((XmlArray)result).Values[0]).Values["Email"]).Value);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)((XmlArray)result).Values[1]).Values["Id"].ValueType);
            Assert.AreEqual("Id", ((XmlObject)((XmlArray)result).Values[1]).Values["Id"].NodeName);
            Assert.AreEqual(2, ((XmlNumber)((XmlObject)((XmlArray)result).Values[1]).Values["Id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)((XmlArray)result).Values[1]).Values["UserName"].ValueType);
            Assert.AreEqual("UserName", ((XmlObject)((XmlArray)result).Values[1]).Values["UserName"].NodeName);
            Assert.AreEqual("Pat", ((XmlString)((XmlObject)((XmlArray)result).Values[1]).Values["UserName"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)((XmlArray)result).Values[1]).Values["Age"].ValueType);
            Assert.AreEqual("Age", ((XmlObject)((XmlArray)result).Values[1]).Values["Age"].NodeName);
            Assert.AreEqual(20, ((XmlNullable)((XmlObject)((XmlArray)result).Values[1]).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)((XmlArray)result).Values[1]).Values["Email"].ValueType);
            Assert.AreEqual("Email", ((XmlObject)((XmlArray)result).Values[1]).Values["Email"].NodeName);
            Assert.AreEqual("pat@domain.com", ((XmlString)((XmlObject)((XmlArray)result).Values[1]).Values["Email"]).Value);
        }

        [TestMethod]
        public void TestListOfObjects_WithMappings()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("user")
                .SetArrayName("MyUsers")
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToXmlValue(array, mappings) as XmlArray;

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("MyUsers", result.NodeName);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result.Values[0]).Values["map_id"].ValueType);
            Assert.AreEqual("map_id", ((XmlObject)result.Values[0]).Values["map_id"].NodeName);
            Assert.AreEqual(1, ((XmlNumber)((XmlObject)((XmlArray)result).Values[0]).Values["map_id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values[0]).Values["map_username"].ValueType);
            Assert.AreEqual("map_username", ((XmlObject)result.Values[0]).Values["map_username"].NodeName);
            Assert.AreEqual("Marie", ((XmlString)((XmlObject)result.Values[0]).Values["map_username"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result.Values[0]).Values["Age"].ValueType);
            Assert.AreEqual("Age", ((XmlObject)result.Values[0]).Values["Age"].NodeName);
            Assert.AreEqual(null, ((XmlNullable)((XmlObject)result.Values[0]).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values[0]).Values["map_email"].ValueType);
            Assert.AreEqual("map_email", ((XmlObject)result.Values[0]).Values["map_email"].NodeName);
            Assert.AreEqual(null, ((XmlString)((XmlObject)result.Values[0]).Values["map_email"]).Value);

            Assert.AreEqual(XmlValueType.Number, ((XmlObject)result.Values[1]).Values["map_id"].ValueType);
            Assert.AreEqual("map_id", ((XmlObject)result.Values[1]).Values["map_id"].NodeName);
            Assert.AreEqual(2, ((XmlNumber)((XmlObject)result.Values[1]).Values["map_id"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values[1]).Values["map_username"].ValueType);
            Assert.AreEqual("map_username", ((XmlObject)result.Values[1]).Values["map_username"].NodeName);
            Assert.AreEqual("Pat", ((XmlString)((XmlObject)result.Values[1]).Values["map_username"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, ((XmlObject)result.Values[1]).Values["Age"].ValueType);
            Assert.AreEqual("Age", ((XmlObject)result.Values[1]).Values["Age"].NodeName);
            Assert.AreEqual(20, ((XmlNullable)((XmlObject)result.Values[1]).Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, ((XmlObject)result.Values[1]).Values["map_email"].ValueType);
            Assert.AreEqual("map_email", ((XmlObject)result.Values[0]).Values["map_email"].NodeName);
            Assert.AreEqual("pat@domain.com", ((XmlString)((XmlObject)result.Values[1]).Values["map_email"]).Value);
        }

        // dictionary

        [TestMethod]
        public void TestDictionary_WithStringKeyAndIntValue()
        {
            var service = this.GetService();

            var value = new Dictionary<string, int>
            {
                {"key1", 10 },
                {"key2", 20 },
            };

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            Assert.AreEqual(2, ((XmlArray)result).Values.Count);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            var result1 = ((XmlArray)result).Values[0] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("key1" ,((XmlString) result1.Values["Key"]).Value);

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
        public void TestDictionary_WithExoticKeyAndIntValue()
        {
            var service = this.GetService();

            var value = new Dictionary<Type, int>
            {
                {typeof(MyItem), 10 },
                {typeof(MyItem2), 20 },
            };

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfInt32", result.NodeName);

            Assert.AreEqual(2, ((XmlArray)result).Values.Count);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            var result1 = ((XmlArray)result).Values[0] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("JsonLibTest.MyItem, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ((XmlString)result1.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result1.Values["Value"].ValueType);
            Assert.AreEqual("Value", result1.Values["Value"].NodeName);
            Assert.AreEqual(10, ((XmlNumber)result1.Values["Value"]).Value);

            var result2 = ((XmlArray)result).Values[1] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result2.Values["Key"].ValueType);
            Assert.AreEqual("Key", result2.Values["Key"].NodeName);
            Assert.AreEqual("JsonLibTest.MyItem2, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ((XmlString)result2.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Number, result2.Values["Value"].ValueType);
            Assert.AreEqual("Value", result2.Values["Value"].NodeName);
            Assert.AreEqual(20, ((XmlNumber)result2.Values["Value"]).Value);
        }

        [TestMethod]
        public void TestDictionary_WithStringKeyAndObjectValue()
        {
            var service = this.GetService();

            var value = new Dictionary<string, User>
            {
                {"key1", new User { Id = 1, UserName = "Marie" } },
                {"key2", new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" } },
            };

            var result = service.ToXmlValue(value);

            Assert.AreEqual(XmlValueType.Array, result.ValueType);
            Assert.AreEqual("ArrayOfUser", result.NodeName);

            Assert.AreEqual(2, ((XmlArray)result).Values.Count);

            Assert.AreEqual(XmlValueType.Object, ((XmlArray)result).Values[0].ValueType);

            var result1 = ((XmlArray)result).Values[0] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result1.Values["Key"].ValueType);
            Assert.AreEqual("Key", result1.Values["Key"].NodeName);
            Assert.AreEqual("key1", ((XmlString)result1.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Object, result1.Values["Value"].ValueType);

            var user1 = result1.Values["Value"] as XmlObject;

            Assert.AreEqual(XmlValueType.Number, user1.Values["Id"].ValueType);
            Assert.AreEqual(1, ((XmlNumber)user1.Values["Id"]).Value);

            Assert.AreEqual(XmlValueType.String, user1.Values["UserName"].ValueType);
            Assert.AreEqual("Marie", ((XmlString)user1.Values["UserName"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, user1.Values["Age"].ValueType);
            Assert.AreEqual(null, ((XmlNullable)user1.Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, user1.Values["Email"].ValueType);
            Assert.AreEqual(null, ((XmlString)user1.Values["Email"]).Value);

            var result2 = ((XmlArray)result).Values[1] as XmlObject;

            Assert.AreEqual(XmlValueType.String, result2.Values["Key"].ValueType);
            Assert.AreEqual("key2", ((XmlString)result2.Values["Key"]).Value);

            Assert.AreEqual(XmlValueType.Object, result2.Values["Value"].ValueType);
            Assert.AreEqual("Value", result2.Values["Value"].NodeName);

            var user2 = result2.Values["Value"] as XmlObject;

            Assert.AreEqual(XmlValueType.Number, user2.Values["Id"].ValueType);
            Assert.AreEqual(2, ((XmlNumber)user2.Values["Id"]).Value);

            Assert.AreEqual(XmlValueType.String, user2.Values["UserName"].ValueType);
            Assert.AreEqual("Pat", ((XmlString)user2.Values["UserName"]).Value);

            Assert.AreEqual(XmlValueType.Nullable, user2.Values["Age"].ValueType);
            Assert.AreEqual(20, ((XmlNullable)user2.Values["Age"]).Value);

            Assert.AreEqual(XmlValueType.String, user2.Values["Email"].ValueType);
            Assert.AreEqual("pat@domain.com", ((XmlString)user2.Values["Email"]).Value);
        }
    }
}
