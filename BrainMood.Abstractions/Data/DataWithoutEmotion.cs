using BrainMood.Abstractions.Data.Core;

namespace BrainMood.Abstractions.Data
{
    public class DataWithoutEmotion
    {
        public EegData Eeg { get; }

        public ESenseData ESense { get; }

        public DataWithoutEmotion(EegData eeg, ESenseData eSense)
        {
            Eeg = eeg;
            ESense = eSense;
        }
    }
}