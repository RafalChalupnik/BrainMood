using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.Data.Core;
using BrainMood.Abstractions.Extensions;

namespace BrainMood.Abstractions.IO
{
    public static class CsvFileWriter
    {
        public static void WriteEegData(string filePath, IEnumerable<EegData> eegData)
        {
            var headerLine = CreateEegDataHeaderLine();
            var contentLines = eegData
                .Select(ToCsvLine)
                .ToList();

            WriteToCsvFile(filePath, headerLine, contentLines);
        }

        public static void WriteDataWithoutEmotions(string filePath, IEnumerable<DataWithoutEmotion> dataWithoutEmotions)
        {
            var headerLine = CreateDataWithoutEmotionsHeaderLine();
            var contentLines = dataWithoutEmotions
                .Select(ToCsvLine)
                .ToList();

            WriteToCsvFile(filePath, headerLine, contentLines);
        }

        public static void WriteDataWithoutESense(string filePath, IEnumerable<DataWithoutESense> dataWithoutESense)
        {
            var headerLine = CreateDataWithoutESenseHeaderLine();
            var contentLines = dataWithoutESense
                .Select(ToCsvLine)
                .ToList();

            WriteToCsvFile(filePath, headerLine, contentLines);
        }

        public static void WriteCompleteData(string filePath, IEnumerable<CompleteData> completeData)
        {
            var headerLine = CreateCompleteDataHeaderLine();
            var contentLines = completeData
                .Select(ToCsvLine)
                .ToList();

            WriteToCsvFile(filePath, headerLine, contentLines);
        }

        private static string CreateEegDataHeaderLine()
        {
            return string.Join(",",
                nameof(EegData.AlphaHigh),
                nameof(EegData.AlphaLow),
                nameof(EegData.BetaHigh),
                nameof(EegData.BetaLow),
                nameof(EegData.GammaHigh),
                nameof(EegData.GammaLow),
                nameof(EegData.Delta),
                nameof(EegData.Theta));
        }

        private static string CreateDataWithoutEmotionsHeaderLine()
        {
            return string.Join(",",
                CreateEegDataHeaderLine(),
                nameof(ESenseData.Attention),
                nameof(ESenseData.Meditation));
        }

        private static string CreateDataWithoutESenseHeaderLine()
        {
            return string.Join(",",
                nameof(Emotion),
                CreateEegDataHeaderLine());
        }

        private static string CreateCompleteDataHeaderLine()
        {
            return string.Join(",",
                nameof(Emotion),
                CreateEegDataHeaderLine(),
                nameof(ESenseData.Attention),
                nameof(ESenseData.Meditation));
        }

        private static string ToCsvLine(EegData eegData)
        {
            return string.Join(";",
                eegData.AlphaHigh,
                eegData.AlphaLow,
                eegData.BetaHigh,
                eegData.BetaLow,
                eegData.GammaHigh,
                eegData.GammaLow,
                eegData.Delta,
                eegData.Theta);
        }

        private static string ToCsvLine(DataWithoutEmotion dataWithoutEmotion)
        {
            return string.Join(";",
                ToCsvLine(dataWithoutEmotion.Eeg),
                dataWithoutEmotion.ESense.Attention,
                dataWithoutEmotion.ESense.Meditation);
        }

        private static string ToCsvLine(DataWithoutESense dataWithoutESense)
        {
            return string.Join(";",
                (int) dataWithoutESense.Emotion,
                ToCsvLine(dataWithoutESense.Eeg));
        }

        private static string ToCsvLine(CompleteData completeData)
        {
            return string.Join(";",
                (int)completeData.Emotion,
                ToCsvLine(completeData.Eeg),
                completeData.ESense.Attention,
                completeData.ESense.Meditation);
        }

        private static void WriteToCsvFile(string filePath, string headerLine, IEnumerable<string> contentLines)
        {
            var csvLines = headerLine
                .Union(contentLines)
                .ToList();

            File.WriteAllLines(filePath, csvLines);
        }
    }
}