using System;
using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Util;
using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    /// <summary>
    /// </summary>
    public class Request
    {
        [JsonProperty("customUrlParams", NullValueHandling = NullValueHandling.Ignore)] //
        private List<CustomUrlParam> _customUrlParams = new List<CustomUrlParam>();

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)] // 
        private List<string> _fields = new List<string>();

        [JsonProperty("orFields", NullValueHandling = NullValueHandling.Ignore)] //
        private List<string> _orFields = new List<string>();

        [JsonProperty("includedNavigations", NullValueHandling = NullValueHandling.Ignore)] //
        private List<string> _includedNavigations = new List<string>();

        [JsonProperty("excludedNavigations", NullValueHandling = NullValueHandling.Ignore)] //
        private List<string> _excludedNavigations = new List<string>();

        [JsonProperty("pruneRefinements")] private bool _pruneRefinements = true;

        [JsonProperty("refinements", NullValueHandling = NullValueHandling.Ignore)] //
        private List<SelectedRefinement> _refinements = new List<SelectedRefinement>();

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)] //
        private List<Sort> _sort = new List<Sort>();

        [JsonProperty("clientKey")] private string _clientKey;

        [JsonProperty("collection")] private string _collection;

        [JsonProperty("area")] private string _area;

        [JsonProperty("sessionId")] private string _sessionId;

        [JsonProperty("visitorId")] private string _visitorId;

        [JsonProperty("biasingProfile")] private string _biasingProfile;

        [JsonProperty("language")] private string _language;

        [JsonProperty("query")] private string _query;

        [JsonProperty("refinementQuery")] private string _refinementQuery;

        [JsonProperty("restrictNavigation")] private RestrictNavigation _restrictNavigation;

        [JsonProperty("skip")] private int _skip;

        [JsonProperty("pageSize")] private int _pageSize;

        [JsonProperty("returnBinary")] private bool _returnBinary;

        [JsonProperty("disableAutocorrection")] private bool _disableAutocorrection;

        [JsonProperty("wildcardSearchEnabled")] private bool? _wildcardSearchEnabled = false;

        [JsonProperty("matchStrategy")] private MatchStrategy _matchStrategy;

        [JsonProperty("matchStrategyName")] private string _matchStrategyName;

        [JsonProperty("biasing")] private Biasing _biasing;

        /// <summary>
        /// </summary>
        public void SetClientKey(string value)
        {
            _clientKey = value;
        }

        /// <summary>
        /// </summary>
        public string GetClientKey()
        {
            return _clientKey;
        }

        public List<CustomUrlParam> GetCustomUrlParams()
        {
            return _customUrlParams;
        }

        public Request SetCustomUrlParams(List<CustomUrlParam> customUrlParams)
        {
            _customUrlParams = customUrlParams;
            return this;
        }

        public List<SelectedRefinement> GetRefinements()
        {
            return _refinements;
        }

        public Request SetRefinements(List<SelectedRefinement> refinements)
        {
            _refinements = refinements;
            return this;
        }

        public List<string> GetFields()
        {
            return _fields;
        }

        public Request SetFields(List<string> fields)
        {
            _fields = fields;
            return this;
        }

        public List<string> GetOrFields()
        {
            return _orFields;
        }

        public Request SetOrFields(List<string> orFields)
        {
            _orFields = orFields;
            return this;
        }

        public List<string> GetIncludedNavigations()
        {
            return _includedNavigations;
        }

        public Request SetIncludedNavigations(List<string> includedNavigations)
        {
            _includedNavigations = includedNavigations;
            return this;
        }

        public List<string> GetExcludedNavigations()
        {
            return _excludedNavigations;
        }

        public Request SetExcludedNavigations(List<string> excludedNavigations)
        {
            _excludedNavigations = excludedNavigations;
            return this;
        }

        public Request SetCollection(string value)
        {
            _collection = value;
            return this;
        }

        public string GetCollection()
        {
            return _collection;
        }

        public Request SetArea(string value)
        {
            _area = value;
            return this;
        }

        public string GetArea()
        {
            return _area;
        }

        public Request SetBiasingProfile(string value)
        {
            _biasingProfile = value;
            return this;
        }

        public string GetBiasingProfile()
        {
            return _biasingProfile;
        }

        public List<Sort> GetSort()
        {
            return _sort;
        }

        public Request SetSort(List<Sort> sort)
        {
            _sort = sort;
            return this;
        }

        public Request SetSort(params Sort[] sort)
        {
            CollectionUtils.AddAll(_sort, sort);
            return this;
        }

        public Request SetLanguage(string value)
        {
            _language = value;
            return this;
        }

        public string GetLanguage()
        {
            return _language;
        }

        public Request SetQuery(string value)
        {
            _query = value;
            return this;
        }

        public string GetQuery()
        {
            return _query;
        }

        public Request SetRefinementQuery(string value)
        {
            _refinementQuery = value;
            return this;
        }

        public string GetRefinementQuery()
        {
            return _refinementQuery;
        }

        public Request SetRestrictNavigation(RestrictNavigation value)
        {
            _restrictNavigation = value;
            return this;
        }

        public RestrictNavigation GetRestrictNavigation()
        {
            return _restrictNavigation;
        }

        public Request SetSkip(int value)
        {
            _skip = value;
            return this;
        }

        public int GetSkip()
        {
            return _skip;
        }

        public Request SetPageSize(int value)
        {
            _pageSize = value;
            return this;
        }

        public int GetPageSize()
        {
            return _pageSize;
        }

        public bool PruneRefinements()
        {
            return _pruneRefinements;
        }

        public Request SetPruneRefinements(bool pruneRefinements)
        {
            _pruneRefinements = pruneRefinements;
            return this;
        }


        public Request SetReturnBinary(bool value)
        {
            _returnBinary = value;
            return this;
        }

        public bool GetReturnBinary()
        {
            return _returnBinary;
        }

        public Request SetDisableAutocorrection(bool value)
        {
            _disableAutocorrection = value;
            return this;
        }

        public bool GetDisableAutocorrection()
        {
            return _disableAutocorrection;
        }

        public bool? IsWildcardSearchEnabled()
        {
            return _wildcardSearchEnabled;
        }

        public Request SetWildcardSearchEnabled(bool? wildcardSearchEnabled)
        {
            _wildcardSearchEnabled = wildcardSearchEnabled;
            return this;
        }

        public MatchStrategy GetMatchStrategy()
        {
            return _matchStrategy;
        }

        public Request SetMatchStrategy(MatchStrategy matchStrategy)
        {
            _matchStrategy = matchStrategy;
            return this;
        }

        public Biasing GetBiasing()
        {
            return _biasing;
        }

        public Request SetBiasing(Biasing biasing)
        {
            _biasing = biasing;
            return this;
        }

        public string GetVisitorId()
        {
            return _visitorId;
        }

        public Request SetVisitorId(string visitorId)
        {
            _visitorId = visitorId;
            return this;
        }

        public string GetSessionId()
        {
            return _sessionId;
        }

        public Request SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
            return this;
        }

        public string GetMatchStrategyName()
        {
            return _matchStrategyName;
        }

        public Request SetMatchStrategyName(string matchStrategyName)
        {
            _matchStrategyName = matchStrategyName;
            return this;
        }
    }
}