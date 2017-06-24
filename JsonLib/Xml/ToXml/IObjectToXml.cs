using JsonLib.Mappings.Xml;

namespace JsonLib.Xml
{
    public interface IObjectToXml
    {
        string ToXml<T>(T value, XmlMappingContainer mappings = null);
    }
}