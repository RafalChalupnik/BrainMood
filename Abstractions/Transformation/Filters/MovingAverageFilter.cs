using System.Collections.Generic;
using System.Linq;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.Data.Core;

namespace BrainMood.Abstractions.Transformation.Filters
{
    public class MovingAverageFilter : IFilter
    {
        private readonly int m_windowWidth;
        private readonly int m_step;

        public MovingAverageFilter(int windowWidth, int step)
        {
            m_windowWidth = windowWidth;
            m_step = step;
        }

        public List<CompleteData> Execute(IReadOnlyCollection<CompleteData> data)
        {
            var output = new List<CompleteData>();

            for (var i = 0; i <= data.Count - m_windowWidth; i += m_step)
            {
                var mean = CalculateMean(data.Skip(i).ToList());
                output.Add(mean);
            }

            return output;
        }

        private static CompleteData CalculateMean(IReadOnlyCollection<CompleteData> data)
        {
            var alphaHigh = data.Average(frame => frame.Eeg.AlphaHigh);
            var alphaLow = data.Average(frame => frame.Eeg.AlphaLow);
            var betaHigh = data.Average(frame => frame.Eeg.BetaHigh);
            var betaLow = data.Average(frame => frame.Eeg.BetaLow);
            var delta = data.Average(frame => frame.Eeg.Delta);
            var gammaHigh = data.Average(frame => frame.Eeg.GammaHigh);
            var gammaLow = data.Average(frame => frame.Eeg.GammaLow);
            var theta = data.Average(frame => frame.Eeg.Theta);

            var attention = data.Average(frame => frame.ESense.Attention);
            var meditation = data.Average(frame => frame.ESense.Meditation);

            var brainwaves = new EegData(
                alphaHigh,
                alphaLow,
                betaHigh,
                betaLow,
                delta,
                gammaHigh,
                gammaLow,
                theta);

            var eSense = new ESenseData(attention, meditation);

            var emotion = data.Select(frame => frame.Emotion).Distinct().Single();
            return new CompleteData(emotion, brainwaves, eSense);
        }
    }
}