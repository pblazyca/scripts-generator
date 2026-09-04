using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ScriptsGenerator.Roslyn;

public sealed class RoslynSyntaxGenerator
{
    public string GenerateClass(
        string namespaceName,
        string className,
        IEnumerable<string>? usings = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(namespaceName);
        ArgumentException.ThrowIfNullOrWhiteSpace(className);

        CompilationUnitSyntax compilationUnit = CompilationUnit();

        if (usings != null)
        {
            compilationUnit = compilationUnit.AddUsings(
                usings.Select(namespaceValue =>
                    UsingDirective(ParseName(namespaceValue))).ToArray());
        }

        ClassDeclarationSyntax classDeclaration = ClassDeclaration(className)
            .AddModifiers(Token(SyntaxKind.PublicKeyword));

        NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(classDeclaration);

        return compilationUnit
            .AddMembers(namespaceDeclaration)
            .NormalizeWhitespace()
            .ToFullString();
    }
}
