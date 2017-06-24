using System;
using System.Collections.Generic;

namespace JsonLib.Json.Mappings
{
    public class JsonTypeMapping
    {
        public Type Type { get; }

        public bool LowerCaseStrategy { get; set; }

        public Dictionary<string, JsonPropertyMapping> Properties { get; }

        public JsonTypeMapping(Type type)
        {
            this.Properties = new Dictionary<string, JsonPropertyMapping>();

            this.Type = type;
        }

        public JsonTypeMapping SetToLowerCaseStrategy(bool value = true)
        {
            this.LowerCaseStrategy = value;
            return this;
        }

        public bool HasByPropertyName(string propertyName)
        {
            return this.Properties.ContainsKey(propertyName);
        }

        public JsonPropertyMapping GetByPropertyName(string propertyName)
        {
            if(!this.HasByPropertyName(propertyName)) { throw new JsonLibException("No mapping registered for property " + propertyName); }

            return this.Properties[propertyName];
        }

        public bool HasByJsonName(string jsonName)
        {
            foreach (var property in this.Properties)
            {
                if(property.Value.JsonName == jsonName)
                {
                    return true;
                }
            }
            return false;
        }

        public JsonPropertyMapping GetByJsonName(string jsonName)
        {
            if (!this.HasByJsonName(jsonName)) { throw new JsonLibException("No mapping registered for " + jsonName); }

            foreach (var property in this.Properties)
            {
                if (property.Value.JsonName == jsonName)
                {
                    return property.Value;
                }
            }
            return null;
        }

        public JsonTypeMapping SetProperty(string propertyName, string jsonName)
        {
            var result = new JsonPropertyMapping(propertyName, jsonName);
            this.Properties[propertyName] = result;
            return this;
        }
    }
}
