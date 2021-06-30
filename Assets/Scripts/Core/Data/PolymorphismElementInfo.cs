using System;

namespace ScriptsGenerator.Structures
{
    public class PolymorphismElementInfo : ElementInfo
    {
        public AccessModifiers Modifier { get; private set; }
        public PolymorphismKeyword Keyword { get; private set; }

        public PolymorphismElementInfo(AccessModifiers modifier, Type type, string name) : base(type, name)
        {
            SetupElementInfo(modifier, PolymorphismKeyword.NONE);
        }

        public PolymorphismElementInfo(AccessModifiers modifier, Type type, string name, PolymorphismKeyword keyword) : base(type, name)
        {
            SetupElementInfo(modifier, keyword);
        }

        private void SetupElementInfo(AccessModifiers modifier, PolymorphismKeyword keyword)
        {
            Modifier = modifier;
            Keyword = keyword;
        }
    }
}