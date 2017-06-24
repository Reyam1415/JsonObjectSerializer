using System;
using System.Collections;
using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IObjectToJsonValue
    {
        JsonArray ToJsonArray(IEnumerable array, JsonMappingContainer mappings = null);
        JsonObject ToJsonObject(object obj, JsonMappingContainer mappings = null);
        IJsonValue ToJsonValue(Type type, object obj, JsonMappingContainer mappings = null);
        IJsonValue ToJsonValue<T>(T obj, JsonMappingContainer mappings = null);
    }
}