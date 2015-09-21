using System;
using System.Collections.Generic;
using System.Text;
using GroupByInc.Api.Exceptions;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Url
{
    public class UrlReplacement
    {
        public enum OperationType
        {
            Insert,
            Swap
        }

        private readonly int _index;
        private readonly string _replacement;
        private readonly OperationType _type;

        public UrlReplacement(int index, string replacement, OperationType type)
        {
            _index = index;
            _replacement = replacement;
            _type = type;
        }

        public static string BuildQueryString(List<UrlReplacement> replacements)
        {
            StringBuilder sb = new StringBuilder();
            foreach (UrlReplacement replacement in replacements)
            {
                if (sb.Length != 0)
                {
                    sb.Append(UrlReplacementRule.ReplacementDelimiter);
                }
                sb.Append(replacement);
            }
            return sb.ToString();
        }

        /// <exception cref="ParserException">
        ///     Replacement Query Delimiters did not match up
        /// </exception>
        public static List<UrlReplacement> ParseQueryString(string query)
        {
            int delimiterIndex = 0;
            List<UrlReplacement> replacements = new List<UrlReplacement>();
            if (string.IsNullOrEmpty(query))
            {
                return replacements;
            }

            StringBuilder queryString = new StringBuilder(query);
            while (delimiterIndex >= 0)
            {
                int pairSeperator = queryString.ToString()
                    .IndexOf(UrlReplacementRule.ReplacementDelimiter, StringComparison.Ordinal);
                if (pairSeperator < 0)
                {
                    throw new ParserException("Replacement Query Delimiters did not match up");
                }
                delimiterIndex = StringUtils.IndexOf(queryString, UrlReplacementRule.ReplacementDelimiter,
                    pairSeperator + 2);

                if (delimiterIndex < 0)
                {
                    break;
                }
                replacements.Add(FromString(queryString.ToString().Substring(0, delimiterIndex)));
                queryString.Remove(0, delimiterIndex);
                if (char.ToString(queryString[0]).Equals(UrlReplacementRule.ReplacementDelimiter))
                {
                    queryString.Remove(0, 1);
                }
            }
            query = queryString.ToString();
            if (!string.IsNullOrEmpty(query))
            {
                replacements.Add(FromString(query));
            }
            replacements.Reverse();
            return replacements;
        }

        /// <exception cref="ParserException">
        ///     Replacement Query Delimiters did not match up
        /// </exception>
        public static UrlReplacement FromString(string urlReplacementString)
        {
            OperationType operationType = OperationType.Swap;
            int delimiterIndex = urlReplacementString.IndexOf(UrlReplacementRule.ReplacementDelimiter,
                StringComparison.Ordinal);
            if (delimiterIndex < 0)
            {
                throw new ParserException("Argument did not match expected format: " + urlReplacementString);
            }
            string replacementValue = urlReplacementString.Substring(delimiterIndex + 1);
            string indexString = urlReplacementString.Substring(0, delimiterIndex);
            if (indexString.StartsWith(UrlReplacementRule.InsertIndicator))
            {
                operationType = OperationType.Insert;
                indexString = indexString.Substring(1);
            }

            int indexValue;
            if (int.TryParse(indexString, out indexValue))
            {
                return new UrlReplacement(indexValue, replacementValue, operationType);
            }
            throw new ParserException("Invalid index:" + indexString);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_type == OperationType.Insert)
            {
                sb.Append(UrlReplacementRule.InsertIndicator);
            }
            sb.Append(_index);
            sb.Append(UrlReplacementRule.ReplacementDelimiter);
            sb.Append(_replacement);
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is UrlReplacement))
            {
                return false;
            }
            UrlReplacement o = (UrlReplacement) obj;
            if (_index != o._index || _type != o._type)
            {
                return false;
            }
            if (_replacement == null)
            {
                return o._replacement == null;
            }
            return _replacement.Equals(o._replacement);
        }

        public override int GetHashCode()
        {
            int result = _index;
            result = 31*result + (_replacement != null ? _replacement.GetHashCode() : 0);
            result = 31*result + (_type != null ? _type.GetHashCode() : 0);
            return result;
        }

        public void Apply(StringBuilder pathSegment, int offset)
        {
            int relativeIndex = _index - offset;
            if (relativeIndex < 0 || (relativeIndex >= pathSegment.Length && _type == OperationType.Swap) ||
                (relativeIndex > pathSegment.Length && _type == OperationType.Insert))
            {
                return;
            }
            switch (_type)
            {
                case OperationType.Insert:
                    pathSegment.Insert(relativeIndex, _replacement);
                    break;
                case OperationType.Swap:
                    int end = relativeIndex + _replacement.Length;
                    if (end <= pathSegment.ToString().Length)
                    {
                        pathSegment.Replace(pathSegment.ToString().Substring(relativeIndex, end - relativeIndex),
                            _replacement, relativeIndex, end - relativeIndex);
                    }
                    break;
            }
        }
    }
}