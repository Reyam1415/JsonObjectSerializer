namespace JsonLib
{
    public class JsonElementBool : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Bool;

        public bool Value { get; }

        public JsonElementBool(bool value)
        {
            this.Value = value;
        }
    }

}
