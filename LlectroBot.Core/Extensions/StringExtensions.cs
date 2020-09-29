using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string ToOxfordCommaList(this string[] options, string lastDelimiter = "and", string qualifier = "")
        {
            return JoinEnumerable((IEnumerable<string>)options, ", ", $" {lastDelimiter} ", qualifier);
        }
        public static string ToOxfordCommaList(this IEnumerable<string> options, string lastDelimiter = "and", string qualifier = "")
        {
            return JoinEnumerable(options, ", ", $" {lastDelimiter} ", qualifier);
        }


        public static string ToSentence(this string[] options, string delimiter = " ")
        {
            return JoinEnumerable((IEnumerable<string>)options, delimiter, delimiter, string.Empty);
        }

        public static string ToSentence(this IEnumerable<string> options, string delimiter = " ")
        {
            return JoinEnumerable(options, delimiter, delimiter, string.Empty);
        }

        private static string JoinEnumerable(this IEnumerable<string> options, string delimiter = " ", string lastDelimiter = " ", string qualifier = "")
        {
            string optionString;
            int length = options.Count();

            if (options == null || length == 0)
            {
                return string.Empty;
            }
            else if (length == 1)
            {
                optionString = options.First();
            }
            // when there are just two options in the list
            else if (length == 2)
            {
                optionString = string.Join($"{qualifier}{lastDelimiter}{qualifier}", options);
            }
            else
            {
                string join = $"{qualifier}{delimiter}{qualifier}";
                optionString = string.Join(join, options.Take(length - 1)); // join all but the last two indices with a comma
                                                                            // when there are three options in the list
                                                                            // append the remaining indices
                optionString += $"{qualifier}{delimiter}{lastDelimiter}{qualifier}".Replace("  ", " ") + options.Last();

            }

            return $"{qualifier}{optionString}{qualifier}";
        }
    }
}
