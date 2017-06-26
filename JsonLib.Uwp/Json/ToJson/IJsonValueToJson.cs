namespace JsonLib.Json
{
    public interface IJsonValueToJson
    {
        string Resolve(IJsonValue jsonValue);
    }
}