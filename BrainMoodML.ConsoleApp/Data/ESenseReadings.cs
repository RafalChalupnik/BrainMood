namespace BrainMoodML.ConsoleApp.Data
{
    public class ESenseReadings
    {
        public double Attention { get; }

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
