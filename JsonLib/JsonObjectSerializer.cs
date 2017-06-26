using JsonLib.Json.Mappings;
using JsonLib.Mappings.Xml;

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

        public static string Stringify<T>(T value, JsonMappingContainer mappings = null)
        {
            return serializationService.Stringify<T>(value, mappings);
        }

        public static string StringifyAndBeautify<T>(T value, JsonMappingContainer mappings = null)
        {
            return serializationService.StringifyAndBeautify<T>(value, mappings);
        }

        public static T Parse<T>(string json, JsonMappingContainer mappings = null)
        {
            return serializationService.Parse<T>(json, mappings);
        }

        public static string ToXml<T>(T value, XmlMappingContainer mappings = null)
        {
            return serializationService.ToXml<T>(value, mappings);
        }

        public static string ToXmlAndBeautify<T>(T value, XmlMappingContainer mappings = null)
        {
            return serializationService.ToXmlAndBeautify<T>(value, mappings);
        }

        public static T FromXml<T>(string xml, XmlMappingContainer mappings = null)
        {
            return serializationService.FromXml<T>(xml, mappings);
        }
    }
}
