using JsonLib.Json.Mappings;
using JsonLib.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonLibTest
{

    [TestClass]
    public class TypeMappingUwpTest
    {
        // lower all
        [TestMethod]
        public void TestAllithLower()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetLowerStrategyForAllTypes();

            Assert.IsTrue(mapping.LowerStrategyForAllTypes);
        }

        [TestMethod]
        public void TestAllithLower_ToFalse()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetLowerStrategyForAllTypes(false);

            Assert.IsFalse(mapping.LowerStrategyForAllTypes);
        }

        // mapping

        [TestMethod]
        public void TestMapping()
        {

            var mapping = new JsonMappingContainer();

            // obj => to json ... lower strategy
            mapping.SetType<User>()
                .SetProperty("Id", "id")
                .SetProperty("UserName", "username")
                .SetProperty("Age", "age")
                .SetProperty("Email", "email");

            // json => property obj ... can not resolve 
            // or if property name to lower == json name

            Assert.AreEqual(1, mapping.Count);
            Assert.IsTrue(mapping.Has<User>());

            var result = mapping.Get<User>();

            Assert.AreEqual(false, result.LowerCaseStrategy);
            Assert.AreEqual("Id", result.Properties["Id"].PropertyName);
            Assert.AreEqual("id", result.Properties["Id"].JsonName);
            Assert.AreEqual("UserName", result.Properties["UserName"].PropertyName);
            Assert.AreEqual("username", result.Properties["UserName"].JsonName);
            Assert.AreEqual("Age", result.Properties["Age"].PropertyName);
            Assert.AreEqual("age", result.Properties["Age"].JsonName);
            Assert.AreEqual("Email", result.Properties["Email"].PropertyName);
            Assert.AreEqual("email", result.Properties["Email"].JsonName);
        }

        [TestMethod]
        public void TestMapping_WithLower()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy();

            Assert.AreEqual(1, mapping.Count);
            Assert.IsTrue(mapping.Has<User>());

            var result = mapping.Get<User>();

            Assert.AreEqual(true, result.LowerCaseStrategy);
        }

        [TestMethod]
        public void TestMapping_WithLowerToFalse()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy(false);

            Assert.IsFalse(mapping.Get<User>().LowerCaseStrategy);
        }

        [TestMethod]
        public void TestMapping_WithLowerMultipleRegistrations()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy();
            mapping.SetType<Product>().SetToLowerCaseStrategy();

            Assert.IsTrue(mapping.Get<User>().LowerCaseStrategy);
            Assert.IsTrue(mapping.Get<Product>().LowerCaseStrategy);
        }

        [TestMethod]
        public void TestMapping_WithMultipleAndLowerDontInterfer()
        {
            var mapping = new JsonMappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy(false);
            mapping.SetType<Product>().SetToLowerCaseStrategy();

            Assert.IsFalse(mapping.Get<User>().LowerCaseStrategy);
            Assert.IsTrue(mapping.Get<Product>().LowerCaseStrategy);
        }

        [TestMethod]
        public void TestMappings()
        {

            var mapping = new JsonMappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy();
            mapping.SetType<Product>().SetToLowerCaseStrategy();


            Assert.AreEqual(2, mapping.Count);
            Assert.IsTrue(mapping.Has<User>());
            Assert.IsTrue(mapping.Has<Product>());
        }

    }

    public class Product
    { }

}
