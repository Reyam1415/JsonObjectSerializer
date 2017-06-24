namespace JsonLib.Json
{
    public class JsonString : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.String;

        public string Value { get; }

        public JsonString(string value)
        {
            this.Value = value;
        }
    }

}
