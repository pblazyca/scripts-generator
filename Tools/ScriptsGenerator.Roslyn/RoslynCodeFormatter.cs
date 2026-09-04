using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ScriptsGenerator.Roslyn;

public sealed record CodeDiagnostic(
    string Id,
    string Severity,
    string Message,
    int Line,
    int Column);

public sealed class RoslynCodeFormatter
{
    public string Format(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
        SyntaxNode root = tree.GetRoot();
        return root.NormalizeWhitespace().ToFullString();
    }

    public IReadOnlyList<CodeDiagnostic> Validate(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
        return tree.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(CreateDiagnostic)
            .ToArray();
    }

    private static CodeDiagnostic CreateDiagnostic(Diagnostic diagnostic)
    {
        FileLinePositionSpan span = diagnostic.Location.GetLineSpan();
        return new CodeDiagnostic(
            diagnostic.Id,
            diagnostic.Severity.ToString(),
            diagnostic.GetMessage(),
            span.StartLinePosition.Line + 1,
            span.StartLinePosition.Character + 1);
    }
}
