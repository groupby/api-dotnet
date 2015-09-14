using System;
using System.Collections.Generic;
using System.Text;

namespace GroupByInc.Api.Util
{
    public static class StringUtils
    {
        /// <summary>
        ///     Removes <see langword="null" /> entries from a string array
        /// </summary>
        /// <param name="listOfStrings"></param>
        /// <returns>
        /// </returns>
        public static string[] RemoveNull(string[] listOfStrings)
        {
            List<string> outList = new List<string>();
            foreach (string entry in listOfStrings)
            {
                if (!string.IsNullOrEmpty(entry))
                {
                    outList.Add(entry);
                }
            }
            return outList.ToArray();
        }

        public static int IndexOf(StringBuilder sb, string value, int start)
        {
            try
            {
                int max = sb.Length;
                if (start < max)
                {
                    return sb.ToString().IndexOf(value, start, StringComparison.Ordinal);
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        ///     Checks if a string has length.
        /// </summary>
        /// <example>
        ///     <code lang="C#">
        /// StringUtils.HasLength(null) = false
        /// StringUtils.HasLength("") = false
        /// StringUtils.HasLength(" ") = true
        /// StringUtils.HasLength("Hello") = true
        /// </code>
        /// </example>
        /// <param name="target">
        ///     The string to check, may be <see langword="null" /> .
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if the string has length and is not
        ///     <see langword="null" /> .
        /// </returns>
        public static bool HasLength(string target)
        {
            return !string.IsNullOrEmpty(target);
        }

        /// <summary>
        ///     Checks if a <see cref="string" /> has text.
        /// </summary>
        /// <remarks>
        ///     More specifically, returns <see langword="true" /> if the string is
        ///     not <see langword="null" /> , it's
        ///     <see cref="System.String.Length" /> is > zero <c>(0)</c> , and it
        ///     has at least one non-whitespace character.
        /// </remarks>
        /// <example>
        ///     <code lang="C#">
        /// StringUtils.HasText(null) = false
        /// StringUtils.HasText("") = false
        /// StringUtils.HasText(" ") = false
        /// StringUtils.HasText("12345") = true
        /// StringUtils.HasText(" 12345 ") = true
        /// </code>
        /// </example>
        /// <param name="target">
        ///     The string to check, may be <see langword="null" /> .
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if the <paramref name="target" /> is not
        ///     <see langword="null" /> , <see cref="System.String.Length" /> > zero
        ///     <c>(0)</c> , and does not consist solely of whitespace.
        /// </returns>
        public static bool HasText(string target)
        {
            if (target == null)
            {
                return false;
            }
            return HasLength(target.Trim());
        }
    }
}