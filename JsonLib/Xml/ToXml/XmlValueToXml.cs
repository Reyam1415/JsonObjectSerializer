using JsonLib.Common;
using System.Collections.Generic;

namespace JsonLib.Xml
{
    public class XmlValueToXml : IXmlValueToXml
    {
        protected IXmlValueToXmlService xmlService;
        protected IAssemblyInfoService assemblyInfoService;

        public XmlValueToXml()
            : this(new XmlValueToXmlService(), new AssemblyInfoService())
        { }

        public XmlValueToXml(IXmlValueToXmlService xmlService, IAssemblyInfoService assemblyInfoService)
        {
            this.xmlService = xmlService;
            this.assemblyInfoService = assemblyInfoService;
        }

        public string ToString(XmlString xmlValue)
        {
            var value = xmlValue.Value;
            if (value != null)
            {
                if (value == "")
                {
                    return this.xmlService.GetRoot(xmlValue.NodeName, null, false);
                }
                else
                {
                    return this.xmlService.GetRoot(xmlValue.NodeName, value, false);
                }
            }
            else
            {
                return this.xmlService.GetNilNode(xmlValue.NodeName);
            }
        }

        public string ToNumber(XmlNumber xmlValue)
        {
            return this.xmlService.GetNumberNode(xmlValue.NodeName, xmlValue.Value);
        }

        public string ToBool(XmlBool xmlValue)
        {
            var value = xmlValue.Value == true ? "true" : "false";
            return this.xmlService.GetNode(xmlValue.NodeName, value);
        }

        public string GetNullableString(object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                // Int16,Int32, ...Byte, ...Decimal, Enum, DateTime, Guid
                if (this.assemblyInfoService.IsNumberType(type))
                {
                    return this.xmlService.GetNumber(value);
                }
                else if (type == typeof(bool))
                {
                    return (bool)value == true ? "true" : "false";
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return null;
            }
        }

        public string ToNullable(XmlNullable xmlValue)
        {
            var valueString = this.GetNullableString(xmlValue.Value);
            if (valueString != null)
            {
                return this.xmlService.GetNode(xmlValue.NodeName, valueString);
            }
            else
            {
                return this.xmlService.GetNilNode(xmlValue.NodeName);
            }
        }

        public string ToObject(XmlObject xmlObject)
        {
            if (xmlObject.IsNil)
            {
                return this.xmlService.GetNilNode(xmlObject.NodeName);
            }
            else
            {
                var result = new List<string>();

                foreach (var keyValue in xmlObject.Values)
                {
                    var xmlValue = keyValue.Value;
                    var valueResult = this.ToValue(xmlValue);
                    result.Add(valueResult);
                }

                return this.xmlService.GetNode(xmlObject.NodeName, string.Join("", result));
            }
        }

        public string ToArray(XmlArray xmlArray)
        {
            if (xmlArray.IsNil)
            {
                return this.xmlService.GetNilNode(xmlArray.NodeName);
            }
            else
            {
                var result = new List<string>();

                foreach (var xmlValue in xmlArray.Values)
                {
                    var valueResult = this.ToValue(xmlValue);
                    result.Add(valueResult);
                }

                return this.xmlService.GetNode(xmlArray.NodeName, string.Join("", result));
            }
        }

        public bool HasNils(IXmlValue xmlValue)
        {
            if (xmlValue.ValueType == XmlValueType.String)
            {
                return ((XmlString)xmlValue).IsNil;
            }
            else if (xmlValue.ValueType == XmlValueType.Nullable)
            {
                return ((XmlNullable)xmlValue).IsNil;
            }
            else if (xmlValue.ValueType == XmlValueType.Object)
            {
                return ((XmlObject)xmlValue).HasNils;
            }
            else if (xmlValue.ValueType == XmlValueType.Array)
            {
                return ((XmlArray)xmlValue).HasNils;
            }

            return false;
        }

        public string ToValue(IXmlValue xmlValue)
        {
            if (xmlValue.ValueType == XmlValueType.String)
            {
                return this.ToString((XmlString)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Number)
            {
                return this.ToNumber((XmlNumber)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Bool)
            {
                return this.ToBool((XmlBool)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Nullable)
            {
                return this.ToNullable((XmlNullable)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Array)
            {
                return this.ToArray((XmlArray)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Object)
            {
                return this.ToObject((XmlObject)xmlValue);
            }

            throw new JsonLibException("Cannot resolve Xml Value");
        }

        public string GetRoot(IXmlValue xmlValue)
        {

            var hasNils = this.HasNils(xmlValue);

            if (xmlValue.ValueType == XmlValueType.String)
            {
                var xmlString = xmlValue as XmlString;
                return this.xmlService.GetRoot(xmlValue.NodeName, xmlString.Value, xmlString.IsNil);
            }
            else if (xmlValue.ValueType == XmlValueType.Number)
            {
                var valueString = this.xmlService.GetNumber(((XmlNumber)xmlValue).Value);
                return this.xmlService.GetRoot(xmlValue.NodeName, valueString);
            }
            else if (xmlValue.ValueType == XmlValueType.Bool)
            {
                var valueString = ((XmlBool) xmlValue).Value == true ? "true" : "false";
                return this.xmlService.GetRoot(xmlValue.NodeName, valueString);
            }
            else if (xmlValue.ValueType == XmlValueType.Nullable)
            {
                var xmlNullable = xmlValue as XmlNullable;
                var valueString = xmlNullable.IsNil ? null : this.GetNullableString(xmlNullable.Value); 
                return this.xmlService.GetRoot(xmlValue.NodeName, valueString, xmlNullable.IsNil);
            }
            else if (xmlValue.ValueType == XmlValueType.Array)
            {
                var xmlArray = xmlValue as XmlArray;
                if (xmlArray.IsNil)
                {
                    return this.xmlService.GetRoot(xmlArray.NodeName, null, true);
                }
                else
                {
                    var result = new List<string>();

                    foreach (var xmlInnerValue in xmlArray.Values)
                    {
                        var valueResult = this.ToValue(xmlInnerValue);
                        result.Add(valueResult);
                    }

                    return this.xmlService.GetRoot(xmlArray.NodeName, string.Join("", result),xmlArray.HasNils);
                }
            }
            else if (xmlValue.ValueType == XmlValueType.Object)
            {
                var xmlObject = xmlValue as XmlObject;
                if (xmlObject.IsNil)
                {
                    return this.xmlService.GetRoot(xmlObject.NodeName, null, true);
                }
                else
                {
                    var result = new List<string>();

                    foreach (var keyValue in xmlObject.Values)
                    {
                        var xmlInnerValue = keyValue.Value;
                        var valueResult = this.ToValue(xmlInnerValue);
                        result.Add(valueResult);
                    }

                    return this.xmlService.GetRoot(xmlObject.NodeName, string.Join("", result), xmlObject.HasNils);
                }
            }

            throw new JsonLibException("Cannot resolve Root");
        }

        public string CreateDocument(IXmlValue xmlValue)
        {
            var declaration = this.xmlService.GetDeclaration();
            var content = this.GetRoot(xmlValue);
            return declaration + content;
        }

    }
}
