using System;
using System.Globalization;
using System.Text;

namespace JsonLib
{
    public class JsonValueToJsonService : IJsonValueToJsonService
    {
        public string GetKey(string key)
        {
            return "\"" + key + "\"";
        }

        public string FormatString(string value)
        {
            var result = new StringBuilder();

            result.Append("\"");

            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (c == '"')
                {
                    result.Append("\\\"");
                }
                else if (c == '\\')
                {
                    result.Append("\\\\");
                }
                else if (c == '\b')
                {
                    result.Append("\\b");
                }
                else if (c == '\f')
                {
                    result.Append("\\f");
                }
                else if (c == '\n')
                {
                    result.Append("\\n");
                }
                else if (c == '\r')
                {
                    result.Append("\\r");
                }
                else if (c == '\t')
                {
                    result.Append("\\t");
                }
                else
                {
                    int codepoint = Convert.ToInt32(c);
                    if ((codepoint >= 32) && (codepoint <= 126))
                    {
                        result.Append(c);
                    }
                    else
                    {
                        result.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                    }
                }
            }

            result.Append("\"");
            return result.ToString();
        }

        public string GetString(string value)
        {
            return value == null ? "null" : this.FormatString(value);
        }

        public string GetString(string name, string value)
        {
            return this.GetKey(name) + ":" + this.GetString(value) ;
        }

        public string GetNumber(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture); 
        }

        public string GetNumber(string name, object value)
        {
            return this.GetKey(name) + ":" + this.GetNumber(value);
        }

        public string GetBool(bool value)
        {
            return value.ToString().ToLower();
        }

        public string GetBool(string name, bool value)
        {
            return this.GetKey(name) + ":" + this.GetBool(value);
        }

        public string GetNullable(object value)
        {
            if (value == null)
            {
                return "null";
            }
            else
            {
                var type = value.GetType();
                if (type == typeof(DateTime) || type == typeof(Guid))
                {
                    return "\"" + value.ToString() + "\"";
                }
                else if(type == typeof(bool))
                {
                    return (bool)value == true ? "true" : "false";
                }
                else if (type.IsEnum)
                {
                    var intValue = Convert.ToInt32(value);
                    return intValue.ToString();
                }
                else
                {
                    return this.GetNumber(value);
                }
            }
        }

        public string GetNullable(string name, object value)
        {
            return this.GetKey(name) + ":" + this.GetNullable(value);
        }
    }
}
