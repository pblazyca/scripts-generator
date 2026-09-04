using System;
using System.IO;
using System.Text;
using CodeHappiness.Core;

namespace ScriptsGenerator.Core
{
    public class BaseGenerator
    {
        public StringBuilder CodeBuilder { get; private set; }

        protected GeneratorSettings Settings { get; set; }

        private int IndentLevel { get; set; }

        private readonly Func<string, string> CodeFormatter;

        public BaseGenerator(GeneratorSettings settings, Func<string, string> codeFormatter = null)
        {
            CodeBuilder = new StringBuilder();
            Settings = settings;
            CodeFormatter = codeFormatter;
        }

        public string GetCode()
        {
            string code = CodeBuilder.ToString();
            return CodeFormatter == null ? code : CodeFormatter(code);
        }

        public void SaveToFile(string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            string directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(filePath, GetCode(), Encoding.UTF8);
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