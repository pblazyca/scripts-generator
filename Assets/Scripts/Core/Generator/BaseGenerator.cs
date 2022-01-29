using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ScriptsGenerator.Core
{
    public class BaseGenerator
    {
        public StringBuilder CodeBuilder { get; private set; }

        protected GeneratorSettings Settings { get; set; }

        private int IndentLevel { get; set; }

        protected const char SPACE = ' ';
        protected const char UNDERLINE = '_';

        private const char NEW_LINE = '\n';
        private const char TAB = '\t';

        public BaseGenerator(GeneratorSettings settings)
        {
            CodeBuilder = new StringBuilder();
            Settings = settings;
        }

        public void BeginBlock()
        {
            WriteIndent();
            WriteChar('{');
            IndentLevel++;
        }

        public void EndBlock()
        {
            IndentLevel--;
            WriteIndent();
            WriteChar('}');
        }

        public void WriteText(string content)
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
}