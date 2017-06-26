using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public class JsonToObject : IJsonToObject
    {
        protected IJsonToJsonValue jsonToJsonValue;
        protected IJsonValueToObject jsonValueToObject;

        public JsonToObject()
            :this(new JsonToJsonValue(), new JsonValueToObject())
        { }

        public JsonToObject(
            IJsonToJsonValue jsonToJsonValue,
            IJsonValueToObject jsonValueToObject)
        {
            this.jsonToJsonValue = jsonToJsonValue;
            this.jsonValueToObject = jsonValueToObject;
        }

        public T ToObject<T>(string json, JsonMappingContainer mappings = null)
        {
            var jsonValue = this.jsonToJsonValue.ToJsonValue(json);
            return this.jsonValueToObject.Resolve<T>(jsonValue, mappings);
        }

    }
}
