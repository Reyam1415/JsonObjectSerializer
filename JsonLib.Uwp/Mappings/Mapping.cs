using System;

namespace JsonLib.Mappings
{

    public class Mapping
    {

        internal static MappingContainer container;

        public static int Count => container.Count;
        public static bool LowerCaseStrategyForAllTypes => container.LowerStrategyForAllTypes;

        static Mapping()
        {
            container = new MappingContainer();
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

        public static TypeMapping Get<T>()
        {
            return container.Get<T>();
        }

        public static TypeMapping Get(Type type)
        {
            return container.Get(type);
        }

        public static TypeMapping SetType<T>()
        {
            return container.SetType<T>();
        }

        public static void Clear()
        {
            container.Clear();
        }

        public static MappingContainer GetContainer()
        {
            return container;
        }
    }
}
