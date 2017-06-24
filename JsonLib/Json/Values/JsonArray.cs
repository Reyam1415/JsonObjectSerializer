using System.Collections.Generic;

namespace JsonLib.Json
{

    public class JsonArray : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.Array;

        public List<IJsonValue> Values { get; set; }

        public JsonArray()
        {
            this.Values = new List<IJsonValue>();
        }

        public JsonArray Add(IJsonValue value)
        {
            this.Values.Add(value);
            return this;
        }

        public JsonArray AddString(string value)
        {
           return this.Add(JsonValue.CreateString(value));
        }

        public JsonArray AddNumber(object value)
        {
            return this.Add(JsonValue.CreateNumber(value));
        }

        public JsonArray AddBool(bool value)
        {
            return this.Add(JsonValue.CreateBool(value));
        }

        public JsonArray AddNullable(object value)
        {
            return this.Add(JsonValue.CreateNullable(value));
        }

        public JsonArray AddObject(JsonObject value)
        {
            return this.Add(value);
        }

        public JsonArray AddArray(JsonArray value)
        {
            return this.Add(value);
        }
    }

}
