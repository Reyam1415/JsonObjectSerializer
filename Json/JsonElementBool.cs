namespace Json
{
    public class JsonElementBool : JsonElement
    {
        public bool Value { get; private set; }

        public JsonElementBool(bool value)
        {
            Value = value;
            this.ElementType = JsonElementType.Boolean;
        }
    }
  
}