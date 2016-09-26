namespace Json
{
    public class JsonElementString : IJsonElement
    {
        public JsonElementString(string value)
        {
            Value = value;
        }

        public JsonElementType ElementType
        {
            get { return JsonElementType.String; }
        }

        public string Value { get; private set; }

    }
}
