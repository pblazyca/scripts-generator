using System.Collections.Generic;
using System.Text;
using ScriptsGenerator.Structures;
using CodeHappiness.Core;

namespace ScriptsGenerator.Core
{
    public class ScriptGenerator : BaseGenerator
    {
        private StringBuilder WriterBuilder { get; set; }

        public ScriptGenerator(GeneratorSettings settings) : base(settings)
        {
            WriterBuilder = new StringBuilder();
        }

        public void WriteUsing(UsingInfo namespaceInfo)
        {
            ExecuteWriteUsing(namespaceInfo);
            ExecuteWriter();
        }

        public void WriteUsing(List<UsingInfo> namespaceCollection)
        {
            for (int i = 0; i < namespaceCollection.Count; i++)
            {
                ExecuteWriteUsing(namespaceCollection[i]);
            }

            ExecuteWriter();
        }

        public void BeginNamespace(NamespaceInfo namespaceInfo)
        {
            WriterBuilder.AppendLine($"namespace {namespaceInfo.Name}");

            ExecuteWriter();
            BeginBlock();
        }

        public void EndNamespace()
        {
            WriteEmptyLine();
            EndBlock();
        }

        public void WriteProperty(PropertyInfo property)
        {
            ExecuteWriteProperty(property);
            ExecuteWriterLine();
        }

        public void WriteProperty(List<PropertyInfo> propertiesCollection)
        {
            for (int i = 0; i < propertiesCollection.Count; i++)
            {
                ExecuteWriteProperty(propertiesCollection[i]);
                ExecuteWriterLine();
            }
        }

        public void WriteField(List<FieldInfo> fieldsCollection)
        {
            for (int i = 0; i < fieldsCollection.Count; i++)
            {
                fieldsCollection[i].Write(WriterBuilder);
                ExecuteWriterLine();
            }
        }

        public void WriteField(FieldInfo field)
        {
            field.Write(WriterBuilder);
            ExecuteWriterLine();
        }

        public void BeginClass(AccessModifiers accessModifier, string className, string baseClassName = null, List<InterfaceInfo> implementedInterfaceNameCollection = null)
        {
            string accessModifierLabel = Converters.ConvertEnumToLabel(accessModifier);
            WriterBuilder.Append($"{accessModifierLabel} class {className}");

            if (baseClassName != null)
            {
                WriterBuilder.Append($" : {baseClassName}");
            }

            if (implementedInterfaceNameCollection != null)
            {
                for (int i = 0; i < implementedInterfaceNameCollection.Count; i++)
                {
                    WriterBuilder.Append($", {implementedInterfaceNameCollection[i].Name}");
                }
            }

            WriteEmptyLine();
            ExecuteWriterLine();
            BeginBlock();
        }

        public void EndClass()
        {
            EndBlock();
        }

        public void WriteAbstractMethod(MethodInfo methodInfo)
        {
            WriteMethod(methodInfo);
        }

        public void BeginMethod(MethodInfo methodInfo)
        {
            WriteMethod(methodInfo);
            BeginBlock();
        }

        public void EndMethod()
        {
            WriteEmptyLine();
            EndBlock();
        }

        private void WriteMethod(MethodInfo methodInfo)
        {
            WriterBuilder.Append($"{Converters.ConvertEnumToLabel(methodInfo.Modifier)} ");

            if (methodInfo.Keyword != PolymorphismKeyword.NONE)
            {
                WriterBuilder.Append($"{Converters.ConvertEnumToLabel(methodInfo.Keyword)} ");
            }

            WriterBuilder.Append($"{GeneratorTools.GetTypeLabel(methodInfo.Type)} ");
            WriterBuilder.Append(methodInfo.Name);

            if (methodInfo.ParametersCollection != null)
            {
                WriteMethodParameters(methodInfo.ParametersCollection);
            }
            else
            {
                WriterBuilder.Append("()");
            }

            if (methodInfo.Keyword == PolymorphismKeyword.ABSTRACT)
            {
                WriterBuilder.Append(';');
            }

            WriteEmptyLine();
            ExecuteWriterLine();
        }

        private void WriteMethodParameters(List<VariableInfo> parametersCollection)
        {
            WriterBuilder.Append('(');

            for (int i = 0; i < parametersCollection.Count; i++)
            {
                if (i != 0)
                {
                    WriterBuilder.Append(Constants.SPACE);
                }

                string parametersTypeLabel = GeneratorTools.GetTypeLabel(parametersCollection[i].Type);
                WriterBuilder.Append($"{parametersTypeLabel} {parametersCollection[i].Name}");

                if (i != parametersCollection.Count - 1)
                {
                    WriterBuilder.Append(',');
                }
            }

            WriterBuilder.Append(')');
        }

        private void ExecuteWriter()
        {
            WriteText(WriterBuilder.ToString());
            WriterBuilder.Clear();
        }

        private void ExecuteWriterLine()
        {
            WriteTextLine(WriterBuilder.ToString());
            WriterBuilder.Clear();
        }

        private void ExecuteWriteUsing(UsingInfo namespaceInfo)
        {
            WriterBuilder.AppendLine($"using {namespaceInfo.Name};");
        }

        private void ExecuteWriteProperty(PropertyInfo property)
        {
            VariableInfo variable = property.Variable;
            string accessModifierLabel = Converters.ConvertEnumToLabel(property.Modifier);

            WriterBuilder.Append($"{accessModifierLabel} {GeneratorTools.GetTypeLabel(variable.Type)} {variable.Name} {{ get; set; }}");

            if (string.IsNullOrEmpty(variable.DefaultValue) == false)
            {
                WriterBuilder.Append($" = {variable.DefaultValue};");
            }
        }
    }
}

