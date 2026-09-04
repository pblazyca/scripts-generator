using ScriptsGenerator.Roslyn;
using Xunit;

namespace ScriptsGenerator.Roslyn.Tests;

public sealed class RoslynCodeFormatterTests
{
    private readonly RoslynCodeFormatter formatter = new();
    private readonly RoslynSyntaxGenerator syntaxGenerator = new();

    [Fact]
    public void Format_ProducesNormalizedCSharp()
    {
        const string source = "using System; public class Example{public void Run(){Console.WriteLine(\"ok\");}}";

        string result = formatter.Format(source);

        Assert.Equal(
            """
            using System;

            public class Example
            {
                public void Run()
                {
                    Console.WriteLine("ok");
                }
            }
            """,
            result);
    }

    [Fact]
    public void Format_IsDeterministic()
    {
        const string source = "public class Example{public int Value{get;set;}}";

        Assert.Equal(formatter.Format(source), formatter.Format(source));
    }

    [Fact]
    public void Validate_ReturnsNoErrorsForValidCSharp()
    {
        const string source = "public class Example { public void Run() { } }";

        Assert.Empty(formatter.Validate(source));
    }

    [Fact]
    public void Validate_ReturnsSyntaxDiagnosticsWithPosition()
    {
        const string source = "public class Example\n{\n    public void Run( }\n";

        CodeDiagnostic diagnostic = Assert.Single(
            formatter.Validate(source).Where(item => item.Id == "CS1026"));

        Assert.Equal("CS1026", diagnostic.Id);
        Assert.Equal("Error", diagnostic.Severity);
        Assert.Equal(3, diagnostic.Line);
        Assert.Equal(22, diagnostic.Column);
    }

    [Fact]
    public void Validate_AllowsEmptySource()
    {
        Assert.Empty(formatter.Validate(string.Empty));
    }

    [Fact]
    public void Format_RejectsNullSource()
    {
        Assert.Throws<ArgumentNullException>(() => formatter.Format(null!));
    }

    [Fact]
    public void Validate_RejectsNullSource()
    {
        Assert.Throws<ArgumentNullException>(() => formatter.Validate(null!));
    }

    [Fact]
    public void GenerateClass_UsesSyntaxTreeForUsingsNamespaceAndClass()
    {
        string result = syntaxGenerator.GenerateClass(
            "Generated",
            "Example",
            new[] { "System", "System.Collections.Generic" });

        Assert.Equal(
            """
            using System;
            using System.Collections.Generic;

            namespace Generated
            {
                public class Example
                {
                }
            }
            """,
            result);
        Assert.Empty(formatter.Validate(result));
    }

    [Fact]
    public void GenerateClass_AddsFieldsAndPropertiesThroughSyntaxNodes()
    {
        string result = syntaxGenerator.GenerateClass(
            "Generated",
            "Example",
            fields: new[] { new RoslynField("int", "_count") },
            properties: new[] { new RoslynProperty("string", "Name") });

        Assert.Equal(
            """
            namespace Generated
            {
                public class Example
                {
                    private int _count;
                    public string Name { get; set; }
                }
            }
            """,
            result);
        Assert.Empty(formatter.Validate(result));
    }

    [Fact]
    public void GenerateClass_AddsMethodAndParametersThroughSyntaxNodes()
    {
        string result = syntaxGenerator.GenerateClass(
            "Generated",
            "Example",
            methods: new[]
            {
                new RoslynMethod(
                    "void",
                    "Run",
                    new[]
                    {
                        new RoslynParameter("int", "count"),
                        new RoslynParameter("string", "label")
                    })
            });

        Assert.Equal(
            """
            namespace Generated
            {
                public class Example
                {
                    public void Run(int count, string label)
                    {
                    }
                }
            }
            """,
            result);
        Assert.Empty(formatter.Validate(result));
    }

    [Fact]
    public void GenerateClass_RejectsInvalidMethodReturnType()
    {
        Assert.Throws<ArgumentException>(() =>
            syntaxGenerator.GenerateClass(
                "Generated",
                "Example",
                methods: new[] { new RoslynMethod(string.Empty, "Run") }));
    }

    [Fact]
    public void GenerateClass_RejectsInvalidFieldType()
    {
        Assert.Throws<ArgumentException>(() =>
            syntaxGenerator.GenerateClass(
                "Generated",
                "Example",
                fields: new[] { new RoslynField(string.Empty, "_count") }));
    }

    [Fact]
    public void GenerateClass_RejectsMissingNamespace()
    {
        Assert.Throws<ArgumentException>(
            () => syntaxGenerator.GenerateClass(string.Empty, "Example"));
    }

    [Fact]
    public void GenerateClass_RejectsMissingClassName()
    {
        Assert.Throws<ArgumentException>(
            () => syntaxGenerator.GenerateClass("Generated", string.Empty));
    }
}
