using Json;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonUwpTests
{
    [TestClass]
    public class JsonMappingUwpTests
    {
        [TestMethod]
        public void Should_Add_Mapping_Manual()
        {
            JsonMapping.Default
                .Add(typeof(MappedItem), "MyInt", "my_int")
                .Add(typeof(MappedItem), "MyDouble", "my_double")
                .Add(typeof(MappedItem), "MyBool", "my_bool")
                .Add(typeof(MappedItem), "MyStrings", "my_strings")
                .Add(typeof(MappedItem), "Sub", "the_sub")
                .Add(typeof(MappedSub), "SubItemInt","the_sub_int");
            // not MyString

            var item = new MappedItem
            {
                MyInt = 10,
                MyString = "my value",
                MyBool = true,
                MyDouble = 5.5,
                MyStrings = new List<string> { "a", "b", "c" },
                Sub = new MappedSub
                {
                    SubItemInt = 100,
                    SubItemString = "sub string value"
                }
            };

            // {"MyInt":10 ... } // false
            // --> {"my_int":10,"my_double":5.5,"MyString":"my value","my_bool":true}
            var json = JsonObjectSerializer.Stringify(item);

            var result = JsonObjectSerializer.Parse<MappedItem>(json);
            Assert.AreEqual(json, "{\"my_int\":10,\"my_double\":5.5,\"my_bool\":true,\"my_strings\":[\"a\",\"b\",\"c\"],\"the_sub\":{\"the_sub_int\":100,\"SubItemString\":\"sub string value\"},\"MyString\":\"my value\"}");
            Assert.AreEqual(result.MyInt, 10);
            Assert.AreEqual(result.MyString, "my value");
            Assert.AreEqual(result.MyBool, true);
            Assert.AreEqual(result.MyDouble, 5.5);
            Assert.AreEqual(result.MyStrings[0], "a");
            Assert.AreEqual(result.MyStrings[1], "b");
            Assert.AreEqual(result.MyStrings[2], "c");
            Assert.AreEqual(result.Sub.SubItemInt, 100);
            Assert.AreEqual(result.Sub.SubItemString, "sub string value");
        }

        [TestMethod]
        public void Should_Add_Mapping_Attribute()
        {
            var item = new MappedItemWithAttribute
            {
                MyInt = 10,
                MyString = "my value",
                MyBool = true,
                MyDouble = 5.5,
                MyStrings = new List<string> { "a", "b", "c" },
                Sub = new MappedSubWithAttribute
                {
                    SubItemInt = 100,
                    SubItemString = "sub string value"
                }
            };

            JsonObjectSerializer.UseJsonMapAttributes = true;

            var json = JsonObjectSerializer.Stringify(item);

            var result = JsonObjectSerializer.Parse<MappedItemWithAttribute>(json);
            Assert.AreEqual(json, "{\"my_int\":10,\"my_double\":5.5,\"MyString\":\"my value\",\"my_bool\":true,\"my_strings\":[\"a\",\"b\",\"c\"],\"the_sub\":{\"the_sub_int\":100,\"SubItemString\":\"sub string value\"}}");
            Assert.AreEqual(result.MyInt, 10);
            Assert.AreEqual(result.MyString, "my value");
            Assert.AreEqual(result.MyBool, true);
            Assert.AreEqual(result.MyDouble, 5.5);
            Assert.AreEqual(result.MyStrings[0], "a");
            Assert.AreEqual(result.MyStrings[1], "b");
            Assert.AreEqual(result.MyStrings[2], "c");
            Assert.AreEqual(result.Sub.SubItemInt, 100);
            Assert.AreEqual(result.Sub.SubItemString, "sub string value");
        }
    }

    public class MappedItem
    {
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public List<string> MyStrings { get; set; }
        public MappedSub Sub { get; set; }
    }

    public class MappedSub
    {
        public int SubItemInt { get; set; }
        public string SubItemString { get; set; }
    }

    public class MappedItemWithAttribute
    {
        [JsonMap(JsonElementKey = "my_int")]
        public int MyInt { get; set; }

        [JsonMap(JsonElementKey = "my_double")]
        public double MyDouble { get; set; }

        public string MyString { get; set; }

        [JsonMap(JsonElementKey = "my_bool")]
        public bool MyBool { get; set; }

        [JsonMap(JsonElementKey = "my_strings")]
        public List<string> MyStrings { get; set; }

        [JsonMap(JsonElementKey = "the_sub")]
        public MappedSubWithAttribute Sub { get; set; }
    }

    public class MappedSubWithAttribute
    {
        [JsonMap(JsonElementKey = "the_sub_int")]
        public int SubItemInt { get; set; }

        public string SubItemString { get; set; }
    }
}
