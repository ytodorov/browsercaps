using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Extensions
{
    public static class StringExtensions
    {
        public static string TrimToLength(this string stringToTrim, int length = 50)
        {
            if (string.IsNullOrEmpty(stringToTrim))
            {
                return string.Empty;
            }
            string result = string.Empty;
            if (stringToTrim.Length > length)
            {
                result = stringToTrim.Substring(0, length) + "...";
            }
            else
            {
                result = stringToTrim;
            }
            return result;
        }
    }
}