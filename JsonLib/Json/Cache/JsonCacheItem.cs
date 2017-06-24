using System;

namespace JsonLib.Json.Cache
{

    public class JsonCacheItem
    {
        public Type Type { get; }
        public object Result { get; }
        public string Json { get; }

        public JsonCacheItem(string json, Type type, object result)
        {
            this.Json = json;
            this.Type = type;
            this.Result = result;
        }
    }

}
