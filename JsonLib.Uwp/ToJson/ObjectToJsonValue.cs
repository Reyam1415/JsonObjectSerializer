using JsonLib.Mappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLib
{

    public class ObjectToJsonValue : IObjectToJsonValue
    {
        protected IAssemblyInfoService assemblyInfoService;

        internal List<Type> numericTypes;

        public ObjectToJsonValue()
            :this(new AssemblyInfoService())
        {  }

        public ObjectToJsonValue(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;

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

        public bool IsNumber(Type type)
        {
            foreach (var numericType in numericTypes)
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
            return this.IsNumber(value.GetType());
        }

        public bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public string GetStringValueOrNull(object value)
        {
            return value == null ? null : value.ToString();
        }

        public string GetJsonPropertyName(string propertyName, bool lowerCaseStrategyForAllTypes, TypeMapping mapping)
        {
            if (lowerCaseStrategyForAllTypes)
            {
                return propertyName.ToLower();
            }
            else if (mapping != null)
            {
                if (mapping.LowerCaseStrategy)
                {
                    return propertyName.ToLower();
                }
                else if (mapping.HasByPropertyName(propertyName))
                {
                    return mapping.Properties[propertyName].JsonName;
                }
            }
            return propertyName;
        }

        public JsonElementArray ToJsonArray(IEnumerable array, MappingContainer mappings = null)
        {
            var result = new JsonElementArray();
            foreach (var value in array)
            {
                var jsonValue = this.ToJsonValue(value.GetType(), value, mappings);
                result.Add(jsonValue);
            }
            return result;
        }

        public JsonElementObject ToJsonObject(object obj, MappingContainer mappings = null)
        {
            var result = JsonElementValue.CreateObject();

            var properties = this.assemblyInfoService.GetProperties(obj);

            var objType = obj.GetType();

            bool hasMappings = mappings != null;

            bool allLower = hasMappings && mappings.LowerStrategyForAllTypes;

            TypeMapping mapping = null;
            if (mappings != null && mappings.Has(objType))
            {
                mapping = mappings.Get(objType);
            }

            foreach (var property in properties)
            {
                var jsonPropertyName = hasMappings ? this.GetJsonPropertyName(property.Name, allLower, mapping): property.Name;
                var propertyValue = property.GetValue(obj);
                var propertyType = property.PropertyType;

                var jsonValue = this.ToJsonValue(propertyType, propertyValue, mappings);
                result.Add(jsonPropertyName, jsonValue);
            }

            return result;
        }

        public IJsonElementValue ToJsonValue(Type type, object obj, MappingContainer mappings = null)
        {
            if (this.IsSystemType(type))
            {
                if (type == typeof(string))
                {
                    return new JsonElementString(this.GetStringValueOrNull(obj));
                }
                else if (this.IsNumber(type))
                {
                    return new JsonElementNumber(obj);
                }
                else if (type == typeof(bool))
                {
                    return new JsonElementBool(Convert.ToBoolean(obj));
                }
                else if (type == typeof(DateTime))
                {
                    return new JsonElementString(obj.ToString());
                }
                else if (type == typeof(Guid))
                {
                    return new JsonElementString(obj.ToString());
                }
                else if (this.IsNullable(type))
                {
                    return new JsonElementNullable(obj);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                {
                    // array example string[]
                    return this.ToJsonArray((IEnumerable)obj, mappings);
                }
            }
            else if (this.IsEnum(type))
            {
                return new JsonElementNumber(Convert.ToInt32(obj));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
            {
                return this.ToJsonArray((IEnumerable)obj, mappings);
            }
            else
            {
                return this.ToJsonObject(obj, mappings);
            }

            throw new JsonLibException("Cannot resolve object");
        }

        public IJsonElementValue ToJsonValue<T>(T obj, MappingContainer mappings = null)
        {
            return this.ToJsonValue(typeof(T), obj, mappings);
        }
    }
}
