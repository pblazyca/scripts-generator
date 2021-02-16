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
    public class BasicGenerator : Generator
    {
        private Dictionary<Type, string> BaseTypeDictionary { get; set; } = new Dictionary<Type, string>();
        private StringBuilder WriterBuilder { get; set; }

        public BasicGenerator(GeneratorSettings settings) : base(settings)
        {
            WriterBuilder = new StringBuilder();
            InitBaseTypeDictionary();
        }

        public void WriteNamespaceBlock(List<string> namespaceCollection)
        {
            for (int i = 0; i < namespaceCollection.Count; i++)
            {
                WriterBuilder.AppendLine($"using {namespaceCollection[i]};");
            }

            WrtieText(WriterBuilder.ToString());
            WriterBuilder.Clear();
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

                WriteTextLine(WriterBuilder.ToString());
                WriterBuilder.Clear();
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

                WriteTextLine(WriterBuilder.ToString());
                WriterBuilder.Clear();
            }
        }

        public void BeginClass(AccessModifiers accessModifier, string className, string baseClassName = null, List<string> implementedInterfaceNameCollection = null)
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
                    WriterBuilder.Append($", {implementedInterfaceNameCollection[i]}");
                }
            }

            WriteEmptyLine();
            WriteTextLine(WriterBuilder.ToString());
            WriterBuilder.Clear();
            BeginBlock();
        }

        public void EndClass()
        {
            WriteEmptyLine();
            EndBlock();
        }

        public void BeginMethod(ScriptsGenerator.Structures.MethodInfo methodInfo)
        {
            WriterBuilder.Append($"{MakeLabelFromEnum(methodInfo.Modifier)} ");

            if (methodInfo.Keyword != PolymorphismKeyword.NONE)
            {
                WriterBuilder.Append($"{MakeLabelFromEnum(methodInfo.Keyword)} ");
            }

            WriterBuilder.Append($"{GetReturnTypeLabel(methodInfo.Type)} ");
            WriterBuilder.Append(methodInfo.Name);

            WriteMethod(methodInfo);

            if (methodInfo.Keyword != PolymorphismKeyword.ABSTRACT)
            {
                BeginBlock();
            }
        }

        public void EndMethod()
        {
            WriteEmptyLine();
            EndBlock();
        }

        private void InitBaseTypeDictionary()
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
            WriteTextLine(WriterBuilder.ToString());
            WriterBuilder.Clear();
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

        private string MakeLabelFromEnum<T>(T toChange) where T : Enum
        {
            string label = toChange.ToString();
            label = label.ToLower();
            label = label.Replace(UNDERLINE, SPACE);

            return label;
        }

        private string GetReturnTypeLabel(Type returnType)
        {
            if (BaseTypeDictionary.ContainsKey(returnType) == true)
            {
                return BaseTypeDictionary[returnType];
            }
            else
            {
                return returnType.Name.ToString();
            }
        }
    }
}

