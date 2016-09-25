using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Json
{
    public class JsonElementArray : List<IJsonElement>, IJsonElement
    {
        public JsonElementType ElementType { get { return JsonElementType.Array; } }
        internal int StartIndex;
        internal int EndIndex;

        public static IJsonParser JsonParser = new JsonParser();

        public string Stringify(bool indented = false)
        {
            if (!indented)
            {
                using (var writer = new StringWriter())
                {
                    writer.Write("[");

                    bool firstElement = true;
                    foreach (var jsonElement in this)
                    {
                        if (!firstElement) writer.Write(",");
                        if (jsonElement.ElementType == JsonElementType.String)
                        {
                            writer.Write("\"" + ((JsonElementString)jsonElement).Value.ToString() + "\"");
                        }
                        else if (jsonElement.ElementType == JsonElementType.Boolean)
                        {
                            writer.Write(((JsonElementBool)jsonElement).Value.ToString().ToLower());
                        }
                        else if (jsonElement.ElementType == JsonElementType.Number)
                        {
                            writer.Write(((JsonElementNumber)jsonElement).Value);
                        }
                        else if (jsonElement.ElementType == JsonElementType.Object)
                        {
                            var json = ((JsonElementObject)jsonElement).Stringify();
                            writer.Write(json);
                        }
                        else if (jsonElement.ElementType == JsonElementType.Array)
                        {
                            var json = ((JsonElementArray)jsonElement).Stringify();
                            writer.Write(json);
                        }
                        firstElement = false;
                    }
                    writer.Write("]");
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

            writer.Append("[");
            bool firstElement = true;
            foreach (var jsonElement in this)
            {
                if (!firstElement) writer.Append(",\r");
                else writer.Append("\r");

                AppendTabs(writer, level);

                if (jsonElement.ElementType == JsonElementType.String)
                {
                    writer.Append("\"" + ((JsonElementString)jsonElement).Value.ToString() + "\"");
                }
                else if (jsonElement.ElementType == JsonElementType.Boolean)
                {
                    writer.Append(((JsonElementBool)jsonElement).Value.ToString().ToLower());
                }
                else if (jsonElement.ElementType == JsonElementType.Number)
                {
                    writer.Append(((JsonElementNumber)jsonElement).Value);
                }
                else if (jsonElement.ElementType == JsonElementType.Object)
                {
                    var json = ((JsonElementObject)jsonElement).StringifyIndented(level + 1);
                    writer.Append(json);
                }
                else if (jsonElement.ElementType == JsonElementType.Array)
                {
                    var json = ((JsonElementArray)jsonElement).StringifyIndented(level + 1);
                    writer.Append(json);
                }
                firstElement = false;
            }
            writer.Append(Environment.NewLine);
            AppendTabs(writer, level - 1);
            writer.Append("]");
            return writer.ToString();
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
