using System;

namespace JsonLib
{
    public class JsonLibException : Exception
    {
        public JsonLibException(string message) 
            : base(message)
        { }
    }
}
