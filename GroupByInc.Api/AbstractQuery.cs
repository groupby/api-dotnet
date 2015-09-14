using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Requests;
using GroupByInc.Api.Requests.Refinement;
using GroupByInc.Api.Util;
using Sort = GroupByInc.Api.Requests.Sort;

namespace GroupByInc.Api
{
    public abstract class AbstractQuery<R, Q>
        where R : AbstractRequest<R>
        where Q : AbstractQuery<R, Q>
    {
        private static readonly string Dots = @"\.\.";
        public static readonly string TildeRegex = @"~((?=[\w.]*[=:]))";
        private readonly List<CustomUrlParam> _customUrlParams = new List<CustomUrlParam>();
        private readonly List<string> _fields = new List<string>();
        private readonly OrderedDictionary _navigations = new OrderedDictionary();
        private readonly List<string> _orFields = new List<string>();
        private string _area;
        private string _biasingProfile;
        private string _collection;
        private bool _disableAutocorrection = true;
        private string _language;
        private int _pageSize = 10;
        private bool _pruneRefinements = true;
        private string _query;
        private bool _returnBinary = true;
        private int _skip;
        protected RestrictNavigation RestrictNavigation;

        private static string RequestToJson(R abstractRequest)
        {
            return Mappers.WriteValueAsString(abstractRequest);
        }

        private static string RequestToJson(RefinementsRequest<R> abstractRequest)
        {
            return Mappers.WriteValueAsString(abstractRequest);
        }

        protected abstract R GenerateRequest();
        protected abstract RefinementsRequest<R> PopulateRefinementRequest();

        protected static Sort ConvertSort(Models.Sort sort)
        {
            Sort convertedSort = null;
            if (sort != null)
            {
                convertedSort = new Sort().SetField(sort.GetField());
                switch (sort.GetOrder())
                {
                    case Models.Sort.Order.Ascending:
                        convertedSort.SetOrder(Sort.Order.Ascending);
                        break;
                    case Models.Sort.Order.Descending:
                        convertedSort.SetOrder(Sort.Order.Descending);
                        break;
                }
            }
            return convertedSort;
        }

        private List<SelectedRefinement> GenerateSelectedRefinements(OrderedDictionary navigations)
        {
            List<SelectedRefinement> refinements = new List<SelectedRefinement>();
            foreach (Navigation n in navigations.Values)
            {
                foreach (Refinement refinement in n.GetRefinements())
                {
                    Refinement r = refinement;
                    switch (r.GetType())
                    {
                        case Refinement.TypeEnum.Range:
                            RefinementRange rr = (RefinementRange) r;
                            refinements.Add(
                                new SelectedRefinementRange().SetLow(rr.GetLow())
                                    .SetHigh(rr.GetHigh())
                                    .SetExclude(rr.GetExclude())
                                    .SetNavigationName(n.GetName()));
                            break;
                        case Refinement.TypeEnum.Value:
                            RefinementValue rv = (RefinementValue) r;
                            refinements.Add(
                                new SelectedRefinementValue().SetValue(rv.GetValue())
                                    .SetNavigationName(n.GetName())
                                    .SetExclude(rv.GetExclude()));
                            break;
                    }
                }
            }
            return refinements;
        }

        private R PopulateRequest(string clientKey)
        {
            R request = GenerateRequest();
            request.SetClientKey(clientKey);
            request.SetArea(_area);
            request.SetCollection(_collection);
            request.SetQuery(_query);
            request.SetFields(_fields);
            request.SetOrFields(_orFields);
            request.SetLanguage(_language);
            request.SetBiasingProfile(_biasingProfile);
            request.SetPageSize(_pageSize);
            request.SetSkip(_skip);
            request.SetCustomUrlParams(_customUrlParams);
            request.SetRefinements(GenerateSelectedRefinements(_navigations));
            request.SetRestrictNavigation(ConvertRestrictNavigation());
            if (!_pruneRefinements)
            {
                request.SetPruneRefinements(false);
            }
            if (_returnBinary)
            {
                request.SetReturnBinary(true);
            }
            if (_disableAutocorrection)
            {
                request.SetDisableAutocorrection(true);
            }

            return request;
        }

        private RestrictNavigation ConvertRestrictNavigation()
        {
            return RestrictNavigation == null
                ? null
                : new RestrictNavigation()
                    .SetName(RestrictNavigation.GetName())
                    .SetCount(RestrictNavigation.GetCount());
        }

        public string GetBridgeJson(string clientKey)
        {
            R request = PopulateRequest(clientKey);
            return RequestToJson(request);
        }

        public string GetBridgeRefinementsJson(string clientKey, string navigationName)
        {
            RefinementsRequest<R> request = PopulateRefinementRequest();
            request.SetOriginalQuery(PopulateRequest(clientKey));
            request.SetNavigationName(navigationName);
            return RequestToJson(request);
        }

        public string GetQuery()
        {
            return _query;
        }

        public Q SetQuery(string query)
        {
            _query = query;
            return (Q) this;
        }

        public string GetCollection()
        {
            return _collection;
        }

        public Q SetCollection(string collection)
        {
            _collection = collection;
            return (Q) this;
        }

        public string GetArea()
        {
            return _area;
        }

        public Q SetArea(string area)
        {
            _area = area;
            return (Q) this;
        }

        public string GetRefinementsString()
        {
            if (!CollectionUtils.IsNullOrEmpty(_navigations.Values))
            {
                StringBuilder resultBuilder = new StringBuilder();
                foreach (Navigation navigation in _navigations.Values)
                {
                    foreach (Refinement refinement in navigation.GetRefinements())
                    {
                        resultBuilder.Append("~").Append(navigation.GetName()).Append(refinement.ToTildeString());
                    }
                }
                if (resultBuilder.Length > 0)
                {
                    return resultBuilder.ToString();
                }
            }
            return null;
        }

        public string GetCustomUrlParamsString()
        {
            if (CollectionUtils.IsNullOrEmpty(_customUrlParams))
            {
                return null;
            }
            StringBuilder resultBuilder = new StringBuilder();
            foreach (CustomUrlParam customUrlParam in _customUrlParams)
            {
                resultBuilder.Append("~").Append(customUrlParam.GetKey()).Append("=").Append(customUrlParam.GetValue());
            }
            return resultBuilder.ToString();
        }

        public List<CustomUrlParam> GetCustomUrlParams()
        {
            return _customUrlParams;
        }

        protected string GetBridgeJsonRefinementSearch(string clientKey)
        {
            R request = GenerateRequest();
            request.SetClientKey(clientKey);
            request.SetCollection(_collection);
            request.SetArea(_area);
            request.SetRefinementQuery(_query);
            return RequestToJson(request);
        }

        public string[] SplitRefinements(string refinementString)
        {
            if (!string.IsNullOrEmpty(refinementString))
            {
                Regex pattern = new Regex(TildeRegex);
                return StringUtils.RemoveNull(pattern.Split(refinementString));
            }
            return new string[] {};
        }

        public Q AddRefinementsByString(string refinementString)
        {
            if (refinementString == null)
            {
                return (Q) this;
            }

            string[] filterStrings = SplitRefinements(refinementString);
            foreach (string filterString in filterStrings)
            {
                if (string.IsNullOrEmpty(filterString) || "=".Equals(filterString))
                {
                    continue;
                }
                int colon = filterString.IndexOf(":", StringComparison.Ordinal);
                int equals = filterString.IndexOf("=", StringComparison.Ordinal);
                bool isRange = colon != -1 && equals == -1;
                string[] nameValue = new Regex("[:=]").Split(filterString, 2);
                Refinement refinement;
                if (isRange)
                {
                    RefinementRange rr = new RefinementRange();
                    if (nameValue[1].EndsWith(".."))
                    {
                        rr.SetLow(new Regex(Dots).Split(nameValue[1])[0]);
                        rr.SetHigh("");
                    }
                    else if (nameValue[1].StartsWith(".."))
                    {
                        rr.SetLow("");
                        rr.SetHigh(new Regex(Dots).Split(nameValue[1])[1]);
                    }
                    else
                    {
                        string[] lowHigh = new Regex(Dots).Split(nameValue[1]);
                        rr.SetLow(lowHigh[0]);
                        rr.SetHigh(lowHigh[1]);
                    }
                    refinement = rr;
                }
                else
                {
                    refinement = new RefinementValue();
                    ((RefinementValue) refinement).SetValue(nameValue[1]);
                }

                if (!string.IsNullOrEmpty(nameValue[0]))
                {
                    AddRefinement(nameValue[0], refinement);
                }
            }

            return (Q) this;
        }

        public Q AddCustomUrlParam(CustomUrlParam customUrlParam)
        {
            _customUrlParams.Add(customUrlParam);
            return (Q) this;
        }

        public Q AddCustomUrlParam(string key, string value)
        {
            _customUrlParams.Add(new CustomUrlParam().SetKey(key).SetValue(value));
            return (Q) this;
        }

        public Q AddCustomUrlParamsByString(string values)
        {
            if (values == null)
            {
                return (Q) this;
            }
            string[] urlParams = values.Split('&');
            foreach (string value in urlParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] keyValue = value.Split('=');
                    if ((keyValue.Length == 2) && !string.IsNullOrEmpty(keyValue[0]) &&
                        !string.IsNullOrEmpty(keyValue[1]))
                    {
                        _customUrlParams.Add(new CustomUrlParam().SetKey(keyValue[0]).SetValue(keyValue[1]));
                    }
                }
            }
            return (Q) this;
        }

        public List<string> GetFields()
        {
            return _fields;
        }

        private Q AddField(List<string> fields, params string[] names)
        {
            if (names == null)
            {
                return (Q) this;
            }

            CollectionUtils.AddAll(fields, names);
            return (Q) this;
        }

        public Q AddFields(params string[] name)
        {
            return AddField(_fields, name);
        }

        public Q AddOrField(params string[] name)
        {
            return AddField(_orFields, name);
        }

        public Q AddRangeRefinement(string navigationName, string low, string high)
        {
            return AddRangeRefinement(navigationName, low, high, false);
        }

        public Q AddRangeRefinement(string navigationName, string low, string high, bool exclude)
        {
            return AddRefinement(navigationName, new RefinementRange().SetLow(low).SetHigh(high).SetExclude(exclude));
        }

        public Q AddValueRefinement(string navigationName, string value)
        {
            return AddValueRefinement(navigationName, value, false);
        }

        public Q AddValueRefinement(string navigationName, string value, bool exclude)
        {
            return AddRefinement(navigationName, new RefinementValue().SetValue(value).SetExclude(exclude));
        }

        private Q AddRefinement(string navigationName, Refinement refinement)
        {
            Navigation navigation = (Navigation) _navigations[navigationName];
            if (navigation == null)
            {
                navigation = new Navigation().SetName(navigationName);
                navigation.SetRange(refinement is RefinementRange);
                _navigations.Add(navigationName, navigation);
            }
            navigation.GetRefinements().Add(refinement);
            return (Q) this;
        }

        public long GetSkip()
        {
            return _skip;
        }

        public Q SetSkip(int skip)
        {
            _skip = skip;
            return (Q) this;
        }

        public long GetPageSize()
        {
            return _skip;
        }

        public Q SetPageSize(int pageSize)
        {
            _pageSize = pageSize;
            return (Q) this;
        }

        public OrderedDictionary GetNavigations()
        {
            return _navigations;
        }

        public bool IsReturnBinary()
        {
            return _returnBinary;
        }

        public Q SetReturnBinary(bool returnBinary)
        {
            _returnBinary = returnBinary;
            return (Q) this;
        }

        public string BiasingProfile()
        {
            return _biasingProfile;
        }

        public Q SetBiasingProfile(string biasingProfile)
        {
            _biasingProfile = biasingProfile;
            return (Q) this;
        }

        public bool GetLanguage()
        {
            return _returnBinary;
        }

        public Q SetLanguage(string language)
        {
            _language = language;
            return (Q) this;
        }

        public bool IsPruneRefinements()
        {
            return _pruneRefinements;
        }

        public Q SetPruneRefinements(bool pruneRefinements)
        {
            _pruneRefinements = pruneRefinements;
            return (Q) this;
        }

        public bool IsAutocorrectionDisabled()
        {
            return _disableAutocorrection;
        }

        public Q SetDisableAutocorrrection(bool disableAutorrection)
        {
            _disableAutocorrection = disableAutorrection;
            return (Q) this;
        }

        public Q SetRestrictNavigation(RestrictNavigation restrictNavigation)
        {
            RestrictNavigation = restrictNavigation;
            return (Q) this;
        }

        public Q SetRestrictNavigation(string name, int count)
        {
            RestrictNavigation = new RestrictNavigation().SetName(name).SetCount(count);
            return (Q) this;
        }
    }
}