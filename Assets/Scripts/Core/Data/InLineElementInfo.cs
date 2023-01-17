namespace ScriptsGenerator.Structures
{
    public class InLineElementInfo : IStructureElement
    {
        public AccessModifiers Modifier { get; private set; }
        public VariableInfo Variable { get; private set; }

        public InLineElementInfo(AccessModifiers modifier, VariableInfo variable)
        {
            Modifier = modifier;
            Variable = variable;
        }
    }
}