namespace JsonLib
{
    public class JsonElementNullable : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Null;

        public object Value { get; }

        public JsonElementNullable(object value)
        {
            this.Value = value;
        }
    }

}
