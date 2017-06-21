using System;
using System.Collections;
using JsonLib.Mappings;

namespace JsonLib
{
    public interface IObjectToJsonValue
    {
        string GetJsonPropertyName(string propertyName, bool lowerCaseStrategyForAllTypes, TypeMapping mapping);
        string GetStringValueOrNull(object value);
        bool IsEnum(Type type);
        bool IsGenericType(Type type);
        bool IsNullable(Type type);
        bool IsNumber(object value);
        bool IsNumber(Type type);
        bool IsSystemType(Type type);
        JsonElementArray ToJsonArray(IEnumerable array, MappingContainer mappings = null);
        JsonElementObject ToJsonObject(object obj, MappingContainer mappings = null);
        IJsonElementValue ToJsonValue(Type type, object obj, MappingContainer mappings = null);
        IJsonElementValue ToJsonValue<T>(T obj, MappingContainer mappings = null);
    }
}