using System;

namespace ScriptsGenerator.Structures
{
    public class TypeElementInfo : ElementInfo
    {
        public Type Type { get; private set; }

        public TypeElementInfo(Type type, string name) : base(name)
        {
            Type = type;
        }
    }
}
