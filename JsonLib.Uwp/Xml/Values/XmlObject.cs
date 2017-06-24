using System.Collections.Generic;

namespace JsonLib.Xml
{

    public class XmlObject : IXmlValue, IXmlNillable
    {
        public XmlValueType ValueType => XmlValueType.Object;

        public string NodeName { get; protected set; }

        public bool IsNil { get; protected set; }

        public bool HasNils { get; protected set; }

        public Dictionary<string, IXmlValue> Values { get; set; }

        public XmlObject(string nodeName)
        {
            this.Values = new Dictionary<string, IXmlValue>();

            this.NodeName = nodeName;
        }

        public XmlObject SetNil()
        {
            this.Values.Clear();
            this.IsNil = true;
            return this;
        }

        public bool HasValue(string nodeName)
        {
            return this.Values.ContainsKey(nodeName);
        }

        public XmlObject Add(string nodeName, IXmlValue xmlValue)
        {
            if (this.HasValue(nodeName)) { throw new JsonLibException("A value with the name " + nodeName + " is already registered"); }

            this.Values[nodeName] = xmlValue;

            if (!this.HasNils)
            {
                if (xmlValue.ValueType == XmlValueType.String)
                {
                    if (((XmlString)xmlValue).IsNil)
                    {
                        this.HasNils = true;
                    }
                }
                else if (xmlValue.ValueType == XmlValueType.Nullable)
                {
                    if (((XmlNullable)xmlValue).IsNil)
                    {
                        this.HasNils = true;
                    }
                }
                if (xmlValue.ValueType == XmlValueType.Object)
                {
                    if (((XmlObject)xmlValue).IsNil || ((XmlObject)xmlValue).HasNils)
                    {
                        this.HasNils = true;
                    }
                }
                else if (xmlValue.ValueType == XmlValueType.Array)
                {
                    if (((XmlArray)xmlValue).IsNil || ((XmlArray)xmlValue).HasNils)
                    {
                        this.HasNils = true;
                    }
                }
            }

            return this;
        }

        public XmlObject AddString(string nodeName, string value)
        {
            return this.Add(nodeName, new XmlString(nodeName, value));
        }

        public XmlObject AddNumber(string nodeName, object value)
        {
            return this.Add(nodeName, new XmlNumber(nodeName, value));
        }

        public XmlObject AddBool(string nodeName, bool value)
        {
            return this.Add(nodeName, new XmlBool(nodeName, value));
        }

        public XmlObject AddNullable<T>(string nodeName, T value)
        {
            return this.Add(nodeName, new XmlNullable(typeof(T), nodeName, value));
        }

        public XmlObject AddObject(string nodeName, XmlObject value)
        {
            return this.Add(nodeName, value);
        }

        public XmlObject AddArray(string nodeName, XmlArray value)
        {
            return this.Add(nodeName, value);
        }
    }


}
