using JsonLib.Json.Mappings;

namespace JsonLib.Json
{
    public class JsonToObject : IJsonToObject
    {
        protected IJsonToJsonValue jsonToJsonValue;
        protected IJsonValueToObject jsonValueToObject;

        public JsonToObject()
            :this(new JsonToJsonValue(), new JsonValueToObject())
        {

        }
        public JsonToObject(
            IJsonToJsonValue jsonToJsonValue,
            IJsonValueToObject jsonValueToObject)
        {
            this.jsonToJsonValue = jsonToJsonValue;
            this.jsonValueToObject = jsonValueToObject;
        }

        public T ToObject<T>(string json, JsonMappingContainer mappings = null)
        {
            var type = typeof(T);
            var jsonValue = this.jsonToJsonValue.ToJsonValue(json);
            if (jsonValue.ValueType == JsonValueType.Object)
            {
                return (T)this.jsonValueToObject.ToObject(type, (JsonObject)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonValueType.Array)
            {
                return (T)this.jsonValueToObject.ToEnumerable(type, (JsonArray)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonValueType.String)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Number)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Bool)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Nullable)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonNullable)jsonValue);
            }

            throw new JsonLibException("Cannot resolve object for json");
        }

    }
}
