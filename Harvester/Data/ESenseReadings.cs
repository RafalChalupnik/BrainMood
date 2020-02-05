using Newtonsoft.Json;

namespace BrainMood.Harvester.Data
{
    public class ESenseReadings
    {
        [JsonProperty("attention")]
        public double Attention { get; }

        [JsonProperty("meditation")]
        public double Meditation { get; }

        public ESenseReadings(double attention = 0.0, double meditation = 0.0)
        {
            Attention = attention;
            Meditation = meditation;
        }

        public override string ToString()
        {
            return $"Attention: {Attention}\n" +
                $"Meditation: {Meditation}";
        }
    }
}
