using System;
using System.Collections;
using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public interface IObjectToJsonValue
    {
        IJsonValue ToJsonValue<T>(T obj, JsonMappingContainer mappings = null);
    }
}