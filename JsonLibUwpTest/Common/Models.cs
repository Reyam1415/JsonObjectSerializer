using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLibTest
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
    }

    //
    public class AssemblyItem
    {
        public Guid MyGuid { get; set; }
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public int? MyNullable { get; set; }
        public AssemblyEnum MyEnum { get; set; }
        public DateTime MyDate { get; set; }
        public AssemblyInner MyObj { get; set; }
        public List<string> MyList { get; set; }
        public string[] MyArray { get; set; }
    }

    public class AssemblyItem2
    {
        public Int64 MyInt64 { get; set; }
    }

    public class AssemblyInner
    {
        public string MyInnerString { get; set; }
    }

    public enum AssemblyEnum
    {
        Default,
        Other
    }


    //
    public enum MyEnum
    {
        Default,
        Other
    }

    public class MyItem
    {
        public Guid MyGuid { get; set; }
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public MyEnum MyEnumValue { get; set; }
        public DateTime MyDate { get; set; }
        public MyInnerItem MyObj { get; set; }
        public List<string> MyList { get; set; }
        public string[] MyArray { get; set; }
    }

    public class MyItemNullables
    {
        public Guid? MyGuid { get; set; }
        public int? MyInt { get; set; }
        public double? MyDouble { get; set; }
        public string MyString { get; set; }
        public bool? MyBool { get; set; }
        public MyEnum? MyEnumValue { get; set; }
        public DateTime? MyDate { get; set; }
        public MyInnerItem MyObj { get; set; }
        public List<string> MyList { get; set; }
        public string[] MyArray { get; set; }
    }

    public class MyItemNumbers
    {
        public Int32 MyInt32 { get; set; }
        public Byte MyByte { get; set; }
        public SByte MySByte { get; set; }
        public UInt16 MyUInt16 { get; set; }
        public UInt32 MyUInt32 { get; set; }
        public UInt64 MyUInt64 { get; set; }
        public Int16 MyInt16 { get; set; }
        public Int64 MyIn64 { get; set; }
        public Decimal MyDec { get; set; }
        public Double MyDouble { get; set; }
        public Single MySingle { get; set; }
    }

    public class MyItem2
    {
        public Int64 MyInt64 { get; set; }
    }

    public class MyInnerItem
    {
        public string MyInnerString { get; set; }
    }

}
