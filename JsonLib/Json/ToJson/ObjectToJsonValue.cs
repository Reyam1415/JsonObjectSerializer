using JsonLib.Common;
using JsonLib.Json.Mappings;
using System;
using System.Collections;

namespace JsonLib.Json
{
    public class ObjectToJsonValue : IObjectToJsonValue
    {
        protected IAssemblyInfoService assemblyInfoService;

        public ObjectToJsonValue()
            :this(new AssemblyInfoService())
        {  }

        public ObjectToJsonValue(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;
        }

        public string GetStringValueOrNull(object value)
        {
            return value == null ? null : value.ToString();
        }

        public string GetJsonPropertyName(string propertyName, bool lowerCaseStrategyForAllTypes, JsonTypeMapping mapping)
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

        public JsonArray ToJsonArray(IEnumerable array, JsonMappingContainer mappings = null)
        {
            var result = new JsonArray();
            foreach (var value in array)
            {
                var jsonValue = this.ToJsonValue(value.GetType(), value, mappings);
                result.Add(jsonValue);
            }
            return result;
        }

        public JsonObject ToJsonObject(object obj, JsonMappingContainer mappings = null)
        {
            var result = JsonValue.CreateObject();

            var properties = this.assemblyInfoService.GetProperties(obj);

            var objType = obj.GetType();

            bool hasMappings = mappings != null;

            bool allLower = hasMappings && mappings.LowerStrategyForAllTypes;

            JsonTypeMapping mapping = null;
            if (mappings != null && mappings.Has(objType))
            {
                mapping = mappings.Get(objType);
            }

            foreach (var property in properties)
            {
                var jsonPropertyName = hasMappings ? this.GetJsonPropertyName(property.Name, allLower, mapping) : property.Name;
                var propertyValue = property.GetValue(obj);
                var propertyType = property.PropertyType;

                var jsonValue = this.ToJsonValue(propertyType, propertyValue, mappings);
                result.Add(jsonPropertyName, jsonValue);
            }

            return result;
        }

        public IJsonValue ToJsonValue(Type type, object obj, JsonMappingContainer mappings = null)
        {
            if (this.assemblyInfoService.IsSystemType(type))
            {
                if (type == typeof(string))
                {
                    return new JsonString(this.GetStringValueOrNull(obj));
                }
                else if (this.assemblyInfoService.IsNumberType(type))
                {
                    return new JsonNumber(obj);
                }
                else if (type == typeof(bool))
                {
                    return new JsonBool(Convert.ToBoolean(obj));
                }
                else if (type == typeof(DateTime))
                {
                    return new JsonString(obj.ToString());
                }
                else if (type == typeof(Guid))
                {
                    return new JsonString(obj.ToString());
                }
                else if (this.assemblyInfoService.IsNullable(type))
                {
                    return new JsonNullable(obj);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                {
                    // array example string[]
                    return this.ToJsonArray((IEnumerable)obj, mappings);
                }
            }
            else if (this.assemblyInfoService.IsEnum(type))
            {
                return new JsonNumber(Convert.ToInt32(obj));
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

        public IJsonValue ToJsonValue<T>(T obj, JsonMappingContainer mappings = null)
        {
            return this.ToJsonValue(typeof(T), obj, mappings);
        }
    }
}
