using JsonLib.Mappings;

namespace JsonLib
{
    public interface IObjectToJson
    {
        string ToJson(object value, MappingContainer mappings = null);
    }
}