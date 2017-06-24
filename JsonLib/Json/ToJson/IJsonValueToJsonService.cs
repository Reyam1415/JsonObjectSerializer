namespace JsonLib.Json
{
    public interface IJsonValueToJsonService
    {
        string GetBool(bool value);
        string GetBool(string name, bool value);
        string GetKey(string key);
        string GetNullable(object value);
        string GetNullable(string name, object value);
        string GetNumber(object value);
        string GetNumber(string name, object value);
        string GetString(string value);
        string GetString(string name, string value);
    }
}