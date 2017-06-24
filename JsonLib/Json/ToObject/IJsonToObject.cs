using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IJsonToObject
    {
        T ToObject<T>(string json, JsonMappingContainer mappings = null);
    }
}