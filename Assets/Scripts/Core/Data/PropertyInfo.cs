namespace ScriptsGenerator.Structures
{
    public class PropertyInfo : InLineElementInfo
    {
        public PropertyMethodInfo GetInfo { get; private set; }
        public PropertyMethodInfo SetInfo { get; private set; }

        public PropertyInfo(AccessModifiers modifier, VariableInfo variable) : base(modifier, variable) { }

        public class PropertyMethodInfo
        {
            public AccessModifiers Modifier { get; private set; }
            public string CustomBody { get; private set; }
        }
    }
}
