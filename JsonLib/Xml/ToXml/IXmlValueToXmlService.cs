namespace JsonLib.Xml
{
    public interface IXmlValueToXmlService
    {
        string GetBooValueString(bool value);
        string GetDeclaration();
        string GetNilNamespace();
        string GetNilNode(string nodeName);
        string GetNode(string nodeName, string value);
        string GetNumber(object value);
        string GetNumberNode(string nodeName, object value);
        string GetOneClosedNode(string nodeName);
        string GetRoot(string nodeName, string content, bool nilNamespace = false);
        bool HasContent(string content);
    }
}