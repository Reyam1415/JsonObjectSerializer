using System;
using System.Collections.Generic;

namespace JsonLib
{
    public class JsonCacheService : IJsonCacheService
    {
        internal Dictionary<string, JsonCacheItem> container;

        public int Count => this.container.Count;

        public JsonCacheService()
        {
            this.container = new Dictionary<string, JsonCacheItem>();
        }

        public bool Has<T>(string json)
        {
            return this.container.ContainsKey(json) 
                && container[json].Type == typeof(T);
        }

        public JsonCacheItem Get<T>(string json)
        {
            if (!this.Has<T>(json)) { throw new JsonLibException("No cached item found"); }

            return container[json];
        }

        public object GetResult<T>(string json)
        {
            return this.Get<T>(json).Result;
        }

        public void Set<T>(string json, object result)
        {
            this.container[json] = new JsonCacheItem(json, typeof(T), result);
        }

        public void Clear()
        {
            this.container.Clear();
        }
    }

}
