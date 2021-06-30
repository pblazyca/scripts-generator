using System.Text;
using ScriptsGenerator.Structures;

namespace ScriptsGenerator.Core
{
    public interface IWriter<T> where T : IStructureElement
    {
        StringBuilder WriterBuilder { get; }

        string Write(T structureElementInfo);
    }
}