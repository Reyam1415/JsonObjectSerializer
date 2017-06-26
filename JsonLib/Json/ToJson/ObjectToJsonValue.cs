using JsonLib.Common;
using JsonLib.Json.Mappings;
using System;
using System.Collections;
using System.Globalization;

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

        public string ResolveDictionaryKey(Type type, object key)
        {
            if (type == typeof(string) || type == typeof(Guid))
            {
                return key.ToString();
            }
            else if (type.FullName == "System.Type")
            {
                return ((Type)key).AssemblyQualifiedName;
            }
            else if (this.assemblyInfoService.IsNumberType(type))
            {
                return Convert.ToString(key, CultureInfo.InvariantCulture);
            }

            throw new JsonLibException("Unsupported type for dictionary key");
        }

        public JsonObject DoToJsonObjectFromDictionary(Type type, Type keyType, Type valueType, IDictionary dictionary, JsonMappingContainer mappings = null)
        {
            var result = new JsonObject();

            foreach (var keyValue in dictionary)
            {
                var entry = (DictionaryEntry)keyValue;

                var key = this.ResolveDictionaryKey(keyType, entry.Key);

                var jsonValue = this.ToJsonValue(valueType, entry.Value, mappings);

                result.Add(key, jsonValue);
            }

            return result;
        }

        public JsonObject ToJsonObjectFromDictionary(Type type, IDictionary dictionary, JsonMappingContainer mappings = null)
        {
            var keyType = this.assemblyInfoService.GetDictionaryKeyType(type);
            var valueType = this.assemblyInfoService.GeDictionaryValueType(type);

            var nullableType = Nullable.GetUnderlyingType(valueType);
            if (nullableType != null)
            {
                return this.DoToJsonObjectFromDictionary(type, keyType, nullableType, dictionary, mappings);
            }
            else
            {
                return this.DoToJsonObjectFromDictionary(type, keyType, valueType, dictionary, mappings);
            }
        }

        public IJsonValue ToJsonValue(Type type, object value, JsonMappingContainer mappings = null)
        {
            if (this.assemblyInfoService.IsSystemType(type))
            {
                if (type == typeof(string))
                {
                    return new JsonString(this.GetStringValueOrNull(value));
                }
                else if (this.assemblyInfoService.IsNumberType(type))
                {
                    return new JsonNumber(value);
                }
                else if (type == typeof(bool))
                {
                    return new JsonBool(Convert.ToBoolean(value));
                }
                else if (type == typeof(DateTime))
                {
                    return new JsonString(value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    return new JsonString(value.ToString());
                }
                else if (this.assemblyInfoService.IsNullable(type))
                {
                    return new JsonNullable(value);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    // array example string[]
                    return this.ToJsonArray((IEnumerable)value, mappings);
                }
            }
            else if (this.assemblyInfoService.IsEnum(type))
            {
                return new JsonNumber(Convert.ToInt32(value));
            }
            else if (this.assemblyInfoService.IsDictionary(type))
            {
                return this.ToJsonObjectFromDictionary(type, (IDictionary)value, mappings);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                return this.ToJsonArray((IEnumerable)value, mappings);
            }
            else
            {
                return this.ToJsonObject(value, mappings);
            }

            throw new JsonLibException("Cannot resolve object");
        }

        public IJsonValue ToJsonValue<T>(T obj, JsonMappingContainer mappings = null)
        {
            return this.ToJsonValue(typeof(T), obj, mappings);
        }
    }
}
