namespace Json
{
    public interface IJsonParser
    {
        IJsonElement Parse(string json);
        IJsonElement ParseArray(string json, int arrayLevel = 0, int index = 1);
        IJsonElement ParseObject(string json, int objectLevel = 0, int index = 1);
    }
}