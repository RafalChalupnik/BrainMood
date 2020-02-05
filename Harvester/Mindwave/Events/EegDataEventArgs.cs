using BrainMood.Harvester.Data;
using System;

namespace BrainMood.Harvester.Mindwave.Events
{
    public class EegDataEventArgs : EventArgs
    {
        public EegData Data { get; }

        public EegDataEventArgs(EegData data)
        {
            Data = data;
        }
    }
}
