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
            return IsNumber(value.GetType());
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

        public bool IsSystemType(Type type)
        {
            return type.Namespace == "System";
        }

        public JsonElementArray ToJsonArray(IEnumerable array, MappingContainer mappings = null)
        {
            var result = new JsonElementArray();
            foreach (var value in array)
            {
                var type = value.GetType();
                if (this.IsSystemType(type))
                {
                    if (type == typeof(string) || type == typeof(DateTime))
                    {
                        result.AddString((string)value);
                    }
                    else if (this.IsNumber(type))
                    {
                        result.AddNumber(value);
                    }
                    else if (type == typeof(bool))
                    {
                        result.AddBool((bool)value);
                    }
                    else if (this.IsNullable(type))
                    {
                        result.AddNullable(value);
                    }
                    else if (type == typeof(Guid))
                    {
                        result.AddString(value.ToString());
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        result.AddArray(this.ToJsonArray((IEnumerable)value, mappings));
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    result.AddArray(this.ToJsonArray((IEnumerable)value, mappings));
                }
                else
                {
                    result.AddObject(this.ToJsonObject(value, mappings));
                }
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

                if (this.IsSystemType(propertyType))
                {
                    if (propertyType == typeof(string) || propertyType == typeof(DateTime))
                    {
                        var value = propertyValue == null ? null : propertyValue.ToString();
                        result.AddString(jsonPropertyName, value);
                    }
                    else if (this.IsNumber(propertyType))
                    {
                        result.AddNumber(jsonPropertyName, propertyValue);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        result.AddBool(jsonPropertyName, (bool)propertyValue);
                    }
                    else if (this.IsNullable(propertyType))
                    {
                        result.AddNullable(jsonPropertyName, propertyValue);
                    }
                    else if (propertyType == typeof(Guid))
                    {
                        result.AddString(jsonPropertyName, propertyValue.ToString());
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyValue.GetType()))
                    {
                        result.AddArray(jsonPropertyName, this.ToJsonArray((IEnumerable)propertyValue, mappings));
                    }
                }
                else if (this.IsEnum(propertyType))
                {
                    result.AddNumber(jsonPropertyName, (int)propertyValue);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(propertyValue.GetType()))
                {
                    result.AddArray(jsonPropertyName, this.ToJsonArray((IEnumerable)propertyValue, mappings));
                }
                else
                {
                    result.AddObject(jsonPropertyName, this.ToJsonObject(propertyValue, mappings));
                }
            }

            return result;
        }

        public IJsonElementValue ToJsonElementValue(object obj, MappingContainer mappings = null)
        {
            if (obj != null)
            {
                var type = obj.GetType();

                if (this.IsSystemType(type))
                {
                    if (type == typeof(string))
                    {
                        return JsonElementValue.CreateString((string)obj);
                    }
                    else if (this.IsNumber(type))
                    {
                        return JsonElementValue.CreateNumber(obj);
                    }
                    else if (type == typeof(bool))
                    {
                        return JsonElementValue.CreateBool((bool)obj);
                    }
                    else if (this.IsNullable(type))
                    {
                        return JsonElementValue.CreateNullable(obj);
                    }
                    else if (type == typeof(Guid))
                    {
                        return JsonElementValue.CreateString(obj.ToString());
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                    {
                        return this.ToJsonArray((IEnumerable)obj, mappings);
                    }
                }
                else if (this.IsEnum(type))
                {
                    return JsonElementValue.CreateNumber((int)obj);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                {
                    return this.ToJsonArray((IEnumerable)obj, mappings);
                }
                else
                {
                    return this.ToJsonObject(obj, mappings);
                }
            }
            else
            {
                return JsonElementValue.CreateNullable(null);
            }

            throw new JsonLibException("Cannot resolve object");
        }
    }
}
