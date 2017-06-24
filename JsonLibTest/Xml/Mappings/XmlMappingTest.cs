using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib.Mappings.Xml;

namespace JsonLibTest.Xml.Mappings
{
    [TestClass]
    public class XmlMappingTest
    {
        [TestMethod]
        public void TestSetType()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser");

            Assert.IsTrue(container.Has<User>());
        }

        [TestMethod]
        public void TestSetTypes()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser");

            container.SetType<Product>("MapProduct");

            Assert.IsTrue(container.Has<User>());
            Assert.IsTrue(container.Has<Product>());
        }

        [TestMethod]
        public void TestGetType()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser");

            var result = container.Get<User>();

            Assert.AreEqual("MapUser", result.XmlTypeName);
            Assert.IsFalse(result.HasXmlArrayName);
        }

        [TestMethod]
        public void TestSetProperties()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser")
                .SetProperty("UserName", "MapUserName");

            Assert.IsTrue(container.Get<User>().Has("UserName"));

            var result = container.Get<User>().Properties["UserName"];
            Assert.AreEqual("UserName", result.PropertyName);
            Assert.AreEqual("MapUserName", result.XmlPropertyName);
        }

        [TestMethod]
        public void TestSetTypeRetrunsExistingType()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser").SetProperty("UserName","MapuserName");

            var result = container.SetType<User>("MapUser");

            Assert.IsTrue(result.Has("UserName"));
        }

        [TestMethod]
        public void TestSetArrayName()
        {
            var container = new XmlMappingContainer();

            container.SetType<User>("MapUser");

            Assert.IsFalse(container.Get<User>().HasXmlArrayName);

            container.SetType<User>("MapUser").SetArrayName("Users");

            Assert.IsTrue(container.Get<User>().HasXmlArrayName);
            Assert.AreEqual("Users", container.Get<User>().XmlArrayName);
        }

    }
}
