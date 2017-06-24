using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public class ObjectToJson : IObjectToJson
    {
        protected IObjectToJsonValue objectToJsonValue;
        protected IJsonValueToJson jsonValueToJson;

        public ObjectToJson()
            :this(new ObjectToJsonValue(), new JsonValueToJson())
        {

        }
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
            if (jsonValue.ValueType == JsonValueType.Object)
            {
                return this.jsonValueToJson.ToObject((JsonObject)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Array)
            {
                return this.jsonValueToJson.ToArray((JsonArray)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.String)
            {
                return this.jsonValueToJson.ToString((JsonString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Number)
            {
                return this.jsonValueToJson.ToNumber((JsonNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Bool)
            {
                return this.jsonValueToJson.ToBool((JsonBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Nullable)
            {
                return this.jsonValueToJson.ToNullable((JsonNullable)jsonValue);
            }

            throw new JsonLibException("Cannot resolve json for object");
        }

    }
}
