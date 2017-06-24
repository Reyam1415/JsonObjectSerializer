﻿using JsonLib.Common;
using JsonLib.Json.Mappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLib.Json
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

        protected object ResolveValue(Type propertyType, JsonString jsonValue)
        {
            if (propertyType == typeof(Guid))
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

        protected object ResolveValue(Type propertyType, JsonNumber jsonValue)
        {
            if (this.assemblyInfoService.IsEnum(propertyType))
            {
                return Enum.Parse(propertyType, jsonValue.Value.ToString());
            }
            else
            {
                var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
                return this.assemblyInfoService.ConvertValueToPropertyType(jsonValue.Value, type);
            }
        }

        public object ToValue(Type propertyType, JsonString jsonValue)
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

        public object ToValue(Type propertyType, JsonNumber jsonValue)
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

        public object ToValue(Type propertyType, JsonBool jsonValue)
        {
            return jsonValue.Value;
        }

        public object ToValue(Type propertyType, JsonNullable jsonValue)
        {
            var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
            return this.assemblyInfoService.ConvertValueToPropertyType(jsonValue.Value, type);
        }


        public PropertyInfo FindProperty(PropertyInfo[] properties, string jsonName, bool lowerCaseStrategyForAllTypes, JsonTypeMapping mapping = null)
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

        public object ResolveValue(Type propertyType, IJsonValue jsonValue, JsonMappingContainer mappings = null)
        {
            if (jsonValue.ValueType == JsonValueType.String)
            {
                return this.ToValue(propertyType, (JsonString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Number)
            {
                return this.ToValue(propertyType, (JsonNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Bool)
            {
                return this.ToValue(propertyType, (JsonBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Nullable)
            {
                return this.ToValue(propertyType, (JsonNullable)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Object)
            {
                return this.ToObject(propertyType, (JsonObject)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonValueType.Array)
            {
                return this.ToEnumerable(propertyType, (JsonArray)jsonValue, mappings);
            }

            throw new JsonLibException("Cannot resolve Value");
        }

        public object ToObject(Type objType, JsonObject jsonObject, JsonMappingContainer mappings = null)
        {
            var instance = this.assemblyInfoService.CreateInstance(objType);
            var properties = this.assemblyInfoService.GetProperties(instance);

            bool hasMappings = mappings != null;

            bool allLower = hasMappings && mappings.LowerStrategyForAllTypes;

            JsonTypeMapping mapping = null;
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

        public object ToList(Type type, JsonArray jsonArray, JsonMappingContainer mappings = null)
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

        public object ToArray(Type type, JsonArray jsonArray, JsonMappingContainer mappings = null)
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

        public object ToEnumerable(Type type, JsonArray jsonArray, JsonMappingContainer mappings = null)
        {
            if (this.assemblyInfoService.IsArray(type))
            {
                return this.ToArray(type, jsonArray, mappings);
            }
            else if (this.assemblyInfoService.IsGenericType(type))
            {
               return this.ToList(type, jsonArray, mappings);
            }

            return null;
        }
    }
}