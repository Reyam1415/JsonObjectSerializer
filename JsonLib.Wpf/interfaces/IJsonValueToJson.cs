namespace JsonLib
{
    public interface IJsonValueToJson
    {
        string ToArray(JsonElementArray arrayElement);
        string ToBool(JsonElementBool element);
        string ToNullable(JsonElementNullable element);
        string ToNumber(JsonElementNumber element);
        string ToObject(JsonElementObject objectElement);
        string ToString(JsonElementString element);
    }
}