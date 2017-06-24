using System.Collections.Generic;

namespace JsonLib.Json
{

    public class JsonValueToJson : IJsonValueToJson
    {
        protected IJsonValueToJsonService jsonService;

        public JsonValueToJson()
            :this(new JsonValueToJsonService())
        { }

        public JsonValueToJson(IJsonValueToJsonService jsonService)
        {
            this.jsonService = jsonService;
        }

        public string ToString(JsonString element)
        {
            return this.jsonService.GetString(element.Value);
        }

        public string ToNumber(JsonNumber element)
        {
            return this.jsonService.GetNumber(element.Value);
        }

        public string ToBool(JsonBool element)
        {
            return this.jsonService.GetBool(element.Value);
        }

        public string ToNullable(JsonNullable element)
        {
            return this.jsonService.GetNullable(element.Value);
        }

        public string ToArray(JsonArray jsonArray)
        {
            var result = new List<string>();

            foreach (var jsonValue in jsonArray.Values)
            {
                if(jsonValue.ValueType == JsonValueType.String)
                {
                    result.Add(this.jsonService.GetString(((JsonString)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Number)
                {
                    result.Add(this.jsonService.GetNumber(((JsonNumber)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Bool)
                {
                    result.Add(this.jsonService.GetBool(((JsonBool)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Nullable)
                {
                    result.Add(this.jsonService.GetNullable(((JsonNullable)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Array)
                {
                    result.Add(this.ToArray((JsonArray)jsonValue));
                }
                else if (jsonValue.ValueType == JsonValueType.Object)
                {
                    result.Add(this.ToObject((JsonObject)jsonValue));
                }
            }
            return "[" + string.Join(",", result) + "]";
        }

        public string ToObject(JsonObject jsonObject)
        {
            var result = new List<string>();
            foreach (var keyValue in jsonObject.Values)
            {
                var key = keyValue.Key;
                var jsonValue = keyValue.Value;
                if (jsonValue.ValueType == JsonValueType.String)
                {
                    result.Add(this.jsonService.GetString(key, ((JsonString)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Number)
                {
                    result.Add(this.jsonService.GetNumber(key, ((JsonNumber)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Bool)
                {
                    result.Add(this.jsonService.GetBool(key, ((JsonBool)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Nullable)
                {
                    result.Add(this.jsonService.GetNullable(key, ((JsonNullable)jsonValue).Value));
                }
                else if (jsonValue.ValueType == JsonValueType.Array)
                {
                    result.Add(this.jsonService.GetKey(key) + ":" + this.ToArray((JsonArray)jsonValue));
                }
                else if (jsonValue.ValueType == JsonValueType.Object)
                {
                    result.Add(this.jsonService.GetKey(key) + ":" + this.ToObject((JsonObject)jsonValue));
                }
            }
            return "{" + string.Join(",", result) + "}";
        }

    }
}
