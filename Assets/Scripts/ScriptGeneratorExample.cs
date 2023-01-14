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
            GenerateUsings();
            GenerateBeginClass();

            GenerateClassProperties();
            GenerateClassFields();

            GenerateMethod();
            GenerateAbstractMethod();

            GenerateNestedClass();

            Generator.WriteEmptyLine();
            Generator.EndClass();
            Generator.EndNamespace();
        }

        private void GenerateBeginClass()
        {
            NamespaceInfo namespaceInfo = new("Awesome.Inc");
            Generator.BeginNamespace(namespaceInfo);

            List<InterfaceInfo> interfaces = new() { new("IInterfaces"), new("IAwesomable") };
            Generator.BeginClass(AccessModifiers.PROTECTED_INTERNAL, "NewClass", "BaseGen", interfaces);
            Generator.WriteEmptyLine();
        }

        private void GenerateUsings()
        {
            List<UsingInfo> usings = new() { new("System"), new("System.Collections"), new("UnityEngine.UI") };
            Generator.WriteUsing(usings);
            Generator.WriteEmptyLine();
        }

        private void GenerateClassProperties()
        {
            List<PropertyInfo> properties = new() { new(AccessModifiers.PRIVATE, new(typeof(int), "a", "20")), new(AccessModifiers.PROTECTED, new(typeof(int), "b", "52")), new(AccessModifiers.PRIVATE, new(typeof(MethodInfo), "method")) };
            Generator.WriteProperty(properties);
            Generator.WriteEmptyLine();
        }

        private void GenerateClassFields()
        {
            List<FieldInfo> fields = new() { new(AccessModifiers.PRIVATE, new(typeof(int), "a")), new(AccessModifiers.PROTECTED, new(typeof(int), "b", "0")), new(AccessModifiers.PRIVATE, new(typeof(MethodInfo), "method")) };
            Generator.WriteField(fields);
        }

        private void GenerateMethod()
        {
            List<VariableInfo> parameters = new() { new(typeof(int), "number"), new(typeof(string), "label") };
            MethodInfo methodInfo = new(AccessModifiers.PRIVATE, typeof(void), "NewMethod", parameters);

            Generator.BeginMethod(methodInfo);
            Generator.WriteEmptyLine();
            Generator.EndMethod();
        }

        private void GenerateAbstractMethod()
        {
            List<VariableInfo> parameters = new() { new(typeof(int), "number") };
            MethodInfo methodInfo = new(AccessModifiers.PRIVATE, typeof(void), "NewVirtualMethod", PolymorphismKeyword.ABSTRACT, parameters);

            Generator.WriteEmptyLine();
            Generator.WriteAbstractMethod(methodInfo);
        }

        private void GenerateNestedClass()
        {
            Generator.BeginClass(AccessModifiers.PRIVATE, "NestedClass");
            Generator.WriteEmptyLine();
            Generator.WriteEmptyLine();
            Generator.EndClass();
        }

        private void ShowGenerationResult()
        {
            Debug.Log(Generator.CodeBuilder.ToString());
            OutputLabel.text = Generator.CodeBuilder.ToString();
        }
    }
}