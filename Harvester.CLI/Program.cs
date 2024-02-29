using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.MindwaveClient;
using BrainMood.Abstractions.MindwaveClient.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BrainMood.Abstractions.IO;

namespace BrainMood.Harvester.CLI
{
    public class Program
    {
        private static List<DataWithoutEmotion> s_data = new List<DataWithoutEmotion>();

        public static void Main(string[] _)
        {
            AsyncMain().Wait();

            Console.WriteLine("Done! Press any key to continue...");
            Console.ReadLine();
        }

        private static async Task AsyncMain()
        {
            var headsetClient = new HeadsetClient();
            headsetClient.DataReceived += DataReceived;
            headsetClient.ErrorOccured += ErrorOccured;

            using var cancellationTokenSource = new CancellationTokenSource();
            var task = headsetClient.Run(cancellationTokenSource.Token);
            Console.ReadLine();
            cancellationTokenSource.Cancel();
            await task;

            CsvFileWriter.WriteDataWithoutEmotions("output.csv", s_data);
        }

        private static void DataReceived(object? sender, DataReceivedEventArgs args)
        {
            s_data.Add(args.Data);

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
