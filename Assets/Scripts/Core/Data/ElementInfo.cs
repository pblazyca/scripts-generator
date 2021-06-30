using System;

namespace ScriptsGenerator.Structures
{
    public class ElementInfo : IName
    {
        public string Name { get; private set; }

        public ElementInfo(string name)
        {
            Name = name;
        }
    }
}
