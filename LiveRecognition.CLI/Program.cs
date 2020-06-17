using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.Data.Core;
using BrainMood.Abstractions.Extensions;
using BrainMood.Abstractions.IO;
using BrainMood.Abstractions.MindwaveClient;
using BrainMood.Abstractions.MindwaveClient.Events;
using BrainMood.Abstractions.Transformation;
using BrainMood.Abstractions.Transformation.Filters;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace BrainMood.LiveRecognition.CLI
{
    public class ModelInput
    {
        public uint Label { get; set; }

        [VectorType(8)]
        public float[] Features { get; set; } = new float[0];
    }

    public class ModelOutput
    {
        // ColumnName attribute is used to change the column name from
        // its default value, which is the name of the field.
        [ColumnName("PredictedLabel")]
        public uint Label { get; set; }

        public float[] Score { get; set; } = new float[0];
    }

    public static class Program
    {
        private const int c_dataCount = 10;

        private static readonly MLContext s_mlContext = new MLContext();
        private static readonly PredictionEngine<ModelInput, ModelOutput> s_model
            = LoadModel();
        private static readonly List<DataWithoutEmotion> s_data 
            = new List<DataWithoutEmotion>();

        private static readonly FilterWrapper s_filters = new FilterWrapper(
            new MovingAverageFilter(windowWidth: c_dataCount, step: 1));

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

            if (s_data.Count < c_dataCount)
            {
                Console.WriteLine($"Gathering data ({s_data.Count}/{c_dataCount})");
            }
            else
            {
                var filteredData = s_filters.Apply(s_data
                    .TakeLast(10)
                    .Select(ToCompleteData)
                    .ToList());
                Predict(filteredData.Last().Eeg);
                Console.WriteLine();
            }
        }

        private static void ErrorOccured(object? sender, ErrorEventArgs args)
        {
            Console.WriteLine($"{args.Error}");
            Console.WriteLine();
        }

        private static PredictionEngine<ModelInput, ModelOutput> LoadModel()
        {
            var model = s_mlContext.Model.Load("model.zip", out var schema);
            return s_mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        }

        private static CompleteData ToCompleteData(DataWithoutEmotion dataWithoutEmotion)
            => new CompleteData(
                Emotion.Undefined, 
                dataWithoutEmotion.Eeg, 
                dataWithoutEmotion.ESense);

        private static void Predict(EegData eegData)
        {
            var modelInput = new ModelInput
            {
                Features = new[]
                {
                    (float) eegData.AlphaHigh,
                    (float) eegData.AlphaLow,
                    (float) eegData.BetaHigh,
                    (float) eegData.BetaLow,
                    (float) eegData.Delta,
                    (float) eegData.GammaHigh,
                    (float) eegData.GammaLow,
                    (float) eegData.Theta,
                }
            };

            var result = s_model.Predict(modelInput);
            var prediction = (Emotion)result.Score.IndexOfMax();

            Console.WriteLine($"Prediction: {prediction}");
            Console.WriteLine($"Scores: {string.Join(';', result.Score)}");
        }
    }
}
