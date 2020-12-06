using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.Core
{
    public static class StringUtils
    {
        public static string RemoveWhitespaces(this string str) => Remove(str, @"\s");

        /// <summary>
        /// Replaces string to empty string using regex
        /// </summary>
        public static string Remove(this string str, string pattern) => Regex.Replace(str, pattern, "");

    }
}
