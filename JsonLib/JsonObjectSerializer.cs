using JsonLib.Mappings;

namespace JsonLib
{
    public class JsonObjectSerializer
    {
        internal static IJsonObjectSerializerService easyJsonService;
        internal static IJsonCacheService jsonCacheService;

        internal static bool cacheIsActive;

        static JsonObjectSerializer()
        {
            easyJsonService = new JsonObjectSerializerService();
            jsonCacheService = new JsonCacheService();
            cacheIsActive = true;
        }

        public static void ActiveCache(bool value = true)
        {
            cacheIsActive = value;
            if (!cacheIsActive)
            {
                jsonCacheService.Clear();
            }
        }

        public static string Stringify(object value, MappingContainer mappings = null)
        {
            return easyJsonService.Stringify(value, mappings);
        }

        public static string StringifyAndBeautify(object value, MappingContainer mappings = null)
        {
            return easyJsonService.StringifyAndBeautify(value, mappings);
        }

        public static T Parse<T>(string json, MappingContainer mappings = null)
        {
            if (jsonCacheService.Has<T>(json))
            {
                return (T)jsonCacheService.GetResult<T>(json);
            }
            else
            {
                var result = (T)easyJsonService.Parse<T>(json, mappings);
                if (cacheIsActive)
                {
                    jsonCacheService.Set<T>(json, result);
                }
                return result;
            }
        }
    }
}
