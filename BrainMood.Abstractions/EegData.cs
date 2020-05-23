using Newtonsoft.Json;

namespace BrainMood.Abstractions
{
    public class EegData
    {
        [JsonProperty("highAlpha")]
        public double AlphaHigh { get; }

        [JsonProperty("lowAlpha")]
        public double AlphaLow { get; }

        [JsonProperty("highBeta")]
        public double BetaHigh { get; }

        [JsonProperty("lowBeta")]
        public double BetaLow { get; }

        [JsonProperty("delta")]
        public double Delta { get; }

        [JsonProperty("highGamma")]
        public double GammaHigh { get; }

        [JsonProperty("lowGamma")]
        public double GammaLow { get; }

        [JsonProperty("theta")]
        public double Theta { get; }

        [JsonProperty("attention")]
        public double Attention { get; }

        [JsonProperty("meditation")]
        public double Meditation { get; }

        public EegData(
            double alphaHigh = 0.0, 
            double alphaLow = 0.0, 
            double betaHigh = 0.0, 
            double betaLow = 0.0, 
            double delta = 0.0, 
            double gammaHigh = 0.0, 
            double gammaLow = 0.0, 
            double theta = 0.0, 
            double attention = 0.0, 
            double meditation = 0.0)
        {
            AlphaHigh = alphaHigh;
            AlphaLow = alphaLow;
            BetaHigh = betaHigh;
            BetaLow = betaLow;
            Delta = delta;
            GammaHigh = gammaHigh;
            GammaLow = gammaLow;
            Theta = theta;
            Attention = attention;
            Meditation = meditation;
        }
    }
}