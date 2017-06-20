using System;
using System.Reflection;

namespace JsonLib
{
    public class AssemblyInfoService : IAssemblyInfoService
    {
        public PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        public PropertyInfo GetPropertyInfo(Type type, string name)
        {
            return type.GetProperty(name);
        }

        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public Type ResolvePropertyType(Type propertyType)
        {
            return Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        }

        public object GetConvertedValue(object jsonValue, Type propertyType)
        {
            return jsonValue == null ? null : Convert.ChangeType(jsonValue, propertyType);
        }

        public object ConvertJsonValueToPropertyValue(PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyType = this.ResolvePropertyType(propertyInfo.PropertyType);
            return this.GetConvertedValue(jsonValue, propertyType);
        }
        public object GetEnumValue(Type propertyType, object value)
        {
           return Enum.Parse(propertyType, value.ToString());
        }

        public void SetValue(object instance, PropertyInfo propertyInfo, object convertedValue)
        {
            propertyInfo.SetValue(instance, convertedValue);
        }

        public object GetValue(object instance, PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue(instance);
        }

        public void ConvertAndSetValueWithNullable(object instance, PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyValue = this.ConvertJsonValueToPropertyValue(propertyInfo, jsonValue);
            this.SetValue(instance, propertyInfo, propertyValue);
        }

        public void ConvertAndSetValue(object instance, PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyValue = this.GetConvertedValue(jsonValue, propertyInfo.PropertyType);
            this.SetValue(instance, propertyInfo, propertyValue);
        }
    }
}
