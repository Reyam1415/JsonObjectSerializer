using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Xml;
using System.Collections.Generic;
using JsonLib.Mappings.Xml;
using System.Reflection;

namespace JsonLibTest.Xml.FromXml
{
    [TestClass]
    public class XmlValueToObjectUwpTest
    {
        public XmlValueToObject GetService()
        {
            return new XmlValueToObject();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("String", "my value");

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestStringNull()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("String", null);

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("DateTime", "12/12/1990 00:00:00");

            var result = service.Resolve<DateTime>(xmlValue);

            Assert.AreEqual(new DateTime(1990,12,12), result);
        }

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("Guid", "344ac1a2-9613-44d7-b64c-8d45b4585176");

            var result = service.Resolve<Guid>(xmlValue);

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            var service = this.GetService();

            var xmlValue = new XmlNumber("Int32", 10);

            var result = service.Resolve<int>(xmlValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();

            var xmlValue = new XmlNumber("Double", 10.5);

            var result = service.Resolve<double>(xmlValue);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNumber_WithInt_IsConverted()
        {
            var service = this.GetService();

            var xmlValue = new XmlNumber("Int64", 10);

            var result = service.Resolve<Int64>(xmlValue);

            Assert.AreEqual((Int64)10, result);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlBool("Boolean", true);

            var result = service.Resolve<bool>(xmlValue);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestBool_WIthFalse()
        {
            var service = this.GetService();

            var xmlValue = new XmlBool("Boolean", false);

            var result = service.Resolve<bool>(xmlValue);

            Assert.AreEqual(false, result);
        }

        // nils

        [TestMethod]
        public void TestNilFromString_ToInt()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("Int32", null);

            var result = service.Resolve<int?>(xmlValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNilFromString_ToBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("Boolean", null);

            var result = service.Resolve<bool?>(xmlValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNilFromString_ToObject()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("MyItem", null);

            var result = service.Resolve<MyItem>(xmlValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestNilFromString_ToArray()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("ArrayOfString", null);

            var result = service.Resolve<string[]>(xmlValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestFindProperty()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser").SetProperty("UserName", "MapUserName");

            var result = service.FindProperty(properties, "MapUserName", mappings.Get<User>());

            Assert.AreEqual("UserName", result.Name);
        }

        [TestMethod]
        public void TestDontFindProperty()
        {
            var service = this.GetService();

            var properties = typeof(User).GetProperties();

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser");

            var result = service.FindProperty(properties, "MapUserName", mappings.Get<User>());

            Assert.AreEqual(null, result);
        }


        // object

        [TestMethod]
        public void TestGuessObject()
        {
            var service = this.GetService();

            var xmlValue = new XmlObject("ArrayOfString").AddString("String","value 1");

            var result = service.Resolve<string[]>(xmlValue);

            Assert.AreEqual("value 1", result[0]);
        }

        [TestMethod]
        public void TestCompleteObject()
        {
            var service = this.GetService();

            var item = new MyItem
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

            var xmlValue = new XmlObject("MyItem")
                .AddString("MyGuid", item.MyGuid.ToString())
                .AddNumber("MyInt", item.MyInt)
                .AddNumber("MyDouble", item.MyDouble)
                .AddBool("MyBool", item.MyBool)
                .AddString("MyString", item.MyString)
                .AddString("MyEnumValue", item.MyEnumValue.ToString())
                .AddString("MyDate", item.MyDate.ToString())
                .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value"))
                .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", "a")).Add(new XmlString("String", "b")))
                .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", "y")).Add(new XmlString("String", "z")));

            var result = service.Resolve<MyItem>(xmlValue);

            Assert.IsNotNull(result);
            Assert.AreEqual(new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"), result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(MyEnum.Other, result.MyEnumValue);
            Assert.AreEqual(new DateTime(1990,12,12), result.MyDate);
            Assert.AreEqual("my inner value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }

        [TestMethod]
        public void TestObjectWithMapping()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 1,
                UserName = "Marie",
                Age = 20,
                Email = "marie@domain.com"
            };

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser")
                .SetProperty("Id","MapId")
                .SetProperty("UserName","MapUserName")
                .SetProperty("Email","MapEmail");

            var xmlValue = new XmlObject("MapUser")
                .AddNumber("MapId", user.Id)
                .AddString("MapUserName", user.UserName)
                .AddNullable("Age", user.Age)
                .AddString("MapEmail", user.Email);

            var result = service.Resolve<User>(xmlValue, mappings);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(20, result.Age);
            Assert.AreEqual("marie@domain.com", result.Email);
        }

        // array

        [TestMethod]
        public void TestArrayOfString()
        {
            var service = this.GetService();

            var xmlValue = new XmlArray("ArrayOfString")
                .Add(new XmlString("String", "a"))
                .Add(new XmlString("String", "b"));


            var result = service.Resolve<string[]>(xmlValue);

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestArrayOfInt()
        {
            var service = this.GetService();

            var xmlValue = new XmlArray("ArrayOfInt32")
                .Add(new XmlNumber("Int32", 1))
                .Add(new XmlNumber("Int32", 2));


            var result = service.Resolve<int[]>(xmlValue);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestArrayOfBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlArray("ArrayOfBoolean")
                .Add(new XmlBool("Boolean", true))
                .Add(new XmlBool("Boolean", false));


            var result = service.Resolve<bool[]>(xmlValue);

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestArrayOfObjects()
        {
            var service = this.GetService();

            var item = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value 1",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 1" },
                MyList = new List<string> { "a1", "b1" },
                MyArray = new string[] { "y1", "z1" }
            };

            var item2 = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c3"),
                MyInt = 2,
                MyDouble = 2.5,
                MyString = "my value 2",
                MyBool = false,
                MyEnumValue = MyEnum.Default,
                MyDate = new DateTime(1990, 10, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 2" },
                MyList = new List<string> { "a2", "b2" },
                MyArray = new string[] { "y2", "z2" }
            };

            var xmlArray = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddString("MyGuid", item.MyGuid.ToString())
                    .AddNumber("MyInt", item.MyInt)
                    .AddNumber("MyDouble", item.MyDouble)
                    .AddBool("MyBool", item.MyBool)
                    .AddString("MyString", item.MyString)
                    .AddString("MyEnumValue", item.MyEnumValue.ToString())
                    .AddString("MyDate", item.MyDate.ToString())
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 1"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item.MyList[0])).Add(new XmlString("String", item.MyList[1])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item.MyArray[0])).Add(new XmlString("String", item.MyArray[1]))))
               .Add(new XmlObject("MyItem")
                    .AddString("MyGuid", item2.MyGuid.ToString())
                    .AddNumber("MyInt", item2.MyInt)
                    .AddNumber("MyDouble", item2.MyDouble)
                    .AddString("MyString", item2.MyString)
                    .AddString("MyEnumValue", item2.MyEnumValue.ToString())
                    .AddString("MyDate", item2.MyDate.ToString())
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 2"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item2.MyList[0])).Add(new XmlString("String", item2.MyList[1])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item2.MyArray[0])).Add(new XmlString("String", item2.MyArray[1]))));

            var results = service.Resolve<List<MyItem>>(xmlArray);

            var result = results[0];

            Assert.IsNotNull(result);
            Assert.AreEqual(new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"), result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual("my value 1", result.MyString);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(MyEnum.Other, result.MyEnumValue);
            Assert.AreEqual(new DateTime(1990, 12, 12), result.MyDate);
            Assert.AreEqual("my inner value 1", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a1", result.MyList[0]);
            Assert.AreEqual("b1", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y1", result.MyArray[0]);
            Assert.AreEqual("z1", result.MyArray[1]);

            var result2 = results[1];

            Assert.IsNotNull(result2);
            Assert.AreEqual(new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c3"), result2.MyGuid);
            Assert.AreEqual(2, result2.MyInt);
            Assert.AreEqual(2.5, result2.MyDouble);
            Assert.AreEqual("my value 2", result2.MyString);
            Assert.AreEqual(false, result2.MyBool);
            Assert.AreEqual(MyEnum.Default, result2.MyEnumValue);
            Assert.AreEqual(new DateTime(1990, 10, 12), result2.MyDate);
            Assert.AreEqual("my inner value 2", result2.MyObj.MyInnerString);
            Assert.AreEqual(2, result2.MyList.Count);
            Assert.AreEqual("a2", result2.MyList[0]);
            Assert.AreEqual("b2", result2.MyList[1]);
            Assert.AreEqual(2, result2.MyArray.Length);
            Assert.AreEqual("y2", result2.MyArray[0]);
            Assert.AreEqual("z2", result2.MyArray[1]);
        }

        [TestMethod]
        public void TestArrayWithMapping()
        {
            var service = this.GetService();

            var users = new List<User> {
                 new User
                {
                    Id = 1,
                    UserName = "Marie",
                },
                  new User
                {
                    Id = 2,
                    UserName = "Pat",
                    Age = 20,
                    Email = "pat@domain.com"
                }
             };

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MapUser")
                .SetProperty("Id", "MapId")
                .SetProperty("UserName", "MapUserName")
                .SetProperty("Email", "MapEmail");

            var xmlValue = new XmlArray("MapUsers")
             .Add(new XmlObject("MapUser")
                .AddNumber("MapId", users[0].Id)
                .AddString("MapUserName", users[0].UserName)
                .AddNullable("Age", users[0].Age)
                .AddString("MapEmail", users[0].Email))
             .Add(new XmlObject("MapUser")
                .AddNumber("MapId", users[1].Id)
                .AddString("MapUserName", users[1].UserName)
                .AddNullable("Age", users[1].Age)
                .AddString("MapEmail", users[1].Email));

            var results = service.Resolve<List<User>>(xmlValue, mappings);

            var result = results[0];

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);

            var result2 = results[1];

            Assert.AreEqual(2, result2.Id);
            Assert.AreEqual("Pat", result2.UserName);
            Assert.AreEqual(20, result2.Age);
            Assert.AreEqual("pat@domain.com", result2.Email);
        }
    }
}
