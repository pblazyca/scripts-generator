using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ScriptsGenerator.Roslyn.Syntax;

public static class RoslynTypeSyntaxFactory
{
    public static TypeSyntax Create(string type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        return ParseTypeName(type);
    }
}
