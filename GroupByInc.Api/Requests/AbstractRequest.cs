using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Util;
using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    /// <summary>
    /// </summary>
    public abstract class AbstractRequest<T>
        where T : AbstractRequest<T>
    {
        [JsonProperty("customUrlParams", NullValueHandling = NullValueHandling.Ignore)] //
        private List<CustomUrlParam> _customUrlParams = new List<CustomUrlParam>();

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)] // 
        private List<string> _fields = new List<string>();

        [JsonProperty("orFields", NullValueHandling = NullValueHandling.Ignore)] //
        private List<string> _orFields = new List<string>();

        [JsonProperty("pruneRefinements")]
        private bool _pruneRefinements = true;

        [JsonProperty("refinements", NullValueHandling = NullValueHandling.Ignore)] //
        private List<SelectedRefinement> _refinements = new List<SelectedRefinement>();

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)] //
        private List<Sort> _sort = new List<Sort>();

        [JsonProperty("clientKey")]
        private string _clientKey;

        [JsonProperty("collection")]
        private string _collection;

        [JsonProperty("area")]
        private string _area;

        [JsonProperty("biasingProfile")]
        private string _biasingProfile;

        [JsonProperty("language")]
        private string _language;

        [JsonProperty("query")]
        private string _query;

        [JsonProperty("refinementQuery")]
        private string _refinementQuery;

        [JsonProperty("restrictedNavigation")]
        private RestrictNavigation _restrictNavigation;

        [JsonProperty("skip")]
        private int _skip;

        [JsonProperty("pageSize")]
        private int _pageSize;

        [JsonProperty("returnBinary")]
        private bool _returnBinary;

        [JsonProperty("disableAutocorrection")]
        private bool _disableAutocorrection;

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

        public T SetCustomUrlParams(List<CustomUrlParam> customUrlParams)
        {
            _customUrlParams = customUrlParams;
            return (T) this;
        }

        public List<SelectedRefinement> GetRefinements()
        {
            return _refinements;
        }

        public T SetRefinements(List<SelectedRefinement> refinements)
        {
            _refinements = refinements;
            return (T) this;
        }

        public List<string> GetFields()
        {
            return _fields;
        }

        public T SetFields(List<string> fields)
        {
            _fields = fields;
            return (T)this;
        }

        public List<string> GetOrFields()
        {
            return _orFields;
        }

        public T SetOrFields(List<string> orFields)
        {
            _orFields = orFields;
            return (T) this;
        }


        public T SetCollection(string value)
        {
            _collection = value;
            return (T) this;
        }

        public string GetCollection()
        {
            return _collection;
        }

        public T SetArea(string value)
        {
            _area = value;
            return (T) this;
        }

        public string GetArea()
        {
            return _area;
        }

        public T SetBiasingProfile(string value)
        {
            _biasingProfile = value;
            return (T) this;
        }

        public string GetBiasingProfile()
        {
            return _biasingProfile;
        }

        public List<Sort> GetSort()
        {
            return _sort;
        }

        public T SetSort(List<Sort> sort)
        {
            _sort = sort;
            return (T) this;
        }

        public T SetSort(params Sort [] sort)
        {
            CollectionUtils.AddAll(_sort, sort);
            return (T) this;
        }

        public T SetLanguage(string value)
        {
            _language = value;
            return (T) this;
        }

        public string GetLanguage()
        {
            return _language;
        }

        public T SetQuery(string value)
        {
            _query = value;
            return (T) this;
        }

        public string GetQuery()
        {
            return _query;
        }

        public T SetRefinementQuery(string value)
        {
            _refinementQuery = value;
            return (T) this;
        }

        public string GetRefinementQuery()
        {
            return _refinementQuery;
        }

        public T SetRestrictNavigation(RestrictNavigation value)
        {
            _restrictNavigation = value;
            return (T) this;
        }

        public RestrictNavigation GetRestrictNavigation()
        {
            return _restrictNavigation;
        }

        public T SetSkip(int value)
        {
            _skip = value;
            return (T) this;
        }

        public int GetSkip()
        {
            return _skip;
        }

        public T SetPageSize(int value)
        {
            _pageSize = value;
            return (T) this;
        }

        public int GetPageSize()
        {
            return _pageSize;
        }

        public bool PruneRefinements()
        {
            return _pruneRefinements;
        }

        public T SetPruneRefinements(bool pruneRefinements)
        {
            _pruneRefinements = pruneRefinements;
            return (T) this;
        }


        public T SetReturnBinary(bool value)
        {
            _returnBinary = value;
            return (T) this;
        }

        public bool GetReturnBinary()
        {
            return _returnBinary;
        }

        public T SetDisableAutocorrection(bool value)
        {
            _disableAutocorrection = value;
            return (T) this;
        }

        public bool GetDisableAutocorrection()
        {
            return _disableAutocorrection;
        }
    }
}