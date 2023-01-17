using System.Text;
using ScriptsGenerator.Structures;

namespace ScriptsGenerator.Core
{
    public interface IWriter<T> where T : ElementInfo
    {
        void Write(T structureElement, ref StringBuilder writerBuilder);
    }
}