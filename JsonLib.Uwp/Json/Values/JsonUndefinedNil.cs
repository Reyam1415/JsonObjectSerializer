namespace JsonLib.Json
{
    public class JsonUndefinedNil : IJsonValue, IJsonNillable
    {
        public bool IsNil => true;

        public JsonValueType ValueType => JsonValueType.UndefinedNil;
    }
}
