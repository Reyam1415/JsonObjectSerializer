using System;
using System.Collections.Generic;

namespace JsonLib.Json.Mappings
{
    public class JsonMappingContainer
    {
        internal Dictionary<Type, JsonTypeMapping> container;

        public bool LowerStrategyForAllTypes { get; protected set; }

        public int Count => container.Count;

        public JsonMappingContainer()
        {
            this.container = new Dictionary<Type, JsonTypeMapping>();
        }

        public JsonMappingContainer SetLowerStrategyForAllTypes(bool value = true)
        {
            this.LowerStrategyForAllTypes = value;
            return this;
        }

        public bool Has(Type type)
        {
            return this.container.ContainsKey(type);
        }

        public bool Has<T>()
        {
            return this.Has(typeof(T));
        }

        public JsonTypeMapping Get(Type type)
        {
            if (!this.Has(type)) { throw new JsonLibException("No type mapping registered for " + type.Name); }

            return this.container[type];
        }

        public JsonTypeMapping Get<T>()
        {
            return this.Get(typeof(T));
        }

        public JsonTypeMapping SetType<T>()
        {
            var result = new JsonTypeMapping(typeof(T));
            container[typeof(T)] = result;
            return result;
        }

        public void Clear()
        {
            this.container.Clear();
        }
    }
}
