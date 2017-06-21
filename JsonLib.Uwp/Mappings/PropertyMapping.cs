namespace JsonLib.Mappings
{
    public class PropertyMapping
    {
        public string PropertyName { get; }
        public string JsonName { get; }

        public PropertyMapping(string propertyName, string jsonName)
        {
            this.PropertyName = propertyName;
            this.JsonName = jsonName;
        }
    }
}
