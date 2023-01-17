using System.Text;
using CodeHappiness.Core;
using ScriptsGenerator.Core;

namespace ScriptsGenerator.Structures
{
    public class FieldInfo : InLineElementInfo, IWriter<FieldInfo>
    {
        public FieldInfo(AccessModifiers modifier, VariableInfo variable) : base(modifier, variable) { }

        public void Write(StringBuilder writerBuilder)
        {
            string accessModifierLabel = Converters.ConvertEnumToLabel(Modifier);

            writerBuilder.Append($"{accessModifierLabel} {GeneratorTools.GetTypeLabel(Variable.Type)} {Variable.Name}");

            if (string.IsNullOrEmpty(Variable.DefaultValue) == false)
            {
                writerBuilder.Append($" = {Variable.DefaultValue};");
            }
            else
            {
                writerBuilder.Append(';');
            }
        }
    }
}
