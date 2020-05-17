using System.Collections.Generic;
using System.Linq;

namespace Extractor.Filters
{
    public class AttentionTresholdFilter : IFilter
    {
        private readonly int m_treshold;

        public AttentionTresholdFilter(int treshold)
        {
            m_treshold = treshold;
        }

        public List<Data> Execute(IReadOnlyCollection<Data> data)
            => data.Where(frame => frame.EegData.ESense.Attention >= m_treshold).ToList();
    }
}