namespace BrainMoodML.ConsoleApp.Data
{
    public class BrainwaveReadings
    {
        public double AlphaHigh { get; }

        public double AlphaLow { get; }

        public double BetaHigh { get; }

        public double BetaLow { get; }

        public double Delta { get; }

        public double GammaHigh { get; }

        public double GammaLow { get; }

        public double Theta { get; }

        public BrainwaveReadings(
            double alphaHigh = 0.0,
            double alphaLow = 0.0,
            double betaHigh = 0.0,
            double betaLow = 0.0,
            double delta = 0.0,
            double gammaHigh = 0.0,
            double gammaLow = 0.0,
            double theta = 0.0)
        {
            AlphaHigh = alphaHigh;
            AlphaLow = alphaLow;
            BetaHigh = betaHigh;
            BetaLow = betaLow;
            Delta = delta;
            GammaHigh = gammaHigh;
            GammaLow = gammaLow;
            Theta = theta;
        }

        public override string ToString()
        {
            return $"AlphaHigh: {AlphaHigh}\n" +
                $"AlphaLow: {AlphaLow}\n" +
                $"BetaHigh: {BetaHigh}\n" +
                $"BetaLow: {BetaLow}\n" +
                $"Delta: {Delta}\n" +
                $"GammaHigh: {GammaHigh}\n" +
                $"Theta: {Theta}";
        }
    }
}
