namespace JsonLib
{
    public class JsonElementNumber : IJsonElementValue
    {
        public JsonElementValueType ValueType => JsonElementValueType.Number;

        public object Value { get; }

        public JsonElementNumber(object value)
        {
            this.CheckValue(value);
            this.Value = value;
        }

        protected void CheckValue(object value)
        {
            if(!double.TryParse(value.ToString(),out double result))
            {
                throw new JsonLibException("Invalid Type. Require a number");
            }
        }
    }

}
