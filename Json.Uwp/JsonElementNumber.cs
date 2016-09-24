namespace Json
{
    public class JsonElementNumber : JsonElement
    {
        public double Value { get; private set; }

        public JsonElementNumber(double value)
        {
            Value = value;
            this.ElementType = JsonElementType.Number;
        }
    }
}