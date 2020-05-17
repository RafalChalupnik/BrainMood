using System.Collections.Generic;

namespace Extractor
{
    public interface IFilter
    {
        List<Data> Execute(IReadOnlyCollection<Data> data);
    }
}