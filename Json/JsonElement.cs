namespace Json
{
    public class JsonElement
    {
        public static JsonElementString CreateString(string value)
        {
            return new JsonElementString(value);
        }

        public static JsonElementNumber CreateNumber(double value)
        {
            return new JsonElementNumber(value);
        }

        public static JsonElementBool CreateBool(bool value)
        {
            return new JsonElementBool(value);
        }
    }
}