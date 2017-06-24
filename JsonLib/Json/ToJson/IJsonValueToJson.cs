namespace JsonLib.Json
{
    public interface IJsonValueToJson
    {
        string ToArray(JsonArray arrayElement);
        string ToBool(JsonBool element);
        string ToNullable(JsonNullable element);
        string ToNumber(JsonNumber element);
        string ToObject(JsonObject objectElement);
        string ToString(JsonString element);
    }
}