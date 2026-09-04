# scripts-generator

## Roslyn formatter and validator

The project includes an isolated .NET tool in
`Tools/ScriptsGenerator.Roslyn`. It formats and validates generated C# without
adding Roslyn Workspaces to the Unity runtime.

```powershell
dotnet run --project Tools\ScriptsGenerator.Roslyn -- format input.cs output.cs
dotnet run --project Tools\ScriptsGenerator.Roslyn -- validate output.cs
dotnet test Tools\ScriptsGenerator.Roslyn.Tests
```

The formatter currently uses Roslyn's syntax tree and
`NormalizeWhitespace()`. The Unity generator exposes a formatter hook through the
`ScriptGenerator(GeneratorSettings, Func<string, string>)` constructor. This
keeps Roslyn outside the Unity runtime while allowing an integration layer to
pass `RoslynCodeFormatter.Format` when both components run in the same .NET
process.