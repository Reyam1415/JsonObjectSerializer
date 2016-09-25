using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonMapAttribute : Attribute
    {
        public string JsonElementKey { get; set; }
    }


    public class JsonMapItem
    {
        public Type ClassType { get; set; }
        public string PropertyName { get; set; }
        public PropertyInfo Property { get; set; }
        public string JsonElementKey { get; set; }
    }

    public class JsonMapping
    {
        public static JsonMapping Default
        {
            get { return Singleton<JsonMapping>.Instance; }
        }

        public int Count
        {
            get { return mapping.Count; }
        }

        private Dictionary<Type, Dictionary<string,JsonMapItem>> mapping = new Dictionary<Type, Dictionary<string, JsonMapItem>>();

        private bool IsValid(string input)
        {
            return new Regex("^[a-zA-Z0-9_]+$").IsMatch(input);
        }

        private PropertyInfo FindProperty(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null) throw new ArgumentNullException("Not property found for " + propertyName + " in " + type.Name);
            return propertyInfo;
        }

        public JsonMapping Add(Type type, string propertyName, string jsonElementKey)
        {
            if (!IsValid(propertyName)) throw new ArgumentException("Invalid format for " + propertyName);
            if (!IsValid(jsonElementKey)) throw new ArgumentException("Invalid format for " + jsonElementKey);

            // find property
            var property = FindProperty(type, propertyName);
            if (property == null) throw new ArgumentNullException("No property found for " + propertyName + " in " + type.Name);

            if (!mapping.ContainsKey(type)) mapping[type] = new Dictionary<string, JsonMapItem>();

            var jsonMapItem = new JsonMapItem
            {
                ClassType = type,
                Property = property,
                PropertyName = propertyName,
                JsonElementKey = jsonElementKey
            };

            mapping[type].Add(jsonElementKey,jsonMapItem);

            return this;
        }

        public PropertyInfo GetProperty(Type type, string jsonElementKey)
        {
            if (mapping.ContainsKey(type))
            {
                var item = mapping[type];
                if (item.ContainsKey(jsonElementKey))
                {
                    var resolved = item[jsonElementKey];
                    return resolved.Property;
                }
            }
            return null;
        }

        public string GetJsonElementKey(Type type, string propertyName)
        {
            if (mapping.ContainsKey(type))
            {
                var item = mapping[type];
                foreach (var p in item)
                {
                    if (p.Value.PropertyName == propertyName) return p.Value.JsonElementKey;
                }
            }
            return null;
        }

        public Dictionary<string,JsonMapItem> Get(Type type)
        {
            if (mapping.ContainsKey(type))
            {
                var item = mapping[type];
                return item;
            }
            return null;
        }


        public void Clear()
        {
            mapping.Clear();
        }
    }
}
