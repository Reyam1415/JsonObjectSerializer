namespace JsonLib
{
    public class JsonElementNumber : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Number;

        public object Value { get; }

        public JsonElementNumber(object value)
        {
            this.Value = value;
        }
    }

}
