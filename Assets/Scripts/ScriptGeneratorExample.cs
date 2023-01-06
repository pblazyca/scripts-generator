using System.Collections.Generic;
using UnityEngine;
using ScriptsGenerator.Structures;
using UnityEngine.UIElements;

namespace ScriptsGenerator.Demo
{
    public class ScriptGeneratorExample : MonoBehaviour
    {
        [field: SerializeField]
        private GeneratorSettings Settings { get; set; }
        [field: SerializeField]
        private UIDocument MainUIDocument { get; set; }

        protected virtual void Start()
        {
            Label output = MainUIDocument.rootVisualElement.Q<Label>("OutputLabel");
            Core.ScriptGenerator generator = new Core.ScriptGenerator(Settings);

            List<UsingInfo> usings = new List<UsingInfo>() { new UsingInfo("System"), new UsingInfo("System.Collections"), new UsingInfo("UnityEngine.UI") };
            generator.WriteUsing(usings);
            generator.WriteEmptyLine();

            NamespaceInfo namespaceInfo = new("Awesome.Inc");
            generator.BeginNamespace(namespaceInfo);

            List<InterfaceInfo> interfaces = new List<InterfaceInfo>() { new InterfaceInfo("IInterfaces"), new InterfaceInfo("IAwesomable") };
            generator.BeginClass(AccessModifiers.PROTECTED_INTERNAL, "NewClass", "BaseGen", interfaces);
            generator.WriteEmptyLine();

            List<PropertyInfo> properties = new List<PropertyInfo>() { new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a", "20")), new PropertyInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "52")), new PropertyInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            generator.WriteProperty(properties);
            generator.WriteEmptyLine();

            List<FieldInfo> fields = new List<FieldInfo>() { new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(int), "a")), new FieldInfo(AccessModifiers.PROTECTED, new VariableInfo(typeof(int), "b", "0")), new FieldInfo(AccessModifiers.PRIVATE, new VariableInfo(typeof(MethodInfo), "method")) };
            generator.WriteField(fields);

            List<VariableInfo> parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number"), new VariableInfo(typeof(string), "label") };
            MethodInfo methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewMethod", parameters);

            generator.BeginMethod(methodInfo);
            generator.WriteEmptyLine();
            generator.EndMethod();

            parameters = new List<VariableInfo>() { new VariableInfo(typeof(int), "number") };
            methodInfo = new MethodInfo(AccessModifiers.PRIVATE, typeof(void), "NewVirtualMethod", PolymorphismKeyword.ABSTRACT, parameters);

            generator.WriteEmptyLine();
            generator.WriteAbstractMethod(methodInfo);

            generator.EndClass();
            generator.EndNamespace();

            Debug.Log(generator.CodeBuilder.ToString());
            output.text = generator.CodeBuilder.ToString();
        }
    }
}