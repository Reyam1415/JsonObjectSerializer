using System;
using System.Collections.Generic;

namespace JsonLib.Mappings
{
    public class TypeMapping
    {
        public Type Type { get; }

        public bool LowerCaseStrategy { get; set; }

        public Dictionary<string, PropertyMapping> Properties { get; }

        public TypeMapping(Type type)
        {
            this.Properties = new Dictionary<string, PropertyMapping>();

            this.Type = type;
        }

        public void SetToLowerCaseStrategy(bool value = true)
        {
            this.LowerCaseStrategy = value;
        }

        public bool HasByPropertyName(string propertyName)
        {
            return this.Properties.ContainsKey(propertyName);
        }

        public PropertyMapping GetByPropertyName(string propertyName)
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

        public PropertyMapping GetByJsonName(string jsonName)
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

        public TypeMapping SetProperty(string propertyName, string jsonName)
        {
            var result = new PropertyMapping(propertyName, jsonName);
            this.Properties[propertyName] = result;
            return this;
        }
    }
}
