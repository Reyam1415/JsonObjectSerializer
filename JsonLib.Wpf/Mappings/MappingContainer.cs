using System;
using System.Collections.Generic;

namespace JsonLib.Mappings
{
    public class MappingContainer
    {
        internal Dictionary<Type, TypeMapping> container;

        public bool LowerStrategyForAllTypes { get; protected set; }

        public int Count => container.Count;

        public MappingContainer()
        {
            this.container = new Dictionary<Type, TypeMapping>();
        }

        public void SetLowerStrategyForAllTypes(bool value = true)
        {
            this.LowerStrategyForAllTypes = value;
        }

        public bool Has(Type type)
        {
            return this.container.ContainsKey(type);
        }

        public bool Has<T>()
        {
            return this.Has(typeof(T));
        }

        public TypeMapping Get(Type type)
        {
            if (!this.Has(type)) { throw new JsonLibException("No type mapping registered for " + type.Name); }

            return this.container[type];
        }

        public TypeMapping Get<T>()
        {
            return this.Get(typeof(T));
        }

        public TypeMapping SetType<T>()
        {
            var result = new TypeMapping(typeof(T));
            container[typeof(T)] = result;
            return result;
        }

        public void Clear()
        {
            this.container.Clear();
        }
    }
}
