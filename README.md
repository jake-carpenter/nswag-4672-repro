# Repro for Nswag #4672

[https://github.com/RicoSuter/NSwag/issues/4672](https://github.com/RicoSuter/NSwag/issues/4672)

## Repro instructions

```shell
cd Consumer

# Validate build works before generating code.
dotnet build

# Restore tools. Version 14.0.7 of the Nswag CLI tool is installed with the tool manifest.
dotnet tool restore

# Generate client from `./openapi.json`. It was created from the Server project.
dotnet nswag openapi2csclient /input:openapi.json /classname:ExampleClient /namespace:Consumer /output:ExampleClientClient.g.cs

# Run build again and observe failure.
dotnet build
```

## Error

```plaintext
<...>/ExampleClientClient.g.cs(95,45): error CS1061: 'IEnumerable<string>' does not contain a definition for 'Length' and no accessible extension method 'Length' accepting a first argument of type 'IEnumerable<string>' could be found (are you missing a using directive or an assembly reference?)

<...>/ExampleClientClient.g.cs(98,60): error CS0021: Cannot apply indexing with [] to an expression of type 'IEnumerable<string>'
```

## Explanation

A previous refactor to remove LINQ usage overused the `Length` property in generated code by assuming enumerables would be arrays.

This example specifically uses serialized path parameters so that comma-separated values can be provided in the path. This generated code uses `IEnumerable<string>` by default, but any configuration of other options such as `IList<string>`, `ICollection<string>`, or `List<string>` will have the same issue (they all use `Count` over `Length`).

## Example generated code

Here is a snippet of the generated code that will cause issues

```csharp
// Operation Path: "todos/{ids}"
urlBuilder_.Append("todos/");
for (var i = 0; i < ids.Length; i++)
{
    if (i > 0) urlBuilder_.Append(',');
    urlBuilder_.Append(ConvertToString(ids[i], System.Globalization.CultureInfo.InvariantCulture));
}
```
