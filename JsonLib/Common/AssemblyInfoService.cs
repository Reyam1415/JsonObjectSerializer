using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        public bool IsSystemType(Type type)
        {
            return type.Namespace == "System";
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

        public bool IsBaseType(Type type)
        {
            return type.FullName == "System.Type";
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

        public bool IsDictionary(Type type)
        {
            return this.IsGenericType(type) && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
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

        public string GetAssemblyQualifiedName(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        public Type GetTypeFromAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return Type.GetType(assemblyQualifiedName);
        }

        public Type GetDictionaryKeyType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public Type GetDictionaryValueType(Type type)
        {
            return type.GetGenericArguments()[1];
        }

        public string ConvertToStringWithInvariantCulture(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public IList CreateList(Type singleItemType)
        {
            var listType = typeof(List<>).MakeGenericType(singleItemType);
            return this.CreateInstance(listType) as IList;
        }

        public Array CreateArray(Type singleItemType, int length)
        {
            return Array.CreateInstance(singleItemType, length);
        }

        public PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        public Type GetNullableUnderlyingType(Type propertyType)
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
            if(property == null) { throw new JsonLibException("No property found for " + propertyName); } 

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

        public object GetEnumValue(Type propertyType, object value)
        {
            return Enum.Parse(propertyType, value.ToString());
        }

    }
}
