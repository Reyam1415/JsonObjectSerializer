using System.Collections.Generic;

namespace JsonLib
{

    public class JsonElementArray : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Array;

        public List<IJsonElementValue> Values { get; set; }

        public JsonElementArray()
        {
            this.Values = new List<IJsonElementValue>();
        }

        public JsonElementArray Add(IJsonElementValue value)
        {
            this.Values.Add(value);
            return this;
        }

        public JsonElementArray AddString(string value)
        {
            this.Values.Add(JsonElementValue.CreateString(value));
            return this;
        }

        public JsonElementArray AddNumber(object value)
        {
            this.Values.Add(JsonElementValue.CreateNumber(value));
            return this;
        }

        public JsonElementArray AddBool(bool value)
        {
            this.Values.Add(JsonElementValue.CreateBool(value));
            return this;
        }

        public JsonElementArray AddNullable(object value)
        {
            this.Values.Add(JsonElementValue.CreateNullable(value));
            return this;
        }

        public JsonElementArray AddObject(JsonElementObject value)
        {
            this.Values.Add(value);
            return this;
        }

        public JsonElementArray AddArray(JsonElementArray value)
        {
            this.Values.Add(value);
            return this;
        }
    }

}
