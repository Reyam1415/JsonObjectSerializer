using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Xml;

namespace JsonLibTest
{
    [TestClass]
    public class XmlValueToXmlServiceUwpTest
    {

        public XmlValueToXmlService GetService()
        {
            return new XmlValueToXmlService();
        }

        // gte number

        [TestMethod]
        public void TestGetNumber()
        {
            var service = this.GetService();

            double value = 10.5;

            var result = service.GetNumber(value);

            Assert.AreEqual("10.5", result);
        }

        // get bool string

        [TestMethod]
        public void TestGetBool()
        {
            var service = this.GetService();

            var result = service.GetBooValueString(true);
            var result2 = service.GetBooValueString(false);

            Assert.AreEqual("true", result);
            Assert.AreEqual("false", result2);
        }

        // get number node

        [TestMethod]
        public void TestGetNumberNode()
        {
            var service = this.GetService();

            double value = 10.5;

            var result = service.GetNumberNode("MyNode",value);

            Assert.AreEqual("<MyNode>10.5</MyNode>", result);
        }

        // get one close node

        [TestMethod]
        public void TestGetOneClosedNode()
        {
            var service = this.GetService();

            var result = service.GetOneClosedNode("MyNode");

            Assert.AreEqual("<MyNode />", result);
        }

        // get node

        [TestMethod]
        public void TestGetNode()
        {
            var service = this.GetService();

            var result = service.GetNode("MyNode","Content");

            Assert.AreEqual("<MyNode>Content</MyNode>", result);
        }

        [TestMethod]
        public void TestGetNode_WithEmptyContent_ReturnsOneCloseNode()
        {
            var service = this.GetService();

            var result = service.GetNode("MyNode", "");

            Assert.AreEqual("<MyNode />", result);
        }

        // get root

        [TestMethod]
        public void TestGetRoot_WithContent()
        {
            var service = this.GetService();

            var result = service.GetRoot("MyNode", "Content");

            Assert.AreEqual("<MyNode>Content</MyNode>", result);
        }

        [TestMethod]
        public void TestGetRoot_WithNoContent()
        {
            var service = this.GetService();

            var result = service.GetRoot("MyNode", null);

            Assert.AreEqual("<MyNode />", result);
        }

        [TestMethod]
        public void TestGetRoot_WithContentAndNil()
        {
            var service = this.GetService();

            var result = service.GetRoot("MyNode", "Content", true);

            Assert.AreEqual("<MyNode xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">Content</MyNode>", result);
        }

        [TestMethod]
        public void TestGetRoot_WithNoContentAndNil()
        {
            var service = this.GetService();

            var result = service.GetRoot("MyNode", null, true);

            Assert.AreEqual("<MyNode xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }
    }
}
