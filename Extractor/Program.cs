using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extractor.File;
using Extractor.Filters;

namespace Extractor
{
    public class Program
    {
        private const string c_dataDirectory = "TODO";

        private const int c_treshold = 50;

        private const int c_windowWidth = 10;
        private const int c_step = 1;

        private const string c_outputFile = "TODO";

        public static void Main(string[] _)
        {
            // Extract();

            var data = CsvFileIO.Read(c_outputFile);
        }

        private static void Extract()
        {
            Console.WriteLine($"Getting CSV files from {c_dataDirectory}...");
            var csvFiles = Directory.GetFiles(c_dataDirectory, "*.csv");

            var processedData = csvFiles
                .SelectMany(ProcessFile)
                .ToList();

            Console.WriteLine($"Saving data to {c_outputFile}");
            CsvFileIO.Write(c_outputFile, processedData);

            Console.WriteLine("### Done! ###");
        }

        private static List<Data> ProcessFile(string filePath)
        {
            Console.WriteLine($"Processing file: {filePath}:");

            Console.WriteLine($"Reading data from {c_dataDirectory}...");
            var data = CsvFileIO.Read(filePath);

            Console.WriteLine($"Filtering by treshold = {c_treshold}...");
            var filteredByTreshold = new AttentionTresholdFilter(c_treshold).Execute(data);

            Console.WriteLine($"Smoothing with window width = {c_windowWidth}, step = {c_step}");
            var meanData = new MeanSmoothingFilter(c_windowWidth, c_step).Execute(filteredByTreshold);

            Console.WriteLine();
            return meanData;
        }

        private static List<Data> LoadAllCsvFromFolder(string folderPath)
        {
            var csvFiles = Directory.GetFiles(folderPath, "*.csv");

            return csvFiles
                .SelectMany(CsvFileIO.Read)
                .ToList();
        }
    }
}
