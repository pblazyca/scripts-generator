using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSettings", menuName = "ScriptsGenerator/Generator Settings")]
public class GeneratorSettings : ScriptableObject
{
    [field: Header("Indent")]
    [field: SerializeField]
    public IndentStyle IndentStyle { get; private set; }
    [field: SerializeField]
    public int IndentSize { get; private set; }
}