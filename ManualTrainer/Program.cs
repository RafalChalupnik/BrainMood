using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BrainMood.Abstractions.Extensions;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

namespace BrainMood.ManualTrainer
{
    public class Program
    {
        public static void Main(string[] _)
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

            var avgMicroAccuracy = output.Average(x => x.MicroAccuracy);
        }

        private static MulticlassClassificationMetrics RunLearning(MLContext context, IEnumerable<DataPoint> trainingData, IEnumerable<DataPoint> testData)
        {
            var trainingDataView = context.Data.LoadFromEnumerable(trainingData);

            var options = new FastTreeBinaryTrainer.Options
            {
                // Use L2Norm for early stopping.
                EarlyStoppingMetric = EarlyStoppingMetric.L2Norm,
                // Create a simpler model by penalizing usage of new features.
                FeatureFirstUsePenalty = 0.1,
                // Reduce the number of trees to 50.
                NumberOfTrees = 50
            };

            var pipeline = context.Transforms.Conversion.MapValueToKey("Label")
                .Append(context.MulticlassClassification.Trainers.OneVersusAll(
                    context.BinaryClassification.Trainers.FastTree(options)));

            // Train the model.
            var model = pipeline.Fit(trainingDataView);

            // Create testing data. Use different random seed to make it different
            // from training data.
            var testDataView = context.Data
                .LoadFromEnumerable(testData);

            // Run the model on test data set.
            var transformedTestData = model.Transform(testDataView);

            return context.MulticlassClassification
                .Evaluate(transformedTestData);
        }

        private static IEnumerable<DataPoint> LoadData()
        {
            var csvData = File.ReadAllLines("TODO");

            foreach (var record in csvData.Skip(1))
            {
                var splittedRecord = record.Split(',');

                var features = new[]
                {
                    float.Parse(splittedRecord[1], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[2], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[3], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[4], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[5], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[6], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[7], CultureInfo.InvariantCulture),
                    float.Parse(splittedRecord[8], CultureInfo.InvariantCulture)
                };

                yield return new DataPoint
                {
                    Label = uint.Parse(splittedRecord[0], CultureInfo.InvariantCulture),
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

        // Class used to capture predictions.
        private class Prediction
        {
            public uint Label { get; set; }

            [VectorType(8)]
            public float[] Features { get; set; }

            // Original label.
            public float[] Score { get; set; }

            // Predicted label from the trainer.
            public uint PredictedLabel { get; set; }
        }

        // Pretty-print BinaryClassificationMetrics objects.
        //private static void PrintMetrics(MulticlassClassificationMetrics metrics)
        //{
        //    Console.WriteLine($"Accuracy: {metrics.MicroAccuracy:F2}");
        //    Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:F2}");
        //    Console.WriteLine($"F1 Score: {metrics.F1Score:F2}");
        //    Console.WriteLine($"Negative Precision: " +
        //                      $"{metrics.NegativePrecision:F2}");

        //    Console.WriteLine($"Negative Recall: {metrics.NegativeRecall:F2}");
        //    Console.WriteLine($"Positive Precision: " +
        //                      $"{metrics.PositivePrecision:F2}");

        //    Console.WriteLine($"Positive Recall: {metrics.PositiveRecall:F2}\n");
        //    Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
        //}
    }
}
