namespace ScriptsGenerator.Roslyn.Adapters;

public static class GenerationRequestAdapter
{
    public static string Generate(GenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new RoslynSyntaxGenerator().GenerateClass(
            request.Namespace ?? throw new ArgumentException("Namespace is required.", nameof(request)),
            request.ClassName ?? throw new ArgumentException("Class name is required.", nameof(request)),
            request.Usings,
            request.Fields,
            request.Properties,
            request.Methods);
    }
}
