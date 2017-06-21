using JsonLib.Mappings;

namespace JsonLib
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

        public string ToJson<T>(T value, MappingContainer mappings = null)
        {

            var jsonValue = this.objectToJsonValue.ToJsonValue<T>(value, mappings);
            if (jsonValue.ValueType == JsonElementValueType.Object)
            {
                return this.jsonValueToJson.ToObject((JsonElementObject)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Array)
            {
                return this.jsonValueToJson.ToArray((JsonElementArray)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.String)
            {
                return this.jsonValueToJson.ToString((JsonElementString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Number)
            {
                return this.jsonValueToJson.ToNumber((JsonElementNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Bool)
            {
                return this.jsonValueToJson.ToBool((JsonElementBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Null)
            {
                return this.jsonValueToJson.ToNullable((JsonElementNullable)jsonValue);
            }

            throw new JsonLibException("Cannot resolve json for object");
        }

    }
}
