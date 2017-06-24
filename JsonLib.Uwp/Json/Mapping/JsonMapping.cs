using System;

namespace JsonLib.Json.Mappings
{

    public class JsonMapping
    {

        internal static JsonMappingContainer container;

        public static int Count => container.Count;
        public static bool LowerCaseStrategyForAllTypes => container.LowerStrategyForAllTypes;

        static JsonMapping()
        {
            container = new JsonMappingContainer();
        }

        public static void SetLowerStrategyForAllTypes(bool value = true)
        {
            container.SetLowerStrategyForAllTypes(value);
        }

        public static bool Has<T>()
        {
            return container.Has<T>();
        }

        public static bool Has(Type type)
        {
            return container.Has(type);
        }

        public static JsonTypeMapping Get<T>()
        {
            return container.Get<T>();
        }

        public static JsonTypeMapping Get(Type type)
        {
            return container.Get(type);
        }

        public static JsonTypeMapping SetType<T>()
        {
            return container.SetType<T>();
        }

        public static void Clear()
        {
            container.Clear();
        }

        public static JsonMappingContainer GetContainer()
        {
            return container;
        }
    }
}
