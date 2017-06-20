using System.Collections;
using JsonLib.Mappings;

namespace JsonLib
{
    public interface IObjectToJsonValue
    {
        IJsonElementValue ToJsonElementValue(object obj, MappingContainer mappings = null);
        JsonElementArray ToJsonArray(IEnumerable array, MappingContainer mappings = null);
        JsonElementObject ToJsonObject(object obj, MappingContainer mappings = null);
    }
}