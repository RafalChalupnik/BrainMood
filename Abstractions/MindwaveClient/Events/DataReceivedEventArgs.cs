using BrainMood.Abstractions.Data;
using System;

namespace BrainMood.Abstractions.MindwaveClient.Events
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataWithoutEmotion Data { get; }

        public DataReceivedEventArgs(DataWithoutEmotion data)
        {
            Data = data;
        }
    }
}