using System;
using System.Reflection;

namespace JsonLib.Common
{
    public interface IAssemblyInfoService
    {
        void ConvertAndSetValue(object instance, PropertyInfo propertyInfo, object jsonValue);
        void ConvertAndSetValueWithNullable(object instance, PropertyInfo propertyInfo, object jsonValue);
        object ConvertJsonValueToPropertyValue(PropertyInfo propertyInfo, object jsonValue);
        object ConvertValueToPropertyType(object value, Type propertyType);
        object CreateInstance(Type type);
        object GetEnumValue(Type propertyType, object value);
        Type GetNullableTargetType(Type propertyType);
        PropertyInfo[] GetProperties(object obj);
        PropertyInfo GetProperty(Type type, string name);
        Type GetSingleItemType(Type type);
        object GetValue(object instance, PropertyInfo property);
        object GetValue(object instance, string propertyName);
        bool IsArray(Type type);
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