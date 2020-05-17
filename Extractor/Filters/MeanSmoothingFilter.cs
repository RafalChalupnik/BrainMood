using System.Collections.Generic;
using System.Linq;
using BrainMood.Harvester.Data;

namespace Extractor.Filters
{
    public class MeanSmoothingFilter : IFilter
    {
        private readonly int m_windowWidth;
        private readonly int m_step;

        public MeanSmoothingFilter(int windowWidth, int step)
        {
            m_windowWidth = windowWidth;
            m_step = step;
        }

        public List<Data> Execute(IReadOnlyCollection<Data> data)
        {
            var output = new List<Data>();

            for (var i = 0; i <= data.Count - m_windowWidth; i += m_step)
            {
                var mean = CalculateMean(data.Skip(i).ToList());
                output.Add(mean);
            }

            return output;
        }

        private static Data CalculateMean(IReadOnlyCollection<Data> data)
        {
            var alphaHigh = data.Average(frame => frame.EegData.Brainwaves.AlphaHigh);
            var alphaLow = data.Average(frame => frame.EegData.Brainwaves.AlphaLow);
            var betaHigh = data.Average(frame => frame.EegData.Brainwaves.BetaHigh);
            var betaLow = data.Average(frame => frame.EegData.Brainwaves.BetaLow);
            var delta = data.Average(frame => frame.EegData.Brainwaves.Delta);
            var gammaHigh = data.Average(frame => frame.EegData.Brainwaves.GammaHigh);
            var gammaLow = data.Average(frame => frame.EegData.Brainwaves.GammaLow);
            var theta = data.Average(frame => frame.EegData.Brainwaves.Theta);

            var attention = data.Average(frame => frame.EegData.ESense.Attention);
            var meditation = data.Average(frame => frame.EegData.ESense.Meditation);

            var brainwaves = new BrainwaveReadings(
                alphaHigh, 
                alphaLow, 
                betaHigh, 
                betaLow, 
                delta, 
                gammaHigh, 
                gammaLow,
                theta);

            var eSense = new ESenseReadings(attention, meditation);

            var emotion = data.Select(frame => frame.Emotion).Distinct().Single();
            var eegData = new EegData(brainwaves, eSense);

            return new Data(emotion, eegData);
        }
    }
}