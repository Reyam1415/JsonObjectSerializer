namespace JsonLib.Json
{
    public class JsonNullable : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.Nullable;

        public object Value { get; }

        public JsonNullable(object value)
        {
            this.Value = value;
        }
    }

}
