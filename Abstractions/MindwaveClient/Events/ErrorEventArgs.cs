using System;

namespace BrainMood.Abstractions.MindwaveClient.Events
{
    public class ErrorEventArgs : EventArgs
    {
        public string Error { get; }

        public ErrorEventArgs(string error)
        {
            Error = error;
        }
    }
}