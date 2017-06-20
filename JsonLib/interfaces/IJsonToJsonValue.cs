namespace JsonLib
{
    public interface IJsonToJsonValue
    {
        IJsonElementValue ToJsonValue(string json);
    }
}