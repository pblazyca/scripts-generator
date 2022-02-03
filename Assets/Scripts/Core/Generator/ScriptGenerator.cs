using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using ScriptsGenerator.Structures;

using PropertyInfo = ScriptsGenerator.Structures.PropertyInfo;
using MethodInfo = ScriptsGenerator.Structures.MethodInfo;
using FieldInfo = ScriptsGenerator.Structures.FieldInfo;

namespace ScriptsGenerator.Core
{
    public class ScriptGenerator : BaseGenerator
    {
        private Dictionary<Type, string> BaseTypeDictionary { get; set; } = new Dictionary<Type, string>();
        private StringBuilder WriterBuilder { get; set; }

        public ScriptGenerator(GeneratorSettings settings) : base(settings)
        {
            WriterBuilder = new StringBuilder();
            PopulateBaseTypeDictionary();
        }

        public void WriteUsing(List<UsingInfo> namespaceCollection)
        {
            for (int i = 0; i < namespaceCollection.Count; i++)
            {
                WriterBuilder.AppendLine($"using {namespaceCollection[i].Name};");
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

        public void WriteProperty(List<PropertyInfo> propertiesCollection)
        {
            for (int i = 0; i < propertiesCollection.Count; i++)
            {
                VariableInfo variable = propertiesCollection[i].Variable;
                string accessModifierLabel = MakeLabelFromEnum(propertiesCollection[i].Modifier);

                WriterBuilder.Append($"{accessModifierLabel} {GetReturnTypeLabel(variable.Type)} {variable.Name} {{ get; set; }}");

                if (string.IsNullOrEmpty(variable.DefaultValue) == false)
                {
                    WriterBuilder.Append($" = {variable.DefaultValue};");
                }

                ExecuteWriter();
            }
        }

        public void WriteField(List<FieldInfo> fieldsCollection)
        {
            for (int i = 0; i < fieldsCollection.Count; i++)
            {
                VariableInfo variable = fieldsCollection[i].Variable;
                string accessModifierLabel = MakeLabelFromEnum(fieldsCollection[i].Modifier);

                WriterBuilder.Append($"{accessModifierLabel} {GetReturnTypeLabel(variable.Type)} {variable.Name}");

                if (string.IsNullOrEmpty(variable.DefaultValue) == false)
                {
                    WriterBuilder.Append($" = {variable.DefaultValue};");
                }
                else
                {
                    WriterBuilder.Append(';');
                }

                ExecuteWriter();
            }
        }

        public void BeginClass(AccessModifiers accessModifier, string className, string baseClassName = null, List<InterfaceInfo> implementedInterfaceNameCollection = null)
        {
            string accessModifierLabel = MakeLabelFromEnum(accessModifier);
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
            ExecuteWriter();
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

        private void PopulateBaseTypeDictionary()
        {
            Assembly msCSharpLib = Assembly.GetAssembly(typeof(int));

            using (CSharpCodeProvider csCodeProvider = new CSharpCodeProvider())
            {
                foreach (TypeInfo csType in msCSharpLib.DefinedTypes)
                {
                    if (string.Equals(csType.Namespace, "System"))
                    {
                        CodeTypeReference csTypeRef = new CodeTypeReference(csType);
                        string csTypeName = csCodeProvider.GetTypeOutput(csTypeRef);

                        if (csTypeName.IndexOf('.') == -1)
                        {
                            BaseTypeDictionary.Add(csType.AsType(), csTypeName);
                        }
                    }
                }
            }
        }

        private void WriteMethod(MethodInfo methodInfo)
        {
            WriterBuilder.Append($"{MakeLabelFromEnum(methodInfo.Modifier)} ");

            if (methodInfo.Keyword != PolymorphismKeyword.NONE)
            {
                WriterBuilder.Append($"{MakeLabelFromEnum(methodInfo.Keyword)} ");
            }

            WriterBuilder.Append($"{GetReturnTypeLabel(methodInfo.Type)} ");
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
            ExecuteWriter();
        }

        private void WriteMethodParameters(List<VariableInfo> parametersCollection)
        {
            WriterBuilder.Append('(');

            for (int i = 0; i < parametersCollection.Count; i++)
            {
                if (i != 0)
                {
                    WriterBuilder.Append(SPACE);
                }

                string parametersTypeLabel = GetReturnTypeLabel(parametersCollection[i].Type);
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

        private string MakeLabelFromEnum<T>(T toChange) where T : Enum
        {
            string label = toChange.ToString();
            label = label.ToLower();
            label = label.Replace(UNDERLINE, SPACE);

            return label;
        }

        private string GetReturnTypeLabel(Type returnType)
        {
            return BaseTypeDictionary.ContainsKey(returnType) == true ? BaseTypeDictionary[returnType] : returnType.Name.ToString();
        }
    }
}

