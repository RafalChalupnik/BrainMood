using BrainMood.Harvester.Data;
using BrainMood.Harvester.Mindwave.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BrainMood.Harvester
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

        private static void ErrorOccured(object? sender, Mindwave.Events.ErrorEventArgs args)
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
}
