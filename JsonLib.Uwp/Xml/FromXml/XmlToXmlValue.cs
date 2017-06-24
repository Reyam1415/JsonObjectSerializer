using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JsonLib.Xml
{
    public class XmlToXmlValue : IXmlToXmlValue
    {
        public bool IsTextElement(XElement element)
        {
            if (element.FirstNode != null && element.FirstNode.NodeType == XmlNodeType.Text)
            {
                return true;
            }
            return false;
        }

        public bool IsArrayElement(XElement element)
        {
            if (element.HasElements)
            {
                var elements = element.Elements();
                if (elements.Count() > 1)
                {
                    var first = elements.FirstOrDefault();
                    string nodeName = first.Name.LocalName;
                    foreach (var child in elements)
                    {
                        var childNodeName = child.Name.LocalName;
                        if (childNodeName != nodeName)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool TryParseNumber(string valueString, out object result)
        {
            if (int.TryParse(valueString, out int intResult))
            {
                result = intResult;
                return true;
            }
            else if (double.TryParse(valueString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double doubleResult))
            {
                result = doubleResult;
                return true;
            }
            result = null;
            return false;
        }

        public IXmlValue ToXmlValue(XElement element)
        {
            // string , bool, number , nullable ?
            var nodeName = element.Name.LocalName;
            var text = element.FirstNode as XText;
            var value = text.Value;
            if(this.TryParseNumber(value, out object numberResult))
            {
                return new XmlNumber(nodeName, numberResult);
            }
            else if (value == "true")
            {
                return new XmlBool(nodeName, true);
            }
            if (value == "false")
            {
                return new XmlBool(nodeName, false);
            }
            else
            {
                return new XmlString(nodeName, value);
            }
        }

        public IXmlValue FindNextElement(XElement element)
        {
            if (element.IsEmpty)
            {
                var nodeName = element.Name.LocalName;
                var nil = element.Attribute(XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance"));
                if (nil != null)
                {
                    return new XmlString(nodeName, null);
                }
                else
                {
                    return new XmlString(nodeName, "");
                }
            }
            else if (this.IsTextElement(element))
            {
                return this.ToXmlValue(element);
            }
            else if (this.IsArrayElement(element))
            {
                return this.ToXmlArray(element);
            }
            else
            {
                return this.ToXmlObject(element);
            }
        }

        public XmlObject ToXmlObject(XElement element)
        {
            var nodeName = element.Name.LocalName;
            var result = new XmlObject(nodeName);

            foreach (var child in element.Elements())
            {
                var xmlValue = this.FindNextElement(child);
                result.Add(xmlValue.NodeName, xmlValue);
            }

            return result;
        }

        public XmlArray ToXmlArray(XElement element)
        {
            var nodeName = element.Name.LocalName;
            var result = new XmlArray(nodeName);

            foreach (var child in element.Elements())
            {
                var xmlValue = this.FindNextElement(child);
                result.Add(xmlValue);
            }

            return result;
        }

        public IXmlValue ToXmlValue(string xml)
        {
            var document = XDocument.Parse(xml);
            var root = document.Root;

            return this.FindNextElement(root);
        }

    }
}
