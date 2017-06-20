using JsonLib.Mappings;

namespace JsonLib
{
    public interface IJsonObjectSerializerService
    {
        T Parse<T>(string json, MappingContainer mappings = null);
        string Stringify(object value, MappingContainer mappings = null);
        string StringifyAndBeautify(object value, MappingContainer mappings = null);
    }
}