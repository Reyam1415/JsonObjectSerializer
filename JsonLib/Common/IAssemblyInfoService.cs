using System;
using System.Collections;
using System.Reflection;

namespace JsonLib.Common
{
    public interface IAssemblyInfoService
    {
        string ConvertToStringWithInvariantCulture(object value);
        object ConvertValueToPropertyType(object value, Type propertyType);
        Array CreateArray(Type singleItemType, int length);
        object CreateInstance(Type type);
        IList CreateList(Type singleItemType);
        string GetAssemblyQualitiedName(Type type);
        Type GetDictionaryKeyType(Type type);
        Type GetDictionaryValueType(Type type);
        object GetEnumValue(Type propertyType, object value);
        Type GetNullableUnderlyingType(Type propertyType);
        PropertyInfo[] GetProperties(object obj);
        PropertyInfo GetProperty(Type type, string name);
        Type GetSingleItemType(Type type);
        Type GetTypeFromAssemblyQualifiedName(string assemblyQualifiedName);
        object GetValue(object instance, PropertyInfo property);
        object GetValue(object instance, string propertyName);
        bool IsArray(Type type);
        bool IsBaseType(Type type);
        bool IsDictionary(Type type);
        bool IsEnum(Type type);
        bool IsGenericType(Type type);
        bool IsNullable(Type type);
        bool IsNumber(object value);
        bool IsNumberType(Type type);
        bool IsSystemType(Type type);
        void SetValue(object instance, PropertyInfo property, object convertedValue);
        void SetValue(object instance, string propertyName, object convertedValue);
    }
}