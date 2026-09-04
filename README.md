# scripts-generator

## Roslyn formatter and validator

The project includes an isolated .NET tool in
`Tools/ScriptsGenerator.Roslyn`. It formats and validates generated C# without
adding Roslyn Workspaces to the Unity runtime.

```powershell
dotnet run --project Tools\ScriptsGenerator.Roslyn -- format input.cs output.cs
dotnet run --project Tools\ScriptsGenerator.Roslyn -- validate output.cs
dotnet run --project Tools\ScriptsGenerator.Roslyn -- generate request.json output.cs
dotnet test Tools\ScriptsGenerator.Roslyn.Tests
```

The formatter currently uses Roslyn's syntax tree and
`NormalizeWhitespace()`. The Unity generator exposes a formatter hook through the
`ScriptGenerator(GeneratorSettings, Func<string, string>)` constructor. This
keeps Roslyn outside the Unity runtime while allowing an integration layer to
pass `RoslynCodeFormatter.Format` when both components run in the same .NET
process.

The `generate` command accepts a JSON contract:

```json
{
  "namespace": "Generated",
  "className": "Example",
  "usings": ["System"],
  "fields": [{ "type": "int", "name": "_count" }],
  "properties": [{ "type": "string", "name": "Name" }],
  "methods": [{
    "returnType": "void",
    "name": "Run",
    "parameters": [{ "type": "int", "name": "count" }]
  }]
}
```

The generated result can be written directly to a C# file:

```csharp
generator.SaveToFile("Generated/Example.cs");
```

`SaveToFile` uses `GetCode()`, so an injected formatter runs before the file is
written and missing directories are created automatically.

In the Unity Editor, select a `.cs` asset and use
`Scripts Generator/Roslyn/Format Selected C# File` or
`Scripts Generator/Roslyn/Validate Selected C# File`. The menu invokes the
isolated CLI through `dotnet`, so the Roslyn assemblies are not loaded by
Unity.