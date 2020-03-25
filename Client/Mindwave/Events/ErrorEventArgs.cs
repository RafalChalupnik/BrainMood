using BrainMood.Client.Types;
using System;

namespace BrainMood.Client.Mindwave.Events
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
