using System;
using System.Collections.Generic;

namespace Json
{
    public class JsonElementArray : List<IJsonElement>, IJsonElement
    {
        public JsonElementType ElementType { get { return JsonElementType.Array; } }
        internal int StartIndex;
        internal int EndIndex;

        public static IJsonParser JsonParser = new JsonParser();

        public string Stringify()
        {
            string result = "[";

            bool firstElement = true;
            foreach (var jsonElement in this)
            {
                if (!firstElement) result += ",";
                if (jsonElement.ElementType == JsonElementType.String)
                {
                    result += "\"" + ((JsonElementString)jsonElement).Value.ToString() + "\"";
                }
                else if (jsonElement.ElementType == JsonElementType.Boolean)
                {
                    result += ((JsonElementBool)jsonElement).Value.ToString().ToLower();
                }
                else if (jsonElement.ElementType == JsonElementType.Number)
                {
                    result += ((JsonElementNumber)jsonElement).Value;
                }
                else if (jsonElement.ElementType == JsonElementType.Object)
                {
                    var json = ((JsonElementObject)jsonElement).Stringify();
                    result += json;
                }
                else if (jsonElement.ElementType == JsonElementType.Array)
                {
                    var json = ((JsonElementArray)jsonElement).Stringify();
                    result += json;
                }
                firstElement = false;
            }

            result += "]";

            return result;
        }

        public static bool TryParse(string json, out JsonElementArray rootJsonArray)
        {
            try
            {
                var result = JsonParser.ParseArray(json);
                rootJsonArray = (JsonElementArray)result;
                return true;
            }
            catch (Exception ex)
            { }

            rootJsonArray = null;
            return false;
        }
    }

}
