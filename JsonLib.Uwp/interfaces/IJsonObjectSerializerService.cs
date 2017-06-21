﻿using JsonLib.Mappings;

namespace JsonLib
{
    public interface IJsonObjectSerializerService
    {
        bool CacheIsActive { get; }

        void ActiveCache(bool value = true);
        bool CacheHas<T>(string json);
        T Parse<T>(string json, MappingContainer mappings = null);
        string Stringify<T>(T value, MappingContainer mappings = null);
        string StringifyAndBeautify<T>(T value, MappingContainer mappings = null);
    }
}