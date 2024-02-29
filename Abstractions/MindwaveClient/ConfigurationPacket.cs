using Newtonsoft.Json;

namespace BrainMood.Abstractions.MindwaveClient
{
    public class ConfigurationPacket
    {
        [JsonProperty("enableRawOutput")]
        public bool EnableRawOutput { get; }

        [JsonProperty("format")]
        public string Format { get; }

        public ConfigurationPacket(bool enableRawOutput, string format)
        {
            EnableRawOutput = enableRawOutput;
            Format = format;
        }
    }
}