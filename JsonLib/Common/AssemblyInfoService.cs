using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLib.Common
{
    public class AssemblyInfoService : IAssemblyInfoService
    {
        internal List<Type> numericTypes;

        public AssemblyInfoService()
        {
            this.numericTypes = new List<Type>
                {
                    typeof(Int32),
                    typeof(Byte),
                    typeof(SByte),
                    typeof(UInt16),
                    typeof(UInt32),
                    typeof(UInt64),
                    typeof(Int16),
                    typeof(Int64),
                    typeof(Decimal),
                    typeof(Double),
                    typeof(Single)
                };
        }

        public bool IsNumberType(Type type)
        {
            foreach (var numericType in this.numericTypes)
            {
                if (numericType == type)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsNumber(object value)
        {
            return this.IsNumberType(value.GetType());
        }

        public bool IsSystemType(Type type)
        {
            return type.Namespace == "System";
        }

        public bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public bool IsArray(Type type)
        {
            return type.IsArray;
        }

        public Type GetSingleItemType(Type type)
        {
            if (this.IsArray(type))
            {
                return type.GetTypeInfo().GetElementType();
            }
            else if (this.IsGenericType(type))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }

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
