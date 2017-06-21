namespace JsonLib
{
    public class JsonElementString : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.String;

        public string Value { get; }

        public JsonElementString(string value)
        {
            this.Value = value;
        }
    }

}
