using System.Collections.Generic;

namespace Rassoodock.DifferenceEngine
{
    public interface IDifferentiator<T>
    {
        string GetDifferenceAlterString(IEnumerable<T> objectsFromFileSystem, IEnumerable<T> objectsInDb);
    }
}