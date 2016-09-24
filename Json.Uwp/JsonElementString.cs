namespace Json
{
    public class JsonElementString : JsonElement
    {
        public string Value { get; private set; }

        public JsonElementString(string value)
        {
            Value = value;
            this.ElementType = JsonElementType.String;
        }
    }
}
