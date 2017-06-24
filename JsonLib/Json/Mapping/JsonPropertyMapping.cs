namespace JsonLib.Json.Mappings
{
    public class JsonPropertyMapping
    {
        public string PropertyName { get; }
        public string JsonName { get; }

        public JsonPropertyMapping(string propertyName, string jsonName)
        {
            this.PropertyName = propertyName;
            this.JsonName = jsonName;
        }
    }
}
