using ScriptsGenerator.Roslyn;

if (args.Length is < 2 or > 3)
{
    PrintUsage();
    return 2;
}

string command = args[0].ToLowerInvariant();
string inputPath = args[1];
string outputPath = args.Length == 3 ? args[2] : inputPath;

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
    return 1;
}

RoslynCodeFormatter formatter = new();
string source = File.ReadAllText(inputPath);

switch (command)
{
    case "format":
        File.WriteAllText(outputPath, formatter.Format(source));
        return 0;

    case "validate":
        IReadOnlyList<CodeDiagnostic> diagnostics = formatter.Validate(source);
        foreach (CodeDiagnostic diagnostic in diagnostics)
        {
            Console.WriteLine(
                $"{diagnostic.Id} {diagnostic.Severity} " +
                $"({diagnostic.Line},{diagnostic.Column}): {diagnostic.Message}");
        }

        return diagnostics.Count == 0 ? 0 : 1;

    default:
        PrintUsage();
        return 2;
}

static void PrintUsage()
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("  dotnet run -- format <input.cs> [output.cs]");
    Console.Error.WriteLine("  dotnet run -- validate <input.cs>");
}
