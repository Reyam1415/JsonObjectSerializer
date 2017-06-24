using System;
using System.Collections.Generic;

namespace JsonLib.Mappings.Xml
{
    public class XmlTypeMapping
    {
        public Type Type { get; }

        public string XmlTypeName { get; protected set; }

        public string XmlArrayName { get; protected set; }

        public bool HasXmlArrayName => !string.IsNullOrEmpty(this.XmlArrayName);

        public Dictionary<string, XmlPropertyMapping> Properties { get; }

        public XmlTypeMapping(Type type, string xmlObjectName)
        {
            this.Properties = new Dictionary<string, XmlPropertyMapping>();

            this.Type = type;
            this.XmlTypeName = xmlObjectName;
        }

        public bool Has(string propertyName)
        {
            return this.Properties.ContainsKey(propertyName);
        }

        public XmlPropertyMapping Get(string propertyName)
        {
            if (!this.Has(propertyName)) { throw new JsonLibException("No mapping registered for property " + propertyName); }

            return this.Properties[propertyName];
        }

        public XmlTypeMapping SetArrayName(string xmlArrayName)
        {
            this.XmlArrayName = xmlArrayName;
            return this;
        }

        public XmlTypeMapping SetProperty(string propertyName, string xmlPropertyName)
        {
            this.Properties[propertyName] = new XmlPropertyMapping(propertyName, xmlPropertyName);
            return this;
        }

        public bool HasByXmlPropertyName(string xmlPropertyName)
        {
            foreach (var property in this.Properties)
            {
                if (property.Value.XmlPropertyName == xmlPropertyName)
                {
                    return true;
                }
            }
            return false;
        }

        public XmlPropertyMapping GetByXmlPropertyName(string xmlPropertyName)
        {
            if (!this.HasByXmlPropertyName(xmlPropertyName)) { throw new JsonLibException("No mapping registered for " + xmlPropertyName); }

            foreach (var property in this.Properties)
            {
                if (property.Value.XmlPropertyName == xmlPropertyName)
                {
                    return property.Value;
                }
            }
            return null;
        }
    }
}
