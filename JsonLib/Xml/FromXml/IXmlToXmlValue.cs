using JsonLib.Xml;

namespace JsonLib.Xml
{
    public interface IXmlToXmlValue
    {
        IXmlValue ToXmlValue(string xml);
    }
}