using JsonLib.Common;
using JsonLib.Json;
using JsonLib.Json.Cache;
using JsonLib.Json.Mappings;
using JsonLib.Mappings.Xml;
using JsonLib.Xml;

namespace JsonLib
{
    public class JsonObjectSerializerService : IJsonObjectSerializerService
    {
        protected IObjectToJson objectToJson;
        protected IJsonToObject jsonToObject;
        protected IBeautifier jsonBeautifier;
        protected IJsonCacheService jsonCacheService;

        protected IObjectToXml objectToXml;
        protected IXmlToObject xmlToObject;
        protected IBeautifier xmlBeautifier;

        public bool CacheIsActive { get; protected set; }

        public JsonObjectSerializerService()
            :this(new ObjectToJson(), 
                 new JsonToObject(),
                 new ObjectToXml(), 
                 new XmlToObject(),
                 new JsonCacheService(), 
                 new JsonBeautifier(), 
                 new XmlBeautifier())
        {  }

        public JsonObjectSerializerService(
            IObjectToJson objectToJson, 
            IJsonToObject jsonToObject,
            IObjectToXml objectToXml,
            IXmlToObject xmlToObject,
            IJsonCacheService jsonCacheService, 
            IBeautifier jsonBeautifier,
            IBeautifier xmlBeautifier)
        {
            this.objectToJson = objectToJson;
            this.jsonToObject = jsonToObject;
            this.jsonBeautifier = jsonBeautifier;
            this.jsonCacheService = jsonCacheService;

            this.objectToXml = objectToXml;
            this.xmlToObject = xmlToObject;
            this.xmlBeautifier = xmlBeautifier;

            this.CacheIsActive = true;
        }

        public void ActiveCache(bool value = true)
        {
            this.CacheIsActive = value;
            if (!this.CacheIsActive)
            {
                this.jsonCacheService.Clear();
            }
        }

        public string Stringify<T>(T value,JsonMappingContainer mappings = null)
        {
            return this.objectToJson.ToJson<T>(value, mappings);
        }

        public string StringifyAndBeautify<T>(T value, JsonMappingContainer mappings = null)
        {
            var json = this.Stringify<T>(value, mappings);
            return this.jsonBeautifier.Format(json);
        }

        public T Parse<T>(string json, JsonMappingContainer mappings = null)
        {
            if (this.jsonCacheService.Has<T>(json))
            {
                return (T)this.jsonCacheService.GetResult<T>(json);
            }
            else
            {
                var result = this.jsonToObject.ToObject<T>(json, mappings);
                if (this.CacheIsActive)
                {
                    this.jsonCacheService.Set<T>(json, result);
                }
                return result;
            }
        }

        public string ToXml<T>(T value, XmlMappingContainer mappings = null)
        {
            return this.objectToXml.ToXml<T>(value, mappings);
        }

        public string ToXmlAndBeautify<T>(T value, XmlMappingContainer mappings = null)
        {
            var xml = this.objectToXml.ToXml<T>(value, mappings);
            return this.xmlBeautifier.Format(xml);
        }

        public T FromXml<T>(string xml, XmlMappingContainer mappings = null)
        {
            return this.xmlToObject.ToObject<T>(xml, mappings);
        }
    }
}
