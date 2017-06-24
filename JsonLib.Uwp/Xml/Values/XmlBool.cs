namespace JsonLib.Xml
{
    public class XmlBool : IXmlValue
    {
        public XmlValueType ValueType => XmlValueType.Bool;

        public string NodeName { get; protected set; }

        public bool Value { get; }

        public XmlBool(string nodeName, bool value)
        {
            this.NodeName = nodeName;
            this.Value = value;
        }
    }


}
