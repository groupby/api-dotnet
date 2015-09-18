using System;
using System.Collections.Generic;
using System.Text;

namespace GroupByInc.Api.Url
{
    public class UrlReplacementRule
    {
        public static readonly string InsertIndicator = "i";
        public static readonly string ReplacementDelimiter = "-";
        private readonly string _navigationName;
        private readonly string _target;
        private string _replacement;

        public UrlReplacementRule(char? target, char? replacement, string navigationName)
        {
            _target = target.HasValue ? target.ToString() : "";
            _replacement = replacement.HasValue ? replacement.ToString() : "";
            _navigationName = navigationName;
        }

        public void Apply(StringBuilder url, int indexOffSet, string navigationName,
            List<UrlReplacement> replacements)
        {
            if (url != null && (_navigationName == null || navigationName.Equals(_navigationName)))
            {
                int index = url.ToString().IndexOf(_target, StringComparison.Ordinal);
                while (index != -1)
                {
                    UrlReplacement.OperationType type = UrlReplacement.OperationType.Swap;
                    if (string.IsNullOrEmpty(_replacement))
                    {
                        _replacement = "";
                        type = UrlReplacement.OperationType.Insert;
                        url.Remove(index, 1);
//                        url = new StringBuilder(url.ToString().Remove(index));
                    }
                    else
                    {
                        int end = index + _replacement.Length;
                        if (end <= url.ToString().Length)
                        {
                            url.Replace(url.ToString().Substring(index, end - index), _replacement, index, end - index);
                        }
                    }
                    UrlReplacement replacement = new UrlReplacement(index + indexOffSet, _target, type);
                    replacements.Add(replacement);
                    index = url.ToString().IndexOf(_target, index, StringComparison.Ordinal);
                }
            }
        }
    }
}