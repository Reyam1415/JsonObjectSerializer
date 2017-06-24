using JsonLib.Json.Mappings;
using JsonLib.Mappings.Xml;

namespace JsonLib
{
    public interface IJsonObjectSerializerService
    {
        bool CacheIsActive { get; }

        void ActiveCache(bool value = true);
        T FromXml<T>(string xml, XmlMappingContainer mappings = null);
        T Parse<T>(string json, JsonMappingContainer mappings = null);
        string Stringify<T>(T value, JsonMappingContainer mappings = null);
        string StringifyAndBeautify<T>(T value, JsonMappingContainer mappings = null);
        string ToXml<T>(T value, XmlMappingContainer mappings = null);
        string ToXmlAndBeautify<T>(T value, XmlMappingContainer mappings = null);
    }
}