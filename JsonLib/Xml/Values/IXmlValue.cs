namespace JsonLib.Xml
{
    public interface IXmlNillable
    {
        bool IsNil { get; }
    }

    public interface IXmlValue
    {
        string NodeName { get; }
        XmlValueType ValueType { get; }
    }

}
