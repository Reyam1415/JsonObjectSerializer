using JsonLib;
using JsonLib.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest.Services
{
    [TestClass]
    public class AssemblyInfoServiceTest
    {
        public AssemblyInfoService GetService()
        {
            return new AssemblyInfoService();
        }

        // check is system type

        [TestMethod]
        public void TestIsSystemType()
        {
            var service = this.GetService();

            // namespace => System
            Assert.IsTrue(service.IsSystemType(typeof(int)));
            Assert.IsTrue(service.IsSystemType(typeof(double)));
            Assert.IsTrue(service.IsSystemType(typeof(string)));
            Assert.IsTrue(service.IsSystemType(typeof(bool)));
            Assert.IsTrue(service.IsSystemType(typeof(string[])));
            Assert.IsTrue(service.IsSystemType(typeof(Guid)));
            Assert.IsTrue(service.IsSystemType(typeof(DateTime)));

            Assert.IsTrue(service.IsSystemType(typeof(int?)));

            // namespace => System.Collections.Generic
            Assert.IsFalse(service.IsSystemType(typeof(List<string>)));
            Assert.IsFalse(service.IsSystemType(typeof(List<User>)));

            Assert.IsFalse(service.IsSystemType(typeof(User)));
            Assert.IsFalse(service.IsSystemType(typeof(AssemblyEnum)));
        }

        // check numeric

        [TestMethod]
        public void TestIsNumberWithType()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumberType(typeof(int)));
            Assert.IsTrue(service.IsNumberType(typeof(uint)));
            Assert.IsTrue(service.IsNumberType(typeof(UInt64)));
            Assert.IsFalse(service.IsNumberType(typeof(string)));
        }

        [TestMethod]
        public void TestIsNumberWithValue()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumber(10));
            Assert.IsTrue(service.IsNumber(10.99));
            Assert.IsFalse(service.IsNumber("10"));
            Assert.IsFalse(service.IsNumber(true));
        }

        // check is base type


        [TestMethod]
        public void TestIsBaseType()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsBaseType(typeof(Type)));

            Assert.IsFalse(service.IsBaseType(typeof(MyItem)));
            Assert.IsFalse(service.IsBaseType(typeof(MyItem[])));
            Assert.IsFalse(service.IsBaseType(typeof(List<MyItem>)));
        }


        // check nullable

        [TestMethod]
        public void TestIsNullable()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNullable(typeof(int?)));
            Assert.IsFalse(service.IsNullable(typeof(int)));
        }

        // check enum

        [TestMethod]
        public void TestIsEnum()
        {
            var service = this.GetService();

            var propertyMyEnum = typeof(AssemblyItem).GetProperty("MyEnum");
            var propertyMyString = typeof(AssemblyItem).GetProperty("MyString");

            Assert.IsTrue(service.IsEnum(propertyMyEnum.PropertyType));
            Assert.IsFalse(service.IsEnum(propertyMyString.PropertyType));
        }

        // check generic

        [TestMethod]
        public void TestIsGeneric()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsGenericType(typeof(MyItemGeneric<string>)));
            Assert.IsTrue(service.IsGenericType(typeof(List<string>)));
            Assert.IsTrue(service.IsGenericType(typeof(List<User>)));
            Assert.IsFalse(service.IsGenericType(typeof(User)));
            Assert.IsFalse(service.IsGenericType(typeof(int)));
        }

        // check is array

        [TestMethod]
        public void TestIsArray()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsArray(typeof(string[])));
            Assert.IsTrue(service.IsArray(typeof(int?[])));
            Assert.IsTrue(service.IsArray(typeof(User[])));

            Assert.IsFalse(service.IsArray(typeof(List<string>)));
            Assert.IsFalse(service.IsArray(typeof(Dictionary<string, string>)));
            Assert.IsFalse(service.IsArray(typeof(User)));
            Assert.IsFalse(service.IsArray(typeof(MyEnum)));
            Assert.IsFalse(service.IsArray(typeof(int)));
        }

        // check dictionary

        [TestMethod]
        public void TestIsDictionary()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsDictionary(typeof(Dictionary<int,int>)));
            Assert.IsTrue(service.IsDictionary(typeof(Dictionary<int, User>)));
            Assert.IsTrue(service.IsDictionary(typeof(Dictionary<Type, User>)));

            Assert.IsFalse(service.IsDictionary(typeof(List<User>)));
            Assert.IsFalse(service.IsDictionary(typeof(User[])));
            Assert.IsFalse(service.IsDictionary(typeof(MyItemGeneric<string>)));
        }

        // get sinlgle tiem

        [TestMethod]
        public void TestGetSingleItemType()
        {
            var service = this.GetService();

           Assert.AreEqual(typeof(string), service.GetSingleItemType(typeof(List<string>)));
           Assert.AreEqual(typeof(int), service.GetSingleItemType(typeof(int[])));
        }

        // get type from assembly qualified name

        [TestMethod]
        public void TestGetAssemblyQualifiedName()
        {
            var service = this.GetService();

            Assert.AreEqual("JsonLibTest.MyItem, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", service.GetAssemblyQualitiedName(typeof(MyItem)));
        }

        [TestMethod]
        public void TestGetTypefromAssemblyQualifiedName()
        {
            var service = this.GetService();

            var name = "JsonLibTest.MyItem, JsonLibTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            Assert.AreEqual(typeof(MyItem), service.GetTypeFromAssemblyQualifiedName(name));
        }

        // get dictionary infos

        [TestMethod]
        public void TestIsDictionaryKeyType()
        {
            var service = this.GetService();

            var value = new Dictionary<string, int>();

            var result = service.GetDictionaryKeyType(value.GetType());

            Assert.AreEqual(typeof(string), result);
        }

        [TestMethod]
        public void TestIsDictionaryValueType()
        {
            var service = this.GetService();

            var value = new Dictionary<string, int>();

            var result = service.GetDictionaryValueType(value.GetType());

            Assert.AreEqual(typeof(int), result);
        }

        // ConvertToStringWithInvariantCulture

        [TestMethod]
        public void TestConvertToStringWithInvariantCulture()
        {
            var service = this.GetService();

            Assert.AreEqual("10.5", service.ConvertToStringWithInvariantCulture("10.5"));
        }

        // create instance

        [TestMethod]
        public void TestCreateInstance()
        {
            var service = this.GetService();

            var result = service.CreateInstance(typeof(User));
            var result2 = service.CreateInstance(typeof(List<string>));
            var result3 = service.CreateInstance(typeof(Dictionary<int,string>));
            var result4 = service.CreateInstance(typeof(List<User>));
            // var result5 = service.CreateInstance(typeof(string[]));

            Assert.AreEqual(typeof(User), result.GetType());
            Assert.AreEqual(typeof(List<string>),result2.GetType());
            Assert.AreEqual(typeof(Dictionary<int,string>), result3.GetType());
            Assert.AreEqual(typeof(List<User>), result4.GetType());
            // Assert.AreEqual(typeof(string[]), result5.GetType()); // => use Array.CreateInstance
        }

        // create list

        [TestMethod]
        public void TestCreateList()
        {
            var service = this.GetService();

            var result = service.CreateList(typeof(User));

            Assert.AreEqual(typeof(List<User>), result.GetType());
        }

        // create array

        [TestMethod]
        public void TestCreateArray()
        {
            var service = this.GetService();

            var result = service.CreateArray(typeof(string),10);
            var result2 = service.CreateArray(typeof(User), 5);

            Assert.AreEqual(typeof(string[]), result.GetType());
            Assert.AreEqual(10, ((string[]) result).Length);

            Assert.AreEqual(typeof(User[]), result2.GetType());
            Assert.AreEqual(5, ((User[])result2).Length);
        }

        // get properties

        [TestMethod]
        public void TestGetProperties()
        {
            var service = this.GetService();

            var item = new AssemblyItem
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

            var properties = service.GetProperties(item);

            Assert.AreEqual(11, properties.Count());

            Assert.AreEqual(typeof(Guid), properties[0].PropertyType);
            Assert.AreEqual("MyGuid", properties[0].Name);

            Assert.AreEqual(typeof(int), properties[1].PropertyType);
            Assert.AreEqual("MyInt", properties[1].Name);

            Assert.AreEqual(typeof(double), properties[2].PropertyType);
            Assert.AreEqual("MyDouble", properties[2].Name);

            Assert.AreEqual(typeof(string), properties[3].PropertyType);
            Assert.AreEqual("MyString", properties[3].Name);

            Assert.AreEqual(typeof(bool), properties[4].PropertyType);
            Assert.AreEqual("MyBool", properties[4].Name);

            Assert.AreEqual(typeof(int?), properties[5].PropertyType);
            Assert.AreEqual("MyNullable", properties[5].Name);

            Assert.AreEqual(typeof(AssemblyEnum), properties[6].PropertyType);
            Assert.AreEqual("MyEnum", properties[6].Name);

            Assert.AreEqual(typeof(DateTime), properties[7].PropertyType);
            Assert.AreEqual("MyDate", properties[7].Name);

            Assert.AreEqual(typeof(AssemblyInner), properties[8].PropertyType);
            Assert.AreEqual("MyObj", properties[8].Name);

            Assert.AreEqual(typeof(List<string>), properties[9].PropertyType);
            Assert.AreEqual("MyList", properties[9].Name);

            Assert.AreEqual(typeof(string[]), properties[10].PropertyType);
            Assert.AreEqual("MyArray", properties[10].Name);
        }

        // get property

        [TestMethod]
        public void TestGetProperty()
        {
            var service = this.GetService();

            var item = new AssemblyItem
            {
                MyGuid = Guid.NewGuid(),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnum = AssemblyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new AssemblyInner { MyInnerString="my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y","z" }
            };

            var property = service.GetProperty(typeof(AssemblyItem),"MyString");

            Assert.AreEqual(typeof(string), property.PropertyType);
            Assert.AreEqual("MyString", property.Name);
        }

        // get nullable target type

        [TestMethod]
        public void TestGetNullableTargetType_WithNotNullable_ReturnsType()
        {
            var service = this.GetService();

            Assert.AreEqual(typeof(string), service.GetNullableUnderlyingType(typeof(string)));
        }

        [TestMethod]
        public void TestGetNullableTargetType_WithNullable()
        {
            var service = this.GetService();

            Assert.AreEqual(typeof(int), service.GetNullableUnderlyingType(typeof(int?)));
        }

        // set value

        [TestMethod]
        public void TestSetValue()
        {
            var service = this.GetService();

            var instance = service.CreateInstance(typeof(AssemblyItem));

            var property = service.GetProperty(typeof(AssemblyItem), "MyString");

            service.SetValue(instance, property, "my new value");

            Assert.AreEqual(typeof(AssemblyItem), instance.GetType());
            Assert.AreEqual("my new value",((AssemblyItem)instance).MyString);
        }

        [TestMethod]
        public void TestSetValue_WithExotic()
        {
            var service = this.GetService();

            var instance = service.CreateInstance(typeof(MyItem)) as MyItem;

            // Int32 => string : fail
            // service.SetValue(instance, "MyString", 10);

            // string => Int32 : fail
            // service.SetValue(instance, "MyInt", "10");

            // Int32 => double : success
            service.SetValue(instance, "MyDouble", (int)10);

            // string => enum : fail
            // service.SetValue(instance, "MyEnumValue", "Other");

            // Int32 => Enum : success
             service.SetValue(instance, "MyEnumValue", 1);

            Assert.AreEqual((double)10, instance.MyDouble);
            Assert.AreEqual(MyEnum.Other, instance.MyEnumValue);
        }

        [TestMethod]
        public void TestSetValue_WithPropertyName()
        {
            var service = this.GetService();

            var instance = service.CreateInstance(typeof(AssemblyItem));

            service.SetValue(instance, "MyString", "my new value");

            Assert.AreEqual(typeof(AssemblyItem), instance.GetType());
            Assert.AreEqual("my new value", ((AssemblyItem)instance).MyString);
        }

        [TestMethod]
        public void TestSetValue_WithNotFound_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            var instance = service.CreateInstance(typeof(AssemblyItem));

            try
            {
                service.SetValue(instance, "MyNotFound", "my new value");
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }

        // get value
        [TestMethod]
        public void TestGetValue()
        {
            var service = this.GetService();

            var item = new AssemblyItem
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

            Assert.AreEqual(item.MyGuid,service.GetValue(item, "MyGuid"));
            Assert.AreEqual(item.MyInt, service.GetValue(item, "MyInt"));
            Assert.AreEqual(item.MyDouble, service.GetValue(item, "MyDouble"));
            Assert.AreEqual(item.MyString, service.GetValue(item, "MyString"));
            Assert.AreEqual(item.MyBool, service.GetValue(item, "MyBool"));
            Assert.AreEqual(item.MyNullable, service.GetValue(item, "MyNullable"));
            Assert.AreEqual(item.MyEnum, service.GetValue(item, "MyEnum"));
            Assert.AreEqual(item.MyDate, service.GetValue(item, "MyDate"));
            Assert.AreEqual(item.MyObj, service.GetValue(item, "MyObj"));
            Assert.AreEqual(item.MyList, service.GetValue(item, "MyList"));
            Assert.AreEqual(item.MyArray, service.GetValue(item, "MyArray"));
        }

        [TestMethod]
        public void TestGetValue_WithNulls()
        {
            var service = this.GetService();

            var item = new AssemblyItem();

            Assert.AreEqual(null, service.GetValue(item, "MyString"));
            Assert.AreEqual(null, service.GetValue(item, "MyNullable"));
            Assert.AreEqual(null, service.GetValue(item, "MyObj"));
            Assert.AreEqual(null, service.GetValue(item, "MyList"));
            Assert.AreEqual(null, service.GetValue(item, "MyArray"));
        }

        [TestMethod]
        public void TestGetValue_WithNotFound_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            var item = new AssemblyItem();

            try
            {
                Assert.AreEqual(null, service.GetValue(item, "MyNotFound"));
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }


        // get converted value

        [TestMethod]
        public void TestConvertValue()
        {
            var service = this.GetService();

            var value = 10;

            Assert.AreEqual(typeof(int), value.GetType());

            var result = service.ConvertValueToPropertyType(value, typeof(double));

            Assert.AreEqual(typeof(double), result.GetType());
            Assert.AreEqual((double)10, result);
            Assert.AreEqual(10.0, result);
        }

        [TestMethod]
        public void TestConvertValueStringToDateTime()
        {
            var service = this.GetService();

            var value = "12/12/1990 00:00:00";

            var result = service.ConvertValueToPropertyType(value, typeof(DateTime));
            Assert.AreEqual(typeof(DateTime), result.GetType());
            Assert.AreEqual(value, result.ToString());
        }

        //[TestMethod]
        //public void TestConvertValueStringToGuid()
        //{
        //    // string => Guid : fail
        //    var service = this.GetService();

        //    var value = "344ac1a2-9613-44d7-b64c-8d45b4585176";

        //    var result = service.ConvertValueToPropertyType(value, typeof(Guid));

        //    Assert.AreEqual(typeof(Guid), result.GetType());
        //    Assert.AreEqual(value, result.ToString());
        //}

        [TestMethod]
        public void TestConvertValue_WithNull_ReturnsNull()
        {
            // Json => Object
            // json value ... property type => conversion required
            // string | Guid | DateTime | null => **required**
            // number int | double | enum value => required?
                //  (could convert int32 => Int64 but not double to int)
                //  (enum 0 .. 1 could set value without conversion to Default,...)
            // bool => x
            // nullable null | value => not required if target type number or json value defined as null but property type is not nullable (int for example)
            // object => x
            // array => x

            var service = this.GetService();

            int? value = null;

            var result = service.ConvertValueToPropertyType(value, typeof(double));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSetValue_WithDateTimeToString_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            var item = new AssemblyItem();

            try
            {
                service.SetValue(item, "MyString", new DateTime(1990, 12, 12));
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestSetValue_WithGuidToString_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            var item = new AssemblyItem();

            try
            {
                service.SetValue(item, "MyString", Guid.NewGuid());
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }

        [TestMethod]
        public void TestSetValue_WithNullableWithValueToInt_NotFail()
        {
            bool failed = false;

            var service = this.GetService();

            int? value = 10;
            var item = new AssemblyItem2();

            try
            {
                service.SetValue(item, "MyInt64", value);
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestSetValue_WithNullableWithNoValueToInt_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            int? value = null;
            var item = new AssemblyItem2();

            try
            {
                service.SetValue(item, "MyInt64", value);
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void TestSetValue_WithDoubleToInt_Fail()
        {
            bool failed = false;

            var service = this.GetService();

            var item = new AssemblyItem();

            try
            {
                service.SetValue(item, "MyInt", 10.99);
            }
            catch (Exception)
            {
                failed = true;
            }
            Assert.IsTrue(failed);
        }


      

    }

}
