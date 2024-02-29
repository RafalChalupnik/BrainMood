using BrainMood.Abstractions.Data.Core;
using BrainMood.Abstractions.Extensions;

namespace BrainMood.Abstractions.Data
{
    public class CompleteData
    {
        public Emotion Emotion { get; }

        public EegData Eeg { get; }

        public ESenseData ESense { get; }

        public CompleteData(Emotion emotion, EegData eeg, ESenseData eSense)
        {
            emotion.AssertIsDefined();

            Emotion = emotion;
            Eeg = eeg;
            ESense = eSense;
        }
    }
}