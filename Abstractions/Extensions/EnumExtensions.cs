using System;

namespace BrainMood.Abstractions.Extensions
{
    public static class EnumExtensions
    {
        public static void AssertIsDefined<T>(this T enumToCheck) where T : System.Enum
        {
            if (!Enum.IsDefined(typeof(T), enumToCheck))
            {
                throw new ArgumentException("Invalid enum value.");
            }
        }
    }
}