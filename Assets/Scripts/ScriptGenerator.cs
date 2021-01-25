using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGenerator : MonoBehaviour
{
    [field: SerializeField]
    private GeneratorSettings Settings { get; set; }

    protected virtual void Start()
    {
        BasicGenerator generator = new BasicGenerator(Settings);

        //generator.WriteTextLine("nowy kawałek tekstu");
        //generator.BeginBlock();
        //generator.WriteTextLine("kawałek tekstu w bloku");
        //generator.WriteEmptyLine();
        //generator.EndBlock();
        //generator.WriteEmptyLine();
        //generator.WriteTextLine("tekst");
        //generator.WrtieText("pierwszy tekst");
        //generator.WrtieText(" drugi doklejony tekst");
        //generator.WriteEmptyLine();
        //generator.BeginBlock();
        //generator.EndBlock();

        List<string> namespaces = new List<string>() { "System", "System.Collections", "UnityEngine.UI" };
        generator.WriteNamespaceBlock(namespaces);

        List<string> interfaces = new List<string>() { "IInterfaces", "IAwesomable" };
        generator.BeginClass(AccessModifiers.PROTECTED_INTERNAL, "NewClass", "BaseGen", interfaces);
        generator.EndClass();

        Debug.Log(generator.CodeBuilder.ToString());
    }
}
