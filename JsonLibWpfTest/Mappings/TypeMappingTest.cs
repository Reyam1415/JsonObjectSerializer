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

    /*
        obj => json string
        obj => serialization + mapping (type/property names)
        => objective get json type name/ json property name (for object) + property value à convert to json property value

        Object
        obj type, objet name
        properties: 
        - property name, property type, propery value
        - MyInt 10, MyString "a", MyBool true, MyNullable null
        - MyObjProperty
        - MyEnumerableProperty => json array

        property type conversion =>

        Json
        {"MyInt":10,"MyString:"a","MyBool":true,"MyNullable":null,"MyObjProperty":{...},"MyEnumerableProperty":[...]}


        Enumerable
        string[] {"a","b"} or list<string>{"a","b"} .. or list int, list bool, list whith nullable
        List<User>{ new User{ Id=1,UserName="a"}}

        Json
        ["a","b"]
        [{ "Id":1,"UserName":"a"},...]

    */
    [TestClass]
    public class TypeMappingWpfTest
    {
        [TestMethod]
        public void TestMapping()
        {

            var mapping = new MappingContainer();

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
            var mapping = new MappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy();

            Assert.AreEqual(1, mapping.Count);
            Assert.IsTrue(mapping.Has<User>());

            var result = mapping.Get<User>();

            Assert.AreEqual(true, result.LowerCaseStrategy);
        }

        [TestMethod]
        public void TestMappings()
        {

            var mapping = new MappingContainer();

            mapping.SetType<User>().SetToLowerCaseStrategy();
            mapping.SetType<Product>().SetToLowerCaseStrategy();


            Assert.AreEqual(2, mapping.Count);
            Assert.IsTrue(mapping.Has<User>());
            Assert.IsTrue(mapping.Has<Product>());
        }

    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
    }

    public class Product
    { }

}
