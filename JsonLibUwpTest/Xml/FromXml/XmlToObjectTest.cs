using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib.Xml;
using System.Collections.Generic;
using JsonLib;
using JsonLib.Mappings.Xml;

namespace JsonLibTest.Xml.FromXml
{
    [TestClass]
    public class XmlToObjectUwpTest
    {
        public XmlToObject GetService()
        {
            return new XmlToObject();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();
            var xml = "<String>my value</String>";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestStringEmpty()
        {
            var service = this.GetService();
            var xml = "<String />";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TestStringNil()
        {
            var service = this.GetService();
            var xml = "<String xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual(null, result);
        }

        // Guid

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();
            var xml = "<Guid>344ac1a2-9613-44d7-b64c-8d45b4585176</Guid>";

            var result = service.ToObject<Guid>(xml);

            Assert.AreEqual(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176"), result);
        }

        // DateTime

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();
            var xml = "<DateTime>12/12/1990 00:00:00</DateTime>";

            var result = service.ToObject<DateTime>(xml);

            Assert.AreEqual(new DateTime(1990,12,12), result);
        }

        // enum

        [TestMethod]
        public void TestEnum()
        {
            var service = this.GetService();
            var xml = "<MyEnum>Other</MyEnum>";

            var result = service.ToObject<MyEnum>(xml);

            Assert.AreEqual(MyEnum.Other, result);
        }

        [TestMethod]
        public void TestNilEnum()
        {
            var service = this.GetService();
            var xml = "<MyEnum>Other</MyEnum>";

            var result = service.ToObject<MyEnum?>(xml);

            Assert.AreEqual(MyEnum.Other, result);
        }

        [TestMethod]
        public void TestNilEnumNull()
        {
            var service = this.GetService();
            var xml = "<MyEnum xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />";

            var result = service.ToObject<MyEnum?>(xml);

            Assert.AreEqual(null, result);
        }

        // number

        [TestMethod]
        public void TestNumber_WithInt()
        {
            var service = this.GetService();
            var xml = "<Int32>10</Int32>";

            var result = service.ToObject<int>(xml);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNilNumber_WithInt()
        {
            var service = this.GetService();
            var xml = "<Int32>10</Int32>";

            var result = service.ToObject<int?>(xml);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumber_WithDouble()
        {
            var service = this.GetService();
            var xml = "<Double>10.5</Double>";

            var result = service.ToObject<double>(xml);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestNilNumber_WithDouble()
        {
            var service = this.GetService();
            var xml = "<Double>10.5</Double>";

            var result = service.ToObject<double?>(xml);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestGuessNumberString_WithInt()
        {
            var service = this.GetService();
            var xml = "<Int32>10</Int32>";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGuessNumberString_WithDouble()
        {
            var service = this.GetService();
            var xml = "<Double>10.5</Double>";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual("10.5", result);
        }

        [TestMethod]
        public void TestNilNumberNull()
        {
            var service = this.GetService();
            var xml = "<Int32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />";

            var result = service.ToObject<int?>(xml);

            Assert.AreEqual(null, result);
        }

        // bool

        [TestMethod]
        public void TestBool_WithTrue()
        {
            var service = this.GetService();
            var xml = "<Boolean>true</Boolean>";

            var result = service.ToObject<bool>(xml);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestBool_WithFalse()
        {
            var service = this.GetService();
            var xml = "<Boolean>false</Boolean>";

            var result = service.ToObject<bool>(xml);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestGuessBoolString_WithTrue()
        {
            var service = this.GetService();
            var xml = "<Boolean>true</Boolean>";

            var result = service.ToObject<string>(xml);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestNilBool_WithTrue()
        {
            var service = this.GetService();
            var xml = "<Boolean>true</Boolean>";

            var result = service.ToObject<bool?>(xml);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestNilBoolNull()
        {
            var service = this.GetService();
            var xml = "<Boolean xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />";

            var result = service.ToObject<bool?>(xml);

            Assert.AreEqual(null, result);
        }

        // object

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var xml = "<MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value</MyString><MyBool>true</MyBool><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value</MyInnerString></MyObj><MyList><String>a</String><String>b</String></MyList><MyArray><String>y</String><String>z</String></MyArray></MyItem>";

            var result = service.ToObject<MyItem>(xml);

            Assert.IsNotNull(result);
            Assert.AreEqual(new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"), result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(MyEnum.Other, result.MyEnumValue);
            Assert.AreEqual(new DateTime(1990, 12, 12), result.MyDate);
            Assert.AreEqual("my inner value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }

        [TestMethod]
        public void TestObjectWithNullables()
        {
            var service = this.GetService();

            var xml = "<MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value</MyString><MyBool>true</MyBool><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value</MyInnerString></MyObj><MyList><String>a</String><String>b</String></MyList><MyArray><String>y</String><String>z</String></MyArray></MyItem>";

            var result = service.ToObject<MyItemNullables>(xml);

            Assert.IsNotNull(result);
            Assert.AreEqual(new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"), result.MyGuid);
            Assert.AreEqual(1, result.MyInt);
            Assert.AreEqual(1.5, result.MyDouble);
            Assert.AreEqual(true, result.MyBool);
            Assert.AreEqual(MyEnum.Other, result.MyEnumValue);
            Assert.AreEqual(new DateTime(1990, 12, 12), result.MyDate);
            Assert.AreEqual("my inner value", result.MyObj.MyInnerString);
            Assert.AreEqual(2, result.MyList.Count);
            Assert.AreEqual("a", result.MyList[0]);
            Assert.AreEqual("b", result.MyList[1]);
            Assert.AreEqual(2, result.MyArray.Length);
            Assert.AreEqual("y", result.MyArray[0]);
            Assert.AreEqual("z", result.MyArray[1]);
        }

        [TestMethod]
        public void TestObjectNullablesNull()
        {
            var service = this.GetService();

            var xml = "<MyItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyGuid xsi:nil=\"true\" /><MyInt xsi:nil=\"true\" /><MyDouble xsi:nil=\"true\" /><MyString xsi:nil=\"true\" /><MyBool xsi:nil=\"true\" /><MyEnumValue xsi:nil=\"true\" /><MyDate xsi:nil=\"true\" /><MyObj xsi:nil=\"true\" /><MyList xsi:nil=\"true\" /><MyArray xsi:nil=\"true\" /></MyItem>";

            var result = service.ToObject<MyItemNullables>(xml);

            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.MyGuid);
            Assert.AreEqual(null, result.MyInt);
            Assert.AreEqual(null, result.MyDouble);
            Assert.AreEqual(null, result.MyBool);
            Assert.AreEqual(null, result.MyEnumValue);
            Assert.AreEqual(null, result.MyDate);
            Assert.AreEqual(null, result.MyObj);
            Assert.AreEqual(null, result.MyList);
            Assert.AreEqual(null, result.MyArray);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            //var user = new UserWithMapping
            //{
            //    Id = 1,
            //    UserName = "Marie",
            //    Age = 20,
            //    Email = "marie@domain.com",
            //    EnumValue = MyEnum.Other,
            //    Role = new UserRole { RoleName = "Admin" },
            //    Posts = new List<Post> { new Post { Title = "Post 1" } },
            //    Tips = new Tip[] { new Tip { TipName = "Tip 1" } }
            //};

            var mappings = new XmlMappingContainer();
            mappings.SetType<UserWithMapping>("MyUserEntity")
                .SetProperty("Id", "MyId")
                .SetProperty("UserName", "MyUserName")
                .SetProperty("Email", "MyEmail")
                .SetProperty("EnumValue", "MyEnumMap")
                .SetProperty("Role", "MyRole")
                .SetProperty("Posts", "MyPosts")
                .SetProperty("Tips","MyTips");

            mappings.SetType<UserRole>("MyRoleEntity")
               .SetProperty("RoleName", "MyRoleName");

            mappings.SetType<Post>("MyPostEntity")
              .SetProperty("Title", "MyTitle");

            mappings.SetType<Tip>("MyTipEntity")
              .SetProperty("TipName", "MyTipName");

            // var x = JsonObjectSerializer.ToXml(user, mappings);

            var xml = "<?xml version=\"1.0\"?>\r<MyUserEntity><MyId>1</MyId><MyUserName>Marie</MyUserName><Age>20</Age><MyEmail>marie@domain.com</MyEmail><MyEnumMap>Other</MyEnumMap><MyRole><MyRoleName>Admin</MyRoleName></MyRole><MyPosts><MyPostEntity><MyTitle>Post 1</MyTitle></MyPostEntity></MyPosts><MyTips><MyTipEntity><MyTipName>Tip 1</MyTipName></MyTipEntity></MyTips></MyUserEntity>";

            var result = service.ToObject<UserWithMapping>(xml, mappings);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(20, result.Age);
            Assert.AreEqual("marie@domain.com", result.Email);
            Assert.AreEqual(MyEnum.Other, result.EnumValue);
            Assert.AreEqual("Admin", result.Role.RoleName);
            Assert.AreEqual("Post 1", result.Posts[0].Title);
            Assert.AreEqual("Tip 1", result.Tips[0].TipName);
        }

        [TestMethod]
        public void TestObject_WithDictionary()
        {
            var service = this.GetService();

            //var value = new MyItemWithDictionaryIntString
            //{
            //    Items = new Dictionary<int, string>
            //    {
            //        { 10, "value 1" },
            //        { 20, "value 2" }
            //    }
            //};

            var xml = "<?xml version=\"1.0\"?>\r<MyItemWithDictionaryIntString><Items><String><Key>10</Key><Value>value 1</Value></String><String><Key>20</Key><Value>value 2</Value></String></Items></MyItemWithDictionaryIntString>";

            var result = service.ToObject<MyItemWithDictionaryIntString>(xml);

            Assert.IsNotNull(result.Items);

            var dictionary = result.Items as Dictionary<int, string>;

            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual("value 1",dictionary[10]);
            Assert.AreEqual("value 2", dictionary[20]);
        }

        [TestMethod]
        public void TestObject_WithDictionaryGuessIsObject()
        {
            var service = this.GetService();

            //var value = new MyItemWithDictionaryIntString
            //{
            //    Items = new Dictionary<int, string>
            //    {
            //        { 10, "value 1" }
            //    }
            //};

            var xml = "<?xml version=\"1.0\"?>\r<MyItemWithDictionaryIntString><Items><String><Key>10</Key><Value>value 1</Value></String></Items></MyItemWithDictionaryIntString>";

            var result = service.ToObject<MyItemWithDictionaryIntString>(xml);

            Assert.IsNotNull(result.Items);

            var dictionary = result.Items as Dictionary<int, string>;

            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("value 1", dictionary[10]);
        }

        [TestMethod]
        public void TestObject_WithDictionaryAndMapping()
        {
            var service = this.GetService();

            //var value = new MyItemWithDictionaryIntUser
            //{
            //    Users = new Dictionary<int, User>
            //    {
            //            { 1, new User { Id = 1, UserName = "Marie" } },
            //            { 2, new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" } }
            //    }
            //};

            var mappings = new XmlMappingContainer();
            mappings.SetType<MyItemWithDictionaryIntUser>("MyItemWithDictionaryIntUserEntity")
                .SetProperty("Users", "MyUsers");

            mappings.SetType<User>("MyUserEntity")
               .SetProperty("Id", "MyId")
               .SetProperty("UserName", "MyUserName")
               .SetProperty("Email", "MyEmail");

            var xml = "<?xml version=\"1.0\"?>\r<MyItemWithDictionaryIntUserEntity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyUsers><User><Key>1</Key><Value><MyId>1</MyId><MyUserName>Marie</MyUserName><Age xsi:nil=\"true\" /><MyEmail xsi:nil=\"true\" /></Value></User><User><Key>2</Key><Value><MyId>2</MyId><MyUserName>Pat</MyUserName><Age>20</Age><MyEmail>pat@domain.com</MyEmail></Value></User></MyUsers></MyItemWithDictionaryIntUserEntity>";

            var result = service.ToObject<MyItemWithDictionaryIntUser>(xml, mappings);

            Assert.IsNotNull(result.Users);

            var dictionary = result.Users as Dictionary<int, User>;

            Assert.AreEqual(2, dictionary.Count);

            Assert.AreEqual(1, dictionary[1].Id);
            Assert.AreEqual("Marie", dictionary[1].UserName);
            Assert.AreEqual(null, dictionary[1].Age);
            Assert.AreEqual(null, dictionary[1].Email);

            Assert.AreEqual(2, dictionary[2].Id);
            Assert.AreEqual("Pat", dictionary[2].UserName);
            Assert.AreEqual(20, dictionary[2].Age);
            Assert.AreEqual("pat@domain.com", dictionary[2].Email);
        }


        [TestMethod]
        public void TestObject_WithDictionaryAndMappingGuessOneObject()
        {
            var service = this.GetService();
            var mappings = new XmlMappingContainer();
            mappings.SetType<MyItemWithDictionaryIntUser>("MyItemWithDictionaryIntUserEntity")
                .SetProperty("Users", "MyUsers");

            mappings.SetType<User>("MyUserEntity")
               .SetProperty("Id", "MyId")
               .SetProperty("UserName", "MyUserName")
               .SetProperty("Email", "MyEmail");

            var xml = "<?xml version=\"1.0\"?>\r<MyItemWithDictionaryIntUserEntity xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyUsers><User><Key>1</Key><Value><MyId>1</MyId><MyUserName>Marie</MyUserName><Age xsi:nil=\"true\" /><MyEmail xsi:nil=\"true\" /></Value></User></MyUsers></MyItemWithDictionaryIntUserEntity>";

            var result = service.ToObject<MyItemWithDictionaryIntUser>(xml, mappings);

            Assert.IsNotNull(result.Users);

            var dictionary = result.Users as Dictionary<int, User>;

            Assert.AreEqual(1, dictionary.Count);

            Assert.AreEqual(1, dictionary[1].Id);
            Assert.AreEqual("Marie", dictionary[1].UserName);
            Assert.AreEqual(null, dictionary[1].Age);
            Assert.AreEqual(null, dictionary[1].Email);
        }

        // array

        [TestMethod]
        public void TestArrayOfInt()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32><Int32>2</Int32></ArrayOfInt32>";

            var result = service.ToObject<int[]>(xml);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestArrayOfInt_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32><Int32>2</Int32></ArrayOfInt32>";

            var result = service.ToObject<string[]>(xml);

            Assert.AreEqual("1", result[0]);
            Assert.AreEqual("2", result[1]);
        }

        [TestMethod]
        public void TestArrayOfInt_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32></ArrayOfInt32>";

            var result = service.ToObject<int[]>(xml);

            Assert.AreEqual(1, result[0]);
        }

        [TestMethod]
        public void TestArrayOfDoubles()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double><Double>2.5</Double></ArrayOfDouble>";

            var result = service.ToObject<double[]>(xml);

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestArrayOfDoubles_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double><Double>2.5</Double></ArrayOfDouble>";

            var result = service.ToObject<string[]>(xml);

            Assert.AreEqual("1.5", result[0]);
            Assert.AreEqual("2.5", result[1]);
        }

        [TestMethod]
        public void TestArrayOfDoubles_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double></ArrayOfDouble>";

            var result = service.ToObject<double[]>(xml);

            Assert.AreEqual(1.5, result[0]);
        }

        [TestMethod]
        public void TestArrayOfString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfString><String>my value 1</String><String>my value 2</String></ArrayOfString>";

            var result = service.ToObject<string[]>(xml);

            Assert.AreEqual("my value 1", result[0]);
            Assert.AreEqual("my value 2", result[1]);
        }

        [TestMethod]
        public void TestArrayOfBool()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<bool[]>(xml);

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestArrayOfBool_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<string[]>(xml);

            Assert.AreEqual("true", result[0]);
            Assert.AreEqual("false", result[1]);
        }

        [TestMethod]
        public void TestArrayOfBool_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<bool[]>(xml);

            Assert.AreEqual(true, result[0]);
        }


        [TestMethod]
        public void TestArrayOfObjects()
        {
            var service = this.GetService();

            var value = new List<User>
               {
                    new User { Id = 1, UserName = "Marie" } ,
                    new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" }
               };

            // var x = JsonObjectSerializer.ToXml(value);

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><User><Id>1</Id><UserName>Marie</UserName><Age xsi:nil=\"true\" /><Email xsi:nil=\"true\" /></User><User><Id>2</Id><UserName>Pat</UserName><Age>20</Age><Email>pat@domain.com</Email></User></ArrayOfUser>";

            var results = service.ToObject<User[]>(xml);

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


        [TestMethod]
        public void TestArrayOfObjects_WithMappings()
        {
            var service = this.GetService();

            //var value = new List<User>
            //   {
            //        new User { Id = 1, UserName = "Marie" } ,
            //        new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" }
            //   };

           // var x = JsonObjectSerializer.ToXml(value, mappings);

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MyUserEntity")
                .SetProperty("Id", "MyId")
                .SetProperty("UserName", "MyUserName")
                .SetProperty("Email", "MyEmail");


            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyUserEntity><MyId>1</MyId><MyUserName>Marie</MyUserName><Age xsi:nil=\"true\" /><MyEmail xsi:nil=\"true\" /></MyUserEntity><MyUserEntity><MyId>2</MyId><MyUserName>Pat</MyUserName><Age>20</Age><MyEmail>pat@domain.com</MyEmail></MyUserEntity></ArrayOfUser>";

            var results = service.ToObject<User[]>(xml, mappings);

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

        [TestMethod]
        public void TestArrayOfObjects_WithMappingsGuessObject()
        {
            var service = this.GetService();

            var mappings = new XmlMappingContainer();
            mappings.SetType<User>("MyUserEntity")
                .SetProperty("Id", "MyId")
                .SetProperty("UserName", "MyUserName")
                .SetProperty("Email", "MyEmail");


            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyUserEntity><MyId>1</MyId><MyUserName>Marie</MyUserName><Age xsi:nil=\"true\" /><MyEmail xsi:nil=\"true\" /></MyUserEntity></ArrayOfUser>";

            var results = service.ToObject<User[]>(xml, mappings);

            var result = results[0];

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestArray_WithDictionary()
        {
            var service = this.GetService();

            //var value = new List<MyItemWithDictionaryIntUser>
            //{
            //   new MyItemWithDictionaryIntUser
            //   {
            //        Users = new Dictionary<int, User>
            //        {
            //                { 1, new User { Id = 1, UserName = "Marie" } },
            //        }
            //   },
            //   new MyItemWithDictionaryIntUser
            //   {
            //        Users = new Dictionary<int, User>
            //        {
            //                { 2, new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" } }
            //        }
            //   }
            //};

            var xml = "<?xml version=\"1.0\"?>\r<ArrayOfMyItemWithDictionaryIntUser xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyItemWithDictionaryIntUser><Users><User><Key>1</Key><Value><Id>1</Id><UserName>Marie</UserName><Age xsi:nil=\"true\" /><Email xsi:nil=\"true\" /></Value></User></Users></MyItemWithDictionaryIntUser><MyItemWithDictionaryIntUser><Users><User><Key>2</Key><Value><Id>2</Id><UserName>Pat</UserName><Age>20</Age><Email>pat@domain.com</Email></Value></User></Users></MyItemWithDictionaryIntUser></ArrayOfMyItemWithDictionaryIntUser>";

            var result = service.ToObject<MyItemWithDictionaryIntUser[]>(xml);

            var dictionary = result[0].Users as Dictionary<int, User>;

            Assert.AreEqual(1, dictionary.Count);

            Assert.AreEqual(1, dictionary[1].Id);
            Assert.AreEqual("Marie", dictionary[1].UserName);
            Assert.AreEqual(null, dictionary[1].Age);
            Assert.AreEqual(null, dictionary[1].Email);

            var dictionary2 = result[1].Users as Dictionary<int, User>;

            Assert.AreEqual(1, dictionary2.Count);

            Assert.AreEqual(2, dictionary2[2].Id);
            Assert.AreEqual("Pat", dictionary2[2].UserName);
            Assert.AreEqual(20, dictionary2[2].Age);
            Assert.AreEqual("pat@domain.com", dictionary2[2].Email);

        }

        // list

        [TestMethod]
        public void TestListOfInt()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32><Int32>2</Int32></ArrayOfInt32>";

            var result = service.ToObject<List<int>>(xml);

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestListOfInt_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32><Int32>2</Int32></ArrayOfInt32>";

            var result = service.ToObject<List<string>>(xml);

            Assert.AreEqual("1", result[0]);
            Assert.AreEqual("2", result[1]);
        }

        [TestMethod]
        public void TestListOfInt_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfInt32><Int32>1</Int32></ArrayOfInt32>";

            var result = service.ToObject<List<int>>(xml);

            Assert.AreEqual(1, result[0]);
        }

        [TestMethod]
        public void TestListOfDoubles()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double><Double>2.5</Double></ArrayOfDouble>";

            var result = service.ToObject<List<double>>(xml);

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestListOfDoubles_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double><Double>2.5</Double></ArrayOfDouble>";

            var result = service.ToObject<List<string>>(xml);

            Assert.AreEqual("1.5", result[0]);
            Assert.AreEqual("2.5", result[1]);
        }

        [TestMethod]
        public void TestListOfDoubles_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfDouble><Double>1.5</Double></ArrayOfDouble>";

            var result = service.ToObject<List<double>>(xml);

            Assert.AreEqual(1.5, result[0]);
        }

        [TestMethod]
        public void TestListOfString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfString><String>my value 1</String><String>my value 2</String></ArrayOfString>";

            var result = service.ToObject<List<string>>(xml);

            Assert.AreEqual("my value 1", result[0]);
            Assert.AreEqual("my value 2", result[1]);
        }

        [TestMethod]
        public void TestListOfBool()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<List<bool>>(xml);

            Assert.AreEqual(true, result[0]);
            Assert.AreEqual(false, result[1]);
        }

        [TestMethod]
        public void TestListOfBool_GuessString()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<List<string>>(xml);

            Assert.AreEqual("true", result[0]);
            Assert.AreEqual("false", result[1]);
        }

        [TestMethod]
        public void TestListOfBool_GuessObject()
        {
            var service = this.GetService();
            var xml = "<ArrayOfBoolean><Boolean>true</Boolean></ArrayOfBoolean>";

            var result = service.ToObject<List<bool>>(xml);

            Assert.AreEqual(true, result[0]);
        }
    }
}
