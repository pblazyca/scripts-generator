using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
public class BasicGenerator : Generator
{
    private Dictionary<Type, string> BaseTypeDictionary { get; set; } = new Dictionary<Type, string>();

    public BasicGenerator(GeneratorSettings settings) : base(settings)
    {
        InitBaseTypeDictionary();
    }

    public void WriteNamespaceBlock(List<string> namespaceCollection)
    {
        string namespaceLine = string.Empty;

        for (int i = 0; i < namespaceCollection.Count; i++)
        {
            namespaceLine = $"using {namespaceCollection[i]};";
            WriteTextLine(namespaceLine);
        }
    }

    public void BeginClass(AccessModifiers accessModifier, string className, string baseClassName = null, List<string> implementedInterfaceNameCollection = null)
    {
        string accessModifierLabel = GetAccessModifiersLabel(accessModifier);
        string beginClassLine = $"{accessModifierLabel} class {className}";

        if (baseClassName != null)
        {
            beginClassLine += $" : {baseClassName}";
        }

        if (implementedInterfaceNameCollection != null)
        {
            for (int i = 0; i < implementedInterfaceNameCollection.Count; i++)
            {
                beginClassLine += $", {implementedInterfaceNameCollection[i]}";
            }
        }

        WriteEmptyLine();
        WriteTextLine(beginClassLine);
        BeginBlock();
    }

    public void EndClass()
    {
        WriteEmptyLine();
        EndBlock();
    }

    public void BeginMethod(AccessModifiers accessModifier, Type methodReturnType, string methodName, List<Tuple<Type, string>> parametersCollection = null)
    {
        string accessModifierLabel = GetAccessModifiersLabel(accessModifier);
        string methodReturnTypeLabel = methodReturnType.Name;

        string beginMethodLine = $"{accessModifierLabel} {methodReturnTypeLabel} {methodName}";

        //if (implementedInterfaceNameCollection != null)
        //{
        //    for (int i = 0; i < implementedInterfaceNameCollection.Count; i++)
        //    {
        //        beginMethodLine += $", {implementedInterfaceNameCollection[i]}";
        //    }
        //}

        WriteEmptyLine();
        WriteTextLine(beginMethodLine);
        BeginBlock();
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

    private string GetAccessModifiersLabel(AccessModifiers accessModifier)
    {
        string label = accessModifier.ToString();
        label = label.ToLower();
        label = label.Replace(UNDERLINE, SPACE);

        return label;
    }
}
