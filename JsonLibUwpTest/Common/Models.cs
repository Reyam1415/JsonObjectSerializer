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

    public class MyItemWithDictionaryIntString
    {
        public Dictionary<int,string> Items { get; set; }
    }

    public class MyItemWithDictionaryIntUser
    {
        public Dictionary<int, User> Users { get; set; }
    }

    public class UserWithMapping
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public MyEnum EnumValue { get; set; }
        public UserRole Role { get; set; }
        public List<Post> Posts { get; set; }
        public Tip[] Tips { get; set; }

        public UserWithMapping()
        {

        }
    }

    public class UserRole
    {
        public string RoleName { get; set; }

        public UserRole()
        {

        }
    }

    public class Post
    {
        public string Title { get; set; }

        public Post()
        {

        }
    }

    public class Tip
    {
        public string TipName { get; set; }
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

    // guess

    public class MyItemGuess
    {
        public string MyIntString { get; set; }
        public string MyDoubleString { get; set; }
        public string MyBoolString { get; set; }
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

    public class CompleteUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public double Quota { get; set; }
        public Int64 MyInt64 { get; set; }
    }

    public class UserWithInner
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }

        public UserWithInner()
        {
            this.Role = new Role();
        }
    }

    public class UserWithInnerAndList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public List<string> Strings { get; set; }

        public UserWithInnerAndList()
        {
            this.Role = new Role();
            this.Strings = new List<string>();
        }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
    }
}
