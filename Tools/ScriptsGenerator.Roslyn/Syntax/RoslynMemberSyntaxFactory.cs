using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ScriptsGenerator.Roslyn.Syntax;

public static class RoslynMemberSyntaxFactory
{
    public static FieldDeclarationSyntax CreateField(RoslynField field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(field.Type);
        ArgumentException.ThrowIfNullOrWhiteSpace(field.Name);

        return FieldDeclaration(
                VariableDeclaration(RoslynTypeSyntaxFactory.Create(field.Type))
                    .AddVariables(VariableDeclarator(field.Name)))
            .AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }

    public static PropertyDeclarationSyntax CreateProperty(RoslynProperty property)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(property.Type);
        ArgumentException.ThrowIfNullOrWhiteSpace(property.Name);

        return PropertyDeclaration(
                RoslynTypeSyntaxFactory.Create(property.Type),
                property.Name)
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

    public static MethodDeclarationSyntax CreateMethod(RoslynMethod method)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(method.ReturnType);
        ArgumentException.ThrowIfNullOrWhiteSpace(method.Name);

        MethodDeclarationSyntax declaration = MethodDeclaration(
                RoslynTypeSyntaxFactory.Create(method.ReturnType),
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
            .WithType(RoslynTypeSyntaxFactory.Create(parameter.Type));
    }
}
