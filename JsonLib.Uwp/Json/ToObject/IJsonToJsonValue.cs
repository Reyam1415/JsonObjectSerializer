namespace JsonLib.Json
{
    public interface IJsonToJsonValue
    {
        IJsonValue ToJsonValue(string json);
    }
}