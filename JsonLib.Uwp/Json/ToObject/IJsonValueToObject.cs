using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IJsonValueToObject
    {
        T Resolve<T>(IJsonValue jsonValue, JsonMappingContainer mappings = null);
    }
}