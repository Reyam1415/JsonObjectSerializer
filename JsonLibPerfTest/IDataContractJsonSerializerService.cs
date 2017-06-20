using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace UwpLib.Services.Serialization
{
    public interface IDataContractJsonSerializerService
    {
        List<Type> ResolveTypes(Type type, List<Type> knownTypes = null);
        T Parse<T>(string json, DataContractJsonSerializerSettings settings);
        T Parse<T>(string json, List<Type> knownTypes);
        string Stringify(object obj, DataContractJsonSerializerSettings settings);
        string Stringify(object obj, List<Type> knownTypes);
        bool TryParse<T>(string json, out T result);
        bool TryStringify(object obj, out string result);
    }
}