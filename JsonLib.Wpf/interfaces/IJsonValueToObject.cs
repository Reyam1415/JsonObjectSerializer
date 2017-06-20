using System;
using JsonLib.Mappings;

namespace JsonLib
{
    public interface IJsonValueToObject
    {
        object ToArray(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null);
        object ToBool(Type propertyType, JsonElementBool jsonValue);
        object ToEnumerable(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null);
        object ToList(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null);
        object ToNullable(Type propertyType, JsonElementNullable jsonValue);
        object ToNumber(Type propertyType, JsonElementNumber jsonValue);
        object ToObject(Type objType, JsonElementObject jsonObjectValue, MappingContainer mappings = null);
        object ToString(Type propertyType, JsonElementString jsonValue);
    }
}