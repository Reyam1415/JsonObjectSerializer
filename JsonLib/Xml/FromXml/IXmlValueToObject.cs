using JsonLib.Mappings.Xml;
using JsonLib.Xml;

namespace JsonLib.Xml
{
    public interface IXmlValueToObject
    {
        T Resolve<T>(IXmlValue xmlValue, XmlMappingContainer mappings = null);
    }
}