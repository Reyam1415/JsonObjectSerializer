using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Json;
using System.Linq;

namespace JsonUwpTests
{
    [TestClass]
    public class JsonObjectSerializerUwpTests
    {
        [TestMethod]
        public void Should_Stringify_And_Parse_An_Array()
        {
            var items = new List<Item>
            {
                new Item
                {
                    MyInt = 10,
                    MyDouble = 5.5,
                    MyString = "my value string",
                    MyBool = true,
                    Sub = new SubItem
                    {
                        SubItemInt = 50,
                        SubItemString = "sub item value"
                    },
                    MyStrings = new List<string> { "a", "b", "c" },
                    Others = new List<OtherItem>
                    {
                        new OtherItem {OtherInt=100,OtherName="other 1" },
                        new OtherItem {OtherInt=200,OtherName="other 2" }
                    }
                },
                  new Item
                {
                    MyInt = 20,
                    MyDouble = 11.5,
                    MyString = "my value string 2",
                    MyBool = false,
                    Sub = new SubItem
                    {
                        SubItemInt = 70,
                        SubItemString = "sub item value 2"
                    },
                    MyStrings = new List<string> { "x", "y", "z" },
                    Others = new List<OtherItem>
                    {
                        new OtherItem {OtherInt=100,OtherName="other a" },
                        new OtherItem {OtherInt=200,OtherName="other b" }
                    }
                }
            };

            var json = JsonObjectSerializer.Stringify(items);
            var result = JsonObjectSerializer.Parse<List<Item>>(json);
            Assert.AreEqual(result[0].MyInt, 10);
            Assert.AreEqual(result[0].MyDouble, 5.5);
            Assert.AreEqual(result[0].MyString, "my value string");
            Assert.AreEqual(result[0].MyBool, true);
            Assert.AreEqual(result[0].Sub.SubItemInt, 50);
            Assert.AreEqual(result[0].Sub.SubItemString, "sub item value");
            Assert.AreEqual(result[0].MyStrings[0], "a");
            Assert.AreEqual(result[0].MyStrings[1], "b");
            Assert.AreEqual(result[0].MyStrings[2], "c");
            Assert.AreEqual(result[0].Others[0].OtherInt, 100);
            Assert.AreEqual(result[0].Others[0].OtherName, "other 1");
            Assert.AreEqual(result[0].Others[1].OtherInt, 200);
            Assert.AreEqual(result[0].Others[1].OtherName, "other 2");
            Assert.AreEqual(result[1].MyInt, 20);
            Assert.AreEqual(result[1].MyDouble, 11.5);
            Assert.AreEqual(result[1].MyString, "my value string 2");
            Assert.AreEqual(result[1].MyBool, false);
            Assert.AreEqual(result[1].Sub.SubItemInt, 70);
            Assert.AreEqual(result[1].Sub.SubItemString, "sub item value 2");
            Assert.AreEqual(result[1].MyStrings[0], "x");
            Assert.AreEqual(result[1].MyStrings[1], "y");
            Assert.AreEqual(result[1].MyStrings[2], "z");
            Assert.AreEqual(result[1].Others[0].OtherInt, 100);
            Assert.AreEqual(result[1].Others[0].OtherName, "other a");
            Assert.AreEqual(result[1].Others[1].OtherInt, 200);
            Assert.AreEqual(result[1].Others[1].OtherName, "other b");
        }

        [TestMethod]
        public void Should_Stringify_And_Parse_Json_Object()
        {
            var item = new Item
            {
                MyInt = 10,
                MyDouble = 5.5,
                MyString = "my value string",
                MyBool = true,
                Sub = new SubItem
                {
                    SubItemInt = 50,
                    SubItemString = "sub item value"
                },
                MyStrings = new List<string> { "a", "b", "c" },
                Others = new List<OtherItem>
                {
                    new OtherItem {OtherInt=100,OtherName="other 1" },
                    new OtherItem {OtherInt=200,OtherName="other 2" }
                }
            };

            var json = JsonObjectSerializer.Stringify(item);

            var result = JsonObjectSerializer.Parse<Item>(json);
            Assert.AreEqual(result.MyInt, 10);
            Assert.AreEqual(result.MyDouble, 5.5);
            Assert.AreEqual(result.MyString, "my value string");
            Assert.AreEqual(result.MyBool, true);
            Assert.AreEqual(result.Sub.SubItemInt, 50);
            Assert.AreEqual(result.Sub.SubItemString, "sub item value");
            Assert.AreEqual(result.MyStrings[0], "a");
            Assert.AreEqual(result.MyStrings[1], "b");
            Assert.AreEqual(result.MyStrings[2], "c");
            Assert.AreEqual(result.Others[0].OtherInt, 100);
            Assert.AreEqual(result.Others[0].OtherName, "other 1");
            Assert.AreEqual(result.Others[1].OtherInt, 200);
            Assert.AreEqual(result.Others[1].OtherName, "other 2");
        }

        [TestMethod]
        public void Showld_Serialize_Deserialize_Lists()
        {
            var items = new List<I1>();
            items.Add(new I1
            {
                subs = new List<SubI1>
                {
                    new SubI1
                    {
                        subs_subs = new List<Sub_SubI1>
                        {
                            new Sub_SubI1 {k = "sub sub 1", myint=10,mybool=false,mydouble=5.5 },
                            new Sub_SubI1 {k = "sub sub 2", myint=20,mybool=true,mydouble=11.8 }
                        }
                    }
                }
            });

            // list of I1 (1)
            //    -> list of SubI1 (2)
            //          -> sub_subI1
            //               - k="sub sub 1"
            //               - myint=10
            //               - mybool=false
            //               - mydouble=5.5
            //          -> sub_subI1
            //               - k="sub sub 2"
            //               - myint=20
            //               - mybool=true
            //               - mydouble=11.8

            //[{"subs":[{"subs_subs":[{"myint":10,"mydouble":5.5,"mybool":false,"k":"sub sub 1"},{"myint":20,"mydouble":11.8,"mybool":true,"k":"sub sub 2"}]}]}]
            var json = JsonObjectSerializer.Stringify(items);

            var result = JsonObjectSerializer.Parse<List<I1>>(json);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].k, "sub sub 1");
            Assert.AreEqual(result[0].subs[0].subs_subs[1].k, "sub sub 2");
            Assert.AreEqual(result[0].subs[0].subs_subs[0].myint, 10);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].mydouble, 5.5);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].mybool, false);
            Assert.AreEqual(result[0].subs[0].subs_subs[1].myint, 20);
            Assert.AreEqual(result[0].subs[0].subs_subs[1].mydouble, 11.8);
            Assert.AreEqual(result[0].subs[0].subs_subs[1].mybool, true);
        }

        [TestMethod]
        public void Idiot_Test()
        {
            var items = new List<int[]>
            {
                new int[] { 1,2,3,4,5}
            };

            // [[1,2,3,4,5]]
            var json = JsonObjectSerializer.Stringify(items);
            var result = JsonObjectSerializer.Parse<int[][]>(json);
            var list = JsonObjectSerializer.Parse<int[][]>(json);
            var jsonResult = JsonObjectSerializer.Stringify(result);

            Assert.AreEqual(result[0][0], 1);
            Assert.AreEqual(result[0][1], 2);
            Assert.AreEqual(result[0][2], 3);
            Assert.AreEqual(result[0][3], 4);
            Assert.AreEqual(result[0][4], 5);
            Assert.AreEqual(list[0][0], 1);
            Assert.AreEqual(list[0][1], 2);
            Assert.AreEqual(list[0][2], 3);
            Assert.AreEqual(list[0][3], 4);
            Assert.AreEqual(list[0][4], 5);
            Assert.AreEqual(jsonResult, "[[1,2,3,4,5]]");
        }

        public class I1
        {
            public List<SubI1> subs { get; set; }
            public I1()
            {
                subs = new List<SubI1>();
            }
        }

        public class SubI1
        {
            public List<Sub_SubI1> subs_subs { get; set; }
            public SubI1()
            {
                subs_subs = new List<Sub_SubI1>();
            }
        }

        public class Sub_SubI1
        {
            public int myint { get; set; }
            public double mydouble { get; set; }
            public bool mybool { get; set; }
            public string k { get; set; }
        }

        [TestMethod]
        public void Showld_Serialize_Deserialize_Array_Or_List()
        {

            var subs = new List<SubI2>
            {
                new SubI2 {subs_subs = new List<Sub_SubI2>
                {
                     new Sub_SubI2 {k = "sub sub 1", myint=10,mybool=false,mydouble=5.5 },
                     new Sub_SubI2 {k = "sub sub 2", myint=20,mybool=true,mydouble=11.8 }
                }}
            };
            var items = new List<I2>
            {
                new I2
                {
                    subs = subs.ToArray<SubI2>()
                }
            };

            var json = JsonObjectSerializer.Stringify(items);

            var result = JsonObjectSerializer.Parse<I2[]>(json);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].k, "sub sub 1");
            Assert.AreEqual(result[0].subs[0].subs_subs[1].k, "sub sub 2");
            Assert.AreEqual(result[0].subs[0].subs_subs[0].myint, 10);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].mydouble, 5.5);
            Assert.AreEqual(result[0].subs[0].subs_subs[0].mybool, false);
        }

        public class I2
        {
            public SubI2[] subs { get; set; }
        }

        public class SubI2
        {
            public List<Sub_SubI2> subs_subs { get; set; }
        }

        public class Sub_SubI2
        {
            public int myint { get; set; }
            public double mydouble { get; set; }
            public bool mybool { get; set; }
            public string k { get; set; }
        }


        [TestMethod]
        public void Showld_Serialize_Deserialize_Object_With_Enum()
        {
            var item = new ItemWithEnum
            {
                MyString = "My string value",
                MyEnumValue = MyEnum.EnumValue2
            };

            // {"MyString":"My string value","MyEnumValue":"EnumValue2"}
            var json = JsonObjectSerializer.Stringify(item);

            var result = JsonObjectSerializer.Parse<ItemWithEnum>(json);
            Assert.AreEqual(result.MyString, "My string value");
            Assert.AreEqual(result.MyEnumValue, MyEnum.EnumValue2);
        }
        
        public class ItemWithEnum
        {
            public string MyString { get; set; }
            public MyEnum MyEnumValue { get; set; }

        }
        public enum MyEnum
        {
            EnumValue1 = 0,
            EnumValue2 = 1
        }


        [TestMethod]
        public void Showld_Serialize_Deserialize_DateTime_Object()
        {
            var item = new ItemWithDateTime
            {
                Name = "Item 1",
                Date = new DateTime(1990, 01, 01)
            };

            // {"Name":"Item 1","Date":"01/01/1990 00:00:00"}
            var json = JsonObjectSerializer.Stringify(item);

            var result = JsonObjectSerializer.Parse<ItemWithDateTime>(json);
            Assert.AreEqual(result.Name, "Item 1");
            Assert.AreEqual(result.Date.Year, 1990);
            Assert.AreEqual(result.Date.Month, 01);
            Assert.AreEqual(result.Date.Day, 01);
        }

        public class ItemWithDateTime
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        [TestMethod]
        public void Showld_Serialize_Deserialize_DateTime_List_And_Array()
        {
            var dateTimeList = new List<DateTime>
            {
                new DateTime(1990,01,01),
                new DateTime(2000,02,02)
            };

            // ["01/01/1990 00:00:00","02/02/2000 00:00:00"]
            var json = JsonObjectSerializer.Stringify(dateTimeList);

            var result = JsonObjectSerializer.Parse<List<DateTime>>(json);
            var result2 = JsonObjectSerializer.Parse<DateTime[]>(json);
            Assert.AreEqual(result[0].Year, 1990);
            Assert.AreEqual(result[1].Year, 2000);
            Assert.AreEqual(result2[0].Year, 1990);
            Assert.AreEqual(result2[1].Year, 2000);
        }

        [TestMethod]
        public void Should_Stringify_And_Parse_Doubles()
        {
            var items = new List<ItemWithDouble>();
            for (int i = 0; i < 5; i++)
            {
                var item = new ItemWithDouble
                {
                    MyDouble = (i + 1) * 3.14
                };
                items.Add(item);
            }

            var json = JsonObjectSerializer.Stringify(items);
            var result = JsonObjectSerializer.Parse<List<ItemWithDouble>>(json);
            Assert.AreEqual(result[0].MyDouble, 3.14);
            Assert.AreEqual(result[1].MyDouble, 6.28);
            Assert.AreEqual(result[2].MyDouble, 9.42);
            Assert.AreEqual(result[3].MyDouble, 12.56);
        }
        public class ItemWithDouble
        {
            public double MyDouble { get; set; }
        }

        [TestMethod]
        public void Should_Stringify_An_Object()
        {
            var item = new Item
            {
                MyInt = 10,
                MyDouble = 5.5,
                MyString = "my value string",
                MyBool = true,
                Sub = new SubItem
                {
                    SubItemInt = 50,
                    SubItemString = "sub item value"
                },
                MyStrings = new List<string> { "a", "b", "c" },
                Others = new List<OtherItem>
                {
                    new OtherItem {OtherInt=100,OtherName="other 1" },
                    new OtherItem {OtherInt=200,OtherName="other 2" }
                }

            };

            var json = JsonObjectSerializer.Stringify(item);
            var x = 10;
        }

    }

    public class Item
    {
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public List<string> MyStrings { get; set; }
        public List<OtherItem> Others { get; set; }
        public SubItem Sub { get; set; }
    }

    public class OtherItem
    {
        public int OtherInt { get; set; }
        public string OtherName { get; set; }
    }

    public class SubItem
    {
        public int SubItemInt { get; set; }
        public string SubItemString { get; set; }
    }
}
