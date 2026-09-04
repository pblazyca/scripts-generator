using ScriptsGenerator.Roslyn;
using Xunit;

namespace ScriptsGenerator.Roslyn.Tests;

public sealed class RoslynCodeFormatterTests
{
    private readonly RoslynCodeFormatter formatter = new();

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
}
