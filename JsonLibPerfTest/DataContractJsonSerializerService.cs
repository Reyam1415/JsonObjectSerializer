﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace UwpLib.Services.Serialization
{
    public class DataJsonSerializer
    {
        internal static IDataContractJsonSerializerService serializerService;

        static DataJsonSerializer()
        {
            serializerService = new DataContractJsonSerializerService();
        }


        public static List<Type> ResolveTypes(Type type, List<Type> knownTypes = null)
        {
            return serializerService.ResolveTypes(type, knownTypes);
        }

        public static string Stringify(object obj, DataContractJsonSerializerSettings settings)
        {
            return serializerService.Stringify(obj, settings);
        }

        public static string Stringify(object obj, List<Type> knownTypes)
        {
            return serializerService.Stringify(obj, knownTypes);
        }

        public static bool TryStringify(object obj, out string result)
        {
            return serializerService.TryStringify(obj, out result);
        }

        public static T Parse<T>(string json, DataContractJsonSerializerSettings settings)
        {
            return serializerService.Parse<T>(json, settings);
        }

        public static T Parse<T>(string json, List<Type> knownTypes)
        {
            return serializerService.Parse<T>(json, knownTypes);
        }

        public static bool TryParse<T>(string json, out T result)
        {
            return serializerService.TryParse<T>(json, out result);
        }

    }

    public class DataContractJsonSerializerService : IDataContractJsonSerializerService
    {
        public List<Type> ResolveTypes(Type type, List<Type> knownTypes = null)
        {
            if (knownTypes == null)
            {
                knownTypes = new List<Type>();
            }
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
                    this.ResolveTypes(propertyType, knownTypes);
                }
            }
            return knownTypes;
        }


        public string Stringify(object obj, DataContractJsonSerializerSettings settings)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(object), settings);
                serializer.WriteObject(stream, obj);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }

        public string Stringify(object obj, List<Type> knownTypes)
        {
            return Stringify(obj, new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            });
        }

        public bool TryStringify(object obj, out string result)
        {
            try
            {
                var knownTypes = this.ResolveTypes(obj.GetType());
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(object), knownTypes);
                    serializer.WriteObject(stream, obj);

                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                result = string.Empty;
                return false;
            }
        }

        public T Parse<T>(string json, DataContractJsonSerializerSettings settings)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T), settings);
                var result = (T)serializer.ReadObject(stream);
                return result;
            }
        }

        public T Parse<T>(string json, List<Type> knownTypes)
        {
            return Parse<T>(json, new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            });
        }

        public bool TryParse<T>(string json, out T result)
        {
            try
            {
                var knownTypes = this.ResolveTypes(typeof(T));
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
