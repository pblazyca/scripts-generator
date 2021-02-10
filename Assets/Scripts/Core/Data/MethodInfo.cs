using System.Collections.Generic;
using System;

namespace ScriptsGenerator.Structures
{
    public class MethodInfo : IName
    {
        public AccessModifiers Modifier { get; private set; }
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public List<VariableInfo> ParametersCollection { get; private set; }
        public PolymorphismKeyword Keyword { get; private set; }

        public MethodInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword)
        {
            SetupMethodInfo(modifier, type, name, keyword);
        }

        public MethodInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword, List<VariableInfo> parametersCollection)
        {
            SetupMethodInfo(modifier, type, name, keyword);
        }

        private void SetupMethodInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword)
        {
            Modifier = modifier;
            Type = type;
            Name = name;
            Keyword = keyword;
        }

        private void SetupMethodParameters(List<VariableInfo> parametersCollection)
        {
            ParametersCollection = parametersCollection;
        }
    }
}