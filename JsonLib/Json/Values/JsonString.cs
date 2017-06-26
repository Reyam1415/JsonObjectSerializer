namespace JsonLib.Json
{
    public class JsonString : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.String;

        public string Value { get; }

        public bool IsNil { get; protected set; }

        public JsonString(string value)
        {
            this.Value = value;
            this.IsNil = value == null;
        }
    }

}
