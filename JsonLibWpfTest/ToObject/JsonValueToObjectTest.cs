using JsonLib;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest
{
    [TestClass]
    public class JsonValueToObjectWpfTest
    {
        public JsonValueToObject GetService()
        {
            return new JsonValueToObject();
        }

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateString("my value");

            var result = service.ToString(typeof(string),jsonValue);

            Assert.AreEqual("my value", result);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateString(null);

            var result = service.ToString(typeof(string), jsonValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestString_WithGuid()
        {
            var service = this.GetService();

            var g = Guid.NewGuid();

            var jsonValue = JsonElementValue.CreateString(g.ToString());

            var result = service.ToString(typeof(Guid), jsonValue);

            Assert.AreEqual(g.ToString(), result.ToString());
        }

        [TestMethod]
        public void TestNumber()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateNumber(10);

            var result = service.ToNumber(typeof(int), jsonValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNumbe_WithDouble()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateNumber(10.5);

            var result = service.ToNumber(typeof(double), jsonValue);

            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateBool(true);

            var result = service.ToBool(typeof(bool), jsonValue);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestNullable()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateNullable(10);

            var result = service.ToNullable(typeof(int?), jsonValue);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestNullable_WithNull()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateNullable(null);

            var result = service.ToNullable(typeof(int?), jsonValue);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("Id", 1)
                .AddString("UserName", "Marie")
                .AddNumber("Quota",10.99)
                .AddNumber("MyInt64",100);

           var result =  service.ToObject(typeof(CompleteUser), jsonValue) as CompleteUser;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
            Assert.AreEqual(10.99, result.Quota);
            Assert.AreEqual(100, result.MyInt64);
        }

        [TestMethod]
        public void TestObjectWitInnerObject()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("Id", 1)
                .AddString("UserName", "Marie")
                .AddObject("Role",JsonElementValue.CreateObject().AddNumber("RoleId",10).AddString("Name","Admin"));

            var result = service.ToObject(typeof(UserWithInner), jsonValue) as UserWithInner;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(10, result.Role.RoleId);
            Assert.AreEqual("Admin", result.Role.Name);
        }

        [TestMethod]
        public void TestArray_WithStrings()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToArray(typeof(string[]), jsonValue) as string[];

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestArray_WithNumbers()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToArray(typeof(double[]), jsonValue) as double[];

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestList()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToList(typeof(List<string>), jsonValue) as List<string>;

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestList_WithNumbers()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddNumber(1.5)
                .AddNumber(2.5);

            var result = service.ToList(typeof(List<double>), jsonValue) as List<double>;

            Assert.AreEqual(1.5, result[0]);
            Assert.AreEqual(2.5, result[1]);
        }

        [TestMethod]
        public void TestEnumerable_WithList()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToEnumerable(typeof(List<string>), jsonValue) as List<string>;

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestEnumerable_WithArray()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddString("a")
                .AddString("b");

            var result = service.ToEnumerable(typeof(string[]), jsonValue) as string[];

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void TestEnumerable_WithArrayOfObjects()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddObject(JsonElementValue.CreateObject().AddNumber("Id",1).AddString("UserName","Marie"))
                .AddObject(JsonElementValue.CreateObject().AddNumber("Id", 2).AddString("UserName", "Pat"));

            var result = service.ToEnumerable(typeof(User[]), jsonValue) as User[];

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }

        [TestMethod]
        public void TestEnumerable_WithListOfObjects()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateArray()
                .AddObject(JsonElementValue.CreateObject().AddNumber("Id", 1).AddString("UserName", "Marie"))
                .AddObject(JsonElementValue.CreateObject().AddNumber("Id", 2).AddString("UserName", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue) as List<User>;

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }

        [TestMethod]
        public void TestArray_WithMapping()
        {
            var service = this.GetService();

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var jsonValue = JsonElementValue.CreateArray()
                .AddObject(JsonElementValue.CreateObject().AddNumber("map_id", 1).AddString("map_username", "Marie"))
                .AddObject(JsonElementValue.CreateObject().AddNumber("map_id", 2).AddString("map_username", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue, mappings) as List<User>;

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }

        [TestMethod]
        public void TestArray_WithLowerStrategy()
        {
            var service = this.GetService();

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var jsonValue = JsonElementValue.CreateArray()
                .AddObject(JsonElementValue.CreateObject().AddNumber("id", 1).AddString("username", "Marie"))
                .AddObject(JsonElementValue.CreateObject().AddNumber("id", 2).AddString("username", "Pat"));

            var result = service.ToEnumerable(typeof(List<User>), jsonValue, mappings) as List<User>;

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Marie", result[0].UserName);
            Assert.AreEqual(null, result[0].Age);
            Assert.AreEqual(null, result[0].Email);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("Pat", result[1].UserName);
            Assert.AreEqual(null, result[1].Age);
            Assert.AreEqual(null, result[1].Email);
        }

        [TestMethod]
        public void TestObject_WithMapping()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("map_id", 1)
                .AddString("map_username", "Marie");

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id","map_id")
                .SetProperty("UserName", "map_username");

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithMappingNotFound_Fail()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie");

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username");

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
            Assert.AreEqual(null, result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithLowerStrategy()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie");

            var mappings = new MappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var result = service.ToObject(typeof(User), jsonValue, mappings) as User;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual(null, result.Age);
            Assert.AreEqual(null, result.Email);
        }

        [TestMethod]
        public void TestObject_WithInnerList()
        {
            var service = this.GetService();

            var jsonValue = JsonElementValue.CreateObject()
                .AddNumber("id", 1)
                .AddString("username", "Marie")
                .AddArray("Strings", JsonElementValue.CreateArray().AddString("a").AddString("b"));

            var mappings = new MappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var result = service.ToObject(typeof(UserWithInnerAndList), jsonValue, mappings) as UserWithInnerAndList;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Marie", result.UserName);
            Assert.AreEqual("a", result.Strings[0]);
            Assert.AreEqual("b", result.Strings[1]);

        }
    }

    public class CompleteUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public double Quota { get; set; }
        public Int64 MyInt64 { get; set; }
    }

    public class UserWithInner
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }

        public UserWithInner()
        {
            this.Role = new Role();
        }
    }

    public class UserWithInnerAndList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public List<string> Strings { get; set; }

        public UserWithInnerAndList()
        {
            this.Role = new Role();
            this.Strings = new List<string>();
        }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
    }
}
