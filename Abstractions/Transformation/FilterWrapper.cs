using System.Collections.Generic;
using System.Linq;
using BrainMood.Abstractions.Data;

namespace BrainMood.Abstractions.Transformation
{
    public class FilterWrapper
    {
        private readonly List<IFilter> m_filters;

        public FilterWrapper(params IFilter[] filters)
        {
            m_filters = filters.ToList();
        }

        public List<CompleteData> Apply(List<CompleteData> data)
            => m_filters.Aggregate(data, (currentData, filter) => filter.Execute(currentData));
    }
}