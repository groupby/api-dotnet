using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public abstract class AbstractResults<R, T>
        where R : AbstractRecord<R>
        where T : AbstractResults<R, T>
    {
        [JsonProperty("totalRecordCount")]
        protected long TotalRecordCount;

        [JsonProperty("area")]
        protected string Area;

        [JsonProperty("biasingProfile")]
        protected string BiasingProfile;

        [JsonProperty("redirect")]
        protected string Redirect;

        [JsonProperty("errors")]
        protected string Errors;

        [JsonProperty("query")]
        protected string Query;

        [JsonProperty("originalQuery")]
        protected string OriginalQuery;

        [JsonProperty("correctedQuery")]
        protected string CorrectedQuery;

        [JsonProperty("template")]
        protected Template Template;

        [JsonProperty("pageInfo")]
        protected PageInfo PageInfo = new PageInfo();

        [JsonProperty("availableNavigation")]
        protected List<Navigation> AvailableNavigations = new List<Navigation>();

        [JsonProperty("selectedNavigation")]
        protected List<Navigation> SelectedNavigations = new List<Navigation>();

        [JsonProperty("records")]
        protected List<R> Records = new List<R>();

        [JsonProperty("didYouMean")]
        protected List<string> DidYouMean = new List<string>();

        [JsonProperty("relatedQueries")]
        protected List<string> RelatedQueries = new List<string>();

        [JsonProperty("rewrites")]
        protected List<string> Rewrites = new List<string>();

        [JsonProperty("siteParams")]
        protected List<Metadata> SiteParams = new List<Metadata>();

        [JsonProperty("clusters")]
        protected List<Cluster> Clusters = new List<Cluster>();

        public T SetTotalRecordCount(long totalRecordCount)
        {
            TotalRecordCount = totalRecordCount;
            return (T)this;
        }

        public long GetTotalRecordCount()
        {
            return TotalRecordCount;
        }

        public T SetArea(string area)
        {
            Area = area;
            return (T)this;
        }

        public string GetArea()
        {
            return Area;
        }

        public T SetBiasingProfile(string biasingProfile)
        {
            BiasingProfile = biasingProfile;
            return (T)this;
        }

        public string GetBiasingProfile()
        {
            return BiasingProfile;
        }

        public T SetRedirect(string redirect)
        {
            Redirect = redirect;
            return (T)this;
        }

        public string GetRedirect()
        {
            return Redirect;
        }

        public T SetErrors(string errors)
        {
            Errors = errors;
            return (T)this;
        }

        public string GetErrors()
        {
            return Errors;
        }

        public T SetQuery(string query)
        {
            Query = query;
            return (T)this;
        }

        public string GetQuery()
        {
            return Query;
        }

        public T SetOriginalQuery(string originalQuery)
        {
            OriginalQuery = originalQuery;
            return (T)this;
        }

        public string GetOriginalQuery()
        {
            return OriginalQuery;
        }

        public T SetCorrectedQuery(string correctedQuery)
        {
            CorrectedQuery = correctedQuery;
            return (T)this;
        }

        public string GetCorrectedQuery()
        {
            return CorrectedQuery;
        }

        public T SetTemplate(Template template)
        {
            Template = template;
            return (T)this;
        }

        public Template GetTemplate()
        {
            return Template;
        }

        public T SetPageInfo(PageInfo pageInfo)
        {
            PageInfo = pageInfo;
            return (T)this;
        }

        public PageInfo GetPageInfo()
        {
            return PageInfo;
        }

        public T SetAvailableNavigations(List<Navigation> availableNavigations)
        {
            AvailableNavigations = availableNavigations;
            return (T)this;
        }

        public List<Navigation> GetAvailableNavigations()
        {
            return AvailableNavigations;
        }

        public T SetSelectedNavigations(List<Navigation> selectedNavigations)
        {
            SelectedNavigations = selectedNavigations;
            return (T)this;
        }

        public List<Navigation> GetSelectedNavigations()
        {
            return SelectedNavigations;
        }

        public T SetRecords(List<R> records)
        {
            Records = records;
            return (T)this;
        }

        public List<R> GetRecords()
        {
            return Records;
        }

        public T SetDidYouMean(List<string> didYouMean)
        {
            DidYouMean = didYouMean;
            return (T)this;
        }

        public List<string> GetDidYouMean()
        {
            return DidYouMean;
        }

        public T SetRelatedQueries(List<string> relatedQueries)
        {
            RelatedQueries = relatedQueries;
            return (T)this;
        }

        public List<string> GetRelatedQueries()
        {
            return RelatedQueries;
        }

        public T SetRewrites(List<string> rewrites)
        {
            Rewrites = rewrites;
            return (T)this;
        }

        public List<string> GetRewrites()
        {
            return Rewrites;
        }

        public T SetSiteParams(List<Metadata> metadatas)
        {
            SiteParams = metadatas;
            return (T)this;
        }

        public List<Metadata> GetSiteParams()
        {
            return SiteParams;
        }

        public T SetClusters(List<Cluster> clusters)
        {
            Clusters = clusters;
            return (T)this;
        }

        public List<Cluster> GetClusters()
        {
            return Clusters;
        }
    }
}
