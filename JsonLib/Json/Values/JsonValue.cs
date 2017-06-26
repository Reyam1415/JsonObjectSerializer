namespace JsonLib.Json
{

    public class JsonValue
    {
        public static JsonString CreateString(string value)
        {
            return new JsonString(value);
        }

        public static JsonNumber CreateNumber(object value)
        {
            return new JsonNumber(value);
        }

        public static JsonBool CreateBool(bool value)
        {
            return new JsonBool(value);
        }

        public static JsonNullable CreateNullable(object value)
        {
            return new JsonNullable(value);
        }

        public static JsonObject CreateObject()
        {
            return new JsonObject();
        }

        public static JsonArray CreateArray()
        {
            return new JsonArray();
        }

        public static JsonUndefinedNil CreateUndefinedNil()
        {
            return new JsonUndefinedNil();
        }
    }

}
