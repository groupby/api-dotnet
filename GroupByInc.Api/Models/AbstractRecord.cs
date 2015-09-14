using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public abstract class AbstractRecord<T>
        where T : AbstractRecord<T>
    {
        [JsonProperty("_id")] //
        private string _id;
        [JsonProperty("_u")] //
        private string _url;
        [JsonProperty("_snippet")] //
        private string _snippet;
        [JsonProperty("_t")] //
        private string _title;
        [JsonProperty("allMeta")]
        private IDictionary<string, object> _allMeta;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return _id;
        }

        public T SetId(String id)
        {
            _id = id;
            return (T) this;
        }

        public String GetUrl()
        {
            return _url;
        }

        public T SetUrl(String url)
        {
            _url = url;
            return (T) this;
        }


        public String GetSnippet()
        {
            return _snippet;
        }


        public T SetSnippet(String snippet)
        {
            _snippet = snippet;
            return (T) this;
        }

        public object GetMetaValue(String name)
        {
            object result;
            _allMeta.TryGetValue(name, out result);
            return result;
        }

        public IDictionary<string, object> GetAllMeta()
        {
            return _allMeta;
        }


        public T SetAllMeta(IDictionary<string, object> allMeta)
        {
            _allMeta = allMeta;
            return (T) this;
        }

        public String GetTitle()
        {
            return _title;
        }


        public T SetTitle(String title)
        {
            _title = title;
            return (T) this;
        }
    }
}
