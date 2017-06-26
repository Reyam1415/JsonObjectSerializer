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

        public string ToString(JsonString jsonValue)
        {
            return jsonValue.IsNil ? "null" : this.jsonService.GetString(jsonValue.Value);
        }

        public string ToNumber(JsonNumber jsonValue)
        {
            return this.jsonService.GetNumber(jsonValue.Value);
        }

        public string ToBool(JsonBool jsonValue)
        {
            return this.jsonService.GetBool(jsonValue.Value);
        }

        public string ToNullable(JsonNullable jsonValue)
        {
            return jsonValue.IsNil ? "null" : this.jsonService.GetNullable(jsonValue.Value);
        }

        public string ToArray(JsonArray jsonArray)
        {
            var result = new List<string>();

            if (jsonArray.IsNil)
            {
                return "null";
            }
            else
            {
                foreach (var jsonValue in jsonArray.Values)
                {
                    if (jsonValue.ValueType == JsonValueType.String)
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
        }

        public string ToObject(JsonObject jsonObject)
        {
            var result = new List<string>();

            if (jsonObject.IsNil)
            {
                return "null";
            }
            else
            {
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

        public string Resolve(IJsonValue jsonValue)
        {
            if (jsonValue.ValueType == JsonValueType.Object)
            {
                return this.ToObject((JsonObject)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Array)
            {
                return this.ToArray((JsonArray)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.String)
            {
                return this.ToString((JsonString)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Number)
            {
                return this.ToNumber((JsonNumber)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Bool)
            {
                return this.ToBool((JsonBool)jsonValue);
            }
            else if (jsonValue.ValueType == JsonValueType.Nullable)
            {
                return this.ToNullable((JsonNullable)jsonValue);
            }

            throw new JsonLibException("Cannot resolve json for object");
        }
    }
}
