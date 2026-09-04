using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ScriptsGenerator.Roslyn.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ScriptsGenerator.Roslyn;

public sealed class RoslynSyntaxGenerator
{
    public string GenerateClass(
        string namespaceName,
        string className,
        IEnumerable<string>? usings = null,
        IEnumerable<RoslynField>? fields = null,
        IEnumerable<RoslynProperty>? properties = null,
        IEnumerable<RoslynMethod>? methods = null)
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

        if (fields != null)
        {
            classDeclaration = classDeclaration.AddMembers(
                fields.Select(RoslynMemberSyntaxFactory.CreateField).ToArray());
        }

        if (properties != null)
        {
            classDeclaration = classDeclaration.AddMembers(
                properties.Select(RoslynMemberSyntaxFactory.CreateProperty).ToArray());
        }

        if (methods != null)
        {
            classDeclaration = classDeclaration.AddMembers(
                methods.Select(RoslynMemberSyntaxFactory.CreateMethod).ToArray());
        }

        NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(classDeclaration);

        return compilationUnit
            .AddMembers(namespaceDeclaration)
            .NormalizeWhitespace()
            .ToFullString();
    }

}
