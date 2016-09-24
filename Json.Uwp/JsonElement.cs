namespace Json
{
    public class JsonElement : IJsonElement
    {
        public JsonElementType ElementType { get; protected set; }

        public static JsonElementString CreateString(string value)
        {
            return new JsonElementString(value);
        }

        public static JsonElementBool CreateBoolean(bool value)
        {
            return new JsonElementBool(value);
        }

        public static JsonElementNumber CreateNumber(double value)
        {
            return new JsonElementNumber(value);
        }
    }
}