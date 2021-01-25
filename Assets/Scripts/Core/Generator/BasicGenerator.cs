using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BasicGenerator : Generator
{
    public BasicGenerator(GeneratorSettings settings) : base(settings) { }

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


    private string GetAccessModifiersLabel(AccessModifiers accessModifier)
    {
        string label = accessModifier.ToString();
        label = label.ToLower();
        label = label.Replace(UNDERLINE, SPACE);

        return label;
    }
}
