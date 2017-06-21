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

        public PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public Type GetNullableTargetType(Type propertyType)
        {
            return Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        }

        public void SetValue(object instance, PropertyInfo property, object convertedValue)
        {
            property.SetValue(instance, convertedValue);
        }

        public void SetValue(object instance, string propertyName, object convertedValue)
        {
            var property = this.GetProperty(instance.GetType(), propertyName);
            if(property == null)
            {
                throw new JsonLibException("No property found for " + propertyName);
            } 
            property.SetValue(instance, convertedValue);
        }

        public object GetValue(object instance, PropertyInfo property)
        {
            return property.GetValue(instance);
        }

        public object GetValue(object instance, string propertyName)
        {
            var property = this.GetProperty(instance.GetType(), propertyName);
            if (property == null)
            {
                throw new JsonLibException("No property found for " + propertyName);
            }
            return this.GetValue(instance, property);
        }

        public object ConvertValueToPropertyType(object value, Type propertyType)
        {
            return value == null ? null : Convert.ChangeType(value, propertyType);
        }

        public object ConvertJsonValueToPropertyValue(PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyType = this.GetNullableTargetType(propertyInfo.PropertyType);
            return this.ConvertValueToPropertyType(jsonValue, propertyType);
        }

        public void ConvertAndSetValueWithNullable(object instance, PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyValue = this.ConvertJsonValueToPropertyValue(propertyInfo, jsonValue);
            this.SetValue(instance, propertyInfo, propertyValue);
        }

        public void ConvertAndSetValue(object instance, PropertyInfo propertyInfo, object jsonValue)
        {
            var propertyValue = this.ConvertValueToPropertyType(jsonValue, propertyInfo.PropertyType);
            this.SetValue(instance, propertyInfo, propertyValue);
        }

        public object GetEnumValue(Type propertyType, object value)
        {
            return Enum.Parse(propertyType, value.ToString());
        }

    }
}
