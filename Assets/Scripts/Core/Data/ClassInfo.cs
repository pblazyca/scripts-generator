using System;

namespace ScriptsGenerator.Structures
{
    public class ClassInfo : PolymorphismElementInfo
    {
        public ClassInfo(AccessModifiers modifier, Type type, string name) : base(modifier, type, name) { }
        public ClassInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword) : base(modifier, type, name, keyword) { }
    }
}