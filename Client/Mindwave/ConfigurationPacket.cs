using BrainMood.Client.Types;
using Newtonsoft.Json;

namespace BrainMood.Client.Mindwave
{
    public class ConfigurationPacket
    {
        [JsonProperty("enableRawOutput")]
        public bool EnableRawOutput { get; }

        [JsonProperty("format")]
        public string Format { get; }

        public ConfigurationPacket(bool enableRawOutput, NotEmptyString format)
        {
            EnableRawOutput = enableRawOutput;
            Format = format;
        }
    }
}
