namespace JsonLib.Json
{
    public class JsonBool : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.Bool;

        public bool Value { get; }

        public JsonBool(bool value)
        {
            this.Value = value;
        }
    }

}
