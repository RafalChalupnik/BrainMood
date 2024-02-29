using BrainMood.Abstractions.Extensions;
using BrainMood.Abstractions.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrainMood.ManualTrainer
{
    public class Program
    {
        public static void Main(string[] _)
        {
            CrossValidation();
        }

        private static void CrossValidation()
        {
            var mlContext = new MLContext();
            var random = new Random();

            var data = Shuffle(LoadData(), random);
            var batches = Shuffle(Split(data, 10), random);

            var output = batches
                .Select(batch =>
                {
                    var testData = batch;
                    var trainingData = batches
                        .Except(testData)
                        .SelectMany(x => x);

                    return RunLearning(mlContext, trainingData, testData);
                })
                .ToList();

            foreach (var batchOutput in output)
            {
                Console.WriteLine("Confusion matrix:");
                Console.WriteLine(batchOutput.ConfusionMatrix.GetFormattedConfusionTable());
                Console.WriteLine($"MicroAccuracy: {batchOutput.MicroAccuracy}");
                Console.WriteLine($"MacroAccuracy: {batchOutput.MacroAccuracy}");
                Console.WriteLine();
            }

            Console.WriteLine("Done!");
        }

        private static MulticlassClassificationMetrics RunLearning(
            MLContext context, 
            IEnumerable<DataPoint> trainingData, 
            IEnumerable<DataPoint> testData)
        {
            var trainingDataView = context.Data.LoadFromEnumerable(trainingData);

            var options = new FastTreeBinaryTrainer.Options
            {
                EarlyStoppingMetric = EarlyStoppingMetric.L2Norm,
                FeatureFirstUsePenalty = 0.1,
                NumberOfTrees = 50
            };

            var pipeline = context.Transforms.Conversion.MapValueToKey("Label")
                .Append(context.MulticlassClassification.Trainers.OneVersusAll(
                    context.BinaryClassification.Trainers.FastTree(options)));

            var model = pipeline.Fit(trainingDataView);

            var testDataView = context.Data
                .LoadFromEnumerable(testData);

            var transformedTestData = model.Transform(testDataView);

            return context.MulticlassClassification
                .Evaluate(transformedTestData);
        }

        private static IEnumerable<DataPoint> LoadData()
        {
            var data = CsvFileReader.ReadDataWithoutESense("../../../../_Data/Data.csv");

            foreach (var record in data)
            {
                var features = new[]
                {
                    (float) record.Eeg.AlphaHigh,
                    (float) record.Eeg.AlphaLow,
                    (float) record.Eeg.BetaHigh,
                    (float) record.Eeg.BetaLow,
                    (float) record.Eeg.GammaHigh,
                    (float) record.Eeg.GammaLow,
                    (float) record.Eeg.Delta,
                    (float) record.Eeg.Theta
                };

                yield return new DataPoint
                {
                    Label = Convert.ToUInt32((int) record.Emotion),
                    Features = features
                };
            }
        }

        private static List<T> Shuffle<T>(IEnumerable<T> enumerable, Random random)
        {
            var input = enumerable.ToList();
            var output = new List<T>();

            while (input.Any())
            {
                var element = input[random.Next(input.Count)];
                input.Remove(element);
                output.Add(element);
            }

            return output;
        }

        private static List<List<T>> Split<T>(IEnumerable<T> enumerable, int parts)
        {
            var input = enumerable.ToList();
            var output = new List<List<T>>();

            var partLength = (int) Math.Ceiling(input.Count / (double) parts);

            while (input.Any())
            {
                var batchSize = Math.Min(input.Count, partLength);
                var batch = input.Take(batchSize).ToList();
                input.RemoveRange(0, batchSize);
                output.Add(batch.ToList());
            }

            return output;
        }

        private class DataPoint
        {
            public uint Label { get; set; }

            [VectorType(8)]
            public float[] Features { get; set; }
        }
    }
}
