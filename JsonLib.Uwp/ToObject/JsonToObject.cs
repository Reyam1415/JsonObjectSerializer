using JsonLib.Mappings;
using System;

namespace JsonLib
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

        public T ToObject<T>(string json, MappingContainer mappings = null)
        {
            var type = typeof(T);
            var jsonValue = this.jsonToJsonValue.ToJsonValue(json);
            if (jsonValue.ValueType == JsonElementValueType.Object)
            {
                return (T)this.jsonValueToObject.ToObject(type, (JsonElementObject)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Array)
            {
                return (T)this.jsonValueToObject.ToEnumerable(type, (JsonElementArray)jsonValue, mappings);
            }
            else if (jsonValue.ValueType == JsonElementValueType.String)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonElementString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Number)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonElementNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Bool)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonElementBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonElementValueType.Null)
            {
                return (T)this.jsonValueToObject.ToValue(type, (JsonElementNullable)jsonValue);
            }

            throw new JsonLibException("Cannot resolve object for json");
        }

    }
}
