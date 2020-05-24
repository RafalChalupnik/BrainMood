using BrainMood.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BrainMood.Client.Data;
using BrainMood.Client.Mindwave.Events;

namespace BrainMood.Harvester.GUI
{
    internal static class Extensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list, Random random)
        {
            var availableElements = list.ToList();

            while (availableElements.Any())
            {
                var randomElement = availableElements[random.Next(availableElements.Count)];
                availableElements.Remove(randomElement);
                yield return randomElement;
            }
        }
    }

    internal class Session
    {
        private readonly SessionConfig m_sessionConfig;
        private readonly List<EegData> m_data;

        public Session(SessionConfig config)
        {
            m_sessionConfig = config;
            m_data = new List<EegData>();
        }

        public async void Run()
        {
            var sampleFiles = GetSampleFiles(m_sessionConfig.DataDirectory);
            var emotions = GetEmotions(sampleFiles);
            var playlist = CreatePlaylist(sampleFiles, emotions, m_sessionConfig.SamplesPerEmotion);

            var headsetClient = new HeadsetClient();
            
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = headsetClient.Run(cancellationTokenSource.Token);
                WaitForHeadsetToConnect(headsetClient);
                RegisterDataEvents(headsetClient);
                
                foreach (var sample in playlist)
                {
                    ShowBreak(m_sessionConfig.BreakTime);
                    m_data.Clear();
                    ShowSample(sample, m_sessionConfig.TimePerSample);
                    SaveSampleData(sample, m_sessionConfig.OutputDirectory, m_data);
                    
                    var assessment = ShowAssessment();
                    SaveAssessment(sample, m_sessionConfig.OutputDirectory, assessment);
                }

                cancellationTokenSource.Cancel();
                await task;
            }
        }

        private static List<string> GetSampleFiles(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, searchPattern: "*.jpg").ToList();
        }

        private static List<string> GetEmotions(IEnumerable<string> sampleFiles)
        {
            return sampleFiles
                .Select(Path.GetFileNameWithoutExtension)
                .Select(fileName => fileName.Split('_').First())
                .Distinct()
                .ToList();
        }

        private static List<string> CreatePlaylist(IEnumerable<string> sampleFiles, List<string> emotions, int samplesPerEmotion)
        {
            var availableSamples = sampleFiles.ToList();

            var random = new Random();
            var playlist = new List<string>();

            foreach (var emotion in emotions)
            {
                var emotionSamples = availableSamples
                    .Where(fileName => Path.GetFileName(fileName).Split('_').First() == emotion)
                    .ToList();

                availableSamples = availableSamples.Except(emotionSamples).ToList();

                playlist.AddRange(SelectRandom(emotionSamples, random, samplesPerEmotion));
            }

            return playlist;
        }

        private static List<string> SelectRandom(IEnumerable<string> sampleFiles, Random random, int samplesCount)
        {
            return sampleFiles.Randomize(random).Take(samplesCount).ToList();
        }

        private static void WaitForHeadsetToConnect(HeadsetClient headsetClient)
        {
            var headsetConnectingForm = new HeadsetConnecting(headsetClient);
            _ = headsetConnectingForm.ShowDialog();
        }

        private void RegisterDataEvents(HeadsetClient headsetClient)
        {
            headsetClient.DataReceived += (sender, args) => m_data.Add(args.Data);
            headsetClient.ErrorOccured += (sender, args) => throw new Exception(args.Error);
        }

        private static void ShowBreak(int time)
        {
            var breakForm = new Break(time);
            _ = breakForm.ShowDialog();
        }

        private static void ShowSample(string sample, int time)
        {
            var photoDisplayForm = new PhotoDisplay(sample, time);
            _ = photoDisplayForm.ShowDialog();
        }

        private static void SaveSampleData(string sampleFilePath, string outputDirectory, IEnumerable<EegData> data)
        {
            var outputFileName = $"{Path.GetFileNameWithoutExtension(sampleFilePath)}_data.csv";
            var outputFilePath = Path.Combine(outputDirectory, outputFileName);

            var serializedData = SerializeEegData(data);
            File.WriteAllLines(outputFilePath, serializedData);
        }

        private static AssessmentResult ShowAssessment()
        {
            var assessmentForm = new Assessment();
            _ = assessmentForm.ShowDialog();
            return assessmentForm.Result;
        }

        private static void SaveAssessment(string sampleFilePath, string outputDirectory, AssessmentResult result)
        {
            var outputFileName = $"{Path.GetFileNameWithoutExtension(sampleFilePath)}_assessment.csv";
            var outputFilePath = Path.Combine(outputDirectory, outputFileName);

            var fileContent = new List<string>
            {
                "happiness,excitement,control",
                $"{result.Happiness},{result.Excitement},{result.Control}"
            };

            File.WriteAllLines(outputFilePath, fileContent);
        }

        private static List<string> SerializeEegData(IEnumerable<EegData> data)
        {
            var output = new List<string>
            {
                "alphaHigh,alphaLow,betaHigh,betaLow,delta,gammaHigh,gammaLow,theta,attention,meditation"
            };

            output.AddRange(data.Select(EegDataToCsvString));

            return output;
        }

        private static string EegDataToCsvString(EegData data)
        {
            return string.Join(",",
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