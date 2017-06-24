using System.Collections.Generic;

namespace JsonLib.Json.Cache
{
    public class JsonCacheService : IJsonCacheService
    {
        internal Dictionary<string, JsonCacheItem> container;
        internal int cacheSize;

        public int Count => this.container.Count;

        public JsonCacheService()
        {
            this.container = new Dictionary<string, JsonCacheItem>();
            this.cacheSize = 10;
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
            this.CheckClear();
            this.container[json] = new JsonCacheItem(json, typeof(T), result);
        }

        public void CheckClear()
        {
            if (this.container.Count + 1 >= this.cacheSize)
            {
                this.Clear();
            }
        }

        public void Clear()
        {
            this.container.Clear();
        }
    }

}
