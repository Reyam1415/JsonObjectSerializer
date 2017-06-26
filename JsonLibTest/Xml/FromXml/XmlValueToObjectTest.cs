using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Xml;
using System.Collections.Generic;
using JsonLib.Mappings.Xml;
using System.Collections;

namespace JsonLibTest.Xml.FromXml
{
    [TestClass]
    public class XmlValueToObjectTest
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
        public void TestGuessInt_ToPropertyTypeString()
        {
            var service = this.GetService();

            var xmlValue = new XmlNumber("Int32", 10);

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual("10", result);
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


        [TestMethod]
        public void TestGuessDouble_ToPropertyTypeString()
        {
            var service = this.GetService();

            var xmlValue = new XmlNumber("Double", 10.5);

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual("10.5", result);
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

        [TestMethod]
        public void TestGuessBool_ToPropertyTypeString()
        {
            var service = this.GetService();

            var xmlValue = new XmlBool("Boolean", true);

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGuessBool_ToPropertyTypeString_WithFalse()
        {
            var service = this.GetService();

            var xmlValue = new XmlBool("Boolean", false);

            var result = service.Resolve<string>(xmlValue);

            Assert.AreEqual("false", result);
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
        public void TestGuessObject_WithPropertyTypeIntAndBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlObject("MyItem")
                .AddNumber("MyIntString", 10)
                .AddNumber("MyDoubleString", 10.5)
                .AddBool("MyBoolString", true);

            var result = service.Resolve<MyItemGuess>(xmlValue);

            Assert.AreEqual("10", result.MyIntString);
            Assert.AreEqual("10.5", result.MyDoubleString);
            Assert.AreEqual("true", result.MyBoolString);
        }

        [TestMethod]
        public void TestGuessObject_ToPropertyTypeArray()
        {
            var service = this.GetService();

            var xmlValue = new XmlObject("ArrayOfString").AddString("String", "value 1");

            var result = service.Resolve<string[]>(xmlValue);

            Assert.AreEqual("value 1", result[0]);
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
        public void TestGuessArray_WithPropertyTypeIntAndBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddNumber("MyIntString", 10)
                    .AddNumber("MyDoubleString", 10.5)
                    .AddBool("MyBoolString", true))
                 .Add(new XmlObject("MyItem")
                    .AddNumber("MyIntString", 20)
                    .AddNumber("MyDoubleString", 20.5)
                    .AddBool("MyBoolString", false));

            var result = service.Resolve<MyItemGuess[]>(xmlValue);

            Assert.AreEqual("10", result[0].MyIntString);
            Assert.AreEqual("10.5", result[0].MyDoubleString);
            Assert.AreEqual("true", result[0].MyBoolString);

            Assert.AreEqual("20", result[1].MyIntString);
            Assert.AreEqual("20.5", result[1].MyDoubleString);
            Assert.AreEqual("false", result[1].MyBoolString);
        }

        [TestMethod]
        public void TestGuessList_WithPropertyTypeIntAndBool()
        {
            var service = this.GetService();

            var xmlValue = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddNumber("MyIntString", 10)
                    .AddNumber("MyDoubleString", 10.5)
                    .AddBool("MyBoolString", true))
                 .Add(new XmlObject("MyItem")
                    .AddNumber("MyIntString", 20)
                    .AddNumber("MyDoubleString", 20.5)
                    .AddBool("MyBoolString", false));

            var result = service.Resolve<List<MyItemGuess>>(xmlValue);

            Assert.AreEqual("10", result[0].MyIntString);
            Assert.AreEqual("10.5", result[0].MyDoubleString);
            Assert.AreEqual("true", result[0].MyBoolString);

            Assert.AreEqual("20", result[1].MyIntString);
            Assert.AreEqual("20.5", result[1].MyDoubleString);
            Assert.AreEqual("false", result[1].MyBoolString);
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

        // resolve dictionary key

        [TestMethod]
        public void TestResolveDictionaryKey_WithInt()
        {
            var service = this.GetService();

            var result = service.ResolveDictionaryKey(typeof(int), new XmlNumber("MyItnt", 10));
            
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithGuessInt_ToPropertyTypeString()
        {
            var service = this.GetService();

            var result = service.ResolveDictionaryKey(typeof(string), new XmlNumber("MyItnt", 10));

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithDouble()
        {
            var service = this.GetService();

            var result = service.ResolveDictionaryKey(typeof(double), new XmlNumber("MyDouble", 10.5));

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithGuessDouble_ToPropertyTypeString()
        {
            var service = this.GetService();

            var result = service.ResolveDictionaryKey(typeof(string), new XmlNumber("MyDouble", 10.5));

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithType()
        {
            var service = this.GetService();

            // "JsonLibTest.MyItem, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            var r = typeof(MyItem).AssemblyQualifiedName;

            var result = service.ResolveDictionaryKey(typeof(Type), new XmlString("MyItem", r));

            Assert.AreEqual(typeof(MyItem), result);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithType_ThrowExceptionIfNotFound()
        {
            bool failed = false;

            var service = this.GetService();

            try
            {
                var result = service.ResolveDictionaryKey(typeof(Type), new XmlString("MyItem", "not found"));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithBool_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            try
            {
                var result = service.ResolveDictionaryKey(typeof(bool), new XmlBool("MyBool", true));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);


        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithObject_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            try
            {
                var result = service.ResolveDictionaryKey(typeof(MyItem), new XmlObject("MyItem"));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionaryKey_WithArray_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            try
            {
                var result = service.ResolveDictionaryKey(typeof(MyItem[]), new XmlArray("MyItems"));
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        // dictionary

        [TestMethod]
        public void TestResolveDictionary_WithNoItems_ReturnsEmptyDictionary()
        {
            var service = this.GetService();

            var xmlArray = new XmlArray("ArrayOfString");
            var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestResolveDictionary_WithArrayString_Fail()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlString("MyItem", "value 1"));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithArrayOfBool_Fail()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("Array")
                .Add(new XmlBool("MyItem", true));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithArrayOfArray_Fail()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("Array")
                .Add(new XmlArray("MyArray"));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithObjectButNot2Values_Fail()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem").AddNumber("Int32",10));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithObjectButWith2ValuesButNotGoodType_Fail()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem").AddString("MyString", "10").AddString("String","value 1"));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithObjectButWith2Values_Success()
        {
            bool failed = false;

            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem").AddNumber("Int32", 10).AddString("String", "value 1"));
            try
            {
                var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddString("String", "value 1"))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddString("String", "value 2"));

            var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;

            Assert.AreEqual("value 1",result[10]);
            Assert.AreEqual("value 2", result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithStringKeyStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key1").AddString("String", "value 1"))
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key2").AddString("String", "value 2"));

            var result = service.ToDictionary(typeof(Dictionary<string, string>), xmlArray) as Dictionary<string, string>;

            Assert.AreEqual("value 1", result["key1"]);
            Assert.AreEqual("value 2", result["key2"]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyIntValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddNumber("Value", 100))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddNumber("Value", 200));

            var result = service.ToDictionary(typeof(Dictionary<int, int>), xmlArray) as Dictionary<int, int>;

            Assert.AreEqual(100, result[10]);
            Assert.AreEqual(200, result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyGuessIntStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddNumber("Value", 100))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddNumber("Value", 200));

            var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;

            Assert.AreEqual("100", result[10]);
            Assert.AreEqual("200", result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyGuessDoubleStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddNumber("Value", 1.5))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddNumber("Value", 2.5));

            var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;

            Assert.AreEqual("1.5", result[10]);
            Assert.AreEqual("2.5", result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyBoolValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddBool("Value", true))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddBool("Value", false));

            var result = service.ToDictionary(typeof(Dictionary<int, bool>), xmlArray) as Dictionary<int, bool>;

            Assert.AreEqual(true, result[10]);
            Assert.AreEqual(false, result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithIntKeyGuessBoolStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddBool("Value", true))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddBool("Value", false));

            var result = service.ToDictionary(typeof(Dictionary<int, string>), xmlArray) as Dictionary<int, string>;

            Assert.AreEqual("true", result[10]);
            Assert.AreEqual("false", result[20]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithStringKeyDateTimeValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key1").AddString("Value", "12/12/1990 00:00:00"))
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key2").AddString("Value", "12/10/1990 00:00:00"));

            var result = service.ToDictionary(typeof(Dictionary<string, DateTime>), xmlArray) as Dictionary<string, DateTime>;

            Assert.AreEqual(new DateTime(1990,12,12), result["key1"]);
            Assert.AreEqual(new DateTime(1990, 10, 12), result["key2"]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithStringKeyGuidValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key1").AddString("Value", "344ac1a2-9613-44d7-b64c-8d45b4585176"))
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key2").AddString("Value", "344ac1a2-9613-44d7-b64c-8d45b4585178"));

            var result = service.ToDictionary(typeof(Dictionary<string, Guid>), xmlArray) as Dictionary<string, Guid>;

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result["key1"]);
            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585178"), result["key2"]);
        }
        [TestMethod]
        public void TestResolveDictionary_WithStringKeyEnumValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key1").AddString("Value", "Other"))
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key2").AddString("Value", "Default"));

            var result = service.ToDictionary(typeof(Dictionary<string, MyEnum>), xmlArray) as Dictionary<string, MyEnum>;

            Assert.AreEqual(MyEnum.Other, result["key1"]);
            Assert.AreEqual(MyEnum.Default, result["key2"]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithGuessIntStringKeyStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 10).AddString("Value", "value 1"))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Int32", 20).AddString("Value", "value 2"));

            var result = service.ToDictionary(typeof(Dictionary<string, string>), xmlArray) as Dictionary<string, string>;

            Assert.AreEqual("value 1", result["10"]);
            Assert.AreEqual("value 2", result["20"]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithGuessDoubleStringKeyStringValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddNumber("Key", 1.5).AddString("Value", "value 1"))
                .Add(new XmlObject("MyItem")
                    .AddNumber("Key", 2.5).AddString("Value", "value 2"));

            var result = service.ToDictionary(typeof(Dictionary<string, string>), xmlArray) as Dictionary<string, string>;

            Assert.AreEqual("value 1", result["1.5"]);
            Assert.AreEqual("value 2", result["2.5"]);
        }

        [TestMethod]
        public void TestResolveDictionary_WithStringKeyObjectValue()
        {
            var service = this.GetService();
            var xmlArray = new XmlArray("ArrayOfString")
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key1")
                    .AddObject("User", new XmlObject("User").AddNumber("Id",1).AddString("UserName", "Marie")))
                .Add(new XmlObject("MyItem")
                    .AddString("Key", "key2")
                   .AddObject("User", new XmlObject("User").AddNumber("Id", 2).AddString("UserName", "Pat").AddNumber("Age", 20).AddString("Email", "pat@domain.com")));

            var result = service.ToDictionary(typeof(Dictionary<string, User>), xmlArray) as Dictionary<string, User>;

            Assert.AreEqual(1, result["key1"].Id);
            Assert.AreEqual("Marie", result["key1"].UserName);
            Assert.AreEqual(null, result["key1"].Age);
            Assert.AreEqual(null, result["key1"].Email);

            Assert.AreEqual(2, result["key2"].Id);
            Assert.AreEqual("Pat", result["key2"].UserName);
            Assert.AreEqual(20, result["key2"].Age);
            Assert.AreEqual("pat@domain.com", result["key2"].Email);
        }
    }
}
