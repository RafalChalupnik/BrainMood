using Newtonsoft.Json;

namespace BrainMood.Harvester.Data
{
    public class EegData
    {
        [JsonProperty("eegPower")]
        public BrainwaveReadings Brainwaves { get; }

        [JsonProperty("eSense")]
        public ESenseReadings ESense { get; }

        public EegData(
            BrainwaveReadings? brainwaves = null, 
            ESenseReadings? eSense = null)
        {
            Brainwaves = brainwaves ?? new BrainwaveReadings();
            ESense = eSense ?? new ESenseReadings();
        }

        public override string ToString()
        {
            return "eSense:\n" +
                $"{ESense}\n" +
                "eegData:\n" +
                $"{Brainwaves}";
        }
    }
}
