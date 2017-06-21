using JsonLib.Mappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLib
{
    public class JsonValueToObject : IJsonValueToObject
    {
        protected IAssemblyInfoService assemblyInfoService;

        public JsonValueToObject()
            :this(new AssemblyInfoService())
        { }

        public JsonValueToObject(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;
        }

        public bool IsArray(Type type)
        {
            return type.IsArray;
        }

        public bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public bool IsGuid(Type type)
        {
            return type == typeof(Guid);
        }

        protected object ResolveValue(Type propertyType, JsonElementString jsonValue)
        {
            if (this.IsGuid(propertyType))
            {
                return new Guid(jsonValue.Value);
            }
            else if (propertyType == typeof(DateTime))
            {
                return DateTime.Parse(jsonValue.Value);
            }
            else
            {
                return jsonValue.Value;
            }
        }

        protected object ResolveValue(Type propertyType, JsonElementNumber jsonValue)
        {
            if (this.IsEnum(propertyType))
            {
                return Enum.Parse(propertyType, jsonValue.Value.ToString());
            }
            else
            {
                var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
                return this.assemblyInfoService.ConvertValueToPropertyType(jsonValue.Value, type);
            }
        }

        public object ToValue(Type propertyType, JsonElementString jsonValue)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if(nullableType != null)
            {
                // is nullable
                return jsonValue.Value == null ? null : this.ResolveValue(nullableType, jsonValue);
            }
            else
            {
                return this.ResolveValue(propertyType, jsonValue);
            }
        }

        public object ToValue(Type propertyType, JsonElementNumber jsonValue)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                // is nullable
                return jsonValue.Value == null ? null : this.ResolveValue(nullableType, jsonValue);
            }
            else
            {
                return this.ResolveValue(propertyType, jsonValue);
            }         
        }

        public object ToValue(Type propertyType, JsonElementBool jsonValue)
        {
            return jsonValue.Value;
        }

        public object ToValue(Type propertyType, JsonElementNullable jsonValue)
        {
            var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
            return this.assemblyInfoService.ConvertValueToPropertyType(jsonValue.Value, type);
        }


        public PropertyInfo FindProperty(PropertyInfo[] properties, string jsonName, bool lowerCaseStrategyForAllTypes, TypeMapping mapping = null)
        {
            var propertyName = jsonName;
            var toLower = lowerCaseStrategyForAllTypes || (mapping != null && mapping.LowerCaseStrategy);
            if (toLower)
            {
                propertyName = propertyName.ToLower();
            }
            else if(mapping != null && mapping.HasByJsonName(jsonName))
            {
                propertyName = mapping.GetByJsonName(jsonName).PropertyName;
            }

            foreach (var property in properties)
            {
                var currentPropertyName = toLower ? property.Name.ToLower() : property.Name;
                if (currentPropertyName == propertyName)
                {
                    return property;
                }
            }
            return null;
        }

        public PropertyInfo FindProperty(PropertyInfo[] properties, string jsonName)
        {
            foreach (var property in properties)
            {
                if (property.Name == jsonName)
                {
                    return property;
                }
            }
            return null;
        }

        public object ResolveValue(Type propertyType, IJsonElementValue jsonValue, MappingContainer mappings = null)
        {
            if (jsonValue.ValueType == JsonElementValueType.String)
            {
                return this.ToValue(propertyType, (JsonElementString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Number)
            {
                return this.ToValue(propertyType, (JsonElementNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Bool)
            {
                return this.ToValue(propertyType, (JsonElementBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Null)
            {
                return this.ToValue(propertyType, (JsonElementNullable)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Object)
            {
                return this.ToObject(propertyType, (JsonElementObject)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Array)
            {
                return this.ToEnumerable(propertyType, (JsonElementArray)jsonValue, mappings);
            }

            throw new JsonLibException("Cannot resolve Value");
        }

        public object ToObject(Type objType, JsonElementObject jsonObject, MappingContainer mappings = null)
        {
            var instance = this.assemblyInfoService.CreateInstance(objType);
            var properties = this.assemblyInfoService.GetProperties(instance);

            bool hasMappings = mappings != null;

            bool allLower = hasMappings && mappings.LowerStrategyForAllTypes;

            TypeMapping mapping = null;
            if (hasMappings && mappings.Has(objType))
            {
                mapping = mappings.Get(objType);
            }

            foreach (var jsonValue in jsonObject.Values)
            {
                var property = hasMappings ? this.FindProperty(properties, jsonValue.Key, allLower, mapping) 
                    : this.FindProperty(properties, jsonValue.Key);
                if (property != null)
                {
                    var value = this.ResolveValue(property.PropertyType, jsonValue.Value, mappings);
                    this.assemblyInfoService.SetValue(instance, property, value);
                }
            }

            return instance;
        }

        public object ToList(Type type, JsonElementArray jsonArray, MappingContainer mappings = null)
        {
            var singleItemType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(singleItemType);
            var result = this.assemblyInfoService.CreateInstance(listType) as IList;

            foreach (var jsonValue in jsonArray.Values)
            {
                var value = this.ResolveValue(singleItemType, jsonValue, mappings);
                result.Add(value);
            }
            return result;
        }

        public object ToArray(Type type, JsonElementArray jsonArray, MappingContainer mappings = null)
        {
            var singleItemType = type.GetTypeInfo().GetElementType();
            var result = Array.CreateInstance(singleItemType, jsonArray.Values.Count);
            int index = 0;

            foreach (var jsonValue in jsonArray.Values)
            {
                var value = this.ResolveValue(singleItemType, jsonValue, mappings);
                result.SetValue(value, index);
                index++;
            }
            return result;
        }

        public object ToEnumerable(Type type, JsonElementArray jsonArray, MappingContainer mappings = null)
        {
            if (this.IsArray(type))
            {
                return this.ToArray(type, jsonArray, mappings);
            }
            else if (this.IsGenericType(type))
            {
               return this.ToList(type, jsonArray, mappings);
            }

            return null;
        }
    }
}
