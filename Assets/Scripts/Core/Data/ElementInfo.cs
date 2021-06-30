using System;

public class ElementInfo : IName
{
    public Type Type { get; private set; }
    public string Name { get; private set; }

    public ElementInfo(Type type, string name)
    {
        Type = type;
        Name = name;
    }
}

