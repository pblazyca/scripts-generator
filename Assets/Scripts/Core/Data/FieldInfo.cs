using System;

namespace ScriptsGenerator.Structures
{
    public class FieldInfo
    {
        public AccessModifiers Modifier { get; private set; }
        public VariableInfo Variable { get; private set; }

        public FieldInfo(AccessModifiers modifier, VariableInfo variable)
        {
            Modifier = modifier;
            Variable = variable;
        }
    }
}
