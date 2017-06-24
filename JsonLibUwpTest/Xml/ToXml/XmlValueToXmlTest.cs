using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLib;
using JsonLib.Xml;
using System.Collections.Generic;

namespace JsonLibTest
{
    [TestClass]
    public class XmlValueToXmlUwpTest
    {
        public XmlValueToXml GetService()
        {
            return new XmlValueToXml();
        }

        // string

        [TestMethod]
        public void TestString()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("MyString", "my value");

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyString>my value</MyString>", result);
        }

        [TestMethod]
        public void TestString_WithEmpty()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("MyString", "");

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyString />", result);
        }

        [TestMethod]
        public void TestString_WithNull()
        {
            var service = this.GetService();

            var xmlValue = new XmlString("MyString", null);

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        // number

        [TestMethod]
        public void TestNumber()
        {
            var service = this.GetService();

            int MyInt = 10;
            Int16 MyInt16 = 10;
            Int32 MyInt32 = 10;
            Int64 MyInt64 = 10;

            Byte MyByte = 201;
            SByte MySByte = -102;

            UInt16 MyUInt16 = 10;
            UInt32 MyUInt32 = 10;
            UInt64 MyUInt64 = 10;

            Decimal MyDec = 10.5M;
            Double MyDouble = 10.5;
            Single MySingle = 10.5F;

            var xmlInt = new XmlNumber("Int32", MyInt);
            var xmlInt16 = new XmlNumber("Int16", MyInt16);
            var xmlInt32 = new XmlNumber("Int32", MyInt32);
            var xmlInt64 = new XmlNumber("Int64", MyInt64);

            var xmlByte = new XmlNumber("Byte", MyByte);
            var xmlSByte = new XmlNumber("SByte", MySByte);

            var xmlUInt16 = new XmlNumber("UInt16", MyUInt16);
            var xmlUInt32 = new XmlNumber("UInt32", MyUInt32);
            var xmlUInt64 = new XmlNumber("UInt64", MyUInt64);

            var xmlMyDec = new XmlNumber("Decimal", MyDec);
            var xmlMyDouble = new XmlNumber("Double", MyDouble);
            var xmlMySingle = new XmlNumber("Single", MySingle);

            var resultInt = service.GetRoot(xmlInt);
            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<Int32>10</Int32>", resultInt);
            Assert.AreEqual("<Int16>10</Int16>", resultInt16);
            Assert.AreEqual("<Int32>10</Int32>", resultInt32);
            Assert.AreEqual("<Int64>10</Int64>", resultInt64);

            Assert.AreEqual("<Byte>201</Byte>", resultByte);
            Assert.AreEqual("<SByte>-102</SByte>", resultSByte);

            Assert.AreEqual("<UInt16>10</UInt16>", resultUInt16);
            Assert.AreEqual("<UInt32>10</UInt32>", resultUInt32);
            Assert.AreEqual("<UInt64>10</UInt64>", resultUInt64);

            Assert.AreEqual("<Decimal>10.5</Decimal>", resultMyDec);
            Assert.AreEqual("<Double>10.5</Double>", resultMyDouble);
            Assert.AreEqual("<Single>10.5</Single>", resultMySingle);
        }

        [TestMethod]
        public void TestNumberNullables()
        {
            var service = this.GetService();

            int? MyInt = 10;
            Int16? MyInt16 = 10;
            Int32? MyInt32 = 10;
            Int64? MyInt64 = 10;

            Byte? MyByte = 201;
            SByte? MySByte = -102;

            UInt16? MyUInt16 = 10;
            UInt32? MyUInt32 = 10;
            UInt64? MyUInt64 = 10;

            Decimal? MyDec = 10.5M;
            Double? MyDouble = 10.5;
            Single? MySingle = 10.5F;

            var xmlInt = XmlValue.CreateNullable<int?>("Int32", MyInt);
            var xmlInt16 = XmlValue.CreateNullable<Int16?>("Int16", MyInt16);
            var xmlInt32 = XmlValue.CreateNullable<Int32?>("Int32", MyInt32);
            var xmlInt64 = XmlValue.CreateNullable<Int64?>("Int64", MyInt64);

            var xmlByte = XmlValue.CreateNullable<Byte?>("Byte", MyByte);
            var xmlSByte = XmlValue.CreateNullable<SByte?>("SByte", MySByte);

            var xmlUInt16 = XmlValue.CreateNullable<UInt16?>("UInt16", MyUInt16);
            var xmlUInt32 = XmlValue.CreateNullable<UInt32?>("UInt32", MyUInt32);
            var xmlUInt64 = XmlValue.CreateNullable<UInt64?>("UInt64", MyUInt64);

            var xmlMyDec = XmlValue.CreateNullable<Decimal?>("Decimal", MyDec);
            var xmlMyDouble = XmlValue.CreateNullable<Double?>("Double", MyDouble);
            var xmlMySingle = XmlValue.CreateNullable<Single?>("Single", MySingle);

            var resultInt = service.GetRoot(xmlInt);
            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<Int32>10</Int32>", resultInt);
            Assert.AreEqual("<Int16>10</Int16>", resultInt16);
            Assert.AreEqual("<Int32>10</Int32>", resultInt32);
            Assert.AreEqual("<Int64>10</Int64>", resultInt64);

            Assert.AreEqual("<Byte>201</Byte>", resultByte);
            Assert.AreEqual("<SByte>-102</SByte>", resultSByte);

            Assert.AreEqual("<UInt16>10</UInt16>", resultUInt16);
            Assert.AreEqual("<UInt32>10</UInt32>", resultUInt32);
            Assert.AreEqual("<UInt64>10</UInt64>", resultUInt64);

            Assert.AreEqual("<Decimal>10.5</Decimal>", resultMyDec);
            Assert.AreEqual("<Double>10.5</Double>", resultMyDouble);
            Assert.AreEqual("<Single>10.5</Single>", resultMySingle);
        }

        [TestMethod]
        public void TestNumberNullablesNull()
        {
            var service = this.GetService();

            int? MyInt = null;
            Int16? MyInt16 = null;
            Int32? MyInt32 = null;
            Int64? MyInt64 = null;

            Byte? MyByte = null;
            SByte? MySByte = null;

            UInt16? MyUInt16 = null;
            UInt32? MyUInt32 = null;
            UInt64? MyUInt64 = null;

            Decimal? MyDec = null;
            Double? MyDouble = null;
            Single? MySingle = null;

            var xmlInt = XmlValue.CreateNullable<int?>("Int32", MyInt);
            var xmlInt16 = XmlValue.CreateNullable<Int16?>("Int16", MyInt16);
            var xmlInt32 = XmlValue.CreateNullable<Int32?>("Int32", MyInt32);
            var xmlInt64 = XmlValue.CreateNullable<Int64?>("Int64", MyInt64);

            var xmlByte = XmlValue.CreateNullable<Byte?>("Byte", MyByte);
            var xmlSByte = XmlValue.CreateNullable<SByte?>("SByte", MySByte);

            var xmlUInt16 = XmlValue.CreateNullable<UInt16?>("UInt16", MyUInt16);
            var xmlUInt32 = XmlValue.CreateNullable<UInt32?>("UInt32", MyUInt32);
            var xmlUInt64 = XmlValue.CreateNullable<UInt64?>("UInt64", MyUInt64);

            var xmlMyDec = XmlValue.CreateNullable<Decimal?>("Decimal", MyDec);
            var xmlMyDouble = XmlValue.CreateNullable<Double?>("Double", MyDouble);
            var xmlMySingle = XmlValue.CreateNullable<Single?>("Single", MySingle);

            var resultInt = service.GetRoot(xmlInt);
            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<Int32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultInt);
            Assert.AreEqual("<Int16 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultInt16);
            Assert.AreEqual("<Int32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultInt32);
            Assert.AreEqual("<Int64 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultInt64);

            Assert.AreEqual("<Byte xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultByte);
            Assert.AreEqual("<SByte xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultSByte);

            Assert.AreEqual("<UInt16 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultUInt16);
            Assert.AreEqual("<UInt32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultUInt32);
            Assert.AreEqual("<UInt64 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultUInt64);

            Assert.AreEqual("<Decimal xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultMyDec);
            Assert.AreEqual("<Double xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultMyDouble);
            Assert.AreEqual("<Single xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", resultMySingle);
        }

        // bool

        [TestMethod]
        public void TestBool()
        {
            var service = this.GetService();

            var resultTrue = service.GetRoot(new XmlBool("Boolean", true));
            var resultFalse = service.GetRoot(new XmlBool("Boolean", false));

            Assert.AreEqual("<Boolean>true</Boolean>", resultTrue);
            Assert.AreEqual("<Boolean>false</Boolean>", resultFalse);
        }

        [TestMethod]
        public void TestBoolNullable()
        {
            var service = this.GetService();

            var resultTrue = service.GetRoot(XmlValue.CreateNullable<bool?>("Boolean", true));
            var resultFalse = service.GetRoot(XmlValue.CreateNullable<bool?>("Boolean", false));

            Assert.AreEqual("<Boolean>true</Boolean>", resultTrue);
            Assert.AreEqual("<Boolean>false</Boolean>", resultFalse);
        }

        [TestMethod]
        public void TestBoolNullableNull()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<bool?>("Boolean", null));

            Assert.AreEqual("<Boolean xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        // DateTime

        [TestMethod]
        public void TestDateTime()
        {
            var service = this.GetService();

            var result = service.GetRoot(new XmlString("DateTime",new DateTime(1990, 12, 12).ToString()));

            Assert.AreEqual("<DateTime>12/12/1990 00:00:00</DateTime>", result);
        }

        [TestMethod]
        public void TestDateTimeNullable()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<DateTime?>("DateTime",new DateTime(1990, 12, 12)));

            Assert.AreEqual("<DateTime>12/12/1990 00:00:00</DateTime>", result);
        }

        [TestMethod]
        public void TestDateTimeWithNullableNull()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<DateTime?>("DateTime", null));

            Assert.AreEqual("<DateTime xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        // guid

        [TestMethod]
        public void TestGuid()
        {
            var service = this.GetService();

            var result = service.GetRoot(new XmlString("Guid", new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176").ToString()));

            Assert.AreEqual("<Guid>344ac1a2-9613-44d7-b64c-8d45b4585176</Guid>", result);
        }

        [TestMethod]
        public void TestGuidNullable()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<Guid?>("Guid", new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176")));

            Assert.AreEqual("<Guid>344ac1a2-9613-44d7-b64c-8d45b4585176</Guid>", result);
        }

        [TestMethod]
        public void TestGuidNullableNull()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<Guid?>("Guid", null));

            Assert.AreEqual("<Guid xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        // enum

        [TestMethod]
        public void TestEnum()
        {
            var service = this.GetService();

            var result = service.GetRoot(new XmlString("MyEnum", MyEnum.Other.ToString()));

            Assert.AreEqual("<MyEnum>Other</MyEnum>", result);
        }

        [TestMethod]
        public void TestEnumNullable()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<MyEnum?>("MyEnum", MyEnum.Other));

            Assert.AreEqual("<MyEnum>Other</MyEnum>", result);
        }

        [TestMethod]
        public void TestEnumNullableNull()
        {
            var service = this.GetService();

            var result = service.GetRoot(XmlValue.CreateNullable<MyEnum?>("MyEnum", null));

            Assert.AreEqual("<MyEnum xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        // object

        [TestMethod]
        public void TestObject()
        {
            var service = this.GetService();

            var item = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var xmlValue = new XmlObject("MyItem")
                .AddString("MyGuid", item.MyGuid.ToString())
                .AddNumber("MyInt", item.MyInt)
                .AddNumber("MyDouble", item.MyDouble)
                .AddString("MyString", item.MyString)
                .AddString("MyEnumValue", item.MyEnumValue.ToString())
                .AddString("MyDate", item.MyDate.ToString())
                .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value"))
                .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", "a")).Add(new XmlString("String", "b")))
                .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", "y")).Add(new XmlString("String", "z")));

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value</MyString><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value</MyInnerString></MyObj><MyList><String>a</String><String>b</String></MyList><MyArray><String>y</String><String>z</String></MyArray></MyItem>", result);
        }


        [TestMethod]
        public void TestObjectWithNullables()
        {
            var service = this.GetService();

            var item = new MyItemNullables
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value" },
                MyList = new List<string> { "a", "b" },
                MyArray = new string[] { "y", "z" }
            };

            var xmlValue = new XmlObject("MyItem")
                .AddNullable("MyGuid", item.MyGuid)
                .AddNullable("MyInt", item.MyInt)
                .AddNullable("MyDouble", item.MyDouble)
                .AddString("MyString", item.MyString)
                .AddNullable("MyEnumValue", item.MyEnumValue)
                .AddNullable("MyDate", item.MyDate)
                .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value"))
                .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", "a")).Add(new XmlString("String", "b")))
                .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", "y")).Add(new XmlString("String", "z")));

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value</MyString><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value</MyInnerString></MyObj><MyList><String>a</String><String>b</String></MyList><MyArray><String>y</String><String>z</String></MyArray></MyItem>", result);
        }

        [TestMethod]
        public void TestObjectWithNullablesNull()
        {
            var service = this.GetService();

            var item = new MyItemNullables();

            var xmlValue = new XmlObject("MyItem")
                .AddNullable<Guid?>("MyGuid", null)
                .AddNullable<int?>("MyInt", null)
                .AddNullable<double?>("MyDouble", null)
                .AddString("MyString", null)
                .AddNullable<MyEnum?>("MyEnumValue", null)
                .AddNullable<DateTime?>("MyDate", null)
                .AddObject("MyObj", XmlValue.CreateObject("MyObj").SetNil())
                .AddArray("MyList", XmlValue.CreateArray("MyList").SetNil())
                .AddArray("MyArray", XmlValue.CreateArray("MyArray").SetNil());

            var result = service.GetRoot(xmlValue);

            Assert.AreEqual("<MyItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyGuid xsi:nil=\"true\" /><MyInt xsi:nil=\"true\" /><MyDouble xsi:nil=\"true\" /><MyString xsi:nil=\"true\" /><MyEnumValue xsi:nil=\"true\" /><MyDate xsi:nil=\"true\" /><MyObj xsi:nil=\"true\" /><MyList xsi:nil=\"true\" /><MyArray xsi:nil=\"true\" /></MyItem>", result);
        }

        // array

        [TestMethod]
        public void TestArrayOfString_WithEmptyArray()
        {
            var service = this.GetService();

            var values = new string[] { };

            var result = service.GetRoot(new XmlArray("ArrayOfString"));

            Assert.AreEqual("<ArrayOfString />", result);
        }

        [TestMethod]
        public void TestArrayOfString()
        {
            var service = this.GetService();

            var values = new string[] { "a", "b" };

            var result = service.GetRoot(new XmlArray("ArrayOfString").Add(new XmlString("String", values[0])).Add(new XmlString("String", values[1])));

            Assert.AreEqual("<ArrayOfString><String>a</String><String>b</String></ArrayOfString>", result);
        }

        [TestMethod]
        public void TestArrayOfString_WithNull()
        {
            var service = this.GetService();

            var values = new string[] { "a", null };

            var result = service.GetRoot(new XmlArray("ArrayOfString").Add(new XmlString("String", values[0])).Add(XmlValue.CreateString("String", values[1])));

            Assert.AreEqual("<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><String>a</String><String xsi:nil=\"true\" /></ArrayOfString>", result);
        }

        [TestMethod]
        public void TestArrayOfNumbers()
        {
            var service = this.GetService();

            Int16[] MyInt16 = new Int16[] { 10, 20 };
            Int32[] MyInt32 = new Int32[] { 10, 20 };
            Int64[] MyInt64 = new Int64[] { 10, 20 };

            Byte[] MyByte = new Byte[] { 201, 202 };
            SByte[] MySByte = new SByte[] { -102, 101 };

            UInt16[] MyUInt16 = new UInt16[] { 10, 20 };
            UInt32[] MyUInt32 = new UInt32[] { 10, 20 };
            UInt64[] MyUInt64 = new UInt64[] { 10, 20 };

            Decimal[] MyDec = new Decimal[] { 10.5M, 11.5M };
            Double[] MyDouble = new Double[] { 10.5, 11.5 };
            Single[] MySingle = new Single[] { 10.5F, 11.5F };

            var xmlInt16 = new XmlArray("ArrayOfInt16").Add(new XmlNumber("Int16", MyInt16[0])).Add(new XmlNumber("Int16", MyInt16[1]));
            var xmlInt32 = new XmlArray("ArrayOfInt32").Add(new XmlNumber("Int32", MyInt32[0])).Add(new XmlNumber("Int32", MyInt32[1]));
            var xmlInt64 = new XmlArray("ArrayOfInt64").Add(new XmlNumber("Int64", MyInt32[0])).Add(new XmlNumber("Int64", MyInt64[1]));

            var xmlByte = new XmlArray("ArrayOfByte").Add(new XmlNumber("Byte", MyByte[0])).Add(new XmlNumber("Byte", MyByte[1]));
            var xmlSByte = new XmlArray("ArrayOfSByte").Add(new XmlNumber("SByte", MySByte[0])).Add(new XmlNumber("SByte", MySByte[1]));

            var xmlUInt16 = new XmlArray("ArrayOfUInt16").Add(new XmlNumber("UInt16", MyInt16[0])).Add(new XmlNumber("UInt16", MyInt16[1]));
            var xmlUInt32 = new XmlArray("ArrayOfUInt32").Add(new XmlNumber("UInt32", MyInt32[0])).Add(new XmlNumber("UInt32", MyInt32[1]));
            var xmlUInt64 = new XmlArray("ArrayOfUInt64").Add(new XmlNumber("UInt64", MyInt32[0])).Add(new XmlNumber("UInt64", MyInt64[1]));

            var xmlMyDec = new XmlArray("ArrayOfDecimal").Add(new XmlNumber("Decimal", MyDec[0])).Add(new XmlNumber("Decimal", MyDec[1]));
            var xmlMyDouble = new XmlArray("ArrayOfDouble").Add(new XmlNumber("Double", MyDouble[0])).Add(new XmlNumber("Double", MyDouble[1]));
            var xmlMySingle = new XmlArray("ArrayOfSingle").Add(new XmlNumber("Single", MySingle[0])).Add(new XmlNumber("Single", MySingle[1]));

            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<ArrayOfInt16><Int16>10</Int16><Int16>20</Int16></ArrayOfInt16>", resultInt16);
            Assert.AreEqual("<ArrayOfInt32><Int32>10</Int32><Int32>20</Int32></ArrayOfInt32>", resultInt32);
            Assert.AreEqual("<ArrayOfInt64><Int64>10</Int64><Int64>20</Int64></ArrayOfInt64>", resultInt64);

            Assert.AreEqual("<ArrayOfByte><Byte>201</Byte><Byte>202</Byte></ArrayOfByte>", resultByte);
            Assert.AreEqual("<ArrayOfSByte><SByte>-102</SByte><SByte>101</SByte></ArrayOfSByte>", resultSByte);

            Assert.AreEqual("<ArrayOfUInt16><UInt16>10</UInt16><UInt16>20</UInt16></ArrayOfUInt16>", resultUInt16);
            Assert.AreEqual("<ArrayOfUInt32><UInt32>10</UInt32><UInt32>20</UInt32></ArrayOfUInt32>", resultUInt32);
            Assert.AreEqual("<ArrayOfUInt64><UInt64>10</UInt64><UInt64>20</UInt64></ArrayOfUInt64>", resultUInt64);

            Assert.AreEqual("<ArrayOfDecimal><Decimal>10.5</Decimal><Decimal>11.5</Decimal></ArrayOfDecimal>", resultMyDec);
            Assert.AreEqual("<ArrayOfDouble><Double>10.5</Double><Double>11.5</Double></ArrayOfDouble>", resultMyDouble);
            Assert.AreEqual("<ArrayOfSingle><Single>10.5</Single><Single>11.5</Single></ArrayOfSingle>", resultMySingle);
        }

        [TestMethod]
        public void TestArrayOfNullables()
        {
            var service = this.GetService();

            Int16?[] MyInt16 = new Int16?[] { 10, 20 };
            Int32?[] MyInt32 = new Int32?[] { 10, 20 };
            Int64?[] MyInt64 = new Int64?[] { 10, 20 };

            Byte?[] MyByte = new Byte?[] { 201, 202 };
            SByte?[] MySByte = new SByte?[] { -102, 101 };

            UInt16?[] MyUInt16 = new UInt16?[] { 10, 20 };
            UInt32?[] MyUInt32 = new UInt32?[] { 10, 20 };
            UInt64?[] MyUInt64 = new UInt64?[] { 10, 20 };

            Decimal?[] MyDec = new Decimal?[] { 10.5M, 11.5M };
            Double?[] MyDouble = new Double?[] { 10.5, 11.5 };
            Single?[] MySingle = new Single?[] { 10.5F, 11.5F };

            var xmlInt16 = new XmlArray("ArrayOfInt16").Add(XmlValue.CreateNullable("Int16", MyInt16[0])).Add(XmlValue.CreateNullable("Int16", MyInt16[1]));
            var xmlInt32 = new XmlArray("ArrayOfInt32").Add(XmlValue.CreateNullable("Int32", MyInt32[0])).Add(XmlValue.CreateNullable("Int32", MyInt32[1]));
            var xmlInt64 = new XmlArray("ArrayOfInt64").Add(XmlValue.CreateNullable("Int64", MyInt32[0])).Add(XmlValue.CreateNullable("Int64", MyInt64[1]));

            var xmlByte = new XmlArray("ArrayOfByte").Add(XmlValue.CreateNullable("Byte", MyByte[0])).Add(XmlValue.CreateNullable("Byte", MyByte[1]));
            var xmlSByte = new XmlArray("ArrayOfSByte").Add(XmlValue.CreateNullable("SByte", MySByte[0])).Add(XmlValue.CreateNullable("SByte", MySByte[1]));

            var xmlUInt16 = new XmlArray("ArrayOfUInt16").Add(XmlValue.CreateNullable("UInt16", MyInt16[0])).Add(XmlValue.CreateNullable("UInt16", MyInt16[1]));
            var xmlUInt32 = new XmlArray("ArrayOfUInt32").Add(XmlValue.CreateNullable("UInt32", MyInt32[0])).Add(XmlValue.CreateNullable("UInt32", MyInt32[1]));
            var xmlUInt64 = new XmlArray("ArrayOfUInt64").Add(XmlValue.CreateNullable("UInt64", MyInt32[0])).Add(XmlValue.CreateNullable("UInt64", MyInt64[1]));

            var xmlMyDec = new XmlArray("ArrayOfDecimal").Add(XmlValue.CreateNullable("Decimal", MyDec[0])).Add(XmlValue.CreateNullable("Decimal", MyDec[1]));
            var xmlMyDouble = new XmlArray("ArrayOfDouble").Add(XmlValue.CreateNullable("Double", MyDouble[0])).Add(XmlValue.CreateNullable("Double", MyDouble[1]));
            var xmlMySingle = new XmlArray("ArrayOfSingle").Add(XmlValue.CreateNullable("Single", MySingle[0])).Add(XmlValue.CreateNullable("Single", MySingle[1]));

            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<ArrayOfInt16><Int16>10</Int16><Int16>20</Int16></ArrayOfInt16>", resultInt16);
            Assert.AreEqual("<ArrayOfInt32><Int32>10</Int32><Int32>20</Int32></ArrayOfInt32>", resultInt32);
            Assert.AreEqual("<ArrayOfInt64><Int64>10</Int64><Int64>20</Int64></ArrayOfInt64>", resultInt64);

            Assert.AreEqual("<ArrayOfByte><Byte>201</Byte><Byte>202</Byte></ArrayOfByte>", resultByte);
            Assert.AreEqual("<ArrayOfSByte><SByte>-102</SByte><SByte>101</SByte></ArrayOfSByte>", resultSByte);

            Assert.AreEqual("<ArrayOfUInt16><UInt16>10</UInt16><UInt16>20</UInt16></ArrayOfUInt16>", resultUInt16);
            Assert.AreEqual("<ArrayOfUInt32><UInt32>10</UInt32><UInt32>20</UInt32></ArrayOfUInt32>", resultUInt32);
            Assert.AreEqual("<ArrayOfUInt64><UInt64>10</UInt64><UInt64>20</UInt64></ArrayOfUInt64>", resultUInt64);

            Assert.AreEqual("<ArrayOfDecimal><Decimal>10.5</Decimal><Decimal>11.5</Decimal></ArrayOfDecimal>", resultMyDec);
            Assert.AreEqual("<ArrayOfDouble><Double>10.5</Double><Double>11.5</Double></ArrayOfDouble>", resultMyDouble);
            Assert.AreEqual("<ArrayOfSingle><Single>10.5</Single><Single>11.5</Single></ArrayOfSingle>", resultMySingle);
        }

        [TestMethod]
        public void TestArrayOfNullablesNull()
        {
            var service = this.GetService();

            Int16?[] MyInt16 = new Int16?[] { 10, null };
            Int32?[] MyInt32 = new Int32?[] { 10, null };
            Int64?[] MyInt64 = new Int64?[] { 10, null };

            Byte?[] MyByte = new Byte?[] { 201, null };
            SByte?[] MySByte = new SByte?[] { -102, null };

            UInt16?[] MyUInt16 = new UInt16?[] { 10, null };
            UInt32?[] MyUInt32 = new UInt32?[] { 10, null };
            UInt64?[] MyUInt64 = new UInt64?[] { 10, null };

            Decimal?[] MyDec = new Decimal?[] { 10.5M, null };
            Double?[] MyDouble = new Double?[] { 10.5, null };
            Single?[] MySingle = new Single?[] { 10.5F, null };

            var xmlInt16 = new XmlArray("ArrayOfInt16").Add(XmlValue.CreateNullable("Int16", MyInt16[0])).Add(XmlValue.CreateNullable("Int16", MyInt16[1]));
            var xmlInt32 = new XmlArray("ArrayOfInt32").Add(XmlValue.CreateNullable("Int32", MyInt32[0])).Add(XmlValue.CreateNullable("Int32", MyInt32[1]));
            var xmlInt64 = new XmlArray("ArrayOfInt64").Add(XmlValue.CreateNullable("Int64", MyInt32[0])).Add(XmlValue.CreateNullable("Int64", MyInt64[1]));

            var xmlByte = new XmlArray("ArrayOfByte").Add(XmlValue.CreateNullable("Byte", MyByte[0])).Add(XmlValue.CreateNullable("Byte", MyByte[1]));
            var xmlSByte = new XmlArray("ArrayOfSByte").Add(XmlValue.CreateNullable("SByte", MySByte[0])).Add(XmlValue.CreateNullable("SByte", MySByte[1]));

            var xmlUInt16 = new XmlArray("ArrayOfUInt16").Add(XmlValue.CreateNullable("UInt16", MyInt16[0])).Add(XmlValue.CreateNullable("UInt16", MyInt16[1]));
            var xmlUInt32 = new XmlArray("ArrayOfUInt32").Add(XmlValue.CreateNullable("UInt32", MyInt32[0])).Add(XmlValue.CreateNullable("UInt32", MyInt32[1]));
            var xmlUInt64 = new XmlArray("ArrayOfUInt64").Add(XmlValue.CreateNullable("UInt64", MyInt32[0])).Add(XmlValue.CreateNullable("UInt64", MyInt64[1]));

            var xmlMyDec = new XmlArray("ArrayOfDecimal").Add(XmlValue.CreateNullable("Decimal", MyDec[0])).Add(XmlValue.CreateNullable("Decimal", MyDec[1]));
            var xmlMyDouble = new XmlArray("ArrayOfDouble").Add(XmlValue.CreateNullable("Double", MyDouble[0])).Add(XmlValue.CreateNullable("Double", MyDouble[1]));
            var xmlMySingle = new XmlArray("ArrayOfSingle").Add(XmlValue.CreateNullable("Single", MySingle[0])).Add(XmlValue.CreateNullable("Single", MySingle[1]));

            var resultInt16 = service.GetRoot(xmlInt16);
            var resultInt32 = service.GetRoot(xmlInt32);
            var resultInt64 = service.GetRoot(xmlInt64);

            var resultByte = service.GetRoot(xmlByte);
            var resultSByte = service.GetRoot(xmlSByte);

            var resultUInt16 = service.GetRoot(xmlUInt16);
            var resultUInt32 = service.GetRoot(xmlUInt32);
            var resultUInt64 = service.GetRoot(xmlUInt64);

            var resultMyDec = service.GetRoot(xmlMyDec);
            var resultMyDouble = service.GetRoot(xmlMyDouble);
            var resultMySingle = service.GetRoot(xmlMySingle);

            Assert.AreEqual("<ArrayOfInt16 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Int16>10</Int16><Int16 xsi:nil=\"true\" /></ArrayOfInt16>", resultInt16);
            Assert.AreEqual("<ArrayOfInt32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Int32>10</Int32><Int32 xsi:nil=\"true\" /></ArrayOfInt32>", resultInt32);
            Assert.AreEqual("<ArrayOfInt64 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Int64>10</Int64><Int64 xsi:nil=\"true\" /></ArrayOfInt64>", resultInt64);

            Assert.AreEqual("<ArrayOfByte xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Byte>201</Byte><Byte xsi:nil=\"true\" /></ArrayOfByte>", resultByte);
            Assert.AreEqual("<ArrayOfSByte xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><SByte>-102</SByte><SByte xsi:nil=\"true\" /></ArrayOfSByte>", resultSByte);

            Assert.AreEqual("<ArrayOfUInt16 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><UInt16>10</UInt16><UInt16 xsi:nil=\"true\" /></ArrayOfUInt16>", resultUInt16);
            Assert.AreEqual("<ArrayOfUInt32 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><UInt32>10</UInt32><UInt32 xsi:nil=\"true\" /></ArrayOfUInt32>", resultUInt32);
            Assert.AreEqual("<ArrayOfUInt64 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><UInt64>10</UInt64><UInt64 xsi:nil=\"true\" /></ArrayOfUInt64>", resultUInt64);

            Assert.AreEqual("<ArrayOfDecimal xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Decimal>10.5</Decimal><Decimal xsi:nil=\"true\" /></ArrayOfDecimal>", resultMyDec);
            Assert.AreEqual("<ArrayOfDouble xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Double>10.5</Double><Double xsi:nil=\"true\" /></ArrayOfDouble>", resultMyDouble);
            Assert.AreEqual("<ArrayOfSingle xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Single>10.5</Single><Single xsi:nil=\"true\" /></ArrayOfSingle>", resultMySingle);
        }

        [TestMethod]
        public void TestArrayOfBool()
        {
            var service = this.GetService();

            var values = new bool[] { true, false };

            var result = service.GetRoot(new XmlArray("ArrayOfBoolean").Add(new XmlBool("Boolean", values[0])).Add(new XmlBool("Boolean", values[1])));

            Assert.AreEqual("<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>", result);
        }

        [TestMethod]
        public void TestArrayOfBoolNullables()
        {
            var service = this.GetService();

            var values = new bool?[] { true, false };

            var result = service.GetRoot(new XmlArray("ArrayOfBoolean").Add(XmlValue.CreateNullable("Boolean", values[0])).Add(XmlValue.CreateNullable("Boolean", values[1])));

            Assert.AreEqual("<ArrayOfBoolean><Boolean>true</Boolean><Boolean>false</Boolean></ArrayOfBoolean>", result);
        }

        [TestMethod]
        public void TestArrayOfBoolNullablesWithNulls()
        {
            var service = this.GetService();

            var values = new bool?[] { true, null };

            var result = service.GetRoot(new XmlArray("ArrayOfBoolean").Add(XmlValue.CreateNullable("Boolean", values[0])).Add(XmlValue.CreateNullable("Boolean", values[1])));

            Assert.AreEqual("<ArrayOfBoolean xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Boolean>true</Boolean><Boolean xsi:nil=\"true\" /></ArrayOfBoolean>", result);
        }

        [TestMethod]
        public void TestArrayOfBoolNull()
        {
            var service = this.GetService();

            bool[] values = null;

            var result = service.GetRoot(new XmlArray("ArrayOfBoolean").SetNil());

            Assert.AreEqual("<ArrayOfBoolean xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:nil=\"true\" />", result);
        }

        [TestMethod]
        public void TestArrayOfObjects()
        {
            var service = this.GetService();

            var item = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value 1",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 1" },
                MyList = new List<string> { "a1", "b1" },
                MyArray = new string[] { "y1", "z1" }
            };

            var item2 = new MyItem
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c3"),
                MyInt = 2,
                MyDouble = 2.5,
                MyString = "my value 2",
                MyBool = false,
                MyEnumValue = MyEnum.Default,
                MyDate = new DateTime(1990, 10, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 2" },
                MyList = new List<string> { "a2", "b2" },
                MyArray = new string[] { "y2", "z2" }
            };

            var xmlArray = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddString("MyGuid", item.MyGuid.ToString())
                    .AddNumber("MyInt", item.MyInt)
                    .AddNumber("MyDouble", item.MyDouble)
                    .AddString("MyString", item.MyString)
                    .AddString("MyEnumValue", item.MyEnumValue.ToString())
                    .AddString("MyDate", item.MyDate.ToString())
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 1"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item.MyList[0])).Add(new XmlString("String", item.MyList[1])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item.MyArray[0])).Add(new XmlString("String", item.MyArray[1]))))
               .Add(new XmlObject("MyItem")
                    .AddString("MyGuid", item2.MyGuid.ToString())
                    .AddNumber("MyInt", item2.MyInt)
                    .AddNumber("MyDouble", item2.MyDouble)
                    .AddString("MyString", item2.MyString)
                    .AddString("MyEnumValue", item2.MyEnumValue.ToString())
                    .AddString("MyDate", item2.MyDate.ToString())
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 2"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item2.MyList[0])).Add(new XmlString("String", item2.MyList[0])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item2.MyArray[0])).Add(new XmlString("String", item2.MyArray[1]))));

            var result = service.GetRoot(xmlArray);

            Assert.AreEqual("<MyItems><MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value 1</MyString><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value 1</MyInnerString></MyObj><MyList><String>a1</String><String>b1</String></MyList><MyArray><String>y1</String><String>z1</String></MyArray></MyItem><MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c3</MyGuid><MyInt>2</MyInt><MyDouble>2.5</MyDouble><MyString>my value 2</MyString><MyEnumValue>Default</MyEnumValue><MyDate>12/10/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value 2</MyInnerString></MyObj><MyList><String>a2</String><String>a2</String></MyList><MyArray><String>y2</String><String>z2</String></MyArray></MyItem></MyItems>", result);
        }

        [TestMethod]
        public void TestArrayOfObjectsNullables()
        {
            var service = this.GetService();

            var item = new MyItemNullables
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c2"),
                MyInt = 1,
                MyDouble = 1.5,
                MyString = "my value 1",
                MyBool = true,
                MyEnumValue = MyEnum.Other,
                MyDate = new DateTime(1990, 12, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 1" },
                MyList = new List<string> { "a1", "b1" },
                MyArray = new string[] { "y1", "z1" }
            };

            var item2 = new MyItemNullables
            {
                MyGuid = new Guid("11ba7957-5afb-4b59-9d9b-c06a18cda5c3"),
                MyInt = 2,
                MyDouble = 2.5,
                MyString = "my value 2",
                MyBool = false,
                MyEnumValue = MyEnum.Default,
                MyDate = new DateTime(1990, 10, 12),
                MyObj = new MyInnerItem { MyInnerString = "my inner value 2" },
                MyList = new List<string> { "a2", "b2" },
                MyArray = new string[] { "y2", "z2" }
            };

            var xmlArray = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddNullable("MyGuid", item.MyGuid)
                    .AddNullable("MyInt", item.MyInt)
                    .AddNullable("MyDouble", item.MyDouble)
                    .AddString("MyString", item.MyString)
                    .AddNullable("MyEnumValue", item.MyEnumValue)
                    .AddNullable("MyDate", item.MyDate)
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 1"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item.MyList[0])).Add(new XmlString("String", item.MyList[1])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item.MyArray[0])).Add(new XmlString("String", item.MyArray[1]))))
               .Add(new XmlObject("MyItem")
                    .AddNullable("MyGuid", item2.MyGuid)
                    .AddNullable("MyInt", item2.MyInt)
                    .AddNullable("MyDouble", item2.MyDouble)
                    .AddString("MyString", item2.MyString)
                    .AddNullable("MyEnumValue", item2.MyEnumValue)
                    .AddNullable("MyDate", item2.MyDate)
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").AddString("MyInnerString", "my inner value 2"))
                    .AddArray("MyList", XmlValue.CreateArray("MyList").Add(new XmlString("String", item2.MyList[0])).Add(new XmlString("String", item2.MyList[0])))
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").Add(new XmlString("String", item2.MyArray[0])).Add(new XmlString("String", item2.MyArray[1]))));

            var result = service.GetRoot(xmlArray);

            Assert.AreEqual("<MyItems><MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c2</MyGuid><MyInt>1</MyInt><MyDouble>1.5</MyDouble><MyString>my value 1</MyString><MyEnumValue>Other</MyEnumValue><MyDate>12/12/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value 1</MyInnerString></MyObj><MyList><String>a1</String><String>b1</String></MyList><MyArray><String>y1</String><String>z1</String></MyArray></MyItem><MyItem><MyGuid>11ba7957-5afb-4b59-9d9b-c06a18cda5c3</MyGuid><MyInt>2</MyInt><MyDouble>2.5</MyDouble><MyString>my value 2</MyString><MyEnumValue>Default</MyEnumValue><MyDate>12/10/1990 00:00:00</MyDate><MyObj><MyInnerString>my inner value 2</MyInnerString></MyObj><MyList><String>a2</String><String>a2</String></MyList><MyArray><String>y2</String><String>z2</String></MyArray></MyItem></MyItems>", result);
        }

        [TestMethod]
        public void TestArrayOfObjectsNullablesNull()
        {
            var service = this.GetService();

            var item = new MyItemNullables();

            var item2 = new MyItemNullables();

            var xmlArray = new XmlArray("MyItems")
                .Add(new XmlObject("MyItem")
                    .AddNullable("MyGuid", item.MyGuid)
                    .AddNullable("MyInt", item.MyInt)
                    .AddNullable("MyDouble", item.MyDouble)
                    .AddString("MyString", item.MyString)
                    .AddNullable("MyEnumValue", item.MyEnumValue)
                    .AddNullable("MyDate", item.MyDate)
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").SetNil())
                    .AddArray("MyList", XmlValue.CreateArray("MyList").SetNil())
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").SetNil()))
               .Add(new XmlObject("MyItem")
                    .AddNullable("MyGuid", item2.MyGuid)
                    .AddNullable("MyInt", item2.MyInt)
                    .AddNullable("MyDouble", item2.MyDouble)
                    .AddString("MyString", item2.MyString)
                    .AddNullable("MyEnumValue", item2.MyEnumValue)
                    .AddNullable("MyDate", item2.MyDate)
                    .AddObject("MyObj", XmlValue.CreateObject("MyObj").SetNil())
                    .AddArray("MyList", XmlValue.CreateArray("MyList").SetNil())
                    .AddArray("MyArray", XmlValue.CreateArray("MyArray").SetNil()));

            var result = service.GetRoot(xmlArray);

            Assert.AreEqual("<MyItems xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MyItem><MyGuid xsi:nil=\"true\" /><MyInt xsi:nil=\"true\" /><MyDouble xsi:nil=\"true\" /><MyString xsi:nil=\"true\" /><MyEnumValue xsi:nil=\"true\" /><MyDate xsi:nil=\"true\" /><MyObj xsi:nil=\"true\" /><MyList xsi:nil=\"true\" /><MyArray xsi:nil=\"true\" /></MyItem><MyItem><MyGuid xsi:nil=\"true\" /><MyInt xsi:nil=\"true\" /><MyDouble xsi:nil=\"true\" /><MyString xsi:nil=\"true\" /><MyEnumValue xsi:nil=\"true\" /><MyDate xsi:nil=\"true\" /><MyObj xsi:nil=\"true\" /><MyList xsi:nil=\"true\" /><MyArray xsi:nil=\"true\" /></MyItem></MyItems>", result);
        }
    }

  
}
