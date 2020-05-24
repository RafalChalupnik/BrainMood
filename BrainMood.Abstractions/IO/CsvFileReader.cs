﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.Data.Core;
using BrainMood.Abstractions.Extensions;

namespace BrainMood.Abstractions.IO
{
    public class CsvFileReader
    {
        public List<EegData> ReadEegData(string filePath)
        {
            var csvFile = ReadCsvFile(filePath);
            return ReadEegDataFromCsv(csvFile);
        }

        public List<DataWithoutEmotion> ReadDataWithoutEmotions(string filePath)
        {
            var csvFile = ReadCsvFile(filePath);

            var eegData = ReadEegDataFromCsv(csvFile);
            var eSenseData = ReadESenseDataFromCsv(csvFile);

            return eegData
                .Zip(eSenseData, (eeg, eSense) => new DataWithoutEmotion(eeg, eSense))
                .ToList();
        }

        public List<DataWithoutESense> ReadDataWithoutESense(string filePath)
        {
            var csvFile = ReadCsvFile(filePath);

            var emotions = ReadEmotionsFromCsv(csvFile);
            var eegData = ReadEegDataFromCsv(csvFile);

            return emotions
                .Zip(eegData, (emotion, eeg) => new DataWithoutESense(emotion, eeg))
                .ToList();
        }

        public List<CompleteData> ReadCompleteData(string filePath)
        {
            var csvFile = ReadCsvFile(filePath);

            var emotions = ReadEmotionsFromCsv(csvFile);
            var eegData = ReadEegDataFromCsv(csvFile);
            var eSenseData = ReadESenseDataFromCsv(csvFile);

            return emotions
                .Zip(eegData, (emotion, eeg) => new DataWithoutESense(emotion, eeg))
                .Zip(eSenseData, (dataWithoutESense, eSense) => new CompleteData(dataWithoutESense.Emotion, dataWithoutESense.Eeg, eSense))
                .ToList();
        }

        private static CsvFile ReadCsvFile(string filePath)
            => new CsvFile(File.ReadAllLines(filePath));

        private List<EegData> ReadEegDataFromCsv(CsvFile csvFile)
        {
            return csvFile.Lines
                .Select(ParseEegDataLine)
                .ToList();
        }

        private List<ESenseData> ReadESenseDataFromCsv(CsvFile csvFile)
        {
            return csvFile.Lines
                .Select(ParseESenseDataLine)
                .ToList();
        }

        private List<Emotion> ReadEmotionsFromCsv(CsvFile csvFile)
        {
            return csvFile.Lines
                .Select(ParseEmotionDataLine)
                .ToList();
        }

        private EegData ParseEegDataLine(IReadOnlyDictionary<string, string> csvFileLine)
        {
            return new EegData(
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.AlphaHigh)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.AlphaLow)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.BetaHigh)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.BetaLow)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.Delta)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.GammaHigh)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.GammaLow)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(EegData.Theta)));
        }

        private ESenseData ParseESenseDataLine(IReadOnlyDictionary<string, string> csvFileLine)
        {
            return new ESenseData(
                csvFileLine.GetDoubleFromInvariantKey(nameof(ESenseData.Attention)),
                csvFileLine.GetDoubleFromInvariantKey(nameof(ESenseData.Meditation)));
        }

        private Emotion ParseEmotionDataLine(IReadOnlyDictionary<string, string> csvFileLine)
        {
            return (Emotion) Enum.Parse(typeof(Emotion), csvFileLine.GetFromInvariantKey(nameof(Emotion)));
        }
    }
}