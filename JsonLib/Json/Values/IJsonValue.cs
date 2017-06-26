using System;

namespace JsonLib.Json
{
    public interface IJsonNillable
    {
        bool IsNil { get;  }
    }

    public interface IJsonValue
    {
        JsonValueType ValueType { get; }
    }
}
