using System;

namespace ScriptsGenerator.Structures
{
    public class VariableInfo : IName
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public string DefaultValue { get; private set; }

        public VariableInfo(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public VariableInfo(Type type, string name, string defaultValue)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
        }
    }
}
