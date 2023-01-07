using System.Collections.Generic;
using UnityEngine;
using ScriptsGenerator.Structures;
using UnityEngine.UIElements;
using ScriptsGenerator.Core;

namespace ScriptsGenerator.Demo
{
    public class ScriptGeneratorExample : MonoBehaviour
    {
        [field: SerializeField]
        private GeneratorSettings Settings { get; set; }
        [field: SerializeField]
        private UIDocument MainUIDocument { get; set; }

        private ScriptGenerator Generator { get; set; }
        private Label OutputLabel { get; set; }

        protected virtual void Awake()
        {
            Generator = new(Settings);
            OutputLabel = MainUIDocument.rootVisualElement.Q<Label>("OutputLabel");
        }

        protected virtual void Start()
        {
            GenerateSampleClass();
            ShowGenerationResult();

        }

        private void GenerateSampleClass()
        {
            List<UsingInfo> usings = new List<UsingInfo>() { new UsingInfo("System"), new UsingInfo("System.Collections"), new UsingInfo("UnityEngine.UI") };
            Generator.WriteUsing(usings);
            Generator.WriteEmptyLine();

            NamespaceInfo namespaceInfo = new("Awesome.Inc");
            Generator.BeginNamespace(namespaceInfo);

            List<InterfaceInfo> interfaces = new List<InterfaceInfo>() { new InterfaceInfo("IInterfaces"), new InterfaceInfo("IAwesomable") };
            Generator.BeginClass(AccessModifiers.PROTECTED_INTERNAL, "NewClass", "BaseGen", interfaces);
            Generator.WriteEmptyLine();

            List<PropertyInfo> properties = new List<PropertyInfo>() { new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a", "20")), new PropertyInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "52")), new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            Generator.WriteProperty(properties);
            Generator.WriteEmptyLine();

            List<FieldInfo> fields = new List<FieldInfo>() { new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a")), new FieldInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "0")), new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            Generator.WriteField(fields);

            List<VariableInfo> parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number"), new VariableInfo(typeof(string), "label") };
            MethodInfo methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewMethod", parameters);

            Generator.BeginMethod(methodInfo);
            Generator.WriteEmptyLine();
            Generator.EndMethod();

            parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number") };
            methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewVirtualMethod", PolymorphismKeyword.ABSTRACT, parameters);

            Generator.WriteEmptyLine();
            Generator.WriteAbstractMethod(methodInfo);

            Generator.EndClass();
            Generator.EndNamespace();
        }

        private void ShowGenerationResult()
        {
            Debug.Log(Generator.CodeBuilder.ToString());
            OutputLabel.text = Generator.CodeBuilder.ToString();
        }
    }
}