namespace ScriptsGenerator.Roslyn;

public sealed class GenerationRequest
{
    public string? Namespace { get; init; }
    public string? ClassName { get; init; }
    public IReadOnlyList<string>? Usings { get; init; }
    public IReadOnlyList<RoslynField>? Fields { get; init; }
    public IReadOnlyList<RoslynProperty>? Properties { get; init; }
    public IReadOnlyList<RoslynMethod>? Methods { get; init; }
}
