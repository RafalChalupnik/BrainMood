using BrainMood.Harvester.Types;
using System;

namespace BrainMood.Harvester.Mindwave.Events
{
    public class ErrorEventArgs : EventArgs
    {
        public string Error { get; }

        public ErrorEventArgs(NotEmptyString error)
        {
            Error = error;
        }
    }
}
