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
           return this.Add(JsonElementValue.CreateString(value));
        }

        public JsonElementArray AddNumber(object value)
        {
            return this.Add(JsonElementValue.CreateNumber(value));
        }

        public JsonElementArray AddBool(bool value)
        {
            return this.Add(JsonElementValue.CreateBool(value));
        }

        public JsonElementArray AddNullable(object value)
        {
            return this.Add(JsonElementValue.CreateNullable(value));
        }

        public JsonElementArray AddObject(JsonElementObject value)
        {
            return this.Add(value);
        }

        public JsonElementArray AddArray(JsonElementArray value)
        {
            return this.Add(value);
        }
    }

}
