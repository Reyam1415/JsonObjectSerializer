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
    * **ActiveCache** (active by default)

* **Services**:
   * **JsonObjectSerializerService** (IJsonObjectSerializerService)
   * **Beautifier** (IBeautifier): used to Format / Indent Json
   * **AssemblyInfoService** (IAssemblyInfoService): used to resolve Object values and properties

* **Json Values** (IJsonElementValue) (named JsonElement... to avoid conflicts with Windows.Data.Json):
   * **String** (JsonElementString) => Value string, used for Guid and DateTime
   * **Number** (JsonElementNumber) => Value Number (int, double, Int64, etc.) or for Enum
   * **Bool** (JsonElementBool) => Value true | false
   * **Nullable** (JsonElementNullable) => value null or value (10 for example for a nullable "int?")
   * **Object** (JsonElementObject) => values: dictionary of key (Json property name used for Json) and Json Value (IJsonElementValue)
   * **Array** (JsonElementArray) => Values: List of Json Values
   * **JsonElementValue** is a _helper_ .Allow to create easily Json Values:
       * _CreateString_
       * _CreateNumber_
       * _CreateBool_
       * _CreateNullable_
       * _CreateObject_
       * _CreateArray_

_Examples:_

```cs
var jsonValue = JsonElementValue.CreateString("my string value");
```

With Object
```cs
var jsonValue = JsonElementValue
    .CreateObject()
    .AddNumber("Id",1)
    .AddString("UserName","Marie")
    .AddNullable("Age", null)
    .AddString("Email", null)
    .AddObject("Role", JsonElementValue.CreateObject().AddNumber("RoleId",2).AddString("Name","Admin"))
    .AddArray("Hobbies", JsonElementValue.CreateArray().AddString("Shopping").AddString("Cooking"));
```

With Array of Objects
```cs
var jsonValue = JsonElementValue.CreateArray()
    .AddObject(JsonElementValue.CreateObject().AddString("UserName", "Marie"))
    .AddObject(JsonElementValue.CreateObject().AddString("UserName", "Pat"));
```

* **Converters**:

   _Json => Object_:
   * **JsonToJsonValue**: allow to convert Json string to Json Value
   * **JsonValueToObject**: allow to convert Json Value to Object (with Reflection)
   * **JsonToObject** (used by JsonObjectSerializer) : use JsonToJsonValue and JsonValueToObject to convert Json to Object

   _Object => Json_:
   * ***ObjectToJsonValue**: allow to convert Object to Json Value
   * **JsonValueToJson**: : allow to convert Json Value to Json
   * **ObjectToJson** (used by JsonObjectSerializer) : use ObjectToJsonValue and JsonValueToJson to convert Object to Json

_Example_:

```cs
 var user = new User
        {
            Id = 10,
            UserName = "Marie"
        };

var service = new ObjectToJsonValue();
var jsonValue = service.ToJsonObject(user);
```

## Stringify Object => Json

An **object**

```cs
var json = JsonObjectSerializer.Stringify(user);
```

A **list**/ **array**
```cs
var json = JsonObjectSerializer.Stringify(users);
```

**Values** (number, string, bool, nullable)

Example with a Guid

```cs
var json = JsonObjectSerializer.Stringify(new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176")); // \"344ac1a2-9613-44d7-b64c-8d45b4585176\"
```

With a Nullable

```cs
var json = JsonObjectSerializer.Stringify<DateTime?>(new DateTime(1990,12,12));  // \"12/12/1990 00:00:00\"
```

Null

```cs
var json = JsonObjectSerializer.Stringify<DateTime?>(null);  // null
```

Enum Nullable
```cs
var json = JsonObjectSerializer.Stringify<MyEnum?>(MyEnum.Default); // 0
```

## Parse Json => Object

**Object**

```cs
var user = JsonObjectSerializer.Parse<User>(json);
```

**List**

```cs
var users = JsonObjectSerializer.Parse<List<User>>(json);
```
**Array**

```cs
var users = JsonObjectSerializer.Parse<User[]>(json);
```

**Values**

Example Enum

```cs
var myEnum = JsonObjectSerializer.Parse<MyEnum>("1"); // => MyEnum.Other
```

Guid Nullable

```cs
var myGuid = JsonObjectSerializer.Parse<Guid?>("\"344ac1a2-9613-44d7-b64c-8d45b4585176\"")); // => new Guid("344ac1a2-9613-44d7-b64c-8d45b4585176")
```

Null

```cs
var myGuid = JsonObjectSerializer.Parse<int?>("null")); // => null
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