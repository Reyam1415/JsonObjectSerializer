using JsonLib.Mappings.Xml;

namespace JsonLib.Xml
{
    public class ObjectToXml : IObjectToXml
    {
        protected IObjectToXmlValue objectToXmlValue;
        protected IXmlValueToXml xmlValueToXml;

        public ObjectToXml()
            :this(new ObjectToXmlValue(), new XmlValueToXml())
        { }

        public ObjectToXml(
            IObjectToXmlValue objectToXmlValue,
            IXmlValueToXml xmlValueToXml)
        {
            this.objectToXmlValue = objectToXmlValue;
            this.xmlValueToXml = xmlValueToXml;
        }

        public string ToXml<T>(T value, XmlMappingContainer mappings = null)
        {
            var xmlValue = this.objectToXmlValue.ToXmlValue<T>(value, mappings);
            return this.xmlValueToXml.CreateDocument(xmlValue);
        }
    }
}
