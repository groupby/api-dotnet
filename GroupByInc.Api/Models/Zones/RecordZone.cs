using System.Collections.Generic;

namespace GroupByInc.Api.Models.Zones
{
    public sealed class RecordZone<TD> : Zone
        where TD : AbstractRecord<TD>
    {
        private string _query;
        private List<TD> _records;

        public override Type GeType()
        {
            return Type.Record;
        }

        public RecordZone<TD> SetId(string id)
        {
            Id = id;
            return this;
        }

        public RecordZone<TD> SetName(string name)
        {
            Name = name;
            return this;
        }

        public string GetQuery()
        {
            return _query;
        }

        public RecordZone<TD> SetQuery(string query)
        {
            _query = query;
            return this;
        }

        public List<TD> GetRecords()
        {
            return _records;
        }

        public RecordZone<TD> SetRecords(List<TD> records)
        {
            _records = records;
            return this;
        }
    }
}