using System.Collections.Generic;
using System.Linq;

namespace BrainMood.Abstractions.Extensions
{
    public static class GenericExtensions
    {
        public static List<T> AsList<T>(this T element)
            => new List<T> {element};

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T elementToRemove)
            => enumerable.Except(elementToRemove.AsList());

        public static IEnumerable<T> Union<T>(this T element, IEnumerable<T> enumerable)
            => element.AsList().Union(enumerable);
    }
}