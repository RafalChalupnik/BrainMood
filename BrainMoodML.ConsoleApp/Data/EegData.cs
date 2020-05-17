namespace BrainMoodML.ConsoleApp.Data
{
    public class EegData
    {
        public BrainwaveReadings Brainwaves { get; }

        public ESenseReadings ESense { get; }

        public EegData(
            BrainwaveReadings brainwaves = null, 
            ESenseReadings eSense = null)
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
