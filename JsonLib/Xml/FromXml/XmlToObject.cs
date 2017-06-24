using JsonLib.Mappings.Xml;

namespace JsonLib.Xml
{
    public  class XmlToObject : IXmlToObject
    {
        protected IXmlToXmlValue xmlToXmlValue;
        protected IXmlValueToObject xmlValueToObject;

        public XmlToObject()
            :this(new XmlToXmlValue(), new XmlValueToObject())
        {

        }
        public XmlToObject(
            IXmlToXmlValue xmlToXmlValue,
            IXmlValueToObject xmlValueToObject)
        {
            this.xmlToXmlValue = xmlToXmlValue;
            this.xmlValueToObject = xmlValueToObject;
        }

        public T ToObject<T>(string xml, XmlMappingContainer mappings = null)
        {
            var type = typeof(T);
            var xmlValue = this.xmlToXmlValue.ToXmlValue(xml);
            return this.xmlValueToObject.Resolve<T>(xmlValue, mappings);          
        }

    }
}
