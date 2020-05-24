using BrainMood.Client.Data;

namespace Extractor
{
    public class Data
    {
        public int Emotion { get; }

        public EegData EegData { get; }

        public Data(int emotion, EegData eegData)
        {
            Emotion = emotion;
            EegData = eegData;
        }
    }
}