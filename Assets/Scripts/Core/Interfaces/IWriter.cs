using System.Text;
using ScriptsGenerator.Structures;

namespace ScriptsGenerator.Core
{
    public interface IWriter<T> where T : IStructureElement
    {
        void Write(ref StringBuilder writerBuilder);
    }
}