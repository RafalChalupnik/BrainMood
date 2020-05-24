using System.Collections.Generic;
using System.Globalization;

namespace BrainMood.Abstractions.Extensions
{
    public static class DictionaryExtensions
    {
        public static double GetDoubleFromInvariantKey(this IReadOnlyDictionary<string, string> dictionary, string key)
            => double.Parse(dictionary[key.ToLowerInvariant()], CultureInfo.InvariantCulture);

        public static string GetFromInvariantKey(this IReadOnlyDictionary<string, string> dictionary, string key)
            => dictionary[key.ToLowerInvariant()];
    }
}