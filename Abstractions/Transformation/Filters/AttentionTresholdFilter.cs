using System.Collections.Generic;
using System.Linq;
using BrainMood.Abstractions.Data;

namespace BrainMood.Abstractions.Transformation.Filters
{
    public class AttentionTresholdFilter : IFilter
    {
        private readonly int m_treshold;

        public AttentionTresholdFilter(int treshold)
        {
            m_treshold = treshold;
        }

        public List<CompleteData> Execute(IReadOnlyCollection<CompleteData> data)
            => data.Where(frame => frame.ESense.Attention >= m_treshold).ToList();
    }
}