using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Generator
{
    public StringBuilder CodeBuilder { get; private set; }

    private int IndentLevel { get; set; }
    private GeneratorSettings Settings { get; set; }

    private const char NEW_LINE = '\n';
    private const char TAB = '\t';
    private const char SPACE = ' ';

    //TODO: To think - maybe better get Environment.NewLine
    private readonly string NewLine;

    public Generator(GeneratorSettings settings)
    {
        CodeBuilder = new StringBuilder();
        NewLine = Environment.NewLine;
        Settings = settings;
    }

    public void BeginBlock()
    {
        WriteIndent();
        WriteChar('{');
        WriteEmptyLine();
        IndentLevel++;
    }

    public void EndBlock()
    {
        IndentLevel--;
        WriteIndent();
        WriteChar('}');
        WriteEmptyLine();
    }

    public void WrtieText(string content)
    {
        CodeBuilder.Append(content);
    }

    public void WriteTextLine(string content)
    {
        WriteIndent();
        CodeBuilder.AppendLine(content);
    }

    public void WriteEmptyLine()
    {
        WriteChar(NEW_LINE);
    }

    public void WriteIndent()
    {
        switch (Settings.IndentStyle)
        {
            case IndentStyle.TAB:
                WriteTab(IndentLevel);
                break;

            case IndentStyle.SPACE:
                WriteSpace(IndentLevel);
                break;
        }
    }

    public void WriteSpace()
    {
        WriteChar(SPACE);
    }

    public void WriteSpace(int count)
    {
        WriteChar(SPACE, count);
    }

    public void WriteTab()
    {
        WriteChar(TAB);
    }

    public void WriteTab(int count)
    {
        WriteChar(TAB, count);
    }

    private void WriteChar(char value)
    {
        CodeBuilder.Append(value);
    }

    private void WriteChar(char value, int count)
    {
        CodeBuilder.Append(value, count);
    }
}
