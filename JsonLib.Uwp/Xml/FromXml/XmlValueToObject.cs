using JsonLib.Common;
using JsonLib.Mappings.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace JsonLib.Xml
{
    public class XmlValueToObject : IXmlValueToObject
    {
        protected IAssemblyInfoService assemblyInfoService;

        public XmlValueToObject()
            :this(new AssemblyInfoService())
        { }

        public XmlValueToObject(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;
        }

        protected object ResolveValue(Type propertyType, XmlString xmlValue)
        {
            if (propertyType == typeof(Guid))
            {
                return new Guid(xmlValue.Value);
            }
            else if (propertyType == typeof(DateTime))
            {
                return DateTime.Parse(xmlValue.Value);
            }
            else if (this.assemblyInfoService.IsEnum(propertyType))
            {
                return Enum.Parse(propertyType, xmlValue.Value);
            }
            else
            {
                return xmlValue.Value;
            }
        }

        protected object ResolveValue(Type propertyType, XmlNumber xmlValue)
        {
            var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
            return this.assemblyInfoService.ConvertValueToPropertyType(xmlValue.Value, type);
        }

        public object ToValue(Type propertyType, XmlString xmlValue)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                // is nullable
                return xmlValue.Value == null ? null : this.ResolveValue(nullableType, xmlValue);
            }
            else
            {
                return this.ResolveValue(propertyType, xmlValue);
            }
        }

        public object ToValue(Type propertyType, XmlNumber xmlValue)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                // is nullable
                return xmlValue.Value == null ? null : this.ResolveValue(nullableType, xmlValue);
            }
            else
            {
                return this.ResolveValue(propertyType, xmlValue);
            }
        }

        public object ToValue(Type propertyType, XmlBool xmlValue)
        {
            return xmlValue.Value;
        }

        public object ToValue(Type propertyType, XmlNullable xmlValue)
        {
            var type = this.assemblyInfoService.GetNullableTargetType(propertyType);
            return this.assemblyInfoService.ConvertValueToPropertyType(xmlValue.Value, type);
        }

        public PropertyInfo FindProperty(PropertyInfo[] properties, string nodeName, XmlTypeMapping mapping = null)
        {
            var propertyName = nodeName;
            if (mapping != null && mapping.HasByXmlPropertyName(nodeName))
            {
                propertyName = mapping.GetByXmlPropertyName(nodeName).PropertyName;
            }

            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    return property;
                }
            }
            return null;
        }

        public PropertyInfo FindProperty(PropertyInfo[] properties, string xmlName)
        {
            foreach (var property in properties)
            {
                if (property.Name == xmlName)
                {
                    return property;
                }
            }
            return null;
        }

        public object ResolveValue(Type propertyType, IXmlValue xmlValue, XmlMappingContainer mappings = null)
        {
            if (xmlValue.ValueType == XmlValueType.String)
            {
                return this.ToValue(propertyType, (XmlString)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Number)
            {
                return this.ToValue(propertyType, (XmlNumber)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Bool)
            {
                return this.ToValue(propertyType, (XmlBool)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Nullable)
            {
                return this.ToValue(propertyType, (XmlNullable)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Object)
            {
                return this.ToObject(propertyType, (XmlObject)xmlValue, mappings);
            }
            else if (xmlValue.ValueType == XmlValueType.Array)
            {
                return this.ToEnumerable(propertyType, (XmlArray)xmlValue, mappings);
            }

            throw new JsonLibException("Cannot resolve Value");
        }

        public object ToObject(Type objType, XmlObject xmlObject, XmlMappingContainer mappings = null)
        {
            var instance = this.assemblyInfoService.CreateInstance(objType);
            var properties = this.assemblyInfoService.GetProperties(instance);

            bool hasMappings = mappings != null;

            XmlTypeMapping mapping = null;
            if (hasMappings && mappings.Has(objType))
            {
                mapping = mappings.Get(objType);
            }

            foreach (var xmlValue in xmlObject.Values)
            {
                var property = hasMappings ? this.FindProperty(properties, xmlValue.Key, mapping)
                     : this.FindProperty(properties, xmlValue.Key);
                if (property != null)
                {
                    var value = this.ResolveValue(property.PropertyType, xmlValue.Value, mappings);
                    this.assemblyInfoService.SetValue(instance, property, value);
                }
            }

            return instance;
        }

        public object ToList(Type type, XmlArray xmlArray, XmlMappingContainer mappings = null)
        {
            var singleItemType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(singleItemType);
            var result = this.assemblyInfoService.CreateInstance(listType) as IList;

            foreach (var xmlValue in xmlArray.Values)
            {
                var value = this.ResolveValue(singleItemType, xmlValue, mappings);
                result.Add(value);
            }
            return result;
        }

        public object ToArray(Type type, XmlArray xmlArray, XmlMappingContainer mappings = null)
        {
            var singleItemType = type.GetTypeInfo().GetElementType();
            var result = Array.CreateInstance(singleItemType, xmlArray.Values.Count);
            int index = 0;

            foreach (var xmlValue in xmlArray.Values)
            {
                var value = this.ResolveValue(singleItemType, xmlValue, mappings);
                result.SetValue(value, index);
                index++;
            }
            return result;
        }

        public object ResolveDictionaryKey(Type propertyType, IXmlValue xmlValueKey)
        {
            if(xmlValueKey.ValueType == XmlValueType.String)
            {
                if (propertyType.FullName == "System.Type")
                {
                    var resolvedType =  Type.GetType(((XmlString)xmlValueKey).Value);
                    if(resolvedType == null) { throw new JsonLibException("cannot resolve Type with the qualified name " + ((XmlString)xmlValueKey).Value); }
                    return resolvedType;
                }
                else
                {
                    return this.ResolveValue(propertyType, (XmlString)xmlValueKey);
                }
            }
            else if (xmlValueKey.ValueType == XmlValueType.Number)
            {
                if (propertyType == typeof(string))
                {
                    return Convert.ToString(((XmlNumber)xmlValueKey).Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    return ((XmlNumber)xmlValueKey).Value;
                }
            }            

            throw new JsonLibException("Unsupported type in xml for dictionary key");
        }

        public object ToXmlArray_FromDictionary(Type type, XmlArray xmlArray, XmlMappingContainer mappings = null)
        {
            var keyType = this.assemblyInfoService.GetDictionaryKeyType(type);
            var valueType = this.assemblyInfoService.GeDictionaryValueType(type);

            var result = this.assemblyInfoService.CreateInstance(type) as IDictionary;
            if (result == null) { throw new JsonLibException("Cannot create dictionary"); }

            foreach (var xmlValue in xmlArray.Values)
            {
                var xmlObject = xmlValue as XmlObject;
                if(xmlObject == null || xmlObject.Values.Count != 2) { throw new JsonLibException("Cannot resolve dictionary from xml");  }

                var xmlEntryKey = xmlObject.Values.ElementAt(0).Value;
                var key = this.ResolveDictionaryKey(keyType, xmlEntryKey);

                var xmlEntryValue = xmlObject.Values.ElementAt(1).Value;
                var value = this.ResolveValue(valueType, xmlEntryValue, mappings);

                result.Add(key, value);
            }

            return result;
        }

        public object ToEnumerable(Type type, XmlArray xmlArray, XmlMappingContainer mappings = null)
        {
            if (this.assemblyInfoService.IsArray(type))
            {
                return this.ToArray(type, xmlArray, mappings);
            }
            else if (this.assemblyInfoService.IsDictionary(type))
            {
                return this.ToXmlArray_FromDictionary(type, xmlArray, mappings);
            }
            else if (this.assemblyInfoService.IsGenericType(type))
            {
                return this.ToList(type, xmlArray, mappings);
            }

            throw new JsonLibException("Type is not enumerable " + type.Name);
        }

        public XmlArray ConvertToArray(XmlObject xmlObject)
        {
            var result = new XmlArray(xmlObject.NodeName);
            foreach (var xmlValue in xmlObject.Values)
            {
                result.Add(xmlValue.Value);
            }
            return result;
        }

        public object Resolve(Type type, IXmlValue xmlValue, XmlMappingContainer mappings = null)
        {
            var xmlNillable = xmlValue as IXmlNillable;
            if (xmlNillable != null && xmlNillable.IsNil)
            {
                return null;
            }
            else if (xmlValue.ValueType == XmlValueType.String)
            {
                return this.ToValue(type, (XmlString)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Number)
            {
                return this.ToValue(type, (XmlNumber)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Bool)
            {
                return this.ToValue(type, (XmlBool)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Nullable)
            {
                return this.ToValue(type, (XmlNullable)xmlValue);
            }
            else if (xmlValue.ValueType == XmlValueType.Array)
            {
                return this.ToEnumerable(type, (XmlArray)xmlValue, mappings);
            }
            else if (xmlValue.ValueType == XmlValueType.Object)
            {
                // guess is object with only one node value 
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    var xmlArray = this.ConvertToArray((XmlObject)xmlValue);
                    return this.ToEnumerable(type, xmlArray, mappings);
                }
                else
                {
                    return this.ToObject(type, (XmlObject)xmlValue, mappings);
                }
            }

            throw new JsonLibException("Cannot resolve value");
        }

        public T Resolve<T>(IXmlValue xmlValue, XmlMappingContainer mappings = null)
        {
            return (T)this.Resolve(typeof(T), xmlValue, mappings);
        }
    }
}
