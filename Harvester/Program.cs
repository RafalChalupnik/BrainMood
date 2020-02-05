using BrainMood.Harvester.Mindwave.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BrainMood.Harvester
{
    public class Program
    {
        public static void Main(string[] _)
        {
            AsyncMain().Wait();

            Console.WriteLine("Done! Press any key to continue...");
            Console.ReadLine();
        }

        private async static Task AsyncMain()
        {
            var headsetClient = new HeadsetClient();
            headsetClient.DataReceived += DataReceived;
            headsetClient.ErrorOccured += ErrorOccured;

            using var cancellationTokenSource = new CancellationTokenSource();
            var task = headsetClient.Run(cancellationTokenSource.Token);
            Console.ReadLine();
            cancellationTokenSource.Cancel();
            await task;
        }

        private static void DataReceived(object? sender, EegDataEventArgs args)
        {
            Console.WriteLine(args.Data);
            Console.WriteLine();
        }

        private static void ErrorOccured(object? sender, ErrorEventArgs args)
        {
            Console.WriteLine($"{args.Error}");
            Console.WriteLine();
        }
    }
}
