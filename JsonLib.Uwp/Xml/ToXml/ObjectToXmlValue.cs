using JsonLib.Common;
using JsonLib.Mappings.Xml;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace JsonLib.Xml
{
    public class ObjectToXmlValue : IObjectToXmlValue
    {
        protected IAssemblyInfoService assemblyInfoService;

        public ObjectToXmlValue()
            : this(new AssemblyInfoService())
        { }

        public ObjectToXmlValue(IAssemblyInfoService assemblyInfoService)
        {
            this.assemblyInfoService = assemblyInfoService;
        }

        public string GetStringValueOrNull(object value)
        {
            return value == null ? null : value.ToString();
        }

        public string GetXmlPropertyName(string propertyName, XmlTypeMapping mapping)
        {
            if (mapping.Has(propertyName))
            {
                return mapping.Properties[propertyName].XmlPropertyName;
            }
            return propertyName;
        }

        protected XmlArray DoToXmlArray(Type type, Type singleItemType, string nodeName, IEnumerable enumerable, XmlMappingContainer mappings = null)
        {
            // string[],List<string>, int[]...MyEnum[],Guid[], DateTime[]
            // int?[],... DateTime?[], Guid?[], MyEnum?[]
            // MyItem[]

            var result = new XmlArray(nodeName);
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    var resultValue = this.ToXmlValue(singleItemType, value, mappings);
                    result.Add(resultValue);
                }
            }
            else
            {
                result.SetNil();
            }
            return result;
        }

        public XmlArray ToXmlArray(Type type, IEnumerable enumerable, string nodeName, XmlMappingContainer mappings = null)
        {
            var singleItemType = this.assemblyInfoService.GetSingleItemType(type);
            if (singleItemType == null) { throw new JsonLibException("Require an enumerable"); }

            return this.DoToXmlArray(type, singleItemType, nodeName, enumerable, mappings);
        }

        public XmlArray ToXmlArray(Type type, IEnumerable enumerable, XmlMappingContainer mappings = null)
        {
            var singleItemType = this.assemblyInfoService.GetSingleItemType(type);
            if (singleItemType == null) { throw new JsonLibException("Require an enumerable"); }

            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(singleItemType))
            {
                mapping = mappings.Get(singleItemType);
            }

            var nullableType = Nullable.GetUnderlyingType(singleItemType);
            if(nullableType != null)
            {
                var mainNodeName = mapping != null && mapping.HasXmlArrayName ? mapping.XmlArrayName : "ArrayOf" + nullableType.Name;
                return this.DoToXmlArray(type, singleItemType, mainNodeName, enumerable, mappings);
            }
            else
            {
                var mainNodeName = mapping != null && mapping.HasXmlArrayName ? mapping.XmlArrayName : "ArrayOf" + singleItemType.Name;
                return this.DoToXmlArray(type, singleItemType, mainNodeName, enumerable, mappings);
            }             
        }

        protected XmlObject DoToXmlObject(Type type, object obj, string nodeName, XmlTypeMapping mapping, XmlMappingContainer mappings = null)
        {
            var result = new XmlObject(nodeName);

            var properties = this.assemblyInfoService.GetProperties(obj);

            bool hasMapping = mapping != null;

            if (obj != null)
            {
                foreach (var property in properties)
                {
                    var propertyNodeName = hasMapping ? this.GetXmlPropertyName(property.Name, mapping) : property.Name;
                    var propertyValue = property.GetValue(obj);
                    var propertyType = property.PropertyType;

                    var xmlValue = this.ToXmlValue(propertyType, propertyValue, propertyNodeName, mappings);
                    result.Add(propertyNodeName, xmlValue);
                }
            }
            else
            {
                result.SetNil();
            }
            return result;
        }

        public XmlObject ToXmlObject(Type type, object obj, string nodeName, XmlMappingContainer mappings = null)
        {
            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(type))
            {
                mapping = mappings.Get(type);
            }
            return this.DoToXmlObject(type, obj, nodeName, mapping, mappings);
        }

        public XmlObject ToXmlObject(Type type, object obj, XmlMappingContainer mappings = null)
        {
            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(type))
            {
                mapping = mappings.Get(type);
            }

            var nodeName = mapping != null ? mapping.XmlTypeName : type.Name;
            return this.DoToXmlObject(type, obj, nodeName, mapping, mappings);
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

        public XmlArray DoToXmlArray_FromDictionary(Type type, Type keyType, Type valueType, IDictionary dictionary, string mainNodeName, XmlMappingContainer mappings = null)
        {
            var result = new XmlArray(mainNodeName);

            var valueTypeName = valueType.Name;

            foreach (var keyValue in dictionary)
            {
                var entry = (DictionaryEntry)keyValue;

                // Int32 for example
                var keyValueXmlObject = new XmlObject(valueTypeName);

                // key
                var keyString = this.ResolveDictionaryKey(keyType, entry.Key);
                keyValueXmlObject.Add("Key", new XmlString("Key", keyString));   

                // value
                var resultValue = this.ToXmlValue(valueType, entry.Value, "Value", mappings);
                keyValueXmlObject.Add("Value", resultValue);

                result.Add(keyValueXmlObject);
            }

            return result;
        }

        public XmlArray ToXmlArray_FromDictionary(Type type, IDictionary dictionary, string mainNodeName, XmlMappingContainer mappings = null)
        {
            var keyType = this.assemblyInfoService.GetDictionaryKeyType(type);
            var valueType = this.assemblyInfoService.GetDictionaryValueType(type);

            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(valueType))
            {
                mapping = mappings.Get(valueType);
            }

            var nullableType = Nullable.GetUnderlyingType(valueType);
            if (nullableType != null)
            {
                return this.DoToXmlArray_FromDictionary(type, keyType, nullableType, dictionary, mainNodeName, mappings);
            }
            else
            {
                return this.DoToXmlArray_FromDictionary(type, keyType, valueType, dictionary, mainNodeName, mappings);
            }
        }

        public XmlArray ToXmlArray_FromDictionary(Type type, IDictionary dictionary, XmlMappingContainer mappings = null)
        {
            var keyType = this.assemblyInfoService.GetDictionaryKeyType(type);
            var valueType = this.assemblyInfoService.GetDictionaryValueType(type);

            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(valueType))
            {
                mapping = mappings.Get(valueType);
            }

            var nullableType = Nullable.GetUnderlyingType(valueType);
            if (nullableType != null)
            {
                var mainNodeName = mapping != null && mapping.HasXmlArrayName ? mapping.XmlArrayName : "ArrayOf" + nullableType.Name;
                return this.DoToXmlArray_FromDictionary(type, keyType, nullableType, dictionary, mainNodeName, mappings);
            }
            else
            {
                var mainNodeName = mapping != null && mapping.HasXmlArrayName ? mapping.XmlArrayName : "ArrayOf" + valueType.Name;
                return this.DoToXmlArray_FromDictionary(type, keyType, valueType, dictionary, mainNodeName, mappings);
            }
        }

        public IXmlValue ToXmlValue(Type type, object value, XmlMappingContainer mappings = null)
        {
            XmlTypeMapping mapping = null;
            if (mappings != null && mappings.Has(type))
            {
                mapping = mappings.Get(type);
            }

            bool hasMapping = mapping != null;

            if (this.assemblyInfoService.IsSystemType(type))
            {
                if (type == typeof(string))
                {
                    var nodeName = hasMapping ? mapping.XmlTypeName : "String";
                    return new XmlString(nodeName, this.GetStringValueOrNull(value));
                }
                else if (this.assemblyInfoService.IsNumberType(type))
                {
                    var nodeName = hasMapping ? mapping.XmlTypeName : type.Name;
                    return new XmlNumber(nodeName, value);
                }
                else if (type == typeof(bool))
                {
                    var nodeName = hasMapping ? mapping.XmlTypeName : "Boolean";
                    return new XmlBool(nodeName, Convert.ToBoolean(value));
                }
                else if (type == typeof(DateTime))
                {
                    var nodeName = hasMapping ? mapping.XmlTypeName : "DateTime";
                    return new XmlString(nodeName, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    var nodeName = hasMapping ? mapping.XmlTypeName : "Guid";
                    return new XmlString(nodeName, value.ToString());
                }
                else if (this.assemblyInfoService.IsNullable(type))
                {
                    var underlyingType = Nullable.GetUnderlyingType(type);
                    if(underlyingType != null)
                    {
                        var nodeName = hasMapping ? mapping.XmlTypeName : underlyingType.Name;
                        return new XmlNullable(type, nodeName, value);
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    // array example string[] 
                    return this.ToXmlArray(type, (IEnumerable)value, mappings);
                }
            }
            else if (this.assemblyInfoService.IsDictionary(type))
            {
              return this.ToXmlArray_FromDictionary(type, (IDictionary) value, mappings);
            }
            else if (this.assemblyInfoService.IsEnum(type))
            {
                var nodeName = hasMapping ? mapping.XmlTypeName : type.Name;
                return new XmlString(nodeName, value.ToString());
            }
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                return this.ToXmlArray(type, (IEnumerable)value, mappings);
            }
            else
            {
                return this.ToXmlObject(type, value, mappings);
            }

            throw new JsonLibException("Cannot resolve object");
        }

        public IXmlValue ToXmlValue(Type type, object value, string nodeName, XmlMappingContainer mappings = null)
        {

            if (this.assemblyInfoService.IsSystemType(type))
            {
                if (type == typeof(string))
                {
                    return new XmlString(nodeName, this.GetStringValueOrNull(value));
                }
                else if (this.assemblyInfoService.IsNumberType(type))
                {
                    return new XmlNumber(nodeName, value);
                }
                else if (type == typeof(bool))
                {
                    return new XmlBool(nodeName, Convert.ToBoolean(value));
                }
                else if (type == typeof(DateTime))
                {
                    return new XmlString(nodeName, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    return new XmlString(nodeName, value.ToString());
                }
                else if (this.assemblyInfoService.IsNullable(type))
                {
                    var underlyingType = Nullable.GetUnderlyingType(type);
                    if (underlyingType != null)
                    {
                        return new XmlNullable(type, nodeName, value);
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    // array example string[]
                    return this.ToXmlArray(type, (IEnumerable)value, nodeName, mappings);
                }
            }
            else if (this.assemblyInfoService.IsEnum(type))
            {
                return new XmlString(nodeName, value.ToString());
            }
            else if (this.assemblyInfoService.IsDictionary(type))
            {
                return this.ToXmlArray_FromDictionary(type, (IDictionary)value, nodeName, mappings);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                return this.ToXmlArray(type, (IEnumerable)value, nodeName, mappings);
            }
            else
            {
                return this.ToXmlObject(type, value, nodeName, mappings);
            }

            throw new JsonLibException("Cannot resolve object");
        }

        public IXmlValue ToXmlValue<T>(T obj, XmlMappingContainer mappings = null)
        {
            return this.ToXmlValue(typeof(T), obj, mappings);
        }
    }
}
