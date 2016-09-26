namespace Json
{
    public class JsonElementBool : IJsonElement
    {
        public JsonElementBool(bool value)
        {
            Value = value;
        }

        public JsonElementType ElementType
        {
            get { return JsonElementType.Bool; }
        }

        public bool Value { get; private set; }

    }
}