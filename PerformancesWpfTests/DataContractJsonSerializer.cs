using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WpfLib.Services
{
    public class SerializationHelper
    {
        public static void FindKnownTypesOf(Type type, List<Type> knownTypes)
        {
            if (type.Namespace != "System" && !knownTypes.Contains(type))
            {
                knownTypes.Add(type);
            }
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                var propertyType = propertyInfo.PropertyType;
                if (propertyType.Namespace != "System"
                    && !knownTypes.Contains(propertyType)
                    && type.GetTypeInfo().IsClass)
                {
                    FindKnownTypesOf(propertyType, knownTypes);
                }
            }
        }
    }

    public class DataJsonSerializer
    {

        // receive an object
        // returns a string
        public static string Stringify(object obj, List<Type> knownTypes)
        {
            return Stringify(obj, new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            });
        }

        public static string Stringify(object obj, DataContractJsonSerializerSettings settings)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(object), settings); // json
                    serializer.WriteObject(stream, obj);

                    // read
                    stream.Position = 0;
                    var reader = new StreamReader(stream);
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // receive a string (json)
        // returns an object (T typed)
        public static T Parse<T>(string json, List<Type> knownTypes)
        {
            return Parse<T>(json, new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            });
        }

        public static T Parse<T>(string json, DataContractJsonSerializerSettings settings)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T),settings);
                    var result = (T)serializer.ReadObject(stream);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool TryStringify(object obj, out string result)
        {
            try
            {
                var knownTypes = new List<Type>();
                SerializationHelper.FindKnownTypesOf(obj.GetType(), knownTypes);
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(object), knownTypes); // json
                    serializer.WriteObject(stream, obj);

                    // read
                    stream.Position = 0;
                    var reader = new StreamReader(stream);
                    result = reader.ReadToEnd();
                    return true;
                }
            }
            catch (Exception)
            {
                result = string.Empty;
                return false;
            }
        }

        public static bool TryParse<T>(string json, out T result)
        {
            try
            {
                var knownTypes = new List<Type>();
                var type = typeof(T);
                SerializationHelper.FindKnownTypesOf(type, knownTypes);
                using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
                    result = (T)serializer.ReadObject(stream);
                    return true;
                }
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

    }

}
