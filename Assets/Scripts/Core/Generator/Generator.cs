using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ScriptsGenerator.Core
{
    public class Generator
    {
        public StringBuilder CodeBuilder { get; private set; }

        protected GeneratorSettings Settings { get; set; }

        private int IndentLevel { get; set; }

        protected const char SPACE = ' ';
        protected const char UNDERLINE = '_';

        private const char NEW_LINE = '\n';
        private const char TAB = '\t';

        public Generator(GeneratorSettings settings)
        {
            CodeBuilder = new StringBuilder();
            Settings = settings;
        }

        protected void BeginBlock()
        {
            WriteIndent();
            WriteChar('{');
            WriteEmptyLine();
            IndentLevel++;
        }

        protected void EndBlock()
        {
            IndentLevel--;
            WriteIndent();
            WriteChar('}');
            WriteEmptyLine();
        }

        protected void WriteText(string content)
        {
            CodeBuilder.Append(content);
        }

        protected void WriteTextLine(string content)
        {
            WriteIndent();
            CodeBuilder.AppendLine(content);
        }

        protected void WriteEmptyLine()
        {
            WriteChar(NEW_LINE);
        }

        protected void WriteIndent()
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

        protected void WriteSpace()
        {
            WriteChar(SPACE);
        }

        protected void WriteSpace(int count)
        {
            WriteChar(SPACE, count);
        }

        protected void WriteTab()
        {
            WriteChar(TAB);
        }

        protected void WriteTab(int count)
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
}