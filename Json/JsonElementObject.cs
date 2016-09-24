using System;
using System.Collections.Generic;
namespace Json
{

    public class JsonElementObject : Dictionary<string, IJsonElement>, IJsonElement
    {
        public JsonElementType ElementType { get { return JsonElementType.Object; } }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public static IJsonParser JsonParser = new JsonParser();

        public string Stringify()
        {
            string result = "{";

            bool firstElement = true;
            foreach (var jsonElement in this)
            {
                if (!firstElement) result += ",";
                if (jsonElement.Value.ElementType == JsonElementType.String)
                {
                    result += "\"" + jsonElement.Key + "\":\"" + ((JsonElementString)jsonElement.Value).Value + "\"";
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Boolean)
                {
                    result += "\"" + jsonElement.Key + "\":" + ((JsonElementBool)jsonElement.Value).Value.ToString().ToLower();
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Number)
                {
                    string value = ((JsonElementNumber)jsonElement.Value).Value.ToString();
                    value = value.Replace(',', '.');
                    result += "\"" + jsonElement.Key + "\":" + value;
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Object)
                {
                    var json = ((JsonElementObject)jsonElement.Value).Stringify();

                    result += "\"" + jsonElement.Key + "\":" + json;
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Array)
                {
                    var json = ((JsonElementArray)jsonElement.Value).Stringify();
                    result += "\"" + jsonElement.Key + "\":" + json;
                }
                firstElement = false;
            }
            result += "}";
            return result;
        }

        public static bool TryParse(string json, out JsonElementObject rootJsonObject)
        {
            var jsonObject = new JsonElementObject();
            try
            {
                var result = JsonParser.ParseObject(json);
                rootJsonObject = (JsonElementObject)result;
                return true;
            }
            catch (Exception)
            { }
            rootJsonObject = null;
            return false;
        }
    }

}
