using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainMood.Abstractions.IO
{
    internal class CsvFile
    {
        public IReadOnlyList<Dictionary<string, string>> Lines { get; }

        public CsvFile(IReadOnlyCollection<string> csvFileLines, char separator = ',')
        {
            var headers = csvFileLines.First().Split(separator).ToList();
            var contentLines = csvFileLines.Skip(1).ToList();

            var lines = contentLines
                .Select(contentLine => ConvertLineToDictionary(contentLine, separator, headers))
                .ToList();

            Lines = lines;
        }

        private static Dictionary<string, string> ConvertLineToDictionary(string line, char separator, IReadOnlyList<string> headers)
        {
            var expectedValuesCount = headers.Count;
            var values = line.Split(separator).ToList();

            if (values.Count != expectedValuesCount)
            {
                throw new IOException("Invalid CSV file");
            }

            var rowDictionary = new Dictionary<string, string>();

            for (var i = 0; i < expectedValuesCount; i++)
            {
                rowDictionary.Add(headers[i], values[i]);
            }

            return rowDictionary;
        }
    }
}