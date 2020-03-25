namespace BrainMood.Harvester.GUI
{
    public class AssessmentResult
    {
        public int Happiness { get; }

        public int Excitement { get; }

        public int Control { get; }

        public AssessmentResult(int happiness, int excitement, int control)
        {
            Happiness = happiness;
            Excitement = excitement;
            Control = control;
        }
    }
}