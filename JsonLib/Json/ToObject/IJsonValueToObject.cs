using System;
using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IJsonValueToObject
    {
        object ToEnumerable(Type type, JsonArray jsonArrayValue, JsonMappingContainer mappings = null);
        object ToList(Type type, JsonArray jsonArrayValue, JsonMappingContainer mappings = null);
        object ToObject(Type objType, JsonObject jsonObjectValue, JsonMappingContainer mappings = null);
        object ToValue(Type propertyType, JsonBool jsonValue);
        object ToValue(Type propertyType, JsonNullable jsonValue);
        object ToValue(Type propertyType, JsonNumber jsonValue);
        object ToValue(Type propertyType, JsonString jsonValue);
    }
}