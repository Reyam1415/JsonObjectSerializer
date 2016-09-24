using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Json
{
    public class JsonObjectSerializer
    {
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> cache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        public static bool UseJsonMapAttributes { get; set; } = false;

        private static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool IsString(Type type)
        {
            return type == typeof(string);
        }

        private static bool IsNumber(Type type)
        {
            return type == typeof(int) || type == typeof(double);
        }

        private static bool IsInt(Type type)
        {
            return type == typeof(int);
        }

        private static bool IsDouble(Type type)
        {
            return type == typeof(double);
        }

        private static bool IsBool(Type type)
        {
            return type == typeof(bool);
        }

        private static bool IsDateTime(Type type)
        {
            return type == typeof(DateTime);
        }

        private static bool IsObject(Type type)
        {
            return typeof(object).IsAssignableFrom(type);
        }

        private static bool IsArray(Type type)
        {
            return type.IsArray;
        }

        private static bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        private static bool IsDictionary(Type type)
        {
            return type == typeof(Dictionary<string, object>);
        }

        private static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        private static void DebugWrite(string message, string severity = "Warning")
        {
            var caller = "JsonObjectSerializer";
            Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {message}");
        }

        private static void ParseJsonArrayToArray(JsonElementArray sourceJsonArray, Array resultArray, Type singleItemType)
        {
            int index = 0;
            foreach (var jsonElement in sourceJsonArray)
            {
                // json array of string "strings" : [ "a, "b", "c" ]
                if (jsonElement.ElementType == JsonElementType.String)
                {
                    // json value
                    var valueResult = ((JsonElementString)jsonElement).Value;
                    // 
                    if (IsString(singleItemType))
                    {
                        resultArray.SetValue(valueResult, index);
                        index++;
                    }
                    else if (IsDateTime(singleItemType))
                    {
                        DateTime dateTimeResult = default(DateTime);
                        if (DateTime.TryParse(valueResult, out dateTimeResult))
                        {
                            try
                            {
                                resultArray.SetValue(dateTimeResult, index);
                                index++;
                            }
                            catch (Exception)
                            {
                                DebugWrite($"Cannot set datetime value for : {valueResult}");
                            }
                        }
                    }
                }
                else if (jsonElement.ElementType == JsonElementType.Number)
                {
                    // json value
                    var valueResult = ((JsonElementNumber)jsonElement).Value;
                    // 
                    if (IsInt(singleItemType)) resultArray.SetValue((int)valueResult, index);
                    else resultArray.SetValue((double)valueResult, index);
                    index++;
                }
                else if (jsonElement.ElementType == JsonElementType.Boolean)
                {
                    // json value
                    var valueResult = ((JsonElementBool)jsonElement).Value;
                    // 
                    resultArray.SetValue(valueResult, index);
                    index++;
                }
                else if (jsonElement.ElementType == JsonElementType.Object)
                {
                    // json value
                    var jsonObject = (JsonElementObject)jsonElement;

                    var valueResult = Activator.CreateInstance(singleItemType);
                    if (valueResult != null)
                    {
                        ParseJsonObjectToObject(jsonObject, valueResult);
                        resultArray.SetValue(valueResult, index);
                        index++;
                    }
                    else
                    {
                        DebugWrite($"Cannot create instance for JsonObject : {jsonObject.Stringify()}");
                    }
                }
                else if (jsonElement.ElementType == JsonElementType.Array)
                {
                    // json value
                    var jsonArray = (JsonElementArray)jsonElement;

                    if (IsArray(singleItemType))
                    {
                        var innerSingleItemType = singleItemType.GetTypeInfo().GetElementType();
                        if (innerSingleItemType != null)
                        {
                            var valueResult = Array.CreateInstance(innerSingleItemType, jsonArray.Count);
                            ParseJsonArrayToArray(jsonArray, valueResult, innerSingleItemType);
                            resultArray.SetValue(valueResult, index);
                            index++;
                        }
                        else
                        {
                            DebugWrite($"Cannot create array for JsonArray : {jsonArray.Stringify()}");
                        }
                    }
                    else if (IsGenericType(singleItemType))
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
                        else
                        {
                            DebugWrite($"Cannot create generic list for JsonArray : {jsonArray.Stringify()}");
                        }
                    }
                }
            }
        }

        private static void ParseJsonArrayToList(JsonElementArray sourceJsonArray, IList resultList, Type singleItemType)
        {
            foreach (var jsonElement in sourceJsonArray)
            {

                if (jsonElement.ElementType == JsonElementType.Object)
                {
                    var jsonObject = (JsonElementObject)jsonElement;

                    var resultObject = Activator.CreateInstance(singleItemType);
                    if (resultObject != null)
                    {
                        ParseJsonObjectToObject(jsonObject, resultObject);
                        resultList.Add(resultObject);
                    }
                    else
                    {
                        DebugWrite($"Cannot create instance for JsonObject : {jsonObject.Stringify()}");
                    }
                }
                else if (jsonElement.ElementType == JsonElementType.Array)
                {
                    var jsonArray = (JsonElementArray)jsonElement;

                    var innerSingleItemType = singleItemType.GetTypeInfo().GetElementType();
                    if (innerSingleItemType != null)
                    {
                        var innerArray = Array.CreateInstance(innerSingleItemType, jsonArray.Count);
                        ParseJsonArrayToArray(jsonArray, innerArray, innerSingleItemType);
                        resultList.Add(innerArray);
                    }
                    else
                    {
                        DebugWrite($"Cannot create array for JsonArray : {jsonArray.Stringify()}");
                    }
                }
                // json array of string "strings" : [ "a, "b", "c" ]
                else if (jsonElement.ElementType == JsonElementType.String)
                {
                    var valueResult = ((JsonElementString)jsonElement).Value;
                    if (IsString(singleItemType))
                    {
                        resultList.Add(valueResult);
                    }
                    else if (IsDateTime(singleItemType))
                    {
                        DateTime dateTimeResult = default(DateTime);
                        if (DateTime.TryParse(valueResult, out dateTimeResult))
                        {
                            try
                            {
                                resultList.Add(dateTimeResult);
                            }
                            catch (Exception)
                            {
                                DebugWrite($"Cannot set datetime value for : {valueResult}");
                            }
                        }
                    }
                }
                else if (jsonElement.ElementType == JsonElementType.Number)
                {
                    var valueResult = ((JsonElementNumber)jsonElement).Value;
                    if (IsInt(singleItemType)) resultList.Add((int)valueResult);
                    else resultList.Add(valueResult);
                }
                else if (jsonElement.ElementType == JsonElementType.Boolean)
                {
                    var valueResult = ((JsonElementBool)jsonElement).Value;
                    resultList.Add(valueResult);
                }
            }
        }

        // Mapping
        //{"myint":10} // json
        // public int MyInt {get;set} // property
        private static void ParseJsonObjectToObject(JsonElementObject sourceJsonObject, object resultObject)
        {
            var mapping = ResolveMapping(resultObject.GetType());

            foreach (var item in sourceJsonObject)
            {
                if (item.Value.ElementType == JsonElementType.String)
                {
                    var key = item.Key;

                    // json value
                    var valueResult = ((JsonElementString)sourceJsonObject[key]).Value;
                    // object property

                    var property = mapping[key];
                    if (property != null)
                    {
                        if (IsEnum(property.PropertyType))
                        {
                            try
                            {
                                var enumResult = Enum.Parse(property.PropertyType, valueResult);
                                property.SetValue(resultObject, enumResult);
                            }
                            catch (Exception)
                            {
                                DebugWrite($"Cannot set enum for : {valueResult}");
                            }
                        }
                        else
                        {
                            if (IsString(property.PropertyType))
                            {
                                property.SetValue(resultObject, valueResult);
                            }
                            else if (IsDateTime(property.PropertyType))
                            {
                                DateTime dateTimeResult = default(DateTime);
                                if (DateTime.TryParse(valueResult, out dateTimeResult))
                                {
                                    try
                                    {
                                        property.SetValue(resultObject, dateTimeResult);
                                    }
                                    catch (Exception)
                                    {
                                        DebugWrite($"Cannot set datetime value for : {valueResult} with {property.Name}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DebugWrite($"Cannot find property for : {resultObject.GetType().Name}");
                    }
                }
                else if (item.Value.ElementType == JsonElementType.Number)
                {
                    var key = item.Key;
                    // json value
                    var valueResult = ((JsonElementNumber)sourceJsonObject[key]).Value;
                    // object property
                    var property = mapping[key];
                    if (property != null)
                    {
                        if (IsInt(property.PropertyType))
                        {
                            property.SetValue(resultObject, (int)valueResult);
                        }
                        else if (IsDouble(property.PropertyType))
                        {
                            property.SetValue(resultObject, (double)valueResult);
                        }
                    }
                    else
                    {
                        DebugWrite($"Cannot find property for : {resultObject.GetType().Name}");
                    }
                }
                else if (item.Value.ElementType == JsonElementType.Boolean)
                {
                    var key = item.Key;
                    // json value
                    var valueResult = ((JsonElementBool)sourceJsonObject[key]).Value;
                    // object property
                    var property = mapping[key];
                    if (property != null)
                    {
                        property.SetValue(resultObject, valueResult);
                    }
                    else
                    {
                        DebugWrite($"Cannot find property for : {resultObject.GetType().Name}");
                    }
                }
                else if (item.Value.ElementType == JsonElementType.Array)
                {
                    var key = item.Key;
                    // named inner json array
                    var innerJsonArray = ((JsonElementArray)sourceJsonObject[key]);
                    // object property, get by name
                    var property = mapping[key];
                    if (property != null)
                    {
                        // Array ? or generic list
                        if (IsArray(property.PropertyType))
                        {
                            var singleItemType = property.PropertyType.GetTypeInfo().GetElementType();
                            if (singleItemType != null)
                            {
                                var valueResult = Array.CreateInstance(singleItemType, innerJsonArray.Count);
                                ParseJsonArrayToArray(innerJsonArray, valueResult, singleItemType);
                                property.SetValue(resultObject, valueResult);
                            }
                            else
                            {
                                DebugWrite($"Cannot create array for JsonArray : {innerJsonArray.Stringify()}");
                            }
                        }
                        else if (IsGenericType(property.PropertyType))
                        {
                            var singleItemType = property.PropertyType.GetGenericArguments()[0];
                            if (singleItemType != null)
                            {
                                var listType = typeof(List<>).MakeGenericType(singleItemType);
                                var valueResult = Activator.CreateInstance(listType);

                                ParseJsonArrayToList(innerJsonArray, (IList)valueResult, singleItemType);
                                property.SetValue(resultObject, valueResult);
                            }
                            else
                            {
                                DebugWrite($"Cannot create generic list for JsonArray : {innerJsonArray.Stringify()}");
                            }
                        }
                    }
                    else
                    {
                        DebugWrite($"Cannot find property for JsonArray : {innerJsonArray.Stringify()} in {resultObject.GetType().Name}");
                    }
                }
                else if (item.Value.ElementType == JsonElementType.Object)
                {
                    var key = item.Key;
                    // named inner json object "myitem" : { "inner" : "inner value" },
                    var innerJsonObject = ((JsonElementObject)sourceJsonObject[key]);

                    // find type by name example : "myitem" (name) > Item (type)
                    var property = mapping[key];
                    // typed
                    if (property != null)
                    {
                        // create anonymous dictionary string, object ?
                        if (IsDictionary(property.PropertyType)) // dynamic
                        {
                            // source : jsonObject, target : dictionary<string,object>
                            var innerDictionary = new Dictionary<string, object>();
                            ParseJsonObjectToDictionary(innerJsonObject, innerDictionary);
                            property.SetValue(resultObject, innerDictionary);
                        }
                        else if (IsObject(property.PropertyType))
                        {
                            var innerObject = Activator.CreateInstance(property.PropertyType);
                            if (innerObject != null)
                            {
                                ParseJsonObjectToObject(innerJsonObject, innerObject);
                                property.SetValue(resultObject, innerObject);
                            }
                            else
                            {
                                DebugWrite($"Cannot create instance for JsonObject : {innerJsonObject.Stringify()}");
                            }
                        }
                    }
                    else
                    {
                        DebugWrite($"Cannot find property for JsonObject : {innerJsonObject.Stringify()} in {resultObject.GetType().Name}");
                    }
                }
            }
        }

        private static void ParseJsonObjectToDictionary(JsonElementObject jsonObject, Dictionary<string, object> resultDictionary)
        {
            foreach (var item in jsonObject)
            {
                if (item.Value.ElementType == JsonElementType.String)
                {
                    var valueResult = ((JsonElementString)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (item.Value.ElementType == JsonElementType.Number)
                {
                    var valueResult = ((JsonElementNumber)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (item.Value.ElementType == JsonElementType.Boolean)
                {
                    var valueResult = ((JsonElementBool)jsonObject[item.Key]).Value;
                    resultDictionary[item.Key] = valueResult;
                }
                else if (item.Value.ElementType == JsonElementType.Object)
                {
                    var nextJsonObject = ((JsonElementObject)jsonObject[item.Key]);

                    var dictionary = new Dictionary<string, object>();
                    ParseJsonObjectToDictionary(nextJsonObject, dictionary);
                    resultDictionary[item.Key] = dictionary;
                }
            }
        }

        // receive a string (json)
        // return an object
        public static T Parse<T>(string json)
        {
            var resultType = typeof(T);
            if (IsEnumerable(resultType))
            {
                JsonElementArray rootJsonArray = null;
                if (JsonElementArray.TryParse(json, out rootJsonArray))
                {
                    if (IsArray(resultType))
                    {
                        var singleItemType = typeof(T).GetTypeInfo().GetElementType();
                        var resultArray = Array.CreateInstance(singleItemType, rootJsonArray.Count);

                        ParseJsonArrayToArray(rootJsonArray, resultArray, singleItemType);
                        return (T)(object)resultArray;
                    }
                    else if (IsGenericType(resultType))
                    {
                        var singleItemType = resultType.GetGenericArguments()[0];
                        var listType = typeof(List<>).MakeGenericType(singleItemType);
                        var resultList = Activator.CreateInstance(listType);

                        ParseJsonArrayToList(rootJsonArray, (IList)resultList, singleItemType);

                        return (T)(object)resultList;
                    }
                }
            }
            else
            {
                var resultObject = Activator.CreateInstance(typeof(T));
                JsonElementObject rootJsonObject = null;
                if (JsonElementObject.TryParse(json, out rootJsonObject))
                {
                    ParseJsonObjectToObject(rootJsonObject, resultObject);
                    return (T)resultObject;
                }
            }
            return default(T);
        }

        private static void InspectList(IEnumerable list, JsonElementArray resultJsonArray)
        {
            foreach (var item in list)
            {
                var itemType = item.GetType();
                if (IsString(itemType))
                {
                    resultJsonArray.Add(JsonElement.CreateString((string)item));
                }
                else if (IsInt(itemType))
                {
                    resultJsonArray.Add(JsonElement.CreateNumber((int)item));
                }
                else if (IsDouble(itemType))
                {
                    resultJsonArray.Add(JsonElement.CreateNumber((double)item));
                }
                else if (IsBool(itemType))
                {
                    resultJsonArray.Add(JsonElement.CreateBoolean((bool)item));
                }
                else if (IsDateTime(itemType))
                {
                    resultJsonArray.Add(JsonElement.CreateString(item.ToString()));
                }
                else if (IsEnumerable(itemType))
                {
                    JsonElementArray jsonArray = new JsonElementArray();
                    InspectList((IEnumerable)item, jsonArray);
                    resultJsonArray.Add(jsonArray);
                }
                else if (IsObject(itemType))
                {
                    JsonElementObject innerObject = new JsonElementObject();
                    InspectObject(item, innerObject);
                    resultJsonArray.Add(innerObject);
                }
            }
        }

        private static Dictionary<string, PropertyInfo> ResolveMapping(Type type)
        {
            if (cache.ContainsKey(type))
            {
                return cache[type];
            }
            else
            {
                var result = new Dictionary<string, PropertyInfo>();

                var propertiesInfos = type.GetProperties().ToList<PropertyInfo>();
                foreach (var property in propertiesInfos)
                {
                    bool isFound = false;
                    // Attribute
                    if (UseJsonMapAttributes)
                    {
                        var attribute = Attribute.GetCustomAttribute(property, typeof(JsonMapAttribute)) as JsonMapAttribute;
                        if (attribute != null)
                        {
                            // add column name / property
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
                cache[type] = result;
                return result;
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
                if (IsString(propertyType))
                {
                    resultJsonObject[key] = JsonElement.CreateString((string)property.GetValue(obj));
                }
                else if (IsInt(propertyType))
                {
                    resultJsonObject[key] = JsonElement.CreateNumber((int)property.GetValue(obj));
                }
                else if (IsDouble(propertyType))
                {
                    resultJsonObject[key] = JsonElement.CreateNumber((double)property.GetValue(obj));
                }
                else if (IsBool(propertyType))
                {
                    resultJsonObject[key] = JsonElement.CreateBoolean((bool)property.GetValue(obj));
                }
                else if (IsDateTime(propertyType) || IsEnum(propertyType))
                {
                    resultJsonObject[key] = JsonElement.CreateString(property.GetValue(obj).ToString());
                }
                else if (IsEnumerable(propertyType))
                {
                    JsonElementArray innerJsonArray = new JsonElementArray();
                    var innerEnumerable = property.GetValue(obj);
                    if (innerEnumerable != null)
                    {
                        InspectList((IEnumerable)innerEnumerable, innerJsonArray);
                        resultJsonObject[key] = innerJsonArray;
                    }
                }
                else if (IsObject(propertyType))
                {
                    JsonElementObject innerJsonObject = new JsonElementObject();
                    var innerObj = (object)property.GetValue(obj);
                    if (innerObj != null)
                    {
                        InspectObject(innerObj, innerJsonObject);
                        resultJsonObject[key] = innerJsonObject;
                    }
                }
            }
        }

        // receive an object
        // return a string
        public static string Stringify(object obj)
        {
            var objType = obj.GetType();
            if (IsEnumerable(objType))
            {
                JsonElementArray result = new JsonElementArray();
                InspectList((IEnumerable)obj, result);

                var json = result.Stringify();
                return json;
            }
            else
            {
                JsonElementObject result = new JsonElementObject();
                InspectObject(obj, result);

                var json = result.Stringify();
                return json;
            }
        }
    }
}
