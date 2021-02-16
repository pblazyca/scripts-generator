using System;

namespace ScriptsGenerator.Structures
{
    public class PropertyInfo
    {
        public AccessModifiers Modifier { get; private set; }
        public VariableInfo Variable { get; private set; }
        public PropertyMethodInfo GetInfo { get; private set; }
        public PropertyMethodInfo SetInfo { get; private set; }

        public PropertyInfo(AccessModifiers modifier, VariableInfo variable)
        {
            Modifier = modifier;
            Variable = variable;
        }

        public class PropertyMethodInfo
        {
            public AccessModifiers SetModifier { get; private set; }
            public string CustomSet { get; private set; }
        }
    }
}
