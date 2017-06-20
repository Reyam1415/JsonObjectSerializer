using JsonLib.Mappings;

namespace JsonLib
{
    public class JsonObjectSerializerService : IJsonObjectSerializerService
    {
        protected IObjectToJson objectToJson;
        protected IJsonToObject jsonToObject;
        protected IBeautifier beautifier;

        public JsonObjectSerializerService()
            :this(new ObjectToJson(), new JsonToObject(), new Beautifier())
        {  }

        public JsonObjectSerializerService(IObjectToJson objectToJson, IJsonToObject jsonToObject, IBeautifier beautifier)
        {
            this.objectToJson = objectToJson;
            this.jsonToObject = jsonToObject;
            this.beautifier = beautifier;
        }

        public string Stringify(object value,MappingContainer mappings = null)
        {
            return this.objectToJson.ToJson(value, mappings);
        }

        public string StringifyAndBeautify(object value, MappingContainer mappings = null)
        {
            var json = this.Stringify(value, mappings);
            return this.beautifier.Format(json);
        }

        public T Parse<T>(string json, MappingContainer mappings = null)
        {
            return (T)this.jsonToObject.ToObject<T>(json, mappings);
        }
    }
}
