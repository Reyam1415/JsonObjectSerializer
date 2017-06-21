using System;
using System.Collections.Generic;

namespace JsonLib
{
    public class JsonElementObject : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Object;

        public Dictionary<string, IJsonElementValue> Values { get; set; }

        public JsonElementObject()
        {
            this.Values = new Dictionary<string, IJsonElementValue>();
        }

        public bool HasValue(string key)
        {
            return this.Values.ContainsKey(key);
        }

        public JsonElementObject Add(string key, IJsonElementValue value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = value;
            return this;
        }

        public JsonElementObject AddString(string key, string value)
        {
            return this.Add(key, new JsonElementString(value));
        }

        public JsonElementObject AddNumber(string key, object value)
        {
            return this.Add(key, new JsonElementNumber(value));
        }

        public JsonElementObject AddBool(string key, bool value)
        {
            return this.Add(key, new JsonElementBool(value));
        }

        public JsonElementObject AddNullable(string key, object value)
        {
            return this.Add(key, new JsonElementNullable(value));
        }

        public JsonElementObject AddObject(string key, JsonElementObject value)
        {
            return this.Add(key, value);
        }

        public JsonElementObject AddArray(string key, JsonElementArray value)
        {
            return this.Add(key, value);
        }
    }

}
