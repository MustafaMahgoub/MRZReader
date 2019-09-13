using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
            DateTime? res=null;
            try
            {
                res = DateTime.ParseExact(input, "yyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                // Log
                return res;
            }
            return res;
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
