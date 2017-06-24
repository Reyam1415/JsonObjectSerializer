using System;

namespace JsonLib.Xml
{

    public class XmlNullable : IXmlValue, IXmlNillable
    {
        public XmlValueType ValueType => XmlValueType.Nullable;

        public string NodeName { get; protected set; }

        public object Value { get; }

        public Type Type { get; protected set; }

        public Type UnderlyingType { get; protected set; }

        public bool IsNil { get; protected set; }

        internal XmlNullable(Type type, string nodeName, object value)
        {
            this.NodeName = nodeName;

            this.CheckAndSetUnderlyingType(type);

            this.Value = value;
            this.IsNil = value == null;
        }

        public void CheckAndSetUnderlyingType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType == null)
            {
                throw new JsonLibException("XmlNullable require a Nullable type ( int? for example )");
            }
            else
            {
                this.Type = type;
                this.UnderlyingType = underlyingType;
            }
        }

    }

}
