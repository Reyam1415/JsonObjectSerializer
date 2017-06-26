namespace JsonLib.Json
{
    public class JsonNullable : IJsonValue, IJsonNillable
    {
        public JsonValueType ValueType => JsonValueType.Nullable;

        public object Value { get; }

        public bool IsNil { get; protected set; }

        public JsonNullable(object value)
        {
            this.Value = value;
            this.IsNil = value == null;
        }
    }

}
