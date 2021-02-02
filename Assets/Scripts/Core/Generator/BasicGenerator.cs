using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

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

    public void BeginClass(AccessModifiers accessModifier, string className, string baseClassName = null, List<string> implementedInterfaceNameCollection = null)
    {
        string accessModifierLabel = GetAccessModifiersLabel(accessModifier);
        WriterBuilder.Append($"{accessModifierLabel} class {className}");

        if (baseClassName != null)
        {
            WriterBuilder.Append($" : {baseClassName}";);
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

    public void BeginMethod(AccessModifiers accessModifier, Type methodReturnType, string methodName, List<VariableStruct> parametersCollection = null)
    {
        string accessModifierLabel = GetAccessModifiersLabel(accessModifier);
        string methodReturnTypeLabel = GetReturnTypeLabel(methodReturnType);

        WriterBuilder.Append($"{accessModifierLabel} {methodReturnTypeLabel} {methodName}");

        if (parametersCollection != null)
        {
            WriteMethodParameters(parametersCollection);
        }
        else
        {
            WriterBuilder.Append("()");
        }

        WriteEmptyLine();
        WriteTextLine(WriterBuilder.ToString());
        WriterBuilder.Clear();
        BeginBlock();
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

    private void WriteMethodParameters(List<VariableStruct> parametersCollection)
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

    private string GetAccessModifiersLabel(AccessModifiers accessModifier)
    {
        string label = accessModifier.ToString();
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
