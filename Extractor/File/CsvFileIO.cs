using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BrainMood.Client.Data;

namespace Extractor.File
{
    public static class CsvFileIO
    {
        public static List<Data> Read(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath).Skip(1).ToList();
            var emotion = GetEmotion(filePath);

            return lines
                .Select(Deserialize)
                .Select(eegData => new Data(emotion, eegData))
                .ToList();
        }

        public static void Write(string filePath, IEnumerable<Data> data)
        {
            var header = new List<string>
            {
                "emotion,alphaHigh,alphaLow,betaHigh,betaLow,delta,gammaHigh,gammaLow,theta"
            };

            var dataLines = data
                .Select(Serialize)
                .ToList();

            var fileLines = header.Union(dataLines).ToList();

            System.IO.File.WriteAllLines(filePath, fileLines);
        }

        private static EegData Deserialize(string csvLine)
        {
            var values = csvLine.Split(',');

            var alphaHigh = double.Parse(values[0]);
            var alphaLow = double.Parse(values[1]);
            var betaHigh = double.Parse(values[2]);
            var betaLow = double.Parse(values[3]);
            var delta = double.Parse(values[4]);
            var gammaHigh = double.Parse(values[5]);
            var gammaLow = double.Parse(values[6]);
            var theta = double.Parse(values[7]);

            var attention = int.Parse(values[8]);
            var meditation = int.Parse(values[9]);

            var brainwaveReadings = new BrainwaveReadings(
                alphaHigh,
                alphaLow,
                betaHigh,
                betaLow,
                delta,
                gammaHigh,
                gammaLow,
                theta);

            var eSenseReadings = new ESenseReadings(attention, meditation);

            return new EegData(brainwaveReadings, eSenseReadings);
        }

        private static string Serialize(Data data)
        {
            var eegData = data.EegData;

            var values = new List<string>
            {
                data.Emotion.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.AlphaHigh.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.AlphaLow.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.BetaHigh.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.BetaLow.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.Delta.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.GammaHigh.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.GammaLow.ToString(CultureInfo.InvariantCulture),
                eegData.Brainwaves.Theta.ToString(CultureInfo.InvariantCulture),
                //eegData.ESense.Attention.ToString(CultureInfo.InvariantCulture),
                //eegData.ESense.Meditation.ToString(CultureInfo.InvariantCulture)
            };

            return string.Join(",", values);
        }

        private static int GetEmotion(string filePath)
        {
            if (filePath.Contains("anger"))
            {
                return 0;
            }

            if (filePath.Contains("depression"))
            {
                return 1;
            }

            if (filePath.Contains("happiness"))
            {
                return 2;
            }

            if (filePath.Contains("relax"))
            {
                return 3;
            }

            throw new Exception("Nope nope nope");
        }
    }
}