using System.Collections.Generic;

namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     A cluster represents a set of documents that are considered closely
    ///     related based on a search term.c
    /// </summary>
    public class Cluster
    {
        /// <summary>
        /// </summary>
        private List<ClusterRecord> _records = new List<ClusterRecord>();

        /// <summary>
        ///     <see cref="System.Collections.Generic.List`1" /> of Clustered
        ///     records
        /// </summary>
        public List<ClusterRecord> GetRecords
        {
            get { return _records; }
            set { _records = value; }
        }

        /// <summary>
        ///     <see cref="Cluster.Term" /> for this
        ///     cluster
        /// </summary>
        public string Term { get; set; }
    }
}