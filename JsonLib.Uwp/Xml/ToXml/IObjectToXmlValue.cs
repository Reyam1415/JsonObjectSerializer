using JsonLib.Mappings.Xml;
using JsonLib.Xml;

namespace JsonLib.Xml
{
    public interface IObjectToXmlValue
    {
        IXmlValue ToXmlValue<T>(T obj, XmlMappingContainer mappings = null);
    }
}