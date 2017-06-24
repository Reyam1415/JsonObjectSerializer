namespace JsonLib.Xml
{
    public class XmlNumber : IXmlValue
    {
        public XmlValueType ValueType => XmlValueType.Number;

        public string NodeName { get; protected set; }

        public object Value { get; }

        public XmlNumber(string nodeName, object value)
        {
            this.NodeName = nodeName;

            this.CheckValue(value);
            this.Value = value;
        }

        protected void CheckValue(object value)
        {
            if (!double.TryParse(value.ToString(), out double result))
            {
                throw new JsonLibException("Invalid Type. Require a number");
            }
        }

    }

}
