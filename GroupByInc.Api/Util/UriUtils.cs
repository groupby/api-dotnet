﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace GroupByInc.Api.Util
{
    public class UriUtils
    {
        public static string NormalizePath(string path)
        {
            if (path == null)
            {
                return null;
            }
            int n = 0;
            for (; n < path.Length; n++)
            {
                if (path[n] != '/')
                {
                    break;
                }
            }
            if (n > 1)
            {
                path = path.Substring(n - 1);
            }
            return path;
        }

        public static string ToQueryString(NameValueCollection nvc)
        {
            List<string> array = new List<string>();
            foreach (string allKey in nvc.AllKeys)
            {
                string s = nvc[allKey];
                array.Add(string.Format("{0}={1}", UrlEncode(allKey), UrlEncode(s)));
            }

            return string.Join("&", array.ToArray());
        }

        public static string UrlEncode(string s)
        {
            if (s == null)
            {
                return null;
            }
            return HttpUtility.UrlEncode(s)
                .Replace("!", "%21")
                .Replace("'", "%27")
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("*", "%2A");
        }

        public static void AddPath(UriBuilder uri, string value)
        {
            value = NormalizePath(value);
            if (!string.IsNullOrEmpty(uri.Path) && uri.Path.Length > 1)
            {
                uri.Path += value;
            }
            else
            {
                uri.Path = value;
            }
        }

        public static bool IsRelative(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Relative, out result);
        }

        public static string GetQuery(Uri uri)
        {
            string query = uri.Query;
            if (query.StartsWith("?"))
            {
                return query.Substring(1, query.Length - 1);
            }
            return query;
        }

        public static string GetDecodedQuery(Uri uri)
        {
            return HttpUtility.UrlDecode(GetQuery(uri));
        }

        public static string GetPath(UriBuilder uri)
        {
            string path = uri.Path;
            if (path.Length == 1 && path.Equals("/"))
            {
                return "";
            }
            return path;
        }

        public static string GetRawPath(Uri uri)
        {
            int host = uri.OriginalString.LastIndexOf(uri.Host, StringComparison.Ordinal) + uri.Host.Length;
            int @params = uri.OriginalString.LastIndexOf('?');
            return @params > 0 ? uri.OriginalString.Substring(host, @params - host) : uri.OriginalString.Substring(host);
        }

        public static string UriToString(UriBuilder uri)
        {
            string s = uri.Path + uri.Query;

            return uri.Path.Length == 1 ? s.Substring(1) : s;
        }

        public static string DoubleDecode(string value)
        {
            return HttpUtility.UrlDecode(HttpUtility.UrlDecode(value));
        }
    }
}