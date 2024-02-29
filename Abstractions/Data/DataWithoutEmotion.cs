using BrainMood.Abstractions.Data.Core;
using Newtonsoft.Json;

namespace BrainMood.Abstractions.Data
{
    public class DataWithoutEmotion
    {
        [JsonProperty("eegPower")]
        public EegData Eeg { get; }

        [JsonProperty("eSense")]
        public ESenseData ESense { get; }

        public DataWithoutEmotion(EegData eeg, ESenseData eSense)
        {
            Eeg = eeg;
            ESense = eSense;
        }
    }
}