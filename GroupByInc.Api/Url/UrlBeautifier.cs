using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using GroupByInc.Api.Exceptions;
using GroupByInc.Api.Injector;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Url
{
    public class UrlBeautifier
    {
        public static readonly string ParamReplacement = "z";
        public static readonly string SearchNavigationName = "search";
        private static readonly string RefinementsParamDefault = "refinements";
        private static readonly string Id = "id";
        private static readonly Regex idPattern = new Regex("(?:\\A|.*&)id=([^&]*).*", RegexOptions.Compiled);

        public static readonly StaticInjector<Dictionary<string, UrlBeautifier>> Injector =
            new StaticInjectorFactory<Dictionary<string, UrlBeautifier>>().Create();

        private readonly List<UrlReplacementRule> _replacementRules = new List<UrlReplacementRule>();
        private readonly Navigation _searchNavigation = new Navigation().SetDisplayName("");
        private string _append;
        private OrderedDictionary _nameToToken = new OrderedDictionary();
        private NameValueCollection _queryParams = new NameValueCollection();
        private string _refinementsQueryParameterName = RefinementsParamDefault;
        private List<Navigation> _remainingMappings = new List<Navigation>();
        private IOrderedDictionary _tokenToName = new OrderedDictionary();

        static UrlBeautifier()
        {
            Injector.Set(new Dictionary<string, UrlBeautifier>());
        }

        protected Query CreateQuery()
        {
            return new Query();
        }

        public static void CreateBeautifier(string name)
        {
            GetUrlBeautifiers().Add(name, new UrlBeautifier());
        }

        public static Dictionary<string, UrlBeautifier> GetUrlBeautifiers()
        {
            return Injector.Get();
        }

        public string ToUrl(string searchString, string existingRefinements)
        {
            StringBuilder pathSegmentLookup = new StringBuilder("/");
            Query query = CreateQuery();
            if (!string.IsNullOrEmpty(searchString))
            {
                query.SetQuery(searchString);
            }

            UriBuilder uri = new UriBuilder("", "");
            uri.Path = "";
            query.AddRefinementsByString(existingRefinements);
            OrderedDictionary navigations = GetDistinctRefinements(query);
            AddRefinements(query.GetQuery(), navigations, pathSegmentLookup, uri);
            AddReferenceBlock(pathSegmentLookup, uri);
            AddAppend(uri);
            AddUnMappedRefinements(navigations, uri);
            uri.Query = UriUtils.ToQueryString(_queryParams);

            // clear query params
            _queryParams = new NameValueCollection();
            string uriString = UriUtils.UriToString(uri);

            return uriString.StartsWith("null") ? uriString.Substring(4) : uriString;
        }

        /// <exception cref="UrlBeautificationException">
        ///     URL refence block is invalid, could not convert to query
        /// </exception>
        public Query FromUrl(string url)
        {
            Query query = FromUrl(url, null);
            if (query == null)
            {
                throw new UrlBeautificationException("URL refence block is invalid, could not convert to query");
            }
            return query;
        }

        /// <exception cref="UrlBeautificationException">
        ///     Unable to parse <paramref name="url" />
        /// </exception>
        public Query FromUrl(string url, Query defaultQuery)
        {
            Uri uri = null;

            try
            {
                if (UriUtils.IsRelative(url))
                {
                    uri = new Uri(string.Format("{0}{1}", "http://groupbyinc.com", url));
                }
                else
                {
                    uri = new Uri(url);
                }
            }
            catch
            {
                throw new UrlBeautificationException("Unable to parse url");
            }

            string urlQueryString = UriUtils.GetDecodedQuery(uri);
            if (!string.IsNullOrEmpty(urlQueryString) && idPattern.IsMatch(urlQueryString))
            {
                Group @group = idPattern.Match(urlQueryString).Groups[1];
                return CreateQuery().AddValueRefinement(Id, @group.Value);
            }
            Query query = CreateQuery();
            string replacementUrlQueryString = GetReplacementQuery(UriUtils.GetQuery(uri));
            List<string> pathSegments = new List<string>();
            string uriPath = UriUtils.GetRawPath(uri);
            if (!string.IsNullOrEmpty(_append) && uriPath.EndsWith(_append))
            {
                uriPath = uriPath.Substring(0, uriPath.Length - _append.Length);
            }
            CollectionUtils.AddAll(pathSegments, uriPath.Split('/'));
            string pathSegementLookup = LastSegment(pathSegments);

            if (pathSegments.Count > pathSegementLookup.Length)
            {
                RemoveUnusedPathSegments(pathSegments, pathSegementLookup);
            }
            else if (pathSegments.Count < pathSegementLookup.Length)
            {
                return defaultQuery;
            }
            try
            {
                pathSegments = ApplyReplacementToPathSegment(pathSegments,
                    UrlReplacement.ParseQueryString(replacementUrlQueryString));
            }
            catch (ParserException  e)
            {
                throw new UrlBeautificationException("Replacement Query is malformed, returning default query", e);
            }

            while (pathSegments.Count > 0)
            {
                AddRefinement(pathSegments, query, pathSegementLookup);
            }

            if (!string.IsNullOrEmpty(urlQueryString))
            {
                string[] queryParams = urlQueryString.Split('&');
                if (CollectionUtils.IsNotNullOrEmpty(queryParams))
                {
                    foreach (string queryParam in queryParams)
                    {
                        if (queryParam.StartsWith(_refinementsQueryParameterName + "="))
                        {
                            string v = queryParam.Substring(_refinementsQueryParameterName.Length);
                            query.AddRefinementsByString(v);
                            break;
                        }
                    }
                }
            }
            return query;
        }

        private void RemoveUnusedPathSegments(List<string> pathSegments, string pathSegementLookup)
        {
            while (pathSegments.Count > pathSegementLookup.Length)
            {
                pathSegments.RemoveAt(0);
            }
        }

        private void AddRefinement(List<string> pathSegments, Query query, string pathSegementLookup)
        {
            string token = pathSegementLookup[pathSegementLookup.Length - pathSegments.Count].ToString();
            if (token.Equals(_searchNavigation.GetDisplayName()))
            {
                query.SetQuery(pathSegments[0]);
                pathSegments.RemoveAt(0);
            }
            else if (GetFieldName(token) != null)
            {
                query.AddValueRefinement(GetFieldName(token), pathSegments[0]);
                pathSegments.RemoveAt(0);
            }
            else
            {
                pathSegments.RemoveAt(0);
            }
        }

        private List<string> ApplyReplacementToPathSegment(List<string> pathSegments,
            List<UrlReplacement> replacements)
        {
            if (CollectionUtils.IsNullOrEmpty(pathSegments))
            {
                return pathSegments;
            }
            List<string> replacedPathSegments = new List<string>(pathSegments.Count);
            int indexOffset = 1;
            foreach (string pathSegment in pathSegments)
            {
                StringBuilder decodedPathSegment = new StringBuilder(UriUtils.DoubleDecode(pathSegment));
                foreach (UrlReplacement urlReplacement in replacements)
                {
                    urlReplacement.Apply(decodedPathSegment, indexOffset);
                }

                replacedPathSegments.Add(decodedPathSegment.ToString());
                indexOffset += decodedPathSegment.Length + 1;
            }
            return replacedPathSegments;
        }

        private string LastSegment(List<string> pathSegments)
        {
            if (pathSegments.Count > 0)
            {
                int idx = pathSegments.Count - 1;
                for (int i = 0; i < idx; i++)
                {
                    if (string.IsNullOrEmpty(pathSegments[idx]))
                    {
                        pathSegments.RemoveAt(idx);
                        idx--;
                        continue;
                    }

                    string last = pathSegments[idx];
                    pathSegments.RemoveAt(idx);
                    return last;
                }
            }
            return "";
        }

        public void AddReplacementRule(char? target, char? replacement)
        {
            AddReplacementRule(target, replacement, null);
        }

        public void AddReplacementRule(char? target, char? replacement, string refinementName)
        {
            if (target != replacement)
            {
                _replacementRules.Add(new UrlReplacementRule(target, replacement, refinementName));
            }
        }

        private string GetReplacementQuery(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                foreach (string token in query.Split('&'))
                {
                    if (token.StartsWith(ParamReplacement + "="))
                    {
                        return HttpUtility.UrlDecode(token.Substring(2));
                    }
                }
            }
            return "";
        }

        private OrderedDictionary GetDistinctRefinements(Query query)
        {
            OrderedDictionary navigations = query.GetNavigations();
            foreach (Navigation n in navigations.Values)
            {
                HashSet<string> names = new HashSet<string>();
                List<Refinement> deletRefinements = new List<Refinement>();
                foreach (Refinement refinement1 in n.GetRefinements())
                {
                    Refinement refinement = refinement1;
                    string name = n.GetName() + refinement.ToTildeString();
                    if (!names.Contains(name))
                    {
                        names.Add(name);
                    }
                    else
                    {
                        deletRefinements.Add(refinement1);
                    }
                }

                foreach (Refinement refinement in deletRefinements)
                {
                    n.GetRefinements().Remove(refinement);
                }
            }
            return navigations;
        }

        private void AddUnMappedRefinements(OrderedDictionary navigations, UriBuilder uri)
        {
            if (CollectionUtils.IsNotNullOrEmpty(navigations))
            {
                Query query = CreateQuery();
                OrderedDictionary distinctRefinements = GetDistinctRefinements(query);

                IDictionaryEnumerator enumerator = navigations.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Navigation n = (Navigation) distinctRefinements[enumerator.Key];
                    if (n == null)
                    {
                        distinctRefinements.Add(enumerator.Key, enumerator.Value);
                    }
                    else
                    {
                        CollectionUtils.AddAll(n.GetRefinements(), ((Navigation) enumerator.Value).GetRefinements());
                    }
                }

                string refinements = query.GetRefinementsString();
                if (!string.IsNullOrEmpty(refinements))
                {
                    //TODO need to update this. not sure if this is a sound implementation
                    _queryParams.Add(_refinementsQueryParameterName, refinements);
                }
            }
        }

        public string GetAppend()
        {
            return _append;
        }

        public void AddAppend(UriBuilder uri)
        {
            if (!string.IsNullOrEmpty(_append))
            {
                UriUtils.AddPath(uri, _append);
            }
        }

        public string GetRefinementsQueryParameterName()
        {
            return _refinementsQueryParameterName;
        }

        public void SetRefinementsQueryParameterName(string refinementQueryParameterName)
        {
            _refinementsQueryParameterName = refinementQueryParameterName;
        }

        private void AddReferenceBlock(StringBuilder pathSegmentLookup, UriBuilder uri)
        {
            if (pathSegmentLookup.Length > 1)
            {
                UriUtils.AddPath(uri, pathSegmentLookup.ToString());
            }
        }

        private void AddRefinements(string searchString, OrderedDictionary navigations,
            StringBuilder pathSegmentLookup, UriBuilder uri)
        {
            int indexOffSet = UriUtils.GetPath(uri).Length + 1;
            List<UrlReplacement> replacements = new List<UrlReplacement>();
            foreach (Navigation m in _remainingMappings)
            {
                if (m == _searchNavigation && !string.IsNullOrEmpty(searchString))
                {
                    searchString = ApplyReplacementRule(m, searchString, indexOffSet, replacements);
                    indexOffSet += searchString.Length + 1;
                    AddSearchString(searchString, pathSegmentLookup, uri);
                    continue;
                }

                Navigation n = (Navigation) navigations[m.GetName()];
                if (n != null)
                {
                    List<Refinement>.Enumerator enumerator = n.GetRefinements().GetEnumerator();
                    List<Refinement> deletRefinements = new List<Refinement>();
                    while (enumerator.MoveNext())
                    {
                        Refinement r = enumerator.Current;
                        switch (r.GetType())
                        {
                            case Refinement.TypeEnum.Value:
                                pathSegmentLookup.Append(GetToken(n.GetName()));
                                RefinementValue rv = (RefinementValue) r;
                                rv.SetValue(ApplyReplacementRule(n, rv.GetValue(), indexOffSet, replacements));
                                string encodedRefValue = "/" + HttpUtility.UrlEncode(rv.GetValue());
                                indexOffSet += rv.GetValue().Length + 1;
                                UriUtils.AddPath(uri, encodedRefValue);
                                deletRefinements.Add(r);
                                break;
                            case Refinement.TypeEnum.Range:
                                throw new UrlBeautificationException("You should not map ranges into URLS.");
                        }
                    }

                    // purge refinements
                    foreach (Refinement deletRefinement in deletRefinements)
                    {
                        n.GetRefinements().Remove(deletRefinement);
                    }

                    if (n.GetRefinements().Count == 0)
                    {
                        navigations.Remove(n.GetName());
                    }
                }
            }
            if (replacements.Count != 0)
            {
                _queryParams.Add(ParamReplacement, UrlReplacement.BuildQueryString(replacements));
            }
        }

        public void SetAppend(string append)
        {
            _append = append;
        }

        private string GetFieldName(string token)
        {
            Navigation mapping = (Navigation) _tokenToName[token];
            return mapping == null ? null : mapping.GetName();
        }

        private string GetToken(string name)
        {
            Navigation mapping = (Navigation) _nameToToken[name];
            return mapping == null ? null : mapping.GetDisplayName();
        }

        public void SetSearchMapping(char c)
        {
            _searchNavigation.SetName(SearchNavigationName);
            _searchNavigation.SetDisplayName(c.ToString());
            AddMapping(_searchNavigation);
        }

        private void AddSearchString(string searchString, StringBuilder reference, UriBuilder uri)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                uri.Path = UriUtils.NormalizePath(uri.Path + "/" + HttpUtility.UrlEncode(searchString));
                reference.Append(_searchNavigation.GetDisplayName());
            }
        }

        private string ApplyReplacementRule(Navigation navigation, string searchString, int indexOffSet,
            List<UrlReplacement> replacements)
        {
            StringBuilder builder = new StringBuilder(searchString);
            foreach (UrlReplacementRule replacementRule in _replacementRules)
            {
                replacementRule.Apply(builder, indexOffSet, navigation.GetName(), replacements);
            }
            return builder.ToString();
        }

        public void AddRefinementMapping(char token, string name)
        {
            Navigation mapping = new Navigation();
            mapping.SetName(name).SetDisplayName(token.ToString());
            AddMapping(mapping);
        }

        private void AddMapping(Navigation mapping)
        {
            string name = mapping.GetName();
            string token = mapping.GetDisplayName();

            if (token.Length != 1 || string.IsNullOrEmpty(token))
            {
                throw new UrlBeautificationException("Token length must be one");
            }
            if (Regex.Matches(token, "[aoeuiAOEUIyY]").Count > 0)
            {
                throw new UrlBeautificationException("Vowels are not allowed to avoid Dictionary words appearing");
            }
            if (_tokenToName.Contains(token))
            {
                throw new UrlBeautificationException(string.Format("This token: {0} is already mapped to: {1}", token,
                    ((Navigation) _tokenToName[token]).GetName()));
            }

            _tokenToName.Add(token, mapping);
            _nameToToken.Add(name, mapping);
            _remainingMappings.Add(mapping);
        }

        public void ClearSavedFields()
        {
            _append = null;
            _tokenToName = new OrderedDictionary();
            _nameToToken = new OrderedDictionary();
            _remainingMappings = new List<Navigation>();
        }
    }
}