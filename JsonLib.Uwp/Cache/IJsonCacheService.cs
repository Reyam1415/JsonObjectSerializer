namespace JsonLib
{
    public interface IJsonCacheService
    {
        int Count { get; }

        void CheckClear();
        void Clear();
        JsonCacheItem Get<T>(string json);
        object GetResult<T>(string json);
        bool Has<T>(string json);
        void Set<T>(string json, object result);
    }
}