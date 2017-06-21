using System.Collections.Generic;

namespace JsonLib
{

    public class JsonValueToJson : IJsonValueToJson
    {
        protected IJsonValueToJsonService writerService;

        public JsonValueToJson()
            :this(new JsonValueToJsonService())
        { }

        public JsonValueToJson(IJsonValueToJsonService writerService)
        {
            this.writerService = writerService;
        }

        public string ToString(JsonElementString element)
        {
            return this.writerService.GetString(element.Value);
        }

        public string ToNumber(JsonElementNumber element)
        {
            return this.writerService.GetNumber(element.Value);
        }

        public string ToBool(JsonElementBool element)
        {
            return this.writerService.GetBool(element.Value);
        }

        public string ToNullable(JsonElementNullable element)
        {
            return this.writerService.GetNullable(element.Value);
        }

        public string ToArray(JsonElementArray jsonArray)
        {
            var result = new List<string>();

            foreach (var jsonValue in jsonArray.Values)
            {
                if(jsonValue.ValueType == JsonElementValueType.String)
                {
                    result.Add(this.writerService.GetString(((JsonElementString)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Number)
                {
                    result.Add(this.writerService.GetNumber(((JsonElementNumber)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Bool)
                {
                    result.Add(this.writerService.GetBool(((JsonElementBool)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Null)
                {
                    result.Add(this.writerService.GetNullable(((JsonElementNullable)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Array)
                {
                    result.Add(this.ToArray((JsonElementArray)jsonValue));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Object)
                {
                    result.Add(this.ToObject((JsonElementObject)jsonValue));
                }
            }
            return "[" + string.Join(",", result) + "]";
        }

        public string ToObject(JsonElementObject jsonObject)
        {
            var result = new List<string>();
            foreach (var keyValue in jsonObject.Values)
            {
                var key = keyValue.Key;
                var jsonValue = keyValue.Value;
                if (jsonValue.ValueType == JsonElementValueType.String)
                {
                    result.Add(this.writerService.GetString(key, ((JsonElementString)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Number)
                {
                    result.Add(this.writerService.GetNumber(key, ((JsonElementNumber)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Bool)
                {
                    result.Add(this.writerService.GetBool(key, ((JsonElementBool)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Null)
                {
                    result.Add(this.writerService.GetNullable(key, ((JsonElementNullable)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Array)
                {
                    result.Add(this.writerService.GetKey(key) + ":" + this.ToArray((JsonElementArray)jsonValue));
                }
                else if (jsonValue.ValueType == JsonElementValueType.Object)
                {
                    result.Add(this.writerService.GetKey(key) + ":" + this.ToObject((JsonElementObject)jsonValue));
                }
            }
            return "{" + string.Join(",", result) + "}";
        }

    }
}
