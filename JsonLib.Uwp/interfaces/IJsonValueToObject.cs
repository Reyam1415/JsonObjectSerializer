using System;
using JsonLib.Mappings;

namespace JsonLib
{
    public interface IJsonValueToObject
    {
        object ToEnumerable(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null);
        object ToList(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null);
        object ToObject(Type objType, JsonElementObject jsonObjectValue, MappingContainer mappings = null);
        object ToValue(Type propertyType, JsonElementBool jsonValue);
        object ToValue(Type propertyType, JsonElementNullable jsonValue);
        object ToValue(Type propertyType, JsonElementNumber jsonValue);
        object ToValue(Type propertyType, JsonElementString jsonValue);
    }
}