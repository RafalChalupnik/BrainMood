using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.Data.Core;
using BrainMood.Abstractions.IO;
using BrainMood.Abstractions.Transformation;
using BrainMood.Abstractions.Transformation.Filters;

namespace BrainMood.DataTransformer
{
    public class Program
    {
        private const string c_dataDirectory = "TODO";
        private const string c_outputFile = "TODO";

        private static readonly FilterWrapper s_filters = new FilterWrapper(
            //new AttentionTresholdFilter(treshold: 50),
            new MovingAverageFilter(windowWidth: 10, step: 1));

        public static void Main(string[] _)
        {
            var csvFiles = Directory.GetFiles(c_dataDirectory, "*.csv");

            var processedData = csvFiles
                .SelectMany(ProcessFile)
                .ToList();

            WriteToCsvFile(c_outputFile, processedData);

            Console.WriteLine("### Done! ###");
        }

        private static List<CompleteData> ProcessFile(string filePath)
        {
            var data = ReadCsvFile(filePath);
            return s_filters.Apply(data);
        }

        private static List<CompleteData> ReadCsvFile(string filePath)
        {
            var emotion = GetEmotion(filePath);
            var dataWithoutEmotion = CsvFileReader.ReadDataWithoutEmotions(filePath);

            return dataWithoutEmotion
                .Select(data => new CompleteData(emotion, data.Eeg, data.ESense))
                .ToList();
        }

        private static Emotion GetEmotion(string filePath)
        {
            if (filePath.Contains("anger"))
            {
                return Emotion.Anger;
            }

            if (filePath.Contains("depression"))
            {
                return Emotion.Depression;
            }

            if (filePath.Contains("happiness"))
            {
                return Emotion.Happiness;
            }

            if (filePath.Contains("relax"))
            {
                return Emotion.Relax;
            }

            throw new Exception("Emotion cannot be recognized from the file name.");
        }

        private static void WriteToCsvFile(string filePath, IEnumerable<CompleteData> data)
        {
            var dataWithoutESense = data
                .Select(DropESense)
                .ToList();

            CsvFileWriter.WriteDataWithoutESense(filePath, dataWithoutESense);
        }

        private static DataWithoutESense DropESense(CompleteData completeData)
            => new DataWithoutESense(completeData.Emotion, completeData.Eeg);
    }
}
