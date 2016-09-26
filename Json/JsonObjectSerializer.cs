using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Json
{
    public class TypeHelper
    {
        public static bool IsBaseType(Type type)
        {
            return type == typeof(string) || type == typeof(DateTime) || type == typeof(int) || type == typeof(double) || type == typeof(bool) || IsEnum(type) || IsNullable(type);
        }

        public static bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }

    public class ParsedItem
    {
        public Type Type { get; set; }
        public object Item { get; set; }
    }

    public class JsonObjectSerializer
    {
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> typesCache =
            new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        private static Dictionary<string, ParsedItem> parsedJsonCache =
            new Dictionary<string, ParsedItem>();

        public static bool UseJsonMapAttributes { get; set; } = false;

        private static void DebugWrite(string message, string severity = "Warning")
        {
            var caller = "JsonObjectSerializer";
            Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {message}");
        }

        private static Dictionary<string, PropertyInfo> ResolveMapping(Type type)
        {
            if (typesCache.ContainsKey(type))
            {
                return typesCache[type];
            }
            else
            {
                var result = new Dictionary<string, PropertyInfo>();

                var propertiesInfos = type.GetProperties().ToList();
                foreach (var property in propertiesInfos)
                {
                    bool isFound = false;
                    // Attribute
                    if (UseJsonMapAttributes)
                    {
                        var attribute = Attribute.GetCustomAttribute(property, typeof(JsonMapAttribute)) as JsonMapAttribute;
                        if (attribute != null)
                        {
                            // add json element key / property
                            var keyByAttribute = attribute.JsonElementKey;
                            if (string.IsNullOrEmpty(keyByAttribute)) throw new ArgumentException("No ColumnName found on MapAttribute. Property " + property.Name + " of " + type.Name);
                            result[keyByAttribute] = property;
                            isFound = true;
                        }
                    }
                    // manual mapping
                    if (JsonMapping.Default.Count > 0)
                    {
                        var keyByManual = JsonMapping.Default.GetJsonElementKey(type, property.Name);
                        if (!string.IsNullOrEmpty(keyByManual))
                        {
                            result[keyByManual] = property;
                            isFound = true;
                        }
                    }
                    // not found with attribute or manual
                    if (!isFound)
                    {
                        var defaultKey = property.Name;
                        result[defaultKey] = property;
                    }

                }
                typesCache[type] = result;
                return result;
            }
        }

        private static void ParseJsonArrayToArray(JsonElementArray sourceJsonArray, Array resultArray, Type singleItemType)
        {
            int index = 0;
            foreach (var jsonElement in sourceJsonArray)
            {
                var jsonElementType = jsonElement.ElementType;

                // json array of string "strings" : [ "a, "b", "c" ]
                if (jsonElementType == JsonElementType.String)
                {
                    // json
                    var valueResult = ((JsonElementString)jsonElement).Value;
                    // object
                    if (singleItemType == typeof(string))
                    {
                        resultArray.SetValue(valueResult, index);
                        index++;
                    }
                    else if (singleItemType == typeof(DateTime))
                    {
                        resultArray.SetValue(DateTime.Parse(valueResult), index);
                        index++;
                    }
                }
                else if (jsonElementType == JsonElementType.Number)
                {
                    // json 
                    var valueResult = ((JsonElementNumber)jsonElement).Value;
                    // object
                    if (singleItemType == typeof(int)) resultArray.SetValue((int)valueResult, index);
                    else resultArray.SetValue(valueResult, index);
                    index++;
                }
                else if (jsonElementType == JsonElementType.Bool)
                {
                    // json 
                    var valueResult = ((JsonElementBool)jsonElement).Value;
                    // object
                    resultArray.SetValue(valueResult, index);
                    index++;
                }
                else if (jsonElementType == JsonElementType.Object)
                {
                    // json 
                    var jsonObject = (JsonElementObject)jsonElement;
                    // object
                    var valueResult = Activator.CreateInstance(singleItemType);
                    if (valueResult != null)
                    {
                        ParseJsonObjectToObject(jsonObject, valueResult);
                        resultArray.SetValue(valueResult, index);
                        index++;
                    }
                }
                else if (jsonElementType == JsonElementType.Array)
                {
                    // json 
                    var jsonArray = (JsonElementArray)jsonElement;
                    // object
                    if (singleItemType.IsArray)
                    {
                        var innerSingleItemType = singleItemType.GetTypeInfo().GetElementType();
                        if (innerSingleItemType != null)
                        {
                            var valueResult = Array.CreateInstance(innerSingleItemType, jsonArray.Count);
                            ParseJsonArrayToArray(jsonArray, valueResult, innerSingleItemType);
                            resultArray.SetValue(valueResult, index);
                            index++;
                        }
                    }
                    else if (TypeHelper.IsGenericType(singleItemType))
                    {
                        var innerSingleItemType = singleItemType.GetGenericArguments()[0];
                        if (innerSingleItemType != null)
                        {
                            var listType = typeof(List<>).MakeGenericType(innerSingleItemType);
                            var valueResult = Activator.CreateInstance(listType);

                            ParseJsonArrayToList(jsonArray, (IList)valueResult, singleItemType);
                            resultArray.SetValue(valueResult, index);
                            index++;
                        }
                    }
                }
            }
        }

        private static void ParseJsonArrayToList(JsonElementArray sourceJsonArray, IList resultList, Type singleItemType)
        {
            foreach (var jsonElement in sourceJsonArray)
            {
                var jsonElementType = jsonElement.ElementType;

                if (jsonElementType == JsonElementType.Object)
                {
                    // json
                    var jsonObject = (JsonElementObject)jsonElement;
                    // object
                    var resultObject = Activator.CreateInstance(singleItemType);
                    if (resultObject != null)
                    {
                        ParseJsonObjectToObject(jsonObject, resultObject);
                        resultList.Add(resultObject);
                    }
                }
                else if (jsonElementType == JsonElementType.Array)
                {
                    // json
                    var jsonArray = (JsonElementArray)jsonElement;
                    // object
                    var innerSingleItemType = singleItemType.GetTypeInfo().GetElementType();
                    if (innerSingleItemType != null)
                    {
                        var innerArray = Array.CreateInstance(innerSingleItemType, jsonArray.Count);
                        ParseJsonArrayToArray(jsonArray, innerArray, innerSingleItemType);
                        resultList.Add(innerArray);
                    }
                }
                // json array of string "strings" : [ "a, "b", "c" ]
                else if (jsonElementType == JsonElementType.String)
                {
                    // json
                    var valueResult = ((JsonElementString)jsonElement).Value;
                    // object
                    if (singleItemType == typeof(string))
                    {
                        resultList.Add(valueResult);
                    }
                    else if (singleItemType == typeof(DateTime))
                    {
                        resultList.Add(DateTime.Parse(valueResult));
                    }
                }
                else if (jsonElementType == JsonElementType.Number)
                {
                    // json
                    var valueResult = ((JsonElementNumber)jsonElement).Value;
                    // object
                    if (singleItemType == typeof(int)) resultList.Add((int)valueResult);
                    else resultList.Add(valueResult);
                }
                else if (jsonElementType == JsonElementType.Bool)
                {
                    // json
                    var valueResult = ((JsonElementBool)jsonElement).Value;
                    // object
                    resultList.Add(valueResult);
                }
            }
        }

        private static void ParseJsonObjectToObject(JsonElementObject sourceJsonObject, object resultObject)
        {
            var mapping = ResolveMapping(resultObject.GetType());

            foreach (var item in sourceJsonObject)
            {
                var key = item.Key;
                var property = mapping[key];
                if (property != null)
                {
                    var propertyType = property.PropertyType;
                    var jsonElementType = item.Value.ElementType;

                    if (jsonElementType == JsonElementType.String)
                    {
                        var valueResult = ((JsonElementString)sourceJsonObject[key]).Value;

                        if (TypeHelper.IsEnum(propertyType))
                        {
                            property.SetValue(resultObject, Enum.Parse(propertyType, valueResult));
                        }
                        else
                        {
                            if (propertyType == typeof(string))
                            {
                                property.SetValue(resultObject, valueResult);
                            }
                            else if (propertyType == typeof(DateTime))
                            {
                                property.SetValue(resultObject, DateTime.Parse(valueResult));
                            }
                            else
                            {
                                var innerPropertyType = Nullable.GetUnderlyingType(propertyType);
                                if (innerPropertyType == typeof(DateTime))
                                {
                                    property.SetValue(resultObject, DateTime.Parse(valueResult));
                                }
                            }
                        }
                    }
                    else if (jsonElementType == JsonElementType.Number)
                    {
                        var valueResult = ((JsonElementNumber)sourceJsonObject[key]).Value;

                        if (propertyType == typeof(int))
                        {
                            property.SetValue(resultObject, (int)valueResult);
                        }
                        else if (propertyType == typeof(double))
                        {
                            property.SetValue(resultObject, valueResult);
                        }
                        else
                        {
                            var innerPropertyType = Nullable.GetUnderlyingType(propertyType);
                            if (innerPropertyType == typeof(int))
                            {
                                property.SetValue(resultObject, (int)valueResult);
                            }
                            else if (innerPropertyType == typeof(double))
                            {
                                property.SetValue(resultObject, valueResult);
                            }
                        }
                    }
                    else if (jsonElementType == JsonElementType.Bool)
                    {
                        var valueResult = ((JsonElementBool)sourceJsonObject[key]).Value;
                        property.SetValue(resultObject, valueResult);
                    }
                    else if (jsonElementType == JsonElementType.Array)
                    {
                        var innerJsonArray = ((JsonElementArray)sourceJsonObject[key]);
                        // Array ? or generic list
                        if (propertyType.IsArray)
                        {
                            var singleItemType = propertyType.GetTypeInfo().GetElementType();
                            if (singleItemType != null)
                            {
                                var valueResult = Array.CreateInstance(singleItemType, innerJsonArray.Count);
                                ParseJsonArrayToArray(innerJsonArray, valueResult, singleItemType);
                                property.SetValue(resultObject, valueResult);
                            }
                        }
                        else if (TypeHelper.IsGenericType(propertyType))
                        {
                            var singleItemType = propertyType.GetGenericArguments()[0];
                            if (singleItemType != null)
                            {
                                var listType = typeof(List<>).MakeGenericType(singleItemType);
                                var valueResult = Activator.CreateInstance(listType);

                                ParseJsonArrayToList(innerJsonArray, (IList)valueResult, singleItemType);
                                property.SetValue(resultObject, valueResult);
                            }
                        }
                    }
                    else if (jsonElementType == JsonElementType.Object)
                    {
                        var innerJsonObject = ((JsonElementObject)sourceJsonObject[key]);

                        if (propertyType == typeof(Dictionary<string, object>)) // dynamic
                        {
                            var innerDictionary = new Dictionary<string, object>();
                            ParseJsonObjectToDictionary(innerJsonObject, innerDictionary);
                            property.SetValue(resultObject, innerDictionary);
                        }
                        else
                        {
                            var innerObject = Activator.CreateInstance(propertyType);
                            if (innerObject != null)
                            {
                                ParseJsonObjectToObject(innerJsonObject, innerObject);
                                property.SetValue(resultObject, innerObject);
                            }
                        }
                    }
                }
                else
                {
                    DebugWrite($"Cannot find property for {resultObject.GetType().Name} and json key {key}");
                }
            }
        }

        private static void ParseJsonObjectToDictionary(JsonElementObject jsonObject, Dictionary<string, object> resultDictionary)
        {
            foreach (var item in jsonObject)
            {
                var jsonElementType = item.Value.ElementType;
                if (jsonElementType == JsonElementType.String)
                {
                    var valueResult = ((JsonElementString)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (jsonElementType == JsonElementType.Number)
                {
                    var valueResult = ((JsonElementNumber)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (jsonElementType == JsonElementType.Bool)
                {
                    var valueResult = ((JsonElementBool)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (jsonElementType == JsonElementType.Object)
                {
                    var nextJsonObject = ((JsonElementObject)jsonObject[item.Key]);

                    var dictionary = new Dictionary<string, object>();
                    ParseJsonObjectToDictionary(nextJsonObject, dictionary);
                    resultDictionary[item.Key] = dictionary;
                }
            }
        }

        public static T Parse<T>(string json)
        {
            var objType = typeof(T);
            if (parsedJsonCache.ContainsKey(json) && parsedJsonCache[json].Type == objType)
            {
                return (T)(object)parsedJsonCache[json].Item;
            }
            else
            {
                if (TypeHelper.IsBaseType(objType))
                {
                    if (objType == typeof(string))
                    {
                        return (T)(object)json;
                    }
                    if (TypeHelper.IsEnum(objType))
                    {
                        return (T)(object)Enum.Parse(objType, json);
                    }
                    if (objType == typeof(DateTime))
                    {
                        return (T)(object)DateTime.Parse(json);
                    }
                    else if (objType == typeof(int))
                    {
                        return (T)(object)int.Parse(json);
                    }
                    else if (objType == typeof(double))
                    {
                        var value = json.Replace('.', ',');
                        return (T)(object)double.Parse(value);
                    }
                    else if (objType == typeof(bool))
                    {
                        return (T)(object)bool.Parse(json);
                    }
                    else if (TypeHelper.IsNullable(objType))
                    {
                        if (json == "null") return (T)(object)null;

                        var innerPropertyType = Nullable.GetUnderlyingType(objType);
                        if (innerPropertyType == typeof(DateTime))
                        {
                            return (T)(object)DateTime.Parse(json);
                        }
                        else if (innerPropertyType == typeof(int))
                        {
                            return (T)(object)int.Parse(json);
                        }
                        else if (innerPropertyType == typeof(double))
                        {
                            var value = json.Replace('.', ',');
                            return (T)(object)double.Parse(value);
                        }
                        else if (innerPropertyType == typeof(bool))
                        {
                            return (T)(object)bool.Parse(json);
                        }
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(objType))
                {
                    JsonElementArray rootJsonArray = null;
                    if (JsonElementArray.TryParse(json, out rootJsonArray))
                    {
                        if (objType.IsArray)
                        {
                            var singleItemType = typeof(T).GetTypeInfo().GetElementType();
                            var resultArray = Array.CreateInstance(singleItemType, rootJsonArray.Count);

                            ParseJsonArrayToArray(rootJsonArray, resultArray, singleItemType);
                            return (T)(object)resultArray;
                        }
                        else if (TypeHelper.IsGenericType(objType))
                        {
                            var singleItemType = objType.GetGenericArguments()[0];
                            var listType = typeof(List<>).MakeGenericType(singleItemType);
                            var resultList = Activator.CreateInstance(listType);

                            ParseJsonArrayToList(rootJsonArray, (IList)resultList, singleItemType);

                            parsedJsonCache[json] = new ParsedItem { Type = objType, Item = resultList };

                            return (T)(object)resultList;
                        }
                        return default(T);
                    }
                }
                else
                {
                    var resultObject = Activator.CreateInstance(typeof(T));
                    JsonElementObject rootJsonObject = null;
                    if (JsonElementObject.TryParse(json, out rootJsonObject))
                    {
                        ParseJsonObjectToObject(rootJsonObject, resultObject);

                        parsedJsonCache[json] = new ParsedItem { Type = objType, Item = resultObject };

                        return (T)resultObject;
                    }
                }
                return default(T);
            }
        }

        private static void InspectList(IEnumerable list, JsonElementArray resultJsonArray)
        {
            foreach (var item in list)
            {
                var itemType = item.GetType();
                if (itemType == typeof(string))
                {
                    resultJsonArray.Add(JsonElement.CreateString((string)item));
                }
                else if (itemType == typeof(int))
                {
                    resultJsonArray.Add(JsonElement.CreateNumber((int)item));
                }
                else if (itemType == typeof(double))
                {
                    resultJsonArray.Add(JsonElement.CreateNumber((double)item));
                }
                else if (itemType == typeof(bool))
                {
                    resultJsonArray.Add(JsonElement.CreateBool((bool)item));
                }
                else if (itemType == typeof(DateTime))
                {
                    resultJsonArray.Add(JsonElement.CreateString(item.ToString()));
                }
                else if (typeof(IEnumerable).IsAssignableFrom(itemType))
                {
                    JsonElementArray jsonArray = new JsonElementArray();
                    InspectList((IEnumerable)item, jsonArray);
                    resultJsonArray.Add(jsonArray);
                }
                else
                {
                    JsonElementObject innerObject = new JsonElementObject();
                    InspectObject(item, innerObject);
                    resultJsonArray.Add(innerObject);
                }
            }
        }

        private static void InspectObject(object obj, JsonElementObject resultJsonObject)
        {
            var mapping = ResolveMapping(obj.GetType());

            foreach (var item in mapping)
            {
                var key = item.Key;
                var property = item.Value;
                var propertyType = property.PropertyType;
                var value = property.GetValue(obj);
                if (value != null)
                {
                    if (propertyType == typeof(string))
                    {
                        resultJsonObject[key] = JsonElement.CreateString((string)value);
                    }
                    else if (propertyType == typeof(int))
                    {
                        resultJsonObject[key] = JsonElement.CreateNumber((int)value);
                    }
                    else if (propertyType == typeof(double))
                    {
                        resultJsonObject[key] = JsonElement.CreateNumber((double)value);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        resultJsonObject[key] = JsonElement.CreateBool((bool)value);
                    }
                    else if (propertyType == typeof(DateTime) || TypeHelper.IsEnum(propertyType))
                    {
                        resultJsonObject[key] = JsonElement.CreateString(value.ToString());
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyType))
                    {
                        var innerJsonArray = new JsonElementArray();
                        InspectList((IEnumerable)value, innerJsonArray);
                        resultJsonObject[key] = innerJsonArray;
                    }
                    else
                    {
                        if (!TypeHelper.IsNullable(propertyType))
                        {
                            var innerJsonObject = new JsonElementObject();
                            InspectObject(value, innerJsonObject);
                            resultJsonObject[key] = innerJsonObject;
                        }
                        else
                        {
                            var innerPropertyType = Nullable.GetUnderlyingType(propertyType);
                            if (innerPropertyType == typeof(int))
                            {
                                resultJsonObject[key] = JsonElement.CreateNumber((int)value);
                            }
                            else if (innerPropertyType == typeof(double))
                            {
                                resultJsonObject[key] = JsonElement.CreateNumber((double)value);
                            }
                            else if (innerPropertyType == typeof(bool))
                            {
                                resultJsonObject[key] = JsonElement.CreateBool((bool)value);
                            }
                            else if (innerPropertyType == typeof(DateTime) || TypeHelper.IsEnum(innerPropertyType))
                            {
                                resultJsonObject[key] = JsonElement.CreateString(value.ToString());
                            }
                        }

                    }
                }
            }
        }

        public static string Stringify(object obj, bool indented = false)
        {
            if (obj == null) return "null";

            var objType = obj.GetType();

            if (TypeHelper.IsBaseType(objType))
            {
                if (objType == typeof(string) || objType == typeof(DateTime) || TypeHelper.IsEnum(objType))
                {
                    return obj.ToString();
                }
                else if (objType == typeof(int) || objType == typeof(double))
                {
                    var result = obj.ToString().Replace(',', '.');
                    return result;
                }
                else if (objType == typeof(bool))
                {
                    return obj.ToString().ToLower();
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(objType))
            {
                var result = new JsonElementArray();
                InspectList((IEnumerable)obj, result);

                var json = result.Stringify(indented);
                return json;
            }
            else
            {
                var result = new JsonElementObject();
                InspectObject(obj, result);

                var json = result.Stringify(indented);
                return json;
            }
            return default(string);
        }

        public static void ClearCaches()
        {
            parsedJsonCache.Clear();
            typesCache.Clear();
        }
    }
}
