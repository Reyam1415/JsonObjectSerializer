using JsonLib.Mappings;

namespace JsonLib
{
    public class JsonObjectSerializerService : IJsonObjectSerializerService
    {
        protected IObjectToJson objectToJson;
        protected IJsonToObject jsonToObject;
        protected IBeautifier beautifier;
        protected IJsonCacheService jsonCacheService;
        public bool CacheIsActive { get; protected set; }

        public JsonObjectSerializerService()
            :this(new ObjectToJson(), new JsonToObject(), new JsonCacheService(), new Beautifier())
        {  }

        public JsonObjectSerializerService(
            IObjectToJson objectToJson, 
            IJsonToObject jsonToObject, 
            IJsonCacheService jsonCacheService, 
            IBeautifier beautifier)
        {
            this.objectToJson = objectToJson;
            this.jsonToObject = jsonToObject;
            this.beautifier = beautifier;
            this.jsonCacheService = jsonCacheService;
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

        public bool CacheHas<T>(string json)
        {
           return this.jsonCacheService.Has<T>(json);
        }

        public string Stringify<T>(T value,MappingContainer mappings = null)
        {
            return this.objectToJson.ToJson<T>(value, mappings);
        }

        public string StringifyAndBeautify<T>(T value, MappingContainer mappings = null)
        {
            var json = this.Stringify<T>(value, mappings);
            return this.beautifier.Format(json);
        }

        public T Parse<T>(string json, MappingContainer mappings = null)
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
    }
}
