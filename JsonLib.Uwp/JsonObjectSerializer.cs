using JsonLib.Mappings;

namespace JsonLib
{
    public class JsonObjectSerializer
    {
        internal static IJsonObjectSerializerService serializationService;

        public static bool CacheIsActive => serializationService.CacheIsActive;

        static JsonObjectSerializer()
        {
            serializationService = new JsonObjectSerializerService();
        }

        public static void ActiveCache(bool value = true)
        {
            serializationService.ActiveCache(value);
        }

        public static string Stringify(object value, MappingContainer mappings = null)
        {
            return serializationService.Stringify(value, mappings);
        }

        public static string StringifyAndBeautify(object value, MappingContainer mappings = null)
        {
            return serializationService.StringifyAndBeautify(value, mappings);
        }

        public static T Parse<T>(string json, MappingContainer mappings = null)
        {
            return serializationService.Parse<T>(json, mappings);
        }
    }
}
