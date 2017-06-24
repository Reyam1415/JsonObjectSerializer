namespace JsonLib.Json
{
    public class JsonNumber : IJsonValue
    {
        public JsonValueType ValueType => JsonValueType.Number;

        public object Value { get; }

        public JsonNumber(object value)
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
