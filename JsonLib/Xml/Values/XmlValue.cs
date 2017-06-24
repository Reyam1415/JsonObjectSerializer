namespace JsonLib.Xml
{

    public class XmlValue
    {
        public static XmlString CreateString(string nodeName, string value)
        {
            return new XmlString(nodeName, value);
        }

        public static XmlNumber CreateNumber(string nodeName, object value)
        {
            return new XmlNumber(nodeName, value);
        }

        public static XmlBool CreateBool(string nodeName, bool value)
        {
            return new XmlBool(nodeName, value);
        }

        public static XmlNullable CreateNullable<T>(string nodeName, T value)
        {
            return new XmlNullable(typeof(T), nodeName, value);
        }

        public static XmlObject CreateObject(string nodeName)
        {
            return new XmlObject(nodeName);
        }

        public static XmlArray CreateArray(string nodeName)
        {
            return new XmlArray(nodeName);
        }
    }

}
