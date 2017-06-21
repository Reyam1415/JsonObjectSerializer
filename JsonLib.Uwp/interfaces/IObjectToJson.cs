using JsonLib.Mappings;

namespace JsonLib
{
    public interface IObjectToJson
    {
        string ToJson<T>(T value, MappingContainer mappings = null);
    }
}