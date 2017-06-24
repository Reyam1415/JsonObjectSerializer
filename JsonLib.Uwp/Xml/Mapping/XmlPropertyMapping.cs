namespace JsonLib.Mappings.Xml
{
    public class XmlPropertyMapping
    {
        public string PropertyName { get; }
        public string XmlPropertyName { get; }

        public XmlPropertyMapping(string propertyName, string xmlPropertyName)
        {
            this.PropertyName = propertyName;
            this.XmlPropertyName = xmlPropertyName;
        }
    }
}
