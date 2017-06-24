using JsonLib.Mappings.Xml;

namespace JsonLib.Xml
{
    public interface IXmlToObject
    {
        T ToObject<T>(string xml, XmlMappingContainer mappings = null);
    }
}