# JsonObjectSerializer

> Json Serializer for .NET projects

Support :
* _Uwp_
* _Wpf_
* _Windows Forms_
* etc.

_Note : "Windows.Data.Json" (JsonObject,...) is only available for Windows store applications, so i create my own library._

### Installation with NuGet

```
PM> Install-Package JsonObjectSerializer
```

* **JsonObjectSerializer** with **Static methods**
    * **Stringify**: Object => Json
    * **StringifyAndBeautify**: Stringify + Format Json
    * **Parse**: Json => Object
    * **ToXml**: Object => Xml
    * **ToXmlAndBeautify**
    * **FromXml**: Xml => Object
    * **ActiveCache** (active by default)

* **Services**:
   * **JsonObjectSerializerService** (IJsonObjectSerializerService)
   * **JsonBeautifier** and **XmlBeautifier** (IBeautifier): used to Format / Indent Json and Xml
   * **AssemblyInfoService** (IAssemblyInfoService): used to resolve Object values and properties

* **Json Values** (IJsonValue):
   * **String** (JsonString) => Value string, used for Guid and DateTime
   * **Number** (JsonNumber) => Value Number (int, double, Int64, etc.) or for Enum
   * **Bool** (JsonBool) => Value true | false
   * **Nullable** (JsonNullable) => value null or value (10 for example for a nullable "int?")
   * **Object** (JsonObject) => values: dictionary of key (Json property name used for Json) and Json Value (IJsonValue)
   * **Array** (JsonArray) => Values: List of Json Values
   * **JsonValue** is a _helper_ .Allow to create easily Json Values:
       * _CreateString_
       * _CreateNumber_
       * _CreateBool_
       * _CreateNullable_
       * _CreateObject_
       * _CreateArray_

_Examples:_

```cs
var jsonValue = JsonValue.CreateString("my string value");
```

With Object
```cs
var jsonValue = JsonValue
    .CreateObject()
    .AddNumber("Id",1)
    .AddString("UserName","Marie")
    .AddNullable("Age", null)
    .AddString("Email", null)
    .AddObject("Role", JsonValue.CreateObject().AddNumber("RoleId",2).AddString("Name","Admin"))
    .AddArray("Hobbies", JsonValue.CreateArray().AddString("Shopping").AddString("Cooking"));
```

With Array of Objects
```cs
var jsonValue = JsonValue.CreateArray()
    .AddObject(JsonValue.CreateObject().AddString("UserName", "Marie"))
    .AddObject(JsonValue.CreateObject().AddString("UserName", "Pat"));
```

* **Converters**:

   _Json => Object_:
   * **JsonToJsonValue**: allow to convert Json string to Json Value
   * **JsonValueToObject**: allow to convert Json Value to Object (with Reflection)
   * **JsonToObject** (used by JsonObjectSerializer) : use JsonToJsonValue and JsonValueToObject to convert Json to Object

   _Object => Json_:
   * **ObjectToJsonValue**: allow to convert Object to Json Value
   * **JsonValueToJson**: : allow to convert Json Value to Json
   * **ObjectToJson** (used by JsonObjectSerializer) : use ObjectToJsonValue and JsonValueToJson to convert Object to Json

Converters Xml: 
* _XmlToObject, XmlToXmlValue, XmlValueToObject_
* _ObjectToXml, ObjectToXmlValue, XmlValueToXml_

_Examples_:

Object to Json

```cs
 var user = new User
        {
            Id = 10,
            UserName = "Marie"
        };

var service = new ObjectToJsonValue();
var jsonValue = service.ToJsonObject(user);
```

JsonValue to Json

```cs
var service = new JsonValueToJson();

var jsonValue = JsonValue
        .CreateObject()
        .AddNumber("Id", 1)
        .AddString("UserName", "Marie")
        .AddNullable("Age", null)
        .AddString("Email", null)
        .AddObject("Role", JsonValue.CreateObject().AddNumber("RoleId", 2).AddString("Name", "Admin"))
        .AddArray("Hobbies", JsonValue.CreateArray().AddString("Shopping").AddString("Cooking"));

var json = service.ToObject(jsonValue);

// => {"Id":1,"UserName":"Marie","Age":null,"Email":null,"Role":{"RoleId":2,"Name":"Admin"},"Hobbies":["Shopping","Cooking"]}
```
Other Methods:
* _ToArray : (from JsonArray)_
* _ToNumber: (from JsonNumber)_
* _ToString : (from JsonString)_
* _ToBool : (from JsonBool)_
* _ToNullable: (from JsonNullable)_


## Stringify Object => Json

An **object**

```cs
var json = JsonObjectSerializer.Stringify(user);
```

A **list**/ **array**
```cs
var json = JsonObjectSerializer.Stringify(users);
```

**Values** (_number_, _string_, _bool_, _nullable_)

Example with a _Guid_

```cs
var json = JsonObjectSerializer.Stringify(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176")); // json => "\"344ac1a2-9613-44d7-b64c-8d45b4585176\""
```

With a _Nullable_

```cs
var json = JsonObjectSerializer.Stringify<DateTime?>(new DateTime(1990,12,12));  // json => "\"12/12/1990 00:00:00\""
```

_Null_

```cs
var json = JsonObjectSerializer.Stringify<DateTime?>(null);  // json => "null"
```

_Enum Nullable_

```cs
var json = JsonObjectSerializer.Stringify<MyEnum?>(MyEnum.Default); // json => "0"
```

## Parse Json => Object

**Object**

```cs
// Age is a nullable ( int? )
var json = "{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null}";
var user = JsonObjectSerializer.Parse<User>(json);
```

**List**

```cs
var json ="[{\"Id\":1,\"UserName\":\"Marie\",\"Age\":null,\"Email\":null},{\"Id\":2,\"UserName\":\"Pat\",\"Age\":20,\"Email\":\"pat@domain.com\"}]";
var users = JsonObjectSerializer.Parse<List<User>>(json);
```
**Array**

```cs
var users = JsonObjectSerializer.Parse<User[]>(json);
```

with **Nullables**

```cs
var json = "[1.5,2.5]";
var myNumbers = JsonObjectSerializer.Parse<double?[]>(json);

// or
var myNumbers = JsonObjectSerializer.Parse<List<double?>>(json);
```

**Values**

Example _Enum_

```cs
var json = "1";
var myEnum = JsonObjectSerializer.Parse<MyEnum>(json); // => MyEnum.Other
```

_Guid Nullable_

```cs
var json = "\"344ac1a2-9613-44d7-b64c-8d45b4585176\"";
var myGuid = JsonObjectSerializer.Parse<Guid?>(json); // => new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176")
```

_Null_

```cs
var json = "null";
var myInt = JsonObjectSerializer.Parse<int?>(json); // => null
```

## Mapping

* **LowerCase Strategy** for a **Type** (User for example) or for **all types**
    * Object => Json : property names are converted to lower case in Json
    * Json => Object:  Check property names in lower case to resolve json names
* Or **Mapping** for each property of a Type

Set Lower Strategy for all Types

```cs
Mapping.SetLowerStrategyForAllTypes();
```

Set Lower Case for a Type
```cs
 Mapping.SetType<User>().SetToLowerCaseStrategy();
 ```

 Set The Mapping for a Type
 ```cs
Mapping.SetType<User>()
        .SetProperty("Id", "id")
        .SetProperty("UserName", "username")
        .SetProperty("Age", "age")
        .SetProperty("Email", "email");
```

**Use**

```cs
var json = JsonObjectSerializer.Stringify(user, Mapping.GetContainer());

var user = JsonObjectSerializer.Parse<User>(json, Mapping.GetContainer());
```

Or **Create** a **container** (contains mappings for all types)
```cs
var mappings = new MappingContainer();

mappings.SetType<User>()
        .SetProperty("Id", "user_id")
        .SetProperty("UserName", "user_username")
        .SetProperty("Age", "user_age")
        .SetProperty("Email", "user_email");

mappings.SetType<Role>().SetToLowerCaseStrategy();
```

And use the container
```cs
var json = JsonObjectSerializer.Stringify(user, mappings);

var user = JsonObjectSerializer.Parse<User>(json, mappings);
```

## Xml

### Object => Xml

Support values: _String_, _Number_ ( Int32, Int64, Uint32, Double, Single, etc. ), _Bool_, _Nullable_ ( example int? or DateTime? ), _Object_, _Array_. 

If a value is _null_ (string, nullable, object, array), the attribute _xsi:nil="true"_ is added on the element and the namespace _xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"_ on the root element.

Example with a list

```cs
var users = new List<User>
            {
                new User{ Id = 1, UserName = "Marie"},
                new User{ Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com"}
            };

var xml = JsonObjectSerializer.ToXml(users);
```

_output:_
```xml
<?xml version="1.0"?>
<ArrayOfUser xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<User>
		<Id>1</Id>
		<UserName>Marie</UserName>
		<Age xsi:nil="true" />
		<Email xsi:nil="true" />
	</User>
	<User>
		<Id>2</Id>
		<UserName>Pat</UserName>
		<Age>20</Age>
		<Email>pat@domain.com</Email>
	</User>
</ArrayOfUser>
```

With **Xml Mapping**. Allow to rename nodes. Example:

```cs
var users = new List<User>
            {
                new User{ Id = 1, UserName = "Marie"},
                new User{ Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com"}
            };

var mappings = new XmlMappingContainer();
mappings.SetType<User>("MyUser")
                .SetArrayName("MyUsers")
                .SetProperty("Id", "MyId")
                .SetProperty("UserName", "MyUserName")
                .SetProperty("Age", "MyAge")
                .SetProperty("Email", "MyEmail");

var xml = service.ToXml(users, mappings);
```

_output:_
```xml
<?xml version="1.0"?>
<MyUsers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<MyUser>
		<MyId>1</MyId>
		<MyUserName>Marie</MyUserName>
		<MyAge xsi:nil="true" />
		<MyEmail xsi:nil="true" />
	</MyUser>
	<MyUser>
		<MyId>2</MyId>
		<MyUserName>Pat</MyUserName>
		<MyAge>20</MyAge>
		<MyEmail>pat@domain.com</MyEmail>
	</MyUser>
</MyUsers>
```

### Xml => Object

Example :

```cs
var users = JsonObjectSerializer.FromXml<List<User>(xml);
```

### With Dictionaries

oject => Xml

```cs
 var users = new Dictionary<int, User>
            {
                { 1, new User { Id = 1, UserName = "Marie" } },
                { 2, new User { Id = 2, UserName = "Pat", Age = 20, Email = "pat@domain.com" } }
            };

var xml = JsonObjectSerializer.ToXml(users);
```

_output:_

```xml
<?xml version="1.0"?>
<ArrayOfUser xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<User>
		<Key>1</Key>
		<Value>
			<Id>1</Id>
			<UserName>Marie</UserName>
			<Age xsi:nil="true" />
			<Email xsi:nil="true" />
		</Value>
	</User>
	<User>
		<Key>2</Key>
		<Value>
			<Id>2</Id>
			<UserName>Pat</UserName>
			<Age>20</Age>
			<Email>pat@domain.com</Email>
		</Value>
	</User>
</ArrayOfUser>
```
_note: support Mapping_

Xml => Object

Nodes not require to be named "Key" and "Value". But The key have to be the first element of key / value pair. 

```cs
var users = service.FromXml<Dictionary<int,User>>(xml);
```


## Pro / Cons

Pro:
* _Not Have to define Known Types_ (_DataContractJsonSerializer_)
* Easy To set **Mapping**
* **Easy** To **Use** (_Parse_, _Stringify_)
* **Converters**, **Services**
* **Testable**
* _Available for Uwp, Wpf, Windows Forms, Asp.Net, ..._

Cons:
* _Not the fastest_