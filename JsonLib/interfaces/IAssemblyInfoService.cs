using System;
using System.Reflection;

namespace JsonLib
{
    public interface IAssemblyInfoService
    {
        void ConvertAndSetValue(object instance, PropertyInfo propertyInfo, object jsonValue);
        void ConvertAndSetValueWithNullable(object instance, PropertyInfo propertyInfo, object jsonValue);
        object ConvertJsonValueToPropertyValue(PropertyInfo propertyInfo, object jsonValue);
        object CreateInstance(Type type);
        object GetConvertedValue(object jsonValue, Type propertyType);
        object GetEnumValue(Type propertyType, object value);
        PropertyInfo[] GetProperties(object obj);
        PropertyInfo GetPropertyInfo(Type type, string name);
        object GetValue(object instance, PropertyInfo propertyInfo);
        Type ResolvePropertyType(Type propertyType);
        void SetValue(object instance, PropertyInfo propertyInfo, object convertedValue);
    }
}