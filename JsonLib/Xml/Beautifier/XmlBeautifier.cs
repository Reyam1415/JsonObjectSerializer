using JsonLib.Common;
using System.Xml.Linq;

namespace JsonLib.Xml
{
    public class XmlBeautifier : IBeautifier
    {
        public string Format(string xml)
        {
            var result = XDocument.Parse(xml);
            return result.Declaration + "\r" + result.ToString();
        }
    }
}
