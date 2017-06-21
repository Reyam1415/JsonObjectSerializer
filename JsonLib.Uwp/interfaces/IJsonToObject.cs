using JsonLib.Mappings;

namespace JsonLib
{
    public interface IJsonToObject
    {
        T ToObject<T>(string json, MappingContainer mappings = null);
    }
}