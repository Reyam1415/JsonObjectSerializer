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

        public bool IsArray(Type type)
        {
            return type.IsArray;
        }

        public bool IsGeneric(Type type)
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

        public JsonValueToObject(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;
        }

        public object ToString(Type propertyType, JsonElementString jsonValue)
        {
            if (this.IsGuid(propertyType))
            {
                return new Guid(jsonValue.Value);
            }
            else
            {
                return jsonValue.Value;
            }
        }

        public object ToNumber(Type propertyType, JsonElementNumber jsonValue)
        {
            var type = this.assemblyInfoService.ResolvePropertyType(propertyType);
            return this.assemblyInfoService.GetConvertedValue(jsonValue.Value, type);
        }

        public object ToBool(Type propertyType, JsonElementBool jsonValue)
        {
            var type = this.assemblyInfoService.ResolvePropertyType(propertyType);
            return this.assemblyInfoService.GetConvertedValue(jsonValue.Value, type);
        }

        public object ToNullable(Type propertyType, JsonElementNullable jsonValue)
        {
            var type = this.assemblyInfoService.ResolvePropertyType(propertyType);
            return this.assemblyInfoService.GetConvertedValue(jsonValue.Value, type);
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

        public object ToObject(Type objType, JsonElementObject jsonObjectValue, MappingContainer mappings = null)
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

            foreach (var jsonValue in jsonObjectValue.Values)
            {
                var property = hasMappings ? this.FindProperty(properties, jsonValue.Key, allLower, mapping) 
                    : this.FindProperty(properties, jsonValue.Key);
                if (property != null)
                {
                    if (jsonValue.Value.ValueType == JsonElementValueType.String)
                    {
                        if (this.IsGuid(property.PropertyType))
                        {
                            this.assemblyInfoService.SetValue(instance, property, new Guid(((JsonElementString)jsonValue.Value).Value));
                        }
                        else
                        {
                            this.assemblyInfoService.ConvertAndSetValue(instance, property, ((JsonElementString)jsonValue.Value).Value);
                        }
                    }
                    else if (jsonValue.Value.ValueType == JsonElementValueType.Number)
                    {
                        if (this.IsEnum(property.PropertyType))
                        {
                            var enumValue = this.assemblyInfoService.GetEnumValue(property.PropertyType, ((JsonElementNumber)jsonValue.Value).Value);
                            this.assemblyInfoService.SetValue(instance, property, enumValue);
                        }
                        else
                        {
                            this.assemblyInfoService.ConvertAndSetValueWithNullable(instance, property, ((JsonElementNumber)jsonValue.Value).Value);
                        }
                    }
                    else if (jsonValue.Value.ValueType == JsonElementValueType.Bool)
                    {
                        this.assemblyInfoService.SetValue(instance, property, ((JsonElementBool)jsonValue.Value).Value);
                    }
                    else if (jsonValue.Value.ValueType == JsonElementValueType.Null)
                    {
                        this.assemblyInfoService.SetValue(instance, property, null);
                    }
                    else if (jsonValue.Value.ValueType == JsonElementValueType.Object)
                    {
                        var inner = this.ToObject(property.PropertyType, (JsonElementObject)jsonValue.Value, mappings);
                        this.assemblyInfoService.ConvertAndSetValue(instance, property,inner);
                    }
                    else if (jsonValue.Value.ValueType == JsonElementValueType.Array)
                    {
                        var inner = this.ToEnumerable(property.PropertyType, (JsonElementArray)jsonValue.Value, mappings);
                        this.assemblyInfoService.ConvertAndSetValue(instance, property, inner);
                    }
                }
            }

            return instance;
        }

        public object ToList(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null)
        {
            var singleItemType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(singleItemType);
            var result = this.assemblyInfoService.CreateInstance(listType) as IList;

            foreach (var jsonValue in jsonArrayValue.Values)
            {
                if (jsonValue.ValueType == JsonElementValueType.String)
                {
                    result.Add(((JsonElementString)jsonValue).Value);
                }
                else if (jsonValue.ValueType == JsonElementValueType.Number)
                {
                    var numberValue = this.assemblyInfoService.GetConvertedValue(((JsonElementNumber)jsonValue).Value, singleItemType);
                    result.Add(numberValue);
                }
                else if (jsonValue.ValueType == JsonElementValueType.Bool)
                {
                    result.Add(((JsonElementBool)jsonValue).Value);
                }
                else if (jsonValue.ValueType == JsonElementValueType.Null)
                {
                    var nullableValue = this.assemblyInfoService.GetConvertedValue(((JsonElementNullable)jsonValue).Value, singleItemType);
                    result.Add(nullableValue);
                }
                else if (jsonValue.ValueType == JsonElementValueType.Object)
                {
                    var inner = this.ToObject(singleItemType, (JsonElementObject)jsonValue, mappings);
                    result.Add(inner);
                }
                else if (jsonValue.ValueType == JsonElementValueType.Array)
                {
                    var inner = this.ToEnumerable(singleItemType, (JsonElementArray)jsonValue, mappings);
                    result.Add(inner);
                }
            }
            return result;
        }

        public object ToArray(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null)
        {
            var singleItemType = type.GetTypeInfo().GetElementType();
            var result = Array.CreateInstance(singleItemType, jsonArrayValue.Values.Count);
            int index = 0;

            foreach (var jsonValue in jsonArrayValue.Values)
            {
                if (jsonValue.ValueType == JsonElementValueType.String)
                {
                    result.SetValue(((JsonElementString)jsonValue).Value, index);
                    index++;
                }
                else if (jsonValue.ValueType == JsonElementValueType.Number)
                {
                    var numberValue = this.assemblyInfoService.GetConvertedValue(((JsonElementNumber)jsonValue).Value, singleItemType);
                    result.SetValue(numberValue, index);
                    index++;
                }
                else if (jsonValue.ValueType == JsonElementValueType.Bool)
                {
                    result.SetValue(((JsonElementBool)jsonValue).Value, index);
                    index++;
                }
                else if (jsonValue.ValueType == JsonElementValueType.Null)
                {
                    var nullableValue = this.assemblyInfoService.GetConvertedValue(((JsonElementNullable)jsonValue).Value, singleItemType);
                    result.SetValue(nullableValue, index);
                    index++;
                }
                else if (jsonValue.ValueType == JsonElementValueType.Object)
                {
                    var inner = this.ToObject(singleItemType, (JsonElementObject)jsonValue, mappings);
                    result.SetValue(inner, index);
                    index++;
                }
                else if (jsonValue.ValueType == JsonElementValueType.Array)
                {
                    var inner = this.ToEnumerable(singleItemType, (JsonElementArray)jsonValue, mappings);
                    result.SetValue(inner, index);
                    index++;
                }
            }
            return result;
        }

        public object ToEnumerable(Type type, JsonElementArray jsonArrayValue, MappingContainer mappings = null)
        {
            if (this.IsArray(type))
            {
                return this.ToArray(type, jsonArrayValue, mappings);
            }
            else if (this.IsGeneric(type))
            {
               return this.ToList(type, jsonArrayValue, mappings);
            }

            return null;
        }
    }
}
