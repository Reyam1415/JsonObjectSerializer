using System.Collections.Generic;

namespace JsonLib.Xml
{

    public class XmlArray : IXmlValue, IXmlNillable
    {
        public XmlValueType ValueType => XmlValueType.Array;

        public string NodeName { get; protected set; }

        public List<IXmlValue> Values { get; set; }

        public bool HasValues => this.Values.Count > 0;

        public bool HasNils { get; protected set; }

        public bool IsNil { get; protected set; }

        public XmlArray(string nodeName)
        {
            this.Values = new List<IXmlValue>();

            this.NodeName = nodeName;
        }

        public XmlArray SetNil()
        {
            this.Values.Clear();
            this.IsNil = true;
            return this;
        }

        protected void CheckValue(IXmlValue xmlValue)
        {
            if (this.HasValues)
            {
                if (xmlValue.ValueType != this.Values[0].ValueType)
                {
                    throw new JsonLibException("Array require items with same type");
                }
                else if (xmlValue.NodeName != this.Values[0].NodeName)
                {
                    throw new JsonLibException("Array require items with same node name");
                }
            }
        }

        public XmlArray Add(IXmlValue xmlValue)
        {
            this.CheckValue(xmlValue);

            this.Values.Add(xmlValue);

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
    }

}
