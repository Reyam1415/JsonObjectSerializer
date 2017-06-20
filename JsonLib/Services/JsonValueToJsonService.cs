using System.Globalization;

namespace JsonLib
{
    public class JsonValueToJsonService : IJsonValueToJsonService
    {
        public string GetKey(string key)
        {
            return "\"" + key + "\"";
        }

        public string GetString(string value)
        {
            // escape inner string
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("\"", "\\\"");
            }
            return value == null ? "null" : "\"" + value + "\"";
        }

        public string GetString(string name, string value)
        {
            return this.GetKey(name) + ":" + this.GetString(value) ;
        }

        public string GetNumber(object value)
        {
            return value.ToString().Replace(',', '.');
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
            return value == null ? "null" : value.ToString().ToLower();
        }

        public string GetNullable(string name, object value)
        {
            return this.GetKey(name) + ":" + this.GetNullable(value);
        }
    }
}
