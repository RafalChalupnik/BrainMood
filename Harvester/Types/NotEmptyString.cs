using System;

namespace BrainMood.Harvester.Types
{
    public class NotEmptyString
    {
        public string Value;

        public NotEmptyString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be empty!");
            }

            Value = value;
        }

        public static implicit operator string(NotEmptyString notEmptyString) 
            => notEmptyString.Value;

        public static implicit operator NotEmptyString(string value)
            => new NotEmptyString(value);
    }
}
