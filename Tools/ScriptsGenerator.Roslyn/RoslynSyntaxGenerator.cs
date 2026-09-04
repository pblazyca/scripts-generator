using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ScriptsGenerator.Roslyn;

public sealed record RoslynField(string Type, string Name);

public sealed record RoslynProperty(string Type, string Name);

public sealed record RoslynParameter(string Type, string Name);

public sealed record RoslynMethod(
    string ReturnType,
    string Name,
    IEnumerable<RoslynParameter>? Parameters = null);

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
                fields.Select(CreateField).ToArray());
        }

        if (properties != null)
        {
            classDeclaration = classDeclaration.AddMembers(
                properties.Select(CreateProperty).ToArray());
        }

        if (methods != null)
        {
            classDeclaration = classDeclaration.AddMembers(
                methods.Select(CreateMethod).ToArray());
        }

        NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(classDeclaration);

        return compilationUnit
            .AddMembers(namespaceDeclaration)
            .NormalizeWhitespace()
            .ToFullString();
    }

    private static FieldDeclarationSyntax CreateField(RoslynField field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(field.Type);
        ArgumentException.ThrowIfNullOrWhiteSpace(field.Name);

        return FieldDeclaration(
                VariableDeclaration(ParseTypeName(field.Type))
                    .AddVariables(VariableDeclarator(field.Name)))
            .AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }

    private static PropertyDeclarationSyntax CreateProperty(RoslynProperty property)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property.Type);
        ArgumentException.ThrowIfNullOrWhiteSpace(property.Name);

        return PropertyDeclaration(ParseTypeName(property.Type), property.Name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithAccessorList(AccessorList(
                List(new[]
                {
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                })));
    }

    private static MethodDeclarationSyntax CreateMethod(RoslynMethod method)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(method.ReturnType);
        ArgumentException.ThrowIfNullOrWhiteSpace(method.Name);

        MethodDeclarationSyntax declaration = MethodDeclaration(
                ParseTypeName(method.ReturnType),
                method.Name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithBody(Block());

        if (method.Parameters != null)
        {
            declaration = declaration.AddParameterListParameters(
                method.Parameters.Select(CreateParameter).ToArray());
        }

        return declaration;
    }

    private static ParameterSyntax CreateParameter(RoslynParameter parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter.Type);
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter.Name);

        return Parameter(Identifier(parameter.Name))
            .WithType(ParseTypeName(parameter.Type));
    }
}
