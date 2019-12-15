using System;
using System.Threading;

namespace BrainMood.Harvester
{
    class Program
    {
        async static void Main(string[] args)
        {
            var client = new MindWaveClient(new System.Net.Sockets.TcpClient());
            client.DataReceived += DataReceived;

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = client.Run(cancellationTokenSource.Token);

                cancellationTokenSource.Cancel();

                await task;
            }
        }

        static void DataReceived(object? sender, MindWaveEventArgs args)
        {

        }
    }
}
