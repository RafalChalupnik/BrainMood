using BrainMood.Abstractions.Data.Core;

namespace BrainMood.Abstractions.Data
{
    public class DataWithoutESense
    {
        public Emotion Emotion { get; }

        public EegData Eeg { get; }

        public DataWithoutESense(Emotion emotion, EegData eeg)
        {
            Emotion = emotion;
            Eeg = eeg;
        }
    }
}