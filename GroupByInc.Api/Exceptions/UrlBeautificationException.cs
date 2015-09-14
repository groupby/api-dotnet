using System;

namespace GroupByInc.Api.Exceptions
{
    public class UrlBeautificationException : Exception
    {
        public UrlBeautificationException(string message)
            : base(message)
        {
        }

        public UrlBeautificationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}