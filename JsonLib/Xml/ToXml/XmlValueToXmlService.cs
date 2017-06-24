using System;
using System.Globalization;

namespace JsonLib.Xml
{
    public class XmlValueToXmlService : IXmlValueToXmlService
    {
        public string GetNumber(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public string GetNumberNode(string nodeName, object value)
        {
            return "<" + nodeName + ">" + this.GetNumber(value) + "</" + nodeName + ">";
        }

        public string GetDeclaration()
        {
            return "<?xml version=\"1.0\"?>\r";
        }

        public string GetBooValueString(bool value)
        {
            return value ? "true" : "false";
        }

        public string GetNilNamespace()
        {
            return "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
        }

        public bool HasContent(string content)
        {
            return !string.IsNullOrEmpty(content);
        }

        public string GetRoot(string nodeName, string content, bool nilNamespace = false)
        {
            bool hasContent = this.HasContent(content);

            if (nilNamespace)
            {
                if (hasContent)
                {
                    return "<" + nodeName + " " + this.GetNilNamespace() + ">" + content + "</" + nodeName + ">";
                }
                else
                {
                    return "<" + nodeName + " " + this.GetNilNamespace() + " xsi:nil=\"true\" />";
                }
            }
            else
            {
                if (hasContent)
                {
                    return "<" + nodeName + ">" + content + "</" + nodeName + ">";
                }
                else
                {
                    return "<" + nodeName + " />";
                }
            }
        }

        public string GetOneClosedNode(string nodeName)
        {
            return "<" + nodeName + " />";
        }

        public string GetNode(string nodeName, string value)
        {
            return !this.HasContent(value) ? this.GetOneClosedNode(nodeName) : "<" + nodeName + ">" + value + "</" + nodeName + ">";
        }

        public string GetNilNode(string nodeName)
        {
            return "<" + nodeName + " xsi:nil=\"true\" />";
        }

    }
}
