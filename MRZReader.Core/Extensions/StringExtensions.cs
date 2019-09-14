using System;
using System.Globalization;

namespace MRZReader.Core
{
    public static class StringExtensions
    {
        public static bool ConvertToBoolSafely(this string input)
        {
            bool.TryParse(input, out bool value);
            return value;
        }
        public static DateTime? ConvertToDateTimeSafely(this string input)
        {
            try
            {
                return DateTime.ParseExact(input, "yyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static int ConvertToIntSafely(this string input)
        {
            if (!int.TryParse(input, out int i))
            {
                i = -1;
            }
            return i;
        }
    }
}
