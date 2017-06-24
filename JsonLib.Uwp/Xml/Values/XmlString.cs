namespace JsonLib.Xml
{
    public class XmlString : IXmlValue
    {
        public XmlValueType ValueType => XmlValueType.String;

        public string NodeName { get; protected set; }

        public bool IsNil { get; protected set; }

        public string Value { get; }

        public XmlString(string nodeName, string value)
        {
            this.NodeName = nodeName;
            this.Value = value;
            this.IsNil = value == null;
        }

    }

}
