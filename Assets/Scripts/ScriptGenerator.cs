using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGenerator : MonoBehaviour
{
    [field: SerializeField]
    private GeneratorSettings Settings { get; set; }

    protected virtual void Start()
    {
        Generator generator = new Generator(Settings);

        generator.WriteTextLine("nowy kawałek tekstu");
        generator.BeginBlock();
        generator.WriteTextLine("kawałek tekstu w bloku");
        generator.WriteEmptyLine();
        generator.EndBlock();
        generator.WriteEmptyLine();
        generator.WriteTextLine("tekst");
        generator.WrtieText("pierwszy tekst");
        generator.WrtieText(" drugi doklejony tekst");
        generator.WriteEmptyLine();
        generator.BeginBlock();
        generator.EndBlock();

        Debug.Log(generator.CodeBuilder.ToString());
    }
}
