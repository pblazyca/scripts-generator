using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CSharp;

public static class GeneratorTools
{
    public static Dictionary<Type, string> PopulateBaseTypeDictionary()
    {
        Assembly msCSharpLib = Assembly.GetAssembly(typeof(int));
        Dictionary<Type, string> baseTypeDictionary = new();

        using (CSharpCodeProvider csCodeProvider = new())
        {
            foreach (TypeInfo csType in msCSharpLib.DefinedTypes)
            {
                if (string.Equals(csType.Namespace, "System"))
                {
                    CodeTypeReference csTypeRef = new(csType);
                    string csTypeName = csCodeProvider.GetTypeOutput(csTypeRef);

                    if (csTypeName.IndexOf('.') == -1)
                    {
                        baseTypeDictionary.Add(csType.AsType(), csTypeName);
                    }
                }
            }
        }

        return baseTypeDictionary;
    }
}
