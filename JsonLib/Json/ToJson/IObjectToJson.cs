using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IObjectToJson
    {
        string ToJson<T>(T value, JsonMappingContainer mappings = null);
    }
}