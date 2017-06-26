using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public class ObjectToJson : IObjectToJson
    {
        protected IObjectToJsonValue objectToJsonValue;
        protected IJsonValueToJson jsonValueToJson;

        public ObjectToJson()
            :this(new ObjectToJsonValue(), new JsonValueToJson())
        { }

        public ObjectToJson(
            IObjectToJsonValue objectToJsonValueConverter,
            IJsonValueToJson jsonValueToJsonConverter)
        {
            this.objectToJsonValue = objectToJsonValueConverter;
            this.jsonValueToJson = jsonValueToJsonConverter;
        }

        public string ToJson<T>(T value, JsonMappingContainer mappings = null)
        {
            var jsonValue = this.objectToJsonValue.ToJsonValue<T>(value, mappings);
            return this.jsonValueToJson.Resolve(jsonValue);
        }

    }
}
