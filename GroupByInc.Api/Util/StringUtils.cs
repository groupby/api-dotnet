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
    }
}