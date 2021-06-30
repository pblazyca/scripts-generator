using System;

namespace ScriptsGenerator.Structures
{
    public class VariableInfo : ElementInfo
    {
        public string DefaultValue { get; private set; }

        public VariableInfo(Type type, string name) : base(type, name) { }

        public VariableInfo(Type type, string name, string defaultValue) : base(type, name)
        {
            DefaultValue = defaultValue;
        }
    }
}
