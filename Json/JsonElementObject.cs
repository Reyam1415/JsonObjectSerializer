using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Json
{

    public class JsonElementObject : Dictionary<string, IJsonElement>, IJsonElement
    {
        public JsonElementType ElementType { get { return JsonElementType.Object; } }
        internal int StartIndex;
        internal int EndIndex;

        public static IJsonParser JsonParser = new JsonParser();

        public string Stringify(bool indented = false)
        {
            if (!indented)
            {
                using (var writer = new StringWriter())
                {
                    writer.Write("{");

                    bool firstElement = true;
                    foreach (var jsonElement in this)
                    {
                        if (!firstElement) writer.Write(",");
                        if (jsonElement.Value.ElementType == JsonElementType.String)
                        {
                            writer.Write("\"" + jsonElement.Key + "\":\"" + ((JsonElementString)jsonElement.Value).Value + "\"");
                        }
                        else if (jsonElement.Value.ElementType == JsonElementType.Boolean)
                        {
                            writer.Write("\"" + jsonElement.Key + "\":" + ((JsonElementBool)jsonElement.Value).Value.ToString().ToLower());
                        }
                        else if (jsonElement.Value.ElementType == JsonElementType.Number)
                        {
                            string value = ((JsonElementNumber)jsonElement.Value).Value.ToString();
                            value = value.Replace(',', '.');
                            writer.Write("\"" + jsonElement.Key + "\":" + value);
                        }
                        else if (jsonElement.Value.ElementType == JsonElementType.Object)
                        {
                            var json = ((JsonElementObject)jsonElement.Value).Stringify();

                            writer.Write("\"" + jsonElement.Key + "\":" + json);
                        }
                        else if (jsonElement.Value.ElementType == JsonElementType.Array)
                        {
                            var json = ((JsonElementArray)jsonElement.Value).Stringify();
                            writer.Write("\"" + jsonElement.Key + "\":" + json);
                        }
                        firstElement = false;
                    }
                    writer.Write("}");
                    return writer.ToString();
                }
            }
            else return StringifyIndented();
        }

        private static void AppendTabs(StringBuilder writer, int count)
        {
            for (int i = 0; i < count; i++) writer.Append("   ");
        }

        internal string StringifyIndented(int level = 1)
        {
            var writer = new StringBuilder();

            writer.Append("{");
            bool firstElement = true;
            foreach (var jsonElement in this)
            {
                if (!firstElement) writer.Append(",\r");
                else writer.Append("\r");

                AppendTabs(writer, level);

                if (jsonElement.Value.ElementType == JsonElementType.String)
                {
                    writer.Append("\"" + jsonElement.Key + "\": \"" + ((JsonElementString)jsonElement.Value).Value + "\"");
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Boolean)
                {
                    writer.Append("\"" + jsonElement.Key + "\": " + ((JsonElementBool)jsonElement.Value).Value.ToString().ToLower());
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Number)
                {
                    string value = ((JsonElementNumber)jsonElement.Value).Value.ToString();
                    value = value.Replace(',', '.');
                    writer.Append("\"" + jsonElement.Key + "\": " + value);
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Object)
                {
                    var json = ((JsonElementObject)jsonElement.Value).StringifyIndented(level + 1);

                    writer.Append("\"" + jsonElement.Key + "\": " + json);
                }
                else if (jsonElement.Value.ElementType == JsonElementType.Array)
                {
                    var json = ((JsonElementArray)jsonElement.Value).StringifyIndented(level + 1);
                    writer.Append("\"" + jsonElement.Key + "\": " + json);
                }
                firstElement = false;
            }
            writer.Append(Environment.NewLine);
            AppendTabs(writer, level - 1);
            writer.Append("}");
            return writer.ToString();         
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
            catch (Exception ex)
            {
                var x = 10;
            }
            rootJsonObject = null;
            return false;
        }
    }

}
