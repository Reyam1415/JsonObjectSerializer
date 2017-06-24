using System;
using System.Collections.Generic;

namespace JsonLib.Mappings.Xml
{
    public class XmlMappingContainer
    {
        internal Dictionary<Type, XmlTypeMapping> container;

        public int Count => container.Count;

        public XmlMappingContainer()
        {
            this.container = new Dictionary<Type, XmlTypeMapping>();
        }

        public bool Has(Type type)
        {
            return this.container.ContainsKey(type);
        }

        public bool Has<T>()
        {
            return this.Has(typeof(T));
        }

        public XmlTypeMapping Get(Type type)
        {
            if (!this.Has(type)) { throw new JsonLibException("No type mapping registered for " + type.Name); }

            return this.container[type];
        }

        public XmlTypeMapping Get<T>()
        {
            return this.Get(typeof(T));
        }

        public XmlTypeMapping SetType<T>(string xmlObjectName)
        {
            if (this.Has<T>())
            {
                return this.Get<T>();
            }
            else
            {
                var result = new XmlTypeMapping(typeof(T), xmlObjectName);
                container[typeof(T)] = result;
                return result;
            }
        }

        public void Clear()
        {
            this.container.Clear();
        }
    }
}
