using System.Collections.Generic;
using System;

namespace ScriptsGenerator.Structures
{
    public class MethodInfo : PolymorphismElementInfo
    {
        public List<VariableInfo> ParametersCollection { get; private set; }

        public MethodInfo(AccessModifiers modifier, Type type, string name) : base(modifier, type, name) { }
        public MethodInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword) : base(modifier, type, name, keyword) { }

        public MethodInfo(AccessModifiers modifier, Type type, string name, List<VariableInfo> parametersCollection) : base(modifier, type, name)
        {
            SetupMethodParameters(parametersCollection);
        }

        public MethodInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword, List<VariableInfo> parametersCollection) : base(modifier, type, name, keyword)
        {
            SetupMethodParameters(parametersCollection);
        }

        private void SetupMethodParameters(List<VariableInfo> parametersCollection)
        {
            ParametersCollection = parametersCollection;
        }
    }
}