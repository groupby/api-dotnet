using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    [JsonObject(MemberSerialization.Fields)]
    public class Navigation
    {
        public enum Sort
        {
            Count_Ascending,
            Count_Descending,
            Value_Ascending,
            Value_Descending
        }

        private string _id;

        [JsonProperty("name")] private string _name;

        [JsonProperty("displayName")] private string _displayName;

        [JsonProperty("range")] private bool _range;

        [JsonProperty("or")] private bool _or;

        [JsonProperty("sort")] private Sort _sort;

        [JsonProperty("moreRefinements", DefaultValueHandling = DefaultValueHandling.Ignore)] private bool?
            _moreRefinements;

        [JsonProperty("refinements")] private List<Refinement> _refinements = new List<Refinement>();

        [JsonProperty("metadata")] private List<Metadata> _metadata = new List<Metadata>();

        public string GetName()
        {
            return _name;
        }

        public Navigation SetName(string name)
        {
            _name = name;
            return this;
        }

        public string GetDisplayName()
        {
            return _displayName;
        }

        public Navigation SetDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public List<Refinement> GetRefinements()
        {
            return _refinements;
        }

        public Navigation SetRefinements(List<Refinement> refinements)
        {
            _refinements = refinements;
            return this;
        }

        public string GetId()
        {
            return _id;
        }

        public Navigation SetId(string id)
        {
            _id = id;
            return this;
        }

        public bool IsRange()
        {
            return _range;
        }

        public Navigation SetRange(bool range)
        {
            _range = range;
            return this;
        }


        public bool IsOr()
        {
            return _or;
        }

        public Navigation SetOr(bool or)
        {
            _or = or;
            return this;
        }

        public Sort GetSort()
        {
            return _sort;
        }

        public Navigation SetSort(Sort sort)
        {
            _sort = sort;
            return this;
        }

        public List<Metadata> GetMetadata()
        {
            return _metadata;
        }

        public Navigation SetMetadata(List<Metadata> metadata)
        {
            _metadata = metadata;
            return this;
        }

        public bool? IsMoreRefinements()
        {
            return _moreRefinements;
        }

        public Navigation SetMoreRefinements(bool? moreRefinements)
        {
            _moreRefinements = moreRefinements;
            return this;
        }
    }
}