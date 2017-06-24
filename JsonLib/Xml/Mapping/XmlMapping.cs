using System;

namespace JsonLib.Mappings.Xml
{

    public class XmlMapping
    {
        internal static XmlMappingContainer container;

        public static int Count => container.Count;

        static XmlMapping()
        {
            container = new XmlMappingContainer();
        }

        public static bool Has<T>()
        {
            return container.Has<T>();
        }

        public static bool Has(Type type)
        {
            return container.Has(type);
        }

        public static XmlTypeMapping Get<T>()
        {
            return container.Get<T>();
        }

        public static XmlTypeMapping Get(Type type)
        {
            return container.Get(type);
        }

        public static XmlTypeMapping SetType<T>(string xmlName)
        {
            return container.SetType<T>(xmlName);
        }

        public static void Clear()
        {
            container.Clear();
        }

        public static XmlMappingContainer GetContainer()
        {
            return container;
        }
    }
}
