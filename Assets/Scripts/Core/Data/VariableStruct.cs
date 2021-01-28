using System;

public struct VariableStruct
{
    public Type Type { get; private set; }
    public string Name { get; private set; }

    public VariableStruct(Type type, string name)
    {
        Type = type;
        Name = name;
    }
}