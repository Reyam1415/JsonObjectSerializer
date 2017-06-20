using System.Text;

namespace JsonLib
{
    public class Beautifier : IBeautifier
    {
        protected static void AppendTabs(StringBuilder result, int count)
        {
            for (int i = 0; i < count; i++)
            {
                result.Append("   ");
            }
        }

        public string Format(string json)
        {
            var result = new StringBuilder();
            int level = 0;
            int len = json.Length;
            char[] jsonChars = json.ToCharArray();
            for (int i = 0; i < len; ++i)
            {
                char c = jsonChars[i];

                if (c == '\"') 
                {
                    bool stringOpen = true;
                    while (stringOpen)
                    {
                        result.Append(c);
                        c = jsonChars[++i];
                        if (c == '\\')
                        {
                            result.Append(c);
                            c = jsonChars[++i];
                        }
                        else if (c == '\"')
                        {
                            stringOpen = false;
                        }
                    }
                }

                switch (c)
                {
                    case '{':
                    case '[':
                        result.Append(c);
                        result.AppendLine();
                        AppendTabs(result, ++level);
                        break;
                    case '}':
                    case ']':
                        result.AppendLine();
                        AppendTabs(result, --level);
                        result.Append(c);
                        break;
                    case ',':
                        result.Append(c);
                        result.AppendLine();
                        AppendTabs(result, level);
                        break;
                    case ':':
                        result.Append(" : ");
                        break;
                    default:
                        if (!char.IsWhiteSpace(c))
                        {
                            result.Append(c);
                        }
                        break;
                }
            }

            return result.ToString();
        }

    }
}
