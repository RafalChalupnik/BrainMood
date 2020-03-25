using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accord.Audio;
using BrainMood.Client;
using BrainMood.Client.Data;
using BrainMood.Client.Mindwave.Events;
using CsvHelper;

namespace BrainMood.CLI
{
    public class Program
    {
        private static List<string> s_data = new List<string>
        {
            "alphaHigh,alphaLow,betaHigh,betaLow,delta,gammaHigh,gammaLow,theta,attention,meditation"
        };

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

            File.WriteAllLines("output.csv", s_data);
        }

        private static void DataReceived(object? sender, EegDataEventArgs args)
        {
            s_data.Add(EegDataToCsvString(args.Data));

            //var inputModel = new ModelInput
            //{
            //    AlphaHigh = (float) args.Data.Brainwaves.AlphaHigh,
            //    AlphaLow = (float)args.Data.Brainwaves.AlphaLow,
            //    Attention = (float)args.Data.ESense.Attention,
            //    BetaHigh = (float)args.Data.Brainwaves.BetaHigh,
            //    BetaLow = (float)args.Data.Brainwaves.BetaLow,
            //    Delta = (float)args.Data.Brainwaves.Delta,
            //    GammaHigh = (float)args.Data.Brainwaves.GammaHigh,
            //    GammaLow = (float)args.Data.Brainwaves.GammaLow,
            //    Meditation = (float)args.Data.ESense.Meditation,
            //    Theta = (float)args.Data.Brainwaves.Theta
            //};

            //var outputModel = ConsumeModel.Predict(inputModel);
            //Console.WriteLine($"Prediction: {outputModel.Prediction}");
            //Console.WriteLine($"Score: {string.Join(',', outputModel.Score)}");

            Console.WriteLine(args.Data);
            Console.WriteLine();
        }

        private static void ErrorOccured(object? sender, Client.Mindwave.Events.ErrorEventArgs args)
        {
            Console.WriteLine($"{args.Error}");
            Console.WriteLine();
        }

        private static string EegDataToCsvString(EegData data)
        {
            return string.Join(',',
                data.Brainwaves.AlphaHigh,
                data.Brainwaves.AlphaLow,
                data.Brainwaves.BetaHigh,
                data.Brainwaves.BetaLow,
                data.Brainwaves.Delta,
                data.Brainwaves.GammaHigh,
                data.Brainwaves.GammaLow,
                data.Brainwaves.Theta,
                data.ESense.Attention,
                data.ESense.Meditation);
        }
    }

    class Program2
    {
        static void Main2(string[] args)
        {
            const string fileName = @"D:\Repos\BrainMood\Harvester\bin\Debug\netcoreapp3.1\output.csv";
            var data = ReadFromFile(fileName);

            CalculateDescriptors(data, reading => reading.AlphaHigh, "AlphaHigh");
            CalculateDescriptors(data, reading => reading.AlphaLow, "AlphaLow");
            CalculateDescriptors(data, reading => reading.BetaHigh, "BetaHigh");
            CalculateDescriptors(data, reading => reading.BetaLow, "BetaLow");
            CalculateDescriptors(data, reading => reading.Delta, "Delta");
            CalculateDescriptors(data, reading => reading.GammaHigh, "GammaHigh");
            CalculateDescriptors(data, reading => reading.GammaLow, "GammaLow");
            CalculateDescriptors(data, reading => reading.Theta, "Theta");

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static List<BrainwaveReadings> ReadFromFile(string fileName)
        {
            using var streamReader = File.OpenText(fileName);
            var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            return reader.GetRecords<BrainwaveReadings>().ToList();
        }

        private static void CalculateDescriptors(List<BrainwaveReadings> readings, Func<BrainwaveReadings, double> selector, string title)
        {
            var data = readings
                .Select(selector)
                .ToArray();

            var signal = Signal.FromArray(data, sampleRate: 1);

            var mfcc = new MelFrequencyCepstrumCoefficient();

            var descriptor = mfcc.Transform(signal).Single();

            Console.WriteLine(title);
            foreach (var value in descriptor.Descriptor)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine();
        }
    }
}
