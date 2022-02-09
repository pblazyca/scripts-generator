using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CodeHappiness.Core;

namespace ScriptsGenerator.Core
{
    public class BaseGenerator
    {
        public StringBuilder CodeBuilder { get; private set; }

        protected GeneratorSettings Settings { get; set; }

        private int IndentLevel { get; set; }

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
            WriteChar(Constants.NEW_LINE);
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
            WriteChar(Constants.SPACE);
        }

        public void WriteSpace(int count)
        {
            WriteChar(Constants.SPACE, count);
        }

        public void WriteTab()
        {
            WriteChar(Constants.TAB);
        }

        public void WriteTab(int count)
        {
            WriteChar(Constants.TAB, count);
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