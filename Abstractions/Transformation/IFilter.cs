using System.Collections.Generic;
using BrainMood.Abstractions.Data;

namespace BrainMood.Abstractions.Transformation
{
    public interface IFilter
    {
        List<CompleteData> Execute(IReadOnlyCollection<CompleteData> data);
    }
}