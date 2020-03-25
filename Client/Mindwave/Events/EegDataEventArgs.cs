using BrainMood.Client.Data;
using System;

namespace BrainMood.Client.Mindwave.Events
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
