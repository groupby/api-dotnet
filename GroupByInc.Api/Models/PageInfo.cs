using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     Holds information about this page of data.
    /// </summary>
    public class PageInfo
    {
        [JsonProperty("recordStart")]
        private int _recordStart;

        [JsonProperty("recordEnd")]
        private int _recordEnd;

        public int GetRecordStart()
        {
            return _recordStart;
        }

        public PageInfo SetRecordStart(int recordStart)
        {
            _recordStart = recordStart;
            return this;
        }

        public int GetRecordEnd()
        {
            return _recordEnd;
        }


        public PageInfo SetRecordEnd(int recordEnd)
        {
            _recordEnd = recordEnd;
            return this;
        }

    }
}