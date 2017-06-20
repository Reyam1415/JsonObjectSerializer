using JsonLib;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class ObjectToJsonWpfTest
    {
        public ObjectToJson GetService()
        {
            return new ObjectToJson();
        }

        [TestMethod]
        public void TestGetString()
        {
            var service = this.GetService();

            var result = service.ToJson("my value");

            Assert.AreEqual("\"my value\"", result);
        }

        [TestMethod]
        public void TestGetNumber()
        {
            var service = this.GetService();

            var result = service.ToJson(10);

            Assert.AreEqual("10", result);
        }

        [TestMethod]
        public void TestGetBool()
        {
            var service = this.GetService();

            var result = service.ToJson(true);

            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void TestGetNull()
        {
            var service = this.GetService();

            var result = service.ToJson(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void TestGetObject()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 1,
                UserName = "Marie",
                Age = 20,
                Email = "marie@domain.com"
            };

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Age\":20,\"Email\":\"marie@domain.com\"}", result);
        }

        [TestMethod]
        public void TestGetObject_WithNulls()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 1,
                UserName = "Marie"
            };

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}", result);
        }

        [TestMethod]
        public void TestGetObject_WithMappings()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 1,
                UserName = "Marie",
                Age = 20,
                Email = "marie@domain.com"
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"map_id\":1,\"map_username\":\"Marie\",\"Age\":20,\"map_email\":\"marie@domain.com\"}", result);
        }


        [TestMethod]
        public void TestGetObject_WithLowerStrategy()
        {
            var service = this.GetService();

            var user = new User
            {
                Id = 1,
                UserName = "Marie",
                Age = 20,
                Email = "marie@domain.com"
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"id\":1,\"username\":\"Marie\",\"age\":20,\"email\":\"marie@domain.com\"}", result);
        }


        [TestMethod]
        public void TestGetObject_WithInnerObject()
        {
            var service = this.GetService();

            var user = new UserWithInner
            {
                Id = 1,
                UserName = "Marie"
            };
            user.Role.RoleId = 10;
            user.Role.Name = "Admin";
            user.Role.Status = 100;

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":\"Admin\",\"Status\":100}}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerObjectAndNull()
        {
            var service = this.GetService();

            var user = new UserWithInner
            {
                Id = 1,
                UserName = "Marie"
            };
            user.Role.RoleId = 10;

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":null,\"Status\":null}}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerObjectAndList()
        {
            var service = this.GetService();

            var user = new UserWithInnerAndList
            {
                Id = 1,
                UserName = "Marie"
            };
            user.Role.RoleId = 10;
            user.Role.Name = "Admin";
            user.Role.Status = 100;
            user.Strings = new List<string> { "a", "b" };

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":1,\"UserName\":\"Marie\",\"Role\":{\"RoleId\":10,\"Name\":\"Admin\",\"Status\":100},\"Strings\":[\"a\",\"b\"]}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerAndMappings()
        {
            var service = this.GetService();

            var user = new UserWithInnerAndList
            {
                Id = 1,
                UserName = "Marie"
            };
            user.Role.RoleId = 10;
            user.Role.Name = "Admin";
            user.Role.Status = 100;

            user.Strings = new List<string> { "a", "b" };

            var mappings = new MappingContainer();

            mappings.SetType<UserWithInnerAndList>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Role","map__role")
                .SetProperty("Strings", "map__strings");

            mappings.SetType<Role>()
              .SetProperty("RoleId", "map__roleid");

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"map_id\":1,\"map_username\":\"Marie\",\"map__role\":{\"map__roleid\":10,\"Name\":\"Admin\",\"Status\":100},\"map__strings\":[\"a\",\"b\"]}", result);
        }

        [TestMethod]
        public void TestGetObject_WithInnerAndLowerStrategy()
        {
            var service = this.GetService();

            var user = new UserWithInnerAndList
            {
                Id = 1,
                UserName = "Marie"
            };
            user.Role.RoleId = 10;
            user.Role.Name = "Admin";
            user.Role.Status = 100;
            user.Strings = new List<string> { "a", "b" };

            var mappings = new MappingContainer();
            mappings.SetLowerStrategyForAllTypes();

            var result = service.ToJson(user, mappings);

            Assert.AreEqual("{\"id\":1,\"username\":\"Marie\",\"role\":{\"roleid\":10,\"name\":\"Admin\",\"status\":100},\"strings\":[\"a\",\"b\"]}", result);
        }

        [TestMethod]
        public void TestGetArray()
        {
            var service = this.GetService();

            var array = new string[] { "a", "b" };

            var result = service.ToJson(array);

            Assert.AreEqual("[\"a\",\"b\"]", result);
        }

        [TestMethod]
        public void TestGetList()
        {
            var service = this.GetService();

            var array = new List<string> { "a", "b" };

            var result = service.ToJson(array);

            Assert.AreEqual("[\"a\",\"b\"]", result);
        }

        [TestMethod]
        public void TestGetListOfObjects()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var result = service.ToJson(array);

            Assert.AreEqual("[{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},{\"Id\":2,\"UserName\":\"Pat\",\"Age\":20,\"Email\":\"pat@domain.com\"}]", result);
        }

        [TestMethod]
        public void TestGetListOfObjects_WithMappings()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>()
                .SetProperty("Id", "map_id")
                .SetProperty("UserName", "map_username")
                .SetProperty("Email", "map_email");

            var result = service.ToJson(array, mappings);

            Assert.AreEqual("[{\"map_id\":1,\"map_username\":\"Marie\",\"Age\":null,\"map_email\":null},{\"map_id\":2,\"map_username\":\"Pat\",\"Age\":20,\"map_email\":\"pat@domain.com\"}]", result);
        }

        [TestMethod]
        public void TestGetListOfObjects_WithLowerStrategy()
        {
            var service = this.GetService();

            var array = new List<User>
            {
                new User{ Id=1, UserName="Marie"},
                new User{ Id=2, UserName="Pat", Age=20, Email="pat@domain.com"}
            };

            var mappings = new MappingContainer();
            mappings.SetType<User>().SetToLowerCaseStrategy();

            var result = service.ToJson(array, mappings);

            Assert.AreEqual("[{\"id\":1,\"username\":\"Marie\",\"age\":null,\"email\":null},{\"id\":2,\"username\":\"Pat\",\"age\":20,\"email\":\"pat@domain.com\"}]", result);
        }

        [TestMethod]
        public void TestObject_WithDateTime()
        {
            var service = this.GetService();

            var user = new UserWithDate
            {
                UserName = "Marie",
                Birth = new DateTime(1990, 12, 12)
            };

            var result = service.ToJson(user);

            Assert.AreEqual("{\"UserName\":\"Marie\",\"Birth\":\"12/12/1990 00:00:00\"}", result);
        }

        [TestMethod]
        public void TestObject_WithGuid()
        {
            var service = this.GetService();

            var user = new UserWithGuid
            {
                Id= Guid.NewGuid(),
                UserName = "Marie"
            };

            var result = service.ToJson(user);

            Assert.AreEqual("{\"Id\":\"" + user.Id.ToString() + "\",\"UserName\":\"Marie\"}", result);
        }
    }


    public class UserWithDate
    {
        public string UserName { get; set; }
        public DateTime Birth { get; set; }
    }

    public class UserWithGuid
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }

}
