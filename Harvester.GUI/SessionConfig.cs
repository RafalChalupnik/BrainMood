namespace BrainMood.Harvester.GUI
{
    public class SessionConfig
    {
        public string OutputDirectory { get; }

        public string DataDirectory { get; }

        public int SamplesPerEmotion { get; }

        public int TimePerSample { get; }

        public int BreakTime { get; }

        public SessionConfig(
            string outputDirectory, 
            string dataDirectory, 
            int samplesPerEmotion, 
            int timePerSample, 
            int breakTime)
        {
            OutputDirectory = outputDirectory;
            DataDirectory = dataDirectory;
            SamplesPerEmotion = samplesPerEmotion;
            TimePerSample = timePerSample;
            BreakTime = breakTime;
        }
    }
}