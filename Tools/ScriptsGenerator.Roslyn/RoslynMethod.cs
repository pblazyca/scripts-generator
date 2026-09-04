namespace ScriptsGenerator.Roslyn;

public sealed record RoslynMethod(
    string ReturnType,
    string Name,
    IEnumerable<RoslynParameter>? Parameters = null);
