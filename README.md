# JsonObjectSerializer
Json Serializer for .NET projects (DOTNET)


Support :
- string, number, boolean, date, enum, nullables, object, array
- recursive
- Available for :
    - Wpf
    - Windows forms
    - Uwp
    - etc.


### Installation with NuGet

```
PM> Install-Package JsonObjectSerializer
```

## Serialization (from object(s) to Json)

An object

```cs
var json = JsonObjectSerializer.Stringify(item);
```

A list / array
```cs
var json = JsonObjectSerializer.Stringify(items);
```

### Format Json (indented)

```cs
var json = JsonObjectSerializer.Stringify(item, true);
```


## Deserialization (from json string to object(s))
An object

```cs
var result = JsonObjectSerializer.Parse<Item>(json);
```

A list / array
```cs
var result = JsonObjectSerializer.Parse<List<Item>>(json);
```
Or
```cs
var result = JsonObjectSerializer.Parse<Item[]>(json);
```

## Mapping


### Manual
```cs
JsonMapping.Default
        .Add(typeof(Item), "MyInt", "my_int")
        .Add(typeof(Item), "MyDouble", "my_double")
        .Add(typeof(Item), "MyBool", "my_bool")
        .Add(typeof(Item), "MyStrings", "my_strings")
        .Add(typeof(Item), "Sub", "the_sub")
        .Add(typeof(SubItem), "SubItemInt","the_sub_int");
                
// then stringify
```


### By Attribute

Define mapping on properties

```cs
public class Item
{
    [JsonMap(JsonElementKey = "my_int")]
    public int MyInt { get; set; }

    [JsonMap(JsonElementKey = "my_double")]
    public double MyDouble { get; set; }

    // not modified
    public string MyString { get; set; }

    // etc.

    [JsonMap(JsonElementKey = "my_strings")]
    public List<string> MyStrings { get; set; }

    [JsonMap(JsonElementKey = "the_sub")]
    public SubItem Sub { get; set; }
}
public class SubItem
{
    [JsonMap(JsonElementKey = "the_sub_int")]
    public int SubItemInt { get; set; }

    public string SubItemString { get; set; }
}
```

Important : set "UseJsonMapAttributes" to "true". By default, it's not active for best performances.

```cs
JsonObjectSerializer.UseJsonMapAttributes = true;

var json = JsonObjectSerializer.Stringify(item);
```


