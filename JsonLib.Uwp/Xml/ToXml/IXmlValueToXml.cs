using JsonLib.Xml;

namespace JsonLib.Xml
{
    public interface IXmlValueToXml
    {
        string CreateDocument(IXmlValue xmlValue);
    }
}