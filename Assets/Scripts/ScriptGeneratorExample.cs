using System.Collections.Generic;
using UnityEngine;
using ScriptsGenerator.Core;
using ScriptsGenerator.Structures;

namespace ScriptsGenerator.Demo
{
    public class ScriptGeneratorExample : MonoBehaviour
    {
        [field: SerializeField]
        private GeneratorSettings Settings { get; set; }

        protected virtual void Start()
        {
            Core.ScriptGenerator generator = new Core.ScriptGenerator(Settings);

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

            List<UsingInfo> usings = new List<UsingInfo>() { new UsingInfo("System"), new UsingInfo("System.Collections"), new UsingInfo("UnityEngine.UI") };
            generator.WriteUsing(usings);

            NamespaceInfo namespaceInfo = new("Awesome.Inc");
            generator.BeginNamespace(namespaceInfo);

            List<InterfaceInfo> interfaces = new List<InterfaceInfo>() { new InterfaceInfo("IInterfaces"), new InterfaceInfo("IAwesomable") };
            generator.BeginClass(AccessModifiers.PROTECTED_INTERNAL, "NewClass", "BaseGen", interfaces);

            List<PropertyInfo> properties = new List<PropertyInfo>() { new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a", "20")), new PropertyInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "52")), new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            generator.WriteProperty(properties);

            List<FieldInfo> fields = new List<FieldInfo>() { new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a")), new FieldInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "0")), new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            generator.WriteField(fields);

            List<VariableInfo> parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number"), new VariableInfo(typeof(string), "label") };
            MethodInfo methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewMethod", parameters);

            generator.BeginMethod(methodInfo);
            generator.EndMethod();

            parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number") };
            methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewVirtualMethod", PolymorphismKeyword.ABSTRACT, parameters);

            generator.BeginMethod(methodInfo);

            generator.EndClass();
            generator.EndNamespace();

            Debug.Log(generator.CodeBuilder.ToString());
        }
    }
}