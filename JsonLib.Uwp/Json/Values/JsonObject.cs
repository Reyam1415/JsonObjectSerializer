using System;
using System.Collections.Generic;

namespace JsonLib.Json
{
    public class JsonObject : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.Object;

        public Dictionary<string, IJsonValue> Values { get; set; }

        public JsonObject()
        {
            this.Values = new Dictionary<string, IJsonValue>();
        }

        public bool HasValue(string key)
        {
            return this.Values.ContainsKey(key);
        }

        public JsonObject Add(string key, IJsonValue value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = value;
            return this;
        }

        public JsonObject AddString(string key, string value)
        {
            return this.Add(key, new JsonString(value));
        }

        public JsonObject AddNumber(string key, object value)
        {
            return this.Add(key, new JsonNumber(value));
        }

        public JsonObject AddBool(string key, bool value)
        {
            return this.Add(key, new JsonBool(value));
        }

        public JsonObject AddNullable(string key, object value)
        {
            return this.Add(key, new JsonNullable(value));
        }

        public JsonObject AddObject(string key, JsonObject value)
        {
            return this.Add(key, value);
        }

        public JsonObject AddArray(string key, JsonArray value)
        {
            return this.Add(key, value);
        }
    }

}
