using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Discord.Cards.Extensions
{
    public static class StringExtensions
    {
        public static string ToSentence(this string[] options, string qualifier = "")
        {
            return ToSentence((IEnumerable<string>)options, qualifier);
        }

        public static string ToSentence(this IEnumerable<string> options, string qualifier = "")
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
                optionString = string.Join($"{qualifier} and {qualifier}", options);
            }
            else
            {
                string join = $"{qualifier}, and {qualifier}";
                optionString = string.Join($"{qualifier}, {qualifier}", options.Take(length - 2)); // join all but the last two indices with a comma
                                                                                        // when there are three options in the list
                if (optionString.Length == 3)
                {
                    // append the last index manually
                    optionString += $"{join}{options.Last()}"; // add the last index
                }
                // when there are four or more options
                else
                {
                    // append the remaining indices
                    optionString += string.Join(join, options.Skip(length - 2));
                }
            }

            return $"{qualifier}{optionString}{qualifier}";
        }
    }
}
