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
        public void TestIsNumberWithValue()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumber(10));
            Assert.IsTrue(service.IsNumber(10.99));
            Assert.IsFalse(service.IsNumber("10"));
            Assert.IsFalse(service.IsNumber(true));
        }

        [TestMethod]
        public void TestIsNumberWithType()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNumberType(typeof(int)));
            Assert.IsTrue(service.IsNumberType(typeof(uint)));
            Assert.IsTrue(service.IsNumberType(typeof(UInt64)));
            Assert.IsFalse(service.IsNumberType(typeof(string)));
        }

        // check nullable

        [TestMethod]
        public void TestIsNullable()
        {
            var service = this.GetService();

            Assert.IsTrue(service.IsNullable(typeof(int?)));
            Assert.IsFalse(service.IsNullable(typeof(int)));
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


        // get properties

        [TestMethod]
        public void TestGetPorperties()
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

        // get nullable target type

        [TestMethod]
        public void TestGetNullableTargetType_WithNotNullable_ReetunrsType()
        {
            var service = this.GetService();

            Assert.AreEqual(typeof(string), service.GetNullableTargetType(typeof(string)));
        }

        [TestMethod]
        public void TestGetNullableTargetType_WithNullable()
        {
            var service = this.GetService();

            Assert.AreEqual(typeof(int), service.GetNullableTargetType(typeof(int?)));
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

        //[TestMethod]
        //public void TestSetValue_WithInt32ToInt64_Fail()
        //{
        //    bool failed = false;

        //    var service = this.GetService();

        //    object value = 10;
        //    var item = new AssemblyItem2();

        //    try
        //    {
        //        service.SetValue(item, "MyInt64", value);
        //    }
        //    catch (Exception)
        //    {
        //        failed = true;
        //    }
        //    Assert.IsTrue(failed);
        //}

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


        [TestMethod]
        public void TestConvertValueStringToDateTime()
        {
            var service = this.GetService();

            var value = "12/12/1990 00:00:00";

            var item = new AssemblyItem();

            var result = service.ConvertValueToPropertyType(value, typeof(DateTime));
            Assert.AreEqual(typeof(DateTime), result.GetType());
            Assert.AreEqual(value, result.ToString());
        }


    }

   
}
