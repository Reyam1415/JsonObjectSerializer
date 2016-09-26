namespace Json
{

    public class JsonElementNumber : IJsonElement
    {
        public JsonElementNumber(double value)
        {
            Value = value;
        }

        public JsonElementType ElementType
        {
            get { return JsonElementType.Number; }
        }

        public double Value { get; private set; }

    }
}