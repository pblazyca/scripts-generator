using System;

namespace ScriptsGenerator.Helpers
{
    public static class Converters
    {
        public static string ConvertEnumToLabel<T>(T toChange) where T : Enum
        {
            string label = toChange.ToString();
            label = label.ToLower();
            label = label.Replace(Constants.UNDERLINE, Constants.SPACE);

            return label;
        }
    }
}