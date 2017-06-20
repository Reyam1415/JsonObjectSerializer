namespace JsonLib
{

    public class JsonElementValue
    {
        public static JsonElementString CreateString(string value)
        {
            return new JsonElementString(value);
        }

        public static JsonElementNumber CreateNumber(object value)
        {
            return new JsonElementNumber(value);
        }

        public static JsonElementBool CreateBool(bool value)
        {
            return new JsonElementBool(value);
        }

        public static JsonElementNullable CreateNullable(object value)
        {
            return new JsonElementNullable(value);
        }

        public static JsonElementObject CreateObject()
        {
            return new JsonElementObject();
        }

        public static JsonElementArray CreateArray()
        {
            return new JsonElementArray();
        }
    }

}
