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
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = JsonElementValue.CreateString(value);
            return this;
        }

        public JsonElementObject AddNumber(string key, object value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = JsonElementValue.CreateNumber(value);
            return this;
        }

        public JsonElementObject AddBool(string key, bool value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = JsonElementValue.CreateBool(value);
            return this;
        }

        public JsonElementObject AddNullable(string key, object value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = JsonElementValue.CreateNullable(value);
            return this;
        }

        public JsonElementObject AddObject(string key, JsonElementObject value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = value;
            return this;
        }

        public JsonElementObject AddArray(string key, JsonElementArray value)
        {
            if (this.HasValue(key)) { throw new JsonLibException("A value with the name " + key + " is already registered"); }

            this.Values[key] = value;
            return this;
        }
    }

}
