# scripts-generator

## Roslyn formatter and validator

The project includes an isolated .NET tool in
`Tools/ScriptsGenerator.Roslyn`. It formats and validates generated C# without
adding Roslyn Workspaces to the Unity runtime.

```powershell
dotnet run --project Tools\ScriptsGenerator.Roslyn -- format input.cs output.cs
dotnet run --project Tools\ScriptsGenerator.Roslyn -- validate output.cs
```

The formatter currently uses Roslyn's syntax tree and
`NormalizeWhitespace()`. The Unity generator can be connected to this tool in
a later step, after the formatting contract is covered by tests.