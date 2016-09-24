# JsonObjectSerializer
Json Serializer for .NET projects (DOTNET)


Support :
- enum, date, string, number, boolean, object, array
- recursive
- Available for :
    - Wpf
    - Windows forms
    - Uwp
    - etc.


## Serialization (from object(s) to Json)

An object

```cs
var json = JsonObjectSerializer.Stringify(item);
```

A list / array
```cs
var json = JsonObjectSerializer.Stringify(items);
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

